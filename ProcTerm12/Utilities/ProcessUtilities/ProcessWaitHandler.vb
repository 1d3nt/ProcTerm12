Namespace Utilities.ProcessUtilities

    ''' <summary>
    ''' Provides methods to handle process waiting and prompt the user via the specified <see cref="IUserPrompter"/> interface.
    ''' </summary>
    Friend NotInheritable Class ProcessWaitHandler

        ''' <summary>
        ''' Prevents a default instance of the <see cref="ProcessWaitHandler"/> class from being created.
        ''' </summary>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Waits for the specified process to terminate and handles the result.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to wait for.</param>
        ''' <param name="userPrompter">The user prompter to use for displaying messages.</param>
        ''' <returns>True if the process terminated successfully; otherwise, False.</returns>
        ''' <exception cref="Exception">Thrown when an error occurs while waiting for the process.</exception>
        Friend Shared Function WaitForProcessExit(processHandle As IntPtr, userPrompter As IUserPrompter) As Boolean
            Dim waitResult As UInteger = NativeMethods.WaitForSingleObject(processHandle, 5000)
            Return HandleWaitResult(waitResult, processHandle, userPrompter)
        End Function

        ''' <summary>
        ''' Handles the result of the wait operation.
        ''' </summary>
        ''' <param name="waitResult">The result of the wait operation.</param>
        ''' <param name="processHandle">The handle of the process.</param>
        ''' <param name="userPrompter">The user prompter to use for displaying messages.</param>
        ''' <returns>True if the process terminated successfully; otherwise, False.</returns>
        ''' <exception cref="Exception">Thrown when an error occurs while waiting for the process.</exception>
        Private Shared Function HandleWaitResult(waitResult As UInteger, processHandle As IntPtr, userPrompter As IUserPrompter) As Boolean
            Select Case waitResult
                Case NativeMethods.WaitObject0
                    Return ProcessExitCodeRetriever.HandleProcessTermination(processHandle, userPrompter)
                Case NativeMethods.WaitTimeout
                    userPrompter.Prompt("Process termination timed out")
                    Return False
                Case NativeMethods.WaitFailed
                    Throw New Exception($"WaitForSingleObject failed. Error message: {Win32Error.GetLastPInvokeError()}")
                Case Else
                    Return False
            End Select
        End Function
    End Class
End Namespace
