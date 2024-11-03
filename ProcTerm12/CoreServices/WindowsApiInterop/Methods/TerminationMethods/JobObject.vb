Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' Provides methods to terminate processes by creating and managing job objects.
    ''' </summary>
    Friend Class JobObject

        ''' <summary>
        ''' Terminates a process by creating a job object and assigning the process to it.
        ''' </summary>
        ''' <param name="processHandle">A <see cref="SafeProcessHandle"/> representing the handle of the process to be terminated.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the process was successfully terminated; otherwise, <c>False</c>.</returns>
        Friend Shared Function Kill(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            If Not ValidateProcessHandle(processHandle, userPrompter) Then
                Return False
            End If
            Dim processId As UInteger = ProcessUtility.GetProcessId(processHandle, userPrompter)
            If processId = 0 Then
                userPrompter.Prompt("Failed to get process ID from handle.")
                Return False
            End If
            Using jobHandle As SafeProcessHandle = CreateJobObject(processId, userPrompter)
                If Not ValidateJobHandle(jobHandle, userPrompter) Then
                    Return False
                End If
                Dim jobInfo = AllocateJobInfo(userPrompter)
                If Not SetJobObjectInformation(jobHandle, jobInfo, userPrompter) Then
                    MemoryManager.FreeMemoryIfNotNull(jobInfo)
                    Return False
                End If
                If Not AssignProcessToJobObject(jobHandle, processId, userPrompter) Then
                    Return False
                End If
                Dim isSuccessful As Boolean = TerminateJobObject(jobHandle, userPrompter)
                MemoryManager.FreeMemoryIfNotNull(jobInfo)
                Return isSuccessful
            End Using
        End Function

        ''' <summary>
        ''' Validates the provided process handle.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to validate.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the process handle is valid; otherwise, <c>False</c>.</returns>
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
        ''' Validates the provided job handle.
        ''' </summary>
        ''' <param name="jobHandle">The handle of the job to validate.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the job handle is valid; otherwise, <c>False</c>.</returns>
        Private Shared Function ValidateJobHandle(jobHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            Try
                ProcessHandleValidator.ValidateProcessHandle(jobHandle)
                Return True
            Catch ex As ArgumentException
                userPrompter.Prompt("Invalid job handle.")
                Return False
            End Try
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
    End Class
End Namespace
