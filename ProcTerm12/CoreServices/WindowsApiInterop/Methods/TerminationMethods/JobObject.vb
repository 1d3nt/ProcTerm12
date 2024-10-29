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
            ProcessHandleValidator.ValidateProcessHandle(processHandle)
            Dim processId As UInteger = ProcessUtility.GetProcessId(processHandle, userPrompter)
            If processId = 0 Then
                userPrompter.Prompt("Failed to get process ID from handle.")
                Return False
            End If
            Dim jobHandle = CreateJobObject(processId, userPrompter)
            If Equals(jobHandle, NativeMethods.NullHandleValue) Then
                Return False
            End If
            Dim jobInfo = AllocateJobInfo(userPrompter)
            If Not SetJobObjectInformation(jobHandle, jobInfo, userPrompter) Then
                HandleManager.CloseHandleIfNotNull(jobHandle)
                MemoryManager.FreeMemoryIfNotNull(jobInfo)
                Return False
            End If
            Dim procHandle As IntPtr = NativeMethods.OpenProcess(ProcessAccessRights.All, False, processId)
            Try
                If Not Equals(procHandle, NativeMethods.NullHandleValue) Then
                    If Not NativeMethods.AssignProcessToJobObject(jobHandle, procHandle) Then
                        userPrompter.Prompt("Failed to assign process to job object.")
                        Return False
                    End If
                End If
            Finally
                HandleManager.CloseHandleIfNotNull(procHandle)
            End Try
            Dim isSuccessful As Boolean = TerminateJobObject(jobHandle, userPrompter)
            HandleManager.CloseHandleIfNotNull(jobHandle)
            MemoryManager.FreeMemoryIfNotNull(jobInfo)
            Return isSuccessful
        End Function

        ''' <summary>
        ''' Creates a job object for the specified process ID.
        ''' </summary>
        ''' <param name="processId">The ID of the process.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>The handle of the created job object.</returns>
        Private Shared Function CreateJobObject(processId As UInteger, userPrompter As IUserPrompter) As IntPtr
            Dim jobHandle = NativeMethods.CreateJobObjectA(NativeMethods.NullHandleValue, $"ProcessJob{processId}")
            If Equals(jobHandle, NativeMethods.NullHandleValue) Then
                userPrompter.Prompt("Failed to create job object.")
            Else
                userPrompter.Prompt("Job object created successfully.")
            End If
            Return jobHandle
        End Function

        ''' <summary>
        ''' Allocates memory for job object information.
        ''' </summary>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>The pointer to the allocated memory.</returns>
        Private Shared Function AllocateJobInfo(userPrompter As IUserPrompter) As IntPtr
            Dim jobBasicInfo = New JobObjectBasicLimitInformation With {.LimitFlags = &H2000}
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
        Private Shared Function SetJobObjectInformation(jobHandle As IntPtr, jobInfo As IntPtr, userPrompter As IUserPrompter) As Boolean
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
        Private Shared Function TerminateJobObject(jobHandle As IntPtr, userPrompter As IUserPrompter) As Boolean
            If Not NativeMethods.TerminateJobObject(jobHandle, 0) Then
                userPrompter.Prompt("Failed to terminate job object.")
                Return False
            End If
            Return True
        End Function
    End Class
End Namespace
