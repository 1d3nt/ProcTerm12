Namespace CoreServices.WindowsApiInterop.Structs

    ''' <summary>
    ''' Contains information about a module in a process.
    ''' </summary>
    ''' <remarks>
    ''' This structure corresponds to the <c>MODULEENTRY32</c> structure in the Windows API.
    ''' <list type="bullet">
    '''     <item><description><see cref="ModuleEntry32.Size"/>: The size of the structure, in bytes. In C++: <code>DWORD dwSize;</code></description></item>
    '''     <item><description><see cref="ModuleEntry32.ModuleID"/>: The module ID for the module. In C++: <code>DWORD th32ModuleID;</code></description></item>
    '''     <item><description><see cref="ModuleEntry32.ProcessID"/>: The process ID for the module. In C++: <code>DWORD th32ProcessID;</code></description></item>
    '''     <item><description><see cref="ModuleEntry32.GlobalUsageCount"/>: The global usage count of the module. In C++: <code>DWORD GlblcntUsage;</code></description></item>
    '''     <item><description><see cref="ModuleEntry32.ProcessUsageCount"/>: The process usage count of the module. In C++: <code>DWORD ProccntUsage;</code></description></item>
    '''     <item><description><see cref="ModuleEntry32.BaseAddress"/>: The base address of the module in the process's address space. In C++: <code>BYTE* modBaseAddr;</code></description></item>
    '''     <item><description><see cref="ModuleEntry32.ModuleSize"/>: The size of the module, in bytes. In C++: <code>DWORD modBaseSize;</code></description></item>
    '''     <item><description><see cref="ModuleEntry32.ModuleHandle"/>: The handle to the module. In C++: <code>HMODULE hModule;</code></description></item>
    '''     <item><description><see cref="ModuleEntry32.ModuleName"/>: The name of the module. In C++: <code>char szModule[MAX_MODULE_NAME32 + 1];</code></description></item>
    '''     <item><description><see cref="ModuleEntry32.ExePath"/>: The path of the executable file for the module. In C++: <code>char szExePath[MAX_PATH];</code></description></item>
    ''' </list>
    ''' 
    ''' For further details, see:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/ns-tlhelp32-moduleentry32">MODULEENTRY32 Structure</see>.
    ''' 
    ''' <example>
    ''' In C++:
    ''' <code>
    ''' typedef struct tagMODULEENTRY32 {
    '''     DWORD   dwSize;
    '''     DWORD   th32ModuleID;
    '''     DWORD   th32ProcessID;
    '''     DWORD   GlblcntUsage;
    '''     DWORD   ProccntUsage;
    '''     BYTE*   modBaseAddr;
    '''     DWORD   modBaseSize;
    '''     HMODULE hModule;
    '''     char    szModule[MAX_MODULE_NAME32 + 1];
    '''     char    szExePath[MAX_PATH];
    ''' } MODULEENTRY32;
    ''' </code>
    ''' </example>
    ''' </remarks>
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Structure ModuleEntry32

        ''' <summary>
        ''' The size of the structure, in bytes.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>DWORD dwSize;</code>
        ''' </remarks>
        Friend dwSize As Integer

        ''' <summary>
        ''' The module ID for the module.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>DWORD th32ModuleID;</code>
        ''' </remarks>
        Friend th32ModuleID As UInteger

        ''' <summary>
        ''' The process ID for the module.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>DWORD th32ProcessID;</code>
        ''' </remarks>
        Friend th32ProcessID As UInteger

        ''' <summary>
        ''' The global usage count of the module.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>DWORD GlblcntUsage;</code>
        ''' </remarks>
        Friend GlblcntUsage As UInteger

        ''' <summary>
        ''' The process usage count of the module.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>DWORD ProccntUsage;</code>
        ''' </remarks>
        Friend ProccntUsage As UInteger

        ''' <summary>
        ''' The base address of the module in the process's address space.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>BYTE* modBaseAddr;</code>
        ''' </remarks>
        Friend modBaseAddr As IntPtr

        ''' <summary>
        ''' The size of the module, in bytes.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>DWORD modBaseSize;</code>
        ''' </remarks>
        Friend modBaseSize As UInteger

        ''' <summary>
        ''' The handle to the module.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>HMODULE hModule;</code>
        ''' </remarks>
        Friend hModule As IntPtr

        ''' <summary>
        ''' The name of the module, represented as a fixed-length string.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>char szModule[MAX_MODULE_NAME32 + 1];</code>
        ''' The <see cref="MarshalAsAttribute"/> attribute is used with <see cref="UnmanagedType.ByValTStr"/> to represent
        ''' a fixed-length string that is null-terminated. The length is specified by the constant <c>MAX_MODULE_NAME32 + 1</c>,
        ''' which corresponds to <c>256</c> in this case.
        ''' </remarks>
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=256)>
        Friend szModule As String

        ''' <summary>
        ''' The path of the executable file for the module, represented as a fixed-length string.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>char szExePath[MAX_PATH];</code>
        ''' The <see cref="MarshalAsAttribute"/> attribute is used with <see cref="UnmanagedType.ByValTStr"/> to define
        ''' a null-terminated string with a fixed length specified by the constant <c>MAX_PATH</c>, which is <c>260</c>.
        ''' </remarks>
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)>
        Friend szExePath As String
    End Structure
End Namespace
