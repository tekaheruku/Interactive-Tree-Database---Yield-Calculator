Imports System.ComponentModel
Imports System.Drawing.Printing
Imports System.IO
Imports System.Text
Imports System.Windows.Forms.DataVisualization.Charting
Imports TreeYield.Core.Models
Imports TreeYield.Core.Services

''' <summary>
''' The main window: category filter chips, the yield calculator (species, tree count,
''' result cards) and three sections - species records, multi-year projection and
''' saved scenarios.
''' </summary>
Public Class MainForm

    ''' <summary>Species selected when the program starts (matches the design mock-up).</summary>
    Private Const DefaultSpeciesName As String = "Carabao Mango"

    Private ReadOnly _services As AppServices

    ' Data loaded from the database. The species list is small (tens of rows), so it is
    ' kept in memory for the drop-down; the records grid queries the database instead.
    Private _categories As New List(Of Category)()
    Private _allSpecies As New List(Of TreeSpecies)()
    Private _scenarios As New List(Of Scenario)()

    Private _categoryFilter As Integer?           ' Nothing = all categories
    Private _selectedSpecies As TreeSpecies       ' species used by the calculator
    Private _currentResult As YieldResult
    Private _currentProjection As YieldProjection
    Private _dataLoaded As Boolean

    ' True until the constructor has finished. The Handles clauses are connected while
    ' InitializeComponent creates the controls, so change events can fire before the
    ' chart and the services are ready; the handlers ignore them until then.
    Private _initializing As Boolean = True

    ' Guards that stop event handlers from reacting to changes made by our own code
    ' (for example the slider updating the number box, which updates the slider...).
    Private _syncingTreeCount As Boolean
    Private _syncingSelection As Boolean

    ' Increases with every search; results from an older, slower search are ignored.
    Private _searchVersion As Integer

    ''' <summary>
    ''' Used by the Visual Basic application framework (Project Properties > Application >
    ''' Startup form). The services were created in ApplicationEvents.vb.
    ''' </summary>
    Public Sub New()
        Me.New(AppServices.Current)
    End Sub

    ''' <summary>Creates the main window for a set of services.</summary>
    Friend Sub New(services As AppServices)
        If services Is Nothing Then
            Throw New ArgumentNullException(NameOf(services))
        End If

        _services = services

        ' This call is required by the designer.
        InitializeComponent()

        ConfigureControls()
        _initializing = False
    End Sub

    ' ------------------------------------------------------------------
    ' Start-up
    ' ------------------------------------------------------------------

    ''' <summary>Settings that cannot be expressed in the designer.</summary>
    Private Sub ConfigureControls()
        Icon = DrawingHelpers.AppIcon

        GridStyler.Apply(gridRecords)
        GridStyler.Apply(gridProjection)
        GridStyler.Apply(gridScenarios)

        GridStyler.AlignRight(colYield, "#,##0.##")
        GridStyler.AlignRight(colPrice, Formatters.PesoSign & "#,##0.00")
        GridStyler.AlignRight(colFirstHarvest, Nothing)
        GridStyler.AlignRight(colProjYear, Nothing)
        GridStyler.AlignRight(colProjYield, Nothing)
        GridStyler.AlignRight(colProjSales, Nothing)
        GridStyler.AlignRight(colProjCumulative, Nothing)
        GridStyler.AlignRight(colScenarioTrees, "N0")
        GridStyler.AlignRight(colScenarioSales, Formatters.PesoSign & "#,##0")

        NativeMethods.SetCueBanner(txtSearch, "Search by name, scientific name or product...")
        toolTip.SetToolTip(lblDisclaimer, YieldCalculator.EstimateDisclaimer)

        nudTreeCount.Minimum = YieldCalculator.MinTreeCount
        nudTreeCount.Maximum = YieldCalculator.MaxTreeCount
        nudYears.Minimum = YieldCalculator.MinProjectionYears
        nudYears.Maximum = YieldCalculator.MaxProjectionYears
        nudYears.Value = YieldCalculator.DefaultProjectionYears

        ConfigureChart()
        UpdateResults()
        UpdateAdminStatus()
    End Sub

    ''' <summary>Chart styling: one series, thin columns, hairline grid, compact peso axis.</summary>
    Private Sub ConfigureChart()
        chartProjection.BackColor = Theme.Surface
        chartProjection.AntiAliasing = AntiAliasingStyles.All
        chartProjection.TextAntiAliasingQuality = TextAntiAliasingQuality.High
        chartProjection.Titles.Add(New Title("Gross sales per year (PHP)", Docking.Top,
                                             New Font(Theme.SemiboldFontFamily, 10.0F), Theme.TextSecondary) With {
                                       .Alignment = ContentAlignment.TopLeft})

        Dim area As ChartArea = chartProjection.ChartAreas(0)
        area.BackColor = Theme.Surface
        For Each axis As Axis In {area.AxisX, area.AxisY}
            axis.LineColor = Theme.Border
            axis.MajorTickMark.Enabled = False
            axis.LabelStyle.ForeColor = Theme.TextSecondary
            axis.LabelStyle.Font = New Font(Theme.FontFamily, 8.5F)
            axis.TitleForeColor = Theme.TextSecondary
            axis.TitleFont = New Font(Theme.FontFamily, 8.5F)
        Next

        area.AxisX.Title = "Year after planting"
        area.AxisX.MajorGrid.Enabled = False
        area.AxisX.Interval = 1
        area.AxisY.MajorGrid.LineColor = Theme.GridLine
        area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid
        ' "Compact" is not a real .NET format; chartProjection_FormatNumber replaces it.
        area.AxisY.LabelStyle.Format = "Compact"

        Dim series As Series = chartProjection.Series(0)
        series.ChartType = SeriesChartType.Column
        series.Color = Theme.ChartSeries
        series.IsXValueIndexed = False
        series("PixelPointWidth") = "22"   ' thin columns that never fill the slot
    End Sub

    ''' <summary>
    ''' On small or high-DPI screens (for example 1366 x 768, or 1920 x 1080 at 150%)
    ''' the default size would run off the screen, so the window starts maximized.
    ''' </summary>
    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Dim workArea As Rectangle = Screen.FromControl(Me).WorkingArea
        If Width > workArea.Width OrElse Height > workArea.Height Then
            WindowState = FormWindowState.Maximized
        End If
    End Sub

    ''' <summary>Opens the database once the window is visible, so the user sees progress.</summary>
    Protected Overrides Async Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        Await InitializeDatabaseAsync()
    End Sub

    Private Async Function InitializeDatabaseAsync() As Task
        Do
            SetBusy(True, "Opening the database...")
            Dim failure As Exception = Nothing
            Try
                Await _services.InitializeDatabaseAsync()
                Await ReloadAllAsync(Nothing)
                lblDatabase.Text = "Database: " & _services.ConnectionFactory.DatabaseFileName
                lblDatabase.ToolTipText = _services.ConnectionFactory.DatabasePath
                SetBusy(False, "Ready. Choose a species and a tree count to see the projected harvest.")
                cboSpecies.Focus()
                Return
            Catch ex As Exception
                failure = ex
            End Try

            ' Opening failed: explain the problem and let the user retry or exit.
            ErrorReporter.Log(failure)
            SetBusy(False, "The database is not available.")
            lblDatabase.Text = "Database: not connected"

            Using dialog As New StartupErrorForm(ErrorReporter.GetFriendlyMessage(failure), failure, _services.ConnectionFactory)
                If dialog.ShowDialog(Me) <> System.Windows.Forms.DialogResult.Retry Then
                    Close()
                    Return
                End If
            End Using
        Loop
    End Function

    ''' <summary>Reloads categories, species and scenarios, keeping (or choosing) the selected species.</summary>
    Private Async Function ReloadAllAsync(speciesIdToSelect As Integer?) As Task
        _categories = Await _services.Species.GetCategoriesAsync()
        _allSpecies = Await _services.Species.GetAllAsync()
        _scenarios = Await _services.Scenarios.GetAllAsync()

        Dim keep As Integer? = speciesIdToSelect
        If Not keep.HasValue AndAlso _selectedSpecies IsNot Nothing Then
            keep = _selectedSpecies.SpeciesId
        End If

        If Not keep.HasValue Then
            Dim preferred As TreeSpecies = _allSpecies.FirstOrDefault(Function(s) s.CommonName = DefaultSpeciesName)
            If preferred IsNot Nothing Then
                keep = preferred.SpeciesId
            End If
        End If

        ' The category IDs come from the database, so refresh the active filter's ID too.
        If chipFruit.Selected Then
            _categoryFilter = CategoryIdByName(Category.FruitBearing)
        ElseIf chipNonFruit.Selected Then
            _categoryFilter = CategoryIdByName(Category.NonFruitBearing)
        Else
            _categoryFilter = Nothing
        End If

        _dataLoaded = True
        RefreshSpeciesCombo(keep)
        Await RefreshRecordsAsync()
        RefreshScenarioGrid()
    End Function

    Private Sub SetBusy(busy As Boolean, status As String)
        layoutRoot.Enabled = Not busy
        UseWaitCursor = busy
        SetStatus(status)
    End Sub

    Private Sub SetStatus(text As String)
        lblStatus.Text = text
    End Sub

    ' ------------------------------------------------------------------
    ' Category filter (All / Fruit-Bearing / Non-Fruit-Bearing)
    ' ------------------------------------------------------------------

    Private Async Sub FilterChip_Click(sender As Object, e As EventArgs) Handles chipAll.Click, chipFruit.Click, chipNonFruit.Click
        Try
            Await ApplyCategoryFilterAsync(DirectCast(sender, ChipButton))
        Catch ex As Exception
            ErrorReporter.ShowError(Me, "filter the species", ex)
        End Try
    End Sub

    Private Async Function ApplyCategoryFilterAsync(chip As ChipButton) As Task
        chipAll.Selected = chip Is chipAll
        chipFruit.Selected = chip Is chipFruit
        chipNonFruit.Selected = chip Is chipNonFruit

        If chip Is chipFruit Then
            _categoryFilter = CategoryIdByName(Category.FruitBearing)
        ElseIf chip Is chipNonFruit Then
            _categoryFilter = CategoryIdByName(Category.NonFruitBearing)
        Else
            _categoryFilter = Nothing
        End If

        RefreshSpeciesCombo(SelectedSpeciesId())
        Await RefreshRecordsAsync()
        SetStatus(If(chip Is chipAll, "Showing all species.", "Showing " & chip.Text.ToLowerInvariant() & " species only."))
    End Function

    Private Function CategoryIdByName(name As String) As Integer?
        Dim match As Category = _categories.FirstOrDefault(Function(c) c.Name = name)
        ' -1 matches nothing, so a missing category shows an empty list instead of everything.
        Return If(match Is Nothing, -1, match.CategoryId)
    End Function

    ''' <summary>ID of the species in the calculator, or Nothing when none is selected.</summary>
    Private Function SelectedSpeciesId() As Integer?
        If _selectedSpecies Is Nothing Then
            Return Nothing
        End If

        Return _selectedSpecies.SpeciesId
    End Function

    ' ------------------------------------------------------------------
    ' Calculator inputs
    ' ------------------------------------------------------------------

    ''' <summary>Fills the species drop-down with the species of the active category.</summary>
    Private Sub RefreshSpeciesCombo(preferredSpeciesId As Integer?)
        Dim visible As List(Of TreeSpecies) = _allSpecies.Where(
            Function(s) Not _categoryFilter.HasValue OrElse s.CategoryId = _categoryFilter.Value).ToList()

        Dim toSelect As TreeSpecies = visible.FirstOrDefault(
            Function(s) preferredSpeciesId.HasValue AndAlso s.SpeciesId = preferredSpeciesId.Value)
        If toSelect Is Nothing Then
            toSelect = visible.FirstOrDefault()
        End If

        _syncingSelection = True
        Try
            cboSpecies.BeginUpdate()
            cboSpecies.DataSource = Nothing
            cboSpecies.DisplayMember = NameOf(TreeSpecies.CommonName)
            cboSpecies.ValueMember = NameOf(TreeSpecies.SpeciesId)
            cboSpecies.DataSource = visible
            cboSpecies.SelectedItem = toSelect
            cboSpecies.Enabled = visible.Count > 0
        Finally
            cboSpecies.EndUpdate()
            _syncingSelection = False
        End Try

        SelectSpecies(toSelect)
    End Sub

    Private Sub cboSpecies_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSpecies.SelectedIndexChanged
        If _initializing OrElse _syncingSelection Then
            Return
        End If

        SelectSpecies(TryCast(cboSpecies.SelectedItem, TreeSpecies))
    End Sub

    Private Sub SelectSpecies(species As TreeSpecies)
        _selectedSpecies = species
        UpdateResults()
        HighlightGridRow(species)
    End Sub

    Private Sub sliderTreeCount_ValueChanged(sender As Object, e As EventArgs) Handles sliderTreeCount.ValueChanged
        If _initializing OrElse _syncingTreeCount Then
            Return
        End If

        _syncingTreeCount = True
        nudTreeCount.Value = sliderTreeCount.Value
        _syncingTreeCount = False
        UpdateResults()
    End Sub

    Private Sub nudTreeCount_ValueChanged(sender As Object, e As EventArgs) Handles nudTreeCount.ValueChanged
        If _initializing OrElse _syncingTreeCount Then
            Return
        End If

        ' The slider covers 1-1,000 for quick changes; bigger counts are typed in the box.
        _syncingTreeCount = True
        sliderTreeCount.Value = CInt(Math.Min(sliderTreeCount.Maximum, nudTreeCount.Value))
        _syncingTreeCount = False
        UpdateResults()
    End Sub

    Private Sub nudYears_ValueChanged(sender As Object, e As EventArgs) Handles nudYears.ValueChanged
        If _initializing Then
            Return
        End If

        UpdateResults()
    End Sub

    ' ------------------------------------------------------------------
    ' Results (stat cards) and projection
    ' ------------------------------------------------------------------

    ''' <summary>Recalculates everything for the current species, tree count and horizon.</summary>
    Private Sub UpdateResults()
        Dim species As TreeSpecies = _selectedSpecies
        Dim treeCount As Integer = CInt(nudTreeCount.Value)
        Dim years As Integer = CInt(nudYears.Value)
        chipProjection.Text = years & "-Year Projection"

        If species Is Nothing Then
            _currentResult = Nothing
            _currentProjection = Nothing
            cardSpecies.Value = "None selected"
            cardSpecies.Caption = If(_dataLoaded, "No species in this category yet", "Loading species...")
            cardYield.Value = "-"
            cardYield.Caption = String.Empty
            cardSales.Value = "-"
            cardSales.Caption = String.Empty
            SetCalculationActionsEnabled(False)
            UpdateProjectionView()
            Return
        End If

        Dim result As YieldResult = YieldCalculator.Calculate(species, treeCount)
        _currentResult = result
        _currentProjection = YieldCalculator.Project(species, treeCount, years)

        cardSpecies.Value = species.CommonName
        cardSpecies.Caption = species.CategoryName & Formatters.Separator & species.ProductType
        cardYield.Value = Formatters.Quantity(result.AnnualYield, species.YieldUnit)
        cardSales.Value = Formatters.Peso(result.GrossSales)

        If result.IsAnnualized Then
            cardYield.Caption = "Annualized; whole stand is cut at year " & result.HarvestYear
            cardSales.Caption = "Annualized; " & Formatters.Peso(result.HarvestSales) & " when harvested at year " & result.HarvestYear
        Else
            cardYield.Caption = "Per year at full production (" & Formatters.Quantity(species.YieldPerTreePerYear, species.YieldUnit) & " per tree)"
            cardSales.Caption = "Per year at " & Formatters.PesoWithCentavos(species.PricePerUnit) & " per " & species.YieldUnit
        End If

        SetCalculationActionsEnabled(True)
        UpdateProjectionView()
    End Sub

    Private Sub SetCalculationActionsEnabled(enabled As Boolean)
        btnSaveScenario.Enabled = enabled
        btnExportCsv.Enabled = enabled
        btnPrint.Enabled = enabled
    End Sub

    ''' <summary>Fills the projection chart, table and summary line.</summary>
    Private Sub UpdateProjectionView()
        Dim series As Series = chartProjection.Series(0)
        series.Points.Clear()
        gridProjection.Rows.Clear()

        Dim projection As YieldProjection = _currentProjection
        If projection Is Nothing Then
            lblProjectionSummary.Text = "Select a species to see its projection."
            Return
        End If

        Dim species As TreeSpecies = projection.Species
        For Each row As ProjectionYear In projection.Rows
            Dim stage As String = Formatters.ProductionStage(row, species.HarvestType)

            Dim index As Integer = series.Points.AddXY(row.Year, CDbl(row.GrossSales))
            series.Points(index).ToolTip = String.Format("Year {0}: {1}  ({2}, {3})",
                                                         row.Year, Formatters.Peso(row.GrossSales), stage,
                                                         Formatters.Quantity(row.Yield, species.YieldUnit))

            gridProjection.Rows.Add(row.Year, stage, Formatters.Quantity(row.Yield, species.YieldUnit),
                                    Formatters.Peso(row.GrossSales), Formatters.Peso(row.CumulativeSales))
        Next

        Dim area As ChartArea = chartProjection.ChartAreas(0)
        area.AxisX.Interval = If(projection.Years <= 15, 1, 2)
        area.AxisX.Minimum = 0.5
        area.AxisX.Maximum = projection.Years + 0.5

        Dim summary As New StringBuilder()
        If projection.FirstHarvestBeyondHorizon Then
            summary.AppendFormat("No harvest within {0}: the first harvest is expected in year {1}",
                                 Formatters.Years(projection.Years), projection.FirstHarvestYear)
            If species.HarvestType = HarvestType.Rotation AndAlso _currentResult IsNot Nothing Then
                summary.AppendFormat(" (about {0}).", Formatters.Peso(_currentResult.HarvestSales))
            Else
                summary.Append(".")
            End If
        Else
            summary.AppendFormat("{0}-year total: {1}{2}{3}", projection.Years,
                                 Formatters.Quantity(projection.TotalYield, species.YieldUnit), Formatters.Separator,
                                 Formatters.Peso(projection.TotalSales))
        End If

        lblProjectionSummary.Text = summary.ToString()
    End Sub

    ''' <summary>Turns Y-axis numbers into short labels such as PHP 250K or PHP 1.2M.</summary>
    Private Sub chartProjection_FormatNumber(sender As Object, e As FormatNumberEventArgs) Handles chartProjection.FormatNumber
        If e.ElementType = ChartElementType.AxisLabels AndAlso e.Format = "Compact" Then
            e.LocalizedValue = CompactPeso(e.Value)
        End If
    End Sub

    ''' <summary>Short axis labels: 0, 250K, 1.5M, 2B (with the peso sign).</summary>
    Private Shared Function CompactPeso(value As Double) As String
        Dim abs As Double = Math.Abs(value)
        If abs >= 1000000000.0 Then
            Return Formatters.PesoSign & (value / 1000000000.0).ToString("0.#") & "B"
        End If

        If abs >= 1000000.0 Then
            Return Formatters.PesoSign & (value / 1000000.0).ToString("0.#") & "M"
        End If

        If abs >= 1000.0 Then
            Return Formatters.PesoSign & (value / 1000.0).ToString("0.#") & "K"
        End If

        Return Formatters.PesoSign & value.ToString("0")
    End Function

    ' ------------------------------------------------------------------
    ' Species records grid (search, select, inspect)
    ' ------------------------------------------------------------------

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If _initializing Then
            Return
        End If

        ' Debounce: wait until the user pauses typing for 300 ms before querying.
        searchTimer.Stop()
        searchTimer.Start()
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Escape AndAlso txtSearch.TextLength > 0 Then
            txtSearch.Clear()
            e.SuppressKeyPress = True
        ElseIf e.KeyCode = Keys.Down AndAlso gridRecords.Rows.Count > 0 Then
            gridRecords.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Async Sub searchTimer_Tick(sender As Object, e As EventArgs) Handles searchTimer.Tick
        searchTimer.Stop()
        Try
            Await RefreshRecordsAsync()
        Catch ex As Exception
            ErrorReporter.ShowError(Me, "search the species records", ex)
        End Try
    End Sub

    ''' <summary>Runs the (parameterized) database search and shows the results in the grid.</summary>
    Private Async Function RefreshRecordsAsync() As Task
        _searchVersion += 1
        Dim version As Integer = _searchVersion
        Dim rows As List(Of TreeSpecies) = Await _services.Species.SearchAsync(txtSearch.Text, _categoryFilter)

        If version <> _searchVersion Then
            Return ' a newer search started while this one was running
        End If

        PopulateRecordsGrid(rows)
    End Function

    Private Sub PopulateRecordsGrid(rows As List(Of TreeSpecies))
        _syncingSelection = True
        Try
            Dim sortColumn As DataGridViewColumn = gridRecords.SortedColumn
            Dim order As SortOrder = gridRecords.SortOrder

            gridRecords.Rows.Clear()
            For Each s As TreeSpecies In rows
                Dim index As Integer = gridRecords.Rows.Add(s.CommonName, s.ScientificName, s.CategoryName, s.ProductType,
                                                            s.YieldPerTreePerYear, s.YieldUnit, s.PricePerUnit, s.YearsToFirstHarvest,
                                                            If(s.HarvestType = HarvestType.Rotation, "Rotation", "Annual"))
                gridRecords.Rows(index).Tag = s
            Next

            ' Keep the user's chosen sort order after a refresh.
            If sortColumn IsNot Nothing AndAlso order <> SortOrder.None Then
                gridRecords.Sort(sortColumn, If(order = SortOrder.Ascending,
                                                ListSortDirection.Ascending,
                                                ListSortDirection.Descending))
            End If

            Dim empty As Boolean = rows.Count = 0
            gridRecords.Visible = Not empty
            lblRecordsEmpty.Visible = empty
            lblRecordsEmpty.Text = If(String.IsNullOrWhiteSpace(txtSearch.Text),
                                      "No species in this category yet.",
                                      "No species match """ & txtSearch.Text.Trim() & """. Try a shorter word or choose All Species.")
            lblRecordCount.Text = String.Format("Showing {0} of {1} records", rows.Count, _allSpecies.Count)
        Finally
            _syncingSelection = False
        End Try

        HighlightGridRow(_selectedSpecies)
    End Sub

    ''' <summary>Selects the grid row of a species (without triggering selection events).</summary>
    Private Sub HighlightGridRow(species As TreeSpecies)
        _syncingSelection = True
        Try
            gridRecords.ClearSelection()
            If species Is Nothing Then
                Return
            End If

            For Each row As DataGridViewRow In gridRecords.Rows
                Dim rowSpecies As TreeSpecies = TryCast(row.Tag, TreeSpecies)
                If rowSpecies Is Nothing OrElse rowSpecies.SpeciesId <> species.SpeciesId Then
                    Continue For
                End If

                row.Selected = True
                If gridRecords.Visible Then
                    gridRecords.CurrentCell = row.Cells(0)
                End If

                Exit For
            Next
        Finally
            _syncingSelection = False
        End Try
    End Sub

    Private Sub gridRecords_SelectionChanged(sender As Object, e As EventArgs) Handles gridRecords.SelectionChanged
        If _initializing OrElse _syncingSelection OrElse gridRecords.CurrentRow Is Nothing Then
            Return
        End If

        Dim species As TreeSpecies = TryCast(gridRecords.CurrentRow.Tag, TreeSpecies)
        If species IsNot Nothing AndAlso (_selectedSpecies Is Nothing OrElse species.SpeciesId <> _selectedSpecies.SpeciesId) Then
            SelectInCombo(species.SpeciesId)
        End If
    End Sub

    Private Sub SelectInCombo(speciesId As Integer)
        Dim items As List(Of TreeSpecies) = TryCast(cboSpecies.DataSource, List(Of TreeSpecies))
        Dim match As TreeSpecies = Nothing
        If items IsNot Nothing Then
            match = items.FirstOrDefault(Function(s) s.SpeciesId = speciesId)
        End If

        If match IsNot Nothing Then
            cboSpecies.SelectedItem = match ' raises SelectedIndexChanged -> SelectSpecies
        End If
    End Sub

    Private Sub gridRecords_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridRecords.CellDoubleClick
        If e.RowIndex >= 0 Then
            ShowSpeciesDetails(TryCast(gridRecords.Rows(e.RowIndex).Tag, TreeSpecies))
        End If
    End Sub

    Private Sub gridRecords_KeyDown(sender As Object, e As KeyEventArgs) Handles gridRecords.KeyDown
        If e.KeyCode = Keys.Enter AndAlso gridRecords.CurrentRow IsNot Nothing Then
            e.SuppressKeyPress = True ' stop the grid from moving to the next row
            ShowSpeciesDetails(TryCast(gridRecords.CurrentRow.Tag, TreeSpecies))
        End If
    End Sub

    Private Sub ShowSpeciesDetails(species As TreeSpecies)
        If species Is Nothing Then
            Return
        End If

        Using details As New SpeciesDetailsForm(species, CInt(nudTreeCount.Value))
            If details.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                SelectInCombo(species.SpeciesId)
                cboSpecies.Focus()
            End If
        End Using
    End Sub

    ' ------------------------------------------------------------------
    ' Section switching (Records / Projection / Scenarios)
    ' ------------------------------------------------------------------

    Private Sub SectionChip_Click(sender As Object, e As EventArgs) Handles chipRecords.Click, chipProjection.Click, chipScenarios.Click
        ShowSection(DirectCast(sender, ChipButton))
    End Sub

    Private Sub ShowSection(chip As ChipButton)
        chipRecords.Selected = chip Is chipRecords
        chipProjection.Selected = chip Is chipProjection
        chipScenarios.Selected = chip Is chipScenarios

        pnlRecords.Visible = chip Is chipRecords
        pnlProjection.Visible = chip Is chipProjection
        pnlScenarios.Visible = chip Is chipScenarios

        If chip Is chipRecords Then
            HighlightGridRow(_selectedSpecies)
        End If
    End Sub

    ' ------------------------------------------------------------------
    ' Saved scenarios
    ' ------------------------------------------------------------------

    Private Async Sub btnSaveScenario_Click(sender As Object, e As EventArgs) Handles btnSaveScenario.Click
        If _currentResult Is Nothing Then
            ErrorReporter.ShowInfo(Me, "Choose a species first.")
            Return
        End If

        Dim treeCount As Integer = CInt(nudTreeCount.Value)
        Dim years As Integer = CInt(nudYears.Value)

        Using dialog As New SaveScenarioForm(_selectedSpecies, treeCount, years, _currentResult)
            If dialog.ShowDialog(Me) <> System.Windows.Forms.DialogResult.OK Then
                Return
            End If

            Dim scenario As New Scenario With {
                .Name = dialog.ScenarioName,
                .Notes = dialog.ScenarioNotes,
                .SpeciesId = _selectedSpecies.SpeciesId,
                .TreeCount = treeCount,
                .ProjectionYears = years
            }

            Try
                Await _services.Scenarios.SaveAsync(scenario)
                _scenarios = Await _services.Scenarios.GetAllAsync()
                RefreshScenarioGrid()
                SetStatus("Scenario """ & scenario.Name & """ saved. Open Saved Scenarios to see it.")
            Catch ex As Exception
                ErrorReporter.ShowError(Me, "save the scenario", ex)
            End Try
        End Using
    End Sub

    Private Sub RefreshScenarioGrid()
        gridScenarios.Rows.Clear()
        For Each scenario As Scenario In _scenarios
            ' Results are recalculated with today's species data, so price updates show up.
            Dim species As TreeSpecies = _allSpecies.FirstOrDefault(Function(s) s.SpeciesId = scenario.SpeciesId)
            Dim sales As Object
            If species Is Nothing Then
                sales = "-"
            Else
                sales = YieldCalculator.Calculate(species, scenario.TreeCount).GrossSales
            End If

            Dim index As Integer = gridScenarios.Rows.Add(scenario.Name, scenario.SpeciesName, scenario.TreeCount,
                                                          Formatters.Years(scenario.ProjectionYears), sales,
                                                          Formatters.LocalDateTime(scenario.CreatedAtUtc))
            gridScenarios.Rows(index).Tag = scenario
        Next

        Dim empty As Boolean = _scenarios.Count = 0
        gridScenarios.Visible = Not empty
        lblScenariosEmpty.Visible = empty
        chipScenarios.Text = "Saved Scenarios (" & _scenarios.Count & ")"
        UpdateScenarioButtons()
    End Sub

    Private Sub gridScenarios_SelectionChanged(sender As Object, e As EventArgs) Handles gridScenarios.SelectionChanged
        If _initializing Then
            Return
        End If

        UpdateScenarioButtons()
    End Sub

    Private Sub UpdateScenarioButtons()
        Dim hasSelection As Boolean = SelectedScenario() IsNot Nothing
        btnLoadScenario.Enabled = hasSelection
        btnDeleteScenario.Enabled = hasSelection
    End Sub

    Private Function SelectedScenario() As Scenario
        If gridScenarios.CurrentRow Is Nothing Then
            Return Nothing
        End If

        Return TryCast(gridScenarios.CurrentRow.Tag, Scenario)
    End Function

    Private Async Sub btnLoadScenario_Click(sender As Object, e As EventArgs) Handles btnLoadScenario.Click
        Await LoadScenarioAsync(SelectedScenario())
    End Sub

    Private Async Sub gridScenarios_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridScenarios.CellDoubleClick
        If e.RowIndex >= 0 Then
            Await LoadScenarioAsync(TryCast(gridScenarios.Rows(e.RowIndex).Tag, Scenario))
        End If
    End Sub

    Private Async Sub gridScenarios_KeyDown(sender As Object, e As KeyEventArgs) Handles gridScenarios.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Await LoadScenarioAsync(SelectedScenario())
        ElseIf e.KeyCode = Keys.Delete Then
            e.SuppressKeyPress = True
            Await DeleteScenarioAsync(SelectedScenario())
        End If
    End Sub

    Private Async Function LoadScenarioAsync(scenario As Scenario) As Task
        If scenario Is Nothing Then
            Return
        End If

        If Not _allSpecies.Any(Function(s) s.SpeciesId = scenario.SpeciesId) Then
            ErrorReporter.ShowInfo(Me, "The species of this scenario no longer exists.")
            Return
        End If

        Try
            ' Show all categories so the scenario's species is certainly in the drop-down.
            Await ApplyCategoryFilterAsync(chipAll)
            nudYears.Value = scenario.ProjectionYears
            nudTreeCount.Value = scenario.TreeCount
            SelectInCombo(scenario.SpeciesId)
            ShowSection(chipProjection)
            SetStatus("Loaded scenario """ & scenario.Name & """.")
        Catch ex As Exception
            ErrorReporter.ShowError(Me, "load the scenario", ex)
        End Try
    End Function

    Private Async Sub btnDeleteScenario_Click(sender As Object, e As EventArgs) Handles btnDeleteScenario.Click
        Await DeleteScenarioAsync(SelectedScenario())
    End Sub

    Private Async Function DeleteScenarioAsync(scenario As Scenario) As Task
        If scenario Is Nothing OrElse Not ErrorReporter.Confirm(Me,
            "Delete the scenario """ & scenario.Name & """?" & vbCrLf & vbCrLf & "This cannot be undone.", "Delete Scenario") Then
            Return
        End If

        Try
            Await _services.Scenarios.DeleteAsync(scenario.ScenarioId)
            _scenarios = Await _services.Scenarios.GetAllAsync()
            RefreshScenarioGrid()
            SetStatus("Scenario """ & scenario.Name & """ deleted.")
        Catch ex As Exception
            ErrorReporter.ShowError(Me, "delete the scenario", ex)
        End Try
    End Function

    ' ------------------------------------------------------------------
    ' Export and print
    ' ------------------------------------------------------------------

    Private Sub btnExportCsv_Click(sender As Object, e As EventArgs) Handles btnExportCsv.Click
        If _currentResult Is Nothing Then
            Return
        End If

        Dim suggestedName As String = SafeFileName(String.Format("TreeYield_{0}_{1}trees_{2:yyyyMMdd}",
                                                                 _selectedSpecies.CommonName, _currentResult.TreeCount, DateTime.Now))

        Using dialog As New SaveFileDialog()
            dialog.Title = "Export Projection to CSV"
            dialog.Filter = "CSV file (*.csv)|*.csv"
            dialog.DefaultExt = "csv"
            dialog.AddExtension = True
            dialog.OverwritePrompt = True
            dialog.FileName = suggestedName & ".csv"

            If dialog.ShowDialog(Me) <> System.Windows.Forms.DialogResult.OK Then
                Return
            End If

            Try
                Dim csv As String = CsvExporter.BuildReport(_currentResult, _currentProjection, DateTime.Now)
                ' UTF-8 with a byte-order mark so Excel shows the peso sign and accents correctly.
                File.WriteAllText(dialog.FileName, csv, New UTF8Encoding(True))
                SetStatus("Exported to " & dialog.FileName)
            Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
                ErrorReporter.ShowError(Me, "export the CSV file", ex)
            End Try
        End Using
    End Sub

    Private Shared Function SafeFileName(name As String) As String
        For Each invalid As Char In Path.GetInvalidFileNameChars()
            name = name.Replace(invalid, "_"c)
        Next

        Return name.Replace(" "c, "_"c)
    End Function

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If _currentResult Is Nothing Then
            Return
        End If

        Try
            Using printer As New ReportPrinter(_currentResult, _currentProjection)
                printer.ShowPreview(Me)
            End Using
        Catch ex As InvalidPrinterException
            ErrorReporter.Log(ex)
            ErrorReporter.ShowInfo(Me, "No printer is installed. Add a printer (or ""Microsoft Print to PDF"") in Windows Settings and try again.")
        End Try
    End Sub

    ' ------------------------------------------------------------------
    ' Administrator area
    ' ------------------------------------------------------------------

    Private Async Sub btnAdmin_Click(sender As Object, e As EventArgs) Handles btnAdmin.Click
        Dim session As AdminSession = _services.Session

        If session.IsExpired Then
            Await SignOutAsync()
            ErrorReporter.ShowInfo(Me, "Your administrator session ended after 15 minutes without activity. Please sign in again.")
        End If

        If Not session.IsSignedIn Then
            Using login As New LoginForm(_services.Auth)
                If login.ShowDialog(Me) <> System.Windows.Forms.DialogResult.OK Then
                    Return
                End If

                session.SignIn(login.SignedInUser)
            End Using

            If session.CurrentUser.MustChangePassword Then
                Dim changed As Boolean
                Using change As New ChangePasswordForm(_services.Auth, session.CurrentUser, True)
                    changed = change.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK
                End Using

                If Not changed Then
                    Await SignOutAsync()
                    ErrorReporter.ShowInfo(Me, "The default password must be changed before records can be managed.")
                    Return
                End If
            End If

            UpdateAdminStatus()
        End If

        session.Touch()
        Dim dataChanged As Boolean
        Using admin As New AdminForm(_services)
            admin.ShowDialog(Me)
            dataChanged = admin.DataChanged
        End Using

        UpdateAdminStatus()
        If dataChanged Then
            Try
                Await ReloadAllAsync(Nothing)
                SetStatus("Species records updated.")
            Catch ex As Exception
                ErrorReporter.ShowError(Me, "reload the species records", ex)
            End Try
        End If
    End Sub

    Private Async Function SignOutAsync() As Task
        Dim user As AppUser = _services.Session.CurrentUser
        _services.Session.SignOut()
        Await _services.Auth.LogoutAsync(user)
        UpdateAdminStatus()
    End Function

    Private Sub UpdateAdminStatus()
        Dim session As AdminSession = _services.Session
        Dim signedIn As Boolean = session.IsSignedIn
        btnAdmin.Text = If(signedIn, "Manage Records", "Admin Login")
        lblAdmin.Text = If(signedIn, "Signed in as " & session.CurrentUser.Username, "Guest (read-only)")
    End Sub

    Private Sub btnAbout_Click(sender As Object, e As EventArgs) Handles btnAbout.Click
        Using about As New AboutForm()
            about.ShowDialog(Me)
        End Using
    End Sub

    ' ------------------------------------------------------------------
    ' Keyboard shortcuts and closing
    ' ------------------------------------------------------------------

    ''' <summary>Handles the keyboard shortcuts listed in the tooltips and the About window.</summary>
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If layoutRoot.Enabled Then
            Select Case keyData
                Case Keys.Control Or Keys.S
                    btnSaveScenario.PerformClick() ' PerformClick does nothing while the button is disabled
                    Return True
                Case Keys.Control Or Keys.E
                    btnExportCsv.PerformClick()
                    Return True
                Case Keys.Control Or Keys.P
                    btnPrint.PerformClick()
                    Return True
                Case Keys.Control Or Keys.F
                    ShowSection(chipRecords)
                    txtSearch.Focus()
                    txtSearch.SelectAll()
                    Return True
                Case Keys.Control Or Keys.D1
                    ShowSection(chipRecords)
                    Return True
                Case Keys.Control Or Keys.D2
                    ShowSection(chipProjection)
                    Return True
                Case Keys.Control Or Keys.D3
                    ShowSection(chipScenarios)
                    Return True
                Case Keys.F1
                    btnAbout.PerformClick()
                    Return True
            End Select
        End If

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    ''' <summary>Records the logout in the audit log when the program closes while an admin is signed in.</summary>
    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        Dim user As AppUser = _services.Session.CurrentUser
        If user IsNot Nothing Then
            Try
                ' The service uses ConfigureAwait(False), so waiting here cannot deadlock.
                _services.Auth.LogoutAsync(user).Wait(TimeSpan.FromSeconds(2))
            Catch ex As AggregateException
                ErrorReporter.Log(ex)
            End Try
        End If

        MyBase.OnFormClosed(e)
    End Sub

End Class
