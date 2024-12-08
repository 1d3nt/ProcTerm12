Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' Provides functionality to terminate a process using the <c>NtTerminateProcess</c> API, a lower-level, undocumented 
    ''' Windows NT Native API function that terminates a process and all of its associated threads.
    ''' </summary>
    ''' <remarks>
    ''' The <c>NtTerminateProcess</c> API is part of the Windows NT Native API set, which operates below the Win32 API layer.
    ''' Unlike the Win32 <c>TerminateProcess</c> function, <c>NtTerminateProcess</c> is intended for use by the 
    ''' operating system itself and, as such, is not officially documented for direct use by applications.
    ''' 
    ''' <para>
    ''' <b>Behavior and Usage</b>:
    ''' <c>NtTerminateProcess</c> allows direct termination of a process by its handle, terminating all threads associated
    ''' with that process and setting an exit code. It bypasses any registered cleanup routines or handlers, such as those 
    ''' in DLL_PROCESS_DETACH notifications, making it a "hard" termination approach. This method is useful in scenarios 
    ''' where process cleanup is unnecessary or when terminating potentially harmful or unresponsive processes, especially 
    ''' when higher-level APIs may be intercepted or insufficient.
    ''' </para>
    ''' 
    ''' <para>
    ''' <b>System Internals</b>:
    ''' Internally, <c>NtTerminateProcess</c> is implemented in <c>ntdll.dll</c> and interacts directly with the kernel to 
    ''' facilitate process termination. The termination process relies on the kernel mode function <c>PsTerminateProcess</c>, 
    ''' which ensures the termination of all threads within the process and deallocates resources. Because <c>NtTerminateProcess</c> 
    ''' is a native API call, its execution is subject to fewer restrictions than standard user-mode APIs, although it still 
    ''' requires appropriate access rights (i.e., <see cref="ProcessAccessRights.Terminate"/>).
    ''' </para>
    ''' 
    ''' <para>
    ''' <b>Considerations and Risks</b>:
    ''' Using <c>NtTerminateProcess</c> requires careful consideration, as it forcibly terminates the target process without 
    ''' cleanup, potentially causing data loss or corruption if the process is handling critical tasks. This method should be 
    ''' restricted to scenarios where immediate termination is imperative. Furthermore, as an undocumented API, its behavior 
    ''' can vary between different Windows versions, and its use can impact system stability or reliability in production 
    ''' environments.
    ''' </para>
    ''' 
    ''' <para>
    ''' <b>Alternative Methods</b>:
    ''' In cases where a less aggressive termination is preferred, consider using <c>TerminateProcess</c>, which 
    ''' integrates with the standard Win32 API and may provide a safer or more predictable outcome. Additionally, 
    ''' <c>CreateRemoteThread</c> with <c>ExitProcess</c> can offer a more controlled termination within user-mode space.
    ''' </para>
    '''
    ''' <para>API Functions Used:</para>
    ''' <list>
    '''     <item>
    '''         <term>NtTerminateProcess</term>
    '''         <description>
    '''         Terminates a process and all of its threads. It is invoked with a handle 
    '''         to the process and an exit code. The process will terminate immediately, 
    '''         and the operating system will perform cleanup operations for that process.
    '''         </description>
    '''     </item>
    ''' </list>
    ''' </remarks>
    Friend Class NtTerminateProcess

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
            If Not TerminateProcess(processHandle, exitStatus) Then
                Return False
            End If
            Return WaitForProcessToExit(processHandle, userPrompter)
        End Function

        ''' <summary>
        ''' Validates the provided process handle.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to validate.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the process handle is valid; otherwise, <c>False</c>.</returns>
        Private Shared Function ValidateProcessHandle(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Return ProcessHandleValidatorUtility.ValidateProcessHandle(processHandle, userPrompter)
        End Function

        ''' <summary>
        ''' Terminates the process using NtTerminateProcess.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to terminate.</param>
        ''' <param name="exitStatus">The exit code to be used by the process and threads terminated as a result of this call.</param>
        ''' <returns>True if the process was terminated successfully; otherwise, false.</returns>
        Private Shared Function TerminateProcess(processHandle As SafeProcessHandle, exitStatus As Integer) As Boolean
            Dim status = UnsafeNativeMethods.NtTerminateProcess(processHandle, exitStatus)
            Return status = NtStatus.StatusSuccess
        End Function

        ''' <summary>
        ''' Waits for the process to exit and handles the result.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to wait for.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the process exited successfully; otherwise, false.</returns>
        Private Shared Function WaitForProcessToExit(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Dim rawProcessHandle As IntPtr = processHandle.DangerousGetHandle()
            Dim processExited As Boolean = ProcessWaitHandler.WaitForProcessExit(rawProcessHandle, userPrompter)
            Return processExited
        End Function
    End Class
End Namespace
