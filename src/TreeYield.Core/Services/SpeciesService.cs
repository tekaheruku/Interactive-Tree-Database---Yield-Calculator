using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using TreeYield.Core.Data;
using TreeYield.Core.Models;

namespace TreeYield.Core.Services
{
    /// <summary>
    /// Everything the UI can do with species records. Read operations are open to
    /// everyone; changes require a signed-in administrator, are validated first,
    /// and are written to the audit log.
    /// </summary>
    public class SpeciesService
    {
        private const string EntityName = "TreeSpecies";

        private readonly SpeciesRepository _species;
        private readonly CategoryRepository _categories;
        private readonly ScenarioRepository _scenarios;
        private readonly AuditLogRepository _audit;
        private readonly DatabaseInitializer _initializer;

        /// <summary>Creates the service.</summary>
        public SpeciesService(SpeciesRepository species, CategoryRepository categories, ScenarioRepository scenarios,
            AuditLogRepository audit, DatabaseInitializer initializer)
        {
            if (species == null) throw new ArgumentNullException(nameof(species));
            if (categories == null) throw new ArgumentNullException(nameof(categories));
            if (scenarios == null) throw new ArgumentNullException(nameof(scenarios));
            if (audit == null) throw new ArgumentNullException(nameof(audit));
            if (initializer == null) throw new ArgumentNullException(nameof(initializer));

            _species = species;
            _categories = categories;
            _scenarios = scenarios;
            _audit = audit;
            _initializer = initializer;
        }

        /// <summary>Returns all categories.</summary>
        public Task<List<Category>> GetCategoriesAsync()
        {
            return _categories.GetAllAsync();
        }

        /// <summary>Returns every species, sorted by name.</summary>
        public Task<List<TreeSpecies>> GetAllAsync()
        {
            return _species.GetAllAsync();
        }

        /// <summary>Searches species by text, optionally within one category.</summary>
        public Task<List<TreeSpecies>> SearchAsync(string searchText, int? categoryId)
        {
            return _species.SearchAsync(searchText, categoryId);
        }

        /// <summary>Returns how many saved scenarios use the species.</summary>
        public Task<int> CountScenariosAsync(int speciesId)
        {
            return _scenarios.CountBySpeciesAsync(speciesId);
        }

        /// <summary>Returns the recent audit log entries.</summary>
        public Task<List<AuditEntry>> GetAuditLogAsync(int count)
        {
            return _audit.GetRecentAsync(count);
        }

        /// <summary>
        /// Validates and saves a species: INSERT when <see cref="TreeSpecies.SpeciesId"/> is 0,
        /// otherwise UPDATE. Returns the species ID.
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">No administrator is signed in.</exception>
        /// <exception cref="ValidationException">A field is invalid.</exception>
        /// <exception cref="DuplicateRecordException">The common name is already used.</exception>
        public async Task<int> SaveAsync(TreeSpecies species, AppUser actor)
        {
            RequireAdministrator(actor);

            TreeSpecies clean = Normalize(species);
            ValidationResult validation = SpeciesValidator.Validate(clean);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation);
            }

            if (clean.SpeciesId == 0)
            {
                int newId = await _species.InsertAsync(clean).ConfigureAwait(false);
                await TryAuditAsync(actor, AuditActions.Create, newId, "Added species '" + clean.CommonName + "'.")
                    .ConfigureAwait(false);
                return newId;
            }

            await _species.UpdateAsync(clean).ConfigureAwait(false);
            await TryAuditAsync(actor, AuditActions.Update, clean.SpeciesId, string.Format(CultureInfo.InvariantCulture,
                "Updated species '{0}' (yield {1} {2}/tree/yr, price {3}/{2}).",
                clean.CommonName, clean.YieldPerTreePerYear, clean.YieldUnit, clean.PricePerUnit)).ConfigureAwait(false);
            return clean.SpeciesId;
        }

        /// <summary>Deletes a species and (through ON DELETE CASCADE) its saved scenarios.</summary>
        /// <exception cref="UnauthorizedAccessException">No administrator is signed in.</exception>
        public async Task DeleteAsync(TreeSpecies species, AppUser actor)
        {
            RequireAdministrator(actor);
            if (species == null)
            {
                throw new ArgumentNullException(nameof(species));
            }

            int scenarioCount = await _scenarios.CountBySpeciesAsync(species.SpeciesId).ConfigureAwait(false);
            await _species.DeleteAsync(species.SpeciesId).ConfigureAwait(false);
            await TryAuditAsync(actor, AuditActions.Delete, species.SpeciesId, string.Format(CultureInfo.InvariantCulture,
                "Deleted species '{0}' and {1} saved scenario(s).", species.CommonName, scenarioCount)).ConfigureAwait(false);
        }

        /// <summary>Restores the original sample species (deletes all species and scenarios first).</summary>
        /// <exception cref="UnauthorizedAccessException">No administrator is signed in.</exception>
        public async Task ResetSampleDataAsync(AppUser actor)
        {
            RequireAdministrator(actor);
            await _initializer.ResetSampleDataAsync().ConfigureAwait(false);
            await TryAuditAsync(actor, AuditActions.ResetData, null, "Restored the sample species data.").ConfigureAwait(false);
        }

        /// <summary>
        /// Returns a cleaned copy: text trimmed, empty optional text turned into NULL,
        /// and for rotation species "full production" set equal to the rotation age.
        /// </summary>
        public static TreeSpecies Normalize(TreeSpecies species)
        {
            if (species == null)
            {
                return null;
            }

            TreeSpecies copy = species.Clone();
            copy.CommonName = TrimToNull(copy.CommonName);
            copy.ScientificName = TrimToNull(copy.ScientificName);
            copy.ProductType = TrimToNull(copy.ProductType);
            copy.YieldUnit = TrimToNull(copy.YieldUnit);
            copy.HarvestSeason = TrimToNull(copy.HarvestSeason);
            copy.Notes = TrimToNull(copy.Notes);

            if (copy.HarvestType == HarvestType.Rotation)
            {
                copy.YearsToFullProduction = copy.YearsToFirstHarvest;
            }

            return copy;
        }

        private static string TrimToNull(string value)
        {
            if (value == null)
            {
                return null;
            }

            string trimmed = value.Trim();
            return trimmed.Length == 0 ? null : trimmed;
        }

        private static void RequireAdministrator(AppUser actor)
        {
            // The UI hides the admin screens from guests; this check makes sure the
            // rule also holds if a service method is ever called from somewhere else.
            if (actor == null || actor.UserId <= 0)
            {
                throw new UnauthorizedAccessException("You must be signed in as an administrator to change species records.");
            }
        }

        private async Task TryAuditAsync(AppUser actor, string action, int? entityId, string details)
        {
            try
            {
                await _audit.AddAsync(actor.UserId, action, EntityName, entityId, details).ConfigureAwait(false);
            }
            catch (DataAccessException ex)
            {
                // The data change already succeeded; a failed log entry must not undo it.
                Trace.TraceWarning("Audit log write failed: " + ex);
            }
        }
    }
}
