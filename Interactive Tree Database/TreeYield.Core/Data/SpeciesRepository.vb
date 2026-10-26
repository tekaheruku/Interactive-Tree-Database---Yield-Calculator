Imports System.Data.OleDb
Imports System.Text
Imports TreeYield.Core.Models

Namespace Data

    ''' <summary>Create, read, update and delete (CRUD) for the TreeSpecies table.</summary>
    Public Class SpeciesRepository
        Inherits RepositoryBase

        ' One column list shared by every SELECT so the Map function always finds its columns.
        Private Const SelectColumns As String =
            "SELECT s.SpeciesId, s.CommonName, s.ScientificName, s.CategoryId, c.CategoryName, " &
            "s.ProductType, s.YieldUnit, s.YieldPerTreePerYear, s.PricePerUnit, s.HarvestType, " &
            "s.YearsToFirstHarvest, s.YearsToFullProduction, s.HarvestSeason, s.Notes, s.CreatedAt, s.UpdatedAt " &
            "FROM TreeSpecies AS s INNER JOIN Categories AS c ON s.CategoryId = c.CategoryId"

        ''' <summary>Creates the repository.</summary>
        Public Sub New(connectionFactory As AccessConnectionFactory)
            MyBase.New(connectionFactory)
        End Sub

        ''' <summary>Returns every species sorted by common name.</summary>
        Public Function GetAllAsync() As Task(Of List(Of TreeSpecies))
            Return SearchAsync(Nothing, Nothing)
        End Function

        ''' <summary>
        ''' Searches by name, scientific name or product, optionally limited to one category.
        ''' The search text is passed as a parameter, so input such as <c>' OR 1=1 --</c>
        ''' is only ever compared as text and can never change the query.
        ''' </summary>
        ''' <param name="searchText">Text to look for (case-insensitive, matches anywhere). Nothing or blank means no text filter.</param>
        ''' <param name="categoryId">Category to keep, or Nothing for all categories.</param>
        Public Function SearchAsync(searchText As String, categoryId As Integer?) As Task(Of List(Of TreeSpecies))
            Dim trimmed As String = If(searchText Is Nothing, Nothing, searchText.Trim())
            Dim pattern As String = If(String.IsNullOrEmpty(trimmed), Nothing, "%" & SqlText.EscapeLikePattern(trimmed) & "%")

            Return QueryAsync("search the species list", Function(connection)
                Dim results As New List(Of TreeSpecies)()
                Using command As OleDbCommand = CreateCommand(connection, String.Empty)
                    ' Only the SHAPE of the WHERE clause depends on the input (is there a
                    ' category? a search text?). The values themselves are always parameters.
                    Dim conditions As New List(Of String)()
                    If categoryId.HasValue Then
                        conditions.Add("s.CategoryId = ?")
                        AddInteger(command, categoryId.Value)
                    End If

                    If pattern IsNot Nothing Then
                        conditions.Add("(s.CommonName LIKE ? OR s.ScientificName LIKE ? OR s.ProductType LIKE ?)")
                        AddText(command, pattern, 255)
                        AddText(command, pattern, 255)
                        AddText(command, pattern, 255)
                    End If

                    Dim sql As New StringBuilder(SelectColumns)
                    If conditions.Count > 0 Then
                        sql.Append(" WHERE ").Append(String.Join(" AND ", conditions))
                    End If

                    sql.Append(" ORDER BY s.CommonName")
                    command.CommandText = sql.ToString()

                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            results.Add(Map(reader))
                        End While
                    End Using
                End Using

                Return results
            End Function)
        End Function

        ''' <summary>Returns one species, or Nothing if it does not exist.</summary>
        Public Function GetByIdAsync(speciesId As Integer) As Task(Of TreeSpecies)
            Return QueryAsync("load the species", Function(connection)
                Using command As OleDbCommand = CreateCommand(connection, SelectColumns & " WHERE s.SpeciesId = ?")
                    AddInteger(command, speciesId)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        Return If(reader.Read(), Map(reader), Nothing)
                    End Using
                End Using
            End Function)
        End Function

        ''' <summary>Inserts a new species and returns its generated ID.</summary>
        ''' <exception cref="DuplicateRecordException">Another species already has the same common name.</exception>
        Public Function InsertAsync(species As TreeSpecies) As Task(Of Integer)
            Const sql As String =
                "INSERT INTO TreeSpecies (CommonName, ScientificName, CategoryId, ProductType, YieldUnit, " &
                "YieldPerTreePerYear, PricePerUnit, HarvestType, YearsToFirstHarvest, YearsToFullProduction, " &
                "HarvestSeason, Notes, CreatedAt, UpdatedAt) " &
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"

            Return QueryAsync("add the species", Function(connection)
                Dim now As DateTime = UtcNow()
                Using command As OleDbCommand = CreateCommand(connection, sql)
                    AddSpeciesValues(command, species)
                    AddDate(command, now)
                    AddDate(command, now)
                    command.ExecuteNonQuery()
                End Using

                Return LastInsertedId(connection)
            End Function)
        End Function

        ''' <summary>Saves changes to an existing species.</summary>
        ''' <exception cref="DuplicateRecordException">Another species already has the same common name.</exception>
        ''' <exception cref="DataAccessException">The species was deleted in the meantime.</exception>
        Public Function UpdateAsync(species As TreeSpecies) As Task
            Const sql As String =
                "UPDATE TreeSpecies SET CommonName = ?, ScientificName = ?, CategoryId = ?, ProductType = ?, " &
                "YieldUnit = ?, YieldPerTreePerYear = ?, PricePerUnit = ?, HarvestType = ?, " &
                "YearsToFirstHarvest = ?, YearsToFullProduction = ?, HarvestSeason = ?, Notes = ?, UpdatedAt = ? " &
                "WHERE SpeciesId = ?"

            Return ExecuteAsync("update the species", Sub(connection)
                Using command As OleDbCommand = CreateCommand(connection, sql)
                    AddSpeciesValues(command, species)
                    AddDate(command, UtcNow())
                    AddInteger(command, species.SpeciesId)

                    If command.ExecuteNonQuery() = 0 Then
                        Throw New DataAccessException("This species no longer exists. It may have been deleted.", Nothing)
                    End If
                End Using
            End Sub)
        End Function

        ''' <summary>
        ''' Deletes a species. Its saved scenarios are deleted with it by the
        ''' ON DELETE CASCADE rule of the Scenarios relationship.
        ''' </summary>
        Public Function DeleteAsync(speciesId As Integer) As Task
            Return ExecuteAsync("delete the species", Sub(connection)
                Using command As OleDbCommand = CreateCommand(connection, "DELETE FROM TreeSpecies WHERE SpeciesId = ?")
                    AddInteger(command, speciesId)
                    command.ExecuteNonQuery()
                End Using
            End Sub)
        End Function

        ''' <summary>Adds the 12 editable columns in the order used by the INSERT and UPDATE statements.</summary>
        Private Shared Sub AddSpeciesValues(command As OleDbCommand, s As TreeSpecies)
            AddText(command, s.CommonName, 100)
            AddText(command, s.ScientificName, 150)
            AddInteger(command, s.CategoryId)
            AddText(command, s.ProductType, 60)
            AddText(command, s.YieldUnit, 20)
            AddDecimal(command, s.YieldPerTreePerYear)
            AddDecimal(command, s.PricePerUnit)
            AddText(command, s.HarvestType.ToString(), 10)
            AddInteger(command, s.YearsToFirstHarvest)
            AddInteger(command, s.YearsToFullProduction)
            AddText(command, s.HarvestSeason, 100)
            AddMemo(command, s.Notes)
        End Sub

        Private Shared Function Map(reader As OleDbDataReader) As TreeSpecies
            Dim kind As HarvestType
            If Not [Enum].TryParse(ReadString(reader, "HarvestType"), True, kind) Then
                kind = HarvestType.Annual
            End If

            Return New TreeSpecies With {
                .SpeciesId = ReadInteger(reader, "SpeciesId"),
                .CommonName = ReadString(reader, "CommonName"),
                .ScientificName = ReadString(reader, "ScientificName"),
                .CategoryId = ReadInteger(reader, "CategoryId"),
                .CategoryName = ReadString(reader, "CategoryName"),
                .ProductType = ReadString(reader, "ProductType"),
                .YieldUnit = ReadString(reader, "YieldUnit"),
                .YieldPerTreePerYear = ReadDecimal(reader, "YieldPerTreePerYear"),
                .PricePerUnit = ReadDecimal(reader, "PricePerUnit"),
                .HarvestType = kind,
                .YearsToFirstHarvest = ReadInteger(reader, "YearsToFirstHarvest"),
                .YearsToFullProduction = ReadInteger(reader, "YearsToFullProduction"),
                .HarvestSeason = ReadString(reader, "HarvestSeason"),
                .Notes = ReadString(reader, "Notes"),
                .CreatedAtUtc = ReadDate(reader, "CreatedAt"),
                .UpdatedAtUtc = ReadDate(reader, "UpdatedAt")
            }
        End Function

    End Class

End Namespace
