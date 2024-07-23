using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Browsers.Api;
using FWBS.OMS.UI.Windows;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace FWBS.OMS.UI.UserControls.Browsers
{
    public partial class ucRichBrowser : UserControl
    {
        private uint _invalidAuthCredentialsCount;

        // Workaround to the bug in WebView2 SDK v1.0.1150.38. In Office 2013 the following error occurs:
        // Unable to load DLL 'WebView2Loader.dll': The specified module could not be found. (Exception from HRESULT: 0x8007007E)
        static ucRichBrowser()
        {
            string runtimePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "runtimes", Environment.Is64BitProcess ? "win-x64" : "win-x86", "native");
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + runtimePath);
        }

        public ucRichBrowser()
        {
            InitializeComponent();
            browser.CreationProperties = new CoreWebView2CreationProperties()
            {
                UserDataFolder = Global.GetDBAppDataPath()
            };
        }

        public event EventHandler Initialized;
        public event WebBrowserNavigatingEventHandler Navigating;
        public event WebBrowserNavigateErrorEventHandler NavigateError;
        public event WebBrowserDocumentCompletedEventHandler DocumentCompleted;

        protected virtual void OnInitialized(EventArgs e)
        {
            IsInitialized = true;
            Initialized?.Invoke(this, e);
        }

        [Browsable(false)]
        public bool IsInitialized { get; private set; }

        public void Navigate(string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                    url = "about:blank";
                browser.Source = new Uri(url);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        public void Stop()
        {
            browser.Stop();
        }

        public bool SetCookie(string baseUrl, string cookieName, string data)
        {
            try
            {
                var uri = new Uri(baseUrl);
                var cookieManager = browser.CoreWebView2.CookieManager;
                var cookie = cookieManager.CreateCookie(cookieName, data, $".{uri.Host}", "/");
                cookieManager.AddOrUpdateCookie(cookie);
                return true;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
                return false;
            }
        }

        private void WebView2Control_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                CoreWebView2Settings settings = browser.CoreWebView2.Settings;
                
                bool isInDebug = Session.CurrentSession.IsInDebug;
                settings.AreDefaultContextMenusEnabled = isInDebug;
                settings.AreDevToolsEnabled = isInDebug;
                
                settings.IsStatusBarEnabled = false;
                settings.IsZoomControlEnabled = false;
                settings.AreBrowserAcceleratorKeysEnabled = false;
                settings.IsPasswordAutosaveEnabled = true;
                
                browser.CoreWebView2.BasicAuthenticationRequested += CoreWebView2_BasicAuthenticationRequested;
                browser.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
                browser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
                browser.CoreWebView2.WindowCloseRequested += CoreWebView2_WindowCloseRequested;
                OnInitialized(EventArgs.Empty);
            }
            else
            {
                ucErrorBox err = new ucErrorBox();
                err.SetErrorBox(e.InitializationException);
                err.Location = new System.Drawing.Point(10, 20);
                Controls.Add(err, true);
                browser.Hide();
            }
        }

        private static System.Net.NetworkCredential QueryCredentials(string host, IntPtr parentWnd)
        {
            System.Net.NetworkCredential credentials = null;
            var credentialsDialog = new WindowsSecurityDialogApi(
                    string.Format("{0} : {1}", Branding.APPLICATION_NAME, host),
                    Session.CurrentSession.Resources.GetResource("ENTRCRDNTLS", "Enter your credentials", "").Text,
                    Session.CurrentSession.Resources.GetResource("CRDNTLWLSBEUSD", "These credentials will be used to connect to '%1%'", "", host).Text,
                    parentWnd);
            bool authenticationUsedCredManager;
            if (credentialsDialog.ValidateCredentials(out authenticationUsedCredManager))
            {
                credentials = credentialsDialog.Credentials;
            }
            return credentials;
        }

        // In order to avoid re-entrancy issue, open credentials prompt dialog asynchronously and wait for the result.
        // https://docs.microsoft.com/en-us/microsoft-edge/webview2/concepts/threading-model#re-entrancy
        private void CoreWebView2_BasicAuthenticationRequested(object sender, CoreWebView2BasicAuthenticationRequestedEventArgs e)
        {
            using (CoreWebView2Deferral deferral = e.GetDeferral())
            {
                string host = new Uri(e.Uri).Host;
                IntPtr wnd = Handle;
                var credentials = System.Threading.Tasks.Task.Run(() => QueryCredentials(host, wnd)).Result;
                if (credentials != null)
                {
                    e.Response.UserName = credentials.UserName;
                    e.Response.Password = credentials.Password;
                }
                else
                {
                    e.Cancel = true;
                }
            }
         }

        private void CoreWebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                _invalidAuthCredentialsCount = 0;
            }
            else if (e.WebErrorStatus == CoreWebView2WebErrorStatus.ValidAuthenticationCredentialsRequired && _invalidAuthCredentialsCount++ > 1)
            {
                // Ensure that we don't stuck forever with invalid saved credentials
                string host = new Uri(((CoreWebView2)sender).Source).Host;
                string credentialsKey = string.Format("{0} : {1}", Branding.APPLICATION_NAME, host);
                CredentialManagerApi.DeleteCredentials(credentialsKey);
                _invalidAuthCredentialsCount = 0;
            }
        }

        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            e.Handled = true; // Disable new windows (bug, doesn't always work)
        }

        private void CoreWebView2_WindowCloseRequested(object sender, object e)
        {
        }

        private void browser_KeyEvent(object sender, KeyEventArgs e)
        {
            e.Handled = true; // Disable accelerators
        }

        private void browser_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            if (Navigating != null)
            {
                var args = new WebBrowserNavigatingEventArgs(new Uri(e.Uri), string.Empty);
                Navigating.Invoke(sender, args);
                e.Cancel = args.Cancel;
            }
        }

        private void browser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess && DocumentCompleted != null)
            {
                var args = new WebBrowserDocumentCompletedEventArgs(((WebView2)sender).Source);
                DocumentCompleted.Invoke(sender, args);
            }
            else if (!e.IsSuccess && NavigateError != null)
            {
                var args = new WebBrowserNavigateErrorEventArgs(((WebView2)sender).Source, string.Empty, (int)e.WebErrorStatus, false);
                NavigateError.Invoke(sender, args);
                if (args.Cancel)
                    ((WebView2)sender).Stop();
            }
        }
    }
}
