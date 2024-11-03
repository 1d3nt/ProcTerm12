Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' Provides methods for terminating processes using debug objects.
    ''' </summary>
    ''' <remarks>
    ''' This class contains methods to initialize object attributes, create debug objects,
    ''' attach to processes for debugging, and check process exit codes.
    ''' </remarks>
    Friend Class DebugObjectMethods

        ''' <summary>
        ''' Terminates a process using the DebugObjectMethods method.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to terminate.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the process was terminated successfully; otherwise, false.</returns>
        Friend Shared Function Kill(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean        
            If Not ValidateProcessHandle(processHandle, userPrompter) Then
                Return False
            End If
            Dim objectAttributes As New ObjectAttributes()
            If Not InitializeObjectAttributes(objectAttributes, userPrompter) Then
                Return False
            End If
            Using debugObjectHandle As SafeProcessHandle = CreateDebugObject(objectAttributes, userPrompter)
                If Not ValidateProcessHandle(debugObjectHandle, userPrompter) Then
                    Return False
                End If
                If Not AttachToProcessForDebugging(processHandle, debugObjectHandle, userPrompter) Then
                    Return False
                End If
            End Using
            Return CheckProcessExitCode(processHandle)
        End Function

        ''' <summary>
        ''' Validates the process handle and prompts the user if invalid.
        ''' </summary>
        ''' <param name="processHandle">The handle to validate.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the handle is valid; otherwise, false.</returns>
        Private Shared Function ValidateProcessHandle(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Try
                ProcessHandleValidator.ValidateProcessHandle(processHandle)
                Return True
            Catch ex As ArgumentException
                userPrompter.Prompt("Invalid process handle.")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Initializes the object attributes for the debug object.
        ''' </summary>
        ''' <param name="objectAttributes">The object attributes to initialize.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the object attributes were initialized successfully; otherwise, false.</returns>
        Private Shared Function InitializeObjectAttributes(ByRef objectAttributes As ObjectAttributes, userPrompter As IUserPrompter) As Boolean
            Dim result As UInteger = ObjectAttributesHelper.InitializeObjectAttributes(objectAttributes, NativeMethods.NullHandleValue, 0, NativeMethods.NullHandleValue, NativeMethods.NullHandleValue)
            If result <> NtStatus.StatusSuccess Then
                userPrompter.Prompt("Failed to initialize object.")
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Creates a debug object.
        ''' </summary>
        ''' <param name="objectAttributes">The object attributes for the debug object.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>The handle of the created debug object.</returns>
        Private Shared Function CreateDebugObject(objectAttributes As ObjectAttributes, userPrompter As IUserPrompter) As SafeProcessHandle
            Dim debugObjectHandle As IntPtr
            Dim result As UInteger = UnsafeNativeMethods.NtCreateDebugObject(debugObjectHandle, ProcessAccessRights.All, objectAttributes, True)
            If result < NtStatus.StatusSuccess Then
                userPrompter.Prompt("Failed to create debug object.")
                Return Nothing
            End If
            Return New SafeProcessHandle(debugObjectHandle, True)
        End Function

        ''' <summary>
        ''' Attaches to the process for debugging.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to attach to.</param>
        ''' <param name="debugObjectHandle">The handle of the debug object.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the process was attached for debugging successfully; otherwise, false.</returns>
        Private Shared Function AttachToProcessForDebugging(processHandle As SafeProcessHandle, debugObjectHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Dim result As Integer = UnsafeNativeMethods.NtDebugActiveProcess(processHandle.DangerousGetHandle(), debugObjectHandle.DangerousGetHandle())
            If result < NtStatus.StatusSuccess Then
                userPrompter.Prompt("Failed to attach to the process for debugging.")
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Checks the exit code of the process.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process.</param>
        ''' <returns>True if the process has exited; otherwise, false.</returns>
        Private Shared Function CheckProcessExitCode(processHandle As SafeProcessHandle) As Boolean
            Dim exitCode As UInteger
            If Not NativeMethods.GetExitCodeProcess(processHandle.DangerousGetHandle(), exitCode) OrElse exitCode = NativeMethods.StillActive Then
                Return False
            End If
            Return True
        End Function
    End Class
End Namespace
