Namespace Utilities

    ''' <summary>
    ''' Singleton class to provide a single instance of IUserPrompter throughout the application.
    ''' </summary>
    Public Class UserPrompterSingleton

        ''' <summary>
        ''' The user prompter used for displaying messages to the user.
        ''' </summary>
        Private Shared ReadOnly InstanceValue As New Lazy(Of IUserPrompter)(Function() New UserPrompter())

        ''' <summary>
        ''' Private constructor to prevent instantiation.
        ''' </summary>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Gets the single instance of IUserPrompter.
        ''' </summary>
        ''' <returns>The single instance of IUserPrompter.</returns>
        Public Shared ReadOnly Property Instance As IUserPrompter
            Get
                Return InstanceValue.Value
            End Get
        End Property
    End Class
End Namespace
