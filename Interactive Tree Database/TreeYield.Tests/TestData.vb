Imports TreeYield.Core.Models

''' <summary>Ready-made species used by several test classes.</summary>
Friend NotInheritable Class TestData

    Private Sub New()
        ' Only Shared members.
    End Sub

    ''' <summary>Same figures as the seeded Carabao Mango: 300 kg/tree, PHP 80/kg, bears at year 5, full at year 10.</summary>
    Public Shared Function Mango() As TreeSpecies
        Return New TreeSpecies With {
            .SpeciesId = 1,
            .CommonName = "Carabao Mango",
            .ScientificName = "Mangifera indica",
            .CategoryId = 1,
            .CategoryName = Category.FruitBearing,
            .ProductType = "Fresh fruit",
            .YieldUnit = "kg",
            .YieldPerTreePerYear = 300D,
            .PricePerUnit = 80D,
            .HarvestType = HarvestType.Annual,
            .YearsToFirstHarvest = 5,
            .YearsToFullProduction = 10
        }
    End Function

    ''' <summary>Rotation species like the seeded Gmelina: 15 board ft/tree/yr, PHP 35, cut at year 10.</summary>
    Public Shared Function Gmelina() As TreeSpecies
        Return New TreeSpecies With {
            .SpeciesId = 2,
            .CommonName = "Gmelina",
            .ScientificName = "Gmelina arborea",
            .CategoryId = 2,
            .CategoryName = Category.NonFruitBearing,
            .ProductType = "Timber",
            .YieldUnit = "board ft",
            .YieldPerTreePerYear = 15D,
            .PricePerUnit = 35D,
            .HarvestType = HarvestType.Rotation,
            .YearsToFirstHarvest = 10,
            .YearsToFullProduction = 10
        }
    End Function

End Class
