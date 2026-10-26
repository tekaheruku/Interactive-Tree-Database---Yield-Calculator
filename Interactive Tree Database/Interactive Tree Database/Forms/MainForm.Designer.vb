<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Dim chartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.toolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.searchTimer = New System.Windows.Forms.Timer(Me.components)
        Me.layoutRoot = New System.Windows.Forms.TableLayoutPanel()
        Me.layoutHeader = New System.Windows.Forms.TableLayoutPanel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblSubtitle = New System.Windows.Forms.Label()
        Me.flowActions = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnSaveScenario = New TreeYield.WinForms.ChipButton()
        Me.btnExportCsv = New TreeYield.WinForms.ChipButton()
        Me.btnPrint = New TreeYield.WinForms.ChipButton()
        Me.btnAbout = New TreeYield.WinForms.ChipButton()
        Me.btnAdmin = New TreeYield.WinForms.ChipButton()
        Me.flowFilters = New System.Windows.Forms.FlowLayoutPanel()
        Me.chipAll = New TreeYield.WinForms.ChipButton()
        Me.chipFruit = New TreeYield.WinForms.ChipButton()
        Me.chipNonFruit = New TreeYield.WinForms.ChipButton()
        Me.pnlCalculator = New TreeYield.WinForms.RoundedPanel()
        Me.layoutCalculator = New System.Windows.Forms.TableLayoutPanel()
        Me.layoutInputs = New System.Windows.Forms.TableLayoutPanel()
        Me.lblSpecies = New System.Windows.Forms.Label()
        Me.lblTreeCount = New System.Windows.Forms.Label()
        Me.cboSpecies = New System.Windows.Forms.ComboBox()
        Me.layoutTreeCount = New System.Windows.Forms.TableLayoutPanel()
        Me.sliderTreeCount = New TreeYield.WinForms.FlatSlider()
        Me.nudTreeCount = New System.Windows.Forms.NumericUpDown()
        Me.layoutCards = New System.Windows.Forms.TableLayoutPanel()
        Me.cardSpecies = New TreeYield.WinForms.StatCard()
        Me.cardYield = New TreeYield.WinForms.StatCard()
        Me.cardSales = New TreeYield.WinForms.StatCard()
        Me.layoutSectionBar = New System.Windows.Forms.TableLayoutPanel()
        Me.flowSections = New System.Windows.Forms.FlowLayoutPanel()
        Me.chipRecords = New TreeYield.WinForms.ChipButton()
        Me.chipProjection = New TreeYield.WinForms.ChipButton()
        Me.chipScenarios = New TreeYield.WinForms.ChipButton()
        Me.lblDisclaimer = New System.Windows.Forms.Label()
        Me.pnlContent = New TreeYield.WinForms.RoundedPanel()
        Me.pnlRecords = New System.Windows.Forms.Panel()
        Me.gridRecords = New System.Windows.Forms.DataGridView()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colScientific = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colCategory = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colProduct = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colYield = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUnit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colPrice = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFirstHarvest = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colHarvestType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblRecordsEmpty = New System.Windows.Forms.Label()
        Me.layoutRecordsBar = New System.Windows.Forms.TableLayoutPanel()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblRecordCount = New System.Windows.Forms.Label()
        Me.pnlProjection = New System.Windows.Forms.Panel()
        Me.layoutProjectionBody = New System.Windows.Forms.TableLayoutPanel()
        Me.chartProjection = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.gridProjection = New System.Windows.Forms.DataGridView()
        Me.colProjYear = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colProjStage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colProjYield = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colProjSales = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colProjCumulative = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.layoutProjectionBar = New System.Windows.Forms.TableLayoutPanel()
        Me.lblHorizon = New System.Windows.Forms.Label()
        Me.nudYears = New System.Windows.Forms.NumericUpDown()
        Me.lblProjectionSummary = New System.Windows.Forms.Label()
        Me.pnlScenarios = New System.Windows.Forms.Panel()
        Me.gridScenarios = New System.Windows.Forms.DataGridView()
        Me.colScenarioName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colScenarioSpecies = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colScenarioTrees = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colScenarioYears = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colScenarioSales = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colScenarioSaved = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblScenariosEmpty = New System.Windows.Forms.Label()
        Me.flowScenarioActions = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnLoadScenario = New TreeYield.WinForms.ChipButton()
        Me.btnDeleteScenario = New TreeYield.WinForms.ChipButton()
        Me.lblScenarioHint = New System.Windows.Forms.Label()
        Me.statusStrip = New System.Windows.Forms.StatusStrip()
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblDatabase = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblAdmin = New System.Windows.Forms.ToolStripStatusLabel()
        Me.layoutRoot.SuspendLayout()
        Me.layoutHeader.SuspendLayout()
        Me.flowActions.SuspendLayout()
        Me.flowFilters.SuspendLayout()
        Me.pnlCalculator.SuspendLayout()
        Me.layoutCalculator.SuspendLayout()
        Me.layoutInputs.SuspendLayout()
        Me.layoutTreeCount.SuspendLayout()
        CType(Me.nudTreeCount, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.layoutCards.SuspendLayout()
        Me.layoutSectionBar.SuspendLayout()
        Me.flowSections.SuspendLayout()
        Me.pnlContent.SuspendLayout()
        Me.pnlRecords.SuspendLayout()
        CType(Me.gridRecords, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.layoutRecordsBar.SuspendLayout()
        Me.pnlProjection.SuspendLayout()
        Me.layoutProjectionBody.SuspendLayout()
        CType(Me.chartProjection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridProjection, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.layoutProjectionBar.SuspendLayout()
        CType(Me.nudYears, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlScenarios.SuspendLayout()
        CType(Me.gridScenarios, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.flowScenarioActions.SuspendLayout()
        Me.statusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        ' searchTimer
        '
        Me.searchTimer.Interval = 300
        '
        ' layoutRoot
        '
        Me.layoutRoot.BackColor = System.Drawing.Color.FromArgb(247, 247, 248)
        Me.layoutRoot.ColumnCount = 1
        Me.layoutRoot.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutRoot.Controls.Add(Me.layoutHeader, 0, 0)
        Me.layoutRoot.Controls.Add(Me.flowFilters, 0, 1)
        Me.layoutRoot.Controls.Add(Me.pnlCalculator, 0, 2)
        Me.layoutRoot.Controls.Add(Me.layoutSectionBar, 0, 3)
        Me.layoutRoot.Controls.Add(Me.pnlContent, 0, 4)
        Me.layoutRoot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutRoot.Location = New System.Drawing.Point(0, 0)
        Me.layoutRoot.Name = "layoutRoot"
        Me.layoutRoot.Padding = New System.Windows.Forms.Padding(28, 18, 28, 10)
        Me.layoutRoot.RowCount = 5
        Me.layoutRoot.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutRoot.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutRoot.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutRoot.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutRoot.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutRoot.Size = New System.Drawing.Size(1184, 836)
        Me.layoutRoot.TabIndex = 0
        '
        ' layoutHeader
        '
        Me.layoutHeader.AutoSize = True
        Me.layoutHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutHeader.ColumnCount = 2
        Me.layoutHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.layoutHeader.Controls.Add(Me.lblTitle, 0, 0)
        Me.layoutHeader.Controls.Add(Me.lblSubtitle, 0, 1)
        Me.layoutHeader.Controls.Add(Me.flowActions, 1, 0)
        Me.layoutHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutHeader.Location = New System.Drawing.Point(28, 18)
        Me.layoutHeader.Margin = New System.Windows.Forms.Padding(0, 0, 0, 14)
        Me.layoutHeader.Name = "layoutHeader"
        Me.layoutHeader.RowCount = 2
        Me.layoutHeader.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutHeader.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutHeader.SetRowSpan(Me.flowActions, 2)
        Me.layoutHeader.Size = New System.Drawing.Size(1128, 62)
        Me.layoutHeader.TabIndex = 0
        '
        ' lblTitle
        '
        Me.lblTitle.AccessibleName = "Tree Yield and Revenue Calculator"
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(398, 37)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Tree Yield & Revenue Calculator"
        Me.lblTitle.UseMnemonic = False
        '
        ' lblSubtitle
        '
        Me.lblSubtitle.AutoSize = True
        Me.lblSubtitle.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblSubtitle.Location = New System.Drawing.Point(2, 39)
        Me.lblSubtitle.Margin = New System.Windows.Forms.Padding(2, 2, 0, 0)
        Me.lblSubtitle.Name = "lblSubtitle"
        Me.lblSubtitle.Size = New System.Drawing.Size(560, 19)
        Me.lblSubtitle.TabIndex = 1
        Me.lblSubtitle.Text = "Explore tree species data, filter categories, and project annual yields and estimated PHP gross sales."
        Me.lblSubtitle.UseMnemonic = False
        '
        ' flowActions
        '
        Me.flowActions.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flowActions.AutoSize = True
        Me.flowActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowActions.Controls.Add(Me.btnSaveScenario)
        Me.flowActions.Controls.Add(Me.btnExportCsv)
        Me.flowActions.Controls.Add(Me.btnPrint)
        Me.flowActions.Controls.Add(Me.btnAbout)
        Me.flowActions.Controls.Add(Me.btnAdmin)
        Me.flowActions.Location = New System.Drawing.Point(620, 0)
        Me.flowActions.Margin = New System.Windows.Forms.Padding(0)
        Me.flowActions.Name = "flowActions"
        Me.flowActions.Size = New System.Drawing.Size(508, 40)
        Me.flowActions.TabIndex = 2
        Me.flowActions.WrapContents = False
        '
        ' btnSaveScenario
        '
        Me.btnSaveScenario.AutoSize = True
        Me.btnSaveScenario.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnSaveScenario.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnSaveScenario.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveScenario.Location = New System.Drawing.Point(8, 4)
        Me.btnSaveScenario.Margin = New System.Windows.Forms.Padding(8, 4, 0, 0)
        Me.btnSaveScenario.Name = "btnSaveScenario"
        Me.btnSaveScenario.Size = New System.Drawing.Size(118, 36)
        Me.btnSaveScenario.TabIndex = 0
        Me.btnSaveScenario.Text = "Save Scenario"
        Me.toolTip.SetToolTip(Me.btnSaveScenario, "Save the current species and tree count as a named scenario (Ctrl+S)")
        '
        ' btnExportCsv
        '
        Me.btnExportCsv.AutoSize = True
        Me.btnExportCsv.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnExportCsv.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnExportCsv.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportCsv.Location = New System.Drawing.Point(134, 4)
        Me.btnExportCsv.Margin = New System.Windows.Forms.Padding(8, 4, 0, 0)
        Me.btnExportCsv.Name = "btnExportCsv"
        Me.btnExportCsv.Size = New System.Drawing.Size(104, 36)
        Me.btnExportCsv.TabIndex = 1
        Me.btnExportCsv.Text = "Export CSV"
        Me.toolTip.SetToolTip(Me.btnExportCsv, "Save the calculation and projection as a CSV file for Excel (Ctrl+E)")
        '
        ' btnPrint
        '
        Me.btnPrint.AutoSize = True
        Me.btnPrint.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnPrint.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnPrint.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(246, 4)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(8, 4, 0, 0)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(108, 36)
        Me.btnPrint.TabIndex = 2
        Me.btnPrint.Text = "Print Report"
        Me.toolTip.SetToolTip(Me.btnPrint, "Preview and print a one-page report (Ctrl+P)")
        '
        ' btnAbout
        '
        Me.btnAbout.AutoSize = True
        Me.btnAbout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnAbout.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnAbout.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAbout.Location = New System.Drawing.Point(362, 4)
        Me.btnAbout.Margin = New System.Windows.Forms.Padding(8, 4, 0, 0)
        Me.btnAbout.Name = "btnAbout"
        Me.btnAbout.Size = New System.Drawing.Size(76, 36)
        Me.btnAbout.TabIndex = 3
        Me.btnAbout.Text = "About"
        Me.toolTip.SetToolTip(Me.btnAbout, "About this program, the data and keyboard shortcuts (F1)")
        '
        ' btnAdmin
        '
        Me.btnAdmin.AutoSize = True
        Me.btnAdmin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnAdmin.ChipStyle = TreeYield.WinForms.ChipStyle.Primary
        Me.btnAdmin.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdmin.Location = New System.Drawing.Point(446, 4)
        Me.btnAdmin.Margin = New System.Windows.Forms.Padding(8, 4, 0, 0)
        Me.btnAdmin.Name = "btnAdmin"
        Me.btnAdmin.Size = New System.Drawing.Size(112, 36)
        Me.btnAdmin.TabIndex = 4
        Me.btnAdmin.Text = "Admin Login"
        Me.toolTip.SetToolTip(Me.btnAdmin, "Sign in as administrator to add, edit or delete species records")
        '
        ' flowFilters
        '
        Me.flowFilters.AutoSize = True
        Me.flowFilters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowFilters.Controls.Add(Me.chipAll)
        Me.flowFilters.Controls.Add(Me.chipFruit)
        Me.flowFilters.Controls.Add(Me.chipNonFruit)
        Me.flowFilters.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowFilters.Location = New System.Drawing.Point(28, 94)
        Me.flowFilters.Margin = New System.Windows.Forms.Padding(0, 0, 0, 14)
        Me.flowFilters.Name = "flowFilters"
        Me.flowFilters.Size = New System.Drawing.Size(1128, 38)
        Me.flowFilters.TabIndex = 1
        '
        ' chipAll
        '
        Me.chipAll.AccessibleName = "Show all species"
        Me.chipAll.AutoSize = True
        Me.chipAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.chipAll.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chipAll.Location = New System.Drawing.Point(0, 0)
        Me.chipAll.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.chipAll.Name = "chipAll"
        Me.chipAll.Selected = True
        Me.chipAll.Size = New System.Drawing.Size(116, 38)
        Me.chipAll.TabIndex = 0
        Me.chipAll.Text = "All Species"
        '
        ' chipFruit
        '
        Me.chipFruit.AccessibleName = "Show fruit-bearing species"
        Me.chipFruit.AutoSize = True
        Me.chipFruit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.chipFruit.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chipFruit.Location = New System.Drawing.Point(124, 0)
        Me.chipFruit.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.chipFruit.Name = "chipFruit"
        Me.chipFruit.Size = New System.Drawing.Size(132, 38)
        Me.chipFruit.TabIndex = 1
        Me.chipFruit.Text = "Fruit-Bearing"
        '
        ' chipNonFruit
        '
        Me.chipNonFruit.AccessibleName = "Show non-fruit-bearing species"
        Me.chipNonFruit.AutoSize = True
        Me.chipNonFruit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.chipNonFruit.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chipNonFruit.Location = New System.Drawing.Point(264, 0)
        Me.chipNonFruit.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.chipNonFruit.Name = "chipNonFruit"
        Me.chipNonFruit.Size = New System.Drawing.Size(170, 38)
        Me.chipNonFruit.TabIndex = 2
        Me.chipNonFruit.Text = "Non-Fruit-Bearing"
        '
        ' pnlCalculator
        '
        Me.pnlCalculator.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlCalculator.BackColor = System.Drawing.Color.FromArgb(241, 239, 239)
        Me.pnlCalculator.Controls.Add(Me.layoutCalculator)
        Me.pnlCalculator.CornerRadius = 16
        Me.pnlCalculator.Location = New System.Drawing.Point(28, 146)
        Me.pnlCalculator.Margin = New System.Windows.Forms.Padding(0, 0, 0, 16)
        Me.pnlCalculator.Name = "pnlCalculator"
        Me.pnlCalculator.Padding = New System.Windows.Forms.Padding(22, 16, 22, 18)
        Me.pnlCalculator.Size = New System.Drawing.Size(1128, 214)
        Me.pnlCalculator.TabIndex = 2
        '
        ' layoutCalculator
        '
        Me.layoutCalculator.BackColor = System.Drawing.Color.FromArgb(241, 239, 239)
        Me.layoutCalculator.ColumnCount = 1
        Me.layoutCalculator.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutCalculator.Controls.Add(Me.layoutInputs, 0, 0)
        Me.layoutCalculator.Controls.Add(Me.layoutCards, 0, 1)
        Me.layoutCalculator.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutCalculator.Location = New System.Drawing.Point(22, 16)
        Me.layoutCalculator.Margin = New System.Windows.Forms.Padding(0)
        Me.layoutCalculator.Name = "layoutCalculator"
        Me.layoutCalculator.RowCount = 2
        Me.layoutCalculator.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutCalculator.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutCalculator.Size = New System.Drawing.Size(1084, 180)
        Me.layoutCalculator.TabIndex = 0
        '
        ' layoutInputs
        '
        Me.layoutInputs.AutoSize = True
        Me.layoutInputs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutInputs.ColumnCount = 2
        Me.layoutInputs.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.layoutInputs.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.layoutInputs.Controls.Add(Me.lblSpecies, 0, 0)
        Me.layoutInputs.Controls.Add(Me.lblTreeCount, 1, 0)
        Me.layoutInputs.Controls.Add(Me.cboSpecies, 0, 1)
        Me.layoutInputs.Controls.Add(Me.layoutTreeCount, 1, 1)
        Me.layoutInputs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutInputs.Location = New System.Drawing.Point(0, 0)
        Me.layoutInputs.Margin = New System.Windows.Forms.Padding(0)
        Me.layoutInputs.Name = "layoutInputs"
        Me.layoutInputs.RowCount = 2
        Me.layoutInputs.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutInputs.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutInputs.Size = New System.Drawing.Size(1084, 62)
        Me.layoutInputs.TabIndex = 0
        '
        ' lblSpecies
        '
        Me.lblSpecies.AutoSize = True
        Me.lblSpecies.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpecies.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.lblSpecies.Location = New System.Drawing.Point(0, 0)
        Me.lblSpecies.Margin = New System.Windows.Forms.Padding(0, 0, 3, 8)
        Me.lblSpecies.Name = "lblSpecies"
        Me.lblSpecies.Size = New System.Drawing.Size(176, 19)
        Me.lblSpecies.TabIndex = 0
        Me.lblSpecies.Text = "Target Crop / Tree &Species"
        '
        ' lblTreeCount
        '
        Me.lblTreeCount.AutoSize = True
        Me.lblTreeCount.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTreeCount.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.lblTreeCount.Location = New System.Drawing.Point(542, 0)
        Me.lblTreeCount.Margin = New System.Windows.Forms.Padding(0, 0, 3, 8)
        Me.lblTreeCount.Name = "lblTreeCount"
        Me.lblTreeCount.Size = New System.Drawing.Size(76, 19)
        Me.lblTreeCount.TabIndex = 2
        Me.lblTreeCount.Text = "&Tree Count"
        '
        ' cboSpecies
        '
        Me.cboSpecies.AccessibleName = "Target crop or tree species"
        Me.cboSpecies.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSpecies.BackColor = System.Drawing.Color.White
        Me.cboSpecies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSpecies.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboSpecies.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSpecies.FormattingEnabled = True
        Me.cboSpecies.IntegralHeight = False
        Me.cboSpecies.Location = New System.Drawing.Point(0, 27)
        Me.cboSpecies.Margin = New System.Windows.Forms.Padding(0, 0, 28, 0)
        Me.cboSpecies.MaxDropDownItems = 16
        Me.cboSpecies.Name = "cboSpecies"
        Me.cboSpecies.Size = New System.Drawing.Size(514, 28)
        Me.cboSpecies.TabIndex = 1
        '
        ' layoutTreeCount
        '
        Me.layoutTreeCount.AutoSize = True
        Me.layoutTreeCount.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutTreeCount.ColumnCount = 2
        Me.layoutTreeCount.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutTreeCount.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.layoutTreeCount.Controls.Add(Me.sliderTreeCount, 0, 0)
        Me.layoutTreeCount.Controls.Add(Me.nudTreeCount, 1, 0)
        Me.layoutTreeCount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutTreeCount.Location = New System.Drawing.Point(542, 27)
        Me.layoutTreeCount.Margin = New System.Windows.Forms.Padding(0)
        Me.layoutTreeCount.Name = "layoutTreeCount"
        Me.layoutTreeCount.RowCount = 1
        Me.layoutTreeCount.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutTreeCount.Size = New System.Drawing.Size(542, 35)
        Me.layoutTreeCount.TabIndex = 3
        '
        ' sliderTreeCount
        '
        Me.sliderTreeCount.AccessibleName = "Tree count slider"
        Me.sliderTreeCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.sliderTreeCount.Location = New System.Drawing.Point(0, 0)
        Me.sliderTreeCount.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Me.sliderTreeCount.Maximum = 1000
        Me.sliderTreeCount.Minimum = 1
        Me.sliderTreeCount.Name = "sliderTreeCount"
        Me.sliderTreeCount.Size = New System.Drawing.Size(422, 34)
        Me.sliderTreeCount.TabIndex = 0
        Me.toolTip.SetToolTip(Me.sliderTreeCount, "Drag, click, or use the arrow keys. Type larger counts (up to 100,000) in the box.")
        Me.sliderTreeCount.Value = 25
        '
        ' nudTreeCount
        '
        Me.nudTreeCount.AccessibleName = "Tree count"
        Me.nudTreeCount.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudTreeCount.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudTreeCount.Location = New System.Drawing.Point(438, 3)
        Me.nudTreeCount.Margin = New System.Windows.Forms.Padding(0)
        Me.nudTreeCount.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudTreeCount.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudTreeCount.Name = "nudTreeCount"
        Me.nudTreeCount.Size = New System.Drawing.Size(104, 27)
        Me.nudTreeCount.TabIndex = 1
        Me.nudTreeCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudTreeCount.ThousandsSeparator = True
        Me.toolTip.SetToolTip(Me.nudTreeCount, "Number of trees (1 to 100,000). Type a number and press Enter.")
        Me.nudTreeCount.Value = New Decimal(New Integer() {25, 0, 0, 0})
        '
        ' layoutCards
        '
        Me.layoutCards.ColumnCount = 3
        Me.layoutCards.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.layoutCards.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34!))
        Me.layoutCards.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.layoutCards.Controls.Add(Me.cardSpecies, 0, 0)
        Me.layoutCards.Controls.Add(Me.cardYield, 1, 0)
        Me.layoutCards.Controls.Add(Me.cardSales, 2, 0)
        Me.layoutCards.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutCards.Location = New System.Drawing.Point(0, 78)
        Me.layoutCards.Margin = New System.Windows.Forms.Padding(0, 16, 0, 0)
        Me.layoutCards.Name = "layoutCards"
        Me.layoutCards.RowCount = 1
        Me.layoutCards.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutCards.Size = New System.Drawing.Size(1084, 102)
        Me.layoutCards.TabIndex = 1
        '
        ' cardSpecies
        '
        Me.cardSpecies.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cardSpecies.Font = New System.Drawing.Font("Segoe UI Semibold", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cardSpecies.Location = New System.Drawing.Point(0, 0)
        Me.cardSpecies.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.cardSpecies.Name = "cardSpecies"
        Me.cardSpecies.Size = New System.Drawing.Size(353, 102)
        Me.cardSpecies.TabIndex = 0
        Me.cardSpecies.Title = "Active Species"
        '
        ' cardYield
        '
        Me.cardYield.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cardYield.Font = New System.Drawing.Font("Consolas", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cardYield.Location = New System.Drawing.Point(369, 0)
        Me.cardYield.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.cardYield.Name = "cardYield"
        Me.cardYield.Size = New System.Drawing.Size(345, 102)
        Me.cardYield.TabIndex = 1
        Me.cardYield.Title = "Annual Yield"
        '
        ' cardSales
        '
        Me.cardSales.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cardSales.Font = New System.Drawing.Font("Consolas", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cardSales.Location = New System.Drawing.Point(730, 0)
        Me.cardSales.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.cardSales.Name = "cardSales"
        Me.cardSales.Size = New System.Drawing.Size(354, 102)
        Me.cardSales.TabIndex = 2
        Me.cardSales.Title = "Estimated Gross Sales"
        Me.cardSales.ValueColor = System.Drawing.Color.FromArgb(11, 128, 67)
        '
        ' layoutSectionBar
        '
        Me.layoutSectionBar.AutoSize = True
        Me.layoutSectionBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutSectionBar.ColumnCount = 2
        Me.layoutSectionBar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.layoutSectionBar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutSectionBar.Controls.Add(Me.flowSections, 0, 0)
        Me.layoutSectionBar.Controls.Add(Me.lblDisclaimer, 1, 0)
        Me.layoutSectionBar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutSectionBar.Location = New System.Drawing.Point(28, 376)
        Me.layoutSectionBar.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.layoutSectionBar.Name = "layoutSectionBar"
        Me.layoutSectionBar.RowCount = 1
        Me.layoutSectionBar.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutSectionBar.Size = New System.Drawing.Size(1128, 38)
        Me.layoutSectionBar.TabIndex = 3
        '
        ' flowSections
        '
        Me.flowSections.AutoSize = True
        Me.flowSections.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowSections.Controls.Add(Me.chipRecords)
        Me.flowSections.Controls.Add(Me.chipProjection)
        Me.flowSections.Controls.Add(Me.chipScenarios)
        Me.flowSections.Location = New System.Drawing.Point(0, 0)
        Me.flowSections.Margin = New System.Windows.Forms.Padding(0)
        Me.flowSections.Name = "flowSections"
        Me.flowSections.Size = New System.Drawing.Size(470, 38)
        Me.flowSections.TabIndex = 0
        Me.flowSections.WrapContents = False
        '
        ' chipRecords
        '
        Me.chipRecords.AutoSize = True
        Me.chipRecords.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.chipRecords.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chipRecords.Location = New System.Drawing.Point(0, 0)
        Me.chipRecords.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.chipRecords.Name = "chipRecords"
        Me.chipRecords.Selected = True
        Me.chipRecords.Size = New System.Drawing.Size(144, 38)
        Me.chipRecords.TabIndex = 0
        Me.chipRecords.Text = "Species Records"
        Me.toolTip.SetToolTip(Me.chipRecords, "Browse and search all species records (Ctrl+1)")
        '
        ' chipProjection
        '
        Me.chipProjection.AutoSize = True
        Me.chipProjection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.chipProjection.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chipProjection.Location = New System.Drawing.Point(152, 0)
        Me.chipProjection.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.chipProjection.Name = "chipProjection"
        Me.chipProjection.Size = New System.Drawing.Size(154, 38)
        Me.chipProjection.TabIndex = 1
        Me.chipProjection.Text = "10-Year Projection"
        Me.toolTip.SetToolTip(Me.chipProjection, "Year-by-year yield and sales projection with chart (Ctrl+2)")
        '
        ' chipScenarios
        '
        Me.chipScenarios.AutoSize = True
        Me.chipScenarios.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.chipScenarios.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chipScenarios.Location = New System.Drawing.Point(314, 0)
        Me.chipScenarios.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.chipScenarios.Name = "chipScenarios"
        Me.chipScenarios.Size = New System.Drawing.Size(148, 38)
        Me.chipScenarios.TabIndex = 2
        Me.chipScenarios.Text = "Saved Scenarios"
        Me.toolTip.SetToolTip(Me.chipScenarios, "Calculations you saved earlier (Ctrl+3)")
        '
        ' lblDisclaimer
        '
        Me.lblDisclaimer.AutoEllipsis = True
        Me.lblDisclaimer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDisclaimer.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDisclaimer.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblDisclaimer.Location = New System.Drawing.Point(470, 0)
        Me.lblDisclaimer.Margin = New System.Windows.Forms.Padding(0)
        Me.lblDisclaimer.Name = "lblDisclaimer"
        Me.lblDisclaimer.Size = New System.Drawing.Size(658, 38)
        Me.lblDisclaimer.TabIndex = 1
        Me.lblDisclaimer.Text = "Figures are sample planning estimates (point here for details)."
        Me.lblDisclaimer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        ' pnlContent
        '
        Me.pnlContent.BackColor = System.Drawing.Color.White
        Me.pnlContent.BorderColor = System.Drawing.Color.FromArgb(226, 226, 226)
        Me.pnlContent.Controls.Add(Me.pnlRecords)
        Me.pnlContent.Controls.Add(Me.pnlProjection)
        Me.pnlContent.Controls.Add(Me.pnlScenarios)
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(28, 424)
        Me.pnlContent.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Padding = New System.Windows.Forms.Padding(18, 14, 18, 14)
        Me.pnlContent.Size = New System.Drawing.Size(1128, 402)
        Me.pnlContent.TabIndex = 4
        '
        ' pnlRecords
        '
        Me.pnlRecords.BackColor = System.Drawing.Color.White
        Me.pnlRecords.Controls.Add(Me.gridRecords)
        Me.pnlRecords.Controls.Add(Me.lblRecordsEmpty)
        Me.pnlRecords.Controls.Add(Me.layoutRecordsBar)
        Me.pnlRecords.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlRecords.Location = New System.Drawing.Point(18, 14)
        Me.pnlRecords.Name = "pnlRecords"
        Me.pnlRecords.Size = New System.Drawing.Size(1092, 374)
        Me.pnlRecords.TabIndex = 0
        '
        ' gridRecords
        '
        Me.gridRecords.AccessibleName = "Species records"
        Me.gridRecords.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colName, Me.colScientific, Me.colCategory, Me.colProduct, Me.colYield, Me.colUnit, Me.colPrice, Me.colFirstHarvest, Me.colHarvestType})
        Me.gridRecords.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridRecords.Location = New System.Drawing.Point(0, 42)
        Me.gridRecords.Name = "gridRecords"
        Me.gridRecords.Size = New System.Drawing.Size(1092, 332)
        Me.gridRecords.TabIndex = 1
        Me.toolTip.SetToolTip(Me.gridRecords, "Click a row to use it in the calculator. Double-click or press Enter to see all details.")
        '
        ' colName
        '
        Me.colName.FillWeight = 150.0!
        Me.colName.HeaderText = "Species"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        '
        ' colScientific
        '
        Me.colScientific.FillWeight = 150.0!
        Me.colScientific.HeaderText = "Scientific Name"
        Me.colScientific.Name = "colScientific"
        Me.colScientific.ReadOnly = True
        '
        ' colCategory
        '
        Me.colCategory.FillWeight = 115.0!
        Me.colCategory.HeaderText = "Category"
        Me.colCategory.Name = "colCategory"
        Me.colCategory.ReadOnly = True
        '
        ' colProduct
        '
        Me.colProduct.FillWeight = 120.0!
        Me.colProduct.HeaderText = "Product"
        Me.colProduct.Name = "colProduct"
        Me.colProduct.ReadOnly = True
        '
        ' colYield
        '
        Me.colYield.FillWeight = 95.0!
        Me.colYield.HeaderText = "Yield / Tree / Yr"
        Me.colYield.Name = "colYield"
        Me.colYield.ReadOnly = True
        '
        ' colUnit
        '
        Me.colUnit.FillWeight = 65.0!
        Me.colUnit.HeaderText = "Unit"
        Me.colUnit.Name = "colUnit"
        Me.colUnit.ReadOnly = True
        '
        ' colPrice
        '
        Me.colPrice.FillWeight = 90.0!
        Me.colPrice.HeaderText = "Price / Unit"
        Me.colPrice.Name = "colPrice"
        Me.colPrice.ReadOnly = True
        '
        ' colFirstHarvest
        '
        Me.colFirstHarvest.FillWeight = 90.0!
        Me.colFirstHarvest.HeaderText = "Harvest From (Yr)"
        Me.colFirstHarvest.Name = "colFirstHarvest"
        Me.colFirstHarvest.ReadOnly = True
        Me.colFirstHarvest.ToolTipText = "Annual species: first harvest year. Rotation species: rotation age."
        '
        ' colHarvestType
        '
        Me.colHarvestType.FillWeight = 75.0!
        Me.colHarvestType.HeaderText = "Harvest"
        Me.colHarvestType.Name = "colHarvestType"
        Me.colHarvestType.ReadOnly = True
        Me.colHarvestType.ToolTipText = "Annual = harvested every year. Rotation = cut once at rotation age."
        '
        ' lblRecordsEmpty
        '
        Me.lblRecordsEmpty.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblRecordsEmpty.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecordsEmpty.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblRecordsEmpty.Location = New System.Drawing.Point(0, 42)
        Me.lblRecordsEmpty.Name = "lblRecordsEmpty"
        Me.lblRecordsEmpty.Size = New System.Drawing.Size(1092, 332)
        Me.lblRecordsEmpty.TabIndex = 2
        Me.lblRecordsEmpty.Text = "No species match your search."
        Me.lblRecordsEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblRecordsEmpty.UseMnemonic = False
        Me.lblRecordsEmpty.Visible = False
        '
        ' layoutRecordsBar
        '
        Me.layoutRecordsBar.AutoSize = True
        Me.layoutRecordsBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutRecordsBar.ColumnCount = 2
        Me.layoutRecordsBar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.layoutRecordsBar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutRecordsBar.Controls.Add(Me.txtSearch, 0, 0)
        Me.layoutRecordsBar.Controls.Add(Me.lblRecordCount, 1, 0)
        Me.layoutRecordsBar.Dock = System.Windows.Forms.DockStyle.Top
        Me.layoutRecordsBar.Location = New System.Drawing.Point(0, 0)
        Me.layoutRecordsBar.Name = "layoutRecordsBar"
        Me.layoutRecordsBar.Padding = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.layoutRecordsBar.RowCount = 1
        Me.layoutRecordsBar.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutRecordsBar.Size = New System.Drawing.Size(1092, 42)
        Me.layoutRecordsBar.TabIndex = 0
        '
        ' txtSearch
        '
        Me.txtSearch.AccessibleName = "Search species"
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(0, 0)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(0)
        Me.txtSearch.MaxLength = 100
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(380, 26)
        Me.txtSearch.TabIndex = 0
        Me.toolTip.SetToolTip(Me.txtSearch, "Type part of a name, scientific name or product (Ctrl+F). Press Esc to clear.")
        '
        ' lblRecordCount
        '
        Me.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblRecordCount.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblRecordCount.Location = New System.Drawing.Point(380, 0)
        Me.lblRecordCount.Margin = New System.Windows.Forms.Padding(0)
        Me.lblRecordCount.Name = "lblRecordCount"
        Me.lblRecordCount.Size = New System.Drawing.Size(712, 32)
        Me.lblRecordCount.TabIndex = 1
        Me.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        ' pnlProjection
        '
        Me.pnlProjection.BackColor = System.Drawing.Color.White
        Me.pnlProjection.Controls.Add(Me.layoutProjectionBody)
        Me.pnlProjection.Controls.Add(Me.layoutProjectionBar)
        Me.pnlProjection.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlProjection.Location = New System.Drawing.Point(18, 14)
        Me.pnlProjection.Name = "pnlProjection"
        Me.pnlProjection.Size = New System.Drawing.Size(1092, 374)
        Me.pnlProjection.TabIndex = 1
        Me.pnlProjection.Visible = False
        '
        ' layoutProjectionBody
        '
        Me.layoutProjectionBody.ColumnCount = 2
        Me.layoutProjectionBody.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.0!))
        Me.layoutProjectionBody.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.0!))
        Me.layoutProjectionBody.Controls.Add(Me.chartProjection, 0, 0)
        Me.layoutProjectionBody.Controls.Add(Me.gridProjection, 1, 0)
        Me.layoutProjectionBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutProjectionBody.Location = New System.Drawing.Point(0, 41)
        Me.layoutProjectionBody.Name = "layoutProjectionBody"
        Me.layoutProjectionBody.RowCount = 1
        Me.layoutProjectionBody.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutProjectionBody.Size = New System.Drawing.Size(1092, 333)
        Me.layoutProjectionBody.TabIndex = 1
        '
        ' chartProjection
        '
        chartArea1.Name = "Main"
        Me.chartProjection.ChartAreas.Add(chartArea1)
        Me.chartProjection.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chartProjection.Location = New System.Drawing.Point(0, 0)
        Me.chartProjection.Margin = New System.Windows.Forms.Padding(0, 0, 14, 0)
        Me.chartProjection.Name = "chartProjection"
        series1.ChartArea = "Main"
        series1.Name = "GrossSales"
        Me.chartProjection.Series.Add(series1)
        Me.chartProjection.Size = New System.Drawing.Size(619, 333)
        Me.chartProjection.TabIndex = 0
        Me.chartProjection.TabStop = False
        Me.chartProjection.Text = "Gross sales per year"
        '
        ' gridProjection
        '
        Me.gridProjection.AccessibleName = "Projection table"
        Me.gridProjection.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colProjYear, Me.colProjStage, Me.colProjYield, Me.colProjSales, Me.colProjCumulative})
        Me.gridProjection.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridProjection.Location = New System.Drawing.Point(633, 0)
        Me.gridProjection.Margin = New System.Windows.Forms.Padding(0)
        Me.gridProjection.Name = "gridProjection"
        Me.gridProjection.Size = New System.Drawing.Size(459, 333)
        Me.gridProjection.TabIndex = 1
        '
        ' colProjYear
        '
        Me.colProjYear.FillWeight = 45.0!
        Me.colProjYear.HeaderText = "Year"
        Me.colProjYear.Name = "colProjYear"
        Me.colProjYear.ReadOnly = True
        Me.colProjYear.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        ' colProjStage
        '
        Me.colProjStage.FillWeight = 95.0!
        Me.colProjStage.HeaderText = "Production"
        Me.colProjStage.Name = "colProjStage"
        Me.colProjStage.ReadOnly = True
        Me.colProjStage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        ' colProjYield
        '
        Me.colProjYield.HeaderText = "Yield"
        Me.colProjYield.Name = "colProjYield"
        Me.colProjYield.ReadOnly = True
        Me.colProjYield.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        ' colProjSales
        '
        Me.colProjSales.HeaderText = "Gross Sales"
        Me.colProjSales.Name = "colProjSales"
        Me.colProjSales.ReadOnly = True
        Me.colProjSales.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        ' colProjCumulative
        '
        Me.colProjCumulative.HeaderText = "Cumulative"
        Me.colProjCumulative.Name = "colProjCumulative"
        Me.colProjCumulative.ReadOnly = True
        Me.colProjCumulative.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        ' layoutProjectionBar
        '
        Me.layoutProjectionBar.AutoSize = True
        Me.layoutProjectionBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutProjectionBar.ColumnCount = 3
        Me.layoutProjectionBar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.layoutProjectionBar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.layoutProjectionBar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutProjectionBar.Controls.Add(Me.lblHorizon, 0, 0)
        Me.layoutProjectionBar.Controls.Add(Me.nudYears, 1, 0)
        Me.layoutProjectionBar.Controls.Add(Me.lblProjectionSummary, 2, 0)
        Me.layoutProjectionBar.Dock = System.Windows.Forms.DockStyle.Top
        Me.layoutProjectionBar.Location = New System.Drawing.Point(0, 0)
        Me.layoutProjectionBar.Name = "layoutProjectionBar"
        Me.layoutProjectionBar.Padding = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.layoutProjectionBar.RowCount = 1
        Me.layoutProjectionBar.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutProjectionBar.Size = New System.Drawing.Size(1092, 41)
        Me.layoutProjectionBar.TabIndex = 0
        '
        ' lblHorizon
        '
        Me.lblHorizon.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblHorizon.AutoSize = True
        Me.lblHorizon.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.lblHorizon.Location = New System.Drawing.Point(0, 7)
        Me.lblHorizon.Margin = New System.Windows.Forms.Padding(0, 0, 6, 0)
        Me.lblHorizon.Name = "lblHorizon"
        Me.lblHorizon.Size = New System.Drawing.Size(170, 17)
        Me.lblHorizon.TabIndex = 0
        Me.lblHorizon.Text = "Projection &horizon (years):"
        '
        ' nudYears
        '
        Me.nudYears.AccessibleName = "Projection horizon in years"
        Me.nudYears.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudYears.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudYears.Location = New System.Drawing.Point(176, 2)
        Me.nudYears.Margin = New System.Windows.Forms.Padding(0, 0, 18, 0)
        Me.nudYears.Maximum = New Decimal(New Integer() {30, 0, 0, 0})
        Me.nudYears.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudYears.Name = "nudYears"
        Me.nudYears.Size = New System.Drawing.Size(64, 26)
        Me.nudYears.TabIndex = 1
        Me.nudYears.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudYears.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        ' lblProjectionSummary
        '
        Me.lblProjectionSummary.AutoEllipsis = True
        Me.lblProjectionSummary.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblProjectionSummary.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProjectionSummary.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.lblProjectionSummary.Location = New System.Drawing.Point(258, 0)
        Me.lblProjectionSummary.Margin = New System.Windows.Forms.Padding(0)
        Me.lblProjectionSummary.Name = "lblProjectionSummary"
        Me.lblProjectionSummary.Size = New System.Drawing.Size(834, 31)
        Me.lblProjectionSummary.TabIndex = 2
        Me.lblProjectionSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblProjectionSummary.UseMnemonic = False
        '
        ' pnlScenarios
        '
        Me.pnlScenarios.BackColor = System.Drawing.Color.White
        Me.pnlScenarios.Controls.Add(Me.gridScenarios)
        Me.pnlScenarios.Controls.Add(Me.lblScenariosEmpty)
        Me.pnlScenarios.Controls.Add(Me.flowScenarioActions)
        Me.pnlScenarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlScenarios.Location = New System.Drawing.Point(18, 14)
        Me.pnlScenarios.Name = "pnlScenarios"
        Me.pnlScenarios.Size = New System.Drawing.Size(1092, 374)
        Me.pnlScenarios.TabIndex = 2
        Me.pnlScenarios.Visible = False
        '
        ' gridScenarios
        '
        Me.gridScenarios.AccessibleName = "Saved scenarios"
        Me.gridScenarios.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colScenarioName, Me.colScenarioSpecies, Me.colScenarioTrees, Me.colScenarioYears, Me.colScenarioSales, Me.colScenarioSaved})
        Me.gridScenarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridScenarios.Location = New System.Drawing.Point(0, 46)
        Me.gridScenarios.Name = "gridScenarios"
        Me.gridScenarios.Size = New System.Drawing.Size(1092, 328)
        Me.gridScenarios.TabIndex = 1
        '
        ' colScenarioName
        '
        Me.colScenarioName.FillWeight = 150.0!
        Me.colScenarioName.HeaderText = "Scenario"
        Me.colScenarioName.Name = "colScenarioName"
        Me.colScenarioName.ReadOnly = True
        '
        ' colScenarioSpecies
        '
        Me.colScenarioSpecies.FillWeight = 130.0!
        Me.colScenarioSpecies.HeaderText = "Species"
        Me.colScenarioSpecies.Name = "colScenarioSpecies"
        Me.colScenarioSpecies.ReadOnly = True
        '
        ' colScenarioTrees
        '
        Me.colScenarioTrees.FillWeight = 70.0!
        Me.colScenarioTrees.HeaderText = "Trees"
        Me.colScenarioTrees.Name = "colScenarioTrees"
        Me.colScenarioTrees.ReadOnly = True
        '
        ' colScenarioYears
        '
        Me.colScenarioYears.FillWeight = 70.0!
        Me.colScenarioYears.HeaderText = "Horizon"
        Me.colScenarioYears.Name = "colScenarioYears"
        Me.colScenarioYears.ReadOnly = True
        '
        ' colScenarioSales
        '
        Me.colScenarioSales.FillWeight = 110.0!
        Me.colScenarioSales.HeaderText = "Est. Annual Sales"
        Me.colScenarioSales.Name = "colScenarioSales"
        Me.colScenarioSales.ReadOnly = True
        Me.colScenarioSales.ToolTipText = "Recalculated with the current species data"
        '
        ' colScenarioSaved
        '
        Me.colScenarioSaved.FillWeight = 120.0!
        Me.colScenarioSaved.HeaderText = "Saved On"
        Me.colScenarioSaved.Name = "colScenarioSaved"
        Me.colScenarioSaved.ReadOnly = True
        '
        ' lblScenariosEmpty
        '
        Me.lblScenariosEmpty.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblScenariosEmpty.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScenariosEmpty.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblScenariosEmpty.Location = New System.Drawing.Point(0, 46)
        Me.lblScenariosEmpty.Name = "lblScenariosEmpty"
        Me.lblScenariosEmpty.Size = New System.Drawing.Size(1092, 328)
        Me.lblScenariosEmpty.TabIndex = 2
        Me.lblScenariosEmpty.Text = "No saved scenarios yet. Choose a species and tree count, then click Save Scenario."
        Me.lblScenariosEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblScenariosEmpty.Visible = False
        '
        ' flowScenarioActions
        '
        Me.flowScenarioActions.AutoSize = True
        Me.flowScenarioActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowScenarioActions.Controls.Add(Me.btnLoadScenario)
        Me.flowScenarioActions.Controls.Add(Me.btnDeleteScenario)
        Me.flowScenarioActions.Controls.Add(Me.lblScenarioHint)
        Me.flowScenarioActions.Dock = System.Windows.Forms.DockStyle.Top
        Me.flowScenarioActions.Location = New System.Drawing.Point(0, 0)
        Me.flowScenarioActions.Name = "flowScenarioActions"
        Me.flowScenarioActions.Padding = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.flowScenarioActions.Size = New System.Drawing.Size(1092, 46)
        Me.flowScenarioActions.TabIndex = 0
        Me.flowScenarioActions.WrapContents = False
        '
        ' btnLoadScenario
        '
        Me.btnLoadScenario.AutoSize = True
        Me.btnLoadScenario.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnLoadScenario.ChipStyle = TreeYield.WinForms.ChipStyle.Primary
        Me.btnLoadScenario.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadScenario.Location = New System.Drawing.Point(0, 0)
        Me.btnLoadScenario.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.btnLoadScenario.Name = "btnLoadScenario"
        Me.btnLoadScenario.Size = New System.Drawing.Size(122, 36)
        Me.btnLoadScenario.TabIndex = 0
        Me.btnLoadScenario.Text = "Load Selected"
        '
        ' btnDeleteScenario
        '
        Me.btnDeleteScenario.AutoSize = True
        Me.btnDeleteScenario.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnDeleteScenario.ChipStyle = TreeYield.WinForms.ChipStyle.Danger
        Me.btnDeleteScenario.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeleteScenario.Location = New System.Drawing.Point(130, 0)
        Me.btnDeleteScenario.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.btnDeleteScenario.Name = "btnDeleteScenario"
        Me.btnDeleteScenario.Size = New System.Drawing.Size(128, 36)
        Me.btnDeleteScenario.TabIndex = 1
        Me.btnDeleteScenario.Text = "Delete Selected"
        '
        ' lblScenarioHint
        '
        Me.lblScenarioHint.AutoSize = True
        Me.lblScenarioHint.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblScenarioHint.Location = New System.Drawing.Point(274, 9)
        Me.lblScenarioHint.Margin = New System.Windows.Forms.Padding(8, 9, 0, 0)
        Me.lblScenarioHint.Name = "lblScenarioHint"
        Me.lblScenarioHint.Size = New System.Drawing.Size(330, 17)
        Me.lblScenarioHint.TabIndex = 2
        Me.lblScenarioHint.Text = "Double-click a scenario (or press Enter) to load it into the calculator."
        '
        ' statusStrip
        '
        Me.statusStrip.BackColor = System.Drawing.Color.White
        Me.statusStrip.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.statusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus, Me.lblDatabase, Me.lblAdmin})
        Me.statusStrip.Location = New System.Drawing.Point(0, 836)
        Me.statusStrip.Name = "statusStrip"
        Me.statusStrip.Size = New System.Drawing.Size(1184, 25)
        Me.statusStrip.SizingGrip = False
        Me.statusStrip.TabIndex = 1
        '
        ' lblStatus
        '
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(940, 20)
        Me.lblStatus.Spring = True
        Me.lblStatus.Text = "Starting..."
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        ' lblDatabase
        '
        Me.lblDatabase.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left
        Me.lblDatabase.Name = "lblDatabase"
        Me.lblDatabase.Padding = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.lblDatabase.Size = New System.Drawing.Size(120, 20)
        Me.lblDatabase.Text = "Database: connecting"
        '
        ' lblAdmin
        '
        Me.lblAdmin.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left
        Me.lblAdmin.Name = "lblAdmin"
        Me.lblAdmin.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.lblAdmin.Size = New System.Drawing.Size(110, 20)
        Me.lblAdmin.Text = "Guest (read-only)"
        '
        ' MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.FromArgb(247, 247, 248)
        Me.ClientSize = New System.Drawing.Size(1184, 861)
        Me.Controls.Add(Me.layoutRoot)
        Me.Controls.Add(Me.statusStrip)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimumSize = New System.Drawing.Size(960, 640)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tree Yield & Revenue Calculator"
        Me.layoutRoot.ResumeLayout(False)
        Me.layoutRoot.PerformLayout()
        Me.layoutHeader.ResumeLayout(False)
        Me.layoutHeader.PerformLayout()
        Me.flowActions.ResumeLayout(False)
        Me.flowActions.PerformLayout()
        Me.flowFilters.ResumeLayout(False)
        Me.flowFilters.PerformLayout()
        Me.pnlCalculator.ResumeLayout(False)
        Me.layoutCalculator.ResumeLayout(False)
        Me.layoutCalculator.PerformLayout()
        Me.layoutInputs.ResumeLayout(False)
        Me.layoutInputs.PerformLayout()
        Me.layoutTreeCount.ResumeLayout(False)
        CType(Me.nudTreeCount, System.ComponentModel.ISupportInitialize).EndInit()
        Me.layoutCards.ResumeLayout(False)
        Me.layoutSectionBar.ResumeLayout(False)
        Me.layoutSectionBar.PerformLayout()
        Me.flowSections.ResumeLayout(False)
        Me.flowSections.PerformLayout()
        Me.pnlContent.ResumeLayout(False)
        Me.pnlRecords.ResumeLayout(False)
        Me.pnlRecords.PerformLayout()
        CType(Me.gridRecords, System.ComponentModel.ISupportInitialize).EndInit()
        Me.layoutRecordsBar.ResumeLayout(False)
        Me.layoutRecordsBar.PerformLayout()
        Me.pnlProjection.ResumeLayout(False)
        Me.pnlProjection.PerformLayout()
        Me.layoutProjectionBody.ResumeLayout(False)
        CType(Me.chartProjection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridProjection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.layoutProjectionBar.ResumeLayout(False)
        Me.layoutProjectionBar.PerformLayout()
        CType(Me.nudYears, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlScenarios.ResumeLayout(False)
        Me.pnlScenarios.PerformLayout()
        CType(Me.gridScenarios, System.ComponentModel.ISupportInitialize).EndInit()
        Me.flowScenarioActions.ResumeLayout(False)
        Me.flowScenarioActions.PerformLayout()
        Me.statusStrip.ResumeLayout(False)
        Me.statusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents toolTip As System.Windows.Forms.ToolTip
    Friend WithEvents searchTimer As System.Windows.Forms.Timer
    Friend WithEvents layoutRoot As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents layoutHeader As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblSubtitle As System.Windows.Forms.Label
    Friend WithEvents flowActions As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnSaveScenario As TreeYield.WinForms.ChipButton
    Friend WithEvents btnExportCsv As TreeYield.WinForms.ChipButton
    Friend WithEvents btnPrint As TreeYield.WinForms.ChipButton
    Friend WithEvents btnAbout As TreeYield.WinForms.ChipButton
    Friend WithEvents btnAdmin As TreeYield.WinForms.ChipButton
    Friend WithEvents flowFilters As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents chipAll As TreeYield.WinForms.ChipButton
    Friend WithEvents chipFruit As TreeYield.WinForms.ChipButton
    Friend WithEvents chipNonFruit As TreeYield.WinForms.ChipButton
    Friend WithEvents pnlCalculator As TreeYield.WinForms.RoundedPanel
    Friend WithEvents layoutCalculator As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents layoutInputs As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblSpecies As System.Windows.Forms.Label
    Friend WithEvents lblTreeCount As System.Windows.Forms.Label
    Friend WithEvents cboSpecies As System.Windows.Forms.ComboBox
    Friend WithEvents layoutTreeCount As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents sliderTreeCount As TreeYield.WinForms.FlatSlider
    Friend WithEvents nudTreeCount As System.Windows.Forms.NumericUpDown
    Friend WithEvents layoutCards As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cardSpecies As TreeYield.WinForms.StatCard
    Friend WithEvents cardYield As TreeYield.WinForms.StatCard
    Friend WithEvents cardSales As TreeYield.WinForms.StatCard
    Friend WithEvents layoutSectionBar As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flowSections As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents chipRecords As TreeYield.WinForms.ChipButton
    Friend WithEvents chipProjection As TreeYield.WinForms.ChipButton
    Friend WithEvents chipScenarios As TreeYield.WinForms.ChipButton
    Friend WithEvents lblDisclaimer As System.Windows.Forms.Label
    Friend WithEvents pnlContent As TreeYield.WinForms.RoundedPanel
    Friend WithEvents pnlRecords As System.Windows.Forms.Panel
    Friend WithEvents gridRecords As System.Windows.Forms.DataGridView
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colScientific As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCategory As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colProduct As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colYield As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colUnit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPrice As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFirstHarvest As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colHarvestType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblRecordsEmpty As System.Windows.Forms.Label
    Friend WithEvents layoutRecordsBar As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents lblRecordCount As System.Windows.Forms.Label
    Friend WithEvents pnlProjection As System.Windows.Forms.Panel
    Friend WithEvents layoutProjectionBody As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents chartProjection As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents gridProjection As System.Windows.Forms.DataGridView
    Friend WithEvents colProjYear As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colProjStage As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colProjYield As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colProjSales As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colProjCumulative As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents layoutProjectionBar As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblHorizon As System.Windows.Forms.Label
    Friend WithEvents nudYears As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblProjectionSummary As System.Windows.Forms.Label
    Friend WithEvents pnlScenarios As System.Windows.Forms.Panel
    Friend WithEvents gridScenarios As System.Windows.Forms.DataGridView
    Friend WithEvents colScenarioName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colScenarioSpecies As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colScenarioTrees As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colScenarioYears As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colScenarioSales As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colScenarioSaved As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblScenariosEmpty As System.Windows.Forms.Label
    Friend WithEvents flowScenarioActions As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnLoadScenario As TreeYield.WinForms.ChipButton
    Friend WithEvents btnDeleteScenario As TreeYield.WinForms.ChipButton
    Friend WithEvents lblScenarioHint As System.Windows.Forms.Label
    Friend WithEvents statusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents lblStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblDatabase As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblAdmin As System.Windows.Forms.ToolStripStatusLabel
End Class
