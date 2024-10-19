Namespace Utilities.ErrorHandling.Interfaces

    ''' <summary>
    ''' Defines methods for exiting the application with specific error codes.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="IExitUtility.ExitWithError"/> method is used to terminate 
    ''' the application with a predefined error code, ensuring that any necessary 
    ''' cleanup occurs before the application exits.
    ''' </remarks>
    Public Interface IExitUtility

        ''' <summary>
        ''' Exits the application with the predefined error code.
        ''' </summary>
        ''' <remarks>
        ''' This method should be called to terminate the application 
        ''' gracefully, ensuring that any necessary cleanup occurs 
        ''' before the application exits.
        ''' </remarks>
        Sub ExitWithError()
    End Interface
End Namespace
