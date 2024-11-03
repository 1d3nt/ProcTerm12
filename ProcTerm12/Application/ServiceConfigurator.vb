Namespace Application

    ''' <summary>
    ''' Configures the services for dependency injection.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="ServiceConfigurator"/> class provides methods to configure and register services
    ''' for dependency injection. It creates a new instance of the <see cref="ServiceCollection"/> class,
    ''' registers various services and their corresponding implementations, and builds an <see cref="IServiceProvider"/>
    ''' which can be used to resolve services at runtime.
    ''' </remarks>
    Friend Class ServiceConfigurator
        Implements IServiceConfigurator

        ''' <summary>
        ''' Registers all services required by the application.
        ''' </summary>
        ''' <param name="services">
        ''' The service collection to which the services will be added.
        ''' </param>
        ''' <remarks>
        ''' This method encapsulates the registration of various service categories essential for the application. 
        ''' It calls the following methods to register the respective services:
        ''' <list type="bullet">
        '''   <item>
        '''     <description>
        '''       <see cref="RegisterErrorHandlingServices"/> - Registers services for error handling.
        '''     </description>
        '''   </item>
        '''   <item>
        '''     <description>
        '''       <see cref="RegisterUserInputServices"/> - Registers services related to user input.
        '''     </description>
        '''   </item>
        '''   <item>
        '''     <description>
        '''       <see cref="RegisterAppRunner"/> - Registers the application runner service, responsible for executing the core logic of the application.
        '''     </description>
        '''   </item>
        '''   <item>
        '''     <description>
        '''       <see cref="RegisterTerminationMethodProvider"/> - Registers the termination method provider service for selecting termination methods.
        '''     </description>
        '''   </item>
        '''   <item>
        '''     <description>
        '''       <see cref="RegisterProcessTerminator"/> - Registers the process terminator service for handling process terminations.
        '''     </description>
        '''   </item>
        ''' </list>
        ''' </remarks>
        Private Shared Sub RegisterServices(services As IServiceCollection)
            RegisterErrorHandlingServices(services)
            RegisterUserInputServices(services)
            RegisterAppRunner(services)
            RegisterTerminationMethodProvider(services)
            RegisterProcessTerminator(services)
        End Sub

        ''' <summary>
        ''' Registers the application runner services.
        ''' </summary>
        ''' <param name="services">
        ''' The service collection to which the application runner services are added. This instance of <see cref="IServiceCollection"/> 
        ''' is used to register services and their implementations for dependency injection.
        ''' </param>
        ''' <remarks>
        ''' This method registers the following services:
        ''' <list type="bullet">
        '''   <item>
        '''     <description>
        '''       <see cref="IAppRunner"/> is registered as a transient service. This service is responsible for managing application runs and operations.
        '''     </description>
        '''   </item>
        ''' </list>
        ''' </remarks>
        ''' <seealso cref="AppRunner"/>
        ''' <seealso cref="IAppRunner"/>
        Private Shared Sub RegisterAppRunner(services As IServiceCollection)
            Dim appRunnerServices As New Dictionary(Of Type, Type) From {
                        {GetType(IAppRunner), GetType(AppRunner)},
                        {GetType(IProcessLauncher), GetType(ProcessLauncher)}
                    }
            AddServices(services, appRunnerServices)
        End Sub

        ''' <summary>
        ''' Registers error handling services.
        ''' </summary>
        ''' <param name="services">
        ''' The service collection to which the error handling services are added. This instance of <see cref="IServiceCollection"/> 
        ''' is used to register services and their implementations for dependency injection.
        ''' </param>
        ''' <remarks>
        ''' This method registers the following error handling services:
        ''' <list type="bullet">
        '''   <item>
        '''     <description>
        '''       <see cref="IExitUtility"/> is implemented by <see cref="ExitUtility"/>. This service provides methods for exiting the application 
        '''       with specific error codes.
        '''     </description>
        '''   </item>
        ''' </list>
        ''' </remarks>
        ''' <seealso cref="AddServices"/>
        ''' <seealso cref="IExitUtility"/>
        ''' <seealso cref="ExitUtility"/>
        Private Shared Sub RegisterErrorHandlingServices(services As IServiceCollection)
            Dim errorHandlingServices As New Dictionary(Of Type, Type) From {
                        {GetType(IExitUtility), GetType(ExitUtility)}
                    }
            AddServices(services, errorHandlingServices)
        End Sub

        ''' <summary>
        ''' Registers user input services.
        ''' </summary>
        ''' <param name="services">
        ''' The service collection to which the user input services are added. This instance of <see cref="IServiceCollection"/> 
        ''' is used to register services and their implementations for dependency injection.
        ''' </param>
        ''' <remarks>
        ''' This method registers the following user input services:
        ''' <list type="bullet">
        '''   <item>
        '''     <description>
        '''       <see cref="IUserInputReader"/> is implemented by <see cref="UserInputReader"/>. This service handles reading user inputs 
        '''       during interaction processes.
        '''     </description>
        '''   </item>
        '''   <item>
        '''     <description>
        '''       <see cref="IUserPrompter"/> is implemented by <see cref="UserPrompter"/>. This service prompts the user for inputs, 
        '''       typically used in setup tasks where user confirmation is needed.
        '''     </description>
        '''   </item>
        '''   <item>
        '''     <description>
        '''       <see cref="IConsoleClearer"/> is implemented by <see cref="ConsoleClearer"/>. This service clears the console.
        '''     </description>
        '''   </item>
        ''' </list>
        ''' </remarks>
        ''' <seealso cref="AddServices"/>
        ''' <seealso cref="IUserInputReader"/>
        ''' <seealso cref="UserInputReader"/>
        ''' <seealso cref="IUserPrompter"/>
        ''' <seealso cref="UserPrompter"/>
        ''' <seealso cref="IConsoleClearer"/>
        ''' <seealso cref="ConsoleClearer"/>
        Private Shared Sub RegisterUserInputServices(services As IServiceCollection)
            Dim userInputServices As New Dictionary(Of Type, Type) From {
                        {GetType(IUserInputReader), GetType(UserInputReader)},
                        {GetType(IUserPrompter), GetType(UserPrompter)},
                        {GetType(IConsoleClearer), GetType(ConsoleClearer)}
                    }
            AddServices(services, userInputServices)
        End Sub

        ''' <summary>
        ''' Registers termination method provider services.
        ''' </summary>
        ''' <param name="services">
        ''' The service collection to which the termination method provider services are added. This instance of <see cref="IServiceCollection"/>
        ''' is used to register services and their implementations for dependency injection.
        ''' </param>
        ''' <remarks>
        ''' This method registers the following termination method provider services:
        ''' <list type="bullet">
        '''   <item>
        '''     <description>
        '''       <see cref="ITerminationMethodProvider"/> is implemented by <see cref="TerminationMethodProvider"/>. This service allows 
        '''       the user to select a termination method and ensures the correctness of the selection.
        '''     </description>
        '''   </item>
        ''' </list>
        ''' 
        ''' The service is registered with a transient lifetime, meaning a new instance of <see cref="TerminationMethodProvider"/> will be created 
        ''' each time it is requested by the application. This ensures that termination method selection can be performed independently across 
        ''' different program components.
        ''' </remarks>
        ''' <seealso cref="ITerminationMethodProvider"/>
        ''' <seealso cref="TerminationMethodProvider"/>
        Private Shared Sub RegisterTerminationMethodProvider(services As IServiceCollection)
            Dim terminationMethodProviderServices As New Dictionary(Of Type, Type) From {
                        {GetType(ITerminationMethodProvider), GetType(TerminationMethodProvider)}
                    }
            AddServices(services, terminationMethodProviderServices)
        End Sub

        ''' <summary>
        ''' Registers the process terminator services.
        ''' </summary>
        ''' <param name="services">
        ''' The service collection to which the process terminator services are added. This instance of <see cref="IServiceCollection"/> 
        ''' is used to register services and their implementations for dependency injection.
        ''' </param>
        ''' <remarks>
        ''' This method registers the following services:
        ''' <list type="bullet">
        '''   <item>
        '''     <description>
        '''       <see cref="IProcessTerminator"/> is registered as a transient service. This service is responsible for handling process terminations.
        '''     </description>
        '''   </item>
        ''' </list>
        ''' </remarks>
        ''' <seealso cref="ProcessTerminator"/>
        ''' <seealso cref="IProcessTerminator"/>
        Private Shared Sub RegisterProcessTerminator(services As IServiceCollection)
            Dim processTerminatorServices As New Dictionary(Of Type, Type) From {
                        {GetType(IProcessTerminator), GetType(ProcessTerminator)}
                    }
            AddServices(services, processTerminatorServices)
        End Sub
        ''' <summary>
        ''' Adds the specified services to the service collection.
        ''' </summary>
        ''' <param name="services">
        ''' The service collection to which the services are added. This instance of <see cref="IServiceCollection"/> 
        ''' is used to register services and their implementations for dependency injection.
        ''' </param>
        ''' <param name="serviceRegistrations">
        ''' The dictionary containing service registrations, where each key represents a service type and each value 
        ''' represents its corresponding implementation type.
        ''' </param>
        ''' <remarks>
        ''' This method iterates over the provided dictionary of service registrations and adds each service
        ''' to the <paramref name="services"/> collection with a transient lifetime.
        ''' 
        ''' This method uses to add the services. 
        ''' Transient services are created each time they are requested, which is suitable for lightweight, stateless services.
        ''' </remarks>
        Private Shared Sub AddServices(services As IServiceCollection, serviceRegistrations As Dictionary(Of Type, Type))
            For Each kvp As KeyValuePair(Of Type, Type) In serviceRegistrations
                services.AddTransient(kvp.Key, kvp.Value)
            Next
        End Sub

        ''' <summary>
        ''' Configures the services for dependency injection.
        ''' </summary>
        ''' <returns>
        ''' An <see cref="IServiceProvider"/> that provides the configured services. This provider can be used to resolve services
        ''' at runtime.
        ''' </returns>
        ''' <remarks>
        ''' The <see cref="ConfigureServices"/> method creates a new instance of the <see cref="ServiceCollection"/>
        ''' and registers various services and their corresponding implementations by calling the <see cref="RegisterServices"/> method.
        ''' The method then builds and returns an <see cref="IServiceProvider"/> which can be used to resolve services at runtime.
        ''' 
        ''' The returned <see cref="IServiceProvider"/> is the main interface for accessing the configured services and is used 
        ''' throughout the application to resolve dependencies.
        ''' </remarks>
        Public Function ConfigureServices() As IServiceProvider Implements IServiceConfigurator.ConfigureServices
            Dim services As New ServiceCollection()
            RegisterServices(services)
            Return services.BuildServiceProvider()
        End Function
    End Class
End Namespace
