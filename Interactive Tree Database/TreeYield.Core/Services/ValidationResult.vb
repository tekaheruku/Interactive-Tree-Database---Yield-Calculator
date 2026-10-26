Namespace Services

    ''' <summary>One validation problem tied to a field (property name) of the object being checked.</summary>
    Public Class ValidationError

        ''' <summary>Creates an error for a field.</summary>
        Public Sub New(field As String, message As String)
            Me.Field = field
            Me.Message = message
        End Sub

        ''' <summary>Property name, for example "CommonName". The UI uses it to find the input box.</summary>
        Public ReadOnly Property Field As String

        ''' <summary>Message for the user, for example "Common name is required."</summary>
        Public ReadOnly Property Message As String

    End Class

    ''' <summary>
    ''' A list of validation errors. Validators return this instead of throwing on the
    ''' first problem, so the form can highlight every invalid field at once.
    ''' </summary>
    Public Class ValidationResult

        Private ReadOnly _errors As New List(Of ValidationError)()

        ''' <summary>True when no errors were added.</summary>
        Public ReadOnly Property IsValid As Boolean
            Get
                Return _errors.Count = 0
            End Get
        End Property

        ''' <summary>All errors in the order they were found.</summary>
        Public ReadOnly Property Errors As IReadOnlyList(Of ValidationError)
            Get
                Return _errors
            End Get
        End Property

        ''' <summary>Adds an error for a field.</summary>
        Public Sub Add(field As String, message As String)
            _errors.Add(New ValidationError(field, message))
        End Sub

        ''' <summary>Returns the first error message for a field, or Nothing if the field is valid.</summary>
        Public Function GetError(field As String) As String
            Dim found As ValidationError = _errors.FirstOrDefault(Function(e) e.Field = field)
            Return If(found Is Nothing, Nothing, found.Message)
        End Function

        ''' <summary>All messages, one per line.</summary>
        Public Overrides Function ToString() As String
            Return String.Join(Environment.NewLine, _errors.Select(Function(e) e.Message))
        End Function

    End Class

End Namespace
