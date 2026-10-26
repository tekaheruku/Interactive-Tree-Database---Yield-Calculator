Namespace Data

    ''' <summary>
    ''' A database problem translated into a message the user can understand.
    ''' The original <see cref="System.Data.OleDb.OleDbException"/> is kept in
    ''' <see cref="Exception.InnerException"/> for the log file.
    ''' </summary>
    Public Class DataAccessException
        Inherits Exception

        ''' <summary>Creates the exception.</summary>
        ''' <param name="message">User-friendly message.</param>
        ''' <param name="innerException">The original database exception (may be Nothing).</param>
        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

    End Class

End Namespace
