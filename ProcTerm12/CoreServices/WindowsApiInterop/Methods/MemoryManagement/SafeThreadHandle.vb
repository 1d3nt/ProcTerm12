Namespace CoreServices.WindowsApiInterop.Methods.MemoryManagement

    ''' <summary>
    ''' A SafeHandle wrapper for a thread handle to ensure proper resource management.
    ''' </summary>
    <SecurityCritical>
    Public NotInheritable Class SafeThreadHandle
        Inherits SafeHandleZeroOrMinusOneIsInvalid

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SafeThreadHandle"/> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(True)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SafeThreadHandle"/> class with an existing handle.
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

        ''' <summary>
        ''' Creates a new instance of the <see cref="SafeThreadHandle"/> class from a thread handle.
        ''' </summary>
        ''' <param name="threadHandle">The thread handle to wrap.</param>
        ''' <returns>A new <see cref="SafeThreadHandle"/> instance.</returns>
        Public Shared Function FromHandle(threadHandle As IntPtr) As SafeThreadHandle
            Return New SafeThreadHandle(threadHandle, True)
        End Function
    End Class
End Namespace
