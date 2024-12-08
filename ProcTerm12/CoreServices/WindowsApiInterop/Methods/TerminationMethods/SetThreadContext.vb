Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods


    Friend Class SetThreadContext

        ' Define the RtlAdjustPrivilege function signature for P/Invoke
        <DllImport("ntdll.dll", SetLastError:=True, CharSet:=CharSet.Ansi)>
        Public shared Function RtlAdjustPrivilege(ByVal Privilege As UInteger, ByVal Enable As Boolean, ByVal ClientContext As Boolean, ByRef Adjusted As Boolean) As Integer
        End Function
        ' Declare the necessary P/Invoke functions
        <DllImport("kernel32.dll", CharSet:=CharSet.Auto)>
        Public shared Function LoadLibraryA(ByVal lpFileName As String) As IntPtr
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Auto)>
        Public Shared Function GetProcAddress(ByVal hModule As IntPtr, ByVal lpProcName As String) As IntPtr
        End Function

        ''' <summary>
        ''' Terminates a process using the SetThreadContext method.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process to terminate.</param>
        ''' <param name="userPrompter">The user prompter for interaction.</param>
        ''' <returns>True if the process was terminated successfully; otherwise, false.</returns>
        Friend Shared Function Kill(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean

            Dim hNtdll As IntPtr = LoadLibraryA("ntdll.dll")
        
            ' Check if the library loaded successfully
            If hNtdll.Equals(IntPtr.Zero) Then
                Dim lastError = Win32Error.GetLastPInvokeError()
                Console.WriteLine($"Failed to load ntdll.dll {lastError}")
                Return false
            End If
        
            ' Get the address of RtlAdjustPrivilege function
            Dim RtlAdjustPrivilegePtr As IntPtr = GetProcAddress(hNtdll, "RtlAdjustPrivilege")

            ' Check if the function was retrieved successfully
            If RtlAdjustPrivilegePtr.Equals(IntPtr.Zero) Then
                Console.WriteLine("Failed to get address of RtlAdjustPrivilege")
                Return false
            End If

            ' Declare variables
            Dim boAdjustPrivRet As Boolean = False

            ' Call RtlAdjustPrivilege
            Dim result As Integer = RtlAdjustPrivilege(20, True, False, boAdjustPrivRet)

            ' Check the result
            If result.Equals(0) Then
                Console.WriteLine("Privilege adjusted successfully.")
            Else
                Console.WriteLine("Failed to adjust privilege.")
            End If



            If Not ValidateProcessHandle(processHandle, userPrompter) Then
                Return False
            End If
            Dim processId As UInteger
            If Not TryGetProcessId(processHandle, processId, userPrompter) Then
                Return False
            End If
            Dim handle As IntPtr = CreateSnapshot(processId)
            If Equals(handle, NativeMethods.NullHandleValue) Then
                Return False
            End If
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
        ''' Creates a snapshot of the process.
        ''' </summary>
        ''' <param name="processId">The ID of the process.</param>
        ''' <returns>The handle to the snapshot.</returns>
        Private Shared Function CreateSnapshot(processId As UInteger) As IntPtr
            Dim handle As IntPtr = NativeMethods.CreateToolhelp32Snapshot(NativeMethods.Th32CsSnapAll, processId)
            If Equals(handle, NativeMethods.NullHandleValue) OrElse Equals(handle, NativeMethods.InvalidHandleValue) Then
                Dim lastError = Win32Error.GetLastPInvokeError()
                Throw New NullReferenceException($"The 'handle' object was null or invalid. Error: {lastError}")
            End If
            Return handle
        End Function
    End Class
End Namespace
