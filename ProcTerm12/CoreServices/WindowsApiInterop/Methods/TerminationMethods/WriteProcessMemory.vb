Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles

Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

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

        <StructLayout(LayoutKind.Sequential)>
        Private Structure MEMORY_BASIC_INFORMATION
            Public BaseAddress As IntPtr
            Public AllocationBase As IntPtr
            Public AllocationProtect As UInteger
            Public RegionSize As UIntPtr
            Public State As UInteger
            Public Protect As UInteger
            Public Type As UInteger
        End Structure

        <DllImport("kernel32.dll", SetLastError:=True)>
        Private Shared Function VirtualQueryEx(
            hProcess As IntPtr,
            lpAddress As IntPtr,
            <Out> ByRef lpBuffer As MEMORY_BASIC_INFORMATION,
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

        Private Shared Function ValidateProcessHandle(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Return ProcessHandleValidatorUtility.ValidateProcessHandle(processHandle, userPrompter)
        End Function

        Private Shared Function TryGetProcessId(processHandle As SafeProcessHandle, ByRef processId As UInteger, userPrompter As IUserPrompter) As Boolean
            processId = ProcessUtility.GetProcessId(processHandle, userPrompter)
            If processId = NativeMethods.InvalidProcessId Then
                userPrompter.Prompt("Failed to retrieve process ID.")
                Return False
            End If
            Return True
        End Function

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
                Dim info As MEMORY_BASIC_INFORMATION

                Do
                    Dim result = VirtualQueryEx(hProcess, current, info, CType(Marshal.SizeOf(GetType(MEMORY_BASIC_INFORMATION)), UIntPtr))

                    If Equals(result, UIntPtr.Zero) Then
                        Return False ' Explicitly return False if VirtualQueryEx fails on any iteration
                    End If

                    Dim isCommitted = info.State = MemoryState.Commit
                    Dim isWritable =
                            info.Protect = MemoryProtect.ReadWrite Or
                            info.Protect = MemoryProtect.WriteCopy Or
                            info.Protect = MemoryProtect.ExecReadWrite Or
                            info.Protect = MemoryProtect.ExecWriteCopy

                    If isCommitted AndAlso isWritable Then
                        Dim size As UInteger = CUInt(info.RegionSize.ToUInt64())
                        Dim randomData(CInt(size) - 1) As Byte
                        Dim rnd As New Random
                        rnd.NextBytes(randomData)

                        Dim written As UInteger
                        WriteProcessMemory(hProcess, info.BaseAddress, randomData, size, written)
                    End If

                    current = IntPtr.Add(info.BaseAddress, CInt(info.RegionSize.ToUInt64()))
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
