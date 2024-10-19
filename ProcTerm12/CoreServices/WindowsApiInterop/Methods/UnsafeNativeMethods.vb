Namespace CoreServices.WindowsApiInterop.Methods

    ''' <summary>
    ''' Provides methods for interacting with native Windows APIs that may have unsafe operations.
    ''' This class contains P/Invoke declarations for functions that require careful handling.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="UnsafeNativeMethods"/> class uses the <c>DllImport</c> attribute to define methods imported 
    ''' from unmanaged DLLs that may affect the stability of the operating system if misused.
    ''' 
    ''' The <c>SuppressUnmanagedCodeSecurity</c> attribute is applied to this class to improve performance when
    ''' calling unmanaged code. Use this attribute with caution, as it bypasses some of the security measures
    ''' provided by the .NET runtime.
    ''' </remarks>
    <SuppressUnmanagedCodeSecurity>
    Friend NotInheritable Class UnsafeNativeMethods

        ''' <summary>
        ''' Terminates the specified process and all of its threads.
        ''' </summary>
        ''' <param name="processHandle">
        ''' A handle to the process to be terminated. The handle must have the <see cref="ProcessAccessFlags.Terminate"/> access right.
        ''' For more information, see Process Security and Access Rights.
        ''' <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms684880(v=vs.85).aspx">MSDN Documentation</see>.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="exitStatus">
        ''' The exit code to be used by the process and threads terminated as a result of this call. Use the
        ''' GetExitCodeProcess function to retrieve a process's exit value. Use the GetExitCodeThread function 
        ''' to retrieve a thread's exit value.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' An NTSTATUS value indicating the outcome of the termination attempt.
        ''' If the function succeeds, the return value is STATUS_SUCCESS (0x0).
        ''' If the function fails, the return value is an NTSTATUS error code.
        ''' </returns>
        ''' <remarks>
        ''' The <see cref="NtTerminateProcess"/> function provides additional functionality and flexibility compared to its counterpart, <see cref="NativeMethods.TerminateProcess"/>.
        ''' 
        ''' 1. Access to Native API: <see cref="NtTerminateProcess"/> is part of the Native API provided by <c>ntdll.dll</c>, offering lower-level access to the operating system compared to the Windows API.
        ''' 2. Status Codes: <see cref="NtTerminateProcess"/> returns an <see cref="Ntstatus"/> value, providing detailed information about the outcome of the termination attempt,
        ''' including success, failure, and specific error conditions.
        ''' 3. Process State Control: <see cref="NtTerminateProcess"/> allows for more nuanced control over process termination by allowing developers to specify an exit status for the terminated process,
        ''' facilitating more granular handling of termination events by the process's parent.
        ''' 4. Access Rights: <see cref="NtTerminateProcess"/> requires a process handle with specific access rights (<c>PROCESS_TERMINATE</c>), providing finer-grained control
        ''' over process termination permissions.
        ''' 
        ''' Overall, <see cref="NtTerminateProcess"/> offers greater flexibility and control over process termination operations, enabling developers to interact more closely with the
        ''' operating system's underlying mechanisms and obtain detailed information about the outcome of termination attempts.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' NTSTATUS NtTerminateProcess(
        '''   [in, optional] HANDLE ProcessHandle,
        '''   [in] NTSTATUS ExitStatus
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Ntdll, SetLastError:=True)>
        Friend Shared Function NtTerminateProcess(
            <[In]> processHandle As IntPtr,
            <[In]> exitStatus As Integer
        ) As Ntstatus
        End Function
    End Class
End Namespace
