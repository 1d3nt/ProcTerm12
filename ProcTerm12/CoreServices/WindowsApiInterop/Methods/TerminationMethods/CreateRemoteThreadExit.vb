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
                ProcessHandleValidator.ValidateProcessHandle(processHandle)
                Dim processId As UInteger = NativeMethods.GetProcessId(processHandle.DangerousGetHandle())
                If processId = 0 Then
                    userPrompter.Prompt("Failed to get process ID. Last error: " & Marshal.GetLastWin32Error())
                    Return False
                End If

                Dim kernel32Handle As IntPtr = NativeMethods.GetModuleHandle("kernel32.dll")
                If Equals(kernel32Handle, NativeMethods.NullHandleValue) Then
                    userPrompter.Prompt("Failed to get handle to kernel32.dll. Last error: " & Marshal.GetLastWin32Error())
                    Return False
                End If

                Dim exitProcessAddress As IntPtr = NativeMethods.GetProcAddress(kernel32Handle, "ExitProcess")
                If Equals(exitProcessAddress, NativeMethods.NullHandleValue) Then
                    userPrompter.Prompt("Failed to get address of ExitProcess. Last error: " & Marshal.GetLastWin32Error())
                    Return False
                End If

                Dim allocatedMemory As IntPtr = NativeMethods.VirtualAllocEx(processHandle.DangerousGetHandle(), IntPtr.Zero,
                                                                             New UIntPtr(CUInt(Marshal.SizeOf(exitProcessAddress))),
                                                                             NativeMethods.MemCommit Or NativeMethods.MemReserve,
                                                                             NativeMethods.PageExecuteReadWrite)
                If Equals(allocatedMemory, NativeMethods.NullHandleValue) Then
                    userPrompter.Prompt("Failed to allocate memory in target process. Last error: " & Marshal.GetLastWin32Error())
                    Return False
                End If

                Dim bytesWritten As UInteger
                Dim addressBytes As Byte() = BitConverter.GetBytes(exitProcessAddress.ToInt64())
                Dim addressIntPtr = New IntPtr(BitConverter.ToInt64(addressBytes, 0))
                If Not NativeMethods.WriteProcessMemory(processHandle.DangerousGetHandle(), allocatedMemory,
                                                        addressIntPtr,
                                                        CType(addressBytes.Length, UInteger), bytesWritten) Then
                    userPrompter.Prompt("Failed to write memory in target process. Last error: " & Marshal.GetLastWin32Error())
                    Return False
                End If

                Dim remoteThreadHandle As IntPtr = NativeMethods.CreateRemoteThread(processHandle.DangerousGetHandle(),
                                                                                    IntPtr.Zero,
                                                                                    0,
                                                                                    allocatedMemory,
                                                                                    IntPtr.Zero,
                                                                                    0,
                                                                                    0)
                If Equals(remoteThreadHandle, NativeMethods.NullHandleValue) Then
                    userPrompter.Prompt("Failed to create remote thread. Last error: " & Marshal.GetLastWin32Error())
                    Return False
                End If

                Dim waitResult As UInteger = NativeMethods.WaitForSingleObject(remoteThreadHandle, NativeMethods.MemInfinite)
                If waitResult <> 0 Then
                    userPrompter.Prompt("Failed to wait for remote thread. Result: " & waitResult)
                    If CheckIfProcessIsRunning(processId, userPrompter, "Notepad process is still running.") Then
                        Return False
                    End If
                End If

                Dim freeResult As Boolean = NativeMethods.VirtualFreeEx(processHandle.DangerousGetHandle(), allocatedMemory, UIntPtr.Zero, NativeMethods.MemRelease)
                If Not freeResult Then
                    Dim lastError As Integer = Marshal.GetLastWin32Error()
                    If lastError <> InvalidHandle AndAlso lastError <> AccessDenied Then
                        userPrompter.Prompt("Failed to free allocated memory in target process. Last error: " & lastError)
                        If CheckIfProcessIsRunning(processId, userPrompter, "Notepad process is still running.") Then
                            Return False
                        End If
                    End If
                End If

                Return True
            Catch ex As Exception
                userPrompter.Prompt("An error occurred: " & ex.Message)
                Return False
            Finally
                HandleManager.CloseProcessHandleIfNotNull(processHandle)
            End Try
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
