Namespace Utilities

    ''' <summary>
    ''' Provides utility methods for process management.
    ''' </summary>
    Friend NotInheritable Class ProcessUtility

        ''' <summary>
        ''' Retrieves the handle of the first running Notepad process.
        ''' </summary>
        ''' <returns>
        ''' A handle to the first Notepad process if it is running; otherwise, <see cref="NativeMethods.NullHandleValue"/>.
        ''' </returns>
        ''' <remarks>
        ''' This method searches for all instances of Notepad and returns the handle of the first one found.
        ''' If no Notepad process is running, <see cref="NativeMethods.NullHandleValue"/> is returned.
        ''' </remarks>
        Friend Shared Function GetNotepadHandleByName() As IntPtr
            Const processName = "notepad"
            Try
                Dim notepadProcess = Process.GetProcessesByName(processName).FirstOrDefault()
                If notepadProcess IsNot Nothing Then
                    Return notepadProcess.Handle
                End If
            Catch ex As Exception
            End Try
            Return NativeMethods.NullHandleValue
        End Function

        ''' <summary>
        ''' Retrieves the handle of the process with the specified ID.
        ''' </summary>
        ''' <param name="processId">The ID of the process.</param>
        ''' <returns>
        ''' A handle to the process if it is running; otherwise, <see cref="NativeMethods.NullHandleValue"/>.
        ''' </returns>
        ''' <remarks>
        ''' This method searches for the process with the specified ID and returns its handle.
        ''' If the process is not running, <see cref="NativeMethods.NullHandleValue"/> is returned.
        ''' </remarks>
        Friend Shared Function GetProcessHandleById(processId As Integer) As IntPtr
            Try
                Dim process As Process = Process.GetProcessById(processId)
                Return process.Handle
            Catch ex As ArgumentException
                Return NativeMethods.NullHandleValue
            End Try
        End Function

        ''' <summary>
        ''' Checks if a process with the specified process ID is running.
        ''' </summary>
        ''' <param name="processId">The ID of the process to check.</param>
        ''' <returns>
        ''' <c>True</c> if the process is running; otherwise, <c>False</c>.
        ''' </returns>
        Friend Shared Function IsProcessRunning(processId As UInteger) As Boolean
            Try
                Dim process As Process = Process.GetProcessById(CInt(processId))
                Return Not process.HasExited
            Catch ex As ArgumentException
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Retrieves the process ID for the specified process handle.
        ''' </summary>
        ''' <param name="processHandle">The handle to the process.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used to display messages to the user.</param>
        ''' <returns>The process ID, or 0 if an error occurred.</returns>
        Friend Shared Function GetProcessId(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As UInteger
            Dim processId As UInteger = NativeMethods.GetProcessId(processHandle)
            If processHandle.IsInvalid Then
                userPrompter.Prompt($"Failed to get process ID. Last error: {Win32Error.GetLastPInvokeError()}")
                Return 0
            End If
            Return processId
        End Function
    End Class
End Namespace
