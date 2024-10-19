Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' Represents the method to terminate a process using NtTerminateProcess.
    ''' </summary>
    Public Class NtTerminateProcessMethod

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
        ''' An NTSTATUS value indicating the outcome of the termination attempt.
        ''' If the function succeeds, the return value is STATUS_SUCCESS (0x0).
        ''' If the function fails, the return value is an NTSTATUS error code.
        ''' </returns>
        ''' <remarks>
        ''' The <see cref="UnsafeNativeMethods.NtTerminateProcess"/> function provides additional functionality and flexibility compared to its counterpart, <see cref="NativeMethods.TerminateProcess"/>.
        ''' </remarks>
        Friend shared Sub Kill(processHandle As SafeProcessHandle, exitStatus As Integer)
            ProcessHandleValidator.ValidateProcessHandle(processHandle)

            Try
                Dim status As NtStatus = UnsafeNativeMethods.NtTerminateProcess(processHandle.DangerousGetHandle(), exitStatus)
                If status <> NtStatus.StatusSuccess Then
                    Throw New Win32Exception(Marshal.GetLastWin32Error())
                End If
            Finally
                processHandle.Dispose()
            End Try
        End Sub
    End Class
End Namespace
