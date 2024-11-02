'Imports System.Runtime.InteropServices
'Imports ProcTerm12.CoreServices.WindowsApiInterop

'Public Class ModuleEnumerator
'    ' Win32 Constants
'    Public Const TH32CS_SNAPMODULE As Integer = &H8
'    Public Const TH32CS_SNAPMODULE32 As Integer = &H10
'    Public Const INVALID_HANDLE_VALUE As Integer = -1
'    Public Const MAX_PATH As Integer = 260
'    Public Const MAX_MODULE_NAME32 As Integer = 255
'    Friend ReadOnly InvalidHandleValue As New IntPtr(-1)

'    ''' <summary>
'    ''' Specifies that the snapshot created by CreateToolhelp32Snapshot will include all modules 
'    ''' in the specified process.
'    ''' </summary>
'    ''' <remarks>
'    ''' This flag allows CreateToolhelp32Snapshot to capture the loaded modules within the process.
'    ''' For further details, see:
'    ''' https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-createtoolhelp32snapshot
'    ''' Example C++ representation:
'    ''' <code>
'    ''' #define TH32CS_SNAPMODULE 0x00000008
'    ''' </code>
'    ''' </remarks>
'    Public Const Th32CsSnapModule As UInteger = &H8

'    ''' <summary>
'    ''' Specifies that the snapshot created by CreateToolhelp32Snapshot will include all modules 
'    ''' in the specified process and is specific to 32-bit modules.
'    ''' </summary>
'    ''' <remarks>
'    ''' This flag allows CreateToolhelp32Snapshot to capture the loaded 32-bit modules within the process.
'    ''' For further details, see:
'    ''' https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-createtoolhelp32snapshot
'    ''' Example C++ representation:
'    ''' <code>
'    ''' #define TH32CS_SNAPMODULE32 0x00000010
'    ''' </code>
'    ''' </remarks>
'    Public Const Th32CsSnapModule32 As UInteger = &H10

'    ''' <summary>
'    ''' Specifies that the snapshot created by CreateToolhelp32Snapshot will include all processes and modules.
'    ''' </summary>
'    ''' <remarks>
'    ''' This constant is used to indicate that both regular and 32-bit module snapshots should be taken.
'    ''' For further details, see:
'    ''' https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-createtoolhelp32snapshot
'    ''' Example C++ representation:
'    ''' <code>
'    ''' #define TH32CS_SNAPALL (TH32CS_SNAPMODULE | TH32CS_SNAPMODULE32)
'    ''' </code>
'    ''' </remarks>
'    Public Const Th32CsSnapAll As UInteger = Th32CsSnapModule Or Th32CsSnapModule32


'    '' MODULEENTRY32 Structure - Explicit Unicode version
'    '<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
'    'Public Structure ModuleEntry32
'    '    Friend dwSize As Integer
'    '    Friend th32ModuleID As UInteger
'    '    Friend th32ProcessID As UInteger
'    '    Friend GlblcntUsage As UInteger
'    '    Friend ProccntUsage As UInteger
'    '    Friend modBaseAddr As IntPtr
'    '    Friend modBaseSize As UInteger
'    '    Friend hModule As IntPtr

'    '    <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=256)>
'    '    Friend szModule As String

'    '    <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)>
'    '    Friend szExePath As String
'    'End Structure
'    ' Win32 API Declarations - Explicit Unicode versions
'    <DllImport(ExternDll.Kernel32, SetLastError:=True)>
'    Friend Shared Function CreateToolhelp32Snapshot(
'                                                    <[In]> dwFlags As UInteger,
'                                                    <[In]> th32ProcessId As UInteger
'                                                    ) As IntPtr
'    End Function

'    ''' A handle to the snapshot of the modules. This parameter is passed with the <c>[In]</c> attribute.
'    ''' </param>
'    ''' <param name="lpme">
'    ''' A reference to a <see cref="ModuleEntry32"/> structure that will receive information about the module. 
'    ''' This parameter should not be marked with the <c>[Out]</c> attribute. If marked incorrectly as <c>[Out]</c>, 
'    ''' the function will fail to operate correctly and will not return the expected results. 
'    ''' This is critical because the structure needs to be filled with data from the API call, and marking it incorrectly 
'    ''' can disrupt the expected behavior.
'    ''' </param>
'    ''' <remarks>
'    ''' <para>
'    ''' For this function to work correctly, the <c>CharSet</c> must be set to <c>CharSet.Unicode</c>. This is essential because the Windows API function it calls, <c>Module32FirstW</c>,
'    ''' is specifically the Unicode version. If the <c>CharSet</c> is not set to Unicode, it will default to ANSI, which can lead to unexpected behavior or failure to retrieve module information.
'    ''' </para>
'    ''' <para>
'    ''' The <c>EntryPoint</c> parameter is set to <c>Module32FirstW</c> to indicate that we are using the Unicode version of this function. This is crucial for ensuring that string data is handled
'    ''' correctly when retrieving module names and paths, which may contain Unicode characters.
'    ''' </para>
'    ''' <para>
'    ''' The <c>lpme</c> parameter is declared as a <c>ByRef</c> reference to a <see cref="ModuleEntry32"/> structure. It is important that this parameter is marked with the <c>[Out]</c> attribute.
'    ''' If marked incorrectly, such as using <c>[In]</c>, the function may break or fail to provide the expected output. This is because the structure needs to be populated with data from the API call,
'    ''' which requires it to be treated as an output parameter.
'    ''' 
'    ''' The <c>lpme</c> parameter should not be marked with the <c>[Out]</c> attribute. If it is incorrectly marked as <c>[Out]</c>, 
'    ''' the function will fail to operate correctly and will not return the expected results. 
'    ''' This is critical because the structure needs to be filled with data from the API call, and marking it incorrectly can disrupt the expected behavior.
'    ''' </para>
'    ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-module32firstw">Module32FirstW documentation</see> for more information.
'    ''' </remarks>


'    <DllImport(ExternDll.Kernel32, SetLastError:=True, CharSet:=CharSet.Unicode, EntryPoint:="Module32FirstW")>
'    Friend Shared Function Module32First(
'        <[In]> hSnapshot As IntPtr,
'         ByRef lpme As ModuleEntry32
'    ) As <MarshalAs(UnmanagedType.Bool)> Boolean
'    End Function

'    ''' <summary>
'    ''' Retrieves information about the next module in a module snapshot.
'    ''' </summary>
'    ''' <param name="hSnapshot">
'    ''' A handle to the snapshot of the modules. This parameter is passed with the <c>[In]</c> attribute.
'    ''' </param>
'    ''' <param name="lpme">
'    ''' A reference to a <see cref="ModuleEntry32"/> structure that receives information about the module. 
'    ''' This parameter is passed with the <c>[Out]</c> attribute.
'    ''' </param>
'    ''' <returns>
'    ''' If the function succeeds, the return value is nonzero. If the function fails or there are no more modules, the return value is zero.
'    ''' </returns>
'    ''' <remarks>
'    ''' <para>
'    ''' For this function to work correctly, the <c>CharSet</c> must be set to <c>CharSet.Unicode</c>. This is essential because the Windows API function it calls, <c>Module32NextW</c>, is specifically the Unicode version. If the <c>CharSet</c> is not set to Unicode, it will default to ANSI, which can lead to unexpected behavior or failure to retrieve module information.
'    ''' </para>
'    ''' <para>
'    ''' The <c>EntryPoint</c> parameter is set to <c>Module32NextW</c> to indicate that we are using the Unicode version of this function. This is crucial for ensuring that string data is handled correctly when retrieving module names and paths, which may contain Unicode characters.
'    ''' </para>
'    ''' <para>
'    ''' The <c>lpme</c> parameter is declared as a <c>ByRef</c> reference to a <see cref="ModuleEntry32"/> structure. It is important that this parameter is marked with the <c>[Out]</c> attribute. If marked incorrectly, such as using <c>[In]</c>, the function may break or fail to provide the expected output. This is because the structure needs to be populated with data from the API call, which requires it to be treated as an output parameter.
'    ''' </para>
'    ''' For more details, refer to the <see href="https://learn.microsoft.com/en-us/windows/win32/api/toolhelp/nf-toolhelp-module32next">Module32Next documentation</see> for more information.
'    ''' 
'    ''' The function signature in C++ is:
'    ''' <code>
'    ''' BOOL Module32Next(
'    '''   HANDLE hSnapshot,
'    '''   LPMODULEENTRY32 lpme
'    ''' );
'    ''' </code>
'    ''' </remarks>
'    <DllImport(ExternDll.Kernel32, CharSet:=CharSet.Unicode, EntryPoint:="Module32NextW", SetLastError:=True)>
'    Friend Shared Function Module32Next(
'        <[In]> hSnapshot As IntPtr,
'        ByRef lpme As ModuleEntry32
'    ) As <MarshalAs(UnmanagedType.Bool)> Boolean
'    End Function



'    <DllImport("kernel32.dll", SetLastError:=True)>
'    Public Shared Function CloseHandle(ByVal hObject As IntPtr) As Boolean
'    End Function


'End Class