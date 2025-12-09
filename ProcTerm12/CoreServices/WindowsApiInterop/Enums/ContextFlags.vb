Namespace CoreServices.WindowsApiInterop.Enums

    ''' <summary>
    ''' Defines flags used to specify which parts of a thread's context are retrieved or set.
    ''' </summary>
    ''' <remarks>
    ''' These flags are used with the <see cref="CONTEXT64"/> structure to indicate which portions of the processor context 
    ''' should be accessed or modified during debugging or thread manipulation.
    ''' 
    ''' The context flags determine whether control registers, integer registers, floating-point registers, or all registers 
    ''' are included. For more details, refer to the Windows API documentation:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-context">CONTEXT Structure (64-bit)</see>.
    ''' </remarks>
    ''' <example>
    ''' In C++:
    ''' <code>
    ''' #define CONTEXT_FULL 0x00010007
    ''' 
    ''' // Usage in a debugging scenario:
    ''' CONTEXT64 context;
    ''' context.ContextFlags = CONTEXT_FULL;
    ''' GetThreadContext(hThread, &context);
    ''' </code>
    ''' </example>
    Friend Enum ContextFlags As UInteger

        ''' <summary>
        ''' Specifies that all parts of the thread context should be retrieved, including control, integer, and floating-point registers.
        ''' </summary>
        ''' <remarks>
        ''' The value <c>0x00010007</c> combines several individual flags to include all processor state components.
        ''' This is commonly used in debugging scenarios to retrieve the complete state of a thread.
        ''' </remarks>
        ''' <example>
        ''' In C++:
        ''' <code>
        ''' #define CONTEXT_FULL 0x00010007
        ''' 
        ''' CONTEXT64 context;
        ''' context.ContextFlags = CONTEXT_FULL;
        ''' if (GetThreadContext(hThread, &context)) {
        '''     // Inspect the thread's full context
        ''' }
        ''' </code>
        ''' </example>
        ContextFull = &H10007
    End Enum
End Namespace
