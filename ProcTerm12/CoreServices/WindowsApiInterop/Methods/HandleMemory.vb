Namespace CoreServices.WindowsApiInterop.Methods

    ''' <summary>
    ''' Provides utility methods for managing handles in P/Invoke operations.
    ''' </summary>
    ''' <remarks>
    ''' This class is marked as <c>NotInheritable</c> to prevent inheritance.
    ''' </remarks>
    Friend NotInheritable Class HandleManager

        ''' <summary>
        ''' Closes the token handle if it is not null.
        ''' </summary>
        ''' <param name="tokenHandle">The handle to be closed.</param>
        ''' <remarks>
        ''' This method checks if the provided <paramref name="tokenHandle"/> is not equal to <see cref="NativeMethods.NullHandleValue"/>. 
        ''' If it is not equal, the method proceeds to close the handle by calling the <see cref="NativeMethods.CloseHandle"/> method. 
        ''' If the handle is equal to <see cref="NativeMethods.NullHandleValue"/>, which indicates an invalid or uninitialized handle, 
        ''' the method skips the closing operation.
        ''' </remarks>
        Friend Shared Sub CloseTokenHandleIfNotNull(tokenHandle As SafeTokenHandle)
            If tokenHandle IsNot Nothing AndAlso Not tokenHandle.IsInvalid Then
                tokenHandle.Dispose()
            End If
        End Sub
    End Class
End Namespace
