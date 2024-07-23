using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// Summary description for eWebBrowser.
    /// </summary>
    public class eWebBrowser : System.Windows.Forms.UserControl, IBasicEnquiryControl2, IWebBrowserControl
	{
		#region Fields
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>
		/// Various variables used by IEnquiryControl2 Implementation
		/// </summary>
		private bool _omsdesignmode = false;
		private bool _readonly = false;
		private bool _isdirty = false;
		private int _captionWidth = 0;
		private bool _required = false;
        private AxWebBrowser axWebBrowser1;
		
		/// <summary>
		/// string used as a url. 
		/// </summary>
		string _sVal;
        #endregion

        #region Properties

        [Category("Misc")]
        public bool AutoZoom { get; set; }

        private int ScaleZoom => DeviceDpi * 100 / 96;

        #endregion

        #region Constructors and Destructors
        public eWebBrowser()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		
		#endregion
		
		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.axWebBrowser1 = new AxWebBrowser();
            this.SuspendLayout();
            // 
            // axWebBrowser1
            // 
            this.axWebBrowser1.AllowActiveX = true;
            this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axWebBrowser1.Location = new System.Drawing.Point(10, 10);
            this.axWebBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.axWebBrowser1.Name = "axWebBrowser1";
            this.axWebBrowser1.Size = new System.Drawing.Size(478, 333);
            this.axWebBrowser1.TabIndex = 0;
            // 
            // eWebBrowser
            // 
            this.Controls.Add(this.axWebBrowser1);
            this.Name = "eWebBrowser";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(498, 353);
            this.ResumeLayout(false);

		}
        #endregion

        #region Overrides

        protected override void OnDpiChangedBeforeParent(EventArgs e)
        {
            base.OnDpiChangedBeforeParent(e);
            if (AutoZoom)
            {
                axWebBrowser1.Zoom(ScaleZoom);
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                BeginInvoke(new Action<EventArgs>(OnInitialized), EventArgs.Empty);
            }
        }

        #endregion

        #region IBasicEnquiryControl2 Implementation

        /// <summary>
        /// Changed Event
        /// </summary>
        public event EventHandler Changed;
		
		/// <summary>
		/// ActiveChanged event
		/// </summary>
		public event EventHandler ActiveChanged;

		/// <summary>
		/// Gets whether the current control can be stretched by its Y co-ordinate.
		/// This is a design mode property and is set to true.
		/// </summary>
		[Browsable(false)]
		public bool LockHeight 
		{
			get
			{
				return true;
			}
		}
		
		/// <summary>
		/// Gets or Sets the value of the control.  In this case it should be True of False.
		/// </summary>
		public object Value
		{
			get
			{
				return _sVal;
			}
			set
			{
				try
				{
					_sVal = Convert.ToString(value);
					if (!omsDesignMode && IsInitialized)
						Navigate(_sVal);
				}
				catch
				{
				}
				if (this.Parent != null)
				{
					IsDirty=true;
					OnChanged();
				}
			}
		}

		
		/// <summary>
		/// Gets or Sets the flag that tells the rendering form that this control is required
		/// to be filled in case the underlying value is DBNull.
		/// </summary>
		[Category("Behavior")]
		public bool Required 
		{
			get
			{
				return _required;
			}
			set
			{
				_required = value;
			}
		}

		
		/// <summary>
		/// Gets or Sets the editable format of the control.
		/// </summary>
		[Category("Behavior")]
		public bool ReadOnly 
		{
			get
			{
				return _readonly;
			}
			set
			{
				_readonly = value;
			}
		}

		/// <summary>
		/// Has data within the control changed.
		/// </summary>
		[Browsable(false)]
		public bool IsDirty
		{
			get
			{
				return _isdirty;
			}
			set
			{
				_isdirty = value;
			}
		}

		/// <summary>
		/// Gets the caption width of a control, leaving the rest of the width of the control
		/// to be the width of the internal editing control.
		/// </summary>
		[Browsable(false)]
		public int CaptionWidth
		{
			get
			{
				return _captionWidth;
			}
			set
			{
			}
		}

        [Browsable(false)]
        public bool CaptionTop
        {
            get
            {
                return false;
            }
            set { }
        }

		/// <summary>
		/// Executes the changed event.
		/// </summary>
		public void OnChanged()
		{
			if (Changed!= null && IsDirty)
				Changed(this, EventArgs.Empty);
			IsDirty=false;
		}

		public void OnActiveChanged()
		{
			IsDirty = true;
            ActiveChanged?.Invoke(this, EventArgs.Empty);
        }

		/// <summary>
		/// Returns an instance of the control.
		/// </summary>
		public object Control
		{
			get
			{
				return axWebBrowser1;
			}
		}
		
		/// <summary>
		/// Gets or Sets the design mode property of the control.
		/// </summary>
		[Browsable(false)]
		public bool omsDesignMode
		{
			get
			{
				return _omsdesignmode;
			}
			set
			{
				_omsdesignmode = value;
			}
		}

        #endregion

        #region _cookieProcessing
        //WIN32 API Needed to set cookie for Microsoft IE web browser control
        [DllImport("wininet.dll")]
        static extern InternetCookieState InternetSetCookieEx(
            string lpszURL,
            string lpszCookieName,
            string lpszCookieData,
            int dwFlags,
            IntPtr dwReserved);

        const int INTERNET_COOKIE_HTTPONLY = 8192;

        enum InternetCookieState : int
        {
            COOKIE_STATE_UNKNOWN = 0x0,
            COOKIE_STATE_ACCEPT = 0x1,
            COOKIE_STATE_PROMPT = 0x2,
            COOKIE_STATE_LEASH = 0x3,
            COOKIE_STATE_DOWNGRADE = 0x4,
            COOKIE_STATE_REJECT = 0x5,
            COOKIE_STATE_MAX = COOKIE_STATE_REJECT
        }
      
        public bool SetCookie(string baseUrl, string cookieName, string data)
        {
            return InternetSetCookieEx(baseUrl, cookieName, data, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero) != InternetCookieState.COOKIE_STATE_UNKNOWN;
        }
        #endregion

		public event EventHandler Initialized;

		protected virtual void OnInitialized(EventArgs e)
		{
            if (!omsDesignMode)
            {
                IsInitialized = true;
                Initialized?.Invoke(this, e);
                if (!string.IsNullOrEmpty(_sVal))
                    Navigate(_sVal);
            }
		}

		public bool IsInitialized { get; private set; }

		public void Navigate(string url)
		{
			axWebBrowser1.Navigate(url);
			if (AutoZoom)
				axWebBrowser1.Zoom(ScaleZoom);
		}

        public void Stop()
        {
            axWebBrowser1.Stop();
        }

        public event WebBrowserNavigatingEventHandler Navigating
        {
            add { axWebBrowser1.Navigating += value; }
            remove { axWebBrowser1.Navigating -= value; }
        }

        public event WebBrowserNavigateErrorEventHandler NavigateError
        {
            add { axWebBrowser1.NavigateError += value; }
            remove { axWebBrowser1.NavigateError -= value; }
        }

        public event WebBrowserDocumentCompletedEventHandler DocumentCompleted
        {
            add { axWebBrowser1.DocumentCompleted += value; }
            remove { axWebBrowser1.DocumentCompleted -= value; }
        }
    }
}
