using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.Common.UI;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows
{

    /// <summary>
    /// Windows enquiry form Renderer.  This uses FormRenderBase to render most of the form.
    /// This render does copes with wizard paging and integrates with the Enquiry engine within
    /// the business layer.
    /// </summary>
    [ToolboxItem(true)]
	public class EnquiryForm : FormRendererBase, FWBS.OMS.UI.Windows.Interfaces.IOpenOMSType, FWBS.OMS.UI.Windows.Interfaces.IOMSItem, IOBjectDirty
	{
		#region Fields
		/// <summary>
		/// Designer generated variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Enquiry engine object.
		/// </summary>
		private Enquiry _enq = null;

		/// <summary>
		/// If the Controls are changed then
		/// </summary>
		private bool _isdirty = false;

		/// <summary>
		/// 		/// To Be Refreshed when Active
		/// </summary>
		private bool _toberefresh=false;

		//
		// Wizard / Data manipulation Action buttons.
		//
		private System.Windows.Forms.Button btnAddNew = null;
		private System.Windows.Forms.Button btnUpdate = null;
		private System.Windows.Forms.Button btnCancel = null;
		private System.Windows.Forms.Button btnBack = null;
		private System.Windows.Forms.Button btnNext = null;
		private System.Windows.Forms.Button btnFinish = null;
		private System.Windows.Forms.Button btnHelp = null;
		private System.Windows.Forms.Label lblPageHeader = null;
		private System.Windows.Forms.Label lblWelcomeHeader = null;
        private System.Windows.Forms.PictureBox picturePageHeader = null;
        private System.Windows.Forms.PictureBox pictureWelcomePage = null;
        private System.Windows.Forms.RichTextBox txtWelcomeText = null;

		/// <summary>
		/// Current page if set to wizard mode.
		/// -1 is used to show the welcome page.
		/// </summary>
		private short _page = -2; // Set to -2 to Force the First Page Change

		/// <summary>
		/// The Current Page Name
		/// </summary>
		private string _pagename = "";

        /// <summary>
        /// A boolean flag to decide whether the form can be closed.  If set to false then
        /// the wizard will not be closed, which would normally mean that the wizard did not pass
        /// its validation.
        /// </summary>
        private bool allowFinish = false;

		/// <summary>
		/// Y coordinate leading for wizard controls when they are placed immediately after one another.
		/// </summary>
		private int newY = 0;


		/// <summary>
		/// Enquiry form style.
		/// </summary>
		private EnquiryStyle _style = EnquiryStyle.Standard;

		
		/// <summary>
		/// Data view to display only those non deleted pages.
		/// </summary>
		private DataView _pages;

		/// <summary>
		/// Holds a reference to all those controls that are to filter others.
		/// </summary>
		private ArrayList _filterControls = new ArrayList();

		/// <summary>
		/// A flag to quickly check whether the enquiry form should resize the parent form or not.
		/// </summary>
		private bool _changeFormSize = true;

		/// <summary>
		/// The Back Page Order
		/// </summary>
		private ArrayList _pageflow = new ArrayList();
		private System.Windows.Forms.ContextMenu Diagnostics;
		private System.Windows.Forms.MenuItem mnuDump;
		private System.Windows.Forms.MenuItem mnuRefreshRebind;
		private System.Windows.Forms.MenuItem mnuSize;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem mnuName;
		private System.Windows.Forms.MenuItem mnuToolTips;
		private System.Windows.Forms.HelpProvider helpProvider1;

		/// <summary>
		/// The Current Page Flow Index
		/// </summary>
		private int _pageflowinx = 0;
		#endregion

		#region Events

		/// <summary>
		/// Event that occurs before a page change in the wizard style.
		/// </summary>
		[Category("Wizard")]
		public event PageChangingEventHandler PageChanging = null;
		
		/// <summary>
		/// Event that occurs after a page change in the wizard style.
		/// </summary>
		[Category("Wizard")]
		public event PageChangedEventHandler PageChanged = null;
		
		/// <summary>
		/// Event that occurs when the wizard has successfully finished.
		/// </summary>
		[Category("Wizard")]
		public event EventHandler Finished = null;

		/// <summary>
		/// Event that occurs when the wizard is just about to finish.
		/// </summary>
		[Category("Wizard")]
		public event CancelEventHandler Finishing = null;

		/// <summary>
		/// Event that occurs when the Standard is just about to update.
		/// </summary>
		[Category("Standard")]
		public event CancelEventHandler Updating = null;
		
		/// <summary>
		/// Event that occurs when the Standard has successfully updated.
		/// </summary>
		[Category("Standard")]
		public event EventHandler Updated = null;

		/// <summary>
		/// Event the occurs before the RefreshControls 
		/// </summary>
		[Category("Standard")]
		public event CancelEventHandler RefreshingControls = null;

		/// <summary>
		/// Event the occurs after the RefreshControls 
		/// </summary>
		[Category("Standard")]
		public event EventHandler RefreshedControls = null;

		/// <summary>
		/// Event the occurs after the RefreshControls 
		/// </summary>
		[Category("Standard")]
		public event EventHandler EnquiryLoaded = null;
		
		/// <summary>
		/// An event that gets raised if the return object of an enquiry command is
		/// an OMSType object.
		/// </summary>
		public event NewOMSTypeWindowEventHandler NewOMSTypeWindow = null;
		
		/// <summary>
		/// An event that gets raised when the enquiry form cancels any changes.
		/// </summary>
		public event EventHandler Cancelled = null;

        /// <summary>
        /// An event that gets raised when the Enquiry Property it changed
        /// </summary>
        public event EventHandler EnquiryPropertyChanged = null;

		/// <summary>
		/// If the Form Become Dirty
		/// </summary>
		public event EventHandler Dirty = null;

		#endregion

		#region Constructors & Destructors

		public EnquiryForm()
		{
            /// <summary>
            /// Required for Windows.Forms Class Composition Designer support
            /// </summary>
            InitializeComponent();
            this.ControlRemoved += new ControlEventHandler(EnquiryForm_ControlRemoved);
		}


        void EnquiryForm_ControlRemoved(object sender, ControlEventArgs e)
        {
            Control c = e.Control;
            if (c is IBasicEnquiryControl2)
                ((IBasicEnquiryControl2)c).Changed -= new EventHandler(DataBindingChanged);
        }


		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.Diagnostics = new System.Windows.Forms.ContextMenu();
            this.mnuName = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuDump = new System.Windows.Forms.MenuItem();
            this.mnuToolTips = new System.Windows.Forms.MenuItem();
            this.mnuRefreshRebind = new System.Windows.Forms.MenuItem();
            this.mnuSize = new System.Windows.Forms.MenuItem();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.req)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._lists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._questions)).BeginInit();
            this.SuspendLayout();
            // 
            // Diagnostics
            // 
            this.Diagnostics.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuName,
            this.menuItem2,
            this.mnuDump,
            this.mnuToolTips,
            this.mnuRefreshRebind,
            this.mnuSize});
            this.Diagnostics.Popup += new System.EventHandler(this.Diagnostics_Popup);
            // 
            // mnuName
            // 
            this.mnuName.Enabled = false;
            this.mnuName.Index = 0;
            this.mnuName.Text = "Name";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "-";
            // 
            // mnuDump
            // 
            this.mnuDump.Index = 2;
            this.mnuDump.Text = "Show Data Table";
            this.mnuDump.Click += new System.EventHandler(this.mnuDump_Click);
            // 
            // mnuToolTips
            // 
            this.mnuToolTips.Index = 3;
            this.mnuToolTips.Text = "Show Diagnostic Tool Tips";
            this.mnuToolTips.Click += new System.EventHandler(this.mnuToolTips_Click);
            // 
            // mnuRefreshRebind
            // 
            this.mnuRefreshRebind.Index = 4;
            this.mnuRefreshRebind.Text = "Refresh Data";
            this.mnuRefreshRebind.Click += new System.EventHandler(this.mnuRefreshRebind_Click);
            // 
            // mnuSize
            // 
            this.mnuSize.Index = 5;
            this.mnuSize.Text = "Size";
            // 
            // EnquiryForm
            // 
            this.AutoScroll = true;
            this.helpProvider1.SetHelpString(this, "");
            this.Name = "EnquiryForm";
            this.helpProvider1.SetShowHelp(this, false);
            this.Size = new System.Drawing.Size(199, 186);
            this.RightToLeftChanged += new System.EventHandler(this.EnquiryForm_RightToLeftChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.EnquiryForm_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EnquiryForm_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.req)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._lists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._questions)).EndInit();
            this.ResumeLayout(false);

		}

		protected override void Dispose( bool disposing )
		{
            try
            {
                if (disposing)
                {
                    Global.RemoveAndDisposeControls(this); 

                    if (components != null)
                    {
                        components.Dispose();
                    }

                    if (_enq != null)
                    {
                        _enq.Dispose();
                        _enq = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
		}
		#endregion

		#region Properties

		/// <summary>
		/// Sets the parent form size flag.
		/// </summary>
		[DefaultValue(true)]
		[Category("Wizard")]
		public bool ChangeParentFormSize
		{
			set
			{
				_changeFormSize = value;
			}
			get
			{
				return _changeFormSize;
			}
		}

		/// <summary>
		/// Gets or Sets a label object that manipulates the page header text.
		/// </summary>
		[DefaultValue(null)]
		[Category("Wizard")]
		public System.Windows.Forms.Label PageHeader
		{
			get
			{
				return lblPageHeader;
			}
			set
			{
				lblPageHeader = value;
			}
		}

        /// <summary>
        /// Gets or Sets a Welcome Page Picture Box
        /// </summary>
        [DefaultValue(null)]
        [Category("Wizard")]
        public System.Windows.Forms.PictureBox WelcomePagePicture
        {
            get
            {
                return pictureWelcomePage;
            }
            set
            {
                pictureWelcomePage = value;
            }
        }
        
        /// <summary>
        /// Gets or Sets a Page Picture Box
        /// </summary>
        [DefaultValue(null)]
        [Category("Wizard")]
        public System.Windows.Forms.PictureBox PageHeaderPicture
        {
            get
            {
                return picturePageHeader;
            }
            set
            {
                picturePageHeader = value;
            }
        }

		/// <summary>
		/// Gets or Sets a label that manipulates the welcome header of a wizard style form.
		/// </summary>
		[DefaultValue(null)]
		[Category("Wizard")]
		public System.Windows.Forms.Label WelcomeHeader
		{
			get
			{
				return lblWelcomeHeader;
			}
			set
			{
				lblWelcomeHeader = value;
			}
		}

		/// <summary>
		/// Gets or Sets a label that manipulates the more descriptive welcome text of a wizard style form.
		/// </summary>
		[DefaultValue(null)]
		[Category("Wizard")]
		public System.Windows.Forms.RichTextBox WelcomeText
		{
			get
			{
				return txtWelcomeText;
			}
			set
			{
				txtWelcomeText = value;
			}
		}

		/// <summary>
		/// Get or Sets the forms enquiry style, standard or wizard etc...
		/// </summary>
		[DefaultValue(EnquiryStyle.Standard)]
		[Category("Behavior")]
		public EnquiryStyle Style
		{
			get
			{
				return _style;
			}
			set
			{
				if (_style != value)
				{
					_style = value;
				}
			}
		}

		/// <summary>
		/// Gets the unque enquiry code.
		/// </summary>
		[Browsable(false)]
		public string Code
		{
			get
			{
				try
				{
					object obj = GetExtraInfo("enqcode");
					return obj.ToString() ;
				}
				catch
				{
					return "";
				}
			}
		}

		/// <summary>
		/// Gets the description of the enquiry form.
		/// </summary>
		[Browsable(false)]
		public string Description
		{
			get
			{
				try
				{
					object obj = GetExtraInfo("enqdesc");
					return obj.ToString() ;
				}
				catch
				{
					return "";
				}
			}
		}

		/// <summary>
		/// To Be Refreshed when Active
		/// </summary>
		[Browsable(false)]
		public bool ToBeRefreshed
		{
			get
			{
				return _toberefresh;
			}
			set
			{
				_toberefresh = value;
			}
		}

		
		/// <summary>
		/// Gets the version of the enquiry form header.
		/// </summary>
		[Browsable(false)]
		public long Version
		{
			get
			{
				try
				{
					object obj = GetExtraInfo("enqversion");
					return (long)obj;
				}
				catch
				{
					return (long)0;
				}
			}
		}

		/// <summary>
		/// Refreshes the Welcome Page Labels
		/// </summary>
		public void RefreshWizardWelcomePage()
		{
            if (lblWelcomeHeader != null)
            {
                lblWelcomeHeader.Text = _enq.Source.Tables["ENQUIRY"].Rows[0]["enqWelcomeHeader"].ToString();
                HandleFont(lblWelcomeHeader, 14);
            }

            if (pictureWelcomePage != null)
            {
                if (Enquiry.WelcomePageImage != null)
                {
                    pictureWelcomePage.Image = Enquiry.WelcomePageImage;
                    pictureWelcomePage.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureWelcomePage.Tag = pictureWelcomePage.Image;
                }
                else
                {
                    pictureWelcomePage.SizeMode = PictureBoxSizeMode.Normal;
                    pictureWelcomePage.Tag = null;
                }
            }

            if (txtWelcomeText != null)
			{
				try
				{
					txtWelcomeText.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(_enq.Source.Tables["ENQUIRY"].Rows[0]["enqWelcomeText"].ToString());
                    HandleFont(txtWelcomeText);
                    
                    if (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                        Global.SetRichTextBoxRightToLeft(txtWelcomeText);
				}
				catch
				{
					txtWelcomeText.Text = _enq.Source.Tables["ENQUIRY"].Rows[0]["enqWelcomeText"].ToString();
				}
			}
		}

        /// <summary>
        /// Enlarge small image canvas size to 48*48 pixels.  
        /// </summary>
        /// <param name="image">Source image</param>
        /// <returns>Image</returns>
        public static Image EnsurePageHeaderImageSize(Image image)
        {
            if (image != null && image.Width < 48 && image.Height < 48)
            {
                Bitmap bitmap = new Bitmap(48, 48);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawImage(image, (bitmap.Width - image.Width) / 2, (bitmap.Height - image.Width) / 2, image.Width, image.Height);
                }
                image = bitmap;
            }
            return image;
        }

        /// <summary>
        /// Sets the font and font size of a given control if MatterSphere UI Version is 2.
        /// Leaves untouched and defaults to standard settings if Version 1.
        /// </summary>
        /// <param name="control"></param>
        private void HandleFont(Control control, int fontSize = 0)
        {
            control.Font = new Font(CurrentUIVersion.Font
                                    ,(fontSize == 0) ? CurrentUIVersion.FontSize : fontSize);
        }


		/// <summary>
		/// Gets or Sets an enquiry engine object to the source of this enquiry form.  This object is created
		/// by the business layer.
		/// </summary>	
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Enquiry Enquiry
		{
			get
			{
				return _enq;
			}
			set
			{
                long start = DateTime.Now.Ticks;
                {
					if (_enq != null)
					{
						if (_enq.HasScript)
						{
							Script.EnquiryFormScriptType script = null;
							script = _enq.Script.Scriptlet as Script.EnquiryFormScriptType;
							if (script != null)
								script.UnbindEnquiryFormObject(this);
						}

                        Global.RemoveAndDisposeControls(this);

						_enq.DataChanged -= new EventHandler(this.DataChanged);
						_enq.Updated -= new EventHandler(this.EnqUpdated);
						_enq.Refreshed -= new EventHandler(this.Refreshed);
						_enq.ModeChanged -= new EventHandler(this.ModeChanged);
					}

					_enq = value;
#if NODATABIND
					this.ValueChanged -= new System.EventHandler(this.EnquiryForm_ValueChanged);
#endif

					//Default all internal buttons to be disabled.
					SetButtonEnabledProperty(btnAddNew, false);
					SetButtonEnabledProperty(btnBack, false);
					SetButtonEnabledProperty(btnCancel, true);
					SetButtonEnabledProperty(btnHelp, false);
					SetButtonEnabledProperty(btnNext, false);
					SetButtonEnabledProperty(btnUpdate, false);
					SetButtonEnabledProperty(btnFinish, false);

					if (_enq != null)
					{
						//Grab the table full of the rendered questions and set up the event
						//capturing of the underlying data tables.
						_questions = _enq.Source.Tables["QUESTIONS"];
						_lists = _enq.Source;
						base.InDesignMode = _enq.InDesignMode;
						_enq.DataChanged += new EventHandler(this.DataChanged);
						_enq.Updated += new EventHandler(this.EnqUpdated);
						_enq.Refreshed += new EventHandler(this.Refreshed);
						_enq.ModeChanged += new EventHandler(this.ModeChanged);

                        //Call HelpFilePathSetup helper
                        HelpFilePathSetUp();

						SetButtonEnabledProperty(btnFinish, true);
						_pages = new DataView(_enq.Source.Tables["PAGES"]);
						_enq.FormPropertyChanged -= new FWBS.OMS.EnquiryEngine.PropertyChangedEventHandler(this.FormPropertyChanged);

                        this.DockPadding.Top = FWBS.Common.ConvertDef.ToInt32(_enq.Source.Tables["ENQUIRY"].Rows[0]["enqPaddingY"], 0);
                        this.DockPadding.Left = FWBS.Common.ConvertDef.ToInt32(_enq.Source.Tables["ENQUIRY"].Rows[0]["enqPaddingX"], 0);
                        this.DockPadding.Right = this.DockPadding.Left;
                        this.DockPadding.Bottom = this.DockPadding.Top;
                        
                        //Set the specific labels when in enquiry mode, and size the form.
						if (_style == EnquiryStyle.Wizard)
						{
							//Filter all the deleted rows out of the rendering pages procedure.
							_pages.RowStateFilter = DataViewRowState.CurrentRows;
                            if (_pages.Table.Columns.Contains("pgeOrder"))
                            {
                                try { _pages.Sort = "pgeOrder"; }
                                catch { }
                            }
							_pagename = "";
							_pageflowinx = 0;
							ClearPageFlowHistory();
							RefreshWizardWelcomePage();
							RenderControls(true);
							SetParentSize();
							GotoWelcomePage();
							_enq.FormPropertyChanged += new FWBS.OMS.EnquiryEngine.PropertyChangedEventHandler(this.FormPropertyChanged);
						}
						else
						{
							_enq.FormPropertyChanged += new FWBS.OMS.EnquiryEngine.PropertyChangedEventHandler(this.FormPropertyChanged);
                            if (InDesignMode == false)
                            {
                                Visible = false;
                                SetCanvasSize();
                                Application.DoEvents();
                                RenderControls(true);
                                Visible = true;
                            }
						}

#if NODATABIND
						this.ValueChanged += new System.EventHandler(this.EnquiryForm_ValueChanged);
#endif
						this.IsDirty = false;
						OnEnquiryPropertyChanged();
						OnEnquiryLoaded();
					}
				}
                if (_enq != null)
                    Debug.WriteLine(String.Format("Enquiry({1}) - Seconds : {0}", new TimeSpan(DateTime.Now.Ticks - start).TotalSeconds,_enq.Code));
			}
		}

        private void HelpFilePathSetUp()
        {
            if (string.IsNullOrEmpty(helpProvider1.HelpNamespace))
            {
                string helpPath = Session.CurrentSession.GetHelpPath(_enq.Code);
                if (String.IsNullOrEmpty(helpPath))
                {
                    helpProvider1.SetShowHelp(this, false);
                }
                else
                {
                    helpProvider1.HelpNamespace = helpPath;
                    helpProvider1.SetShowHelp(this, true);
                    helpProvider1.SetHelpKeyword(this, Convert.ToString(GetExtraInfo("enqHelpKeyword")));
                    helpProvider1.SetHelpNavigator(this,HelpNavigator.KeywordIndex);
                }
            }
        }

		/// <summary>
		/// Gets the current page number of a wizard form.
		/// </summary>
		[Browsable(false)]
		public short PageNumber
		{
			get
			{
				return _page;
			}
		}

		/// <summary>
		/// Gets the current Page Name of a wizard form
		/// </summary>
		[Browsable(false)]
		public string PageName
		{
			get
			{
				return _pagename;
			}
		}

		/// <summary>
		/// Gets the maximum number of pages within the wizard form.
		/// </summary>
		[Browsable(false)]
		public short PageCount
		{
			get
			{
				if (_pages == null)
					return 0;
				else
					return (short)_pages.Count;
			}
		}

		/// <summary>
		/// Gets the data row information of the page to the corresponding current page number.
		/// </summary>
		[Browsable(false)]
		public DataRow CurrentPage
		{
			get
			{
				if (_page < -1) _page = 0;
				return _pages[(int)_page].Row;
			}
		}

		/// <summary>
		/// Gets or Sets a button object which cancels a potential record or cancels all wizard processing.
		/// </summary>
		[DefaultValue(null)]
		[Category("Actions")]
		public System.Windows.Forms.Button ActionCancel
		{
			get
			{
				return btnCancel;
			}
			set
			{
				if (btnCancel != null)
					btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);

				btnCancel = value;
				
				if (value != null)
					btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

			}
		}

		/// <summary>
		/// Gets or Sets a button object which enables the addition of a record.
		/// </summary>
		[DefaultValue(null)]
		[Category("Actions")]
		public System.Windows.Forms.Button ActionAddNew
		{
			get
			{
				return btnAddNew;
			}
			set
			{
				if (btnAddNew != null)
					btnAddNew.Click -= new System.EventHandler(this.btnAddNew_Click);

				btnAddNew = value;
				
				if (value != null)
					btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
			}
		}

		/// <summary>
		/// Gets or Sets a button object which updates the underlying data through the enquiry engine.
		/// </summary>
		[DefaultValue(null)]
		[Category("Actions")]
		public System.Windows.Forms.Button ActionUpdate
		{
			get
			{
				return btnUpdate;
			}
			set
			{
				if (btnUpdate != null)
					btnUpdate.Click -= new System.EventHandler(this.btnUpdate_Click);

				btnUpdate = value;
				
				if (value != null)
					btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			}
		}

		/// <summary>
		/// Gets or Sets a button object which executes any help information.
		/// </summary>
		[DefaultValue(null)]
		[Category("Actions")]
		public System.Windows.Forms.Button ActionHelp
		{
			get
			{
				return btnHelp;
			}
			set
			{
				if (btnHelp != null)
					btnHelp.Click -= new System.EventHandler(this.btnHelp_Click);

				btnHelp = value;
				
				if (value != null)
					btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			}
		}

		/// <summary>
		/// Gets or Sets q button object which moves next in a wizard style form.
		/// </summary>
		[DefaultValue(null)]
		[Category("Actions")]
		public System.Windows.Forms.Button ActionNext
		{
			get
			{
				return btnNext;
			}
			set
			{
				if (btnNext != null)
					btnNext.Click -= new System.EventHandler(this.btnNext_Click);

				btnNext = value;
				
				if (value != null)
					btnNext.Click += new System.EventHandler(this.btnNext_Click);
			}
		}

		/// <summary>
		/// Gets or Sets a button object which moves back in a wizard style form.
		/// </summary>
		[DefaultValue(null)]
		[Category("Actions")]
		public System.Windows.Forms.Button ActionBack
		{
			get
			{
				return btnBack;
			}
			set
			{
				if (btnBack != null)
					btnBack.Click -= new System.EventHandler(this.btnBack_Click);

				btnBack = value;
				
				if (value != null)
					btnBack.Click += new System.EventHandler(this.btnBack_Click);

			}
		}

		/// <summary>
		/// Gets or Sets a button object which finishes a wizard style form.
		/// </summary>
		[DefaultValue(null)]
		[Category("Actions")]
		public System.Windows.Forms.Button ActionFinish
		{
			get
			{
				return btnFinish;
			}
			set
			{
				if (btnFinish != null)
					btnFinish.Click -= new System.EventHandler(this.btnFinish_Click);

				btnFinish = value;
				
				if (value != null)
					btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
			}
		}

		private void FormPropertyChanged(object sender, FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs e)
		{
			if (_enq != null)
			{
				DataTable questions = _enq.Source.Tables["QUESTIONS"];

				DataView vw = new DataView(questions);
				vw.RowFilter = "quproperty = '" + e.Property.Replace("'", "''") + "'";
				if (vw.Count > 0)
				{
					foreach (DataRowView row in vw)
					{	
						IBasicEnquiryControl2 ecl = GetIBasicEnquiryControl2(Convert.ToString(row["quname"]));
						if (ecl != null)
						{
							bool oldisdirty = _isdirty;
							ecl.Value = e.Value;
							_isdirty = oldisdirty;
						}
					}
				}
			}
		}
		#endregion

		#region Internal Private Procedures

		/// <summary>
		/// Enables or disables a specified button.
		/// </summary>
		/// <param name="btn">Button object.</param>
		/// <param name="enabled">Enabled property value.</param>
		private void SetButtonEnabledProperty(Button btn, bool enabled)
		{
			if (btn != null) btn.Enabled = enabled;
		}

		/// <summary>
		/// Retrieves the information based on the field name.
		/// </summary>
		/// <param name="field">Field name to get the value of.</param>
		/// <returns>Returns a type aware table value.</returns>
		private object GetExtraInfo(string fieldName)
		{
            object val;
			if (_enq.Source.Tables["ENQUIRY"].Rows.Count > 0)
				val = _enq.Source.Tables["ENQUIRY"].Rows[0][fieldName];
			else
				val = null;

            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}
		
		/// <summary>
		/// Function to check if any of the required fields have a null or empty string value
		/// </summary>
		/// <returns>True if all required fields are populated</returns>
		public bool RequiredFieldsComplete()
		{
			bool retval = true;
			DataView dv = null;

			try
			{
				//get a reference to the data behind this form
				DataTable dt = _enq.Source.Tables["DATA"];

				//get a view of all the questions behind this form
				dv = new DataView(_questions);
				
				//filter to only required fields
				dv.RowFilter = "qurequired = true";
				
				foreach(DataRowView row in dv)
				{
					//get the name of the required field
					string fieldname = Convert.ToString(row["quname"]);
					
					//get the value from this field into an object datatype
					object val = dt.Rows[0][fieldname];

					//check if it is null or empty string
					if( val == DBNull.Value || Convert.ToString(val) == "")
					{
						//if so set flag and break out of loop
						retval = false;
						break;
					}
				}
			}
			catch(Exception ex){string s = ex.Message;}
			finally
			{
				if(dv != null)
					dv.Dispose();
			}
			return retval;
		}


		#endregion

		#region Rendering Methods

		/// <summary>
		/// Renders all enquiry question controls.
		/// </summary>
		public override void RenderControls()
		{
			//Initial Y padding from the top of the wizard.
			//For wizard styles, please ignore the x and y coordinates an automatically put one control after another.
			newY = (int)GetExtraInfo("enqPaddingY");  
						
			base.RenderControls();

			newY = 0;


		}

		/// <summary>
		/// Iterates through the whole data set and renders each of the control / question items
		/// by creating the controls from scratch.  It also specifies whether the controls collection
		/// is tyo be wiped first.
		/// </summary>
		/// <param name="clearExisting">Clears all exisisting controls when set to true.</param>
		public override void RenderControls(bool clearExisting)
		{
            long start = DateTime.Now.Ticks;
            if (RefreshingControls != null)
			{
				CancelEventArgs e = new CancelEventArgs();
				RefreshingControls(this,e);
				if (e.Cancel) return;
			}

            //Clear the filter controls collection.
            _filterControls.Clear();

			this.Visible=false;
			Script.EnquiryFormScriptType script = null;
			if (_enq.HasScript)
			{
				script = _enq.Script.Scriptlet as Script.EnquiryFormScriptType;
				if (script != null)
					script.SetEnquiryFormObject(this);
			}

			_questions = _enq.Source.Tables["QUESTIONS"];

			//Set the canvas size of the user control so that any anchored items can be shown.
			DockStyle dock = this.Dock;
			if (this.Style == EnquiryStyle.Standard)
				this.Dock = DockStyle.None;
            else if (this.Style == EnquiryStyle.Wizard && !_enq.InDesignMode && !this.ParentForm.TopLevel)
                SetWizardSize(!clearExisting);
			newY = (int)GetExtraInfo("enqPaddingY");  
			base.RenderControls(clearExisting,false);
			this.Dock = dock;

			if (clearExisting) ExecuteFilters();

			//
			this.BindingContext[_enq.Source.Tables["DATA"]].EndCurrentEdit();

			if (script != null)
				script.SetEnquiryFormObjectControls(this);
			this.Visible=true;

			if (RefreshedControls != null)
				RefreshedControls(this,EventArgs.Empty);
            Debug.WriteLine(String.Format("      RenderControls() Seconds : {0}", new TimeSpan(DateTime.Now.Ticks - start).TotalSeconds));
		}

		/// <summary>
		/// Renders a control per property setting.
		/// </summary>
		/// <param name="ctrl">Control to modify.</param>
		/// <param name="settings">Settings data row object, matching to the control.</param>
		/// <param name="property">Individual property to change.</param>
		public override void RenderControl (ref Control ctrl, DataRow settings, string property)
		{
			if (_questions.Columns.Contains(property))
			{
				switch(property.ToLower())
				{
					case "moveboat":
						ctrl.Anchor = AnchorStyles.None;

						int x = (int)settings["quX"];
						int y = (int)settings["quY"];
			
						ctrl.RightToLeft = this.RightToLeft;

                        // 3. Set Size
                        ctrl.Width = ConvertUnits(ConvertDef.ToInt32(settings["quwidth"], ctrl.Width));
                        ctrl.Height = ConvertUnits(ConvertDef.ToInt32(settings["quheight"], ctrl.Height));
                        
                        if (_style == EnquiryStyle.Wizard) 
						{
							if (settings["quwizx"] != DBNull.Value && settings["quwizy"] != DBNull.Value)
							{
                                // 2. Move Boat in Wizard
                                MoveBoat(ctrl, (int)settings["quwizx"], (int)settings["quwizy"]);
							}
							else
							{
								if (_pagename == Convert.ToString(settings["qupage"]))
								{
									if (settings["quwizx"] != DBNull.Value && settings["quwizy"] != DBNull.Value)
									{
										// 2. Move Boat in Wizard
										MoveBoat(ctrl,(int)settings["quwizx"],(int)settings["quwizy"]);
									}
									else
									{
										MoveBoat(ctrl,this.DockPadding.Left,newY);
										newY = newY + ConvertDef.ToInt32(settings["quheight"],22) + FWBS.Common.ConvertDef.ToInt32(_enq.Source.Tables["ENQUIRY"].Rows[0]["enqLeadingY"],0);
									}
								}
							}
						}
						else
						{
                            // 2. Move Boat
                            MoveBoat(ctrl, (int)settings["qux"], (int)settings["quY"]);
						}

						// Anchoring or Docking
                        SetAnchorOrDocking(ctrl, settings);
						break;
					case "quanchor":
						goto case "moveboat";
					case "quwidth":
						goto case "moveboat";
					case "quheight":
						goto case "moveboat";
					case "qux":
						goto case "moveboat";
					case "quy":
						goto case "moveboat";
					case "quwizx":
						goto case "moveboat";
					case "quwizy":
						goto case "moveboat";
					case "quhidden":
						if (_enq.InDesignMode)
						{
							//Still show item in design mode.
							if (_style == EnquiryStyle.Standard)
								ctrl.Visible = true;
						}
						else
						{
                            ctrl.Visible = !(bool)settings[property];
						}

						break;
					default:
						base.RenderControl(ref ctrl, settings, property);
						break;

				}
			}
		}

		/// <summary>
		/// Renders a specific control with a matching settings data row.  This overrides certain
		/// functionality that FormRendererBase has.  The control will be created if a null reference 
		/// is given to the function.
		/// </summary>
		/// <param name="ctrl">Reference to a control.</param>
		/// <param name="settings">Settings data row object.</param>
		public override void RenderControl(ref Control ctrl, DataRow settings)
		{
            long start = DateTime.Now.Ticks;
            // Added by DCT - 11/02/03 to stop the render in Wizard Mode if the control
			// is already created
			if (ctrl == null || _style == EnquiryStyle.Standard || _enq.InDesignMode)
				base.RenderControl(ref ctrl, settings);

			AnchorStyles anchor = ctrl.Anchor;

			#if NODATABIND
			ctrl.DataBindings.Clear();
			#endif
			string ctrlPage = Convert.ToString(settings["quPage"]);
			

			//If in wizard style then place one control after another using a leading value.
			if (_style == EnquiryStyle.Wizard)
			{
				ctrl.Visible = false;
				
				if(ctrl is IUsesRequiredStars)
				{
					((IUsesRequiredStars)ctrl).RequiredIconsOn(false);
					((IUsesRequiredStars)ctrl).ErrorIconsOn(false);
				}
				else
				{
					// Added by DCT - 11/02/03
                    if (_style == EnquiryStyle.Wizard)
                    {
                        req.SetError(ctrl, "");
                        // Added by DCT - 26/02/03 Oversite
                        err.SetError(ctrl, "");
                    }
				}

				RenderControl(ref ctrl,settings,"quWizX");
                ctrl.BringToFront(); // Added by DCT to fix the BringToFront problem of .NET 2
			}

			//*************************************************
			//IBasicEnquiryControl specific property settings.
			//*************************************************
			if (ctrl is IBasicEnquiryControl2)
			{
				IBasicEnquiryControl2 basic = (IBasicEnquiryControl2)ctrl;
		
				// Added by DCT - 11/02/03
				if (basic.Required && _style == EnquiryStyle.Wizard)
				{
					if(ctrl is IUsesRequiredStars)
					{
						((IUsesRequiredStars)ctrl).RequiredIconsOn(true);
						((IUsesRequiredStars)ctrl).ErrorIconsOn(false);
					}
					else
                    {
                        _reqFieldRenderer.MarkRequiredControl(ctrl);
                    }
				}	

				//Added by DJRM - 08/08/02 to make sure that the controls value has been set.
				//I had some issues with relying on the DataBindings to set the value of an
				//invisible control.
				if (_enq.Source.Tables["DATA"].Columns.Contains(ctrl.Name))
				{
					// Code modified by MNW 14/08/02 to comply with dbnull issue
					// Code modified by DCT 07/07/03 to Optimize the Setting of the Controls Value to be set in wizard mode 
					// only when the pages becomes visible
					try
					{
                        string s_page = Convert.ToString(settings["quPage"]);
                        int i_page;

                        if (_style == EnquiryStyle.Wizard && (s_page == this.PageName || (int.TryParse(s_page, out i_page) && i_page == this.PageNumber)))
						{
							if (_enq.Source.Tables["DATA"].Rows.Count > 0)
								basic.Value = _enq.Source.Tables["DATA"].Rows[0][ctrl.Name];
						}
						else if (_style != EnquiryStyle.Wizard) // If Not in Wizard Mode
						{
                            if (_enq.Source.Tables["DATA"].Rows.Count > 0)
                            {
                                basic.Value = _enq.Source.Tables["DATA"].Rows[0][ctrl.Name];
                                basic.IsDirty = false;
                            }
						}
					}
					catch
					{
						// Invalid Cast if not setup correct in database
					} 
				}

				if (!_enq.InDesignMode) 
				{
					#if NODATABIND
						ctrl.DataBindings.Add("Value", _enq.Source.Tables["DATA"], ctrl.Name);
					#else
						basic.Changed -= new EventHandler(DataBindingChanged);
						basic.Changed += new EventHandler(DataBindingChanged);
						basic.ActiveChanged -= new EventHandler(EnquiryForm_ValueChanged);
						basic.ActiveChanged += new EventHandler(EnquiryForm_ValueChanged);
					#endif

				}

				//If the control has a filter associated to it then capture the change 
				//event.
				if (Convert.ToString(settings["qufilter"]) != "<filters/>")
				{
					basic.ActiveChanged -= new EventHandler(this.FilterChanged);
					basic.ActiveChanged += new EventHandler(this.FilterChanged);

					//Add this filter control to a collection.
					_filterControls.Add(basic);
				}

				// Help System
				if (Convert.ToString(settings["quHelpKeyword"]) != "")
				{
					if (helpProvider1.HelpNamespace != "")
					{
						helpProvider1.SetHelpKeyword(ctrl,Convert.ToString(settings["quHelpKeyword"]));
						helpProvider1.SetHelpNavigator(ctrl,HelpNavigator.KeywordIndex);
						helpProvider1.SetShowHelp(ctrl,true);
					}
				}

			}	


			if (_style == EnquiryStyle.Wizard && (ctrlPage == _pagename || Convert.ToString(_page) == ctrlPage))
			{
				if ((bool)settings["quHidden"])
				{
					if (Enquiry.InDesignMode == false) ctrl.Visible = false;
				}
				else
				{
					ctrl.Visible = true;
					if (ctrl.TabStop)
					{
						if (_firstControl == null) _firstControl = ctrl;
					}
				}
			}
			else if (_style == EnquiryStyle.Standard)
			{
				if ((bool)settings["quHidden"])
				{
					if (Enquiry.InDesignMode == false) ctrl.Visible = false;
				}
				else
				{
                    ctrl.TabStop = !(settings["qutaborder"] == DBNull.Value);
					if (ctrl.TabStop)
					{
						if (_firstControl == null) _firstControl = ctrl;
					}

				}
			}

			if (this.InDesignMode == false)
			{
				try
				{
					string edtrole = Convert.ToString(settings["quEditableRole"]);
					string viwrole = Convert.ToString(settings["quVisibleRole"]);
					if (edtrole != "" || viwrole != "")
					{
						if (edtrole != "" && FWBS.OMS.Session.CurrentSession.CurrentUser.IsInRoles(edtrole)==false)
						{
							if (ctrl is IBasicEnquiryControl2)
								((IBasicEnquiryControl2)ctrl).ReadOnly = true;
							else
								ctrl.Enabled = false;
						}
						if (viwrole != "" && FWBS.OMS.Session.CurrentSession.CurrentUser.IsInRoles(viwrole)==false)
							ctrl.Visible = false;
					}
				}
				catch
				{}
			}
			
			ctrl.Anchor = anchor;
            Debug.WriteLine(String.Format("          RenderControl({1}) Seconds : {0}", new TimeSpan(DateTime.Now.Ticks - start).TotalSeconds, ctrl.Name));
		}

		#endregion

		#region Captured Event Actions


		/// <summary>
		/// Captures the cancel click event.
		/// </summary>
		/// <param name="sender">Cancel button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			CancelItem();
		}

		/// <summary>
		/// Captures the add click event.
		/// </summary>
		/// <param name="sender">Add button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void btnAddNew_Click(object sender, System.EventArgs e)
		{
			//WARNING: Add New Click not yet implemented exception.
			throw new OMSException2("ERRNOTIMPLEMENTED", "This feature is not yet implemented");
		}

		/// <summary>
		/// Captures the update click event.
		/// </summary>
		/// <param name="sender">Update button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			try
			{
				UpdateItem();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
		}

		/// <summary>
		/// Captures the help click event.
		/// </summary>
		/// <param name="sender">Help button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void btnHelp_Click(object sender, System.EventArgs e)
		{
			//WARNING: Help Click not yet implemented exception.
            throw new OMSException2("ERRNOTIMPLEMENTED", "This feature is not yet implemented");
        }

		/// <summary>
		/// Captures the next click event.
		/// </summary>
		/// <param name="sender">Next button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void btnNext_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (ParentForm == null)
					Cursor = Cursors.WaitCursor;
				else
					ParentForm.Cursor = Cursors.WaitCursor;
				NextPage();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
                if (ParentForm == null)
                    Cursor = Cursors.Default;
                else
                {
                    ParentForm.Cursor = Cursors.Default;
                }
			}
		}

		/// <summary>
		/// Captures the back click event.
		/// </summary>
		/// <param name="sender">Back button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void btnBack_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (ParentForm == null)
					Cursor = Cursors.WaitCursor;
				else
					ParentForm.Cursor = Cursors.WaitCursor;
				PreviousPage();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
				if (ParentForm == null)
					Cursor = Cursors.Default;
				else
					ParentForm.Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Captures the finish click event.
		/// </summary>
		/// <param name="sender">Finish button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void btnFinish_Click(object sender, System.EventArgs e)
		{
			System.ComponentModel.CancelEventArgs finishing = new System.ComponentModel.CancelEventArgs();
			OnFinishing(finishing);
			if (finishing.Cancel) return;
			try
			{
				UpdateItem();
				if (allowFinish)
				{
					OnFinished();
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}

		}

		/// <summary>
		/// Captures the command event on each of the enquiry controls which implement the ICommandEnquiryControl interface.
		/// This method will be passed information to execute a method in a specified object.
		/// </summary>
		/// <param name="sender">Calling control object.</param>
		/// <param name="e">Empty event arguments.</param>
		protected override void CommandCapture(object sender, EventArgs e)
		{
			if (this.Enquiry.InDesignMode) return;
			try
			{
				Cursor = Cursors.WaitCursor;
				Control ctrl = (Control)sender;
				DataView exec = new DataView(_questions);
				exec.RowFilter = "quname = '" + ctrl.Name.Replace("'", "''") + "'";

				if (exec.Count > 0)
				{
					string type = Convert.ToString(exec[0].Row["qucmdtype"]);
					string method = Convert.ToString(exec[0].Row["qucmdmethod"]);
					string parameters = Convert.ToString(exec[0].Row["qucmdparameters"]);
					bool retval = Convert.ToBoolean(exec[0].Row["qucommandretval"]);

					SourceEngine.StaticMethodSource cmd = new SourceEngine.StaticMethodSource(type, method, parameters);
					cmd.ParameterHandler = new SourceEngine.SetParameterHandler(this.CommandSetParameter);
					object ret = cmd.Run();

					//Refresh the list, if there is a list.
					if (ctrl is IListEnquiryControl)
					{
						if (ret != null)
							_enq.RefreshDataList(exec[0].Row);
					}

					//Set the value to the control which was returned from the command.
					if (retval)
					{
						if (sender is IBasicEnquiryControl2 && ret != null)
						{
							((IBasicEnquiryControl2)sender).Value = ret;
						}

					}

					//If the return value is an OMSType then give the parent OMS type dialog a chance
					//to deal with the actions.
					if (ret is FWBS.OMS.Interfaces.IOMSType)
					{
						OnNewOMSTypeWindow(new NewOMSTypeWindowEventArgs((FWBS.OMS.Interfaces.IOMSType)ret));
					}

				}


				exec.Dispose();
				exec = null;

			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}

		}

		/// <summary>
		/// An abstract method which must be implemented so that each parameter in the paramater
		/// list has its value populated.
		/// </summary>
		/// <param name="name">The name of the parameter being set.</param>
		/// <param name="value">The value to be returned for the parameter listing.</param>
		private void CommandSetParameter (string name, out object value)
		{
			if (name.ToUpper() == "THIS")
				value = _enq.Object;
			else
				value = _enq.Source.Tables["DATA"].Rows[0][name];
		}

		/// <summary>
		/// Reapply all Required Field Stars
		/// </summary>
		private void ReApplyRequiredFieldStars()
		{
			DataView _p = new DataView(_enq.Source.Tables["QUESTIONS"]);
			_p.RowFilter = "[quRequired] = true";
			foreach(DataRowView rw in _p)
			{
				Control ctrl = GetControl(Convert.ToString(rw["quName"]));
                if (ctrl is IUsesRequiredStars)
                {
                    ((IUsesRequiredStars)ctrl).ErrorIconsOn(false);
                    ((IUsesRequiredStars)ctrl).RequiredIconsOn(true);
                }
                else if (ctrl != null)
                {
                    _reqFieldRenderer.MarkRequiredControl(ctrl);
                }
            }

		}
		#endregion
		
		#region Public Functions
		/// <summary>
		/// Clears the Page Flow History and Any Next Or Previeous Buttons Returns to the Welcome Page
		/// </summary>
		public void ClearPageFlowHistory()
		{
			_pageflow.Clear();
			_pageflowinx = -1;
			_page =-2;
			if (_pages.Count >0)
				SetButtonEnabledProperty(btnNext,true);
		}

		/// <summary>
		/// Clears the Page History Forward of the Current Page
		/// </summary>
		public void ClearForwardHistory()
		{
			int current = Convert.ToInt32(_page)+2;
			if (current < _pageflow.Count)
				_pageflow.RemoveRange(current,_pageflow.Count - current);
		}

		/// <summary>
		/// Adds a Array of Pages to the Flow Array
		/// </summary>
		/// <param name="pages"></param>
		public void AddPageFlowRange(ArrayList pages, Int32 FlowIndex)
		{
			_pageflow.AddRange(pages);
			_pageflowinx = FlowIndex;

		}

		/// <summary>
		/// Loop through each control and fire the initial change event so that the filters can
		/// be applied.  This loop is needed at this point in time because all of the controls
		/// should have bee created.
		/// </summary>
		public void ExecuteFilters()
		{
			foreach (object ctrl in _filterControls)
			{
                 FilterChanged(ctrl, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Sets the dataset to accept a new record.
		/// </summary>
		private void AddNewData()
		{
			if (_enq == null) return;

			this.BindingContext[_enq.Source.Tables["DATA"]].EndCurrentEdit();
			this.BindingContext[_enq.Source.Tables["DATA"]].AddNew();
			
			SetButtonEnabledProperty(btnCancel, true);
			SetButtonEnabledProperty(btnAddNew, false);
			SetButtonEnabledProperty(btnUpdate, true);
		}


		/// <summary>
		/// Goes to the next page when in wizard format.
		/// </summary>
		public void NextPage()
		{
			GotoPage(Convert.ToInt16(_page+1),true);
		}

		/// <summary>
		/// Goes back to the previous page when in wizard format.
		/// </summary>
		public void PreviousPage()
		{
			_pageflowinx--;
			if (_pageflowinx < 0)
				_pageflowinx = 0;

				GotoPage(Convert.ToInt16(_pageflow[_pageflowinx]),true, true);
		}

		/// <summary>
		/// Jumps to the welcome page.
		/// </summary>
		public void GotoWelcomePage()
		{
			GotoPage((short)-1);
		}

		/// <summary>
		/// Goto Page with Control Name
		/// </summary>
		/// <param name="ControlName"></param>
		public void GotoControl(string ControlName)
		{
			GotoControl(ControlName,true);
		}
		
		/// <summary>
		/// Goto Page With Control Name
		/// </summary>
		/// <param name="ControlName">The Control Name to look for</param>
		/// <param name="UseBackHistory">If the Control is on a page fore the Current use History</param>
		public void GotoControl(string ControlName, bool UseBackHistory)
		{
			DataView dtv = new DataView(_enq.Source.Tables["QUESTIONS"],"quName = '" + ControlName + "'","",DataViewRowState.CurrentRows);
			if (dtv.Count > 0)
			{
				GotoPage(Convert.ToString(dtv[0]["quPage"]),true,UseBackHistory);
				if (GetControl(ControlName) != null)
                    GetControl(ControlName).Focus();
			}
		}

		/// <summary>
		/// Goto Page with Control with Field Name
		/// </summary>
		/// <param name="FieldName">The Fieldname to look for</param>
		public void GotoControlField(string FieldName)
		{
			GotoControlField(FieldName,true);
		}
			
		/// <summary>
		/// Goto Page With Control with Fieldname
		/// </summary>
		/// <param name="FieldName">The Fieldname to look for</param>
		/// <param name="UseBackHistory">If the control is on a page before the Current use the History</param>
		public void GotoControlField(string FieldName, bool UseBackHistory)
		{
			DataView dtv = new DataView(_enq.Source.Tables["QUESTIONS"],"quFieldName = '" + FieldName + "'","",DataViewRowState.CurrentRows);
			if (dtv.Count > 0)
			{
				GotoPage(Convert.ToString(dtv[0]["quPage"]),true,UseBackHistory);
			}

		}
	

		/// <summary>
		/// Goto To Page by Name and Use History
		/// </summary>
		/// <param name="PageName">Page Name</param>
		public void GotoPage(string PageName)
		{
			GotoPage(PageName,true,false);
		}
		
		/// <summary>
		/// Goto To Page by Name and Use History
		/// </summary>
		/// <param name="PageName">Page Name</param>
		/// <param name="raisePageChangingEvent">Raise Change Event</param>
		public void GotoPage(string PageName, bool raisePageChangingEvent)
		{
			GotoPage(PageName,raisePageChangingEvent,false);
		}
			
		/// <summary>
		/// Goto To Page by Name
		/// </summary>
		/// <param name="PageName">The Page Name</param>
		/// <param name="raisePageChangingEvent">Raise Change Event</param>
		/// <param name="UseBackHistory">Use History if the page is before the current</param>
		public void GotoPage(string PageName, bool raisePageChangingEvent, bool UseBackHistory)
		{
			GotoPage(PageName,raisePageChangingEvent,UseBackHistory,false);
		}

		/// <summary>
		/// Goto To Page by Name
		/// </summary>
		/// <param name="PageName">The Page Name</param>
		/// <param name="raisePageChangingEvent">Raise Change Event</param>
		/// <param name="UseBackHistory">Use History if the page is before the current</param>
		/// <param name="CreateForwardFlow">if the page if after well after the current page then give a option to create a flow</param>
		public void GotoPage(string PageName, bool raisePageChangingEvent, bool UseBackHistory, bool CreateForwardFlow)
		{
			for(int i = 0; i < _pages.Count; i++)
			{
				if (Convert.ToString(_pages[i]["pgeName"]).ToUpper() == PageName.ToUpper())
				{
					GotoPage(Convert.ToInt16(i),false,raisePageChangingEvent,UseBackHistory,CreateForwardFlow); // Internal
					break;
				}
			}
		}

		/// <summary>
		/// Jumps to a specified page of a wizard by the Page Number
		/// </summary>
		/// <param name="newPage">Page to be set to.</param>
		public void GotoPage(short newPage)
		{
			GotoPage(newPage, false,true,true,false);
		}

		/// <summary>
		/// Jumps to a specified page of a wizard by the Page Number
		/// </summary>
		/// <param name="newPage">Page to be set to.</param>
		/// <param name="raisePageChangingEvent">Raise Change Event</param>
		public void GotoPage(short newPage, bool raisePageChangingEvent)
		{
			GotoPage(newPage, false,raisePageChangingEvent,true,false);
		}

		/// <summary>
		/// Jumps to a specified page of a wizard by the Page Number
		/// </summary>
		/// <param name="newPage">Page to be set to.</param>
		/// <param name="raisePageChangingEvent">Raise Change Event</param>
		/// <param name="UseBackHistory">Use History if the page is before the current</param>
		/// <param name="CreateForwardFlow">Option to create forward history</param>
		public void GotoPage(short newPage, bool raisePageChangingEvent, bool UseBackHistory)
		{
			GotoPage(newPage, false,raisePageChangingEvent,UseBackHistory,false);
		}
		
		/// <summary>
		/// Jumps to a specified page of a wizard by the Page Number
		/// </summary>
		/// <param name="newPage">Page to be set to.</param>
		/// <param name="raisePageChangingEvent">Raise Change Event</param>
		/// <param name="UseBackHistory">Use History if the page is before the current</param>
		/// <param name="CreateForwardFlow">Option to create forward history</param>
		public void GotoPage(short newPage, bool raisePageChangingEvent, bool UseBackHistory, bool CreateForwardFlow)
		{
			GotoPage(newPage, false,raisePageChangingEvent,UseBackHistory,CreateForwardFlow);
		}

			
		/// <summary>
		/// Jumps to a specified page of a wizard.
		/// </summary>
		/// <param name="newPage">Page to be set to.</param>
		/// <param name="raisePageChangingEvent">Stops any PageChanging events from being raised that may causes a stack overflow.</param>
		/// <param name="back">The Directory to which the next page should be</param>
		/// <param name="UseBackHistory">Use the History if the page is before the current</param>
		/// <param name="CreateForwardFlow">Create the History of the missing Pages</param>
		internal void GotoPage(short newPage, bool back, bool raisePageChangingEvent, bool UseBackHistory, bool CreateForwardFlow)
		{
			if (_enq == null) return;

			if (_page != newPage)
			{
                if (this.VerticalScroll.Visible)
                    this.VerticalScroll.Value = 0;
                if (this.HorizontalScroll.Visible)
                    this.HorizontalScroll.Value = 0;

				bool custom = false;
				bool move = true;
				EnquiryPageDirection dir;

				if (newPage < _page && UseBackHistory)
					back = true;
				
				if (back) 
					dir = EnquiryPageDirection.Back;
				else
					dir = EnquiryPageDirection.Next;
				
				if (raisePageChangingEvent)
				{
					PageChangingEventArgs before = new PageChangingEventArgs(_pagename,_page, custom, dir, false); 
					OnPageChanging(before);
					if (before.Cancel) 
						return;
				}
				
				// if the new page is before the current and UseBackHistory = true then use the history to find the page
				if (newPage < _page && UseBackHistory)
				{
					for(int i = _pageflowinx; i > -1; i--)
						if (Convert.ToInt16(_pageflow[i]) == newPage)
						{
							_pageflowinx = i;
							break;
						}
				}
				else
				{
					if (CreateForwardFlow)
					{
						// If CreateForwardFlow then create the history for the back feature
						if (newPage > _page)
						{
							while(_page < newPage)
							{
								_page++;
								if (_pageflowinx > _pageflow.Count - 2)
								{
									_pageflow.Add(_page);
								}
								_pageflowinx++;
							}
						}
					}
					else
					{
						// Else simply add the new page to the History
						if (_pageflowinx > _pageflow.Count - 2)
						{
							_pageflow.Add(newPage);
						}

						_pageflowinx++;
					}
				}

				// If newPage < the Welcome Page then Goto Welcome Page
                if (newPage < -1)
				{
					custom = true;
					_page = -1;
					move = false;
				}
				else if (newPage > (_pages.Count -1)) // If new Page is Greater then Goto Last Page
				{
					_page = (short)(_pages.Count -1);
					move = false;
				}
				else // Else set the current page to the new page
				{
					_page = newPage;
                    _pagename = _page > -1 ? Convert.ToString(_pages[(int)_page].Row["pgeName"]) : "-1";

					if (_pagename == "") 
                        _pagename = _page.ToString();
				}
		
				if (!move) return; // If Move is false then exit here
			
				// if _page is greater than the welcome page then get details
				if (_page > -1)
				{
					object caption = _pages[(int)_page].Row["pgeDesc"];
					
                    if (lblPageHeader != null) 
					{
						lblPageHeader.Text = caption.ToString();
                        HandleFont(lblPageHeader);                        
					}
                    
                    if (picturePageHeader != null)
                    {
                        if (picturePageHeader.Tag == null)
                        {
                            picturePageHeader.Tag = picturePageHeader.Image;
                        }

                        if (_pages.Table.Columns.Contains("pgeSettings") && _pages[(int)_page].Row["pgeSettings"] != DBNull.Value)
                        {
                            FWBS.Common.ConfigSetting _xmlSetting = new ConfigSetting(_pages[(int)_page].Row, "pgeSettings");
                            string base64 = _xmlSetting.GetSetting("Page", "Image", "");
                            try
                            {
                                using (System.IO.MemoryStream reader = new System.IO.MemoryStream())
                                {
                                    byte[] buffer = Convert.FromBase64String(base64);
                                    reader.Write(buffer, 0, buffer.Length);
                                    reader.Position = 0;
                                    Image image = Image.FromStream(reader);
                                    picturePageHeader.Image = EnsurePageHeaderImageSize(image);
                                }
                            }
                            catch
                            {
                                picturePageHeader.Image = (Image)picturePageHeader.Tag;
                            }
                        }
                        else
                        {
                            picturePageHeader.Image = (Image)picturePageHeader.Tag;
                        }
                    }
					custom = (bool)_pages[(int)_page].Row["pgeCustom"];
				}

				RenderControls();

				if (_page == -1)
				{
					SetButtonEnabledProperty(btnNext, true);
					SetButtonEnabledProperty(btnBack, false);
				}
				else if (newPage == (_pages.Count -1))
				{
					SetButtonEnabledProperty(btnNext, false);
					SetButtonEnabledProperty(btnBack, true);
				}
				else
				{
					SetButtonEnabledProperty(btnNext, true);
					SetButtonEnabledProperty(btnBack, true);
				}

				if (_page > -1 && btnFinish != null && dir == EnquiryPageDirection.Next && btnFinish.Enabled == false && ConvertDef.ToBoolean(_pages[(int)_page].Row["pgeFinishedEnabled"],true))
					btnFinish.Enabled = true;

				if (ParentForm != null)
					if (btnNext != null && btnNext.Enabled == false)
					{
						if (btnFinish != null) ParentForm.AcceptButton = btnFinish;
					}
					else
					{
						if (btnNext != null) ParentForm.AcceptButton = btnNext;
					}
				
				//Raise the after page change event.
				PageChangedEventArgs after = new PageChangedEventArgs(_pagename, _page, custom, dir);
				OnPageChanged(after);
				if (_firstControl != null)
				{
					_firstControl.Focus();
				}
			}

		}


		/// <summary>
		/// Sets the Next Page Number when the Next Page Methods is Fired
		/// </summary>
		/// <param name="newPage">The New Page to Goto</param>
		public void SetNextPage(short newPage)
		{
			// The User sets the new Page Number we set it minus one so the
			// NextPage Function can increment it.
			_page = Convert.ToInt16(Convert.ToInt32(newPage) - 1);
			if (_pageflow.Count -1 > _pageflowinx)
				_pageflow.RemoveRange(_pageflowinx + 1,(_pageflow.Count - _pageflowinx)-1);
		}

		/// <summary>
		/// Sets the Next Page by Name when the Next Page Methods is Fired
		/// </summary>
		/// <param name="newPage">The New Page Name to Goto</param>
		public void SetNextPage(string NextPage)
		{
			// The User sets the new Page Number we set it minus one so the
			// NextPage Function can increment it.
			for(int i = 0; i < _pages.Count; i++)
			{
				if (Convert.ToString(_pages[i]["pgeName"]).ToUpper() == NextPage.ToUpper())
				{
					_page = Convert.ToInt16(i - 1);
					if (_pageflow.Count -1 > _pageflowinx)
						_pageflow.RemoveRange(_pageflowinx + 1,(_pageflow.Count - _pageflowinx)-1);				
					break;
				}
			}
		}

		public void RequiredControl(string Name,bool Enabled)
		{
			DataRow dr = this.GetSettings(Name);
			dr["quRequired"] = Enabled;
			Control c = GetControl(Name);
			RenderControl(ref c,dr,"quRequired");
		}

		/// <summary>
		/// Passes the focus to another control to update the underlying data source.
		/// The focus will return then be returned back to the original active control.
		/// </summary>
		public override void ReBind()
		{
			try
			{
				base.ReBind();
				
				#if !NODATABIND
				
					foreach (Control ctrl in this.Controls)
					{
						if (ctrl is IBasicEnquiryControl2)
						{
							try
							{
								_enq.SetValue(ctrl.Name,((IBasicEnquiryControl2)ctrl).Value);
							}
							catch
							{}
						}
					}
				#endif
			}
			catch {}
		}

		/// <summary>
		/// Sets user controls size to the size of the enquiry canvas.  The method does nothing
		/// if there is no enquiry form set.  NO null reference is raised.
		/// </summary>
		public void SetCanvasSize()
		{
			if (_enq != null)
			{
				this.Size = new Size((int)GetExtraInfo("enqCanvasWidth"), (int)GetExtraInfo("enqCanvasHeight"));
			}
		}

		/// <summary>
		/// Gets user controls size to the size of the enquiry canvas.  The method does nothing
		/// if there is no enquiry form set.  NO null reference is raised.
		/// </summary>
		public Size GetCanvasSize()
		{
			if (_enq != null)
			{
				return new Size((int)GetExtraInfo("enqCanvasWidth"), (int)GetExtraInfo("enqCanvasHeight"));
			}
			else
				return Size.Empty;
		}


		/// <summary>
		/// Sets the parent forms size to the size of the enquiry canvas.  The method does nothing
		/// if there is no enquiry form set.  NO null reference is raised.
		/// </summary>
		public void SetParentSize()
		{
			if (!_enq.InDesignMode)
			{
				if (_enq != null && ParentForm != null && _changeFormSize)
				{
					Size clientSize;
					if (Style == EnquiryStyle.Standard)
						clientSize = LogicalToDeviceUnits(new Size((int)GetExtraInfo("enqCanvasWidth"), (int)GetExtraInfo("enqCanvasHeight")));
					else
						clientSize = LogicalToDeviceUnits(new Size((int)GetExtraInfo("enqWizardWidth"), (int)GetExtraInfo("enqWizardHeight")));
					clientSize += new Size(this.ParentForm.Padding.Horizontal, this.ParentForm.Padding.Vertical);
					// make the parent form fit on the screen
					var workingArea = System.Windows.Forms.Screen.FromControl(this.ParentForm).WorkingArea;
					var newSize = new Size(
						System.Math.Min(clientSize.Width, workingArea.Width),
						System.Math.Min(clientSize.Height, workingArea.Height));
					this.ParentForm.ClientSize = newSize;
				}
			}
		}

        private void SetWizardSize(bool scaled)
        {
            Size size = new Size((int)GetExtraInfo("enqWizardWidth"), (int)GetExtraInfo("enqWizardHeight"));
            this.Dock = DockStyle.None;
            this.Size = scaled ? LogicalToDeviceUnits(size) : size;
        }

		#endregion
		
		#region Enquiry Form Events
		/// <summary>
		/// Redraw the controls if the right to left setting has changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EnquiryForm_RightToLeftChanged(object sender, System.EventArgs e)
		{
			if (_enq != null)
				RenderControls();
		}

		/// <summary>
		/// Captures each time the data changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DataChanged(object sender, System.EventArgs e)
		{
		
			this.SuspendLayout();
			#if NODATABIND
				foreach (Control ctrl in this.Controls)
				{
					if (ctrl is IBasicEnquiryControl2 && _enq.InDesignMode==false)
					{
						ctrl.DataBindings.Clear();
						ctrl.DataBindings.Add( new Binding("Value", _enq.Source.Tables["DATA"], ctrl.Name));
					}
				}
			#endif

			this.ResumeLayout();


		}

		/// <summary>
		/// Captures the update event when the enquiry has succeefully been updated.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EnqUpdated(object sender, System.EventArgs e)
		{
			this.SuspendLayout();
			#if NODATABIND
				foreach (Control ctrl in this.Controls)
				{
					if (ctrl is IBasicEnquiryControl2)
					{
						ctrl.DataBindings.Clear();
						ctrl.DataBindings.Add( new Binding("Value", _enq.Source.Tables["DATA"], ctrl.Name));
					}
				}
			#endif
			this.ResumeLayout();
		}


		/// <summary>
		/// Captures the refresh event of the enquiry.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Refreshed(object sender, System.EventArgs e)
		{
            //MAkes sure that the _lists field is still accessing the same enquiry source.
            if (_enq != null)
                _lists = _enq.Source;

			#if !NODATABIND
				foreach (Control ctrl in this.Controls)
				{
					if (ctrl is IBasicEnquiryControl2)
					{
						if (_enq.Source.Tables["DATA"].Columns.Contains(ctrl.Name))
						{
							// Code modified by MNW 14/08/02 to comply with dbnull issue
							try
							{
								if (_enq.Source.Tables["DATA"].Rows.Count > 0)
									((IBasicEnquiryControl2)ctrl).Value = _enq.Source.Tables["DATA"].Rows[0][ctrl.Name];
							}
							catch
							{
							} 
						}
					}
				}
			#endif
		}

		
		/// <summary>
		/// Captures the mode changed event of the enquiry, this may mean a refresh in schema.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ModeChanged(object sender, System.EventArgs e)
		{
			RenderControls();
		}

		/// <summary>
		/// The None Data Binded Mode
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DataBindingChanged(object sender, EventArgs e)
		{
			try
			{
                // 10/12/2009 DCT - Happens when the Associate UI jumps to the Contact 
                // and the Salutation on the Associate is Null it fires to early.
                if (_enq == null || _enq.Source == null || _enq.Source.Tables["DATA"] == null)
                    return;

				if (sender is IBasicEnquiryControl2 && sender is Control)
				{ 
					// The Control Name and thus Field Name
					string ctrlname = ((Control)sender).Name;
					// The Type of Stored Data
					Type ctype = _enq.Source.Tables["DATA"].Columns[ctrlname].DataType;
					// The Controls Data
					object rawvalue = ((IBasicEnquiryControl2)sender).Value;
					// The Stored Data
					
					if (_enq.Source.Tables["DATA"].Rows.Count == 0 || _enq.Source.Tables["DATA"].Rows[0].RowState == DataRowState.Deleted) return;

					object rawbvalue = _enq.Source.Tables["DATA"].Rows[0][ctrlname];
					
					object myvalue = DBNull.Value;
					object bvalue = null;
					if (rawvalue != null)
					{
						//
						// Get the New Value converted to the Expected Type
						//
						try
						{
							if (rawvalue.GetType() == ctype || rawvalue == DBNull.Value) 
								myvalue = rawvalue; // If the Type is a Match
							else if (ctype == typeof(FWBS.Common.DateTimeNULL))
								myvalue = FWBS.Common.ConvertDef.ToDateTimeNULL(rawvalue,DBNull.Value); // If DateTimeNull
							else if (ctype == typeof(Int64))
								myvalue = FWBS.Common.ConvertDef.ToInt64(rawvalue,0); // If Int64
							else if (ctype == typeof(Int32))
								myvalue = FWBS.Common.ConvertDef.ToInt32(rawvalue,0); // If Int32
							else if (ctype == typeof(Int16))
								myvalue = FWBS.Common.ConvertDef.ToInt16(rawvalue,0); // If Int16
							else if (ctype == typeof(Decimal))
								myvalue = FWBS.Common.ConvertDef.ToDecimal(rawvalue,0); // If Decimal
							else if (ctype == typeof(Double))
								myvalue = FWBS.Common.ConvertDef.ToDouble(rawvalue,0); // If Double
							else if (ctype == typeof(Boolean))
								myvalue = FWBS.Common.ConvertDef.ToBoolean(rawvalue,false); // If Boolean
							else
								myvalue = Convert.ChangeType(rawvalue,ctype); // Convert to that Type
						}
						catch{}

						//
						// Get the Original Value converted to the Expected Type
						//
						try
						{
							if (rawbvalue.GetType() == ctype || rawbvalue == DBNull.Value) 
								bvalue = rawbvalue; // If the Type is a Match
							else if (ctype == typeof(FWBS.Common.DateTimeNULL))
								bvalue = FWBS.Common.ConvertDef.ToDateTimeNULL(rawbvalue,DBNull.Value); // If DateTimeNull
							else if (ctype == typeof(Int64))
								bvalue = FWBS.Common.ConvertDef.ToInt64(rawbvalue,0); // If Int64
							else if (ctype == typeof(Int32))
								bvalue = FWBS.Common.ConvertDef.ToInt32(rawbvalue,0); // If Int32
							else if (ctype == typeof(Int16))
								bvalue = FWBS.Common.ConvertDef.ToInt16(rawbvalue,0); // If Int16
							else if (ctype == typeof(Decimal))
								bvalue = FWBS.Common.ConvertDef.ToDecimal(rawbvalue,0); // If Decimal
							else if (ctype == typeof(Double))
								bvalue = FWBS.Common.ConvertDef.ToDouble(rawbvalue,0); // If Double
							else if (ctype == typeof(Boolean))
								bvalue = FWBS.Common.ConvertDef.ToBoolean(rawbvalue,false); // If Boolean
							else
								bvalue = Convert.ChangeType(rawbvalue,ctype); // Convert to that Type
						}
						catch{}
				
						bool match = false;
						// Compare the Two values
                        if (myvalue != null)
                        {
                            //UTCFIX: DM - 01/12/06 - Make sure date values are compared with same kind.
                            if (myvalue is DateTime && bvalue is DateTime)
                                match = ((DateTime)myvalue).ToLocalTime().Equals(((DateTime)bvalue).ToLocalTime());
                            else
                                match = (myvalue.Equals(bvalue));
                        }

						// If No Match then Update
						if (!match)
						{
							if (myvalue == null && GetSettings(ctrlname)["quProperty"] == DBNull.Value)
								_enq.SetValue(ctrlname,DBNull.Value);
							else
								_enq.SetValue(ctrlname,myvalue);
							this.IsDirty=true;
							OnDirty();
						}
					}
				}
			}
			catch(Exception ex)
			{
				 ErrorBox.Show(ParentForm, ex);
			}
		}


		/// <summary>
		/// Captures the change event of controls with filters specified.
		/// </summary>
		/// <param name="sender">The control who's value is changed.</param>
		/// <param name="e">Empty event arguments.</param>
		private void FilterChanged (object sender, System.EventArgs e)
		{
			if (sender is IBasicEnquiryControl2)
			{
				IBasicEnquiryControl2 basic = (IBasicEnquiryControl2)sender;
				DataRow settings = GetSettings((Control)basic);
				if (settings != null)
				{
					ConfigSetting cfg = new ConfigSetting(Convert.ToString(settings["qufilter"]));
				
					//Count the number of parameters to be used.
					cfg.Current = "filters";
					FWBS.Common.ConfigSettingItem [] filters = cfg.CurrentChildItems;
					int cnt = filters.Length;

					//Loop through each of the parameters,
					for (int ctr = 0; ctr < cnt; ctr++)
					{
						//Get the name of the control to filter.
						string control = filters[ctr].GetString("control", "");
						//Get the field name of the result set within the control being filtered.
						string fieldName = filters[ctr].GetString("fieldName", ""); 			

						//Find the control to be filtered then filter it.
						Control ctrl = GetControl(control);
						if (ctrl is IListEnquiryControl && control.ToUpper() != ((Control)sender).Name.ToUpper())
						{
							object n = ((IBasicEnquiryControl2)ctrl).Value;
							
							((IListEnquiryControl)ctrl).Filter(fieldName, basic.Value);
							
							try
							{
                                if (Convert.ToString(((IBasicEnquiryControl2)ctrl).Value) != Convert.ToString(n))
                                    ((IBasicEnquiryControl2)ctrl).Value = n;
							}
							catch
							{
							}
						}
					}
				}
			}
		}

		#endregion

		#region Event Methods

		protected void OnEnquiryLoaded()
		{
            if (EnquiryLoaded != null)
				EnquiryLoaded(this,EventArgs.Empty);
		}

		/// <summary>
		/// Raises the OnNewOMSTypeWindow event with the specified event arguments.
		/// </summary>
		/// <param name="e">NewOMSTypeWindow Event arguments.</param>
		public void OnNewOMSTypeWindow(NewOMSTypeWindowEventArgs e)
		{
			if (NewOMSTypeWindow != null)
			{
				NewOMSTypeWindow(this, e);
				if (this.IsDirty == false)
                    this.RefreshItem();
			}
		}

		/// <summary>
		/// Raises the PageChanging event with the specified event arguments.
		/// </summary>
		/// <param name="e">PageChanging Event arguments.</param>
		protected void OnPageChanging(PageChangingEventArgs e)
		{
			if (PageChanging != null)
				PageChanging(this, e);
		}

		/// <summary>
		/// Raises the PageChanged event with the specified event arguments.
		/// </summary>
		/// <param name="e">PageChanged Event arguments.</param>
		protected void OnPageChanged(PageChangedEventArgs e)
		{
			if (PageChanged != null)
				PageChanged(this, e);
		}

		/// <summary>
		/// Raises the Finished event with the specified event arguments.
		/// </summary>
		/// <param name="e">Finished Event arguments.</param>
		protected void OnFinished()
		{
			if (Finished != null)
				InvokeEventHandler(Finished, EventArgs.Empty);
		}

		
		/// <summary>
		/// Raises the Finishing event with the specified event arguments.
		/// </summary>
		/// <param name="e">Finishing Event arguments.</param>
		protected void OnFinishing(CancelEventArgs e)
		{
			if (Finishing != null)
				InvokeEventHandler(Finishing, e);
		}

        private void InvokeEventHandler(Delegate eventHandler, EventArgs args)
        {
            foreach (Delegate d in eventHandler.GetInvocationList().OrderBy(d => (d.Target is OMS.Script.ScriptType) ? 0 : ((d.Target is Form) ? 1 : -1)))
            {
                d.DynamicInvoke(this, args);
            }
        }

		/// <summary>
		/// Raises the Cancelled event with the specified event arguments.
		/// </summary>
		protected void OnCancelled()
		{
			if (Cancelled != null)
				Cancelled(this, EventArgs.Empty);
		}


		protected void OnEnquiryPropertyChanged()
		{
			if (EnquiryPropertyChanged != null)
				EnquiryPropertyChanged(this, EventArgs.Empty);
		}

		protected void OnDirty()
		{
			if (Dirty != null)
				Dirty(this,EventArgs.Empty);
		}
		#endregion

		#region IOMSItem Implementation

		/// <summary>
		/// IOMSItem Member: Refreshes the data within this object.
		/// </summary>
		public void RefreshItem()
		{
			if (_enq == null) return;

			try
			{
                Cursor = Cursors.WaitCursor;
				_enq.Refresh();
				this.IsDirty = false;
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		public void ShowMissingField(Control ctrl, string message)
		{
			if(ctrl is IUsesRequiredStars)
			{}
			else
			{
				if (this.RightToLeft == RightToLeft.Yes)
				{
					err.SetIconAlignment(ctrl, ErrorIconAlignment.MiddleLeft);
					req.SetIconAlignment(ctrl, ErrorIconAlignment.MiddleLeft);
				}
				else
				{
					err.SetIconAlignment(ctrl, ErrorIconAlignment.MiddleRight);
					req.SetIconAlignment(ctrl, ErrorIconAlignment.MiddleRight);
				}
			}
			if (_style == EnquiryStyle.Wizard)
			{
				GotoControl(ctrl.Name);
				if(ctrl is IUsesRequiredStars)
				{
					((IUsesRequiredStars)ctrl).ErrorIconsOn(true);
					((IUsesRequiredStars)ctrl).RequiredIconsOn(false);
				}
				else
				{
					err.SetError(ctrl, message);
                    req.SetError(ctrl, "");
                }
				
				ctrl.Focus();
			}					
			else
			{
				if(ctrl is IUsesRequiredStars)
				{
					((IUsesRequiredStars)ctrl).ErrorIconsOn(true);
					((IUsesRequiredStars)ctrl).RequiredIconsOn(false);
				}
				else
				{	
					err.SetError(ctrl,message);
                    req.SetError(ctrl, "");
                }
				ctrl.Focus();
			}
		}

		/// <summary>
		/// IOMSItem Member: Updates the data within this object.
		/// </summary>
		public void UpdateItem()
		{
			if (_enq == null) return;

			CancelEventArgs _cancel = new CancelEventArgs();
			if (Updating != null) Updating(this,_cancel);
			if (_cancel.Cancel) return; 


			#if NODATABIND
				//
				// Update any changes the control may make to the Data
				// added by Danny
				// 
				foreach (Control ctrl in this.Controls)
				{
					if (ctrl is IBasicEnquiryControl2)
					{
						try
						{
							_enq.SetValue(ctrl.Name,((IBasicEnquiryControl2)ctrl).Value);
						}
						catch
						{}
					}
				}
			#endif

            try
            {
                Cursor = Cursors.WaitCursor;
                this.BindingContext[_enq.Source.Tables["DATA"]].EndCurrentEdit();
                _enq.Update();
                allowFinish = true;
                if (this.ParentForm != null && _style == EnquiryStyle.Wizard)
                    this.ParentForm.DialogResult = DialogResult.OK;

                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is IUsesRequiredStars)
                        ((IUsesRequiredStars)ctrl).ErrorIconsOn(false);
                    else
                        err.SetError(ctrl, "");
                }

                ReApplyRequiredFieldStars();
                this.IsDirty = false;
                if (Updated != null)
                    Updated(this, EventArgs.Empty);
            }
            catch (EnquiryValidationFieldException ex)
            {
                //If a validation error occurs find the control that failed the validation
                //and mark it with the error provider, again taking into account 
                //the RightToLeft setting.
                allowFinish = false;

                Control ctrl = null;
                ctrl = GetControl(ex.ValidatedField.FieldName);

                ShowMissingField(ctrl, ex.Message);

                if (_enq.Source.Tables["DATA"].Rows.Count > 0) _enq.Source.Tables["DATA"].Rows[0].BeginEdit();

                if (((EnquiryValidationFieldException)ex).ValidatedField.Description != "")
                {
                    if (this.Parent != null && this.Parent.Parent != null)
                    {
                        var typeTabs = this.Parent.Parent.Parent as ucOMSTypeTabs;
                        if (typeTabs != null)
                        {
                            typeTabs.IsDirty = true;
                            typeTabs.OnActiveChanged();
                            typeTabs.OnChanged();
                            throw new EnquiryValidationFieldException(ex.HelpID, new ValidatedField(typeTabs.Name, ex.ValidatedField.Description, ex.ValidatedField.Page));
                        }
                        else
                        {
                            throw;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            catch (UpdateCancelledException)
            {
                allowFinish = false;
                if (_enq.Source.Tables["DATA"].Rows.Count > 0) _enq.Source.Tables["DATA"].Rows[0].BeginEdit();
            }
            catch (Exception ex)
            {
                var ap = ex as IAssociatedEnquiryPage;

                allowFinish = false;
                if (_enq.Source.Tables["DATA"].Rows.Count > 0) _enq.Source.Tables["DATA"].Rows[0].BeginEdit();
                //INFO: ErrorBox.Show(ex);
                if (ap != null)
                    GotoPage(ap.PageName);

                throw;
            }
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		
		/// <summary>
		/// IOMSItem Member: Cancels the data within this object.
		/// </summary>
		public void CancelItem()
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				if (_enq == null) return;
				_enq.Cancel();
				OnCancelled();
				foreach (Control ctrl in Controls)
				{
					if(ctrl is IUsesRequiredStars)
						((IUsesRequiredStars)ctrl).ErrorIconsOn(false);
					else
						err.SetError(ctrl,"");
				}

                ReApplyRequiredFieldStars();
				SetButtonEnabledProperty(btnCancel, false);
				SetButtonEnabledProperty(btnAddNew, true);
				SetButtonEnabledProperty(btnUpdate, false);
			}
			finally
			{
				Cursor = Cursors.Default;
				this.IsDirty=false;
			}
		}

		
		/// <summary>
		/// IOMSItem Member: Called when the tab that this object sits on is clicked upon 
		/// from the OMS type dialog.
		/// </summary>
		public virtual void SelectItem()
		{
		}

		
		private void EnquiryForm_ValueChanged(object sender, System.EventArgs e)
		{
			this.IsDirty = true;
			OnDirty();
		}

		public Control FirstControl
		{
			get
			{
				if (_firstControl == null)
					return this;
				else
					return _firstControl;
			}
		}


		/// <summary>
		/// IOMSItem Member: Gets a boolean value that indicates whether this class is holding any
		/// unsaved dirty data.
		/// </summary>
		[Browsable(false)]
		public bool IsDirty
		{
			get
			{
				if (_isdirty) 
					return true;
				// DMB 24/02/2004 changed to check all required fields are filled in
				// If not return false to force validation of the form
				// Unfortunately this would force an update of the data when it is not needed
				// On the plus side it should not happen very often only when a form has changed or
				// incomplete data has been imported 
				else if(_enq != null && RequiredFieldsComplete() == false)
					return true;
				else if (_enq != null && _enq.Object is FWBS.OMS.Interfaces.IUpdateable)
					return (_enq.Object as FWBS.OMS.Interfaces.IUpdateable).IsDirty;
				else
					return false;
			}
			set
			{
				_isdirty = value;
			}
		}

		#endregion

		#region Diagnostics
		private void mnuDump_Click(object sender, System.EventArgs e)
		{
			FWBS.OMS.UI.Windows.Design.frmGrid frm = new FWBS.OMS.UI.Windows.Design.frmGrid(Enquiry.Source);
			frm.Show();
		}

		private void mnuRefreshRebind_Click(object sender, System.EventArgs e)
		{
			this.RenderControls(true);
		}

		private void Diagnostics_Popup(object sender, System.EventArgs e)
		{
			try
			{
				mnuSize.Text = this.Size.ToString();
				mnuName.Text = "[" + this.Enquiry.Code + "] " +  Enquiry.EnquiryName;
			}
			catch
			{}
		}

		private void mnuToolTips_Click(object sender, System.EventArgs e)
		{
			this.tp.SetToolTip(this,this.Enquiry.Code + Environment.NewLine + "Local : " + this.Enquiry.Local.ToString() + Environment.NewLine + "Version : " + this.Enquiry.Version.ToString() + Environment.NewLine + "Binding : " + this.Enquiry.Binding.ToString() + Environment.NewLine + "Mode : " + this.Enquiry.Mode + Environment.NewLine + "Offline: " + this.Enquiry.Offline.ToString());

			foreach (DataRow dr in this.Enquiry.Source.Tables["QUESTIONS"].Rows)
			{
				foreach (Control existing in this.Controls)
				{
					if (existing.Name == dr["quname"].ToString())
					{
						this.tp.SetToolTip(existing,"Control Name: " + existing.Name + Environment.NewLine + "Location : (" + existing.Left.ToString() + "," + existing.Top.ToString() + "," + existing.Width.ToString() + "," + existing.Height.ToString() + ")" + Environment.NewLine + "Control Type Name : " + existing.GetType().ToString() + Environment.NewLine + "Code Lookup Code: " + dr["QUCODE"].ToString() + Environment.NewLine + "Field Name : " + dr["quFieldName"] + Environment.NewLine + "Table : " + dr["quTable"] + Environment.NewLine + "Property : " + dr["quProperty"] + Environment.NewLine + "Field Type : " + dr["quType"] + Environment.NewLine + "DataList : " + dr["quDataList"] + Environment.NewLine + "Tab Stop : " + existing.TabStop.ToString());
						break;
					}
				}	
			}
			this.tp.AutomaticDelay = 0;
		}
		#endregion

		private void EnquiryForm_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ApplicationSetting debugset = new ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Debug", "Enabled", "false");
			bool debug = debugset.ToBoolean();

			if (e.Button == MouseButtons.Right && _enq != null)
			{
				if (Session.CurrentSession.CurrentUser.IsInRoles("ADMIN") || debug)
				{
					Diagnostics.Show(this,new Point(e.X - 15,e.Y - 15));
				}
			}
		}

		private void EnquiryForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (_enq != null)
			{
				if (_enq.InDesignMode == true)
					ControlPaint.DrawGrid(e.Graphics, e.ClipRectangle, new Size(5, 5), this.BackColor);
			}
		}

        public bool IsObjectDirty()
        {
            return this.IsDirty;
        }
        
        public bool IsFormDirty
        {
            get
            {
                return _isdirty;
            }
        }
    }
}
