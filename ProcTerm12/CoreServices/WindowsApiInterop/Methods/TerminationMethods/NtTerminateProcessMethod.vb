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
        ''' <param name="userPrompter">
        ''' An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.
        ''' </param>
        ''' <returns>
        ''' A boolean indicating whether the termination was successful.
        ''' </returns>
        ''' <remarks>
        ''' The <see cref="UnsafeNativeMethods.NtTerminateProcess"/> function provides additional functionality and flexibility compared to its counterpart, <see cref="NativeMethods.TerminateProcess"/>.
        ''' </remarks>
        ''' <summary>
        ''' Terminates a process using the NtTerminateProcess method.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to terminate.</param>
        ''' <param name="exitStatus">The exit code to be used by the process and threads terminated as a result of this call.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the process was terminated successfully; otherwise, false.</returns>
        Friend Shared Function Kill(processHandle As SafeProcessHandle, exitStatus As Integer, userPrompter As IUserPrompter) As Boolean
            If Not ValidateProcessHandle(processHandle, userPrompter) Then
                Return False
            End If
            Return TerminateProcess(processHandle, exitStatus, userPrompter)
        End Function

        ''' <summary>
        ''' Validates the provided process handle.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to validate.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the process handle is valid; otherwise, <c>False</c>.</returns>
        Private Shared Function ValidateProcessHandle(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Try
                ProcessHandleValidator.ValidateProcessHandle(processHandle)
                Return True
            Catch ex As ArgumentException
                userPrompter.Prompt("Invalid process handle.")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Terminates the process using NtTerminateProcess.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to terminate.</param>
        ''' <param name="exitStatus">The exit code to be used by the process and threads terminated as a result of this call.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the process was terminated successfully; otherwise, false.</returns>
        Private Shared Function TerminateProcess(processHandle As SafeProcessHandle, exitStatus As Integer, userPrompter As IUserPrompter) As Boolean
            Try
                Dim status = UnsafeNativeMethods.NtTerminateProcess(processHandle, exitStatus)
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
