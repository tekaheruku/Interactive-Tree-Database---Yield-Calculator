Imports System.IO
Imports TreeYield.Core.Data
Imports TreeYield.Tests.Framework

''' <summary>
''' Tests for the data-layer helpers that need no database: LIKE escaping, script
''' splitting, the embedded schema, the database file locator and the Access error codes.
''' </summary>
Public Class DataHelperTests

    <Test>
    Public Sub EscapeLikePattern_PutsWildcardsInBrackets()
        Assert.AreEqual("50[%][_][[]x]", SqlText.EscapeLikePattern("50%_[x]"))
    End Sub

    <Test>
    Public Sub EscapeLikePattern_LeavesNormalTextAlone()
        Assert.AreEqual("mango", SqlText.EscapeLikePattern("mango"))
        Assert.AreEqual("' OR 1=1 --", SqlText.EscapeLikePattern("' OR 1=1 --"), "quotes are harmless inside a parameter")
        Assert.IsNull(SqlText.EscapeLikePattern(Nothing))
    End Sub

    <Test>
    Public Sub SplitStatements_RemovesCommentsAndSplitsOnSemicolons()
        Dim script As String =
            "-- header comment" & vbCrLf &
            "CREATE TABLE A (Id INTEGER);" & vbCrLf &
            vbCrLf &
            "  -- indented comment" & vbCrLf &
            "CREATE INDEX IX_A ON A (Id);" & vbCrLf &
            "   ;" & vbCrLf

        Dim statements As IList(Of String) = SqlScriptRunner.SplitStatements(script)

        Assert.AreEqual(2, statements.Count)
        Assert.AreEqual("CREATE TABLE A (Id INTEGER)", statements(0))
        Assert.AreEqual("CREATE INDEX IX_A ON A (Id)", statements(1))
    End Sub

    <Test>
    Public Sub EmbeddedSchema_IsFoundAndHasEveryTable()
        Dim schema As String = SqlScriptRunner.LoadEmbeddedScript(DatabaseInitializer.SchemaScriptName)

        For Each table As String In {"SchemaVersion", "Categories", "TreeSpecies", "Users", "Scenarios", "AuditLog"}
            Assert.IsTrue(schema.Contains("CREATE TABLE " & table & " ("), table)
        Next
    End Sub

    <Test>
    Public Sub SeedData_Has22SpeciesWithValidRules()
        Assert.AreEqual(2, SeedData.Categories.Count)
        Assert.AreEqual(22, SeedData.Species.Count)
        For Each species In SeedData.Species
            Dim result = TreeYield.Core.Services.SpeciesValidator.Validate(New TreeYield.Core.Models.TreeSpecies With {
                .CommonName = species.CommonName, .ScientificName = species.ScientificName, .CategoryId = 1,
                .ProductType = species.ProductType, .YieldUnit = species.YieldUnit,
                .YieldPerTreePerYear = species.YieldPerTreePerYear, .PricePerUnit = species.PricePerUnit,
                .HarvestType = species.HarvestType, .YearsToFirstHarvest = species.YearsToFirstHarvest,
                .YearsToFullProduction = species.YearsToFullProduction, .HarvestSeason = species.HarvestSeason,
                .Notes = species.Notes})
            Assert.IsTrue(result.IsValid, species.CommonName & ": " & result.ToString())
        Next
    End Sub

    <Test>
    Public Sub Locator_FindsProjectCopyWhenRunningFromBinDebug()
        Dim root As String = NewTempFolder()
        Dim project As String = Path.Combine(root, "MyApp")
        Dim binDebug As String = Path.Combine(project, "bin", "Debug")
        Directory.CreateDirectory(binDebug)
        Directory.CreateDirectory(Path.Combine(project, "Database"))
        File.WriteAllText(Path.Combine(project, "Database", "TreeYieldDb.mdb"), "x")

        Dim found As String = DatabaseFileLocator.Locate(binDebug, "Database\TreeYieldDb.mdb")

        Assert.AreEqual(Path.Combine(project, "Database", "TreeYieldDb.mdb"), found)
        Directory.Delete(root, True)
    End Sub

    <Test>
    Public Sub Locator_PrefersCopyNextToTheProgram()
        Dim root As String = NewTempFolder()
        Directory.CreateDirectory(Path.Combine(root, "Database"))
        File.WriteAllText(Path.Combine(root, "Database", "TreeYieldDb.mdb"), "x")

        Assert.AreEqual(Path.Combine(root, "Database", "TreeYieldDb.mdb"), DatabaseFileLocator.Locate(root, "Database\TreeYieldDb.mdb"))
        Directory.Delete(root, True)
    End Sub

    <Test>
    Public Sub Locator_MissingFile_PointsToProjectFolderFromBin()
        Dim root As String = NewTempFolder()
        Dim binRelease As String = Path.Combine(root, "MyApp", "bin", "Release")
        Directory.CreateDirectory(binRelease)

        Assert.AreEqual(Path.Combine(root, "MyApp", "Database", "TreeYieldDb.mdb"),
                        DatabaseFileLocator.Locate(binRelease, "Database\TreeYieldDb.mdb"))
        Assert.AreEqual("C:\Data\Trees.mdb", DatabaseFileLocator.Locate(binRelease, "C:\Data\Trees.mdb"), "absolute paths are used as they are")
        Directory.Delete(root, True)
    End Sub

    <Test>
    Public Sub ErrorCodes_AreTranslatedToFriendlyMessages()
        Assert.IsTrue(TypeOf AccessErrorTranslator.FromCode("save", "3022", Nothing, Nothing) Is DuplicateRecordException, "3022 = duplicate")
        Assert.IsTrue(AccessErrorTranslator.FromCode("save", "3316", Nothing, Nothing).Message.Contains("rules"), "3316 = CHECK rule")
        Assert.IsTrue(AccessErrorTranslator.FromCode("open", "3045", Nothing, Nothing).Message.Contains("in use"), "3045 = file in use")
        Assert.IsTrue(AccessErrorTranslator.FromCode("open", "3024", Nothing, Nothing).Message.Contains("could not be found"), "3024 = file not found")
        Assert.IsTrue(AccessErrorTranslator.FromCode("save", "9999", "Operation must use an updateable query.", Nothing).Message.Contains("read-only"))
        Assert.IsTrue(AccessErrorTranslator.FromCode("load the list", "9999", "x", Nothing).Message.Contains("load the list"))
    End Sub

    <Test>
    Public Sub ConnectionFactory_RequiresPathAndDefaultsToJet()
        Assert.Throws(Of ArgumentException)(Sub()
                                                Dim unused As New AccessConnectionFactory("")
                                            End Sub)
        Dim factory As New AccessConnectionFactory("C:\Temp\x.mdb", Nothing)
        Assert.AreEqual(AccessConnectionFactory.DefaultProvider, factory.Provider)
        Assert.AreEqual("x.mdb", factory.DatabaseFileName)
    End Sub

    Private Shared Function NewTempFolder() As String
        Dim folder As String = Path.Combine(Path.GetTempPath(), "TreeYieldLocator_" & Guid.NewGuid().ToString("N").Substring(0, 8))
        Directory.CreateDirectory(folder)
        Return folder
    End Function

End Class
