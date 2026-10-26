Namespace Models

    ''' <summary>One row of the AuditLog table: who did what, and when.</summary>
    Public Class AuditEntry

        ''' <summary>Primary key.</summary>
        Public Property AuditId As Integer

        ''' <summary>The user who performed the action, or Nothing (for example a failed login with an unknown name).</summary>
        Public Property UserId As Integer?

        ''' <summary>Username loaded through a JOIN. Display only.</summary>
        Public Property Username As String

        ''' <summary>Action code, see <see cref="Services.AuditActions"/>.</summary>
        Public Property Action As String

        ''' <summary>Kind of record affected, for example "TreeSpecies".</summary>
        Public Property EntityName As String

        ''' <summary>Primary key of the affected record, if any.</summary>
        Public Property EntityId As Integer?

        ''' <summary>Human-readable description of the change.</summary>
        Public Property Details As String

        ''' <summary>When the action happened (UTC).</summary>
        Public Property OccurredAtUtc As DateTime

    End Class

End Namespace
