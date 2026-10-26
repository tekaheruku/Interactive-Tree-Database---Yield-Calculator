Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports TreeYield.Core.Models

Namespace Data

    ''' <summary>
    ''' Makes sure the Access database file exists and has the tables and sample data.
    ''' <para>
    ''' The project ships a ready-made TreeYieldDb.mdb, so normally there is nothing to do.
    ''' If the file is missing (for example deleted to start over, or a new PC), a new
    ''' .mdb is created with ADOX, the tables are created from the embedded Schema.sql,
    ''' and the sample data is inserted.
    ''' </para>
    ''' It is safe to call <see cref="InitializeAsync"/> every time the program starts.
    ''' </summary>
    Public Class DatabaseInitializer
        Inherits RepositoryBase

        ''' <summary>Schema version this build of the program expects.</summary>
        Public Const CurrentSchemaVersion As Integer = 1

        ''' <summary>Embedded script that creates the tables.</summary>
        Public Const SchemaScriptName As String = "Schema.sql"

        ''' <summary>Creates the initializer.</summary>
        Public Sub New(connectionFactory As AccessConnectionFactory)
            MyBase.New(connectionFactory)
        End Sub

        ''' <summary>Runs <see cref="Initialize"/> on a background thread.</summary>
        Public Function InitializeAsync() As Task
            Return Task.Run(AddressOf Initialize)
        End Function

        ''' <summary>
        ''' Creates the file, tables and sample data if they are missing. The sample data
        ''' is inserted in one transaction. Access cannot roll back CREATE TABLE, so a file
        ''' that this call created and could not finish is deleted again; the next start
        ''' then simply tries again from the beginning.
        ''' </summary>
        Public Sub Initialize()
            Dim createdNow As Boolean = False
            Try
                If Not File.Exists(ConnectionFactory.DatabasePath) Then
                    CreateDatabaseFile()
                    createdNow = True
                End If

                Using connection As OleDbConnection = ConnectionFactory.Open()
                    If TableExists(connection, "SchemaVersion") Then
                        Return
                    End If

                    SqlScriptRunner.Execute(connection, Nothing, SqlScriptRunner.LoadEmbeddedScript(SchemaScriptName))

                    Using transaction As OleDbTransaction = connection.BeginTransaction()
                        InsertMissingCategories(connection, transaction)
                        InsertSampleSpecies(connection, transaction)

                        Using command As OleDbCommand = CreateCommand(connection,
                            "INSERT INTO SchemaVersion (VersionNumber, AppliedAt) VALUES (?, ?)", transaction)
                            AddInteger(command, CurrentSchemaVersion)
                            AddDate(command, UtcNow())
                            command.ExecuteNonQuery()
                        End Using

                        transaction.Commit()
                    End Using
                End Using
            Catch ex As OleDbException
                If createdNow Then
                    DeleteDatabaseFile()
                End If

                Throw AccessErrorTranslator.Translate("prepare the database", ex)
            Catch ex As Exception When createdNow
                DeleteDatabaseFile()
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Deletes all species and saved scenarios and inserts the original sample species
        ''' again. User accounts and the audit log are kept.
        ''' </summary>
        Public Function ResetSampleDataAsync() As Task
            Return ExecuteAsync("restore the sample data", Sub(connection)
                Using transaction As OleDbTransaction = connection.BeginTransaction()
                    For Each sql As String In {"DELETE FROM Scenarios", "DELETE FROM TreeSpecies"}
                        Using command As OleDbCommand = CreateCommand(connection, sql, transaction)
                            command.ExecuteNonQuery()
                        End Using
                    Next

                    InsertMissingCategories(connection, transaction)
                    InsertSampleSpecies(connection, transaction)
                    transaction.Commit()
                End Using
            End Sub)
        End Function

        ''' <summary>
        ''' Creates an empty .mdb file with ADOX, the "ActiveX Data Objects Extensions" that
        ''' come with Windows. It is used through late binding (by name), so the project
        ''' needs no COM reference.
        ''' </summary>
        Private Sub CreateDatabaseFile()
            Dim folder As String = Path.GetDirectoryName(ConnectionFactory.DatabasePath)
            Directory.CreateDirectory(folder)

            Dim catalogType As Type = Type.GetTypeFromProgID("ADOX.Catalog")
            If catalogType Is Nothing Then
                Throw New DataAccessException("A new database file cannot be created because ADOX is not available on this PC.", Nothing)
            End If

            Dim catalog As Object = Activator.CreateInstance(catalogType)
            Try
                ' Engine Type 5 = the Jet 4.0 file format (Access 2000-2003 .mdb).
                Dim createString As String = ConnectionFactory.ConnectionString & ";Jet OLEDB:Engine Type=5"
                catalogType.InvokeMember("Create", BindingFlags.InvokeMethod, Nothing, catalog, New Object() {createString})

                ' Close the connection ADOX keeps open, otherwise the new file stays locked.
                Dim activeConnection As Object = catalogType.InvokeMember("ActiveConnection", BindingFlags.GetProperty, Nothing, catalog, Nothing)
                activeConnection.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, Nothing, activeConnection, Nothing)
                Marshal.FinalReleaseComObject(activeConnection)
            Catch ex As TargetInvocationException
                Dim reason As Exception = If(ex.InnerException, ex)
                Throw New DataAccessException("A new database file could not be created at " &
                    ConnectionFactory.DatabasePath & ". " & reason.Message, reason)
            Finally
                Marshal.FinalReleaseComObject(catalog)
            End Try
        End Sub

        Private Sub DeleteDatabaseFile()
            Try
                ' Pooled connections keep the file open; release them first.
                OleDbConnection.ReleaseObjectPool()
                File.Delete(ConnectionFactory.DatabasePath)
            Catch ex As IOException
                Trace.TraceWarning("Could not delete the unfinished database file: " & ex.Message)
            Catch ex As UnauthorizedAccessException
                Trace.TraceWarning("Could not delete the unfinished database file: " & ex.Message)
            End Try
        End Sub

        Private Shared Function TableExists(connection As OleDbConnection, tableName As String) As Boolean
            ' Restrictions: catalog, schema, table name, table type.
            Dim tables As DataTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                New Object() {Nothing, Nothing, tableName, "TABLE"})
            Return tables IsNot Nothing AndAlso tables.Rows.Count > 0
        End Function

        Private Shared Sub InsertMissingCategories(connection As OleDbConnection, transaction As OleDbTransaction)
            For Each category As Category In SeedData.Categories
                Using check As OleDbCommand = CreateCommand(connection, "SELECT COUNT(*) FROM Categories WHERE CategoryName = ?", transaction)
                    AddText(check, category.Name, 50)
                    If Convert.ToInt32(check.ExecuteScalar()) > 0 Then
                        Continue For
                    End If
                End Using

                Using insert As OleDbCommand = CreateCommand(connection,
                    "INSERT INTO Categories (CategoryName, Description) VALUES (?, ?)", transaction)
                    AddText(insert, category.Name, 50)
                    AddText(insert, category.Description, 200)
                    insert.ExecuteNonQuery()
                End Using
            Next
        End Sub

        Private Shared Sub InsertSampleSpecies(connection As OleDbConnection, transaction As OleDbTransaction)
            ' The AutoNumber IDs are only known after the categories are inserted, so look them up by name.
            Dim categoryIds As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
            Using command As OleDbCommand = CreateCommand(connection, "SELECT CategoryId, CategoryName FROM Categories", transaction)
                Using reader As OleDbDataReader = command.ExecuteReader()
                    While reader.Read()
                        categoryIds(ReadString(reader, "CategoryName")) = ReadInteger(reader, "CategoryId")
                    End While
                End Using
            End Using

            Dim now As DateTime = UtcNow()
            For Each species As TreeSpecies In SeedData.Species
                Using command As OleDbCommand = CreateCommand(connection,
                    "INSERT INTO TreeSpecies (CommonName, ScientificName, CategoryId, ProductType, YieldUnit, " &
                    "YieldPerTreePerYear, PricePerUnit, HarvestType, YearsToFirstHarvest, YearsToFullProduction, " &
                    "HarvestSeason, Notes, CreatedAt, UpdatedAt) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", transaction)
                    AddText(command, species.CommonName, 100)
                    AddText(command, species.ScientificName, 150)
                    AddInteger(command, categoryIds(species.CategoryName))
                    AddText(command, species.ProductType, 60)
                    AddText(command, species.YieldUnit, 20)
                    AddDecimal(command, species.YieldPerTreePerYear)
                    AddDecimal(command, species.PricePerUnit)
                    AddText(command, species.HarvestType.ToString(), 10)
                    AddInteger(command, species.YearsToFirstHarvest)
                    AddInteger(command, species.YearsToFullProduction)
                    AddText(command, species.HarvestSeason, 100)
                    AddMemo(command, species.Notes)
                    AddDate(command, now)
                    AddDate(command, now)
                    command.ExecuteNonQuery()
                End Using
            Next
        End Sub

    End Class

End Namespace
