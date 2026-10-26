Imports TreeYield.Core.Data
Imports TreeYield.Core.Models

Namespace Services

    ''' <summary>Saves, lists and deletes scenarios (saved calculations).</summary>
    Public Class ScenarioService

        ''' <summary>Maximum length of a scenario name.</summary>
        Public Const MaxNameLength As Integer = 100

        ''' <summary>Maximum length of scenario notes.</summary>
        Public Const MaxNotesLength As Integer = 500

        Private ReadOnly _scenarios As ScenarioRepository

        ''' <summary>Creates the service.</summary>
        Public Sub New(scenarios As ScenarioRepository)
            If scenarios Is Nothing Then
                Throw New ArgumentNullException(NameOf(scenarios))
            End If

            _scenarios = scenarios
        End Sub

        ''' <summary>Returns all scenarios, newest first.</summary>
        Public Function GetAllAsync() As Task(Of List(Of Scenario))
            Return _scenarios.GetAllAsync()
        End Function

        ''' <summary>Validates and saves a new scenario. Returns its ID.</summary>
        ''' <exception cref="ValidationException">A field is invalid.</exception>
        Public Function SaveAsync(scenario As Scenario) As Task(Of Integer)
            Dim validation As ValidationResult = Validate(scenario)
            If Not validation.IsValid Then
                Throw New ValidationException(validation)
            End If

            scenario.Name = scenario.Name.Trim()
            scenario.Notes = If(String.IsNullOrWhiteSpace(scenario.Notes), Nothing, scenario.Notes.Trim())
            Return _scenarios.InsertAsync(scenario)
        End Function

        ''' <summary>Deletes a scenario.</summary>
        Public Function DeleteAsync(scenarioId As Integer) As Task
            Return _scenarios.DeleteAsync(scenarioId)
        End Function

        ''' <summary>Checks a scenario before it is saved.</summary>
        Public Shared Function Validate(scenario As Scenario) As ValidationResult
            Dim result As New ValidationResult()
            If scenario Is Nothing Then
                result.Add("Scenario", "No scenario data was provided.")
                Return result
            End If

            If String.IsNullOrWhiteSpace(scenario.Name) Then
                result.Add(NameOf(Scenario.Name), "Enter a name for the scenario.")
            ElseIf scenario.Name.Trim().Length > MaxNameLength Then
                result.Add(NameOf(Scenario.Name), String.Format("The name must be {0} characters or fewer.", MaxNameLength))
            End If

            If scenario.Notes IsNot Nothing AndAlso scenario.Notes.Trim().Length > MaxNotesLength Then
                result.Add(NameOf(Scenario.Notes), String.Format("Notes must be {0} characters or fewer.", MaxNotesLength))
            End If

            If scenario.SpeciesId <= 0 Then
                result.Add(NameOf(Scenario.SpeciesId), "Select a species first.")
            End If

            If scenario.TreeCount < YieldCalculator.MinTreeCount OrElse scenario.TreeCount > YieldCalculator.MaxTreeCount Then
                result.Add(NameOf(Scenario.TreeCount), String.Format("Tree count must be from {0:N0} to {1:N0}.",
                    YieldCalculator.MinTreeCount, YieldCalculator.MaxTreeCount))
            End If

            If scenario.ProjectionYears < YieldCalculator.MinProjectionYears OrElse scenario.ProjectionYears > YieldCalculator.MaxProjectionYears Then
                result.Add(NameOf(Scenario.ProjectionYears), String.Format("The projection must cover {0} to {1} years.",
                    YieldCalculator.MinProjectionYears, YieldCalculator.MaxProjectionYears))
            End If

            Return result
        End Function

    End Class

End Namespace
