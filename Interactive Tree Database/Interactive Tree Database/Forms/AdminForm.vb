Imports TreeYield.Core.Models
Imports TreeYield.Core.Services

''' <summary>
''' Administrator screen: add, edit and delete species records, view the audit log,
''' change the password and restore the sample data. Only opened after a successful login.
''' </summary>
Public Class AdminForm

    Private Const AuditEntriesToShow As Integer = 200

    Private ReadOnly _services As AppServices
    Private _species As New List(Of TreeSpecies)()
    Private _categories As New List(Of Category)()
    Private _dataChanged As Boolean

    Friend Sub New(services As AppServices)
        If services Is Nothing Then
            Throw New ArgumentNullException(NameOf(services))
        End If

        _services = services

        ' This call is required by the designer.
        InitializeComponent()

        Icon = DrawingHelpers.AppIcon
        GridStyler.Apply(gridSpecies)
        GridStyler.Apply(gridAudit)
        GridStyler.AlignRight(colYield, "#,##0.##")
        GridStyler.AlignRight(colPrice, Formatters.PesoSign & "#,##0.00")
        NativeMethods.SetCueBanner(txtFilter, "Filter by name or product...")

        Dim user As AppUser = _services.Session.CurrentUser
        lblSignedIn.Text = "Signed in as " & If(user Is Nothing, "-", user.Username) &
                           ". Changes are saved immediately and recorded in the audit log."
        UpdateButtons()
    End Sub

    ''' <summary>True when any species record was added, changed or deleted (the main window then reloads).</summary>
    Public ReadOnly Property DataChanged As Boolean
        Get
            Return _dataChanged
        End Get
    End Property

    Private ReadOnly Property CurrentUser As AppUser
        Get
            Return _services.Session.CurrentUser
        End Get
    End Property

    Protected Overrides Async Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        Await LoadSpeciesAsync(Nothing)
    End Sub

    ''' <summary>
    ''' Called before every change. If the session timed out, the window closes so
    ''' the administrator has to sign in again.
    ''' </summary>
    Private Function EnsureSessionActive() As Boolean
        If _services.Session.IsSignedIn Then
            _services.Session.Touch()
            Return True
        End If

        ErrorReporter.ShowInfo(Me, "Your administrator session has ended. Please sign in again.")
        Close()
        Return False
    End Function

    ' ------------------------------------------------------------------
    ' Species list
    ' ------------------------------------------------------------------

    Private Async Function LoadSpeciesAsync(speciesIdToSelect As Integer?) As Task
        SetBusy(True)
        Try
            _categories = Await _services.Species.GetCategoriesAsync()
            _species = Await _services.Species.GetAllAsync()
            ApplyFilter(speciesIdToSelect)
        Catch ex As Exception
            ErrorReporter.ShowError(Me, "load the species records", ex)
        Finally
            SetBusy(False)
        End Try
    End Function

    ''' <summary>Filters the loaded list in memory (the admin list is small) and fills the grid.</summary>
    Private Sub ApplyFilter(speciesIdToSelect As Integer?)
        Dim filter As String = txtFilter.Text.Trim()
        Dim visible As List(Of TreeSpecies) = _species.Where(
            Function(s) filter.Length = 0 OrElse
                        ContainsText(s.CommonName, filter) OrElse
                        ContainsText(s.ScientificName, filter) OrElse
                        ContainsText(s.ProductType, filter)).ToList()

        gridSpecies.Rows.Clear()
        For Each s As TreeSpecies In visible
            Dim rotation As Boolean = s.HarvestType = HarvestType.Rotation
            Dim years As String = If(rotation,
                                     s.YearsToFirstHarvest & " (rotation)",
                                     s.YearsToFirstHarvest & " / " & s.YearsToFullProduction)

            Dim index As Integer = gridSpecies.Rows.Add(s.CommonName, s.CategoryName, s.ProductType, s.YieldPerTreePerYear,
                                                        s.YieldUnit, s.PricePerUnit, If(rotation, "Rotation", "Annual"), years,
                                                        Formatters.LocalDateTime(s.UpdatedAtUtc))
            gridSpecies.Rows(index).Tag = s

            If speciesIdToSelect.HasValue AndAlso s.SpeciesId = speciesIdToSelect.Value Then
                gridSpecies.CurrentCell = gridSpecies.Rows(index).Cells(0)
            End If
        Next

        Dim empty As Boolean = visible.Count = 0
        gridSpecies.Visible = Not empty
        lblSpeciesEmpty.Visible = empty
        lblSpeciesEmpty.Text = If(_species.Count = 0,
                                  "There are no species yet. Click Add Species to create one.",
                                  "No species match """ & filter & """.")
        lblSpeciesCount.Text = String.Format("{0} of {1} records", visible.Count, _species.Count)
        UpdateButtons()
    End Sub

    Private Shared Function ContainsText(text As String, part As String) As Boolean
        Return text IsNot Nothing AndAlso text.IndexOf(part, StringComparison.OrdinalIgnoreCase) >= 0
    End Function

    Private Function SelectedSpecies() As TreeSpecies
        If gridSpecies.CurrentRow Is Nothing OrElse Not gridSpecies.Visible Then
            Return Nothing
        End If

        Return TryCast(gridSpecies.CurrentRow.Tag, TreeSpecies)
    End Function

    ''' <summary>ID of the selected species, or Nothing when no row is selected.</summary>
    Private Function SelectedSpeciesId() As Integer?
        Dim selected As TreeSpecies = SelectedSpecies()
        If selected Is Nothing Then
            Return Nothing
        End If

        Return selected.SpeciesId
    End Function

    Private Sub UpdateButtons()
        Dim hasSelection As Boolean = SelectedSpecies() IsNot Nothing
        btnEdit.Enabled = hasSelection
        btnDelete.Enabled = hasSelection
    End Sub

    Private Sub txtFilter_TextChanged(sender As Object, e As EventArgs) Handles txtFilter.TextChanged
        ApplyFilter(SelectedSpeciesId())
    End Sub

    Private Sub gridSpecies_SelectionChanged(sender As Object, e As EventArgs) Handles gridSpecies.SelectionChanged
        UpdateButtons()
    End Sub

    Private Async Sub gridSpecies_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridSpecies.CellDoubleClick
        If e.RowIndex >= 0 Then
            Await EditSpeciesAsync(SelectedSpecies())
        End If
    End Sub

    Private Async Sub gridSpecies_KeyDown(sender As Object, e As KeyEventArgs) Handles gridSpecies.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Await EditSpeciesAsync(SelectedSpecies())
        ElseIf e.KeyCode = Keys.Delete Then
            e.SuppressKeyPress = True
            Await DeleteSpeciesAsync(SelectedSpecies())
        End If
    End Sub

    ' ------------------------------------------------------------------
    ' Add / edit / delete
    ' ------------------------------------------------------------------

    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not EnsureSessionActive() Then
            Return
        End If

        Using editor As New SpeciesEditorForm(_services.Species, _categories, Nothing, CurrentUser)
            If editor.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                _dataChanged = True
                txtFilter.Clear()
                Await LoadSpeciesAsync(editor.SavedSpeciesId)
            End If
        End Using
    End Sub

    Private Async Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Await EditSpeciesAsync(SelectedSpecies())
    End Sub

    Private Async Function EditSpeciesAsync(species As TreeSpecies) As Task
        If species Is Nothing OrElse Not EnsureSessionActive() Then
            Return
        End If

        ' The editor works on a copy, so Cancel leaves the list unchanged.
        Using editor As New SpeciesEditorForm(_services.Species, _categories, species.Clone(), CurrentUser)
            If editor.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                _dataChanged = True
                Await LoadSpeciesAsync(editor.SavedSpeciesId)
            End If
        End Using
    End Function

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Await DeleteSpeciesAsync(SelectedSpecies())
    End Sub

    Private Async Function DeleteSpeciesAsync(species As TreeSpecies) As Task
        If species Is Nothing OrElse Not EnsureSessionActive() Then
            Return
        End If

        Try
            Dim scenarioCount As Integer = Await _services.Species.CountScenariosAsync(species.SpeciesId)
            Dim message As String = "Delete """ & species.CommonName & """?"
            If scenarioCount > 0 Then
                message &= vbCrLf & vbCrLf & String.Format("{0} saved scenario{1} that use{2} this species will also be deleted.",
                                                           scenarioCount, If(scenarioCount = 1, String.Empty, "s"),
                                                           If(scenarioCount = 1, "s", String.Empty))
            End If

            message &= vbCrLf & vbCrLf & "This cannot be undone."
            If Not ErrorReporter.Confirm(Me, message, "Delete Species") Then
                Return
            End If

            SetBusy(True)
            Await _services.Species.DeleteAsync(species, CurrentUser)
            _dataChanged = True
            Await LoadSpeciesAsync(Nothing)
        Catch ex As Exception
            ErrorReporter.ShowError(Me, "delete the species", ex)
        Finally
            SetBusy(False)
        End Try
    End Function

    Private Async Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Await LoadSpeciesAsync(SelectedSpeciesId())
        If pnlAudit.Visible Then
            Await LoadAuditAsync()
        End If
    End Sub

    Private Async Sub btnResetData_Click(sender As Object, e As EventArgs) Handles btnResetData.Click
        If Not EnsureSessionActive() OrElse Not ErrorReporter.Confirm(Me,
            "Restore the original sample data?" & vbCrLf & vbCrLf &
            "All species records - including ones you added or edited - and ALL saved scenarios will be deleted, " &
            "then the sample species are inserted again. User accounts and the audit log are kept.",
            "Reset Sample Data") Then
            Return
        End If

        SetBusy(True)
        Try
            Await _services.Species.ResetSampleDataAsync(CurrentUser)
            _dataChanged = True
            Await LoadSpeciesAsync(Nothing)
            ErrorReporter.ShowInfo(Me, "The sample data was restored.")
        Catch ex As Exception
            ErrorReporter.ShowError(Me, "restore the sample data", ex)
        Finally
            SetBusy(False)
        End Try
    End Sub

    ' ------------------------------------------------------------------
    ' Audit log, password, sign out
    ' ------------------------------------------------------------------

    Private Async Sub SectionChip_Click(sender As Object, e As EventArgs) Handles chipSpecies.Click, chipAudit.Click
        Dim showAudit As Boolean = sender Is chipAudit
        chipSpecies.Selected = Not showAudit
        chipAudit.Selected = showAudit
        pnlSpecies.Visible = Not showAudit
        pnlAudit.Visible = showAudit

        If showAudit Then
            Await LoadAuditAsync()
        End If
    End Sub

    Private Async Function LoadAuditAsync() As Task
        Try
            Dim entries As List(Of AuditEntry) = Await _services.Species.GetAuditLogAsync(AuditEntriesToShow)
            gridAudit.Rows.Clear()
            For Each entry As AuditEntry In entries
                Dim record As String = String.Empty
                If entry.EntityName IsNot Nothing Then
                    record = entry.EntityName & If(entry.EntityId.HasValue, " #" & entry.EntityId.Value, String.Empty)
                End If

                gridAudit.Rows.Add(Formatters.LocalDateTime(entry.OccurredAtUtc), If(entry.Username, "(unknown)"),
                                   entry.Action, record, entry.Details)
            Next
        Catch ex As Exception
            ErrorReporter.ShowError(Me, "load the audit log", ex)
        End Try
    End Function

    Private Sub btnChangePassword_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        If Not EnsureSessionActive() Then
            Return
        End If

        Using dialog As New ChangePasswordForm(_services.Auth, CurrentUser, False)
            dialog.ShowDialog(Me)
        End Using
    End Sub

    Private Async Sub btnSignOut_Click(sender As Object, e As EventArgs) Handles btnSignOut.Click
        Dim user As AppUser = CurrentUser
        _services.Session.SignOut()
        Try
            Await _services.Auth.LogoutAsync(user)
        Finally
            Close()
        End Try
    End Sub

    Private Sub SetBusy(busy As Boolean)
        pnlContent.Enabled = Not busy
        flowFooterLeft.Enabled = Not busy
        UseWaitCursor = busy
    End Sub

    ''' <summary>Keyboard shortcuts: Ctrl+N adds a species, F5 refreshes.</summary>
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = (Keys.Control Or Keys.N) Then
            btnAdd.PerformClick()
            Return True
        End If

        If keyData = Keys.F5 Then
            btnRefresh.PerformClick()
            Return True
        End If

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

End Class
