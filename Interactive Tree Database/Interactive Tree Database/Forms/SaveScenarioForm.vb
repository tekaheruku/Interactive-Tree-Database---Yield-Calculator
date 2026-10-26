Imports TreeYield.Core.Models
Imports TreeYield.Core.Services

''' <summary>Asks for a name (and optional notes) before the current calculation is saved.</summary>
Public Class SaveScenarioForm

    Private ReadOnly _species As TreeSpecies
    Private ReadOnly _treeCount As Integer
    Private ReadOnly _years As Integer

    ''' <summary>Creates the dialog for the current calculation.</summary>
    Public Sub New(species As TreeSpecies, treeCount As Integer, years As Integer, result As YieldResult)
        If species Is Nothing Then
            Throw New ArgumentNullException(NameOf(species))
        End If

        If result Is Nothing Then
            Throw New ArgumentNullException(NameOf(result))
        End If

        _species = species
        _treeCount = treeCount
        _years = years

        ' This call is required by the designer.
        InitializeComponent()

        txtName.MaxLength = ScenarioService.MaxNameLength
        txtNotes.MaxLength = ScenarioService.MaxNotesLength
        lblSummary.Text = String.Format("{0}  x  {1:N0} trees{2}{3}-year horizon{4}Estimated annual gross sales: {5}",
                                        species.CommonName, treeCount, Formatters.Separator, years, Environment.NewLine,
                                        Formatters.Peso(result.GrossSales))
        txtName.Text = String.Format("{0} - {1:N0} trees", species.CommonName, treeCount)
    End Sub

    ''' <summary>The name typed by the user (trimmed).</summary>
    Public ReadOnly Property ScenarioName As String
        Get
            Return txtName.Text.Trim()
        End Get
    End Property

    ''' <summary>The notes typed by the user.</summary>
    Public ReadOnly Property ScenarioNotes As String
        Get
            Return txtNotes.Text
        End Get
    End Property

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        txtName.SelectAll()
        txtName.Focus()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Same rules the service applies before saving, checked here to show them next to the fields.
        Dim validation As ValidationResult = ScenarioService.Validate(New Scenario With {
            .Name = txtName.Text,
            .Notes = txtNotes.Text,
            .SpeciesId = _species.SpeciesId,
            .TreeCount = _treeCount,
            .ProjectionYears = _years
        })

        Dim nameError As String = validation.GetError(NameOf(Scenario.Name))
        errorProvider.SetError(txtName, If(nameError, String.Empty))
        errorProvider.SetError(txtNotes, If(validation.GetError(NameOf(Scenario.Notes)), String.Empty))

        If Not validation.IsValid Then
            If nameError IsNot Nothing Then
                txtName.Focus()
            Else
                txtNotes.Focus()
            End If

            Return
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

End Class
