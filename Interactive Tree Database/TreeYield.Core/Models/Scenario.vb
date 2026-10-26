Namespace Models

    ''' <summary>
    ''' A saved calculation (row of the Scenarios table). Only the inputs are
    ''' stored; results are recalculated with the current species data when loaded.
    ''' </summary>
    Public Class Scenario

        ''' <summary>Primary key. 0 means not saved yet.</summary>
        Public Property ScenarioId As Integer

        ''' <summary>Name given by the user, for example "Backyard mango plan".</summary>
        Public Property Name As String

        ''' <summary>Foreign key to TreeSpecies.</summary>
        Public Property SpeciesId As Integer

        ''' <summary>Species common name loaded through a JOIN. Display only.</summary>
        Public Property SpeciesName As String

        ''' <summary>Number of trees in the scenario.</summary>
        Public Property TreeCount As Integer

        ''' <summary>Projection horizon in years.</summary>
        Public Property ProjectionYears As Integer

        ''' <summary>Optional remarks.</summary>
        Public Property Notes As String

        ''' <summary>When the scenario was saved (UTC).</summary>
        Public Property CreatedAtUtc As DateTime

    End Class

End Namespace
