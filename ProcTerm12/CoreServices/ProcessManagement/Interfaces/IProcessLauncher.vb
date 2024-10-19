Namespace CoreServices.ProcessManagement.Interfaces

    ''' <summary>
    ''' Defines a contract for launching processes.
    ''' </summary>
    ''' <remarks>
    ''' Implementations of this interface should provide the logic to launch a process specified by a string.
    ''' This interface serves as a contract that enforces the implementation of the <see cref="IProcessLauncher.LaunchProcess"/> method.
    ''' </remarks>
    Public Interface IProcessLauncher

        ''' <summary>
        ''' Launches Notepad.
        ''' </summary>
        ''' <remarks>
        ''' This method encapsulates the logic required to start Notepad.
        ''' For testing purposes, a hardcoded path to Notepad is used.
        ''' </remarks>
        Sub LaunchProcess()
    End Interface
End Namespace
