using System;

namespace TreeYield.Core.Models
{
    /// <summary>One row of the AuditLog table: who did what, and when.</summary>
    public class AuditEntry
    {
        /// <summary>Primary key.</summary>
        public int AuditId { get; set; }

        /// <summary>The user who performed the action, or null (for example a failed login with an unknown name).</summary>
        public int? UserId { get; set; }

        /// <summary>Username loaded through a JOIN. Display only.</summary>
        public string Username { get; set; }

        /// <summary>Action code, see <see cref="Services.AuditActions"/>.</summary>
        public string Action { get; set; }

        /// <summary>Kind of record affected, for example "TreeSpecies".</summary>
        public string EntityName { get; set; }

        /// <summary>Primary key of the affected record, if any.</summary>
        public int? EntityId { get; set; }

        /// <summary>Human-readable description of the change.</summary>
        public string Details { get; set; }

        /// <summary>When the action happened (UTC).</summary>
        public DateTime OccurredAtUtc { get; set; }
    }
}
