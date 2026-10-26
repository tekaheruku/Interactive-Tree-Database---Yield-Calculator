Imports System.Data.OleDb
Imports TreeYield.Core.Models

Namespace Data

    ''' <summary>Reads the Categories table.</summary>
    Public Class CategoryRepository
        Inherits RepositoryBase

        ''' <summary>Creates the repository.</summary>
        Public Sub New(connectionFactory As AccessConnectionFactory)
            MyBase.New(connectionFactory)
        End Sub

        ''' <summary>Returns all categories in ID order.</summary>
        Public Function GetAllAsync() As Task(Of List(Of Category))
            Return QueryAsync("load the categories", Function(connection)
                Dim categories As New List(Of Category)()
                Using command As OleDbCommand = CreateCommand(connection,
                    "SELECT CategoryId, CategoryName, Description FROM Categories ORDER BY CategoryId")
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            categories.Add(New Category With {
                                .CategoryId = ReadInteger(reader, "CategoryId"),
                                .Name = ReadString(reader, "CategoryName"),
                                .Description = ReadString(reader, "Description")
                            })
                        End While
                    End Using
                End Using

                Return categories
            End Function)
        End Function

    End Class

End Namespace
