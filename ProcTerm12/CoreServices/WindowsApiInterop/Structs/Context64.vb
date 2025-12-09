Namespace CoreServices.WindowsApiInterop.Structs

    ''' <summary>
    ''' Represents the 64-bit context structure for thread execution in the Windows operating system.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="CONTEXT64"/> structure contains processor-specific register data and state information 
    ''' for debugging and exception handling in 64-bit Windows. It corresponds to the <c>CONTEXT</c> structure 
    ''' when targeting 64-bit processors.
    ''' 
    ''' In C++:
    ''' <code>
    ''' typedef struct _CONTEXT {
    '''     DWORD64 P1Home;
    '''     DWORD64 P2Home;
    '''     DWORD64 P3Home;
    '''     DWORD64 P4Home;
    '''     DWORD64 P5Home;
    '''     DWORD64 P6Home;
    '''     DWORD   ContextFlags;
    '''     DWORD   MxCsr;
    '''     WORD    SegCs;
    '''     WORD    SegDs;
    '''     WORD    SegEs;
    '''     WORD    SegFs;
    '''     WORD    SegGs;
    '''     WORD    SegSs;
    '''     DWORD   EFlags;
    '''     DWORD64 Dr0;
    '''     DWORD64 Dr1;
    '''     DWORD64 Dr2;
    '''     DWORD64 Dr3;
    '''     DWORD64 Dr6;
    '''     DWORD64 Dr7;
    '''     DWORD64 Rax;
    '''     DWORD64 Rcx;
    '''     DWORD64 Rdx;
    '''     DWORD64 Rbx;
    '''     DWORD64 Rsp;
    '''     DWORD64 Rbp;
    '''     DWORD64 Rsi;
    '''     DWORD64 Rdi;
    '''     DWORD64 R8;
    '''     DWORD64 R9;
    '''     DWORD64 R10;
    '''     DWORD64 R11;
    '''     DWORD64 R12;
    '''     DWORD64 R13;
    '''     DWORD64 R14;
    '''     DWORD64 R15;
    '''     DWORD64 Rip;
    ''' } CONTEXT, *PCONTEXT;
    ''' </code>
    ''' 
    ''' For more information, see:
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-context">CONTEXT Structure (64-bit)</see>.
    ''' </remarks>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure Context64

        ''' <summary>
        ''' The first home parameter. Used for parameter passing and function calls.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 P1Home;</code>
        ''' </example>
        Friend P1Home As ULong

        ''' <summary>
        ''' The second home parameter. Used for parameter passing and function calls.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 P2Home;</code>
        ''' </example>
        Friend P2Home As ULong

        ''' <summary>
        ''' The third home parameter. Used for parameter passing and function calls.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 P3Home;</code>
        ''' </example>
        Friend P3Home As ULong

        ''' <summary>
        ''' The fourth home parameter. Used for parameter passing and function calls.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 P4Home;</code>
        ''' </example>
        Friend P4Home As ULong

        ''' <summary>
        ''' The fifth home parameter. Used for parameter passing and function calls.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 P5Home;</code>
        ''' </example>
        Friend P5Home As ULong

        ''' <summary>
        ''' The sixth home parameter. Used for parameter passing and function calls.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 P6Home;</code>
        ''' </example>
        Friend P6Home As ULong

        ''' <summary>
        ''' Flags that indicate the contents of the CONTEXT structure.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD ContextFlags;</code>
        ''' </example>
        Friend ContextFlags As UInteger

        ''' <summary>
        ''' The control and status register of the x87 FPU and SIMD.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD MxCsr;</code>
        ''' </example>
        Friend MxCsr As UInteger

        ''' <summary>
        ''' The segment selector for the code segment.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>WORD SegCs;</code>
        ''' </example>
        Friend SegCs As UShort

        ''' <summary>
        ''' The segment selector for the data segment.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>WORD SegDs;</code>
        ''' </example>
        Friend SegDs As UShort

        ''' <summary>
        ''' The segment selector for the extra segment.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>WORD SegEs;</code>
        ''' </example>
        Friend SegEs As UShort

        ''' <summary>
        ''' The segment selector for the FS register.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>WORD SegFs;</code>
        ''' </example>
        Friend SegFs As UShort

        ''' <summary>
        ''' The segment selector for the GS register.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>WORD SegGs;</code>
        ''' </example>
        Friend SegGs As UShort

        ''' <summary>
        ''' The segment selector for the stack segment.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>WORD SegSs;</code>
        ''' </example>
        Friend SegSs As UShort

        ''' <summary>
        ''' The CPU flags register.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD EFlags;</code>
        ''' </example>
        Friend EFlags As UInteger

        ''' <summary>
        ''' The debug register 0.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Dr0;</code>
        ''' </example>
        Friend Dr0 As ULong

        ''' <summary>
        ''' The debug register 1.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Dr1;</code>
        ''' </example>
        Friend Dr1 As ULong

        ''' <summary>
        ''' The debug register 2.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Dr2;</code>
        ''' </example>
        Friend Dr2 As ULong

        ''' <summary>
        ''' The debug register 3.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Dr3;</code>
        ''' </example>
        Friend Dr3 As ULong

        ''' <summary>
        ''' The debug register 6.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Dr6;</code>
        ''' </example>
        Friend Dr6 As ULong

        ''' <summary>
        ''' The debug register 7.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Dr7;</code>
        ''' </example>
        Friend Dr7 As ULong

        ''' <summary>
        ''' The general-purpose register RAX.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Rax;</code>
        ''' </example>
        Friend Rax As ULong

        ''' <summary>
        ''' The general-purpose register RCX.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Rcx;</code>
        ''' </example>
        Friend Rcx As ULong

        ''' <summary>
        ''' The general-purpose register RDX.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Rdx;</code>
        ''' </example>
        Friend Rdx As ULong

        ''' <summary>
        ''' The general-purpose register RBX.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Rbx;</code>
        ''' </example>
        Friend Rbx As ULong

        ''' <summary>
        ''' The stack pointer register.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Rsp;</code>
        ''' </example>
        Friend Rsp As ULong

        ''' <summary>
        ''' The base pointer register.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Rbp;</code>
        ''' </example>
        Friend Rbp As ULong

        ''' <summary>
        ''' The general-purpose register RSI.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Rsi;</code>
        ''' </example>
        Friend Rsi As ULong

        ''' <summary>
        ''' The general-purpose register RDI.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Rdi;</code>
        ''' </example>
        Friend Rdi As ULong

        ''' <summary>
        ''' The general-purpose register R8.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 R8;</code>
        ''' </example>
        Friend R8 As ULong

        ''' <summary>
        ''' The general-purpose register R9.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 R9;</code>
        ''' </example>
        Friend R9 As ULong

        ''' <summary>
        ''' The general-purpose register R10.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 R10;</code>
        ''' </example>
        Friend R10 As ULong

        ''' <summary>
        ''' The general-purpose register R11.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 R11;</code>
        ''' </example>
        Friend R11 As ULong

        ''' <summary>
        ''' The general-purpose register R12.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 R12;</code>
        ''' </example>
        Friend R12 As ULong

        ''' <summary>
        ''' The general-purpose register R13.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 R13;</code>
        ''' </example>
        Friend R13 As ULong

        ''' <summary>
        ''' The general-purpose register R14.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 R14;</code>
        ''' </example>
        Friend R14 As ULong

        ''' <summary>
        ''' The general-purpose register R15.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 R15;</code>
        ''' </example>
        Friend R15 As ULong

        ''' <summary>
        ''' The instruction pointer register.
        ''' </summary>
        ''' <example>
        ''' In C++: <code>DWORD64 Rip;</code>
        ''' </example>
        Friend Rip As ULong
    End Structure
End Namespace
