Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' Provides a method to terminate processes by closing most open handles using handle duplication.
    ''' </summary>
    Friend Class DuplicateHandle

        ''' <summary>
        ''' Represents the maximum handle value used in the <see cref="DuplicateOptions.DuplicateCloseSource"/> operation.
        ''' </summary>
        ''' <remarks>
        ''' This constant is typically utilized when looping through process handles and duplicating them, followed by closing the original handle.
        ''' The value &HFFFF (65535 in decimal) is often used to indicate the maximum possible handle value in certain Windows API scenarios.
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
            Using handle As SafeProcessHandle = OpenProcessHandle(processId, userPrompter)
                If handle Is Nothing Then
                    Return False
                End If
                If Not TerminateProcessByClosingHandles(handle, userPrompter) Then
                    OutputUnableToCloseHandleMessage(userPrompter)
                End If
            End Using
            If Not IfAllElseFailsFinalProcessIsAliveCheck(processId, userPrompter) Then
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Validates the process handle and prompts the user if invalid.
        ''' </summary>
        ''' <param name="processHandle">The handle to validate.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the handle is valid; otherwise, false.</returns>
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
            Dim isSuccess = True
            For i = 0 To MaxHandleValue Step 4
                If Not CloseHandle(handle, CType(i, IntPtr), userPrompter) Then
                    isSuccess = False
                Else
                    userPrompter.Prompt($"Closed handle 0x{i:X4}")
                End If
            Next
            Return isSuccess
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
        ''' It is important to note that this method may be redundant in certain situations, as it is known that it 
        ''' will often fail due to expected conditions (e.g., access violations). However, it is included for 
        ''' completeness of the code.
        ''' 
        ''' For more details on process termination and exit codes, refer to the following:
        ''' <see href="https://learn.microsoft.com/en-us/windows/desktop/api/processthreads/nf-processthreads-getexitcodeprocess">GetExitCodeProcess documentation</see>.
        ''' </remarks>
        Private Shared Function WaitForProcessTermination(handle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Dim timeout = UnsafeNativeMethods.Infinite
            Dim waitResult As Integer = UnsafeNativeMethods.NtWaitForSingleObject(handle.DangerousGetHandle(), False, timeout)
            If waitResult = NativeMethods.WaitObject0 Then
                Dim exitCode As UInteger
                If NativeMethods.GetExitCodeProcess(handle.DangerousGetHandle(), exitCode) AndAlso exitCode <> NativeMethods.StillActive Then
                    userPrompter.Prompt("Process has exited successfully.")
                    Return True
                End If
            End If
            userPrompter.Prompt("Process is still running.")
            Return False
        End Function

        ''' <summary>
        ''' Closes a handle by duplicating it with the <see cref="DuplicateOptions.DuplicateCloseSource"/> option.
        ''' </summary>
        ''' <param name="handle">The handle of the process containing the source handle.</param>
        ''' <param name="sourceHandle">The handle to attempt to close.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns><c>True</c> if the handle was closed successfully; otherwise, <c>False</c>.</returns>
        Friend Shared Function CloseHandle(handle As SafeProcessHandle, sourceHandle As IntPtr, userPrompter As IUserPrompter) As Boolean
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
        Friend Shared Function IfAllElseFailsFinalProcessIsAliveCheck(processId As UInteger, userPrompter As IUserPrompter) As Boolean
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
