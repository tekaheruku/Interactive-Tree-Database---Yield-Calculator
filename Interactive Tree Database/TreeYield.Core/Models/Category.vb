Namespace Models

    ''' <summary>
    ''' A tree classification (row of the Categories table) used by the
    ''' "All Species / Fruit-Bearing / Non-Fruit-Bearing" filter.
    ''' </summary>
    Public Class Category

        ''' <summary>Name of the fruit-bearing category as stored in the database.</summary>
        Public Const FruitBearing As String = "Fruit-Bearing"

        ''' <summary>Name of the non-fruit-bearing category as stored in the database.</summary>
        Public Const NonFruitBearing As String = "Non-Fruit-Bearing"

        ''' <summary>Primary key.</summary>
        Public Property CategoryId As Integer

        ''' <summary>Display name, for example "Fruit-Bearing". Unique.</summary>
        Public Property Name As String

        ''' <summary>Optional longer explanation of the category.</summary>
        Public Property Description As String

        ''' <summary>Returns the category name so the object can be shown directly in a ComboBox.</summary>
        Public Overrides Function ToString() As String
            Return If(Name, String.Empty)
        End Function

    End Class

End Namespace
