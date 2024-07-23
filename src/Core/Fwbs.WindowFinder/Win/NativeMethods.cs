using System;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace Fwbs
{
    namespace WinFinder
    {
        using System.Runtime.InteropServices;

        internal static class NativeMethods
        {
            private const string UserDll = "user32.dll";
            private const string DwmDll = "dwmapi.dll";
            private const string GdiDll = "gdi32.dll";
            private const string KernelDll = "kernel32.dll";

            #region Vista Specific

            private static UnmanagedLibrary Dwm;
            public static int DwmRegisterThumbnail(HandleRef destHwnd, HandleRef srcHwnd, out IntPtr thumb)
            {
                if (Dwm == null)
                    Dwm = new UnmanagedLibrary(DwmDll);
                return Dwm.GetUnmanagedFunction<DwmRegisterThumbnailDelegate>
                    (System.Reflection.MethodInfo.GetCurrentMethod().Name).Invoke(
                    destHwnd, srcHwnd, out thumb); 
            }

            public static int DwmUnregisterThumbnail(IntPtr thumb)
            {
                if (Dwm == null)
                    Dwm = new UnmanagedLibrary(DwmDll);
                return Dwm.GetUnmanagedFunction<DwmUnregisterThumbnailDelegate>
                    (System.Reflection.MethodInfo.GetCurrentMethod().Name).Invoke(
                    thumb); 
            }

            public static int DwmQueryThumbnailSourceSize(IntPtr thumb, out System.Drawing.Size size)
            {
                if (Dwm == null)
                    Dwm = new UnmanagedLibrary(DwmDll);
                return Dwm.GetUnmanagedFunction<DwmQueryThumbnailSourceSizeDelegate>
                    (System.Reflection.MethodInfo.GetCurrentMethod().Name).Invoke(
                    thumb, out size); 
            }

            public static int DwmUpdateThumbnailProperties(IntPtr hThumb, ref DwmThumbailProperties props)
            {
                if (Dwm == null)
                    Dwm = new UnmanagedLibrary(DwmDll);
                return Dwm.GetUnmanagedFunction<DwmUpdateThumbnailPropertiesDelegate>
                    (System.Reflection.MethodInfo.GetCurrentMethod().Name).Invoke(
                    hThumb, ref props); 
            }


            private delegate int DwmRegisterThumbnailDelegate(HandleRef destHwnd, HandleRef srcHwnd, out IntPtr thumb);
            private delegate int DwmUnregisterThumbnailDelegate(IntPtr thumb);
            private delegate int DwmQueryThumbnailSourceSizeDelegate(IntPtr thumb, out System.Drawing.Size size);
            private delegate int DwmUpdateThumbnailPropertiesDelegate(IntPtr hThumb, ref DwmThumbailProperties props);

            [StructLayout(LayoutKind.Sequential)]
            public struct DwmThumbailProperties
            {
                public DwmThumbnailPropertyFlags dwFlags;
                public System.Drawing.Rectangle rcDestination;
                public System.Drawing.Rectangle rcSource;
                public byte opacity;
                public bool fVisible;
                public bool fSourceClientAreaOnly;
            }

            [Flags]
            public enum DwmThumbnailPropertyFlags
            {
                /// <summary>
                /// Indicates a value for rcDestination has been specified.
                /// </summary>
                RectDestination = 0x00000001,
                /// <summary>
                /// Indicates a value for rcSource has been specified.
                /// </summary>
                RectSource = 0x00000002,
                /// <summary>
                /// Indicates a value for opacity has been specified.
                /// </summary>
                Opacity = 0x00000004,
                /// <summary>
                /// Indicates a value for fVisible has been specified.
                /// </summary>
                Visible = 0x00000008,
                /// <summary>
                /// Indicates a value for fSourceClientAreaOnly has been specified.
                /// </summary>
                SourceClientAreaOnly = 0x00000010
            }

            #endregion

            #region API Functions

            #region GDI

            [DllImport(GdiDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool PatBlt(IntPtr hdcDest, int left, int top, int width, int height, BitBltFlags rop);
            [DllImport(GdiDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, BitBltFlags flags);
            [DllImport(GdiDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool StretchBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXSrc, int nYSrc, int nWidthSrc, int nHeightSrc, BitBltFlags flags);

            #endregion

            #region Kernel


            [DllImport(KernelDll, CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern SafeLibraryHandle LoadLibrary(string fileName);
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [DllImport(KernelDll, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool FreeLibrary(IntPtr hModule);
            [DllImport(KernelDll, CharSet = CharSet.Ansi, ExactSpelling = true, BestFitMapping = false, SetLastError = true)]
            internal static extern IntPtr GetProcAddress(SafeLibraryHandle hModule, String procname);

            #endregion

            #region User

            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetClientRect(HandleRef hWnd, ref System.Drawing.Rectangle rect);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetWindowRect(HandleRef hWnd, ref System.Drawing.Rectangle rect);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool UpdateWindow(HandleRef hWnd);

            // Used to register the special Windows messages 
            // used for this application.
            [DllImport(UserDll, CharSet = CharSet.Unicode)]
            internal static extern int RegisterWindowMessage(string Msg);

            //internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [DllImport(UserDll, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern IntPtr FindWindowEx(IntPtr parentHWnd, IntPtr childAfter, string lpClassName, string lpWindowName);
            [DllImport(UserDll)]
            internal static extern IntPtr GetWindow(HandleRef hWnd, GetWindowFlags uCmd);
            [DllImport(UserDll)]
            internal static extern IntPtr GetDesktopWindow();
            [DllImport(UserDll)]
            public static extern IntPtr GetParent(HandleRef hwnd);
            [DllImport(UserDll)]
            public static extern IntPtr GetParent(IntPtr hwnd);
            [DllImport(UserDll)]
            public static extern IntPtr GetActiveWindow();
            [DllImport(UserDll, SetLastError = true, CharSet = CharSet.Unicode)]
            private static extern int GetClassName(HandleRef hWnd, [Out] System.Text.StringBuilder lpClassName, int nMaxCount);
            [DllImport(UserDll, SetLastError = true, CharSet = CharSet.Unicode)]
            private static extern int GetWindowText(HandleRef hWnd, [Out]System.Text.StringBuilder lpWindowText, int nMaxCount);

            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, WindowFilter filter);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool EnumChildWindows(IntPtr hWnd, EnumWindowsProc lpEnumFunc, WindowFilter filter);
            [DllImport(UserDll)]
            internal static extern IntPtr GetAncestor(HandleRef hWnd, GetAncestorFlags flags);


            [DllImport(UserDll)]
            public static extern int GetWindowThreadProcessId(HandleRef hWnd, out int ProcessId);

            [DllImport(UserDll)]
            public static extern IntPtr SetParent(HandleRef hWndChild, IntPtr hWndNewParent);
            [DllImport(UserDll)]
            public static extern IntPtr SetParent(HandleRef hWndChild, HandleRef hWndNewParent);

            [DllImport(UserDll)]
            public static extern short CascadeWindows(HandleRef hwndParent, MdiTitle wHow, IntPtr lpRect, int cKids, IntPtr[] lpKids);
            [DllImport(UserDll)]
            public static extern short TileWindows(HandleRef hwndParent, MdiTitle wHow, IntPtr lpRect, int cKids, IntPtr[] lpKids);

            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool ShowWindow(HandleRef hWnd, ShowWindowStyles nCmdShow);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool ShowWindowAsync(HandleRef hWnd, ShowWindowStyles nCmdShow);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetWindowPos(HandleRef hWnd, SetWindowPosZOrder hWndAfter, int X, int Y, int Width, int Height, SetWindowPosFlags flags);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool MoveWindow(HandleRef hWnd, int x, int y, int width, int height, [MarshalAs(UnmanagedType.Bool)] bool repaint);

            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool IsWindowUnicode(HandleRef hWnd);

            [DllImport(UserDll)]
            internal static extern IntPtr GetForegroundWindow();
            [DllImport(UserDll)]
            internal static extern IntPtr GetFocus();


            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetTitleBarInfo(HandleRef hWnd, ref TitleBarInfo info);

            [DllImport(UserDll)]
            internal static extern int GetWindowLong(HandleRef hWnd, GetWindowLongFlags Index);
            [DllImport(UserDll)]
            internal static extern int SetWindowLong(HandleRef hWnd, GetWindowLongFlags Index, uint Value);

            [DllImport(UserDll)]
            internal static extern IntPtr SendMessage(HandleRef hWnd, WindowMessage msg, IntPtr param1, IntPtr param2);
            [DllImport(UserDll)]
            internal static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr param1, IntPtr param2);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool PostMessage(HandleRef hWnd, WindowMessage msg, IntPtr param1, IntPtr param2);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool PostMessage(HandleRef hWnd, int msg, IntPtr param1, IntPtr param2);

            [DllImport(UserDll, SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "SendMessageW")]
            internal static extern IntPtr SendMessageText(HandleRef hWnd, WindowMessage msg, IntPtr len, StringBuilder sb);

            [DllImport(UserDll)]
            internal static extern int GetDlgCtrlID(HandleRef hWndCtl);
            [DllImport(UserDll)]
            internal static extern IntPtr GetDlgItem(HandleRef hWnd, int dialogId);

            [DllImport(UserDll, SetLastError = true)]
            internal static extern IntPtr SendMessageTimeout(HandleRef hWnd, WindowMessage msg, IntPtr param1, IntPtr param2, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool IsWindowVisible(HandleRef hwnd);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool IsZoomed(HandleRef hWnd);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool IsIconic(HandleRef hWnd);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool IsChild(HandleRef hWndParent, HandleRef hWnd);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool IsWindow(HandleRef hWnd);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool IsHungAppWindow(HandleRef hWnd);

            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DestroyWindow(HandleRef hWnd);
            [DllImport(UserDll, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool EndTask(HandleRef hWnd, [MarshalAs(UnmanagedType.Bool)]bool shutdown, [MarshalAs(UnmanagedType.Bool)] bool force);
            [DllImport(UserDll)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool FlashWindowEx(ref FlashWindowInfo pwfi);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
            [DllImport(UserDll)]
            internal static extern IntPtr WindowFromPoint(Point point);

            [DllImport("user32.dll")]
            internal static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

            [DllImport(UserDll, SetLastError = true)]
            internal static extern IntPtr SetWindowsHookEx(Hooks.HookType idHook, Hooks.HookProc lpfn, IntPtr hMod, Int32 dwThreadId);

            //// This function unhooks an existing Windows hook.
            [DllImport(UserDll, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern Boolean UnhookWindowsHookEx(IntPtr hhk);

            [DllImport(UserDll, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            internal static extern IntPtr CallNextHookEx(Hooks.HookSafeHandle idHook, int nCode, IntPtr param1, IntPtr param2);

            [DllImport(UserDll, EntryPoint = "GetClassLong")]
            private static extern IntPtr GetClassLongPtr32(HandleRef hWnd, int nIndex);

            [DllImport(UserDll, EntryPoint = "GetClassLongPtr")]
            private static extern IntPtr GetClassLongPtr64(HandleRef hWnd, int nIndex);

            #endregion


            #endregion

            #region Delegates

            internal delegate bool EnumWindowsProc(IntPtr handle, WindowFilter filter);

            #endregion

            #region Structures

            [StructLayout(LayoutKind.Sequential)]
            public struct FlashWindowInfo
            {
                public UInt32 cbSize;
                public IntPtr hwnd;
                public FlashVals dwFlags;
                public UInt32 uCount;
                public UInt32 dwTimeout;
            }

            public struct TitleBarInfo
            {
                public int cbSize;
                public System.Drawing.Rectangle rcTitleBar;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.AsAny)]
                public int[] rgstate;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct Msg
            {
                public IntPtr hwnd;
                public Int32 message;
                public IntPtr param1;
                public IntPtr param2;
                public Int32 time;
                public Point pt;
            }


            [StructLayout(LayoutKind.Sequential)]
            public struct Point
            {
                public int X;
                public int Y;

                public Point(int x, int y)
                {
                    this.X = x;
                    this.Y = y;
                }

                public static implicit operator System.Drawing.Point(Point p)
                {
                    return new System.Drawing.Point(p.X, p.Y);
                }

                public static implicit operator Point(System.Drawing.Point p)
                {
                    return new Point(p.X, p.Y);
                }
            }


            [StructLayout(LayoutKind.Sequential)]
            public struct CopyDataStruct
            {
                public int dwData;
                public int cbData;
                public IntPtr lpData;
            }

            #endregion

            #region Constants & Enums

            [Flags]
            internal enum AnimationFlags
            {
                HorizontalPositive = 0x00000001,
                HorizontalNegative = 0x00000002,
                VerticalPositive = 0x00000004,
                VerticalNegative = 0x00000008,
                Center = 0x00000010,
                Hide = 0x00010000,
                Activate = 0x00020000,
                Slide = 0x00040000,
                Blend = 0x00080000
            }

            [Flags]
            internal enum MdiTitle
            {
                Default = 0,
                Vertical = 1,
                Horizontal = 1,
                SkipDisabled = 2,
                ZOrder = 4
            }

            [Flags]
            internal enum FlashVals : uint
            {
                //Stop flashing. The system restores the window to its original state. 
                Stop = 0,
                //Flash the window caption. 
                Caption = 1,
                //Flash the taskbar button. 
                Tray = 2,
                //Flash both the window caption and taskbar button.
                //This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags. 
                All = 3,
                //Flash continuously, until the FLASHW_STOP flag is set. 
                Continuous = 4,
                //Flash continuously until the window comes to the foreground. 
                Foreground = 12
            }


            public enum GetClassLongFlags
            {
                HIcon = (-14),
                HIconSmall = (-34)
            }

            public const int HC_ACTION = 0;




            public enum GetAncestorFlags
            {
                Parent = 1,
                Root = 2,
                RootOwner = 3
            }

            [Flags]
            public enum ChildWindowPointFlags
            {
                All = 0,
                SkipInvisible = 1,
                SkipDisabled = 2,
                SkipTransparent = 4
            }

            public enum IconSize : int
            {
                Small = 0,
                Big = 1
            }

            [Flags]
            public enum SendMessageTimeoutFlags : uint
            {
                Normal = 0x0000,
                Block = 0x0001,
                AbortIfHung = 0x0002,
                NoTimeoutIfNotHung = 0x0008
            }


            [Flags]
            public enum DrawingOptions : int
            {
                CheckVisible = 0x01,
                NonClient = 0x02,
                Client = 0x04,
                EraseBackground = 0x08,
                Children = 0x10,
                Owned = 0x20
            }

            public enum GetWindowFlags
            {
                First = 0,
                Last = 1,
                Next = 2,
                Previous = 3,
                Owner = 4,
                Child = 5,
                Max = 6,
                EnabledPopup = 6

            }

            [Flags]
            public enum BitBltFlags : uint
            {
                Blackness = 0x42,
                DestInvert = 0x550009,
                CaptureBlt = 0x40000000,
                MergeCopy = 0xC000CA,
                MergePaint = 0xBB0226,
                NoMirrorBitmap = 0x80000000,
                NotSourceCopy = 0x330008,
                NotSourceErase = 0x1100A6,
                PatCopy = 0xF00021,
                PatInvert = 0x5A0049,
                PatPaint = 0xFB0A09,
                SourceCopy = 0xCC0020,
                SourceAnd = 0x8800C6,
                SourceErase = 0x440328,
                SourceInvert = 0x660046,
                SourcePaint = 0xEE0086,
                Whiteness = 0xFF0062
            }



            [Flags]
            public enum SetWindowPosFlags : uint
            {
                NoSize = 0x0001,
                NoMove = 0x0002,
                NoZOrder = 0x0004,
                NoRedraw = 0x0008,
                NoActivate = 0x0010,
                FrameChanged = 0x0020,
                ShowWindow = 0x0040,
                HideWindow = 0x0080,
                NoCopyBits = 0x0100,
                NoOwnerZOrder = 0x0200,
                NoSendChanging = 0x0400,
                DrawFrame = 0x0020,
                NoReposition = 0x0200,
                DeferErase = 0x2000,
                AsyncWindowPos = 0x4000
            }

            public enum SetWindowPosZOrder
            {
                Top = 0,
                Bottom = 1,
                TopMost = -1,
                NoTopMost = -2
            }

            public enum ShowWindowStyles
            {
                Hide = 0,
                ShowNormal = 1,
                Normal = 1,
                ShowMinimised = 2,
                ShowMaximised = 3,
                Maximise = 3,
                ShowNoActivate = 4,
                Show = 5,
                Minimise = 6,
                ShowMinNoActive = 7,
                ShowNA = 8,
                Restore = 9,
                ShowDefault = 10,
                ForceMinimise = 11,
                Max = 11
            }



            public enum GetWindowLongFlags
            {
                WndProc = -4,
                HInstance = -6,
                HwndParent = -8,
                Style = -16,
                ExStyle = -20,
                UserData = -21,
                Id = -12
            }

            #endregion

            #region Methods

            public static IntPtr GetClassLongPtr(HandleRef hWnd, int nIndex)
            {
                try
                {
                    if (IntPtr.Size > 4)
                        return GetClassLongPtr64(hWnd, nIndex);
                    else
                        return GetClassLongPtr32(hWnd, nIndex);
                }
                catch (OverflowException)
                {
                    return IntPtr.Zero;
                }
            }

            static public string GetWindowText(HandleRef handle)
            {
                const int BUFF = 1024;

                StringBuilder sb = new StringBuilder(BUFF);
                int ret = GetWindowText(handle, sb, BUFF);
                if (ret == 0)
                    return String.Empty;
                else
                    return sb.ToString();
            }

            static public string GetWindowTextEx(HandleRef handle)
            {
                const int BUFF = 1024;

                StringBuilder sb = new StringBuilder(BUFF);
                SendMessageText(handle, WindowMessage.GetText, new IntPtr(BUFF), sb);
                return sb.ToString();
            }

            static public void SetWindowText(HandleRef handle, string value)
            {
                StringBuilder sb = new StringBuilder(value);
                SendMessageText(handle, WindowMessage.SetText, new IntPtr(0), sb);
            }

            static public string GetClassName(HandleRef handle)
            {
                const int BUFF = 255;

                StringBuilder sb = new StringBuilder(BUFF);
                int len = GetClassName(handle, sb, BUFF);
                if (len > 0)
                    return sb.ToString();
                else
                    return String.Empty;

            }



            #endregion

        }
    }
}