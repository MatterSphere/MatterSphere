using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Dialogs.Common
{
    class CheckedMessageBox: System.Windows.Forms.UserControl
    {
        protected LocalCbtHook m_cbt;
		protected IntPtr m_hwnd = IntPtr.Zero;
		protected IntPtr m_hwndBtn = IntPtr.Zero;
		protected bool m_bInit = false;
		protected bool m_bCheck = false;
		protected string m_strCheck;

		public CheckedMessageBox()
		{
			m_cbt = new LocalCbtHook();
			m_cbt.WindowCreated += new LocalCbtHook.CbtEventHandler(WindowCreated);
			m_cbt.WindowDestroyed += new LocalCbtHook.CbtEventHandler(WindowDisposed);
			m_cbt.WindowActivated += new LocalCbtHook.CbtEventHandler(WindowActivated);
		}


        public DialogResult Show(DialogResult drResult, string checkBoxText, string messageText, string messageTitle, MessageBoxButtons messageButtons, MessageBoxIcon messageIcon)
        {
            m_strCheck = checkBoxText;
            m_cbt.Install();
            drResult = System.Windows.Forms.MessageBox.Show(messageText, messageTitle, messageButtons, messageIcon);
            m_cbt.Uninstall();
            return drResult;
        }

		private void WindowCreated(object sender, CbtEventArgs e)
		{
			if (e.IsDialogWindow)
			{
				m_bInit = false;
				m_hwnd = e.Handle;
			}
		}

        private void WindowDisposed(object sender, CbtEventArgs e)
		{
			if (e.Handle == m_hwnd)
			{
				m_bInit = false;
				m_hwnd = IntPtr.Zero;
                if (BST_CHECKED == (int)SendMessage(m_hwndBtn, BM_GETCHECK, IntPtr.Zero, IntPtr.Zero))
                {
                    m_bCheck = true;
                    Session.CurrentSession.CurrentUser.HideCancellationConfirmationDialog = FWBS.Common.TriState.True;
                }
			}
		}

        private void WindowActivated(object sender, CbtEventArgs e)
		{
			if (m_hwnd != e.Handle)
				return;

			// Not the first time
			if (m_bInit)
				return;
			else
				m_bInit = true;

			// Get the current font, either from the static text window or the message box itself
			IntPtr hwndText = GetDlgItem(m_hwnd, 0xFFFF);
            IntPtr hFont = SendMessage(hwndText != IntPtr.Zero ? hwndText : m_hwnd, WM_GETFONT, IntPtr.Zero, IntPtr.Zero);
			Font fCur = Font.FromHfont(hFont);

            float scaleFactor = CalcScaleFactor();
            // Get the x coordinate for the check box.  Align it with the icon if possible,
            // or one character height in
            int x = 0;
			IntPtr hwndIcon = GetDlgItem(m_hwnd, 0x0014);
            if (hwndIcon != IntPtr.Zero)
            {
                RECT rcIcon = new RECT();
                GetClientRect(hwndIcon, rcIcon);
                POINT pt = new POINT() { x = rcIcon.left, y = rcIcon.top };
                MapWindowPoints(hwndIcon, m_hwnd, pt, 1);
                x = Convert.ToInt32(pt.x / scaleFactor);
            }
            else
            {
                x = Convert.ToInt32(fCur.GetHeight());
            }

			// Get the y coordinate for the check box, which is the bottom of the
			// current message box client area
			RECT rc = new RECT();
			GetClientRect(m_hwnd, rc);
			int y = Convert.ToInt32(fCur.GetHeight() + rc.Height / scaleFactor);

			// Resize the message box with room for the check box
			GetWindowRect(m_hwnd, rc);
			MoveWindow(m_hwnd, rc.left, rc.top, rc.Width, rc.Height + Convert.ToInt32(3*fCur.GetHeight()*scaleFactor), true);

            using (var contextBlock = Windows.DPIAwareness.IsSupported ? new Windows.DPIContextBlock(Windows.DPIAwareness.FromWindow(m_hwnd)) : null)
            {   // Size and position of child checkbox control should be in logical units
                m_hwndBtn = CreateWindowEx(0, "BUTTON", m_strCheck, BS_AUTOCHECKBOX | WS_CHILD | WS_VISIBLE | WS_TABSTOP,
                    x, y, Convert.ToInt32(rc.Width / scaleFactor) - x, Convert.ToInt32(fCur.GetHeight()),
                    m_hwnd, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }

            SendMessage(m_hwndBtn, WM_SETFONT, hFont, new IntPtr(1));
            fCur.Dispose();
        }

        private float CalcScaleFactor()
        {
            float scaleFactor = 1.0F;
            if (Windows.DPIAwareness.IsSupported)
            {
                RECT rcPhysical = new RECT(), rcLogical = new RECT();
                GetWindowRect(m_hwnd, rcPhysical);

                POINT pt = new POINT() { x = rcPhysical.left, y = rcPhysical.top };
                PhysicalToLogicalPointForPerMonitorDPI(m_hwnd, pt);
                rcLogical.left = pt.x; rcLogical.top = pt.y;

                pt = new POINT() { x = rcPhysical.right, y = rcPhysical.bottom };
                PhysicalToLogicalPointForPerMonitorDPI(m_hwnd, pt);
                rcLogical.right = pt.x; rcLogical.bottom = pt.y;

                scaleFactor = (float)rcPhysical.Width / (float)rcLogical.Width;
            }
            return scaleFactor;
        }

		#region Win32 Imports
		private const int WS_VISIBLE		= 0x10000000;
		private const int WS_CHILD			= 0x40000000;
		private const int WS_TABSTOP        = 0x00010000;
		private const int WM_SETFONT		= 0x00000030;
		private const int WM_GETFONT		= 0x00000031;
		private const int BS_AUTOCHECKBOX	= 0x00000003; 
		private const int BM_GETCHECK       = 0x00F0;
		private const int BST_CHECKED       = 0x0001;

		[DllImport("User32.dll", SetLastError = true)]
		protected static extern bool DestroyWindow(IntPtr hwnd);
	
		[DllImport("User32.dll", SetLastError = true)]
		protected static extern IntPtr GetDlgItem(IntPtr hwnd, int id);
		
		[DllImport("User32.dll", SetLastError = true)]
		protected static extern bool GetWindowRect(IntPtr hwnd, RECT rc);
		
		[DllImport("User32.dll", SetLastError = true)]
		protected static extern bool GetClientRect(IntPtr hwnd, RECT rc);
		
		[DllImport("User32.dll", SetLastError = true)]
		protected static extern bool MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
		
		[DllImport("User32.dll", ExactSpelling = true)] // Minimum supported client: Windows 8.1
        protected static extern bool PhysicalToLogicalPointForPerMonitorDPI(IntPtr hWnd, POINT lpPoint);

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        protected static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] POINT pt, int cPoints);
		
		[DllImport("User32.dll", EntryPoint="MessageBox")]
		protected static extern int _MessageBox(IntPtr hwnd, string text, string caption,
			int options);
	
		[DllImport("User32.dll")]
		protected static extern IntPtr SendMessage(IntPtr hwnd, 
			int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("User32.dll", SetLastError = true)]
		protected static extern IntPtr CreateWindowEx(
			int dwExStyle,			// extended window style
			string lpClassName,		// registered class name
			string lpWindowName,	// window name
			int dwStyle,			// window style
			int x,					// horizontal position of window
			int y,					// vertical position of window
			int nWidth,				// window width
			int nHeight,			// window height
			IntPtr hWndParent,      // handle to parent or owner window
			IntPtr hMenu,			// menu handle or child identifier
			IntPtr hInstance,		// handle to application instance
			IntPtr lpParam			// window-creation data
			);
	
		[StructLayout(LayoutKind.Sequential)]
		protected class POINT
		{ 
			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
        protected class RECT
		{ 
			public int left;
			public int top;
			public int right;
			public int bottom;

            public int Width { get { return right - left; } }
            public int Height { get { return bottom - top; } }
        }

		#endregion
    }
}
