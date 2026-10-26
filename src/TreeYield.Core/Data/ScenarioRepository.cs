using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TreeYield.Core.Models;

namespace TreeYield.Core.Data
{
    /// <summary>Reads and writes the Scenarios table (saved calculations).</summary>
    public class ScenarioRepository : RepositoryBase
    {
        /// <summary>Creates the repository.</summary>
        public ScenarioRepository(SqlConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        /// <summary>Returns all scenarios, newest first, with the species name joined in.</summary>
        public Task<List<Scenario>> GetAllAsync()
        {
            const string sql = @"
SELECT sc.ScenarioId, sc.Name, sc.SpeciesId, sp.CommonName AS SpeciesName,
       sc.TreeCount, sc.ProjectionYears, sc.Notes, sc.CreatedAt
FROM dbo.Scenarios AS sc
INNER JOIN dbo.TreeSpecies AS sp ON sp.SpeciesId = sc.SpeciesId
ORDER BY sc.CreatedAt DESC, sc.ScenarioId DESC;";

            return WithConnectionAsync("load the saved scenarios", async connection =>
            {
                var scenarios = new List<Scenario>();
                using (SqlCommand command = CreateCommand(connection, sql))
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        scenarios.Add(new Scenario
                        {
                            ScenarioId = reader.GetInt32(reader.GetOrdinal("ScenarioId")),
                            Name = ReadString(reader, "Name"),
                            SpeciesId = reader.GetInt32(reader.GetOrdinal("SpeciesId")),
                            SpeciesName = ReadString(reader, "SpeciesName"),
                            TreeCount = reader.GetInt32(reader.GetOrdinal("TreeCount")),
                            ProjectionYears = reader.GetInt32(reader.GetOrdinal("ProjectionYears")),
                            Notes = ReadString(reader, "Notes"),
                            CreatedAtUtc = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                        });
                    }
                }

                return scenarios;
            });
        }

        /// <summary>Saves a new scenario and returns its generated ID.</summary>
        public Task<int> InsertAsync(Scenario scenario)
        {
            const string sql = @"
INSERT INTO dbo.Scenarios (Name, SpeciesId, TreeCount, ProjectionYears, Notes)
OUTPUT INSERTED.ScenarioId
VALUES (@Name, @SpeciesId, @TreeCount, @ProjectionYears, @Notes);";

            return WithConnectionAsync("save the scenario", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection, sql))
                {
                    AddParameter(command, "@Name", SqlDbType.NVarChar, 100, scenario.Name);
                    AddParameter(command, "@SpeciesId", SqlDbType.Int, scenario.SpeciesId);
                    AddParameter(command, "@TreeCount", SqlDbType.Int, scenario.TreeCount);
                    AddParameter(command, "@ProjectionYears", SqlDbType.Int, scenario.ProjectionYears);
                    AddParameter(command, "@Notes", SqlDbType.NVarChar, 500, scenario.Notes);
                    return Convert.ToInt32(await command.ExecuteScalarAsync().ConfigureAwait(false));
                }
            });
        }

        /// <summary>Deletes one scenario.</summary>
        public Task DeleteAsync(int scenarioId)
        {
            return WithConnectionAsync("delete the scenario", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection, "DELETE FROM dbo.Scenarios WHERE ScenarioId = @ScenarioId;"))
                {
                    AddParameter(command, "@ScenarioId", SqlDbType.Int, scenarioId);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });
        }

        /// <summary>Counts the scenarios that use a species (shown before deleting the species).</summary>
        public Task<int> CountBySpeciesAsync(int speciesId)
        {
            return WithConnectionAsync("count the saved scenarios", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection, "SELECT COUNT(*) FROM dbo.Scenarios WHERE SpeciesId = @SpeciesId;"))
                {
                    AddParameter(command, "@SpeciesId", SqlDbType.Int, speciesId);
                    return Convert.ToInt32(await command.ExecuteScalarAsync().ConfigureAwait(false));
                }
            });
        }
    }
}
