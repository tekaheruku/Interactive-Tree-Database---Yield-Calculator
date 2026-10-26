using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TreeYield.Core.Models;

namespace TreeYield.Core.Data
{
    /// <summary>Writes and reads the AuditLog table (history of administrator actions).</summary>
    public class AuditLogRepository : RepositoryBase
    {
        /// <summary>Maximum length of the Details column.</summary>
        public const int MaxDetailsLength = 500;

        /// <summary>Creates the repository.</summary>
        public AuditLogRepository(SqlConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        /// <summary>Adds one audit entry.</summary>
        /// <param name="userId">Who did it (null if unknown).</param>
        /// <param name="action">Action code from <see cref="Services.AuditActions"/>.</param>
        /// <param name="entityName">Kind of record, for example "TreeSpecies".</param>
        /// <param name="entityId">ID of the record, if any.</param>
        /// <param name="details">Description; cut to <see cref="MaxDetailsLength"/> characters.</param>
        public Task AddAsync(int? userId, string action, string entityName, int? entityId, string details)
        {
            const string sql = @"
INSERT INTO dbo.AuditLog (UserId, Action, EntityName, EntityId, Details)
VALUES (@UserId, @Action, @EntityName, @EntityId, @Details);";

            string safeDetails = details != null && details.Length > MaxDetailsLength
                ? details.Substring(0, MaxDetailsLength)
                : details;

            return WithConnectionAsync("write the audit log", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection, sql))
                {
                    AddParameter(command, "@UserId", SqlDbType.Int, userId);
                    AddParameter(command, "@Action", SqlDbType.NVarChar, 30, action);
                    AddParameter(command, "@EntityName", SqlDbType.NVarChar, 50, entityName);
                    AddParameter(command, "@EntityId", SqlDbType.Int, entityId);
                    AddParameter(command, "@Details", SqlDbType.NVarChar, MaxDetailsLength, safeDetails);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });
        }

        /// <summary>Returns the most recent entries, newest first.</summary>
        /// <param name="count">How many entries to return.</param>
        public Task<List<AuditEntry>> GetRecentAsync(int count)
        {
            const string sql = @"
SELECT TOP (@Count) a.AuditId, a.UserId, u.Username, a.Action, a.EntityName, a.EntityId, a.Details, a.OccurredAt
FROM dbo.AuditLog AS a
LEFT JOIN dbo.Users AS u ON u.UserId = a.UserId
ORDER BY a.OccurredAt DESC, a.AuditId DESC;";

            return WithConnectionAsync("load the audit log", async connection =>
            {
                var entries = new List<AuditEntry>();
                using (SqlCommand command = CreateCommand(connection, sql))
                {
                    AddParameter(command, "@Count", SqlDbType.Int, count);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            entries.Add(new AuditEntry
                            {
                                AuditId = reader.GetInt32(reader.GetOrdinal("AuditId")),
                                UserId = ReadNullableInt(reader, "UserId"),
                                Username = ReadString(reader, "Username"),
                                Action = ReadString(reader, "Action"),
                                EntityName = ReadString(reader, "EntityName"),
                                EntityId = ReadNullableInt(reader, "EntityId"),
                                Details = ReadString(reader, "Details"),
                                OccurredAtUtc = reader.GetDateTime(reader.GetOrdinal("OccurredAt"))
                            });
                        }
                    }
                }

                return entries;
            });
        }
    }
}
