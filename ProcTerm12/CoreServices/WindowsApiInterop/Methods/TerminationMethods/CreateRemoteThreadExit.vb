Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' Provides methods to terminate a specified process using the CreateRemoteThread technique.
    ''' </summary>
    Friend Class CreateRemoteThreadExit

        ''' <summary>
        ''' Terminates the specified process by creating a remote thread that executes ExitProcess.
        ''' </summary>
        ''' <param name="processHandle">
        ''' A handle to the process to be terminated. The handle must have the 
        ''' <see cref="ProcessAccessRights.Terminate"/> access right.
        ''' </param>
        ''' <returns>
        ''' Returns <c>True</c> if the process was successfully terminated; 
        ''' otherwise, returns <c>False</c>.
        ''' </returns>
        ''' <remarks>
        ''' This method creates a thread within the target process that calls ExitProcess, which effectively terminates the process.
        ''' For more information on how to implement this, refer to the Windows API documentation.
        ''' </remarks>
        Friend Shared Function Kill(processHandle As SafeProcessHandle) As Boolean
            Try
                ' Validate the process handle.
                ProcessHandleValidator.ValidateProcessHandle(processHandle)

                ' Get the process ID from the handle.
                Dim processId As UInteger = NativeMethods.GetProcessId(processHandle.DangerousGetHandle())
                If processId = 0 Then
                    Console.WriteLine("Failed to get process ID. Last error: " & Marshal.GetLastWin32Error())
                    Return False
                End If

                ' Obtain a handle to the kernel32 module.
                Dim kernel32Handle As IntPtr = NativeMethods.GetModuleHandle("kernel32.dll")
                If Equals(kernel32Handle, NativeMethods.NullHandleValue) Then
                    Console.WriteLine("Failed to get handle to kernel32.dll. Last error: " & Marshal.GetLastWin32Error())
                    Return False
                End If

                ' Get the address of ExitProcess.
                Dim exitProcessAddress As IntPtr = NativeMethods.GetProcAddress(kernel32Handle, "ExitProcess")
                If  Equals(exitProcessAddress, NativeMethods.NullHandleValue) Then
                    Console.WriteLine("Failed to get address of ExitProcess. Last error: " & Marshal.GetLastWin32Error())
                    Return False
                End If

  
                Dim allocatedMemory As IntPtr = NativeMethods.VirtualAllocEx(processHandle.DangerousGetHandle(), IntPtr.Zero,
                                                                             New UIntPtr(CUInt(Marshal.SizeOf(exitProcessAddress))),
                                                                             NativeMethods.MemCommit Or NativeMethods.MemReserve,
                                                                             NativeMethods.PageExecuteReadWrite)


                If Equals(allocatedMemory, NativeMethods.NullHandleValue) Then
                    Console.WriteLine("Failed to allocate memory in target process. Last error: " & Marshal.GetLastWin32Error())
                    Return False
                End If

                Dim bytesWritten As UInteger
                Dim addressBytes As Byte() = BitConverter.GetBytes(exitProcessAddress.ToInt64())
                Dim addressIntPtr As IntPtr = New IntPtr(BitConverter.ToInt64(addressBytes, 0))
                If Not NativeMethods.WriteProcessMemory(processHandle.DangerousGetHandle(), allocatedMemory,
                                                        addressIntPtr,
                                                        CType(addressBytes.Length, UInteger), bytesWritten) Then
                    Console.WriteLine("Failed to write memory in target process. Last error: " & Marshal.GetLastWin32Error())
                    Return False
                End If


                Dim remoteThreadHandle As IntPtr = NativeMethods.CreateRemoteThread(processHandle.DangerousGetHandle(),
                                                                                    IntPtr.Zero,
                                                                                    0, ' dwStackSize
                                                                                    allocatedMemory,
                                                                                    IntPtr.Zero,
                                                                                    0, ' dwCreationFlags
                                                                                    0) ' lpThreadId

                If Equals(remoteThreadHandle, NativeMethods.NullHandleValue) Then
                    Console.WriteLine("Failed to create remote thread. Last error: " & Marshal.GetLastWin32Error())
                    Return False
                End If

                ' Optionally, wait for the remote thread to finish (this is optional).
                NativeMethods.WaitForSingleObject(remoteThreadHandle, NativeMethods.MemInfinite)

                ' Clean up: free the allocated memory in the target process.
                NativeMethods.VirtualFreeEx(processHandle.DangerousGetHandle(), allocatedMemory,  UIntPtr.Zero, NativeMethods.MemRelease)

                Return True
            Catch ex As Exception
                Console.WriteLine("An error occurred: " & ex.Message)
                Return False
            Finally
                processHandle.Dispose()
            End Try
        End Function
    End Class
End Namespace
