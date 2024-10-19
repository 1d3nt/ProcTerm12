Namespace Utilities.Interfaces

    ''' <summary>
    ''' Defines methods for providing termination methods for process termination.
    ''' </summary>
    Public Interface ITerminationMethodProvider

        ''' <summary>
        ''' Retrieves the termination method to be used for terminating a process.
        ''' </summary>
        ''' <returns>The termination method as a string.</returns>
        Function GetTerminationMethod() As String
    End Interface
End Namespace
