Namespace CoreServices.WindowsApiInterop.Enums

    ''' <summary>
    ''' A set of flags to control the behavior of the duplication operation.
    ''' Set this parameter to zero or to the bitwise OR of one or more of the following flags.
    ''' </summary>
    ''' <remarks>
    ''' These flags are used with the duplication operation, specifically for functions like 
    ''' <see cref="UnsafeNativeMethods.NtDuplicateObject"/>. Although the <see cref="UnsafeNativeMethods.NtDuplicateObject"/> function 
    ''' may not have extensive documentation, its behavior can often be inferred from the related 
    ''' <c>ZwDuplicateObject</c> function, which is documented in the Windows Driver Kit (WDK).
    ''' For further details, see <see href="https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/nf-ntifs-zwduplicateobject">ZwDuplicateObject</see>.
    ''' 
    ''' <list type="bullet">
    ''' <item>
    '''     <description>
    '''         <see cref="DuplicateOptions.DuplicateCloseSource"/>: 
    '''         Closes the source handle after it has been duplicated. 
    '''         In C++: <code>0x1</code>.
    '''     </description>
    ''' </item>
    ''' <item>
    '''     <description>
    '''         <see cref="DuplicateOptions.DuplicateSameAccess"/>: 
    '''         Duplicates the handle with the same access rights as the original handle. 
    '''         In C++: <code>0x2</code>.
    '''     </description>
    ''' </item>
    ''' <item>
    '''     <description>
    '''         <see cref="DuplicateOptions.DuplicateSameAttributes"/>: 
    '''         Duplicates the handle while preserving the original object's attributes. 
    '''         In C++: <code>0x4</code>.
    '''     </description>
    ''' </item>
    ''' <item>
    '''     <description>
    '''         <see cref="DuplicateOptions.DuplicateProcessDupHandle"/>: 
    '''         Allows the duplication of a handle from the source process to the destination process. 
    '''         In C++: <code>0x40</code>.
    '''     </description>
    ''' </item>
    ''' </list>
    ''' </remarks>
    <Flags>
    Friend Enum DuplicateOptions As Integer

        ''' <summary>
        ''' Closes the source handle after it has been duplicated.
        ''' </summary>
        DuplicateCloseSource = &H1

        ''' <summary>
        ''' Duplicates the handle with the same access rights as the original handle.
        ''' This member is marked with <see cref="UsedImplicitlyAttribute"/> because it is not referenced directly 
        ''' in the current code, but is included for completeness and potential future use.
        ''' </summary>
        <UsedImplicitly>
        DuplicateSameAccess = &H2

        ''' <summary>
        ''' Duplicates the handle while preserving the original object's attributes.
        ''' This member is marked with <see cref="UsedImplicitlyAttribute"/> because it is not referenced directly 
        ''' in the current code, but is included for completeness and potential future use.
        ''' </summary>
        <UsedImplicitly>
        DuplicateSameAttributes = &H4

        ''' <summary>
        ''' Allows the duplication of a handle from the source process to the destination process.
        ''' This member is marked with <see cref="UsedImplicitlyAttribute"/> because it is not referenced directly 
        ''' in the current code, but is included for completeness and potential future use.
        ''' </summary>
        <UsedImplicitly>
        DuplicateProcessDupHandle = &H40
    End Enum
End Namespace
