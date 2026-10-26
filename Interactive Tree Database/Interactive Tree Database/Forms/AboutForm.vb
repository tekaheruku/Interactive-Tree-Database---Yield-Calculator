Imports System.Reflection
Imports TreeYield.Core.Services

''' <summary>Shows the program version, the data disclaimer and the keyboard shortcuts.</summary>
Public Class AboutForm

    ''' <summary>Creates the dialog.</summary>
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        lblVersion.Text = "Version " & Assembly.GetExecutingAssembly().GetName().Version.ToString(3)
        lblData.Text = YieldCalculator.EstimateDisclaimer &
                       " The sample species and prices were prepared for this school project; administrators can update them at any time."
    End Sub

End Class
