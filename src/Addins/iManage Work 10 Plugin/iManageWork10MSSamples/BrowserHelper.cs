using System;
using System.Runtime.InteropServices;

namespace iManageWork10MSSamples
{
    public class BrowserHelper
    {
        [DllImport("wininet.dll")]
        static extern InternetCookieState InternetSetCookieEx(
            string lpszURL,
            string lpszCookieName,
            string lpszCookieData,
            int dwFlags,
            IntPtr dwReserved);

        const int INTERNET_COOKIE_HTTPONLY = 8192;
        
        enum InternetCookieState
        {
            COOKIE_STATE_UNKNOWN = 0x0,
            COOKIE_STATE_ACCEPT = 0x1,
            COOKIE_STATE_PROMPT = 0x2,
            COOKIE_STATE_LEASH = 0x3,
            COOKIE_STATE_DOWNGRADE = 0x4,
            COOKIE_STATE_REJECT = 0x5,
            COOKIE_STATE_MAX = COOKIE_STATE_REJECT
        }
        
        public void SetCookie(string baseUrl, string cookieName, string data)
        {
            InternetSetCookieEx(baseUrl, cookieName, data, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero);
        }

    }
}