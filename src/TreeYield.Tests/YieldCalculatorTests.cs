using System;
using TreeYield.Core.Models;
using TreeYield.Core.Services;
using TreeYield.Tests.Framework;

namespace TreeYield.Tests
{
    /// <summary>Tests for the calculation engine (<see cref="YieldCalculator"/>).</summary>
    public class YieldCalculatorTests
    {
        [Test]
        public void Calculate_25CarabaoMangoTrees_Gives7500KgAnd600000Pesos()
        {
            // The example from the original design mock-up.
            YieldResult result = YieldCalculator.Calculate(TestData.Mango(), 25);

            Assert.AreEqual(7500m, result.AnnualYield);
            Assert.AreEqual(600000m, result.GrossSales);
            Assert.IsFalse(result.IsAnnualized);
        }

        [Test]
        public void Calculate_DecimalYield_KeepsExactDecimals()
        {
            TreeSpecies cacao = TestData.Mango();
            cacao.YieldPerTreePerYear = 1.5m;
            cacao.PricePerUnit = 180m;

            YieldResult result = YieldCalculator.Calculate(cacao, 25);

            Assert.AreEqual(37.5m, result.AnnualYield);
            Assert.AreEqual(6750m, result.GrossSales);
        }

        [Test]
        public void Calculate_ZeroTrees_IsRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => YieldCalculator.Calculate(TestData.Mango(), 0));
        }

        [Test]
        public void Calculate_NegativeTrees_IsRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => YieldCalculator.Calculate(TestData.Mango(), -5));
        }

        [Test]
        public void Calculate_MoreThanMaximumTrees_IsRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                YieldCalculator.Calculate(TestData.Mango(), YieldCalculator.MaxTreeCount + 1));
        }

        [Test]
        public void Calculate_NullSpecies_IsRejected()
        {
            Assert.Throws<ArgumentNullException>(() => YieldCalculator.Calculate(null, 10));
        }

        [Test]
        public void Calculate_NegativePrice_IsRejected()
        {
            TreeSpecies bad = TestData.Mango();
            bad.PricePerUnit = -1m;
            Assert.Throws<ArgumentException>(() => YieldCalculator.Calculate(bad, 10));
        }

        [Test]
        public void Calculate_LargestAllowedValues_DoNotOverflow()
        {
            TreeSpecies huge = TestData.Mango();
            huge.YieldPerTreePerYear = SpeciesValidator.MaxYieldPerTree;
            huge.PricePerUnit = SpeciesValidator.MaxPricePerUnit;

            YieldResult result = YieldCalculator.Calculate(huge, YieldCalculator.MaxTreeCount);

            Assert.AreEqual(10000000000m, result.AnnualYield);
            Assert.AreEqual(10000000000000000m, result.GrossSales);
        }

        [Test]
        public void Calculate_RotationSpecies_ReportsOneTimeHarvest()
        {
            YieldResult result = YieldCalculator.Calculate(TestData.Gmelina(), 25);

            Assert.IsTrue(result.IsAnnualized);
            Assert.AreEqual(375m, result.AnnualYield, "25 trees x 15 board ft");
            Assert.AreEqual(10, result.HarvestYear);
            Assert.AreEqual(3750m, result.HarvestYield, "375 board ft x 10 years");
            Assert.AreEqual(131250m, result.HarvestSales, "3,750 board ft x PHP 35");
        }

        [Test]
        public void ProductionFactor_BeforeFirstHarvest_IsZero()
        {
            Assert.AreEqual(0m, YieldCalculator.GetProductionFactor(TestData.Mango(), 1));
            Assert.AreEqual(0m, YieldCalculator.GetProductionFactor(TestData.Mango(), 4));
        }

        [Test]
        public void ProductionFactor_RampsUpInEqualSteps()
        {
            TreeSpecies species = TestData.Mango();
            species.YearsToFirstHarvest = 5;
            species.YearsToFullProduction = 8;

            Assert.AreEqual(0.25m, YieldCalculator.GetProductionFactor(species, 5));
            Assert.AreEqual(0.5m, YieldCalculator.GetProductionFactor(species, 6));
            Assert.AreEqual(0.75m, YieldCalculator.GetProductionFactor(species, 7));
            Assert.AreEqual(1m, YieldCalculator.GetProductionFactor(species, 8));
            Assert.AreEqual(1m, YieldCalculator.GetProductionFactor(species, 20));
        }

        [Test]
        public void ProductionFactor_SameFirstAndFullYear_JumpsToFull()
        {
            TreeSpecies species = TestData.Mango();
            species.YearsToFirstHarvest = 3;
            species.YearsToFullProduction = 3;

            Assert.AreEqual(0m, YieldCalculator.GetProductionFactor(species, 2));
            Assert.AreEqual(1m, YieldCalculator.GetProductionFactor(species, 3));
        }

        [Test]
        public void ProductionFactor_ZeroYears_MeansBearingFromYearOne()
        {
            TreeSpecies species = TestData.Mango();
            species.YearsToFirstHarvest = 0;
            species.YearsToFullProduction = 0;

            Assert.AreEqual(1m, YieldCalculator.GetProductionFactor(species, 1));
        }

        [Test]
        public void ProductionFactor_YearZero_IsRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => YieldCalculator.GetProductionFactor(TestData.Mango(), 0));
        }

        [Test]
        public void Project_Mango10Years_FollowsRampAndTotals()
        {
            // First harvest year 5, full production year 10: years 5..10 give 1/6, 2/6 ... 6/6.
            YieldProjection projection = YieldCalculator.Project(TestData.Mango(), 25, 10);

            Assert.AreEqual(10, projection.Rows.Count);
            Assert.AreEqual(0m, projection.Rows[3].Yield, "year 4 is not bearing yet");
            Assert.AreEqual(1250m, decimal.Round(projection.Rows[4].Yield, 6), "year 5 = 7,500 / 6");
            Assert.AreEqual(7500m, projection.Rows[9].Yield, "year 10 = full production");

            // 7,500 x (1+2+3+4+5+6)/6 = 26,250 kg; x PHP 80 = PHP 2,100,000.
            Assert.AreEqual(26250m, decimal.Round(projection.TotalYield, 6));
            Assert.AreEqual(2100000m, decimal.Round(projection.TotalSales, 4));
            Assert.AreEqual(projection.TotalSales, projection.Rows[9].CumulativeSales);
            Assert.AreEqual(5, projection.FirstHarvestYear);
            Assert.IsFalse(projection.FirstHarvestBeyondHorizon);
        }

        [Test]
        public void Project_RotationSpecies_HarvestsOnlyAtRotationAge()
        {
            YieldProjection projection = YieldCalculator.Project(TestData.Gmelina(), 25, 10);

            for (int i = 0; i < 9; i++)
            {
                Assert.AreEqual(0m, projection.Rows[i].GrossSales, "year " + (i + 1));
            }

            Assert.IsTrue(projection.Rows[9].IsHarvestYear);
            Assert.AreEqual(3750m, projection.Rows[9].Yield);
            Assert.AreEqual(131250m, projection.TotalSales);
        }

        [Test]
        public void Project_RotationSpecies_RepeatsAfterReplanting()
        {
            YieldProjection projection = YieldCalculator.Project(TestData.Gmelina(), 25, 20);

            Assert.IsTrue(projection.Rows[9].IsHarvestYear, "year 10");
            Assert.IsTrue(projection.Rows[19].IsHarvestYear, "year 20");
            Assert.AreEqual(262500m, projection.TotalSales);
        }

        [Test]
        public void Project_HarvestAfterHorizon_IsFlagged()
        {
            TreeSpecies mahogany = TestData.Gmelina();
            mahogany.YearsToFirstHarvest = 25;
            mahogany.YearsToFullProduction = 25;

            YieldProjection projection = YieldCalculator.Project(mahogany, 10, 10);

            Assert.IsTrue(projection.FirstHarvestBeyondHorizon);
            Assert.AreEqual(25, projection.FirstHarvestYear);
            Assert.AreEqual(0m, projection.TotalSales);
        }

        [Test]
        public void Project_HorizonOutsideRange_IsRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => YieldCalculator.Project(TestData.Mango(), 10, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                YieldCalculator.Project(TestData.Mango(), 10, YieldCalculator.MaxProjectionYears + 1));
        }
    }
}
