Namespace CoreServices.WindowsApiInterop.Methods.MemoryManagement

    ''' <summary>
    ''' A SafeHandle wrapper for various types of handles to ensure proper resource management.
    ''' </summary>
    <SecurityCritical>
    Friend NotInheritable Class SafeHandleWrapper
        Inherits SafeHandleZeroOrMinusOneIsInvalid

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SafeHandleWrapper"/> class.
        ''' </summary>
        Friend Sub New()
            MyBase.New(True)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SafeHandleWrapper"/> class with an existing handle.
        ''' </summary>
        ''' <param name="existingHandle">The existing handle to wrap.</param>
        ''' <param name="ownsHandle">Indicates whether the handle should be released when this SafeHandle is released.</param>
        Friend Sub New(existingHandle As IntPtr, ownsHandle As Boolean)
            MyBase.New(ownsHandle)
            SetHandle(existingHandle)
        End Sub

        ''' <summary>
        ''' Releases the handle when the SafeHandle is disposed.
        ''' </summary>
        ''' <returns>True if the handle was released successfully; otherwise, false.</returns>
        Protected Overrides Function ReleaseHandle() As Boolean
            Return HandleManager.CloseHandleIfNotNull(handle)
        End Function

        ''' <summary>
        ''' Creates a new instance of the <see cref="SafeHandleWrapper"/> class from a handle.
        ''' </summary>
        ''' <param name="handle">The handle to wrap.</param>
        ''' <returns>A new <see cref="SafeHandleWrapper"/> instance.</returns>
        Friend Shared Function FromHandle(handle As IntPtr) As SafeHandleWrapper
            Return New SafeHandleWrapper(handle, True)
        End Function
    End Class
End Namespace
