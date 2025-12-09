Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' The <c>JobObject</c> class provides methods to terminate processes by creating and managing job objects. This technique involves creating a job object, assigning the process to the job object, setting job object
    ''' information to ensure termination when the job is closed, and finally terminating the process by closing the job object.
    ''' </summary>
    ''' <remarks>
    ''' This technique involves the following steps:
    ''' 1. **Validate Process Handle**: The method first validates the provided process handle using <see cref="ProcessHandleValidatorUtility.ValidateProcessHandle"/>.
    ''' 
    ''' 2. **Retrieve Process ID**: The process ID is retrieved from the process handle using <see cref="ProcessUtility.GetProcessId"/>.
    ''' 
    ''' 3. **Create and Configure Job Object**: A job object is created using <see cref="NativeMethods.CreateJobObjectA"/>, and process assignment is handled using <see cref="NativeMethods.AssignProcessToJobObject"/>.
    ''' 
    ''' 4. **Set Job Object Information**: The job object information is set to ensure termination when the job is closed, using <see cref="NativeMethods.SetInformationJobObject"/>.
    ''' 
    ''' 5. **Terminate Job Object**: The job object is terminated using <see cref="NativeMethods.TerminateJobObject"/> to ensure the process is killed when the job object is closed.
    ''' 
    ''' 6. **Wait for Process Exit**: After termination, the method waits for the process to exit using <see cref="ProcessWaitHandler.WaitForProcessExit"/>.
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
    '''         <term>ProcessUtility.GetProcessId</term>
    '''         <description>
    '''         Retrieves the process ID from the provided process handle.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>NativeMethods.CreateJobObjectA</term>
    '''         <description>
    '''         Creates a job object that can be used to manage and terminate processes.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>NativeMethods.AssignProcessToJobObject</term>
    '''         <description>
    '''         Assigns a process to the created job object.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>NativeMethods.SetInformationJobObject</term>
    '''         <description>
    '''         Sets job object information, including termination behavior when the job is closed.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>NativeMethods.TerminateJobObject</term>
    '''         <description>
    '''         Terminates the job object, which also terminates the process assigned to it.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>ProcessWaitHandler.WaitForProcessExit</term>
    '''         <description>
    '''         Waits for the target process to exit, checking periodically and handling necessary actions upon termination.
    '''         </description>
    '''     </item>
    ''' </list>
    ''' <para>Possible Error Codes:</para>
    ''' <list>
    '''     <item>
    '''         <term>NtStatus.StatusSuccess</term>
    '''         <description>
    '''         Indicates that the operation completed successfully.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>NtStatus.StatusAccessDenied</term>
    '''         <description>
    '''         Indicates insufficient permissions to terminate the process or manage the job object.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>NtStatus.StatusObjectNameNotFound</term>
    '''         <description>
    '''         Indicates that the job object could not be created or located.
    '''         </description>
    '''     </item>
    ''' </list>
    Friend Class JobObject

        ''' <summary>
        ''' Terminates a process by creating a job object, assigning the process to it, and managing the termination process.
        ''' </summary>
        ''' <param name="processHandle">
        ''' A <see cref="SafeProcessHandle"/> representing the handle of the process to be terminated. 
        ''' This handle must have the necessary access rights to allow termination operations.
        ''' </param>
        ''' <param name="userPrompter">
        ''' An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation. 
        ''' This allows the method to notify the user of any errors or progress during the termination process.
        ''' </param>
        ''' <returns>
        ''' <c>True</c> if the process was successfully terminated; otherwise, <c>False</c>.
        ''' The method returns <c>False</c> if any step in the termination process fails, such as validating the process handle, 
        ''' retrieving the process ID, creating and configuring the job object, or waiting for the process to exit.
        ''' </returns>
        ''' <remarks>
        ''' This method performs the following steps to terminate the process:
        ''' 1. Validates the provided process handle to ensure it is valid and has the required access rights.
        ''' 2. Retrieves the process ID from the provided process handle.
        ''' 3. Creates a job object, assigns the process to it, sets job object information, and terminates the job object 
        '''    using the <see cref="CreateAndTerminateJobObject"/> method.
        ''' 4. Waits for the process to exit using the <see cref="WaitForProcessToExit"/> method to ensure the termination is complete.
        ''' 
        ''' If any step fails, the method ensures that resources are cleaned up appropriately and returns <c>False</c>.
        ''' </remarks>
        Friend Shared Function Kill(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            If Not ValidateProcessHandle(processHandle, userPrompter) Then
                Return False
            End If
            Dim processId As UInteger
            If Not TryGetProcessId(processHandle, processId, userPrompter) Then
                Return False
            End If
            If Not CreateAndTerminateJobObject(processId, userPrompter) Then
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
        ''' Retrieves the process ID from the process handle.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the process ID was retrieved successfully; otherwise, <c>False</c>.</returns>
        Private Shared Function TryGetProcessId(processHandle As SafeProcessHandle, ByRef processId As UInteger, userPrompter As IUserPrompter) As Boolean
            processId = ProcessUtility.GetProcessId(processHandle, userPrompter)
            If processId = 0 Then
                userPrompter.Prompt("Failed to retrieve process ID.")
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Validates the provided job handle.
        ''' </summary>
        ''' <param name="jobHandle">The handle of the job to validate.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the job handle is valid; otherwise, <c>False</c>.</returns>
        Private Shared Function ValidateJobHandle(jobHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Return ProcessHandleValidatorUtility.ValidateProcessHandle(jobHandle, userPrompter)
        End Function

        ''' <summary>
        ''' Assigns a process to a job object.
        ''' </summary>
        ''' <param name="jobHandle">The handle of the job object.</param>
        ''' <param name="processId">The ID of the process to assign to the job object.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the process was successfully assigned to the job object; otherwise, <c>False</c>.</returns>
        Private Shared Function AssignProcessToJobObject(jobHandle As SafeProcessHandle, processId As UInteger, userPrompter As IUserPrompter) As Boolean
            Using procHandle As SafeProcessHandle = OpenProcess(ProcessAccessRights.All, False, processId)
                If Not procHandle.IsInvalid Then
                    If Not NativeMethods.AssignProcessToJobObject(jobHandle, procHandle) Then
                        userPrompter.Prompt("Failed to assign process to job object.")
                        Return False
                    End If
                End If
            End Using
            Return True
        End Function

        ''' <summary>
        ''' Opens a process with the specified access rights.
        ''' </summary>
        ''' <param name="desiredAccess">The access rights for the process.</param>
        ''' <param name="inheritHandle">Indicates whether the handle is inheritable.</param>
        ''' <param name="processId">The ID of the process to open.</param>
        ''' <returns>A <see cref="SafeProcessHandle"/> representing the handle of the opened process.</returns>
        Private Shared Function OpenProcess(desiredAccess As ProcessAccessRights, inheritHandle As Boolean, processId As UInteger) As SafeProcessHandle
            Dim handle = NativeMethods.OpenProcess(desiredAccess, inheritHandle, processId)
            Return New SafeProcessHandle(handle, True)
        End Function

        ''' <summary>
        ''' Creates a job object for the specified process ID.
        ''' </summary>
        ''' <param name="processId">The ID of the process.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>The handle of the created job object.</returns>
        Private Shared Function CreateJobObject(processId As UInteger, userPrompter As IUserPrompter) As SafeProcessHandle
            Dim jobHandle = NativeMethods.CreateJobObjectA(NativeMethods.NullHandleValue, $"ProcessJob{processId}")
            If Equals(jobHandle, NativeMethods.NullHandleValue) Then
                userPrompter.Prompt("Failed to create job object.")
                Return New SafeProcessHandle(NativeMethods.NullHandleValue, True)
            Else
                userPrompter.Prompt("Job object created successfully.")
                Return New SafeProcessHandle(jobHandle, True)
            End If
        End Function

        ''' <summary>
        ''' Allocates memory for job object information.
        ''' </summary>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>The pointer to the allocated memory.</returns>
        Private Shared Function AllocateJobInfo(userPrompter As IUserPrompter) As IntPtr
            Dim jobBasicInfo = New JobObjectBasicLimitInformation With {.LimitFlags = JobObjectLimitFlags.JobObjectLimitKillOnJobClose}
            Dim jobExtendedInfo = New JobObjectExtendedLimitInformation With {.BasicLimitInformation = jobBasicInfo}
            Dim jobInfoLength = Marshal.SizeOf(GetType(JobObjectExtendedLimitInformation))
            Dim jobInfo = Marshal.AllocHGlobal(jobInfoLength)
            Marshal.StructureToPtr(jobExtendedInfo, jobInfo, False)
            userPrompter.Prompt("Allocated memory for job object information.")
            Return jobInfo
        End Function

        ''' <summary>
        ''' Sets information for the job object.
        ''' </summary>
        ''' <param name="jobHandle">The handle of the job object.</param>
        ''' <param name="jobInfo">The pointer to the job object information.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns><c>True</c> if the information was set successfully; otherwise, <c>False</c>.</returns>
        Private Shared Function SetJobObjectInformation(jobHandle As SafeProcessHandle, jobInfo As IntPtr, userPrompter As IUserPrompter) As Boolean
            Dim jobInfoLength = Marshal.SizeOf(GetType(JobObjectExtendedLimitInformation))
            If Not NativeMethods.SetInformationJobObject(jobHandle, JobObjectInformationClass.JobObjectExtendedLimitInformation, jobInfo, CUInt(jobInfoLength)) Then
                userPrompter.Prompt("Failed to set job object information.")
                Return False
            End If
            userPrompter.Prompt("Job object information set successfully.")
            Return True
        End Function

        ''' <summary>
        ''' Terminates the job object.
        ''' </summary>
        ''' <param name="jobHandle">The handle of the job object.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns><c>True</c> if the job object was terminated successfully; otherwise, <c>False</c>.</returns>
        Private Shared Function TerminateJobObject(jobHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            If Not NativeMethods.TerminateJobObject(jobHandle, 0) Then
                userPrompter.Prompt("Failed to terminate job object.")
                Return False
            End If
            userPrompter.Prompt("Job object terminated successfully.")
            Return True
        End Function

        ''' <summary>
        ''' Creates a job object, assigns the process to it, sets job object information, and terminates the job object.
        ''' </summary>
        ''' <param name="processId">
        ''' The ID of the process to assign to the job object. This is used to identify the process that will be 
        ''' managed and terminated by the job object.
        ''' </param>
        ''' <param name="userPrompter">
        ''' An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.
        ''' This allows the method to notify the user of any errors or progress during the operation.
        ''' </param>
        ''' <returns>
        ''' <c>True</c> if the job object was successfully created, configured, and terminated; otherwise, <c>False</c>.
        ''' The method returns <c>False</c> if any step in the process fails.
        ''' </returns>
        ''' <remarks>
        ''' This method performs the following steps:
        ''' 1. Creates a job object using the specified process ID.
        ''' 2. Validates the created job object handle to ensure it is valid.
        ''' 3. Allocates memory for job object information, which is used to configure the job object.
        ''' 4. Sets the job object information to configure the job object with specific limits (e.g., terminate the process when the job object is closed).
        ''' 5. Assigns the process to the job object, effectively associating the process with the job object for management.
        ''' 6. Terminates the job object, which also terminates the associated process.
        ''' 7. Frees any allocated memory for job object information after it is no longer needed.
        ''' 
        ''' If any step fails, the method ensures that allocated memory is freed before returning <c>False</c>.
        ''' </remarks>
        Private Shared Function CreateAndTerminateJobObject(processId As UInteger, userPrompter As IUserPrompter) As Boolean
            Using jobHandle As SafeProcessHandle = CreateJobObject(processId, userPrompter)
                If Not ValidateJobHandle(jobHandle, userPrompter) Then
                    Return False
                End If
                Dim jobInfo = AllocateJobInfo(userPrompter)
                If Not SetJobObjectInformation(jobHandle, jobInfo, userPrompter) Then
                    FreeMemoryIfNotNull(jobInfo)
                    Return False
                End If
                If Not AssignProcessToJobObject(jobHandle, processId, userPrompter) Then
                    FreeMemoryIfNotNull(jobInfo)
                    Return False
                End If
                If Not TerminateJobObject(jobHandle, userPrompter) Then
                    FreeMemoryIfNotNull(jobInfo)
                    Return False
                End If
                FreeMemoryIfNotNull(jobInfo)
            End Using
            Return True
        End Function

        ''' <summary>
        ''' Frees allocated memory.
        ''' </summary>
        ''' <param name="memoryPointer">The pointer to the allocated memory to be freed.</param>
        Private Shared Sub FreeMemoryIfNotNull(memoryPointer As IntPtr)
            MemoryManager.FreeMemoryIfNotNull(memoryPointer)
        End Sub

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
