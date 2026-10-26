using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TreeYield.Core.Models;

namespace TreeYield.Core.Data
{
    /// <summary>Create, read, update and delete (CRUD) for the TreeSpecies table.</summary>
    public class SpeciesRepository : RepositoryBase
    {
        // One column list shared by every SELECT so the Map method always finds its columns.
        private const string SelectColumns = @"
SELECT s.SpeciesId, s.CommonName, s.ScientificName, s.CategoryId, c.Name AS CategoryName,
       s.ProductType, s.YieldUnit, s.YieldPerTreePerYear, s.PricePerUnit, s.HarvestType,
       s.YearsToFirstHarvest, s.YearsToFullProduction, s.HarvestSeason, s.Notes,
       s.CreatedAt, s.UpdatedAt
FROM dbo.TreeSpecies AS s
INNER JOIN dbo.Categories AS c ON c.CategoryId = s.CategoryId";

        /// <summary>Creates the repository.</summary>
        public SpeciesRepository(SqlConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        /// <summary>Returns every species sorted by common name.</summary>
        public Task<List<TreeSpecies>> GetAllAsync()
        {
            return SearchAsync(null, null);
        }

        /// <summary>
        /// Searches by name, scientific name or product, optionally limited to one category.
        /// The search text is passed as a parameter, so input such as <c>' OR 1=1 --</c>
        /// is only ever compared as text and can never change the query.
        /// </summary>
        /// <param name="searchText">Text to look for (case-insensitive, matches anywhere). Null or blank means no text filter.</param>
        /// <param name="categoryId">Category to keep, or null for all categories.</param>
        public Task<List<TreeSpecies>> SearchAsync(string searchText, int? categoryId)
        {
            string trimmed = searchText == null ? null : searchText.Trim();
            string pattern = string.IsNullOrEmpty(trimmed) ? null : "%" + SqlText.EscapeLikePattern(trimmed) + "%";

            const string sql = SelectColumns + @"
WHERE (@CategoryId IS NULL OR s.CategoryId = @CategoryId)
  AND (@Pattern IS NULL
       OR s.CommonName LIKE @Pattern ESCAPE '\'
       OR s.ScientificName LIKE @Pattern ESCAPE '\'
       OR s.ProductType LIKE @Pattern ESCAPE '\')
ORDER BY s.CommonName;";

            return WithConnectionAsync("search the species list", async connection =>
            {
                var results = new List<TreeSpecies>();
                using (SqlCommand command = CreateCommand(connection, sql))
                {
                    AddParameter(command, "@CategoryId", SqlDbType.Int, categoryId);
                    // 204 = room for the 100-character name limit, escapes and the two % signs.
                    AddParameter(command, "@Pattern", SqlDbType.NVarChar, 204, pattern);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            results.Add(Map(reader));
                        }
                    }
                }

                return results;
            });
        }

        /// <summary>Returns one species, or null if it does not exist.</summary>
        public Task<TreeSpecies> GetByIdAsync(int speciesId)
        {
            return WithConnectionAsync("load the species", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection, SelectColumns + " WHERE s.SpeciesId = @SpeciesId;"))
                {
                    AddParameter(command, "@SpeciesId", SqlDbType.Int, speciesId);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        return await reader.ReadAsync().ConfigureAwait(false) ? Map(reader) : null;
                    }
                }
            });
        }

        /// <summary>Inserts a new species and returns its generated ID.</summary>
        /// <exception cref="DuplicateRecordException">Another species already has the same common name.</exception>
        public Task<int> InsertAsync(TreeSpecies species)
        {
            const string sql = @"
INSERT INTO dbo.TreeSpecies
    (CommonName, ScientificName, CategoryId, ProductType, YieldUnit, YieldPerTreePerYear,
     PricePerUnit, HarvestType, YearsToFirstHarvest, YearsToFullProduction, HarvestSeason, Notes)
OUTPUT INSERTED.SpeciesId
VALUES
    (@CommonName, @ScientificName, @CategoryId, @ProductType, @YieldUnit, @YieldPerTreePerYear,
     @PricePerUnit, @HarvestType, @YearsToFirstHarvest, @YearsToFullProduction, @HarvestSeason, @Notes);";

            return WithConnectionAsync("add the species", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection, sql))
                {
                    AddSpeciesParameters(command, species);
                    object id = await command.ExecuteScalarAsync().ConfigureAwait(false);
                    return Convert.ToInt32(id);
                }
            });
        }

        /// <summary>Saves changes to an existing species.</summary>
        /// <exception cref="DuplicateRecordException">Another species already has the same common name.</exception>
        /// <exception cref="DataAccessException">The species was deleted by someone else.</exception>
        public Task UpdateAsync(TreeSpecies species)
        {
            const string sql = @"
UPDATE dbo.TreeSpecies
SET CommonName = @CommonName,
    ScientificName = @ScientificName,
    CategoryId = @CategoryId,
    ProductType = @ProductType,
    YieldUnit = @YieldUnit,
    YieldPerTreePerYear = @YieldPerTreePerYear,
    PricePerUnit = @PricePerUnit,
    HarvestType = @HarvestType,
    YearsToFirstHarvest = @YearsToFirstHarvest,
    YearsToFullProduction = @YearsToFullProduction,
    HarvestSeason = @HarvestSeason,
    Notes = @Notes,
    UpdatedAt = SYSUTCDATETIME()
WHERE SpeciesId = @SpeciesId;";

            return WithConnectionAsync("update the species", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection, sql))
                {
                    AddSpeciesParameters(command, species);
                    AddParameter(command, "@SpeciesId", SqlDbType.Int, species.SpeciesId);

                    int rows = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    if (rows == 0)
                    {
                        throw new DataAccessException("This species no longer exists. It may have been deleted.", null);
                    }
                }
            });
        }

        /// <summary>
        /// Deletes a species. Its saved scenarios are deleted with it by the
        /// ON DELETE CASCADE rule on the Scenarios foreign key.
        /// </summary>
        public Task DeleteAsync(int speciesId)
        {
            return WithConnectionAsync("delete the species", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection, "DELETE FROM dbo.TreeSpecies WHERE SpeciesId = @SpeciesId;"))
                {
                    AddParameter(command, "@SpeciesId", SqlDbType.Int, speciesId);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });
        }

        private static void AddSpeciesParameters(SqlCommand command, TreeSpecies s)
        {
            AddParameter(command, "@CommonName", SqlDbType.NVarChar, 100, s.CommonName);
            AddParameter(command, "@ScientificName", SqlDbType.NVarChar, 150, s.ScientificName);
            AddParameter(command, "@CategoryId", SqlDbType.Int, s.CategoryId);
            AddParameter(command, "@ProductType", SqlDbType.NVarChar, 60, s.ProductType);
            AddParameter(command, "@YieldUnit", SqlDbType.NVarChar, 20, s.YieldUnit);
            AddDecimal(command, "@YieldPerTreePerYear", s.YieldPerTreePerYear);
            AddDecimal(command, "@PricePerUnit", s.PricePerUnit);
            AddParameter(command, "@HarvestType", SqlDbType.NVarChar, 10, s.HarvestType.ToString());
            AddParameter(command, "@YearsToFirstHarvest", SqlDbType.Int, s.YearsToFirstHarvest);
            AddParameter(command, "@YearsToFullProduction", SqlDbType.Int, s.YearsToFullProduction);
            AddParameter(command, "@HarvestSeason", SqlDbType.NVarChar, 100, s.HarvestSeason);
            AddParameter(command, "@Notes", SqlDbType.NVarChar, 1000, s.Notes);
        }

        private static TreeSpecies Map(SqlDataReader reader)
        {
            HarvestType harvestType;
            if (!Enum.TryParse(ReadString(reader, "HarvestType"), true, out harvestType))
            {
                harvestType = HarvestType.Annual;
            }

            return new TreeSpecies
            {
                SpeciesId = reader.GetInt32(reader.GetOrdinal("SpeciesId")),
                CommonName = ReadString(reader, "CommonName"),
                ScientificName = ReadString(reader, "ScientificName"),
                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                CategoryName = ReadString(reader, "CategoryName"),
                ProductType = ReadString(reader, "ProductType"),
                YieldUnit = ReadString(reader, "YieldUnit"),
                YieldPerTreePerYear = reader.GetDecimal(reader.GetOrdinal("YieldPerTreePerYear")),
                PricePerUnit = reader.GetDecimal(reader.GetOrdinal("PricePerUnit")),
                HarvestType = harvestType,
                YearsToFirstHarvest = reader.GetInt32(reader.GetOrdinal("YearsToFirstHarvest")),
                YearsToFullProduction = reader.GetInt32(reader.GetOrdinal("YearsToFullProduction")),
                HarvestSeason = ReadString(reader, "HarvestSeason"),
                Notes = ReadString(reader, "Notes"),
                CreatedAtUtc = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAtUtc = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }
    }
}
