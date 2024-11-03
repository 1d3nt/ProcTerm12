Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' Provides methods to terminate a specified process using the CreateRemoteThread technique.
    ''' </summary>
    Friend Class CreateRemoteThreadExit

        ''' <summary>
        ''' Represents the error code for access denied.
        ''' </summary>
        Private Const AccessDenied As Integer = 5

        ''' <summary>
        ''' Represents the error code for an invalid handle.
        ''' </summary>
        Private Const InvalidHandle As Integer = 6

        ''' <summary>
        ''' Represents the name of the Kernel32 DLL used in Windows API calls.
        ''' </summary>
        ''' <remarks>
        ''' This constant is primarily used in P/Invoke declarations that require a reference to the Kernel32 DLL, 
        ''' such as the <see cref="NativeMethods.GetModuleHandle"/> method.
        ''' </remarks>
        Private Const Kernel32Dll As String = "kernel32.dll"

        ''' <summary>
        ''' Terminates the specified process by creating a remote thread that executes ExitProcess.
        ''' </summary>
        ''' <param name="processHandle">
        ''' A handle to the process to be terminated. The handle must have the 
        ''' <see cref="ProcessAccessRights.Terminate"/> access right.
        ''' </param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used to display messages to the user.</param>
        ''' <returns>
        ''' Returns <c>True</c> if the process was successfully terminated; 
        ''' otherwise, returns <c>False</c>.
        ''' </returns>
        Friend Shared Function Kill(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Try
                If Not ValidateProcessHandle(processHandle, userPrompter) Then
                    Return False
                End If
                Dim processId As UInteger
                If Not TryGetProcessId(processHandle, processId, userPrompter) Then
                    Return False
                End If
                Dim exitProcessAddress As IntPtr = GetExitProcessAddress(userPrompter)
                If Equals(exitProcessAddress, NativeMethods.NullHandleValue) Then
                    Return False
                End If
                Dim allocatedMemory As IntPtr = AllocateMemory(processHandle, exitProcessAddress, userPrompter)
                If Equals(allocatedMemory, NativeMethods.NullHandleValue) Then
                    Return False
                End If
                If Not WriteMemory(processHandle, allocatedMemory, exitProcessAddress, userPrompter) Then
                    Return False
                End If
                If Not CreateAndWaitForRemoteThread(processHandle, allocatedMemory, userPrompter) Then
                    Return False
                End If
                If Not FreeAllocatedMemory(processHandle, allocatedMemory, processId, userPrompter) Then
                    Return False
                End If
                Return True
            Catch ex As Exception
                userPrompter.Prompt("An error occurred: " & ex.Message)
                Return False
            End Try
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
        ''' Retrieves the address of the ExitProcess function in kernel32.dll.
        ''' </summary>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used to display messages to the user.</param>
        ''' <returns>The address of the ExitProcess function, or NativeMethods.NullHandleValue if an error occurred.</returns>
        Private Shared Function GetExitProcessAddress(userPrompter As IUserPrompter) As IntPtr
            Dim hModule As IntPtr = NativeMethods.GetModuleHandle(Kernel32Dll)
            If Equals(hModule, NativeMethods.NullHandleValue) Then
                userPrompter.Prompt($"Failed to get module handle for kernel32.dll. Last error: {Win32Error.GetLastPInvokeError()}")
                Return NativeMethods.NullHandleValue
            End If
            Dim exitProcessAddress As IntPtr = NativeMethods.GetProcAddress(hModule, "ExitProcess")
            If Equals(exitProcessAddress, NativeMethods.NullHandleValue) Then
                userPrompter.Prompt($"Failed to get address of ExitProcess. Last error: {Win32Error.GetLastPInvokeError()}")
            End If
            Return exitProcessAddress
        End Function

        ''' <summary>
        ''' Allocates memory in the specified process for the ExitProcess function.
        ''' </summary>
        ''' <param name="processHandle">The handle to the process.</param>
        ''' <param name="exitProcessAddress">The address of the ExitProcess function.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used to display messages to the user.</param>
        ''' <returns>The address of the allocated memory, or NativeMethods.NullHandleValue if an error occurred.</returns>
        Private Shared Function AllocateMemory(processHandle As SafeProcessHandle, exitProcessAddress As IntPtr, userPrompter As IUserPrompter) As IntPtr
            Dim allocatedMemory As IntPtr = NativeMethods.VirtualAllocEx(processHandle, IntPtr.Zero, New UIntPtr(CUInt(Marshal.SizeOf(exitProcessAddress))), NativeMethods.MemCommit Or NativeMethods.MemReserve, NativeMethods.PageExecuteReadWrite)
            If Equals(allocatedMemory, NativeMethods.NullHandleValue) Then
                userPrompter.Prompt($"Failed to allocate memory in target process. Last error: {Win32Error.GetLastPInvokeError()}")
            End If
            Return allocatedMemory
        End Function

        ''' <summary>
        ''' Writes the address of the ExitProcess function to the allocated memory in the specified process.
        ''' </summary>
        ''' <param name="processHandle">The handle to the process.</param>
        ''' <param name="allocatedMemory">The address of the allocated memory.</param>
        ''' <param name="exitProcessAddress">The address of the ExitProcess function.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used to display messages to the user.</param>
        ''' <returns><c>True</c> if the memory was successfully written; otherwise, <c>False</c>.</returns>
        Private Shared Function WriteMemory(processHandle As SafeProcessHandle, allocatedMemory As IntPtr, exitProcessAddress As IntPtr, userPrompter As IUserPrompter) As Boolean
            Dim bytesWritten As UInteger
            Dim addressBytes As Byte() = BitConverter.GetBytes(exitProcessAddress.ToInt64())
            Dim addressIntPtr = New IntPtr(BitConverter.ToInt64(addressBytes, 0))
            Dim success As Boolean = NativeMethods.WriteProcessMemory(processHandle, allocatedMemory, addressIntPtr, CType(addressBytes.Length, UInteger), bytesWritten)
            If Not success Then
                userPrompter.Prompt($"Failed to write memory in target process. Last error: {Win32Error.GetLastPInvokeError()}")
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Creates a remote thread in the specified process to execute the ExitProcess function and waits for it to complete.
        ''' </summary>
        ''' <param name="processHandle">The handle to the process.</param>
        ''' <param name="allocatedMemory">The address of the allocated memory.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used to display messages to the user.</param>
        ''' <returns><c>True</c> if the remote thread was successfully created and completed; otherwise, <c>False</c>.</returns>
        Private Shared Function CreateAndWaitForRemoteThread(processHandle As SafeProcessHandle, allocatedMemory As IntPtr, userPrompter As IUserPrompter) As Boolean
            userPrompter.Prompt("Attempting to create a remote thread in the target process.")
            Using threadHandle As New SafeProcessHandle(NativeMethods.CreateRemoteThread(processHandle, NativeMethods.NullHandleValue, 0, allocatedMemory, NativeMethods.NullHandleValue, 0, 0), True)
                If threadHandle.IsInvalid Then
                    userPrompter.Prompt($"Failed to create remote thread in target process. Last error: {Win32Error.GetLastPInvokeError()}")
                    Return False
                End If
                userPrompter.Prompt("Remote thread created successfully. Waiting for the remote thread to complete.")
                Dim waitResult As UInteger = NativeMethods.WaitForSingleObject(threadHandle, NativeMethods.MemInfinite)
                If waitResult <> 0 Then
                    userPrompter.Prompt($"Failed to wait for remote thread to complete. Last error: {Win32Error.GetLastPInvokeError()}")
                    Return False
                End If
                userPrompter.Prompt("Remote thread completed successfully.")
            End Using
            Return True
        End Function

        ''' <summary>
        ''' Frees allocated memory in the specified process and handles potential race conditions 
        ''' that may result in an invalid handle or access denied error.
        ''' </summary>
        ''' <param name="processHandle">A <see cref="SafeProcessHandle"/> to the target process, 
        ''' which must have been previously validated.</param>
        ''' <param name="allocatedMemory">The memory address in the target process to be freed.</param>
        ''' <param name="processId">The ID of the target process, used to check its status if freeing fails.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> for displaying user notifications.</param>
        ''' <returns><c>True</c> if the memory was successfully freed or the handle is invalid due to a race condition; 
        ''' otherwise, <c>False</c> if an unexpected error occurs.</returns>
        ''' <remarks>
        ''' This method calls <c>VirtualFreeEx</c> to release memory allocated in a target process. 
        ''' Due to potential race conditions, the target process may terminate just before this call, 
        ''' resulting in a possible invalid handle or access denied error. These specific errors are ignored 
        ''' since the process is assumed to have exited if they occur, but any other errors are reported.
        ''' </remarks>
        Private Shared Function FreeAllocatedMemory(processHandle As SafeProcessHandle, allocatedMemory As IntPtr, processId As UInteger, userPrompter As IUserPrompter) As Boolean
            Dim success As Boolean = NativeMethods.VirtualFreeEx(processHandle, allocatedMemory, UIntPtr.Zero, NativeMethods.MemRelease)
            If Not success Then
                Dim lastError As Integer = Win32Error.GetLastPInvokeErrorCode()
                If lastError <> InvalidHandle AndAlso lastError <> AccessDenied Then
                    userPrompter.Prompt($"Failed to free allocated memory in target process. Last error: {LastError}")
                    If CheckIfProcessIsRunning(processId, userPrompter, "Notepad process is still running.") Then
                        Return False
                    End If
                End If
            End If
            Return True
        End Function

        ''' <summary>
        ''' Checks if a process with the specified process ID is running and prompts the user with a message if it is.
        ''' </summary>
        ''' <param name="processId">The ID of the process to check.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used to display messages to the user.</param>
        ''' <param name="message">The message to display if the process is still running.</param>
        ''' <returns>
        ''' <c>True</c> if the process is still running and the user was prompted; otherwise, <c>False</c>.
        ''' </returns>
        Private Shared Function CheckIfProcessIsRunning(processId As UInteger, userPrompter As IUserPrompter, message As String) As Boolean
            If ProcessUtility.IsProcessRunning(processId) Then
                userPrompter.Prompt(message)
                Return True
            End If
            Return False
        End Function
    End Class
End Namespace
