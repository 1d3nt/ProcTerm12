Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' The <c>DebugObjectMethods</c> class provides methods for terminating a process using a debug object. This technique involves attaching a debug object to the process and waiting for it to exit. It is primarily used when traditional termination methods do not work, requiring low-level manipulation of the target process.
    ''' </summary>
    ''' <remarks>
    ''' This technique involves the following steps:
    ''' 1. **Validate Process Handle**: The method first validates the provided process handle using <see cref="ProcessHandleValidatorUtility.ValidateProcessHandle"/>.
    ''' 
    ''' 2. **Initialize Object Attributes**: The method initializes the attributes for the debug object by calling <see cref="ObjectAttributesHelper.InitializeObjectAttributes"/>.
    ''' 
    ''' 3. **Create Debug Object**: A debug object is created using <see cref="UnsafeNativeMethods.NtCreateDebugObject"/>, which will be used to manage the debugging session and control the process.
    ''' 
    ''' 4. **Attach Debug Object to Process**: The method attaches the created debug object to the target process using <see cref="UnsafeNativeMethods.NtDebugActiveProcess"/>.
    ''' 
    ''' 5. **Wait for Process Exit**: The method waits for the process to exit using <see cref="ProcessWaitHandler.WaitForProcessExit"/>, which blocks until the process terminates or exits unexpectedly.
    ''' 
    ''' 6. **Handle Memory Cleanup**: After the process has exited, any resources or memory associated with the debug object are cleaned up as necessary.
    ''' </remarks>
    ''' <para>API Functions Used:</para>
    ''' <list>
    '''     <item>
    '''         <term>ProcessHandleValidatorUtility.ValidateProcessHandle</term>
    '''         <description>
    '''         Validates the process handle to ensure it is valid and has appropriate access rights for termination.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>ObjectAttributesHelper.InitializeObjectAttributes</term>
    '''         <description>
    '''         Initializes the object attributes for the debug object, which is necessary for attaching to the target process.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>UnsafeNativeMethods.NtCreateDebugObject</term>
    '''         <description>
    '''         Creates a debug object in the system that can be used to interact with and control the target process for termination.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>UnsafeNativeMethods.NtDebugActiveProcess</term>
    '''         <description>
    '''         Attaches the debug object to the target process, enabling interaction and monitoring of its state.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>ProcessWaitHandler.WaitForProcessExit</term>
    '''         <description>
    '''         Waits for the target process to exit, checking periodically and handling any necessary actions when the process terminates.
    '''         </description>
    '''     </item>
    ''' </list>
    ''' <para>Possible Error Codes:</para>
    ''' <list>
    '''     <item>
    '''         <term>NtStatus.StatusSuccess</term>
    '''         <description>
    '''         Indicates that the operation completed successfully without issues.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>NtStatus.StatusAccessDenied</term>
    '''         <description>
    '''         Occurs if the process cannot be accessed due to insufficient permissions or invalid access rights.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>NtStatus.StatusObjectNameNotFound</term>
    '''         <description>
    '''         Indicates that the debug object could not be created or located, typically due to improper configuration or initialization failure.
    '''         </description>
    '''     </item>
    ''' </list>
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
            If Not CreateAndAttachDebugObject(processHandle, objectAttributes, userPrompter) Then
                Return False
            End If
            Return WaitForProcessToExit(processHandle, userPrompter)
        End Function

        ''' <summary>
        ''' Validates the provided process handle.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to validate.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the process handle is valid; otherwise, <c>False</c>.</returns>
        Private Shared Function ValidateProcessHandle(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Return ProcessHandleValidatorUtility.ValidateProcessHandle(processHandle, userPrompter)
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
        ''' Creates and manages a debug object, attaching it to the specified process for debugging.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to attach to.</param>
        ''' <param name="objectAttributes">The object attributes for the debug object.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the debug object was successfully created and attached; otherwise, false.</returns>
        Private Shared Function CreateAndAttachDebugObject(processHandle As SafeProcessHandle, objectAttributes As ObjectAttributes, userPrompter As IUserPrompter) As Boolean
            Using debugObjectHandle As SafeProcessHandle = CreateDebugObject(objectAttributes, userPrompter)
                If Not ValidateProcessHandle(debugObjectHandle, userPrompter) Then
                    Return False
                End If
                If Not AttachToProcessForDebugging(processHandle, debugObjectHandle, userPrompter) Then
                    Return False
                End If
            End Using
            Return True
        End Function

        ''' <summary>
        ''' Waits for the process to exit and handles the result.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to wait for.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the process exited successfully; otherwise, false.</returns>
        Private Shared Function WaitForProcessToExit(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Dim rawProcessHandle As IntPtr = processHandle.DangerousGetHandle()
            Dim processExited As Boolean = ProcessWaitHandler.WaitForProcessExit(rawProcessHandle, userPrompter)
            Return processExited
        End Function
    End Class
End Namespace
