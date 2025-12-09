Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles

Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' The <c>VirtualAllocExMethods</c> class provides functionality for forcefully terminating a target
    ''' process by allocating memory inside the remote process, writing termination instructions into that
    ''' memory, and executing them through a remote thread. This technique uses <c>VirtualAllocEx</c>,
    ''' <c>WriteProcessMemory</c>, and <c>CreateRemoteThread</c> to perform controlled process termination.
    ''' </summary>
    ''' 
    ''' <remarks>
    ''' This termination technique performs the following operational steps:
    ''' 
    ''' <para>
    ''' 1. <b>Validate Process Handle</b>: The method verifies that the supplied process handle is valid and
    ''' usable for memory allocation and remote thread execution. This is performed using
    ''' <see cref="ProcessHandleValidatorUtility.ValidateProcessHandle"/>.
    ''' </para>
    ''' 
    ''' <para>
    ''' 2. <b>Allocate Memory in the Remote Process</b>: A memory region is allocated inside the target process
    ''' using <see cref="NativeMethods.VirtualAllocEx"/> with read/write/execute protection to allow both writing
    ''' and executing the termination routine.
    ''' </para>
    ''' 
    ''' <para>
    ''' 3. <b>Write Termination Routine</b>: The method writes the address of <c>ExitProcess</c>, or other termination
    ''' payload, into the allocated memory using <see cref="NativeMethods.WriteProcessMemory"/>.
    ''' </para>
    ''' 
    ''' <para>
    ''' 4. <b>Start Remote Execution</b>: A thread is created inside the remote process using
    ''' <see cref="NativeMethods.CreateRemoteThread"/>, which begins execution at the address previously written.
    ''' This causes the target process to terminate cleanly or forcibly depending on the injected routine.
    ''' </para>
    ''' 
    ''' <para>
    ''' 5. <b>Wait for Process Exit</b>: The method waits for the target process to exit via
    ''' <see cref="ProcessWaitHandler.WaitForProcessExit"/>, confirming successful termination.
    ''' </para>
    ''' 
    ''' <para>
    ''' 6. <b>Cleanup</b>: Finally, any allocated resources are released by calling
    ''' <see cref="NativeMethods.VirtualFreeEx"/> as required, and handles are closed with
    ''' <see cref="NativeMethods.CloseHandle"/>.
    ''' </para>
    ''' </remarks>
    ''' 
    ''' <para>Native API Functions Used:</para>
    ''' <list>
    '''     <item>
    '''         <term><see cref="NativeMethods.VirtualAllocEx"/></term>
    '''         <description>
    '''         Allocates a block of memory inside the remote process to hold termination code or parameters.
    '''         </description>
    '''     </item>
    ''' 
    '''     <item>
    '''         <term><see cref="NativeMethods.WriteProcessMemory"/></term>
    '''         <description>
    '''         Writes termination code, function pointers, or shellcode into the allocated remote memory block.
    '''         </description>
    '''     </item>
    ''' 
    '''     <item>
    '''         <term><see cref="NativeMethods.CreateRemoteThread"/></term>
    '''         <description>
    '''         Creates a thread inside the target process and begins execution at the specified remote address.
    '''         </description>
    '''     </item>
    ''' 
    '''     <item>
    '''         <term><see cref="NativeMethods.VirtualFreeEx"/></term>
    '''         <description>
    '''         Frees previously allocated remote memory after the process has terminated.
    '''         </description>
    '''     </item>
    ''' 
    '''     <item>
    '''         <term><see cref="NativeMethods.GetModuleHandle"/></term>
    '''         <description>
    '''         Retrieves the handle of a loaded module (typically <c>kernel32.dll</c>) in order to find the
    '''         address of <c>ExitProcess</c>.
    '''         </description>
    '''     </item>
    ''' 
    '''     <item>
    '''         <term><see cref="NativeMethods.GetProcAddress"/></term>
    '''         <description>
    '''         Retrieves the function pointer for <c>ExitProcess</c> or another termination API to write into
    '''         the remote process.
    '''         </description>
    '''     </item>
    ''' 
    '''     <item>
    '''         <term><see cref="NativeMethods.CloseHandle"/></term>
    '''         <description>
    '''         Closes memory, thread, and process handles once execution and cleanup are complete.
    '''         </description>
    '''     </item>
    ''' </list>
    ''' 
    ''' <para>Possible Error Codes:</para>
    ''' <list>
    '''     <item>
    '''         <term>ERROR_INVALID_HANDLE</term>
    '''         <description>
    '''         The supplied process handle is not valid for memory or thread operations.
    '''         </description>
    '''     </item>
    ''' 
    '''     <item>
    '''         <term>ERROR_ACCESS_DENIED</term>
    '''         <description>
    '''         The caller does not have sufficient rights to allocate memory, write memory, or create threads
    '''         inside the target process.
    '''         </description>
    '''     </item>
    ''' 
    '''     <item>
    '''         <term>ERROR_NOT_ENOUGH_MEMORY</term>
    '''         <description>
    '''         The target process cannot allocate the requested memory region.
    '''         </description>
    '''     </item>
    ''' 
    '''     <item>
    '''         <term>ERROR_PARTIAL_COPY</term>
    '''         <description>
    '''         The <see cref="NativeMethods.WriteProcessMemory"/> call failed or wrote only partial data.
    '''         </description>
    '''     </item>
    ''' 
    '''     <item>
    '''         <term>ERROR_INVALID_ADDRESS</term>
    '''         <description>
    '''         The memory address passed to <see cref="NativeMethods.VirtualFreeEx"/> or WriteProcessMemory was
    '''         invalid or unmapped.
    '''         </description>
    '''     </item>
    ''' </list>
    ''' 
    ''' <remarks>
    ''' <b>Architecture Limitation:</b><br/>
    ''' This technique requires the injector and target to share the same processor architecture:
    ''' 
    ''' <para>
    ''' • A 64‑bit application cannot inject remote threads into a 32‑bit process using
    '''   <see cref="NativeMethods.VirtualAllocEx"/> or <see cref="NativeMethods.CreateRemoteThread"/>.
    ''' </para>
    ''' 
    ''' <para>
    ''' • A 32‑bit application cannot inject into a 64‑bit process for the same reason  
    '''   (pointer size, calling conventions, thread context mismatch).
    ''' </para>
    ''' 
    ''' <para>
    ''' Therefore, this technique is only usable against <b>32‑bit processes when your tool is 32‑bit</b>,
    ''' and against <b>64‑bit processes when your tool is 64‑bit</b>.
    ''' On Windows 11, this is typically limited to specific WOW64 applications that are still 32‑bit.
    ''' </para>
    ''' </remarks>
    Friend NotInheritable Class VirtualAllocEx

        ''' <summary>
        ''' Memory allocation flag for reserving address space.
        ''' </summary>
        Private Const MEM_RESERVE As UInteger = &H2000UI

        ''' <summary>
        ''' Memory allocation flag for committing memory.
        ''' </summary>
        Private Const MEM_COMMIT As UInteger = &H1000UI

        ''' <summary>
        ''' Page protection constant for inaccessible memory.
        ''' </summary>
        Private Const PAGE_NOACCESS As UInteger = &H1UI

        ''' <summary>
        ''' Exit code indicating the process is still active.
        ''' </summary>
        Private Const STILL_ACTIVE As UInteger = &H103UI

        ''' <summary>
        ''' Wait result indicating timeout.
        ''' </summary>
        Private Const WAIT_TIMEOUT As UInteger = &H102UI

        ''' <summary>
        ''' Allocation block size. Larger values increase effectiveness for 32-bit targets.
        ''' </summary>
        Private Const ALLOCATION_SIZE As UInteger = &H20000000UI

        ''' <summary>
        ''' Allocates memory in a remote process.
        ''' </summary>
        <DllImport("kernel32.dll", SetLastError:=True)>
        Private Shared Function VirtualAllocEx(
            hProcess As IntPtr,
            lpAddress As IntPtr,
            dwSize As UIntPtr,
            flAllocationType As UInteger,
            flProtect As UInteger) As IntPtr
        End Function

        ''' <summary>
        ''' Waits for a handle to be signaled.
        ''' </summary>
        <DllImport("kernel32.dll", SetLastError:=True)>
        Private Shared Function WaitForSingleObject(
            hHandle As IntPtr,
            dwMilliseconds As UInteger) As UInteger
        End Function

        ''' <summary>
        ''' Retrieves the exit code of a process.
        ''' </summary>
        <DllImport("kernel32.dll", SetLastError:=True)>
        Private Shared Function GetExitCodeProcess(
            hProcess As IntPtr,
            ByRef lpExitCode As UInteger) As Boolean
        End Function

        ''' <summary>
        ''' Terminates a process using VirtualAllocEx-based memory exhaustion.
        ''' </summary>
        ''' <param name="processHandle">Safe handle to the target process.</param>
        ''' <param name="userPrompter">Prompter for reporting feedback.</param>
        ''' <returns>True if the termination succeeds, otherwise False.</returns>
        Friend Shared Function Kill(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            If Not ValidateProcessHandle(processHandle, userPrompter) Then
                Return False
            End If

            Dim processId As UInteger
            If Not TryGetProcessId(processHandle, processId, userPrompter) Then
                Return False
            End If

            If Environment.Is64BitProcess Then
                userPrompter.Prompt("This technique only works reliably against 32-bit target processes.")
            End If

            If Not PerformTermination(processId, userPrompter) Then
                Return False
            End If

            Return WaitForProcessToExit(processHandle, userPrompter)
        End Function

        ''' <summary>
        ''' Executes the VirtualAllocEx memory exhaustion crash technique.
        ''' </summary>
        ''' <param name="hProcess">Handle to the target process.</param>
        ''' <returns>True if the target terminates, otherwise False.</returns>
        Private Shared Function ExecuteVirtualAllocExCrash(hProcess As IntPtr) As Boolean
            Dim attempts As Integer = 0
            Dim maxAttempts As Integer = 64

            Do
                Dim addr As IntPtr = VirtualAllocEx(
                    hProcess,
                    IntPtr.Zero,
                    New UIntPtr(ALLOCATION_SIZE),
                    MEM_RESERVE Or MEM_COMMIT,
                    PAGE_NOACCESS)

                If Equals(addr, IntPtr.Zero) Then
                    Dim waitResult As UInteger = WaitForSingleObject(hProcess, 1500UI)
                    If waitResult <> WAIT_TIMEOUT Then
                        Dim exitCode As UInteger
                        If GetExitCodeProcess(hProcess, exitCode) AndAlso exitCode <> STILL_ACTIVE Then
                            Return True
                        End If
                    End If
                    Exit Do
                End If

                attempts += 1
                If attempts >= maxAttempts Then
                    Exit Do
                End If

                If attempts Mod 4 = 0 Then
                    Dim exitCode As UInteger
                    If GetExitCodeProcess(hProcess, exitCode) AndAlso exitCode <> STILL_ACTIVE Then
                        Return True
                    End If
                End If
            Loop

            Return WaitForSingleObject(hProcess, 2000UI) <> WAIT_TIMEOUT
        End Function

        ''' <summary>
        ''' Executes the VirtualAllocEx crash technique against the target process.
        ''' </summary>
        ''' <param name="processId">Target process identifier.</param>
        ''' <param name="userPrompter">Prompter for user feedback.</param>
        ''' <returns>True if the target terminates, otherwise False.</returns>
        Private Shared Function PerformTermination(processId As UInteger, userPrompter As IUserPrompter) As Boolean
            Dim hProcess As IntPtr = NativeMethods.OpenProcess(
                ProcessAccessRights.VirtualMemoryOperation Or
                ProcessAccessRights.VirtualMemoryWrite Or
                ProcessAccessRights.QueryInformation Or
                ProcessAccessRights.Synchronize,
                False,
                processId)

            If Equals(hProcess, IntPtr.Zero) Then
                userPrompter.Prompt("Failed to open the target process.")
                Return False
            End If

            Try
                Return ExecuteVirtualAllocExCrash(hProcess)
            Finally
                NativeMethods.CloseHandle(hProcess)
            End Try
        End Function

        ''' <summary>
        ''' Validates the provided process handle.
        ''' </summary>
        Private Shared Function ValidateProcessHandle(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Return ProcessHandleValidatorUtility.ValidateProcessHandle(processHandle, userPrompter)
        End Function

        ''' <summary>
        ''' Attempts to extract a process identifier from a SafeProcessHandle.
        ''' </summary>
        Private Shared Function TryGetProcessId(processHandle As SafeProcessHandle, ByRef processId As UInteger, userPrompter As IUserPrompter) As Boolean
            processId = ProcessUtility.GetProcessId(processHandle, userPrompter)
            If processId = NativeMethods.InvalidProcessId Then
                userPrompter.Prompt("Failed to retrieve process ID.")
                Return False
            End If
            Return True
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
