using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucContactManager.
    /// </summary>
    public class ucSearchManager : System.Windows.Forms.UserControl
	{
        /// <summary>
        /// An event that gets raised when a new OMS type object needs to be opened in
        /// a navigational format on the dialog form.
        /// </summary>
        public event NewOMSTypeWindowEventHandler NewOMSTypeWindow = null;
        
        #region Auto Fields
		private System.Windows.Forms.Panel pnlHeading;
		private System.Windows.Forms.Label labHeading;
		private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlBack;
        private System.Windows.Forms.Panel pnlMain;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Label labSearchWhat;
		private System.Windows.Forms.ComboBox cmbSearchFor;
		private FWBS.OMS.UI.Windows.EnquiryForm enqSearch;
		private System.Windows.Forms.LinkLabel lnkCreateNew;
		protected FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.Windows.Forms.Panel pnlBorder;
		private System.ComponentModel.IContainer components;
        private omsSplitter splitter1;
        private Panel pnlMangerParent;
        private ucSearchControl ucSearchControl1;
        private Panel pnlButtons;
        #endregion


        #region Fields
        private int _lastsearchforindex = -1;
		#endregion

		#region Contructors
		public ucSearchManager()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            if (Session.CurrentSession.IsLoggedIn)
            {
                cmbSearchFor.Items.Add(Session.CurrentSession.Resources.GetResource("CONTACT", "Contact", "").Text);
                cmbSearchFor.Items.Add(Session.CurrentSession.Terminology.Parse("%CLIENT%", true));
                cmbSearchFor.Items.Add(Session.CurrentSession.Terminology.Parse("%FILE%", true));
            }
            SetOfficeStyle();
		}

        private void SetOfficeStyle()
        {
            this.btnClose.Image = global::FWBS.OMS.UI.Properties.Resources.BlackColapse;
            this.pnlHeading.BorderStyle = BorderStyle.None;
            this.BackgroundImage = pnlBack.BackgroundImage;
            this.pnlMain.BackColor = new ExtColor(ExtColorPresets.TaskPainBackColor, ExtColorTheme.Auto).Color;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.pnlBack.BackgroundImageLayout = ImageLayout.Stretch;
            this.pnlMain.BackgroundImageLayout = ImageLayout.Stretch;
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
            this.components = new System.ComponentModel.Container();
            this.pnlHeading = new System.Windows.Forms.Panel();
            this.labHeading = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlBack = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.enqSearch = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.cmbSearchFor = new System.Windows.Forms.ComboBox();
            this.labSearchWhat = new System.Windows.Forms.Label();
            this.pnlBorder = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.lnkCreateNew = new System.Windows.Forms.LinkLabel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.pnlMangerParent = new System.Windows.Forms.Panel();
            this.ucSearchControl1 = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.splitter1 = new FWBS.OMS.UI.Windows.omsSplitter();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlHeading.SuspendLayout();
            this.pnlBack.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.pnlMangerParent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeading
            // 
            this.pnlHeading.BackColor = System.Drawing.Color.White;
            this.pnlHeading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlHeading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeading.Controls.Add(this.labHeading);
            this.pnlHeading.Controls.Add(this.btnClose);
            this.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeading.Location = new System.Drawing.Point(0, 0);
            this.pnlHeading.Name = "pnlHeading";
            this.pnlHeading.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.pnlHeading.Size = new System.Drawing.Size(250, 24);
            this.pnlHeading.TabIndex = 0;
            // 
            // labHeading
            // 
            this.labHeading.BackColor = System.Drawing.Color.Transparent;
            this.labHeading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labHeading.Location = new System.Drawing.Point(5, 4);
            this.resourceLookup1.SetLookup(this.labHeading, new FWBS.OMS.UI.Windows.ResourceLookupItem("SMHEADING", "Search Manager", ""));
            this.labHeading.Name = "labHeading";
            this.labHeading.Size = new System.Drawing.Size(220, 14);
            this.labHeading.TabIndex = 0;
            this.labHeading.Text = "Search Manager";
            this.labHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LightGray;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Image = global::FWBS.OMS.UI.Properties.Resources.BlackColapse;
            this.btnClose.Location = new System.Drawing.Point(225, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(18, 14);
            this.btnClose.TabIndex = 4;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlBack
            // 
            this.pnlBack.BackColor = System.Drawing.Color.White;
            this.pnlBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBack.Controls.Add(this.pnlMain);
            this.pnlBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBack.Location = new System.Drawing.Point(0, 24);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Padding = new System.Windows.Forms.Padding(7);
            this.pnlBack.Size = new System.Drawing.Size(250, 400);
            this.pnlBack.TabIndex = 0;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Controls.Add(this.enqSearch);
            this.pnlMain.Controls.Add(this.cmbSearchFor);
            this.pnlMain.Controls.Add(this.labSearchWhat);
            this.pnlMain.Controls.Add(this.pnlBorder);
            this.pnlMain.Controls.Add(this.pnlButtons);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(7, 7);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(10);
            this.pnlMain.Size = new System.Drawing.Size(234, 384);
            this.pnlMain.TabIndex = 0;
            // 
            // enqSearch
            // 
            this.enqSearch.AutoScroll = true;
            this.enqSearch.BackColor = System.Drawing.Color.White;
            this.enqSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enqSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.enqSearch.IsDirty = false;
            this.enqSearch.Location = new System.Drawing.Point(10, 86);
            this.enqSearch.Name = "enqSearch";
            this.enqSearch.Size = new System.Drawing.Size(212, 286);
            this.enqSearch.TabIndex = 2;
            this.enqSearch.ToBeRefreshed = false;
            // 
            // cmbSearchFor
            // 
            this.cmbSearchFor.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbSearchFor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchFor.Location = new System.Drawing.Point(10, 63);
            this.cmbSearchFor.Name = "cmbSearchFor";
            this.cmbSearchFor.Size = new System.Drawing.Size(212, 23);
            this.cmbSearchFor.TabIndex = 1;
            this.cmbSearchFor.SelectedIndexChanged += new System.EventHandler(this.ucSearchManager1_SearchForChanged);
            // 
            // labSearchWhat
            // 
            this.labSearchWhat.Dock = System.Windows.Forms.DockStyle.Top;
            this.labSearchWhat.Location = new System.Drawing.Point(10, 43);
            this.resourceLookup1.SetLookup(this.labSearchWhat, new FWBS.OMS.UI.Windows.ResourceLookupItem("SMSEARCHFOR", "Search For", ""));
            this.labSearchWhat.Name = "labSearchWhat";
            this.labSearchWhat.Size = new System.Drawing.Size(212, 20);
            this.labSearchWhat.TabIndex = 7;
            this.labSearchWhat.Text = "Search For";
            this.labSearchWhat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBorder
            // 
            this.pnlBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBorder.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBorder.Location = new System.Drawing.Point(10, 42);
            this.pnlBorder.Name = "pnlBorder";
            this.pnlBorder.Size = new System.Drawing.Size(212, 1);
            this.pnlBorder.TabIndex = 9;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.lnkCreateNew);
            this.pnlButtons.Controls.Add(this.btnSearch);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButtons.Location = new System.Drawing.Point(10, 10);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.pnlButtons.Size = new System.Drawing.Size(212, 32);
            this.pnlButtons.TabIndex = 11;
            // 
            // lnkCreateNew
            // 
            this.lnkCreateNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lnkCreateNew.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup1.SetLookup(this.lnkCreateNew, new FWBS.OMS.UI.Windows.ResourceLookupItem("SMNEW", "New", ""));
            this.lnkCreateNew.Name = "lnkCreateNew";
            this.lnkCreateNew.Size = new System.Drawing.Size(148, 24);
            this.lnkCreateNew.TabIndex = 8;
            this.lnkCreateNew.TabStop = true;
            this.lnkCreateNew.Text = "New";
            this.lnkCreateNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkCreateNew.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCreateNew_LinkClicked);
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Location = new System.Drawing.Point(148, 0);
            this.resourceLookup1.SetLookup(this.btnSearch, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnSearch", "Search", ""));
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnSearch.Size = new System.Drawing.Size(64, 24);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // pnlMangerParent
            // 
            this.pnlMangerParent.Controls.Add(this.pnlBack);
            this.pnlMangerParent.Controls.Add(this.pnlHeading);
            this.pnlMangerParent.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMangerParent.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMangerParent.Location = new System.Drawing.Point(0, 0);
            this.pnlMangerParent.Name = "pnlMangerParent";
            this.pnlMangerParent.Size = new System.Drawing.Size(250, 424);
            this.pnlMangerParent.TabIndex = 17;
            // 
            // ucSearchControl1
            // 
            this.ucSearchControl1.BackColor = System.Drawing.Color.White;
            this.ucSearchControl1.BackGroundColor = System.Drawing.Color.White;
            this.ucSearchControl1.ButtonPanelVisible = true;
            this.ucSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchControl1.DoubleClickAction = "None";
            this.ucSearchControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucSearchControl1.GraphicalPanelVisible = true;
            this.ucSearchControl1.Location = new System.Drawing.Point(255, 0);
            this.ucSearchControl1.Margin = new System.Windows.Forms.Padding(0);
            this.ucSearchControl1.Name = "ucSearchControl1";
            this.ucSearchControl1.NavCommandPanel = null;
            this.ucSearchControl1.Padding = new System.Windows.Forms.Padding(5);
            this.ucSearchControl1.SearchListCode = "";
            this.ucSearchControl1.SearchListType = "";
            this.ucSearchControl1.Size = new System.Drawing.Size(579, 424);
            this.ucSearchControl1.TabIndex = 18;
            this.ucSearchControl1.ToBeRefreshed = false;
            this.ucSearchControl1.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.ucSearchControl1_SearchButtonCommands);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(250, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 424);
            this.splitter1.TabIndex = 16;
            this.splitter1.TabStop = false;
            // 
            // ucSearchManager
            // 
            this.Controls.Add(this.ucSearchControl1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlMangerParent);
            this.Name = "ucSearchManager";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.Size = new System.Drawing.Size(839, 424);
            this.pnlHeading.ResumeLayout(false);
            this.pnlBack.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.pnlMangerParent.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Properties
		public bool CloseVisible
		{
			get
			{
				return btnClose.Visible;
			}
			set
			{
				btnClose.Visible = value;
			}
		}

		public string Heading
		{
			get
			{
				return labHeading.Text;
			}
			set
			{
				labHeading.Text = value;
			}
		}

		[DefaultValue(null)]
		public EnquiryForm EnquiryForm
		{
			get
			{
				return enqSearch;
			}
		}

		public string SearchForText
		{
			get
			{
				return cmbSearchFor.Text;
			}
			set
			{
				cmbSearchFor.Text = value;
			}
		}

        public bool CreateNewVisible
        {
            get
            {
                return lnkCreateNew.Visible;
            }
            set
            {
                lnkCreateNew.Visible = value;
            }
        }

		public int SearchForIndex
		{
			get
			{
				return cmbSearchFor.SelectedIndex;
			}
			set
			{
				if (cmbSearchFor.SelectedIndex != value)
				{
					cmbSearchFor.SelectedIndex = value;
					OnSearchForChanged();
				}
			}
		}

		public Button cmdSearch
		{
			get
			{
				return btnSearch;
			}
		}

        public void Search()
        {
            ucSearchControl1.Search();
        }

		#endregion

        #region Public Methods
        public void Close()
		{
			OnClosed();
		}

		public void ClearSearch()
		{
			
		}
		
		#endregion

		#region Events
		public event EventHandler Closed;
		public event EventHandler Searched;
		public event EventHandler SearchForChanged;
		public event EventHandler CreateNewClick;

        protected void OnCreateNewClick()
		{
			if (CreateNewClick != null)
				CreateNewClick(this,EventArgs.Empty);
		}

        protected void OnSearchForChanged()
		{
			if (SearchForChanged != null)
				SearchForChanged(this,EventArgs.Empty);
		}

        protected void OnClosed()
		{
			if (Closed != null)
				Closed(this,EventArgs.Empty);
		}

		protected void OnSearch()
		{
			if (Searched != null)
				Searched(this,EventArgs.Empty);
		}
		#endregion

		#region Private Events
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			OnClosed();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			OnSearch();
		}

		private void lnkCreateNew_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
            if (this.SearchForIndex == 0)
            {
                FWBS.OMS.Contact tmpcontact =  FWBS.OMS.UI.Windows.Services.Wizards.CreateContact();
                if (tmpcontact != null)
                {
                    NewOMSTypeWindowEventArgs eva = new NewOMSTypeWindowEventArgs(tmpcontact);
                    OnNewOMSTypeWindow(this, eva);
                }
            }
            else if (this.SearchForIndex == 1)
            {
                FWBS.OMS.Client tmpclient = FWBS.OMS.UI.Windows.Services.Wizards.CreateClient(true);
                if (tmpclient != null)
                {
                    NewOMSTypeWindowEventArgs eva = new NewOMSTypeWindowEventArgs(tmpclient);
                    OnNewOMSTypeWindow(this, eva);
                }
            }
            else if (this.SearchForIndex == 2)
            {
                Client c = FWBS.OMS.UI.Windows.Services.SelectClient();
                if (c != null)
                {
                    OMSFile n = FWBS.OMS.UI.Windows.Services.Wizards.CreateFile(c);
                    if (n != null)
                    {
                        NewOMSTypeWindowEventArgs eva = new NewOMSTypeWindowEventArgs(n);
                        OnNewOMSTypeWindow(this, eva);
                    }
                 }
            }
            try
            {
                OnCreateNewClick();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
		}

		#endregion

        protected void OnNewOMSTypeWindow(object sender, NewOMSTypeWindowEventArgs e)
        {
            if (NewOMSTypeWindow != null)
                NewOMSTypeWindow(this, e);
            else
                FWBS.OMS.UI.Windows.Services.ShowOMSType(e.OMSObject, e.DefaultPage);
        }

        public void OnNewOMSTypeWindow(NewOMSTypeWindowEventArgs e)
        {
            this.OnNewOMSTypeWindow(this, e);
        }

        private void cmbSearchFor_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_lastsearchforindex != cmbSearchFor.SelectedIndex)
            {
                OnSearchForChanged();
                _lastsearchforindex = cmbSearchFor.SelectedIndex;
            }
        }

        private void ucSearchManager1_SearchForChanged(object sender, System.EventArgs e)
        {
            switch (cmbSearchFor.SelectedIndex)
            {
                case 0:
                    {
                        ucSearchControl1.EnquiryForm = this.EnquiryForm;
                        ucSearchControl1.cmdSearch = this.cmdSearch;
                        ucSearchControl1.SetSearchListType(Session.CurrentSession.DefaultSystemSearchListGroups(FWBS.OMS.SystemSearchListGroups.SearchManagerContact), null, new FWBS.Common.KeyValueCollection());
                        ucSearchControl1.dgSearchResults.DataSource = null;
                        ucSearchControl1.dgSearchResults.CaptionText = "";
                        this.CreateNewVisible = true;
                        break;
                    }
                case 1:
                    {
                        ucSearchControl1.EnquiryForm = this.EnquiryForm;
                        ucSearchControl1.cmdSearch = this.cmdSearch;
                        ucSearchControl1.SetSearchListType(Session.CurrentSession.DefaultSystemSearchListGroups(FWBS.OMS.SystemSearchListGroups.SearchManagerClient), null, new FWBS.Common.KeyValueCollection());
                        ucSearchControl1.dgSearchResults.DataSource = null;
                        ucSearchControl1.dgSearchResults.CaptionText = "";
                        Common.ApplicationSetting ccl = new Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, @"UI\Tweaks", "CreateClientButton", "True");
                        this.CreateNewVisible = ccl.ToBoolean();
                        break;
                    }
                case 2:
                    {
                        ucSearchControl1.EnquiryForm = this.EnquiryForm;
                        ucSearchControl1.cmdSearch = this.cmdSearch;
                        ucSearchControl1.SetSearchListType(Session.CurrentSession.DefaultSystemSearchListGroups(FWBS.OMS.SystemSearchListGroups.SearchManagerFile), null, new FWBS.Common.KeyValueCollection());
                        ucSearchControl1.dgSearchResults.DataSource = null;
                        ucSearchControl1.dgSearchResults.CaptionText = "";
                        Common.ApplicationSetting cf = new Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, @"UI\Tweaks", "CreateFileButton", "True");
                        this.CreateNewVisible = cf.ToBoolean();
                        break;
                    }
            }
        }

        private void ucSearchControl1_SearchButtonCommands(object sender, FWBS.OMS.UI.Windows.SearchButtonEventArgs e)
        {
            if (e.Action == SearchEngine.ButtonActions.Select)
            {
                switch (cmbSearchFor.SelectedIndex)
                {
                    case 0:
                        {
                            FWBS.OMS.Contact tmpcontact = FWBS.OMS.Contact.GetContact((long)ucSearchControl1.CurrentItem()["CONTID"].Value);
                            NewOMSTypeWindowEventArgs eva = new NewOMSTypeWindowEventArgs(tmpcontact);
                            OnNewOMSTypeWindow(this, eva);
                            break;
                        }
                    case 1:
                        {
                            FWBS.OMS.Client tmpclient = FWBS.OMS.Client.GetClient((long)ucSearchControl1.CurrentItem()["CLID"].Value);
                            NewOMSTypeWindowEventArgs eva = new NewOMSTypeWindowEventArgs(tmpclient);
                            OnNewOMSTypeWindow(this, eva);
                            break;
                        }
                    case 2:
                        {
                            FWBS.OMS.OMSFile tmpfile = FWBS.OMS.OMSFile.GetFile((long)ucSearchControl1.CurrentItem()["FILEID"].Value);
                            NewOMSTypeWindowEventArgs eva = new NewOMSTypeWindowEventArgs(tmpfile);
                            OnNewOMSTypeWindow(this, eva);
                            break;
                        }
                }
            }
        }
	}
}
