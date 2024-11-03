Namespace Utilities.Interfaces

    ''' <summary>
    ''' Provides a method to clear the console screen.
    ''' </summary>
    ''' <remarks>
    ''' This interface defines the contract for clearing the console screen.
    ''' Implementations of this interface will provide the actual mechanism 
    ''' for resetting the console display.
    ''' </remarks>
    Public Interface IConsoleClearer

        ''' <summary>
        ''' Clears the console screen.
        ''' </summary>
        ''' <remarks>
        ''' This method clears all content currently displayed on the console screen,
        ''' providing a blank slate for subsequent output. This can be useful for 
        ''' improving the readability of the console output by removing prior text.
        ''' </remarks>
        Sub ClearConsole()
    End Interface
End Namespace
