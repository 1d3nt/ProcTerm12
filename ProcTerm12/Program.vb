''' <author>
''' Sam (ident)
''' Twitter: <see href="https://twitter.com/1d3nt">https://twitter.com/1d3nt</see>
''' GitHub: <see href="https://github.com/1d3nt">https://github.com/1d3nt</see>
''' Email: <see href="mailto:ident@simplecoders.com">ident@simplecoders.com</see>
''' VBForums: <see href="https://www.vbforums.com/member.php?113656-ident">https://www.vbforums.com/member.php?113656-ident</see>
''' ORCID: <see href="https://orcid.org/0009-0007-1363-3308">https://orcid.org/0009-0007-1363-3308</see>
''' </author>
''' <date>17/10/2024</date>
''' <version>1.0.0</version>
''' <license>Creative Commons Attribution 4.0 International (CC BY 4.0) - See LICENSE.md for details</license>
''' <summary>
''' The entry point for the ProcTerm12 application.
''' This application provides various advanced methods for terminating processes
''' using both standard and Windows API-based techniques.
''' </summary>
''' <remarks>
''' This module contains the <see cref="Main"/> method, which serves as the entry point 
''' for the application. It manages user input and controls the flow of the process 
''' termination operations.
''' 
''' The application is designed to showcase 12 different ways of terminating processes, 
''' including techniques using P/Invoke for system-level interaction. It allows users 
''' to terminate processes by name, PID, and more advanced methods.
''' 
''' Contributions are welcome to extend its functionality or integrate additional process 
''' management techniques. This project is for learning and exploring system-level API calls.
''' 
''' Enjoy terminating processes with care!
'''
''' Just a hobby programmer that enjoys P/Invoke and exploring complex interactions with system-level APIs.
''' My mallory x
''' </remarks>
Module Program

    ''' <summary>
    ''' Entry point for the ProcTerm12 console application. 
    ''' Manages the user interactions for configuring and executing process termination tasks.
    ''' </summary>
    ''' <param name="args">Command-line arguments (not used in this implementation).</param>
    ''' <remarks>
    ''' The <paramref name="args"/> parameter is present to follow the standard signature of a console 
    ''' application's <see cref="Main"/> method. While not used here, it adheres to best practices.
    ''' 
    ''' This method initializes necessary services through <see cref="ServiceConfigurator.ConfigureServices"/> 
    ''' and passes them into <see cref="AppRunner"/>, which handles the core logic for executing the process 
    ''' termination tasks.
    ''' 
    ''' It synchronously waits for the completion of the core logic by calling 
    ''' <see cref="M:System.Threading.Tasks.Task.GetAwaiter().GetResult"/>, ensuring that the application 
    ''' terminates only after all tasks are completed.
    ''' 
    ''' <para>
    ''' The variable <c>serviceProvider</c> is responsible for managing services.
    ''' </para>
    ''' <para>
    ''' The <c>appRunner</c> variable orchestrates the core logic of the process termination operations.
    ''' </para>
    ''' </remarks>

    <SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification:="Standard Main method parameter signature.")>
    Sub Main(args As String())
        Dim serviceProvider As IServiceProvider = ConfigureServices()
        Dim userPrompter = serviceProvider.GetService(Of IUserPrompter)()
        Dim userInputReader = serviceProvider.GetService(Of IUserInputReader)()
        Dim appRunner = serviceProvider.GetService(Of IAppRunner)()
        If appRunner Is Nothing Then
            HandleAppRunnerResolutionFailure(serviceProvider, userPrompter, userInputReader)
        End If
        RunApplication(appRunner, userPrompter)
    End Sub

    ''' <summary>
    ''' Configures services for dependency injection, including services for both the main application and the VbWorkerServiceDeployer.
    ''' </summary>
    ''' <returns>
    ''' An instance of <see cref="IServiceProvider"/> configured with the necessary services from both the main application and the deployer.
    ''' </returns>
    Private Function ConfigureServices() As IServiceProvider
        Dim mainServiceConfigurator As New ServiceConfigurator()
        Dim serviceProvider As IServiceProvider = mainServiceConfigurator.ConfigureServices()
        Return serviceProvider
    End Function

    ''' <summary>
    ''' Handles the failure to resolve the <see cref="IAppRunner"/> instance.
    ''' Prompts the user with an error message, reads input, and exits the application with an error code.
    ''' </summary>
    ''' <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    ''' <param name="userPrompter">The user prompter used to display messages to the user.</param>
    ''' <param name="userInputReader">The user input reader used to read user input.</param>
    Private Sub HandleAppRunnerResolutionFailure(serviceProvider As IServiceProvider, userPrompter As IUserPrompter, userInputReader As IUserInputReader)
        userPrompter.Prompt("Failed to resolve IAppRunner.")
        userInputReader.ReadInput()
        Dim exitUtility = serviceProvider.GetService(Of IExitUtility)()
        exitUtility.ExitWithError()
    End Sub

    ''' <summary>
    ''' Runs the application and handles any exceptions that occur during execution.
    ''' </summary>
    ''' <param name="appRunner">The application runner used to run the application.</param>
    ''' <param name="userPrompter">The user prompter used to display messages to the user.</param>
    Private Sub RunApplication(appRunner As IAppRunner, userPrompter As IUserPrompter)
        Try
            appRunner.RunAsync().GetAwaiter().GetResult()
        Catch ex As Exception
            userPrompter.Prompt($"An error occurred: {ex.Message}")
        End Try
    End Sub
End Module
