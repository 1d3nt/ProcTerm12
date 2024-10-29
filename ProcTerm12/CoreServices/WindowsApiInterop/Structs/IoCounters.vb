Namespace CoreServices.WindowsApiInterop.Structs

    ''' <summary>
    ''' Contains I/O accounting information for a process or a job object. For a job object,
    ''' the counters include all operations performed by all processes that have ever been associated with the job,
    ''' in addition to all processes currently associated with the job.
    ''' </summary>
    ''' <remarks>
    ''' This structure corresponds to the <c>IO_COUNTERS</c> structure in the Windows API:
    ''' <list type="bullet">
    ''' <item><description><see cref="IoCounters.ReadOperationCount"/>: The number of read operations performed. In C++: <code>ULONGLONG ReadOperationCount;</code></description></item>
    ''' <item><description><see cref="IoCounters.WriteOperationCount"/>: The number of write operations performed. In C++: <code>ULONGLONG WriteOperationCount;</code></description></item>
    ''' <item><description><see cref="IoCounters.OtherOperationCount"/>: The number of other operations performed (neither read nor write). In C++: <code>ULONGLONG OtherOperationCount;</code></description></item>
    ''' <item><description><see cref="IoCounters.ReadTransferCount"/>: The total number of bytes read. In C++: <code>ULONGLONG ReadTransferCount;</code></description></item>
    ''' <item><description><see cref="IoCounters.WriteTransferCount"/>: The total number of bytes written. In C++: <code>ULONGLONG WriteTransferCount;</code></description></item>
    ''' <item><description><see cref="IoCounters.OtherTransferCount"/>: The total number of bytes transferred for other operations (neither read nor write). In C++: <code>ULONGLONG OtherTransferCount;</code></description></item>
    ''' </list>
    ''' 
    ''' For additional details on the <see cref="IoCounters"/> structure, refer to:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-io_counters">IO_COUNTERS Structure</see>.
    ''' 
    ''' <example>
    ''' In C++:
    ''' <code>
    ''' typedef struct _IO_COUNTERS {
    '''     ULONGLONG ReadOperationCount;
    '''     ULONGLONG WriteOperationCount;
    '''     ULONGLONG OtherOperationCount;
    '''     ULONGLONG ReadTransferCount;
    '''     ULONGLONG WriteTransferCount;
    '''     ULONGLONG OtherTransferCount;
    ''' } IO_COUNTERS;
    ''' </code>
    ''' </example>
    ''' </remarks>
    <StructLayout(LayoutKind.Sequential)>
    Friend Structure IoCounters

        ''' <summary>
        ''' The number of read operations performed.
        ''' </summary>
        ''' <remarks>
        ''' This member corresponds to <code>ULONGLONG ReadOperationCount;</code> in C++.
        ''' </remarks>
        Friend ReadOperationCount As UInt64

        ''' <summary>
        ''' The number of write operations performed.
        ''' </summary>
        ''' <remarks>
        ''' This member corresponds to <code>ULONGLONG WriteOperationCount;</code> in C++.
        ''' </remarks>
        Friend WriteOperationCount As UInt64

        ''' <summary>
        ''' The number of other operations performed (neither read nor write).
        ''' </summary>
        ''' <remarks>
        ''' This member corresponds to <code>ULONGLONG OtherOperationCount;</code> in C++.
        ''' </remarks>
        Friend OtherOperationCount As UInt64

        ''' <summary>
        ''' The total number of bytes read.
        ''' </summary>
        ''' <remarks>
        ''' This member corresponds to <code>ULONGLONG ReadTransferCount;</code> in C++.
        ''' </remarks>
        Friend ReadTransferCount As UInt64

        ''' <summary>
        ''' The total number of bytes written.
        ''' </summary>
        ''' <remarks>
        ''' This member corresponds to <code>ULONGLONG WriteTransferCount;</code> in C++.
        ''' </remarks>
        Friend WriteTransferCount As UInt64

        ''' <summary>
        ''' The total number of bytes transferred for other operations (neither read nor write).
        ''' </summary>
        ''' <remarks>
        ''' This member corresponds to <code>ULONGLONG OtherTransferCount;</code> in C++.
        ''' </remarks>
        Friend OtherTransferCount As UInt64
    End Structure
End Namespace
