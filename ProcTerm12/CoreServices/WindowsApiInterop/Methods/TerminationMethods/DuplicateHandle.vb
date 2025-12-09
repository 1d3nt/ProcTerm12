Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' Provides a method to terminate processes by closing most open handles using handle duplication.
    ''' This method utilizes low-level Windows API calls to forcibly terminate a process by duplicating its handles
    ''' and closing the original ones, effectively disabling resources the process holds, leading to its termination.
    ''' </summary>
    ''' <remarks>
    ''' This method is experimental and is designed to explore unconventional ways of terminating processes by forcibly
    ''' invalidating their open handles. It uses low-level Windows API calls to duplicate and close handles, which can
    ''' lead to process termination by depriving the process of access to its resources. This approach is not recommended
    ''' for production use due to its potential risks, including resource leaks, system instability, or undefined behavior.
    ''' 
    ''' <para>
    ''' The method relies on the <see cref="UnsafeNativeMethods.NtDuplicateObject"/> API to duplicate handles and 
    ''' <see cref="UnsafeNativeMethods.NtClose"/> to close them. By iterating through all possible handle values up to 
    ''' <see cref="DuplicateHandle.MaxHandleValue"/>, it attempts to close all handles associated with the target process.
    ''' </para>
    ''' 
    ''' <para>
    ''' The process termination is confirmed using <see cref="NativeMethods.WaitForSingleObject"/>, which waits 
    ''' for the process to exit. However, due to the forced crashing nature of this method, <see cref="NativeMethods.WaitForSingleObject"/> 
    ''' may return unexpected exit codes, as the process may terminate in an abnormal state.
    ''' </para>
    ''' 
    ''' <para>API Functions Used:</para>
    ''' <list>
    '''     <item>
    '''         <term><see cref="UnsafeNativeMethods.NtOpenProcess"/></term>
    '''         <description>
    '''         Opens a process with the specified access rights (e.g., <c>PROCESS_DUP_HANDLE</c>). This is the first step 
    '''         in acquiring access to the process's handles.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term><see cref="UnsafeNativeMethods.NtDuplicateObject"/></term>
    '''         <description>
    '''         Duplicates a process handle to allow us to control and close the handle in the context of another process, 
    '''         forcing the termination of resources associated with that handle.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term><see cref="UnsafeNativeMethods.NtClose"/></term>
    '''         <description>
    '''         Closes the duplicated process handle, which effectively disables the process’s resources, leading to termination.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term><see cref="NativeMethods.WaitForSingleObject"/></term>
    '''         <description>
    '''         Waits for a specified process to terminate. This is used to ensure that the termination process has 
    '''         completed successfully before proceeding. However, in cases where the program is being forcibly crashed,
    '''         this function may fail due to abnormal termination states.
    '''         </description>
    '''         <para>Potential Failures:</para>
    '''         <list>
    '''             <item>
    '''                 <term><c>STATUS_INVALID_HANDLE</c></term>
    '''                 <description>
    '''                 This error occurs if the handle to the process is invalid. In a forced crash scenario, the process
    '''                 may have terminated or become inaccessible before <see cref="NativeMethods.WaitForSingleObject"/> can query it. If the
    '''                 process is unexpectedly killed or crashes during the wait, this error is triggered.
    '''                 </description>
    '''             </item>
    '''             <item>
    '''                 <term><c>STATUS_OBJECT_TYPE_MISMATCH</c></term>
    '''                 <description>
    '''                 This error is thrown if the object being waited on is not a valid process object. In cases where the
    '''                 process is already crashed and cleaned up by the system, the handle may no longer represent a valid
    '''                 process object, causing this error.
    '''                 </description>
    '''             </item>
    '''             <item>
    '''                 <term><c>STATUS_ACCESS_DENIED</c></term>
    '''                 <description>
    '''                 If the process is in a state where it cannot be queried (perhaps due to system-level security policies or
    '''                 other factors), this error will occur. The process might not have proper permissions for <see cref="NativeMethods.WaitForSingleObject"/>.
    '''                 </description>
    '''             </item>
    '''         </list>
    '''     </item>
    ''' </list>
    ''' 
    ''' <para>
    ''' Despite the availability of <see cref="UnsafeNativeMethods.NtWaitForSingleObject"/>, this method uses 
    ''' <see cref="NativeMethods.WaitForSingleObject"/> for simplicity and compatibility. However, <see cref="UnsafeNativeMethods.NtWaitForSingleObject"/> may be a more 
    ''' appropriate choice for low-level process management, as it provides finer control and better integration with 
    ''' other native APIs. The decision to avoid <see cref="UnsafeNativeMethods.NtWaitForSingleObject"/> in this implementation is primarily due to 
    ''' its complexity and the experimental nature of the method.
    ''' </para>
    ''' </remarks>
    ''' <summary>
    ''' Possible return values and errors:
    ''' </summary>
    ''' <returns>
    ''' <para>Possible Return Values from WaitForSingleObject:</para>
    ''' <list>
    '''     <item><description><c>STATUS_SUCCESS</c> - The process has terminated successfully and the wait is completed.</description></item>
    '''     <item><description><c>STATUS_TIMEOUT</c> - The process did not terminate in the expected time, and the wait timed out.</description></item>
    '''     <item><description><c>STATUS_INVALID_HANDLE</c> - The handle was invalid, possibly due to abnormal process termination (e.g., a crash).</description></item>
    '''     <item><description><c>STATUS_OBJECT_TYPE_MISMATCH</c> - The object is not a valid process, which may occur after a crash or cleanup.</description></item>
    ''' </list>
    ''' </returns>
    Friend MustInherit Class DuplicateHandle

        ''' <summary>
        ''' Represents the maximum handle value used in the <see cref="DuplicateOptions.DuplicateCloseSource"/> operation.
        ''' </summary>
        ''' <remarks>
        ''' This constant is critical when iterating through all possible handles associated with a process.
        ''' It defines the upper boundary for handle values, ensuring that the iteration does not exceed the maximum possible handle value.
        ''' 
        ''' <para>
        ''' In Windows, handles are references to system resources such as files, processes, and threads. 
        ''' These handles are represented as integer values, and their range is determined by the operating system.
        ''' The value <c>&HFFFF</c> (65535 in decimal) is often used as the maximum possible handle value in certain Windows API scenarios.
        ''' </para>
        ''' 
        ''' <para>
        ''' During the <see cref="DuplicateHandle"/> operation, this constant is utilized to loop through all potential handle values 
        ''' within a process. The goal is to duplicate each handle and subsequently close the original handle using the 
        ''' <see cref="DuplicateOptions.DuplicateCloseSource"/> flag. This approach is commonly employed to forcefully terminate 
        ''' a process by invalidating its open handles.
        ''' </para>
        ''' 
        ''' <para>
        ''' The importance of this constant lies in its role in ensuring the completeness of the handle iteration process. 
        ''' Without a defined upper limit, the iteration could either miss valid handles or result in unnecessary overhead 
        ''' by attempting to access invalid handle values.
        ''' </para>
        ''' 
        ''' <example>
        ''' Example usage:
        ''' <code>
        ''' For handleValue As Integer = 0 To MaxHandleValue
        '''     ' Attempt to duplicate and close the handle
        '''     DuplicateHandle(sourceProcessHandle, handleValue, targetProcessHandle, ...)
        ''' Next
        ''' </code>
        ''' </example>
        ''' </remarks>
        Private Const MaxHandleValue As Integer = &HFFFF

        ''' <summary>
        ''' Terminates a process by closing all handles associated with it through handle duplication.
        ''' </summary>
        ''' <param name="processHandle">
        ''' A <see cref="SafeProcessHandle"/> representing the handle of the process to be terminated.
        ''' </param>
        ''' <param name="userPrompter">
        ''' An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.
        ''' </param>
        ''' <returns>
        ''' <c>True</c> if the process was successfully terminated; otherwise, <c>False</c>.
        ''' </returns>
        ''' <remarks>
        ''' This method validates the provided process handle and retrieves the corresponding process ID. It then attempts 
        ''' to open the process with the appropriate access rights. The method iterates through the handles of the process 
        ''' and attempts to close each one using <see cref="NativeMethods.CloseHandle"/>. After closing the handles, it waits for the 
        ''' process to enter a signaled state using <see cref="UnsafeNativeMethods.NtWaitForSingleObject"/> with an 
        ''' infinite timeout. Upon completion, it checks the exit code of the process to determine if it is still active 
        ''' or has exited successfully.
        ''' 
        ''' If an access violation occurs due to the application crashing, the method will always call the 
        ''' <see cref="IfAllElseFailsFinalProcessIsAliveCheck"/> method to ensure the process state is correctly assessed.
        ''' </remarks>
        Friend Shared Function Kill(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            If Not ValidateProcessHandle(processHandle, userPrompter) Then
                Return False
            End If
            Dim processId As UInteger
            If Not TryGetProcessId(processHandle, processId, userPrompter) Then
                Return False
            End If
            If Not HandleProcessTermination(processId, userPrompter) Then
                Return False
            End If
            If Not IfAllElseFailsFinalProcessIsAliveCheck(processId, userPrompter) Then
                Return False
            End If
            Return True
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
        ''' Retrieves the process ID from the process handle.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the process ID was retrieved successfully; otherwise, <c>False</c>.</returns>
        Private Shared Function TryGetProcessId(processHandle As SafeProcessHandle, ByRef processId As UInteger, userPrompter As IUserPrompter) As Boolean
            processId = ProcessUtility.GetProcessId(processHandle, userPrompter)
            If processId = 0 Then
                userPrompter.Prompt("Failed to retrieve process ID.")
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Handles the termination of a process by opening a handle to the process and attempting to close all associated handles.
        ''' </summary>
        ''' <param name="processId">
        ''' The ID of the process to be terminated. This is used to open a handle to the process for further operations.
        ''' </param>
        ''' <param name="userPrompter">
        ''' An instance of <see cref="IUserPrompter"/> used to display messages to the user during the operation.
        ''' This allows for user interaction and feedback in case of errors or important events.
        ''' </param>
        ''' <returns>
        ''' <c>True</c> if the process was successfully handled (i.e., the handle was opened and the termination process was attempted);
        ''' otherwise, <c>False</c> if the handle could not be opened or other critical errors occurred.
        ''' </returns>
        ''' <remarks>
        ''' This method performs the following steps:
        ''' <list type="number">
        '''     <item>
        '''         <description>
        '''         Opens a handle to the process using the provided <paramref name="processId"/>. 
        '''         This is achieved through the <see cref="OpenProcessHandle"/> method, which uses the Windows API to obtain a handle
        '''         with the necessary access rights for further operations.
        '''         </description>
        '''     </item>
        '''     <item>
        '''         <description>
        '''         If the handle is successfully opened, the method attempts to terminate the process by closing all associated handles.
        '''         This is done using the <see cref="TerminateProcessByClosingHandles"/> method, which iterates through the process's handles
        '''         and closes them to force the process to terminate.
        '''         </description>
        '''     </item>
        '''     <item>
        '''         <description>
        '''         If the termination process fails (e.g., due to insufficient permissions or protected handles), 
        '''         the method calls <see cref="OutputUnableToCloseHandleMessage"/> to notify the user of the failure.
        '''         </description>
        '''     </item>
        ''' </list>
        ''' <para>
        ''' This method ensures that all resources are properly disposed of by using a <c>Using</c> block for the process handle.
        ''' The handle is automatically released when the block exits, even if an exception occurs.
        ''' </para>
        ''' <para>
        ''' Note that this method does not directly terminate the process but instead attempts to force termination
        ''' by closing its handles. This approach may not work for all processes, especially those with protected or critical handles.
        ''' </para>
        ''' </remarks>
        ''' <exception cref="ArgumentException">
        ''' Thrown if the provided <paramref name="processId"/> is invalid or if the handle cannot be opened.
        ''' </exception>
        Private Shared Function HandleProcessTermination(processId As UInteger, userPrompter As IUserPrompter) As Boolean
            Using handle As SafeProcessHandle = OpenProcessHandle(processId, userPrompter)
                If handle Is Nothing Then
                    Return False
                End If
                If Not TerminateProcessByClosingHandles(handle, userPrompter) Then
                    OutputUnableToCloseHandleMessage(userPrompter)
                End If
            End Using
            Return True
        End Function

        ''' <summary>
        ''' Opens a handle to the process with the specified process ID.
        ''' </summary>
        ''' <param name="processId">The ID of the process.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>The safe handle to the process.</returns>
        Private Shared Function OpenProcessHandle(processId As UInteger, userPrompter As IUserPrompter) As SafeProcessHandle
            Dim handle As IntPtr = NativeMethods.NullHandleValue
            Dim clientId As New ClientId(New IntPtr(processId), NativeMethods.NullHandleValue)
            Dim objectAttributes As New ObjectAttributes()
            Dim status As Integer = UnsafeNativeMethods.NtOpenProcess(handle, ProcessAccessRights.DuplicateHandle, objectAttributes, clientId)
            If status <> 0 OrElse Equals(handle, IntPtr.Zero) Then
                userPrompter.Prompt("Failed to open process handle.")
                Return Nothing
            End If
            Return New SafeProcessHandle(handle, True)
        End Function

        ''' <summary>
        ''' Terminates the process by closing all handles and waiting for termination.
        ''' </summary>
        ''' <param name="handle">The handle to the process.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the process was successfully terminated; otherwise, False.</returns>
        Private Shared Function TerminateProcessByClosingHandles(handle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Dim isSuccess As Boolean = CloseAllHandles(handle, userPrompter)
            If Not WaitForProcessTermination(handle, userPrompter) Then
                userPrompter.Prompt("Process termination wait failed.")
            End If
            Return isSuccess
        End Function

        ''' <summary>
        ''' Outputs a message indicating that the handle could not be closed, including the last error code.
        ''' </summary>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        Private Shared Sub OutputUnableToCloseHandleMessage(userPrompter As IUserPrompter)
            Dim lastError = Win32Error.GetLastPInvokeError()
            userPrompter.Prompt($"Unable to close handle. Last Error Code: {lastError}")
        End Sub

        ''' <summary>
        ''' Closes all handles associated with the process.
        ''' </summary>
        ''' <param name="handle">The handle of the process.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns><c>True</c> if all handles were closed successfully; otherwise, <c>False</c>.</returns>
        Private Shared Function CloseAllHandles(handle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Dim allClosed = True
            For Each handleValue In Enumerable.Range(0, (MaxHandleValue \ 4) + 1).Select(Function(i) CType(i * 4, IntPtr))
                If Not CloseHandle(handle, handleValue, userPrompter) Then
                    allClosed = False
                End If
                userPrompter.Prompt($"Closed handle 0x{handleValue:X4}")
            Next
            Return allClosed
        End Function

        ''' <summary>
        ''' Waits for the specified process to enter a signaled state and checks its exit code.
        ''' </summary>
        ''' <param name="handle">
        ''' The handle of the process to wait on. This parameter must be a valid handle obtained from functions like 
        ''' <c>CreateProcess</c> or <c>OpenProcess</c>, and it should have the appropriate access rights for waiting.
        ''' </param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>
        ''' <c>True</c> if the process has exited successfully; otherwise, <c>False</c>. If the process is still running at 
        ''' the end of the wait period, this method will return <c>False</c>.
        ''' </returns>
        ''' <remarks>
        ''' This method implements a waiting mechanism that uses the <see cref="UnsafeNativeMethods.NtWaitForSingleObject"/> 
        ''' function to check the state of the process handle. If the process enters a signaled state, it retrieves the 
        ''' exit code using <see cref="NativeMethods.GetExitCodeProcess"/>. The process is considered to have exited 
        ''' successfully if its exit code is not <see cref="NativeMethods.StillActive"/>. The method will wait indefinitely 
        ''' for the process to terminate.
        ''' 
        ''' <para>
        ''' Although the project includes two dedicated classes, <see cref="ProcessWaitHandler"/> and 
        ''' <see cref="ProcessExitCodeRetriever"/>, this method is included in this class because it specifically uses 
        ''' <see cref="UnsafeNativeMethods.NtWaitForSingleObject"/> for waiting and <see cref="NativeMethods.GetExitCodeProcess"/> 
        ''' for retrieving the process exit code. These low-level APIs provide finer control over the waiting mechanism and 
        ''' exit code retrieval, making this method more suitable for scenarios where precise control is required.
        ''' </para>
        ''' 
        ''' <para>
        ''' It is important to note that this method may be redundant in certain situations, as it is known that it 
        ''' will often fail due to expected conditions (e.g., access violations). However, it is included for 
        ''' completeness of the code and to demonstrate the use of <see cref="UnsafeNativeMethods.NtWaitForSingleObject"/> 
        ''' and <see cref="NativeMethods.GetExitCodeProcess"/> in a single implementation.
        ''' </para>
        ''' 
        ''' <para>
        ''' For more details on process termination and exit codes, refer to the following:
        ''' <see href="https://learn.microsoft.com/en-us/windows/desktop/api/processthreads/nf-processthreads-getexitcodeprocess">GetExitCodeProcess documentation</see>.
        ''' </para>
        ''' </remarks>
        Private Shared Function WaitForProcessTermination(handle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Dim timeout = UnsafeNativeMethods.Infinite
            Dim waitResult As Integer = UnsafeNativeMethods.NtWaitForSingleObject(handle.DangerousGetHandle(), False, timeout)
            Select Case waitResult
                Case NativeMethods.WaitObject0
                    Dim exitCode As UInteger
                    If NativeMethods.GetExitCodeProcess(handle.DangerousGetHandle(), exitCode) Then
                        If exitCode <> NativeMethods.StillActive Then
                            userPrompter.Prompt($"Process has exited successfully with exit code: {exitCode}.")
                            Return True
                        Else
                            userPrompter.Prompt("Process is still active despite wait completion.")
                            Return False
                        End If
                    Else
                        userPrompter.Prompt("Failed to retrieve process exit code.")
                        Return False
                    End If
                Case NativeMethods.WaitTimeout
                    userPrompter.Prompt("Wait operation timed out. Process may still be running.")
                    Return False
                Case Else
                    userPrompter.Prompt($"Unexpected wait result: {waitResult}. Process state is unknown.")
                    Return False
            End Select
        End Function


        ''' <summary>
        ''' Closes a handle by duplicating it with the <see cref="DuplicateOptions.DuplicateCloseSource"/> option.
        ''' </summary>
        ''' <param name="handle">The handle of the process containing the source handle.</param>
        ''' <param name="sourceHandle">The handle to attempt to close.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns><c>True</c> if the handle was closed successfully; otherwise, <c>False</c>.</returns>
        Private Shared Function CloseHandle(handle As SafeProcessHandle, sourceHandle As IntPtr, userPrompter As IUserPrompter) As Boolean
            Dim targetHandle = NativeMethods.NullHandleValue
            Dim success = DuplicateHandleWithSameAccess(handle, sourceHandle, targetHandle)
            If success = NtStatus.StatusSuccess AndAlso Not Equals(targetHandle, NativeMethods.NullHandleValue) Then
                Try
                    success = DuplicateAndCloseSourceHandle(handle, sourceHandle, targetHandle)
                Finally
                    CloseTargetHandle(targetHandle, sourceHandle, userPrompter)
                End Try
            End If
            Return success = NtStatus.StatusSuccess
        End Function

        ''' <summary>
        ''' Closes the target handle and prompts the user if the operation fails.
        ''' </summary>
        ''' <param name="targetHandle">The handle to be closed.</param>
        ''' <param name="sourceHandle">The original source handle.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        Private Shared Sub CloseTargetHandle(targetHandle As IntPtr, sourceHandle As IntPtr, userPrompter As IUserPrompter)
            If Not Equals(targetHandle, NativeMethods.NullHandleValue) Then
                Dim closeResult = UnsafeNativeMethods.NtClose(targetHandle)
                If closeResult <> NtStatus.StatusSuccess Then
                    userPrompter.Prompt($"Failed to close handle 0x{sourceHandle.ToInt32():X4}")
                End If
            End If
        End Sub

        ''' <summary>
        ''' Duplicates a handle with the same access rights.
        ''' </summary>
        ''' <param name="handle">The handle of the process containing the source handle.</param>
        ''' <param name="sourceHandle">The handle to duplicate.</param>
        ''' <param name="targetHandle">The duplicated handle.</param>
        ''' <returns>The status of the duplication operation.</returns>
        Private Shared Function DuplicateHandleWithSameAccess(handle As SafeProcessHandle, sourceHandle As IntPtr, ByRef targetHandle As IntPtr) As Integer
            Return UnsafeNativeMethods.NtDuplicateObject(handle, sourceHandle, handle, targetHandle,
                                                         ProcessAccessRights.All,
                                                         ObjectAttributeFlags.DefaultAttributes,
                                                         DuplicateOptions.DuplicateSameAccess)
        End Function

        '' <summary>
        ''' Duplicates a handle and closes the source handle.
        ''' </summary>
        ''' <param name="handle">The handle of the process containing the source handle.</param>
        ''' <param name="sourceHandle">The handle to duplicate and close.</param>
        ''' <param name="targetHandle">The duplicated handle.</param>
        ''' <returns>The status of the duplication and close operation.</returns>
        Private Shared Function DuplicateAndCloseSourceHandle(handle As SafeProcessHandle, sourceHandle As IntPtr, ByRef targetHandle As IntPtr) As Integer
            Using invalidHandle As New SafeProcessHandle(NativeMethods.NullHandleValue, False)
                Return UnsafeNativeMethods.NtDuplicateObject(handle, sourceHandle, invalidHandle, targetHandle,
                                                             ProcessAccessRights.DefaultAccess,
                                                             ObjectAttributeFlags.DefaultAttributes,
                                                             DuplicateOptions.DuplicateCloseSource)
            End Using
        End Function

        ''' <summary>
        ''' Performs a final check to determine if a process is still alive.
        ''' </summary>
        ''' <param name="processId">The ID of the process to check.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used to notify the user about the delay.</param>
        ''' <returns>True if the process is still alive; otherwise, false.</returns>
        ''' <remarks>
        ''' This method introduces a delay before checking if the process is still alive. 
        ''' It will prompt the user about the delay duration, allowing any necessary preparation 
        ''' before the final process check. The actual implementation of the process check logic 
        ''' should be included to determine the process status.
        ''' </remarks>
        Private Shared Function IfAllElseFailsFinalProcessIsAliveCheck(processId As UInteger, userPrompter As IUserPrompter) As Boolean
            DelayBeforeCheckingProcessAlive(userPrompter).GetAwaiter().GetResult()
            Dim isAlive As Boolean = ProcessUtility.IsProcessRunning(processId)
            Return isAlive
        End Function

        ''' <summary>
        ''' Introduces a delay before checking if the process is alive.
        ''' </summary>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used to notify the user.</param>
        ''' <returns>
        ''' A task that represents the asynchronous operation.
        ''' </returns>
        ''' <remarks>
        ''' The <see cref="DelayBeforeCheckingProcessAlive"/> method prompts the user about the delay duration and then simulates a delay
        ''' before proceeding to check if the specified process is still alive. This delay allows for any necessary preparation 
        ''' and ensures that the process state is stable before the check.
        ''' </remarks>
        Private Shared Async Function DelayBeforeCheckingProcessAlive(userPrompter As IUserPrompter) As Task
            Const delayMilliseconds = 30000
            PromptUserAboutDelay(delayMilliseconds, userPrompter)
            Await AsynchronousProcessor.SimulateDelayedResponse(delayMilliseconds)
        End Function

        ''' <summary>
        ''' Prompts the user about the delay duration.
        ''' </summary>
        ''' <param name="delayMilliseconds">The delay duration in milliseconds.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used to notify the user.</param>
        ''' <remarks>
        ''' The <see cref="PromptUserAboutDelay"/> method uses the <see cref="IUserPrompter"/> service to notify the user about
        ''' the delay before checking if the specified process is alive. This ensures that the user is informed of the wait 
        ''' period before the check.
        ''' </remarks>
        Private Shared Sub PromptUserAboutDelay(delayMilliseconds As Integer, userPrompter As IUserPrompter)
            userPrompter.Prompt($"The process will wait for {delayMilliseconds / 1000} seconds before checking if it is alive.")
        End Sub
    End Class
End Namespace
