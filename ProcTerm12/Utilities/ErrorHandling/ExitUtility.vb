Namespace Utilities.ErrorHandling

    ''' <summary>
    ''' Provides utility methods for exiting the application with specific error codes.
    ''' </summary>
    Friend Class ExitUtility
        Implements IExitUtility

        ''' <summary>
        ''' The error code used to indicate a general error.
        ''' </summary>
        Friend Const ErrorCode As Integer = 1

        ''' <summary>
        ''' Exits the application with the predefined error code.
        ''' </summary>
        ''' <remarks>
        ''' This method should be called to terminate the application 
        ''' gracefully, ensuring that any necessary cleanup occurs 
        ''' before the application exits. 
        ''' The <see cref="IExitUtility.ExitWithError"/> method is used to 
        ''' indicate a general error when exiting the application.
        ''' </remarks>
        Friend Sub ExitWithError() Implements IExitUtility.ExitWithError
            Environment.Exit(ErrorCode)
        End Sub
    End Class
End Namespace
