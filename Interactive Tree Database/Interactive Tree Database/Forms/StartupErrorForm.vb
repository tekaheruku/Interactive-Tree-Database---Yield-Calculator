Imports System.Runtime.InteropServices
Imports System.Text
Imports TreeYield.Core.Data

''' <summary>
''' Shown when the database cannot be opened at start-up. Explains how to fix the most
''' common causes and offers Retry (try again) or Exit, instead of crashing.
''' </summary>
Public Class StartupErrorForm

    ''' <summary>Creates the dialog.</summary>
    ''' <param name="friendlyMessage">Short explanation for the user.</param>
    ''' <param name="errorDetails">The original error (shown in the details box).</param>
    ''' <param name="connectionFactory">Used to show which database file was tried.</param>
    Public Sub New(friendlyMessage As String, errorDetails As Exception, connectionFactory As AccessConnectionFactory)
        ' This call is required by the designer.
        InitializeComponent()

        picIcon.Image = SystemIcons.Error.ToBitmap()
        lblMessage.Text = friendlyMessage
        txtDetails.Text = BuildDetails(errorDetails, connectionFactory)
    End Sub

    ''' <summary>Puts the focus on Retry so the details text is not shown fully selected.</summary>
    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        txtDetails.Select(0, 0)
        btnRetry.Focus()
    End Sub

    Private Shared Function BuildDetails(errorDetails As Exception, factory As AccessConnectionFactory) As String
        Dim text As New StringBuilder()
        text.AppendLine("HOW TO FIX")
        ' Lines are kept under 72 characters so they fit the details box without wrapping.
        text.AppendLine("1. If the database is open in Microsoft Access, close Access (or the")
        text.AppendLine("   table you are designing) and click Retry. Access locks the whole")
        text.AppendLine("   file when it is opened with ""Open Exclusive"".")
        text.AppendLine("2. Make sure the folder of the .mdb file is not read-only, so the")
        text.AppendLine("   program can save changes and create its .ldb lock file there.")
        text.AppendLine("3. The program uses the Microsoft Jet 4.0 OLE DB provider, which is")
        text.AppendLine("   built into Windows but 32-bit only. Keep Platform target = x86.")
        text.AppendLine("4. To start with a fresh database, close the program and rename or")
        text.AppendLine("   delete the .mdb file. A new one with the sample data is created")
        text.AppendLine("   on the next start.")
        text.AppendLine("5. Check the DatabaseFile setting in")
        text.AppendLine("   ""Interactive Tree Database.exe.config"".")
        text.AppendLine()
        text.AppendLine("DATABASE FILE")
        text.AppendLine(If(factory Is Nothing, "-", factory.DatabasePath))
        text.AppendLine("Provider: " & If(factory Is Nothing, "-", factory.Provider))
        text.AppendLine()
        text.AppendLine("TECHNICAL DETAILS (also saved in the log folder)")
        text.AppendLine(ErrorReporter.LogDirectory)

        Dim current As Exception = errorDetails
        While current IsNot Nothing
            text.AppendLine(current.GetType().Name & ": " & current.Message)
            current = current.InnerException
        End While

        Return text.ToString()
    End Function

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click
        Try
            Clipboard.SetText(txtDetails.Text)
            btnCopy.Text = "Copied"
        Catch ex As ExternalException
            ' Another program is holding the clipboard; the user can select the text instead.
            txtDetails.SelectAll()
            txtDetails.Focus()
        End Try
    End Sub

End Class
