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
        Friend Shared Function Kill(threadHandle As SafeProcessHandle) As Boolean
            Try
                ProcessHandleValidator.ValidateProcessHandle(threadHandle)
                Dim processId = GetProcessIdFromHandle(threadHandle)
                If processId = 0 Then
                    Return False
                End If
                Return TerminateAllThreads(processId)
            Catch ex As Exception
                Throw New Win32Exception(Marshal.GetLastWin32Error())
            Finally
                threadHandle.Dispose()
            End Try
        End Function

        ''' <summary>
        ''' Retrieves the process ID from the given thread handle.
        ''' </summary>
        ''' <param name="threadHandle">The handle of the thread.</param>
        ''' <returns>The process ID associated with the thread handle.</returns>
        Private Shared Function GetProcessIdFromHandle(threadHandle As SafeProcessHandle) As UInteger
            Return NativeMethods.GetProcessId(threadHandle.DangerousGetHandle())
        End Function

        ''' <summary>
        ''' Terminates all threads of the specified process.
        ''' </summary>
        ''' <param name="processId">The ID of the process whose threads are to be terminated.</param>
        ''' <returns>Returns <c>True</c> if all threads were successfully terminated; otherwise, returns <c>False</c>.</returns>
        Private Shared Function TerminateAllThreads(processId As UInteger) As Boolean
            Using snapshotHandle As New SafeProcessHandle(NativeMethods.CreateToolhelp32Snapshot(NativeMethods.Th32CsSnapThread, processId), True)
                Dim entrySize As New ThreadEntry32 With {.dwSize = CUInt(Marshal.SizeOf(GetType(ThreadEntry32)))}
                If NativeMethods.Thread32First(snapshotHandle.DangerousGetHandle(), entrySize) Then
                    Do
                        If entrySize.th32OwnerProcessID = processId Then
                            If Not TerminateThreadById(entrySize.th32ThreadID) Then
                                Return False
                            End If
                        End If
                    Loop While NativeMethods.Thread32Next(snapshotHandle.DangerousGetHandle(), entrySize)
                End If
            End Using
            Return True
        End Function

        ''' <summary>
        ''' Terminates the thread with the specified thread ID.
        ''' </summary>
        ''' <param name="threadId">The ID of the thread to be terminated.</param>
        ''' <returns>Returns <c>True</c> if the thread was successfully terminated; otherwise, returns <c>False</c>.</returns>
        Private Shared Function TerminateThreadById(threadId As UInteger) As Boolean
            Using currentThreadHandle As New SafeProcessHandle(NativeMethods.OpenThread(NativeMethods.ThreadTerminate, False, threadId), True)
                ProcessHandleValidator.ValidateProcessHandle(currentThreadHandle)
                Return NativeMethods.TerminateThread(currentThreadHandle.DangerousGetHandle(), 0)
            End Using
        End Function
    End Class
End Namespace
