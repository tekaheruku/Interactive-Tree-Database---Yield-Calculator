Imports System.Globalization
Imports TreeYield.Core.Models

Namespace Services

    ''' <summary>
    ''' Turns numbers into the text shown on screen. Keeping all formatting in one
    ''' place makes the cards, grids, CSV file and printed report consistent.
    ''' </summary>
    Public NotInheritable Class Formatters

        ''' <summary>The Philippine peso sign, written with ChrW so the source file stays plain ASCII.</summary>
        Public Const PesoSign As String = ChrW(&H20B1)

        ''' <summary>A middle dot used as a separator in captions.</summary>
        Public Const Separator As String = "  " & ChrW(&HB7) & "  "

        ' Invariant culture gives "1,234.5" on every PC, whatever its regional settings are.
        Private Shared ReadOnly Culture As CultureInfo = CultureInfo.InvariantCulture

        Private Sub New()
            ' Only Shared members.
        End Sub

        ''' <summary>Formats whole pesos, for example 600000 becomes "(peso sign)600,000".</summary>
        Public Shared Function Peso(amount As Decimal) As String
            Return PesoSign & RoundToWholePeso(amount).ToString("#,##0", Culture)
        End Function

        ''' <summary>Formats pesos with centavos, for example 80 becomes "(peso sign)80.00".</summary>
        Public Shared Function PesoWithCentavos(amount As Decimal) As String
            Return PesoSign & amount.ToString("#,##0.00", Culture)
        End Function

        ''' <summary>Rounds to the nearest peso (0.5 rounds up, like a cashier would).</summary>
        Public Shared Function RoundToWholePeso(amount As Decimal) As Decimal
            Return Math.Round(amount, 0, MidpointRounding.AwayFromZero)
        End Function

        ''' <summary>Formats a quantity with up to 2 decimals, for example 7500 becomes "7,500" and 37.5 becomes "37.5".</summary>
        Public Shared Function Quantity(value As Decimal) As String
            Return Math.Round(value, 2, MidpointRounding.AwayFromZero).ToString("#,##0.##", Culture)
        End Function

        ''' <summary>Formats a quantity followed by its unit, for example "7,500 kg".</summary>
        Public Shared Function Quantity(value As Decimal, unit As String) As String
            Dim number As String = Quantity(value)
            Return If(String.IsNullOrWhiteSpace(unit), number, number & " " & unit.Trim())
        End Function

        ''' <summary>Formats a 0-1 fraction as a percentage, for example 0.1667 becomes "16.7%".</summary>
        Public Shared Function Percent(fraction As Decimal) As String
            Return Math.Round(fraction * 100D, 1, MidpointRounding.AwayFromZero).ToString("0.#", Culture) & "%"
        End Function

        ''' <summary>Text for the "Production" column of the projection table.</summary>
        Public Shared Function ProductionStage(row As ProjectionYear, kind As HarvestType) As String
            If row Is Nothing Then
                Return String.Empty
            End If

            If kind = HarvestType.Rotation Then
                Return If(row.IsHarvestYear, "Harvest", "Growing")
            End If

            Return If(row.ProductionFactor <= 0D, "Not bearing yet", Percent(row.ProductionFactor))
        End Function

        ''' <summary>Formats a number of years, for example "1 year" or "10 years".</summary>
        Public Shared Function Years(count As Integer) As String
            Return If(count = 1, "1 year", count.ToString(Culture) & " years")
        End Function

        ''' <summary>Converts a UTC time from the database to local time text, for example "Oct 26, 2026 3:45 PM".</summary>
        Public Shared Function LocalDateTime(utc As DateTime) As String
            Return DateTime.SpecifyKind(utc, DateTimeKind.Utc).ToLocalTime().ToString("MMM d, yyyy h:mm tt", Culture)
        End Function

    End Class

End Namespace
