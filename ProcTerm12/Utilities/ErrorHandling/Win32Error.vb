Namespace Utilities.ErrorHandling

    ''' <summary>
    ''' Provides utility methods for handling errors encountered in unmanaged code through platform invoke (P/Invoke).
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="Win32Error"/> class includes methods to retrieve error information from the last unmanaged call that set an error, 
    ''' such as those marked with the <see cref="DllImportAttribute.SetLastError"/> flag.
    ''' <para>This class is especially useful in scenarios where unmanaged code interacts with Windows API functions, allowing for 
    ''' streamlined error handling and consistent error messages.</para>
    ''' </remarks>
    Friend NotInheritable Class Win32Error

        ''' <summary>
        ''' Prevents a default instance of the <see cref="Win32Error"/> class from being created.
        ''' </summary>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Retrieves the error message associated with the error code returned by the last unmanaged function called via platform invoke (P/Invoke)
        ''' that has the <see cref="DllImportAttribute.SetLastError"/> flag set.
        ''' This method is available in .NET Framework and earlier .NET versions.
        ''' </summary>
        ''' <returns>The error message associated with the last Win32 error on the current thread.</returns>
        ''' <remarks>
        ''' <para>Although this method is not currently used, it is kept for completeness.</para>
        ''' <para>This function wraps the call to <c>Marshal.GetLastWin32Error</c> and returns the error message corresponding to the last error 
        ''' set by a Win32 API call.</para>
        ''' <para><strong>Note:</strong> In .NET 8.0 and later, it is recommended to use <see cref="GetLastPInvokeError"/> for retrieving 
        ''' the last P/Invoke error due to performance and accuracy improvements. Specifically, <see cref="GetLastPInvokeError"/> allows for 
        ''' more accurate error handling with modern P/Invoke calls, as it retrieves errors directly from the calling thread's error state.</para>
        ''' For more details, see <a href="https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.marshal.getlastwin32error?view=netframework-4.8">Marshal.GetLastWin32Error</a>.
        ''' </remarks>
        <UsedImplicitly>
        Friend Shared Function GetLastWin32Error() As String
            Return New Win32Exception(Marshal.GetLastWin32Error()).Message
        End Function

        ''' <summary>
        ''' Retrieves the error message associated with the error code returned by the last unmanaged function called using platform invoke (P/Invoke)
        ''' on the current thread, supporting improved error handling in .NET 8.0 and later.
        ''' </summary>
        ''' <returns>The error message associated with the last P/Invoke error on the current thread.</returns>
        ''' <remarks>
        ''' <para>With the introduction of .NET 8.0, <c>Marshal.GetLastPInvokeError</c> offers a more reliable way to retrieve the last error 
        ''' set by P/Invoke calls. Unlike <see cref="GetLastWin32Error"/>, which relies on Win32-specific error retrieval, <see cref="GetLastPInvokeError"/> 
        ''' operates with thread-local error handling, leading to improved accuracy in retrieving the last error set during a P/Invoke operation.</para>
        ''' <para><strong>Preferred Method:</strong> It is recommended to use <see cref="GetLastPInvokeError"/> over <see cref="GetLastWin32Error"/>
        ''' in modern .NET applications, as it provides enhanced compatibility with cross-platform .NET environments and reflects Microsoft's 
        ''' continued improvements in P/Invoke interop functionality.</para>
        ''' For more details, see <a href="https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.marshal.getlastpinvokeerror?view=net-8.0">Marshal.GetLastPInvokeError</a>.
        ''' </remarks>
        Friend Shared Function GetLastPInvokeError() As String
            Return New Win32Exception(Marshal.GetLastPInvokeError()).Message
        End Function

        ''' <summary>
        ''' Retrieves the error code returned by the last unmanaged function called using platform invoke (P/Invoke)
        ''' on the current thread.
        ''' </summary>
        ''' <returns>The error code associated with the last P/Invoke error on the current thread.</returns>
        ''' <remarks>
        ''' <para>With the introduction of .NET 8.0, <c>Marshal.GetLastPInvokeError</c> offers a more reliable way to retrieve the last error 
        ''' set by P/Invoke calls. Unlike <see cref="GetLastWin32Error"/>, which relies on Win32-specific error retrieval, <see cref="GetLastPInvokeError"/> 
        ''' operates with thread-local error handling, leading to improved accuracy in retrieving the last error set during a P/Invoke operation.</para>
        ''' <para><strong>Preferred Method:</strong> It is recommended to use <see cref="GetLastPInvokeErrorCode"/> over <see cref="GetLastWin32Error"/>
        ''' in modern .NET applications, as it provides enhanced compatibility with cross-platform .NET environments and reflects Microsoft's 
        ''' continued improvements in P/Invoke interop functionality.</para>
        ''' For more details, see <a href="https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.marshal.getlastpinvokeerror?view=net-8.0">Marshal.GetLastPInvokeError</a>.
        ''' </remarks>
        Friend Shared Function GetLastPInvokeErrorCode() As Integer
            Return Marshal.GetLastPInvokeError()
        End Function
    End Class
End Namespace