Imports System.Data.OleDb

Namespace Data

    ''' <summary>
    ''' Converts Microsoft Access (Jet) error numbers into friendly
    ''' <see cref="DataAccessException"/>s, so users never see raw database messages.
    ''' Jet reports its error number in <see cref="OleDbError.SQLState"/>.
    ''' </summary>
    Public NotInheritable Class AccessErrorTranslator

        ''' <summary>A UNIQUE index or primary key would get a duplicate value.</summary>
        Public Const DuplicateValue As String = "3022"

        ''' <summary>A CHECK constraint (Access "validation rule") was broken.</summary>
        Public Const ValidationRuleBroken As String = "3316"

        ''' <summary>A field-level validation rule was broken.</summary>
        Public Const FieldValidationRuleBroken As String = "3317"

        ''' <summary>A foreign key points to a record that does not exist.</summary>
        Public Const RelatedRecordRequired As String = "3201"

        ''' <summary>A record cannot be deleted because other records refer to it.</summary>
        Public Const RelatedRecordsExist As String = "3200"

        ''' <summary>The file is open exclusively in another program.</summary>
        Public Const FileInUse As String = "3045"

        ''' <summary>The database file does not exist.</summary>
        Public Const FileNotFound As String = "3024"

        Private Const FileInUseHelp As String =
            "The database file is in use by another program (for example Microsoft Access opened in exclusive " &
            "mode, or a table open in Design View). Close it there and try again."

        Private Const ReadOnlyHelp As String =
            "The database file cannot be changed. Make sure TreeYieldDb.mdb is not marked read-only and that " &
            "its folder can be written to."

        Private Sub New()
            ' Only Shared members.
        End Sub

        ''' <summary>Returns the exception that should be thrown instead of <paramref name="exception"/>.</summary>
        ''' <param name="operation">What was being done, for example "load the species list".</param>
        ''' <param name="exception">The original OLE DB exception.</param>
        Public Shared Function Translate(operation As String, exception As OleDbException) As DataAccessException
            Dim code As String = String.Empty
            Dim engineMessage As String = exception.Message
            If exception.Errors.Count > 0 Then
                code = exception.Errors(0).SQLState
                engineMessage = exception.Errors(0).Message
            End If

            Return FromCode(operation, code, engineMessage, exception)
        End Function

        ''' <summary>
        ''' Maps one Jet error number to the exception to throw. Separate from
        ''' <see cref="Translate"/> so it can be unit-tested without a real database error.
        ''' </summary>
        Public Shared Function FromCode(operation As String, code As String, engineMessage As String, inner As Exception) As DataAccessException
            Select Case code
                Case DuplicateValue
                    Return New DuplicateRecordException("A record with the same name already exists.", inner)

                Case ValidationRuleBroken, FieldValidationRuleBroken
                    Return New DataAccessException(
                        "The database rejected the change because a value breaks one of its rules " &
                        "(for example a negative price or a missing name).", inner)

                Case RelatedRecordRequired
                    Return New DataAccessException(
                        "The database rejected the change because a related record (such as the category) does not exist.", inner)

                Case RelatedRecordsExist
                    Return New DataAccessException("This record cannot be deleted because other records still use it.", inner)

                Case FileInUse, "3006", "3008", "3051", "3356"
                    Return New DataAccessException(FileInUseHelp, inner)

                Case "3188", "3218", "3260"
                    Return New DataAccessException(
                        "The record is locked because it is being edited somewhere else (for example in Microsoft Access). " &
                        "Finish or cancel that edit and try again.", inner)

                Case FileNotFound, "3044"
                    Return New DataAccessException("The database file could not be found. Check the DatabaseFile setting in the program's .config file.", inner)
            End Select

            If engineMessage IsNot Nothing AndAlso engineMessage.IndexOf("updateable", StringComparison.OrdinalIgnoreCase) >= 0 Then
                Return New DataAccessException(ReadOnlyHelp, inner)
            End If

            Return New DataAccessException(String.Format(
                "A database error occurred while trying to {0}. (Access error {1})", operation, code), inner)
        End Function

    End Class

End Namespace
