using Microsoft.Win32.SafeHandles;

namespace Fwbs
{
    namespace WinFinder
    {
        [System.Security.SecurityCritical]
        public sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            private SafeLibraryHandle() : base(true) { }

            protected override bool ReleaseHandle()
            {
                return NativeMethods.FreeLibrary(handle);
            }
        }


    }
}