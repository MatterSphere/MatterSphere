using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using FWBS.Common.UI;
using FWBS.OMS.SearchEngine;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for eContactPassiveSearch.
    /// </summary>
    public class eContactPassiveSearch : System.Windows.Forms.UserControl
	{
		#region Control Fields
		private System.Windows.Forms.Label labTotal;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.LinkLabel lnkShowMe;
		private System.Windows.Forms.Timer timSearch;
		private System.Windows.Forms.Timer timDisplay;
		private System.Windows.Forms.PictureBox picFind;
		private System.Windows.Forms.ErrorProvider err;
        private System.Windows.Forms.ErrorProvider req;
        private System.Windows.Forms.LinkLabel lnkFind;
		#endregion

		#region Fields
        /// <summary>
        /// Created Now is a bool that tells the control if the Contact and Address were created in this
        /// wizards session. If so it may update the address if the wizard fails validation and has to
        /// write the Contact and Address again.
        /// </summary>
        private bool _creatednow = false;

		/// <summary>
		/// The XML String that contains the Property Mapping.
		/// </summary>
		private FWBS.Common.ConfigSetting _contactconfigsetting = new FWBS.Common.ConfigSetting("");

		/// <summary>
		/// The Control name that can contain the name of the Check Box to give the option of a new Contact.
		/// </summary>
		private string _contAnotherContactControlName = "";

		/// <summary>
		/// The Reference to the Address Control.
		/// </summary>
		private eAddress _IcontAddressControlName = null;

		/// <summary>
		/// The Referance to the Name Control.
		/// </summary>
		private FWBS.Common.UI.IBasicEnquiryControl2 _IcontNameControlName = null;

		/// <summary>
		/// The Referance to the Another Check Box.
		/// </summary>
		private FWBS.Common.UI.Windows.eCheckBox2 _IcontAnotherContactControlName = null;

		/// <summary>
		/// The Referance to the Enquiry Form.
		/// </summary>
		private EnquiryForm _parent = null;

		/// <summary>
		/// The Search List Object used for search the Contacts.
		/// </summary>
		private FWBS.OMS.SearchEngine.SearchList _searchlist = null;

		/// <summary>
		/// The Return Data Table of Contacts.
		/// </summary>
		private DataTable _data = null;

		/// <summary>
		/// A Reference to the Popout Vertical Contacts Object.
		/// </summary>
		private ucVerticalContacts vc = null;

		/// <summary>
		/// A Reference to the Form Contain this Control.
		/// </summary>
		private Form _ptparent = null;

		/// <summary>
		/// A Spitter to seperate the Vertical Contacts from the Wizard.
		/// </summary>
		private omsSplitter sp1 = null;

		/// <summary>
		/// The Contact Object which contains the Final Contact.
		/// </summary>
		private Contact cnt = null;

		/// <summary>
		/// The Contact Type used to Create the New Contact.
		/// </summary>
		private string _contacttype = "";

		/// <summary>
		/// If the Contact has Another Tick Option availabe if Skipped which page to skip too.
		/// </summary>
		private string _contactgotopageifnotrequired = "";

		/// <summary>
		/// The eCheckBox2 that will contain a reference Copy From. 
		/// </summary>
		private FWBS.Common.UI.Windows.eCheckBox2 _Icopyfromprinciple = null;

		/// <summary>
		/// The Type of Contact.
		/// </summary>
		private OMSTypeContactGeneralType _generaltype = OMSTypeContactGeneralType.Individual;

		/// <summary>
		/// Title & Initials Surname Link to Contact.
		/// </summary>
		private FWBS.Common.UI.IBasicEnquiryControl2 _title = null;
		private FWBS.Common.UI.IBasicEnquiryControl2 _intitials = null;
		private FWBS.Common.UI.IBasicEnquiryControl2 _surname = null;
		private FWBS.Common.UI.IBasicEnquiryControl2 _clienttype = null;

		/// <summary>
		/// The Search List Code.
		/// </summary>
		private string _searchlistcode;

		/// <summary>
		/// The Search Params.
		/// </summary>
		private SortedList _searchparams = new SortedList();

		/// <summary>
		/// The View Contact Button on the Popout Panel.
		/// </summary>
		ToolStripButton btContact = null;
		/// <summary>
		/// The Control Relationship Name.
		/// </summary>
		private string _contrelationshipname = "";
        private ResourceLookup resourceLookup1;

		/// <summary>
		/// Enabled the Finish button on Accept.
		/// </summary>
		private bool _enablefinsihedonaccept = false;
        private Panel panel1;
        bool showme = true;
        private RequiredFieldRenderer _reqFieldRenderer;
		#endregion

		#region Constructors & Dispose
		public eContactPassiveSearch()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			if (Session.CurrentSession.IsLoggedIn)
				_searchlistcode = OMS.Session.CurrentSession.DefaultSystemSearchList(SystemSearchLists.SearchContacts);
            _reqFieldRenderer = new RequiredFieldRenderer(req);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(eContactPassiveSearch));
            this.picFind = new System.Windows.Forms.PictureBox();
            this.labTotal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkShowMe = new System.Windows.Forms.LinkLabel();
            this.timSearch = new System.Windows.Forms.Timer(this.components);
            this.timDisplay = new System.Windows.Forms.Timer(this.components);
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.req = new System.Windows.Forms.ErrorProvider(this.components);
            this.lnkFind = new System.Windows.Forms.LinkLabel();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picFind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.req)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picFind
            // 
            this.picFind.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picFind.Image = ((System.Drawing.Image)(resources.GetObject("picFind.Image")));
            this.picFind.Location = new System.Drawing.Point(30, 4);
            this.picFind.Name = "picFind";
            this.picFind.Size = new System.Drawing.Size(42, 34);
            this.picFind.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFind.TabIndex = 0;
            this.picFind.TabStop = false;
            this.picFind.Click += new System.EventHandler(this.picFind_Click);
            // 
            // labTotal
            // 
            this.labTotal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labTotal.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labTotal.Location = new System.Drawing.Point(0, 65);
            this.labTotal.Name = "labTotal";
            this.labTotal.Size = new System.Drawing.Size(102, 24);
            this.labTotal.TabIndex = 1;
            this.labTotal.Text = "0";
            this.labTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(0, 89);
            this.resourceLookup1.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("Found", " Found", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = " Found";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lnkShowMe
            // 
            this.lnkShowMe.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lnkShowMe.Enabled = false;
            this.lnkShowMe.Location = new System.Drawing.Point(0, 104);
            this.lnkShowMe.Name = "lnkShowMe";
            this.lnkShowMe.Size = new System.Drawing.Size(102, 15);
            this.lnkShowMe.TabIndex = 3;
            this.lnkShowMe.TabStop = true;
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
            // err
            // 
            this.err.ContainerControl = this;
            this.err.Icon = ((System.Drawing.Icon)(resources.GetObject("err.Icon")));
            // 
            // req
            // 
            this.req.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.req.ContainerControl = this;
            this.req.Icon = ((System.Drawing.Icon)(resources.GetObject("req.Icon")));
            // 
            // lnkFind
            // 
            this.lnkFind.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkFind.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkFind.Location = new System.Drawing.Point(0, 5);
            this.resourceLookup1.SetLookup(this.lnkFind, new FWBS.OMS.UI.Windows.ResourceLookupItem("ContactSch", "Contact Search", ""));
            this.lnkFind.Name = "lnkFind";
            this.lnkFind.Size = new System.Drawing.Size(102, 18);
            this.lnkFind.TabIndex = 5;
            this.lnkFind.TabStop = true;
            this.lnkFind.Text = "Contact Search";
            this.lnkFind.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lnkFind.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFind_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.picFind);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(102, 42);
            this.panel1.TabIndex = 6;
            // 
            // eContactPassiveSearch
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labTotal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lnkShowMe);
            this.Controls.Add(this.lnkFind);
            this.Name = "eContactPassiveSearch";
            this.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.Size = new System.Drawing.Size(102, 124);
            this.Load += new System.EventHandler(this.eContactPassiveSearch_Load);
            this.EnabledChanged += new System.EventHandler(this.eContactPassiveSearch_EnabledChanged);
            this.VisibleChanged += new System.EventHandler(this.eContactPassiveSearch_VisibleChanged);
            this.ParentChanged += new System.EventHandler(this.eContactPassiveSearch_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.picFind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.req)).EndInit();
            this.panel1.ResumeLayout(false);
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

        /// <summary>
        /// Gets or Set the XML for the Contact Property Mapping.
        /// </summary>
        [Category("OMS")]
		[Editor(typeof(ContactMappingEditor),typeof(UITypeEditor))]
		public string ContactProperyMapping
		{
			get
			{
				return _contactconfigsetting.DocObject.OuterXml;;
			}
			set
			{
				_contactconfigsetting = new FWBS.Common.ConfigSetting(value);
			}
		}

		/// <summary>
		/// Gets of Sets the Name of the Relationship Control.
		/// </summary>
		[Category("OMS")]
		[TypeConverter(typeof(ContactEnquiryControlLister))]
		public string RelationshipControl
		{
			get
			{
				return _contrelationshipname;
			}
			set
			{
				_contrelationshipname = value;
			}
		}

		/// <summary>
		/// Gets of Sets the Name of the CheckBox if a option to use is available.
		/// </summary>
		[Category("OMS")]
		[TypeConverter(typeof(ContactEnquiryControlLister))]
		public string AnotherContactControlName
		{
			get
			{
				return _contAnotherContactControlName;
			}
			set
			{
				_contAnotherContactControlName = value;
			}
		}

		[Category("OMS")]
		[Description("If the AnotherContactControlName is filled in then you can skip to this page if not ticked")]
		public string IfAnotherNotTickedSkipToo
		{
			get
			{
				return _contactgotopageifnotrequired;
			}
			set
			{
				_contactgotopageifnotrequired = value;
			}
		}

		[Category("OMS")]
		[Description("Will enable the Finish button on Accept if used within a wizard")]
		public bool EnableFinsihOnAccept
		{
			get
			{
				return _enablefinsihedonaccept;
			}
			set
			{
				_enablefinsihedonaccept = value;
			}
		}
		[Category("OMS")]
		[Description("Override the Standard Contact Search List")]
		public string SearchListCode
		{
			get
			{
				return _searchlistcode;
			}
			set
			{
				_searchlistcode = value;
			}
		}

		/// <summary>
		/// The Contact Type used to Create a new Contact.
		/// </summary>
		[Browsable(true)]
		public string ContactTypeName
		{
			get
			{
				return _contacttype;
			}
			set
			{
				_contacttype = value;
			}
		}

		/// <summary>
		/// The Enquiry form used by the Designers to get the controls used.
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
		public SearchList SearchList
		{
			get 
			{
				if (_searchlist == null)
					_searchlist = new FWBS.OMS.SearchEngine.SearchList(_searchlistcode,null,new FWBS.Common.KeyValueCollection());

				return _searchlist;
			}
		}

		[Browsable(false)]
		public eAddress AddressControl
		{
			get
			{
				return _IcontAddressControlName;
			}
		}

		[Browsable(false)]
		public string IsAddressSameAsPrinciple
		{
			get
			{
				string ret = "";
				foreach(System.Xml.XmlAttribute xe in _contactconfigsetting.DocCurrent.Attributes)
					if (xe.Name == "IsAddressSameAsPrinciple")
						ret = MappingValue(xe.Value);
				return ret;
			}
		}

		/// <summary>
		/// Will Return True if the Contact Name and First Line of address is filled in.
		/// </summary>
		[Browsable(false)]
		public bool IsContact
		{
			get
			{
				try
				{
					if (EnquiryForm.GetControl(_contAnotherContactControlName) != null && Convert.ToBoolean(EnquiryForm.GetIBasicEnquiryControl2(_contAnotherContactControlName).Value) == false)
						return true;
					else if (Convert.ToString(_IcontNameControlName.Value).Trim() != "" && _IcontAddressControlName.Value != DBNull.Value)
						return true;
					else return false;
				}
				catch
				{
					return false;
				}
			}
		}

		/// <summary>
		/// The Contact Object When Accessed if the Contact does not exists will create.
		/// </summary>
		[Browsable(false)]
		public Contact Contact
		{
			get
			{
				if (cnt == null) // *************** CREATE A CONTACT ***************
				{
					if (EnquiryForm.GetControl(_contAnotherContactControlName) != null && Convert.ToBoolean(EnquiryForm.GetIBasicEnquiryControl2(_contAnotherContactControlName).Value) == false)
						return null;
					
					ContactType ct = null;
					try
					{
						ct = ContactType.GetContactType(_contacttype);
					}
					catch (Exception ex)
					{
						throw new OMSException2("PASGETCONTTYPE","Error loading Contact Type '%1%' for ContactPassiveSearch Control called '%2%'",ex,true,_contacttype,this.Name);
					}

					cnt = new Contact(ct);
					
					Address ad = _IcontAddressControlName.Value as Address;
					ad.Update();

					foreach(System.Xml.XmlAttribute xe in _contactconfigsetting.DocCurrent.Attributes)
					{
						IBasicEnquiryControl2 objval = _parent.GetIBasicEnquiryControl2(MappingValue(xe.Value));
						if (objval != null)
						{
							if (xe.Name.ToLower().StartsWith("none") == false)
							{
								try
								{
									if (xe.Name.IndexOf(".") > 0)
									{
										string[] extdata = xe.Name.Split('.');
										object thevalue = _parent.GetIBasicEnquiryControl2(MappingValue(xe.Value)).Value;
										if (thevalue == null) thevalue = DBNull.Value;
										if (thevalue is Address)
											((Address)thevalue).Update();
										cnt.ExtendedData[extdata[0]].SetExtendedData(extdata[1],thevalue);
									}
									else
										cnt.GetType().GetProperty(xe.Name).SetValue(cnt,_parent.GetIBasicEnquiryControl2(MappingValue(xe.Value)).Value,null);
								}
								catch (Exception ex)
								{
									string svalue = "";
									try
									{
										svalue = Convert.ToString(_parent.GetIBasicEnquiryControl2(MappingValue(xe.Value)).Value);
									}
									catch{}
									ErrorBox.Show(ParentForm, new OMSException2("PASCONTERR1","Error setting data for ContactPassiveSearch '%1%'" + Environment.NewLine + Environment.NewLine + "Fieldname '%2%' with value '%3%'",ex,true,this.Name,xe.Name,svalue));
								}
							}
						}
					}

                    
					cnt.Update();       
					cnt.Refresh(true);
                    _creatednow = true; // Set to True so if the forms fails validation it can modify the address again.
				}
				else // *************** EDIT A CONTACT ***************
				{
                    if (_creatednow)
                    {
                        Address ad = _IcontAddressControlName.Value as Address;
                        if (ad != null) ad.Update();
                    }

                    foreach(System.Xml.XmlAttribute xe in _contactconfigsetting.DocCurrent.Attributes)
					{
						IBasicEnquiryControl2 objval = _parent.GetIBasicEnquiryControl2(MappingValue(xe.Value));
						if (objval != null)
						{
							if (xe.Name.ToLower().StartsWith("none") == false)
							{
								try
								{
									if (_parent.GetControl(MappingValue(xe.Value)).Enabled == true)
									{
                                        if (xe.Name.IndexOf(".") > 0)
                                        {
                                            string[] extdata = xe.Name.Split('.');
                                            object thevalue = _parent.GetIBasicEnquiryControl2(MappingValue(xe.Value)).Value;
                                            if (thevalue == null) thevalue = DBNull.Value;
                                            if (thevalue is Address)
                                                ((Address)thevalue).Update();
                                            cnt.ExtendedData[extdata[0]].SetExtendedData(extdata[1], thevalue);
                                        }
                                        else
                                        {
                                            cnt.GetType().GetProperty(xe.Name).SetValue(cnt, _parent.GetIBasicEnquiryControl2(MappingValue(xe.Value)).Value, null);
                                        }
									}
								}
								catch (Exception ex)
								{
									string svalue = "";
									try
									{
										svalue = Convert.ToString(_parent.GetIBasicEnquiryControl2(MappingValue(xe.Value)).Value);
									}
									catch{}
									ErrorBox.Show(ParentForm, new OMSException2("PASCONTERR1","Error setting data for ContactPassiveSearch '%1%'" + Environment.NewLine + Environment.NewLine + "Fieldname '%2%' with value '%3%'",ex,true,this.Name,xe.Name,svalue));
								}
							}
						}
					}
                    //If this is a newly created Contact then skip the Advanced Security check 'Update Permission'
                    UpdateContact(_creatednow);
				}
				return cnt;
			}
		}

		#endregion

		#region Methods
        /// <summary>
        /// Use reflection to invoke non public Update method on the Contact object (in order to skip the Advanced Security Update Permissions check).
        /// </summary>
        private void UpdateContact(bool skipUpdatePermissionCheck)
        {
            MethodInfo mInfoMethod =
                cnt.GetType().GetMethod(
                    "Update",
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    Type.DefaultBinder,
                    new[] { typeof(bool) },
                    null);

            mInfoMethod.Invoke(cnt, new object[] { skipUpdatePermissionCheck });
        }


		public void Clear()
		{
		}

		/// <summary>
		/// Closes the Popout Vertical Contacts.
		/// </summary>
		public void Close()
		{
			if (vc != null && vc.Visible)
			{
				if (vc != null) vc.Visible=false;
				if (sp1 != null) sp1.Visible=false;
				if (_ptparent != null) _ptparent.Width -= (vc.Width + sp1.Width);
			}
		}

		/// <summary>
		/// Displays the Error Symbols if Validation Fails.
		/// </summary>
		public void ErrorsOn()
		{
			if (_IcontNameControlName != null && Str(_IcontNameControlName.Value) == "")
			{
				err.SetError((Control)_IcontNameControlName,_reqFieldRenderer.Description);
				req.SetError((Control)_IcontNameControlName,"");
            }
			if (_IcontAddressControlName != null && _IcontAddressControlName.Value == DBNull.Value)
			{
				_IcontAddressControlName.ErrorIconsOn(true);
			}
		}
		
		/// <summary>
		/// Hides the Errors Symbols and reset the Required Symbols.
		/// </summary>
		public void ErrorsOff()
		{
			if (_IcontNameControlName != null && Str(_IcontNameControlName.Value) == "")
			{
				_reqFieldRenderer.MarkRequiredControl((Control)_IcontNameControlName);
				err.SetError((Control)_IcontNameControlName,"");
            }
			if (_IcontAddressControlName != null)
			{
				_IcontAddressControlName.ErrorIconsOn(false);
			}
		}
		#endregion

		#region Private Events
		/// <summary>
		/// The Main Limit Action of this Control attaching itself to the Parent if a compatible
		/// host.
		/// </summary>
		/// <param name="sender">This Control</param>
		/// <param name="e">Empty</param>
		private void eContactPassiveSearch_ParentChanged(object sender, System.EventArgs e)
		{
			if (Parent is EnquiryForm)
			{
				_parent = Parent as EnquiryForm;
                lnkShowMe.Text = Session.CurrentSession.Resources.GetResource("ShowMe", "Show Me", "").Text;
			}
			else if (Parent == null && _parent != null)
			{
				if (_IcontNameControlName != null) 
				{
					_IcontNameControlName.ActiveChanged -=new EventHandler(_IcontNameControlName_Changed);
					_IcontNameControlName = null;
				}
				if (_IcontAddressControlName != null)
				{
					_IcontAddressControlName.ActiveChanged -=new EventHandler(_IcontNameControlName_Changed);
					_IcontAddressControlName = null;
				}
				if (_IcontAnotherContactControlName != null) 
				{
					_IcontAnotherContactControlName.ActiveChanged -=new EventHandler(_IcontAnotherContactControlName_Changed);
					_IcontAnotherContactControlName = null;
				}
				if (_Icopyfromprinciple != null)
				{
					_Icopyfromprinciple.ActiveChanged -=new EventHandler(_Icopyfromprinciple_Changed);
					_Icopyfromprinciple = null;
				}
				if (_clienttype != null)
				{
					_clienttype.ActiveChanged -=new EventHandler(eContactPassiveSearch_Type_Changed);
					_clienttype = null;
				}

				if (_title != null)
				{
					_title.Changed -=new EventHandler(eContactPassiveSearch_Name_Changed);
					_title = null;
				}
				if (_intitials != null)
				{
					_intitials.Changed -=new EventHandler(eContactPassiveSearch_Name_Changed);
					_intitials = null;
				}
				if (_surname != null)
				{
					_surname.Changed -=new EventHandler(eContactPassiveSearch_Name_Changed);
					_surname = null;
				}
				_parent.PageChanged -=new PageChangedEventHandler(_parent_PageChanged);
				_parent.PageChanging -=new PageChangingEventHandler(_parent_PageChanging);

                if (_parent.ParentForm != null)
                {
                    _parent.ParentForm.FormClosing -= new FormClosingEventHandler(ParentForm_FormClosing);
                }

				_parent = null;
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

		/// <summary>
		/// Returns the Number of Matches and starts the Auto Display if criteria match.
		/// </summary>
		/// <param name="sender">Search List Object</param>
		/// <param name="e">Returned DataTable</param>
		private void Main_searchlist_Searched(object sender, FWBS.OMS.SearchEngine.SearchedEventArgs e)
		{
			labTotal.Text = _searchlist.ResultCount.ToString();
			_data = e.Data;

			// If the Return Count equals One then start the Display Timer
			if (cnt == null && (vc == null || vc.Visible==false))
			    if (_searchlist.ResultCount <= 2 && _searchlist.ResultCount > 0)
                    timDisplay.Enabled = true;
                else timDisplay.Enabled = false;

			lnkShowMe.Enabled = (_searchlist.ResultCount != 0);
			// If the Panel Is Visible and the Searching return less than 10 then
			if (vc != null && vc.Visible==true && cnt == null && _searchlist.ResultCount < 10)
				timDisplay_Tick(sender,EventArgs.Empty);
		}

		/// <summary>
		/// Not Implemented.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Main_searchlist_Searching(object sender, CancelEventArgs e)
		{ }

		/// <summary>
		/// When text is changed in the Name start the Search Timer.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _IcontNameControlName_Changed(object sender, EventArgs e)
		{
			timSearch.Enabled=false;
			timSearch.Enabled=true;
		}

		/// <summary>
		/// Used by the Search List to get the Parameters.
		/// </summary>
		/// <param name="name">Name of Control on the Enquiry Form</param>
		/// <param name="value">The Return Value</param>
		private void SetParam(string name,out object value)
		{
			try
			{
				if (_searchparams[name] != null)
				{
					if (Convert.ToString(((IBasicEnquiryControl2)_searchparams[name]).Value) == "")
						value = DBNull.Value;
					else
						value = ((IBasicEnquiryControl2)_searchparams[name]).Value;
				}
				else	
					value = DBNull.Value;
			}
			catch
			{
				value = DBNull.Value;
			}
		}

		/// <summary>
		/// In Debug mode break on this exception.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _searchlist_Error(object sender, MessageEventArgs e)
		{
			ErrorBox.Show(ParentForm, e.Exception);
		}

		/// <summary>
		/// Manual Show Me Link Clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lnkShowMe_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			// If Contact is already assigned then Reset
            if (showme == false)
			{
				timSearch.Enabled=false;
				timSearch.Interval = 500000;
				if (cnt != null)
				{
					cnt = null;
					foreach(System.Xml.XmlAttribute xe in _contactconfigsetting.DocCurrent.Attributes)
					{
						IBasicEnquiryControl2 _value = _parent.GetIBasicEnquiryControl2(MappingValue(xe.Value),EnquiryControlMissing.None);	
						if (_value != null)
						{
							_value.Value = DBNull.Value;
							_parent.GetControl(MappingValue(xe.Value)).Enabled = true;
						}
					}
				}
				else if (_IcontAddressControlName.Value != DBNull.Value)
				{
					_IcontAddressControlName.Value = DBNull.Value;
                    _IcontAddressControlName.ReadOnly = false;
				}
                lnkShowMe.Text = Session.CurrentSession.Resources.GetResource("ShowMe", "Show Me", "").Text;
                showme = true;
				timDisplay.Interval = 500;
				timSearch.Interval = 500;
				timSearch.Enabled=false;
				labTotal.Text = "0";
				((Control)_IcontNameControlName).Focus();
			}
			else
				showresults();
		}

		private string MappingValue(string value)
		{
			return value.Split('|')[0];
		}
		
		private string SearchParamValue(string value)
		{
			string[] gvalue = value.Split('|');	
			string retval = "";
			if (gvalue.Length > 2) retval =  gvalue[2]; else retval = "";
			if (retval.StartsWith("%")) retval = retval.TrimStart("%".ToCharArray());
			if (retval.EndsWith("%")) retval = retval.TrimEnd("%".ToCharArray());
			return retval;
		}

		private string GeneralTypeValue(string value)
		{
			string[] gvalue = value.Split('|');	
			if (gvalue.Length > 1) return gvalue[1]; else return "Both";
		}
		
		/// <summary>
		/// Private procedure to Show The Contacts Found.
		/// </summary>
		private void showresults()
		{
            SizeF scaleFactor = new SizeF(DeviceDpi / 96F, DeviceDpi / 96F);
            if (vc == null) 
			{
				vc = new ucVerticalContacts();
                btContact = vc.AddToolbarButton("btContact", Session.CurrentSession.Resources.GetResource("VIEWCONT", "View Contact", "").Text, btContact_Click);
                vc.Scale(scaleFactor);
                vc.Click +=new EventHandler(vc_Click);
				_ptparent = Global.GetParentForm(this);
				if (_ptparent != null)
				{
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
            btContact.Enabled = false;
			if (vc.Contacts.Count > 0) vc.Contacts.Clear();
			if (_data.Rows.Count == 0) return;
			vc.MainPanelVisible = false;
			foreach (DataRow rw in _data.Rows)
			{
				ucVerticalContact vct = new ucVerticalContact();
				foreach (DataRow dr in _searchlist.ListView.Rows)
				{
					if (rw["contid"] == DBNull.Value)
						vct.HeaderColor = SystemColors.Info;
					vct.Add(Convert.ToString(dr["lvdesc"]),rw[Convert.ToString(dr["lvmapping"])],Convert.ToString(dr["lvformat"]),Convert.ToInt32(dr["lvwidth"]));
				}
				vct.Tag = rw;
                vct.Scale(scaleFactor);
				vc.Contacts.Add(vct);
			}
			if (sp1.Visible==false)
				_ptparent.Width += (vc.Width + sp1.Width);
			vc.MainPanelVisible = true;
			sp1.Visible=true;
			vc.Visible=true;

		}

		/// <summary>
		/// Quick Access to the Convert.ToString Method.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private string Str(object value)
		{
			return Convert.ToString(value).Trim();
		}

		/// <summary>
		/// If the Close Gadget is Pressed then Hide.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void vc_Close(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// If the Accept Button is Pressed then Assign the Contact to this Object.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void vc_Accept(object sender, EventArgs e)
		{
			DataRow dr = vc.Selected.Tag as DataRow;
			if (dr != null)
			{
				if (dr["contID"] == DBNull.Value)
				{
					if (dr["addid"] != DBNull.Value)
					{
                        lnkShowMe.Text = Session.CurrentSession.Resources.GetResource("Reset", "&Reset", "").Text;
                        showme = false;
                        _IcontAddressControlName.Value = dr["addid"];
                        _IcontAddressControlName.ReadOnly = true;
						vc_Close(sender,e);
						timSearch.Enabled = false;
						timDisplay.Enabled = false;
					}
				}
				else
				{
                    long contid = Convert.ToInt64(dr["contID"]);
                    Populate(contid);
				}
			}
		}

		/// <summary>
		/// Run the SearchList Thread.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timSearch_Tick(object sender, System.EventArgs e)
		{
			timSearch.Enabled=false;
			_searchlist.Search(true);
		}

		/// <summary>
		/// Display the Contact Found Automatically.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timDisplay_Tick(object sender, System.EventArgs e)
		{
			if (this.Visible)
			{
				timDisplay.Enabled=false;
				showresults();
			}
		}

		/// <summary>
		/// If the Control is changed Visibility.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void eContactPassiveSearch_VisibleChanged(object sender, System.EventArgs e)
		{
			if (!this.Visible)
			{
				// If the Control is Hidden then stop tha autodisplay feature	
				timDisplay.Enabled=false;

                // Remove the Required and Error Symbols
                if (_IcontNameControlName != null)
				{
					req.SetError((Control)_IcontNameControlName,"");
                    err.SetError((Control)_IcontNameControlName,"");
				}
				if (_IcontAddressControlName != null)
				{
					_IcontAddressControlName.ErrorIconsOn(false);
					_IcontAddressControlName.RequiredIconsOn(false);
				}

				// Close the Contact Control if Visible
				vc_Close(sender,e);
				if (_parent != null)
					_parent.PageChanging -=new PageChangingEventHandler(_parent_PageChanging);
			}
			else
			{
				if (_parent != null)
				{
					_parent.PageChanging -=new PageChangingEventHandler(_parent_PageChanging);
					_parent.PageChanging +=new PageChangingEventHandler(_parent_PageChanging);
                    if (_parent.ParentForm != null)
                    {
                        _parent.ParentForm.FormClosing -= new FormClosingEventHandler(ParentForm_FormClosing);
                        _parent.ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);
                    }
				}
				
				_IcontAnotherContactControlName_Changed(sender, e);
																	  
				// Show the Required Fields
				if (_IcontAnotherContactControlName == null || _IcontAnotherContactControlName != null && Convert.ToBoolean(_IcontAnotherContactControlName.Value) == true)
					ErrorsOff();
			}
		}

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Close();
        }

		/// <summary>
		/// If the Magnafiying Glass is clicked then Show Found Contacts.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void picFind_Click(object sender, System.EventArgs e)
		{
			if (lnkShowMe.Enabled)
				showresults();
		}

		/// <summary>
		/// If the Another Contact Check Box is available show or hide.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _IcontAnotherContactControlName_Changed(object sender, EventArgs e)
		{
			if (_IcontAnotherContactControlName != null)
			{
				this.Enabled = _IcontAnotherContactControlName.Checked;
				foreach(System.Xml.XmlAttribute xe in _contactconfigsetting.DocCurrent.Attributes)
				{
					try
					{
                        if (MappingValue(xe.Value) != null &&
                            _parent.GetSettings(MappingValue(xe.Value)) != null &&
                            Convert.ToString(_parent.GetSettings(MappingValue(xe.Value))["quPage"]) == _parent.PageName)
                        {
                            _parent.GetControl(MappingValue(xe.Value)).Visible = Convert.ToBoolean(_IcontAnotherContactControlName.Value);
                        }
					}
					catch
					{
					}
				}
			}
		}

		/// <summary>
		/// When then Control is Fully Loaded attach Tenticles to Host and bleed dry.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void eContactPassiveSearch_Load(object sender, System.EventArgs e)
		{
			if (_parent != null && _parent.Enquiry.InDesignMode == false)
			{
				_parent.PageChanged +=new PageChangedEventHandler(_parent_PageChanged);
				_parent.PageChanging +=new PageChangingEventHandler(_parent_PageChanging);

				if (_clienttype == null)
				{
					
					try
					{
						_clienttype = _parent.GetIBasicEnquiryControl2("_type",EnquiryControlMissing.Exception);
						_clienttype.Changed +=new EventHandler(eContactPassiveSearch_Type_Changed);
						eContactPassiveSearch_Type_Changed(sender,e);
					}
					catch{}
				}
				
				_IcontNameControlName = _parent.GetIBasicEnquiryControl2(MappingValue(_contactconfigsetting.GetString("Name","")),EnquiryControlMissing.Exception);
				
				foreach(System.Xml.XmlAttribute xe in _contactconfigsetting.DocCurrent.Attributes)
				{
					
					// Builds a List of Replace Search Params
					if (SearchParamValue(xe.Value) != "")
					{
						if (_parent.GetControl(MappingValue(xe.Value)) is eAddress)
						{
							string[] retvals = SearchParamValue(xe.Value).Split(",".ToCharArray());
							eAddress addcontrol = _parent.GetControl(MappingValue(xe.Value)) as eAddress;
							foreach (string rvals in retvals)
								_searchparams.Add(rvals,addcontrol.GetIBasicEnquiryControl2(rvals));
							_IcontAddressControlName = addcontrol;
						}
						else if (_searchparams.Contains(SearchParamValue(xe.Value)) == false)
							_searchparams.Add(SearchParamValue(xe.Value),_parent.GetIBasicEnquiryControl2(MappingValue(xe.Value)));
						else
							ErrorBox.Show(ParentForm, new OMSException2("ERRALREADYMP","Passive Search - Parameter Mapping - '%1% is already mapped to another Control. Please modify within the Designer",new Exception(),true,SearchParamValue(xe.Value)));

						_parent.GetIBasicEnquiryControl2(MappingValue(xe.Value)).ActiveChanged +=new EventHandler(_IcontNameControlName_Changed);

					}
				}

				_IcontAnotherContactControlName = _parent.GetControl(_contAnotherContactControlName) as FWBS.Common.UI.Windows.eCheckBox2;

				_searchlist = new FWBS.OMS.SearchEngine.SearchList(_searchlistcode,null,new FWBS.Common.KeyValueCollection());
				_searchlist.Searched +=new FWBS.OMS.SearchEngine.SearchedEventHandler(_searchlist_Searched);
				_searchlist.Searching +=new CancelEventHandler(_searchlist_Searching);
				_searchlist.ParameterHandler += new SourceEngine.SetParameterHandler(this.SetParam);
				_searchlist.Error +=new MessageEventHandler(_searchlist_Error);
					
				if (_IcontAnotherContactControlName != null) 
				{
					_IcontAnotherContactControlName_Changed(sender,e);
					_IcontAnotherContactControlName.ActiveChanged +=new EventHandler(_IcontAnotherContactControlName_Changed);
				}

				if (_IcontNameControlName!= null)
                {
                    //Mark related control as required
                    _reqFieldRenderer.MarkRequiredControl((Control)_IcontNameControlName);
                }
		
				if (_IcontAddressControlName!= null)
					_IcontAddressControlName.RequiredIconsOn(true);

				_Icopyfromprinciple = _parent.GetControl(this.IsAddressSameAsPrinciple) as FWBS.Common.UI.Windows.eCheckBox2;
				if (_Icopyfromprinciple != null)
					_Icopyfromprinciple.ActiveChanged += new EventHandler(_Icopyfromprinciple_Changed);

				foreach(System.Xml.XmlAttribute xe in _contactconfigsetting.DocCurrent.Attributes)
				{
					try
					{
						if (xe.Name.ToUpper() == "EXTCONTINDIV.CONTTITLE")
						{
							_title = _parent.GetIBasicEnquiryControl2(MappingValue(xe.Value));
							_title.Changed +=new EventHandler(eContactPassiveSearch_Name_Changed);
						}
						if (xe.Name.ToUpper() == "EXTCONTINDIV.CONTINITIALS")
						{
							_intitials = _parent.GetIBasicEnquiryControl2(MappingValue(xe.Value));
							_intitials.Changed +=new EventHandler(eContactPassiveSearch_Name_Changed);
						}
						if (xe.Name.ToUpper() == "EXTCONTINDIV.CONTSURNAME")
						{
							_surname = _parent.GetIBasicEnquiryControl2(MappingValue(xe.Value));
							_surname.Changed +=new EventHandler(eContactPassiveSearch_Name_Changed);
						}					
					}
					catch{}
				}
			}
		}

        /// <summary>
        /// If the Enquiry Form Page Change Event is fired check the Visibility of this Content
        /// e.g. Hide all Required and Errors Symbols.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _parent_PageChanged(object sender, PageChangedEventArgs e)
		{
			try
			{
				foreach(System.Xml.XmlAttribute xe in _contactconfigsetting.DocCurrent.Attributes)
				{
					try
					{
						if (GeneralTypeValue(xe.Value) != "Both" && GeneralTypeValue(xe.Value) != _generaltype.ToString())
							_parent.GetControl(MappingValue(xe.Value)).Visible=false;
					}
					catch{}
				}
			}
			catch{}
			eContactPassiveSearch_VisibleChanged(sender,EventArgs.Empty);
		}

		private void lnkFind_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			FWBS.Common.KeyValueCollection returnvals = null;
			returnvals = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(Session.CurrentSession.DefaultSystemSearchListGroups(FWBS.OMS.SystemSearchListGroups.ContactConflict),null,new FWBS.Common.KeyValueCollection());
			if (returnvals != null)
			{
				if (_IcontAnotherContactControlName != null)
				{
					_parent.Enquiry.SetValue(_IcontAnotherContactControlName.Name,true);
					_IcontAnotherContactControlName_Changed(sender,EventArgs.Empty);
					
				}
                long contid = Convert.ToInt64(returnvals["contID"].Value);
                Populate(contid);
			}
		}

        /// <summary>
        /// Populates the Enquiry Form with the Contact.
        /// </summary>
        /// <param name="contid">The Contact ID</param>
        private void Populate(long contid)
        {
            cnt = Contact.GetContact(contid);
            if (cnt.ContactTypeCode != this.ContactTypeName && String.IsNullOrEmpty(this.ContactTypeName) == false)
            {
                ErrorBox.Show(ParentForm, new OMSException2("ERRWRONGCONT", "Incorrect contact type please choose a contact type of %1%.", new Exception(), false, this.ContactTypeName)); 
                return;
            }
			timSearch.Interval = 500000;
			timDisplay.Interval = 500000;
			foreach(System.Xml.XmlAttribute xe in _contactconfigsetting.DocCurrent.Attributes)
			{
				object output = null;
				try
				{
					IBasicEnquiryControl2 objval = _parent.GetIBasicEnquiryControl2(MappingValue(xe.Value));
					if (objval != null)
					{
						if (xe.Name.IndexOf(".") > 0)
						{
							string[] extdata = xe.Name.Split('.');
							output = cnt.ExtendedData[extdata[0]].GetExtendedData(extdata[1]);
						}
						else
							output = cnt.GetType().GetProperty(xe.Name).GetValue(cnt,null);
						//DMB 17/5/2004 added to leave inputted values if underlying values are empty.
						if(Convert.ToString(output) !="")
						{
							_parent.GetIBasicEnquiryControl2(MappingValue(xe.Value)).Value = output;
							_parent.GetIBasicEnquiryControl2(MappingValue(xe.Value)).OnActiveChanged();
							_parent.GetIBasicEnquiryControl2(MappingValue(xe.Value)).OnChanged();
							_parent.GetControl(MappingValue(xe.Value)).Enabled = false;
						}
						
						if (Convert.ToString(_parent.GetIBasicEnquiryControl2(MappingValue(xe.Value)).Value) == "")
							_parent.GetControl(MappingValue(xe.Value)).Enabled = true;
						try
						{
							if (Convert.ToInt32(_parent.GetIBasicEnquiryControl2(MappingValue(xe.Value)).Value) == 0)
								_parent.GetControl(MappingValue(xe.Value)).Enabled = true;
						}
						catch
						{}
					}
				}
				catch(Exception ex)
				{
					if (xe.Name.ToLower().StartsWith("none") == false)
					{
						string extorprop = "";
						if (xe.Name.IndexOf(".") > 0) extorprop = "Extended Data"; else extorprop = "Property";
						ErrorBox.Show(ParentForm, new OMSException2("ERRCPSLOAD","Error Populating Control '%1%' from %2% called '%3%' with value '%4%'",ex,true,MappingValue(xe.Value),extorprop,xe.Name,Convert.ToString(output)));
					}
					else
					{
						_parent.GetControl(MappingValue(xe.Value)).Enabled = false;
					}
				}
			}
			vc_Close(this,EventArgs.Empty);
			if (_enablefinsihedonaccept && _parent.ActionFinish != null)
				_parent.ActionFinish.Enabled =true;
            lnkShowMe.Text = Session.CurrentSession.Resources.GetResource("Reset", "&Reset", "").Text;
            showme = false;
            timSearch.Enabled = false;
			timDisplay.Enabled = false;
       }

		private void _parent_PageChanging(object sender, PageChangingEventArgs e)
		{
			if (_IcontAnotherContactControlName != null && e.Direction == EnquiryPageDirection.Next && _contactgotopageifnotrequired != "")
			{
				if (FWBS.Common.ConvertDef.ToBoolean(_IcontAnotherContactControlName.Value,false) == false)
				{
					_parent.GotoPage(_contactgotopageifnotrequired,false,false);
					e.Cancel=true;
				}
				else
					_parent.ClearForwardHistory();
			}
		}

		private void _Icopyfromprinciple_Changed(object sender, EventArgs e)
		{
			_IcontAddressControlName.Enabled = !Convert.ToBoolean(_Icopyfromprinciple.Value);
		}

		private void eContactPassiveSearch_Name_Changed(object sender, EventArgs e)
		{
			string contname = Convert.ToString(_title.Value).Trim() + " " + Convert.ToString(_intitials.Value).Trim() + " " + Convert.ToString(_surname.Value).Trim();
			_IcontNameControlName.Value = contname.Trim();
			_IcontNameControlName.IsDirty = true;
			_IcontNameControlName.OnActiveChanged();
		}

		private void eContactPassiveSearch_Type_Changed(object sender, EventArgs e)
		{
			Clear();
			string type = "";
			type = Convert.ToString(_clienttype.Value);
			ClientType r = ClientType.GetClientType(type);
			ContactType c = ContactType.GetContactType(r.ContactType); 
			_generaltype = (OMSTypeContactGeneralType)FWBS.Common.ConvertDef.ToEnum(c.GeneralType,OMSTypeContactGeneralType.Individual);
		}

		private void eContactPassiveSearch_EnabledChanged(object sender, System.EventArgs e)
		{
			lnkFind.Visible = this.Enabled;
			picFind.Visible = this.Enabled;
			labTotal.Visible = this.Enabled;
			label1.Visible = this.Enabled;
			lnkShowMe.Visible = this.Enabled;
		}

		private void btContact_Click(object sender, EventArgs e)
        {
			if (vc.Selected.Tag is DataRow && ((DataRow)vc.Selected.Tag).Table.Columns.Contains("contid"))
			{
				DataRow dr = (DataRow)vc.Selected.Tag;
				if (dr["contid"] != DBNull.Value)
					FWBS.OMS.UI.Windows.Services.ShowContact(FWBS.OMS.Contact.GetContact(Convert.ToInt64(dr["contid"])));
			}
			else
				ErrorBox.Show(ParentForm, new OMSException2("ERRNOCONTID","Error no 'contid' field in the Search Lists Results",new Exception(),false));
		}

		private void vc_Click(object sender, EventArgs e)
		{
			DataRow dr = vc.Selected.Tag as DataRow;
			btContact.Enabled = (dr != null && dr["contid"] != DBNull.Value);
		}
		#endregion
	}

	public enum ContactGeneralType {Both, Individual, Company}
	
	/// <summary>
	/// Internal Desinger for the Mapping of the Contact Properties.
	/// </summary>
	internal class ContactMapping
	{
		#region Fields
		/// <summary>
		/// The Control on the Enquiry Form.
		/// </summary>
		private string _controlmappingname;
		/// <summary>
		/// The Property in the Contact Object.
		/// </summary>
		private string _propertyname;
		/// <summary>
		/// The Enquiry Form Containing the eContactPassiveSearch.
		/// </summary>
		private EnquiryForm _enquiryform;
		/// <summary>
		/// The General Contact Type.
		/// </summary>
		private ContactGeneralType _type;
		/// <summary>
		/// Which Search Parameter to Use.
		/// </summary>
		private string _searchmapping;
		/// <summary>
		/// The Search List Object.
		/// </summary>
		private SearchList _searchlist;
		#endregion

		#region Constructors
		/// <summary>
		/// Contructor to Create a Contact Mapping Object.
		/// </summary>
		/// <param name="EnquiryForm">The Enquiry Form Connected</param>
		/// <param name="PropertyName">The Property Name</param>
		/// <param name="ControlMappingName">The Control name on the Enquiry Form</param>
		public ContactMapping(EnquiryForm EnquiryForm, SearchList SearchList, string PropertyName, string ControlMappingName, ContactGeneralType type, string SearchMapping)
		{
			_propertyname = PropertyName;
			_controlmappingname = ControlMappingName;
			_type = type;
			_enquiryform = EnquiryForm;
			_searchmapping = SearchMapping;
			_searchlist = SearchList;
		}

		/// <summary>
		/// Overrides the To String to Display a Friendly Name in the Designer.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _propertyname + " | " + _controlmappingname;
		}

		#endregion

		#region Properties
		/// <summary>
		/// Gets or Set the Property Name from the Contact List.
		/// </summary>
		[TypeConverter(typeof(ContactPropertyLister))]
		public string PropertyName
		{
			get
			{
				return _propertyname;
			}
			set
			{
				_propertyname = value;
			}
		}

		/// <summary>
		/// The Control Name within the Designer to Connect To.
		/// </summary>
		[TypeConverter(typeof(ContactEnquiryControlLister))]
		public string ControlMappingName
		{
			get
			{
				return _controlmappingname;
			}
			set
			{
				_controlmappingname = value;
			}
		}

		public ContactGeneralType GeneralType
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		public string SearchMappingName
		{
			get
			{
				return _searchmapping;
			}
			set
			{
				_searchmapping = value;
			}
		}


		
		/// <summary>
		/// The Enquiry Form used by the Designers to get the Controls.
		/// </summary>
		[Browsable(false)]
		public EnquiryForm EnquiryForm
		{
			get
			{
				return _enquiryform;
			}
		}

		/// <summary>
		/// The Search List Object used by the Designers to get the Parameters.
		/// </summary>
		[Browsable(false)]
		public SearchList SearchList
		{
			get
			{
				return _searchlist;
			}
		}
		#endregion
	}

	/// <summary>
	/// A Internal Collection of Contact Mapping Objects.
	/// </summary>
	internal class ContactMappingCollection : Crownwood.Magic.Collections.CollectionWithEvents
	{
		#region Collection Methods
		public ContactMapping Add(ContactMapping value)
		{
			// Use base class to process actual collection operation
			base.List.Add(value as object);

			return value;
		}

		public void AddRange(ContactMapping[] values)
		{
			// Use existing method to add each array entry
			foreach(ContactMapping page in values)
				Add(page);
		}

		public void Remove(ContactMapping value)
		{
			// Use base class to process actual collection operation
			base.List.Remove(value as object);
		}

		public void Insert(int index, ContactMapping value)
		{
			// Use base class to process actual collection operation
			base.List.Insert(index, value as object);
		}

		public bool Contains(ContactMapping value)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(value as object);
		}

		public ContactMapping this[int index]
		{
			// Use base class to process actual collection operation
			get { return (base.List[index] as ContactMapping); }
		}

		public int IndexOf(ContactMapping value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}
		#endregion
	}

	
	/// <summary>
	/// A Designer used by the Admin Kit to help Create the Mapping XML.
	/// </summary>
	internal class ContactMappingEditor : System.ComponentModel.Design.CollectionEditor
	{
		#region Fields
		/// <summary>
		/// A XML String used to turn from XML to the Collection and Back.
		/// </summary>
		private FWBS.Common.ConfigSetting _value;
		/// <summary>
		/// A Mapping Collection used by this Designer to containg the Mapping Objects.
		/// </summary>
		private ContactMappingCollection _contactmapping;
		
		/// <summary>
		/// A Contructor to Create the Contact Mapping Editor.
		/// </summary>
		public ContactMappingEditor() : base (typeof(ContactMappingCollection)) 
		{
			_contactmapping = new ContactMappingCollection();
		}
		#endregion

		#region Overrides
		/// <summary>
		/// Overrides the Natural Design of the Collection Editor to return a XML String.
		/// </summary>
		/// <param name="context">The Object Contect</param>
		/// <param name="provider">Don't Now</param>
		/// <param name="value">The XML String to Process</param>
		/// <returns>Returns the XML String</returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			try
			{
				FWBS.OMS.UI.Windows.EnquiryForm enq = (FWBS.OMS.UI.Windows.EnquiryForm)TypeDescriptor.GetProperties(context.Instance)["EnquiryForm"].GetValue(context.Instance);
				SearchList sch = (SearchList)TypeDescriptor.GetProperties(context.Instance)["SearchList"].GetValue(context.Instance);
				_value = new FWBS.Common.ConfigSetting(Convert.ToString(value));

				foreach(System.Xml.XmlAttribute xe in _value.DocCurrent.Attributes)
				{
					string[] xsvalue = xe.Value.Split('|');
					string svalue = xsvalue[0];
					string stype = "Both";
					string ssearch = "";
					if (xsvalue.Length > 1) stype = xsvalue[1];
					if (xsvalue.Length > 2) ssearch = xsvalue[2];
					_contactmapping.Add(new ContactMapping(enq,sch, xe.Name,svalue,(ContactGeneralType)FWBS.Common.ConvertDef.ToEnum(stype,ContactGeneralType.Both),ssearch));
				}
				object _valueout = base.EditValue (context, provider, _contactmapping);
				_value = new FWBS.Common.ConfigSetting("");
				foreach (ContactMapping cm in _contactmapping)
				{
					_value.SetString(cm.PropertyName,cm.ControlMappingName + "|" + cm.GeneralType.ToString() + "|" + cm.SearchMappingName);
				}
				_contactmapping.Clear();
				return _value.DocObject.OuterXml;
			}
			catch (Exception ex)
			{
                ErrorBox.Show(new Exception(Session.CurrentSession.Resources.GetMessage("ERRPRCXMLFRCTRL", "ERROR Processing XML for the Control try cutting out the XML saving quiting the admin kit and the re-open and paste the XML Code back", "").Text, ex));
				return null;
			}
		}

		/// <summary>
		/// What Object Should be Created when the New Button is Clicked.
		/// </summary>
		/// <returns></returns>
		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (ContactMapping);
		}

		/// <summary>
		/// How to Create a New ContactMapping Object.
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		protected override object CreateInstance(System.Type t)
		{
			// The Resson why the Enquiry Form Reference is Public in the eContactPassiveSearch
			FWBS.OMS.UI.Windows.EnquiryForm enq = (FWBS.OMS.UI.Windows.EnquiryForm)TypeDescriptor.GetProperties(this.Context.Instance)["EnquiryForm"].GetValue(this.Context.Instance);
			SearchList sch = (SearchList)TypeDescriptor.GetProperties(this.Context.Instance)["SearchList"].GetValue(this.Context.Instance);
			ContactMapping cm = new ContactMapping(enq,sch, "","",ContactGeneralType.Both,"");
			_contactmapping.Add(cm);
			return cm;
		}

		/// <summary>
		/// What Object Should be Created when the New Button is Clicked.
		/// </summary>
		/// <returns></returns>
		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(ContactMapping)};
		}
		#endregion
	}

	/// <summary>
	/// A Desinger to List the Parameters from the Search List.
	/// </summary>
	internal class SearchListParameterLister : StringConverter
	{
		#region Overrides
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			ArrayList controls = new ArrayList();
			try
			{
				FWBS.OMS.SearchEngine.SearchList search = (FWBS.OMS.SearchEngine.SearchList)TypeDescriptor.GetProperties(context.Instance)["SearchList"].GetValue(context.Instance);
				controls.Add("");
				foreach(FWBS.Common.KeyValueItem p in search.ReplacementParameters)
					controls.Add(p.Key);
			}
			catch
			{}

			string[] vals;
			vals = new string[controls.Count];
			controls.CopyTo(vals);
			return new StandardValuesCollection(vals);
		}

		/// <summary>
		/// Must be a Match.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}
		#endregion
	}

	/// <summary>
	/// A Desinger to List the Enquiry Controls within the Enquiry Form.
	/// </summary>
	internal class ContactEnquiryControlLister : StringConverter
	{
		#region Overrides
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			ArrayList controls = new ArrayList();
			try
			{
				FWBS.OMS.UI.Windows.EnquiryForm enq = (FWBS.OMS.UI.Windows.EnquiryForm)TypeDescriptor.GetProperties(context.Instance)["EnquiryForm"].GetValue(context.Instance);
				controls.Add("");
				foreach(Control ctrl in enq.Controls)
					controls.Add(ctrl.Name);
			}
			catch
			{}
			controls.Sort();
			string[] vals;
			vals = new string[controls.Count];
			controls.CopyTo(vals);
			return new StandardValuesCollection(vals);
		}

		/// <summary>
		/// Must be a Match.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}
		#endregion
	}

	/// <summary>
	/// The Contact Property List.
	/// </summary>
	internal class ContactPropertyLister : StringConverter
	{
		#region Overrides
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			EnquiryEngine.EnquiryPropertyCollection props = EnquiryEngine.Enquiry.GetObjectProperties(typeof(Contact));
			ArrayList aprops = new ArrayList();
            foreach (EnquiryEngine.EnquiryProperty prop in props)
				aprops.Add(prop.Name);
			aprops.Add("none");
			aprops.Add("IsAddressSameAsPrinciple");
			string[] vals;
			vals = new string[aprops.Count];
			aprops.CopyTo(vals);
			return new StandardValuesCollection(vals);
		}

		/// <summary>
		/// Must be a Match.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}
		#endregion
	}
}		

