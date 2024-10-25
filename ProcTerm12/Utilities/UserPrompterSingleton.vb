Namespace Utilities

    ''' <summary>
    ''' Singleton class to provide a single instance of IUserPrompter throughout the application.
    ''' </summary>
    Public Class UserPrompterSingleton

        ''' <summary>
        ''' The user prompter used for displaying messages to the user.
        ''' </summary>
        Private Shared _userPrompter As IUserPrompter

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
                If _userPrompter Is Nothing Then
                    _userPrompter = New UserPrompter()
                End If
                Return _userPrompter
            End Get
        End Property
    End Class
End Namespace
