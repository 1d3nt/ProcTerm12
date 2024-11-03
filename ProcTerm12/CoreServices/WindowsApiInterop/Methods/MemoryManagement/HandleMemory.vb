Namespace CoreServices.WindowsApiInterop.Methods.MemoryManagement

    ''' <summary>
    ''' Provides utility methods for managing handles in P/Invoke operations.
    ''' </summary>
    ''' <remarks>
    ''' This class is marked as <c>NotInheritable</c> to prevent inheritance.
    ''' </remarks>
    Friend NotInheritable Class HandleManager

        ''' <summary>
        ''' Closes the handle if it is not null.
        ''' </summary>
        ''' <param name="handle">The handle to be closed.</param>
        ''' <returns>True if the handle was closed successfully; otherwise, false.</returns>
        ''' <remarks>
        ''' This method checks if the provided <paramref name="handle"/> is not equal to <see cref="NativeMethods.NullHandleValue"/>. 
        ''' If it is not equal, the method proceeds to close the handle by calling the <see cref="NativeMethods.CloseHandle"/> method. 
        ''' If the handle is equal to <see cref="NativeMethods.NullHandleValue"/>, which indicates an invalid or uninitialized handle, 
        ''' the method skips the closing operation.
        ''' </remarks>
        Friend Shared Function CloseHandleIfNotNull(handle As IntPtr) As Boolean
            Try
                If Not Equals(handle, NativeMethods.NullHandleValue) Then
                    Return NativeMethods.CloseHandle(handle)
                End If
            Catch ex As SEHException
                LogException("SEHException occurred while closing handle", ex)
            Catch ex As Exception
                LogException("An unexpected error occurred while closing handle", ex)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Closes the process handle if it is not null.
        ''' </summary>
        ''' <param name="processHandle">The handle to be closed.</param>
        ''' <returns>True if the handle was closed successfully; otherwise, false.</returns>
        ''' <remarks>
        ''' This method checks if the provided <paramref name="processHandle"/> is valid before disposing of it.
        ''' </remarks>
        Friend Shared Function CloseProcessHandleIfNotNull(processHandle As SafeProcessHandle) As Boolean
            Try
                If processHandle IsNot Nothing AndAlso Not processHandle.IsInvalid AndAlso Not processHandle.IsClosed Then
                    processHandle.Dispose()
                    Return True
                End If
            Catch ex As SEHException
                LogException("SEHException occurred while closing process handle", ex)
            Catch ex As Exception
                LogException("An unexpected error occurred while closing process handle", ex)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Closes the handle if it is not null and not invalid.
        ''' </summary>
        ''' <param name="safeHandle">The handle to be closed.</param>
        ''' <returns>True if the handle was closed successfully; otherwise, false.</returns>
        ''' <remarks>
        ''' This method checks if the provided <paramref name="safeHandle"/> is not null and not invalid. 
        ''' If it is not null and not invalid, the method proceeds to close the handle by calling the <see cref="SafeHandle.Dispose"/> method. 
        ''' If the handle is null or invalid, the method skips the closing operation.
        ''' </remarks>
        Friend Shared Function CloseSafeHandleIfNotNull(safeHandle As SafeHandleWrapper) As Boolean
            Try
                If safeHandle IsNot Nothing AndAlso Not safeHandle.IsInvalid Then
                    safeHandle.Dispose()
                    Return True
                End If
            Catch ex As SEHException
                LogException("SEHException occurred while closing safe handle", ex)
            Catch ex As Exception
                LogException("An unexpected error occurred while closing safe handle", ex)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Closes the handle if it is not null.
        ''' </summary>
        ''' <param name="handle">The handle to be closed.</param>
        ''' <returns>True if the handle was closed successfully; otherwise, false.</returns>
        ''' <remarks>
        ''' This method checks if the provided <paramref name="handle"/> is not equal to <see cref="NativeMethods.NullHandleValue"/>. 
        ''' If it is not equal, the method proceeds to close the handle by calling the <see cref="UnsafeNativeMethods.NtClose"/> method. 
        ''' If the handle is equal to <see cref="NativeMethods.NullHandleValue"/>, which indicates an invalid or uninitialized handle, 
        ''' the method skips the closing operation.
        ''' </remarks>
        Friend Shared Function NtCloseIfNotNull(handle As IntPtr) As Boolean
            Try
                If Not Equals(handle, NativeMethods.NullHandleValue) Then
                    Dim result As Integer = UnsafeNativeMethods.NtClose(handle)
                    Return result = NtStatus.StatusSuccess
                End If
            Catch ex As SEHException
                LogException("SEHException occurred while closing handle with NtClose", ex)
            Catch ex As Exception
                LogException("An unexpected error occurred while closing handle with NtClose", ex)
            End Try
            Return False
        End Function

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
