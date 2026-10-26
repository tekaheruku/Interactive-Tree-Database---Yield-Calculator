Namespace Models

    ''' <summary>
    ''' One tree species record from the TreeSpecies table: the agronomic and
    ''' market figures that the yield and revenue calculator needs.
    ''' </summary>
    Public Class TreeSpecies

        ''' <summary>Primary key. 0 means the record has not been saved yet.</summary>
        Public Property SpeciesId As Integer

        ''' <summary>Common or local name shown in the UI, for example "Carabao Mango". Unique.</summary>
        Public Property CommonName As String

        ''' <summary>Botanical name, for example "Mangifera indica". Optional.</summary>
        Public Property ScientificName As String

        ''' <summary>Foreign key to the Categories table.</summary>
        Public Property CategoryId As Integer

        ''' <summary>Category name loaded through a JOIN. Display only; not saved.</summary>
        Public Property CategoryName As String

        ''' <summary>What is sold, for example "Fresh fruit", "Timber" or "Cup lump".</summary>
        Public Property ProductType As String

        ''' <summary>Unit the yield is measured in, for example "kg", "pcs" or "board ft".</summary>
        Public Property YieldUnit As String

        ''' <summary>
        ''' Annual species: harvest of one mature tree in one year.
        ''' Rotation species: mean annual increment (growth per tree per year) that is
        ''' realized when the tree is harvested at rotation age.
        ''' </summary>
        Public Property YieldPerTreePerYear As Decimal

        ''' <summary>Estimated farm-gate price in Philippine pesos for one <see cref="YieldUnit"/>.</summary>
        Public Property PricePerUnit As Decimal

        ''' <summary>Whether the species is harvested every year or once at rotation age.</summary>
        Public Property HarvestType As HarvestType

        ''' <summary>
        ''' Annual: number of years after planting until the first harvest.
        ''' Rotation: rotation age, the number of years until the one-time harvest.
        ''' </summary>
        Public Property YearsToFirstHarvest As Integer

        ''' <summary>
        ''' Annual: year in which the tree reaches full production.
        ''' Rotation: always equal to <see cref="YearsToFirstHarvest"/>.
        ''' </summary>
        Public Property YearsToFullProduction As Integer

        ''' <summary>Usual harvest months, for example "March - June". Optional.</summary>
        Public Property HarvestSeason As String

        ''' <summary>Free-text remarks such as data sources or growing tips. Optional.</summary>
        Public Property Notes As String

        ''' <summary>When the record was created (UTC).</summary>
        Public Property CreatedAtUtc As DateTime

        ''' <summary>When the record was last changed (UTC).</summary>
        Public Property UpdatedAtUtc As DateTime

        ''' <summary>
        ''' Creates a shallow copy. The editor form works on a copy so that pressing
        ''' Cancel never changes the original object.
        ''' </summary>
        Public Function Clone() As TreeSpecies
            Return DirectCast(MemberwiseClone(), TreeSpecies)
        End Function

        ''' <summary>Returns the common name so the object can be shown directly in a ComboBox.</summary>
        Public Overrides Function ToString() As String
            Return If(CommonName, String.Empty)
        End Function

    End Class

End Namespace
