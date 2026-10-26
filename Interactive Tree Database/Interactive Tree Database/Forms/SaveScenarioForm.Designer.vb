<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SaveScenarioForm
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
        Me.components = New System.ComponentModel.Container()
        Me.layoutMain = New System.Windows.Forms.TableLayoutPanel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblSummary = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblNotes = New System.Windows.Forms.Label()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.lblRequired = New System.Windows.Forms.Label()
        Me.flowButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnSave = New TreeYield.WinForms.ChipButton()
        Me.btnCancel = New TreeYield.WinForms.ChipButton()
        Me.errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.layoutMain.SuspendLayout()
        Me.flowButtons.SuspendLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' layoutMain
        '
        Me.layoutMain.ColumnCount = 1
        Me.layoutMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutMain.Controls.Add(Me.lblTitle, 0, 0)
        Me.layoutMain.Controls.Add(Me.lblSummary, 0, 1)
        Me.layoutMain.Controls.Add(Me.lblName, 0, 2)
        Me.layoutMain.Controls.Add(Me.txtName, 0, 3)
        Me.layoutMain.Controls.Add(Me.lblNotes, 0, 4)
        Me.layoutMain.Controls.Add(Me.txtNotes, 0, 5)
        Me.layoutMain.Controls.Add(Me.lblRequired, 0, 6)
        Me.layoutMain.Controls.Add(Me.flowButtons, 0, 8)
        Me.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutMain.Location = New System.Drawing.Point(0, 0)
        Me.layoutMain.Name = "layoutMain"
        Me.layoutMain.Padding = New System.Windows.Forms.Padding(28, 22, 28, 18)
        Me.layoutMain.RowCount = 9
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.Size = New System.Drawing.Size(484, 421)
        Me.layoutMain.TabIndex = 0
        '
        ' lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(28, 22)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(143, 30)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Save Scenario"
        '
        ' lblSummary
        '
        Me.lblSummary.AutoSize = True
        Me.lblSummary.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblSummary.Location = New System.Drawing.Point(28, 54)
        Me.lblSummary.Margin = New System.Windows.Forms.Padding(0, 2, 0, 16)
        Me.lblSummary.MaximumSize = New System.Drawing.Size(428, 0)
        Me.lblSummary.Name = "lblSummary"
        Me.lblSummary.Size = New System.Drawing.Size(120, 17)
        Me.lblSummary.TabIndex = 1
        Me.lblSummary.Text = "Scenario summary"
        Me.lblSummary.UseMnemonic = False
        '
        ' lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(28, 87)
        Me.lblName.Margin = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(108, 17)
        Me.lblName.TabIndex = 2
        Me.lblName.Text = "Scenario &name *"
        '
        ' txtName
        '
        Me.txtName.AccessibleName = "Scenario name (required)"
        Me.txtName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtName.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.errorProvider.SetIconPadding(Me.txtName, -22)
        Me.txtName.Location = New System.Drawing.Point(28, 108)
        Me.txtName.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(428, 27)
        Me.txtName.TabIndex = 3
        '
        ' lblNotes
        '
        Me.lblNotes.AutoSize = True
        Me.lblNotes.Location = New System.Drawing.Point(28, 147)
        Me.lblNotes.Margin = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.lblNotes.Name = "lblNotes"
        Me.lblNotes.Size = New System.Drawing.Size(105, 17)
        Me.lblNotes.TabIndex = 4
        Me.lblNotes.Text = "N&otes (optional)"
        '
        ' txtNotes
        '
        Me.txtNotes.AccessibleName = "Notes (optional)"
        Me.txtNotes.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.errorProvider.SetIconPadding(Me.txtNotes, -22)
        Me.txtNotes.Location = New System.Drawing.Point(28, 168)
        Me.txtNotes.Margin = New System.Windows.Forms.Padding(0, 0, 0, 6)
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtNotes.Size = New System.Drawing.Size(428, 90)
        Me.txtNotes.TabIndex = 5
        '
        ' lblRequired
        '
        Me.lblRequired.AutoSize = True
        Me.lblRequired.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRequired.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblRequired.Location = New System.Drawing.Point(28, 264)
        Me.lblRequired.Margin = New System.Windows.Forms.Padding(0)
        Me.lblRequired.Name = "lblRequired"
        Me.lblRequired.Size = New System.Drawing.Size(320, 13)
        Me.lblRequired.TabIndex = 6
        Me.lblRequired.Text = "* required. Results are recalculated with current prices when loaded."
        '
        ' flowButtons
        '
        Me.flowButtons.AutoSize = True
        Me.flowButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowButtons.Controls.Add(Me.btnSave)
        Me.flowButtons.Controls.Add(Me.btnCancel)
        Me.flowButtons.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flowButtons.Location = New System.Drawing.Point(28, 367)
        Me.flowButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flowButtons.Name = "flowButtons"
        Me.flowButtons.Size = New System.Drawing.Size(428, 36)
        Me.flowButtons.TabIndex = 7
        Me.flowButtons.WrapContents = False
        '
        ' btnSave
        '
        Me.btnSave.AutoSize = True
        Me.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnSave.ChipStyle = TreeYield.WinForms.ChipStyle.Primary
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(312, 0)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(116, 36)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Save Scenario"
        '
        ' btnCancel
        '
        Me.btnCancel.AutoSize = True
        Me.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnCancel.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(224, 0)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 36)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        ' errorProvider
        '
        Me.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.errorProvider.ContainerControl = Me
        '
        ' SaveScenarioForm
        '
        Me.AcceptButton = Me.btnSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(484, 421)
        Me.Controls.Add(Me.layoutMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SaveScenarioForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Save Scenario"
        Me.layoutMain.ResumeLayout(False)
        Me.layoutMain.PerformLayout()
        Me.flowButtons.ResumeLayout(False)
        Me.flowButtons.PerformLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents layoutMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblSummary As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents lblNotes As System.Windows.Forms.Label
    Friend WithEvents txtNotes As System.Windows.Forms.TextBox
    Friend WithEvents lblRequired As System.Windows.Forms.Label
    Friend WithEvents flowButtons As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnSave As TreeYield.WinForms.ChipButton
    Friend WithEvents btnCancel As TreeYield.WinForms.ChipButton
    Friend WithEvents errorProvider As System.Windows.Forms.ErrorProvider
End Class
