Namespace CoreServices.WindowsApiInterop.Enums

    ''' <summary>
    ''' Specifies memory protection constants that can be applied when allocating or modifying memory pages.
    ''' These constants define the access permissions for the memory region.
    ''' </summary>
    ''' <remarks>
    ''' This enumeration corresponds to the <c>MEMORY_PROTECTION_CONSTANTS</c> used in the Windows API to manage memory protection.
    ''' <list type="bullet">
    '''     <item><description><see cref="MemoryProtection.PageNoAccess"/> corresponds to <c>PAGE_NOACCESS</c>.</description></item>
    '''     <item><description><see cref="MemoryProtection.PageExecute"/> corresponds to <c>PAGE_EXECUTE</c>.</description></item>
    '''     <item><description><see cref="MemoryProtection.PageExecuteRead"/> corresponds to <c>PAGE_EXECUTE_READ</c>.</description></item>
    '''     <item><description><see cref="MemoryProtection.PageExecuteReadWrite"/> corresponds to <c>PAGE_EXECUTE_READWRITE</c>.</description></item>
    '''     <item><description><see cref="MemoryProtection.PageExecuteWriteCopy"/> corresponds to <c>PAGE_EXECUTE_WRITE_COPY</c>.</description></item>
    '''     <item><description><see cref="MemoryProtection.PageReadOnly"/> corresponds to <c>PAGE_READONLY</c>.</description></item>
    '''     <item><description><see cref="MemoryProtection.PageReadWrite"/> corresponds to <c>PAGE_READWRITE</c>.</description></item>
    '''     <item><description><see cref="MemoryProtection.PageWriteCopy"/> corresponds to <c>PAGE_WRITECOPY</c>.</description></item>
    '''     <item><description><see cref="MemoryProtection.PageGuard"/> corresponds to <c>PAGE_GUARD</c>.</description></item>
    '''     <item><description><see cref="MemoryProtection.PageNoCache"/> corresponds to <c>PAGE_NOCACHE</c>.</description></item>
    '''     <item><description><see cref="MemoryProtection.PageWriteCombine"/> corresponds to <c>PAGE_WRITECOMBINE</c>.</description></item>
    ''' </list>
    ''' 
    ''' For further details, see:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/memory/memory-protection-constants">Memory Protection Constants</see>.
    ''' 
    ''' <example>
    ''' In C++:
    ''' <code>
    ''' typedef enum _MEMORY_PROTECTION_CONSTANTS {
    '''     PAGE_NOACCESS = 0x01,
    '''     PAGE_READONLY = 0x02,
    '''     PAGE_READWRITE = 0x04,
    '''     PAGE_WRITECOPY = 0x08,
    '''     PAGE_EXECUTE = 0x10,
    '''     PAGE_EXECUTE_READ = 0x20,
    '''     PAGE_EXECUTE_READWRITE = 0x40,
    '''     PAGE_EXECUTE_WRITE_COPY = 0x80,
    '''     PAGE_GUARD = 0x100,
    '''     PAGE_NOCACHE = 0x200,
    '''     PAGE_WRITECOMBINE = 0x400
    ''' } MEMORY_PROTECTION_CONSTANTS;
    ''' </code>
    ''' </example>
    ''' </remarks>
    Friend Enum MemoryProtection As UInteger

        ''' <summary>
        ''' Indicates that there is no access to the memory.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define PAGE_NOACCESS 0x01</code>
        ''' </remarks>
        PageNoAccess = &H1

        ''' <summary>
        ''' Indicates that the memory can be executed.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define PAGE_EXECUTE 0x10</code>
        ''' </remarks>
        PageExecute = &H10

        ''' <summary>
        ''' Indicates that the memory can be executed and read.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define PAGE_EXECUTE_READ 0x20</code>
        ''' </remarks>
        PageExecuteRead = &H20

        ''' <summary>
        ''' Indicates that the memory can be executed, read, and written.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define PAGE_EXECUTE_READWRITE 0x40</code>
        ''' </remarks>
        PageExecuteReadWrite = &H40

        ''' <summary>
        ''' Indicates that the memory can be executed, read, written, and copied.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define PAGE_EXECUTE_WRITE_COPY 0x80</code>
        ''' </remarks>
        PageExecuteWriteCopy = &H80

        ''' <summary>
        ''' Indicates that the memory is read-only.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define PAGE_READONLY 0x02</code>
        ''' </remarks>
        PageReadOnly = &H2

        ''' <summary>
        ''' Indicates that the memory can be read and written.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define PAGE_READWRITE 0x04</code>
        ''' </remarks>
        PageReadWrite = &H4

        ''' <summary>
        ''' Indicates that the memory can be written to and copied.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define PAGE_WRITECOPY 0x08</code>
        ''' </remarks>
        PageWriteCopy = &H8

        ''' <summary>
        ''' Indicates that the memory is guarded.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define PAGE_GUARD 0x100</code>
        ''' </remarks>
        PageGuard = &H100

        ''' <summary>
        ''' Indicates that the memory is not cached.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define PAGE_NOCACHE 0x200</code>
        ''' </remarks>
        PageNoCache = &H200

        ''' <summary>
        ''' Indicates that the memory can be written to and combined.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define PAGE_WRITECOMBINE 0x400</code>
        ''' </remarks>
        PageWriteCombine = &H400
    End Enum
End Namespace
