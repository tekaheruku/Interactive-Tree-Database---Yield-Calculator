Imports System.Data.OleDb
Imports System.IO

Namespace Data

    ''' <summary>
    ''' Creates connections to the Microsoft Access database file (.mdb) through OLE DB.
    ''' Connections are opened late and closed early (inside Using blocks); OLE DB
    ''' session pooling makes re-opening them cheap.
    ''' </summary>
    Public Class AccessConnectionFactory

        ''' <summary>
        ''' The Jet 4.0 engine reads and writes Access .mdb files. It is part of every
        ''' Windows installation, but only for 32-bit programs - which is why the
        ''' application and the test runner are compiled for x86.
        ''' </summary>
        Public Const DefaultProvider As String = "Microsoft.Jet.OLEDB.4.0"

        ''' <summary>Creates the factory.</summary>
        ''' <param name="databasePath">Full or relative path of the .mdb file.</param>
        ''' <param name="provider">OLE DB provider; Nothing or blank means <see cref="DefaultProvider"/>.</param>
        Public Sub New(databasePath As String, Optional provider As String = DefaultProvider)
            If String.IsNullOrWhiteSpace(databasePath) Then
                Throw New ArgumentException("A database file path is required.", NameOf(databasePath))
            End If

            Me.DatabasePath = Path.GetFullPath(databasePath)
            Me.Provider = If(String.IsNullOrWhiteSpace(provider), DefaultProvider, provider.Trim())

            Dim builder As New OleDbConnectionStringBuilder()
            builder.Provider = Me.Provider
            builder.DataSource = Me.DatabasePath
            ConnectionString = builder.ConnectionString
        End Sub

        ''' <summary>Full path of the .mdb file.</summary>
        Public ReadOnly Property DatabasePath As String

        ''' <summary>OLE DB provider name, for example "Microsoft.Jet.OLEDB.4.0".</summary>
        Public ReadOnly Property Provider As String

        ''' <summary>The OLE DB connection string built from the path and provider.</summary>
        Public ReadOnly Property ConnectionString As String

        ''' <summary>File name only, for example "TreeYieldDb.mdb" (shown in the status bar).</summary>
        Public ReadOnly Property DatabaseFileName As String
            Get
                Return Path.GetFileName(DatabasePath)
            End Get
        End Property

        ''' <summary>Opens a connection to the database file.</summary>
        ''' <exception cref="DataAccessException">The Jet provider is not available to this process.</exception>
        Public Function Open() As OleDbConnection
            Dim connection As New OleDbConnection(ConnectionString)
            Try
                connection.Open()
                Return connection
            Catch ex As InvalidOperationException When ex.Message.IndexOf("not registered", StringComparison.OrdinalIgnoreCase) >= 0
                ' Happens when the program runs as 64-bit: Jet 4.0 has no 64-bit version.
                connection.Dispose()
                Throw New DataAccessException(String.Format(
                    "The database engine '{0}' is not available to this program. Jet 4.0 works only in 32-bit " &
                    "programs, so the project's Target CPU must be x86 (Project Properties > Compile).", Provider), ex)
            Catch
                ' Do not leak the connection object if opening fails.
                connection.Dispose()
                Throw
            End Try
        End Function

    End Class

End Namespace
