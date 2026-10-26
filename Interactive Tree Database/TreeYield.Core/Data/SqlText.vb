Imports System.Text

Namespace Data

    ''' <summary>Small helpers for building safe Access SQL text.</summary>
    Public NotInheritable Class SqlText

        Private Sub New()
            ' Only Shared members.
        End Sub

        ''' <summary>
        ''' Makes the LIKE wildcards %, _ and [ match literally by putting each one in
        ''' brackets (Access SQL has no ESCAPE clause, but [%] means "a real % sign").
        ''' The value is still sent as a parameter; this only stops "50%" from meaning
        ''' "50 followed by anything".
        ''' </summary>
        Public Shared Function EscapeLikePattern(text As String) As String
            If text Is Nothing Then
                Return Nothing
            End If

            Dim escaped As New StringBuilder(text.Length + 8)
            For Each c As Char In text
                Select Case c
                    Case "%"c, "_"c, "["c
                        escaped.Append("["c).Append(c).Append("]"c)
                    Case Else
                        escaped.Append(c)
                End Select
            Next

            Return escaped.ToString()
        End Function

    End Class

End Namespace
