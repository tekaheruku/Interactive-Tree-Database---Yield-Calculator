Imports System.Globalization
Imports TreeYield.Core.Data
Imports TreeYield.Core.Models

Namespace Services

    ''' <summary>
    ''' Everything the UI can do with species records. Read operations are open to
    ''' everyone; changes require a signed-in administrator, are validated first,
    ''' and are written to the audit log.
    ''' </summary>
    Public Class SpeciesService

        Private Const EntityName As String = "TreeSpecies"

        Private ReadOnly _species As SpeciesRepository
        Private ReadOnly _categories As CategoryRepository
        Private ReadOnly _scenarios As ScenarioRepository
        Private ReadOnly _audit As AuditLogRepository
        Private ReadOnly _initializer As DatabaseInitializer

        ''' <summary>Creates the service.</summary>
        Public Sub New(species As SpeciesRepository, categories As CategoryRepository, scenarios As ScenarioRepository,
                       audit As AuditLogRepository, initializer As DatabaseInitializer)
            If species Is Nothing Then Throw New ArgumentNullException(NameOf(species))
            If categories Is Nothing Then Throw New ArgumentNullException(NameOf(categories))
            If scenarios Is Nothing Then Throw New ArgumentNullException(NameOf(scenarios))
            If audit Is Nothing Then Throw New ArgumentNullException(NameOf(audit))
            If initializer Is Nothing Then Throw New ArgumentNullException(NameOf(initializer))

            _species = species
            _categories = categories
            _scenarios = scenarios
            _audit = audit
            _initializer = initializer
        End Sub

        ''' <summary>Returns all categories.</summary>
        Public Function GetCategoriesAsync() As Task(Of List(Of Category))
            Return _categories.GetAllAsync()
        End Function

        ''' <summary>Returns every species, sorted by name.</summary>
        Public Function GetAllAsync() As Task(Of List(Of TreeSpecies))
            Return _species.GetAllAsync()
        End Function

        ''' <summary>Searches species by text, optionally within one category.</summary>
        Public Function SearchAsync(searchText As String, categoryId As Integer?) As Task(Of List(Of TreeSpecies))
            Return _species.SearchAsync(searchText, categoryId)
        End Function

        ''' <summary>Returns how many saved scenarios use the species.</summary>
        Public Function CountScenariosAsync(speciesId As Integer) As Task(Of Integer)
            Return _scenarios.CountBySpeciesAsync(speciesId)
        End Function

        ''' <summary>Returns the recent audit log entries.</summary>
        Public Function GetAuditLogAsync(count As Integer) As Task(Of List(Of AuditEntry))
            Return _audit.GetRecentAsync(count)
        End Function

        ''' <summary>
        ''' Validates and saves a species: INSERT when <see cref="TreeSpecies.SpeciesId"/> is 0,
        ''' otherwise UPDATE. Returns the species ID.
        ''' </summary>
        ''' <exception cref="UnauthorizedAccessException">No administrator is signed in.</exception>
        ''' <exception cref="ValidationException">A field is invalid.</exception>
        ''' <exception cref="DuplicateRecordException">The common name is already used.</exception>
        Public Async Function SaveAsync(species As TreeSpecies, actor As AppUser) As Task(Of Integer)
            RequireAdministrator(actor)

            Dim clean As TreeSpecies = Normalize(species)
            Dim validation As ValidationResult = SpeciesValidator.Validate(clean)
            If Not validation.IsValid Then
                Throw New ValidationException(validation)
            End If

            If clean.SpeciesId = 0 Then
                Dim newId As Integer = Await _species.InsertAsync(clean).ConfigureAwait(False)
                Await TryAuditAsync(actor, AuditActions.Create, newId, "Added species '" & clean.CommonName & "'.").ConfigureAwait(False)
                Return newId
            End If

            Await _species.UpdateAsync(clean).ConfigureAwait(False)
            Await TryAuditAsync(actor, AuditActions.Update, clean.SpeciesId, String.Format(CultureInfo.InvariantCulture,
                "Updated species '{0}' (yield {1} {2}/tree/yr, price {3}/{2}).",
                clean.CommonName, clean.YieldPerTreePerYear, clean.YieldUnit, clean.PricePerUnit)).ConfigureAwait(False)
            Return clean.SpeciesId
        End Function

        ''' <summary>Deletes a species and (through ON DELETE CASCADE) its saved scenarios.</summary>
        ''' <exception cref="UnauthorizedAccessException">No administrator is signed in.</exception>
        Public Async Function DeleteAsync(species As TreeSpecies, actor As AppUser) As Task
            RequireAdministrator(actor)
            If species Is Nothing Then
                Throw New ArgumentNullException(NameOf(species))
            End If

            Dim scenarioCount As Integer = Await _scenarios.CountBySpeciesAsync(species.SpeciesId).ConfigureAwait(False)
            Await _species.DeleteAsync(species.SpeciesId).ConfigureAwait(False)
            Await TryAuditAsync(actor, AuditActions.Delete, species.SpeciesId, String.Format(CultureInfo.InvariantCulture,
                "Deleted species '{0}' and {1} saved scenario(s).", species.CommonName, scenarioCount)).ConfigureAwait(False)
        End Function

        ''' <summary>Restores the original sample species (deletes all species and scenarios first).</summary>
        ''' <exception cref="UnauthorizedAccessException">No administrator is signed in.</exception>
        Public Async Function ResetSampleDataAsync(actor As AppUser) As Task
            RequireAdministrator(actor)
            Await _initializer.ResetSampleDataAsync().ConfigureAwait(False)
            Await TryAuditAsync(actor, AuditActions.ResetData, Nothing, "Restored the sample species data.").ConfigureAwait(False)
        End Function

        ''' <summary>
        ''' Returns a cleaned copy: text trimmed, empty optional text turned into Nothing,
        ''' and for rotation species "full production" set equal to the rotation age.
        ''' </summary>
        Public Shared Function Normalize(species As TreeSpecies) As TreeSpecies
            If species Is Nothing Then
                Return Nothing
            End If

            Dim copy As TreeSpecies = species.Clone()
            copy.CommonName = TrimToNothing(copy.CommonName)
            copy.ScientificName = TrimToNothing(copy.ScientificName)
            copy.ProductType = TrimToNothing(copy.ProductType)
            copy.YieldUnit = TrimToNothing(copy.YieldUnit)
            copy.HarvestSeason = TrimToNothing(copy.HarvestSeason)
            copy.Notes = TrimToNothing(copy.Notes)

            If copy.HarvestType = HarvestType.Rotation Then
                copy.YearsToFullProduction = copy.YearsToFirstHarvest
            End If

            Return copy
        End Function

        Private Shared Function TrimToNothing(value As String) As String
            If value Is Nothing Then
                Return Nothing
            End If

            Dim trimmed As String = value.Trim()
            Return If(trimmed.Length = 0, Nothing, trimmed)
        End Function

        Private Shared Sub RequireAdministrator(actor As AppUser)
            ' The UI hides the admin screens from guests; this check makes sure the
            ' rule also holds if a service method is ever called from somewhere else.
            If actor Is Nothing OrElse actor.UserId <= 0 Then
                Throw New UnauthorizedAccessException("You must be signed in as an administrator to change species records.")
            End If
        End Sub

        Private Async Function TryAuditAsync(actor As AppUser, action As String, entityId As Integer?, details As String) As Task
            Try
                Await _audit.AddAsync(actor.UserId, action, EntityName, entityId, details).ConfigureAwait(False)
            Catch ex As DataAccessException
                ' The data change already succeeded; a failed log entry must not undo it.
                Trace.TraceWarning("Audit log write failed: " & ex.ToString())
            End Try
        End Function

    End Class

End Namespace
