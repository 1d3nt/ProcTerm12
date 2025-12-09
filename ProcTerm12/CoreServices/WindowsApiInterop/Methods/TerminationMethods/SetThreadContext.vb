Namespace CoreServices.WindowsApiInterop.Methods.TerminationMethods

    ''' <summary>
    ''' Provides functionality for terminating a process by modifying the context of its threads.
    ''' This class uses Windows API functions to manipulate the context of the threads in the target process, 
    ''' specifically by setting the instruction pointer (RIP) to the address of the ExitProcess function. 
    ''' The threads are suspended, modified, and resumed to force the process to exit.
    ''' 
    ''' <para>API Functions Used:</para>
    ''' <list>
    '''     <item>
    '''         <term>OpenProcess</term>
    '''         <description>
    '''         Retrieves a handle to the specified process with required access rights. This is essential for 
    '''         interacting with the process's threads, suspending them, and modifying their context.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>GetThreadContext</term>
    '''         <description>
    '''         Retrieves the context of a specified thread. This is necessary for modifying the thread's execution 
    '''         state to point to the ExitProcess function, which effectively terminates the process.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>SetThreadContext</term>
    '''         <description>
    '''         Modifies the context of a specified thread. In this case, it sets the instruction pointer to the 
    '''         address of ExitProcess, thus ensuring the thread executes the termination function.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>ResumeThread</term>
    '''         <description>
    '''         Resumes the execution of a suspended thread. After modifying the thread's context, this function 
    '''         is used to allow the thread to continue and execute the ExitProcess function, effectively terminating 
    '''         the target process.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>WaitForSingleObject</term>
    '''         <description>
    '''         Waits for the termination of a thread or process. This is used to ensure that the thread has completed 
    '''         its execution before continuing with the termination process.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <term>CloseHandle</term>
    '''         <description>
    '''         Closes an open object handle. This is used to release handles to processes and threads after the 
    '''         termination process is complete to avoid resource leaks.
    '''         </description>
    '''     </item>
    ''' </list>
    ''' </summary>
    ''' <remarks>
    ''' The process termination technique using <c>SetThreadContext</c> involves interacting with the threads of 
    ''' a specified process, suspending their execution, modifying their context to point to ExitProcess, and then 
    ''' resuming them. This method allows the process to terminate gracefully, assuming all threads are modified 
    ''' correctly. Special care must be taken to ensure that the process is not in a state where its threads are 
    ''' actively performing critical operations, as this could lead to inconsistent results or improper termination.
    ''' 
    ''' <para><b>Challenges with Context Modification:</b></para>
    ''' Modifying thread context is a delicate operation due to the complexity of the thread's state. When changing 
    ''' the instruction pointer (RIP) of a thread to point to the ExitProcess function, several difficulties arise:
    ''' 
    ''' <list type="bullet">
    '''     <item>
    '''         <description>
    '''         <b>Thread Synchronization:</b> Threads are often executing critical operations, and blindly modifying 
    '''         their context may lead to inconsistent or unexpected behavior. Special attention is needed to ensure 
    '''         that the threads are suspended correctly, and their state is not disrupted in a way that could cause 
    '''         the process to behave unpredictably before termination.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <description>
    '''         <b>Context Retrieval:</b> Retrieving the correct context of a thread is essential to modifying it correctly. 
    '''         If the context is not properly retrieved (for example, due to missing access rights or system limitations), 
    '''         the termination process may fail.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <description>
    '''         <b>Context Modification Accuracy:</b> Modifying the context of a thread requires precise handling of the 
    '''         thread's execution state. A misstep in modifying even one register (such as the RIP) can cause the 
    '''         thread to fail to execute the termination function, potentially leaving the process in an inconsistent 
    '''         or unstable state.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <description>
    '''         <b>Error Handling:</b> Given the complexity of working with system-level thread management, error handling 
    '''         must be robust. Errors encountered during context retrieval, modification, or thread suspension/resumption 
    '''         must be gracefully handled to avoid leaving resources in a locked or inconsistent state.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <description>
    '''         <b>Risk of Corruption:</b> The thread's registers (such as RIP, RSP, etc.) are integral to its execution. 
    '''         Modifying them without careful consideration can lead to memory corruption, undefined behavior, or even 
    '''         system crashes. This risk is particularly high when dealing with system processes or processes with complex 
    '''         execution states.
    '''         </description>
    '''     </item>
    ''' </list>
    ''' 
    ''' These challenges make it critical to perform thorough checks at every stage of the context manipulation process 
    ''' to ensure that the thread’s state is properly handled, and no corruption or unstable behavior is introduced.
    ''' 
    ''' <para><b>Working with a Simplified Context64 Class:</b></para>
    ''' Unlike the full <c>CONTEXT64</c> structure, which includes components like <c>XMM_SAVE_AREA32</c> and <c>DUMMYUNIONNAME</c>, 
    ''' this implementation works with a simplified version of the context structure. This reduced context:
    ''' 
    ''' <list type="bullet">
    '''     <item>
    '''         <description>
    '''         <b>Omissions:</b> The absence of the <c>XMM_SAVE_AREA32</c> and <c>DUMMYUNIONNAME</c> members means 
    '''         that this implementation does not handle the extended SIMD registers or other complex components, 
    '''         which simplifies the context and reduces the amount of data that needs to be modified.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <description>
    '''         <b>Targeted Use:</b> This approach is sufficient for modifying basic thread states, such as the instruction 
    '''         pointer (RIP), and does not require the full context associated with floating-point or SIMD state manipulation. 
    '''         It is specifically designed for terminating processes where only the execution flow needs to be altered, 
    '''         not the entire context of the thread.
    '''         </description>
    '''     </item>
    '''     <item>
    '''         <description>
    '''         <b>Reduced Complexity:</b> The simplified context avoids the need to handle complex structures that would 
    '''         normally store additional thread state data (like XMM registers). This reduces the complexity of the context 
    '''         manipulation process but requires careful attention to the minimal components used (such as the instruction pointer).
    '''         </description>
    '''     </item>
    ''' </list>
    '''
    ''' The process termination technique using <c>SetThreadContext</c> involves interacting with the threads of 
    ''' a specified process, suspending their execution, modifying their context to point to ExitProcess, and then 
    ''' resuming them. This method allows the process to terminate gracefully, assuming all threads are modified 
    ''' correctly. Special care must be taken to ensure that the process is not in a state where its threads are 
    ''' actively performing critical operations, as this could lead to inconsistent results or improper termination.
    ''' 
    ''' After struggling for several weeks with getting the correct thread context, I learned a great deal from 
    ''' this project. The challenges in correctly modifying and working with thread contexts, especially in a 
    ''' way that ensures smooth termination, were significant but rewarding. A special thanks to the insights I 
    ''' gained from the Shellcode-Injection-Techniques project on GitHub, which helped clarify some of the intricacies 
    ''' of working with thread contexts.
    ''' <seealso href="https://github.com/plackyhacker/Shellcode-Injection-Techniques">Shellcode Injection Techniques by plackyhacker</seealso>
    ''' </remarks>
    Friend MustInherit Class SetThreadContext

        ''' <summary>
        ''' Terminates the specified process by modifying the context of its threads.
        ''' </summary>
        ''' <param name="processHandle">The handle of the process whose threads are to be terminated.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if the process was terminated successfully; otherwise, <c>False</c>.</returns>
        Friend Shared Function Kill(processHandle As SafeProcessHandle, userPrompter As IUserPrompter) As Boolean
            If Not ValidateProcessHandle(processHandle, userPrompter) Then
                Return False
            End If
            Dim processId As UInteger
            If Not TryGetProcessId(processHandle, processId, userPrompter) Then
                Return False
            End If
            If Not ProcessAllThreads(processId, userPrompter) Then
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
            If processId = NativeMethods.InvalidProcessId Then
                userPrompter.Prompt("Failed to retrieve process ID.")
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Iterates through all threads of the specified process and modifies their context.
        ''' </summary>
        ''' <param name="processId">The ID of the process whose threads are to be processed.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        ''' <returns><c>True</c> if all threads were successfully processed; otherwise, <c>False</c>.</returns>
        Private Shared Function ProcessAllThreads(processId As UInteger, userPrompter As IUserPrompter) As Boolean
            Using hSnapshot As SafeProcessHandle = CreateThreadSnapshot(processId)
                If hSnapshot Is Nothing Then
                    Return False
                End If
                Dim te As New ThreadEntry32()
                te.dwSize = CUInt(Marshal.SizeOf(te))
                If NativeMethods.Thread32First(hSnapshot, te) Then
                    Do
                        If te.th32OwnerProcessID = processId Then
                            ProcessThread(te.th32ThreadID, userPrompter)
                        End If
                    Loop While NativeMethods.Thread32Next(hSnapshot, te)
                End If
            End Using
            Return True
        End Function

        ''' <summary>
        ''' Creates a snapshot of all threads for the specified process.
        ''' </summary>
        ''' <param name="processId">The ID of the process.</param>
        ''' <returns>A <see cref="SafeProcessHandle"/> representing the snapshot, or <c>Nothing</c> if the snapshot could not be created.</returns>
        Private Shared Function CreateThreadSnapshot(processId As UInteger) As SafeProcessHandle
            Dim hSnapshot As New SafeProcessHandle(NativeMethods.CreateToolhelp32Snapshot(NativeMethods.Th32CsSnapThread, processId), True)
            If hSnapshot.IsInvalid Then
                Return Nothing
            End If
            Return hSnapshot
        End Function

        ''' <summary>
        ''' Processes a single thread by modifying its context.
        ''' </summary>
        ''' <param name="threadId">The ID of the thread to process.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        Private Shared Sub ProcessThread(threadId As UInteger, userPrompter As IUserPrompter)
            Using threadHandle As New SafeProcessHandle(OpenThreadHandle(threadId), True)
                If Not threadHandle.IsInvalid Then
                    GetThreadContextExample(threadHandle.DangerousGetHandle(), userPrompter)
                End If
            End Using
        End Sub

        ''' <summary>
        ''' Opens a handle to the specified thread.
        ''' </summary>
        ''' <param name="threadId">The ID of the thread.</param>
        ''' <returns>An <see cref="IntPtr"/> representing the thread handle.</returns>
        Private Shared Function OpenThreadHandle(threadId As UInteger) As IntPtr
            Return NativeMethods.OpenThread(ThreadAccessRights.ThreadGetContext Or
                                            ThreadAccessRights.ThreadSetContext Or
                                            ThreadAccessRights.ThreadSuspendResume,
                                            False, threadId)
        End Function

        ''' <summary>
        ''' Retrieves and modifies the context of a thread.
        ''' </summary>
        ''' <param name="threadHandle">The handle of the thread to modify.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        Private Shared Sub GetThreadContextExample(threadHandle As IntPtr, userPrompter As IUserPrompter)
            Dim kernel32Handle = GetKernel32Handle()
            Dim exitProcessAddress = GetExitProcessAddress(kernel32Handle)
            SuspendThread(threadHandle)
            Dim context = RetrieveThreadContext(threadHandle)
            DisplayThreadContext(context, userPrompter)
            ModifyThreadContext(threadHandle, context, exitProcessAddress)
            ResumeThread(threadHandle)
        End Sub

        ''' <summary>
        ''' Retrieves the handle for kernel32.dll.
        ''' </summary>
        ''' <returns>The handle for kernel32.dll.</returns>
        Private Shared Function GetKernel32Handle() As IntPtr
            Dim kernel32Handle = NativeMethods.GetModuleHandle(ExternDll.Kernel32)
            If Equals(kernel32Handle, NativeMethods.NullHandleValue) Then
                Dim lastError = Win32Error.GetLastPInvokeError()
                Throw New Exception($"Failed to get handle for kernel32.dll. Error: {lastError}")
            End If
            Return kernel32Handle
        End Function

        ''' <summary>
        ''' Retrieves the address of the ExitProcess function.
        ''' </summary>
        ''' <param name="kernel32Handle">The handle for kernel32.dll.</param>
        ''' <returns>The address of the ExitProcess function.</returns>
        Private Shared Function GetExitProcessAddress(kernel32Handle As IntPtr) As IntPtr
            Dim exitProcessAddress = NativeMethods.GetProcAddress(kernel32Handle, "ExitProcess")
            If Equals(exitProcessAddress, NativeMethods.NullHandleValue) Then
                Dim lastError = Win32Error.GetLastPInvokeError()
                Throw New Exception($"Failed to get address of ExitProcess. Error: {lastError}")
            End If
            Return exitProcessAddress
        End Function

        ''' <summary>
        ''' Suspends the specified thread.
        ''' </summary>
        ''' <param name="threadHandle">The handle of the thread to suspend.</param>
        Private Shared Sub SuspendThread(threadHandle As IntPtr)
            If NativeMethods.SuspendThread(threadHandle) = UInteger.MaxValue Then
                Dim lastError = Win32Error.GetLastPInvokeError()
                Throw New Exception($"Failed to suspend thread. Error: {lastError}")
            End If
        End Sub

        ''' <summary>
        ''' Retrieves the context of the specified thread.
        ''' </summary>
        ''' <param name="threadHandle">The handle of the thread.</param>
        ''' <returns>The context of the thread.</returns>
        Private Shared Function RetrieveThreadContext(threadHandle As IntPtr) As Context64
            Dim context As New Context64 With {
                        .ContextFlags = ContextFlags.ContextFull
                    }

            context.ContextFlags = ContextFlags.ContextFull
            If Not NativeMethods.GetThreadContext(threadHandle, context) Then
                Dim lastError = Win32Error.GetLastPInvokeError()
                Throw New Exception($"GetThreadContext failed. Error: {lastError}")
            End If
            Return context
        End Function

        ''' <summary>
        ''' Displays the context of the thread using the user prompter.
        ''' </summary>
        ''' <param name="context">The context of the thread.</param>
        ''' <param name="userPrompter">An instance of <see cref="IUserPrompter"/> used for prompting user interactions during the operation.</param>
        Private Shared Sub DisplayThreadContext(context As Context64, userPrompter As IUserPrompter)
            userPrompter.Prompt("Thread Context Retrieved Successfully!")
            userPrompter.Prompt("--------------------------------")
            userPrompter.Prompt($"RIP (Instruction Pointer): 0x{context.Rip:X16}")
            userPrompter.Prompt($"RSP (Stack Pointer): 0x{context.Rsp:X16}")
            userPrompter.Prompt($"RAX: 0x{context.Rax:X16}")
            userPrompter.Prompt($"RBX: 0x{context.Rbx:X16}")
            userPrompter.Prompt($"RCX: 0x{context.Rcx:X16}")
            userPrompter.Prompt($"RDX: 0x{context.Rdx:X16}")
            userPrompter.Prompt($"R8:  0x{context.R8:X16}")
            userPrompter.Prompt($"R9:  0x{context.R9:X16}")
            userPrompter.Prompt($"R10: 0x{context.R10:X16}")
            userPrompter.Prompt($"R11: 0x{context.R11:X16}")
            userPrompter.Prompt($"Flags: 0x{context.EFlags:X8}")
            userPrompter.Prompt($"Original RIP: 0x{context.Rip:X16}")
        End Sub

        ''' <summary>
        ''' Modifies the context of the thread to set the RIP to the ExitProcess address.
        ''' </summary>
        ''' <param name="threadHandle">The handle of the thread.</param>
        ''' <param name="context">The context of the thread.</param>
        ''' <param name="exitProcessAddress">The address of the ExitProcess function.</param>
        Private Shared Sub ModifyThreadContext(threadHandle As IntPtr, context As Context64, exitProcessAddress As IntPtr)
            context.Rip = CType(exitProcessAddress.ToInt64(), ULong)
            If Not NativeMethods.SetThreadContext(threadHandle, context) Then
                Dim lastError = Win32Error.GetLastPInvokeError()
                Throw New Exception($"SetThreadContext failed. Error: {lastError}")
            End If
        End Sub

        ''' <summary>
        ''' Resumes the specified thread.
        ''' </summary>
        ''' <param name="threadHandle">The handle of the thread to resume.</param>
        Private Shared Sub ResumeThread(threadHandle As IntPtr)
            If NativeMethods.ResumeThread(threadHandle) = UInteger.MaxValue Then
                Dim lastError = Win32Error.GetLastPInvokeError()
                Throw New Exception($"Failed to resume thread. Error: {lastError}")
            End If
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
