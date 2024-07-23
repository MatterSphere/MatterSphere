using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;
using FWBS.Common;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.SourceEngine;
using FWBS.OMS.UI.Windows.Design;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// EnquiryPage Editor for the Admin Kit
    /// </summary>
    [DefaultProperty("Code")]
	public class EnquiryPageEditor : System.ComponentModel.Component
	{
		#region Fields
		private CodeLookupDisplay _code = new CodeLookupDisplay("ENQPAGE");
		private DataRow _rw;
		public int Page;
		public event EventHandler PageHeaderChange;
		public event EventHandler PageOrderChange;
        public event EventHandler ImageChange;
        private FWBS.Common.ConfigSetting _xmlSetting;
        private CodeLookupDisplayMulti _usrroles = null;

		#endregion

		#region Constructor
		public EnquiryPageEditor()
		{

		}
		#endregion

		#region Private
		private System.Int32 ConvertToInt32(object value, Int32 defaultvalue)
		{
			try
			{
				return Convert.ToInt32(value);
			}
			catch
			{
				return defaultvalue;
			}
		}

		
		private void PageDescriptionChanged(object sender, EventArgs e)
		{
			_rw["pgeDesc"] = _code.Description;
			OnPageHeaderChange(e);
		}
		#endregion

		#region Properties
		[Description("Custom Page"), LocCategory("Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Custom
		{
			get
			{
				try
				{
					if (_rw != null)
						return Convert.ToBoolean(_rw["pgeCustom"]);
					else
						return false;
				}
				catch
				{
					return false;
				}
			}
			set
			{
				if (_rw != null)
					_rw["pgeCustom"] = value;
			}
		}
		
		[LocCategory("Design")]
		[Description("Wizard Page Title")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TextOnly(true)]
		[CodeLookupSelectorTitle("PAGEHEADERS","Page Headers")]
		public CodeLookupDisplay PageHeader
		{
			get
			{
				return _code;
			}
			set
			{
				if (_rw != null)
				{
					_rw["pgeCode"] = value.Code;
					_rw["pgeDesc"] = value.Description;
					_code = value;
					_code.CodeLookupDisplayChanged += new EventHandler(PageDescriptionChanged);
					OnPageHeaderChange(new EventArgs());
				}
			}
		}

		[LocCategory("(Details)")]
		[Description("Wizard Page Order")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[RefreshProperties(RefreshProperties.All)]
		public Int16 PageOrder
		{
			get
			{
				return Convert.ToInt16(_rw["pgeOrder"]);
			}
			set
			{
				if (_rw != null)
				{
					_rw["pgeOrder"] = value;
					OnPageOrderChange(new EventArgs());
				}
			}
		}
		
		[LocCategory("(Details)")]
		[Description("Wizard Page Unique Name")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string PageName
		{
			get
			{
				return Convert.ToString(_rw["pgeName"]);
			}
			set
			{
				if (_rw != null)
				{
					_rw["pgeName"] = value;
					OnPageHeaderChange(new EventArgs());
				}
			}
		}

		[LocCategory("Page")]
		[Description("Wizard Page Number")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PageNumber
		{
			get
			{
				return Page;
			}
		}

		[LocCategory("Page")]
		[Description("Finished Button Enabled State")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool FinishedButtonEnabled
		{
			get
			{
				return ConvertDef.ToBoolean(_rw["pgeFinishedEnabled"],true);
			}
			set
			{
				if (_rw != null)
				{
					_rw["pgeFinishedEnabled"] = value;
					OnPageHeaderChange(new EventArgs());
				}
			}
		}


		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[RefreshProperties(RefreshProperties.All)]
		public DataRow EnquiryRow
		{
			get
			{
				return _rw;
			}
			set
			{
				_rw = value;
                pageimage = null;
                if (_rw.Table.Columns.Contains("pgeImage") == false)
                    _rw.Table.Columns.Add("pgeImage", typeof(FWBS.OMS.Images));
                _xmlSetting = new FWBS.Common.ConfigSetting(_rw, "pgeSettings");
                try
				{
					_usrroles = null;
					_code = new CodeLookupDisplay("ENQPAGE");
					_code.Code = Convert.ToString(_rw["pgeCode"]);
					_code.CodeLookupDisplayChanged -= new EventHandler(PageDescriptionChanged);
					_code.CodeLookupDisplayChanged += new EventHandler(PageDescriptionChanged);
				}
				catch
				{
				}
			}
		}

        Image pageimage;

        [LocCategory("Page")]
        [RefreshProperties(RefreshProperties.All)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(ImageTypeConvertor))]
        public Image Image
        {
            get
            {
                if (pageimage == null && String.IsNullOrEmpty(_xmlSetting.GetSetting("Page", "Image", "")) == false)
                {
                    string base64 = _xmlSetting.GetSetting("Page", "Image", "");
                    try
                    {
                        using (System.IO.MemoryStream reader = new System.IO.MemoryStream())
                        {
                            byte[] buffer = Convert.FromBase64String(base64);
                            reader.Write(buffer, 0, buffer.Length);
                            reader.Position = 0;
                            pageimage = Image.FromStream(reader);
                        }
                    }
                    catch { }
                }
                return pageimage;
            }
            set
            {
                if (_rw != null)
                {
                    pageimage = EnquiryForm.EnsurePageHeaderImageSize(value);
                    if (pageimage == null)
                    {
                        _xmlSetting.SetSetting("Page", "Image", "");
                    }
                    else
                    {
                        using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                        {
                            pageimage.Save(stream, global::System.Drawing.Imaging.ImageFormat.Png);
                            byte[] buffer = new byte[stream.Length];
                            stream.Position = 0;
                            stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
                            _xmlSetting.SetSetting("Page", "Image", Convert.ToBase64String(buffer));
                            _xmlSetting.Synchronise();
                        }
                    }
                    OnImageChange(EventArgs.Empty);
                }
            }
        }
        
        [LocCategory("PAGEVISIBLE")]
		[RefreshProperties(RefreshProperties.All)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string[] Conditional
		{
			get
			{
				return Convert.ToString(_rw["pgeCondition"]).Split(Environment.NewLine.ToCharArray());
			}
			set
			{
				if (_rw != null)
				{
					_rw["pgeCondition"] = String.Join(Environment.NewLine,value);
				}
			}
		}

		[LocCategory("PAGEVISIBLE")]
		[System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[CodeLookupSelectorTitle("USERROLES","User Roles")]
		public CodeLookupDisplayMulti UserRoles
		{
			get
			{
				if (_usrroles == null)
				{
					_usrroles = new CodeLookupDisplayMulti("USRROLES");
					_usrroles.Codes = Convert.ToString(_rw["pgeRole"]);
				}
				return _usrroles;
			}
			set
			{
				_usrroles = value;
				if (_rw != null)
				{
					_rw["pgeRole"] = value.Codes;
				}
			}
		}
		#endregion

		#region Deligates
        protected virtual void OnImageChange(EventArgs e)
        {
            if (ImageChange != null)
                ImageChange(this, e);
        }
        protected virtual void OnPageHeaderChange(EventArgs e) 
		{
			if (PageHeaderChange != null)
				PageHeaderChange(this, e);
		}	
		protected virtual void OnPageOrderChange(EventArgs e) 
		{
			if (PageOrderChange != null)
				PageOrderChange(this, e);
		}	
		#endregion
	}
		
	/// <summary>
	/// Summary description for EnquiryHeaderEditor.
	/// </summary>

	[DefaultProperty("Code")]
	public class EnquiryHeaderEditor : System.ComponentModel.Component
	{
		#region Public
		public void ResetDescription()
		{
			_description = "";
			_welcometextcode.Code = "";
			_welcomeheadercode.Code = "";
			_updating = false;

		}
		#endregion

		#region Fields
		private FWBS.OMS.UI.Windows.Design.DataBuilder _db;
		private FWBS.Common.ConfigSetting _xmlSetting;
		private FWBS.Common.ConfigSetting _xmlParams;
		private DataRow _rw;
		private bool _updating;
		private string _description;
		private FWBS.OMS.UI.Windows.EnquiryForm _enq;
		private CodeLookupDisplay _welcometextcode = new CodeLookupDisplay("ENQWELCOME");
		private CodeLookupDisplay _welcomeheadercode = new CodeLookupDisplay("ENQPAGE");
        private Image image;
        private int glyph = -1;
		#endregion

		#region Constructor
		public EnquiryHeaderEditor()
		{
		}
		
		public void LoadEnquiryHeaderEditor(DataRow rw, FWBS.OMS.UI.Windows.EnquiryForm enq)
		{
			_updating = true;
			_welcometextcode.Code="";
			_welcomeheadercode.Code="";
			_description="";
			_db = new FWBS.OMS.UI.Windows.Design.DataBuilder();
			_enq = enq;
			_rw = rw;
			_xmlSetting = new FWBS.Common.ConfigSetting(_rw,"enqSettings");
			_xmlSetting.Changed += new EventHandler(SettingsChanged);
			
			
			_xmlParams = new FWBS.Common.ConfigSetting(_rw,"enqParameters");
			_xmlParams.Current = "params";
			_db.Parameters.Clear();
			_db.ParametersInx.Clear();
			_db.ResetFields();
            foreach (ConfigSettingItem dr in _xmlParams.CurrentChildItems)
            {
                FWBS.OMS.SearchEngine.SearchParameterDateIs kind = (FWBS.OMS.SearchEngine.SearchParameterDateIs)FWBS.Common.ConvertDef.ToEnum(dr.GetString("kind", ""), FWBS.OMS.SearchEngine.SearchColumnsDateIs.NotApplicable);
                _db.Parameters.Add(new Parameter(_db, dr.GetString("name", ""), dr.GetString("type", ""), dr.GetString("", ""), dr.GetString("test", ""),kind, dr.Element));
            }
			_db.SourceType = (SourceType)Enum.Parse(typeof(SourceType),Convert.ToString(_rw["enqSourceType"]),true);
			_db.Source = Convert.ToString(_rw["enqSource"]);
			_db.Call = Convert.ToString(_rw["enqCall"]);
			_db.EnquiryForm = "";
			_db.TableName = Convert.ToString(_rw["enqCall"]);
			_db.Where = Convert.ToString(_rw["enqWhere"]);
			_welcometextcode.Code = Convert.ToString(_rw["enqWelcomeTextCode"]);
			_welcomeheadercode.Code = Convert.ToString(_rw["enqWelcomeHeaderCode"]);
			_welcomeheadercode.CodeLookupDisplayChanged += new EventHandler(WelcomeHeaderDescriptionChanged);
			_welcometextcode.CodeLookupDisplayChanged += new EventHandler(WelcomeTextDescriptionChanged);
			
			DataTable dt = FWBS.OMS.CodeLookup.GetLookups("ENQHEADER", Convert.ToString(_rw["enqCode"]));
			if (dt.Rows.Count > 0)
			{
				_description = Convert.ToString(dt.Rows[0]["cddesc"]);
			}
			_updating=false;
		}
		#endregion

		#region Private
		private System.Int32 ConvertToInt32(object value, Int32 defaultvalue)
		{
			try
			{
				return Convert.ToInt32(value);
			}
			catch
			{
				return defaultvalue;
			}
		}

		private void WriteAttribute(XmlNode Node, string Name, string Value)
		{
			try{Node.SelectSingleNode("@" + Name).Value = Value;}
			catch{Node.Attributes.Append(CreateAttribute(Node, Name,Value));}
		}

		private System.Xml.XmlAttribute CreateAttribute(XmlNode Node, string Name, string Value)
		{
			System.Xml.XmlAttribute n = Node.OwnerDocument.CreateAttribute(Name);
			n.Value = Value;
			return n;
		}

		private void WelcomeTextDescriptionChanged(object sender, EventArgs e)
		{
			_rw["enqWelcomeText"] = _welcometextcode.Description;
			OnWelcomeHeaderCodeChange(e);
		}
		private void WelcomeHeaderDescriptionChanged(object sender, EventArgs e)
		{
			_rw["enqWelcomeHeader"] = _welcomeheadercode.Description;
			OnWelcomeHeaderCodeChange(e);
		}

		private void SettingsChanged(object sender, EventArgs e)
		{
			if (_updating==false) OnDirty(EventArgs.Empty);
		}

		#endregion

		#region Properties

		[Browsable(false)]
		public FWBS.Common.ConfigSetting Settings
		{
			get
			{
				return _xmlSetting;
			}
		}

        [LocCategory("Wizard")]
        [Description("Override the Default Welcome Page Image")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(ImageTypeConvertor))]
        public Image WelcomePageImage
        {
            get
            {
                if (image == null && _xmlSetting != null && String.IsNullOrEmpty(_xmlSetting.GetSetting("Wizard", "Image", "")) == false)
                {
                    string base64 = _xmlSetting.GetSetting("Wizard", "Image", "");
                    try
                    {
                        using (System.IO.MemoryStream reader = new System.IO.MemoryStream())
                        {
                            byte[] buffer = Convert.FromBase64String(base64);
                            reader.Write(buffer, 0, buffer.Length);
                            reader.Position = 0;
                            image = Image.FromStream(reader);
                        }
                    }
                    catch { }
                }
                return image;
            }
            set
            {
                if (_rw != null && _enq != null)
                {
                    image = value;
                    OnDirty(EventArgs.Empty);
                    if (image == null)
                    {
                        _xmlSetting.SetSetting("Wizard", "Image", "");
                    }
                    else
                    {
                        using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                        {
                            image.Save(stream, global::System.Drawing.Imaging.ImageFormat.Png);
                            byte[] buffer = new byte[stream.Length];
                            stream.Position = 0;
                            stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
                            _xmlSetting.SetSetting("Wizard", "Image", Convert.ToBase64String(buffer));
                        }
                    }
                    OnWelcomePageImageChange(EventArgs.Empty);
                }
            }
        }

        [System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.DialogGlyphDisplayEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
        [System.ComponentModel.TypeConverter("FWBS.OMS.UI.Windows.Design.DialogIconsLister,omsadmin")]
        [LocCategory("DESIGN")]
        [Description("Override the Default Glyph")]
        public int Glyph
        {
            get
            {
                if (_xmlSetting != null)
                {
                    glyph = ConvertToInt32(_xmlSetting.GetSetting("View", "Glyph", "-1"), -1);
                }
                return glyph;
            }
            set
            {
                if (_rw != null && _enq != null)
                {
                    glyph = value;
                    _xmlSetting.SetSetting("View", "Glyph", value.ToString());
                    OnDirty(EventArgs.Empty);
                }
            }
        }


        [LocCategory("Data")]
		[Description("Description used to identify the Enquiry Form")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Description
		{
			get
			{
				try
				{
					return _description;
				}
				catch
				{
					return "";
				}
			}
			set
			{
				if (_rw != null && _enq != null && this.Code != "")
				{
					_description = value;
					FWBS.OMS.CodeLookup.Create("ENQHEADER",this.Code,value,"",CodeLookup.DefaultCulture,true,true,true);
					OnDirty(EventArgs.Empty);
				}
			}
		}

		[Description("Enquiry Help Keyword"), LocCategory("Help")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string HelpKeyword
		{
			get
			{
				if (_rw != null && _enq != null)
					return Convert.ToString(_rw["enqHelpKeyword"]);
				else
					return "";
			}
			set
			{
				if (_rw != null && _enq != null)
				{
					_rw["enqHelpKeyword"] = value;
				}
				OnDirty(EventArgs.Empty);
			}
		}
		
		[Description("Enquiry Code Name"), LocCategory("Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ReadOnly(true)]
		public string Code
		{
			get
			{
				if (_rw != null && _enq != null)
					return Convert.ToString(_rw["enqCode"]);
				else
					return "";
			}
			set
			{
				if (_rw != null && _enq != null)
				{
					_rw["enqCode"] = value;
				}
				OnDirty(EventArgs.Empty);
			}
		}

		[Description("Number or Pages in the Wizard"), LocCategory("Wizard")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Int64 PageCount 
		{
			get
			{
                if (_rw != null && _enq != null && _enq.Enquiry != null)
				{
					_enq.Enquiry.Source.Tables["PAGES"].DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
					return _enq.Enquiry.Source.Tables["PAGES"].DefaultView.Count;
				}
				else
					return 0;
			}
		}
		
		[Description("Enquiry Version"), LocCategory("Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Int64 Version 
		{
			get
			{
				try
				{
                    if (_rw != null && _enq != null)
                        return Convert.ToInt64(_rw["enqVersion"]);
                    else
                        return 0;
				}
				catch
				{
					return 0;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Int64 SetVersion
		{
			get
			{
				try
				{
                    if (_rw != null && _enq != null)
                        return Convert.ToInt64(_rw["enqVersion"]);
                    else
                        return 0;
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				if (_rw != null && _enq != null)
					_rw["enqVersion"] = value;
			}
		}

		[Description("Enquiry Engine Version"), LocCategory("Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Int64 EngineVersion
		{
			get
			{
				try
				{
					if (_rw != null && _enq != null)
						return Convert.ToInt64(_rw["enqEngineVersion"]);
					else 
						return 0;
				}
				catch
				{
					if (_rw != null && _enq != null)
						return Enquiry.EngineVersion;
					else
						return 0;
				}
			}
		}
		
		[Description("Commands to build a Data List"), LocCategory("Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[RefreshProperties(RefreshProperties.All)]
		[DataBuilder(DataBuilderMode.EnquiryMode)] 
		[DataBuilderSourceTypeExclude(SourceType.Instance | SourceType.Class)]
		public DataBuilder DataBuilder
		{
			get
			{

				if (_rw != null && _enq != null)
					return _db;
				else
					return null;
			}
			set
			{
				if (_rw != null && _enq != null)
				{
					_db = value;
					if (_db.SourceType == SourceType.Object)
						this.Binding = EnquiryBinding.BusinessMapping;
					_rw["enqSource"] = _db.Source;
					_rw["enqSourceType"] = _db.SourceType.ToString();
					_rw["enqCall"] = _db.Call;
					if (_db.SourceType == SourceType.Object)
						this.Binding = EnquiryBinding.BusinessMapping;
					else if (_db.Call != "" && (_db.SourceType == SourceType.Dynamic || _db.SourceType == SourceType.OMS || _db.SourceType == SourceType.Linked))
						this.Binding = EnquiryBinding.Bound;
					else
						this.Binding = EnquiryBinding.Unbound;
					if (_db.Where != "" && _db.Where != null)
					{
						 if (_db.Where.ToUpper().StartsWith("WHERE") == false)
							 _rw["enqWhere"] = "WHERE " + _db.Where;
						 else
							 _rw["enqWhere"] = _db.Where;
					}
					else
						_rw["enqWhere"] = "";


					_xmlParams.ClearElements();
					ConfigSettingItem item;
					foreach (Parameter p in _db.Parameters)
					{
						if (p.Node == null)
							item = _xmlParams.AddChildItem("param");
						else
							item = _xmlParams.AddChildItem((XmlElement)p.Node);
						item.SetString("name",p.SQLParameter);
						item.SetString("type",p.FieldType);
						item.SetString("test",p.TestValue);
						item.SetString(p.BoundValue);
					}
					_xmlParams.Synchronise();
					OnDataBuilderChange(new EventArgs());
					OnDirty(EventArgs.Empty);
				}
			}
		}

		[Description("Welcome Wizard Header Code"), LocCategory("Wizard")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[CodeLookupSelectorTitle("WELCOMEHEADERS","Welcome Headers")]
		public CodeLookupDisplay WelcomeHeaderCode
		{
			get
			{
				return _welcomeheadercode;
			}
			set
			{
				if (_rw != null && _enq != null)
				{
					_rw["enqWelcomeHeaderCode"] = value.Code;
					_rw["enqWelcomeHeader"] = value.Description;
					_welcomeheadercode = value;
					_welcomeheadercode.CodeLookupDisplayChanged += new EventHandler(WelcomeHeaderDescriptionChanged);
					OnWelcomeHeaderCodeChange(new EventArgs());
					OnDirty(EventArgs.Empty);
				}
			}
		}

		[Description("Help Filename"), LocCategory("Help")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string HelpFilename
		{
			get
			{
                if (_rw != null)
                    return Convert.ToString(_rw["enqHelp"]);
                else
                    return "";
			}
			set
			{
				if (_rw != null && _enq != null)
				{
					_rw["enqHelp"] = value;
					OnDirty(EventArgs.Empty);
				}
			}
		}
		
		[Description("Welcome Wizard Description Code"), LocCategory("Wizard")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[CodeLookupSelectorTitle("WELCOMETEXTS","Welcome Texts")]
		public CodeLookupDisplay WelcomeTextCode
		{
			get
			{
				return _welcometextcode;
			}
			set
			{
				if (_rw != null && _enq != null)
				{
					_rw["enqWelcomeTextCode"] = value.Code;
					_rw["enqWelcomeText"] = value.Description;
					_welcometextcode = value;
					_welcometextcode.CodeLookupDisplayChanged += new EventHandler(WelcomeTextDescriptionChanged);
					OnWelcomeTextCodeChange(new EventArgs());
					OnDirty(EventArgs.Empty);
				}
			}
		}


		[Description("Padding in Pixels from the Top Left Edge"), LocCategory("Wizard")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Point Margin
		{
			get
			{
				if (_rw != null && _enq != null)
					return new Point(ConvertToInt32(_rw["enqPaddingX"],0),ConvertToInt32(_rw["enqPaddingY"],0));
				else
					return new Point(0,0);
			}
			set
			{
				if (_rw != null && _enq != null)
				{
					_rw["enqPaddingY"] = value.Y;
					_rw["enqPaddingX"] = value.X;
					OnMarginChange(new EventArgs());
					OnDirty(EventArgs.Empty);
				}
			}
		}

		[Description("Padding in Pixels between Controls"), LocCategory("Wizard")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int LineSpacing
		{
			get
			{
				if (_rw != null && _enq != null)
					return ConvertToInt32(_rw["enqLeadingY"],0);
				else
					return 0;
			}
			set
			{
				if (_rw != null && _enq != null && LineSpacing != value)
				{
					_rw["enqLeadingY"] = value;
					OnMarginChange(new EventArgs());
				}
				OnDirty(EventArgs.Empty);
			}
		}

		[Description("Enquiry Mode"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Editor(typeof(FWBS.OMS.UI.Windows.Design.FlagsEditor),typeof(UITypeEditor))]
		public EnquiryMode Modes
		{
			get
			{
				if (_rw != null && _enq != null)
					return (EnquiryMode)Convert.ToInt64(_rw["enqModes"]);
				else
					return EnquiryMode.Add;
			}
			set
			{
				if (_rw != null && _enq != null)
				{
					_rw["enqModes"] = (Int64)value;
				}
				OnDirty(EventArgs.Empty);
			}
		}
		
		[Description("Binding Mode"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ReadOnly(true)]
		public EnquiryBinding Binding
		{
			get
			{
				if (_rw != null && _enq != null)
					return (EnquiryBinding)Convert.ToInt64(_rw["enqBindings"]);
				else
					return EnquiryBinding.Unbound;
			}
			set
			{
				if (_rw != null && _enq != null)
					if (_db.SourceType == SourceType.Object)
						_rw["enqBindings"] = EnquiryBinding.BusinessMapping;
					else
						_rw["enqBindings"] = (Int64)value;
				OnDirty(EventArgs.Empty);
			}
		}

		[Description("Editor Canvas Size"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[RefreshProperties(RefreshProperties.All)]
		public Size StandardSize
		{
			get
			{
				if (_rw != null && _enq != null)
					return new Size(ConvertToInt32(_rw["enqCanvasWidth"],0),ConvertToInt32(_rw["enqCanvasHeight"],0));
				else
					return new Size(0,0);
			}
			set
			{
				if (_rw != null && _enq != null && StandardSize != value)
				{
					_rw["enqCanvasHeight"] = value.Height;
					_rw["enqCanvasWidth"] = value.Width;
					OnStandardSizeChange(new EventArgs());
					OnDirty(EventArgs.Empty);
				}
			}
		}

		[RefreshProperties(RefreshProperties.All)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Description("Wizard Size"), LocCategory("Design")]
		public Size WizardSize
		{
			get
			{
				if (_rw != null && _enq != null)
					return new Size(ConvertToInt32(_rw["enqWizardWidth"],0),ConvertToInt32(_rw["enqWizardHeight"],0));
				else
					return new Size(0,0);
			}
			set
			{
				if (_rw != null && _enq != null && WizardSize != value)
				{
					_rw["enqWizardHeight"] = value.Height;
					_rw["enqWizardWidth"] = value.Width;
					OnWizardSizeChange(new EventArgs());
					OnDirty(EventArgs.Empty);
				}
			}
		}

		[Description("Fields used by Enquiry"), LocCategory("Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string[] Fields
		{
			get
			{
				if (_rw != null && _enq != null)
					return Convert.ToString(_rw["enqFields"]).Split(",".ToCharArray());
				else
					return null;
			}
			set
			{
				if (_rw != null && _enq != null)
				{
					string _enqFields ="";
					foreach(string n in value)
						_enqFields = _enqFields.Trim() + n + ",";
					_enqFields = _enqFields.Trim(", ".ToCharArray());
					_rw["enqFields"] = _enqFields;
					OnDirty(EventArgs.Empty);
				}
			}
		}
		
		[Description("Turns on or off the Automatic Field Builder"), LocCategory("Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool FieldsManualMode
		{
			get
			{
				try
				{
					if (_rw != null && _enq != null)
						return Convert.ToBoolean(_rw["enqDesManBindMode"]);
					else
						return false;
				}
				catch
				{
					return false;
				}
			}
			set
			{
				if (_rw != null && _enq != null)
					_rw["enqDesManBindMode"] = value;
				OnDirty(EventArgs.Empty);
			}
		}
		
		[Description("Is a System Enquiry"), LocCategory("System")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool System
		{
			get
			{
				try
				{
                    if (_rw != null && _enq != null)
                        return Convert.ToBoolean(_rw["enqSystem"]);
                    else
                        return false;
				}
				catch
				{
					return false;
				}
			}
			set
			{
				if (_rw != null && _enq != null)
					_rw["enqSystem"] = value;
				OnDirty(EventArgs.Empty);
			}
		}

		[Description("Change Password"), LocCategory("System")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string Password
		{
			get
			{
				if (_rw != null && _enq != null)
					return Convert.ToString(_rw["enqPassword"]);
				else
					return "";
			}
			set
			{
				if (_rw != null && _enq != null)
					_rw["enqPassword"] = value;
				OnDirty(EventArgs.Empty);
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FWBS.OMS.UI.Windows.EnquiryForm EnquiryForm
		{
			get
			{
				return _enq;
			}
			set
			{
				_enq = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataRow EnquiryRow
		{
			get
			{
				return _rw;
			}
			set
			{
				_rw = value;
			}
		}

		[Description("Assign a Script"), LocCategory("Script")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof(ScriptLister))]
		[ScriptTypeParam("ENQUIRYFORM")]
		public string Script
		{
			get
			{
				if (_rw != null && _enq != null)
					return Convert.ToString(_rw["enqScript"]);
				else
					return "";
			}
			set
			{
                if (Session.CurrentSession.IsLicensedFor("SDKALL") == true || frmMain.PartnerAccess == true)
                {
                    if (_rw != null && _enq != null)
                    {
                        _rw["enqScript"] = value;
                        if (!String.IsNullOrEmpty(_enq.Enquiry.Script.Code))
                            _enq.Enquiry.NewScript();

                        _enq.Enquiry.Script.Code = value;
                        OnScriptChange(EventArgs.Empty);
                    }
                    OnDirty(EventArgs.Empty);
                }
			}
		}
		#endregion

		#region Deligates
		//
		// Deligates
		//
		public event EventHandler Dirty;
		public event EventHandler MarginChange;
		public event EventHandler DataBuilderChange;
		public event EventHandler StandardSizeChange;
		public event EventHandler WizardSizeChange;
		public event EventHandler WelcomeTextCodeChange;
		public event EventHandler WelcomeHeaderCodeChange;
		public event EventHandler ScriptChange;
        public event EventHandler WelcomePageImageChange;

        protected virtual void OnWelcomePageImageChange(EventArgs e)
        {
            if (WelcomePageImageChange != null)
                WelcomePageImageChange(this, e);
        }

		protected virtual void OnDirty(EventArgs e) 
		{
			if (Dirty != null)
				Dirty(this, e);
		}

		protected virtual void OnMarginChange(EventArgs e) 
		{
			if (MarginChange != null)
				MarginChange(this, e);
		}

		protected virtual void OnDataBuilderChange(EventArgs e) 
		{
			if (DataBuilderChange != null)
				DataBuilderChange(this, e);
		}

		protected virtual void OnStandardSizeChange(EventArgs e) 
		{
			if (StandardSizeChange != null)
				StandardSizeChange(this, e);
		}		
		protected virtual void OnScriptChange(EventArgs e) 
		{
			if (ScriptChange != null)
				ScriptChange(this, e);
		}		
		protected virtual void OnWizardSizeChange(EventArgs e) 
		{
			if (WizardSizeChange != null)
				WizardSizeChange(this, e);
		}		
		protected virtual void OnWelcomeTextCodeChange(EventArgs e) 
		{
			if (WelcomeTextCodeChange != null)
				WelcomeTextCodeChange(this, e);
		}		
		protected virtual void OnWelcomeHeaderCodeChange(EventArgs e) 
		{
			if (WelcomeHeaderCodeChange != null)
				WelcomeHeaderCodeChange(this, e);
		}		
		#endregion
	}

	/// <summary>
	/// 32000 EnquiryControl Editor for the Admin Kit
	/// </summary>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class EnquiryControl : System.ComponentModel.Component
	{
		#region Fields
		/*
		0	quID	bigint	8	0
		1	enqID	int	4	1
		1	quName	nvarchar	25	0
		1	quOrder	tinyint	1	0
		1	quCode	uCodeLookup (nvarchar)	15	0
		1	quPage	smallint	2	-1
		1	quTable	varchar	50	1
		1	quExtendedData	uCodeLookup (nvarchar)	15	1
		1	quFieldName	varchar	50	1
		1	quProperty	varchar	50	1
		1	quType	varchar	50	0
		1	quctrlid	tinyint	1	0
		1	quDataList	uCodeLookup (nvarchar)	15	1
		1	quAdd	bit	1	0
		1	quEdit	bit	1	0
		1	quAddSecLevel	tinyint	1	0
		1	quEditSecLevel	tinyint	1	0
		1	quSearch	bit	1	0
		1	quUnique	bit	1	0
		1	quConstraint	varchar	75	1
		1	quHidden	bit	1	0
		1	quDefault	nvarchar	255	1
		1	quTabOrder	int	4	1
		1	quReadonly	bit	1	0
		1	quRequired	bit	1	0
		0	quMinLength	int	4	0
		1	quMaxLength	int	4	0
		1	quWidth	int	4	0
		1	quHeight	int	4	0
		1	quMask	nvarchar	100	1
		1	quX	int	4	0
		1	quY	int	4	0
		1	quWizX	int	4	1
		1	quWizY	int	4	1
		1	quCaptionWidth	int	4	0
		0	quColumn	tinyint	1	0
		0	quEdition	varchar	2	1
		1	quCommand	uCodeLookup (nvarchar)	15	1
		1	quCommandRetVal	bit	1	0
		0	quSystem	bit	1	0
		0	quCustom	image	16	1
		0	quFilter	uXML (ntext)	16	0
		1	quVisibleRole	uCodeLookup (nvarchar)	15	1
		0	quEditableRole	uCodeLookup (nvarchar)	15	1
		*/
		private DataRow _rw;
		private CodeLookupDisplay _caption = new CodeLookupDisplay("ENQQUESTION");
		private CodeLookupDisplayReadOnly _extended = new CodeLookupDisplayReadOnly("EXTENDEDDATA");
		private CodeLookupDisplayReadOnly _datalist = new CodeLookupDisplayReadOnly("ENQDATALIST");
		private CodeLookupDisplayReadOnly _commands = new CodeLookupDisplayReadOnly("ENQCOMMAND");
		private CodeLookupDisplayMulti _rlsvisible = new CodeLookupDisplayMulti("USRROLES");
		private CodeLookupDisplayMulti _rlseditable = new CodeLookupDisplayMulti("USRROLES");
		private Unique _unique;
		private System.Windows.Forms.Control _ctrl;
		private FWBS.OMS.UI.Windows.EnquiryForm _enq;
		private FWBS.OMS.UI.Windows.Admin.ControlType _controltype = new FWBS.OMS.UI.Windows.Admin.ControlType();
		private FilterCollection _filter = new FilterCollection();
		private CustomCollection _custom = new CustomCollection();
		private string FilIndex = "";
		private XmlDocument _filterDocument;
		private XmlNode _filterHeader;
		private ConfigSetting _customxml;

		private CodeLookupDisplayMulti _usrroles = null;
		#endregion

		#region Constructors
		public EnquiryControl(DataRow rw, System.Windows.Forms.Control ctrl, FWBS.OMS.UI.Windows.EnquiryForm enq)
		{
			EnquiryRow = rw;
			Control = ctrl;
			EnquiryForm = enq;
			if (Convert.ToInt64(enq.Enquiry.Source.Tables["ENQUIRY"].Rows[0]["enqBindings"]) != (Int64)EnquiryBinding.BusinessMapping)
			{
				_extended.ReadOnly=true;
				_extended.ReadOnlyMessage = "The Enquiry Form is not Business Mapped";
			}
			if (this.Control is IList == false && this.Control is FWBS.Common.UI.IListEnquiryControl == false)
			{
				_datalist.ReadOnly=true;
				_datalist.ReadOnlyMessage = "Data List cannot be set if the Control cannot contain a list";
			}
			_caption.CodeLookupDisplayChanged += new EventHandler(CaptionDescriptionChanged);


			ConfigSetting cfg = new ConfigSetting(Convert.ToString(rw["qufilter"]));
			_filterDocument = cfg.DocObject;
			//Count the number of parameters to be used.
			cfg.Current = "filters";
			FWBS.Common.ConfigSettingItem [] filters = cfg.CurrentChildItems;
			_filterHeader = cfg.DocCurrent;
			int cnt = filters.Length;

			_filter.Clear();
			//Loop through each of the parameters,
			for (int ctr = 0; ctr < cnt; ctr++)
			{
				//Get the name of the control to filter.
				string control = filters[ctr].GetString("control", "");
				//Get the field name of the result set within the control being filtered.
				string fieldName = filters[ctr].GetString("fieldName", ""); 			
				_filter.Add(new Filter(filters[ctr].Element,control,fieldName,enq,this.Name));
			}
			_filter.Cleared += new Crownwood.Magic.Collections.CollectionClear(ClearFilter);
			_filter.Inserted += new Crownwood.Magic.Collections.CollectionChange(AddFilter);

			_customxml = new ConfigSetting(rw,"qufilter");
			//Count the number of parameters to be used.
			_customxml.Current = "custom";
			cnt = _customxml.DocCurrent.Attributes.Count;
			_custom.Clear();

			//Loop through each of the parameters,
			for (int ctr = 0; ctr < cnt; ctr++)
			{
				//Get the name of the control to filter.
				string propname = _customxml.DocCurrent.Attributes[ctr].Name;
				//Get the field name of the result set within the control being filtered.
				string propvalue = _customxml.GetString(propname,""); 			
				_custom.Add(new Custom(propname,propvalue));
			}
			_custom.Inserted += new Crownwood.Magic.Collections.CollectionChange(AddCustom);
			_custom.Cleared +=new Crownwood.Magic.Collections.CollectionClear(Custom_Cleared);
		}
		#endregion

		#region Private
		

		private void CaptionDescriptionChanged(object sender, EventArgs e)
		{
			this.Control.Text = _caption.Description;
			_rw["quHelp"] = _caption.Help;
			_rw["quDesc"] = _caption.Description;
		}

		private void Control_Resize(object sender, System.EventArgs e)
		{
			this.Size = (Control).Size;
		}

		private void AddFilter(int index, object value)
		{
			Filter fil = (Filter)value;
			if (FilIndex.IndexOf(fil.Control + ";") == -1)
			{
				FilIndex = FilIndex + fil.Control + ";";
				if (fil._node == null)
				{
					fil._node = _filterHeader.OwnerDocument.CreateElement("","filter","");
				}
				WriteAttribute(fil._node,"control",fil.Control);
				WriteAttribute(fil._node,"fieldName",fil.Fieldname);
				_filterHeader.AppendChild(fil._node);
				_rw["quFilter"] = _filterDocument.InnerXml;
			}
			else
				_filter.Remove(fil);
		}

		private void AddCustom(int index, object value)
		{
			Custom cus = (Custom)value;
			_customxml.SetString(cus.Property,cus.Value);
			_customxml.Synchronise();

			object val = null;
					
			Type t = _ctrl.GetType();
			System.Reflection.PropertyInfo prop = t.GetProperty(cus.Property);
			if (prop != null)
			{
				object[] attrs = prop.PropertyType.GetCustomAttributes(typeof(System.ComponentModel.TypeConverterAttribute), true);
					
				if (attrs.Length > 0)
				{
					System.ComponentModel.TypeConverterAttribute typeconv = (System.ComponentModel.TypeConverterAttribute)attrs[0];
                    Type tct = Session.CurrentSession.TypeManager.Load(typeconv.ConverterTypeName);
					System.ComponentModel.TypeConverter conv = (System.ComponentModel.TypeConverter)tct.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
					if (conv.CanConvertFrom(typeof(string)))
						val = conv.ConvertFromInvariantString(cus.Value);
					else
						val = DBNull.Value;
				}
				else
					val = cus.Value;
					
				try
				{
					if (val != DBNull.Value)
					{
						if (prop.PropertyType.BaseType == typeof(System.Enum))
							prop.SetValue(_ctrl, Enum.Parse(prop.PropertyType,cus.Value,true), null);
						else
							prop.SetValue(_ctrl, Convert.ChangeType(val, prop.PropertyType, System.Globalization.CultureInfo.InvariantCulture), null);
					}
				}
				catch{}
			}
		}

		
		private void Custom_Cleared()
		{
			_customxml.DocCurrent.RemoveAllAttributes();
			_customxml.Synchronise();
		}

		private void ClearFilter()
		{
			FilIndex = "";
			for(int ui = _filterHeader.ChildNodes.Count-1; ui > -1; ui--)
			{
				XmlNode dr = _filterHeader.ChildNodes[ui];
				_filterHeader.RemoveChild(dr);
			}
			_rw["quFilter"] = _filterDocument.InnerXml;
		}

		private void WriteAttribute(XmlNode Node, string Name, object Value)
		{
			try{Node.SelectSingleNode("@" + Name).Value = Convert.ToString(Value);}
			catch{Node.Attributes.Append(CreateAttribute(Node, Name, Convert.ToString(Value)));}
		}

		private System.Xml.XmlAttribute CreateAttribute(XmlNode Node, string Name, string Value)
		{
			System.Xml.XmlAttribute n = Node.OwnerDocument.CreateAttribute(Name);
			n.Value = Value;
			return n;
		}

		private string GetAttribute(XmlNode Node, string Name, string Value)
		{
			try
			{
				return Node.SelectSingleNode("@" + Name).Value;
			}
			catch
			{
				return Value;
			}
		}

		#endregion

		#region Properties
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FWBS.OMS.UI.Windows.EnquiryForm EnquiryForm
		{
			get
			{
				return _enq;
			}
			set
			{
				_enq = value;
				_controltype.EnquiryForm = _enq;
			}
		}
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataRow EnquiryRow
		{
			get
			{
				return _rw;
			}
			set
			{
				_rw = value;
				_caption.Code = Convert.ToString(_rw["quCode"]);
				_datalist.Code = Convert.ToString(_rw["quDataList"]);
				_extended.Code = Convert.ToString(_rw["quExtendedData"]);
				_commands.Code = Convert.ToString(_rw["quCommand"]);
				_rlseditable.Codes = Convert.ToString(_rw["quEditableRole"]);
				_rlsvisible.Codes = Convert.ToString(_rw["quVisibleRole"]);
				_unique = new Unique(_rw);
				_controltype.EnquiryRow = _rw;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public System.Windows.Forms.Control Control
		{
			get
			{
				return _ctrl;
			}
			set
			{
				_ctrl = value;
				_controltype.Control = value;
				_ctrl.SizeChanged += new EventHandler(Control_Resize);
			}
		}

		[LocCategory("CTRLVISIBLE")]
		[RefreshProperties(RefreshProperties.All)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MergableProperty(false)]
		public string[] Conditional
		{
			get
			{
				return Convert.ToString(_rw["quCondition"]).Split(Environment.NewLine.ToCharArray());
			}
			set
			{
				if (_rw != null)
				{
					_rw["quCondition"] = String.Join(Environment.NewLine,value);
				}
			}
		}

		[LocCategory("CTRLVISIBLE")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MergableProperty(false)]
		[CodeLookupSelectorTitle("USERROLES","User Roles")]
		public CodeLookupDisplayMulti UserRoles
		{
			get
			{
				if (_usrroles == null)
				{
					_usrroles = new CodeLookupDisplayMulti("USRROLES");
					_usrroles.Codes = Convert.ToString(_rw["quRole"]);
				}
				return _usrroles;
			}
			set
			{
				_usrroles = value;
				if (_rw != null)
				{
					_rw["quRole"] = value.Codes;
				}
			}
		}

		[Description("Control Name"), LocCategory("Data")]
		[MergableProperty(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Name
		{
			get
			{
				if (_rw != null)
					return Convert.ToString(_rw["quName"]);
				else
					return "";
			}
			set
			{
				if (_rw != null)
				{
					if (value.IndexOfAny(@"-¬!""£$%^&*()+=|\/<>.,;'[]{}:;@~#` ".ToCharArray()) != -1)
						throw new OMSException2("32001",@"The Control Name cannot contain any of following -¬!""£$%^&*()+=|\/<>.,;'[]{}:;@~#`(Space)");
					if (value.StartsWith("0") || value.StartsWith("1") || value.StartsWith("2") || value.StartsWith("3") || value.StartsWith("4") || value.StartsWith("5") || value.StartsWith("6") || value.StartsWith("7") || value.StartsWith("8") || value.StartsWith("9")) 
						throw new OMSException2("32002","The Control Name cannot start with a Number");

					if (value != Name)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					if (value.Length > 19)
					{
						_ctrl.Name = value.Substring(0,19);
						_rw["quName"] = value.Substring(0,19);
						MessageBox.Show (new ResourceItem("The Control Name is more than 19 Characters Long.",""), FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.OK,MessageBoxIcon.Warning);
					}
					else
					{
						_ctrl.Name = value;
						_rw["quName"] = value;
					}
				}
			}
		}

		[Description("Change the Charactor Casing"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CharacterCasing Casing
		{
			get
			{
				if (_rw != null)
					return (CharacterCasing)ConvertDef.ToEnum(_rw["quCasing"],CharacterCasing.Normal);
				else
					return CharacterCasing.Normal;
			}
			set
			{
				if (_rw != null)
				{
					if (value != Casing)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quCasing"] = value;
					_enq.RenderControl(ref _ctrl,_rw,"quCasing");
				}
			}
		}

		[Description("Change the Anchoring on a Control"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public AnchorStyles Anchor
		{
			get
			{
				if (_rw != null)
				{
					string ancset = Convert.ToString(_rw["quanchor"]);
					AnchorStyles an = AnchorStyles.None;
					if (ancset.IndexOf("D") == -1)
					{
						if (ancset.IndexOf("L") > -1)
							an = an | AnchorStyles.Left;
						if (ancset.IndexOf("T") > -1)
							an = an | AnchorStyles.Top;
						if (ancset.IndexOf("R") > -1)
							an = an | AnchorStyles.Right;
						if (ancset.IndexOf("B") > -1)
							an = an | AnchorStyles.Bottom;
					}
					return an;
				}
				else
					return AnchorStyles.None;
			}
			set
			{
				if (_rw != null)
				{
					if (value != Anchor)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					string setanc = "";
					if ((value & AnchorStyles.Left) == AnchorStyles.Left) setanc+="L";
					if ((value & AnchorStyles.Top) == AnchorStyles.Top) setanc+="T";
					if ((value & AnchorStyles.Right) == AnchorStyles.Right) setanc+="R";
					if ((value & AnchorStyles.Bottom) == AnchorStyles.Bottom) setanc+="B";
					_rw["quanchor"] = setanc;
					_enq.RenderControl(ref _ctrl,_rw,"quanchor");
				}
			}
		}

		[Description("Change the Docking on a Control"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DockStyle Dock
		{
			get
			{
				DockStyle an;
				if (_rw != null)
				{
					string ancset = Convert.ToString(_rw["quanchor"]);
					an = DockStyle.None;
					if (ancset.IndexOf("DT") > -1)
						an = DockStyle.Top;
					if (ancset.IndexOf("DL") > -1)
						an = DockStyle.Left;
					if (ancset.IndexOf("DB") > -1)
						an = DockStyle.Bottom;
					if (ancset.IndexOf("DR") > -1)
						an = DockStyle.Right;
					if (ancset.IndexOf("DF") > -1)
						an = DockStyle.Fill;
					return an;
				}
				else
					return DockStyle.None;
			}
			set
			{
				if (_rw != null)
				{
					if (value != Dock)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					string setanc = "";
					if (value == DockStyle.Fill) setanc = "DF";
					if (value == DockStyle.Top) setanc = "DT";
					if (value == DockStyle.Left) setanc = "DL";
					if (value == DockStyle.Bottom) setanc = "DB";
					if (value == DockStyle.Right) setanc = "DR";
					_rw["quanchor"] = setanc;
					_enq.RenderControl(ref _ctrl,_rw,"quanchor");
				}
			}
		}

		[Description("Creation Order of the Controls Drawn on the Form"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MergableProperty(false)]
		public int CreationOrder
		{
			get
			{
				if (_rw != null)
					return ConvertDef.ToInt32(_rw["quOrder"],0);
				else
					return 0;
			}
			set
			{
				if (_rw != null)
				{
					if (value != CreationOrder)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quOrder"] = value;
				}
			}
		}

		[Description("Localised code of the Controls Caption"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MergableProperty(false)]
		[CodeLookupSelectorTitle("CAPTIONS","Captions")]
		public CodeLookupDisplay Caption
		{
			get
			{
				return _caption;
			}
			set
			{
				if (_rw != null)
				{
					if (value != Caption)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["qucode"] = value.Code;
					_rw["qudesc"] = value.Description;
					_caption = value;
					_ctrl.Text = value.Description;
					_caption.CodeLookupDisplayChanged += new EventHandler(CaptionDescriptionChanged);
				}
			}
		}

		[Description("The Page in the Wizard that the Control will be visible"), LocCategory("Wizard")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof(PageLister))]
		public string Page
		{
			get
			{
				if (_rw != null)
					return Convert.ToString(_rw["quPage"]);
				else
					return "0";
			}
			set
			{
				if (_rw != null)
				{
					if (value != Page)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quPage"] = value;
					_enq.RenderControls(true);
				}
			}
		}

		[Description("Table that the Control is Bound To"), LocCategory("Data")]
		[RefreshProperties(RefreshProperties.All)]
		[TypeConverter(typeof(BoundTableLister))]
		public string BoundTable
		{
			get
			{
				if (_rw != null)
					return Convert.ToString(_rw["quTable"]);
				else
					return "";
			}
			set
			{
				if (_rw != null)
				{
					if (value != BoundTable)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					if (value.Length > 49)
						_rw["quTable"] = value.Substring(0,49);
					else
						_rw["quTable"] = value;
					if (value == "" && this.ExtendedData.Code == "")
						this.FieldName="";
				}
			}
		}

		[Description("Set the Control to be Linked to Extended Data"), LocCategory("Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Parameter(CodeLookupDisplaySettings.ExtendedData)]
		[MergableProperty(false)]
		[CodeLookupSelectorTitle("EXTENDEDDATAS","Extended Datas")]
		public CodeLookupDisplayReadOnly ExtendedData
		{
			get
			{
				return _extended;
			}
			set
			{
				if (_rw != null)
				{
					if (value != ExtendedData)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					if (value.Code == "")
						_rw["quExtendedData"] = DBNull.Value;
					else
						_rw["quExtendedData"] = value.Code;
					_extended = value;
					_rw["quFieldName"] = "";
				}
			}
		}

		[Description("If set. The role that will see this control"), LocCategory("Security")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MergableProperty(false)]
		[CodeLookupSelectorTitle("USERROLES","User Roles")]
		public CodeLookupDisplayMulti VisibleRole
		{
			get
			{
				return _rlsvisible;
			}
			set
			{
				if (_rw != null)
				{
					if (value != _rlsvisible)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					if (value.Codes == "")
						_rw["quVisibleRole"] = DBNull.Value;
					else
						_rw["quVisibleRole"] = value.Codes;
					_rlsvisible = value;
				}
			}
		}
		
		[Description("NB: This overrides the VisibleRole if set.   The role that have will have edit rights else if set, the control will be read only"), LocCategory("Security")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MergableProperty(false)]
		[CodeLookupSelectorTitle("USERROLES","User Roles")]
		public CodeLookupDisplayMulti EditableRole
		{
			get
			{
				return _rlseditable;
			}
			set
			{
				if (_rw != null)
				{
					if (value != _rlseditable)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					if (value.Codes == "")
						_rw["quEditableRole"] = DBNull.Value;
					else
						_rw["quEditableRole"] = value.Codes;
					_rlseditable = value;
				}
			}
		}

		[Description("A collection of Controls that contain data and will be filtered when this control is changed"), LocCategory("Data")]
		[Editor(typeof(FilterEditor),typeof(UITypeEditor))]
		[MergableProperty(false)]
		public FilterCollection FilterControls
		{
			get
			{
				if (_rw != null)
					return _filter;
				else
					return null;
			}
			set
			{
				if (_rw != null)
				{
					_filter = value;
				}
			}
		}

		[Description("A collection of Custom Properties for this control"), LocCategory("Design")]
		[Editor(typeof(CustomEditor),typeof(UITypeEditor))]
		[MergableProperty(false)]
		public CustomCollection CustomProperties
		{
			get
			{
				if (_rw != null)
					return _custom;
				else
					return null;
			}
			set
			{
				if (_rw != null)
				{
					_custom = value;
				}
			}
		}

		[Description("Field name to bind the Control to"), LocCategory("Data")]
		[TypeConverter(typeof(FieldNamesLister))]
		[MergableProperty(false)]
		public string FieldName
		{
			get
			{
				if (_rw != null)
					return Convert.ToString(_rw["quFieldName"]);
				else
					return "";
			}
			set
			{
				if (_rw != null)
				{
					if (value != FieldName)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					if (value.Length > 49)
						_rw["quFieldName"] = value.Substring(0,49);
					else
						_rw["quFieldName"] = value;
				}
			}
		}

		[Description("Field name to bind the Control to"), LocCategory("Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof(BusinessMappedPropertiesLister))]
		[MergableProperty(false)]
		public string Property
		{
			get
			{
				if (_rw != null)
					return Convert.ToString(_rw["quProperty"]);
				else
					return "";
			}
			set
			{
				if (_rw != null)
				{
					if (value != Property)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					if (value.Length > 49)
						_rw["quProperty"] = value.Substring(0,49);
					else
						_rw["quProperty"] = value;
				}
			}
		}

		[Description("Control Type e.g. System.String"), LocCategory("Data")]
		[TypeConverter(typeof(DataTypesLister))]
		public string DataType
		{
			get
			{
				if (_rw != null)
					return Convert.ToString(_rw["quType"]);
				else
					return "";
			}
			set
			{
				if (_rw != null)
				{
					if (value != DataType)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					if (value.Length > 49)
						_rw["quType"] = value.Substring(0,49);
					else
						_rw["quType"] = value;
				}
			}
		}

		[Description("Type of Control used"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[RefreshProperties(RefreshProperties.All)]
		public FWBS.OMS.UI.Windows.Admin.ControlType ControlType
		{
			get
			{
				return _controltype;
			}
			set
			{
				_controltype = value;
			}
		}

		[Description("If the Control is a List Source then it set the Data Source"), LocCategory("Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Parameter(CodeLookupDisplaySettings.DataList)]
		[MergableProperty(false)]
		[CodeLookupSelectorTitle("DATALISTS","Data Lists")]
		public CodeLookupDisplayReadOnly DataList
		{
			get
			{
				return _datalist;
			}
			set
			{
				if (_rw != null)
				{
					if (value != DataList)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					if (value.Code == "")
						_rw["quDataList"] = DBNull.Value;
					else
						_rw["quDataList"] = value.Code;
					_datalist = value;
					try
					{
						DataListEditor n = new DataListEditor(value.Code);
						DataTable d = (DataTable)n.Run();
						((FWBS.Common.UI.IListEnquiryControl)_ctrl).AddItem(d);
					}
					catch
					{}
				}
			}
		}

		[Description("Display the Control in Add Mode"), LocCategory("Display")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Add
		{
			get
			{
				try
				{
					if (_rw != null)
						return Convert.ToBoolean(_rw["quAdd"]);
					else
						return false;
				}
				catch
				{
					return true;
				}
			}
			set
			{
				if (_rw != null)
				{
					if (value != Add)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quAdd"] = value;
				}
			}
		}

		[Description("Display the Control in Edit Mode"), LocCategory("Display")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Edit
		{
			get
			{
				try
				{
					if (_rw != null)
						return Convert.ToBoolean(_rw["quEdit"]);
					else
						return false;
				}
				catch
				{
					return true;
				}
			}
			set
			{
				if (_rw != null)
				{
					if (value != Edit)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quEdit"] = value;
				}
			}
		}

		[Description("Display the Control in Search Mode"), LocCategory("Display")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Search
		{
			get
			{
				try
				{
					if (_rw != null)
						return Convert.ToBoolean(_rw["quSearch"]);
					else
						return false;
				}
				catch
				{
					return true;
				}
			}
			set
			{
				if (_rw != null)
				{
					if (value != Search)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quSearch"] = value;
				}
			}
		}

		[Description("Display the Control in Search Mode"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Hidden
		{
			get
			{
				try
				{
					if (_rw != null)
						return Convert.ToBoolean(_rw["quHidden"]);
					else
						return false;
				}
				catch
				{
					return true;
				}
			}
			set
			{
				if (_rw != null)
				{
					if (value != Hidden)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quHidden"] = value;
					_enq.RenderControl(ref _ctrl,_rw);
				}
			}
		}

		[Description("Display the Control in as ReadOnly"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ReadOnly
		{
			get
			{
				try
				{
					if (_rw != null)
						return Convert.ToBoolean(_rw["quReadOnly"]);
					else
						return false;
				}
				catch
				{
					return true;
				}
			}
			set
			{
				if (_rw != null)
				{
					if (value != ReadOnly)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quReadOnly"] = value;
					_enq.RenderControl(ref _ctrl,_rw);
				}
			}
		}

		[Description("Makes the Control Required"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Required
		{
			get
			{
				try
				{
					if (_rw != null)
						return Convert.ToBoolean(_rw["quRequired"]);
					else
						return false;
				}
				catch
				{
					return true;
				}
			}
			set
			{
				if (_rw != null)
				{
					if (value != Required)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quRequired"] = value;
					_enq.RenderControl(ref _ctrl,_rw);
				}
			}
		}

		[Description("Is the Controls Data Unique"), LocCategory("Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MergableProperty(false)]
		public Unique Unique
		{
			get
			{
				return _unique;
			}
			set
			{
				_unique = value;
			}
		}

		[Description("Controls Default Value e.g. %1% or a literal value e.g. '123'"), LocCategory("Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof(EnquryControlStaticDefaults))]
		public string DefaultValue
		{
			get
			{
				if (_rw != null)
					return Convert.ToString(_rw["quDefault"]);
				else
					return "";
			}
			set
			{
				if (_rw != null)
				{
					if (value != DefaultValue)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					if (value.Length > 249)
						_rw["quDefault"] = value.Substring(0,249);
					else
						_rw["quDefault"] = value;
					_enq.RenderControl(ref _ctrl,_rw);
				}
			}
		}

		[Description("The Tab Order. When you keyboard navigate through the Controls"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MergableProperty(false)]
		public int TabIndex
		{
			get
			{
				if (_rw != null)
					return ConvertDef.ToInt32(_rw["quTabOrder"],0);
				else
					return -1;
			}
			set
			{
				if (_rw != null)
				{
					if (value != TabIndex)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quTabOrder"] = value;
				}
			}
		}

		[Description("The Tab Stop. the control is ignored when Tabbing through the control"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool TabStop
		{
			get
			{
				if (_rw != null)
					return (_rw["quTabOrder"] != DBNull.Value);
				else
					return true;
			}
			set
			{
				if (_rw != null)
				{
					if (value != this.TabStop)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					if (value == false)
						_rw["quTabOrder"] = DBNull.Value;
					else
						_rw["quTabOrder"] = 0;
				}
			}
		}

		[Description("Enter the Help Keyword that links to the Help File"), LocCategory("Help")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string HelpKeyword
		{
			get
			{
				if (_rw != null)
					return Convert.ToString(_rw["quHelpKeyword"]);
				else
					return "";
			}
			set
			{
				if (_rw != null)
				{
					if (value != HelpKeyword)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quHelpKeyword"] = value;
				}
			}
		}
		
		[Description("The Maximum Length of the Data for this Control"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int MaxLength
		{
			get
			{
				if (_rw != null)
					return ConvertDef.ToInt32(_rw["quMaxLength"],0);
				else
					return 0;
			}
			set
			{
				if (_rw != null)
				{
					if (value != MaxLength)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quMaxLength"] = value;
				}
			}
		}

		[Description("The Size of the Control"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[RefreshProperties(RefreshProperties.All)]
		public Size Size
		{
			get
			{
				try
				{
					if (_rw != null)
						return new Size(ConvertDef.ToInt32(_rw["quWidth"],0),ConvertDef.ToInt32(_rw["quHeight"],0));
					else
						return new Size(0,0);
				}
				catch
				{
					return new Size(0,0);
				}
			}
			set
			{
				if (_rw != null && value != Size)
				{
					FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quHeight"] = value.Height;
					_rw["quWidth"] = value.Width;
					_ctrl.SuspendLayout();
					if (_ctrl != null) _ctrl.Size = this.Size;
					_ctrl.ResumeLayout();

				}
			}
		}

		[Description("The Location of the Control on the Standard Layout"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Point LocationStandard
		{
			get
			{
				if (_rw != null)
					return new Point(ConvertDef.ToInt32(_rw["quX"],0),ConvertDef.ToInt32(_rw["quY"],0));
				else
					return new Point(0,0);
			}
			set
			{
				if (_rw != null)
				{
					if (value != LocationStandard)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quY"] = value.Y;
					_rw["quX"] = value.X;
					if (_enq.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Standard)
						this.Control.Location = this.LocationStandard;
				}
			}
		}
		
		[Description("The Location of the Control on the Wizard Layout"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Point LocationWizard
		{
			get
			{
				if (_rw != null)
					return new Point(ConvertDef.ToInt32(_rw["quWizX"],0),ConvertDef.ToInt32(_rw["quWizY"],0));
				else
					return new Point(0,0);
			}
			set
			{
				if (_rw != null)
				{
					if (value != LocationWizard)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quWizY"] = value.Y;
					_rw["quWizX"] = value.X;
					if (_enq.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Wizard)
						this.Control.Location = this.LocationWizard;
				}
			}
		}

		[Description("The Mask for the Controls Data"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Mask
		{
			get
			{
				if (_rw != null)
					return Convert.ToString(_rw["quMask"]);
				else
					return "";
			}
			set
			{
				if (_rw != null)
				{
					if (value != Mask)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					if (value.Length > 99)
						_rw["quMask"] = value.Substring(0,99);
					else
						_rw["quMask"] = value;
				}
			}
		}

		[Description("The Width of the Caption of the Control"), LocCategory("Design")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(-1)]
		public int CaptionWidth
		{
			get
			{
				if (_rw != null)
					return ConvertDef.ToInt32(_rw["quCaptionWidth"],0);
				else
					return 0;
			}
			set
			{
				if (_rw != null)
				{
					if (value != CaptionWidth)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
                    if (this.Control is FWBS.Common.UI.IBasicEnquiryControl2)
                    {
                        if (((FWBS.Common.UI.IBasicEnquiryControl2)this.Control).CaptionTop)
                            value = 0;
                        else
                            ((FWBS.Common.UI.IBasicEnquiryControl2)this.Control).CaptionWidth = value;
                    }
                    _rw["quCaptionWidth"] = value;
				}
			}
		}

		[Description("Set Command Button to do"), LocCategory("Commands")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Parameter(CodeLookupDisplaySettings.Commands)]
		[CodeLookupSelectorTitle("COMMANDS","Screen Commands")]
		public CodeLookupDisplayReadOnly Command
		{
			get
			{
				return _commands;
			}
			set
			{
				if (_rw != null)
				{
					if (value != Command)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					if (value.Code == "")
					_rw["quCommand"] = DBNull.Value;
					else
					_rw["quCommand"] = value.Code;
					_commands = value;
					_enq.RenderControl(ref _ctrl,_rw,"quCommand");
				}
			}
		}

		[Description("Should the Control reflect the Return Value of the Command"), LocCategory("Commands")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AcceptReturnValue
		{
			get
			{
				try
				{
					if (_rw != null)
						return Convert.ToBoolean(_rw["quCommandRetVal"]);
					else
						return false;
				}
				catch
				{
					return true;
				}
			}
			set
			{
				if (_rw != null)
				{
					if (value != AcceptReturnValue)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quCommandRetVal"] = value;
				}
			}
		}

		[Description("Is the Control a System Control and cannot be changed"), LocCategory("System")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SystemControl
		{
			get
			{
				try
				{
					if (_rw != null)
						return Convert.ToBoolean(_rw["quSystem"]);
					else
						return false;
				}
				catch
				{
					return true;
				}
			}
			set
			{
				if (_rw != null)
				{
					if (value != SystemControl)
						FWBS.OMS.Design.frmDesigner.CreateUndoRowP(_rw);
					_rw["quSystem"] = value;
				}
			}
		}
		#endregion

		#region Deligates
		#endregion

		#region Filter Class
		public class Filter : LookupTypeDescriptor
		{
			public XmlNode _node;
			private string _control = "";
			private string _fieldname = "";
			private FWBS.OMS.UI.Windows.EnquiryForm _enquiryform = null;
			private string _controlname = "";
			
			public Filter(XmlNode node, string control, string fieldname, FWBS.OMS.UI.Windows.EnquiryForm enquiryform, string controlname)
			{
				_node=node;
				_control = control;
				_fieldname = fieldname;
				_enquiryform = enquiryform;
				_controlname = controlname;
			}

			public override string ToString()
			{
				return _control;
			}

			[LocCategory("Data")]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[TypeConverter(typeof(IListEnquiryControlLister))]
			public string Control
			{
				get
				{
					return _control;
				}
				set
				{
					_control = value;
				}
			}

			[LocCategory("Data")]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public string Fieldname
			{
				get
				{
					return _fieldname;
				}
				set
				{
					_fieldname = value;
				}
			}

			[Browsable(false)]
			public FWBS.OMS.UI.Windows.EnquiryForm EnquiryForm
			{
				get
				{
					return _enquiryform;
				}
			}

			[Browsable(false)]
			public string ControlName
			{
				get
				{
					return _controlname;
				}
			}

		}

		/// <summary>
		/// Filters Collection Editor
		/// </summary>
		public class FilterEditor : CollectionEditorEx
		{
			public FilterEditor() : base (typeof(FilterCollection)) 
			{
			}

			protected override System.Type CreateCollectionItemType ( )
			{
				return typeof (Filter);
			}

			protected override object CreateInstance(System.Type t)
			{
				FWBS.OMS.UI.Windows.EnquiryForm enq = (FWBS.OMS.UI.Windows.EnquiryForm)TypeDescriptor.GetProperties(this.Context.Instance)["EnquiryForm"].GetValue(this.Context.Instance);
				string crl = TypeDescriptor.GetProperties(this.Context.Instance)["Name"].GetValue(this.Context.Instance).ToString();
				return new Filter(null,"","",enq,crl);
			}

			protected override Type[] CreateNewItemTypes ( )
			{
				return new Type[] {typeof(Filter)};
			}
		}
		#endregion

		#region Custom Class
		public class Custom : LookupTypeDescriptor
		{
			private string _property = "";
			private string _value = "";
			
			public Custom(string Property, string Value)
			{
				_property = Property;
				_value = Value;
			}

			public override string ToString()
			{
				return _property + ": "  + _value;
			}

			[LocCategory("Data")]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public string Property
			{
				get
				{
					return _property;
				}
				set
				{
					_property = value;
				}
			}

			[LocCategory("Data")]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public string Value
			{
				get
				{
					return _value;
				}
				set
				{
					_value = value;
				}
			}
		}

		/// <summary>
		/// Customs Collection Editor
		/// </summary>
		public class CustomEditor : CollectionEditorEx
		{
			public CustomEditor() : base (typeof(CustomCollection)) 
			{
			}

			protected override System.Type CreateCollectionItemType ( )
			{
				return typeof (Custom);
			}

			protected override object CreateInstance(System.Type t)
			{
				return new Custom("","");
			}

			protected override Type[] CreateNewItemTypes ( )
			{
				return new Type[] {typeof(Custom)};
			}
		}
		#endregion

		#region Collection
		public class CustomCollection : Crownwood.Magic.Collections.CollectionWithEvents
		{
			public Custom Add(Custom value)
			{
				// Use base class to process actual collection operation
				base.List.Add(value as object);

				return value;
			}

			public void AddRange(Custom[] values)
			{
				// Use existing method to add each array entry
				foreach(Custom page in values)
					Add(page);
			}

			public void Remove(Custom value)
			{
				// Use base class to process actual collection operation
				base.List.Remove(value as object);
			}

			public void Insert(int index, Custom value)
			{
				// Use base class to process actual collection operation
				base.List.Insert(index, value as object);
			}

			public bool Contains(Custom value)
			{
				// Use base class to process actual collection operation
				return base.List.Contains(value as object);
			}

			public Custom this[int index]
			{
				// Use base class to process actual collection operation
				get { return (base.List[index] as Custom); }
			}

			public int IndexOf(Custom value)
			{
				// Find the 0 based index of the requested entry
				return base.List.IndexOf(value);
			}
		}

		public class FilterCollection : Crownwood.Magic.Collections.CollectionWithEvents
		{
			public Filter Add(Filter value)
			{
				// Use base class to process actual collection operation
				base.List.Add(value as object);

				return value;
			}

			public void AddRange(Filter[] values)
			{
				// Use existing method to add each array entry
				foreach(Filter page in values)
					Add(page);
			}

			public void Remove(Filter value)
			{
				// Use base class to process actual collection operation
				base.List.Remove(value as object);
			}

			public void Insert(int index, Filter value)
			{
				// Use base class to process actual collection operation
				base.List.Insert(index, value as object);
			}

			public bool Contains(Filter value)
			{
				// Use base class to process actual collection operation
				return base.List.Contains(value as object);
			}

			public Filter this[int index]
			{
				// Use base class to process actual collection operation
				get { return (base.List[index] as Filter); }
			}

			public int IndexOf(Filter value)
			{
				// Find the 0 based index of the requested entry
				return base.List.IndexOf(value);
			}
		}
		#endregion
	}

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class Unique
	{
		#region Fields
		private DataRow _rw;
		#endregion

		#region Constructor
		public Unique(DataRow rw)
		{
			_rw = rw;
		}
		#endregion

		#region Properties

		[Description("Is the Controls Data Unique")]
		public bool IsUnique
		{
			get
			{
				try
				{
					if (_rw != null)
						return Convert.ToBoolean(_rw["quUnique"]);
					else
						return false;
				}
				catch
				{
					return true;
				}
			}
			set
			{
				if (_rw != null)
					_rw["quUnique"] = value;
			}
		}

		[Description("The Index Name used to make the Control Unique withing the Database")]
		public string Constraint
		{
			get
			{
				if (_rw != null)
					return Convert.ToString(_rw["quConstraint"]);
				else
					return "";
			}
			set
			{
				if (_rw != null)
					if (value.Length > 74)
						_rw["quConstraint"] = value.Substring(0,74);
					else
						_rw["quConstraint"] = value;
			}
		}

		public override string ToString()
		{
			return this.IsUnique.ToString();
		}
		#endregion
	}

	[TypeConverter(typeof(ExpandableObjectConverter))]
	[Editor(typeof(ControlType.ControlTypeEditor),typeof(UITypeEditor))]
	public class ControlType
	{
		#region Fields
		private System.Windows.Forms.Control _ctrl;
		private DataRow _rw;
		private FWBS.OMS.UI.Windows.EnquiryForm _enq;
		#endregion

		#region Constructor
		public ControlType()
		{
		}
		#endregion

		#region Properties

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public System.Windows.Forms.Control Control
		{
			get
			{
				return _ctrl;
			}
			set
			{
				_ctrl = value;
				
			}
		}
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FWBS.OMS.UI.Windows.EnquiryForm EnquiryForm
		{
			get
			{
				return _enq;
			}
			set
			{
				_enq = value;
			}
		}
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataRow EnquiryRow
		{
			get
			{
				return _rw;
			}
			set
			{
				_rw = value;
			}
		}

		[Browsable(false)]
		[RefreshProperties(RefreshProperties.All)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Int32 ControlID
		{
			get
			{
				try
				{
					if (_rw != null)
						return Convert.ToInt32(_rw["quctrlid"]);
					else
						return 0;
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				if (_rw != null)
				{
					_rw["quctrlid"] = value;
					if (this.Control is IList == false && this.Control is FWBS.Common.UI.IListEnquiryControl == false)
						_rw["qudatalist"] = DBNull.Value;
					try
					{
						_enq.RenderControl(ref _ctrl,_rw,"quctrlid");
						if (_ctrl is FWBS.Common.UI.IBasicEnquiryControl2 && ((FWBS.Common.UI.IBasicEnquiryControl2)_ctrl).LockHeight)
						{
							FWBS.Common.UI.IBasicEnquiryControl2 e2 = (FWBS.Common.UI.IBasicEnquiryControl2)_ctrl;
							System.Windows.Forms.Control n = (System.Windows.Forms.Control)e2.Control;
							if (n.Height != _ctrl.Height)
							{
								_ctrl.Height = n.Height;
								_rw["quHeight"] = _ctrl.Height;
							}
						}
					}
					catch
					{
					}
				}
				_enq.RenderControls(true);
			}
		}

		public override string ToString()
		{
            if (_enq.Enquiry != null)
            {
                DataView dv = new DataView(_enq.Enquiry.Source.Tables["CONTROLS"]);
                dv.RowFilter = "ctrlID = " + this.ControlID.ToString();
                if (dv.Count == 1)
                    return Convert.ToString(dv[0]["ctrlCode"]);
                else
                    return "";
            }
            else
                return "";
		}
		#endregion

		#region Editors
		/// <summary>
		/// Summary description for ControlTypeEditor.
		/// </summary>
		internal class ControlTypeEditor : UITypeEditor
		{
			private IWindowsFormsEditorService iWFES;
			public ControlTypeEditor(){}

			public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
			{
				return UITypeEditorEditStyle.Modal ; 
			}

			public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
				frmListSelector frmListSelector1 = new frmListSelector();
				frmListSelector1.CodeType = "";
				object contextSingleInstance = null;
				DataTable _dt = null;
				try
				{
					if (context.Instance is System.Array)
					{
						contextSingleInstance = ((System.Array)context.Instance).GetValue(0);
						_dt = ((FWBS.OMS.UI.Windows.EnquiryForm)TypeDescriptor.GetProperties(contextSingleInstance)["EnquiryForm"].GetValue(contextSingleInstance)).Enquiry.Source.Tables["CONTROLS"];
					}
					else
						_dt = ((FWBS.OMS.UI.Windows.EnquiryForm)TypeDescriptor.GetProperties(context.Instance)["EnquiryForm"].GetValue(context.Instance)).Enquiry.Source.Tables["CONTROLS"];
					_dt.Columns.Add("fullctrldesc",typeof(System.String),"ctrlgroup + ' | ' + ctrldesc");
				}
				catch
				{}
				_dt.DefaultView.RowFilter="";
				_dt.DefaultView.Sort = "fullctrldesc";
				frmListSelector1.List.DataSource = _dt;
				frmListSelector1.List.DisplayMember = "fullctrldesc";
				frmListSelector1.List.ValueMember = "ctrlID";
				if (Convert.ToString(value) != "")
					frmListSelector1.List.SelectedValue = ((FWBS.OMS.UI.Windows.Admin.ControlType)value).ControlID;
				frmListSelector1.ShowHelp = false;
				iWFES.ShowDialog(frmListSelector1);
				if (frmListSelector1.DialogResult == System.Windows.Forms.DialogResult.OK)
				{
					if (context.Instance is System.Array)
					{
						Array contextInstances = ((System.Array)context.Instance);
						FWBS.OMS.UI.Windows.Admin.ControlType[] valueInstances = new FWBS.OMS.UI.Windows.Admin.ControlType[contextInstances.Length];
						for (int i = 0; i < contextInstances.Length; i++)
						{
							FWBS.OMS.UI.Windows.Admin.EnquiryControl m = contextInstances.GetValue(i) as FWBS.OMS.UI.Windows.Admin.EnquiryControl;
							m.ControlType.ControlID = Convert.ToInt32(frmListSelector1.List.SelectedValue);
						}
						return value;
					}
					else
					{
						((FWBS.OMS.UI.Windows.Admin.ControlType)value).ControlID = Convert.ToInt32(frmListSelector1.List.SelectedValue);
					}
				}
				return value;
			}
		}
		#endregion
	}

}
