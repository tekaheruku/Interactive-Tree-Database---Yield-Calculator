Imports System.Drawing.Printing
Imports System.Globalization
Imports TreeYield.Core.Models
Imports TreeYield.Core.Services

''' <summary>
''' Draws the printable projection report with System.Drawing.Printing and shows it
''' in a print preview window (from there it can be printed or saved as PDF with
''' "Microsoft Print to PDF"). Long tables continue on extra pages.
''' </summary>
Friend NotInheritable Class ReportPrinter
    Implements IDisposable

    ' Relative widths of the projection table columns.
    Private Shared ReadOnly ColumnWeights As Single() = {0.1F, 0.18F, 0.22F, 0.25F, 0.25F}

    Private WithEvents _document As New PrintDocument()
    Private ReadOnly _result As YieldResult
    Private ReadOnly _projection As YieldProjection
    Private _nextRow As Integer
    Private _pageNumber As Integer

    ''' <summary>Prepares a report for a calculation and its projection.</summary>
    Public Sub New(result As YieldResult, projection As YieldProjection)
        If result Is Nothing Then
            Throw New ArgumentNullException(NameOf(result))
        End If

        If projection Is Nothing Then
            Throw New ArgumentNullException(NameOf(projection))
        End If

        _result = result
        _projection = projection
        _document.DocumentName = "Tree Yield Report - " & result.Species.CommonName
    End Sub

    ''' <summary>Opens the print preview window.</summary>
    ''' <exception cref="InvalidPrinterException">No printer is installed.</exception>
    Public Sub ShowPreview(owner As IWin32Window)
        Using preview As New PrintPreviewDialog()
            preview.Document = _document
            preview.Icon = DrawingHelpers.AppIcon
            preview.UseAntiAlias = True
            preview.Width = 900
            preview.Height = 1000
            preview.Text = "Print Preview - " & _document.DocumentName
            preview.ShowDialog(owner)
        End Using
    End Sub

    ''' <summary>Releases the print document.</summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        _document.Dispose()
    End Sub

    Private Sub Document_BeginPrint(sender As Object, e As PrintEventArgs) Handles _document.BeginPrint
        ' Printing can run more than once (preview, then print), so start over each time.
        _nextRow = 0
        _pageNumber = 0
    End Sub

    Private Sub Document_PrintPage(sender As Object, e As PrintPageEventArgs) Handles _document.PrintPage
        _pageNumber += 1
        Dim g As Graphics = e.Graphics
        Dim area As Rectangle = e.MarginBounds
        Dim y As Single = area.Top

        Using titleFont As New Font(Theme.FontFamily, 18.0F, FontStyle.Bold),
              headingFont As New Font(Theme.SemiboldFontFamily, 11.0F),
              bodyFont As New Font(Theme.FontFamily, 10.0F),
              boldFont As New Font(Theme.SemiboldFontFamily, 10.0F),
              smallFont As New Font(Theme.FontFamily, 8.5F),
              textBrush As New SolidBrush(Theme.TextPrimary),
              mutedBrush As New SolidBrush(Theme.TextSecondary),
              linePen As New Pen(Theme.Border)

            Dim s As TreeSpecies = _result.Species

            If _pageNumber = 1 Then
                g.DrawString("Tree Yield & Revenue Projection Report", titleFont, textBrush, area.Left, y)
                y += titleFont.GetHeight(g) + 2
                g.DrawString("Generated " & DateTime.Now.ToString("MMMM d, yyyy h:mm tt", CultureInfo.InvariantCulture),
                             smallFont, mutedBrush, area.Left, y)
                y += smallFont.GetHeight(g) + 14

                y = DrawHeading(g, "Species", headingFont, textBrush, linePen, area, y)
                Dim rotation As Boolean = s.HarvestType = HarvestType.Rotation
                Dim harvestText As String
                If rotation Then
                    harvestText = "Rotation - cut once at " & Formatters.Years(s.YearsToFirstHarvest)
                Else
                    harvestText = "Annual - first harvest in year " & Math.Max(1, s.YearsToFirstHarvest) &
                                  ", full production from year " & Math.Max(1, s.YearsToFullProduction)
                End If

                Dim details As New List(Of KeyValuePair(Of String, String)) From {
                    Pair("Species", s.CommonName & If(String.IsNullOrEmpty(s.ScientificName), String.Empty, " (" & s.ScientificName & ")")),
                    Pair("Category / product", s.CategoryName & " / " & s.ProductType),
                    Pair("Yield per tree per year", Formatters.Quantity(s.YieldPerTreePerYear, s.YieldUnit) & If(rotation, " (mean annual increment)", String.Empty)),
                    Pair("Price per unit", Formatters.PesoWithCentavos(s.PricePerUnit) & " per " & s.YieldUnit),
                    Pair("Harvest type", harvestText)
                }
                y = DrawPairs(g, details, bodyFont, boldFont, mutedBrush, textBrush, area, y)

                y += 8
                y = DrawHeading(g, "Results for " & _result.TreeCount.ToString("N0", CultureInfo.InvariantCulture) & " trees",
                                headingFont, textBrush, linePen, area, y)
                Dim results As New List(Of KeyValuePair(Of String, String)) From {
                    Pair(If(rotation, "Annualized yield", "Annual yield (full production)"), Formatters.Quantity(_result.AnnualYield, s.YieldUnit)),
                    Pair(If(rotation, "Annualized gross sales", "Estimated annual gross sales"), Formatters.Peso(_result.GrossSales)),
                    Pair(_projection.Years & "-year total", Formatters.Quantity(_projection.TotalYield, s.YieldUnit) & "  /  " & Formatters.Peso(_projection.TotalSales))
                }
                If rotation Then
                    results.Add(Pair("Harvest at year " & _result.HarvestYear,
                                     Formatters.Quantity(_result.HarvestYield, s.YieldUnit) & "  /  " & Formatters.Peso(_result.HarvestSales)))
                End If

                y = DrawPairs(g, results, bodyFont, boldFont, mutedBrush, textBrush, area, y)
                y += 8
                y = DrawHeading(g, _projection.Years & "-year projection", headingFont, textBrush, linePen, area, y)
            Else
                g.DrawString(_projection.Years & "-year projection (continued) - " & s.CommonName, headingFont, textBrush, area.Left, y)
                y += headingFont.GetHeight(g) + 8
            End If

            y = DrawTable(g, bodyFont, boldFont, textBrush, linePen, area, y)

            Dim finished As Boolean = _nextRow >= _projection.Rows.Count
            Dim footerTop As Single = area.Bottom - smallFont.GetHeight(g) * 3

            If finished Then
                Dim disclaimerBox As New RectangleF(area.Left, Math.Max(y + 12, footerTop - 34), area.Width, 40)
                g.DrawString(YieldCalculator.EstimateDisclaimer, smallFont, mutedBrush, disclaimerBox)
            End If

            g.DrawString("Page " & _pageNumber, smallFont, mutedBrush, area.Right - 60, area.Bottom + 10)
            e.HasMorePages = Not finished
        End Using
    End Sub

    Private Function DrawTable(g As Graphics, bodyFont As Font, boldFont As Font, textBrush As Brush, linePen As Pen,
                               area As Rectangle, y As Single) As Single
        Dim unit As String = _result.Species.YieldUnit
        Dim headers As String() = {"Year", "Production", "Yield (" & unit & ")", "Gross sales", "Cumulative sales"}
        Dim rowHeight As Single = bodyFont.GetHeight(g) + 8
        Dim bottomLimit As Single = area.Bottom - 70

        DrawRow(g, headers, boldFont, textBrush, area, y, rowHeight)
        y += rowHeight
        g.DrawLine(linePen, area.Left, y, area.Right, y)

        While _nextRow < _projection.Rows.Count AndAlso y + rowHeight <= bottomLimit
            Dim row As ProjectionYear = _projection.Rows(_nextRow)
            Dim cells As String() = {
                row.Year.ToString(CultureInfo.InvariantCulture),
                Formatters.ProductionStage(row, _result.Species.HarvestType),
                Formatters.Quantity(row.Yield),
                Formatters.Peso(row.GrossSales),
                Formatters.Peso(row.CumulativeSales)
            }
            DrawRow(g, cells, bodyFont, textBrush, area, y, rowHeight)
            y += rowHeight
            g.DrawLine(linePen, area.Left, y, area.Right, y)
            _nextRow += 1
        End While

        Return y
    End Function

    Private Shared Sub DrawRow(g As Graphics, cells As String(), font As Font, brush As Brush, area As Rectangle,
                               y As Single, height As Single)
        Dim x As Single = area.Left
        For i As Integer = 0 To cells.Length - 1
            Dim width As Single = area.Width * ColumnWeights(i)
            Using format As New StringFormat()
                ' Numbers are right-aligned so digits line up; the first two columns are text.
                format.Alignment = If(i >= 2, StringAlignment.Far, StringAlignment.Near)
                format.LineAlignment = StringAlignment.Center
                format.Trimming = StringTrimming.EllipsisCharacter
                format.FormatFlags = StringFormatFlags.NoWrap
                g.DrawString(cells(i), font, brush, New RectangleF(x + 4, y, width - 8, height), format)
            End Using

            x += width
        Next
    End Sub

    Private Shared Function DrawHeading(g As Graphics, text As String, font As Font, brush As Brush, pen As Pen,
                                        area As Rectangle, y As Single) As Single
        g.DrawString(text, font, brush, area.Left, y)
        y += font.GetHeight(g) + 3
        g.DrawLine(pen, area.Left, y, area.Right, y)
        Return y + 6
    End Function

    Private Shared Function DrawPairs(g As Graphics, pairs As IEnumerable(Of KeyValuePair(Of String, String)), labelFont As Font,
                                      valueFont As Font, labelBrush As Brush, valueBrush As Brush, area As Rectangle,
                                      y As Single) As Single
        Dim labelWidth As Single = area.Width * 0.34F
        For Each pair As KeyValuePair(Of String, String) In pairs
            Dim valueBox As New RectangleF(area.Left + labelWidth, y, area.Width - labelWidth, 200)
            Dim valueSize As SizeF = g.MeasureString(pair.Value, valueFont, CInt(valueBox.Width))
            g.DrawString(pair.Key, labelFont, labelBrush, area.Left, y)
            g.DrawString(pair.Value, valueFont, valueBrush, valueBox)
            y += Math.Max(labelFont.GetHeight(g), valueSize.Height) + 4
        Next

        Return y
    End Function

    Private Shared Function Pair(key As String, value As String) As KeyValuePair(Of String, String)
        Return New KeyValuePair(Of String, String)(key, If(value, String.Empty))
    End Function

End Class
