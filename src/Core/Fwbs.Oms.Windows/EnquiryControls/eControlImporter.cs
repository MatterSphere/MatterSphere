using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using FWBS.Common.UI;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for eControlImporter.
    /// </summary>
    public class eControlImporter : System.Windows.Forms.UserControl, FWBS.Common.UI.IBasicEnquiryControl2
	{
		private System.Windows.Forms.Button btnImport;
		private FWBS.OMS.UI.Windows.ResourceLookup _res;
		private System.Windows.Forms.OpenFileDialog dlg1;
		private System.ComponentModel.IContainer components;
		
		/// <summary>
		///	Array lists for holding Type information 
		/// </summary>
		private string _ctrlType = "";
		/// <summary>
		/// Array List for holding the corresponding control codes
		/// </summary>
		private string _ctrlCode = "";
		/// <summary>
		/// Array list for holding corresponding control categories
		/// </summary>
		private string _ctrlGroup = "";
		/// <summary>
		/// Dataview of control records to prevent round trips to database
		/// </summary>
		private DataView _dvCtrl = null;
		
		/// <summary>
		/// Variables to hold a reference to the 3 controls that will map to values within this object
		/// </summary>
		private FWBS.Common.UI.IBasicEnquiryControl2 _typecontrol;
		private FWBS.Common.UI.IBasicEnquiryControl2 _codecontrol;
		private FWBS.Common.UI.IBasicEnquiryControl2 _groupcontrol;
		
		/// <summary>
		/// The names of the 3 controls that are mapped to properties within this control
		/// </summary>
		private string _typectrlname;
		private string _codectrlname;
		private string _groupctrlname;

		private Type[] _types = null;
		private System.Windows.Forms.ListBox lstTypes;
		private System.Windows.Forms.Label lblNote;
        private TableLayoutPanel tableLayoutPanel;
        private EnquiryForm _parent;

		public eControlImporter()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.btnImport = new System.Windows.Forms.Button();
            this._res = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.lblNote = new System.Windows.Forms.Label();
            this.dlg1 = new System.Windows.Forms.OpenFileDialog();
            this.lstTypes = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.AutoSize = true;
            this.btnImport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnImport.Location = new System.Drawing.Point(3, 3);
            this._res.SetLookup(this.btnImport, new FWBS.OMS.UI.Windows.ResourceLookupItem("IMPORTCTRL", "Import Control", ""));
            this.btnImport.Name = "btnImport";
            this.btnImport.Padding = new System.Windows.Forms.Padding(1);
            this.btnImport.Size = new System.Drawing.Size(88, 24);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import Control";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // lblNote
            // 
            this.lblNote.Location = new System.Drawing.Point(3, 30);
            this._res.SetLookup(this.lblNote, new FWBS.OMS.UI.Windows.ResourceLookupItem("DBLCLKIMP", "Double click control to import", ""));
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(90, 49);
            this.lblNote.TabIndex = 2;
            this.lblNote.Text = "double click to import";
            this.lblNote.Visible = false;
            // 
            // dlg1
            // 
            this.dlg1.Filter = "Libraries|*.dll";
            // 
            // lstTypes
            // 
            this.lstTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTypes.IntegralHeight = false;
            this.lstTypes.Location = new System.Drawing.Point(99, 3);
            this.lstTypes.Name = "lstTypes";
            this.tableLayoutPanel.SetRowSpan(this.lstTypes, 2);
            this.lstTypes.Size = new System.Drawing.Size(472, 79);
            this.lstTypes.TabIndex = 1;
            this.lstTypes.Visible = false;
            this.lstTypes.DoubleClick += new System.EventHandler(this.lstTypes_DoubleClick);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.lstTypes, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.lblNote, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.btnImport, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(574, 85);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // eControlImporter
            // 
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "eControlImporter";
            this.Size = new System.Drawing.Size(574, 85);
            this.ParentChanged += new System.EventHandler(this.eControlImporter_ParentChanged);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion
		
		#region Properties
		
		
		[TypeConverter(typeof(ContactEnquiryControlLister))]
		[LocCategory("OMS")]
		public string TypeControlName
		{
			get
			{
				return _typectrlname;
			}
			set
			{
				_typectrlname = value;
			}
		}
		
		
		[TypeConverter(typeof(ContactEnquiryControlLister))]
		[LocCategory("OMS")]
		public string CodeControlName
		{
			get
			{
				return _codectrlname;
			}
			set
			{
				_codectrlname = value;
			}
		}
		
		
		[TypeConverter(typeof(ContactEnquiryControlLister))]
		[LocCategory("OMS")]
		public string GroupControlName
		{
			get
			{
				return _groupctrlname;
			}
			set
			{
				_groupctrlname = value;
			}
		}

		[Browsable(false)]
		public EnquiryForm EnquiryForm
		{
			get
			{
				try
				{
					return (EnquiryForm)this.Parent;
				}
				catch
				{return null;}
			}
		}


		#endregion
		
		#region Methods
		
		/// <summary>
		/// Performs the main import routine when the button is clicked
		/// </summary>
		private void btnImport_Click(object sender, System.EventArgs e)
		{
			//clear and hide the listbox
			lstTypes.Items.Clear();
			lstTypes.Visible = false;
			lblNote.Visible = false;
			
			DialogResult res = dlg1.ShowDialog();
			
			if(res != DialogResult.OK)
				return;
			try
			{
			
				if (dlg1.FileName != "")
				{
					//get assembly from the filename
                    Assembly a = Session.CurrentSession.AssemblyManager.LoadFrom(dlg1.FileName);
				
					//get the array of types frommthe control
					_types = a.GetTypes();

					//if there is more than 1
					if(_types.Length > 1 )
					{
						for(int i = 0;i < _types.Length;i++)
						{
							lstTypes.Items.Add(_types[i].AssemblyQualifiedName);
						}
						lstTypes.Visible = true;
						lblNote.Visible = true;
					}
					else
					{
						GetInformationFromType(0);
						PopulateControlValues();
					}
				}
			}
			catch
			{
				throw new OMSException2("CTRLIMPERR",@"Unable to get information from library file.  Please make sure the file is valid for this operation.");
			}
		}
		
		
		/// <summary>
		/// populates internal control values checking if they are referenced
		/// </summary>
		private void PopulateControlValues()
		{
			if(_typecontrol != null)
                ((IBasicEnquiryControl2)_typecontrol).Value = _ctrlType;
			
			if(_codecontrol != null)
				((IBasicEnquiryControl2)_codecontrol).Value = _ctrlCode;

			if(_groupcontrol != null)
				((IBasicEnquiryControl2)_groupcontrol).Value = _ctrlGroup;
		}

		/// <summary>
		/// Generates the code and group code for the control from its attributes or generates if no attribute
		/// </summary>
		/// <param name="index">index within the array</param>
		private void GetInformationFromType(int index)
		{
			bool ret = false;
			
			Type t = _types[index];
					
			if(t.GetInterface(typeof(FWBS.Common.UI.IBasicEnquiryControl2).FullName) != null)
			{
				// import into our library
				_ctrlType = t.AssemblyQualifiedName;
						
				// get all custom attributes from the type
				object[] attribs = t.GetCustomAttributes(false);
						
				//reset control code and category variables
				_ctrlCode = "";
				_ctrlGroup = "";

				// firstly iterate all attributes looking for a category code
				for (int ia = 0; ia < attribs.Length; ia++)
				{
					if(attribs[ia].GetType() == typeof(FWBS.Common.UI.ControlCodeAttribute))
					{
						// capture the control code from the attribute
						_ctrlGroup = ((FWBS.Common.UI.ControlCodeAttribute)attribs[ia]).ControlCategory;
						break;
					}
				}
				// if no category has been specified then default to OMS
				if(_ctrlGroup == "")
					_ctrlGroup = "OMS";

				// iterate all atributes looking for control codes 
				for (int ib = 0; ib < attribs.Length; ib++)
				{
					// check if the attribute is Control code attribute 
					if(attribs[ib].GetType() == typeof(FWBS.Common.UI.ControlCodeAttribute))
					{
						// capture the control code from the attribute
						_ctrlCode = ((FWBS.Common.UI.ControlCodeAttribute)attribs[ib]).ControlCode;
						
						//check if the code exists 
						ret = CheckCodeExists(_ctrlCode, _ctrlGroup);
						break;
					}
				}
				//if the code already exists generate a new one based upon the type name
				if(ret == true || _ctrlCode == "")
				{
					_ctrlCode = GenerateNewControlCode(_ctrlType,_ctrlGroup);
					// if an empty string is returned throw an exception as the name cannot be used
					if(_ctrlCode == "")
					{
						throw new OMSException2("CTRLCODEERR",@"Control's Code cannot be used. contact your system administrator.");
					}

				}
				
				//set the data view back to null to force refresh
				_dvCtrl = null;
			}
			else
			{
				throw new OMSException2("CTRLCODEINTF",@"Control cannot be used as it does not implement IEnquiryControl2.");
			}
		}

			
		/// <summary>
		/// Checks to see if a control code and group combination already exists
		/// </summary>
		/// <param name="ctrlcode">the code read from the attribute</param>
		/// <returns>true if it already exists</returns>
		private bool CheckCodeExists(string ctrlcode, string category)
		{
			//generate a filter to apply to the view
			string filter = "ctrlCode = '" + ctrlcode + "' and ctrlGroup = '" + category + "'";
			
			//check if we have retrived the view in this iteration
			if(_dvCtrl == null)
				_dvCtrl = FWBS.OMS.EnquiryEngine.Enquiry.GetEnquiryControls();
         	
			//apply the filter
			_dvCtrl.RowFilter = filter;
			
			//if any rows exist return true that code already exists
			if(_dvCtrl.Count > 0)
				return true;
			else
				return false;
		}
		
		
		/// <summary>
		/// Generates a new control code if it was not supplied or the supplied one already exists
		/// </summary>
		/// <param name="typename">Name of the type to generate the code for</param>
		/// <returns>new generated code</returns>
		private string GenerateNewControlCode(string typename,string category)
		{
			// the logic here is to string the type name to 12 chartacters
			//then check if this exists and if it does add 1 to the end of the name
			//right up to 999 for example MYCUSTCONTRL999 would be the last name possible
			//Hopefully it will never come to the point when we have to exceed 999
			int counter = 0;
			string tempname;
			
			//genearte an uppercase string without full stops and commas
			string cleanname = typename.Replace(".","");
			cleanname = cleanname.Replace(",","");
			cleanname = cleanname.Substring(0,12).ToUpper();

			//start off with the first 12 characters of the typename
			tempname = cleanname;
	        			
			//loop around until 999 is reached - hopefully never
			do 
			{
				if(CheckCodeExists(tempname,category) == false)
				{
					break;
				}
				else
				{
					counter ++;
					tempname = cleanname + Convert.ToString(counter);
				}
			}while(counter < 1000);
			
			//this really shouldnt happen but just in case we have 999 previous controls
			if(tempname.Length > 15)
				tempname = "";

			return tempname;
		}

		
		/// <summary>
		/// runs when the form is running properly and references controls on the enquiry form
		/// </summary>
		private void eControlImporter_ParentChanged(object sender, System.EventArgs e)
		{
			if (Parent is EnquiryForm)
			{
				_parent = Parent as EnquiryForm;
				if (_parent != null && _parent.Enquiry.InDesignMode == false)
				{
					_typecontrol = _parent.GetIBasicEnquiryControl2(_typectrlname,EnquiryControlMissing.Exception);
					_codecontrol = _parent.GetIBasicEnquiryControl2(_codectrlname,EnquiryControlMissing.Exception);
					_groupcontrol = _parent.GetIBasicEnquiryControl2(_groupctrlname,EnquiryControlMissing.Exception);
				}
			}
		}
		
		/// <summary>
		/// Selection event for items in the list
		/// </summary>
		private void lstTypes_DoubleClick(object sender, System.EventArgs e)
		{
			GetInformationFromType(lstTypes.SelectedIndex);	
			lstTypes.Items.Clear();
			lstTypes.Visible = false;
			lblNote.Visible = false;
			PopulateControlValues();
		}



		#endregion


	
		#region IBasicEnquiryControl2 Members
		/// <summary>
		/// readonly variable
		/// </summary>
		private bool _readonly = false;
		/// <summary>
		/// required variable 
		/// </summary>
		private bool _required = false;
		/// <summary>
		/// in design mode variable
		/// </summary>
		private bool _designonly = false;
		/// <summary>
		/// Is Dirty variable
		/// </summary>
		private bool _isdirty = false;
		/// <summary>
		/// Value of this class
		/// </summary>
		private object _value = null;

		public event System.EventHandler ActiveChanged;
		public event System.EventHandler Changed;
				
		/// <summary>
		/// Read Only Property
		/// </summary>
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
		/// Get or Set Property to Store the Required 
		/// </summary>
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
		/// Public Method to launch ActiveChange Event
		/// </summary>
		public void OnActiveChanged()
		{
			IsDirty=true;
			if (ActiveChanged != null)
				ActiveChanged(this,EventArgs.Empty);
		}

		/// <summary>
		/// Property to return the Default Control
		/// </summary>
		public object Control
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// The Value object
		/// </summary>
		//[Browsable(false)]
		public object Value
		{
			get
			{
				return _value;
			}
			set{}
		}

        [Browsable(false)]
		public bool LockHeight
		{
			get
			{
				return false;
			}
		}

        [Browsable(false)]
		public int CaptionWidth
		{
			get
			{
				return -1;
			}
			set
			{}
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

		public void OnChanged()
		{
			if (Changed != null && IsDirty)
				Changed(this,EventArgs.Empty);
		}

        [Browsable(false)]
        [DefaultValue(false)]
		public bool omsDesignMode
		{
			get
			{
				return _designonly;
			}
			set
			{
				_designonly = value;
			}
		}

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




		#endregion

		
	}
}
