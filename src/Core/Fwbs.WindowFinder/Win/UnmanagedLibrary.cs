using System;
using System.Runtime.InteropServices;

namespace Fwbs
{
    namespace WinFinder
    {
        public sealed class UnmanagedLibrary : IDisposable
        {
            public UnmanagedLibrary(string fileName)
            {
                m_hLibrary = NativeMethods.LoadLibrary(fileName);
                if (m_hLibrary.IsInvalid)
                {
                    int hr = Marshal.GetHRForLastWin32Error();
                    Marshal.ThrowExceptionForHR(hr);
                }
            }

            public TDelegate GetUnmanagedFunction<TDelegate>(string functionName) where TDelegate : class
            {
                IntPtr p = NativeMethods.GetProcAddress(m_hLibrary, functionName);
                // Failure is a common case, especially for adaptive code.            
                if (p == IntPtr.Zero)
                {
                    return null;
                }
                Delegate function = Marshal.GetDelegateForFunctionPointer(p, typeof(TDelegate));
                // Ideally, we'd just make the constraint on TDelegate be            
                // System.Delegate, but compiler error CS0702 (constrained can't be System.Delegate)            
                // prevents that. So we make the constraint system.object and do the cast from object-->TDelegate.            
                object o = function;

                return (TDelegate)o;
            }


            #region IDisposable Members

            public void Dispose()
            {
                if (!m_hLibrary.IsClosed)
                {
                    m_hLibrary.Close();
                }
            }

            private SafeLibraryHandle m_hLibrary;

            #endregion

        }
    }
}