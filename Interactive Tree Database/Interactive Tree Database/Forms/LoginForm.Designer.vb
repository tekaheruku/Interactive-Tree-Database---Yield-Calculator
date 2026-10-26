<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginForm
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
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.chkShowPassword = New System.Windows.Forms.CheckBox()
        Me.lblCapsLock = New System.Windows.Forms.Label()
        Me.lblError = New System.Windows.Forms.Label()
        Me.flowButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnSignIn = New TreeYield.WinForms.ChipButton()
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
        Me.layoutMain.Controls.Add(Me.lblUsername, 0, 2)
        Me.layoutMain.Controls.Add(Me.txtUsername, 0, 3)
        Me.layoutMain.Controls.Add(Me.lblPassword, 0, 4)
        Me.layoutMain.Controls.Add(Me.txtPassword, 0, 5)
        Me.layoutMain.Controls.Add(Me.chkShowPassword, 0, 6)
        Me.layoutMain.Controls.Add(Me.lblCapsLock, 0, 7)
        Me.layoutMain.Controls.Add(Me.lblError, 0, 8)
        Me.layoutMain.Controls.Add(Me.flowButtons, 0, 10)
        Me.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutMain.Location = New System.Drawing.Point(0, 0)
        Me.layoutMain.Name = "layoutMain"
        Me.layoutMain.Padding = New System.Windows.Forms.Padding(28, 22, 28, 18)
        Me.layoutMain.RowCount = 11
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
        Me.layoutMain.Size = New System.Drawing.Size(424, 381)
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
        Me.lblTitle.Size = New System.Drawing.Size(236, 30)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Administrator Sign In"
        '
        ' lblIntro
        '
        Me.lblIntro.AutoSize = True
        Me.lblIntro.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblIntro.Location = New System.Drawing.Point(28, 54)
        Me.lblIntro.Margin = New System.Windows.Forms.Padding(0, 2, 0, 16)
        Me.lblIntro.MaximumSize = New System.Drawing.Size(368, 0)
        Me.lblIntro.Name = "lblIntro"
        Me.lblIntro.Size = New System.Drawing.Size(330, 17)
        Me.lblIntro.TabIndex = 1
        Me.lblIntro.Text = "Sign in to add, edit or delete tree species records."
        '
        ' lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.lblUsername.Location = New System.Drawing.Point(28, 87)
        Me.lblUsername.Margin = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(68, 17)
        Me.lblUsername.TabIndex = 2
        Me.lblUsername.Text = "&Username"
        '
        ' txtUsername
        '
        Me.txtUsername.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUsername.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUsername.Location = New System.Drawing.Point(28, 108)
        Me.txtUsername.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.txtUsername.MaxLength = 50
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(368, 27)
        Me.txtUsername.TabIndex = 3
        '
        ' lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.lblPassword.Location = New System.Drawing.Point(28, 147)
        Me.lblPassword.Margin = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(64, 17)
        Me.lblPassword.TabIndex = 4
        Me.lblPassword.Text = "&Password"
        '
        ' txtPassword
        '
        Me.txtPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.Location = New System.Drawing.Point(28, 168)
        Me.txtPassword.Margin = New System.Windows.Forms.Padding(0, 0, 0, 6)
        Me.txtPassword.MaxLength = 128
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(368, 27)
        Me.txtPassword.TabIndex = 5
        Me.txtPassword.UseSystemPasswordChar = True
        '
        ' chkShowPassword
        '
        Me.chkShowPassword.AutoSize = True
        Me.chkShowPassword.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.chkShowPassword.Location = New System.Drawing.Point(28, 201)
        Me.chkShowPassword.Margin = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.chkShowPassword.Name = "chkShowPassword"
        Me.chkShowPassword.Size = New System.Drawing.Size(117, 21)
        Me.chkShowPassword.TabIndex = 6
        Me.chkShowPassword.Text = "&Show password"
        Me.chkShowPassword.UseVisualStyleBackColor = True
        '
        ' lblCapsLock
        '
        Me.lblCapsLock.AutoSize = True
        Me.lblCapsLock.ForeColor = System.Drawing.Color.FromArgb(176, 96, 0)
        Me.lblCapsLock.Location = New System.Drawing.Point(28, 226)
        Me.lblCapsLock.Margin = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.lblCapsLock.Name = "lblCapsLock"
        Me.lblCapsLock.Size = New System.Drawing.Size(242, 17)
        Me.lblCapsLock.TabIndex = 7
        Me.lblCapsLock.Text = "Caps Lock is on. Passwords are case-sensitive."
        Me.lblCapsLock.Visible = False
        '
        ' lblError
        '
        Me.lblError.AutoSize = True
        Me.lblError.ForeColor = System.Drawing.Color.FromArgb(197, 34, 31)
        Me.lblError.Location = New System.Drawing.Point(28, 251)
        Me.lblError.Margin = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.lblError.MaximumSize = New System.Drawing.Size(368, 0)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(0, 17)
        Me.lblError.TabIndex = 8
        '
        ' flowButtons
        '
        Me.flowButtons.AutoSize = True
        Me.flowButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowButtons.Controls.Add(Me.btnSignIn)
        Me.flowButtons.Controls.Add(Me.btnCancel)
        Me.flowButtons.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flowButtons.Location = New System.Drawing.Point(28, 377)
        Me.flowButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flowButtons.Name = "flowButtons"
        Me.flowButtons.Size = New System.Drawing.Size(368, 36)
        Me.flowButtons.TabIndex = 9
        Me.flowButtons.WrapContents = False
        '
        ' btnSignIn
        '
        Me.btnSignIn.AutoSize = True
        Me.btnSignIn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnSignIn.ChipStyle = TreeYield.WinForms.ChipStyle.Primary
        Me.btnSignIn.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSignIn.Location = New System.Drawing.Point(286, 0)
        Me.btnSignIn.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnSignIn.Name = "btnSignIn"
        Me.btnSignIn.Size = New System.Drawing.Size(82, 36)
        Me.btnSignIn.TabIndex = 0
        Me.btnSignIn.Text = "Sign In"
        '
        ' btnCancel
        '
        Me.btnCancel.AutoSize = True
        Me.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnCancel.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(198, 0)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 36)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        ' LoginForm
        '
        Me.AcceptButton = Me.btnSignIn
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(424, 381)
        Me.Controls.Add(Me.layoutMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Administrator Sign In"
        Me.layoutMain.ResumeLayout(False)
        Me.layoutMain.PerformLayout()
        Me.flowButtons.ResumeLayout(False)
        Me.flowButtons.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents layoutMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblIntro As System.Windows.Forms.Label
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents chkShowPassword As System.Windows.Forms.CheckBox
    Friend WithEvents lblCapsLock As System.Windows.Forms.Label
    Friend WithEvents lblError As System.Windows.Forms.Label
    Friend WithEvents flowButtons As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnSignIn As TreeYield.WinForms.ChipButton
    Friend WithEvents btnCancel As TreeYield.WinForms.ChipButton
End Class
