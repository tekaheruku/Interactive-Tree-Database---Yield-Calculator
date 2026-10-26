using System;
using System.Collections.Generic;
using TreeYield.Core.Data;
using TreeYield.Tests.Framework;

namespace TreeYield.Tests
{
    /// <summary>Tests for <see cref="SqlText"/> and <see cref="SqlScriptRunner"/>.</summary>
    public class SqlHelperTests
    {
        [Test]
        public void EscapeLikePattern_EscapesWildcards()
        {
            Assert.AreEqual(@"50\%\_\[x]\\", SqlText.EscapeLikePattern(@"50%_[x]\"));
        }

        [Test]
        public void EscapeLikePattern_LeavesNormalTextAlone()
        {
            Assert.AreEqual("mango", SqlText.EscapeLikePattern("mango"));
            Assert.AreEqual("' OR 1=1 --", SqlText.EscapeLikePattern("' OR 1=1 --"), "quotes are harmless inside a parameter");
            Assert.IsNull(SqlText.EscapeLikePattern(null));
        }

        [Test]
        public void QuoteIdentifier_DoublesClosingBracket()
        {
            Assert.AreEqual("[TreeYieldDb]", SqlText.QuoteIdentifier("TreeYieldDb"));
            Assert.AreEqual("[bad]]; DROP DATABASE x; --]", SqlText.QuoteIdentifier("bad]; DROP DATABASE x; --"));
        }

        [Test]
        public void QuoteIdentifier_RejectsBlankOrLongNames()
        {
            Assert.Throws<ArgumentException>(() => SqlText.QuoteIdentifier(" "));
            Assert.Throws<ArgumentException>(() => SqlText.QuoteIdentifier(new string('a', 129)));
        }

        [Test]
        public void SplitBatches_SplitsOnGoLinesOnly()
        {
            string script = "CREATE TABLE A (Id INT);\r\nGO\r\n-- only a comment\r\ngo -- trailing comment\r\nSELECT 'GO here is text';\r\n  GO  \r\n";

            IList<string> batches = SqlScriptRunner.SplitBatches(script);

            Assert.AreEqual(2, batches.Count);
            Assert.AreEqual("CREATE TABLE A (Id INT);", batches[0]);
            Assert.AreEqual("SELECT 'GO here is text';", batches[1]);
        }

        [Test]
        public void EmbeddedScripts_AreFoundInTheAssembly()
        {
            string schema = SqlScriptRunner.LoadEmbeddedScript(DatabaseInitializer.SchemaScriptName);
            string seed = SqlScriptRunner.LoadEmbeddedScript(DatabaseInitializer.SeedScriptName);

            Assert.IsTrue(schema.Contains("CREATE TABLE dbo.TreeSpecies"));
            Assert.IsTrue(seed.Contains("Carabao Mango"));
        }

        [Test]
        public void ConnectionFactory_RequiresDatabaseName()
        {
            Assert.Throws<ArgumentException>(() => new SqlConnectionFactory(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True"));
            Assert.Throws<ArgumentException>(() => new SqlConnectionFactory(""));
        }
    }
}
