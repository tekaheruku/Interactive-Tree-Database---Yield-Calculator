Imports TreeYield.Core.Models
Imports TreeYield.Core.Services
Imports TreeYield.Tests.Framework

''' <summary>Tests for the calculation engine (<see cref="YieldCalculator"/>).</summary>
Public Class YieldCalculatorTests

    <Test>
    Public Sub Calculate_25CarabaoMangoTrees_Gives7500KgAnd600000Pesos()
        ' The example from the original design mock-up.
        Dim result As YieldResult = YieldCalculator.Calculate(TestData.Mango(), 25)

        Assert.AreEqual(7500D, result.AnnualYield)
        Assert.AreEqual(600000D, result.GrossSales)
        Assert.IsFalse(result.IsAnnualized)
    End Sub

    <Test>
    Public Sub Calculate_DecimalYield_KeepsExactDecimals()
        Dim cacao As TreeSpecies = TestData.Mango()
        cacao.YieldPerTreePerYear = 1.5D
        cacao.PricePerUnit = 180D

        Dim result As YieldResult = YieldCalculator.Calculate(cacao, 25)

        Assert.AreEqual(37.5D, result.AnnualYield)
        Assert.AreEqual(6750D, result.GrossSales)
    End Sub

    <Test>
    Public Sub Calculate_ZeroTrees_IsRejected()
        Assert.Throws(Of ArgumentOutOfRangeException)(Sub() YieldCalculator.Calculate(TestData.Mango(), 0))
    End Sub

    <Test>
    Public Sub Calculate_NegativeTrees_IsRejected()
        Assert.Throws(Of ArgumentOutOfRangeException)(Sub() YieldCalculator.Calculate(TestData.Mango(), -5))
    End Sub

    <Test>
    Public Sub Calculate_MoreThanMaximumTrees_IsRejected()
        Assert.Throws(Of ArgumentOutOfRangeException)(Sub() YieldCalculator.Calculate(TestData.Mango(), YieldCalculator.MaxTreeCount + 1))
    End Sub

    <Test>
    Public Sub Calculate_NullSpecies_IsRejected()
        Assert.Throws(Of ArgumentNullException)(Sub() YieldCalculator.Calculate(Nothing, 10))
    End Sub

    <Test>
    Public Sub Calculate_NegativePrice_IsRejected()
        Dim bad As TreeSpecies = TestData.Mango()
        bad.PricePerUnit = -1D
        Assert.Throws(Of ArgumentException)(Sub() YieldCalculator.Calculate(bad, 10))
    End Sub

    <Test>
    Public Sub Calculate_LargestAllowedValues_DoNotOverflow()
        Dim huge As TreeSpecies = TestData.Mango()
        huge.YieldPerTreePerYear = SpeciesValidator.MaxYieldPerTree
        huge.PricePerUnit = SpeciesValidator.MaxPricePerUnit

        Dim result As YieldResult = YieldCalculator.Calculate(huge, YieldCalculator.MaxTreeCount)

        Assert.AreEqual(10000000000D, result.AnnualYield)
        Assert.AreEqual(10000000000000000D, result.GrossSales)
    End Sub

    <Test>
    Public Sub Calculate_RotationSpecies_ReportsOneTimeHarvest()
        Dim result As YieldResult = YieldCalculator.Calculate(TestData.Gmelina(), 25)

        Assert.IsTrue(result.IsAnnualized)
        Assert.AreEqual(375D, result.AnnualYield, "25 trees x 15 board ft")
        Assert.AreEqual(10, result.HarvestYear)
        Assert.AreEqual(3750D, result.HarvestYield, "375 board ft x 10 years")
        Assert.AreEqual(131250D, result.HarvestSales, "3,750 board ft x PHP 35")
    End Sub

    <Test>
    Public Sub ProductionFactor_BeforeFirstHarvest_IsZero()
        Assert.AreEqual(0D, YieldCalculator.GetProductionFactor(TestData.Mango(), 1))
        Assert.AreEqual(0D, YieldCalculator.GetProductionFactor(TestData.Mango(), 4))
    End Sub

    <Test>
    Public Sub ProductionFactor_RampsUpInEqualSteps()
        Dim species As TreeSpecies = TestData.Mango()
        species.YearsToFirstHarvest = 5
        species.YearsToFullProduction = 8

        Assert.AreEqual(0.25D, YieldCalculator.GetProductionFactor(species, 5))
        Assert.AreEqual(0.5D, YieldCalculator.GetProductionFactor(species, 6))
        Assert.AreEqual(0.75D, YieldCalculator.GetProductionFactor(species, 7))
        Assert.AreEqual(1D, YieldCalculator.GetProductionFactor(species, 8))
        Assert.AreEqual(1D, YieldCalculator.GetProductionFactor(species, 20))
    End Sub

    <Test>
    Public Sub ProductionFactor_SameFirstAndFullYear_JumpsToFull()
        Dim species As TreeSpecies = TestData.Mango()
        species.YearsToFirstHarvest = 3
        species.YearsToFullProduction = 3

        Assert.AreEqual(0D, YieldCalculator.GetProductionFactor(species, 2))
        Assert.AreEqual(1D, YieldCalculator.GetProductionFactor(species, 3))
    End Sub

    <Test>
    Public Sub ProductionFactor_ZeroYears_MeansBearingFromYearOne()
        Dim species As TreeSpecies = TestData.Mango()
        species.YearsToFirstHarvest = 0
        species.YearsToFullProduction = 0

        Assert.AreEqual(1D, YieldCalculator.GetProductionFactor(species, 1))
    End Sub

    <Test>
    Public Sub ProductionFactor_YearZero_IsRejected()
        Assert.Throws(Of ArgumentOutOfRangeException)(Sub() YieldCalculator.GetProductionFactor(TestData.Mango(), 0))
    End Sub

    <Test>
    Public Sub Project_Mango10Years_FollowsRampAndTotals()
        ' First harvest year 5, full production year 10: years 5..10 give 1/6, 2/6 ... 6/6.
        Dim projection As YieldProjection = YieldCalculator.Project(TestData.Mango(), 25, 10)

        Assert.AreEqual(10, projection.Rows.Count)
        Assert.AreEqual(0D, projection.Rows(3).Yield, "year 4 is not bearing yet")
        Assert.AreEqual(1250D, Decimal.Round(projection.Rows(4).Yield, 6), "year 5 = 7,500 / 6")
        Assert.AreEqual(7500D, projection.Rows(9).Yield, "year 10 = full production")

        ' 7,500 x (1+2+3+4+5+6)/6 = 26,250 kg; x PHP 80 = PHP 2,100,000.
        Assert.AreEqual(26250D, Decimal.Round(projection.TotalYield, 6))
        Assert.AreEqual(2100000D, Decimal.Round(projection.TotalSales, 4))
        Assert.AreEqual(projection.TotalSales, projection.Rows(9).CumulativeSales)
        Assert.AreEqual(5, projection.FirstHarvestYear)
        Assert.IsFalse(projection.FirstHarvestBeyondHorizon)
    End Sub

    <Test>
    Public Sub Project_RotationSpecies_HarvestsOnlyAtRotationAge()
        Dim projection As YieldProjection = YieldCalculator.Project(TestData.Gmelina(), 25, 10)

        For i As Integer = 0 To 8
            Assert.AreEqual(0D, projection.Rows(i).GrossSales, "year " & (i + 1).ToString())
        Next

        Assert.IsTrue(projection.Rows(9).IsHarvestYear)
        Assert.AreEqual(3750D, projection.Rows(9).Yield)
        Assert.AreEqual(131250D, projection.TotalSales)
    End Sub

    <Test>
    Public Sub Project_RotationSpecies_RepeatsAfterReplanting()
        Dim projection As YieldProjection = YieldCalculator.Project(TestData.Gmelina(), 25, 20)

        Assert.IsTrue(projection.Rows(9).IsHarvestYear, "year 10")
        Assert.IsTrue(projection.Rows(19).IsHarvestYear, "year 20")
        Assert.AreEqual(262500D, projection.TotalSales)
    End Sub

    <Test>
    Public Sub Project_HarvestAfterHorizon_IsFlagged()
        Dim mahogany As TreeSpecies = TestData.Gmelina()
        mahogany.YearsToFirstHarvest = 25
        mahogany.YearsToFullProduction = 25

        Dim projection As YieldProjection = YieldCalculator.Project(mahogany, 10, 10)

        Assert.IsTrue(projection.FirstHarvestBeyondHorizon)
        Assert.AreEqual(25, projection.FirstHarvestYear)
        Assert.AreEqual(0D, projection.TotalSales)
    End Sub

    <Test>
    Public Sub Project_HorizonOutsideRange_IsRejected()
        Assert.Throws(Of ArgumentOutOfRangeException)(Sub() YieldCalculator.Project(TestData.Mango(), 10, 0))
        Assert.Throws(Of ArgumentOutOfRangeException)(Sub() YieldCalculator.Project(TestData.Mango(), 10, YieldCalculator.MaxProjectionYears + 1))
    End Sub

End Class
