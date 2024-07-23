using System;
using System.Runtime.InteropServices;

namespace Fwbs.Oms.Office.Outlook
{
    internal static class OleWindow
    {
        /// <summary>
        /// Implemented and used by containers and objects to obtain window handles 
        /// and manage context-sensitive help.
        /// </summary>
        /// <remarks>
        /// The IOleWindow interface provides methods that allow an application to obtain  
        /// the handle to the various windows that participate in in-place activation, 
        /// and also to enter and exit context-sensitive help mode.
        /// </remarks>
        [ComImport, Guid("00000114-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IOleWindow
        {
            /// <summary>
            /// Returns the window handle to one of the windows participating in in-place activation 
            /// (frame, document, parent, or in-place object window).
            /// </summary>
            /// <param name="phwnd">Pointer to where to return the window handle.</param>
            void GetWindow(out IntPtr phWnd);

            /// <summary>
            /// Determines whether context-sensitive help mode should be entered during an 
            /// in-place activation session.
            /// </summary>
            /// <param name="fEnterMode"><c>true</c> if help mode should be entered; 
            /// <c>false</c> if it should be exited.</param>
            void ContextSensitiveHelp([In, MarshalAs(UnmanagedType.Bool)] bool fEnterMode);
        }

        public static IntPtr GetWindowHandle(this Microsoft.Office.Interop.Outlook.Explorer explorer)
        {
            IntPtr hWnd = IntPtr.Zero;
            ((IOleWindow)explorer).GetWindow(out hWnd);
            return hWnd;
        }

        public static IntPtr GetWindowHandle(this Microsoft.Office.Interop.Outlook.Inspector inspector)
        {
            IntPtr hWnd = IntPtr.Zero;
            ((IOleWindow)inspector).GetWindow(out hWnd);
            return hWnd;
        }
    }
}
