Imports System.Reflection

Namespace Framework

    ''' <summary>
    ''' Finds every method marked &lt;Test&gt; or &lt;IntegrationTest&gt; in this assembly using
    ''' reflection, runs it, and prints PASS or FAIL. A tiny stand-in for NUnit/MSTest
    ''' so the solution builds in Visual Studio 2015 without downloading packages.
    ''' </summary>
    Public Class TestRunner

        Private ReadOnly _includeIntegrationTests As Boolean

        ''' <summary>Creates the runner.</summary>
        ''' <param name="includeIntegrationTests">False to skip tests that need the database.</param>
        Public Sub New(includeIntegrationTests As Boolean)
            _includeIntegrationTests = includeIntegrationTests
        End Sub

        ''' <summary>Number of tests that passed in the last run.</summary>
        Public Property Passed As Integer

        ''' <summary>Number of tests that failed in the last run.</summary>
        Public Property Failed As Integer

        ''' <summary>Number of tests skipped in the last run.</summary>
        Public Property Skipped As Integer

        ''' <summary>Runs all tests and prints the results to the console.</summary>
        Public Sub RunAll()
            Dim tests As List(Of MethodInfo) = Discover()
            Console.WriteLine("Running {0} tests...", tests.Count)
            Console.WriteLine()

            For Each test As MethodInfo In tests
                Dim isIntegration As Boolean = test.GetCustomAttribute(Of IntegrationTestAttribute)() IsNot Nothing
                Dim name As String = test.DeclaringType.Name & "." & test.Name

                If isIntegration AndAlso Not _includeIntegrationTests Then
                    Skipped += 1
                    Write(ConsoleColor.DarkGray, "SKIP ", name)
                    Continue For
                End If

                Dim stopwatch As Stopwatch = Stopwatch.StartNew()
                Try
                    Dim instance As Object = Activator.CreateInstance(test.DeclaringType)
                    Dim returned As Object = test.Invoke(instance, Nothing)

                    ' Async tests return a Task; wait for it so failures are caught here.
                    Dim pending As Task = TryCast(returned, Task)
                    If pending IsNot Nothing Then
                        pending.GetAwaiter().GetResult()
                    End If

                    Passed += 1
                    Write(ConsoleColor.Green, "PASS ", String.Format("{0} ({1} ms)", name, stopwatch.ElapsedMilliseconds))
                Catch ex As Exception
                    ' Reflection wraps exceptions thrown by the test in TargetInvocationException.
                    Dim actual As Exception = If(TypeOf ex Is TargetInvocationException AndAlso ex.InnerException IsNot Nothing, ex.InnerException, ex)
                    Failed += 1
                    Write(ConsoleColor.Red, "FAIL ", name)
                    Console.WriteLine("       " & actual.GetType().Name & ": " & actual.Message)
                End Try
            Next

            Console.WriteLine()
            Write(If(Failed = 0, ConsoleColor.Green, ConsoleColor.Red), String.Empty,
                String.Format("{0} passed, {1} failed, {2} skipped.", Passed, Failed, Skipped))
        End Sub

        Private Shared Function Discover() As List(Of MethodInfo)
            ' Classes in name order, methods in the order they are written in the file.
            Return GetType(TestRunner).Assembly.GetTypes().
                Where(Function(t) t.IsClass AndAlso Not t.IsAbstract).
                OrderBy(Function(t) t.Name, StringComparer.Ordinal).
                SelectMany(Function(t) t.GetMethods(BindingFlags.Public Or BindingFlags.Instance).
                    Where(Function(m) m.GetCustomAttribute(Of TestAttribute)() IsNot Nothing OrElse
                                      m.GetCustomAttribute(Of IntegrationTestAttribute)() IsNot Nothing).
                    OrderBy(Function(m) m.MetadataToken)).
                ToList()
        End Function

        Private Shared Sub Write(color As ConsoleColor, prefix As String, text As String)
            Dim previous As ConsoleColor = Console.ForegroundColor
            Console.ForegroundColor = color
            Console.Write(prefix)
            Console.ForegroundColor = previous
            Console.WriteLine(text)
        End Sub

    End Class

End Namespace
