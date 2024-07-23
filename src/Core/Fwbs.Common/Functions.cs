using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace FWBS.Common
{
    /// <summary>
    /// A mixture of unrelated functions.
    /// Changed API declarations to be IntPnr where required because of some stack imbalance errors
    /// </summary>
    public static class Functions
    {
        #region Terminal Server Issue

        private enum OSProductType : byte
        {
            NTDomainController = 0x0000002, //The system is a domain controller and the operating system is Windows Server 2008, Windows Server 2003, or Windows 2000 Server.
            NTServer = 0x0000003,            //The operating system is Windows Server 2008, Windows Server 2003, or Windows 2000 Server. Note that a server that is also a domain controller is reported as VER_NT_DOMAIN_CONTROLLER, not VER_NT_SERVER.
            NTWorkstation = 0x0000001       //The operating system is Windows Vista, Windows XP Professional, Windows XP Home Edition, or Windows 2000 Professional.
        }

        [Flags]
        private enum OSSuite : short
        {
            Backoffice = 0x00000004,    //Microsoft BackOffice components are installed.
            Blade = 0x00000400,         //Windows Server 2003, Web Edition is installed.
            ComputeServer = 0x00004000, //Windows Server 2003, Compute Cluster Edition is installed.
            DataCenter = 0x00000080,    //Windows Server 2008 Datacenter, Windows Server 2003, Datacenter Edition, or Windows 2000 Datacenter Server is installed.
            Enterprise = 0x00000002,     //Windows Server 2008 Enterprise, Windows Server 2003, Enterprise Edition, or Windows 2000 Advanced Server is installed. Refer to the Remarks section for more information about this bit flag.
            Embedded = 0x00000040,       //Windows XP Embedded is installed.
            Personal = 0x00000200,       //Windows Vista Home Premium, Windows Vista Home Basic, or Windows XP Home Edition is installed.
            SingleUserTS = 0x00000100,   //Remote Desktop is supported, but only one interactive session is supported. This value is set unless the system is running in application server mode.
            SmallBusiness = 0x00000001,  //Microsoft Small Business Server was once installed on the system, but may have been upgraded to another version of Windows. Refer to the Remarks section for more information about this bit flag.
            SmallBusinessRestricted = 0x00000020, //Microsoft Small Business Server is installed with the restrictive client license in force. Refer to the Remarks section for more information about this bit flag.
            StorageServer = 0x00002000, //Windows Storage Server 2003 R2 or Windows Storage Server 2003is installed.
            Terminal = 0x00000010      //Terminal Services is installed. This value is always set. If VER_SUITE_TERMINAL is set but VER_SUITE_SINGLEUSERTS is not set, the system is running in application server mode.
            //HomeServer = 0x00008000      //Windows Home Server
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public OSSuite wSuiteMask;
            public OSProductType wProductType;
            public byte wReserved;
        }



        [DllImport("User32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

        private const int SM_REMOTESESSION = 0x1000;

        public static bool IsTerminalSession
        {
            get
            {
                return (0 != GetSystemMetrics(SM_REMOTESESSION));
            }
        }

        public static bool IsTerminalServer
        {
            get
            {
                OSVERSIONINFOEX vi = new OSVERSIONINFOEX();
                vi.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

                GetVersionEx(ref vi);

                Console.WriteLine(vi.wSuiteMask);

                if ((vi.wSuiteMask & OSSuite.Terminal) == OSSuite.Terminal)
                    return !((vi.wSuiteMask & OSSuite.SingleUserTS) == OSSuite.SingleUserTS);

                return false;

            }
        }

        public static string GetMachineName()
        {
            string name = Environment.MachineName;

            //Only if the host machine is a terminal server and is running in a session do
            //we use session based terminal names.
            if (IsTerminalServer && IsTerminalSession)
            {
                // New feature with FarmName set in Registry // HKLM\Software\FWBS\FarmName
                var farmName = new FWBS.Common.Reg.ApplicationSetting("",
                                                                      "", "", "FarmName");

                //Changed from Environment.GetEnvironmentVariable("SESSIONNAME"); to Environment.UserName; to workaround change in behaviour in Windows 2012 Terminal Server where SESSIONNAME is changed on each logon
                string username = Environment.UserName;
                if (!String.IsNullOrEmpty(username))
                {
                    // Check not blank
                    if (!String.IsNullOrEmpty(Convert.ToString(farmName.GetSetting(""))))
                        name = String.Format("{0}-{1}", Convert.ToString(farmName.GetSetting("")), username);
                    else
                        name = String.Format("{0}-{1}", Environment.MachineName, username);
                }
                else
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(farmName.GetSetting(""))))
                        name = Convert.ToString(farmName.GetSetting(""));
                    else
                        name = Environment.MachineName;
                }

            }

            //Limit to 50 characters in length
            if (name.Length > 50)
                name = name.Substring(0, 50);

            return name;
        }

        /// <summary>
        /// This method can be used to check whether the current execution environment is the computerName string sent through to the method
        /// </summary>
        /// <param name="computerName"></param>
        /// <returns></returns>
        public static Boolean IsCurrentComputer(string computerName)
        {
            // Basic MachineName from the Environment Variable required incase code setting the value didn't use GetMachineName when running on Terminal Server
            if (computerName.ToUpper() == Environment.MachineName.ToUpper())
                return true;

            // Check if the Machine name is compliant if is a Terminal Server Session 
            if (computerName.ToUpper() == GetMachineName().ToUpper())
                return true;

            // If the terminal Server is running on a farm check for Registry key under HKLM\Software\FWBS\OMS\2.0\FarmName and compare against this
            if (IsTerminalServer && IsTerminalSession)
            {
                var farmName = new FWBS.Common.Reg.ApplicationSetting("",
                                                                      "", "", "FarmName");
                string sessionname = Environment.GetEnvironmentVariable("SESSIONNAME");

                // Check not blank
                if (!String.IsNullOrEmpty(Convert.ToString(farmName.GetSetting(""))))
                {
                    // Check Farmname is storedname
                    if (Convert.ToString(farmName.GetSetting("")).ToUpper() == computerName.ToUpper()) return true;

                    if (sessionname.Length > 3)
                    {       // Check farmname is beginning of machinename string
                        if (computerName.ToUpper().StartsWith(Convert.ToString(farmName.GetSetting("")).ToUpper() + "-" + sessionname.Substring(0, 3)))
                            return true;
                    }
                }


                if (sessionname.Length > 3)
                {
                    // Check computername is beginning of machinename string
                    if (computerName.ToUpper().StartsWith(Environment.MachineName.ToUpper() + "-" + sessionname.Substring(0, 3))) return true;
                }


            }


            return false;
        }

        #endregion

        // Window declarations
        private sealed class Win32Window : System.Windows.Forms.IWin32Window
        {
            public IntPtr Handle { get; internal set; }
        }

        //Could be some breaks here as I have made this public and removed my overload that only searched for
        // visible windows
        [DllImport("User32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr FindWindowW(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr parent, IntPtr childAfter, string lpClassName, string lpWindowName);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, [Out]System.Text.StringBuilder lpString, int nMaxCount);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetWindowText(IntPtr hWnd, string s);

        [DllImport("User32.dll", ExactSpelling = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hwnd);

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("User32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern bool IsWindowVisible(IntPtr hwnd);

        [DllImport("User32.dll", ExactSpelling = true)]
        public static extern IntPtr GetActiveWindow();

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd, [Out] System.Text.StringBuilder lpClassName, int nMaxCount);

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("User32.dll", EntryPoint = "SetParent", SetLastError = true)]// handle to window,new parent window
        private static extern IntPtr SetParentWindow(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("User32.dll", ExactSpelling = true)]// handle to window,new parent window
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndAfter, int X, int Y, int Width, int Height, int flags);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int SendMessageTimeout(IntPtr hWnd, int uMsg, int wParam, int lParam, int fuFlags, int uTimeout, out int lpdwResult);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetClassLong(IntPtr hWnd, int index);


        private const int WM_GETICON = 0x7F;
        private const int WM_QUERYDRAGICON = 0x0037;
        private const int ICON_SMALL = 0;
        private const int SMTO_ABORTIFHUNG = 0x2;
        private const int GCL_HICONSM = (-34);

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int GW_OWNER = 4;
        private const int GW_HWNDNEXT = 2;
        private const int GW_HWNDFIRST = 0;
        private const int GW_CHILD = 5;
        private const int SW_HIDE = 0;
        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMISED = 3;
        private const int SW_RESTORE = 9;
        private const int HWND_TOP = 0;
        private const int HWND_BOTTOM = 1;
        private const int HWND_TOPMOST = -1;
        private const int HWND_NOTOPMOST = -2;

        // Menu declarations
        private const int MF_BYPOSITION = 0x400;
        private const int MF_REMOVE = 0x1000;
        private const int MF_DISABLED = 0x2;
        private const int MF_BYCOMMAND = 0;
        private const int WM_CLOSE = 0x0010;
        private const int WM_COMMAND = 0x111;
        private const int MF_SEPERATOR = 0x800;
        private const int WM_SYSCOMMAND = 0x112;

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool RemoveMenu(IntPtr hMenu, uint nPosition, uint wFlags);

        [DllImport("User32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool DrawMenuBar(IntPtr hwnd);

        [DllImport("User32.dll")]
        public static extern IntPtr GetMenu(IntPtr hwnd);

        [DllImport("User32.dll")]
        public static extern IntPtr GetSubMenu(IntPtr hMenu, int nPos);

        [DllImport("User32.dll")]
        public static extern uint GetMenuItemID(IntPtr hMenu, int nPos);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool SetMenuItemBitmaps(IntPtr hMenu, uint nPosition,
            uint wFlags, IntPtr hBitmapUnchecked, IntPtr hBitmapChecked);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetMenuString(IntPtr hmenu, uint widitem,
            [Out, MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpString, int nmaxcount, uint wflag);

        // append a menu item to the menu
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool AppendMenu(IntPtr hMenu, MenuFlags wFlags, uint uIDNewItem, string lpNewItem);

        [Flags]
        public enum MenuFlags : uint
        {
            MF_STRING = 0,
            MF_BYPOSITION = 0x400,
            MF_SEPARATOR = 0x800,
            MF_REMOVE = 0x1000,
        }


        [DllImport("User32.dll")] // handle window, message,first message param,2nd message param
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        //Process declarations
        private const int STILL_ACTIVE = 0x103;
        private const int PROCESS_ALL_ACCESS = 0x1F0FFF;

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool TerminateProcess(IntPtr hProcessID, uint uExitCode);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        #region Window Functions

        /// <summary>
        /// Overload for the find window function that finds only visible windows
        /// </summary>
        /// <param name="className">class name if known</param>
        /// <param name="windowName">window caption</param>
        /// <returns>Handle to the window</returns>
        static public IntPtr FindWindow(string className, string windowName)
        {
            return FindWindow(className, windowName, true);
        }


        static public string GetWindowText(IntPtr hWnd)
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder(255);

            if (GetWindowText(hWnd, s, 255) != 0)
            {
                return s.ToString();
            }
            else
            {
                return "";
            }
        }

        static public string GetWindowClassName(IntPtr hWnd)
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder(255);

            if (GetClassName(hWnd, s, 255) != 0)
            {
                return s.ToString();
            }
            else
            {
                return "";
            }
        }



        static public IntPtr FindWindow(string className, string windowName, bool visibleOnly)
        {
            IntPtr hwnd = FindWindowW(string.IsNullOrEmpty(className) ? null : className, windowName);
            if (hwnd != IntPtr.Zero && GetWindowCaption(hwnd).Equals(windowName, StringComparison.CurrentCultureIgnoreCase))
                return hwnd;

            //now try and find by window name looping around all windows
            //used when window is unpredicatable e.g. Oyez Forms which contain form name
            hwnd = IntPtr.Zero;
            var clsname = string.IsNullOrEmpty(className) ? null : new System.Text.StringBuilder(256);
            var caption = new System.Text.StringBuilder(256);

            EnumWindowsProc enumWindowsProc = (hWnd, lParam) =>
            {
                if (clsname != null && GetClassName(hWnd, clsname, clsname.Capacity) > 0 && !clsname.ToString().Equals(className))
                    return true; // class name doesn't match, continue enumeration

                if (GetWindowText(hWnd, caption, caption.Capacity) > 0 &&
                    caption.ToString().IndexOf(windowName, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (!visibleOnly || IsWindowVisible(hWnd))
                    {
                        hwnd = hWnd;
                        return false; // found desired window, stop enumeration
                    }
                }

                return true; // window name doesn't match, continue enumeration
            };
            EnumWindows(enumWindowsProc, IntPtr.Zero);
            return hwnd;
        }

        /// <summary>
        /// Finds a child window by passing handle to parent and class name
        /// </summary>
        static public IntPtr FindChildWindowByClass(IntPtr parent, string className)
        {
            return FindChildWindowByClass(parent, className, true);
        }

        static private IntPtr FindChildWindowByClass(IntPtr parent, string className, bool topmost)
        {
            IntPtr hwnd = parent;

            className = className.ToUpper();

            if (topmost)
                hwnd = GetWindow(hwnd, GW_CHILD);

            while (hwnd != IntPtr.Zero)
            {
                string caption = GetWindowClassName(hwnd).ToUpper();
                if (caption == className)
                    return hwnd;

                IntPtr child = GetWindow(hwnd, GW_CHILD);
                if (child != IntPtr.Zero)
                {
                    IntPtr ret = FindChildWindowByClass(child, className, false);
                    if (ret != IntPtr.Zero)
                        return ret;
                }

                hwnd = GetWindow(hwnd, GW_HWNDNEXT);
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Finds a child window by passing handle to parent and caption
        /// </summary>
        /// <param name="handle">handle to parent window</param>
        /// <param name="windowName">cpation or part caption</param>
        /// <returns>handle to child window or 0 if not found</returns>
        static public IntPtr FindChildWindow(IntPtr handle, string windowName)
        {
            IntPtr hwnd = GetWindow(handle, GW_CHILD);

            windowName = windowName.ToUpper();

            //keep going until we 
            while (hwnd != IntPtr.Zero)
            {
                string caption = GetWindowCaption(hwnd).ToUpper();
                if (caption.IndexOf(windowName) > -1)
                    return hwnd;
                else
                {
                    //go to 2nd level below the parent
                    IntPtr hsubwnd = GetWindow(hwnd, GW_CHILD);

                    while (hsubwnd != IntPtr.Zero)
                    {
                        string subcaption = GetWindowCaption(hsubwnd).ToUpper();
                        if (subcaption.IndexOf(windowName) > -1)
                            return hsubwnd;

                        hsubwnd = GetWindow(hsubwnd, GW_HWNDNEXT);
                    }

                }

                hwnd = GetWindow(hwnd, GW_HWNDNEXT);
            }
            //if we get here we havn't fount it so returns 0
            return hwnd;
        }




        static public bool WindowVisible(IntPtr hwnd)
        {
            bool ret = IsWindowVisible(hwnd);

            return ret;
        }

        /// <summary>
        /// Gets the caption of the passed window
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns>caption of the window</returns>
        static public string GetWindowCaption(IntPtr hwnd)
        {
            int cnt = GetWindowTextLength(hwnd) + 1;
            System.Text.StringBuilder buffer = new System.Text.StringBuilder(cnt);

            if (GetWindowText(hwnd, buffer, buffer.Capacity) != 0)
                return buffer.ToString().Trim();
            else
                return "";
        }


        /// <summary>
        /// Maximises a window locating it by it's name
        /// </summary>
        /// <param name="caption">Window caption or part of</param>
        static public void MaximizeWindow(string caption)
        {
            //get handle with 
            IntPtr hwnd = FindWindow(null, caption, true);
            if (hwnd != IntPtr.Zero)
                ShowWindow(hwnd, SW_SHOWMAXIMISED);
        }



        /// <summary>
        /// Maximises a window by handle
        /// </summary>
        /// <param name="hwnd">Window handle</param>
        static public void MaximizeWindow(IntPtr hwnd)
        {
            ShowWindow(hwnd, SW_SHOWMAXIMISED);
        }

        /// <summary>
        /// Maximises a window by handle
        /// </summary>
        /// <param name="hwnd">Window handle in INT</param>
        static public void MaximizeWindow(Int32 hwnd)
        {
            ShowWindow(new System.IntPtr(hwnd), SW_SHOWMAXIMISED);
        }


        static public void ActivateWindow(IntPtr hwnd)
        {
            ShowWindow(hwnd, SW_SHOWNORMAL);
            SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }

        /// <summary>
        /// Maximises a window by handle
        /// </summary>
        /// <param name="hwnd">Window handle</param>
        static public void MinimizeWindow(Int32 hwnd)
        {
            ShowWindow(new System.IntPtr(hwnd), SW_SHOWMINIMIZED);
        }

        /// <summary>
        /// Maximises a window by handle
        /// </summary>
        /// <param name="hwnd">Window handle</param>
        static public void MinimizeWindow(IntPtr hwnd)
        {
            ShowWindow(hwnd, SW_SHOWMINIMIZED);
        }


        /// <summary>
        /// Sets the child parent forrelationship uusing API
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public static void SetParentWindow(System.Windows.Forms.IWin32Window parent, System.Windows.Forms.IWin32Window child)
        {
            try
            {
                if (SetParentWindow(child.Handle, parent.Handle) == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SetParentWindow failed: {0}.\nParent = '{1}', Child = '{2}'", ex.ToString(),
                    parent != null ? parent.Handle.ToString() : null,
                    child != null ? child.Handle.ToString() : null);
            }
        }

        /// <summary>
        /// Retrieves the specified window's owner window, if any.
        /// </summary>
        /// <param name="window"></param>
        /// <returns>Owner window</returns>
        public static System.Windows.Forms.IWin32Window GetOwnerWindow(System.Windows.Forms.IWin32Window window)
        {
            IntPtr owner = window != null ? GetWindow(window.Handle, GW_OWNER) : IntPtr.Zero;
            return owner != IntPtr.Zero ? new Win32Window { Handle = owner } : null;
        }

        /// <summary>
        /// Returns the small Icon from a passed window
        /// </summary>
        /// <param name="hWnd">Window Handle</param>
        /// <returns>Icon</returns>
        public static System.Drawing.Icon GetSmallWindowIcon(IntPtr hWnd)
        {
            try
            {
                int result;
                SendMessageTimeout(
                  hWnd,
                  WM_GETICON,
                  ICON_SMALL,
                  0,
                  SMTO_ABORTIFHUNG,
                  1000, out result);

                IntPtr hIcon = new IntPtr(result);
                if (hIcon == IntPtr.Zero)
                {
                    result = GetClassLong(hWnd, GCL_HICONSM);
                    hIcon = new IntPtr(result);
                }

                if (hIcon == IntPtr.Zero)
                {
                    SendMessageTimeout(
                        hWnd,
                        WM_QUERYDRAGICON,
                        0,
                        0,
                        SMTO_ABORTIFHUNG,
                        1000, out result);
                    hIcon = new IntPtr(result);
                }

                if (hIcon == IntPtr.Zero)
                    return null;
                else
                    return System.Drawing.Icon.FromHandle(hIcon);
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region Menu Functions

        /// <summary>
        /// disables the close button on a passed form
        /// </summary>
        /// <param name="hWnd"></param>
        static public void DisableCloseMenu(IntPtr hWnd)
        {
            //int hMenu;
            IntPtr hMenu;
            int menuItemCount;

            //Obtain the handle to the form's system menu
            hMenu = GetSystemMenu(hWnd, false);

            // Get the count of the items in the system menu
            menuItemCount = GetMenuItemCount(hMenu);

            for (uint pos = 0; pos < menuItemCount; pos++)
            {
                // create the string to use for return value
                System.Text.StringBuilder s = new System.Text.StringBuilder(128);

                //first get by position
                int ret = GetMenuString(hMenu, pos, s, 127, MF_BYPOSITION);

                //try by command
                if (ret == 0)
                {
                    ret = GetMenuString(hMenu, pos, s, 127, MF_BYCOMMAND);
                }
                //test if the word close exists within the string
                string str = s.ToString().ToUpper();
                int iLen = str.IndexOf("CLOSE");
                if (iLen > -1)
                {
                    // Remove the close menuitem
                    RemoveMenu(hMenu, pos, MF_DISABLED | MF_BYPOSITION);
                    // Remove the Separator 
                    RemoveMenu(hMenu, pos - 1, MF_DISABLED | MF_BYPOSITION);
                    // redraw the menu bar
                    DrawMenuBar(hWnd);
                    return;
                }
            }

        }

        /// <summary>
        /// removes the file menu passed forms menu typically the file menu
        /// </summary>
        /// <param name="hWnd">Handle to the application</param>
        /// <param name="menu"></param>
        static public void RemoveMenuItem(IntPtr hWnd, string menu)
        {
            IntPtr hMenu;
            int menuItemCount;

            // get the menu
            hMenu = GetMenu(hWnd);

            // get toal number of items in the menu
            menuItemCount = (int)GetMenuItemCount(hMenu);

            for (uint pos = 0; pos < menuItemCount; pos++)
            {
                // create the string to use for return value
                System.Text.StringBuilder s = new System.Text.StringBuilder(128);

                //first get by position
                int ret = GetMenuString(hMenu, pos, s, 127, MF_BYPOSITION);

                //try by command
                if (ret == 0)
                {
                    ret = GetMenuString(hMenu, pos, s, 127, MF_BYCOMMAND);
                }

                //test if the word close exists within the string
                string str = s.ToString().ToUpper();
                int iLen = str.IndexOf(menu.ToUpper());
                if (iLen > -1)
                {
                    //remove menu 
                    RemoveMenu(hMenu, pos, MF_DISABLED | MF_BYPOSITION);

                    break;
                }
            }
            //redraw the menu
            DrawMenuBar(hWnd);
        }



        /// <summary>
        /// Special function to target the close sub menu item that sometime exists
        /// on an applications main menu bar 
        /// </summary>
        /// <param name="hWnd">application handle</param>
        /// <param name="mainmenu"></param>
        /// <param name="submenu"></param>
        static public void RemoveMenuItem(IntPtr hWnd, string mainmenu, string submenu)
        {
            IntPtr hMenu;
            IntPtr hSubMenu;
            int menuItemCount;
            int submenuItemCount;

            // get the menu
            hMenu = GetMenu(hWnd);

            // get toal number of items in the menu
            menuItemCount = GetMenuItemCount(hMenu);

            for (uint pos = 0; pos < menuItemCount; pos++)
            {
                // create the string to use for return value
                System.Text.StringBuilder s = new System.Text.StringBuilder(128);

                //first get by position
                int ret = GetMenuString(hMenu, pos, s, 127, MF_BYPOSITION);

                //try by command
                if (ret == 0)
                {
                    ret = GetMenuString(hMenu, pos, s, 127, MF_BYCOMMAND);
                }

                string strmenu = s.ToString().ToUpper();
                int imenuLen = strmenu.IndexOf(mainmenu.ToUpper());

                //should be the top level we are looking for
                if (imenuLen > -1)
                {
                    //get handle to this sub menu
                    int ipos = (int)pos;
                    hSubMenu = GetSubMenu(hMenu, ipos);

                    //get number of items in this menu
                    submenuItemCount = (int)GetMenuItemCount(hSubMenu);

                    //loop around all these sub items
                    for (uint i = 0; i < submenuItemCount; i++)
                    {
                        System.Text.StringBuilder sz = new System.Text.StringBuilder(128);

                        //first get by position
                        int val = GetMenuString(hSubMenu, i, sz, 127, MF_BYPOSITION);

                        //try by command
                        if (val == 0)
                        {
                            val = GetMenuString(hSubMenu, i, sz, 127, MF_BYCOMMAND);
                        }

                        //test if the word close exists within the string
                        string str = sz.ToString().ToUpper();
                        int iLen = str.IndexOf(submenu.ToUpper());
                        if (iLen > -1)
                        {
                            RemoveMenu(hSubMenu, i, MF_DISABLED | MF_BYPOSITION);
                            //redraw the menu
                            DrawMenuBar(hWnd);

                            return;
                        }
                    }
                }
            }

        }


        /// <summary>
        /// Remove the spacer lines in menus at specified occurance
        /// </summary>
        /// <param name="hWnd">Window containing menu</param>
        /// <param name="mainmenu">name of the menu item to target</param>
        /// <param name="occurance">which item to remove</param>
        static public void RemoveMenuSpacer(IntPtr hWnd, string mainmenu, int occurance)
        {
            IntPtr hMenu;
            IntPtr hSubMenu;
            int menuItemCount;
            int submenuItemCount;
            int itemcount = 0;

            // get the menu
            hMenu = GetMenu(hWnd);

            // get toal number of items in the menu
            menuItemCount = GetMenuItemCount(hMenu);

            for (uint pos = 0; pos < menuItemCount; pos++)
            {
                // create the string to use for return value
                System.Text.StringBuilder s = new System.Text.StringBuilder(128);

                //first get by position
                int ret = GetMenuString(hMenu, pos, s, 127, MF_BYPOSITION);

                //try by command
                if (ret == 0)
                {
                    ret = GetMenuString(hMenu, pos, s, 127, MF_BYCOMMAND);
                }

                string strmenu = s.ToString().ToUpper();
                int imenuLen = strmenu.IndexOf(mainmenu.ToUpper());

                //should be the top level we are looking for
                if (imenuLen > -1)
                {
                    //get handle to this sub menu
                    int ipos = (int)pos;
                    hSubMenu = GetSubMenu(hMenu, ipos);

                    //get number of items in this menu
                    submenuItemCount = (int)GetMenuItemCount(hSubMenu);

                    //loop around all these sub items
                    for (uint i = 0; i < submenuItemCount; i++)
                    {
                        System.Text.StringBuilder sz = new System.Text.StringBuilder(128);

                        //first get by position
                        int val = GetMenuString(hSubMenu, i, sz, 127, MF_BYPOSITION);

                        //try by command
                        if (val == 0)
                        {
                            val = GetMenuString(hSubMenu, i, sz, 127, MF_BYCOMMAND);
                        }

                        //test if the word close exists within the string
                        string str = sz.ToString().ToUpper();

                        //test if it is a spacer
                        if (str == "")
                        {
                            itemcount++;

                            if (itemcount == occurance)
                            {
                                // remove spacer bar
                                RemoveMenu(hSubMenu, i, MF_DISABLED | MF_BYPOSITION);

                                //redraw the menu
                                DrawMenuBar(hWnd);

                                return;
                            }
                        }
                    }
                }
            }

        }






        /// <summary>
        /// Removes a menu item by position
        /// </summary>
        /// <param name="hWnd">Application handle</param>
        /// <param name="pos">position within the menu</param>
        static public void RemoveMenuItem(IntPtr hWnd, uint pos)
        {
            IntPtr hMenu;
            int menuItemCount;

            // get the menu
            hMenu = GetMenu(hWnd);

            menuItemCount = GetMenuItemCount(hMenu);

            // Remove the close menuitem
            RemoveMenu(hMenu, pos, MF_DISABLED | MF_BYPOSITION);

            DrawMenuBar(hWnd);
        }

        /// <summary>
        /// Useful for removing the Horizontal bars when menu items have been removed
        /// </summary>
        /// <param name="hWnd">Handle to application</param>
        /// <param name="mainmenu">Main Menu name</param>
        static public void RemoveLastMenuItem(IntPtr hWnd, string mainmenu)
        {
            IntPtr hMenu;
            IntPtr hSubMenu;
            int menuItemCount;
            uint submenuItemCount;

            // get the menu
            hMenu = GetMenu(hWnd);

            // get toal number of items in the menu
            menuItemCount = GetMenuItemCount(hMenu);

            for (uint pos = 0; pos < menuItemCount; pos++)
            {
                // create the string to use for return value
                System.Text.StringBuilder s = new System.Text.StringBuilder(128);

                //first get by position
                int ret = GetMenuString(hMenu, pos, s, 127, MF_BYPOSITION);

                //try by command
                if (ret == 0)
                {
                    ret = GetMenuString(hMenu, pos, s, 127, MF_BYCOMMAND);
                }

                string strmenu = s.ToString().ToUpper();
                int imenuLen = strmenu.IndexOf(mainmenu.ToUpper());

                //should be the top level we are looking for
                if (imenuLen > -1)
                {
                    //get handle to this sub menu
                    int ipos = (int)pos;
                    hSubMenu = GetSubMenu(hMenu, ipos);

                    //get number of items in this menu
                    submenuItemCount = (uint)GetMenuItemCount(hSubMenu);

                    RemoveMenu(hSubMenu, submenuItemCount - 1, MF_DISABLED | MF_BYPOSITION);
                    DrawMenuBar(hWnd);

                }
            }
        }


        static public int AddMenuItem(IntPtr hWnd, string mainmenu, string mnuText)
        {

            IntPtr hMenu;
            IntPtr hSubMenu;
            uint menuID = 0;
            bool retval;
            int menuItemCount;

            // get the menu
            hMenu = GetMenu(hWnd);

            // get toal number of items in the menu use this as ID of menu item as items are 0 based
            menuItemCount = GetMenuItemCount(hMenu);

            for (uint pos = 0; pos < menuItemCount; pos++)
            {
                // create the string to use for return value
                System.Text.StringBuilder s = new System.Text.StringBuilder(128);

                //first get by position
                int ret = GetMenuString(hMenu, pos, s, 127, MF_BYPOSITION);

                //try by command
                if (ret == 0)
                {
                    ret = GetMenuString(hMenu, pos, s, 127, MF_BYCOMMAND);
                }

                string strmenu = s.ToString().ToUpper();
                int imenuLen = strmenu.IndexOf(mainmenu.ToUpper());

                //should be the top level we are looking for
                if (imenuLen > -1)
                {
                    //get handle to this sub menu
                    int ipos = (int)pos;
                    hSubMenu = GetSubMenu(hMenu, ipos);

                    //get number of items in this menu
                    menuID = (uint)GetMenuItemCount(hSubMenu);

                    retval = AppendMenu(hSubMenu, MenuFlags.MF_STRING, menuID, mnuText);

                    if (retval == true)
                    {
                        DrawMenuBar(hWnd);
                        return (int)menuID;
                    }

                }

            }
            return 0;
        }

        #endregion

        #region Process Functions

        /// <summary>
        /// Brutal way to kill a process
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        static public void KillProcess(IntPtr hWnd)
        {

            uint processID = 0;
            uint retval = GetWindowThreadProcessId(hWnd, out processID);
            IntPtr proc = OpenProcess(PROCESS_ALL_ACCESS, false, processID);

            try
            {
                TerminateProcess(proc, 0);
            }
            catch (Exception e) { string s = e.Message; }
        }

        /// <summary>
        /// Attempt to close an app with the safer PostMesage function
        /// </summary>
        /// <param name="hWnd"></param>
        static public void CloseProcess(IntPtr hWnd)
        {
            PostMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }




        #endregion

    }
}
