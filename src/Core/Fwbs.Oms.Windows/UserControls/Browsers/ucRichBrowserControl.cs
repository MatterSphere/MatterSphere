using System;
using System.ComponentModel;
using FWBS.Common.UI;

namespace FWBS.OMS.UI.UserControls.Browsers
{
    public class ucRichBrowserControl : ucRichBrowser, IBasicEnquiryControl2, IWebBrowserControl
    {
        public ucRichBrowserControl()
        {
        }

        public event EventHandler ActiveChanged;
        public event EventHandler Changed;
        public object Control { get { return browser; } }

        [Browsable(false)]
        public int CaptionWidth { get; set; }
        [Browsable(false)]
        public bool CaptionTop { get; set; }
        [Browsable(false)]
        public bool LockHeight { get; }
        [Browsable(false)]
        public bool Required { get; set; }
        [Browsable(false)]
        public bool ReadOnly { get; set; }

        [Browsable(false)]
        public bool omsDesignMode { get; set; }

        private string url = string.Empty;
        public object Value
        {
            get { return Url; }
            set { Url = Convert.ToString(value); }
        }

        [Browsable(false)]
        [DefaultValue("")]
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = (string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(SpecificDataCode))
                    ? Convert.ToString(Session.CurrentSession.GetSpecificData(SpecificDataCode)).Trim()
                    : value ?? string.Empty;
                if (!omsDesignMode && !string.IsNullOrEmpty(url))
                    Navigate(url);
                OnChanged();
            }
        }

        [DefaultValue(null)]
        public string SpecificDataCode { get; set; }

        [Browsable(false)]
        public bool IsDirty { get; set; }
        public void OnChanged()
        {
            if (Changed != null && IsDirty)
                Changed(this, EventArgs.Empty);
            IsDirty = false;
        }

        public void OnActiveChanged()
        {
            IsDirty = true;
            ActiveChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
