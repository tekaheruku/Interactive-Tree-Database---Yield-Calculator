<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AdminForm
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
        Me.toolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.layoutRoot = New System.Windows.Forms.TableLayoutPanel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblSignedIn = New System.Windows.Forms.Label()
        Me.flowSections = New System.Windows.Forms.FlowLayoutPanel()
        Me.chipSpecies = New TreeYield.WinForms.ChipButton()
        Me.chipAudit = New TreeYield.WinForms.ChipButton()
        Me.pnlContent = New TreeYield.WinForms.RoundedPanel()
        Me.pnlSpecies = New System.Windows.Forms.Panel()
        Me.gridSpecies = New System.Windows.Forms.DataGridView()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colCategory = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colProduct = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colYield = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUnit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colPrice = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colHarvestType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colYears = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUpdated = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblSpeciesEmpty = New System.Windows.Forms.Label()
        Me.layoutSpeciesBar = New System.Windows.Forms.TableLayoutPanel()
        Me.flowSpeciesActions = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnAdd = New TreeYield.WinForms.ChipButton()
        Me.btnEdit = New TreeYield.WinForms.ChipButton()
        Me.btnDelete = New TreeYield.WinForms.ChipButton()
        Me.btnRefresh = New TreeYield.WinForms.ChipButton()
        Me.lblSpeciesCount = New System.Windows.Forms.Label()
        Me.txtFilter = New System.Windows.Forms.TextBox()
        Me.pnlAudit = New System.Windows.Forms.Panel()
        Me.gridAudit = New System.Windows.Forms.DataGridView()
        Me.colAuditWhen = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAuditUser = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAuditAction = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAuditRecord = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAuditDetails = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblAuditHint = New System.Windows.Forms.Label()
        Me.layoutFooter = New System.Windows.Forms.TableLayoutPanel()
        Me.flowFooterLeft = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnChangePassword = New TreeYield.WinForms.ChipButton()
        Me.btnResetData = New TreeYield.WinForms.ChipButton()
        Me.flowFooterRight = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnClose = New TreeYield.WinForms.ChipButton()
        Me.btnSignOut = New TreeYield.WinForms.ChipButton()
        Me.layoutRoot.SuspendLayout()
        Me.flowSections.SuspendLayout()
        Me.pnlContent.SuspendLayout()
        Me.pnlSpecies.SuspendLayout()
        CType(Me.gridSpecies, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.layoutSpeciesBar.SuspendLayout()
        Me.flowSpeciesActions.SuspendLayout()
        Me.pnlAudit.SuspendLayout()
        CType(Me.gridAudit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.layoutFooter.SuspendLayout()
        Me.flowFooterLeft.SuspendLayout()
        Me.flowFooterRight.SuspendLayout()
        Me.SuspendLayout()
        '
        ' layoutRoot
        '
        Me.layoutRoot.BackColor = System.Drawing.Color.FromArgb(247, 247, 248)
        Me.layoutRoot.ColumnCount = 1
        Me.layoutRoot.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutRoot.Controls.Add(Me.lblTitle, 0, 0)
        Me.layoutRoot.Controls.Add(Me.lblSignedIn, 0, 1)
        Me.layoutRoot.Controls.Add(Me.flowSections, 0, 2)
        Me.layoutRoot.Controls.Add(Me.pnlContent, 0, 3)
        Me.layoutRoot.Controls.Add(Me.layoutFooter, 0, 4)
        Me.layoutRoot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutRoot.Location = New System.Drawing.Point(0, 0)
        Me.layoutRoot.Name = "layoutRoot"
        Me.layoutRoot.Padding = New System.Windows.Forms.Padding(28, 18, 28, 16)
        Me.layoutRoot.RowCount = 5
        Me.layoutRoot.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutRoot.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutRoot.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutRoot.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutRoot.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutRoot.Size = New System.Drawing.Size(1144, 721)
        Me.layoutRoot.TabIndex = 0
        '
        ' lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.lblTitle.Location = New System.Drawing.Point(28, 18)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(282, 32)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Manage Species Records"
        '
        ' lblSignedIn
        '
        Me.lblSignedIn.AutoSize = True
        Me.lblSignedIn.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblSignedIn.Location = New System.Drawing.Point(28, 52)
        Me.lblSignedIn.Margin = New System.Windows.Forms.Padding(1, 2, 0, 12)
        Me.lblSignedIn.Name = "lblSignedIn"
        Me.lblSignedIn.Size = New System.Drawing.Size(90, 17)
        Me.lblSignedIn.TabIndex = 1
        Me.lblSignedIn.Text = "Signed in as -"
        Me.lblSignedIn.UseMnemonic = False
        '
        ' flowSections
        '
        Me.flowSections.AutoSize = True
        Me.flowSections.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowSections.Controls.Add(Me.chipSpecies)
        Me.flowSections.Controls.Add(Me.chipAudit)
        Me.flowSections.Location = New System.Drawing.Point(28, 81)
        Me.flowSections.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.flowSections.Name = "flowSections"
        Me.flowSections.Size = New System.Drawing.Size(220, 38)
        Me.flowSections.TabIndex = 2
        Me.flowSections.WrapContents = False
        '
        ' chipSpecies
        '
        Me.chipSpecies.AutoSize = True
        Me.chipSpecies.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.chipSpecies.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chipSpecies.Location = New System.Drawing.Point(0, 0)
        Me.chipSpecies.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.chipSpecies.Name = "chipSpecies"
        Me.chipSpecies.Selected = True
        Me.chipSpecies.Size = New System.Drawing.Size(92, 38)
        Me.chipSpecies.TabIndex = 0
        Me.chipSpecies.Text = "Species"
        '
        ' chipAudit
        '
        Me.chipAudit.AutoSize = True
        Me.chipAudit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.chipAudit.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chipAudit.Location = New System.Drawing.Point(100, 0)
        Me.chipAudit.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.chipAudit.Name = "chipAudit"
        Me.chipAudit.Size = New System.Drawing.Size(104, 38)
        Me.chipAudit.TabIndex = 1
        Me.chipAudit.Text = "Audit Log"
        Me.toolTip.SetToolTip(Me.chipAudit, "History of sign-ins and changes to the species records")
        '
        ' pnlContent
        '
        Me.pnlContent.BackColor = System.Drawing.Color.White
        Me.pnlContent.BorderColor = System.Drawing.Color.FromArgb(226, 226, 226)
        Me.pnlContent.Controls.Add(Me.pnlSpecies)
        Me.pnlContent.Controls.Add(Me.pnlAudit)
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(28, 129)
        Me.pnlContent.Margin = New System.Windows.Forms.Padding(0, 0, 0, 14)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Padding = New System.Windows.Forms.Padding(18, 14, 18, 14)
        Me.pnlContent.Size = New System.Drawing.Size(1088, 526)
        Me.pnlContent.TabIndex = 3
        '
        ' pnlSpecies
        '
        Me.pnlSpecies.BackColor = System.Drawing.Color.White
        Me.pnlSpecies.Controls.Add(Me.gridSpecies)
        Me.pnlSpecies.Controls.Add(Me.lblSpeciesEmpty)
        Me.pnlSpecies.Controls.Add(Me.layoutSpeciesBar)
        Me.pnlSpecies.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSpecies.Location = New System.Drawing.Point(18, 14)
        Me.pnlSpecies.Name = "pnlSpecies"
        Me.pnlSpecies.Size = New System.Drawing.Size(1052, 498)
        Me.pnlSpecies.TabIndex = 0
        '
        ' gridSpecies
        '
        Me.gridSpecies.AccessibleName = "Species records"
        Me.gridSpecies.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colName, Me.colCategory, Me.colProduct, Me.colYield, Me.colUnit, Me.colPrice, Me.colHarvestType, Me.colYears, Me.colUpdated})
        Me.gridSpecies.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridSpecies.Location = New System.Drawing.Point(0, 46)
        Me.gridSpecies.Name = "gridSpecies"
        Me.gridSpecies.Size = New System.Drawing.Size(1052, 452)
        Me.gridSpecies.TabIndex = 1
        Me.toolTip.SetToolTip(Me.gridSpecies, "Double-click or press Enter to edit. Press Delete to delete.")
        '
        ' colName
        '
        Me.colName.FillWeight = 150.0!
        Me.colName.HeaderText = "Species"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
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
        Me.colProduct.FillWeight = 115.0!
        Me.colProduct.HeaderText = "Product"
        Me.colProduct.Name = "colProduct"
        Me.colProduct.ReadOnly = True
        '
        ' colYield
        '
        Me.colYield.FillWeight = 90.0!
        Me.colYield.HeaderText = "Yield / Tree / Yr"
        Me.colYield.Name = "colYield"
        Me.colYield.ReadOnly = True
        '
        ' colUnit
        '
        Me.colUnit.FillWeight = 60.0!
        Me.colUnit.HeaderText = "Unit"
        Me.colUnit.Name = "colUnit"
        Me.colUnit.ReadOnly = True
        '
        ' colPrice
        '
        Me.colPrice.FillWeight = 85.0!
        Me.colPrice.HeaderText = "Price / Unit"
        Me.colPrice.Name = "colPrice"
        Me.colPrice.ReadOnly = True
        '
        ' colHarvestType
        '
        Me.colHarvestType.FillWeight = 75.0!
        Me.colHarvestType.HeaderText = "Harvest"
        Me.colHarvestType.Name = "colHarvestType"
        Me.colHarvestType.ReadOnly = True
        '
        ' colYears
        '
        Me.colYears.FillWeight = 80.0!
        Me.colYears.HeaderText = "Years (First / Full)"
        Me.colYears.Name = "colYears"
        Me.colYears.ReadOnly = True
        '
        ' colUpdated
        '
        Me.colUpdated.FillWeight = 120.0!
        Me.colUpdated.HeaderText = "Last Updated"
        Me.colUpdated.Name = "colUpdated"
        Me.colUpdated.ReadOnly = True
        '
        ' lblSpeciesEmpty
        '
        Me.lblSpeciesEmpty.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblSpeciesEmpty.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpeciesEmpty.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblSpeciesEmpty.Location = New System.Drawing.Point(0, 46)
        Me.lblSpeciesEmpty.Name = "lblSpeciesEmpty"
        Me.lblSpeciesEmpty.Size = New System.Drawing.Size(1052, 452)
        Me.lblSpeciesEmpty.TabIndex = 2
        Me.lblSpeciesEmpty.Text = "No species match the filter."
        Me.lblSpeciesEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblSpeciesEmpty.UseMnemonic = False
        Me.lblSpeciesEmpty.Visible = False
        '
        ' layoutSpeciesBar
        '
        Me.layoutSpeciesBar.AutoSize = True
        Me.layoutSpeciesBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutSpeciesBar.ColumnCount = 3
        Me.layoutSpeciesBar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.layoutSpeciesBar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutSpeciesBar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.layoutSpeciesBar.Controls.Add(Me.flowSpeciesActions, 0, 0)
        Me.layoutSpeciesBar.Controls.Add(Me.lblSpeciesCount, 1, 0)
        Me.layoutSpeciesBar.Controls.Add(Me.txtFilter, 2, 0)
        Me.layoutSpeciesBar.Dock = System.Windows.Forms.DockStyle.Top
        Me.layoutSpeciesBar.Location = New System.Drawing.Point(0, 0)
        Me.layoutSpeciesBar.Name = "layoutSpeciesBar"
        Me.layoutSpeciesBar.Padding = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.layoutSpeciesBar.RowCount = 1
        Me.layoutSpeciesBar.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutSpeciesBar.Size = New System.Drawing.Size(1052, 46)
        Me.layoutSpeciesBar.TabIndex = 0
        '
        ' flowSpeciesActions
        '
        Me.flowSpeciesActions.AutoSize = True
        Me.flowSpeciesActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowSpeciesActions.Controls.Add(Me.btnAdd)
        Me.flowSpeciesActions.Controls.Add(Me.btnEdit)
        Me.flowSpeciesActions.Controls.Add(Me.btnDelete)
        Me.flowSpeciesActions.Controls.Add(Me.btnRefresh)
        Me.flowSpeciesActions.Location = New System.Drawing.Point(0, 0)
        Me.flowSpeciesActions.Margin = New System.Windows.Forms.Padding(0)
        Me.flowSpeciesActions.Name = "flowSpeciesActions"
        Me.flowSpeciesActions.Size = New System.Drawing.Size(400, 36)
        Me.flowSpeciesActions.TabIndex = 0
        Me.flowSpeciesActions.WrapContents = False
        '
        ' btnAdd
        '
        Me.btnAdd.AutoSize = True
        Me.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnAdd.ChipStyle = TreeYield.WinForms.ChipStyle.Primary
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(0, 0)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(110, 36)
        Me.btnAdd.TabIndex = 0
        Me.btnAdd.Text = "Add Species"
        Me.toolTip.SetToolTip(Me.btnAdd, "Add a new species record (Ctrl+N)")
        '
        ' btnEdit
        '
        Me.btnEdit.AutoSize = True
        Me.btnEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnEdit.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnEdit.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.Location = New System.Drawing.Point(118, 0)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(64, 36)
        Me.btnEdit.TabIndex = 1
        Me.btnEdit.Text = "Edit"
        '
        ' btnDelete
        '
        Me.btnDelete.AutoSize = True
        Me.btnDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnDelete.ChipStyle = TreeYield.WinForms.ChipStyle.Danger
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Location = New System.Drawing.Point(190, 0)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(74, 36)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "Delete"
        '
        ' btnRefresh
        '
        Me.btnRefresh.AutoSize = True
        Me.btnRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnRefresh.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnRefresh.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.Location = New System.Drawing.Point(272, 0)
        Me.btnRefresh.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(84, 36)
        Me.btnRefresh.TabIndex = 3
        Me.btnRefresh.Text = "Refresh"
        Me.toolTip.SetToolTip(Me.btnRefresh, "Reload the records from the database (F5)")
        '
        ' lblSpeciesCount
        '
        Me.lblSpeciesCount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblSpeciesCount.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblSpeciesCount.Location = New System.Drawing.Point(400, 0)
        Me.lblSpeciesCount.Margin = New System.Windows.Forms.Padding(0, 0, 12, 0)
        Me.lblSpeciesCount.Name = "lblSpeciesCount"
        Me.lblSpeciesCount.Size = New System.Drawing.Size(360, 36)
        Me.lblSpeciesCount.TabIndex = 1
        Me.lblSpeciesCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        ' txtFilter
        '
        Me.txtFilter.AccessibleName = "Filter species by name"
        Me.txtFilter.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.txtFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFilter.Location = New System.Drawing.Point(772, 5)
        Me.txtFilter.Margin = New System.Windows.Forms.Padding(0)
        Me.txtFilter.MaxLength = 100
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(280, 25)
        Me.txtFilter.TabIndex = 2
        '
        ' pnlAudit
        '
        Me.pnlAudit.BackColor = System.Drawing.Color.White
        Me.pnlAudit.Controls.Add(Me.gridAudit)
        Me.pnlAudit.Controls.Add(Me.lblAuditHint)
        Me.pnlAudit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlAudit.Location = New System.Drawing.Point(18, 14)
        Me.pnlAudit.Name = "pnlAudit"
        Me.pnlAudit.Size = New System.Drawing.Size(1052, 498)
        Me.pnlAudit.TabIndex = 1
        Me.pnlAudit.Visible = False
        '
        ' gridAudit
        '
        Me.gridAudit.AccessibleName = "Audit log"
        Me.gridAudit.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colAuditWhen, Me.colAuditUser, Me.colAuditAction, Me.colAuditRecord, Me.colAuditDetails})
        Me.gridAudit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridAudit.Location = New System.Drawing.Point(0, 27)
        Me.gridAudit.Name = "gridAudit"
        Me.gridAudit.Size = New System.Drawing.Size(1052, 471)
        Me.gridAudit.TabIndex = 1
        '
        ' colAuditWhen
        '
        Me.colAuditWhen.FillWeight = 90.0!
        Me.colAuditWhen.HeaderText = "When"
        Me.colAuditWhen.Name = "colAuditWhen"
        Me.colAuditWhen.ReadOnly = True
        '
        ' colAuditUser
        '
        Me.colAuditUser.FillWeight = 60.0!
        Me.colAuditUser.HeaderText = "User"
        Me.colAuditUser.Name = "colAuditUser"
        Me.colAuditUser.ReadOnly = True
        '
        ' colAuditAction
        '
        Me.colAuditAction.FillWeight = 75.0!
        Me.colAuditAction.HeaderText = "Action"
        Me.colAuditAction.Name = "colAuditAction"
        Me.colAuditAction.ReadOnly = True
        '
        ' colAuditRecord
        '
        Me.colAuditRecord.FillWeight = 75.0!
        Me.colAuditRecord.HeaderText = "Record"
        Me.colAuditRecord.Name = "colAuditRecord"
        Me.colAuditRecord.ReadOnly = True
        '
        ' colAuditDetails
        '
        Me.colAuditDetails.FillWeight = 250.0!
        Me.colAuditDetails.HeaderText = "Details"
        Me.colAuditDetails.Name = "colAuditDetails"
        Me.colAuditDetails.ReadOnly = True
        '
        ' lblAuditHint
        '
        Me.lblAuditHint.AutoSize = True
        Me.lblAuditHint.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblAuditHint.ForeColor = System.Drawing.Color.FromArgb(95, 99, 104)
        Me.lblAuditHint.Location = New System.Drawing.Point(0, 0)
        Me.lblAuditHint.Name = "lblAuditHint"
        Me.lblAuditHint.Padding = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.lblAuditHint.Size = New System.Drawing.Size(380, 27)
        Me.lblAuditHint.TabIndex = 0
        Me.lblAuditHint.Text = "The 200 most recent sign-ins and changes, newest first. Times are shown in local time."
        '
        ' layoutFooter
        '
        Me.layoutFooter.AutoSize = True
        Me.layoutFooter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutFooter.ColumnCount = 2
        Me.layoutFooter.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutFooter.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.layoutFooter.Controls.Add(Me.flowFooterLeft, 0, 0)
        Me.layoutFooter.Controls.Add(Me.flowFooterRight, 1, 0)
        Me.layoutFooter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutFooter.Location = New System.Drawing.Point(28, 669)
        Me.layoutFooter.Margin = New System.Windows.Forms.Padding(0)
        Me.layoutFooter.Name = "layoutFooter"
        Me.layoutFooter.RowCount = 1
        Me.layoutFooter.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.layoutFooter.Size = New System.Drawing.Size(1088, 36)
        Me.layoutFooter.TabIndex = 4
        '
        ' flowFooterLeft
        '
        Me.flowFooterLeft.AutoSize = True
        Me.flowFooterLeft.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowFooterLeft.Controls.Add(Me.btnChangePassword)
        Me.flowFooterLeft.Controls.Add(Me.btnResetData)
        Me.flowFooterLeft.Location = New System.Drawing.Point(0, 0)
        Me.flowFooterLeft.Margin = New System.Windows.Forms.Padding(0)
        Me.flowFooterLeft.Name = "flowFooterLeft"
        Me.flowFooterLeft.Size = New System.Drawing.Size(330, 36)
        Me.flowFooterLeft.TabIndex = 0
        Me.flowFooterLeft.WrapContents = False
        '
        ' btnChangePassword
        '
        Me.btnChangePassword.AutoSize = True
        Me.btnChangePassword.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnChangePassword.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnChangePassword.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChangePassword.Location = New System.Drawing.Point(0, 0)
        Me.btnChangePassword.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.btnChangePassword.Name = "btnChangePassword"
        Me.btnChangePassword.Size = New System.Drawing.Size(146, 36)
        Me.btnChangePassword.TabIndex = 0
        Me.btnChangePassword.Text = "Change Password"
        '
        ' btnResetData
        '
        Me.btnResetData.AutoSize = True
        Me.btnResetData.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnResetData.ChipStyle = TreeYield.WinForms.ChipStyle.Danger
        Me.btnResetData.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnResetData.Location = New System.Drawing.Point(154, 0)
        Me.btnResetData.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.btnResetData.Name = "btnResetData"
        Me.btnResetData.Size = New System.Drawing.Size(150, 36)
        Me.btnResetData.TabIndex = 1
        Me.btnResetData.Text = "Reset Sample Data"
        Me.toolTip.SetToolTip(Me.btnResetData, "Delete all species and saved scenarios, then restore the original sample species")
        '
        ' flowFooterRight
        '
        Me.flowFooterRight.AutoSize = True
        Me.flowFooterRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowFooterRight.Controls.Add(Me.btnClose)
        Me.flowFooterRight.Controls.Add(Me.btnSignOut)
        Me.flowFooterRight.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flowFooterRight.Location = New System.Drawing.Point(900, 0)
        Me.flowFooterRight.Margin = New System.Windows.Forms.Padding(0)
        Me.flowFooterRight.Name = "flowFooterRight"
        Me.flowFooterRight.Size = New System.Drawing.Size(188, 36)
        Me.flowFooterRight.TabIndex = 1
        Me.flowFooterRight.WrapContents = False
        '
        ' btnClose
        '
        Me.btnClose.AutoSize = True
        Me.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnClose.ChipStyle = TreeYield.WinForms.ChipStyle.Primary
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(116, 0)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(72, 36)
        Me.btnClose.TabIndex = 0
        Me.btnClose.Text = "Close"
        '
        ' btnSignOut
        '
        Me.btnSignOut.AutoSize = True
        Me.btnSignOut.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnSignOut.ChipStyle = TreeYield.WinForms.ChipStyle.Secondary
        Me.btnSignOut.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSignOut.Location = New System.Drawing.Point(20, 0)
        Me.btnSignOut.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnSignOut.Name = "btnSignOut"
        Me.btnSignOut.Size = New System.Drawing.Size(88, 36)
        Me.btnSignOut.TabIndex = 1
        Me.btnSignOut.Text = "Sign Out"
        '
        ' AdminForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.FromArgb(247, 247, 248)
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(1144, 721)
        Me.Controls.Add(Me.layoutRoot)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(980, 620)
        Me.Name = "AdminForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Manage Species Records"
        Me.layoutRoot.ResumeLayout(False)
        Me.layoutRoot.PerformLayout()
        Me.flowSections.ResumeLayout(False)
        Me.flowSections.PerformLayout()
        Me.pnlContent.ResumeLayout(False)
        Me.pnlSpecies.ResumeLayout(False)
        Me.pnlSpecies.PerformLayout()
        CType(Me.gridSpecies, System.ComponentModel.ISupportInitialize).EndInit()
        Me.layoutSpeciesBar.ResumeLayout(False)
        Me.layoutSpeciesBar.PerformLayout()
        Me.flowSpeciesActions.ResumeLayout(False)
        Me.flowSpeciesActions.PerformLayout()
        Me.pnlAudit.ResumeLayout(False)
        Me.pnlAudit.PerformLayout()
        CType(Me.gridAudit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.layoutFooter.ResumeLayout(False)
        Me.layoutFooter.PerformLayout()
        Me.flowFooterLeft.ResumeLayout(False)
        Me.flowFooterLeft.PerformLayout()
        Me.flowFooterRight.ResumeLayout(False)
        Me.flowFooterRight.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents toolTip As System.Windows.Forms.ToolTip
    Friend WithEvents layoutRoot As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblSignedIn As System.Windows.Forms.Label
    Friend WithEvents flowSections As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents chipSpecies As TreeYield.WinForms.ChipButton
    Friend WithEvents chipAudit As TreeYield.WinForms.ChipButton
    Friend WithEvents pnlContent As TreeYield.WinForms.RoundedPanel
    Friend WithEvents pnlSpecies As System.Windows.Forms.Panel
    Friend WithEvents gridSpecies As System.Windows.Forms.DataGridView
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCategory As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colProduct As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colYield As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colUnit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPrice As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colHarvestType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colYears As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colUpdated As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblSpeciesEmpty As System.Windows.Forms.Label
    Friend WithEvents layoutSpeciesBar As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flowSpeciesActions As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnAdd As TreeYield.WinForms.ChipButton
    Friend WithEvents btnEdit As TreeYield.WinForms.ChipButton
    Friend WithEvents btnDelete As TreeYield.WinForms.ChipButton
    Friend WithEvents btnRefresh As TreeYield.WinForms.ChipButton
    Friend WithEvents lblSpeciesCount As System.Windows.Forms.Label
    Friend WithEvents txtFilter As System.Windows.Forms.TextBox
    Friend WithEvents pnlAudit As System.Windows.Forms.Panel
    Friend WithEvents gridAudit As System.Windows.Forms.DataGridView
    Friend WithEvents colAuditWhen As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAuditUser As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAuditAction As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAuditRecord As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAuditDetails As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblAuditHint As System.Windows.Forms.Label
    Friend WithEvents layoutFooter As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flowFooterLeft As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnChangePassword As TreeYield.WinForms.ChipButton
    Friend WithEvents btnResetData As TreeYield.WinForms.ChipButton
    Friend WithEvents flowFooterRight As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnClose As TreeYield.WinForms.ChipButton
    Friend WithEvents btnSignOut As TreeYield.WinForms.ChipButton
End Class
