Namespace Utilities.ProcessUtilities

    ''' <summary>
    ''' Provides methods to retrieve the exit code of a process and prompt the user via the specified <see cref="IUserPrompter"/> interface.
    ''' </summary>
    Friend NotInheritable Class ProcessExitCodeRetriever

        ''' <summary>
        ''' Prevents a default instance of the <see cref="ProcessExitCodeRetriever"/> class from being created.
        ''' </summary>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Handles the process termination and retrieves the exit code.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process.</param>
        ''' <param name="userPrompter">The user prompter to use for displaying messages.</param>
        ''' <returns>True if the process terminated successfully; otherwise, False.</returns>
        ''' <exception cref="Exception">Thrown when an error occurs while retrieving the exit code.</exception>
        Friend Shared Function HandleProcessTermination(processHandle As IntPtr, userPrompter As IUserPrompter) As Boolean
            Dim exitCode As UInteger = 0
            If NativeMethods.GetExitCodeProcess(processHandle, exitCode) Then
                userPrompter.Prompt($"Process terminated with exit code: {exitCode}")
                Return True
            Else
                Throw New Exception($"Failed to get exit code. Error code: {Marshal.GetLastWin32Error()}")
            End If
        End Function
    End Class
End Namespace
