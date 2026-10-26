<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SpeciesDetailsForm
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
        Me.lblName = New System.Windows.Forms.Label()
        Me.lblScientific = New System.Windows.Forms.Label()
        Me.lblTags = New System.Windows.Forms.Label()
        Me.layoutDetails = New System.Windows.Forms.TableLayoutPanel()
        Me.lblYieldKey = New System.Windows.Forms.Label()
        Me.lblYieldValue = New System.Windows.Forms.Label()
        Me.lblPriceKey = New System.Windows.Forms.Label()
        Me.lblPriceValue = New System.Windows.Forms.Label()
        Me.lblHarvestTypeKey = New System.Windows.Forms.Label()
        Me.lblHarvestTypeValue = New System.Windows.Forms.Label()
        Me.lblFirstKey = New System.Windows.Forms.Label()
        Me.lblFirstValue = New System.Windows.Forms.Label()
        Me.lblFullKey = New System.Windows.Forms.Label()
        Me.lblFullValue = New System.Windows.Forms.Label()
        Me.lblSeasonKey = New System.Windows.Forms.Label()
        Me.lblSeasonValue = New System.Windows.Forms.Label()
        Me.lblExampleKey = New System.Windows.Forms.Label()
        Me.lblExampleValue = New System.Windows.Forms.Label()
        Me.lblUpdatedKey = New System.Windows.Forms.Label()
        Me.lblUpdatedValue = New System.Windows.Forms.Label()
        Me.lblNotesTitle = New System.Windows.Forms.Label()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.flowButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnUse = New TreeYield.WinForms.ChipButton()
        Me.btnClose = New TreeYield.WinForms.ChipButton()
        Me.layoutMain.SuspendLayout()
        Me.layoutDetails.SuspendLayout()
        Me.flowButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        ' layoutMain
        '
        Me.layoutMain.ColumnCount = 1
        Me.layoutMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutMain.Controls.Add(Me.lblName, 0, 0)
        Me.layoutMain.Controls.Add(Me.lblScientific, 0, 1)
        Me.layoutMain.Controls.Add(Me.lblTags, 0, 2)
        Me.layoutMain.Controls.Add(Me.layoutDetails, 0, 3)
        Me.layoutMain.Controls.Add(Me.lblNotesTitle, 0, 4)
        Me.layoutMain.Controls.Add(Me.txtNotes, 0, 5)
        Me.layoutMain.Controls.Add(Me.flowButtons, 0, 7)
        Me.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutMain.Location = New System.Drawing.Point(0, 0)
        Me.layoutMain.Name = "layoutMain"
        Me.layoutMain.Padding = New System.Windows.Forms.Padding(28, 22, 28, 18)
        Me.layoutMain.RowCount = 8
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.Size = New System.Drawing.Size(584, 581)
        Me.layoutMain.TabIndex = 0
        '
        ' lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.Location = New System.Drawing.Point(28, 22)
        Me.lblName.Margin = New System.Windows.Forms.Padding(0)
        Me.lblName.MaximumSize = New System.Drawing.Size(528, 0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(163, 32)
        Me.lblName.TabIndex = 0
        Me.lblName.Text = "Species name"
        Me.lblName.UseMnemonic = False
        '
        ' lblScientific
        '
        Me.lblScientific.AutoSize = True
        Me.lblScientific.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScientific.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblScientific.Location = New System.Drawing.Point(28, 54)
        Me.lblScientific.Margin = New System.Windows.Forms.Padding(1, 0, 0, 2)
        Me.lblScientific.Name = "lblScientific"
        Me.lblScientific.Size = New System.Drawing.Size(113, 19)
        Me.lblScientific.TabIndex = 1
        Me.lblScientific.Text = "Scientific name"
        Me.lblScientific.UseMnemonic = False
        '
        ' lblTags
        '
        Me.lblTags.AutoSize = True
        Me.lblTags.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTags.ForeColor = System.Drawing.Color.FromArgb(21, 87, 176)
        Me.lblTags.Location = New System.Drawing.Point(28, 75)
        Me.lblTags.Margin = New System.Windows.Forms.Padding(1, 4, 0, 16)
        Me.lblTags.Name = "lblTags"
        Me.lblTags.Size = New System.Drawing.Size(155, 17)
        Me.lblTags.TabIndex = 2
        Me.lblTags.Text = "Category  -  Product"
        Me.lblTags.UseMnemonic = False
        '
        ' layoutDetails
        '
        Me.layoutDetails.AutoSize = True
        Me.layoutDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutDetails.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.None
        Me.layoutDetails.ColumnCount = 2
        Me.layoutDetails.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.0!))
        Me.layoutDetails.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.0!))
        Me.layoutDetails.Controls.Add(Me.lblYieldKey, 0, 0)
        Me.layoutDetails.Controls.Add(Me.lblYieldValue, 1, 0)
        Me.layoutDetails.Controls.Add(Me.lblPriceKey, 0, 1)
        Me.layoutDetails.Controls.Add(Me.lblPriceValue, 1, 1)
        Me.layoutDetails.Controls.Add(Me.lblHarvestTypeKey, 0, 2)
        Me.layoutDetails.Controls.Add(Me.lblHarvestTypeValue, 1, 2)
        Me.layoutDetails.Controls.Add(Me.lblFirstKey, 0, 3)
        Me.layoutDetails.Controls.Add(Me.lblFirstValue, 1, 3)
        Me.layoutDetails.Controls.Add(Me.lblFullKey, 0, 4)
        Me.layoutDetails.Controls.Add(Me.lblFullValue, 1, 4)
        Me.layoutDetails.Controls.Add(Me.lblSeasonKey, 0, 5)
        Me.layoutDetails.Controls.Add(Me.lblSeasonValue, 1, 5)
        Me.layoutDetails.Controls.Add(Me.lblExampleKey, 0, 6)
        Me.layoutDetails.Controls.Add(Me.lblExampleValue, 1, 6)
        Me.layoutDetails.Controls.Add(Me.lblUpdatedKey, 0, 7)
        Me.layoutDetails.Controls.Add(Me.lblUpdatedValue, 1, 7)
        Me.layoutDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutDetails.Location = New System.Drawing.Point(28, 108)
        Me.layoutDetails.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.layoutDetails.Name = "layoutDetails"
        Me.layoutDetails.RowCount = 8
        Me.layoutDetails.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutDetails.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutDetails.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutDetails.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutDetails.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutDetails.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutDetails.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutDetails.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutDetails.Size = New System.Drawing.Size(528, 248)
        Me.layoutDetails.TabIndex = 3
        '
        ' lblYieldKey
        '
        Me.lblYieldKey.AutoSize = True
        Me.lblYieldKey.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblYieldKey.Location = New System.Drawing.Point(0, 0)
        Me.lblYieldKey.Margin = New System.Windows.Forms.Padding(0, 0, 8, 12)
        Me.lblYieldKey.Name = "lblYieldKey"
        Me.lblYieldKey.Size = New System.Drawing.Size(150, 17)
        Me.lblYieldKey.TabIndex = 0
        Me.lblYieldKey.Text = "Yield per tree per year"
        '
        ' lblYieldValue
        '
        Me.lblYieldValue.AutoSize = True
        Me.lblYieldValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYieldValue.Location = New System.Drawing.Point(200, 0)
        Me.lblYieldValue.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblYieldValue.MaximumSize = New System.Drawing.Size(328, 0)
        Me.lblYieldValue.Name = "lblYieldValue"
        Me.lblYieldValue.Size = New System.Drawing.Size(12, 17)
        Me.lblYieldValue.TabIndex = 1
        Me.lblYieldValue.Text = "-"
        Me.lblYieldValue.UseMnemonic = False
        '
        ' lblPriceKey
        '
        Me.lblPriceKey.AutoSize = True
        Me.lblPriceKey.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblPriceKey.Location = New System.Drawing.Point(0, 29)
        Me.lblPriceKey.Margin = New System.Windows.Forms.Padding(0, 0, 8, 12)
        Me.lblPriceKey.Name = "lblPriceKey"
        Me.lblPriceKey.Size = New System.Drawing.Size(116, 17)
        Me.lblPriceKey.TabIndex = 2
        Me.lblPriceKey.Text = "Farm-gate price"
        '
        ' lblPriceValue
        '
        Me.lblPriceValue.AutoSize = True
        Me.lblPriceValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPriceValue.Location = New System.Drawing.Point(200, 29)
        Me.lblPriceValue.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblPriceValue.MaximumSize = New System.Drawing.Size(328, 0)
        Me.lblPriceValue.Name = "lblPriceValue"
        Me.lblPriceValue.Size = New System.Drawing.Size(12, 17)
        Me.lblPriceValue.TabIndex = 3
        Me.lblPriceValue.Text = "-"
        Me.lblPriceValue.UseMnemonic = False
        '
        ' lblHarvestTypeKey
        '
        Me.lblHarvestTypeKey.AutoSize = True
        Me.lblHarvestTypeKey.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblHarvestTypeKey.Location = New System.Drawing.Point(0, 58)
        Me.lblHarvestTypeKey.Margin = New System.Windows.Forms.Padding(0, 0, 8, 12)
        Me.lblHarvestTypeKey.Name = "lblHarvestTypeKey"
        Me.lblHarvestTypeKey.Size = New System.Drawing.Size(88, 17)
        Me.lblHarvestTypeKey.TabIndex = 4
        Me.lblHarvestTypeKey.Text = "Harvest type"
        '
        ' lblHarvestTypeValue
        '
        Me.lblHarvestTypeValue.AutoSize = True
        Me.lblHarvestTypeValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHarvestTypeValue.Location = New System.Drawing.Point(200, 58)
        Me.lblHarvestTypeValue.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblHarvestTypeValue.MaximumSize = New System.Drawing.Size(328, 0)
        Me.lblHarvestTypeValue.Name = "lblHarvestTypeValue"
        Me.lblHarvestTypeValue.Size = New System.Drawing.Size(12, 17)
        Me.lblHarvestTypeValue.TabIndex = 5
        Me.lblHarvestTypeValue.Text = "-"
        Me.lblHarvestTypeValue.UseMnemonic = False
        '
        ' lblFirstKey
        '
        Me.lblFirstKey.AutoSize = True
        Me.lblFirstKey.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblFirstKey.Location = New System.Drawing.Point(0, 87)
        Me.lblFirstKey.Margin = New System.Windows.Forms.Padding(0, 0, 8, 12)
        Me.lblFirstKey.Name = "lblFirstKey"
        Me.lblFirstKey.Size = New System.Drawing.Size(88, 17)
        Me.lblFirstKey.TabIndex = 6
        Me.lblFirstKey.Text = "First harvest"
        '
        ' lblFirstValue
        '
        Me.lblFirstValue.AutoSize = True
        Me.lblFirstValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFirstValue.Location = New System.Drawing.Point(200, 87)
        Me.lblFirstValue.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblFirstValue.MaximumSize = New System.Drawing.Size(328, 0)
        Me.lblFirstValue.Name = "lblFirstValue"
        Me.lblFirstValue.Size = New System.Drawing.Size(12, 17)
        Me.lblFirstValue.TabIndex = 7
        Me.lblFirstValue.Text = "-"
        Me.lblFirstValue.UseMnemonic = False
        '
        ' lblFullKey
        '
        Me.lblFullKey.AutoSize = True
        Me.lblFullKey.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblFullKey.Location = New System.Drawing.Point(0, 116)
        Me.lblFullKey.Margin = New System.Windows.Forms.Padding(0, 0, 8, 12)
        Me.lblFullKey.Name = "lblFullKey"
        Me.lblFullKey.Size = New System.Drawing.Size(104, 17)
        Me.lblFullKey.TabIndex = 8
        Me.lblFullKey.Text = "Full production"
        '
        ' lblFullValue
        '
        Me.lblFullValue.AutoSize = True
        Me.lblFullValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFullValue.Location = New System.Drawing.Point(200, 116)
        Me.lblFullValue.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblFullValue.MaximumSize = New System.Drawing.Size(328, 0)
        Me.lblFullValue.Name = "lblFullValue"
        Me.lblFullValue.Size = New System.Drawing.Size(12, 17)
        Me.lblFullValue.TabIndex = 9
        Me.lblFullValue.Text = "-"
        Me.lblFullValue.UseMnemonic = False
        '
        ' lblSeasonKey
        '
        Me.lblSeasonKey.AutoSize = True
        Me.lblSeasonKey.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblSeasonKey.Location = New System.Drawing.Point(0, 145)
        Me.lblSeasonKey.Margin = New System.Windows.Forms.Padding(0, 0, 8, 12)
        Me.lblSeasonKey.Name = "lblSeasonKey"
        Me.lblSeasonKey.Size = New System.Drawing.Size(100, 17)
        Me.lblSeasonKey.TabIndex = 10
        Me.lblSeasonKey.Text = "Harvest season"
        '
        ' lblSeasonValue
        '
        Me.lblSeasonValue.AutoSize = True
        Me.lblSeasonValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSeasonValue.Location = New System.Drawing.Point(200, 145)
        Me.lblSeasonValue.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblSeasonValue.MaximumSize = New System.Drawing.Size(328, 0)
        Me.lblSeasonValue.Name = "lblSeasonValue"
        Me.lblSeasonValue.Size = New System.Drawing.Size(12, 17)
        Me.lblSeasonValue.TabIndex = 11
        Me.lblSeasonValue.Text = "-"
        Me.lblSeasonValue.UseMnemonic = False
        '
        ' lblExampleKey
        '
        Me.lblExampleKey.AutoSize = True
        Me.lblExampleKey.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblExampleKey.Location = New System.Drawing.Point(0, 174)
        Me.lblExampleKey.Margin = New System.Windows.Forms.Padding(0, 0, 8, 12)
        Me.lblExampleKey.Name = "lblExampleKey"
        Me.lblExampleKey.Size = New System.Drawing.Size(80, 17)
        Me.lblExampleKey.TabIndex = 12
        Me.lblExampleKey.Text = "For N trees"
        '
        ' lblExampleValue
        '
        Me.lblExampleValue.AutoSize = True
        Me.lblExampleValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExampleValue.ForeColor = System.Drawing.Color.FromArgb(11, 128, 67)
        Me.lblExampleValue.Location = New System.Drawing.Point(200, 174)
        Me.lblExampleValue.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblExampleValue.MaximumSize = New System.Drawing.Size(328, 0)
        Me.lblExampleValue.Name = "lblExampleValue"
        Me.lblExampleValue.Size = New System.Drawing.Size(12, 17)
        Me.lblExampleValue.TabIndex = 13
        Me.lblExampleValue.Text = "-"
        Me.lblExampleValue.UseMnemonic = False
        '
        ' lblUpdatedKey
        '
        Me.lblUpdatedKey.AutoSize = True
        Me.lblUpdatedKey.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblUpdatedKey.Location = New System.Drawing.Point(0, 203)
        Me.lblUpdatedKey.Margin = New System.Windows.Forms.Padding(0, 0, 8, 12)
        Me.lblUpdatedKey.Name = "lblUpdatedKey"
        Me.lblUpdatedKey.Size = New System.Drawing.Size(88, 17)
        Me.lblUpdatedKey.TabIndex = 14
        Me.lblUpdatedKey.Text = "Last updated"
        '
        ' lblUpdatedValue
        '
        Me.lblUpdatedValue.AutoSize = True
        Me.lblUpdatedValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUpdatedValue.Location = New System.Drawing.Point(200, 203)
        Me.lblUpdatedValue.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblUpdatedValue.MaximumSize = New System.Drawing.Size(328, 0)
        Me.lblUpdatedValue.Name = "lblUpdatedValue"
        Me.lblUpdatedValue.Size = New System.Drawing.Size(12, 17)
        Me.lblUpdatedValue.TabIndex = 15
        Me.lblUpdatedValue.Text = "-"
        Me.lblUpdatedValue.UseMnemonic = False
        '
        ' lblNotesTitle
        '
        Me.lblNotesTitle.AutoSize = True
        Me.lblNotesTitle.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblNotesTitle.Location = New System.Drawing.Point(28, 368)
        Me.lblNotesTitle.Margin = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.lblNotesTitle.Name = "lblNotesTitle"
        Me.lblNotesTitle.Size = New System.Drawing.Size(43, 17)
        Me.lblNotesTitle.TabIndex = 4
        Me.lblNotesTitle.Text = "Notes"
        '
        ' txtNotes
        '
        Me.txtNotes.AccessibleName = "Notes"
        Me.txtNotes.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNotes.BackColor = System.Drawing.Color.FromArgb(247, 247, 248)
        Me.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtNotes.Location = New System.Drawing.Point(28, 389)
        Me.txtNotes.Margin = New System.Windows.Forms.Padding(0)
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.ReadOnly = True
        Me.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtNotes.Size = New System.Drawing.Size(528, 96)
        Me.txtNotes.TabIndex = 5
        Me.txtNotes.TabStop = False
        '
        ' flowButtons
        '
        Me.flowButtons.AutoSize = True
        Me.flowButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowButtons.Controls.Add(Me.btnUse)
        Me.flowButtons.Controls.Add(Me.btnClose)
        Me.flowButtons.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flowButtons.Location = New System.Drawing.Point(28, 567)
        Me.flowButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flowButtons.Name = "flowButtons"
        Me.flowButtons.Size = New System.Drawing.Size(528, 36)
        Me.flowButtons.TabIndex = 6
        Me.flowButtons.WrapContents = False
        '
        ' btnUse
        '
        Me.btnUse.AutoSize = True
        Me.btnUse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnUse.ChipStyle = TreeYield.WinForms.ChipStyle.Primary
        Me.btnUse.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnUse.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUse.Location = New System.Drawing.Point(380, 0)
        Me.btnUse.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnUse.Name = "btnUse"
        Me.btnUse.Size = New System.Drawing.Size(148, 36)
        Me.btnUse.TabIndex = 0
        Me.btnUse.Text = "Use in Calculator"
        '
        ' btnClose
        '
        Me.btnClose.AutoSize = True
        Me.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnClose.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(300, 0)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(72, 36)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        '
        ' SpeciesDetailsForm
        '
        Me.AcceptButton = Me.btnUse
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(584, 581)
        Me.Controls.Add(Me.layoutMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SpeciesDetailsForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Species Details"
        Me.layoutMain.ResumeLayout(False)
        Me.layoutMain.PerformLayout()
        Me.layoutDetails.ResumeLayout(False)
        Me.layoutDetails.PerformLayout()
        Me.flowButtons.ResumeLayout(False)
        Me.flowButtons.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents layoutMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents lblScientific As System.Windows.Forms.Label
    Friend WithEvents lblTags As System.Windows.Forms.Label
    Friend WithEvents layoutDetails As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblYieldKey As System.Windows.Forms.Label
    Friend WithEvents lblYieldValue As System.Windows.Forms.Label
    Friend WithEvents lblPriceKey As System.Windows.Forms.Label
    Friend WithEvents lblPriceValue As System.Windows.Forms.Label
    Friend WithEvents lblHarvestTypeKey As System.Windows.Forms.Label
    Friend WithEvents lblHarvestTypeValue As System.Windows.Forms.Label
    Friend WithEvents lblFirstKey As System.Windows.Forms.Label
    Friend WithEvents lblFirstValue As System.Windows.Forms.Label
    Friend WithEvents lblFullKey As System.Windows.Forms.Label
    Friend WithEvents lblFullValue As System.Windows.Forms.Label
    Friend WithEvents lblSeasonKey As System.Windows.Forms.Label
    Friend WithEvents lblSeasonValue As System.Windows.Forms.Label
    Friend WithEvents lblExampleKey As System.Windows.Forms.Label
    Friend WithEvents lblExampleValue As System.Windows.Forms.Label
    Friend WithEvents lblUpdatedKey As System.Windows.Forms.Label
    Friend WithEvents lblUpdatedValue As System.Windows.Forms.Label
    Friend WithEvents lblNotesTitle As System.Windows.Forms.Label
    Friend WithEvents txtNotes As System.Windows.Forms.TextBox
    Friend WithEvents flowButtons As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnUse As TreeYield.WinForms.ChipButton
    Friend WithEvents btnClose As TreeYield.WinForms.ChipButton
End Class
