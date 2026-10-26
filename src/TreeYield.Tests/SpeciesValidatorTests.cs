using TreeYield.Core.Models;
using TreeYield.Core.Services;
using TreeYield.Tests.Framework;

namespace TreeYield.Tests
{
    /// <summary>Tests for the input rules in <see cref="SpeciesValidator"/> and <see cref="ScenarioService.Validate"/>.</summary>
    public class SpeciesValidatorTests
    {
        [Test]
        public void ValidSpecies_HasNoErrors()
        {
            ValidationResult result = SpeciesValidator.Validate(TestData.Mango());
            Assert.IsTrue(result.IsValid, result.ToString());
        }

        [Test]
        public void ValidRotationSpecies_HasNoErrors()
        {
            ValidationResult result = SpeciesValidator.Validate(TestData.Gmelina());
            Assert.IsTrue(result.IsValid, result.ToString());
        }

        [Test]
        public void BlankName_IsRequired()
        {
            TreeSpecies species = TestData.Mango();
            species.CommonName = "   ";

            ValidationResult result = SpeciesValidator.Validate(species);

            Assert.AreEqual("Common name is required.", result.GetError(nameof(TreeSpecies.CommonName)));
        }

        [Test]
        public void NameLongerThan100Characters_IsRejected()
        {
            TreeSpecies species = TestData.Mango();
            species.CommonName = new string('A', 101);

            Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(nameof(TreeSpecies.CommonName)));
        }

        [Test]
        public void ControlCharacters_AreRejected()
        {
            TreeSpecies species = TestData.Mango();
            species.CommonName = "Mango\u0007";

            Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(nameof(TreeSpecies.CommonName)));
        }

        [Test]
        public void MissingCategory_IsRejected()
        {
            TreeSpecies species = TestData.Mango();
            species.CategoryId = 0;

            Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(nameof(TreeSpecies.CategoryId)));
        }

        [Test]
        public void ZeroYield_IsRejected()
        {
            TreeSpecies species = TestData.Mango();
            species.YieldPerTreePerYear = 0m;

            Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(nameof(TreeSpecies.YieldPerTreePerYear)));
        }

        [Test]
        public void YieldWithThreeDecimals_IsRejected()
        {
            TreeSpecies species = TestData.Mango();
            species.YieldPerTreePerYear = 1.555m;

            Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(nameof(TreeSpecies.YieldPerTreePerYear)));
        }

        [Test]
        public void NegativePrice_IsRejected()
        {
            TreeSpecies species = TestData.Mango();
            species.PricePerUnit = -0.01m;

            Assert.AreEqual("Price cannot be negative.", SpeciesValidator.Validate(species).GetError(nameof(TreeSpecies.PricePerUnit)));
        }

        [Test]
        public void FreePrice_IsAllowed()
        {
            TreeSpecies species = TestData.Mango();
            species.PricePerUnit = 0m;

            Assert.IsTrue(SpeciesValidator.Validate(species).IsValid);
        }

        [Test]
        public void FullProductionBeforeFirstHarvest_IsRejected()
        {
            TreeSpecies species = TestData.Mango();
            species.YearsToFirstHarvest = 8;
            species.YearsToFullProduction = 5;

            Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(nameof(TreeSpecies.YearsToFullProduction)));
        }

        [Test]
        public void RotationAgeZero_IsRejected()
        {
            TreeSpecies species = TestData.Gmelina();
            species.YearsToFirstHarvest = 0;
            species.YearsToFullProduction = 0;

            Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(nameof(TreeSpecies.YearsToFirstHarvest)));
        }

        [Test]
        public void SeveralProblems_AreAllReported()
        {
            TreeSpecies species = TestData.Mango();
            species.CommonName = "";
            species.ProductType = "";
            species.PricePerUnit = -5m;

            Assert.AreEqual(3, SpeciesValidator.Validate(species).Errors.Count);
        }

        [Test]
        public void Normalize_TrimsTextAndSyncsRotationYears()
        {
            TreeSpecies species = TestData.Gmelina();
            species.CommonName = "  Gmelina  ";
            species.Notes = "   ";
            species.YearsToFullProduction = 3;

            TreeSpecies clean = SpeciesService.Normalize(species);

            Assert.AreEqual("Gmelina", clean.CommonName);
            Assert.IsNull(clean.Notes);
            Assert.AreEqual(10, clean.YearsToFullProduction);
            Assert.AreEqual("  Gmelina  ", species.CommonName, "the original object must not change");
        }

        [Test]
        public void Scenario_WithoutName_IsRejected()
        {
            var scenario = new Scenario { Name = " ", SpeciesId = 1, TreeCount = 25, ProjectionYears = 10 };
            Assert.IsNotNull(ScenarioService.Validate(scenario).GetError(nameof(Scenario.Name)));
        }

        [Test]
        public void Scenario_TreeCountOutOfRange_IsRejected()
        {
            var scenario = new Scenario { Name = "Plan", SpeciesId = 1, TreeCount = 0, ProjectionYears = 10 };
            Assert.IsNotNull(ScenarioService.Validate(scenario).GetError(nameof(Scenario.TreeCount)));
        }

        [Test]
        public void Scenario_Valid_HasNoErrors()
        {
            var scenario = new Scenario { Name = "Backyard plan", SpeciesId = 1, TreeCount = 25, ProjectionYears = 10 };
            Assert.IsTrue(ScenarioService.Validate(scenario).IsValid);
        }
    }
}
