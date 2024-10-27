Namespace CoreServices.WindowsApiInterop.Methods.MemoryManagement

    ''' <summary>
    ''' Provides utility methods for managing handles in P/Invoke operations.
    ''' </summary>
    ''' <remarks>
    ''' This class is marked as <c>NotInheritable</c> to prevent inheritance.
    ''' </remarks>
    Friend NotInheritable Class HandleManager

        ''' <summary>
        ''' Closes the token handle if it is not null.
        ''' </summary>
        ''' <param name="tokenHandle">The handle to be closed.</param>
        ''' <remarks>
        ''' This method checks if the provided <paramref name="tokenHandle"/> is not equal to <see cref="NativeMethods.NullHandleValue"/>. 
        ''' If it is not equal, the method proceeds to close the handle by calling the <see cref="NativeMethods.CloseHandle"/> method. 
        ''' If the handle is equal to <see cref="NativeMethods.NullHandleValue"/>, which indicates an invalid or uninitialized handle, 
        ''' the method skips the closing operation.
        ''' </remarks>
        Friend Shared Sub CloseTokenHandleIfNotNull(tokenHandle As SafeTokenHandle)
            Try
                If tokenHandle IsNot Nothing AndAlso Not tokenHandle.IsInvalid Then
                    tokenHandle.Dispose()
                End If
            Catch ex As SEHException
                LogException("SEHException occurred while closing token handle", ex)
            Catch ex As Exception
                LogException("An unexpected error occurred while closing token handle", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Closes the process handle if it is not null.
        ''' </summary>
        ''' <param name="processHandle">The handle to be closed.</param>
        ''' <remarks>
        ''' This method checks if the provided <paramref name="processHandle"/> is valid before disposing of it.
        ''' </remarks>
        Friend Shared Sub CloseProcessHandleIfNotNull(processHandle As SafeProcessHandle)
            Try
                If processHandle IsNot Nothing AndAlso Not processHandle.IsInvalid AndAlso Not processHandle.IsClosed Then
                    processHandle.Dispose()
                End If
            Catch ex As SEHException
                LogException("SEHException occurred while closing process handle", ex)
            Catch ex As Exception
                LogException("An unexpected error occurred while closing process handle", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Logs exception details to the trace.
        ''' </summary>
        ''' <param name="message">The custom message to log.</param>
        ''' <param name="ex">The exception to log.</param>
        Private Shared Sub LogException(message As String, ex As Exception)
            Trace.WriteLine($"{message}: {ex.Message}")
            Trace.WriteLine($"Stack Trace: {ex.StackTrace}")
        End Sub
    End Class
End Namespace
