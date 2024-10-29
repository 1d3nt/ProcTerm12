Namespace CoreServices.WindowsApiInterop.Enums

    ''' <summary>
    ''' Defines the various methods for terminating a process.
    ''' </summary>
    Public Enum TerminationMethods

        ''' <summary>
        ''' Terminates a process using the NtTerminateProcess native API.
        ''' </summary>
        NtTerminateProcess = 1

        ''' <summary>
        ''' Creates a remote thread in the target process to execute ExitProcess.
        ''' </summary>
        CreateRemoteThreadExitProcess = 2

        ''' <summary>
        ''' Terminates each thread of the target process using TerminateThread.
        ''' </summary>
        TerminateThread = 3

        ''' <summary>
        ''' Terminates threads by modifying their context to point to ExitProcess.
        ''' </summary>
        SetThreadContext = 4

        ''' <summary>
        ''' Closes handles of the target process to invalidate resources.
        ''' </summary>
        DuplicateHandle = 5

        ''' <summary>
        ''' Uses job object methods to terminate the target process.
        ''' </summary>
        JobObjectMethods = 6

        ''' <summary>
        ''' Uses a debug object to attach to a process and terminate it.
        ''' </summary>
        DebugObjectMethods = 7

        ''' <summary>
        ''' Modifies memory protections to PAGE_NOACCESS to crash the process.
        ''' </summary>
        VirtualQueryExNoAccess = 8

        ''' <summary>
        ''' Overwrites memory regions with random data to cause instability.
        ''' </summary>
        WriteProcessMemory = 9

        ''' <summary>
        ''' Continuously allocates memory in the target process until it fails.
        ''' </summary>
        VirtualAllocEx = 10

        ''' <summary>
        ''' Terminates a process using the internal PsTerminateProcess kernel function.
        ''' </summary>
        PsTerminateProcess = 11

        ''' <summary>
        ''' Terminates threads using the non-exported PspTerminateThreadByPointer function.
        ''' </summary>
        PspTerminateThreadByPointer = 12
    End Enum
End Namespace
