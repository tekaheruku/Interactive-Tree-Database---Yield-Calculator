Imports System.Data.OleDb
Imports System.IO
Imports System.Reflection

Namespace Data

    ''' <summary>Loads the .sql file embedded in this assembly and runs it statement by statement.</summary>
    Public NotInheritable Class SqlScriptRunner

        Private Sub New()
            ' Only Shared members.
        End Sub

        ''' <summary>
        ''' Splits a script into statements. Access runs only one statement per command and
        ''' does not understand comments, so lines that start with -- are removed and the
        ''' rest is split at each semicolon. (The schema script has no semicolons inside text.)
        ''' </summary>
        Public Shared Function SplitStatements(script As String) As IList(Of String)
            If String.IsNullOrWhiteSpace(script) Then
                Return New List(Of String)()
            End If

            Dim codeLines As IEnumerable(Of String) = script.Split(ControlChars.Lf).
                Select(Function(line) line.TrimEnd(ControlChars.Cr)).
                Where(Function(line) Not line.TrimStart().StartsWith("--", StringComparison.Ordinal))

            Return String.Join(vbLf, codeLines).
                Split(";"c).
                Select(Function(statement) statement.Trim()).
                Where(Function(statement) statement.Length > 0).
                ToList()
        End Function

        ''' <summary>Reads an embedded script, for example "Schema.sql".</summary>
        Public Shared Function LoadEmbeddedScript(fileName As String) As String
            Dim assembly As Assembly = GetType(SqlScriptRunner).Assembly

            ' Visual Basic names embedded resources RootNamespace + file name, e.g. "TreeYield.Core.Schema.sql".
            Dim resourceName As String = assembly.GetManifestResourceNames().
                FirstOrDefault(Function(name) name.EndsWith("." & fileName, StringComparison.OrdinalIgnoreCase))

            If resourceName Is Nothing Then
                Throw New InvalidOperationException("The embedded SQL script '" & fileName & "' was not found.")
            End If

            Using stream As Stream = assembly.GetManifestResourceStream(resourceName)
                Using reader As New StreamReader(stream)
                    Return reader.ReadToEnd()
                End Using
            End Using
        End Function

        ''' <summary>Runs every statement of a script on an open connection.</summary>
        Public Shared Sub Execute(connection As OleDbConnection, transaction As OleDbTransaction, script As String)
            For Each statement As String In SplitStatements(script)
                Using command As New OleDbCommand(statement, connection, transaction)
                    command.ExecuteNonQuery()
                End Using
            Next
        End Sub

    End Class

End Namespace
