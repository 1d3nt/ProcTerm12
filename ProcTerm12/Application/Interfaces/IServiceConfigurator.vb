Namespace Application.Interfaces

    ''' <summary>
    ''' Defines the contract for configuring and registering services for dependency injection.
    ''' The <see cref="IServiceConfigurator.ConfigureServices"/> method is essential 
    ''' for setting up the necessary services required by the application to function properly. 
    ''' Implementations should ensure all dependencies are correctly registered for use at runtime.
    ''' </summary>
    Public Interface IServiceConfigurator

        ''' <summary>
        ''' Configures the services for dependency injection.
        ''' This method should include the registration of all necessary services required by the application.
        ''' </summary>
        ''' <returns>
        ''' An <see cref="IServiceProvider"/> that provides the configured services. 
        ''' This provider can be used to resolve services at runtime, enabling dependency injection throughout the application.
        ''' </returns>
        Function ConfigureServices() As IServiceProvider
    End Interface
End Namespace
