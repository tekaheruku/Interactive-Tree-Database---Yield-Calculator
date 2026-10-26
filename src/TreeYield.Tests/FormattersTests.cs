using TreeYield.Core.Models;
using TreeYield.Core.Services;
using TreeYield.Tests.Framework;

namespace TreeYield.Tests
{
    /// <summary>Tests for the display formats in <see cref="Formatters"/>.</summary>
    public class FormattersTests
    {
        [Test]
        public void Peso_UsesPesoSignAndThousandsSeparator()
        {
            Assert.AreEqual("₱600,000", Formatters.Peso(600000m));
        }

        [Test]
        public void Peso_RoundsHalfUp()
        {
            Assert.AreEqual("₱1,235", Formatters.Peso(1234.5m));
            Assert.AreEqual("₱1,234", Formatters.Peso(1234.49m));
        }

        [Test]
        public void PesoWithCentavos_ShowsTwoDecimals()
        {
            Assert.AreEqual("₱80.00", Formatters.PesoWithCentavos(80m));
        }

        [Test]
        public void Quantity_DropsTrailingZeros()
        {
            Assert.AreEqual("7,500 kg", Formatters.Quantity(7500m, "kg"));
            Assert.AreEqual("37.5 kg", Formatters.Quantity(37.5m, "kg"));
            Assert.AreEqual("1,250", Formatters.Quantity(1250.0000001m));
        }

        [Test]
        public void Percent_ShowsOneDecimal()
        {
            Assert.AreEqual("16.7%", Formatters.Percent(1m / 6m));
            Assert.AreEqual("100%", Formatters.Percent(1m));
        }

        [Test]
        public void ProductionStage_DescribesEachYear()
        {
            Assert.AreEqual("Not bearing yet", Formatters.ProductionStage(new ProjectionYear { ProductionFactor = 0m }, HarvestType.Annual));
            Assert.AreEqual("50%", Formatters.ProductionStage(new ProjectionYear { ProductionFactor = 0.5m, IsHarvestYear = true }, HarvestType.Annual));
            Assert.AreEqual("Growing", Formatters.ProductionStage(new ProjectionYear(), HarvestType.Rotation));
            Assert.AreEqual("Harvest", Formatters.ProductionStage(new ProjectionYear { IsHarvestYear = true, ProductionFactor = 1m }, HarvestType.Rotation));
        }

        [Test]
        public void Years_UsesSingularForOne()
        {
            Assert.AreEqual("1 year", Formatters.Years(1));
            Assert.AreEqual("10 years", Formatters.Years(10));
        }
    }
}
