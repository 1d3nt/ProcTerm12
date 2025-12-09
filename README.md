# **ProcTerm12**

## **Overview**
**ProcTerm12** is a fun, experimental project that demonstrates 12 unconventional ways to terminate a process using P/Invoke. This project is not a full-fledged application and is intentionally designed without strict adherence to usual development principles such as single responsibility or dependency injection. Instead, it serves as an educational and entertaining exploration of advanced process termination techniques.

### **Key Points to Note**
- **ProcTerm12** is **not** a production-ready tool. 
- The project’s classes and methods are intentionally focused on demonstrating individual techniques and do **not** follow typical software engineering standards like single responsibility or dependency injection.
- The examples provided are crafted for learning and exploration rather than robustness or scalability.

### **Purpose**
The primary goal of **ProcTerm12** is to explore creative and technical process control using P/Invoke, delving into both standard and non-standard approaches. This project showcases the power and flexibility of Windows API interactions, providing developers with deeper insights into low-level process management through a series of entertaining examples.

### **Techniques Explored**

Below are the 12 methods implemented in **ProcTerm12** to terminate processes, excluding the two PS driver-based techniques due to their kernel-mode dependency.

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
**Note:** This technique is included for historical purposes and to mirror the original article, but it will fail on modern versions of Windows past Vista/7 due to stricter memory protection and process isolation.

#### **10. VirtualAllocEx**
This method involves repeatedly calling `VirtualAllocEx` in the target process to reserve all available memory. Once the process exhausts its memory, it crashes when it cannot allocate more.  
**Note:** This method is also included for historical completeness and demonstration, but it will fail on Windows versions beyond XP/Vista because modern memory management and address space randomization prevent this from working reliably.

---

### **Omitted Techniques**
#### **PsTerminateProcess**
This is a kernel-mode function not exported by `ntoskrnl`. It requires scanning kernel memory to locate its signature and is incompatible with user-mode applications.

#### **PspTerminateThreadByPointer**
Another kernel-mode function that varies between Windows XP and Vista. It also cannot be used in user-mode applications and requires driver-level access.

---

### **A Note on Handles**
Obtaining a handle to the target process can sometimes be challenging due to restrictions or hooks placed by security software. If `OpenProcess` or `NtOpenProcess` fails, you can try using `DuplicateHandle` to elevate access rights. Alternatively, on Windows Vista and later, the Native API functions `NtGetNextProcess` and `NtGetNextThread` can help enumerate processes and threads with minimal interference.

## Project Structure

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

### **Inspiration**
**ProcTerm12** draws inspiration from the article written by the author of Process Hacker, which details various methods to terminate a process. This article is an excellent resource for those interested in low-level Windows process management. You can access the original article [here](https://web.archive.org/web/20130109025650/http://wj32.org/wp/2009/05/10/12-ways-to-terminate-a-process/).

---

### **Disclaimer**
This project is meant for **educational purposes only**. Some of the methods demonstrated may involve behaviors that could disrupt system stability or interact with processes in unconventional ways. Developers are encouraged to use this knowledge responsibly and solely for legitimate and ethical purposes.