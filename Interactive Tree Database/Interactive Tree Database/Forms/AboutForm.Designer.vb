<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AboutForm))
        Me.layoutMain = New System.Windows.Forms.TableLayoutPanel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblDataTitle = New System.Windows.Forms.Label()
        Me.lblData = New System.Windows.Forms.Label()
        Me.lblShortcutsTitle = New System.Windows.Forms.Label()
        Me.lblShortcuts = New System.Windows.Forms.Label()
        Me.lblCredits = New System.Windows.Forms.Label()
        Me.flowButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnOk = New TreeYield.WinForms.ChipButton()
        Me.layoutMain.SuspendLayout()
        Me.flowButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'layoutMain
        '
        Me.layoutMain.ColumnCount = 1
        Me.layoutMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutMain.Controls.Add(Me.lblTitle, 0, 0)
        Me.layoutMain.Controls.Add(Me.lblVersion, 0, 1)
        Me.layoutMain.Controls.Add(Me.lblDescription, 0, 2)
        Me.layoutMain.Controls.Add(Me.lblDataTitle, 0, 3)
        Me.layoutMain.Controls.Add(Me.lblData, 0, 4)
        Me.layoutMain.Controls.Add(Me.lblShortcutsTitle, 0, 5)
        Me.layoutMain.Controls.Add(Me.lblShortcuts, 0, 6)
        Me.layoutMain.Controls.Add(Me.lblCredits, 0, 7)
        Me.layoutMain.Controls.Add(Me.flowButtons, 0, 9)
        Me.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutMain.Location = New System.Drawing.Point(0, 0)
        Me.layoutMain.Name = "layoutMain"
        Me.layoutMain.Padding = New System.Windows.Forms.Padding(28, 22, 28, 18)
        Me.layoutMain.RowCount = 10
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
        Me.layoutMain.Size = New System.Drawing.Size(584, 581)
        Me.layoutMain.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(28, 22)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.lblTitle.MaximumSize = New System.Drawing.Size(528, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(436, 30)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Interactive Tree Database & Yield Calculator"
        Me.lblTitle.UseMnemonic = False
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(95, Byte), Integer), CType(CType(99, Byte), Integer), CType(CType(104, Byte), Integer))
        Me.lblVersion.Location = New System.Drawing.Point(29, 54)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(1, 2, 0, 14)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(72, 17)
        Me.lblVersion.TabIndex = 1
        Me.lblVersion.Text = "Version 1.0"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(28, 85)
        Me.lblDescription.Margin = New System.Windows.Forms.Padding(0, 0, 0, 14)
        Me.lblDescription.MaximumSize = New System.Drawing.Size(528, 0)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(517, 68)
        Me.lblDescription.TabIndex = 2
        Me.lblDescription.Text = resources.GetString("lblDescription.Text")
        '
        'lblDataTitle
        '
        Me.lblDataTitle.AutoSize = True
        Me.lblDataTitle.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDataTitle.Location = New System.Drawing.Point(28, 167)
        Me.lblDataTitle.Margin = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.lblDataTitle.Name = "lblDataTitle"
        Me.lblDataTitle.Size = New System.Drawing.Size(102, 19)
        Me.lblDataTitle.TabIndex = 3
        Me.lblDataTitle.Text = "About the data"
        '
        'lblData
        '
        Me.lblData.AutoSize = True
        Me.lblData.ForeColor = System.Drawing.Color.FromArgb(CType(CType(95, Byte), Integer), CType(CType(99, Byte), Integer), CType(CType(104, Byte), Integer))
        Me.lblData.Location = New System.Drawing.Point(28, 190)
        Me.lblData.Margin = New System.Windows.Forms.Padding(0, 0, 0, 14)
        Me.lblData.MaximumSize = New System.Drawing.Size(528, 0)
        Me.lblData.Name = "lblData"
        Me.lblData.Size = New System.Drawing.Size(68, 17)
        Me.lblData.TabIndex = 4
        Me.lblData.Text = "Disclaimer"
        '
        'lblShortcutsTitle
        '
        Me.lblShortcutsTitle.AutoSize = True
        Me.lblShortcutsTitle.Font = New System.Drawing.Font("Segoe UI Semibold", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShortcutsTitle.Location = New System.Drawing.Point(28, 221)
        Me.lblShortcutsTitle.Margin = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.lblShortcutsTitle.Name = "lblShortcutsTitle"
        Me.lblShortcutsTitle.Size = New System.Drawing.Size(130, 19)
        Me.lblShortcutsTitle.TabIndex = 5
        Me.lblShortcutsTitle.Text = "Keyboard shortcuts"
        '
        'lblShortcuts
        '
        Me.lblShortcuts.AutoSize = True
        Me.lblShortcuts.ForeColor = System.Drawing.Color.FromArgb(CType(CType(95, Byte), Integer), CType(CType(99, Byte), Integer), CType(CType(104, Byte), Integer))
        Me.lblShortcuts.Location = New System.Drawing.Point(28, 244)
        Me.lblShortcuts.Margin = New System.Windows.Forms.Padding(0, 0, 0, 14)
        Me.lblShortcuts.MaximumSize = New System.Drawing.Size(528, 0)
        Me.lblShortcuts.Name = "lblShortcuts"
        Me.lblShortcuts.Size = New System.Drawing.Size(398, 119)
        Me.lblShortcuts.TabIndex = 6
        Me.lblShortcuts.Text = resources.GetString("lblShortcuts.Text")
        Me.lblShortcuts.UseMnemonic = False
        '
        'lblCredits
        '
        Me.lblCredits.AutoSize = True
        Me.lblCredits.Location = New System.Drawing.Point(28, 377)
        Me.lblCredits.Margin = New System.Windows.Forms.Padding(0)
        Me.lblCredits.MaximumSize = New System.Drawing.Size(528, 0)
        Me.lblCredits.Name = "lblCredits"
        Me.lblCredits.Size = New System.Drawing.Size(409, 34)
        Me.lblCredits.TabIndex = 7
        Me.lblCredits.Text = "BSCS Final Project - Human-Computer Interaction" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Developed by: [Gian Kyle Villanu" &
    "eva / Mikhail Giangan / Dave Valdez]"
        Me.lblCredits.UseMnemonic = False
        '
        'flowButtons
        '
        Me.flowButtons.AutoSize = True
        Me.flowButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowButtons.Controls.Add(Me.btnOk)
        Me.flowButtons.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flowButtons.Location = New System.Drawing.Point(28, 530)
        Me.flowButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flowButtons.Name = "flowButtons"
        Me.flowButtons.Size = New System.Drawing.Size(528, 33)
        Me.flowButtons.TabIndex = 8
        Me.flowButtons.WrapContents = False
        '
        'btnOk
        '
        Me.btnOk.AutoSize = True
        Me.btnOk.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnOk.ChipStyle = TreeYield.WinForms.ChipStyle.Primary
        Me.btnOk.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOk.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Location = New System.Drawing.Point(466, 0)
        Me.btnOk.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Padding = New System.Windows.Forms.Padding(18, 0, 18, 0)
        Me.btnOk.Size = New System.Drawing.Size(62, 33)
        Me.btnOk.TabIndex = 0
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'AboutForm
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btnOk
        Me.ClientSize = New System.Drawing.Size(584, 581)
        Me.Controls.Add(Me.layoutMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AboutForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About"
        Me.layoutMain.ResumeLayout(False)
        Me.layoutMain.PerformLayout()
        Me.flowButtons.ResumeLayout(False)
        Me.flowButtons.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents layoutMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents lblDataTitle As System.Windows.Forms.Label
    Friend WithEvents lblData As System.Windows.Forms.Label
    Friend WithEvents lblShortcutsTitle As System.Windows.Forms.Label
    Friend WithEvents lblShortcuts As System.Windows.Forms.Label
    Friend WithEvents lblCredits As System.Windows.Forms.Label
    Friend WithEvents flowButtons As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnOk As TreeYield.WinForms.ChipButton
End Class
