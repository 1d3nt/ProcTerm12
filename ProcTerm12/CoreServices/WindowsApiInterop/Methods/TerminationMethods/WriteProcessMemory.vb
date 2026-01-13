Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles

Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' Provides functionality to terminate a process by corrupting its memory regions using the <c>VirtualQueryEx</c> 
    ''' and <c>WriteProcessMemory</c> APIs. This method loops through the memory regions of the target process and writes 
    ''' random data to them, effectively destabilizing the process.
    ''' </summary>
    ''' <remarks>
    ''' This technique involves the following steps:
    ''' 
    ''' 1. **Validate Process Handle**: The method first validates the provided process handle to ensure it is valid and has 
    ''' the necessary permissions using <see cref="ProcessHandleValidatorUtility.ValidateProcessHandle"/>.
    ''' 
    ''' 2. **Retrieve Process ID**: The process ID is retrieved from the process handle using <see cref="ProcessUtility.GetProcessId"/>.
    ''' 
    ''' 3. **Corrupt Memory Regions**: The method iterates through the memory regions of the target process using 
    ''' <c>VirtualQueryEx</c>, identifies writable and committed regions, and writes random data to them using 
    ''' <c>WriteProcessMemory</c>.
    ''' 
    ''' 4. **Wait for Process Exit**: After corrupting the memory regions, the method waits for the process to completely exit 
    ''' using <see cref="ProcessWaitHandler.WaitForProcessExit"/>.
    ''' 
    ''' <para>
    ''' <b>Behavior and Usage</b>:
    ''' This method destabilizes the target process by corrupting its memory regions. It is a destructive and unreliable 
    ''' termination technique that bypasses cleanup routines and can lead to undefined behavior. This method is not 
    ''' recommended for use on modern systems, as it is unlikely to work due to enhanced security mechanisms such as 
    ''' Data Execution Prevention (DEP), Address Space Layout Randomization (ASLR), and PatchGuard.
    ''' </para>
    ''' 
    ''' <para>
    ''' <b>System Internals</b>:
    ''' The <c>VirtualQueryEx</c> API retrieves information about the memory regions of a process, including their state 
    ''' (e.g., committed or reserved) and protection level (e.g., read/write). The <c>WriteProcessMemory</c> API writes 
    ''' data to the specified memory region of a process. Together, these APIs allow for direct manipulation of a process's 
    ''' memory, which can destabilize or crash the process.
    ''' </para>
    ''' 
    ''' <para>
    ''' <b>Considerations and Risks</b>:
    ''' - This method is highly destructive and should only be used in controlled environments for testing or research purposes.
    ''' - It is unlikely to work on modern systems due to advanced security features.
    ''' - Writing random data to memory regions can lead to unpredictable behavior, including system instability or crashes.
    ''' </para>
    ''' 
    ''' <para>API Functions Used:</para>
    ''' <list>
    '''     <item>
    '''         <term>VirtualQueryEx</term>
    '''         <description>
    '''         Retrieves information about a range of pages in the virtual address space of a specified process. 
    '''         This is used to identify writable and committed memory regions in the target process.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>WriteProcessMemory</term>
    '''         <description>
    '''         Writes data to a specified memory location in the virtual address space of a specified process. 
    '''         This is used to corrupt the memory regions of the target process.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>VirtualAllocEx</term>
    '''         <description>
    '''         Allocates memory in the virtual address space of a specified process. This can be used in a loop 
    '''         to exhaust the memory of the target process, causing it to crash when no more memory can be allocated.
    '''         </description>
    '''     </item>
    ''' </list>
    ''' </remarks>

    Friend Class WriteProcessMemory

        <Flags>
        Private Enum MemoryState As UInteger
            Commit = &H1000UI
        End Enum

        <Flags>
        Private Enum MemoryProtect As UInteger
            NoAccess = &H1UI
            [ReadOnly] = &H2UI
            ReadWrite = &H4UI
            WriteCopy = &H8UI
            ExecReadWrite = &H40UI
            ExecWriteCopy = &H80UI
        End Enum

        '<StructLayout(LayoutKind.Sequential)>
        'Private Structure MemoryBasicInformation
        '    Public BaseAddress As IntPtr
        '    Public AllocationBase As IntPtr
        '    Public AllocationProtect As UInteger
        '    Public RegionSize As UIntPtr
        '    Public State As UInteger
        '    Public Protect As UInteger
        '    Public Type As UInteger
        'End Structure

        <DllImport("kernel32.dll", SetLastError:=True)>
        Private Shared Function VirtualQueryEx(
            hProcess As IntPtr,
            lpAddress As IntPtr,
            <Out> ByRef lpBuffer As MemoryBasicInformation,
            dwLength As UIntPtr) As UIntPtr
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)>
        Private Shared Function WriteProcessMemory(
            hProcess As IntPtr,
            lpBaseAddress As IntPtr,
            lpBuffer As Byte(),
            nSize As UInteger,
            ByRef lpNumberOfBytesWritten As UInteger) As Boolean
        End Function

        ''' <summary>
        ''' Terminates the specified process by corrupting its memory regions.
        ''' </summary>
        ''' <param name="processHandle">
        ''' A handle to the process to be terminated. The handle must have the necessary access rights to query and write 
        ''' to the process's memory.
        ''' </param>
        ''' <param name="userPrompter">
        ''' An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.
        ''' </param>
        ''' <returns>
        ''' <c>True</c> if the process was terminated successfully; otherwise, <c>False</c>.
        ''' </returns>
        Friend Shared Function Kill(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            If Not ValidateProcessHandle(processHandle, userPrompter) Then
                Return False
            End If

            Dim pid As UInteger
            If Not TryGetProcessId(processHandle, pid, userPrompter) Then
                Return False
            End If

            If Not CorruptMemoryRegions(pid, userPrompter) Then
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
        ''' Retrieves the process ID from the process handle.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process.</param>
        ''' <param name="processId">The retrieved process ID.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the process ID was retrieved successfully; otherwise, <c>False</c>.</returns>
        Private Shared Function TryGetProcessId(processHandle As SafeProcessHandle, ByRef processId As UInteger, userPrompter As IUserPrompter) As Boolean
            processId = ProcessUtility.GetProcessId(processHandle, userPrompter)
            If processId = NativeMethods.InvalidProcessId Then
                userPrompter.Prompt("Failed to retrieve process ID.")
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Corrupts the memory regions of the specified process.
        ''' </summary>
        ''' <param name="processId">The ID of the process whose memory regions are to be corrupted.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the memory regions were successfully corrupted; otherwise, <c>False</c>.</returns>
        Private Shared Function CorruptMemoryRegions(processId As UInteger, userPrompter As IUserPrompter) As Boolean
            Dim hProcess As IntPtr = NativeMethods.OpenProcess(
                ProcessAccessRights.QueryInformation Or
                ProcessAccessRights.VirtualMemoryRead Or
                ProcessAccessRights.VirtualMemoryWrite Or
                ProcessAccessRights.VirtualMemoryOperation Or
                ProcessAccessRights.Synchronize,
                False,
                processId)

            If Equals(hProcess, IntPtr.Zero) Then
                userPrompter.Prompt("Failed to open target process.")
                Return False
            End If

            Try
                Dim current As IntPtr = IntPtr.Zero
                Dim info As MemoryBasicInformation

                Do
                    Dim result = VirtualQueryEx(hProcess, current, info, CType(Marshal.SizeOf(GetType(MemoryBasicInformation)), UIntPtr))

                    If Equals(result, UIntPtr.Zero) Then
                        Return False 
                    End If

                    Dim isCommitted = info.State = MemoryState.Commit
                    Dim isWritable =
                            info.Protect = MemoryProtect.ReadWrite Or
                            info.Protect = MemoryProtect.WriteCopy Or
                            info.Protect = MemoryProtect.ExecReadWrite Or
                            info.Protect = MemoryProtect.ExecWriteCopy

                    If isCommitted AndAlso isWritable Then
                        Dim size As UInteger = CUInt(info.RegionSize)
                        Dim randomData(CInt(size) - 1) As Byte
                        Dim rnd As New Random
                        rnd.NextBytes(randomData)

                        Dim written As UInteger
                        WriteProcessMemory(hProcess, info.BaseAddress, randomData, size, written)
                    End If

                    current = IntPtr.Add(info.BaseAddress, CInt(info.RegionSize))
                Loop

                Return True
            Finally
                NativeMethods.CloseHandle(hProcess)
            End Try
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
