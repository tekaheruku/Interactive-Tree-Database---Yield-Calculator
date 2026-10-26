using System;

namespace TreeYield.Core.Models
{
    /// <summary>
    /// A saved calculation (row of the Scenarios table). Only the inputs are
    /// stored; results are recalculated with the current species data when loaded.
    /// </summary>
    public class Scenario
    {
        /// <summary>Primary key. 0 means not saved yet.</summary>
        public int ScenarioId { get; set; }

        /// <summary>Name given by the user, for example "Backyard mango plan".</summary>
        public string Name { get; set; }

        /// <summary>Foreign key to TreeSpecies.</summary>
        public int SpeciesId { get; set; }

        /// <summary>Species common name loaded through a JOIN. Display only.</summary>
        public string SpeciesName { get; set; }

        /// <summary>Number of trees in the scenario.</summary>
        public int TreeCount { get; set; }

        /// <summary>Projection horizon in years.</summary>
        public int ProjectionYears { get; set; }

        /// <summary>Optional remarks.</summary>
        public string Notes { get; set; }

        /// <summary>When the scenario was saved (UTC).</summary>
        public DateTime CreatedAtUtc { get; set; }
    }
}
