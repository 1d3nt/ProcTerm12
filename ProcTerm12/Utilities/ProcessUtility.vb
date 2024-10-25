Namespace Utilities

    ''' <summary>
    ''' Provides utility methods for process management.
    ''' </summary>
    Public NotInheritable Class ProcessUtility

        ''' <summary>
        ''' Retrieves the handle of the first running Notepad process.
        ''' </summary>
        ''' <returns>
        ''' A handle to the first Notepad process if it is running; otherwise, <c>IntPtr.Zero</c>.
        ''' </returns>
        ''' <remarks>
        ''' This method searches for all instances of Notepad and returns the handle of the first one found.
        ''' If no Notepad process is running, <c>IntPtr.Zero</c> is returned.
        ''' </remarks>
        Friend Shared Function GetNotepadHandle() As IntPtr
            Const processName = "notepad"

            Dim notepadProcess = Process.GetProcessesByName(processName).FirstOrDefault()
            If notepadProcess IsNot Nothing Then
                Return notepadProcess.Handle
            End If
            Return IntPtr.Zero
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
    End Class
End Namespace
