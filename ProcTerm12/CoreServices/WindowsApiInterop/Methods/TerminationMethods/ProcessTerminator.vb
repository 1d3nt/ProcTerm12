Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' Handles the termination of processes using various methods.
    ''' </summary>
    Friend Class ProcessTerminator
        Implements IProcessTerminator

        ''' <summary>
        ''' The user prompter used for displaying messages to the user.
        ''' </summary>
        Private ReadOnly _userPrompter As IUserPrompter

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ProcessTerminator"/> class.
        ''' </summary>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used to display messages to the user.</param>
        Public Sub New(userPrompter As IUserPrompter)
            _userPrompter = userPrompter
        End Sub

#Region " Async Wrappers "

        ''' <summary>
        ''' Handles the termination of a process using the NtTerminateProcess method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Friend Async Function NtTerminateProcessHandler() As Task Implements IProcessTerminator.NtTerminateProcessHandler
            Await Task.Run(AddressOf TerminateProcessUsingNtTerminateProcess)
        End Function

        ''' <summary>
        ''' Handles the termination of a process by terminating each thread using the TerminateThread method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Friend Async Function TerminateThreadHandler() As Task Implements IProcessTerminator.TerminateThreadHandler
            Await Task.Run(AddressOf TerminateProcessUsingTerminateThread)
        End Function

        ''' <summary>
        ''' Handles the termination of a process using the CreateRemoteThreadExitProcess method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Friend Async Function CreateRemoteThreadExitProcessHandler() As Task Implements IProcessTerminator.CreateRemoteThreadExitProcessHandler
            Await Task.Run(AddressOf TerminateProcessUsingCreateRemoteThreadExitProcess)
        End Function
#End Region ' Async Wrappers 

        ''' <summary>
        ''' Wrapper method for NtTerminateProcessMethod.Kill to match the expected delegate signature.
        ''' </summary>
        ''' <param name="safeHandle">The safe handle of the process to terminate.</param>
        ''' <returns>True if the process was terminated successfully; otherwise, false.</returns>
        Private shared Function NtTerminateProcessWrapper(safeHandle As SafeProcessHandle) As Boolean
            Return NtTerminateProcessMethod.Kill(safeHandle, -1)
        End Function

        ''' <summary>
        ''' Attempts to terminate the Notepad process using the NtTerminateProcess method.
        ''' </summary>
        Private Sub TerminateProcessUsingNtTerminateProcess()
            TerminateProcess(AddressOf NtTerminateProcessWrapper, "NtTerminateProcess")
        End Sub

        ''' <summary>
        ''' Attempts to terminate a process by looping through and terminating each of its threads.
        ''' </summary>
        Private Sub TerminateProcessUsingTerminateThread()
            TerminateProcess(AddressOf TerminateThread.Kill, "TerminateThread")
        End Sub


        ''' <summary>
        ''' Attempts to terminate a process using the CreateRemoteThreadExitProcess method.
        ''' </summary>
        Private Sub TerminateProcessUsingCreateRemoteThreadExitProcess()
            TerminateProcess(AddressOf CreateRemoteThreadExit.Kill, "CreateRemoteThreadExitProcess")
        End Sub

        ''' <summary>
        ''' Helper method to handle the common logic for terminating a process.
        ''' </summary>
        ''' <param name="terminationMethod">The method used to terminate the process.</param>
        ''' <param name="methodName">The name of the termination method for logging purposes.</param>
        Private Sub TerminateProcess(terminationMethod As Func(Of SafeProcessHandle, Boolean), methodName As String)
            Dim processHandle As IntPtr = ProcessUtility.GetNotepadHandle()
            If Equals(processHandle, NativeMethods.NullHandleValue) Then
                _userPrompter.Prompt($"{methodName}: No running Notepad process found.")
                Exit Sub
            End If
            Dim promptMessage = $"{methodName}: Failed to terminate Notepad process."
            Using safeHandle As New SafeProcessHandle(processHandle, True)
                If terminationMethod(safeHandle) Then
                    promptMessage = $"{methodName}: Notepad process terminated successfully."
                End If
            End Using
            _userPrompter.Prompt(promptMessage)
        End Sub
    End Class
End Namespace
