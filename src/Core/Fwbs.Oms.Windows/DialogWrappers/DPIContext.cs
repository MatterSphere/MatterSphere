using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FWBS.OMS.UI.Windows
{
    public struct DPI_AWARENESS_CONTEXT
    {
        private IntPtr value;

        private DPI_AWARENESS_CONTEXT(IntPtr value)
        {
            this.value = value;
        }

        public static implicit operator DPI_AWARENESS_CONTEXT(IntPtr value)
        {
            return new DPI_AWARENESS_CONTEXT(value);
        }

        public static implicit operator IntPtr(DPI_AWARENESS_CONTEXT context)
        {
            return context.value;
        }

        public static bool operator ==(IntPtr context1, DPI_AWARENESS_CONTEXT context2)
        {
            return AreDpiAwarenessContextsEqual(context1, context2);
        }

        public static bool operator !=(IntPtr context1, DPI_AWARENESS_CONTEXT context2)
        {
            return !AreDpiAwarenessContextsEqual(context1, context2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [DllImport("User32.dll")] // Minimum supported client: Windows 10 version 1607
        private static extern bool AreDpiAwarenessContextsEqual(DPI_AWARENESS_CONTEXT dpiContextA, DPI_AWARENESS_CONTEXT dpiContextB);

        public static readonly DPI_AWARENESS_CONTEXT DPI_AWARENESS_CONTEXT_INVALID = IntPtr.Zero;
        public static readonly DPI_AWARENESS_CONTEXT DPI_AWARENESS_CONTEXT_UNAWARE = new IntPtr(-1);
        public static readonly DPI_AWARENESS_CONTEXT DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = new IntPtr(-2);
        public static readonly DPI_AWARENESS_CONTEXT DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = new IntPtr(-3);
        public static readonly DPI_AWARENESS_CONTEXT DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = new IntPtr(-4);
        public static readonly DPI_AWARENESS_CONTEXT DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED = new IntPtr(-5); // Windows 10 (1809)
    }

    public class DPIContextBlock : IDisposable
    {
        private DPI_AWARENESS_CONTEXT resetContext;
        private bool disposed = false;

        public DPIContextBlock(DPIAwareness.DPI_AWARENESS awareness) :
            this(awareness == DPIAwareness.DPI_AWARENESS.PER_MONITOR_AWARE ? DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 :
                 awareness == DPIAwareness.DPI_AWARENESS.SYSTEM_AWARE ? DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_SYSTEM_AWARE :
                 awareness == DPIAwareness.DPI_AWARENESS.UNAWARE ? DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_UNAWARE :
                 DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_INVALID)
        {
        }

        public DPIContextBlock(System.Windows.Forms.IWin32Window window) :
            this(GetWindowDpiAwarenessContext(window.Handle))
        {
        }

        public DPIContextBlock(DPI_AWARENESS_CONTEXT contextSwitchTo)
        {
            resetContext = SetThreadDpiAwarenessContext(contextSwitchTo);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    SetThreadDpiAwarenessContext(resetContext);
                }
            }
            disposed = true;
        }

        [DllImport("User32.dll")] // Minimum supported client: Windows 10 version 1607
        private static extern DPI_AWARENESS_CONTEXT GetWindowDpiAwarenessContext(IntPtr hwnd);

        [DllImport("User32.dll")] // Minimum supported client: Windows 10 version 1607
        private static extern DPI_AWARENESS_CONTEXT SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT dpiContext);
    }

    public static class DPIAwareness
    {
        private static readonly Version OSVersion = Environment.OSVersion.Version;

        public enum DPI_AWARENESS
        {
            INVALID = -1,
            UNAWARE = 0,
            SYSTEM_AWARE = 1,
            PER_MONITOR_AWARE = 2
        }

        static DPIAwareness()
        {
            var osVersionInfo = new OSVERSIONINFOEX { OSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX)) };
            if (RtlGetVersion(ref osVersionInfo) == 0)
            {
                OSVersion = new Version(osVersionInfo.MajorVersion, osVersionInfo.MinorVersion, osVersionInfo.BuildNumber);
            }
        }

        public static bool IsSupported
        {
            get { return OSVersion >= new Version(10, 0, 14393); } // Windows 10 (1607)
        }

        public static DPI_AWARENESS FromWindow(IntPtr hWnd)
        {
            DPI_AWARENESS value = DPI_AWARENESS.INVALID;
            if (IsSupported)
            {
                value = GetAwarenessFromDpiAwarenessContext(GetWindowDpiAwarenessContext(hWnd));
                System.Diagnostics.Debug.WriteLine("DPI awareness from window {0}: {1}.", hWnd, value);
            }
            return value;
        }

        public static DPI_AWARENESS FromCurrentThread()
        {
            DPI_AWARENESS value = DPI_AWARENESS.INVALID;
            if (IsSupported)
            {
                value = GetAwarenessFromDpiAwarenessContext(GetThreadDpiAwarenessContext());
#pragma warning disable 0618
                System.Diagnostics.Debug.WriteLine("DPI awareness from thread {0}: {1}.", AppDomain.GetCurrentThreadId(), value);
#pragma warning restore 0618
            }
            return value;
        }

        public static DPI_AWARENESS FromProcess(IntPtr hProcess = default(IntPtr))
        {
            DPI_AWARENESS value = DPI_AWARENESS.INVALID;
            if (OSVersion >= new Version(6, 3)) // Windows 8.1
            {
                GetProcessDpiAwareness(hProcess, out value);
                System.Diagnostics.Debug.WriteLine("DPI awareness from process {0}: {1}.", hProcess, value);
            }
            return value;
        }

        [DllImport("User32.dll")] // Minimum supported client: Windows 10 version 1607
        private static extern DPI_AWARENESS_CONTEXT GetWindowDpiAwarenessContext(IntPtr hwnd);

        [DllImport("User32.dll")] // Minimum supported client: Windows 10 version 1607
        private static extern DPI_AWARENESS_CONTEXT GetThreadDpiAwarenessContext();

        [DllImport("User32.dll")] // Minimum supported client: Windows 10 version 1607
        private static extern DPI_AWARENESS GetAwarenessFromDpiAwarenessContext(DPI_AWARENESS_CONTEXT value);

        [DllImport("Shcore.dll")] // Minimum supported client: Windows 8.1
        private static extern int GetProcessDpiAwareness(IntPtr hProcess, out DPI_AWARENESS value);

        [DllImport("Ntdll.dll", ExactSpelling = true)]
        private static extern int RtlGetVersion(ref OSVERSIONINFOEX versionInfo);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct OSVERSIONINFOEX
        {
            internal int OSVersionInfoSize; // Must be set to Marshal.SizeOf(typeof(OSVERSIONINFOEX))
            internal int MajorVersion;
            internal int MinorVersion;
            internal int BuildNumber;
            internal int PlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            internal string CSDVersion;
            internal ushort ServicePackMajor;
            internal ushort ServicePackMinor;
            internal short SuiteMask;
            internal byte ProductType;
            internal byte Reserved;
        }
    }

    public static class ScreenExtensions
    {
        private static readonly FieldInfo _hmonitor = typeof(System.Windows.Forms.Screen).GetField("hmonitor", BindingFlags.Instance | BindingFlags.NonPublic);

        public static IntPtr GetMonitorHandle(this System.Windows.Forms.Screen screen)
        {
            return (IntPtr)_hmonitor.GetValue(screen);
        }

        public static int GetScaleFactor(this System.Windows.Forms.Screen screen)
        {
            int scaleFactor = 100;
            if (DPIAwareness.IsSupported)
            {
                GetScaleFactorForMonitor(GetMonitorHandle(screen), out scaleFactor);
            }
            return scaleFactor;
        }

        [DllImport("Shcore.dll")] // Minimum supported client: Windows 8.1
        private static extern int GetScaleFactorForMonitor(IntPtr hMonitor, out int scaleFactor);
    }
}
