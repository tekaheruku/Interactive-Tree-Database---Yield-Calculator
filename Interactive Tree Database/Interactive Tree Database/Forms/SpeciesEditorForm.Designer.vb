<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SpeciesEditorForm
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
        Me.lblIntro = New System.Windows.Forms.Label()
        Me.layoutFields = New System.Windows.Forms.TableLayoutPanel()
        Me.lblCommonName = New System.Windows.Forms.Label()
        Me.txtCommonName = New System.Windows.Forms.TextBox()
        Me.lblScientificName = New System.Windows.Forms.Label()
        Me.txtScientificName = New System.Windows.Forms.TextBox()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.cboCategory = New System.Windows.Forms.ComboBox()
        Me.lblProductType = New System.Windows.Forms.Label()
        Me.cboProductType = New System.Windows.Forms.ComboBox()
        Me.lblHarvestType = New System.Windows.Forms.Label()
        Me.cboHarvestType = New System.Windows.Forms.ComboBox()
        Me.lblYieldUnit = New System.Windows.Forms.Label()
        Me.cboYieldUnit = New System.Windows.Forms.ComboBox()
        Me.lblYield = New System.Windows.Forms.Label()
        Me.nudYield = New System.Windows.Forms.NumericUpDown()
        Me.lblPrice = New System.Windows.Forms.Label()
        Me.nudPrice = New System.Windows.Forms.NumericUpDown()
        Me.lblFirstHarvest = New System.Windows.Forms.Label()
        Me.nudFirstHarvest = New System.Windows.Forms.NumericUpDown()
        Me.lblFullProduction = New System.Windows.Forms.Label()
        Me.nudFullProduction = New System.Windows.Forms.NumericUpDown()
        Me.lblSeason = New System.Windows.Forms.Label()
        Me.txtSeason = New System.Windows.Forms.TextBox()
        Me.lblNotes = New System.Windows.Forms.Label()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.lblHelp = New System.Windows.Forms.Label()
        Me.lblPreview = New System.Windows.Forms.Label()
        Me.flowButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnSave = New TreeYield.WinForms.ChipButton()
        Me.btnCancel = New TreeYield.WinForms.ChipButton()
        Me.errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.layoutMain.SuspendLayout()
        Me.layoutFields.SuspendLayout()
        CType(Me.nudYield, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPrice, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudFirstHarvest, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudFullProduction, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.flowButtons.SuspendLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' layoutMain
        '
        Me.layoutMain.ColumnCount = 1
        Me.layoutMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutMain.Controls.Add(Me.lblTitle, 0, 0)
        Me.layoutMain.Controls.Add(Me.lblIntro, 0, 1)
        Me.layoutMain.Controls.Add(Me.layoutFields, 0, 2)
        Me.layoutMain.Controls.Add(Me.lblHelp, 0, 3)
        Me.layoutMain.Controls.Add(Me.lblPreview, 0, 4)
        Me.layoutMain.Controls.Add(Me.flowButtons, 0, 6)
        Me.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutMain.Location = New System.Drawing.Point(0, 0)
        Me.layoutMain.Name = "layoutMain"
        Me.layoutMain.Padding = New System.Windows.Forms.Padding(28, 22, 28, 18)
        Me.layoutMain.RowCount = 7
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutMain.Size = New System.Drawing.Size(664, 701)
        Me.layoutMain.TabIndex = 0
        '
        ' lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(28, 22)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(130, 30)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Add Species"
        '
        ' lblIntro
        '
        Me.lblIntro.AutoSize = True
        Me.lblIntro.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblIntro.Location = New System.Drawing.Point(28, 54)
        Me.lblIntro.Margin = New System.Windows.Forms.Padding(0, 2, 0, 14)
        Me.lblIntro.Name = "lblIntro"
        Me.lblIntro.Size = New System.Drawing.Size(380, 17)
        Me.lblIntro.TabIndex = 1
        Me.lblIntro.Text = "Fields marked * are required. Values are checked before saving."
        '
        ' layoutFields
        '
        Me.layoutFields.AutoSize = True
        Me.layoutFields.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutFields.ColumnCount = 2
        Me.layoutFields.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.0!))
        Me.layoutFields.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.0!))
        Me.layoutFields.Controls.Add(Me.lblCommonName, 0, 0)
        Me.layoutFields.Controls.Add(Me.txtCommonName, 1, 0)
        Me.layoutFields.Controls.Add(Me.lblScientificName, 0, 1)
        Me.layoutFields.Controls.Add(Me.txtScientificName, 1, 1)
        Me.layoutFields.Controls.Add(Me.lblCategory, 0, 2)
        Me.layoutFields.Controls.Add(Me.cboCategory, 1, 2)
        Me.layoutFields.Controls.Add(Me.lblProductType, 0, 3)
        Me.layoutFields.Controls.Add(Me.cboProductType, 1, 3)
        Me.layoutFields.Controls.Add(Me.lblHarvestType, 0, 4)
        Me.layoutFields.Controls.Add(Me.cboHarvestType, 1, 4)
        Me.layoutFields.Controls.Add(Me.lblYieldUnit, 0, 5)
        Me.layoutFields.Controls.Add(Me.cboYieldUnit, 1, 5)
        Me.layoutFields.Controls.Add(Me.lblYield, 0, 6)
        Me.layoutFields.Controls.Add(Me.nudYield, 1, 6)
        Me.layoutFields.Controls.Add(Me.lblPrice, 0, 7)
        Me.layoutFields.Controls.Add(Me.nudPrice, 1, 7)
        Me.layoutFields.Controls.Add(Me.lblFirstHarvest, 0, 8)
        Me.layoutFields.Controls.Add(Me.nudFirstHarvest, 1, 8)
        Me.layoutFields.Controls.Add(Me.lblFullProduction, 0, 9)
        Me.layoutFields.Controls.Add(Me.nudFullProduction, 1, 9)
        Me.layoutFields.Controls.Add(Me.lblSeason, 0, 10)
        Me.layoutFields.Controls.Add(Me.txtSeason, 1, 10)
        Me.layoutFields.Controls.Add(Me.lblNotes, 0, 11)
        Me.layoutFields.Controls.Add(Me.txtNotes, 1, 11)
        Me.layoutFields.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutFields.Location = New System.Drawing.Point(28, 85)
        Me.layoutFields.Margin = New System.Windows.Forms.Padding(0)
        Me.layoutFields.Name = "layoutFields"
        Me.layoutFields.RowCount = 12
        Me.layoutFields.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutFields.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutFields.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutFields.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutFields.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutFields.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutFields.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutFields.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutFields.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutFields.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutFields.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutFields.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutFields.Size = New System.Drawing.Size(608, 470)
        Me.layoutFields.TabIndex = 2
        '
        ' lblCommonName
        '
        Me.lblCommonName.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCommonName.AutoSize = True
        Me.lblCommonName.Location = New System.Drawing.Point(0, 9)
        Me.lblCommonName.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.lblCommonName.Name = "lblCommonName"
        Me.lblCommonName.Size = New System.Drawing.Size(108, 17)
        Me.lblCommonName.TabIndex = 0
        Me.lblCommonName.Text = "Common &name *"
        '
        ' txtCommonName
        '
        Me.txtCommonName.AccessibleName = "Common name (required)"
        Me.txtCommonName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCommonName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCommonName.Location = New System.Drawing.Point(231, 4)
        Me.txtCommonName.Margin = New System.Windows.Forms.Padding(0, 4, 26, 4)
        Me.txtCommonName.Name = "txtCommonName"
        Me.txtCommonName.Size = New System.Drawing.Size(351, 25)
        Me.txtCommonName.TabIndex = 1
        '
        ' lblScientificName
        '
        Me.lblScientificName.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblScientificName.AutoSize = True
        Me.lblScientificName.Location = New System.Drawing.Point(0, 42)
        Me.lblScientificName.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.lblScientificName.Name = "lblScientificName"
        Me.lblScientificName.Size = New System.Drawing.Size(103, 17)
        Me.lblScientificName.TabIndex = 2
        Me.lblScientificName.Text = "&Scientific name"
        '
        ' txtScientificName
        '
        Me.txtScientificName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtScientificName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtScientificName.Location = New System.Drawing.Point(231, 37)
        Me.txtScientificName.Margin = New System.Windows.Forms.Padding(0, 4, 26, 4)
        Me.txtScientificName.Name = "txtScientificName"
        Me.txtScientificName.Size = New System.Drawing.Size(351, 25)
        Me.txtScientificName.TabIndex = 3
        '
        ' lblCategory
        '
        Me.lblCategory.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCategory.AutoSize = True
        Me.lblCategory.Location = New System.Drawing.Point(0, 75)
        Me.lblCategory.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(73, 17)
        Me.lblCategory.TabIndex = 4
        Me.lblCategory.Text = "&Category *"
        '
        ' cboCategory
        '
        Me.cboCategory.AccessibleName = "Category (required)"
        Me.cboCategory.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCategory.FormattingEnabled = True
        Me.cboCategory.Location = New System.Drawing.Point(231, 70)
        Me.cboCategory.Margin = New System.Windows.Forms.Padding(0, 4, 26, 4)
        Me.cboCategory.Name = "cboCategory"
        Me.cboCategory.Size = New System.Drawing.Size(351, 25)
        Me.cboCategory.TabIndex = 5
        '
        ' lblProductType
        '
        Me.lblProductType.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblProductType.AutoSize = True
        Me.lblProductType.Location = New System.Drawing.Point(0, 108)
        Me.lblProductType.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.lblProductType.Name = "lblProductType"
        Me.lblProductType.Size = New System.Drawing.Size(67, 17)
        Me.lblProductType.TabIndex = 6
        Me.lblProductType.Text = "&Product *"
        '
        ' cboProductType
        '
        Me.cboProductType.AccessibleName = "Product (required)"
        Me.cboProductType.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboProductType.FormattingEnabled = True
        Me.cboProductType.Location = New System.Drawing.Point(231, 103)
        Me.cboProductType.Margin = New System.Windows.Forms.Padding(0, 4, 26, 4)
        Me.cboProductType.Name = "cboProductType"
        Me.cboProductType.Size = New System.Drawing.Size(351, 25)
        Me.cboProductType.TabIndex = 7
        '
        ' lblHarvestType
        '
        Me.lblHarvestType.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblHarvestType.AutoSize = True
        Me.lblHarvestType.Location = New System.Drawing.Point(0, 141)
        Me.lblHarvestType.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.lblHarvestType.Name = "lblHarvestType"
        Me.lblHarvestType.Size = New System.Drawing.Size(96, 17)
        Me.lblHarvestType.TabIndex = 8
        Me.lblHarvestType.Text = "&Harvest type *"
        '
        ' cboHarvestType
        '
        Me.cboHarvestType.AccessibleName = "Harvest type (required)"
        Me.cboHarvestType.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboHarvestType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboHarvestType.FormattingEnabled = True
        Me.cboHarvestType.Items.AddRange(New Object() {"Annual - harvested every year", "Rotation - cut once at rotation age (timber)"})
        Me.cboHarvestType.Location = New System.Drawing.Point(231, 136)
        Me.cboHarvestType.Margin = New System.Windows.Forms.Padding(0, 4, 26, 4)
        Me.cboHarvestType.Name = "cboHarvestType"
        Me.cboHarvestType.Size = New System.Drawing.Size(351, 25)
        Me.cboHarvestType.TabIndex = 9
        '
        ' lblYieldUnit
        '
        Me.lblYieldUnit.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblYieldUnit.AutoSize = True
        Me.lblYieldUnit.Location = New System.Drawing.Point(0, 174)
        Me.lblYieldUnit.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.lblYieldUnit.Name = "lblYieldUnit"
        Me.lblYieldUnit.Size = New System.Drawing.Size(76, 17)
        Me.lblYieldUnit.TabIndex = 10
        Me.lblYieldUnit.Text = "Yield &unit *"
        '
        ' cboYieldUnit
        '
        Me.cboYieldUnit.AccessibleName = "Yield unit (required)"
        Me.cboYieldUnit.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cboYieldUnit.FormattingEnabled = True
        Me.cboYieldUnit.Items.AddRange(New Object() {"kg", "pcs", "board ft", "L", "cu m"})
        Me.cboYieldUnit.Location = New System.Drawing.Point(231, 169)
        Me.cboYieldUnit.Margin = New System.Windows.Forms.Padding(0, 4, 26, 4)
        Me.cboYieldUnit.Name = "cboYieldUnit"
        Me.cboYieldUnit.Size = New System.Drawing.Size(160, 25)
        Me.cboYieldUnit.TabIndex = 11
        '
        ' lblYield
        '
        Me.lblYield.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblYield.AutoSize = True
        Me.lblYield.Location = New System.Drawing.Point(0, 207)
        Me.lblYield.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.lblYield.Name = "lblYield"
        Me.lblYield.Size = New System.Drawing.Size(165, 17)
        Me.lblYield.TabIndex = 12
        Me.lblYield.Text = "&Yield per tree per year *"
        '
        ' nudYield
        '
        Me.nudYield.AccessibleName = "Yield per tree per year (required)"
        Me.nudYield.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudYield.DecimalPlaces = 2
        Me.nudYield.Location = New System.Drawing.Point(231, 203)
        Me.nudYield.Margin = New System.Windows.Forms.Padding(0, 4, 26, 4)
        Me.nudYield.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudYield.Name = "nudYield"
        Me.nudYield.Size = New System.Drawing.Size(160, 25)
        Me.nudYield.TabIndex = 13
        Me.nudYield.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudYield.ThousandsSeparator = True
        '
        ' lblPrice
        '
        Me.lblPrice.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblPrice.AutoSize = True
        Me.lblPrice.Location = New System.Drawing.Point(0, 240)
        Me.lblPrice.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(150, 17)
        Me.lblPrice.TabIndex = 14
        Me.lblPrice.Text = "P&rice per unit (PHP) *"
        '
        ' nudPrice
        '
        Me.nudPrice.AccessibleName = "Price per unit in pesos (required)"
        Me.nudPrice.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudPrice.DecimalPlaces = 2
        Me.nudPrice.Location = New System.Drawing.Point(231, 236)
        Me.nudPrice.Margin = New System.Windows.Forms.Padding(0, 4, 26, 4)
        Me.nudPrice.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudPrice.Name = "nudPrice"
        Me.nudPrice.Size = New System.Drawing.Size(160, 25)
        Me.nudPrice.TabIndex = 15
        Me.nudPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudPrice.ThousandsSeparator = True
        '
        ' lblFirstHarvest
        '
        Me.lblFirstHarvest.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblFirstHarvest.AutoSize = True
        Me.lblFirstHarvest.Location = New System.Drawing.Point(0, 273)
        Me.lblFirstHarvest.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.lblFirstHarvest.Name = "lblFirstHarvest"
        Me.lblFirstHarvest.Size = New System.Drawing.Size(150, 17)
        Me.lblFirstHarvest.TabIndex = 16
        Me.lblFirstHarvest.Text = "&First harvest (year) *"
        '
        ' nudFirstHarvest
        '
        Me.nudFirstHarvest.AccessibleName = "Years to first harvest (required)"
        Me.nudFirstHarvest.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudFirstHarvest.Location = New System.Drawing.Point(231, 269)
        Me.nudFirstHarvest.Margin = New System.Windows.Forms.Padding(0, 4, 26, 4)
        Me.nudFirstHarvest.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
        Me.nudFirstHarvest.Name = "nudFirstHarvest"
        Me.nudFirstHarvest.Size = New System.Drawing.Size(90, 25)
        Me.nudFirstHarvest.TabIndex = 17
        Me.nudFirstHarvest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        ' lblFullProduction
        '
        Me.lblFullProduction.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblFullProduction.AutoSize = True
        Me.lblFullProduction.Location = New System.Drawing.Point(0, 306)
        Me.lblFullProduction.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.lblFullProduction.Name = "lblFullProduction"
        Me.lblFullProduction.Size = New System.Drawing.Size(170, 17)
        Me.lblFullProduction.TabIndex = 18
        Me.lblFullProduction.Text = "Full pro&duction (year) *"
        '
        ' nudFullProduction
        '
        Me.nudFullProduction.AccessibleName = "Year of full production (required)"
        Me.nudFullProduction.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudFullProduction.Location = New System.Drawing.Point(231, 302)
        Me.nudFullProduction.Margin = New System.Windows.Forms.Padding(0, 4, 26, 4)
        Me.nudFullProduction.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
        Me.nudFullProduction.Name = "nudFullProduction"
        Me.nudFullProduction.Size = New System.Drawing.Size(90, 25)
        Me.nudFullProduction.TabIndex = 19
        Me.nudFullProduction.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        ' lblSeason
        '
        Me.lblSeason.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblSeason.AutoSize = True
        Me.lblSeason.Location = New System.Drawing.Point(0, 339)
        Me.lblSeason.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.lblSeason.Name = "lblSeason"
        Me.lblSeason.Size = New System.Drawing.Size(100, 17)
        Me.lblSeason.TabIndex = 20
        Me.lblSeason.Text = "Harvest s&eason"
        '
        ' txtSeason
        '
        Me.txtSeason.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSeason.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSeason.Location = New System.Drawing.Point(231, 335)
        Me.txtSeason.Margin = New System.Windows.Forms.Padding(0, 4, 26, 4)
        Me.txtSeason.Name = "txtSeason"
        Me.txtSeason.Size = New System.Drawing.Size(351, 25)
        Me.txtSeason.TabIndex = 21
        '
        ' lblNotes
        '
        Me.lblNotes.AutoSize = True
        Me.lblNotes.Location = New System.Drawing.Point(0, 372)
        Me.lblNotes.Margin = New System.Windows.Forms.Padding(0, 6, 8, 0)
        Me.lblNotes.Name = "lblNotes"
        Me.lblNotes.Size = New System.Drawing.Size(43, 17)
        Me.lblNotes.TabIndex = 22
        Me.lblNotes.Text = "N&otes"
        '
        ' txtNotes
        '
        Me.txtNotes.AcceptsReturn = True
        Me.txtNotes.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNotes.Location = New System.Drawing.Point(231, 368)
        Me.txtNotes.Margin = New System.Windows.Forms.Padding(0, 4, 26, 4)
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtNotes.Size = New System.Drawing.Size(351, 92)
        Me.txtNotes.TabIndex = 23
        '
        ' lblHelp
        '
        Me.lblHelp.AutoSize = True
        Me.lblHelp.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHelp.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblHelp.Location = New System.Drawing.Point(28, 563)
        Me.lblHelp.Margin = New System.Windows.Forms.Padding(0, 10, 0, 6)
        Me.lblHelp.MaximumSize = New System.Drawing.Size(608, 0)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(120, 15)
        Me.lblHelp.TabIndex = 3
        Me.lblHelp.Text = "Harvest type help"
        '
        ' lblPreview
        '
        Me.lblPreview.AutoSize = True
        Me.lblPreview.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreview.ForeColor = System.Drawing.Color.FromArgb(11, 128, 67)
        Me.lblPreview.Location = New System.Drawing.Point(28, 584)
        Me.lblPreview.Margin = New System.Windows.Forms.Padding(0, 0, 0, 6)
        Me.lblPreview.MaximumSize = New System.Drawing.Size(608, 0)
        Me.lblPreview.Name = "lblPreview"
        Me.lblPreview.Size = New System.Drawing.Size(60, 17)
        Me.lblPreview.TabIndex = 4
        Me.lblPreview.Text = "Preview"
        Me.lblPreview.UseMnemonic = False
        '
        ' flowButtons
        '
        Me.flowButtons.AutoSize = True
        Me.flowButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowButtons.Controls.Add(Me.btnSave)
        Me.flowButtons.Controls.Add(Me.btnCancel)
        Me.flowButtons.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flowButtons.Location = New System.Drawing.Point(28, 687)
        Me.flowButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flowButtons.Name = "flowButtons"
        Me.flowButtons.Size = New System.Drawing.Size(608, 36)
        Me.flowButtons.TabIndex = 5
        Me.flowButtons.WrapContents = False
        '
        ' btnSave
        '
        Me.btnSave.AutoSize = True
        Me.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnSave.ChipStyle = TreeYield.WinForms.ChipStyle.Primary
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(498, 0)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(110, 36)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Save Species"
        '
        ' btnCancel
        '
        Me.btnCancel.AutoSize = True
        Me.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnCancel.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(410, 0)
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
        ' SpeciesEditorForm
        '
        Me.AcceptButton = Me.btnSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(664, 701)
        Me.Controls.Add(Me.layoutMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SpeciesEditorForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Species"
        Me.layoutMain.ResumeLayout(False)
        Me.layoutMain.PerformLayout()
        Me.layoutFields.ResumeLayout(False)
        Me.layoutFields.PerformLayout()
        CType(Me.nudYield, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPrice, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudFirstHarvest, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudFullProduction, System.ComponentModel.ISupportInitialize).EndInit()
        Me.flowButtons.ResumeLayout(False)
        Me.flowButtons.PerformLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents layoutMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblIntro As System.Windows.Forms.Label
    Friend WithEvents layoutFields As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblCommonName As System.Windows.Forms.Label
    Friend WithEvents txtCommonName As System.Windows.Forms.TextBox
    Friend WithEvents lblScientificName As System.Windows.Forms.Label
    Friend WithEvents txtScientificName As System.Windows.Forms.TextBox
    Friend WithEvents lblCategory As System.Windows.Forms.Label
    Friend WithEvents cboCategory As System.Windows.Forms.ComboBox
    Friend WithEvents lblProductType As System.Windows.Forms.Label
    Friend WithEvents cboProductType As System.Windows.Forms.ComboBox
    Friend WithEvents lblHarvestType As System.Windows.Forms.Label
    Friend WithEvents cboHarvestType As System.Windows.Forms.ComboBox
    Friend WithEvents lblYieldUnit As System.Windows.Forms.Label
    Friend WithEvents cboYieldUnit As System.Windows.Forms.ComboBox
    Friend WithEvents lblYield As System.Windows.Forms.Label
    Friend WithEvents nudYield As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblPrice As System.Windows.Forms.Label
    Friend WithEvents nudPrice As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblFirstHarvest As System.Windows.Forms.Label
    Friend WithEvents nudFirstHarvest As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblFullProduction As System.Windows.Forms.Label
    Friend WithEvents nudFullProduction As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblSeason As System.Windows.Forms.Label
    Friend WithEvents txtSeason As System.Windows.Forms.TextBox
    Friend WithEvents lblNotes As System.Windows.Forms.Label
    Friend WithEvents txtNotes As System.Windows.Forms.TextBox
    Friend WithEvents lblHelp As System.Windows.Forms.Label
    Friend WithEvents lblPreview As System.Windows.Forms.Label
    Friend WithEvents flowButtons As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnSave As TreeYield.WinForms.ChipButton
    Friend WithEvents btnCancel As TreeYield.WinForms.ChipButton
    Friend WithEvents errorProvider As System.Windows.Forms.ErrorProvider
End Class
