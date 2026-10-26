Imports TreeYield.Core.Models
Imports TreeYield.Core.Services
Imports TreeYield.Tests.Framework

''' <summary>Tests for the display formats in <see cref="Formatters"/>.</summary>
Public Class FormattersTests

    <Test>
    Public Sub Peso_UsesPesoSignAndThousandsSeparator()
        Assert.AreEqual(Formatters.PesoSign & "600,000", Formatters.Peso(600000D))
        Assert.AreEqual(&H20B1, AscW(Formatters.PesoSign), "the peso sign is U+20B1")
    End Sub

    <Test>
    Public Sub Peso_RoundsHalfUp()
        Assert.AreEqual(Formatters.PesoSign & "1,235", Formatters.Peso(1234.5D))
        Assert.AreEqual(Formatters.PesoSign & "1,234", Formatters.Peso(1234.49D))
    End Sub

    <Test>
    Public Sub PesoWithCentavos_ShowsTwoDecimals()
        Assert.AreEqual(Formatters.PesoSign & "80.00", Formatters.PesoWithCentavos(80D))
    End Sub

    <Test>
    Public Sub Quantity_DropsTrailingZeros()
        Assert.AreEqual("7,500 kg", Formatters.Quantity(7500D, "kg"))
        Assert.AreEqual("37.5 kg", Formatters.Quantity(37.5D, "kg"))
        Assert.AreEqual("1,250", Formatters.Quantity(1250.0000001D))
    End Sub

    <Test>
    Public Sub Percent_ShowsOneDecimal()
        Assert.AreEqual("16.7%", Formatters.Percent(1D / 6D))
        Assert.AreEqual("100%", Formatters.Percent(1D))
    End Sub

    <Test>
    Public Sub ProductionStage_DescribesEachYear()
        Assert.AreEqual("Not bearing yet", Formatters.ProductionStage(New ProjectionYear With {.ProductionFactor = 0D}, HarvestType.Annual))
        Assert.AreEqual("50%", Formatters.ProductionStage(New ProjectionYear With {.ProductionFactor = 0.5D, .IsHarvestYear = True}, HarvestType.Annual))
        Assert.AreEqual("Growing", Formatters.ProductionStage(New ProjectionYear(), HarvestType.Rotation))
        Assert.AreEqual("Harvest", Formatters.ProductionStage(New ProjectionYear With {.IsHarvestYear = True, .ProductionFactor = 1D}, HarvestType.Rotation))
    End Sub

    <Test>
    Public Sub Years_UsesSingularForOne()
        Assert.AreEqual("1 year", Formatters.Years(1))
        Assert.AreEqual("10 years", Formatters.Years(10))
    End Sub

End Class
