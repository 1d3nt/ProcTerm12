Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods.Interfaces

    ''' <summary>
    ''' Defines the contract for terminating processes using various methods.
    ''' </summary>
    Public Interface IProcessTerminator

        ''' <summary>
        ''' Handles the termination of a process using the NtTerminateProcess method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Function NtTerminateProcessHandler() As Task

        ''' <summary>
        ''' Handles the termination of a process by terminating each thread using the TerminateThread method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Function TerminateThreadHandler() As Task

        ''' <summary>
        ''' Handles the termination of a process using CreateRemoteThread to execute ExitProcess.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Function CreateRemoteThreadExitProcessHandler() As Task
    End Interface
End Namespace
