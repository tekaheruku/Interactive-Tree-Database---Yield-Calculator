Imports System.Data.OleDb
Imports TreeYield.Core.Models

Namespace Data

    ''' <summary>Reads and writes the Scenarios table (saved calculations).</summary>
    Public Class ScenarioRepository
        Inherits RepositoryBase

        ''' <summary>Creates the repository.</summary>
        Public Sub New(connectionFactory As AccessConnectionFactory)
            MyBase.New(connectionFactory)
        End Sub

        ''' <summary>Returns all scenarios, newest first, with the species name joined in.</summary>
        Public Function GetAllAsync() As Task(Of List(Of Scenario))
            Const sql As String =
                "SELECT sc.ScenarioId, sc.ScenarioName, sc.SpeciesId, sp.CommonName, " &
                "sc.TreeCount, sc.ProjectionYears, sc.Notes, sc.CreatedAt " &
                "FROM Scenarios AS sc INNER JOIN TreeSpecies AS sp ON sc.SpeciesId = sp.SpeciesId " &
                "ORDER BY sc.CreatedAt DESC, sc.ScenarioId DESC"

            Return QueryAsync("load the saved scenarios", Function(connection)
                Dim scenarios As New List(Of Scenario)()
                Using command As OleDbCommand = CreateCommand(connection, sql)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            scenarios.Add(New Scenario With {
                                .ScenarioId = ReadInteger(reader, "ScenarioId"),
                                .Name = ReadString(reader, "ScenarioName"),
                                .SpeciesId = ReadInteger(reader, "SpeciesId"),
                                .SpeciesName = ReadString(reader, "CommonName"),
                                .TreeCount = ReadInteger(reader, "TreeCount"),
                                .ProjectionYears = ReadInteger(reader, "ProjectionYears"),
                                .Notes = ReadString(reader, "Notes"),
                                .CreatedAtUtc = ReadDate(reader, "CreatedAt")
                            })
                        End While
                    End Using
                End Using

                Return scenarios
            End Function)
        End Function

        ''' <summary>Saves a new scenario and returns its generated ID.</summary>
        Public Function InsertAsync(scenario As Scenario) As Task(Of Integer)
            Const sql As String =
                "INSERT INTO Scenarios (ScenarioName, SpeciesId, TreeCount, ProjectionYears, Notes, CreatedAt) " &
                "VALUES (?, ?, ?, ?, ?, ?)"

            Return QueryAsync("save the scenario", Function(connection)
                Using command As OleDbCommand = CreateCommand(connection, sql)
                    AddText(command, scenario.Name, 100)
                    AddInteger(command, scenario.SpeciesId)
                    AddInteger(command, scenario.TreeCount)
                    AddInteger(command, scenario.ProjectionYears)
                    AddMemo(command, scenario.Notes)
                    AddDate(command, UtcNow())
                    command.ExecuteNonQuery()
                End Using

                Return LastInsertedId(connection)
            End Function)
        End Function

        ''' <summary>Deletes one scenario.</summary>
        Public Function DeleteAsync(scenarioId As Integer) As Task
            Return ExecuteAsync("delete the scenario", Sub(connection)
                Using command As OleDbCommand = CreateCommand(connection, "DELETE FROM Scenarios WHERE ScenarioId = ?")
                    AddInteger(command, scenarioId)
                    command.ExecuteNonQuery()
                End Using
            End Sub)
        End Function

        ''' <summary>Counts the scenarios that use a species (shown before deleting the species).</summary>
        Public Function CountBySpeciesAsync(speciesId As Integer) As Task(Of Integer)
            Return QueryAsync("count the saved scenarios", Function(connection)
                Using command As OleDbCommand = CreateCommand(connection, "SELECT COUNT(*) FROM Scenarios WHERE SpeciesId = ?")
                    AddInteger(command, speciesId)
                    Return Convert.ToInt32(command.ExecuteScalar())
                End Using
            End Function)
        End Function

    End Class

End Namespace
