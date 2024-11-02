Namespace CoreServices.WindowsApiInterop.Structs

    ''' <summary>
    ''' Represents the attributes of an object in the Windows operating system.
    ''' This structure is used in various Windows API functions to define object properties.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="ObjectAttributes"/> structure is utilized in calls to functions such as 
    ''' <see cref="NativeMethods.NtCreateFile"/> or <see cref="NativeMethods.NtOpenProcess"/>. 
    ''' It defines important characteristics of objects and how they are managed in Windows.
    ''' 
    ''' <list type="bullet">
    ''' <item><description><see cref="ObjectAttributes.Length"/>: The size of the structure, in bytes. In C++: <code>ULONG Length;</code></description></item>
    ''' <item><description><see cref="ObjectAttributes.RootDirectory"/>: A handle to the root directory for the object. In C++: <code>HANDLE RootDirectory;</code></description></item>
    ''' <item><description><see cref="ObjectAttributes.ObjectName"/>: A pointer to a Unicode string that contains the name of the object. In C++: <code>PUNICODE_STRING ObjectName;</code></description></item>
    ''' <item><description><see cref="ObjectAttributes.Attributes"/>: A set of attributes that specify object properties. In C++: <code>ULONG Attributes;</code></description></item>
    ''' <item><description><see cref="ObjectAttributes.SecurityDescriptor"/>: A pointer to a security descriptor that controls access to the object. In C++: <code>PVOID SecurityDescriptor;</code></description></item>
    ''' <item><description><see cref="ObjectAttributes.SecurityQualityOfService"/>: A pointer to a structure that contains the quality of service for the object. In C++: <code>PVOID SecurityQualityOfService;</code></description></item>
    ''' </list>
    ''' 
    ''' For further details on how these attributes are applied, refer to:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/ntdef/ns-ntdef-_object_attributes">OBJECT_ATTRIBUTES Structure</see>.
    ''' 
    ''' <example>
    ''' In C++:
    ''' <code>
    ''' typedef struct _OBJECT_ATTRIBUTES {
    '''     ULONG Length;
    '''     HANDLE RootDirectory;
    '''     PUNICODE_STRING ObjectName;
    '''     ULONG Attributes;
    '''     PVOID SecurityDescriptor;
    '''     PVOID SecurityQualityOfService;
    ''' } OBJECT_ATTRIBUTES;
    ''' </code>
    ''' </example>
    ''' </remarks>
    <StructLayout(LayoutKind.Sequential)>
    Friend Structure ObjectAttributes

        ''' <summary>
        ''' The size of the structure, in bytes.
        ''' </summary>
        Friend Length As UInteger

        ''' <summary>
        ''' A handle to the root directory for the object.
        ''' </summary>
        Friend RootDirectory As IntPtr

        ''' <summary>
        ''' A pointer to a Unicode string that contains the name of the object.
        ''' </summary>
        Friend ObjectName As IntPtr

        ''' <summary>
        ''' A set of attributes that specify object properties.
        ''' </summary>
        Friend Attributes As UInteger

        ''' <summary>
        ''' A pointer to a security descriptor that controls access to the object.
        ''' </summary>
        Friend SecurityDescriptor As IntPtr

        ''' <summary>
        ''' A pointer to a structure that contains the quality of service for the object.
        ''' </summary>
        Friend SecurityQualityOfService As IntPtr
    End Structure
End Namespace
