Namespace CoreServices.WindowsApiInterop.Structs

    ''' <summary>
    ''' Represents a client identifier (ClientId) used to uniquely identify a process and optionally a thread within that process.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="ClientId"/> structure contains the unique identifiers for both a process and a thread, allowing specific targeting 
    ''' of these entities within system calls. It is commonly used in Windows API functions to specify the target process and thread.
    ''' Although there is no direct MSDN documentation for this structure, it is often referenced in conjunction with the 
    ''' <a href="https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntddk/nf-ntddk-ntopenprocess">NtOpenProcess</a> function and others that require client identification.
    ''' </remarks>
    <StructLayout(LayoutKind.Sequential)>
    Friend Structure ClientId

        ''' <summary>
        ''' A handle that uniquely identifies the process.
        ''' </summary>
        Friend UniqueProcess As IntPtr

        ''' <summary>
        ''' A handle that uniquely identifies the thread within the specified process.
        ''' </summary>
        Friend UniqueThread As IntPtr

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ClientId"/> structure with specific <see cref="IntPtr"/> process and thread identifiers.
        ''' </summary>
        ''' <param name="processId">An <see cref="IntPtr"/> value that uniquely identifies the process.</param>
        ''' <param name="threadId">An <see cref="IntPtr"/> value that uniquely identifies the thread within the process.</param>
        Friend Sub New(processId As IntPtr, threadId As IntPtr)
            UniqueProcess = processId
            UniqueThread = threadId
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ClientId"/> structure with specific <see cref="Int32"/> process and thread identifiers.
        ''' </summary>
        ''' <param name="processId">An <see cref="Int32"/> value representing the unique identifier of the process.</param>
        ''' <param name="threadId">An <see cref="Int32"/> value representing the unique identifier of the thread within the process.</param>
        Friend Sub New(processId As Integer, threadId As Integer)
            UniqueProcess = New IntPtr(processId)
            UniqueThread = New IntPtr(threadId)
        End Sub
    End Structure
End Namespace
