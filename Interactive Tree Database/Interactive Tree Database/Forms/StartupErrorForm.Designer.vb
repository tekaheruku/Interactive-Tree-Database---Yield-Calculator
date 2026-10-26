<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StartupErrorForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.layoutMain = New System.Windows.Forms.TableLayoutPanel()
        Me.picIcon = New System.Windows.Forms.PictureBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.txtDetails = New System.Windows.Forms.TextBox()
        Me.flowButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnRetry = New TreeYield.WinForms.ChipButton()
        Me.btnCopy = New TreeYield.WinForms.ChipButton()
        Me.btnExit = New TreeYield.WinForms.ChipButton()
        Me.layoutMain.SuspendLayout()
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.flowButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        ' layoutMain
        '
        Me.layoutMain.ColumnCount = 2
        Me.layoutMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.layoutMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutMain.Controls.Add(Me.picIcon, 0, 0)
        Me.layoutMain.Controls.Add(Me.lblTitle, 1, 0)
        Me.layoutMain.Controls.Add(Me.lblMessage, 1, 1)
        Me.layoutMain.Controls.Add(Me.txtDetails, 0, 2)
        Me.layoutMain.Controls.Add(Me.flowButtons, 0, 3)
        Me.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutMain.Location = New System.Drawing.Point(0, 0)
        Me.layoutMain.Name = "layoutMain"
        Me.layoutMain.Padding = New System.Windows.Forms.Padding(24, 20, 24, 18)
        Me.layoutMain.RowCount = 4
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.SetColumnSpan(Me.txtDetails, 2)
        Me.layoutMain.SetColumnSpan(Me.flowButtons, 2)
        Me.layoutMain.Size = New System.Drawing.Size(624, 461)
        Me.layoutMain.TabIndex = 0
        '
        ' picIcon
        '
        Me.picIcon.Location = New System.Drawing.Point(24, 20)
        Me.picIcon.Margin = New System.Windows.Forms.Padding(0, 2, 14, 0)
        Me.picIcon.Name = "picIcon"
        Me.picIcon.Size = New System.Drawing.Size(32, 32)
        Me.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picIcon.TabIndex = 0
        Me.picIcon.TabStop = False
        '
        ' lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(70, 20)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(272, 25)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "The database could not be opened"
        '
        ' lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Location = New System.Drawing.Point(70, 49)
        Me.lblMessage.Margin = New System.Windows.Forms.Padding(0, 4, 0, 14)
        Me.lblMessage.MaximumSize = New System.Drawing.Size(530, 0)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(100, 17)
        Me.lblMessage.TabIndex = 2
        Me.lblMessage.Text = "Message"
        Me.lblMessage.UseMnemonic = False
        '
        ' txtDetails
        '
        Me.txtDetails.BackColor = System.Drawing.Color.FromArgb(247, 247, 248)
        Me.txtDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDetails.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetails.Location = New System.Drawing.Point(24, 80)
        Me.txtDetails.Margin = New System.Windows.Forms.Padding(0, 0, 0, 14)
        Me.txtDetails.Multiline = True
        Me.txtDetails.Name = "txtDetails"
        Me.txtDetails.ReadOnly = True
        Me.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDetails.Size = New System.Drawing.Size(576, 313)
        Me.txtDetails.TabIndex = 3
        '
        ' flowButtons
        '
        Me.flowButtons.AutoSize = True
        Me.flowButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowButtons.Controls.Add(Me.btnRetry)
        Me.flowButtons.Controls.Add(Me.btnCopy)
        Me.flowButtons.Controls.Add(Me.btnExit)
        Me.flowButtons.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flowButtons.Location = New System.Drawing.Point(24, 407)
        Me.flowButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flowButtons.Name = "flowButtons"
        Me.flowButtons.Size = New System.Drawing.Size(576, 36)
        Me.flowButtons.TabIndex = 4
        Me.flowButtons.WrapContents = False
        '
        ' btnRetry
        '
        Me.btnRetry.AutoSize = True
        Me.btnRetry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnRetry.ChipStyle = TreeYield.WinForms.ChipStyle.Primary
        Me.btnRetry.DialogResult = System.Windows.Forms.DialogResult.Retry
        Me.btnRetry.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRetry.Location = New System.Drawing.Point(504, 0)
        Me.btnRetry.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnRetry.Name = "btnRetry"
        Me.btnRetry.Size = New System.Drawing.Size(72, 36)
        Me.btnRetry.TabIndex = 0
        Me.btnRetry.Text = "Retry"
        '
        ' btnCopy
        '
        Me.btnCopy.AutoSize = True
        Me.btnCopy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnCopy.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnCopy.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCopy.Location = New System.Drawing.Point(380, 0)
        Me.btnCopy.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(116, 36)
        Me.btnCopy.TabIndex = 1
        Me.btnCopy.Text = "Copy Details"
        '
        ' btnExit
        '
        Me.btnExit.AutoSize = True
        Me.btnExit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnExit.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnExit.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(300, 0)
        Me.btnExit.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(72, 36)
        Me.btnExit.TabIndex = 2
        Me.btnExit.Text = "Exit"
        '
        ' StartupErrorForm
        '
        Me.AcceptButton = Me.btnRetry
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btnExit
        Me.ClientSize = New System.Drawing.Size(624, 461)
        Me.Controls.Add(Me.layoutMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "StartupErrorForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Database Not Available"
        Me.layoutMain.ResumeLayout(False)
        Me.layoutMain.PerformLayout()
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.flowButtons.ResumeLayout(False)
        Me.flowButtons.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents layoutMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents picIcon As System.Windows.Forms.PictureBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents txtDetails As System.Windows.Forms.TextBox
    Friend WithEvents flowButtons As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnRetry As TreeYield.WinForms.ChipButton
    Friend WithEvents btnCopy As TreeYield.WinForms.ChipButton
    Friend WithEvents btnExit As TreeYield.WinForms.ChipButton
End Class
