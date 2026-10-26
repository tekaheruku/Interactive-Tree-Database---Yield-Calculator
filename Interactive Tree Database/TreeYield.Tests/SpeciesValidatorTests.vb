Imports TreeYield.Core.Models
Imports TreeYield.Core.Services
Imports TreeYield.Tests.Framework

''' <summary>Tests for the input rules in <see cref="SpeciesValidator"/> and <see cref="ScenarioService.Validate"/>.</summary>
Public Class SpeciesValidatorTests

    <Test>
    Public Sub ValidSpecies_HasNoErrors()
        Dim result As ValidationResult = SpeciesValidator.Validate(TestData.Mango())
        Assert.IsTrue(result.IsValid, result.ToString())
    End Sub

    <Test>
    Public Sub ValidRotationSpecies_HasNoErrors()
        Dim result As ValidationResult = SpeciesValidator.Validate(TestData.Gmelina())
        Assert.IsTrue(result.IsValid, result.ToString())
    End Sub

    <Test>
    Public Sub BlankName_IsRequired()
        Dim species As TreeSpecies = TestData.Mango()
        species.CommonName = "   "

        Dim result As ValidationResult = SpeciesValidator.Validate(species)

        Assert.AreEqual("Common name is required.", result.GetError(NameOf(TreeSpecies.CommonName)))
    End Sub

    <Test>
    Public Sub NameLongerThan100Characters_IsRejected()
        Dim species As TreeSpecies = TestData.Mango()
        species.CommonName = New String("A"c, 101)

        Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(NameOf(TreeSpecies.CommonName)))
    End Sub

    <Test>
    Public Sub ControlCharacters_AreRejected()
        Dim species As TreeSpecies = TestData.Mango()
        species.CommonName = "Mango" & ChrW(7)

        Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(NameOf(TreeSpecies.CommonName)))
    End Sub

    <Test>
    Public Sub MissingCategory_IsRejected()
        Dim species As TreeSpecies = TestData.Mango()
        species.CategoryId = 0

        Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(NameOf(TreeSpecies.CategoryId)))
    End Sub

    <Test>
    Public Sub ZeroYield_IsRejected()
        Dim species As TreeSpecies = TestData.Mango()
        species.YieldPerTreePerYear = 0D

        Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(NameOf(TreeSpecies.YieldPerTreePerYear)))
    End Sub

    <Test>
    Public Sub YieldWithThreeDecimals_IsRejected()
        Dim species As TreeSpecies = TestData.Mango()
        species.YieldPerTreePerYear = 1.555D

        Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(NameOf(TreeSpecies.YieldPerTreePerYear)))
    End Sub

    <Test>
    Public Sub NegativePrice_IsRejected()
        Dim species As TreeSpecies = TestData.Mango()
        species.PricePerUnit = -0.01D

        Assert.AreEqual("Price cannot be negative.", SpeciesValidator.Validate(species).GetError(NameOf(TreeSpecies.PricePerUnit)))
    End Sub

    <Test>
    Public Sub FreePrice_IsAllowed()
        Dim species As TreeSpecies = TestData.Mango()
        species.PricePerUnit = 0D

        Assert.IsTrue(SpeciesValidator.Validate(species).IsValid)
    End Sub

    <Test>
    Public Sub FullProductionBeforeFirstHarvest_IsRejected()
        Dim species As TreeSpecies = TestData.Mango()
        species.YearsToFirstHarvest = 8
        species.YearsToFullProduction = 5

        Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(NameOf(TreeSpecies.YearsToFullProduction)))
    End Sub

    <Test>
    Public Sub RotationAgeZero_IsRejected()
        Dim species As TreeSpecies = TestData.Gmelina()
        species.YearsToFirstHarvest = 0
        species.YearsToFullProduction = 0

        Assert.IsNotNull(SpeciesValidator.Validate(species).GetError(NameOf(TreeSpecies.YearsToFirstHarvest)))
    End Sub

    <Test>
    Public Sub SeveralProblems_AreAllReported()
        Dim species As TreeSpecies = TestData.Mango()
        species.CommonName = ""
        species.ProductType = ""
        species.PricePerUnit = -5D

        Assert.AreEqual(3, SpeciesValidator.Validate(species).Errors.Count)
    End Sub

    <Test>
    Public Sub Normalize_TrimsTextAndSyncsRotationYears()
        Dim species As TreeSpecies = TestData.Gmelina()
        species.CommonName = "  Gmelina  "
        species.Notes = "   "
        species.YearsToFullProduction = 3

        Dim clean As TreeSpecies = SpeciesService.Normalize(species)

        Assert.AreEqual("Gmelina", clean.CommonName)
        Assert.IsNull(clean.Notes)
        Assert.AreEqual(10, clean.YearsToFullProduction)
        Assert.AreEqual("  Gmelina  ", species.CommonName, "the original object must not change")
    End Sub

    <Test>
    Public Sub Scenario_WithoutName_IsRejected()
        Dim scenario As New Scenario With {.Name = " ", .SpeciesId = 1, .TreeCount = 25, .ProjectionYears = 10}
        Assert.IsNotNull(ScenarioService.Validate(scenario).GetError(NameOf(Scenario.Name)))
    End Sub

    <Test>
    Public Sub Scenario_TreeCountOutOfRange_IsRejected()
        Dim scenario As New Scenario With {.Name = "Plan", .SpeciesId = 1, .TreeCount = 0, .ProjectionYears = 10}
        Assert.IsNotNull(ScenarioService.Validate(scenario).GetError(NameOf(Scenario.TreeCount)))
    End Sub

    <Test>
    Public Sub Scenario_Valid_HasNoErrors()
        Dim scenario As New Scenario With {.Name = "Backyard plan", .SpeciesId = 1, .TreeCount = 25, .ProjectionYears = 10}
        Assert.IsTrue(ScenarioService.Validate(scenario).IsValid)
    End Sub

End Class
