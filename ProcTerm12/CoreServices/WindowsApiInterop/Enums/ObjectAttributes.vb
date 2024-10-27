Namespace CoreServices.WindowsApiInterop.Enums

    ''' <summary>
    ''' Specifies attributes that can be applied to objects or object handles by routines that create objects and/or return handles to objects.
    ''' This enum is used in various Windows API functions to define specific object characteristics.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="ObjectAttributes"/> enum defines flags that modify the behavior of object creation and management functions.
    ''' These attributes are utilized in calls to functions such as <see cref="NativeMethods.NtCreateFile"/> or <see cref="NativeMethods.NtOpenProcess"/>.
    ''' 
    ''' <list type="bullet">
    ''' <item>
    '''     <description>
    '''         <see cref="ObjectAttributes.DefaultAttributes"/>: 
    '''         No attributes specified. This is the default state for an object. 
    '''         In C++: <code>0</code>.
    '''     </description>
    ''' </item>
    ''' <item>
    '''     <description>
    '''         <see cref="ObjectAttributes.ProtectFromClose"/>: 
    '''         Prevents the object from being closed. This is useful for objects that 
    '''         should remain accessible even when other handles are being closed. 
    '''         In C++: <code>0x1</code>.
    '''     </description>
    ''' </item>
    ''' <item>
    '''     <description>
    '''         <see cref="ObjectAttributes.Inherit"/>: 
    '''         Allows the object to be inherited by child processes. This is important for 
    '''         synchronization objects and other resources that need to be shared. 
    '''         In C++: <code>0x2</code>.
    '''     </description>
    ''' </item>
    ''' <item>
    '''     <description>
    '''         <see cref="ObjectAttributes.AuditObjectClose"/>: 
    '''         Generates an audit event when the object is closed. This is critical for 
    '''         security and compliance monitoring, ensuring that object usage is tracked. 
    '''         In C++: <code>0x4</code>.
    '''     </description>
    ''' </item>
    ''' </list>
    ''' 
    ''' For further details on how these attributes are applied, refer to:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/ntdef/ns-ntdef-_object_attributes">OBJECT_ATTRIBUTES Structure</see>.
    ''' </remarks>
    <Flags>
    Friend Enum ObjectAttributes As Byte

        ''' <summary>
        ''' No attributes specified. This is the default state for an object.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>0</code>.
        ''' </remarks>
        DefaultAttributes = &H0

        ''' <summary>
        ''' Prevents the object from being closed.
        ''' </summary>
        ''' <remarks>
        ''' This is useful for objects that should remain accessible even when other handles are being closed.
        ''' In C++: <code>0x1</code>.
        ''' This member is marked with <see cref="UsedImplicitlyAttribute"/> because it is not referenced directly 
        ''' in the current code, but is included for completeness and potential future use.
        ''' </remarks>
        <UsedImplicitly>
        ProtectFromClose = &H1

        ''' <summary>
        ''' Allows the object to be inherited by child processes.
        ''' </summary>
        ''' <remarks>
        ''' This is important for synchronization objects and other resources that need to be shared.
        ''' In C++: <code>0x2</code>.
        ''' This member is marked with <see cref="UsedImplicitlyAttribute"/> because it is not referenced directly 
        ''' in the current code, but is included for completeness and potential future use.
        ''' </remarks>
        <UsedImplicitly>
        Inherit = &H2

        ''' <summary>
        ''' Generates an audit event when the object is closed.
        ''' </summary>
        ''' <remarks>
        ''' This is critical for security and compliance monitoring, ensuring that object usage is tracked.
        ''' In C++: <code>0x4</code>.
        ''' This member is marked with <see cref="UsedImplicitlyAttribute"/> because it is not referenced directly 
        ''' in the current code, but is included for completeness and potential future use.
        ''' </remarks>
        <UsedImplicitly>
        AuditObjectClose = &H4
    End Enum
End Namespace
