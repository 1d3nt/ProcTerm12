Namespace Application.Interfaces

    ''' <summary>
    ''' Defines the contract for running the ProcTerm12 application.
    ''' </summary>
    ''' <remarks>
    ''' This interface defines the <see cref="IAppRunner.RunAsync"/> method, which is responsible for managing the execution 
    ''' of the process termination routines in the ProcTerm12 application. Implementations of this interface should 
    ''' encapsulate the logic required to execute process termination tasks asynchronously.
    ''' 
    ''' The application is designed to showcase multiple advanced methods for terminating processes, and the 
    ''' <see cref="IAppRunner.RunAsync"/> method should handle the coordination of these tasks.
    ''' </remarks>
    Public Interface IAppRunner

        ''' <summary>
        ''' Runs the application asynchronously.
        ''' </summary>
        ''' <returns>
        ''' A <see cref="Task"/> representing the asynchronous operation of process termination.
        ''' </returns>
        ''' <remarks>
        ''' This method contains the main logic for running the application asynchronously. It is responsible 
        ''' for managing the execution of the various process termination methods provided by the application. 
        ''' It is expected to be called at the application's entry point to start the termination operations and 
        ''' ensure that they are completed properly.
        ''' </remarks>
        Function RunAsync() As Task
    End Interface
End Namespace
