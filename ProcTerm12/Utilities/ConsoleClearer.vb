Namespace Utilities

    ''' <summary>
    ''' Provides a method to clear the console screen.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="ConsoleClearer"/> class implements the <see cref="IConsoleClearer"/> interface
    ''' and is used to clear the console screen, providing a clean display for further interactions.
    ''' </remarks>
    ''' <seealso cref="IConsoleClearer"/>
    Friend Class ConsoleClearer
        Implements IConsoleClearer

        ''' <summary>
        ''' Clears the console screen.
        ''' </summary>
        ''' <remarks>
        ''' This method clears all content currently displayed on the console screen,
        ''' offering a blank canvas for future output. This is particularly helpful 
        ''' for removing prior console output, improving user experience.
        ''' </remarks>
        Friend Sub ClearConsole() Implements IConsoleClearer.ClearConsole
            Console.Clear()
        End Sub
    End Class
End Namespace
