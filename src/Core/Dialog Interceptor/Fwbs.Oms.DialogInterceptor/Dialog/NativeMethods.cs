using System;

namespace Fwbs.Oms.DialogInterceptor
{
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        public static string PrimaryHookDLL { get { return Environment.Is64BitProcess ? Hook64DLL : Hook32DLL; } }
        public static string SecondaryHookDLL { get { return Environment.Is64BitProcess ? Hook32DLL : Hook64DLL; } }
        private const string Hook32DLL = "OMS.DialogInterceptor.Hook.dll";
        private const string Hook64DLL = "OMS.DialogInterceptor.Hook64.dll";
        private const string UserDLL = @"User32.dll";
        private const string KernelDLL = @"Kernel32.dll";

        [DllImport(UserDLL, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern uint RegisterWindowMessage(string name);
        [DllImport(KernelDLL, SetLastError = true, CharSet = CharSet.Unicode)]
        internal extern static WinFinder.SafeLibraryHandle LoadLibrary(string librayName);
        [DllImport(UserDLL, CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendDlgItemMessage(IntPtr handle, int dlgid, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport(KernelDLL, SetLastError = true)]
        internal static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        [DllImport(KernelDLL, SetLastError = true)]
        internal static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        private static WinFinder.UnmanagedLibrary UserLib;

        internal static bool ChangeWindowMessageFilter(uint msg, int flags)
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                if (UserLib == null)
                    UserLib = new WinFinder.UnmanagedLibrary(UserDLL);

                return UserLib.GetUnmanagedFunction<ChangeWindowMessageFilterDelegate>(
                    System.Reflection.MethodInfo.GetCurrentMethod().Name).Invoke(
                    msg, flags);
            }
            else
                return true;
        }


        [return: MarshalAs(UnmanagedType.Bool)]
        private delegate bool ChangeWindowMessageFilterDelegate(uint msg, int flags);

        public const int MSGFLT_ADD = 1;
        public const int MSGFLT_REMOVE = 2;
    }
}
