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

        ''' <summary>
        ''' Handles the termination of a process by duplicating the process handle and performing 
        ''' cleanup operations, such as closing or modifying the duplicated handle to achieve termination.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Function DuplicateHandleHandler() As Task

        ''' <summary>
        ''' Handles the termination of a process by creating a job object and assigning the process to it.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Function JobCreateTerminatorHandler() As Task

        ''' <summary>
        ''' Handles the termination of a process using the SetThreadContext method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Function SetThreadContextHandler() As Task

        ''' <summary>
        ''' Handles the termination of a process using the DebugObjectMethods method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Function DebugObjectMethodsHandler() As Task

        ''' <summary>
        ''' Handles the termination of a process using the VirtualQueryExNoAccess method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Function VirtualQueryExNoAccessHandler() As Task

        ''' <summary>
        ''' Handles the termination of a process using the WriteProcessMemory method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Function WriteProcessMemoryHandler() As Task

        ''' <summary>
        ''' Handles the termination of a process using the VirtualAllocEx method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Function VirtualAllocExHandler() As Task

        ''' <summary>
        ''' Handles the termination of a process using the PsTerminateProcess method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Function PsTerminateProcessHandler() As Task

        ''' <summary>
        ''' Handles the termination of a process using the PspTerminateThreadByPointer method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Function PspTerminateThreadByPointerHandler() As Task
    End Interface
End Namespace
