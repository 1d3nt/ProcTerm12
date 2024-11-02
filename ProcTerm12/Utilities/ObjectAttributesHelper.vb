Namespace Utilities

    ''' <summary>
    ''' Provides helper methods for initializing and managing object attributes.
    ''' </summary>
    Friend NotInheritable Class ObjectAttributesHelper

        ''' <summary>
        ''' Prevents a default instance of the <see cref="ObjectAttributesHelper"/> class from being created.
        ''' </summary>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Initializes an <see cref="ObjectAttributes"/> structure.
        ''' </summary>
        ''' <param name="objectAttributes">A reference to the ObjectAttributes structure to be initialized.</param>
        ''' <param name="objectName">A pointer to a Unicode string that specifies the name of the object.</param>
        ''' <param name="attributes">The attributes of the object.</param>
        ''' <param name="rootDirectory">A handle to the root directory for the object.</param>
        ''' <param name="securityDescriptor">A pointer to a security descriptor for the object.</param>
        ''' <returns>Returns a status code indicating success or failure.</returns>
        Friend Shared Function InitializeObjectAttributes(
                        ByRef objectAttributes As ObjectAttributes,
                        objectName As IntPtr,
                        attributes As UInteger,
                        rootDirectory As IntPtr,
                        securityDescriptor As IntPtr) As UInteger
            Try
                objectAttributes = CreateObjectAttributes(objectName, attributes, rootDirectory, securityDescriptor)
                Return NtStatus.StatusSuccess
            Catch ex As Exception
                Return HandleInitializationError(ex)
            End Try
        End Function

        ''' <summary>
        ''' Creates an <see cref="ObjectAttributes"/> structure with the specified parameters.
        ''' </summary>
        ''' <param name="objectName">A pointer to a Unicode string that specifies the name of the object.</param>
        ''' <param name="attributes">The attributes of the object.</param>
        ''' <param name="rootDirectory">A handle to the root directory for the object.</param>
        ''' <param name="securityDescriptor">A pointer to a security descriptor for the object.</param>
        ''' <returns>An initialized <see cref="ObjectAttributes"/> structure.</returns>
        Private Shared Function CreateObjectAttributes(objectName As IntPtr, attributes As UInteger, rootDirectory As IntPtr, securityDescriptor As IntPtr) As ObjectAttributes
            Return New ObjectAttributes With {
                .Length = CUInt(Marshal.SizeOf(GetType(ObjectAttributes))),
                .RootDirectory = rootDirectory,
                .Attributes = attributes,
                .ObjectName = objectName,
                .SecurityDescriptor = securityDescriptor,
                .SecurityQualityOfService = NativeMethods.NullHandleValue
            }
        End Function

        ''' <summary>
        ''' Handles errors that occur during the initialization of <see cref="ObjectAttributes"/>.
        ''' </summary>
        ''' <param name="ex">The exception that occurred.</param>
        ''' <returns>
        ''' A status code indicating failure. The returned status code is a generic error code 
        ''' represented by the value <c>0x80000000</c> (or <c>&H80000000UI</c> in VB.NET). 
        ''' This value is a standard HRESULT error code indicating a failure condition.
        ''' </returns>
        ''' <remarks>
        ''' The error code <c>0x80000000</c> is a generic error code used in many Windows API functions 
        ''' to indicate a failure. It is part of the HRESULT error code system, where the high bit 
        ''' (0x80000000) indicates an error. This method uses this generic error code to signal that 
        ''' an exception occurred during the initialization of the <see cref="ObjectAttributes"/> structure.
        ''' </remarks>
        Private Shared Function HandleInitializationError(ex As Exception) As UInteger
            Return CUInt(&H80000000UI)
        End Function
    End Class
End Namespace
