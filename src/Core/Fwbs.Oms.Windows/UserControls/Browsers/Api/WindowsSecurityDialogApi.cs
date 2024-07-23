using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace FWBS.OMS.UI.UserControls.Browsers.Api
{
    public class WindowsSecurityDialogApi
    {
        public WindowsSecurityDialogApi(string credentialsKey,
            string captionText,
            string messageText,
            IntPtr parentWnd)
        {
            this.CredentialsKey = credentialsKey;
            this.CaptionText = captionText;
            this.MessageText = messageText;
            this.ParentWnd = parentWnd;
        }

        public string CredentialsKey { get; private set; }
        public string CaptionText { get; private set; }
        public string MessageText { get; private  set; }
        public IntPtr ParentWnd { get; private set; }
        public NetworkCredential Credentials { get; private set; }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CREDENTIALUI_INFO
        {
            public int cbSize;
            public IntPtr hwndParent;
            public string pszMessageText;
            public string pszCaptionText;
            public IntPtr hbmBanner;
        }

        [Flags]
        private enum CREDUIWIN : uint
        {
            GENERIC = 0x1,
            CHECKBOX = 0x2,
            AUTHPACKAGE_ONLY = 0x10,
            IN_CRED_ONLY = 0x20,
            ENUMERATE_ADMINS = 0x100,
            ENUMERATE_CURRENT_USER = 0x200,
            SECURE_PROMPT = 0x1000,
            PREPROMPTING = 0x2000,
            PACK_32_WOW = 0x10000000
        }

        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredUnPackAuthenticationBuffer(int dwFlags,
                                                                   IntPtr pAuthBuffer,
                                                                   uint cbAuthBuffer,
                                                                   StringBuilder pszUserName,
                                                                   ref int pcchMaxUserName,
                                                                   StringBuilder pszDomainName,
                                                                   ref int pcchMaxDomainame,
                                                                   StringBuilder pszPassword,
                                                                   ref int pcchMaxPassword);

        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int CredUIPromptForWindowsCredentials(ref CREDENTIALUI_INFO credentialuiInfo,
                                                                    int authError,
                                                                    ref uint authPackage,
                                                                    IntPtr InAuthBuffer,
                                                                    uint InAuthBufferSize,
                                                                    out IntPtr refOutAuthBuffer,
                                                                    out uint refOutAuthBufferSize,
                                                                    ref bool fSave,
                                                                    uint flags);

        public bool ValidateCredentials(out bool authenticationUsedCredManager)
        {
            authenticationUsedCredManager = false;
            {
                string user;
                string password;
                CredentialManagerApi.ReadCredentials(CredentialsKey, out user, out password);
                if (user != null && password != null)
                {
                    this.Credentials = new NetworkCredential()
                    {
                        UserName = user,
                        Password = password,
                        Domain = string.Empty
                    };
                    authenticationUsedCredManager = true;
                    return true;
                }
            }

            var credentialuiInfo = new CREDENTIALUI_INFO
            {
                cbSize = Marshal.SizeOf<CREDENTIALUI_INFO>(),
                hwndParent = ParentWnd,
                pszMessageText = MessageText,
                pszCaptionText = CaptionText,
                hbmBanner = IntPtr.Zero
            };

            int authError = 0;
            uint authPackage = 0;
            IntPtr outAuthBuffer;
            uint outAuthBufferSize;
            bool needSave = false;

            int result = CredUIPromptForWindowsCredentials(ref credentialuiInfo,
                                                               authError,
                                                           ref authPackage,
                                                               IntPtr.Zero,
                                                               0,
                                                           out outAuthBuffer,
                                                           out outAuthBufferSize,
                                                           ref needSave,
                                                           (uint)(CREDUIWIN.GENERIC | CREDUIWIN.CHECKBOX));
        
            if (result == 0)
            {
                int pcchMaxUserName = 512;
                int pcchMaxPassword = 512;
                int pcchMaxDomainame = 512;

                var userName = new StringBuilder(pcchMaxUserName);
                var password = new StringBuilder(pcchMaxPassword);
                var domainName = new StringBuilder(pcchMaxDomainame);

                if (CredUnPackAuthenticationBuffer(
                        0, outAuthBuffer, outAuthBufferSize, 
                        userName, ref pcchMaxUserName,
                        domainName, ref pcchMaxDomainame, 
                        password, ref pcchMaxPassword))
                {
                    Marshal.FreeCoTaskMem(outAuthBuffer);
                    this.Credentials = new NetworkCredential()
                    {
                        UserName = userName.ToString(),
                        Password = password.ToString(),
                        Domain = domainName.ToString()
                    };

                    if (needSave)
                    {
                        CredentialManagerApi.WriteCredentials(CredentialsKey, userName.ToString(), password.ToString());
                    }

                    return true;
                }
            }          
            return false;
        }
    }
}
