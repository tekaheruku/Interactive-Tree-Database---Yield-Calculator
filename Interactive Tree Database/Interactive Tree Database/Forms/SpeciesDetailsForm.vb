Imports TreeYield.Core.Models
Imports TreeYield.Core.Services

''' <summary>
''' Read-only view of one species record ("inspect record"). Returns OK when the user
''' clicks "Use in Calculator".
''' </summary>
Public Class SpeciesDetailsForm

    ''' <summary>Creates the dialog for a species.</summary>
    ''' <param name="species">The record to show.</param>
    ''' <param name="treeCount">Tree count used for the "For N trees" example line.</param>
    Public Sub New(species As TreeSpecies, treeCount As Integer)
        If species Is Nothing Then
            Throw New ArgumentNullException(NameOf(species))
        End If

        ' This call is required by the designer.
        InitializeComponent()

        ShowSpecies(species, treeCount)
    End Sub

    Private Sub ShowSpecies(s As TreeSpecies, treeCount As Integer)
        Dim rotation As Boolean = s.HarvestType = HarvestType.Rotation
        Dim example As YieldResult = YieldCalculator.Calculate(s, treeCount)

        Text = s.CommonName & " - Species Details"
        lblName.Text = s.CommonName
        lblScientific.Text = If(String.IsNullOrEmpty(s.ScientificName), "Scientific name not recorded", s.ScientificName)
        lblTags.Text = s.CategoryName & Formatters.Separator & s.ProductType

        lblYieldValue.Text = Formatters.Quantity(s.YieldPerTreePerYear, s.YieldUnit) &
                             If(rotation, " (average growth per year)", " at full production")
        lblPriceValue.Text = Formatters.PesoWithCentavos(s.PricePerUnit) & " per " & s.YieldUnit
        lblHarvestTypeValue.Text = If(rotation,
                                      "Rotation - the tree is cut once at rotation age",
                                      "Annual - harvested every year")

        If rotation Then
            lblFirstKey.Text = "Rotation age"
            lblFirstValue.Text = Formatters.Years(s.YearsToFirstHarvest) & " after planting"
            lblFullValue.Text = "Not applicable (single harvest)"
        Else
            lblFirstValue.Text = "Year " & Math.Max(1, s.YearsToFirstHarvest) & " after planting"
            lblFullValue.Text = "From year " & Math.Max(1, s.YearsToFullProduction)
        End If

        lblSeasonValue.Text = If(String.IsNullOrEmpty(s.HarvestSeason), "Not recorded", s.HarvestSeason)

        lblExampleKey.Text = "For " & treeCount.ToString("N0") & If(treeCount = 1, " tree", " trees")
        If rotation Then
            lblExampleValue.Text = Formatters.Quantity(example.HarvestYield, s.YieldUnit) & " = " & Formatters.Peso(example.HarvestSales) &
                                   " at year " & example.HarvestYear
        Else
            lblExampleValue.Text = Formatters.Quantity(example.AnnualYield, s.YieldUnit) & " = " & Formatters.Peso(example.GrossSales) & " per year"
        End If

        lblUpdatedValue.Text = If(s.UpdatedAtUtc = DateTime.MinValue, "-", Formatters.LocalDateTime(s.UpdatedAtUtc))
        txtNotes.Text = If(String.IsNullOrEmpty(s.Notes), "No notes.", s.Notes)
    End Sub

End Class
