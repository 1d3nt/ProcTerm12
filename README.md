### **Techniques Explored**

Below are the 10 methods implemented in **ProcTerm12** to terminate processes, excluding the two PS driver-based techniques due to their kernel-mode dependency.

#### **1. TerminateProcess or NtTerminateProcess**
The most direct approach to terminating a process. This method involves opening a handle to the target process and calling `TerminateProcess`. To bypass user-mode hooks, the Native API equivalent, `NtTerminateProcess`, can be used for similar functionality with fewer obstructions.

#### **2. CreateRemoteThread with ExitProcess**
This technique injects a thread into the target process that executes `ExitProcess`, effectively terminating the process. The address of `ExitProcess` can usually be found using `GetModuleHandle` and `GetProcAddress`. A remote thread is then created in the target process to execute this function.

#### **3. NtQuerySystemInformation or toolhelp32 with TerminateThread or NtTerminateThread**
By iterating through all threads of the target process, each thread can be terminated individually using `TerminateThread` or its Native API counterpart, `NtTerminateThread`. This method ensures process termination by cutting off all active threads.

#### **4. NtQuerySystemInformation or toolhelp32 with SetThreadContext**
This method modifies the execution context of each thread in the target process using `SetThreadContext`, pointing the instruction pointer (EIP) to `ExitProcess`. This forces the process to exit upon resuming thread execution.

#### **5. DuplicateHandle**
By iterating through possible handle values and using `DuplicateHandle` with specific options, the target process’s handles can be closed. This can disrupt complex applications reliant on active handles, although simpler applications may remain unaffected.

#### **6. CreateJobObject, AssignProcessToJobObject, and TerminateJobObject**
This approach creates a job object using `CreateJobObject`, assigns the target process to it via `AssignProcessToJobObject`, and then calls `TerminateJobObject`. It works only if the process isn’t already assigned to a job object and can bypass hooks in `NtAssignProcessToJobObject` and `NtTerminateJobObject`.

#### **7. NtCreateDebugObject, NtDebugActiveProcess, and CloseHandle**
Using a debug object created with `NtCreateDebugObject`, the target process can be debugged with `NtDebugActiveProcess`. Closing the debug object handle causes the kernel to terminate the debugged process. This method leverages kernel-level termination behavior triggered by handle closure.

#### **8. VirtualQueryEx and VirtualProtectEx**
This technique iterates through memory regions of the target process using `VirtualQueryEx` and modifies their protections to `PAGE_NOACCESS` using `VirtualProtectEx`. The target process crashes when attempting to execute code or access memory regions it no longer has permissions for.

#### **9. VirtualQueryEx and WriteProcessMemory**
By using `VirtualQueryEx` to iterate through memory regions of the target process and writing random or invalid data to these regions with `WriteProcessMemory`, this method causes the process to crash due to corrupted memory.

#### **10. VirtualAllocEx**
This method involves repeatedly calling `VirtualAllocEx` in the target process to reserve all available memory. Once the process exhausts its memory, it crashes when it cannot allocate more.

---

### **Omitted Techniques**
#### **PsTerminateProcess**
This is a kernel-mode function not exported by `ntoskrnl`. It requires scanning kernel memory to locate its signature and is incompatible with user-mode applications.

#### **PspTerminateThreadByPointer**
Another kernel-mode function that varies between Windows XP and Vista. It also cannot be used in user-mode applications and requires driver-level access.

---

### **A Note on Handles**
Obtaining a handle to the target process can sometimes be challenging due to restrictions or hooks placed by security software. If `OpenProcess` or `NtOpenProcess` fails, you can try using `DuplicateHandle` to elevate access rights. Alternatively, on Windows Vista and later, the Native API functions `NtGetNextProcess` and `NtGetNextThread` can help enumerate processes and threads with minimal interference.

## PROJECT STRUCTURE

```bash
|-- Application
|   |-- Interfaces
|   |   |-- IAppRunner.vb
|   |   +-- IServiceConfigurator.vb
|   |-- AppRunner.vb
|   +-- ServiceConfigurator.vb
|
|-- CoreServices
|   |-- ProcessManagement
|   |   |-- Interfaces
|   |   |   +-- IProcessLauncher.vb
|   |   +-- ProcessLauncher.vb
|   +-- WindowsApiInterop
|       |-- Enums
|       |   |-- DuplicateOptions.vb
|       |   |-- JobObjectInformationClass.vb
|       |   |-- MemoryProtection.vb
|       |   |-- NtStatus.vb
|       |   |-- ObjectAttributeFlags.vb
|       |   |-- ProcessAccessRights.vb
|       |   +-- TerminationMethods.vb
|       |-- Methods
|       |   |-- MemoryManagement
|       |   |   |-- HandleMemory.vb
|       |   |   |-- MemoryManager.vb
|       |   |   |-- ProcessHandleValidator.vb
|       |   |   |-- SafeHandleWrapper.vb
|       |   |   +-- SafeTokenHandle.vb
|       |   |-- TerminationMethods
|       |   |   |-- Interfaces
|       |   |   |   +-- IProcessTerminator.vb
|       |   |   |-- CreateRemoteThreadExit.vb
|       |   |   |-- DebugObjectMethods.vb
|       |   |   |-- DuplicateHandle.vb
|       |   |   |-- JobObject.vb
|       |   |   |-- NtTerminateProcess.vb
|       |   |   |-- ProcessTerminator.vb
|       |   |   |-- PspTerminateThreadByPointer.vb
|       |   |   |-- PsTerminateProcess.vb
|       |   |   |-- SetThreadContext.vb
|       |   |   |-- TerminateThread.vb
|       |   |   |-- VirtualAllocEx.vb
|       |   |   |-- VirtualQueryExNoAccess.vb
|       |   |   +-- WriteProcessMemory.vb
|       |   |-- NativeMethods.vb
|       |   +-- UnsafeNativeMethods.vb
|       |-- Structs
|       |   |-- ClientId.vb
|       |   |-- IoCounters.vb
|       |   |-- JobObjectBasicLimitInformation.vb
|       |   |-- JobObjectExtendedLimitInformation.vb
|       |   |-- JobObjectLimitFlags.vb
|       |   |-- MemoryBasicInformation.vb
|       |   |-- ModuleEntry32.vb
|       |   |-- ObjectAttributes.vb
|       |   +-- ThreadEntry32.vb
|       +-- ExternDll.vb
|
|-- Utilities
|   |-- ErrorHandling
|   |   |-- Interfaces
|   |   |   +-- IExitUtility.vb
|   |   |-- ExitUtility.vb
|   |   +-- Win32Error.vb
|   |-- Interfaces
|   |   |-- IConsoleClearer.vb
|   |   |-- ITerminationMethodProvider.vb
|   |   |-- IUserInputReader.vb
|   |   +-- IUserPrompter.vb
|   |-- ProcessUtilities
|   |   |-- ProcessExitCodeRetriever.vb
|   |   |-- ProcessHandleValidatorUtility.vb
|   |   |-- ProcessUtility.vb
|   |   +-- ProcessWaitHandler.vb
|   |-- AsynchronousProcessor.vb
|   |-- ConsoleClearer.vb
|   |-- ObjectAttributesHelper.vb
|   |-- TerminationMethodProvider.vb
|   |-- TerminationStorage.vb
|   |-- UserInputReader.vb
|   |-- UserPrompter.vb
|   +-- UserPrompterSingleton.vb
|
|-- GlobalAttributes.vb
|-- ModuleEnumerator.vb
+-- Program.vb
```
