Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' Represents the method to terminate a process using NtTerminateProcess.
    ''' </summary>
    Friend Class NtTerminateProcessMethod

        ''' <summary>
        ''' Terminates the specified process and all of its threads.
        ''' </summary>
        ''' <param name="processHandle">
        ''' A handle to the process to be terminated. The handle must have the <see cref="ProcessAccessRights.Terminate"/> access right.
        ''' For more information, see Process Security and Access Rights.
        ''' https://msdn.microsoft.com/en-us/library/windows/desktop/ms684880(v=vs.85).aspx
        ''' </param>
        ''' <param name="exitStatus">
        ''' The exit code to be used by the process and threads terminated as a result of this call. Use the
        ''' GetExitCodeProcess function to retrieve a process's exit value. Use the GetExitCodeThread function
        ''' to retrieve a thread's exit value.
        ''' </param>
        ''' <returns>
        ''' A boolean indicating whether the termination was successful.
        ''' </returns>
        ''' <remarks>
        ''' The <see cref="UnsafeNativeMethods.NtTerminateProcess"/> function provides additional functionality and flexibility compared to its counterpart, <see cref="NativeMethods.TerminateProcess"/>.
        ''' </remarks>
        Friend Shared Function Kill(processHandle As SafeProcessHandle, exitStatus As Integer) As Boolean
            ProcessHandleValidator.ValidateProcessHandle(processHandle)
            Try
                Dim status As NtStatus = UnsafeNativeMethods.NtTerminateProcess(processHandle.DangerousGetHandle(), exitStatus)
                If status = NtStatus.StatusSuccess Then
                    Return True
                Else
                    Throw New Win32Exception(Marshal.GetLastWin32Error())
                End If
            Catch ex As Exception
                Throw New InvalidOperationException("Failed to terminate the process.", ex)
            End Try
        End Function
    End Class
End Namespace
