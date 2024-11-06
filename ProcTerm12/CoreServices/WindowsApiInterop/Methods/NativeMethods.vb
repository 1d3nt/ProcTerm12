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

#Region " Constants "

        ''' <summary>
        ''' Represents the error code for an invalid parameter.
        ''' </summary>
        Friend Const ErrorInvalidParameter As Integer = 0

        ''' <summary>
        ''' Represents the success status code returned by wait functions, such as <see cref="WaitForSingleObject"/>, 
        ''' when a specified object is in the signaled state.
        ''' </summary>
        ''' <remarks>
        ''' The <c>WaitObject0</c> constant indicates that the wait operation on a specified object, such as a process or thread handle,
        ''' was successful and that the object is in a signaled state. This value, typically <c>0</c>, is used by Windows API functions
        ''' to show that a requested event has occurred, and execution can proceed.
        ''' </remarks>
        Friend Const WaitObject0 As UInteger = &H0UI

        ''' <summary>
        ''' Represents the code returned by <see cref="GetExitCodeProcess"/> to indicate that a process is still active.
        ''' </summary>
        ''' <remarks>
        ''' The <c>StillActive</c> constant, with a typical value of <c>259</c>, is used in conjunction with <see cref="GetExitCodeProcess"/>
        ''' to indicate that the specified process has not yet terminated. When the process is still running, the function returns this value
        ''' to signify that the process has not exited and remains active.
        ''' </remarks>
        Friend Const StillActive As UInteger = &H103UI

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
        ''' Represents an infinite timeout value used in synchronization functions, such as <see cref="WaitForSingleObject"/>.
        ''' </summary>
        ''' <remarks>
        ''' The <c>Infinite</c> constant can be used as the timeout parameter to instruct the system to wait indefinitely for a 
        ''' synchronization event, such as a process handle or thread handle. This constant is set to <c>-1</c> (or <c>&HFFFFFFFFUI</c>), 
        ''' which is interpreted by Windows API functions as an infinite wait period. Additionally, this value is used for memory-related 
        ''' operations where an "unbounded" or "maximum" size needs to be indicated, as with <c>MemInfinite</c>.
        ''' </remarks>
        Friend Const MemInfinite As UInteger = &HFFFFFFFFUI

        ''' <summary>
        ''' Specifies that the snapshot created by CreateToolhelp32Snapshot will include all modules 
        ''' in the specified process.
        ''' </summary>
        ''' <remarks>
        ''' This flag allows CreateToolhelp32Snapshot to capture the loaded modules within the process.
        ''' For further details, see:
        ''' https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-createtoolhelp32snapshot
        ''' Example C++ representation:
        ''' <code>
        ''' #define TH32CS_SNAPMODULE 0x00000008
        ''' </code>
        ''' This member is marked with <see cref="UsedImplicitlyAttribute"/> because it is not referenced directly 
        ''' in the current code, but is included for completeness and potential future use.
        ''' </remarks>
        <UsedImplicitly>
        Friend Const Th32CsSnapModule As UInteger = &H8

        ''' <summary>
        ''' Specifies that the snapshot created by CreateToolhelp32Snapshot will include all modules 
        ''' in the specified process and is specific to 32-bit modules.
        ''' </summary>
        ''' <remarks>
        ''' This flag allows CreateToolhelp32Snapshot to capture the loaded 32-bit modules within the process.
        ''' For further details, see:
        ''' https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-createtoolhelp32snapshot
        ''' Example C++ representation:
        ''' <code>
        ''' #define TH32CS_SNAPMODULE32 0x00000010
        ''' </code>
        ''' This member is marked with <see cref="UsedImplicitlyAttribute"/> because it is not referenced directly 
        ''' in the current code, but is included for completeness and potential future use.
        ''' </remarks>
        <UsedImplicitly>
        Friend Const Th32CsSnapModule32 As UInteger = &H10

        ''' <summary>
        ''' Specifies that the snapshot created by CreateToolhelp32Snapshot will include all processes and modules.
        ''' </summary>
        ''' <remarks>
        ''' This constant is used to indicate that both regular and 32-bit module snapshots should be taken.
        ''' For further details, see:
        ''' https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-createtoolhelp32snapshot
        ''' Example C++ representation:
        ''' <code>
        ''' #define TH32CS_SNAPALL 0x00000018
        ''' </code>
        ''' </remarks>
        Friend Const Th32CsSnapAll As UInteger = &H18
#End Region ' Constants

        ''' <summary>
        ''' Represents a null handle value used in P/Invoke calls.
        ''' </summary>
        ''' <remarks>
        ''' This field is used to represent a null handle (<see cref="IntPtr.Zero"/>) in P/Invoke calls to unmanaged code.
        ''' </remarks>
        Friend Shared ReadOnly NullHandleValue As IntPtr = IntPtr.Zero

        ''' <summary>
        ''' Represents an invalid handle value, commonly used to indicate that a handle operation failed.
        ''' </summary>
        ''' <remarks>
        ''' Used to check if handle-creation functions like CreateFile, CreateToolhelp32Snapshot, or other handle-returning 
        ''' functions failed. An INVALID_HANDLE_VALUE return typically indicates an error occurred.
        ''' For further details, see:
        ''' https://learn.microsoft.com/en-us/windows/win32/api/handleapi/nf-handleapi-closehandle
        ''' Example C++ representation:
        ''' <code>
        ''' #define INVALID_HANDLE_VALUE ((HANDLE)(LONG_PTR)-1)
        ''' </code>
        ''' </remarks>
        Friend Shared ReadOnly InvalidHandleValue As New IntPtr(-1)

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
            <[In], Out> hThread As SafeProcessHandle,
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
            <[In]> hSnapshot As SafeProcessHandle,
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
            <[In]> hSnapshot As SafeProcessHandle,
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
            <[In]> hThread As SafeProcessHandle
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
        '''
        ''' We’ve tried several ways to marshal the <paramref name="lpProcName"/> string, including various `MarshalAs` options, but all of them resulted in a return value of 0. 
        ''' This suggests that the marshalling may not align with the expectations of the unmanaged function. If the function does not work with specific marshalling types, it's best to use default string handling and ensure 
        ''' that the string encoding and format match the unmanaged API’s requirements. Note that this approach might allow partially trusted callers to interact with unmanaged code, which could pose security risks if not handled properly.
        ''' </remarks>
        <SuppressMessage("Microsoft.Performance", "CA2101:Specify marshaling", Justification:="Various marshaling options were tested, but all resulted in a return value of 0. The default string handling is used as it aligns with the unmanaged API requirements.")>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function GetProcAddress(
           <[In]> hModule As IntPtr,
           <[In]> lpProcName As String
        ) As IntPtr
        End Function

        ''' <summary>
        ''' Ends the specified process and all of its threads.
        ''' </summary>
        ''' <param name="uExitCode">
        ''' The exit code for the process. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-exitprocess">ExitProcess documentation</see> for more information.
        ''' 
        ''' This function signature in C++ is:
        ''' <code>
        ''' void ExitProcess(
        '''   [in] UINT uExitCode
        ''' );
        ''' </code>
        ''' 
        ''' This signature <cref="ExitProcess"/> is kept for potential future use and completeness as it relates to process termination within the project. 
        ''' </remarks>
        <UsedImplicitly, DllImport(ExternDll.Kernel32)>
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
            <[In]> hProcess As SafeProcessHandle,
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
        '''
        ''' We’ve tried several ways to marshal the <paramref name="lpModuleName"/> string, including various `MarshalAs` options, but all of them resulted in a return value of 0. 
        ''' This suggests that the marshalling may not align with the expectations of the unmanaged function. If the function does not work with specific marshalling types, it's best to use default string handling and ensure 
        ''' that the string encoding and format match the unmanaged API’s requirements. Note that this approach might allow partially trusted callers to interact with unmanaged code, which could pose security risks if not handled properly.
        '''
        ''' The libloaderapi.h header defines GetModuleHandle as an alias which automatically selects the ANSI or Unicode version of this function based on the definition of the UNICODE preprocessor constant. 
        ''' Mixing usage of the encoding-neutral alias with code that is not encoding-neutral can lead to mismatches that result in compilation or runtime errors. For more information, see Conventions for Function Prototypes.
        ''' </remarks>
        <SuppressMessage("Microsoft.Performance", "CA2101:Specify marshaling", Justification:="Various marshaling options were tested, but all resulted in a return value of 0. The default string handling is used as it aligns with the unmanaged API requirements.")>
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
            <[In]> hProcess As SafeProcessHandle,
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
            <[In]> hProcess As SafeProcessHandle,
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
        ''' If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.
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
            <[In]> hProcess As SafeProcessHandle,
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
            <[In]> hHandle As SafeProcessHandle,
            <[In]> dwMilliseconds As UInteger
        ) As UInteger
        End Function

        ''' <summary>
        ''' Retrieves the termination status of a specified process.
        ''' </summary>
        ''' <param name="hProcess">
        ''' A handle to the process. The handle must have the <see cref="ProcessAccessRights.QueryInformation" /> access right.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpExitCode">
        ''' A pointer to a variable that receives the exit code of the process. This parameter is passed with the <c>[Out]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is nonzero (<c>True</c>). If the function fails, the return value is zero (<c>False</c>). 
        ''' To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.
        ''' </returns>
        ''' <remarks>
        ''' The <c>GetExitCodeProcess</c> function retrieves the exit code that the process has returned when it terminates.
        ''' For more details, refer to the <a href="https://learn.microsoft.com/en-us/windows/win32/api/processthreads/nf-processthreads-getexitcodeprocess">GetExitCodeProcess</a> documentation.
        ''' The function signature in C++ is:
        ''' <code>
        ''' BOOL GetExitCodeProcess(
        '''   [in] HANDLE hProcess,
        '''   [out] LPDWORD lpExitCode
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function GetExitCodeProcess(
           <[In]> hProcess As IntPtr,
           <Out> ByRef lpExitCode As UInteger
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Creates or opens a job object with specified security attributes and name.
        ''' This function is commonly used to limit the resources that a process and its child processes can consume.
        ''' </summary>
        ''' <param name="lpJobAttributes">
        ''' A pointer to a <c>SECURITY_ATTRIBUTES</c> structure that specifies the security descriptor for the job object 
        ''' and determines whether child processes can inherit the returned handle. If lpJobAttributes is NULL, 
        ''' the job object gets a default security descriptor, and the handle cannot be inherited. 
        ''' The ACLs in the default security descriptor for a job object come from the primary or impersonation token of the creator.
        ''' This parameter is passed with the <c>[In, Optional]</c> attribute.
        ''' </param>
        ''' <param name="lpName">
        ''' The name of the job, limited to MAX_PATH characters. Name comparison is case-sensitive.
        ''' This parameter is passed with the <c>[In, Optional]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is a handle to the job object with JOB_OBJECT_ALL_ACCESS rights.
        ''' If the function fails, the return value is NULL.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-createjobobjecta">CreateJobObject Documentation</see>.
        ''' The function signature in C++ is:
        ''' <code>
        ''' HANDLE CreateJobObjectA(
        '''   [in, optional] LPSECURITY_ATTRIBUTES lpJobAttributes,
        '''   [in, optional] LPCSTR lpName
        ''' );
        ''' </code>
        ''' 
        ''' Although the default CharSet is Auto, which resolves to Unicode on Windows, 
        ''' we explicitly declare CharSet.Unicode here for clarity.
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, CharSet:=CharSet.Unicode, SetLastError:=True)>
        Friend Shared Function CreateJobObjectA(
            <[In], [Optional]> lpJobAttributes As IntPtr,
            <[In], [Optional]> lpName As String
        ) As IntPtr
        End Function

        ''' <summary>
        ''' Assigns a process to an existing job object.
        ''' </summary>
        ''' <param name="hJob">
        ''' A handle to the job object to which the process will be associated. The CreateJobObject or OpenJobObject
        ''' function returns this handle. The handle must have the JOB_OBJECT_ASSIGN_PROCESS access right. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="hProcess">
        ''' A handle to the process to associate with the job object. The handle must have the PROCESS_SET_QUOTA and
        ''' PROCESS_TERMINATE access rights. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/jobapi2/nf-jobapi2-assignprocesstojobobject">AssignProcessToJobObject Documentation</see>.
        ''' The function signature in C++ is:
        ''' <code>
        ''' BOOL AssignProcessToJobObject(
        '''   [in] HANDLE hJob,
        '''   [in] HANDLE hProcess
        ''' );
        ''' </code>
        ''' 
        ''' Although the default CharSet is Auto, which resolves to Unicode on Windows, 
        ''' we explicitly declare CharSet.Unicode here for clarity.
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, CharSet:=CharSet.Unicode, SetLastError:=True)>
        Friend Shared Function AssignProcessToJobObject(
            <[In]> hJob As SafeProcessHandle,
            <[In]> hProcess As SafeProcessHandle
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Terminates all processes currently associated with the job. If the job is nested, this function
        ''' terminates all processes currently associated with the job and all of its child jobs in the hierarchy.
        ''' </summary>
        ''' <param name="hJob">
        ''' A handle to the job whose processes will be terminated. The CreateJobObject or OpenJobObject function returns this handle.
        ''' This handle must have the JOB_OBJECT_TERMINATE access right. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="uExitCode">
        ''' The exit code to be used by all processes and threads in the job object. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/jobapi2/nf-jobapi2-terminatejobobject">TerminateJobObject Documentation</see>.
        ''' The function signature in C++ is:
        ''' <code>
        ''' BOOL TerminateJobObject(
        '''   [in] HANDLE hJob,
        '''   [in] UINT uExitCode
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function TerminateJobObject(
            <[In]> hJob As SafeProcessHandle,
            <[In]> uExitCode As UInteger
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Sets limits for a job object.
        ''' </summary>
        ''' <param name="hJob">
        ''' A handle to the job whose limits are being set. The CreateJobObject or OpenJobObject function returns this handle.
        ''' The handle must have the JOB_OBJECT_SET_ATTRIBUTES access right. For more information, see Job Object Security
        ''' and Access Rights. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="infoType">
        ''' The information class for the limits to be set. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpJobObjectInfo">
        ''' The limits or job state to be set for the job. The format of this data depends on the value of JobObjectInfoClass.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="cbJobObjectInfoLength">
        ''' The size of the job information being set, in bytes. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/jobapi2/nf-jobapi2-setinformationjobobject">SetInformationJobObject Documentation</see>.
        ''' Although the default CharSet is Auto, which resolves to Unicode on Windows, 
        ''' we explicitly declare CharSet.Unicode here for clarity.
        ''' The function signature in C++ is:
        ''' <code>
        ''' BOOL SetInformationJobObject(
        ''' [in] HANDLE hJob,
        ''' [in] JobObjectInformationClass infoType,
        ''' [in] LPVOID lpJobObjectInfo,
        ''' [in] DWORD cbJobObjectInfoLength
        ''' );
        ''' </code>
        ''' 
        ''' Although the default CharSet is Auto, which resolves to Unicode on Windows, 
        ''' we explicitly declare CharSet.Unicode here for clarity.
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, CharSet:=CharSet.Unicode, SetLastError:=True)>
        Friend Shared Function SetInformationJobObject(
            <[In]> hJob As SafeProcessHandle,
            <[In]> infoType As JobObjectInformationClass,
            <[In]> lpJobObjectInfo As IntPtr,
            <[In]> cbJobObjectInfoLength As UInteger
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Retrieves information about a range of pages in the virtual memory of a specified process.
        ''' </summary>
        ''' <param name="hProcess">
        ''' A handle to the process whose virtual memory is being queried. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpAddress">
        ''' A pointer to the starting address of the region of memory to query. This parameter is optional and is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpBuffer">
        ''' A pointer to a <see cref="MemoryBasicInformation"/> structure that receives information about the specified region of pages. This parameter is passed with the <c>[Out]</c> attribute.
        ''' </param>
        ''' <param name="dwLength">
        ''' The size of the <see cref="MemoryBasicInformation"/> structure, in bytes. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' Returns the size of the region of pages that was queried, in bytes. If the function fails, the return value is zero.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-virtualqueryex">VirtualQueryEx documentation</see> for more information.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' SIZE_T VirtualQueryEx(
        '''   [in]           HANDLE                    hProcess,
        '''   [in, optional] LPCVOID                   lpAddress,
        '''   [out]          PMEMORY_BASIC_INFORMATION lpBuffer,
        '''   [in]           SIZE_T                    dwLength
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function VirtualQueryEx(
            <[In]> hProcess As IntPtr,
            <[In]> lpAddress As IntPtr,
            <Out> lpBuffer As MemoryBasicInformation,
            <[In]> dwLength As UInteger
        ) As UInteger
        End Function

        ''' <summary>
        ''' Changes the protection on a region of committed pages in the virtual memory of a specified process.
        ''' </summary>
        ''' <param name="hProcess">
        ''' A handle to the process whose memory protection is to be changed. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpAddress">
        ''' A pointer to the starting address of the region of memory whose protection is to be changed. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="dwSize">
        ''' The size of the region of memory, in bytes, whose protection is to be changed. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="flNewProtect">
        ''' The memory protection to apply to the region of pages. This parameter is passed with the <c>[In]</c> attribute and is of type <see cref="MemoryProtection"/>.
        ''' </param>
        ''' <param name="lpflOldProtect">
        ''' A pointer to a variable that receives the previous protection of the first page in the specified region. This parameter is passed with the <c>[Out]</c> attribute and is of type <see cref="MemoryProtection"/>.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.
        ''' </returns>
        ''' <remarks>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-virtualprotectex">VirtualProtectEx documentation</see> for more information.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' BOOL VirtualProtectEx(
        '''   [in]  HANDLE hProcess,
        '''   [in]  LPVOID lpAddress,
        '''   [in]  SIZE_T dwSize,
        '''   [in]  DWORD  flNewProtect,
        '''   [out] PDWORD lpflOldProtect
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, SetLastError:=True)>
        Friend Shared Function VirtualProtectEx(
            <[In]> hProcess As IntPtr,
            <[In]> lpAddress As IntPtr,
            <[In]> dwSize As UInteger,
            <[In]> flNewProtect As MemoryProtection,
            <Out> ByRef lpflOldProtect As MemoryProtection
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Retrieves information about the first module in a snapshot.
        ''' </summary>
        ''' <param name="hSnapshot">
        ''' A handle to the snapshot of the modules. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpme">
        ''' A reference to a <see cref="ModuleEntry32"/> structure that will receive information about the module. 
        ''' This parameter should not be marked with the <c>[Out]</c> attribute. If marked incorrectly as <c>[Out]</c>, 
        ''' the function will fail to operate correctly and will not return the expected results. 
        ''' This is critical because the structure needs to be filled with data from the API call, and marking it incorrectly 
        ''' can disrupt the expected behavior.
        ''' </param>
        ''' <remarks>
        ''' <para>
        ''' For this function to work correctly, the <c>CharSet</c> must be set to <c>CharSet.Unicode</c>. This is essential because the Windows API function it calls, <c>Module32FirstW</c>,
        ''' is specifically the Unicode version. If the <c>CharSet</c> is not set to Unicode, it will default to ANSI, which can lead to unexpected behavior or failure to retrieve module information.
        ''' </para>
        ''' <para>
        ''' The <c>EntryPoint</c> parameter is set to <c>Module32FirstW</c> to indicate that we are using the Unicode version of this function. This is crucial for ensuring that string data is handled
        ''' correctly when retrieving module names and paths, which may contain Unicode characters.
        ''' </para>
        ''' <para>
        ''' The <c>lpme</c> parameter is declared as a <c>ByRef</c> reference to a <see cref="ModuleEntry32"/> structure. It is important that this parameter is marked with the <c>[Out]</c> attribute.
        ''' If marked incorrectly, such as using <c>[In]</c>, the function may break or fail to provide the expected output. This is because the structure needs to be populated with data from the API call,
        ''' which requires it to be treated as an output parameter.
        ''' 
        ''' The <c>lpme</c> parameter should not be marked with the <c>[Out]</c> attribute. If it is incorrectly marked as <c>[Out]</c>, 
        ''' the function will fail to operate correctly and will not return the expected results. 
        ''' This is critical because the structure needs to be filled with data from the API call, and marking it incorrectly can disrupt the expected behavior.
        ''' </para>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-module32firstw">Module32FirstW documentation</see> for more information.
        ''' </remarks>


        <DllImport(ExternDll.Kernel32, SetLastError:=True, CharSet:=CharSet.Unicode, EntryPoint:="Module32FirstW")>
        Friend Shared Function Module32First(
            <[In]> hSnapshot As IntPtr,
             ByRef lpme As ModuleEntry32
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Retrieves information about the next module in a module snapshot.
        ''' </summary>
        ''' <param name="hSnapshot">
        ''' A handle to the snapshot of the modules. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="lpme">
        ''' A reference to a <see cref="ModuleEntry32"/> structure that receives information about the module. 
        ''' This parameter is passed with the <c>[Out]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' If the function succeeds, the return value is nonzero. If the function fails or there are no more modules, the return value is zero.
        ''' </returns>
        ''' <remarks>
        ''' <para>
        ''' For this function to work correctly, the <c>CharSet</c> must be set to <c>CharSet.Unicode</c>. This is essential because the Windows API function it calls, <c>Module32NextW</c>,
        ''' is specifically the Unicode version. If the <c>CharSet</c> is not set to Unicode, it will default to ANSI, which can lead to unexpected behavior or failure to retrieve module information.
        ''' </para>
        ''' <para>
        ''' The <c>EntryPoint</c> parameter is set to <c>Module32NextW</c> to indicate that we are using the Unicode version of this function. This is crucial for ensuring that string data is handled
        ''' correctly when retrieving module names and paths, which may contain Unicode characters.
        ''' </para>
        ''' <para>
        ''' The <c>lpme</c> parameter is declared as a <c>ByRef</c> reference to a <see cref="ModuleEntry32"/> structure. It is important that this parameter is marked with the <c>[Out]</c> attribute.
        ''' If marked incorrectly, such as using <c>[In]</c>, the function may break or fail to provide the expected output. This is because the structure needs to be populated with data from the API call,
        ''' which requires it to be treated as an output parameter.
        ''' 
        ''' The <c>lpme</c> parameter should not be marked with the <c>[Out]</c> attribute. If it is incorrectly marked as <c>[Out]</c>, 
        ''' the function will fail to operate correctly and will not return the expected results. 
        ''' This is critical because the structure needs to be filled with data from the API call, and marking it incorrectly can disrupt the expected behavior.
        ''' </para>
        ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-module32nextw">Module32NextW documentation</see> for more information.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' BOOL Module32Next(
        '''   [in]  HANDLE           hSnapshot,
        '''   [out] LPMODULEENTRY32W lpme
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Kernel32, CharSet:=CharSet.Unicode, EntryPoint:="Module32NextW", SetLastError:=True)>
        Friend Shared Function Module32Next(
            <[In]> hSnapshot As IntPtr,
            ByRef lpme As ModuleEntry32
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function
    End Class
End Namespace
