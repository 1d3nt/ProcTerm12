Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

''' <summary>
''' The <see cref="VirtualQueryExNoAccess"/> class provides a method for terminating a process by protecting its memory regions 
''' using <see cref="NativeMethods.VirtualQueryEx"/> and <see cref="NativeMethods.VirtualProtectEx"/> to set page access to <see cref="MemoryProtection.PageNoAccess"/>. 
''' This method attempts to crash the process by making its memory inaccessible.
''' </summary>
''' <remarks>
''' This technique involves the following steps:
''' 1. **Validate Process Handle**: Ensures that the provided <see cref="SafeProcessHandle"/> is valid using 
'''    <see cref="ProcessHandleValidator.ValidateProcessHandle"/>.
'''
''' 2. **Retrieve Process ID**: Extracts the process ID from the handle using <see cref="ProcessUtility.GetProcessId"/>.
'''
''' 3. **Create Snapshot**: A snapshot of the process modules is created using 
'''    <see cref="NativeMethods.CreateToolhelp32Snapshot"/> to iterate over memory regions.
'''
''' 4. **Protect Memory Region**: The memory regions of each module are protected using 
'''    <see cref="NativeMethods.VirtualProtectEx"/>, setting them to <see cref="MemoryProtection.PageNoAccess"/>. This step attempts to corrupt
'''    the process memory and trigger a crash.
'''
''' 5. **Wait for Process Termination**: The method waits for the process to exit using 
'''    <see cref="UnsafeNativeMethods.NtWaitForSingleObject"/> and checks the exit code with <see cref="NativeMethods.GetExitCodeProcess"/>.
'''
''' 6. **Final Process Check**: If the process has not exited, a final check is performed to determine if the process is still running 
'''    using <see cref="ProcessUtility.IsProcessRunning"/>. A delay is introduced prior to this check to allow the system to stabilize.
'''
''' 7. **Cleanup**: Any handles opened for snapshots are safely closed using <see cref="HandleManager.CloseHandleIfNotNull"/>.
''' </remarks>
''' <para>API / PInvoke Functions Used:</para>
''' <list>
'''     <item>
'''         <term><see cref="ProcessHandleValidator.ValidateProcessHandle"/></term>
'''         <description>Validates the process handle to ensure it is valid and accessible.</description>
'''     </item>
'''     <item>
'''         <term><see cref="ProcessUtility.GetProcessId"/></term>
'''         <description>Retrieves the process ID from a safe handle.</description>
'''     </item>
'''     <item>
'''         <term><see cref="NativeMethods.CreateToolhelp32Snapshot"/></term>
'''         <description>Creates a snapshot of all modules and threads in the target process.</description>
'''     </item>
'''     <item>
'''         <term><see cref="NativeMethods.Module32First"/></term>
'''         <description>Retrieves information about the first module in the snapshot for memory protection.</description>
'''     </item>
'''     <item>
'''         <term><see cref="NativeMethods.VirtualProtectEx"/></term>
'''         <description>Changes the memory protection of the module region to <see cref="MemoryProtection.PageNoAccess"/>, effectively blocking access.</description>
'''     </item>
'''     <item>
'''         <term><see cref="UnsafeNativeMethods.NtWaitForSingleObject"/></term>
'''         <description>Waits for the process to enter a signaled state (termination).</description>
'''     </item>
'''     <item>
'''         <term><see cref="NativeMethods.GetExitCodeProcess"/></term>
'''         <description>Retrieves the exit code of the process to determine if it has terminated.</description>
'''     </item>
'''     <item>
'''         <term><see cref="ProcessUtility.IsProcessRunning"/></term>
'''         <description>Performs a final check to see if the process is still running.</description>
'''     </item>
'''     <item>
'''         <term><see cref="HandleManager.CloseHandleIfNotNull"/></term>
'''         <description>Safely closes any open handles created during snapshotting.</description>
'''     </item>
''' </list>
''' <para>Possible Error Codes / Conditions:</para>
''' <list>
'''     <item>
'''         <term>Win32Error</term>
'''         <description>Occurs if the snapshot or module retrieval fails.</description>
'''     </item>
'''     <item>
'''         <term>NullReferenceException</term>
'''         <description>Thrown if the handle returned by <see cref="NativeMethods.CreateToolhelp32Snapshot"/> is null or invalid.</description>
'''     </item>
'''     <item>
'''         <term>Exception</term>
'''         <description>Thrown if memory protection fails using <see cref="NativeMethods.VirtualProtectEx"/>.</description>
'''     </item>
''' </list>
''' <para>Notes:</para>
''' <list type="bullet">
'''     <item>
'''         This technique works on modern Windows versions, including Windows 11, but its reliability may vary depending on process protections and access rights.
'''     </item>
'''     <item>
'''         The method is included for completeness and to follow the historical article reference.
'''     </item>
''' </list>
Friend Class VirtualQueryExNoAccess

        ''' <summary>
        ''' Terminates a process using the VirtualQueryExNoAccess method.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to terminate.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the process was terminated successfully; otherwise, false.</returns>
        Friend Shared Function Kill(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            ProcessHandleValidator.ValidateProcessHandle(processHandle)
            Dim processId As UInteger = ProcessUtility.GetProcessId(processHandle, userPrompter)
            If processId = 0 Then
                Return False
            End If
            Dim handle As IntPtr = CreateSnapshot(processId)
            If Equals(handle, NativeMethods.NullHandleValue) Then
                Return False
            End If
            Dim isSuccess As Boolean
            Try
                If ProtectMemoryRegion(processHandle, handle) Then
                    isSuccess = WaitForProcessTermination(handle, userPrompter)
                End If
            Finally
                CloseHandle(handle)
            End Try
            If Not isSuccess AndAlso Not IfAllElseFailsFinalProcessIsAliveCheck(processId, userPrompter) Then
                isSuccess = True
            End If
            Return isSuccess
        End Function

        ''' <summary>
        ''' Creates a snapshot of the process.
        ''' </summary>
        ''' <param name="processId">The ID of the process.</param>
        ''' <returns>The handle to the snapshot.</returns>
        Private Shared Function CreateSnapshot(processId As UInteger) As IntPtr
            Dim handle As IntPtr = NativeMethods.CreateToolhelp32Snapshot(NativeMethods.Th32CsSnapAll, processId)
            If Equals(handle, NativeMethods.NullHandleValue) OrElse Equals(handle, NativeMethods.InvalidHandleValue) Then
                Dim lastError = Win32Error.GetLastPInvokeError()
                Throw New NullReferenceException($"The 'handle' object was null or invalid. Error: {lastError}")
            End If
            Return handle
        End Function

        ''' <summary>
        ''' Protects the memory region of the process.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process.</param>
        ''' <param name="handle">The handle of the snapshot.</param>
        ''' <returns>True if the memory region was protected successfully; otherwise, false.</returns>
        Private Shared Function ProtectMemoryRegion(processHandle As SafeProcessHandle, handle As IntPtr) As Boolean
            Dim moduleEntry As New ModuleEntry32 With {.dwSize = Marshal.SizeOf(GetType(ModuleEntry32))}
            If Not NativeMethods.Module32First(handle, moduleEntry) Then
                Dim lastError = Win32Error.GetLastPInvokeError()
                Throw New Exception($"Failed to retrieve the first module. Error: {lastError}")
            End If
            Dim dwBaseAdr As IntPtr = moduleEntry.hModule
            Dim dwBaseSize As UInteger = moduleEntry.modBaseSize
            Dim dwOldProtect As MemoryProtection
            Dim bProtectRet = NativeMethods.VirtualProtectEx(processHandle.DangerousGetHandle(), dwBaseAdr, dwBaseSize, MemoryProtection.PageNoAccess, dwOldProtect)
            If Not bProtectRet Then
                Dim lastError = Win32Error.GetLastPInvokeError()
                Throw New Exception($"Failed to change the memory region. Error: {lastError}")
            End If
            Return True
        End Function

        ''' <summary>
        ''' Closes the handle if it is not null.
        ''' </summary>
        ''' <param name="handle">The handle to close.</param>
        Private Shared Sub CloseHandle(handle As IntPtr)
            HandleManager.CloseHandleIfNotNull(handle)
        End Sub

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
        Private Shared Function WaitForProcessTermination(handle As IntPtr, userPrompter As IUserPrompter) As Boolean
            Dim timeout = UnsafeNativeMethods.Infinite
            Dim waitResult As Integer = UnsafeNativeMethods.NtWaitForSingleObject(handle, False, timeout)
            If waitResult = NativeMethods.WaitObject0 Then
                Dim exitCode As UInteger
                If NativeMethods.GetExitCodeProcess(handle, exitCode) AndAlso exitCode <> NativeMethods.StillActive Then
                    userPrompter.Prompt("Process has exited successfully.")
                    Return True
                End If
            End If
            userPrompter.Prompt("Process is still running.")
            Return False
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
