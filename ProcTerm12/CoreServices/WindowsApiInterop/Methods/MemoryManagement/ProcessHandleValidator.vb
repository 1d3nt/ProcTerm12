Namespace CoreServices.WindowsApiInterop.Methods.MemoryManagement

    ''' <summary>
    ''' Provides methods for validating various types of handles.
    ''' </summary>
    Friend Class ProcessHandleValidator
        ''' <summary>
        ''' Validates the specified process handle.
        ''' </summary>
        ''' <param name="processHandle">
        ''' The handle to validate.
        ''' </param>
        ''' <exception cref="ArgumentException">
        ''' Thrown when the handle is invalid, closed, or null.
        ''' </exception>
        Friend Shared Sub ValidateProcessHandle(processHandle As SafeProcessHandle)
            If processHandle Is Nothing OrElse processHandle.IsClosed OrElse processHandle.IsInvalid Then
                Throw New ArgumentException("Invalid process handle.")
            End If
        End Sub

        ''' <summary>
        ''' Validates the specified debug handle.
        ''' </summary>
        ''' <param name="debugHandle">
        ''' The handle to validate.
        ''' </param>
        ''' <exception cref="ArgumentException">
        ''' Thrown when the handle is invalid, closed, or null.
        ''' </exception>
        Friend Shared Sub ValidateDebugHandle(debugHandle As SafeDebugHandle)
            If debugHandle Is Nothing OrElse debugHandle.IsClosed OrElse debugHandle.IsInvalid Then
                Throw New ArgumentException("Invalid debug handle.")
            End If
        End Sub

        ''' <summary>
        ''' Validates the specified job handle.
        ''' </summary>
        ''' <param name="jobHandle">
        ''' The handle to validate.
        ''' </param>
        ''' <exception cref="ArgumentException">
        ''' Thrown when the handle is invalid, closed, or null.
        ''' </exception>
        Friend Shared Sub ValidateJobHandle(jobHandle As SafeJobHandle)
            If jobHandle Is Nothing OrElse jobHandle.IsClosed OrElse jobHandle.IsInvalid Then
                Throw New ArgumentException("Invalid job handle.")
            End If
        End Sub

        ''' <summary>
        ''' Validates the specified thread handle.
        ''' </summary>
        ''' <param name="threadHandle">
        ''' The handle to validate.
        ''' </param>
        ''' <exception cref="ArgumentException">
        ''' Thrown when the handle is invalid, closed, or null.
        ''' </exception>
        Friend Shared Sub ValidateThreadHandle(threadHandle As SafeThreadHandle)
            If threadHandle Is Nothing OrElse threadHandle.IsClosed OrElse threadHandle.IsInvalid Then
                Throw New ArgumentException("Invalid thread handle.")
            End If
        End Sub

        ''' <summary>
        ''' Validates the specified token handle.
        ''' </summary>
        ''' <param name="tokenHandle">
        ''' The handle to validate.
        ''' </param>
        ''' <exception cref="ArgumentException">
        ''' Thrown when the handle is invalid, closed, or null.
        ''' </exception>
        Friend Shared Sub ValidateTokenHandle(tokenHandle As SafeTokenHandle)
            If tokenHandle Is Nothing OrElse tokenHandle.IsClosed OrElse tokenHandle.IsInvalid Then
                Throw New ArgumentException("Invalid token handle.")
            End If
        End Sub
    End Class
End Namespace
