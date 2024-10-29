Namespace CoreServices.WindowsApiInterop.Structs

    ''' <summary>
    ''' Sets limits for a job object.
    ''' </summary>
    ''' <remarks>
    ''' This enumeration corresponds to the <c>JobObjectInformationClass</c> used to set information for job objects in the Windows API.
    ''' <list type="bullet">
    ''' <item>
    ''' <description>
    ''' <see cref="JobObjectInformationClass.JobObjectExtendedLimitInformation"/>: 
    ''' Indicates that the job object is to be set with extended limit information. 
    ''' This includes basic limit information and input/output accounting for the job.
    ''' In C++: <code>#define JobObjectExtendedLimitInformation 9</code>
    ''' </description>
    ''' </item>
    ''' </list>
    ''' 
    ''' For additional details on the <see cref="JobObjectInformationClass"/> enumeration, refer to:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-jobobject_extended_limit_information">JobObjectExtendedLimitInformation Structure</see>.
    ''' </remarks>
    Friend Enum JobObjectInformationClass

        ''' <summary>
        ''' Indicates that the job object is to be set with extended limit information.
        ''' </summary>
        ''' <remarks>
        ''' This includes basic limit information and input/output accounting for the job.
        ''' In C++: <code>#define JobObjectExtendedLimitInformation 9</code>
        ''' </remarks>
        JobObjectExtendedLimitInformation = 9
    End Enum
End Namespace

