Namespace Framework

    ''' <summary>
    ''' Marks a public method as a unit test (no database needed). The method must take
    ''' no parameters and be a Sub or a Function that returns a Task.
    ''' </summary>
    <AttributeUsage(AttributeTargets.Method)>
    Public NotInheritable Class TestAttribute
        Inherits Attribute
    End Class

    ''' <summary>
    ''' Marks a test that needs a real Access database file. Skipped when the runner is
    ''' started with --unit-only.
    ''' </summary>
    <AttributeUsage(AttributeTargets.Method)>
    Public NotInheritable Class IntegrationTestAttribute
        Inherits Attribute
    End Class

End Namespace
