Namespace CoreServices.WindowsApiInterop.Structs

    ''' <summary>
    ''' Contains information about a range of pages in the virtual address space of a process.
    ''' </summary>
    ''' <remarks>
    ''' This structure corresponds to the <c>MEMORY_BASIC_INFORMATION</c> structure in the Windows API.
    ''' <list type="bullet">
    '''     <item><description><see cref="MemoryBasicInformation.BaseAddress"/>: The base address of the region of pages. In C++: <code>LPVOID BaseAddress;</code></description></item>
    '''     <item><description><see cref="MemoryBasicInformation.AllocationBase"/>: The base address of the allocated region of pages. In C++: <code>LPVOID AllocationBase;</code></description></item>
    '''     <item><description><see cref="MemoryBasicInformation.AllocationProtect"/>: The memory protection option for the region of pages. In C++: <code>DWORD AllocationProtect;</code></description></item>
    '''     <item><description><see cref="MemoryBasicInformation.RegionSize"/>: The size of the region of pages, in bytes. In C++: <code>SIZE_T RegionSize;</code></description></item>
    '''     <item><description><see cref="MemoryBasicInformation.State"/>: The state of the pages in the region. In C++: <code>DWORD State;</code></description></item>
    '''     <item><description><see cref="MemoryBasicInformation.Protect"/>: The access protection for the region of pages. In C++: <code>DWORD Protect;</code></description></item>
    '''     <item><description><see cref="MemoryBasicInformation.Type"/>: The type of pages in the region. In C++: <code>DWORD Type;</code></description></item>
    ''' </list>
    ''' 
    ''' For further details, see:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-memory_basic_information">MEMORY_BASIC_INFORMATION Structure</see>.
    ''' 
    ''' <example>
    ''' In C++:
    ''' <code>
    ''' typedef struct _MEMORY_BASIC_INFORMATION {
    '''     PVOID BaseAddress;
    '''     PVOID AllocationBase;
    '''     DWORD AllocationProtect;
    '''     SIZE_T RegionSize;
    '''     DWORD State;
    '''     DWORD Protect;
    '''     DWORD Type;
    ''' } MEMORY_BASIC_INFORMATION, *PMEMORY_BASIC_INFORMATION;
    ''' </code>
    ''' </example>
    ''' </remarks>
    <StructLayout(LayoutKind.Sequential)>
    Friend Structure MemoryBasicInformation

        ''' <summary>
        ''' The base address of the region of pages.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>LPVOID BaseAddress;</code>
        ''' </remarks>
        Friend BaseAddress As IntPtr

        ''' <summary>
        ''' The base address of the allocated region of pages.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>LPVOID AllocationBase;</code>
        ''' </remarks>
        Friend AllocationBase As IntPtr

        ''' <summary>
        ''' The memory protection option for the region of pages.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>DWORD AllocationProtect;</code>
        ''' </remarks>
        Friend AllocationProtect As UInteger

        ''' <summary>
        ''' The size of the region of pages, in bytes.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>SIZE_T RegionSize;</code>
        ''' </remarks>
        Friend RegionSize As UInteger

        ''' <summary>
        ''' The state of the pages in the region.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>DWORD State;</code>
        ''' </remarks>
        Friend State As UInteger

        ''' <summary>
        ''' The access protection for the region of pages.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>DWORD Protect;</code>
        ''' </remarks>
        Friend Protect As UInteger

        ''' <summary>
        ''' The type of pages in the region.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>DWORD Type;</code>
        ''' </remarks>
        Friend Type As UInteger
    End Structure
End Namespace
