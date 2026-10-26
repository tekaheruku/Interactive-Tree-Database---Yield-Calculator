Imports System.Configuration
Imports TreeYield.Core.Data
Imports TreeYield.Core.Services

''' <summary>
''' The "composition root": creates every repository and service once and hands
''' them to the forms. Forms never create database objects themselves, which keeps
''' the presentation layer separate from the data layer.
''' </summary>
Friend NotInheritable Class AppServices

    ''' <summary>appSettings key with the path of the Access database file.</summary>
    Public Const DatabaseFileSetting As String = "DatabaseFile"

    ''' <summary>appSettings key with the OLE DB provider name.</summary>
    Public Const DatabaseProviderSetting As String = "DatabaseProvider"

    ''' <summary>Used when App.config does not name a database file.</summary>
    Public Const DefaultDatabaseFile As String = "Database\TreeYieldDb.mdb"

    ''' <summary>
    ''' The services for the running program. Set once at startup by
    ''' ApplicationEvents.vb, before the main window is created.
    ''' </summary>
    Public Shared Property Current As AppServices

    ''' <summary>Builds all services for one Access database file.</summary>
    ''' <param name="databasePath">Full path of the .mdb file (created if missing).</param>
    ''' <param name="provider">OLE DB provider, normally Microsoft.Jet.OLEDB.4.0.</param>
    Public Sub New(databasePath As String, provider As String)
        ConnectionFactory = New AccessConnectionFactory(databasePath, provider)

        Dim speciesRepository As New SpeciesRepository(ConnectionFactory)
        Dim categoryRepository As New CategoryRepository(ConnectionFactory)
        Dim scenarioRepository As New ScenarioRepository(ConnectionFactory)
        Dim userRepository As New UserRepository(ConnectionFactory)
        Dim auditRepository As New AuditLogRepository(ConnectionFactory)

        Initializer = New DatabaseInitializer(ConnectionFactory)
        Species = New SpeciesService(speciesRepository, categoryRepository, scenarioRepository, auditRepository, Initializer)
        Scenarios = New ScenarioService(scenarioRepository)
        Auth = New AuthService(userRepository, auditRepository)
        Session = New AdminSession()
    End Sub

    ''' <summary>Opens connections to the Access database file.</summary>
    Public ReadOnly Property ConnectionFactory As AccessConnectionFactory

    ''' <summary>Creates the database file and its tables when needed.</summary>
    Public ReadOnly Property Initializer As DatabaseInitializer

    ''' <summary>Species records (read for everyone, write for administrators).</summary>
    Public ReadOnly Property Species As SpeciesService

    ''' <summary>Saved scenarios.</summary>
    Public ReadOnly Property Scenarios As ScenarioService

    ''' <summary>Administrator login and passwords.</summary>
    Public ReadOnly Property Auth As AuthService

    ''' <summary>Who is signed in as administrator right now.</summary>
    Public ReadOnly Property Session As AdminSession

    ''' <summary>
    ''' Reads the database settings from "Interactive Tree Database.exe.config" and finds
    ''' the database file. A relative path is looked up next to the program first and then
    ''' in the folders above it, so running from Visual Studio (bin\Debug) uses the copy in
    ''' the project folder - the same file you open in Microsoft Access.
    ''' </summary>
    ''' <exception cref="ConfigurationErrorsException">The settings file cannot be read.</exception>
    Public Shared Function FromConfiguration() As AppServices
        Dim databaseFile As String = ConfigurationManager.AppSettings(DatabaseFileSetting)
        If String.IsNullOrWhiteSpace(databaseFile) Then
            databaseFile = DefaultDatabaseFile
        End If

        Dim provider As String = ConfigurationManager.AppSettings(DatabaseProviderSetting)
        If String.IsNullOrWhiteSpace(provider) Then
            provider = AccessConnectionFactory.DefaultProvider
        End If

        Dim databasePath As String = DatabaseFileLocator.Locate(Application.StartupPath, databaseFile.Trim())
        Return New AppServices(databasePath, provider.Trim())
    End Function

    ''' <summary>Creates the database if needed and makes sure an administrator account exists.</summary>
    Public Async Function InitializeDatabaseAsync() As Task
        Await Initializer.InitializeAsync()
        Await Auth.EnsureDefaultAdminAsync()
    End Function

End Class
