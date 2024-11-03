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
    End Class
End Namespace
