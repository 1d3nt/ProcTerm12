Namespace Utilities

    ''' <summary>
    ''' Provides methods for selecting a termination method by prompting the user for input.
    ''' </summary>
    ''' <remarks>
    ''' This class implements the <see cref="ITerminationMethodProvider"/> interface and is responsible for prompting the user 
    ''' to select a termination method and storing the chosen method for process termination.
    ''' </remarks>
    ''' <seealso cref="ITerminationMethodProvider"/>
    Friend Class TerminationMethodProvider
        Implements ITerminationMethodProvider

        ''' <summary>
        ''' The input reader used to read user input.
        ''' </summary>
        ''' <remarks>
        ''' This field is initialized via dependency injection and is used to read user input from the console or other input sources.
        ''' </remarks>
        Private ReadOnly _inputReader As IUserInputReader

        ''' <summary>
        ''' The prompter used to display messages to the user.
        ''' </summary>
        ''' <remarks>
        ''' This field is initialized via dependency injection and is used to display messages to the user.
        ''' </remarks>
        Private ReadOnly _prompter As IUserPrompter

        ''' <summary>
        ''' Initializes a new instance of the <see cref="TerminationMethodProvider"/> class.
        ''' </summary>
        ''' <param name="inputReader">The input reader to use for reading user input.</param>
        ''' <param name="prompter">The prompter to use for displaying messages to the user.</param>
        ''' <remarks>
        ''' The constructor initializes the fields necessary for reading user input and prompting the user.
        ''' Both dependencies are injected to decouple input handling and prompting from the termination method selection logic.
        ''' </remarks>
        Public Sub New(inputReader As IUserInputReader, prompter As IUserPrompter)
            _inputReader = inputReader
            _prompter = prompter
        End Sub

        ''' <summary>
        ''' Prompts the user to select a termination method.
        ''' </summary>
        ''' <returns>
        ''' The selected termination method entered by the user.
        ''' </returns>
        ''' <remarks>
        ''' This method prompts the user to choose a termination method (1 to 12) and validates the input to ensure it is within the allowed range.
        ''' </remarks>
        ''' <seealso cref="ITerminationMethodProvider.GetTerminationMethod"/>
        Friend Function GetTerminationMethod() As String Implements ITerminationMethodProvider.GetTerminationMethod
            Dim terminationMethod As String
            Do
                terminationMethod = PromptForTerminationMethod()
            Loop Until IsValidMethod(terminationMethod)
            StoreTerminationMethod(terminationMethod)
            Return terminationMethod
        End Function

        ''' <summary>
        ''' Prompts the user to select a termination method.
        ''' </summary>
        ''' <returns>
        ''' The termination method selected by the user.
        ''' </returns>
        Private Function PromptForTerminationMethod() As String
            _prompter.Prompt("Please select a termination method (1-12):")
            Return _inputReader.ReadInput()
        End Function

        ''' <summary>
        ''' Validates if the selected method is within the allowed range.
        ''' </summary>
        ''' <param name="method">The selected termination method to validate.</param>
        ''' <returns>
        ''' <c>True</c> if the method is between 1 and 12; otherwise, <c>False</c>.
        ''' </returns>
        Private Function IsValidMethod(method As String) As Boolean
            Dim methodNumber As Integer
            If Integer.TryParse(method, methodNumber) AndAlso methodNumber >= 1 AndAlso methodNumber <= 12 Then
                Return True
            Else
                _prompter.Prompt($"Invalid method '{method}'. Please select a number between 1 and 12.")
                Return False
            End If
        End Function

        ''' <summary>
        ''' Stores the selected termination method in the <see cref="TerminationStorage"/> singleton.
        ''' </summary>
        ''' <param name="method">The selected termination method to store.</param>
        Private Shared Sub StoreTerminationMethod(method As String)
            TerminationStorage.Instance.TerminationMethod = method
        End Sub
    End Class
End Namespace
