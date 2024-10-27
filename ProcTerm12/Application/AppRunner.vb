Namespace Application

    ''' <summary>
    ''' Represents an application that uses dependency injection to obtain services and perform operations.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="AppRunner"/> class relies on dependency injection to obtain an <see cref="IServiceProvider"/> 
    ''' which is used to retrieve services throughout the application. The main functionality of the class is to
    ''' run the application's core logic, including installing and uninstalling services.
    ''' </remarks>
    Friend Class AppRunner
        Implements IAppRunner

        ''' <summary>
        ''' Dictionary mapping termination methods to action delegates for handling process termination.
        ''' </summary>
        ''' <remarks>
        ''' This dictionary associates each <see cref="TerminationMethods"/> enum value with a corresponding handler method.
        ''' Each handler method is responsible for executing the specific process termination logic associated with its termination method.
        ''' </remarks>
        ''' <list type="bullet">
        ''' <item>
        '''     <term><see cref="TerminationMethods.NtTerminateProcess"/></term>
        '''     <description>Handles the termination of a process using the NtTerminateProcess method.</description>
        ''' </item>
        ''' <item>
        '''     <term><see cref="TerminationMethods.CreateRemoteThreadExitProcess"/></term>
        '''     <description>Handles termination by creating a remote thread to execute ExitProcess.</description>
        ''' </item>
        ''' <item>
        '''     <term><see cref="TerminationMethods.TerminateThread"/></term>
        '''     <description>Handles the termination of a specific thread within a process using TerminateThread.</description>
        ''' </item>
        ''' <item>
        '''     <term><see cref="TerminationMethods.SetThreadContext"/></term>
        '''     <description>Handles termination by modifying the execution context of its threads using SetThreadContext.</description>
        ''' </item>
        ''' <item>
        '''     <term><see cref="TerminationMethods.DuplicateHandle"/></term>
        '''     <description>Handles termination by closing handles using DuplicateHandle.</description>
        ''' </item>
        ''' <item>
        '''     <term><see cref="TerminationMethods.JobObjectMethods"/></term>
        '''     <description>Handles termination using job objects for grouped process management.</description>
        ''' </item>
        ''' <item>
        '''     <term><see cref="TerminationMethods.DebugObjectMethods"/></term>
        '''     <description>Handles termination using debugging techniques.</description>
        ''' </item>
        ''' <item>
        '''     <term><see cref="TerminationMethods.VirtualQueryExNoAccess"/></term>
        '''     <description>Handles termination by setting memory protections to PAGE_NOACCESS.</description>
        ''' </item>
        ''' <item>
        '''     <term><see cref="TerminationMethods.WriteProcessMemory"/></term>
        '''     <description>Handles termination by writing random data to the process memory.</description>
        ''' </item>
        ''' <item>
        '''     <term><see cref="TerminationMethods.VirtualAllocEx"/></term>
        '''     <description>Handles termination by exhausting memory allocation attempts in the target process.</description>
        ''' </item>
        ''' <item>
        '''     <term><see cref="TerminationMethods.PsTerminateProcess"/></term>
        '''     <description>Handles termination using PsTerminateProcess for process termination.</description>
        ''' </item>
        ''' <item>
        '''     <term><see cref="TerminationMethods.PspTerminateThreadByPointer"/></term>
        '''     <description>Handles termination by terminating a thread pointed to by a pointer.</description>
        ''' </item>
        ''' </list>
        Private ReadOnly _terminationMethodsActions As New Dictionary(Of TerminationMethods, Func(Of Task)) From {
            {TerminationMethods.NtTerminateProcess, AddressOf NtTerminateProcessHandler},
            {TerminationMethods.CreateRemoteThreadExitProcess, AddressOf CreateRemoteThreadExitProcessHandler},
            {TerminationMethods.TerminateThread, AddressOf TerminateThreadHandler},
            {TerminationMethods.DuplicateHandle, AddressOf DuplicateHandleHandler}
        }
        ',

        '{TerminationMethods.SetThreadContext, AddressOf SetThreadContextHandler},
        '
        '{TerminationMethods.JobObjectMethods, AddressOf JobObjectMethodsHandler},
        '{TerminationMethods.DebugObjectMethods, AddressOf DebugObjectMethodsHandler},
        '{TerminationMethods.VirtualQueryExNoAccess, AddressOf VirtualQueryExNoAccessHandler},
        '{TerminationMethods.WriteProcessMemory, AddressOf WriteProcessMemoryHandler},
        '{TerminationMethods.VirtualAllocEx, AddressOf VirtualAllocExHandler},
        '{TerminationMethods.PsTerminateProcess, AddressOf PsTerminateProcessHandler},
        '{TerminationMethods.PspTerminateThreadByPointer, AddressOf PspTerminateThreadByPointerHandler}

        ''' <summary>
        ''' The user prompter used for displaying messages to the user.
        ''' </summary>
        Private ReadOnly _userPrompter As IUserPrompter

        ''' <summary>
        ''' Provides the termination method to be used for process termination.
        ''' </summary>
        Private ReadOnly _terminationMethodProvider As ITerminationMethodProvider

        ''' <summary>
        ''' Provides the functionality to launch processes.
        ''' </summary>
        Private ReadOnly _processLauncher As IProcessLauncher

        ''' <summary>
        ''' Handles the termination of processes using various methods.
        ''' </summary>
        Private ReadOnly _processTerminator As IProcessTerminator

        ''' <summary>
        ''' Initializes a new instance of the <see cref="AppRunner"/> class and injects the required dependencies.
        ''' </summary>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used to display messages to the user.</param>
        ''' <param name="terminationMethodProvider">The provider used to retrieve the termination method.</param>
        ''' <param name="processLauncher">The launcher used to start processes.</param>
        ''' <param name="processTerminator">The terminator used to handle process terminations.</param>
        Public Sub New(userPrompter As IUserPrompter, terminationMethodProvider As ITerminationMethodProvider, processLauncher As IProcessLauncher, processTerminator As IProcessTerminator)
            _userPrompter = userPrompter
            _terminationMethodProvider = terminationMethodProvider
            _processLauncher = processLauncher
            _processTerminator = processTerminator
        End Sub

        ''' <summary>
        ''' Runs the application asynchronously.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        ''' <remarks>
        ''' This method contains the main logic for running the application. It retrieves the termination method and prompts the user.
        ''' </remarks>
        Friend Async Function RunAsync() As Task Implements IAppRunner.RunAsync
            Dim terminationMethodNumber = GetTerminationMethod()
            LaunchProcess()
            PromptUser(terminationMethodNumber)
            Await DelayBeforeKillingProcess()
            Await ExecuteTerminationMethod(terminationMethodNumber)
        End Function

        ''' <summary>
        ''' Launches a process using the provided process launcher.
        ''' </summary>
        ''' <remarks>
        ''' This method encapsulates the logic required to start a process using the injected <see cref="IProcessLauncher"/> instance.
        ''' The process launcher is expected to handle the initialization and management of the process.
        ''' </remarks>
        Private Sub LaunchProcess()
            _processLauncher.LaunchProcess()
        End Sub

        ''' <summary>
        ''' Gets the termination method number.
        ''' </summary>
        ''' <returns>The termination method number to be used.</returns>
        Private Function GetTerminationMethod() As Integer
            Dim terminationMethodString As String = _terminationMethodProvider.GetTerminationMethod()
            Dim terminationMethodNumber As Integer
            If Not Integer.TryParse(terminationMethodString, terminationMethodNumber) Then
                Throw New InvalidOperationException($"Invalid termination method number: '{terminationMethodString}'")
            End If
            Return terminationMethodNumber
        End Function

        ''' <summary>
        ''' Prompts the user with the termination method number.
        ''' </summary>
        ''' <param name="terminationMethodNumber">The number of the termination method.</param>
        Private Sub PromptUser(terminationMethodNumber As Integer)
            _userPrompter.Prompt($"Attempting to execute termination method number '{terminationMethodNumber}'.")
        End Sub

        ''' <summary>
        ''' Introduces a delay before attempting to kill the process.
        ''' </summary>
        ''' <returns>
        ''' A task that represents the asynchronous operation.
        ''' </returns>
        ''' <remarks>
        ''' The <see cref="DelayBeforeKillingProcess"/> method prompts the user about the delay duration and then simulates a delay
        ''' before proceeding to attempt to kill the specified process. This delay allows for any necessary preparation 
        ''' before the process is terminated.
        ''' </remarks>
        Private Async Function DelayBeforeKillingProcess() As Task
            Const delayMilliseconds = 5000
            PromptUserAboutDelay(delayMilliseconds)
            Await AsynchronousProcessor.SimulateDelayedResponse(delayMilliseconds)
        End Function

        ''' <summary>
        ''' Prompts the user about the delay duration.
        ''' </summary>
        ''' <param name="delayMilliseconds">The delay duration in milliseconds.</param>
        ''' <remarks>
        ''' The <see cref="PromptUserAboutDelay"/> method uses the <see cref="IUserPrompter"/> service to notify the user about
        ''' the delay before attempting to kill the specified process.
        ''' </remarks>
        Private Sub PromptUserAboutDelay(delayMilliseconds As Integer)
            _userPrompter.Prompt($"The process will wait for {delayMilliseconds / 1000} seconds before proceeding to attempt to kill it.")
        End Sub

        ''' <summary>
        ''' Executes the termination method based on the provided termination method number.
        ''' </summary>
        ''' <param name="terminationMethodNumber">The number representing the termination method to be executed.</param>
        ''' <remarks>
        ''' This method converts the termination method number to the corresponding <see cref="TerminationMethods"/> enum value.
        ''' It then checks if the termination method exists in the <see cref="_terminationMethodsActions"/> dictionary and invokes the associated action asynchronously.
        ''' If the termination method is not found, it prompts the user with an invalid selection message.
        ''' </remarks>
        Private Async Function ExecuteTerminationMethod(terminationMethodNumber As Integer) As Task
            Dim terminationMethod = DirectCast(terminationMethodNumber, TerminationMethods)
            Dim action As Func(Of Task) = Nothing
            If _terminationMethodsActions.TryGetValue(terminationMethod, action) Then
                Await action.Invoke()
            Else
                _userPrompter.Prompt("Invalid termination method selected.")
            End If
        End Function

        ''' <summary>
        ''' Handles the termination of a process using the NtTerminateProcess method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Private Async Function NtTerminateProcessHandler() As Task
            Await _processTerminator.NtTerminateProcessHandler()
        End Function

        ''' <summary>
        ''' Handles the termination of a process using the CreateRemoteThread method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Private Async Function CreateRemoteThreadExitProcessHandler() As Task
            Await _processTerminator.CreateRemoteThreadExitProcessHandler()
        End Function

        ''' <summary>
        ''' Handles the termination of a process by terminating each thread using the TerminateThread method.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Private Async Function TerminateThreadHandler() As Task
            Await _processTerminator.TerminateThreadHandler()
        End Function

        ''' <summary>
        ''' Handles the termination of a process by duplicating the handle and performing necessary operations to terminate the process.
        ''' </summary>
        ''' <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Private Async Function DuplicateHandleHandler() As Task
            Await _processTerminator.DuplicateHandleHandler()
        End Function
    End Class
End Namespace
