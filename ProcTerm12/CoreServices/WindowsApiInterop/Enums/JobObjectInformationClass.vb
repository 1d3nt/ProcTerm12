Namespace CoreServices.WindowsApiInterop.Enums

    ''' <summary>
    ''' Sets limits for a job object.
    ''' </summary>
    ''' <remarks>
    ''' This enumeration corresponds to the <c>JobObjectInformationClass</c> used to set information for job objects in the Windows API.
    ''' <list type="bullet">
    '''     <item><description><see cref="JobObjectInformationClass.JobObjectExtendedLimitInformation"/> corresponds to <c>9</c>.</description></item>
    '''     <item><description><see cref="JobObjectInformationClass.JobObjectBasicLimitInformation"/> corresponds to <c>2</c>.</description></item>
    '''     <item><description><see cref="JobObjectInformationClass.JobObjectBasicUIRestrictions"/> corresponds to <c>11</c>.</description></item>
    '''     <item><description><see cref="JobObjectInformationClass.JobObjectSecurityLimitInformation"/> corresponds to <c>5</c>.</description></item>
    ''' </list>
    ''' 
    ''' For further details, see:
    ''' <see href="https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/nf-ntifs-zwduplicateobject">ZwDuplicateObject</see>.
    ''' 
    ''' <example>
    ''' In C++:
    ''' <code>
    ''' typedef struct _JOBOBJECT_EXTENDED_LIMIT_INFORMATION {
    '''     JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
    '''     IO_COUNTERS                       IoInfo;
    '''     SIZE_T                            ProcessMemoryLimit;
    '''     SIZE_T                            JobMemoryLimit;
    '''     SIZE_T                            PeakProcessMemoryUsed;
    '''     SIZE_T                            PeakJobMemoryUsed;
    ''' } JOBOBJECT_EXTENDED_LIMIT_INFORMATION, *PJOBOBJECT_EXTENDED_LIMIT_INFORMATION;
    ''' </code>
    ''' </example>
    ''' </remarks>
    <Flags>
    Friend Enum JobObjectInformationClass

        ''' <summary>
        ''' Indicates that the job object is to be set with extended limit information.
        ''' </summary>
        ''' <remarks>
        ''' This includes basic limit information and input/output accounting for the job.
        ''' In C++: <code>#define JobObjectExtendedLimitInformation 9</code>
        ''' </remarks>
        JobObjectExtendedLimitInformation = 9

        ''' <summary>
        ''' Indicates that the job object is to be set with basic limit information.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define JobObjectBasicLimitInformation 2</code>
        ''' This member is marked with <see cref="UsedImplicitlyAttribute"/> because it is not referenced directly 
        ''' in the current code, but is included for completeness and potential future use.
        ''' </remarks>
        <UsedImplicitly>
        JobObjectBasicLimitInformation = 2

        ''' <summary>
        ''' Indicates that UI restrictions are to be set for the job object.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define JobObjectBasicUIRestrictions 11</code>
        ''' This member is marked with <see cref="UsedImplicitlyAttribute"/> because it is not referenced directly 
        ''' in the current code, but is included for completeness and potential future use.
        ''' </remarks>
        <UsedImplicitly>
        JobObjectBasicUiRestrictions = 11

        ''' <summary>
        ''' Indicates that the security limit information is to be set for the job object.
        ''' </summary>
        ''' <remarks>
        ''' In C++: <code>#define JobObjectSecurityLimitInformation 5</code>
        ''' This member is marked with <see cref="UsedImplicitlyAttribute"/> because it is not referenced directly 
        ''' in the current code, but is included for completeness and potential future use.
        ''' </remarks>
        <UsedImplicitly>
        JobObjectSecurityLimitInformation = 5
    End Enum
End Namespace
