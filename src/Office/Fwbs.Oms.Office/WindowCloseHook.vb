Imports System.Runtime.InteropServices
Imports System.Text
Imports FWBS.OMS.UI.Dialogs.Common

Class WindowCloseHook
    Inherits LocalWindowsHook
    Implements IDisposable

    Private ReadOnly windowClassName As String
    Private ReadOnly handleWmClose As Boolean

    Public Sub New(ByVal windowClassName As String, ByVal handleWmClose As Boolean)
        MyBase.New(HookType.WH_GETMESSAGE)
        Me.windowClassName = windowClassName
        Me.handleWmClose = handleWmClose
        m_filterFunc = New HookProc(AddressOf GetMsgProc)
    End Sub

    Private Function GetMsgProc(ByVal code As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
        If (code >= 0 AndAlso wParam = 1) Then
            Dim msg As Crownwood.Magic.Win32.MSG = Marshal.PtrToStructure(Of Crownwood.Magic.Win32.MSG)(lParam)
            If ((msg.message = WM_CLOSE AndAlso handleWmClose) OrElse (msg.message = WM_SYSCOMMAND AndAlso msg.wParam = SC_CLOSE AndAlso handleWmClose = False)) Then
                If (IsTargetWindow(msg.hwnd) AndAlso IsSingleWindow(msg.hwnd) AndAlso ShowCloseWarning()) Then
                    msg.message = WM_NULL
                    Marshal.StructureToPtr(msg, lParam, False)
                End If
            End If
        End If
        Return CallNextHookEx(m_hhook, code, wParam, lParam)
    End Function

    Private Function IsTargetWindow(ByVal hWnd As IntPtr) As Boolean
        If IsWindowVisible(hWnd) Then
            Dim sb As New StringBuilder(32)
            Return GetClassName(hWnd, sb, sb.Capacity) > 0 AndAlso sb.ToString().Equals(windowClassName, StringComparison.OrdinalIgnoreCase)
        End If
        Return False
    End Function

    Private Function IsSingleWindow(ByVal hWndCurrent As IntPtr) As Boolean
        If handleWmClose Then
            Return Not FWBS.Common.Functions.GetWindowCaption(hWndCurrent).Contains("-")
        End If
#Disable Warning BC40000
        Return EnumThreadWindows(AppDomain.GetCurrentThreadId,
                    Function(hWnd, lParam) hWnd = hWndCurrent OrElse Not IsTargetWindow(hWnd),
                    IntPtr.Zero)
#Enable Warning BC40000
    End Function

    Private Function ShowCloseWarning() As Boolean
        If (Session.CurrentSession.IsConnected) Then
            For Each frm As System.Windows.Forms.Form In System.Windows.Forms.Application.OpenForms
                If (frm.Visible) Then
                    MessageBox.ShowInformation("OPENFORMS", "Please close all forms before disconnecting from the system.")
                    Return True
                End If
            Next
        End If
        Return False
    End Function

#Region "IDisposable"
    Protected Overrides Sub Finalize()
        Uninstall()
        MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Uninstall()
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#Region "Native Methods"
    Private Const WM_NULL As Integer = &H0
    Private Const WM_CLOSE As Integer = &H10
    Private Const WM_SYSCOMMAND As Integer = &H112
    Private Const SC_CLOSE As Integer = &HF060

    Private Delegate Function EnumThreadWndProc(ByVal hWnd As IntPtr, ByVal lParam As IntPtr) As Boolean

    <DllImport("User32.dll", ExactSpelling:=True)>
    Private Shared Function EnumThreadWindows(ByVal dwThreadId As Integer, ByVal lpfn As EnumThreadWndProc, ByVal lParam As IntPtr) As Boolean
    End Function

    <DllImport("User32.dll", ExactSpelling:=True)>
    Private Shared Function IsWindowVisible(ByVal hWnd As IntPtr) As Boolean
    End Function

    <DllImport("User32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
    Private Shared Function GetClassName(ByVal hWnd As IntPtr, ByVal lpClassName As StringBuilder, ByVal nMaxCount As Integer) As Integer
    End Function
#End Region

End Class
