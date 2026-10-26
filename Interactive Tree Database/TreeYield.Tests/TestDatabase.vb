Imports System.Data.OleDb
Imports System.IO
Imports TreeYield.Core.Data
Imports TreeYield.Core.Services

''' <summary>
''' Creates a fresh, throw-away Access database file in the Windows temp folder the
''' first time an integration test needs it, and deletes it when the run ends. The
''' project's real TreeYieldDb.mdb is never touched.
''' </summary>
Friend NotInheritable Class TestDatabase

    Private Shared _factory As AccessConnectionFactory

    Private Sub New()
        ' Only Shared members.
    End Sub

    ''' <summary>Connection factory for the test database (created on first use).</summary>
    Public Shared ReadOnly Property Factory As AccessConnectionFactory
        Get
            If _factory Is Nothing Then
                _factory = Create()
            End If

            Return _factory
        End Get
    End Property

    ''' <summary>Deletes the test database file if this run created it.</summary>
    Public Shared Sub DeleteIfCreated()
        If _factory Is Nothing Then
            Return
        End If

        Try
            ' Pooled connections keep the file open; release them before deleting.
            OleDbConnection.ReleaseObjectPool()
            File.Delete(_factory.DatabasePath)
        Catch ex As IOException
            Console.WriteLine("Note: could not delete " & _factory.DatabasePath & ": " & ex.Message)
        End Try

        _factory = Nothing
    End Sub

    Private Shared Function Create() As AccessConnectionFactory
        Dim folder As String = Path.Combine(Path.GetTempPath(), "TreeYieldTests")
        Dim fileName As String = "TreeYieldDb_Test_" & Guid.NewGuid().ToString("N").Substring(0, 8) & ".mdb"
        Dim factory As New AccessConnectionFactory(Path.Combine(folder, fileName))

        ' This also tests the "no database file yet" path: ADOX creates the .mdb,
        ' Schema.sql creates the tables and the sample data is inserted.
        Dim initializer As New DatabaseInitializer(factory)
        initializer.Initialize()

        Dim auth As New AuthService(New UserRepository(factory), New AuditLogRepository(factory))
        auth.EnsureDefaultAdminAsync().GetAwaiter().GetResult()
        Return factory
    End Function

End Class
