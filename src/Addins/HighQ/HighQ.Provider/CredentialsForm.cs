using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FWBS.OMS.HighQ
{
    internal class CredentialsForm : Form
    {
        private const string CODE = "code";

        private readonly int _clientId;
        private readonly string _site;
        private readonly string _uri;
        private readonly string _redirectUri;
        private WebBrowser Browser = null;

        public CredentialsForm(int clientId, string site, string redirectUri)
        {
            InitializeComponent();

            this.Text = Session.CurrentSession.Resources.GetResource("HQAUTHTITLE", "HighQ Authentication Form", "").Text;
            _clientId = clientId;
            _site = site;
            _redirectUri = redirectUri;
            _uri = $"{_site}/authorize.action?response_type={CODE}&client_id={_clientId}&redirect_uri={_redirectUri}";
        }

        public string AuthorizationCode { get; private set; }

        private void browser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Dictionary<string, string> parameters = null;
            if (!string.IsNullOrEmpty(e.Url.Query))
            {
                parameters = ParseFragment(e.Url.Query, new char[] { '&', '?' });
            }

            if (e.Url.AbsoluteUri.StartsWith($"{_redirectUri}"))
            {
                if (parameters != null && parameters.ContainsKey(CODE))
                {
                    AuthorizationCode = parameters[CODE];
                }

                Close();
            }
        }

        private Dictionary<string, string> ParseFragment(string queryString, char[] delimeters)
        {
            var parameters = new Dictionary<string, string>();
            string[] pairs = queryString.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
            foreach (string pair in pairs)
            {
                string[] nameValue = pair.Split(new char[] { '=' });
                parameters.Add(nameValue[0], nameValue[1]);
            }

            return parameters;
        }

        public void GetTokens()
        {
            this.Browser.Navigate(this._uri);

            Application.EnableVisualStyles();
            Application.Run(this);
        }

        private void InitializeComponent()
        {
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // Browser
            // 
            this.Browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Browser.Location = new System.Drawing.Point(0, 0);
            this.Browser.Name = "Browser";
            this.Browser.Size = new System.Drawing.Size(620, 400);
            this.Browser.TabIndex = 0;
            this.Browser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.browser_Navigated);
            // 
            // CredentialsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(620, 400);
            this.Controls.Add(this.Browser);
            this.Name = "CredentialsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
    }
}
