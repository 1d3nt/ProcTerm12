Namespace CoreServices.WindowsApiInterop.Structs

    ''' <summary>
    ''' Describes an entry from a list of the threads executing in the system when a snapshot was taken.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="ThreadEntry32"/> structure is used in conjunction with the 
    ''' <c>CreateToolhelp32Snapshot</c> function to retrieve information about threads 
    ''' currently executing in the system.
    ''' 
    ''' This structure corresponds to the <see cref="THREADENTRY32"/> structure in the Windows API:
    ''' <list type="bullet">
    ''' <item><description><see cref="ThreadEntry32.dwSize"/>: The size of the structure. In C++: <code>DWORD dwSize;</code></description></item>
    ''' <item><description><see cref="ThreadEntry32.cntUsage"/>: Reserved; do not use. In C++: <code>DWORD cntUsage;</code></description></item>
    ''' <item><description><see cref="ThreadEntry32.th32ThreadID"/>: The identifier of the thread. In C++: <code>DWORD th32ThreadID;</code></description></item>
    ''' <item><description><see cref="ThreadEntry32.th32OwnerProcessID"/>: The identifier of the process to which the thread belongs. In C++: <code>DWORD th32OwnerProcessID;</code></description></item>
    ''' <item><description><see cref="ThreadEntry32.tpBasePri"/>: The base priority of the thread. In C++: <code>LONG tpBasePri;</code></description></item>
    ''' <item><description><see cref="ThreadEntry32.tpDeltaPri"/>: The thread's priority delta value. In C++: <code>LONG tpDeltaPri;</code></description></item>
    ''' <item><description><see cref="ThreadEntry32.dwFlags"/>: Reserved; do not use. In C++: <code>DWORD dwFlags;</code></description></item>
    ''' </list>
    ''' 
    ''' For additional details on the <see cref="ThreadEntry32"/> structure, refer to:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/ns-tlhelp32-threadentry32">THREADENTRY32 Structure</see>.
    ''' </remarks>
    <StructLayout(LayoutKind.Sequential)>
    Friend Class ThreadEntry32

        ''' <summary>
        ''' The size of the structure.
        ''' </summary>
        ''' <remarks>
        ''' The <code>dwSize</code> member is initialized to the size of the structure before calling 
        ''' <c>Thread32First</c> or <c>Thread32Next</c>.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>DWORD dwSize;</code>
        ''' </example>
        Friend dwSize As UInteger

        ''' <summary>
        ''' Reserved; do not use.
        ''' </summary>
        ''' <remarks>
        ''' This member is reserved for future use and should not be referenced.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>DWORD cntUsage;</code>
        ''' </example>
        Friend cntUsage As UInteger

        ''' <summary>
        ''' The identifier of the thread.
        ''' </summary>
        ''' <remarks>
        ''' The <code>th32ThreadID</code> member is the unique identifier assigned to the thread 
        ''' by the operating system.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>DWORD th32ThreadID;</code>
        ''' </example>
        Friend th32ThreadID As UInteger

        ''' <summary>
        ''' The identifier of the process to which the thread belongs.
        ''' </summary>
        ''' <remarks>
        ''' The <code>th32OwnerProcessID</code> member is the unique identifier of the process that 
        ''' the thread is executing in.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>DWORD th32OwnerProcessID;</code>
        ''' </example>
        Friend th32OwnerProcessID As UInteger

        ''' <summary>
        ''' The base priority of the thread.
        ''' </summary>
        ''' <remarks>
        ''' The <code>tpBasePri</code> member specifies the base priority level of the thread.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>LONG tpBasePri;</code>
        ''' </example>
        Friend tpBasePri As Integer

        ''' <summary>
        ''' The thread's priority delta value.
        ''' </summary>
        ''' <remarks>
        ''' The <code>tpDeltaPri</code> member indicates the difference between the thread's base 
        ''' priority and the priority level of its associated process.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>LONG tpDeltaPri;</code>
        ''' </example>
        Friend tpDeltaPri As Integer

        ''' <summary>
        ''' Reserved; do not use.
        ''' </summary>
        ''' <remarks>
        ''' This member is reserved for future use and should not be referenced.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>DWORD dwFlags;</code>
        ''' </example>
        Friend dwFlags As UInteger
    End Class
End Namespace
