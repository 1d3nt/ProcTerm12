Namespace CoreServices.WindowsApiInterop.Structs

    ''' <summary>
    ''' Contains basic limit information for a job object.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="JobObjectBasicLimitInformation"/> structure provides limit information for 
    ''' job objects, which allows applications to manage processes as a single unit.
    ''' 
    ''' This structure corresponds to the <c>JOBOBJECT_BASIC_LIMIT_INFORMATION</c> structure in the Windows API:
    ''' <list type="bullet">
    ''' <item><description><see cref="JobObjectBasicLimitInformation.PerProcessUserTimeLimit"/>: The limit on the total user time for all processes in the job. In C++: <code>LARGE_INTEGER PerProcessUserTimeLimit;</code></description></item>
    ''' <item><description><see cref="JobObjectBasicLimitInformation.PerJobUserTimeLimit"/>: The limit on the total user time for the job itself. In C++: <code>LARGE_INTEGER PerJobUserTimeLimit;</code></description></item>
    ''' <item><description><see cref="JobObjectBasicLimitInformation.LimitFlags"/>: Flags that specify the limits on the job. In C++: <code>DWORD LimitFlags;</code></description></item>
    ''' <item><description><see cref="JobObjectBasicLimitInformation.MinimumWorkingSetSize"/>: The minimum working set size for processes in the job. In C++: <code>SIZE_T MinimumWorkingSetSize;</code></description></item>
    ''' <item><description><see cref="JobObjectBasicLimitInformation.MaximumWorkingSetSize"/>: The maximum working set size for processes in the job. In C++: <code>SIZE_T MaximumWorkingSetSize;</code></description></item>
    ''' <item><description><see cref="JobObjectBasicLimitInformation.ActiveProcessLimit"/>: The maximum number of active processes in the job. In C++: <code>DWORD ActiveProcessLimit;</code></description></item>
    ''' <item><description><see cref="JobObjectBasicLimitInformation.Affinity"/>: A process affinity mask that specifies the processors on which the threads in the job can run. In C++: <code>ULONG_PTR Affinity;</code></description></item>
    ''' <item><description><see cref="JobObjectBasicLimitInformation.PriorityClass"/>: The priority class for processes in the job. In C++: <code>DWORD PriorityClass;</code></description></item>
    ''' <item><description><see cref="JobObjectBasicLimitInformation.SchedulingClass"/>: The scheduling class for processes in the job. In C++: <code>DWORD SchedulingClass;</code></description></item>
    ''' </list>
    ''' 
    ''' The <see cref="JobObjectBasicLimitInformation"/> structure uses <c>LayoutKind.Sequential</c> to ensure that the fields are laid out
    ''' in the same order as defined in the structure. This ensures compatibility with the unmanaged API.
    ''' 
    ''' <example>
    ''' In C++:
    ''' <code>
    ''' typedef struct _JOBOBJECT_BASIC_LIMIT_INFORMATION {
    '''     LARGE_INTEGER PerProcessUserTimeLimit;
    '''     LARGE_INTEGER PerJobUserTimeLimit;
    '''     DWORD         LimitFlags;
    '''     SIZE_T        MinimumWorkingSetSize;
    '''     SIZE_T        MaximumWorkingSetSize;
    '''     DWORD         ActiveProcessLimit;
    '''     ULONG_PTR     Affinity;
    '''     DWORD         PriorityClass;
    '''     DWORD         SchedulingClass;
    ''' } JOBOBJECT_BASIC_LIMIT_INFORMATION, *PJOBOBJECT_BASIC_LIMIT_INFORMATION;
    ''' </code>
    ''' </example>
    ''' 
    ''' For additional details on <see cref="JobObjectBasicLimitInformation"/>, refer to:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-jobobject_basic_limit_information">JOBOBJECT_BASIC_LIMIT_INFORMATION Structure</see>.
    ''' </remarks>
    <StructLayout(LayoutKind.Sequential)>
    Friend Structure JobObjectBasicLimitInformation

        ''' <summary>
        ''' The limit on the total user time for all processes in the job.
        ''' </summary>
        ''' <remarks>
        ''' This member is initialized to zero, and is specified in units of 
        ''' 100-nanosecond intervals.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>LARGE_INTEGER PerProcessUserTimeLimit;</code>
        ''' </example>
        Friend PerProcessUserTimeLimit As Long

        ''' <summary>
        ''' The limit on the total user time for the job itself.
        ''' </summary>
        ''' <remarks>
        ''' This member is initialized to zero, and is specified in units of 
        ''' 100-nanosecond intervals.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>LARGE_INTEGER PerJobUserTimeLimit;</code>
        ''' </example>
        Friend PerJobUserTimeLimit As Long

        ''' <summary>
        ''' Flags that specify the limits on the job.
        ''' </summary>
        ''' <remarks>
        ''' This member can specify one or more of the <see cref="JobObjectLimitFlags"/> values.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>DWORD LimitFlags;</code>
        ''' </example>
        Friend LimitFlags As UInteger

        ''' <summary>
        ''' The minimum working set size for processes in the job.
        ''' </summary>
        ''' <remarks>
        ''' If this member is set to zero, there is no minimum working set size.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>SIZE_T MinimumWorkingSetSize;</code>
        ''' </example>
        Friend MinimumWorkingSetSize As IntPtr

        ''' <summary>
        ''' The maximum working set size for processes in the job.
        ''' </summary>
        ''' <remarks>
        ''' If this member is set to zero, there is no maximum working set size.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>SIZE_T MaximumWorkingSetSize;</code>
        ''' </example>
        Friend MaximumWorkingSetSize As IntPtr

        ''' <summary>
        ''' The maximum number of active processes in the job.
        ''' </summary>
        ''' <remarks>
        ''' If this member is set to zero, there is no limit on the number of active processes.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>DWORD ActiveProcessLimit;</code>
        ''' </example>
        Friend ActiveProcessLimit As UInteger

        ''' <summary>
        ''' A process affinity mask that specifies the processors on which the threads in the job can run.
        ''' </summary>
        ''' <remarks>
        ''' If this member is set to zero, there are no restrictions on processor usage.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>ULONG_PTR Affinity;</code>
        ''' </example>
        Friend Affinity As UIntPtr

        ''' <summary>
        ''' The priority class for processes in the job.
        ''' </summary>
        ''' <remarks>
        ''' This member can be set to one of the priority class values 
        ''' defined in the Windows API.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>DWORD PriorityClass;</code>
        ''' </example>
        Friend PriorityClass As UInteger

        ''' <summary>
        ''' The scheduling class for processes in the job.
        ''' </summary>
        ''' <remarks>
        ''' This member can be set to one of the scheduling class values 
        ''' defined in the Windows API.
        ''' </remarks>
        ''' <example>
        ''' In C++: <code>DWORD SchedulingClass;</code>
        ''' </example>
        Friend SchedulingClass As UInteger
    End Structure
End Namespace
