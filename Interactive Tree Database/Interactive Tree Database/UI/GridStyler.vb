Imports System.Reflection

''' <summary>
''' Gives every DataGridView the same flat, read-only look: no row headers, thin
''' horizontal lines, roomy rows and a soft blue selection.
''' </summary>
Friend NotInheritable Class GridStyler

    Private Sub New()
    End Sub

    ''' <summary>Applies the shared style to a grid.</summary>
    Public Shared Sub Apply(grid As DataGridView)
        grid.BorderStyle = BorderStyle.None
        grid.BackgroundColor = Theme.Surface
        grid.GridColor = Theme.GridLine
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        grid.RowHeadersVisible = False
        grid.EnableHeadersVisualStyles = False
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        grid.ColumnHeadersHeight = 38
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.MultiSelect = False
        grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.AllowUserToResizeRows = False
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        grid.RowTemplate.Height = 34
        grid.ShowCellToolTips = True

        ' Tab moves to the next control instead of the next cell, so keyboard users
        ' are never trapped inside the grid (accessibility).
        grid.StandardTab = True

        Dim header As DataGridViewCellStyle = grid.ColumnHeadersDefaultCellStyle
        header.BackColor = Theme.Surface
        header.ForeColor = Theme.TextSecondary
        header.SelectionBackColor = Theme.Surface
        header.SelectionForeColor = Theme.TextSecondary
        header.Font = New Font(Theme.SemiboldFontFamily, 9.0F)
        header.Padding = New Padding(6, 0, 6, 0)
        header.Alignment = DataGridViewContentAlignment.MiddleLeft

        Dim cells As DataGridViewCellStyle = grid.DefaultCellStyle
        cells.BackColor = Theme.Surface
        cells.ForeColor = Theme.TextPrimary
        cells.SelectionBackColor = Theme.Selection
        cells.SelectionForeColor = Theme.TextPrimary
        cells.Font = New Font(Theme.FontFamily, 9.75F)
        cells.Padding = New Padding(6, 0, 6, 0)

        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(251, 251, 251)

        ' DataGridView hides its DoubleBuffered property; turning it on removes flicker
        ' while the grid is refilled during slider dragging.
        Dim doubleBuffered As PropertyInfo = GetType(DataGridView).GetProperty("DoubleBuffered",
            BindingFlags.Instance Or BindingFlags.NonPublic)
        If doubleBuffered IsNot Nothing Then
            doubleBuffered.SetValue(grid, True, Nothing)
        End If
    End Sub

    ''' <summary>Right-aligns a numeric column (cells and header), as numbers should be.</summary>
    Public Shared Sub AlignRight(column As DataGridViewColumn, format As String)
        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        If format IsNot Nothing Then
            column.DefaultCellStyle.Format = format
        End If
    End Sub

End Class
