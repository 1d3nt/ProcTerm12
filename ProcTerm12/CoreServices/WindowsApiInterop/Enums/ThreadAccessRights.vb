Namespace CoreServices.WindowsApiInterop.Enums

    ''' <summary>
    ''' Defines access rights that can be requested for thread operations in a Windows environment.
    ''' </summary>
    ''' <remarks>
    ''' These flags are used in functions such as <c>OpenThread</c> to specify the level of access required for operations.
    ''' 
    ''' <list type="bullet">
    '''     <item><description><see cref="ThreadSynchronize"/> corresponds to <c>THREAD_SYNCHRONIZE</c>.</description></item>
    '''     <item><description><see cref="ThreadAllAccess"/> corresponds to <c>THREAD_ALL_ACCESS</c>.</description></item>
    '''     <item><description><see cref="ThreadDirectImpersonation"/> corresponds to <c>THREAD_DIRECT_IMPERSONATION</c>.</description></item>
    '''     <item><description><see cref="ThreadGetContext"/> corresponds to <c>THREAD_GET_CONTEXT</c>.</description></item>
    '''     <item><description><see cref="ThreadImpersonate"/> corresponds to <c>THREAD_IMPERSONATE</c>.</description></item>
    '''     <item><description><see cref="ThreadQueryInformation"/> corresponds to <c>THREAD_QUERY_INFORMATION</c>.</description></item>
    '''     <item><description><see cref="ThreadQueryLimitedInformation"/> corresponds to <c>THREAD_QUERY_LIMITED_INFORMATION</c>.</description></item>
    '''     <item><description><see cref="ThreadSetContext"/> corresponds to <c>THREAD_SET_CONTEXT</c>.</description></item>
    '''     <item><description><see cref="ThreadSetInformation"/> corresponds to <c>THREAD_SET_INFORMATION</c>.</description></item>
    '''     <item><description><see cref="ThreadSetLimitedInformation"/> corresponds to <c>THREAD_SET_LIMITED_INFORMATION</c>.</description></item>
    '''     <item><description><see cref="ThreadSetThreadToken"/> corresponds to <c>THREAD_SET_THREAD_TOKEN</c>.</description></item>
    '''     <item><description><see cref="ThreadSuspendResume"/> corresponds to <c>THREAD_SUSPEND_RESUME</c>.</description></item>
    '''     <item><description><see cref="ThreadTerminate"/> corresponds to <c>THREAD_TERMINATE</c>.</description></item>
    ''' </list>
    ''' 
    ''' For further details, see:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/procthread/thread-security-and-access-rights">Thread Security and Access Rights</see>.
    ''' <example>
    ''' In C++:
    ''' <code>
    ''' #define THREAD_GET_CONTEXT 0x0008
    ''' #define THREAD_SET_CONTEXT 0x0010
    ''' #define THREAD_SUSPEND_RESUME 0x0002
    ''' 
    ''' HANDLE hThread = OpenThread(THREAD_GET_CONTEXT | THREAD_SET_CONTEXT, FALSE, threadId);
    ''' if (hThread != NULL) {
    '''     // Perform operations on the thread
    '''     CloseHandle(hThread);
    ''' }
    ''' </code>
    ''' </example>
    ''' </remarks>
    Friend Enum ThreadAccessRights As UInteger

        ''' <summary>
        ''' Grants permission to synchronize access to a thread object.
        ''' This is a standard access right for any object, enabling the thread to wait until the object is in the signaled state.
        ''' </summary>
        ThreadSynchronize = &H100000

        ''' <summary>
        ''' Grants all possible access rights for a thread object.
        ''' </summary>
        ThreadAllAccess = &H1F03FF

        ''' <summary>
        ''' Grants permission to directly impersonate a client on the thread.
        ''' This is used for server threads that impersonate a client.
        ''' </summary>
        ThreadDirectImpersonation = &H200

        ''' <summary>
        ''' Grants permission to read the context of a thread.
        ''' </summary>
        ThreadGetContext = &H8

        ''' <summary>
        ''' Grants permission to impersonate a client on the thread.
        ''' </summary>
        ThreadImpersonate = &H100

        ''' <summary>
        ''' Grants permission to query certain information from the thread.
        ''' </summary>
        ThreadQueryInformation = &H40

        ''' <summary>
        ''' Grants permission to query limited information about the thread.
        ''' </summary>
        ThreadQueryLimitedInformation = &H800

        ''' <summary>
        ''' Grants permission to write the context of a thread.
        ''' </summary>
        ThreadSetContext = &H10

        ''' <summary>
        ''' Grants permission to set information on a thread object.
        ''' </summary>
        ThreadSetInformation = &H20

        ''' <summary>
        ''' Grants permission to set limited information on a thread object.
        ''' </summary>
        ThreadSetLimitedInformation = &H400

        ''' <summary>
        ''' Grants permission to set the impersonation token for a thread.
        ''' </summary>
        ThreadSetThreadToken = &H80

        ''' <summary>
        ''' Grants permission to suspend or resume a thread.
        ''' </summary>
        ThreadSuspendResume = &H2

        ''' <summary>
        ''' Grants permission to terminate a thread.
        ''' </summary>
        ThreadTerminate = &H1
    End Enum
End Namespace
