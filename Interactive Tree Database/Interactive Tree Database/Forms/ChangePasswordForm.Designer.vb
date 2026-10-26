<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ChangePasswordForm
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
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblIntro = New System.Windows.Forms.Label()
        Me.lblCurrent = New System.Windows.Forms.Label()
        Me.txtCurrent = New System.Windows.Forms.TextBox()
        Me.lblNew = New System.Windows.Forms.Label()
        Me.txtNew = New System.Windows.Forms.TextBox()
        Me.lblPolicy = New System.Windows.Forms.Label()
        Me.lblConfirm = New System.Windows.Forms.Label()
        Me.txtConfirm = New System.Windows.Forms.TextBox()
        Me.lblError = New System.Windows.Forms.Label()
        Me.flowButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnSave = New TreeYield.WinForms.ChipButton()
        Me.btnCancel = New TreeYield.WinForms.ChipButton()
        Me.layoutMain.SuspendLayout()
        Me.flowButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        ' layoutMain
        '
        Me.layoutMain.ColumnCount = 1
        Me.layoutMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutMain.Controls.Add(Me.lblTitle, 0, 0)
        Me.layoutMain.Controls.Add(Me.lblIntro, 0, 1)
        Me.layoutMain.Controls.Add(Me.lblCurrent, 0, 2)
        Me.layoutMain.Controls.Add(Me.txtCurrent, 0, 3)
        Me.layoutMain.Controls.Add(Me.lblNew, 0, 4)
        Me.layoutMain.Controls.Add(Me.txtNew, 0, 5)
        Me.layoutMain.Controls.Add(Me.lblPolicy, 0, 6)
        Me.layoutMain.Controls.Add(Me.lblConfirm, 0, 7)
        Me.layoutMain.Controls.Add(Me.txtConfirm, 0, 8)
        Me.layoutMain.Controls.Add(Me.lblError, 0, 9)
        Me.layoutMain.Controls.Add(Me.flowButtons, 0, 11)
        Me.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutMain.Location = New System.Drawing.Point(0, 0)
        Me.layoutMain.Name = "layoutMain"
        Me.layoutMain.Padding = New System.Windows.Forms.Padding(28, 22, 28, 18)
        Me.layoutMain.RowCount = 12
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.Size = New System.Drawing.Size(444, 451)
        Me.layoutMain.TabIndex = 0
        '
        ' lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.lblTitle.Location = New System.Drawing.Point(28, 22)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(181, 30)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Change Password"
        '
        ' lblIntro
        '
        Me.lblIntro.AutoSize = True
        Me.lblIntro.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblIntro.Location = New System.Drawing.Point(28, 54)
        Me.lblIntro.Margin = New System.Windows.Forms.Padding(0, 2, 0, 16)
        Me.lblIntro.MaximumSize = New System.Drawing.Size(388, 0)
        Me.lblIntro.Name = "lblIntro"
        Me.lblIntro.Size = New System.Drawing.Size(320, 17)
        Me.lblIntro.TabIndex = 1
        Me.lblIntro.Text = "Enter your current password and choose a new one."
        '
        ' lblCurrent
        '
        Me.lblCurrent.AutoSize = True
        Me.lblCurrent.Location = New System.Drawing.Point(28, 87)
        Me.lblCurrent.Margin = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.lblCurrent.Name = "lblCurrent"
        Me.lblCurrent.Size = New System.Drawing.Size(113, 17)
        Me.lblCurrent.TabIndex = 2
        Me.lblCurrent.Text = "&Current password"
        '
        ' txtCurrent
        '
        Me.txtCurrent.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCurrent.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrent.Location = New System.Drawing.Point(28, 108)
        Me.txtCurrent.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.txtCurrent.MaxLength = 128
        Me.txtCurrent.Name = "txtCurrent"
        Me.txtCurrent.Size = New System.Drawing.Size(388, 27)
        Me.txtCurrent.TabIndex = 3
        Me.txtCurrent.UseSystemPasswordChar = True
        '
        ' lblNew
        '
        Me.lblNew.AutoSize = True
        Me.lblNew.Location = New System.Drawing.Point(28, 147)
        Me.lblNew.Margin = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.lblNew.Name = "lblNew"
        Me.lblNew.Size = New System.Drawing.Size(96, 17)
        Me.lblNew.TabIndex = 4
        Me.lblNew.Text = "&New password"
        '
        ' txtNew
        '
        Me.txtNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNew.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNew.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNew.Location = New System.Drawing.Point(28, 168)
        Me.txtNew.Margin = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.txtNew.MaxLength = 128
        Me.txtNew.Name = "txtNew"
        Me.txtNew.Size = New System.Drawing.Size(388, 27)
        Me.txtNew.TabIndex = 5
        Me.txtNew.UseSystemPasswordChar = True
        '
        ' lblPolicy
        '
        Me.lblPolicy.AutoSize = True
        Me.lblPolicy.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicy.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblPolicy.Location = New System.Drawing.Point(28, 199)
        Me.lblPolicy.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblPolicy.MaximumSize = New System.Drawing.Size(388, 0)
        Me.lblPolicy.Name = "lblPolicy"
        Me.lblPolicy.Size = New System.Drawing.Size(300, 13)
        Me.lblPolicy.TabIndex = 6
        Me.lblPolicy.Text = "Password rules"
        '
        ' lblConfirm
        '
        Me.lblConfirm.AutoSize = True
        Me.lblConfirm.Location = New System.Drawing.Point(28, 224)
        Me.lblConfirm.Margin = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.lblConfirm.Name = "lblConfirm"
        Me.lblConfirm.Size = New System.Drawing.Size(150, 17)
        Me.lblConfirm.TabIndex = 7
        Me.lblConfirm.Text = "C&onfirm new password"
        '
        ' txtConfirm
        '
        Me.txtConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtConfirm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtConfirm.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConfirm.Location = New System.Drawing.Point(28, 245)
        Me.txtConfirm.Margin = New System.Windows.Forms.Padding(0, 0, 0, 6)
        Me.txtConfirm.MaxLength = 128
        Me.txtConfirm.Name = "txtConfirm"
        Me.txtConfirm.Size = New System.Drawing.Size(388, 27)
        Me.txtConfirm.TabIndex = 8
        Me.txtConfirm.UseSystemPasswordChar = True
        '
        ' lblError
        '
        Me.lblError.AutoSize = True
        Me.lblError.ForeColor = System.Drawing.Color.FromArgb(197, 34, 31)
        Me.lblError.Location = New System.Drawing.Point(28, 282)
        Me.lblError.Margin = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.lblError.MaximumSize = New System.Drawing.Size(388, 0)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(0, 17)
        Me.lblError.TabIndex = 9
        '
        ' flowButtons
        '
        Me.flowButtons.AutoSize = True
        Me.flowButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowButtons.Controls.Add(Me.btnSave)
        Me.flowButtons.Controls.Add(Me.btnCancel)
        Me.flowButtons.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flowButtons.Location = New System.Drawing.Point(28, 437)
        Me.flowButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flowButtons.Name = "flowButtons"
        Me.flowButtons.Size = New System.Drawing.Size(388, 36)
        Me.flowButtons.TabIndex = 10
        Me.flowButtons.WrapContents = False
        '
        ' btnSave
        '
        Me.btnSave.AutoSize = True
        Me.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnSave.ChipStyle = TreeYield.WinForms.ChipStyle.Primary
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(240, 0)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(148, 36)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Change Password"
        '
        ' btnCancel
        '
        Me.btnCancel.AutoSize = True
        Me.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnCancel.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(152, 0)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 36)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        ' ChangePasswordForm
        '
        Me.AcceptButton = Me.btnSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(444, 451)
        Me.Controls.Add(Me.layoutMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ChangePasswordForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Change Password"
        Me.layoutMain.ResumeLayout(False)
        Me.layoutMain.PerformLayout()
        Me.flowButtons.ResumeLayout(False)
        Me.flowButtons.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents layoutMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblIntro As System.Windows.Forms.Label
    Friend WithEvents lblCurrent As System.Windows.Forms.Label
    Friend WithEvents txtCurrent As System.Windows.Forms.TextBox
    Friend WithEvents lblNew As System.Windows.Forms.Label
    Friend WithEvents txtNew As System.Windows.Forms.TextBox
    Friend WithEvents lblPolicy As System.Windows.Forms.Label
    Friend WithEvents lblConfirm As System.Windows.Forms.Label
    Friend WithEvents txtConfirm As System.Windows.Forms.TextBox
    Friend WithEvents lblError As System.Windows.Forms.Label
    Friend WithEvents flowButtons As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnSave As TreeYield.WinForms.ChipButton
    Friend WithEvents btnCancel As TreeYield.WinForms.ChipButton
End Class
