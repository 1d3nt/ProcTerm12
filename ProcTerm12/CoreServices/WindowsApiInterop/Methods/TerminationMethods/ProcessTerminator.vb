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

        ''' <summary>
        ''' Handles the termination of a process by duplicating the process handle 
        ''' and performing the necessary operations to terminate the process.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Friend Async Function DuplicateHandleHandler() As Task Implements IProcessTerminator.DuplicateHandleHandler
            Await Task.Run(AddressOf TerminateProcessUsingDuplicateHandle)
        End Function
#End Region ' Async Wrappers 

#Region " Synchronous Wrappers "

        ''' <summary>
        ''' Wrapper method for NtTerminateProcessMethod.Kill to match the expected delegate signature.
        ''' </summary>
        ''' <param name="safeHandle">The safe handle of the process to terminate.</param>
        ''' <returns>True if the process was terminated successfully; otherwise, false.</returns>
        Private Shared Function NtTerminateProcessWrapper(safeHandle As SafeProcessHandle) As Boolean
            Return NtTerminateProcessMethod.Kill(safeHandle, -1)
        End Function

        ''' <summary>
        ''' Wrapper method for CreateRemoteThreadExit.Kill to match the expected delegate signature.
        ''' </summary>
        ''' <param name="safeHandle">The safe handle of the process to terminate.</param>
        ''' <returns>True if the process was terminated successfully; otherwise, false.</returns>
        Private Shared Function CreateRemoteThreadExitWrapper(safeHandle As SafeProcessHandle) As Boolean
            Dim userPrompter As IUserPrompter = UserPrompterSingleton.Instance
            Return CreateRemoteThreadExit.Kill(safeHandle, userPrompter)
        End Function

        ''' <summary>
        ''' Wrapper method for DuplicateHandle.Kill to match the expected delegate signature.
        ''' </summary>
        ''' <param name="safeHandle">The safe handle of the process to terminate.</param>
        ''' <returns>True if the process was terminated successfully; otherwise, false.</returns>
        Private Shared Function DuplicateHandleWrapper(safeHandle As SafeProcessHandle) As Boolean
            Dim userPrompter As IUserPrompter = UserPrompterSingleton.Instance
            Return DuplicateHandle.Kill(safeHandle, userPrompter)
        End Function
#End Region ' Synchronous Wrappers

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
            TerminateProcess(AddressOf CreateRemoteThreadExitWrapper, "CreateRemoteThreadExitProcess")
        End Sub

        ''' <summary>
        ''' Attempts to terminate a process using the DuplicateHandle method.
        ''' </summary>
        Private Sub TerminateProcessUsingDuplicateHandle()
            TerminateProcess(AddressOf DuplicateHandleWrapper, "DuplicateHandle")
        End Sub

        ''' <summary>
        ''' Helper method to handle the common logic for terminating a process.
        ''' </summary>
        ''' <param name="terminationMethod">The method used to terminate the process.</param>
        ''' <param name="methodName">The name of the termination method for logging purposes.</param>
        Private Sub TerminateProcess(terminationMethod As Func(Of SafeProcessHandle, Boolean), methodName As String)
            Dim processHandle As IntPtr = GetProcessHandle(methodName)
            If Equals(processHandle, NativeMethods.NullHandleValue) Then
                Exit Sub
            End If

            Dim promptMessage As String = $"{methodName}: Failed to terminate Notepad process."
            Try
                Using safeHandle As New SafeProcessHandle(processHandle, True)
                    If Not ValidateHandle(safeHandle, methodName) Then
                        Exit Sub
                    End If
                    If terminationMethod(safeHandle) Then
                        promptMessage = $"{methodName}: Notepad process terminated successfully."
                    End If
                End Using
            Catch ex As Exception
                _userPrompter.Prompt($"{methodName}: An error occurred: {ex.Message}")
            End Try
            _userPrompter.Prompt(promptMessage)
        End Sub

        ''' <summary>
        ''' Retrieves the process handle for Notepad.
        ''' </summary>
        ''' <param name="methodName">The name of the termination method for logging purposes.</param>
        ''' <returns>The process handle for Notepad, or IntPtr.Zero if not found.</returns>
        Private Function GetProcessHandle(methodName As String) As IntPtr
            Dim processHandle As IntPtr = ProcessUtility.GetNotepadHandle()
            If Equals(processHandle, NativeMethods.NullHandleValue) Then
                _userPrompter.Prompt($"{methodName}: No running Notepad process found.")
                Return IntPtr.Zero
            End If
            Return processHandle
        End Function

        ''' <summary>
        ''' Validates the provided SafeProcessHandle.
        ''' </summary>
        ''' <param name="safeHandle">The SafeProcessHandle to validate.</param>
        ''' <param name="methodName">The name of the termination method for logging purposes.</param>
        ''' <returns>True if the handle is valid; otherwise, false.</returns>
        Private Function ValidateHandle(safeHandle As SafeProcessHandle, methodName As String) As Boolean
            If safeHandle.IsInvalid Then
                _userPrompter.Prompt($"{methodName}: Retrieved an invalid process handle.")
                Return False
            End If
            Return True
        End Function
    End Class
End Namespace
