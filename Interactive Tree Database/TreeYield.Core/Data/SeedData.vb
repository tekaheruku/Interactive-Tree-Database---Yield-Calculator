Imports TreeYield.Core.Models

Namespace Data

    ''' <summary>
    ''' The categories and 22 sample species inserted into a new database and by
    ''' "Reset Sample Data".
    ''' <para>
    ''' IMPORTANT: yields and prices are illustrative planning estimates for a school
    ''' demonstration. Real yields depend on variety, age, spacing, soil and care, and
    ''' farm-gate prices change with season and market. Verify with the Department of
    ''' Agriculture (DA), the Philippine Statistics Authority (PSA) or local buyers.
    ''' </para>
    ''' Units: YieldPerTreePerYear is per mature tree per year. For rotation species it
    ''' is the mean annual increment in board feet; the trees are cut once at rotation age.
    ''' </summary>
    Public NotInheritable Class SeedData

        Private Shared ReadOnly _categories As New List(Of Category) From {
            New Category With {.Name = Category.FruitBearing,
                               .Description = "Trees grown mainly for fruit, nuts or beans that are harvested every year."},
            New Category With {.Name = Category.NonFruitBearing,
                               .Description = "Trees grown mainly for timber, pulpwood or latex instead of fruit."}
        }

        Private Shared ReadOnly _species As New List(Of TreeSpecies) From {
            Fruit("Carabao Mango", "Mangifera indica 'Carabao'", "Fresh fruit", "kg", 300D, 80D, 5, 10, "March - June",
                  "The Philippine ""sweetest mango"". Grafted trees bear earlier; flower induction improves yield. Sample estimate."),
            Fruit("Coconut", "Cocos nucifera", "Mature nuts", "pcs", 60D, 15D, 6, 10, "Year-round (every 45-60 days)",
                  "Yield counted in whole nuts. Copra, coco water and husk add income not included here. Sample estimate."),
            Fruit("Calamansi", "Citrus x microcarpa", "Fresh fruit", "kg", 20D, 60D, 2, 5, "August - January",
                  "Fast to bear and suited to backyards. Prices rise sharply off-season. Sample estimate."),
            Fruit("Rambutan", "Nephelium lappaceum", "Fresh fruit", "kg", 80D, 70D, 5, 9, "August - October",
                  "Needs deep, well-drained soil and regular rainfall. Sample estimate."),
            Fruit("Lanzones", "Lansium parasiticum", "Fresh fruit", "kg", 60D, 90D, 8, 15, "September - November",
                  "Slow to bear from seed; Camiguin and Laguna are known producers. Sample estimate."),
            Fruit("Durian", "Durio zibethinus", "Fresh fruit", "kg", 120D, 100D, 6, 12, "August - October",
                  "Davao is the main producing region. Grafted varieties command higher prices. Sample estimate."),
            Fruit("Jackfruit (Langka)", "Artocarpus heterophyllus", "Fresh fruit", "kg", 250D, 25D, 3, 7, "Year-round (peak March - August)",
                  "Very heavy fruits; unripe fruit is also sold as a vegetable. Sample estimate."),
            Fruit("Avocado", "Persea americana", "Fresh fruit", "kg", 100D, 60D, 4, 8, "June - September",
                  "Grafted trees give more uniform fruit. Sample estimate."),
            Fruit("Cacao", "Theobroma cacao", "Dried beans", "kg", 1.5D, 180D, 3, 6, "Year-round (peak October - January)",
                  "Yield is fermented and dried beans. Often intercropped under coconut. Sample estimate."),
            Fruit("Robusta Coffee", "Coffea canephora", "Green coffee beans", "kg", 1D, 120D, 3, 5, "November - February",
                  "Yield is green (unroasted) beans per tree. Sample estimate."),
            Fruit("Guyabano", "Annona muricata", "Fresh fruit", "kg", 40D, 80D, 3, 6, "Year-round",
                  "Soursop. Hand pollination can raise fruit set. Sample estimate."),
            Fruit("Pomelo (Suha)", "Citrus maxima", "Fresh fruit", "kg", 100D, 50D, 5, 8, "August - October",
                  "Large citrus; Davao varieties are well known. Sample estimate."),
            Fruit("Santol", "Sandoricum koetjape", "Fresh fruit", "kg", 100D, 30D, 5, 10, "June - August",
                  "Hardy tree that tolerates poor soils. Sample estimate."),
            Fruit("Pili", "Canarium ovatum", "Nuts in shell", "kg", 30D, 50D, 6, 12, "May - October",
                  "Native to the Bicol region. Shelled kernels sell for much more. Sample estimate."),
            Fruit("Mangosteen", "Garcinia mangostana", "Fresh fruit", "kg", 50D, 150D, 8, 15, "July - September",
                  """Queen of fruits"". Very slow to bear but high value. Sample estimate."),
            Timber("Mahogany", "Swietenia macrophylla", "Timber", 8D, 70D, 25,
                   "Yield is mean annual increment in board feet; the tree is cut once at rotation age. Sample estimate."),
            Timber("Gmelina (Yemane)", "Gmelina arborea", "Timber", 15D, 35D, 10,
                   "Fast-growing plantation species for lumber and furniture. Sample estimate."),
            Timber("Falcata", "Falcataria falcata", "Peeler logs / timber", 20D, 25D, 8,
                   "Very fast-growing; sold to plywood and veneer mills in Mindanao. Sample estimate."),
            Timber("Teak", "Tectona grandis", "Timber", 6D, 120D, 20,
                   "Premium, durable hardwood. Long rotation but high price. Sample estimate."),
            Timber("Acacia Mangium", "Acacia mangium", "Pulpwood / timber", 15D, 25D, 8,
                   "Fast-growing; improves poor soils by fixing nitrogen. Sample estimate."),
            Timber("Bagras (Rainbow Eucalyptus)", "Eucalyptus deglupta", "Timber / pulpwood", 12D, 30D, 12,
                   "Native to Mindanao; used for pulp, plywood and light construction. Sample estimate."),
            New TreeSpecies With {
                .CommonName = "Rubber", .ScientificName = "Hevea brasiliensis", .CategoryName = Category.NonFruitBearing,
                .ProductType = "Cup lump (latex)", .YieldUnit = "kg", .YieldPerTreePerYear = 4D, .PricePerUnit = 35D,
                .HarvestType = HarvestType.Annual, .YearsToFirstHarvest = 6, .YearsToFullProduction = 9,
                .HarvestSeason = "Year-round tapping (rest when leaves fall)",
                .Notes = "Tapped for latex every year once the trunk is thick enough. Sample estimate."}
        }

        Private Sub New()
            ' Only Shared members.
        End Sub

        ''' <summary>The two categories.</summary>
        Public Shared ReadOnly Property Categories As IReadOnlyList(Of Category)
            Get
                Return _categories
            End Get
        End Property

        ''' <summary>
        ''' The sample species. <see cref="TreeSpecies.CategoryName"/> is filled in;
        ''' the CategoryId is looked up when the rows are inserted.
        ''' </summary>
        Public Shared ReadOnly Property Species As IReadOnlyList(Of TreeSpecies)
            Get
                Return _species
            End Get
        End Property

        Private Shared Function Fruit(name As String, scientific As String, product As String, unit As String,
                                      yieldPerTree As Decimal, price As Decimal, firstHarvest As Integer,
                                      fullProduction As Integer, season As String, notes As String) As TreeSpecies
            Return New TreeSpecies With {
                .CommonName = name, .ScientificName = scientific, .CategoryName = Category.FruitBearing,
                .ProductType = product, .YieldUnit = unit, .YieldPerTreePerYear = yieldPerTree, .PricePerUnit = price,
                .HarvestType = HarvestType.Annual, .YearsToFirstHarvest = firstHarvest,
                .YearsToFullProduction = fullProduction, .HarvestSeason = season, .Notes = notes}
        End Function

        Private Shared Function Timber(name As String, scientific As String, product As String,
                                       yieldPerTree As Decimal, price As Decimal, rotationAge As Integer,
                                       notes As String) As TreeSpecies
            Return New TreeSpecies With {
                .CommonName = name, .ScientificName = scientific, .CategoryName = Category.NonFruitBearing,
                .ProductType = product, .YieldUnit = "board ft", .YieldPerTreePerYear = yieldPerTree, .PricePerUnit = price,
                .HarvestType = HarvestType.Rotation, .YearsToFirstHarvest = rotationAge,
                .YearsToFullProduction = rotationAge, .HarvestSeason = "At rotation age", .Notes = notes}
        End Function

    End Class

End Namespace
