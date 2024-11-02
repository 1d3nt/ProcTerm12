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
        ''' Represents an infinite timeout value for certain Windows API functions.
        ''' </summary>
        ''' <remarks>
        ''' This field is typically used with functions that accept a timeout parameter, such as waiting for an object to be signaled.
        ''' The value of -1 indicates that the function should wait indefinitely.
        ''' </remarks>
        Friend Shared ReadOnly Infinite As New IntPtr(-1)

        ''' <summary>
        ''' Terminates the specified process and all of its threads.
        ''' </summary>
        ''' <param name="processHandle">
        ''' A handle to the process to be terminated. The handle must have the <see cref="ProcessAccessRights.Terminate"/> access right.
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
        ''' For more details, see <a href="https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntddk/nf-ntddk-zwterminateprocess">ZwTerminateProcess documentation</a>
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
        ) As NtStatus
        End Function

        ''' <summary>
        ''' The NtDuplicateObject routine creates a handle that is a duplicate of the specified source handle.
        ''' </summary>
        ''' <param name="sourceProcessHandle">
        ''' A handle to the source process for the handle being duplicated. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="sourceHandle">
        ''' The handle to duplicate. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="targetProcessHandle">
        ''' A handle to the target process that is to receive the new handle.
        ''' This parameter is optional and can be specified as <c>IntPtr.Zero</c> if the <see cref="DuplicateOptions.DuplicateCloseSource"/> flag is set in <paramref name="options"/>. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="targetHandle">
        ''' A pointer to a <c>HANDLE</c> variable into which the routine writes the new duplicated handle.
        ''' The duplicated handle is valid in the specified target process.
        ''' This parameter is optional and can be specified as <c>IntPtr.Zero</c> if no duplicate handle is to be created. 
        ''' This parameter is passed with the <c>[Out]</c> attribute.
        ''' </param>
        ''' <param name="desiredAccess">
        ''' An <see cref="ProcessAccessRights"/> value that specifies the desired access for the new handle. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="handleAttributes">
        ''' A <c>ULONG</c> that specifies the desired attributes for the new handle.
        ''' This parameter uses the flags defined in the <see cref="ObjectAttributeFlags"/> enum
        ''' to specify characteristics and behaviors for the handle instead of directly utilizing
        ''' the <see cref="ObjectAttributes"/> structure. By using the enum, we can simplify 
        ''' the process of setting attributes, as the enum values represent specific options
        ''' without requiring the overhead of the full structure. For more information about attributes, 
        ''' see the description of the Attributes member in <see cref="ObjectAttributes"/>. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="options">
        ''' A set of flags to control the behavior of the duplication operation.
        ''' Set this parameter to zero or to the bitwise OR of one or more of the following <see cref="DuplicateOptions"/> flags. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' NtDuplicateObject returns <c>STATUS_SUCCESS</c> if the call is successful. 
        ''' Otherwise, it returns an appropriate error status code.
        ''' </returns>
        ''' <remarks>
        ''' For more details, see <a href="https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/nf-ntifs-zwduplicateobject">ZwDuplicateObject documentation</a>.
        ''' The function signature in C++ is:
        ''' <code>
        ''' NTSYSAPI NTSTATUS ZwDuplicateObject(
        '''   [in]            HANDLE      SourceProcessHandle,
        '''   [in]            HANDLE      SourceHandle,
        '''   [in, optional]  HANDLE      TargetProcessHandle,
        '''   [out, optional] PHANDLE     TargetHandle,
        '''   [in]            ACCESS_MASK DesiredAccess,
        '''   [in]            ULONG       HandleAttributes,
        '''   [in]            ULONG       Options
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Ntdll)>
        Friend Shared Function NtDuplicateObject(
            <[In]> sourceProcessHandle As IntPtr,
            <[In]> sourceHandle As IntPtr,
            <[In], [Optional]> targetProcessHandle As IntPtr,
            <[Out], [Optional]> ByRef targetHandle As IntPtr,
            <[In]> desiredAccess As ProcessAccessRights,
            <[In]> handleAttributes As ObjectAttributeFlags ,
            <[In]> options As DuplicateOptions
        ) As Integer
        End Function

        ''' <summary>
        ''' Deprecated. Closes the specified handle. NtClose is superseded by <see cref="NativeMethods.CloseHandle"/>.
        ''' </summary>
        ''' <param name="handle">
        ''' The handle being closed. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' The various NTSTATUS values are defined in NTSTATUS.H, which is distributed with the Windows DDK.
        ''' </returns>
        ''' <remarks>
        ''' For more details, see <a href="https://learn.microsoft.com/en-us/windows/win32/api/winternl/nf-winternl-ntclose">NtClose documentation</a>.
        ''' The function signature in C++ is:
        ''' <code>
        ''' NTSYSAPI NTSTATUS NtClose(
        '''   [in] HANDLE Handle
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Ntdll, SetLastError:=True)>
        Friend Shared Function NtClose(
            <[In]> handle As IntPtr
        ) As Integer
        End Function

        ''' <summary>
        ''' Waits for the specified object to enter a signaled state or until a specified time-out period elapses. 
        ''' This method is deprecated and is now generally replaced by <see cref="NativeMethods.WaitForSingleObject"/>.
        ''' </summary>
        ''' <param name="handle">
        ''' The handle to the object being waited on. This must be a valid handle to an object that supports synchronization, 
        ''' such as a process, thread, or event. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="alertable">
        ''' Specifies whether the wait is alertable, allowing the wait to be interrupted by an alert if set to <c>true</c>. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="timeout">
        ''' A pointer to a time-out value in either relative or absolute terms. If set to <c>0</c>, 
        ''' the function returns immediately if the object is unassigned. If <c>null</c>, the function waits indefinitely. 
        ''' If the time-out period elapses before the object enters the signaled state, the wait is considered unsuccessful.
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' An NTSTATUS code indicating the result of the wait operation. <c>STATUS_SUCCESS</c> indicates the object is signaled; 
        ''' other values provide error information, as defined in NTSTATUS.h (Windows DDK).
        ''' </returns>
        ''' <remarks>
        ''' This function is a lower-level alternative to <c>WaitForSingleObject</c> and is used primarily for 
        ''' specific system-level scenarios.
        ''' <para>C++ Declaration: <code>
        ''' NTSTATUS NtWaitForSingleObject(
        '''     HANDLE Handle,
        '''     BOOLEAN Alertable,
        '''     PLARGE_INTEGER Timeout
        ''' ); 
        ''' </code></para>
        ''' For more information, refer to 
        ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winternl/nf-winternl-ntwaitforsingleobject">NtWaitForSingleObject documentation</see>.
        ''' </remarks>

        <DllImport(ExternDll.Ntdll, SetLastError:=True)>
        Friend Shared Function NtWaitForSingleObject(
            <[In]> handle As IntPtr,
            <[In]> alertable As Boolean,
            <[In]> timeout As IntPtr
        ) As Integer
        End Function

        ''' <summary>
        ''' Opens a handle to a specified process with the desired access rights.
        ''' </summary>
        ''' <param name="ProcessHandle">
        ''' A pointer to a <c>HANDLE</c> variable that will receive the process handle. This parameter is passed with the <c>[Out]</c> attribute.
        ''' </param>
        ''' <param name="DesiredAccess">
        ''' An <c>ACCESS_MASK</c> value that specifies the requested access rights to the process handle. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="ObjectAttributes">
        ''' A reference to an <see cref="ObjectAttributes"/> structure that specifies object name, attributes, and security descriptor. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="ClientId">
        ''' A reference to a <see cref="ClientId"/> structure that specifies the target process by its process ID and optionally thread ID. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <returns>
        ''' Returns <c>STATUS_SUCCESS</c> if the function call is successful. Otherwise, returns an error code indicating failure.
        ''' </returns>
        ''' <remarks>
        ''' For more details, see <a href="https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntddk/nf-ntddk-ntopenprocess">NtOpenProcess documentation</a>.
        ''' The function signature in C++ is:
        ''' <code>
        ''' __kernel_entry NTSYSCALLAPI NTSTATUS NtOpenProcess(
        '''   [out]          PHANDLE            ProcessHandle,
        '''   [in]           ACCESS_MASK        DesiredAccess,
        '''   [in]           POBJECT_ATTRIBUTES ObjectAttributes,
        '''   [in, optional] PCLIENT_ID         ClientId
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Ntdll, SetLastError:=True)>
        Friend Shared Function NtOpenProcess(
            <Out> ByRef processHandle As IntPtr,
            <[In]> desiredAccess As ProcessAccessRights,
            <[In]> ByRef objectAttributes As ObjectAttributes,
            <[In], [Optional]> ByRef clientId As ClientId
        ) As Integer
        End Function

        ''' <summary>
        ''' Creates a debug object, which can be used to debug processes and threads.
        ''' </summary>
        ''' <param name="debugObjectHandle">
        ''' The handle to the debug object. This parameter is passed with the <c>[Out]</c> attribute.
        ''' </param>
        ''' <param name="desiredAccess">
        ''' The access rights for the debug object. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="objectAttributes">
        ''' An optional pointer to an <see cref="ObjectAttributes"/> structure that specifies the attributes of the debug object. 
        ''' This parameter is passed with the <c>[In]</c> attribute and can be <c>NULL</c> if no attributes are specified.
        ''' </param>
        ''' <param name="killProcessOnExit">
        ''' A <c>BOOLEAN</c> value that indicates whether the system should terminate all processes associated with the debug object when it is closed. 
        ''' This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <remarks>
        ''' For more details, refer to the <see href="http://undocumented.ntinternals.net/index.html">NtCreateDebugObject documentation</see> for more information.
        ''' The function signature in C++ is:
        ''' <code>
        ''' NTSTATUS NtCreateDebugObject(
        '''   OUT PHANDLE             DebugObjectHandle,
        '''   IN ACCESS_MASK          DesiredAccess,
        '''   IN POBJECT_ATTRIBUTES   ObjectAttributes OPTIONAL,
        '''   IN BOOLEAN              KillProcessOnExit
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Ntdll, SetLastError:=True)>
        Friend Shared Function NtCreateDebugObject(
            <Out> ByRef debugObjectHandle As IntPtr,
            <[In]> desiredAccess As UInteger,
            <[In]> objectAttributes As ObjectAttributes,
            <[In]> killProcessOnExit As Boolean
        ) As UInteger
        End Function

        ''' <summary>
        ''' Attaches a debugger to the specified process.
        ''' </summary>
        ''' <param name="processHandle">
        ''' A handle to the process to which the debugger is to be attached. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <param name="debugObjectHandle">
        ''' A handle to the debug object that will be used for the debugging session. This parameter is passed with the <c>[In]</c> attribute.
        ''' </param>
        ''' <remarks>
        ''' For more details, refer to the <see href="http://undocumented.ntinternals.net/index.html">NtDebugActiveProcess documentation</see> for more information.
        ''' 
        ''' The function signature in C++ is:
        ''' <code>
        ''' NTSTATUS NtDebugActiveProcess(
        '''   [in] HANDLE ProcessHandle,
        '''   [in] HANDLE DebugObjectHandle
        ''' );
        ''' </code>
        ''' </remarks>
        <DllImport(ExternDll.Ntdll, SetLastError:=True)>
        Friend Shared Function NtDebugActiveProcess(
            <[In]> processHandle As IntPtr,
            <[In]> debugObjectHandle As IntPtr
        ) As Integer
        End Function
    End Class
End Namespace

