Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' Provides methods to terminate all threads of a specified process.
    ''' </summary>
    Friend Class TerminateThread

        ''' <summary>
        ''' Terminates all threads of the specified process using the provided thread handle.
        ''' </summary>
        ''' <param name="threadHandle">
        ''' A handle to the thread to be terminated. The handle must have the 
        ''' <see cref="ProcessAccessRights.Terminate"/> access right.
        ''' </param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>
        ''' Returns <c>True</c> if all threads were successfully terminated; 
        ''' otherwise, returns <c>False</c>.
        ''' </returns>
        ''' <remarks>
        ''' For more information, see 
        ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-terminatethread">
        ''' the TerminateThread documentation</see>.
        ''' 
        ''' While we could directly retrieve the process ID from the process object, 
        ''' this implementation first obtains the thread handle. From the thread handle, 
        ''' we extract the process ID using <see cref="NativeMethods.GetProcessId"/>. This approach is 
        ''' used to maintain consistency across all termination methods, ensuring that 
        ''' all operations utilize <see cref="SafeProcessHandle"/> to manage handles safely.
        ''' </remarks>
        ''' <summary>
        ''' Terminates all threads for the specified process.
        ''' </summary>
        ''' <param name="threadHandle">The handle of the process whose threads are to be terminated.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the threads were terminated successfully; otherwise, <c>False</c>.</returns>
        ''' <summary>
        ''' Terminates all threads for the specified process.
        ''' </summary>
        ''' <param name="threadHandle">The handle of the process whose threads are to be terminated.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the threads were terminated successfully; otherwise, <c>False</c>.</returns>
        Friend Shared Function Kill(threadHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            If Not ValidateProcessHandle(threadHandle, userPrompter) Then
                Return False
            End If
            Dim processId As UInteger
            If Not TryGetProcessId(threadHandle, processId, userPrompter) Then
                Return False
            End If
            Return TerminateAllThreads(processId, userPrompter)
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
        ''' Terminates all threads of the specified process.
        ''' </summary>
        ''' <param name="processId">The ID of the process whose threads are to be terminated.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>Returns <c>True</c> if all threads were successfully terminated; otherwise, returns <c>False</c>.</returns>
        Private Shared Function TerminateAllThreads(processId As UInteger, userPrompter As IUserPrompter) As Boolean
            Using snapshotHandle As New SafeProcessHandle(NativeMethods.CreateToolhelp32Snapshot(NativeMethods.Th32CsSnapThread, processId), True)
                Dim entrySize As New ThreadEntry32 With {.dwSize = CUInt(Marshal.SizeOf(GetType(ThreadEntry32)))}
                If NativeMethods.Thread32First(snapshotHandle, entrySize) Then
                    Do
                        If entrySize.th32OwnerProcessID = processId Then
                            If Not TerminateThreadById(entrySize.th32ThreadID, userPrompter) Then
                                userPrompter.Prompt($"Failed to terminate thread ID {entrySize.th32ThreadID}.")
                                Return False
                            End If
                        End If
                    Loop While NativeMethods.Thread32Next(snapshotHandle, entrySize)
                End If
            End Using
            Return True
        End Function

        ''' <summary>
        ''' Terminates the thread with the specified thread ID.
        ''' </summary>
        ''' <param name="threadId">The ID of the thread to be terminated.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>Returns <c>True</c> if the thread was successfully terminated; otherwise, returns <c>False</c>.</returns>
        Private Shared Function TerminateThreadById(threadId As UInteger, userPrompter As IUserPrompter) As Boolean
            Using currentThreadHandle As New SafeProcessHandle(NativeMethods.OpenThread(NativeMethods.ThreadTerminate, False, threadId), True)
                If Not ValidateProcessHandle(currentThreadHandle, userPrompter) Then
                    Return False
                End If
                Return NativeMethods.TerminateThread(currentThreadHandle, 0)
            End Using
        End Function
    End Class
End Namespace
