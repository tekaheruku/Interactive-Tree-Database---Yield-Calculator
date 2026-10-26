Imports TreeYield.Core.Models

Namespace Services

    ''' <summary>
    ''' Business rules for a tree species record. These checks run before anything is
    ''' sent to the database. The database repeats the most important rules as CHECK
    ''' and UNIQUE constraints, so bad data is blocked even if this code is bypassed -
    ''' for example when someone edits the table directly in Microsoft Access
    ''' (defense in depth).
    ''' </summary>
    Public NotInheritable Class SpeciesValidator

        ''' <summary>Maximum length of <see cref="TreeSpecies.CommonName"/> (matches TEXT(100)).</summary>
        Public Const MaxCommonNameLength As Integer = 100

        ''' <summary>Maximum length of <see cref="TreeSpecies.ScientificName"/>.</summary>
        Public Const MaxScientificNameLength As Integer = 150

        ''' <summary>Maximum length of <see cref="TreeSpecies.ProductType"/>.</summary>
        Public Const MaxProductTypeLength As Integer = 60

        ''' <summary>Maximum length of <see cref="TreeSpecies.YieldUnit"/>.</summary>
        Public Const MaxYieldUnitLength As Integer = 20

        ''' <summary>Maximum length of <see cref="TreeSpecies.HarvestSeason"/>.</summary>
        Public Const MaxHarvestSeasonLength As Integer = 100

        ''' <summary>Maximum length of <see cref="TreeSpecies.Notes"/>.</summary>
        Public Const MaxNotesLength As Integer = 1000

        ''' <summary>Largest yield per tree per year accepted.</summary>
        Public Const MaxYieldPerTree As Decimal = 100000D

        ''' <summary>Largest price per unit accepted, in pesos.</summary>
        Public Const MaxPricePerUnit As Decimal = 1000000D

        ''' <summary>Largest number of years accepted for first harvest / full production.</summary>
        Public Const MaxYears As Integer = 60

        Private Sub New()
            ' Only Shared members.
        End Sub

        ''' <summary>Checks every field and returns all problems found.</summary>
        ''' <param name="species">The record to check.</param>
        Public Shared Function Validate(species As TreeSpecies) As ValidationResult
            Dim result As New ValidationResult()
            If species Is Nothing Then
                result.Add("Species", "No species data was provided.")
                Return result
            End If

            RequireText(result, NameOf(TreeSpecies.CommonName), "Common name", species.CommonName, MaxCommonNameLength)
            OptionalText(result, NameOf(TreeSpecies.ScientificName), "Scientific name", species.ScientificName, MaxScientificNameLength)

            If species.CategoryId <= 0 Then
                result.Add(NameOf(TreeSpecies.CategoryId), "Select a category.")
            End If

            RequireText(result, NameOf(TreeSpecies.ProductType), "Product", species.ProductType, MaxProductTypeLength)
            RequireText(result, NameOf(TreeSpecies.YieldUnit), "Yield unit", species.YieldUnit, MaxYieldUnitLength)

            If species.YieldPerTreePerYear <= 0D Then
                result.Add(NameOf(TreeSpecies.YieldPerTreePerYear), "Yield per tree must be greater than zero.")
            ElseIf species.YieldPerTreePerYear > MaxYieldPerTree Then
                result.Add(NameOf(TreeSpecies.YieldPerTreePerYear),
                    String.Format("Yield per tree cannot be more than {0:N0}.", MaxYieldPerTree))
            ElseIf HasMoreThanTwoDecimals(species.YieldPerTreePerYear) Then
                result.Add(NameOf(TreeSpecies.YieldPerTreePerYear), "Yield per tree can have at most 2 decimal places.")
            End If

            If species.PricePerUnit < 0D Then
                result.Add(NameOf(TreeSpecies.PricePerUnit), "Price cannot be negative.")
            ElseIf species.PricePerUnit > MaxPricePerUnit Then
                result.Add(NameOf(TreeSpecies.PricePerUnit),
                    String.Format("Price cannot be more than {0:N0}.", MaxPricePerUnit))
            ElseIf HasMoreThanTwoDecimals(species.PricePerUnit) Then
                result.Add(NameOf(TreeSpecies.PricePerUnit), "Price can have at most 2 decimal places.")
            End If

            If Not [Enum].IsDefined(GetType(HarvestType), species.HarvestType) Then
                result.Add(NameOf(TreeSpecies.HarvestType), "Select a harvest type.")
            End If

            ValidateYears(result, species)

            OptionalText(result, NameOf(TreeSpecies.HarvestSeason), "Harvest season", species.HarvestSeason, MaxHarvestSeasonLength)
            OptionalText(result, NameOf(TreeSpecies.Notes), "Notes", species.Notes, MaxNotesLength)

            Return result
        End Function

        Private Shared Sub ValidateYears(result As ValidationResult, species As TreeSpecies)
            Dim isRotation As Boolean = species.HarvestType = HarvestType.Rotation
            Dim firstLabel As String = If(isRotation, "Rotation age", "Years to first harvest")

            If species.YearsToFirstHarvest < 0 OrElse species.YearsToFirstHarvest > MaxYears Then
                result.Add(NameOf(TreeSpecies.YearsToFirstHarvest),
                    String.Format("{0} must be from 0 to {1}.", firstLabel, MaxYears))
            ElseIf isRotation AndAlso species.YearsToFirstHarvest < 1 Then
                result.Add(NameOf(TreeSpecies.YearsToFirstHarvest), "Rotation age must be at least 1 year.")
            End If

            If species.YearsToFullProduction < 0 OrElse species.YearsToFullProduction > MaxYears Then
                result.Add(NameOf(TreeSpecies.YearsToFullProduction),
                    String.Format("Years to full production must be from 0 to {0}.", MaxYears))
            ElseIf species.YearsToFullProduction < species.YearsToFirstHarvest Then
                result.Add(NameOf(TreeSpecies.YearsToFullProduction),
                    "Full production cannot come before the first harvest.")
            End If
        End Sub

        Private Shared Sub RequireText(result As ValidationResult, field As String, label As String, value As String, maxLength As Integer)
            If String.IsNullOrWhiteSpace(value) Then
                result.Add(field, label & " is required.")
                Return
            End If

            OptionalText(result, field, label, value, maxLength)
        End Sub

        Private Shared Sub OptionalText(result As ValidationResult, field As String, label As String, value As String, maxLength As Integer)
            If String.IsNullOrEmpty(value) Then
                Return
            End If

            If value.Trim().Length > maxLength Then
                result.Add(field, String.Format("{0} must be {1} characters or fewer.", label, maxLength))
            ElseIf value.Any(Function(c) Char.IsControl(c) AndAlso c <> ControlChars.Cr AndAlso c <> ControlChars.Lf AndAlso c <> ControlChars.Tab) Then
                ' Control characters (other than line breaks and tabs) are never typed on
                ' purpose; they usually come from pasted binary data.
                result.Add(field, label & " contains characters that are not allowed.")
            End If
        End Sub

        Private Shared Function HasMoreThanTwoDecimals(value As Decimal) As Boolean
            ' The database columns are DECIMAL(12,2); extra decimals would be rounded silently.
            Return Decimal.Round(value, 2) <> value
        End Function

    End Class

End Namespace
