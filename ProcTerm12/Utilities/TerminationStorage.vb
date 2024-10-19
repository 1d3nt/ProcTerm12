Namespace Utilities

    ''' <summary>
    ''' Represents a singleton class that stores the selected termination method.
    ''' This class ensures that only one instance exists and provides global access to the selected termination method.
    ''' </summary>
    Friend Class TerminationStorage

        ''' <summary>
        ''' Private constructor to prevent external instantiation.
        ''' This ensures that the class remains a singleton.
        ''' </summary>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Gets the single instance of the <see cref="TerminationStorage"/> class.
        ''' This property provides a global point of access to the instance.
        ''' </summary>
        Friend Shared ReadOnly Property Instance As New TerminationStorage()

        ''' <summary>
        ''' Gets or sets the selected termination method.
        ''' This property holds the method used for process termination, which can be accessed and modified globally.
        ''' </summary>
        Friend Property TerminationMethod As String
    End Class
End Namespace
