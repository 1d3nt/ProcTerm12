Namespace CoreServices.WindowsApiInterop.Structs

    ''' <summary>
    ''' Represents job object limit flags.
    ''' </summary>
    ''' <remarks>
    ''' For additional details, refer to:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-jobobject_basic_limit_information">JobObjectBasicLimitInformation Documentation</see>.
    ''' </remarks>
    ''' </remarks>
    Friend Class JobObjectLimitFlags

        ''' <summary>
        ''' Causes all processes associated with the job to terminate when the last handle to the job is closed.
        ''' </summary>
        ''' <remarks>
        ''' This limit requires use of a <see cref="JobObjectExtendedLimitInformation"/> structure.
        ''' Its BasicLimitInformation member is a <see cref="JobObjectBasicLimitInformation"/> structure.
        ''' </remarks>
        Friend Const JobObjectLimitKillOnJobClose As UInteger = &H2000
    End Class
End Namespace
