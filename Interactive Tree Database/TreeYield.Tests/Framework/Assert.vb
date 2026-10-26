Namespace Framework

    ''' <summary>Raised by <see cref="Assert"/> when a check fails.</summary>
    Public Class AssertionException
        Inherits Exception

        ''' <summary>Creates the exception.</summary>
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

    End Class

    ''' <summary>Minimal assertion helpers, so the tests need no external framework (no NuGet).</summary>
    Public NotInheritable Class Assert

        Private Sub New()
            ' Only Shared members.
        End Sub

        ''' <summary>Fails unless the two values are equal.</summary>
        Public Shared Sub AreEqual(Of T)(expected As T, actual As T, Optional message As String = Nothing)
            If Not Equals(expected, actual) Then
                Fail(String.Format("Expected <{0}> but was <{1}>. {2}", expected, actual, message))
            End If
        End Sub

        ''' <summary>Fails unless the condition is True.</summary>
        Public Shared Sub IsTrue(condition As Boolean, Optional message As String = Nothing)
            If Not condition Then
                Fail("Expected True. " & message)
            End If
        End Sub

        ''' <summary>Fails unless the condition is False.</summary>
        Public Shared Sub IsFalse(condition As Boolean, Optional message As String = Nothing)
            If condition Then
                Fail("Expected False. " & message)
            End If
        End Sub

        ''' <summary>Fails unless the value is Nothing.</summary>
        Public Shared Sub IsNull(value As Object, Optional message As String = Nothing)
            If value IsNot Nothing Then
                Fail(String.Format("Expected Nothing but was <{0}>. {1}", value, message))
            End If
        End Sub

        ''' <summary>Fails if the value is Nothing.</summary>
        Public Shared Sub IsNotNull(value As Object, Optional message As String = Nothing)
            If value Is Nothing Then
                Fail("Expected a value but was Nothing. " & message)
            End If
        End Sub

        ''' <summary>Fails unless the action throws <typeparamref name="TException"/> (or a subclass).</summary>
        Public Shared Function Throws(Of TException As Exception)(action As Action) As TException
            Try
                action()
            Catch ex As TException
                Return ex
            Catch ex As Exception
                Fail(String.Format("Expected {0} but {1} was thrown: {2}", GetType(TException).Name, ex.GetType().Name, ex.Message))
            End Try

            Fail(String.Format("Expected {0} but nothing was thrown.", GetType(TException).Name))
            Return Nothing
        End Function

        ''' <summary>Async version of <see cref="Throws"/>.</summary>
        Public Shared Async Function ThrowsAsync(Of TException As Exception)(action As Func(Of Task)) As Task(Of TException)
            Dim caught As Exception = Nothing
            Try
                Await action()
            Catch ex As Exception
                caught = ex
            End Try

            If caught Is Nothing Then
                Fail(String.Format("Expected {0} but nothing was thrown.", GetType(TException).Name))
            End If

            Dim expected As TException = TryCast(caught, TException)
            If expected Is Nothing Then
                Fail(String.Format("Expected {0} but {1} was thrown: {2}", GetType(TException).Name, caught.GetType().Name, caught.Message))
            End If

            Return expected
        End Function

        ''' <summary>Fails the test with a message.</summary>
        Public Shared Sub Fail(message As String)
            Throw New AssertionException(message)
        End Sub

    End Class

End Namespace
