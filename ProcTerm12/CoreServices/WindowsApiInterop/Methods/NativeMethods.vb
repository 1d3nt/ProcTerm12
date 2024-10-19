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
        ''' This field is used to represent a null handle (IntPtr.Zero) in P/Invoke calls to unmanaged code.
        ''' </remarks>
        Friend Shared ReadOnly NullHandleValue As IntPtr = IntPtr.Zero

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
        ''' See <see href="https://docs.microsoft.com/en-us/windows/desktop/api/processthreadsapi/nf-processthreadsapi-terminateprocess">TerminateProcess Documentation</see>.
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
    End Class
End Namespace
