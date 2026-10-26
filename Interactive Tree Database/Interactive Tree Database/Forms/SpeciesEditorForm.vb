Imports TreeYield.Core.Data
Imports TreeYield.Core.Models
Imports TreeYield.Core.Services

''' <summary>
''' Add or edit one species record. Input is checked with the same
''' <see cref="SpeciesValidator"/> the service uses, and every problem is shown next to
''' its field with an ErrorProvider icon.
''' </summary>
Public Class SpeciesEditorForm

    Private Shared ReadOnly SuggestedProducts As String() = {
        "Fresh fruit", "Mature nuts", "Nuts in shell", "Dried beans", "Green coffee beans",
        "Timber", "Pulpwood / timber", "Peeler logs / timber", "Cup lump (latex)", "Fresh leaves", "Resin"
    }

    Private ReadOnly _service As SpeciesService
    Private ReadOnly _actor As AppUser
    Private ReadOnly _original As TreeSpecies
    Private ReadOnly _fieldControls As Dictionary(Of String, Control)
    Private _savedSpeciesId As Integer

    ' True while the form fills its own controls. Starts True because the Handles
    ' clauses are connected as soon as InitializeComponent creates each control, and
    ' those first change events must not count as edits by the user.
    Private _loading As Boolean = True
    Private _dirty As Boolean

    ''' <summary>Creates the editor.</summary>
    ''' <param name="service">Species service used to save.</param>
    ''' <param name="categories">Categories for the drop-down.</param>
    ''' <param name="species">The record to edit, or Nothing to add a new one.</param>
    ''' <param name="actor">The signed-in administrator.</param>
    Public Sub New(service As SpeciesService, categories As IList(Of Category), species As TreeSpecies, actor As AppUser)
        If service Is Nothing Then
            Throw New ArgumentNullException(NameOf(service))
        End If

        If categories Is Nothing Then
            Throw New ArgumentNullException(NameOf(categories))
        End If

        _service = service
        _actor = actor
        _original = species

        ' This call is required by the designer.
        InitializeComponent()

        ' Maps each validated property to the control that edits it.
        _fieldControls = New Dictionary(Of String, Control) From {
            {NameOf(TreeSpecies.CommonName), txtCommonName},
            {NameOf(TreeSpecies.ScientificName), txtScientificName},
            {NameOf(TreeSpecies.CategoryId), cboCategory},
            {NameOf(TreeSpecies.ProductType), cboProductType},
            {NameOf(TreeSpecies.HarvestType), cboHarvestType},
            {NameOf(TreeSpecies.YieldUnit), cboYieldUnit},
            {NameOf(TreeSpecies.YieldPerTreePerYear), nudYield},
            {NameOf(TreeSpecies.PricePerUnit), nudPrice},
            {NameOf(TreeSpecies.YearsToFirstHarvest), nudFirstHarvest},
            {NameOf(TreeSpecies.YearsToFullProduction), nudFullProduction},
            {NameOf(TreeSpecies.HarvestSeason), txtSeason},
            {NameOf(TreeSpecies.Notes), txtNotes}
        }

        ' Input limits come from the validator so the UI and the rules never disagree.
        txtCommonName.MaxLength = SpeciesValidator.MaxCommonNameLength
        txtScientificName.MaxLength = SpeciesValidator.MaxScientificNameLength
        cboProductType.MaxLength = SpeciesValidator.MaxProductTypeLength
        cboYieldUnit.MaxLength = SpeciesValidator.MaxYieldUnitLength
        txtSeason.MaxLength = SpeciesValidator.MaxHarvestSeasonLength
        txtNotes.MaxLength = SpeciesValidator.MaxNotesLength
        nudYield.Maximum = SpeciesValidator.MaxYieldPerTree
        nudPrice.Maximum = SpeciesValidator.MaxPricePerUnit
        nudFirstHarvest.Maximum = SpeciesValidator.MaxYears
        nudFullProduction.Maximum = SpeciesValidator.MaxYears

        cboProductType.Items.AddRange(SuggestedProducts)
        cboCategory.DisplayMember = NameOf(Category.Name)
        cboCategory.ValueMember = NameOf(Category.CategoryId)
        cboCategory.DataSource = categories.ToList()

        Dim isNew As Boolean = species Is Nothing
        Text = If(isNew, "Add Species", "Edit Species")
        lblTitle.Text = If(isNew, "Add Species", "Edit " & species.CommonName)

        LoadSpecies(If(species, NewSpeciesDefaults(categories)))
        _loading = False

        UpdateHarvestTypeUi()
        UpdatePreview()
    End Sub

    ''' <summary>ID of the saved species (set when the dialog returns OK).</summary>
    Public ReadOnly Property SavedSpeciesId As Integer
        Get
            Return _savedSpeciesId
        End Get
    End Property

    Private Shared Function NewSpeciesDefaults(categories As IList(Of Category)) As TreeSpecies
        ' Yield starts at 0 on purpose: the user must enter a real value.
        Return New TreeSpecies With {
            .CategoryId = If(categories.Count > 0, categories(0).CategoryId, 0),
            .ProductType = "Fresh fruit",
            .YieldUnit = "kg",
            .HarvestType = HarvestType.Annual,
            .YearsToFirstHarvest = 3,
            .YearsToFullProduction = 6
        }
    End Function

    Private Sub LoadSpecies(s As TreeSpecies)
        txtCommonName.Text = If(s.CommonName, String.Empty)
        txtScientificName.Text = If(s.ScientificName, String.Empty)
        cboCategory.SelectedValue = s.CategoryId
        cboProductType.Text = If(s.ProductType, String.Empty)
        cboHarvestType.SelectedIndex = If(s.HarvestType = HarvestType.Rotation, 1, 0)
        cboYieldUnit.Text = If(s.YieldUnit, String.Empty)
        nudYield.Value = Clamp(s.YieldPerTreePerYear, nudYield)
        nudPrice.Value = Clamp(s.PricePerUnit, nudPrice)
        nudFirstHarvest.Value = Clamp(s.YearsToFirstHarvest, nudFirstHarvest)
        nudFullProduction.Value = Clamp(s.YearsToFullProduction, nudFullProduction)
        txtSeason.Text = If(s.HarvestSeason, String.Empty)
        txtNotes.Text = If(s.Notes, String.Empty)
    End Sub

    Private Shared Function Clamp(value As Decimal, box As NumericUpDown) As Decimal
        Return Math.Max(box.Minimum, Math.Min(box.Maximum, value))
    End Function

    ''' <summary>Copies the form fields into a new <see cref="TreeSpecies"/> object.</summary>
    Private Function ReadForm() As TreeSpecies
        Dim category As Category = TryCast(cboCategory.SelectedItem, Category)
        Return New TreeSpecies With {
            .SpeciesId = If(_original Is Nothing, 0, _original.SpeciesId),
            .CommonName = txtCommonName.Text,
            .ScientificName = txtScientificName.Text,
            .CategoryId = If(category Is Nothing, 0, category.CategoryId),
            .CategoryName = If(category Is Nothing, Nothing, category.Name),
            .ProductType = cboProductType.Text,
            .YieldUnit = cboYieldUnit.Text,
            .YieldPerTreePerYear = nudYield.Value,
            .PricePerUnit = nudPrice.Value,
            .HarvestType = If(IsRotation, HarvestType.Rotation, HarvestType.Annual),
            .YearsToFirstHarvest = CInt(nudFirstHarvest.Value),
            .YearsToFullProduction = CInt(nudFullProduction.Value),
            .HarvestSeason = txtSeason.Text,
            .Notes = txtNotes.Text
        }
    End Function

    Private ReadOnly Property IsRotation As Boolean
        Get
            Return cboHarvestType.SelectedIndex = 1
        End Get
    End Property

    Private Sub Field_Changed(sender As Object, e As EventArgs) Handles _
        txtCommonName.TextChanged, txtScientificName.TextChanged, cboCategory.SelectedIndexChanged,
        cboProductType.TextChanged, cboYieldUnit.TextChanged, nudYield.ValueChanged, nudPrice.ValueChanged,
        nudFullProduction.ValueChanged, txtSeason.TextChanged, txtNotes.TextChanged

        If _loading Then
            Return
        End If

        _dirty = True
        Dim changed As Control = TryCast(sender, Control)
        If changed IsNot Nothing Then
            errorProvider.SetError(changed, String.Empty) ' the user is fixing it
        End If

        UpdatePreview()
    End Sub

    Private Sub cboHarvestType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboHarvestType.SelectedIndexChanged
        If _loading Then
            Return
        End If

        UpdateHarvestTypeUi()
        Field_Changed(sender, e)
    End Sub

    Private Sub nudFirstHarvest_ValueChanged(sender As Object, e As EventArgs) Handles nudFirstHarvest.ValueChanged
        If _loading Then
            Return
        End If

        If IsRotation Then
            nudFullProduction.Value = nudFirstHarvest.Value ' rotation species have one harvest year
        End If

        Field_Changed(sender, e)
    End Sub

    ''' <summary>Changes labels and help text to match the chosen harvest type.</summary>
    Private Sub UpdateHarvestTypeUi()
        Dim rotation As Boolean = IsRotation
        lblFirstHarvest.Text = If(rotation, "Rotation &age (years) *", "&First harvest (year) *")
        lblYield.Text = If(rotation, "Average &yield per tree per year *", "&Yield per tree per year *")
        nudFullProduction.Enabled = Not rotation
        If rotation Then
            nudFullProduction.Value = nudFirstHarvest.Value
        End If

        If rotation Then
            lblHelp.Text = "Rotation: the whole tree is cut once at rotation age (for example timber). Enter the average growth " &
                           "per tree per year (mean annual increment); the harvest equals that yield x the rotation age."
        Else
            lblHelp.Text = "Annual: the tree is harvested every year. Yield rises evenly from the first harvest year until " &
                           "the year of full production, then stays at the full yield entered above."
        End If
    End Sub

    Private Sub UpdatePreview()
        Dim s As TreeSpecies = ReadForm()
        Dim unit As String = If(String.IsNullOrWhiteSpace(s.YieldUnit), "units", s.YieldUnit.Trim())

        If s.YieldPerTreePerYear <= 0D Then
            lblPreview.Text = "Preview: enter a yield greater than zero to see the value of one tree."
            Return
        End If

        If s.HarvestType = HarvestType.Rotation Then
            Dim age As Integer = Math.Max(1, s.YearsToFirstHarvest)
            Dim harvest As Decimal = s.YieldPerTreePerYear * age
            lblPreview.Text = String.Format("Preview: 1 tree = {0} = {1} when cut at year {2}.",
                                            Formatters.Quantity(harvest, unit), Formatters.Peso(harvest * s.PricePerUnit), age)
        Else
            lblPreview.Text = String.Format("Preview: 1 tree = {0} per year = {1} per year at full production.",
                                            Formatters.Quantity(s.YieldPerTreePerYear, unit),
                                            Formatters.Peso(s.YieldPerTreePerYear * s.PricePerUnit))
        End If
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        errorProvider.Clear()
        Dim species As TreeSpecies = ReadForm()

        ' Check here first so the user sees every problem at once, next to its field.
        Dim validation As ValidationResult = SpeciesValidator.Validate(SpeciesService.Normalize(species))
        If Not validation.IsValid Then
            ShowErrors(validation)
            Return
        End If

        SetBusy(True)
        Try
            _savedSpeciesId = Await _service.SaveAsync(species, _actor)
            _dirty = False
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Catch ex As ValidationException
            ShowErrors(ex.Result)
        Catch ex As DuplicateRecordException
            errorProvider.SetError(txtCommonName, "Another species already uses this name. Choose a different name.")
            txtCommonName.Focus()
            txtCommonName.SelectAll()
        Catch ex As Exception
            ErrorReporter.ShowError(Me, "save the species", ex)
        Finally
            SetBusy(False)
        End Try
    End Sub

    Private Sub ShowErrors(validation As ValidationResult)
        Dim first As Control = Nothing
        For Each problem As ValidationError In validation.Errors
            Dim field As Control = Nothing
            If _fieldControls.TryGetValue(problem.Field, field) Then
                errorProvider.SetError(field, problem.Message)
                If first Is Nothing Then
                    first = field
                End If
            End If
        Next

        If first IsNot Nothing Then
            first.Focus()
        End If

        ErrorReporter.ShowInfo(Me, "Please correct the following:" & vbCrLf & vbCrLf & validation.ToString())
    End Sub

    Private Sub SetBusy(busy As Boolean)
        layoutFields.Enabled = Not busy
        btnSave.Enabled = Not busy
        btnSave.Text = If(busy, "Saving...", "Save Species")
        UseWaitCursor = busy
    End Sub

    ''' <summary>Asks before throwing away unsaved changes.</summary>
    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        If Me.DialogResult <> System.Windows.Forms.DialogResult.OK AndAlso _dirty AndAlso
           Not ErrorReporter.Confirm(Me, "Discard the changes you made to this species?", "Unsaved Changes") Then
            e.Cancel = True
        End If

        MyBase.OnFormClosing(e)
    End Sub

End Class
