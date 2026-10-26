Namespace Data

    ''' <summary>
    ''' Thrown when an INSERT or UPDATE breaks a UNIQUE index, for example when
    ''' two species would get the same common name.
    ''' </summary>
    Public Class DuplicateRecordException
        Inherits DataAccessException

        ''' <summary>Creates the exception.</summary>
        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

    End Class

End Namespace
