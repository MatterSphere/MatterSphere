using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FWBS.OMS.Drive
{
    /// <summary>
    /// This workaround is intented to refresh File Explorer tree view
    /// in order to get rid of phantom Drive folders after the drive has been unmounted.
    /// </summary>
    static class ExplorerHelper
    {
        public static void RefreshWindows()
        {
            StringBuilder sbClassName = new StringBuilder(256);
            List<IntPtr> explorerWindows = new List<IntPtr>();
            int sessionId = Process.GetCurrentProcess().SessionId;

            foreach (var thread in Process.GetProcessesByName("Explorer").Where(p => p.SessionId == sessionId).SelectMany(p => p.Threads.Cast<ProcessThread>()))
            {
                EnumThreadWindows(thread.Id, (hWnd, lParam) =>
                {
                    if (IsWindowVisible(hWnd) && GetClassName(hWnd, sbClassName, sbClassName.Capacity) > 0)
                    {
                        string className = sbClassName.ToString();
                        if (className == "CabinetWClass" || className == "ExplorerWClass")
                        {
                            explorerWindows.Add(hWnd);
                            return false;
                        }
                    }
                    return true;
                }, IntPtr.Zero);
            }

            foreach (IntPtr hWndExplorer in explorerWindows)
            {
                EnumChildWindows(hWndExplorer, (hWnd, lParam) =>
                {
                    if (GetClassName(hWnd, sbClassName, sbClassName.Capacity) > 0)
                    {
                        if (sbClassName.ToString() == "NamespaceTreeControl")
                        {
                            PostMessage(hWnd, 0x0111/*WM_COMMAND*/, (IntPtr)41504/*Refresh*/, IntPtr.Zero);
                            return false;
                        }
                    }
                    return true;
                }, IntPtr.Zero);
            }
        }

        #region Native Methods

        private delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern bool EnumThreadWindows(int dwThreadId, EnumWindowsDelegate lpfn, IntPtr lParam);

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsDelegate lpfn, IntPtr lParam);

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool PostMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        #endregion
    }
}
