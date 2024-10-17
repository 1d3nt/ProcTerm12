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
    <Security.SuppressUnmanagedCodeSecurity>
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
    End Class
End Namespace
