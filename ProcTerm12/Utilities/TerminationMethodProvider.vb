Imports System.Text.RegularExpressions

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
        ''' The message prompting the user to select a termination method or exit.
        ''' </summary>
        Private Const SelectMethodPrompt As String = "Please select a number between 1 and 12 or 'E' to exit."

        ''' <summary>
        ''' The regular expression pattern for validating termination methods.
        ''' </summary>
        Private Const TerminationMethodPattern As String = "^(E|[1-9]|1[0-2])$"

        ''' <summary>
        ''' The compiled regular expression for validating termination methods with IgnoreCase and Compiled options.
        ''' </summary>
        Private ReadOnly _terminationMethodRegex As New Regex(TerminationMethodPattern, RegexOptions.IgnoreCase Or RegexOptions.Compiled)

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
        ''' This method prompts the user to choose a termination method (1 to 12) or exit (E) and validates the input to ensure it is within the allowed range.
        ''' </remarks>
        ''' <seealso cref="ITerminationMethodProvider.GetTerminationMethod"/>
        Friend Function GetTerminationMethod() As String Implements ITerminationMethodProvider.GetTerminationMethod
            Dim terminationMethod As String
            Do
                terminationMethod = PromptForTerminationMethod()
            Loop Until IsValidMethod(terminationMethod)
            If Not String.Equals(terminationMethod, "E", StringComparison.OrdinalIgnoreCase) Then
                StoreTerminationMethod(terminationMethod)
            End If
            Return terminationMethod
        End Function

        ''' <summary>
        ''' Prompts the user to select a termination method.
        ''' </summary>
        ''' <returns>
        ''' The termination method selected by the user.
        ''' </returns>
        Private Function PromptForTerminationMethod() As String
            Dim methods = GetTerminationMethods()
            DisplayTerminationMethods(methods)
            Return _inputReader.ReadInput()
        End Function

        ''' <summary>
        ''' Retrieves a list of termination methods.
        ''' </summary>
        ''' <returns>
        ''' A list of termination methods.
        ''' </returns>
        Private Shared Function GetTerminationMethods() As List(Of String)
            Return New List(Of String) From {
                "NtTerminateProcess - Terminates a process using the NtTerminateProcess native API.",
                "CreateRemoteThreadExitProcess - Creates a remote thread in the target process to execute ExitProcess.",
                "TerminateThread - Terminates each thread of the target process using TerminateThread.",
                "SetThreadContext - Terminates threads by modifying their context to point to ExitProcess.",
                "DuplicateHandle - Closes handles of the target process to invalidate resources.",
                "JobObjectMethods - Uses job object methods to terminate the target process.",
                "DebugObjectMethods - Uses a debug object to attach to a process and terminate it.",
                "VirtualQueryExNoAccess - Modifies memory protections to PAGE_NOACCESS to crash the process.",
                "WriteProcessMemory - Overwrites memory regions with random data to cause instability.",
                "VirtualAllocEx - Continuously allocates memory in the target process until it fails.",
                "PsTerminateProcess - Terminates a process using the internal PsTerminateProcess kernel function.",
                "PspTerminateThreadByPointer - Terminates threads using the non-exported PspTerminateThreadByPointer function."
            }
        End Function

        ''' <summary>
        ''' Displays the list of termination methods to the user.
        ''' </summary>
        ''' <param name="methods">The list of termination methods.</param>
        Private Sub DisplayTerminationMethods(methods As List(Of String))
            _prompter.Prompt(SelectMethodPrompt & Environment.NewLine)
            methods.Select(Function(method, index) $"{index + 1}. {method}").ToList().ForEach(Sub(prompt) _prompter.Prompt(prompt))
        End Sub

        ''' <summary>
        ''' Validates if the selected method is within the allowed range or is 'E' for exit.
        ''' </summary>
        ''' <param name="method">The selected termination method to validate.</param>
        ''' <returns>
        ''' <c>True</c> if the method is between 1 and 12 or 'E'; otherwise, <c>False</c>.
        ''' </returns>
        Private Function IsValidMethod(method As String) As Boolean
            Dim parts = method.Split(" "c)
            If parts.Length = 2 AndAlso Integer.TryParse(parts(0), Nothing) AndAlso Integer.TryParse(parts(1), Nothing) Then
                Return True
            ElseIf _terminationMethodRegex.IsMatch(method) Then
                Return True
            Else
                _prompter.Prompt($"Invalid method '{method}'. {SelectMethodPrompt}")
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
