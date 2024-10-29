Namespace CoreServices.WindowsApiInterop.Methods.MemoryManagement

    ''' <summary>
    ''' A SafeHandle wrapper for a job object handle to ensure proper resource management.
    ''' </summary>
    <SecurityCritical>
    Public NotInheritable Class SafeJobHandle
        Inherits SafeHandleZeroOrMinusOneIsInvalid

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SafeJobHandle"/> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(True)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SafeJobHandle"/> class with an existing handle.
        ''' </summary>
        ''' <param name="existingHandle">The existing handle to wrap.</param>
        ''' <param name="ownsHandle">Indicates whether the handle should be released when this SafeHandle is released.</param>
        Public Sub New(existingHandle As IntPtr, ownsHandle As Boolean)
            MyBase.New(ownsHandle)
            SetHandle(existingHandle)
        End Sub

        ''' <summary>
        ''' Releases the handle when the SafeHandle is disposed.
        ''' </summary>
        ''' <returns>True if the handle was released successfully; otherwise, false.</returns>
        Protected Overrides Function ReleaseHandle() As Boolean
            Return NativeMethods.CloseHandle(handle)
        End Function
    End Class
End Namespace    

