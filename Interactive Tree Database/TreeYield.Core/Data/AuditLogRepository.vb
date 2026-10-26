Imports System.Data.OleDb
Imports System.Globalization
Imports TreeYield.Core.Models

Namespace Data

    ''' <summary>Writes and reads the AuditLog table (history of administrator actions).</summary>
    Public Class AuditLogRepository
        Inherits RepositoryBase

        ''' <summary>Maximum length of the Details text.</summary>
        Public Const MaxDetailsLength As Integer = 500

        ''' <summary>Largest number of entries <see cref="GetRecentAsync"/> returns.</summary>
        Public Const MaxEntries As Integer = 1000

        ''' <summary>Creates the repository.</summary>
        Public Sub New(connectionFactory As AccessConnectionFactory)
            MyBase.New(connectionFactory)
        End Sub

        ''' <summary>Adds one audit entry.</summary>
        ''' <param name="userId">Who did it (Nothing if unknown).</param>
        ''' <param name="action">Action code from <see cref="Services.AuditActions"/>.</param>
        ''' <param name="entityName">Kind of record, for example "TreeSpecies".</param>
        ''' <param name="entityId">ID of the record, if any.</param>
        ''' <param name="details">Description; cut to <see cref="MaxDetailsLength"/> characters.</param>
        Public Function AddAsync(userId As Integer?, action As String, entityName As String, entityId As Integer?, details As String) As Task
            Const sql As String =
                "INSERT INTO AuditLog (UserId, ActionCode, EntityName, EntityId, Details, OccurredAt) " &
                "VALUES (?, ?, ?, ?, ?, ?)"

            Dim safeDetails As String = If(details IsNot Nothing AndAlso details.Length > MaxDetailsLength,
                                           details.Substring(0, MaxDetailsLength), details)

            Return ExecuteAsync("write the audit log", Sub(connection)
                Using command As OleDbCommand = CreateCommand(connection, sql)
                    AddInteger(command, userId)
                    AddText(command, action, 30)
                    AddText(command, entityName, 50)
                    AddInteger(command, entityId)
                    AddMemo(command, safeDetails)
                    AddDate(command, UtcNow())
                    command.ExecuteNonQuery()
                End Using
            End Sub)
        End Function

        ''' <summary>Returns the most recent entries, newest first.</summary>
        ''' <param name="count">How many entries to return (1 to <see cref="MaxEntries"/>).</param>
        Public Function GetRecentAsync(count As Integer) As Task(Of List(Of AuditEntry))
            ' Access does not accept a parameter after TOP, so the number is written into
            ' the SQL - safely, because it is an Integer limited to 1..1000, not user text.
            Dim top As Integer = Math.Max(1, Math.Min(MaxEntries, count))
            Dim sql As String =
                "SELECT TOP " & top.ToString(CultureInfo.InvariantCulture) & " a.AuditId, a.UserId, u.Username, " &
                "a.ActionCode, a.EntityName, a.EntityId, a.Details, a.OccurredAt " &
                "FROM AuditLog AS a LEFT JOIN Users AS u ON a.UserId = u.UserId " &
                "ORDER BY a.OccurredAt DESC, a.AuditId DESC"

            Return QueryAsync("load the audit log", Function(connection)
                Dim entries As New List(Of AuditEntry)()
                Using command As OleDbCommand = CreateCommand(connection, sql)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            entries.Add(New AuditEntry With {
                                .AuditId = ReadInteger(reader, "AuditId"),
                                .UserId = ReadNullableInteger(reader, "UserId"),
                                .Username = ReadString(reader, "Username"),
                                .Action = ReadString(reader, "ActionCode"),
                                .EntityName = ReadString(reader, "EntityName"),
                                .EntityId = ReadNullableInteger(reader, "EntityId"),
                                .Details = ReadString(reader, "Details"),
                                .OccurredAtUtc = ReadDate(reader, "OccurredAt")
                            })
                        End While
                    End Using
                End Using

                Return entries
            End Function)
        End Function

    End Class

End Namespace
