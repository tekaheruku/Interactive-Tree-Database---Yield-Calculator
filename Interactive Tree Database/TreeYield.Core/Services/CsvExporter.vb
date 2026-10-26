Imports System.Globalization
Imports System.Text
Imports TreeYield.Core.Models

Namespace Services

    ''' <summary>
    ''' Builds the CSV (comma-separated values) report that opens in Excel.
    ''' Follows RFC 4180 quoting and blocks "CSV formula injection".
    ''' </summary>
    Public NotInheritable Class CsvExporter

        ' Characters that make Excel treat a cell as a formula, e.g. =HYPERLINK("http://evil").
        Private Const FormulaTriggers As String = "=+-@" & ControlChars.Tab & ControlChars.Cr

        Private Shared ReadOnly CharactersNeedingQuotes As Char() = {","c, """"c, ControlChars.Cr, ControlChars.Lf}

        Private Sub New()
            ' Only Shared members.
        End Sub

        ''' <summary>
        ''' Makes one value safe for a CSV cell:
        ''' <list type="number">
        ''' <item>If it starts with a formula character, a leading apostrophe is added so Excel shows it as text.</item>
        ''' <item>If it contains a comma, quote or line break, it is wrapped in quotes and inner quotes are doubled.</item>
        ''' </list>
        ''' </summary>
        Public Shared Function EscapeField(value As String) As String
            If String.IsNullOrEmpty(value) Then
                Return String.Empty
            End If

            Dim safe As String = If(FormulaTriggers.IndexOf(value(0)) >= 0, "'" & value, value)

            If safe.IndexOfAny(CharactersNeedingQuotes) >= 0 Then
                safe = """" & safe.Replace("""", """""") & """"
            End If

            Return safe
        End Function

        ''' <summary>Joins escaped values into one CSV line (without the line break).</summary>
        Public Shared Function BuildRow(ParamArray fields As String()) As String
            If fields Is Nothing Then
                Return String.Empty
            End If

            Return String.Join(",", fields.Select(AddressOf EscapeField))
        End Function

        ''' <summary>Builds the full report: species details, inputs, results and the projection table.</summary>
        ''' <param name="result">Snapshot from <see cref="YieldCalculator.Calculate"/>.</param>
        ''' <param name="projection">Projection from <see cref="YieldCalculator.Project"/>.</param>
        ''' <param name="generatedAt">Time written in the report header (local time).</param>
        Public Shared Function BuildReport(result As YieldResult, projection As YieldProjection, generatedAt As DateTime) As String
            If result Is Nothing Then
                Throw New ArgumentNullException(NameOf(result))
            End If

            If projection Is Nothing Then
                Throw New ArgumentNullException(NameOf(projection))
            End If

            Dim s As TreeSpecies = result.Species
            Dim unit As String = s.YieldUnit
            Dim isRotation As Boolean = s.HarvestType = HarvestType.Rotation
            Dim csv As New StringBuilder()

            AppendRow(csv, "Tree Yield & Revenue Projection Report")
            AppendRow(csv, "Generated", generatedAt.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture))
            AppendRow(csv)

            AppendRow(csv, "Field", "Value")
            AppendRow(csv, "Species", s.CommonName)
            AppendRow(csv, "Scientific name", s.ScientificName)
            AppendRow(csv, "Category", s.CategoryName)
            AppendRow(csv, "Product", s.ProductType)
            AppendRow(csv, "Harvest type", If(isRotation, "Rotation (one-time harvest)", "Annual"))
            AppendRow(csv, "Yield per tree per year (" & unit & ")", Number(s.YieldPerTreePerYear))
            AppendRow(csv, "Price per " & unit & " (PHP)", Number(s.PricePerUnit))
            AppendRow(csv, If(isRotation, "Rotation age (years)", "Years to first harvest"), Whole(s.YearsToFirstHarvest))
            If Not isRotation Then
                AppendRow(csv, "Years to full production", Whole(s.YearsToFullProduction))
            End If

            AppendRow(csv, "Tree count", Whole(result.TreeCount))
            AppendRow(csv, If(isRotation, "Annualized yield (", "Annual yield at full production (") & unit & ")", Number(result.AnnualYield))
            AppendRow(csv, If(isRotation, "Annualized gross sales (PHP)", "Estimated annual gross sales (PHP)"), Number(result.GrossSales))
            If isRotation Then
                AppendRow(csv, "Harvest year", Whole(result.HarvestYear))
                AppendRow(csv, "Harvest yield (" & unit & ")", Number(result.HarvestYield))
                AppendRow(csv, "Harvest gross sales (PHP)", Number(result.HarvestSales))
            End If

            AppendRow(csv)
            AppendRow(csv, Whole(projection.Years) & "-year projection")
            AppendRow(csv, "Year", "Production", "Yield (" & unit & ")", "Gross sales (PHP)",
                "Cumulative yield (" & unit & ")", "Cumulative gross sales (PHP)")

            For Each row As ProjectionYear In projection.Rows
                AppendRow(csv,
                    Whole(row.Year),
                    Formatters.ProductionStage(row, s.HarvestType),
                    Number(row.Yield),
                    Number(row.GrossSales),
                    Number(row.CumulativeYield),
                    Number(row.CumulativeSales))
            Next

            AppendRow(csv, "Total", String.Empty, Number(projection.TotalYield), Number(projection.TotalSales))
            AppendRow(csv)
            AppendRow(csv, "Note", YieldCalculator.EstimateDisclaimer)

            Return csv.ToString()
        End Function

        Private Shared Sub AppendRow(csv As StringBuilder, ParamArray fields As String())
            ' RFC 4180 uses CRLF line endings.
            csv.Append(BuildRow(fields)).Append(ControlChars.CrLf)
        End Sub

        Private Shared Function Number(value As Decimal) As String
            ' No thousands separator, so Excel reads the cell as a number.
            Return value.ToString("0.00", CultureInfo.InvariantCulture)
        End Function

        Private Shared Function Whole(value As Integer) As String
            Return value.ToString(CultureInfo.InvariantCulture)
        End Function

    End Class

End Namespace
