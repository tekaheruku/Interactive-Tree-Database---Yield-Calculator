Imports System.Globalization
Imports System.IO
Imports TreeYield.Core.Data
Imports TreeYield.Core.Services

''' <summary>
''' Central error handling for the UI: writes full technical details to a log file
''' and shows the user a short, friendly message.
''' </summary>
Friend NotInheritable Class ErrorReporter

    Private Const Caption As String = "Tree Yield & Revenue Calculator"

    Private Sub New()
    End Sub

    ''' <summary>Folder for log files: %LOCALAPPDATA%\TreeYieldCalculator\logs.</summary>
    Public Shared ReadOnly Property LogDirectory As String
        Get
            Return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "TreeYieldCalculator", "logs")
        End Get
    End Property

    ''' <summary>Appends an exception to today's log file. Never throws.</summary>
    Public Shared Sub Log(exception As Exception)
        If exception Is Nothing Then
            Return
        End If

        Try
            Directory.CreateDirectory(LogDirectory)
            Dim file As String = Path.Combine(LogDirectory, "app-" & DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture) & ".log")
            Dim entry As String = String.Format(CultureInfo.InvariantCulture, "[{0:yyyy-MM-dd HH:mm:ss}] {1}{2}{2}",
                                                DateTime.Now, exception, Environment.NewLine)
            System.IO.File.AppendAllText(file, entry)
        Catch ex As Exception
            ' Logging must never cause a second error (for example when the disk is full).
        End Try
    End Sub

    ''' <summary>
    ''' Returns a message that is safe and useful to show. Our own exceptions already
    ''' carry friendly text; anything else is described generically.
    ''' </summary>
    Public Shared Function GetFriendlyMessage(exception As Exception) As String
        If TypeOf exception Is DataAccessException OrElse TypeOf exception Is ValidationException OrElse
           TypeOf exception Is UnauthorizedAccessException OrElse TypeOf exception Is IOException Then
            Return exception.Message
        End If

        Return "An unexpected error occurred. Technical details were saved to the log file in:" & vbCrLf & LogDirectory
    End Function

    ''' <summary>Logs an error and shows "Could not {action}." with the friendly message.</summary>
    ''' <param name="owner">Window that owns the message box.</param>
    ''' <param name="action">What was being done, for example "save the species".</param>
    ''' <param name="exception">The error.</param>
    Public Shared Sub ShowError(owner As IWin32Window, action As String, exception As Exception)
        Log(exception)
        MessageBox.Show(owner, "Could not " & action & "." & vbCrLf & vbCrLf & GetFriendlyMessage(exception),
                        Caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    ''' <summary>Called by the global handler in ApplicationEvents.vb for errors nobody caught.</summary>
    Public Shared Sub HandleUnexpected(exception As Exception)
        Log(exception)
        MessageBox.Show(
            "Something went wrong, but the program can keep running." & vbCrLf & vbCrLf & GetFriendlyMessage(exception),
            Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    ''' <summary>Shows an information message.</summary>
    Public Shared Sub ShowInfo(owner As IWin32Window, message As String)
        MessageBox.Show(owner, message, Caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>Asks a Yes/No question with "No" as the safe default. Returns True for Yes.</summary>
    Public Shared Function Confirm(owner As IWin32Window, message As String, title As String) As Boolean
        Return MessageBox.Show(owner, message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                               MessageBoxDefaultButton.Button2) = DialogResult.Yes
    End Function

End Class
