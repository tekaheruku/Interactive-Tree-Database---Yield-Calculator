Imports TreeYield.Tests.Framework

''' <summary>
''' Entry point of the test runner.
''' <para>Usage: TreeYield.Tests.exe [--unit-only] [--no-pause]</para>
''' The exit code is 0 when every test passed and 1 otherwise.
''' </summary>
Module Program

    Function Main(args As String()) As Integer
        Dim unitOnly As Boolean = args.Contains("--unit-only", StringComparer.OrdinalIgnoreCase)
        Dim noPause As Boolean = args.Contains("--no-pause", StringComparer.OrdinalIgnoreCase)

        Console.WriteLine("Interactive Tree Database & Yield Calculator - automated tests")
        Console.WriteLine(If(unitOnly, "Mode: unit tests only", "Mode: unit + database integration tests (Microsoft Access .mdb)"))
        Console.WriteLine()

        Dim runner As New TestRunner(Not unitOnly)
        Try
            runner.RunAll()
        Finally
            ' Remove the throw-away test database file.
            TestDatabase.DeleteIfCreated()
        End Try

        ' Keep the console window open when started with F5 from Visual Studio.
        If Not noPause AndAlso Environment.UserInteractive AndAlso Not Console.IsInputRedirected Then
            Console.WriteLine()
            Console.WriteLine("Press any key to close...")
            Console.ReadKey(True)
        End If

        Return If(runner.Failed = 0, 0, 1)
    End Function

End Module
