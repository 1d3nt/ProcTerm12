Namespace CoreServices.WindowsApiInterop.Enums

    ''' <summary>
    ''' Represents the NTSTATUS values returned by various Windows API functions.
    ''' This enum is essential for handling errors and statuses in system-level programming.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="NtStatus"/> enum defines constants that correspond to NTSTATUS values used in the Windows API.
    ''' These values indicate the success or failure of operations and are integral for error handling.
    ''' 
    ''' The NTSTATUS values correspond to various system states and outcomes:
    ''' <list type="bullet">
    '''     <item><description><see cref="StatusSuccess"/> indicates successful operation.</description></item>
    ''' </list>
    ''' 
    ''' For detailed information about these NTSTATUS values, refer to:
    ''' <see href="https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-erref/596a1078-e883-4972-9bbc-49e60bebca55">NTSTATUS Documentation</see>.
    ''' Note: The <c>UsedImplicitly</c> attribute is applied to each member of the enum to suppress warnings about these members being unused. 
    ''' This ensures that the enum is kept complete for completeness and future extensibility, despite not being utilized directly in the current project.
    ''' </remarks>
    Friend Enum NtStatus As UInteger

        ''' <summary>
        ''' Indicates successful operation.
        ''' </summary>
        StatusSuccess = &H0
    End Enum
End Namespace
