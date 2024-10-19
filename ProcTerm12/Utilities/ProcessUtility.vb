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
    End Class
End Namespace
