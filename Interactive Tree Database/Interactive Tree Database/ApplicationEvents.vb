Imports System.Configuration
Imports Microsoft.VisualBasic.ApplicationServices

Namespace My

    ' Events of the Visual Basic application framework (Project > Properties >
    ' Application > View Application Events):
    '   Startup            - raised before the main form is created.
    '   UnhandledException - raised when an error is not caught anywhere else.
    Partial Friend Class MyApplication

        ''' <summary>
        ''' Reads the settings and builds the services before MainForm is created.
        ''' If the settings file is broken the program explains why and stops.
        ''' </summary>
        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
            ' Errors on background tasks and non-UI threads are at least written to the log.
            AddHandler AppDomain.CurrentDomain.UnhandledException,
                Sub(s, args) ErrorReporter.Log(TryCast(args.ExceptionObject, Exception))
            AddHandler TaskScheduler.UnobservedTaskException,
                Sub(s, args)
                    ErrorReporter.Log(args.Exception)
                    args.SetObserved()
                End Sub

            Try
                Global.TreeYield.WinForms.AppServices.Current = Global.TreeYield.WinForms.AppServices.FromConfiguration()
            Catch ex As Exception When TypeOf ex Is ConfigurationErrorsException OrElse TypeOf ex Is ArgumentException
                ErrorReporter.Log(ex)
                MessageBox.Show(
                    "The program settings could not be read." & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf &
                    "Check the DatabaseFile setting in ""Interactive Tree Database.exe.config"".",
                    "Tree Yield & Revenue Calculator", MessageBoxButtons.OK, MessageBoxIcon.Error)
                e.Cancel = True
            End Try
        End Sub

        ''' <summary>
        ''' Last line of defense: any error that no other code caught is logged and
        ''' shown as a friendly message instead of crashing the program.
        ''' (Visual Studio's debugger stops on the error first, so this runs when the
        ''' program is started without debugging.)
        ''' </summary>
        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            ErrorReporter.HandleUnexpected(e.Exception)
            e.ExitApplication = False
        End Sub

    End Class

End Namespace
