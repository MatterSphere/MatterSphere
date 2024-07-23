using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.SearchEngine;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for eAddressPassiveSearch.
    /// </summary>
    public class eAddressPassiveSearch : System.Windows.Forms.UserControl
	{
		#region Control Fields
		private System.Windows.Forms.Label labTotal;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timSearch;
		private System.Windows.Forms.Timer timDisplay;
		private System.Windows.Forms.PictureBox picShow;
		#endregion

		#region Fields
		private string _contAddressControlName = "";
		private eAddress _IcontAddressControlName = null;
		private EnquiryForm _parent = null;
		private System.Windows.Forms.LinkLabel lnkShowMe;
		private FWBS.OMS.SearchEngine.SearchList _searchlist = null;
		private DataTable _data = null;
		private ucVerticalContacts vc = null;
		private Form _ptparent = null;
		private omsSplitter sp1=null;
        private ResourceLookup resourceLookup1;
        private Panel panelImage;
		private Address cnt = null;
		#endregion

		#region Constructors & Dispose
		public eAddressPassiveSearch()
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
                Close();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(eAddressPassiveSearch));
            this.picShow = new System.Windows.Forms.PictureBox();
            this.labTotal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkShowMe = new System.Windows.Forms.LinkLabel();
            this.timSearch = new System.Windows.Forms.Timer(this.components);
            this.timDisplay = new System.Windows.Forms.Timer(this.components);
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.panelImage = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picShow)).BeginInit();
            this.panelImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // picShow
            // 
            this.picShow.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picShow.Image = ((System.Drawing.Image)(resources.GetObject("picShow.Image")));
            this.picShow.Location = new System.Drawing.Point(25, 1);
            this.picShow.Name = "picShow";
            this.picShow.Size = new System.Drawing.Size(32, 32);
            this.picShow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picShow.TabIndex = 0;
            this.picShow.TabStop = false;
            this.picShow.Click += new System.EventHandler(this.picShow_Click);
            // 
            // labTotal
            // 
            this.labTotal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labTotal.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labTotal.Location = new System.Drawing.Point(5, 40);
            this.labTotal.Name = "labTotal";
            this.labTotal.Size = new System.Drawing.Size(83, 24);
            this.labTotal.TabIndex = 1;
            this.labTotal.Text = "0";
            this.labTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(5, 64);
            this.resourceLookup1.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("Found", " Found", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = " Found";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lnkShowMe
            // 
            this.lnkShowMe.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lnkShowMe.Enabled = false;
            this.lnkShowMe.Location = new System.Drawing.Point(5, 79);
            this.resourceLookup1.SetLookup(this.lnkShowMe, new FWBS.OMS.UI.Windows.ResourceLookupItem("ShowMe", "Show Me", ""));
            this.lnkShowMe.Name = "lnkShowMe";
            this.lnkShowMe.Size = new System.Drawing.Size(83, 15);
            this.lnkShowMe.TabIndex = 3;
            this.lnkShowMe.TabStop = true;
            this.lnkShowMe.Tag = "ShowMe";
            this.lnkShowMe.Text = "Show Me";
            this.lnkShowMe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkShowMe.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkShowMe_LinkClicked);
            // 
            // timSearch
            // 
            this.timSearch.Interval = 500;
            this.timSearch.Tick += new System.EventHandler(this.timSearch_Tick);
            // 
            // timDisplay
            // 
            this.timDisplay.Interval = 2000;
            this.timDisplay.Tick += new System.EventHandler(this.timDisplay_Tick);
            // 
            // panelImage
            // 
            this.panelImage.Controls.Add(this.picShow);
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(5, 5);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(83, 35);
            this.panelImage.TabIndex = 4;
            // 
            // eAddressPassiveSearch
            // 
            this.Controls.Add(this.panelImage);
            this.Controls.Add(this.labTotal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lnkShowMe);
            this.Name = "eAddressPassiveSearch";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(93, 99);
            this.VisibleChanged += new System.EventHandler(this.eAddressPassiveSearch_VisibleChanged);
            this.ParentChanged += new System.EventHandler(this.eAddressPassiveSearch_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.picShow)).EndInit();
            this.panelImage.ResumeLayout(false);
            this.ResumeLayout(false);

		}
        #endregion

        #region Properties
        /// <summary>
        /// Override base font property to hide in design mode.
        /// </summary>
        [Browsable(false)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [Category("OMS")]
		[TypeConverter(typeof(ContactEnquiryControlLister))]
		public string AddressControlName
		{
			get
			{
				return _contAddressControlName;
			}
			set
			{
				_contAddressControlName = value;
			}
		}

		/// <summary>
		/// The Enquiry form used by the Designers to get the controls used
		/// </summary>
		[Browsable(false)]
		public EnquiryForm EnquiryForm
		{
			get
			{
				return _parent;
			}
		}

		[Browsable(false)]
		public Address Address
		{
			get
			{
				if (cnt == null)
				{
					cnt = _IcontAddressControlName.Value as Address;
					cnt.Update();
				}
				return cnt;
			}
		}
		
		#endregion

		#region Methods
		public void Close()
		{
			if (vc != null && vc.Visible)
			{
				if (vc != null) vc.Visible=false;
				if (sp1 != null) sp1.Visible=false;
				if (_ptparent != null) _ptparent.Width -= (vc.Width + sp1.Width);
			}
		}
		#endregion

		#region Private Events
		private void eAddressPassiveSearch_ParentChanged(object sender, System.EventArgs e)
		{
			if (Parent is EnquiryForm)
			{
				_parent = Parent as EnquiryForm;
				if (_parent != null && _parent.Enquiry.InDesignMode == false)
				{
					_IcontAddressControlName = _parent.GetControl(_contAddressControlName,EnquiryControlMissing.Exception) as eAddress;
					
					_parent.PageChanged +=new PageChangedEventHandler(_parent_PageChanged);
					_parent.Updated +=new EventHandler(_parent_Updated);
					_parent.Cancelled +=new EventHandler(_parent_Updated);

					_searchlist = new FWBS.OMS.SearchEngine.SearchList(FWBS.OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.Address),null,new FWBS.Common.KeyValueCollection());
					_searchlist.Searched +=new FWBS.OMS.SearchEngine.SearchedEventHandler(_searchlist_Searched);
					_searchlist.Searching +=new CancelEventHandler(_searchlist_Searching);
					_searchlist.ParameterHandler += new SourceEngine.SetParameterHandler(this.SetParam);
					_searchlist.Error +=new MessageEventHandler(_searchlist_Error);
					if (_IcontAddressControlName != null) 
					{
						_IcontAddressControlName.ActiveChanged += new EventHandler(_IaddressControlName_ActiveChanged);
						_IcontAddressControlName.EnabledChanged += new EventHandler(_IaddressControlName_EnabledChanged);
					}
				}
			}
			else if (Parent == null && _parent != null)
			{
				_parent.PageChanged -= new PageChangedEventHandler(_parent_PageChanged);
				_parent.Updated -=new EventHandler(_parent_Updated);
				_parent.Cancelled -=new EventHandler(_parent_Updated);
				if (_searchlist != null)
				{
					_searchlist.Searched -= new FWBS.OMS.SearchEngine.SearchedEventHandler(_searchlist_Searched);
					_searchlist.Searching -= new CancelEventHandler(_searchlist_Searching);
					_searchlist.ParameterHandler -= new SourceEngine.SetParameterHandler(this.SetParam);
					_searchlist.Error -= new MessageEventHandler(_searchlist_Error);
					_searchlist = null;
				}
				_parent = null;
				if (_IcontAddressControlName != null) 
				{
					_IcontAddressControlName.ActiveChanged -=new EventHandler(_IaddressControlName_ActiveChanged);
					_IcontAddressControlName.EnabledChanged -= new EventHandler(_IaddressControlName_EnabledChanged);
				}
			}
		}

		#region Threaded To Main Thread
		private void _searchlist_Searched(object sender, FWBS.OMS.SearchEngine.SearchedEventArgs e)
		{
			try
			{
				SearchedEventHandler sch = new SearchedEventHandler(this.Main_searchlist_Searched);
				this.Invoke(sch, new object [2] {sender, e});
			}
			catch
			{}
		}

		private void _searchlist_Searching(object sender, CancelEventArgs e)
		{
			try
			{
				CancelEventHandler sch = new CancelEventHandler(this.Main_searchlist_Searching);
				this.Invoke(sch,new object [2] {sender, e});
			}
			catch
			{}
		}
		#endregion

		private void Main_searchlist_Searched(object sender, FWBS.OMS.SearchEngine.SearchedEventArgs e)
		{
			labTotal.Text = _searchlist.ResultCount.ToString();
			lnkShowMe.Enabled = (_searchlist.ResultCount > 0);

			_data = e.Data;
			if (vc == null || vc.Visible==false && cnt == null)
				if (_searchlist.ResultCount < 10 && _searchlist.ResultCount > 0) timDisplay.Enabled=true; else timDisplay.Enabled=false;
			if (vc != null && vc.Visible==true && cnt == null && _searchlist.ResultCount < 10)
				timDisplay_Tick(sender,EventArgs.Empty);
		}

		private void Main_searchlist_Searching(object sender, CancelEventArgs e)
		{

		}

		private void _IcontNameControlName_Changed(object sender, EventArgs e)
		{
			timSearch.Enabled=false;
			timSearch.Enabled=true;
		}

		private void _IaddressControlName_ActiveChanged(object sender, EventArgs e)
		{
			timSearch.Enabled=false;
			timSearch.Enabled=true;
		}

		private void _IaddressControlName_EnabledChanged(object sender, EventArgs e)
		{
			if (_IcontAddressControlName.ReadOnly)
			{
				lnkShowMe.Text = Session.CurrentSession.Resources.GetResource("Reset", "&Reset", "").Text;
				lnkShowMe.Tag = "Reset";
			}
			else
			{
				lnkShowMe.Text = Session.CurrentSession.Resources.GetResource("ShowMe", "Show Me", "").Text;
				lnkShowMe.Tag = "ShowMe";
			}
		}

		/// <summary>
		/// Used by the Search List to get the Parameters
		/// </summary>
		/// <param name="name">Name of Control on the Enquiry Form</param>
		/// <param name="value">The Return Value</param>
		private void SetParam(string name,out object value)
		{
			try
			{
				if (name == "MAX") value = 50;
				else if (name.ToUpper() == "ADDRESS1") 
				{
					FWBS.Common.UI.IBasicEnquiryControl2 e = _IcontAddressControlName.GetIBasicEnquiryControl2("txtAdd1");
					if (e != null) value= e.Value; else value =DBNull.Value;
				}
				else value = DBNull.Value;
			}
			catch
			{
				value = DBNull.Value;
			}
		}

		private void _searchlist_Error(object sender, MessageEventArgs e)
		{
			System.Diagnostics.Debugger.Break();
		}

		private void lnkShowMe_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			if (lnkShowMe.Tag.Equals("Reset"))
			{
				cnt = null;
				timSearch.Interval = 500000;
				_IcontAddressControlName.Value = DBNull.Value;

				lnkShowMe.Text = Session.CurrentSession.Resources.GetResource("ShowMe", "Show Me", "").Text;
				lnkShowMe.Tag = "ShowMe";
				timSearch.Interval = 500;
				timSearch.Enabled=false;
				labTotal.Text = "0";
				((Control)_IcontAddressControlName).Focus();
			}
			else
				showresults();
		}

		private void showresults()
		{
			try
			{
                SizeF scaleFactor = new SizeF(DeviceDpi / 96F, DeviceDpi / 96F);
				if (vc == null) 
				{
					vc = new ucVerticalContacts();
                    vc.Scale(scaleFactor);
					_ptparent = Global.GetParentForm(this);
					if (_ptparent != null)
					{
						vc.Close -=new EventHandler(vc_Close);
						vc.Accept -=new EventHandler(vc_Accept);

						vc.Close +=new EventHandler(vc_Close);
						vc.Accept +=new EventHandler(vc_Accept);
						sp1 = new omsSplitter();
                        sp1.Scale(scaleFactor);
                        sp1.Dock = DockStyle.Right;
						vc.Dock = DockStyle.Right;
                        _ptparent.Width += (vc.Width + sp1.Width);
						_ptparent.Controls.Add(sp1);
						_ptparent.Controls.Add(vc);
					}
				}
				if (vc.Contacts.Count > 0) vc.Contacts.Clear();
				if (_data.Rows.Count == 0) return;
				vc.MainPanelVisible = false;
				foreach (DataRow rw in _data.Rows)
				{
					ucVerticalContactEx vct = new ucVerticalContactEx(Str(rw["addLine1"]),Str(rw["addLine2"]),Str(rw["addLine3"]),Str(rw["addLine4"]),Str(rw["addLine5"]),Str(rw["addPostcode"]),"","","","","");	
					vct.Tag = rw;
					vct.SizeToFit();
                    vct.Scale(scaleFactor);
                    vc.Contacts.Add(vct);
				}
			}
			catch
			{
			}
			finally
			{
				if (sp1.Visible==false)
					_ptparent.Width += (vc.Width + sp1.Width);
				vc.MainPanelVisible = true;
				sp1.Visible=true;
				vc.Visible=true;
			}
		}

		private string Str(object value)
		{
			return Convert.ToString(value);
		}

		private void _ptparent_Move(object sender, EventArgs e)
		{
			vc.Location = new Point( _ptparent.Left + _ptparent.Width,_ptparent.Top);
		}

		private void vc_Close(object sender, EventArgs e)
		{
			Close();
		}

		private void vc_Accept(object sender, EventArgs e)
		{
			DataRow dr = vc.Selected.Tag as DataRow;
			if (dr != null)
			{
				cnt = Address.GetAddress(Convert.ToInt32(dr["addID"]));
				_IcontAddressControlName.Value = cnt;
				_parent.Enquiry.SetValue(_IcontAddressControlName.Name,cnt);
				_IcontAddressControlName.Refresh();
				_IcontAddressControlName.ReadOnly = true;

				lnkShowMe.Text = Session.CurrentSession.Resources.GetResource("Reset", "&Reset", "").Text;
				lnkShowMe.Tag = "Reset";
				Close();
			}
		}

		private void timSearch_Tick(object sender, System.EventArgs e)
		{
			timSearch.Enabled=false;
			FWBS.Common.UI.IBasicEnquiryControl2 ee = _IcontAddressControlName.GetIBasicEnquiryControl2("txtAdd1");
			if (Convert.ToString(ee.Value) != "")
			{
				if (this.Visible)
					_searchlist.Search(true);
			}
			else
			{
				labTotal.Text = "0";
				lnkShowMe.Enabled = false;
				if (vc != null && vc.Visible==true && cnt == null && vc.Contacts.Count > 0)
					vc.Contacts.Clear();
			}
		}

		private void timDisplay_Tick(object sender, System.EventArgs e)
		{
			timDisplay.Enabled=false;
			showresults();
		}

		private void eAddressPassiveSearch_VisibleChanged(object sender, System.EventArgs e)
		{
			if (!this.Visible)
				timDisplay.Enabled=false;		
		}

		private void _parent_PageChanged(object sender, PageChangedEventArgs e)
		{
			Close();
		}

		private void picShow_Click(object sender, System.EventArgs e)
		{
			showresults();
		}

		private void _parent_Updated(object sender, EventArgs e)
		{
			timDisplay.Enabled=false;		
			timSearch.Enabled=false;
			Close();
		}
		#endregion
	}
}
