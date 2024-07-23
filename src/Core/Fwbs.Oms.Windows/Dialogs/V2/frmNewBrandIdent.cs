using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    using Screen = System.Windows.Forms.Screen;

    enum TitleBarStyle
    {
        System,
        Small,
        Large
    }

    internal partial class frmNewBrandIdent : BaseForm
    {

        #region Window Update Locker

        /// <summary>
        /// Disables or enables drawing in the specified window. Only one window can be locked at a time.
        /// </summary>
        protected class WindowUpdateLocker : IDisposable
        {
            private readonly IntPtr hWnd;

            internal WindowUpdateLocker(Form form)
            {
                hWnd = form.Handle;
                Lock();
            }

            internal bool Lock()
            {
                return LockWindowUpdate(hWnd);
            }

            internal void Unlock()
            {
                LockWindowUpdate(IntPtr.Zero);
            }

            void IDisposable.Dispose()
            {
                Unlock();
                GC.SuppressFinalize(this);
            }
        }

        #endregion

        #region Control Fields

        private System.ComponentModel.IContainer components = null;
        private TitleBarButtonsControl titleBarButtons = null;

        #endregion

        private ICustomTitleBarStyle _customTitleBarStyle;

        private readonly bool _dwmEnabled;
        private bool _inSizeMove;
        private MARGINS _margins;
        private Size _taskbarCompensator;
        private Screen _currentScreen;

        private static readonly bool _enableNewStyle;

        static frmNewBrandIdent()
        {
            _enableNewStyle = new Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, @"UI\Tweaks", "EnableNewStyle", "True").ToBoolean();
        }

        private frmNewBrandIdent() : this(TitleBarStyle.System)
        {
        }

        public frmNewBrandIdent(TitleBarStyle titleBarStyle = TitleBarStyle.Small)
        {
            DwmIsCompositionEnabled(out _dwmEnabled);
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
#if DEBUG
            if (Global.IsInDesignMode())
                titleBarStyle = TitleBarStyle.System;
#endif
            switch (titleBarStyle)
            {
                case TitleBarStyle.Large:
                    _customTitleBarStyle = new LargeTitleBarStyle();
                    break;
                case TitleBarStyle.Small:
                    if (_dwmEnabled && _enableNewStyle)
                    {
                        _customTitleBarStyle = new SmallTitleBarStyle();
                        break;
                    }
                    goto default;
                default:
                    _dwmEnabled = false;
                    _customTitleBarStyle = new EmptyTitleBarStyle();
                    break;
            }
            InitializeComponent();
            InitializeMargins();
            AdjustTitleBarLayout(false);
        }

        private void InitializeMargins()
        {
            Size borderSize = Size.Empty;
            if (_dwmEnabled)
            {
                borderSize = SystemInformation.GetBorderSizeForDpi(DeviceDpi);
                borderSize.Width = Math.Max(2, borderSize.Width);
                borderSize.Height = Math.Max(2, borderSize.Height);
            }
            _margins = new MARGINS(borderSize.Width, borderSize.Width, LogicalToDeviceUnits(_customTitleBarStyle.CaptionHeight), borderSize.Height);
        }

        private void AdjustTitleBarLayout(bool formInitialized = true)
        {
            Size borderSize = Size.Empty;
            int captionHeight = formInitialized ? LogicalToDeviceUnits(_customTitleBarStyle.CaptionHeight) : _customTitleBarStyle.CaptionHeight;

            if (_dwmEnabled)
            {
                borderSize = formInitialized ? SystemInformation.GetBorderSizeForDpi(DeviceDpi) : SystemInformation.BorderSize;
                if (formInitialized || DeviceDpi == 96)
                {
                    borderSize.Width = Math.Max(2, borderSize.Width);
                    borderSize.Height = Math.Max(2, borderSize.Height);
                }
            }

            SuspendLayout();
            this.Padding = new Padding(borderSize.Width, captionHeight, borderSize.Width, borderSize.Height);
            if (_dwmEnabled && formInitialized)
            {
                titleBarButtons.SnapToForm(LogicalToDeviceUnits(_customTitleBarStyle.ButtonsAreaHeight));
                titleBarButtons.BringToFront();
            }
            ResumeLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AdjustTitleBarLayout();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (this.IsHandleCreated && _customTitleBarStyle is SmallTitleBarStyle)
            {
                Invalidate();
                Update();
            }
        }

        protected void SetIcon(Images.DialogIcons dialogIcon)
        {
            if (_customTitleBarStyle is SmallTitleBarStyle)
            {
                this.Icon = Images.GetDialogIcon(dialogIcon) ?? this.Icon;
                _customTitleBarStyle = new SmallTitleBarStyle();
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.titleBarButtons = new FWBS.OMS.UI.Windows.TitleBarButtonsControl();
            this.SuspendLayout();
            // 
            // titleBarButtons
            // 
            this.titleBarButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.titleBarButtons.Enabled = false;
            this.titleBarButtons.Location = new System.Drawing.Point(582, 0);
            this.titleBarButtons.Name = "titleBarButtons";
            this.titleBarButtons.Size = new System.Drawing.Size(146, 48);
            this.titleBarButtons.TabIndex = 0;
            this.titleBarButtons.TabStop = false;
            this.titleBarButtons.Visible = false;
            this.titleBarButtons.FormWindowStateChanged += FormWindowStateChanged;
            // 
            // frmNewBrandIdent
            // 
            this.ClientSize = new System.Drawing.Size(728, 579);
            this.Controls.Add(this.titleBarButtons);
            this.Name = "frmNewBrandIdent";
            this.Padding = new System.Windows.Forms.Padding(1, 48, 1, 1);
            this.Controls.SetChildIndex(this.titleBarButtons, 0);
            this.ResumeLayout(false);

        }

        #endregion

        #region Bounds Issue Workaround

        // https://stackoverflow.com/questions/20085317/custom-window-frame-with-dwm-how-to-handle-wm-nccalcsize-correctly
        private bool _trickCreateParams;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (_trickCreateParams) // Remove styles that affect the border size
                {
                    cp.Style &= ~(WS_BORDER | WS_CAPTION | WS_DLGFRAME | WS_THICKFRAME);
                }
                if (_dwmEnabled)
                {
                    cp.ExStyle |= WS_EX_LAYERED;
                }
                return cp;
            }
        }

        protected override void SetClientSizeCore(int x, int y)
        {
            _trickCreateParams = _dwmEnabled;
            base.SetClientSizeCore(x, y);
            _trickCreateParams = false;
        }

        #endregion

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            InitializeMargins();
            AdjustTitleBarLayout();
        }

        protected override void WndProc(ref Message m)
        {
            bool callDefaultProc = _dwmEnabled ? CustomWndProc(ref m) : true;
            if (callDefaultProc)
            {
                base.WndProc(ref m);
            }
        }

        unsafe private bool CustomWndProc(ref Message m)
        {
            bool callDefaultProc = true;
            
            switch (m.Msg)
            {
                case WM_CREATE:
                    {
                        RECT rect;
                        GetWindowRect(m.HWnd, out rect);
                        SetLayeredWindowAttributes(m.HWnd, 0x00000100, 0xFF, LWA_COLORKEY | LWA_ALPHA);
                        SetWindowPos(m.HWnd, IntPtr.Zero, rect.Left, rect.Top, rect.Width, rect.Height, SWP_FRAMECHANGED);
                    }
                    break;

                case WM_MOVE:
                    if (WindowState == FormWindowState.Normal && _currentScreen != null && _currentScreen.GetMonitorHandle() != MonitorFromWindow(m.HWnd, MONITOR_DEFAULTTONEAREST))
                    {
                        _currentScreen = null;
                        RECT rect;
                        GetWindowRect(m.HWnd, out rect);
                        SetWindowPos(m.HWnd, IntPtr.Zero, rect.Left, rect.Top, rect.Width, rect.Height, SWP_NOMOVE | SWP_NOREPOSITION | SWP_NOZORDER | SWP_NOACTIVATE);
                    }
                    break;

                case WM_SETTINGCHANGE:
                    if (m.WParam == (IntPtr)SPI_SETWORKAREA)
                    {
                        goto case WM_DISPLAYCHANGE;
                    }
                    break;

                case WM_DISPLAYCHANGE:
                    if (_currentScreen != null)
                    {
                        SuspendLayout();
                        if (MaximizeBox && !_currentScreen.Primary)
                        {
                            Padding padding = Padding;
                            padding.Bottom = _margins.cyBottomHeight;
                            padding.Right = _margins.cxRightWidth;
                            Padding = padding;
                        }
                        _currentScreen = null;
                        if (WindowState == FormWindowState.Maximized)
                        {
                            WindowState = FormWindowState.Normal;
                            if (!_currentScreen.Bounds.Contains(Location))
                            {
                                Location = _currentScreen.WorkingArea.Location;
                            }
                            WindowState = FormWindowState.Maximized;
                        }
                        ResumeLayout();
                    }
                    break;

                case WM_WINDOWPOSCHANGED:
                    {
                        _trickCreateParams = true;
                        base.WndProc(ref m);
                        _trickCreateParams = false;
                        callDefaultProc = false;
                    }
                    break;

                case WM_ACTIVATE:
                    if (LOWORD(m.WParam.ToInt32()) != 0)
                    {
                        DwmExtendFrameIntoClientArea(m.HWnd, ref _margins);
                    }
                    break;

                case WM_ENTERSIZEMOVE:
                    {
                        _inSizeMove = true;
                    }
                    break;

                case WM_EXITSIZEMOVE:
                    {
                        _inSizeMove = false;
                    }
                    break;

                case WM_NCCALCSIZE:
                    if (m.WParam == (IntPtr)1)
                    {
                        if (_inSizeMove)
                        {
                            ((NCCALCSIZE_PARAMS*)m.LParam)->rectProposed.Top--; // Workaround to minimize flicker
                        }
                        m.Result = IntPtr.Zero;
                        callDefaultProc = false;
                    }
                    break;

                case WM_NCHITTEST:
                    //if (lRet == IntPtr.Zero)
                    {
                        m.Result = HitTestNCA(m.HWnd, m.WParam, m.LParam);
                        if (m.Result != (IntPtr)HTNOWHERE)
                        {
                            callDefaultProc = false;
                        }
                    }
                    break;

                case WM_NCRBUTTONUP:
                    if (m.WParam == (IntPtr)HTCAPTION)
                    {
                        IntPtr hMenu = GetSystemMenu(m.HWnd, false);
                        if (WindowState == FormWindowState.Maximized)
                            EnableMenuItem(hMenu, 0xF010 /*SC_MOVE*/, MF_GRAYED);
                        int sysCmd = TrackPopupMenu(hMenu, TPM_RETURNCMD, GET_X_LPARAM(m.LParam), GET_Y_LPARAM(m.LParam), 0, m.HWnd, IntPtr.Zero);
                        if (sysCmd != 0)
                            SendMessage(m.HWnd, WM_SYSCOMMAND, (IntPtr)sysCmd, m.LParam);
                    }
                    break;
                    
                case WM_GETMINMAXINFO:
                    {
                        GetMinMaxInfo(m.HWnd, (MINMAXINFO*)m.LParam);
                        m.Result = IntPtr.Zero;
                    }
                    break;
            }

            return callDefaultProc;
        }

        unsafe private void GetMinMaxInfo(IntPtr hWnd, MINMAXINFO* mmi)
        {
            _taskbarCompensator = Size.Empty;
            if (_currentScreen == null)
                _currentScreen = Screen.FromHandle(hWnd);

            Rectangle currWorkingArea = _currentScreen.WorkingArea;
            Rectangle currMonitorArea = _currentScreen.Bounds;

            if (!_currentScreen.Primary)
            {
                Rectangle primMonitorArea = Screen.PrimaryScreen.Bounds;
                if (primMonitorArea.Width < currMonitorArea.Width || primMonitorArea.Height < currMonitorArea.Height)
                {
                    _taskbarCompensator.Width = currMonitorArea.Width - currWorkingArea.Width;
                    _taskbarCompensator.Height = currMonitorArea.Height - currWorkingArea.Height;
                    currWorkingArea.Width = primMonitorArea.Width;
                    currWorkingArea.Height = primMonitorArea.Height;
                }
            }

            mmi->ptMaxPosition.x = currWorkingArea.Left - currMonitorArea.Left;
            mmi->ptMaxPosition.y = currWorkingArea.Top - currMonitorArea.Top + 1;
            mmi->ptMaxSize.x = currWorkingArea.Width;
            mmi->ptMaxSize.y = currWorkingArea.Height;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Color mainColor = Color.FromArgb(37, 17, 76); // 0x25114C
            e.Graphics.Clear(mainColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _customTitleBarStyle.Draw(titleBarButtons, e.Graphics, _margins.cyTopHeight, this.Icon);
        }

        private IntPtr HitTestNCA(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            // Get the point coordinates for the hit test.
            Point ptMouse = new Point(GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam));

            // Get the window rectangle.
            RECT rcWindow;
            GetWindowRect(hWnd, out rcWindow);

            // Get the frame rectangle, adjusted for the style without a caption.
            RECT rcFrame = new RECT(0, 0, 0, 0);
            AdjustWindowRectEx(ref rcFrame, WS_OVERLAPPEDWINDOW & ~WS_CAPTION, false, 0);
            ushort uRow = 1, uCol = 1;
            bool fOnResizeBorder = false;

            // Determine if the point is at the top or bottom of the window.
            if (ptMouse.Y >= rcWindow.Top && ptMouse.Y < rcWindow.Top + _margins.cyTopHeight)
            {
                fOnResizeBorder = (ptMouse.Y < (rcWindow.Top - rcFrame.Top));
                uRow = 0;
            }
            else if (ptMouse.Y <= rcWindow.Bottom && ptMouse.Y >= rcWindow.Bottom - Math.Max(_margins.cyBottomHeight, 2))
            {
                uRow = 2;
            }

            // Determine if the point is at the left or right of the window.
            if (ptMouse.X >= rcWindow.Left && ptMouse.X <= rcWindow.Left + Math.Max(_margins.cxLeftWidth, 2))
            {
                uCol = 0; // left side
            }
            else if (ptMouse.X <= rcWindow.Right && ptMouse.X >= rcWindow.Right - Math.Max(_margins.cxRightWidth, 2))
            {
                uCol = 2; // right side
            }

            int[,] hitTests =
            {
                { HTTOPLEFT, fOnResizeBorder ? HTTOP : HTCAPTION, HTTOPRIGHT },
                { HTLEFT, HTNOWHERE, HTRIGHT },
                { HTBOTTOMLEFT, HTBOTTOM, HTBOTTOMRIGHT }
            };

            return (IntPtr)hitTests[uRow, uCol];
        }

        private void FormWindowStateChanged(object sender, EventArgs e)
        {
            if (MaximizeBox && !_taskbarCompensator.IsEmpty)
            {
                Padding padding = Padding;
                padding.Bottom = _margins.cyBottomHeight;
                padding.Right = _margins.cxRightWidth;
                if (WindowState == FormWindowState.Maximized)
                {
                    padding.Bottom += _taskbarCompensator.Height;
                    padding.Right += _taskbarCompensator.Width;
                }
                Padding = padding;
            }
        }

        #region Native Methods

        private static int GET_X_LPARAM(IntPtr lParam)
        {
            return LOWORD(lParam.ToInt32());
        }

        private static int GET_Y_LPARAM(IntPtr lParam)
        {
            return HIWORD(lParam.ToInt32());
        }

        private static int HIWORD(int i)
        {
            return (short)(i >> 16);
        }

        private static int LOWORD(int i)
        {
            return (short)(i & 0xFFFF);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle rectangle)
            {
                Left = rectangle.X;
                Top = rectangle.Y;
                Right = rectangle.Right;
                Bottom = rectangle.Bottom;
            }

            public int Width
            {
                get { return Right - Left; }
            }

            public int Height
            {
                get { return Bottom - Top; }
            }

            public Rectangle ToRectangle()
            {
                return new Rectangle(Left, Top, Right - Left, Bottom - Top);
            }

            public override string ToString()
            {
                return string.Format("Left: {0}, Top: {1}, Right: {2}, Bottom: {3}", Left, Top, Right, Bottom);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NCCALCSIZE_PARAMS
        {
            public RECT rectProposed;
            public RECT rectBeforeMove;
            public RECT rectClientBeforeMove;
            public IntPtr lpPos; // WINDOWPOS lpPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPOS
        {
            public IntPtr hWnd;
            public IntPtr hWndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }

        private const int WM_CREATE = 0x0001;
        private const int WM_MOVE = 0x0003;
        private const int WM_ACTIVATE = 0x0006;
        private const int WM_SETTINGCHANGE = 0x001A;
        private const int WM_DISPLAYCHANGE = 0x007E;
        private const int WM_GETMINMAXINFO = 0x0024;
        private const int WM_WINDOWPOSCHANGED = 0x0047;
        private const int WM_NCRBUTTONUP = 0x00A5;
        private const int WM_NCCALCSIZE = 0x0083;
        private const int WM_NCHITTEST = 0x0084;
        private const int WM_SYSCOMMAND = 0x0112;
        private const int WM_ENTERSIZEMOVE = 0x0231;
        private const int WM_EXITSIZEMOVE = 0x0232;

        private const int
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = unchecked((int)0x80000000),
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_CAPTION = 0x00C00000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_TABSTOP = 0x00010000,
            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000;
        private const int WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);

        private const int WS_EX_LAYERED = 0x00080000;

        private const int SPI_SETWORKAREA = 0x002F;

        private const int
            SWP_NOSIZE = 0x0001,
            SWP_NOMOVE = 0x0002,
            SWP_NOZORDER = 0x0004,
            SWP_NOREDRAW = 0x0008,
            SWP_NOACTIVATE = 0x0010,
            SWP_FRAMECHANGED = 0x0020,
            SWP_SHOWWINDOW = 0x0040,
            SWP_HIDEWINDOW = 0x0080,
            SWP_NOCOPYBITS = 0x0100,
            SWP_NOOWNERZORDER = 0x0200,
            SWP_NOSENDCHANGING = 0x0400,
            SWP_NOREPOSITION = SWP_NOOWNERZORDER,
            SWP_DEFERERASE = 0x2000,
            SWP_ASYNCWINDOWPOS = 0x4000;

        private const int
            HTERROR = -2,
            HTTRANSPARENT = -1,
            HTNOWHERE = 0,
            HTCLIENT = 1,
            HTCAPTION = 2,
            HTSYSMENU = 3,
            HTGROWBOX = 4,
            HTMENU = 5,
            HTHSCROLL = 6,
            HTVSCROLL = 7,
            HTMINBUTTON = 8,
            HTMAXBUTTON = 9,
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17,
            HTBORDER = 18,
            HTOBJECT = 19,
            HTCLOSE = 20,
            HTHELP = 21,
            HTMDIMAXBUTTON = 66,
            HTMDIMINBUTTON = 67,
            HTMDICLOSE = 68;

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool AdjustWindowRectEx(ref RECT lpRect, uint dwStyle, bool bMenu, uint dwExStyle);

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern int EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        private const uint MF_ENABLED = 0, MF_GRAYED = 1, MF_DISABLED = 2;

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

        private const int TPM_RETURNCMD = 0x0100;

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern bool LockWindowUpdate(IntPtr hWndLock);

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte bAlpha, uint dwFlags);

        private const uint LWA_COLORKEY = 0x00000001, LWA_ALPHA = 0x00000002;

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

        private const uint MONITOR_DEFAULTTONULL = 0, MONITOR_DEFAULTTOPRIMARY = 1, MONITOR_DEFAULTTONEAREST = 2;

        [StructLayout(LayoutKind.Sequential)]
        private struct MARGINS
        {
            public int cxLeftWidth, cxRightWidth, cyTopHeight, cyBottomHeight;

            public MARGINS(int left, int right, int top, int bottom)
            {
                cxLeftWidth = left;
                cyTopHeight = top;
                cxRightWidth = right;
                cyBottomHeight = bottom;
            }
        }

        [DllImport("Dwmapi.dll", ExactSpelling = true)]
        private static extern int DwmIsCompositionEnabled(out bool enabled);

        [DllImport("Dwmapi.dll", ExactSpelling = true)]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

        [DllImport("Dwmapi.dll", ExactSpelling = true)]
        private static extern bool DwmDefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, out IntPtr plResult);

        #endregion Native Methods
    }
}
