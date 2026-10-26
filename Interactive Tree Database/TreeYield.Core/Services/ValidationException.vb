Namespace Services

    ''' <summary>
    ''' Thrown by a service when it is asked to save data that fails validation.
    ''' The UI catches it and shows <see cref="Result"/> next to the input fields.
    ''' </summary>
    Public Class ValidationException
        Inherits Exception

        ''' <summary>Creates the exception from a failed validation result.</summary>
        Public Sub New(result As ValidationResult)
            MyBase.New(If(result Is Nothing, "The data is not valid.", result.ToString()))
            Me.Result = If(result, New ValidationResult())
        End Sub

        ''' <summary>The individual field errors.</summary>
        Public ReadOnly Property Result As ValidationResult

    End Class

End Namespace
