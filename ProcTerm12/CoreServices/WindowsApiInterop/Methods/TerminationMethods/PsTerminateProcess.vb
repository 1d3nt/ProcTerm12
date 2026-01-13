Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods
    Friend Class PsTerminateProcess
        ''' <summary>
        ''' Attempts to terminate a process using the PsTerminateProcess method.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to terminate.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the process was terminated successfully; otherwise, false.</returns>
        ''' <remarks>
        ''' This method relies on the internal kernel-mode function PsTerminateProcess, 
        ''' which is not exported by ntoskrnl and cannot be invoked in user-mode or on modern 
        ''' operating systems such as Windows 11. Using this method in user-mode is unsupported 
        ''' and may result in undefined behavior or system instability.
        ''' </remarks>
        Friend Shared Function Kill(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            ' Implementation goes here
            Return False
        End Function
    End Class
End Namespace
