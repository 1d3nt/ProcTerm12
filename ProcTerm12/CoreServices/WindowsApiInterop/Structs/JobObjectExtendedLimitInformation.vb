Namespace CoreServices.WindowsApiInterop.Structs

    ''' <summary>
    ''' Contains basic and extended limit information for a job object.
    ''' </summary>
    ''' <remarks>
    ''' This structure corresponds to the <c>JOBOBJECT_EXTENDED_LIMIT_INFORMATION</c> structure in the Windows API:
    ''' <list type="bullet">
    ''' <item><description><see cref="JobObjectExtendedLimitInformation.BasicLimitInformation"/>: Basic limit information for the job object. In C++: <code>JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;</code></description></item>
    ''' <item><description><see cref="JobObjectExtendedLimitInformation.IoInfo"/>: Information about the input/output operations for the job object. In C++: <code>IO_COUNTERS IoInfo;</code></description></item>
    ''' <item><description><see cref="JobObjectExtendedLimitInformation.ProcessMemoryLimit"/>: The limit on the memory used by processes in the job. In C++: <code>SIZE_T ProcessMemoryLimit;</code></description></item>
    ''' <item><description><see cref="JobObjectExtendedLimitInformation.JobMemoryLimit"/>: The limit on the memory used by the job itself. In C++: <code>SIZE_T JobMemoryLimit;</code></description></item>
    ''' <item><description><see cref="JobObjectExtendedLimitInformation.PeakProcessMemoryUsed"/>: The peak memory used by processes in the job. In C++: <code>SIZE_T PeakProcessMemoryUsed;</code></description></item>
    ''' <item><description><see cref="JobObjectExtendedLimitInformation.PeakJobMemoryUsed"/>: The peak memory used by the job. In C++: <code>SIZE_T PeakJobMemoryUsed;</code></description></item>
    ''' </list>
    ''' 
    ''' For additional details on the <see cref="JobObjectExtendedLimitInformation"/> structure, refer to:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-jobobject_extended_limit_information">JOBOBJECT_EXTENDED_LIMIT_INFORMATION Structure</see>.
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
    <StructLayout(LayoutKind.Sequential)>
    Friend Structure JobObjectExtendedLimitInformation

        ''' <summary>
        ''' Basic limit information for the job object.
        ''' </summary>
        ''' <remarks>
        ''' This member contains the basic limit information that defines the constraints for processes in the job.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;</code>
        ''' </example>
        Friend BasicLimitInformation As JobObjectBasicLimitInformation

        ''' <summary>
        ''' Information about the input/output operations for the job object.
        ''' </summary>
        ''' <remarks>
        ''' This member provides counters for the I/O operations that have occurred in the job.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>IO_COUNTERS IoInfo;</code>
        ''' </example>
        Friend IoInfo As IoCounters

        ''' <summary>
        ''' The limit on the memory used by processes in the job.
        ''' </summary>
        ''' <remarks>
        ''' This member specifies the maximum amount of memory that can be used by all processes in the job combined.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>SIZE_T ProcessMemoryLimit;</code>
        ''' </example>
        Friend ProcessMemoryLimit As UIntPtr

        ''' <summary>
        ''' The limit on the memory used by the job itself.
        ''' </summary>
        ''' <remarks>
        ''' This member sets the maximum memory limit that can be used by the job as a whole.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>SIZE_T JobMemoryLimit;</code>
        ''' </example>
        Friend JobMemoryLimit As UIntPtr

        ''' <summary>
        ''' The peak memory used by processes in the job.
        ''' </summary>
        ''' <remarks>
        ''' This member tracks the highest amount of memory used by any process in the job.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>SIZE_T PeakProcessMemoryUsed;</code>
        ''' </example>
        Friend PeakProcessMemoryUsed As UIntPtr

        ''' <summary>
        ''' The peak memory used by the job.
        ''' </summary>
        ''' <remarks>
        ''' This member records the highest amount of memory used by the job at any time.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>SIZE_T PeakJobMemoryUsed;</code>
        ''' </example>
        Friend PeakJobMemoryUsed As UIntPtr
    End Structure
End Namespace
