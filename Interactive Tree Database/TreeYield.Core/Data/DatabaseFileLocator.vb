Imports System.IO

Namespace Data

    ''' <summary>
    ''' Decides which .mdb file the program uses.
    ''' <para>
    ''' When the program runs from Visual Studio, the .exe is in bin\Debug, but the
    ''' database lives in the project's Database folder. Searching upward from the
    ''' program's folder finds that project copy, so the file you open in Microsoft
    ''' Access and the file the program changes are the same file.
    ''' </para>
    ''' </summary>
    Public NotInheritable Class DatabaseFileLocator

        ''' <summary>How many parent folders are searched (bin\Debug -> bin -> project folder).</summary>
        Public Const MaxLevelsUp As Integer = 3

        Private Sub New()
            ' Only Shared members.
        End Sub

        ''' <summary>
        ''' Returns the database path to use.
        ''' <list type="number">
        ''' <item>An absolute <paramref name="relativePath"/> is used as it is.</item>
        ''' <item>Otherwise the program folder and up to <see cref="MaxLevelsUp"/> parent folders are searched for an existing file.</item>
        ''' <item>If none exists, the path where a new database should be created is returned: the project folder
        ''' when running from bin\Debug or bin\Release, else the program folder.</item>
        ''' </list>
        ''' </summary>
        ''' <param name="programFolder">Folder of the running .exe.</param>
        ''' <param name="relativePath">For example "Database\TreeYieldDb.mdb".</param>
        Public Shared Function Locate(programFolder As String, relativePath As String) As String
            If String.IsNullOrWhiteSpace(relativePath) Then
                Throw New ArgumentException("A database file name is required.", NameOf(relativePath))
            End If

            If Path.IsPathRooted(relativePath) Then
                Return relativePath
            End If

            Dim folder As DirectoryInfo = New DirectoryInfo(programFolder)
            For level As Integer = 0 To MaxLevelsUp
                If folder Is Nothing Then
                    Exit For
                End If

                Dim candidate As String = Path.Combine(folder.FullName, relativePath)
                If File.Exists(candidate) Then
                    Return candidate
                End If

                folder = folder.Parent
            Next

            Return Path.Combine(DefaultFolder(programFolder), relativePath)
        End Function

        ''' <summary>The project folder when the program runs from ...\bin\Debug or ...\bin\Release; otherwise the program folder.</summary>
        Private Shared Function DefaultFolder(programFolder As String) As String
            Dim folder As New DirectoryInfo(programFolder)
            If folder.Parent IsNot Nothing AndAlso folder.Parent.Parent IsNot Nothing AndAlso
               String.Equals(folder.Parent.Name, "bin", StringComparison.OrdinalIgnoreCase) Then
                Return folder.Parent.Parent.FullName
            End If

            Return folder.FullName
        End Function

    End Class

End Namespace
