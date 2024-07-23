using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    class AxWebBrowser : WebBrowser
    {
        private AxWebBrowserSite _site;
        private AxHost.ConnectionPointCookie _cpCookie;
        private WebBrowser2EventHelper _eventHelper;

        private const int OLECMDEXECOPT_DONTPROMPTUSER = 2;
        private const int OLECMDID_OPTICAL_ZOOM = 63;

        public AxWebBrowser()
        {
        }

        public void Zoom(int zoom)
        {
            dynamic activeXInstance = ActiveXInstance;
            activeXInstance.ExecWB(OLECMDID_OPTICAL_ZOOM, OLECMDEXECOPT_DONTPROMPTUSER, zoom, zoom);
        }

        [Description("Specifies whether the WebBrowser control may download and run ActiveX controls.")]
        [Category("Behavior"), Browsable(true), DefaultValue(null)]
        public bool? AllowActiveX { get; set; }

        protected override WebBrowserSiteBase CreateWebBrowserSiteBase()
        {
            if (_site == null)
                _site = new AxWebBrowserSite(this);

            return _site;
        }

        protected override void CreateSink()
        {
            base.CreateSink();
            // Create an instance of the client that will handle the event and associate it with the underlying ActiveX control.
            _eventHelper = new WebBrowser2EventHelper(this);
            _cpCookie = new AxHost.ConnectionPointCookie(ActiveXInstance, _eventHelper, typeof(_DWebBrowserEvents2));
        }

        protected override void DetachSink()
        {
            // Disconnect the client that handles the event from the underlying ActiveX control.
            if (_cpCookie != null)
            {
                _cpCookie.Disconnect();
                _cpCookie = null;
            }
            base.DetachSink();
        }

        ///<summary>
        ///Fires when an error occurs during navigation.
        ///</summary>
        [Category("Action")]
        public event WebBrowserNavigateErrorEventHandler NavigateError;

        // Raises the NavigateError event.
        protected virtual void OnNavigateError(WebBrowserNavigateErrorEventArgs e)
        {
            NavigateError?.Invoke(this, e);
        }

        #region AxWebBrowserSite Class

        protected class AxWebBrowserSite : WebBrowserSite, IServiceProvider, IInternetSecurityManager
        {
            private const int S_OK = unchecked((int)0x00000000);
            private const int S_FALSE = unchecked((int)0x00000001);
            private const int E_NOINTERFACE = unchecked((int)0x80004002);
            private const int INET_E_DEFAULT_ACTION = unchecked((int)0x800C0011);

            private const int URLPOLICY_ALLOW = 0;
            private const int URLPOLICY_QUERY = 1;
            private const int URLPOLICY_DISALLOW = 3;

            private const int URLACTION_DOWNLOAD_SIGNED_ACTIVEX = unchecked((int)0x00001001);
            private const int URLACTION_ACTIVEX_RUN = unchecked((int)0x00001200);

            private readonly Guid IID_IInternetSecurityManager = typeof(IInternetSecurityManager).GUID;

            private readonly AxWebBrowser _webBrowser;

            public AxWebBrowserSite(AxWebBrowser webBrowser) : base(webBrowser)
            {
                _webBrowser = webBrowser;
            }

            #region IServiceProvider Members

            int IServiceProvider.QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
            {
                if (guidService == IID_IInternetSecurityManager && riid == IID_IInternetSecurityManager)
                {
                    ppvObject = Marshal.GetComInterfaceForObject(this, typeof(IInternetSecurityManager));
                    return S_OK;
                }
                else
                {
                    ppvObject = IntPtr.Zero;
                    return E_NOINTERFACE;
                }
            }

            #endregion IServiceProvider Members

            #region IInternetSecurityManager Members

            int IInternetSecurityManager.SetSecuritySite(IntPtr pSite)
            {
                return INET_E_DEFAULT_ACTION;
            }

            int IInternetSecurityManager.GetSecuritySite(out IntPtr pSite)
            {
                pSite = IntPtr.Zero;
                return INET_E_DEFAULT_ACTION;
            }

            int IInternetSecurityManager.MapUrlToZone(string pwszUrl, out uint pdwZone, uint dwFlags)
            {
                pdwZone = 0;
                return INET_E_DEFAULT_ACTION;
            }

            int IInternetSecurityManager.GetSecurityId(string pwszUrl, IntPtr pbSecurityId, ref uint pcbSecurityId, ref uint dwReserved)
            {
                return INET_E_DEFAULT_ACTION;
            }

            int IInternetSecurityManager.ProcessUrlAction(string pwszUrl, uint dwAction, IntPtr pPolicy, uint cbPolicy, IntPtr pContext, uint cbContext, uint dwFlags, uint dwReserved)
            {
                int hr = INET_E_DEFAULT_ACTION;
                if ((dwAction == URLACTION_DOWNLOAD_SIGNED_ACTIVEX || dwAction == URLACTION_ACTIVEX_RUN) &&
                    _webBrowser.AllowActiveX != null && cbPolicy >= sizeof(int))
                {
                    if (_webBrowser.AllowActiveX.Value)
                    {
                        Marshal.WriteInt32(pPolicy, URLPOLICY_ALLOW);
                        hr = S_OK;
                    }
                    else
                    {
                        Marshal.WriteInt32(pPolicy, URLPOLICY_DISALLOW);
                        hr = S_FALSE;
                    }
                }
                return hr;
            }

            int IInternetSecurityManager.QueryCustomPolicy(string pwszUrl, ref Guid guidKey, out IntPtr ppPolicy, out uint pcbPolicy, IntPtr pContext, uint cbContext, uint dwReserved)
            {
                ppPolicy = IntPtr.Zero;
                pcbPolicy = 0;
                return INET_E_DEFAULT_ACTION;
            }

            int IInternetSecurityManager.SetZoneMapping(uint dwZone, string lpszPattern, uint dwFlags)
            {
                return INET_E_DEFAULT_ACTION;
            }

            int IInternetSecurityManager.GetZoneMappings(uint dwZone, out IEnumString ppEnumString, uint dwFlags)
            {
                ppEnumString = null;
                return INET_E_DEFAULT_ACTION;
            }

            #endregion IInternetSecurityManager Members
        }

        #endregion AxBrowserSite Class

        #region WebBrowser2EventHelper

        // Handles events from the underlying ActiveX control by raising corresponding events defined in this class.
        private class WebBrowser2EventHelper : StandardOleMarshalObject, _DWebBrowserEvents2
        {
            private readonly AxWebBrowser _parent;

            public WebBrowser2EventHelper(AxWebBrowser parent)
            {
                _parent = parent;
            }

            public void NavigateError(object pDisp, ref object url, ref object targetFrameName, ref object statusCode, ref bool cancel)
            {
                WebBrowserNavigateErrorEventArgs args = new WebBrowserNavigateErrorEventArgs(new Uri((string)url), (string)targetFrameName, (int)statusCode, cancel);
                _parent.OnNavigateError(args);
                cancel = args.Cancel;
            }
        }

        #endregion WebBrowser2EventHelper

        #region Native Interfaces

        [ComImport, ComConversionLoss, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        protected interface IServiceProvider
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int QueryService(
                 [In] ref Guid guidService,
                 [In] ref Guid riid,
                 [Out] out IntPtr ppvObject);
        }

        [ComImport, ComConversionLoss, GuidAttribute("79EAC9EE-BAF9-11CE-8C82-00AA004BA90B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        protected interface IInternetSecurityManager
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetSecuritySite(
                [In] IntPtr pSite);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetSecuritySite(
                [Out] out IntPtr pSite);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int MapUrlToZone(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl,
                [Out] out UInt32 pdwZone,
                [In] UInt32 dwFlags);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetSecurityId(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl,
                [Out] IntPtr pbSecurityId,
                [In, Out] ref UInt32 pcbSecurityId,
                [In] ref UInt32 dwReserved);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int ProcessUrlAction(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl,
                [In] UInt32 dwAction,
                [In] IntPtr pPolicy, [In] UInt32 cbPolicy,
                [In] IntPtr pContext, [In] UInt32 cbContext,
                [In] UInt32 dwFlags, [In] UInt32 dwReserved);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int QueryCustomPolicy(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl,
                [In] ref Guid guidKey,
                [Out] out IntPtr ppPolicy, [Out] out UInt32 pcbPolicy,
                [In] IntPtr pContext, [In] UInt32 cbContext,
                [In] UInt32 dwReserved);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetZoneMapping(
                [In] UInt32 dwZone,
                [In, MarshalAs(UnmanagedType.LPWStr)] string lpszPattern,
                [In] UInt32 dwFlags);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetZoneMappings(
                [In] UInt32 dwZone,
                [Out] out IEnumString ppEnumString,
                [In] UInt32 dwFlags);
        }

        [ComImport, Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch), TypeLibType(TypeLibTypeFlags.FDispatchable | TypeLibTypeFlags.FHidden)]
        private interface _DWebBrowserEvents2
        {
            [DispId(271)]
            void NavigateError(
                [In, MarshalAs(UnmanagedType.IDispatch)] object pDisp,
                [In] ref object URL,
                [In] ref object targetFrameName,
                [In] ref object statusCode,
                [In, Out] ref bool cancel);
        }

        #endregion Native Interfaces
    }
}

namespace System.Windows.Forms
{
    // Represents the method that will handle the WebBrowser.NavigateError event.
    public delegate void WebBrowserNavigateErrorEventHandler(object sender, WebBrowserNavigateErrorEventArgs e);
    // Provides data for the WebBrowser.NavigateError event.
    public class WebBrowserNavigateErrorEventArgs : CancelEventArgs
    {
        public Uri Url { get; private set; }
        public string TargetFrameName { get; private set; }
        public int StatusCode { get; private set; }

        public WebBrowserNavigateErrorEventArgs(Uri url, string targetFrameName, int statusCode, bool cancel) : base(cancel)
        {
            Url = url;
            TargetFrameName = targetFrameName;
            StatusCode = statusCode;
        }
    }
}
