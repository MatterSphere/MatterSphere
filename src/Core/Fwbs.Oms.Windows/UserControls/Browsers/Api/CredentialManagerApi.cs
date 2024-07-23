using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace FWBS.OMS.UI.UserControls.Browsers.Api
{
    public static class CredentialManagerApi
    {
        [DllImport("Advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredRead(string target, CREDENTIALS_TYPE type, int reservedFlag, out IntPtr CredentialPtr);

        [DllImport("Advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredWrite([In] ref NativeCredential userCredential, [In] UInt32 flags);

        [DllImport("Advapi32.dll", EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredDelete(string target, CREDENTIALS_TYPE type, int reservedFlag);

        [DllImport("Advapi32.dll", EntryPoint = "CredFree", SetLastError = true)]
        private static extern bool CredFree([In] IntPtr cred);

        private const int ERROR_NOT_FOUND = 1168;

        private enum CREDENTIALS_TYPE : uint
        {
            GENERIC = 1,
            DOMAIN_PASSWORD = 2,
            DOMAIN_CERTIFICATE = 3,
            DOMAIN_VISIBLE_PASSWORD = 4,
            GENERIC_CERTIFICATE = 5,
            DOMAIN_EXTENDED = 6,
            MAXIMUM = 7,
            MAXIMUM_EX = (MAXIMUM + 1000)
        }

        private enum CREDENTIALS_PERSIST : uint
        {
            SESSION = 1,
            LOCAL_MACHINE = 2,
            ENTERPRISE = 3
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct NativeCredential : IDisposable
        {
            public UInt32 Flags;
            public CREDENTIALS_TYPE Type;
            public IntPtr TargetName;
            public IntPtr Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public UInt32 CredentialBlobSize;
            public IntPtr CredentialBlob;
            public UInt32 Persist;
            public UInt32 AttributeCount;
            public IntPtr Attributes;
            public IntPtr TargetAlias;
            public IntPtr UserName;

            internal static NativeCredential GetNativeCredential(Credential cred)
            {
                return new NativeCredential
                {
                    AttributeCount = 0,
                    Attributes = IntPtr.Zero,
                    Comment = IntPtr.Zero,
                    TargetAlias = IntPtr.Zero,
                    Type = (CREDENTIALS_TYPE) cred.Type,
                    Persist = (UInt32) cred.Persist,
                    CredentialBlobSize = (UInt32) cred.CredentialBlobSize,
                    TargetName = Marshal.StringToCoTaskMemUni(cred.TargetName),
                    CredentialBlob = Marshal.StringToCoTaskMemUni(cred.CredentialBlob),
                    UserName = Marshal.StringToCoTaskMemUni(cred.UserName)
                };
            }

            void IDisposable.Dispose()
            {
                Marshal.FreeCoTaskMem(TargetName); TargetName = IntPtr.Zero;
                Marshal.FreeCoTaskMem(CredentialBlob); CredentialBlob = IntPtr.Zero;
                Marshal.FreeCoTaskMem(UserName); UserName = IntPtr.Zero;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct Credential
        {
            public UInt32 Flags;
            public CREDENTIALS_TYPE Type;
            public string TargetName;
            public string Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public UInt32 CredentialBlobSize;
            public string CredentialBlob;
            public CREDENTIALS_PERSIST Persist;
            public UInt32 AttributeCount;
            public IntPtr Attributes;
            public string TargetAlias;
            public string UserName;
        }

        private sealed class CriticalCredentialHandle : CriticalHandleZeroOrMinusOneIsInvalid
        {
            internal CriticalCredentialHandle(IntPtr preexistingHandle)
            {
                SetHandle(preexistingHandle);
            }

            internal Credential GetCredential()
            {
                Credential cred = new Credential();
                if (!IsInvalid)
                {
                    NativeCredential ncred = (NativeCredential)Marshal.PtrToStructure(handle, typeof(NativeCredential));
                    
                    cred.CredentialBlobSize = ncred.CredentialBlobSize;
                    cred.CredentialBlob = Marshal.PtrToStringUni(ncred.CredentialBlob, (int)cred.CredentialBlobSize / 2);
                    cred.UserName = Marshal.PtrToStringUni(ncred.UserName);
                    cred.TargetName = Marshal.PtrToStringUni(ncred.TargetName);
                    cred.TargetAlias = Marshal.PtrToStringUni(ncred.TargetAlias);
                    cred.Type = ncred.Type;
                    cred.Flags = ncred.Flags;
                    cred.Persist = (CREDENTIALS_PERSIST)ncred.Persist;                    
                }
                return cred;
            }

            protected override bool ReleaseHandle()
            {
                if (!IsInvalid)
                {
                    CredFree(handle);
                    SetHandleAsInvalid();
                    return true;
                }
                return false;
            }
        }

        public static void WriteCredentials(string key, string user, string secret)
        {
            var credential = new Credential
            {
                TargetName = key,
                UserName = user,
                CredentialBlob = secret,
                CredentialBlobSize = (UInt32) Encoding.Unicode.GetBytes(secret).Length,
                AttributeCount = 0,
                Attributes = IntPtr.Zero,
                Comment = null,
                TargetAlias = null,
                Type = CREDENTIALS_TYPE.GENERIC,
                Persist = CREDENTIALS_PERSIST.ENTERPRISE
            };
            var nativeCredential = NativeCredential.GetNativeCredential(credential);
            bool written = CredWrite(ref nativeCredential, 0);
            (nativeCredential as IDisposable).Dispose();
            if (!written)
            {
                int lastError = Marshal.GetLastWin32Error();
                string message = string.Format("CredWrite failed with the error code = {0}.", lastError);
                throw new Win32Exception(lastError, message);
            }
        }

        public static void ReadCredentials(string key, out string user, out string password)
        {
            user = null;
            password = null;
            IntPtr nativeCredPtr;       
   
            if (CredRead(key, CREDENTIALS_TYPE.GENERIC, 0, out nativeCredPtr))
            {
                using (var criticalCredentialHandle = new CriticalCredentialHandle(nativeCredPtr))
                {
                    var credential = criticalCredentialHandle.GetCredential();
                    user = credential.UserName;
                    password = credential.CredentialBlob;
                }
            }
            else
            {
                int lastError = Marshal.GetLastWin32Error();
                if (lastError != ERROR_NOT_FOUND)
                {
                    var message = string.Format("CredRead failed with the error code = {0}.", lastError);
                    throw new Win32Exception(lastError, message);
                }
            }
        }

        public static void DeleteCredentials(string key)
        {
            if (!CredDelete(key, CREDENTIALS_TYPE.GENERIC, 0))
            {
                int lastError = Marshal.GetLastWin32Error();
                if (lastError != ERROR_NOT_FOUND)
                {
                    var message = string.Format("CredDelete failed with the error code = {0}.", lastError);
                    throw new Win32Exception(lastError, message);
                }
            }
        }
    }
}
