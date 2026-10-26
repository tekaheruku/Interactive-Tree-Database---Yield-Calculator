Imports System.Data
Imports System.Data.OleDb

Namespace Data

    ''' <summary>
    ''' Shared plumbing for every repository: opening a connection, translating Access
    ''' errors, and adding typed parameters. Every query in this application uses
    ''' parameters (the ? marks) instead of joining user input into the SQL text,
    ''' which is what prevents SQL injection.
    ''' </summary>
    Public MustInherit Class RepositoryBase

        ''' <summary>Creates the repository.</summary>
        Protected Sub New(connectionFactory As AccessConnectionFactory)
            If connectionFactory Is Nothing Then
                Throw New ArgumentNullException(NameOf(connectionFactory))
            End If

            Me.ConnectionFactory = connectionFactory
        End Sub

        ''' <summary>Opens connections to the database file.</summary>
        Protected ReadOnly Property ConnectionFactory As AccessConnectionFactory

        ''' <summary>
        ''' Runs <paramref name="work"/> on a background thread with an open connection,
        ''' then closes the connection. Access (OLE DB) has no real asynchronous calls, so
        ''' Task.Run keeps the window responsive while the file is read.
        ''' </summary>
        ''' <param name="operation">Short description used in error messages, for example "save the species".</param>
        ''' <param name="work">The database work to run on the open connection.</param>
        Protected Function QueryAsync(Of T)(operation As String, work As Func(Of OleDbConnection, T)) As Task(Of T)
            Return Task.Run(Function() Run(operation, work))
        End Function

        ''' <summary>Same as <see cref="QueryAsync"/> for work that returns no value.</summary>
        Protected Function ExecuteAsync(operation As String, work As Action(Of OleDbConnection)) As Task
            Return QueryAsync(operation, Function(connection)
                                             work(connection)
                                             Return True
                                         End Function)
        End Function

        ''' <summary>Runs the work synchronously and converts any <see cref="OleDbException"/> into a <see cref="DataAccessException"/>.</summary>
        Protected Function Run(Of T)(operation As String, work As Func(Of OleDbConnection, T)) As T
            Try
                Using connection As OleDbConnection = ConnectionFactory.Open()
                    Return work(connection)
                End Using
            Catch ex As OleDbException
                Throw AccessErrorTranslator.Translate(operation, ex)
            End Try
        End Function

        ''' <summary>Creates a text command on an open connection (optionally inside a transaction).</summary>
        Protected Shared Function CreateCommand(connection As OleDbConnection, sql As String, Optional transaction As OleDbTransaction = Nothing) As OleDbCommand
            Dim command As OleDbCommand = connection.CreateCommand()
            command.CommandText = sql
            command.CommandType = CommandType.Text
            command.Transaction = transaction
            Return command
        End Function

        ' ------------------------------------------------------------------
        ' Parameters. In Access the names are ignored: values are matched to the
        ' ? marks by POSITION, so they must be added in the same order as in the SQL.
        ' ------------------------------------------------------------------

        ''' <summary>Adds a short text parameter (TEXT(n) column). Nothing is sent as NULL.</summary>
        Protected Shared Sub AddText(command As OleDbCommand, value As String, size As Integer)
            command.Parameters.Add(New OleDbParameter("?", OleDbType.VarWChar, size) With {.Value = DbValue(value)})
        End Sub

        ''' <summary>Adds a long text parameter (MEMO column). Nothing is sent as NULL.</summary>
        Protected Shared Sub AddMemo(command As OleDbCommand, value As String)
            command.Parameters.Add(New OleDbParameter("?", OleDbType.LongVarWChar) With {.Value = DbValue(value)})
        End Sub

        ''' <summary>Adds an INTEGER parameter. Nothing is sent as NULL.</summary>
        Protected Shared Sub AddInteger(command As OleDbCommand, value As Integer?)
            command.Parameters.Add(New OleDbParameter("?", OleDbType.Integer) With {
                .Value = If(value.HasValue, CObj(value.Value), DBNull.Value)})
        End Sub

        ''' <summary>Adds a DECIMAL(12,2) parameter, matching the money and quantity columns.</summary>
        Protected Shared Sub AddDecimal(command As OleDbCommand, value As Decimal)
            command.Parameters.Add(New OleDbParameter("?", OleDbType.Decimal) With {.Precision = 12, .Scale = 2, .Value = value})
        End Sub

        ''' <summary>Adds a DATETIME parameter. Nothing is sent as NULL.</summary>
        Protected Shared Sub AddDate(command As OleDbCommand, value As DateTime?)
            command.Parameters.Add(New OleDbParameter("?", OleDbType.Date) With {
                .Value = If(value.HasValue, CObj(value.Value), DBNull.Value)})
        End Sub

        ''' <summary>Adds a Yes/No (BIT) parameter.</summary>
        Protected Shared Sub AddBoolean(command As OleDbCommand, value As Boolean)
            command.Parameters.Add(New OleDbParameter("?", OleDbType.Boolean) With {.Value = value})
        End Sub

        Private Shared Function DbValue(value As String) As Object
            Return If(value Is Nothing, CObj(DBNull.Value), value)
        End Function

        ''' <summary>
        ''' The current UTC time without milliseconds. Access stores times to the second,
        ''' so dropping the milliseconds keeps saved and re-read values equal.
        ''' </summary>
        Protected Shared Function UtcNow() As DateTime
            Dim now As DateTime = DateTime.UtcNow
            Return New DateTime(now.Ticks - (now.Ticks Mod TimeSpan.TicksPerSecond), DateTimeKind.Utc)
        End Function

        ''' <summary>Returns the AutoNumber value created by the last INSERT on this connection.</summary>
        Protected Shared Function LastInsertedId(connection As OleDbConnection, Optional transaction As OleDbTransaction = Nothing) As Integer
            Using command As OleDbCommand = CreateCommand(connection, "SELECT @@IDENTITY", transaction)
                Return Convert.ToInt32(command.ExecuteScalar())
            End Using
        End Function

        ' ------------------------------------------------------------------
        ' Reading columns.
        ' ------------------------------------------------------------------

        ''' <summary>Reads a nullable text column.</summary>
        Protected Shared Function ReadString(reader As OleDbDataReader, column As String) As String
            Dim ordinal As Integer = reader.GetOrdinal(column)
            Return If(reader.IsDBNull(ordinal), Nothing, Convert.ToString(reader.GetValue(ordinal)))
        End Function

        ''' <summary>Reads an INTEGER or AutoNumber column.</summary>
        Protected Shared Function ReadInteger(reader As OleDbDataReader, column As String) As Integer
            Return Convert.ToInt32(reader.GetValue(reader.GetOrdinal(column)))
        End Function

        ''' <summary>Reads a nullable INTEGER column.</summary>
        Protected Shared Function ReadNullableInteger(reader As OleDbDataReader, column As String) As Integer?
            Dim ordinal As Integer = reader.GetOrdinal(column)
            Return If(reader.IsDBNull(ordinal), CType(Nothing, Integer?), Convert.ToInt32(reader.GetValue(ordinal)))
        End Function

        ''' <summary>Reads a DECIMAL column.</summary>
        Protected Shared Function ReadDecimal(reader As OleDbDataReader, column As String) As Decimal
            Return Convert.ToDecimal(reader.GetValue(reader.GetOrdinal(column)))
        End Function

        ''' <summary>Reads a DATETIME column.</summary>
        Protected Shared Function ReadDate(reader As OleDbDataReader, column As String) As DateTime
            Return Convert.ToDateTime(reader.GetValue(reader.GetOrdinal(column)))
        End Function

        ''' <summary>Reads a nullable DATETIME column.</summary>
        Protected Shared Function ReadNullableDate(reader As OleDbDataReader, column As String) As DateTime?
            Dim ordinal As Integer = reader.GetOrdinal(column)
            Return If(reader.IsDBNull(ordinal), CType(Nothing, DateTime?), Convert.ToDateTime(reader.GetValue(ordinal)))
        End Function

        ''' <summary>Reads a Yes/No column.</summary>
        Protected Shared Function ReadBoolean(reader As OleDbDataReader, column As String) As Boolean
            Return Convert.ToBoolean(reader.GetValue(reader.GetOrdinal(column)))
        End Function

    End Class

End Namespace
