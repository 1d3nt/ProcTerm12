Namespace Utilities.ProcessUtilities

    ''' <summary>
    ''' Provides utility methods for process management.
    ''' </summary>
    ''' <remarks>
    ''' This class currently uses built-in .NET Framework methods for process management because, frankly, 
    ''' I can’t be bothered to implement the P/Invoke equivalents at this stage. While the .NET Framework 
    ''' methods simplify access to process information, handle management, and control operations, they are 
    ''' merely placeholders for now.
    '''
    ''' Eventually, this class may be adapted to use direct P/Invoke calls to achieve finer control over unmanaged 
    ''' resources and for more direct interaction with low-level process operations. For typical process interactions, 
    ''' the managed methods provide adequate functionality with minimal setup, but P/Invoke implementations will replace 
    ''' them when required for specific control.
    ''' </remarks>

    Friend NotInheritable Class ProcessUtility

        ''' <summary>
        ''' Prevents a default instance of the <see cref="ProcessUtility"/> class from being created.
        ''' </summary>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Retrieves the handle of the first running Notepad process with the required access rights.
        ''' </summary>
        ''' <returns>
        ''' A handle to the first Notepad process if it is running; otherwise, <see cref="NativeMethods.NullHandleValue"/>.
        ''' </returns>
        ''' <remarks>
        ''' This method searches for all instances of Notepad and returns the handle of the first one found.
        ''' If no Notepad process is running, <see cref="NativeMethods.NullHandleValue"/> is returned.
        ''' 
        ''' The process handle is opened with all available access rights using <see cref="ProcessAccessRights.All"/>:
        ''' - <see cref="ProcessAccessRights.All"/> (&H1F0FFF): Grants all available access rights.
        ''' 
        ''' Initially, the method attempted to use the minimal set of rights needed for the operations:
        ''' <code>
        ''' Const accessRights As ProcessAccessRights = ProcessAccessRights.Terminate Or ProcessAccessRights.QueryInformation Or ProcessAccessRights.Synchronize Or ProcessAccessRights.VirtualMemoryRead Or ProcessAccessRights.VirtualMemoryOperation Or ProcessAccessRights.VirtualMemoryWrite
        ''' </code>
        ''' However, this combination failed to work as expected. After thorough testing, it was determined that specifying all access rights individually did not provide the necessary permissions for the operations.
        ''' 
        ''' To resolve this issue, the method now uses <see cref="ProcessAccessRights.All"/>, which grants all available access rights:
        ''' <code>
        ''' Const accessRights As ProcessAccessRights = ProcessAccessRights.All
        ''' </code>
        ''' While using <see cref="ProcessAccessRights.All"/> is not ideal due to the principle of THE least privilege, it ensures that the method has all the necessary permissions to perform the required operations. This approach is acceptable for this GitHub example,
        ''' as it prioritizes functionality and simplicity over strict adherence to security best practices.
        ''' </remarks>
        Friend Shared Function GetNotepadHandleByName() As IntPtr
            Const processName = "notepad"
            Const accessRights = ProcessAccessRights.All

            Try
                Dim notepadProcess = Process.GetProcessesByName(processName).FirstOrDefault()
                If notepadProcess IsNot Nothing Then
                    Dim processHandle As IntPtr = NativeMethods.OpenProcess(accessRights, False, CUInt(notepadProcess.Id))
                    If Equals(processHandle, NativeMethods.NullHandleValue) Then
                        Throw New Win32Exception(Win32Error.GetLastPInvokeErrorCode(), Win32Error.GetLastPInvokeError())
                    End If
                    Return processHandle
                End If
            Catch ex As Exception
                Throw New InvalidOperationException("An error occurred while retrieving the Notepad process handle.", ex)
            End Try
            Return NativeMethods.NullHandleValue
        End Function

        ''' <summary>
        ''' Checks if a process with the specified process ID is running.
        ''' </summary>
        ''' <param name="processId">The ID of the process to check.</param>
        ''' <returns>
        ''' <c>True</c> if the process is running; otherwise, <c>False</c>.
        ''' </returns>
        ''' <exception cref="ArgumentException">Thrown when the process is not found or has exited.</exception>
        Friend Shared Function IsProcessRunning(processId As UInteger) As Boolean
            Try
                Dim process As Process = Process.GetProcessById(CInt(processId))
                Return Not process.HasExited
            Catch ex As ArgumentException
                Throw New ArgumentException($"Process with ID {processId} is not running.", ex)
            End Try
        End Function

        ''' <summary>
        ''' Retrieves the process ID for the specified process handle.
        ''' </summary>
        ''' <param name="processHandle">The handle to the process.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used to display messages to the user.</param>
        ''' <returns>The process ID, or 0 if an error occurred.</returns>
        Friend Shared Function GetProcessId(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As UInteger
            Dim processId As UInteger = NativeMethods.GetProcessId(processHandle)
            If processHandle.IsInvalid Then
                userPrompter.Prompt($"Failed to get process ID. Last error: {Win32Error.GetLastPInvokeError()}")
                Return 0
            End If
            Return processId
        End Function
    End Class
End Namespace
