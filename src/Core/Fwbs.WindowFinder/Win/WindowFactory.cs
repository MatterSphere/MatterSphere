using System;
using System.Collections.Generic;

namespace Fwbs
{
    namespace WinFinder
    {
        public static class WindowFactory
        {
            private static Dictionary<IntPtr, Window> windows = new Dictionary<IntPtr, Window>();

            private static Window CreateWindow(IntPtr handle)
            {
                lock (windows)
                {
                    Window win = null;

                    if (windows.ContainsKey(handle))
                    {
                        win = windows[handle];
                        if (!win.IsValid)
                        {
                            windows.Remove(handle);
                            return null;
                        }
                    }

                    if (win == null || !win.IsValid)
                    {
                        win = new Window(handle);
                        windows.Add(handle, win);
                    }

                    return win;
                }
            }

            public static Window GetWindowFromMouse()
            {
                return GetWindowFromPoint(System.Windows.Forms.Control.MousePosition);
            }

            public static Window GetWindowFromPoint(System.Drawing.Point point)
            {
                IntPtr handle = NativeMethods.WindowFromPoint(point);
                if (handle == IntPtr.Zero)
                    return null;
                else
                    return WindowFactory.CreateWindow(handle);
            }

            public static Window GetWindow(IntPtr handle)
            {
                if (handle == IntPtr.Zero)
                    return null;
                else
                    return WindowFactory.CreateWindow(handle);
            }
            public static Window ShowWindowPicker(System.Windows.Forms.IWin32Window owner)
            {
                using (Internal.WindowPicker picker = new Internal.WindowPicker())
                {
                    if (picker.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                        return picker.SelectedWindow;
                    else
                        return null;
                }
            }

            public static int RegisterMessage(string name)
            {
                return NativeMethods.RegisterWindowMessage(name);
            }
        }
    }
}