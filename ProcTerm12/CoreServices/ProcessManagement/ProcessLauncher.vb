Namespace CoreServices.ProcessManagement

    ''' <summary>
    ''' Provides functionality to launch Notepad.
    ''' This class contains a hardcoded path to Notepad for testing purposes.
    ''' </summary>
    Friend Class ProcessLauncher
        Implements IProcessLauncher

        ''' <summary>
        ''' The path to the Notepad executable.
        ''' This is a hardcoded value used for testing purposes.
        ''' </summary>
        Private Const NotepadExePath As String = "C:\Windows\System32\notepad.exe"

        ''' <summary>
        ''' Launches Notepad minimized.
        ''' </summary>
        ''' <remarks>
        ''' This method encapsulates the logic required to start Notepad in a minimized state.
        ''' The path to Notepad is hardcoded as <c>C:\Windows\System32\notepad.exe</c> for testing purposes.
        ''' Implementations should ensure proper handling of process initialization and any associated resources.
        ''' </remarks>
        ''' <exception cref="InvalidOperationException">Thrown when the process fails to start.</exception>
        Friend Sub LaunchProcess() Implements IProcessLauncher.LaunchProcess
            Try
                Dim startInfo As ProcessStartInfo = GetNotepadStartInfo()
                Process.Start(startInfo)
            Catch ex As Exception
                Throw New InvalidOperationException("Failed to launch Notepad process.", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Gets the <see cref="ProcessStartInfo"/> for launching Notepad in a minimized state.
        ''' </summary>
        ''' <returns>
        ''' A <see cref="ProcessStartInfo"/> object configured to launch Notepad minimized.
        ''' </returns>
        ''' <remarks>
        ''' This method encapsulates the configuration required to start Notepad in a minimized state.
        ''' The path to Notepad is hardcoded as <c>C:\Windows\System32\notepad.exe</c> for testing purposes.
        ''' </remarks>
        Private shared Function GetNotepadStartInfo() As ProcessStartInfo
            Return New ProcessStartInfo() With {
                .FileName = NotepadExePath,
                .UseShellExecute = True,
                .WindowStyle = ProcessWindowStyle.Minimized
            }
        End Function
    End Class
End Namespace
