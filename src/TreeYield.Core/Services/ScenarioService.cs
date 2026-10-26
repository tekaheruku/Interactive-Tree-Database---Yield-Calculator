using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeYield.Core.Data;
using TreeYield.Core.Models;

namespace TreeYield.Core.Services
{
    /// <summary>Saves, lists and deletes scenarios (saved calculations).</summary>
    public class ScenarioService
    {
        /// <summary>Maximum length of a scenario name.</summary>
        public const int MaxNameLength = 100;

        /// <summary>Maximum length of scenario notes.</summary>
        public const int MaxNotesLength = 500;

        private readonly ScenarioRepository _scenarios;

        /// <summary>Creates the service.</summary>
        public ScenarioService(ScenarioRepository scenarios)
        {
            if (scenarios == null)
            {
                throw new ArgumentNullException(nameof(scenarios));
            }

            _scenarios = scenarios;
        }

        /// <summary>Returns all scenarios, newest first.</summary>
        public Task<List<Scenario>> GetAllAsync()
        {
            return _scenarios.GetAllAsync();
        }

        /// <summary>Validates and saves a new scenario. Returns its ID.</summary>
        /// <exception cref="ValidationException">A field is invalid.</exception>
        public Task<int> SaveAsync(Scenario scenario)
        {
            ValidationResult validation = Validate(scenario);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation);
            }

            scenario.Name = scenario.Name.Trim();
            scenario.Notes = string.IsNullOrWhiteSpace(scenario.Notes) ? null : scenario.Notes.Trim();
            return _scenarios.InsertAsync(scenario);
        }

        /// <summary>Deletes a scenario.</summary>
        public Task DeleteAsync(int scenarioId)
        {
            return _scenarios.DeleteAsync(scenarioId);
        }

        /// <summary>Checks a scenario before it is saved.</summary>
        public static ValidationResult Validate(Scenario scenario)
        {
            var result = new ValidationResult();
            if (scenario == null)
            {
                result.Add("Scenario", "No scenario data was provided.");
                return result;
            }

            if (string.IsNullOrWhiteSpace(scenario.Name))
            {
                result.Add(nameof(Scenario.Name), "Enter a name for the scenario.");
            }
            else if (scenario.Name.Trim().Length > MaxNameLength)
            {
                result.Add(nameof(Scenario.Name), string.Format("The name must be {0} characters or fewer.", MaxNameLength));
            }

            if (scenario.Notes != null && scenario.Notes.Trim().Length > MaxNotesLength)
            {
                result.Add(nameof(Scenario.Notes), string.Format("Notes must be {0} characters or fewer.", MaxNotesLength));
            }

            if (scenario.SpeciesId <= 0)
            {
                result.Add(nameof(Scenario.SpeciesId), "Select a species first.");
            }

            if (scenario.TreeCount < YieldCalculator.MinTreeCount || scenario.TreeCount > YieldCalculator.MaxTreeCount)
            {
                result.Add(nameof(Scenario.TreeCount), string.Format("Tree count must be from {0:N0} to {1:N0}.",
                    YieldCalculator.MinTreeCount, YieldCalculator.MaxTreeCount));
            }

            if (scenario.ProjectionYears < YieldCalculator.MinProjectionYears || scenario.ProjectionYears > YieldCalculator.MaxProjectionYears)
            {
                result.Add(nameof(Scenario.ProjectionYears), string.Format("The projection must cover {0} to {1} years.",
                    YieldCalculator.MinProjectionYears, YieldCalculator.MaxProjectionYears));
            }

            return result;
        }
    }
}
