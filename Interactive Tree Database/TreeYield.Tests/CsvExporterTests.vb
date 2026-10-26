Imports System.Text.RegularExpressions
Imports TreeYield.Core.Models
Imports TreeYield.Core.Services
Imports TreeYield.Tests.Framework

''' <summary>Tests for CSV quoting and formula-injection protection in <see cref="CsvExporter"/>.</summary>
Public Class CsvExporterTests

    <Test>
    Public Sub PlainText_IsUnchanged()
        Assert.AreEqual("Carabao Mango", CsvExporter.EscapeField("Carabao Mango"))
    End Sub

    <Test>
    Public Sub TextWithComma_IsQuoted()
        Assert.AreEqual("""Mango, Carabao""", CsvExporter.EscapeField("Mango, Carabao"))
    End Sub

    <Test>
    Public Sub InnerQuotes_AreDoubled()
        Assert.AreEqual("""Mangifera indica """"Carabao""""""", CsvExporter.EscapeField("Mangifera indica ""Carabao"""))
    End Sub

    <Test>
    Public Sub LineBreak_IsQuoted()
        Assert.AreEqual("""line1" & vbLf & "line2""", CsvExporter.EscapeField("line1" & vbLf & "line2"))
    End Sub

    <Test>
    Public Sub FormulaCharacters_ArePrefixedWithApostrophe()
        Assert.AreEqual("'=1+1", CsvExporter.EscapeField("=1+1"))
        Assert.AreEqual("'+63 912", CsvExporter.EscapeField("+63 912"))
        Assert.AreEqual("'-2", CsvExporter.EscapeField("-2"))
        Assert.AreEqual("'@SUM(A1)", CsvExporter.EscapeField("@SUM(A1)"))
    End Sub

    <Test>
    Public Sub FormulaWithComma_IsPrefixedAndQuoted()
        Assert.AreEqual("""'=HYPERLINK(""""http://x"""",""""click"""")""",
            CsvExporter.EscapeField("=HYPERLINK(""http://x"",""click"")"))
    End Sub

    <Test>
    Public Sub NothingOrEmpty_BecomesEmptyCell()
        Assert.AreEqual(String.Empty, CsvExporter.EscapeField(Nothing))
        Assert.AreEqual(String.Empty, CsvExporter.EscapeField(String.Empty))
    End Sub

    <Test>
    Public Sub BuildReport_ContainsInputsResultsAndEveryYear()
        Dim mango As TreeSpecies = TestData.Mango()
        Dim result As YieldResult = YieldCalculator.Calculate(mango, 25)
        Dim projection As YieldProjection = YieldCalculator.Project(mango, 25, 10)

        Dim csv As String = CsvExporter.BuildReport(result, projection, New DateTime(2026, 10, 26, 9, 30, 0))
        Dim lines As String() = csv.Split(New String() {vbCrLf}, StringSplitOptions.None)

        Assert.IsTrue(lines.Contains("Species,Carabao Mango"))
        Assert.IsTrue(lines.Contains("Tree count,25"))
        Assert.IsTrue(lines.Contains("Estimated annual gross sales (PHP),600000.00"))
        Assert.IsTrue(lines.Contains("Generated,2026-10-26 09:30"))
        ' Year rows look like "5,16.7%,1250.00,..." - digits followed by a comma.
        Assert.AreEqual(10, lines.Count(Function(line) Regex.IsMatch(line, "^\d+,")), "one row per projected year")
    End Sub

End Class
