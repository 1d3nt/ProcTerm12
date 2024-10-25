Namespace CoreServices.WindowsApiInterop.Methods

    ''' <summary>
    ''' Provides methods for interacting with native Windows APIs. 
    ''' This class contains P/Invoke declarations for various functions used for process and token management.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="NativeMethods"/> class uses the <c>DllImport</c> attribute to define methods that are imported 
    ''' from unmanaged DLLs. These methods are used to interact with the Windows operating system at a low level.
    ''' 
    ''' The <c>SuppressUnmanagedCodeSecurity</c> attribute is applied to this class to improve performance when
    ''' calling unmanaged code. This attribute disables code access security checks for unmanaged code, which
    ''' can reduce overhead in performance-critical applications. Use this attribute with caution, as it bypasses
    ''' some of the security measures provided by the .NET runtime.
    ''' </remarks>
    <SuppressUnmanagedCodeSecurity>
    Friend NotInheritable Class NativeMethods

        ''' <summary>
        ''' Represents a null handle value used in P/Invoke calls.
        ''' </summary>
        ''' <remarks>
        ''' This field is used to represent a null handle (<see cref="IntPtr.Zero"/>) in P/Invoke calls to unmanaged code.
        ''' </remarks>
        Friend Shared ReadOnly NullHandleValue As IntPtr = IntPtr.Zero

        ''' <summary>
        ''' Specifies the access right to terminate a thread.
        ''' </summary>
        ''' <remarks>
        ''' This constant is used with thread-related functions to specify the right to terminate a thread.
        ''' For more information, see <see cref="TerminateThread"/>.
        ''' </remarks>
        Friend Const ThreadTerminate As Integer = &H1

        ''' <summary>
        ''' Includes all threads in the system in a snapshot.
        ''' </summary>
        ''' <remarks>
        ''' This constant is used with functions that take system snapshots to include all threads.
        ''' To enumerate the threads, see <see cref="CreateToolhelp32Snapshot"/>.
        ''' </remarks>
        Friend Const Th32CsSnapThread As Integer = &H4

        ''' <summary>
        ''' Allocates physical storage in memory for a specified region of pages.
        ''' </summary>
        Friend Const MemCommit As UInteger = &H1000

        ''' <summary>
        ''' Reserves a range of the process's virtual address space without allocating physical storage.
        ''' </summary>
        Friend Const MemReserve As UInteger = &H2000

        ''' <summary>
        ''' Enables execute, read, and write access to the committed region of pages.
        ''' </summary>
        Friend Const PageExecuteReadWrite As UInteger = &H40

        ''' <summary>
        ''' Decommits the specified region of committed pages, releasing the physical storage.
        ''' </summary>
        Friend Const MemRelease As UInteger = &H8000

        ''' <summary>
        ''' Allocates all available address space, up to the maximum, in the process's virtual memory.
        ''' </summary>
        Friend Const MemInfinite As UInteger = &HFFFFFFFFUI

        ''' <summary>
        ''' Closes an open object handle.
        ''' </summary>
        ''' <param name="hObject">
        ''' A valid handle to an open object. This handle is typically obtained from functions like <c>CreateFile</c>,
        ''' <c>OpenProcess</c>, or <c>OpenThread</c>. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is nonzero (<c>True</c>). If the function fails, the return value is
        ''' zero (<c>False</c>). To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.
        ''' </returns>
        ''' <remarks>
        ''' The <c>CloseHandle</c> function is used to close an open handle to an object. It is crucial to call this function
        ''' to free system resources when a handle is no longer needed.
        ''' 
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/desktop/api/handleapi/nf-handleapi-closehandle">CloseHandle</see> documentation.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' BOOL CloseHandle(
        '''   [in] HANDLE hObject
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function CloseHandle(
            <[In]> hObject As IntPtr
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Terminates the specified process and all of its threads.
        ''' </summary>
        ''' <param name="hProcess">
        ''' A handle to the process to be terminated. The handle must have the PROCESS_TERMINATE access right. 
        ''' For more information, see Process Security and Access Rights.
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms684880(v=vs.85).aspx">MSDN Documentation</see>.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="uExitCode">
        ''' The exit code to be used by the process and threads terminated as a result of this call. Use the
        ''' GetExitCodeProcess function to retrieve a process's exit value. Use the GetExitCodeThread function 
        ''' to retrieve a thread's exit value.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. 
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://docs.microsoft.com/en-us/windows/desktop/api/processthreadsapi/nf-processthreadsapi-terminateprocess">TerminateProcess Documentation</see>.
        ''' The function signature in C++ is:
        ''' <code>
        ''' BOOL TerminateProcess(
        '''   [in] HANDLE hProcess,
        '''   [in] UINT uExitCode
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function TerminateProcess(
            <[In]> hProcess As IntPtr,
            <[In]> uExitCode As UInteger
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Takes a snapshot of the specified processes, as well as the heaps, modules, and threads used by these processes.
        ''' </summary>
        ''' <param name="dwFlags">
        ''' The portions of the system to be included in the snapshot. This parameter can be one or more of the following values.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="th32ProcessId">
        ''' The process identifier of the process to be included in the snapshot. This parameter can be zero to indicate the current process.
        ''' This parameter is used when the TH32CS_SNAPHEAPLIST, TH32CS_SNAPMODULE, TH32CS_SNAPMODULE32, or TH32CS_SNAPALL value is specified.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' A handle to the snapshot of the specified processes, heaps, modules, and threads. If the function fails, the return value is <c>InvalidHandleValue</c>.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://docs.microsoft.com/en-gb/windows/desktop/api/tlhelp32/nf-tlhelp32-createtoolhelp32snapshot">CreateToolhelp32Snapshot</see> documentation.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' HANDLE CreateToolhelp32Snapshot(
        '''   [in] DWORD dwFlags,
        '''   [in] DWORD th32ProcessID
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function CreateToolhelp32Snapshot(
            <[In]> dwFlags As UInteger,
            <[In]> th32ProcessId As UInteger
        ) As IntPtr
        End Function

        ''' <summary>
        ''' Terminates a thread.
        ''' </summary>
        ''' <param name="hThread">
        ''' A handle to the thread to be terminated. This parameter is passed with the <c>[In, Out]</c> attribute.
        ''' </param>
        ''' <param name="dwExitCode">
        ''' The exit code for the thread. Use the GetExitCodeThread function to retrieve a thread's exit value. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is nonzero (<c>True</c>). If the function fails, the return value is zero (<c>False</c>).
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-terminatethread">TerminateThread</see> documentation.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' BOOL TerminateThread(
        '''   [in, out] HANDLE hThread,
        '''   [in] DWORD dwExitCode
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function TerminateThread(
            <[In], Out> hThread As IntPtr,
            <[In]> dwExitCode As UInteger
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Retrieves information about the first thread of any process encountered in a system snapshot.
        ''' </summary>
        ''' <param name="hSnapshot">
        ''' A handle to the snapshot returned from a previous call to the <c>CreateToolhelp32Snapshot</c> function.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpte">
        ''' A pointer to a <see cref="ThreadEntry32"/> structure that receives the thread information.
        ''' This parameter is passed with the <c>[In, Out]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' Returns <c>TRUE</c> if the first entry of the thread list has been copied to the buffer; 
        ''' otherwise, returns <c>FALSE</c>.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-thread32first">Thread32First function</see> for more information.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' BOOL Thread32First(
        '''   [in]      HANDLE          hSnapshot,
        '''   [in, out] LPTHREADENTRY32 lpte
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function Thread32First(
            <[In]> hSnapshot As IntPtr,
            <[In], Out> lpte As ThreadEntry32
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Retrieves information about the next thread of any process encountered in the system memory snapshot.
        ''' </summary>
        ''' <param name="hSnapshot">
        ''' A handle to the snapshot returned from a previous call to the CreateToolhelp32Snapshot function.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpte">
        ''' A pointer to a <see cref="ThreadEntry32"/> structure that receives information about the thread.
        ''' This parameter is passed with the <c>[Out]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' Returns <c>TRUE</c> if the next entry of the thread list has been copied to the buffer; 
        ''' otherwise, returns <c>FALSE</c>.
        ''' </returns>
        ''' <remarks>
        '''VFor more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-thread32next">Thread32Next documentation</see> for more information.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' BOOL Thread32Next(
        '''   [in]  HANDLE          hSnapshot,
        '''   [out] LPTHREADENTRY32 lpte
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function Thread32Next(
            <[In]> hSnapshot As IntPtr,
            <[Out]> lpte As ThreadEntry32
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Opens an existing thread object.
        ''' </summary>
        ''' <param name="dwDesiredAccess">
        ''' The access to the thread object. This access right is checked against the security descriptor for the thread.
        ''' This parameter can be one or more of the thread access rights.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="bInheritHandle">
        ''' If this value is <c>TRUE</c>, processes created by this process will inherit the handle. 
        ''' Otherwise, the processes do not inherit this handle.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="dwThreadId">
        ''' The identifier of the thread to be opened.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is an open handle to the specified thread. 
        ''' If the function fails, the return value is <c>NULL</c>.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-openthread">OpenThread documentation</see> for more information.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' HANDLE OpenThread(
        '''   [in] DWORD dwDesiredAccess,
        '''   [in] BOOL  bInheritHandle,
        '''   [in] DWORD dwThreadId
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function OpenThread(
            <[In]> dwDesiredAccess As UInteger,
            <[In]> bInheritHandle As Boolean,
            <[In]> dwThreadId As UInteger
        ) As IntPtr
        End Function

        ''' <summary>
        ''' Retrieves the process identifier (PID) for the specified thread handle.
        ''' </summary>
        ''' <param name="hThread">
        ''' A handle to the thread for which the process identifier is to be retrieved.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' The process identifier for the specified thread. If the function fails, the return value is zero.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getprocessid">GetProcessId documentation</see> for more details.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' DWORD GetProcessId(
        '''   [in] HANDLE Process
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function GetProcessId(
            <[In]> hThread As IntPtr
        ) As UInteger
        End Function

        ''' <summary>
        ''' Opens an existing local process object.
        ''' </summary>
        ''' <param name="dwDesiredAccess">
        ''' The access to the process object. This access right is checked against the 
        ''' security descriptor for the process. This parameter can be one or more of the process access rights.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="bInheritHandle">
        ''' If this value is TRUE, processes created by this process will inherit the handle. 
        ''' Otherwise, the processes do not inherit this handle.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="dwProcessId">
        ''' The identifier of the local process to be opened.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is an open handle to the specified process. 
        ''' If the function fails, the return value is NULL.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/desktop/api/processthreadsapi/nf-processthreadsapi-openprocess">OpenProcess documentation</see> for more information.
        '''
        ''' The function signature in C++ is:
        ''' <code>
        ''' HANDLE OpenProcess(
        '''   [in] DWORD dwDesiredAccess,
        '''   [in] BOOL  bInheritHandle,
        '''   [in] DWORD dwProcessId
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, CharSet:=CharSet.Auto, SetLastError:=True)>
        Friend Shared Function OpenProcess(
            <[In]> dwDesiredAccess As ProcessAccessRights,
            <[In]> bInheritHandle As Boolean,
            <[In]> dwProcessId As UInteger
        ) As IntPtr
        End Function

        ''' <summary>
        ''' Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        ''' </summary>
        ''' <param name="hModule">
        ''' A handle to the DLL module that contains the function or variable. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpProcName">
        ''' The name of the function or variable exported by the DLL. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is the address of the exported function or variable. 
        ''' If the function fails, the return value is NULL. To get extended error information, call <c>GetLastError</c>.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getprocaddress">GetProcAddress documentation</see> for more information.
        '''
        ''' The function signature in C++ is:
        ''' <code>
        ''' FARPROC GetProcAddress(
        '''   [in] HMODULE hModule,
        '''   [in] LPCSTR  lpProcName
        ''' );
        ''' </code>
        ''' </remarks>
        <SuppressMessage("Microsoft.Performance", "CA2101:Specify marshaling", Justification:="Various marshaling options were tested, but all resulted in errors  The default string handling is used as it aligns with the unmanaged API requirements.")>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function GetProcAddress(
            <[In]> hModule As IntPtr,
            <[In]> lpProcName As String
        ) As IntPtr
        End Function

        ''' <remarks>
        ''' We’ve tried several ways to marshal the <paramref name="lpServiceName"/> string, including various `MarshalAs` options, but all of them resulted in errors like code 1060 (ERROR_SERVICE_DOES_NOT_EXIST). 
        ''' This suggests that the marshalling may not align with the expectations of the unmanaged function. If the function does not work with specific marshalling types, it's best to use default string handling and ensure 
        ''' that the string encoding and format match the unmanaged API’s requirements. Note that this approach might allow partially trusted callers to interact with unmanaged code, which could pose security risks if not handled properly.
        ''' </remarks>

        ''' <summary>
        ''' Ends the specified process and all of its threads.
        ''' </summary>
        ''' <param name="uExitCode">
        ''' The exit code for the process. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-exitprocess">ExitProcess documentation</see> for more information.
        '''
        ''' The function signature in C++ is:
        ''' <code>
        ''' void ExitProcess(
        '''   [in] UINT uExitCode
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32)>
        Friend Shared Sub ExitProcess(
            <[In]> uExitCode As UInteger
        )
        End Sub

        ''' <summary>
        ''' Creates a thread that runs in the address space of another process.
        ''' </summary>
        ''' <param name="hProcess">
        ''' A handle to the target process. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpThreadAttributes">
        ''' A pointer to a SECURITY_ATTRIBUTES structure that determines whether the returned handle can be inherited by child processes. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="dwStackSize">
        ''' The initial size of the stack, in bytes. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpStartAddress">
        ''' A pointer to the application-defined function to be executed by the thread. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpParameter">
        ''' A pointer to a variable to be passed to the thread function. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="dwCreationFlags">
        ''' The creation flags for the thread. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpThreadId">
        ''' A pointer to a variable that receives the thread identifier. This parameter is passed with the <c>[Out]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is NULL.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-createremotethread">CreateRemoteThread documentation</see> for more information.
        '''
        ''' The function signature in C++ is:
        ''' <code>
        ''' HANDLE CreateRemoteThread(
        '''   [in] HANDLE hProcess,
        '''   [in] LPSECURITY_ATTRIBUTES lpThreadAttributes,
        '''   [in] SIZE_T dwStackSize,
        '''   [in] LPTHREAD_START_ROUTINE lpStartAddress,
        '''   [in] LPVOID lpParameter,
        '''   [in] DWORD dwCreationFlags,
        '''   [out] LPDWORD lpThreadId
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function CreateRemoteThread(
            <[In]> hProcess As IntPtr,
            <[In]> lpThreadAttributes As IntPtr,
            <[In]> dwStackSize As UInteger,
            <[In]> lpStartAddress As IntPtr,
            <[In]> lpParameter As IntPtr,
            <[In]> dwCreationFlags As UInteger,
            <Out> lpThreadId As UInteger
        ) As IntPtr
        End Function

        ''' <summary>
        ''' Retrieves a module handle for the specified module.
        ''' </summary>
        ''' <param name="lpModuleName">
        ''' The name of the module whose handle is to be retrieved. This parameter is passed with the <c>[In]</c> attribute.
        ''' It can be <c>NULL</c> to retrieve the handle of the calling process's executable file.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is a handle to the specified module. 
        ''' If the function fails, the return value is <c>NULL</c>. To get extended error information, call 
        ''' <see cref="Marshal.GetLastWin32Error"/>.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getmodulehandlea">GetModuleHandleA documentation</see> for more information.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' HMODULE GetModuleHandleA(
        '''   [in, optional] LPCSTR lpModuleName
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, CharSet:=CharSet.Ansi, SetLastError:=True)>
        Friend Shared Function GetModuleHandle(
           <[In], [Optional]> lpModuleName As String
        ) As IntPtr
        End Function

        ''' <summary>
        ''' Allocates memory within the virtual address space of a specified process.
        ''' </summary>
        ''' <param name="hProcess">
        ''' A handle to the process in whose virtual address space the memory is to be allocated. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpAddress">
        ''' The pointer specifying the desired starting address for the allocated region of pages. This parameter is optional and is passed with the <c>[In]</c> attribute.
        ''' If <c>NULL</c>, the system determines where to allocate the region.
        ''' </param>
        ''' <param name="dwSize">
        ''' The size of the region of memory to allocate, in bytes. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="flAllocationType">
        ''' The type of memory allocation. This parameter can be one or more of the memory allocation options and is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="flProtect">
        ''' The memory protection for the region of pages to be allocated. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is the base address of the allocated region of pages. If the function fails, the return value is <c>NULL</c>.
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-virtualallocex">VirtualAllocEx documentation</see>.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' LPVOID VirtualAllocEx(
        '''   [in]           HANDLE hProcess,
        '''   [in, optional] LPVOID lpAddress,
        '''   [in]           SIZE_T dwSize,
        '''   [in]           DWORD  flAllocationType,
        '''   [in]           DWORD  flProtect
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function VirtualAllocEx(
            <[In]> hProcess As IntPtr,
            <[In], [Optional]> lpAddress As IntPtr,
            <[In]> dwSize As UIntPtr,
            <[In]> flAllocationType As UInteger,
            <[In]> flProtect As UInteger
        ) As IntPtr
        End Function

        ''' <summary>
        ''' Writes data to the memory of a specified process.
        ''' </summary>
        ''' <param name="hProcess">
        ''' A handle to the process memory to be modified. This handle must have the <see cref="ProcessAccessRights.VirtualMemoryWrite"/> access right.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpBaseAddress">
        ''' A pointer to the base address in the specified process to which data is written. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpBuffer">
        ''' A pointer to the buffer containing the data to be written. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="nSize">
        ''' The number of bytes to be written to the specified process. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpNumberOfBytesWritten">
        ''' A pointer to a variable that receives the number of bytes transferred into the specified process.
        ''' This parameter is passed with the <c>[Out]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' Returns <c>True</c> if the function succeeds; otherwise, returns <c>False</c>.
        ''' </returns>
        ''' <remarks>
        ''' For more details, see the <see href="https://learn.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-writeprocessmemory">WriteProcessMemory documentation</see>.
        '''
        ''' The function signature in C++ is:
        ''' <code>
        ''' BOOL WriteProcessMemory(
        '''   [in]  HANDLE  hProcess,
        '''   [in]  LPVOID  lpBaseAddress,
        '''   [in]  LPCVOID lpBuffer,
        '''   [in]  SIZE_T  nSize,
        '''   [out] SIZE_T  *lpNumberOfBytesWritten
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function WriteProcessMemory(
            <[In]> hProcess As IntPtr,
            <[In]> lpBaseAddress As IntPtr,
            <[In]> lpBuffer As IntPtr,
            <[In]> nSize As UInteger,
            <Out> ByRef lpNumberOfBytesWritten As UInteger
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Releases or decommits the specified region of memory in the virtual address space of a specified process.
        ''' </summary>
        ''' <param name="hProcess">
        ''' A handle to the process whose memory is to be freed. The handle must have the 
        ''' <see cref="ProcessAccessRights.VirtualMemoryOperation"/> access right.
        ''' </param>
        ''' <param name="lpAddress">
        ''' A pointer to the base address of the region of memory to be freed. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="dwSize">
        ''' The size of the region of memory to be freed, in bytes. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="dwFreeType">
        ''' The type of free operation. This parameter can be one of the following values: 
        ''' <c>MEM_RELEASE</c> or <c>MEM_DECOMMIT</c>.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error()"/>.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-virtualfreeex">VirtualFreeEx documentation</see> for more information.
        ''' The function signature in C++ is:
        ''' <code>
        ''' BOOL VirtualFreeEx(
        '''   [in] HANDLE hProcess,
        '''   [in] LPVOID lpAddress,
        '''   [in] SIZE_T dwSize,
        '''   [in] DWORD  dwFreeType
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function VirtualFreeEx(
            <[In]> hProcess As IntPtr,
            <[In]> lpAddress As IntPtr,
            <[In]> dwSize As UIntPtr,
            <[In]> dwFreeType As UInteger
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Waits until the specified object is in the signaled state or the time-out interval elapses.
        ''' </summary>
        ''' <param name="hHandle">
        ''' A handle to the object to be waited on. The handle must be in the <c>signaled</c> state or have the <c>WAIT_OBJECT_0</c> return value.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="dwMilliseconds">
        ''' The time-out interval, in milliseconds. If this parameter is zero, the function returns immediately.
        ''' If this parameter is <c>INFINITE</c>, the function will wait indefinitely for the object to be signaled.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is one of the following values:
        ''' <list type="bullet">
        ''' <item><c>WAIT_OBJECT_0</c>: The state of the object is signaled.</item>
        ''' <item><c>WAIT_TIMEOUT</c>: The time-out interval elapsed.</item>
        ''' <item><c>WAIT_FAILED</c>: The function failed. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</item>
        ''' </list>
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/synchapi/nf-synchapi-waitforsingleobject">WaitForSingleObject documentation</see> for more information.
        ''' The function signature in C++ is:
        ''' <code>
        ''' DWORD WaitForSingleObject(
        '''   [in] HANDLE hHandle,
        '''   [in] DWORD  dwMilliseconds
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function WaitForSingleObject(
            <[In]> hHandle As IntPtr,
            <[In]> dwMilliseconds As UInteger
        ) As UInteger
        End Function

    End Class
End Namespace
