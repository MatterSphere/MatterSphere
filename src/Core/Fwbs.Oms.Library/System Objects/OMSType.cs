using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Xml;
using FWBS.Common;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{

    /// <summary>
    /// An OMS configurable type that describes how a certain IOMSType object looks
    /// to whatever interface is available.
    /// </summary>
    public abstract class OMSType : LookupTypeDescriptor, IDisposable
	{
		#region Extensibility Events

		
		protected void OnObjectEvent(Extensibility.ObjectEventArgs e)
		{
			Session.CurrentSession.OnObjectEvent(e);
		}

		protected Extensibility.ObjectEventArgs OnObjectEvent(Extensibility.ObjectEvent ev)
		{
			//Call the extensibility event for addins.
			Extensibility.ObjectEventArgs e = new Extensibility.ObjectEventArgs(this, Extensibility.ObjectEvent.Loaded);
			Session.CurrentSession.OnObjectEvent(e);
			return e;
		}

		#endregion

		#region Dirty

		public event EventHandler Dirty;

		// Invoke the Dirty event; when changed
		public virtual void OnDirty() 
		{
			if (Dirty != null)
			{
				IsDirty = true;
				Dirty(this, EventArgs.Empty);
			}
		}

		#endregion

		#region Fields

		internal static int fetchcount = 0;

		/// <summary>
		/// A dirty boolean flag.
		/// </summary>
		private bool _isDirty = false;

		/// <summary>
		/// Holds the type description.
		/// </summary>
		private string _description = "";

		/// <summary>
		/// The forms lookup title.
		/// </summary>
		private CodeLookupDisplay _frmlookup;

		/// <summary>
		/// Tabs collection.
		/// </summary>
		private OMSType.TabsCollection _tabs = null;

		/// <summary>
		/// Panels collection.
		/// </summary>
		private OMSType.PanelsCollection _panels = null;

		/// <summary>
		/// Extended Data Collection.
		/// </summary>
		private OMSType.ExtendedDataCollection _extData = null;

		/// <summary>
		/// Default Template Data Collection.
		/// </summary>
		protected OMSType.DefaultTemplateCollection _defaults = null;

		/// <summary>
		/// XML type configuration settings.
		/// </summary>
		protected XmlDocument _config;
		/// <summary>
		/// Xml node for form header information.
		/// </summary>
		protected XmlElement _formHeader;
		/// <summary>
		/// Xml node for tab header information.
		/// </summary>
		protected XmlElement _tabHeader;
		/// <summary>
		/// Xml nodes for panel header information.
		/// </summary>
		protected XmlElement _panelHeader;

		
		/// <summary>
		/// Xml nodes for Extended Data header information.
		/// </summary>
		protected XmlElement _extendedHeader;

		/// <summary>
		/// Holds setting specific information of the object type like the wizard.
		/// </summary>
		protected XmlElement _settings;

		/// <summary>
		/// Holds defaultTemplate Overrides specific information of the object.
		/// </summary>
		protected XmlElement _defaultTemplatesHeader;


		/// <summary>
		/// Holds the unique type code.
		/// </summary>
		protected string _code = "";

		/// <summary>
		/// The Back Color
		/// </summary>
		protected System.Drawing.Color _backcolor = System.Drawing.Color.Empty;

	
		#endregion

		#region Constructors

		public OMSType()
		{
			BuildXML();
		}

		protected OMSType(string code)
		{
			Fetch(code);
			BuildXML();
		}

		#endregion

		#region Abstraction Layer

		public abstract bool Exists(string Code);

		/// <summary>
		/// Gets a flag value indicating whether the current type is being added.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public abstract bool IsNew {get;}

		/// <summary>
		/// Gets the code lookup group for the oms type so that the cultured names
		/// of the OMS types can be stored under their own categories.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public abstract string CodeLookupGroup{get;}

		/// <summary>
		/// Gets an object of the derived type.
		/// 		/// </summary>
		/// <param name="id">A unique identifer for the object.</param>
		/// <returns>An IOMSType compaitble object.</returns>
		public abstract IOMSType GetObject(object id);

		/// <summary>
		/// Creates an instance of the IOMSType.
		/// </summary>
		/// <returns>An OMSType object.</returns>
		public IOMSType CreateObject()
		{
			return CreateObject(null);
		}

        /// <summary>
        /// Creates an instance of the IOMSType.
        /// </summary>
        /// <param name="parameters">Specifies the parameters needed to construct the object.</param>
        /// <returns>An IOMSType object.</returns>
        public abstract IOMSType CreateObject(object [] parameters);

		/// <summary>
		/// Gets the object type name.
		/// </summary>
		[LocCategory("(DETAILS)")]
		[Lookup("TYPE")]
		public abstract Type OMSObjectType{get;}

		/// <summary>
		/// Fetches the xml oms type source.
		/// </summary>
		/// <returns></returns>
		protected abstract string FetchXmlSource();

		#endregion

		#region Manipulation Methods

		public abstract void Fetch (string code);

		public abstract bool Restore();

		public abstract bool Delete();

		public virtual void Cancel()
		{
			_panels = null;
			_tabs = null;
			_extData = null;
			_defaults = null;
			IsDirty = false;
		}

		public virtual void Update()
		{
			//New addin object event arguments
			ObjectState state = State;
            Extensibility.ObjectEvent oev;
            Extensibility.ObjectEventArgs oea;
            if (State != ObjectState.Unchanged)
            {
                switch (state)
                {
                    case ObjectState.Added:
                        oev = Extensibility.ObjectEvent.Creating;
                        break;
                    case ObjectState.Deleted:
                        oev = Extensibility.ObjectEvent.Deleting;
                        break;
                    case ObjectState.Modified:
                        oev = Extensibility.ObjectEvent.Updating;
                        break;
                    default:
                        goto case ObjectState.Added;
                }
                oea = new Extensibility.ObjectEventArgs(this, oev, true);
                OnObjectEvent(oea);
                if (oea.Cancel)
                    return;
            }

			int count = this.Panels.Count;
			count = this.Tabs.Count;
			count = this.ExtData.Count;

			for(int ctr = _panelHeader.ChildNodes.Count -1; ctr >= 0 ; ctr--)
			{
				XmlNode nd = _panelHeader.ChildNodes[ctr];
				if (nd is XmlElement)
					_panelHeader.RemoveChild(nd);
			}

			for(int ctr = _tabHeader.ChildNodes.Count - 1; ctr >= 0; ctr--)
			{
				XmlNode nd = _tabHeader.ChildNodes[ctr];
				if (nd is XmlElement)
					_tabHeader.RemoveChild(nd);
			}

			for(int ctr = _extendedHeader.ChildNodes.Count - 1; ctr >= 0; ctr--)
			{
				XmlNode nd = _extendedHeader.ChildNodes[ctr];
				if (nd is XmlElement)
					_extendedHeader.RemoveChild(nd);
			}

			if (_defaults != null)
			{
				for(int ctr = _defaultTemplatesHeader.ChildNodes.Count - 1; ctr >= 0; ctr--)
				{
					XmlNode nd = _defaultTemplatesHeader.ChildNodes[ctr];
					if (nd is XmlElement)
						_defaultTemplatesHeader.RemoveChild(nd);
				}
			}

			
			foreach (OMSType.Panel pnl in Panels)
			{
				_panelHeader.AppendChild(pnl.Element.Clone());
			}
			foreach (OMSType.Tab tab in Tabs)
			{
				_tabHeader.AppendChild(tab.Element.Clone());
			}
			
			System.Collections.SortedList exts = new System.Collections.SortedList();
			foreach (OMSType.ExtendedData ext in ExtData)
			{
				if (exts[ext.Code] == null)
				{
					_extendedHeader.AppendChild(ext.Element.Clone());
					exts.Add(ext.Code,ext.Code);
				}
			}
			if (_defaults != null)
			{
				foreach (OMSType.DefaultTemplate def in _defaults)
				{
					_defaultTemplatesHeader.AppendChild(def.Element.Clone());
				}
			}

            if (State != ObjectState.Unchanged)
            {
                //New addin object event arguments
                switch (state)
                {
                    case ObjectState.Added:
                        oev = Extensibility.ObjectEvent.Created;
                        break;
                    case ObjectState.Deleted:
                        oev = Extensibility.ObjectEvent.Deleted;
                        break;
                    case ObjectState.Modified:
                        oev = Extensibility.ObjectEvent.Updated;
                        break;
                    default:
                        goto case ObjectState.Added;
                }
                oea = new Extensibility.ObjectEventArgs(this, oev, false);
                OnObjectEvent(oea);
            }
		}

		public abstract OMSType Clone();

		public abstract DataTable GetTypes(bool active);

		[System.ComponentModel.Browsable(false)]
		public abstract string SearchListName {get;}

		[System.ComponentModel.Browsable(false)]
		public bool IsDirty
		{
			get
			{
				return _isDirty;
			}
			set
			{
				_isDirty = value;
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the code lookup code for the oms type so that the cultured text
		/// of the OMS type can be stored in code lookups.
		/// </summary>
		[LocCategory("(DETAILS)")]
		public virtual string Code 
		{
			get
			{
				return _code;
			}
			set
			{
				if (Exists(value))
				{
					if (value != Code)
						throw new ExtendedDataException(HelpIndexes.ExtendedDataCodeAlreadyExists,value);
				}
				else
				{
					if (IsNew)
					{		
						_code = value;
						OnDirty();
					}
					else
					{
						throw new OMSException2("ERRCODECANTCHGE", "The Code cannot be changed when set");
					}
				}
			}
		}
		
		/// <summary>
		/// Gets the localized description of the OMS type.
		/// </summary>
		[LocCategory("(DETAILS)")]
		public virtual string Description
		{
			get
			{
				return _description;
			}
			set
			{
				if (Code == "")
				{
					throw new FWBS.OMS.OMSTypeException(HelpIndexes.OMSTypeNoCode);
				}
				else
				{
					_description = value;
					FWBS.OMS.CodeLookup.Create(CodeLookupGroup,Code,_description,"",CodeLookup.DefaultCulture,true,true,true);
					OnDirty();
				}
			}
		}

		/// <summary>
		/// Gets or Sets the Help URL of the file type
		/// </summary>
		[LocCategory("HELP")]
		public string HelpURL
		{
			get
			{
				return ReadAttribute(_settings,"helpUrl","");
			}
			set
			{
				WriteAttribute(_settings,"helpUrl",value);
			}
		}
		
		/// <summary>
		/// Gets the current version number of the type.
		/// This is used for caching purposes on the client machine.
		/// </summary>
		[LocCategory("DATA")]
		public abstract long Version {get;}
		
		/// <summary>
		/// Gets or Sets the types icon index for the internal icon resources
		/// </summary>
		[LocCategory("DESIGN")]
		public abstract int Glyph {get;set;}

        public virtual System.Drawing.Icon GetAlternateGlyph()
        {
            return null;
        }

		/// <summary>
		/// Gets or Sets the wizard that gets used when creating the OMS type.
		/// </summary>
		[LocCategory("SETTINGS")]
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.EnquiryFormEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
		public virtual string Wizard
		{
			get
			{
				return ReadAttribute(_settings,"wizard","").Trim();
			}
			set
			{
				WriteAttribute(_settings,"wizard",value);
			}
		}

		/// <summary>
		/// Gets or Sets the SearchOnCreate atrtibute of the creation wizard.  If this
		/// is set to true then the wizard should display a conflict search first.
		/// </summary>
		[LocCategory("SETTINGS")]
		public virtual bool SearchOnCreate
		{
			get
			{
				return Convert.ToBoolean(ReadAttribute(_settings,"searchOnCreate","true"));
			}
			set
			{
				WriteAttribute(_settings,"searchOnCreate", value);
			}
		}

        [Lookup("FormTitle")]
		[LocCategory("DATA")]
		[CodeLookupSelectorTitle("FORMTITLES","Form Titles")]
		public virtual CodeLookupDisplay FormTitle
		{
			get
			{
				if (_frmlookup == null)
				{
					_frmlookup = new CodeLookupDisplay("DLGFRMCAPTION");
					_frmlookup.Code = FormLookup;
				}
				return _frmlookup;
			}
			set
			{
				_frmlookup = value;
				this.FormLookup = value.Code;
			}
		}

		/// <summary>
		/// Gets or Sets the forms lookup text.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public virtual string FormLookup
		{
			get
			{
				return ReadAttribute(_formHeader,"lookup","");
			}
			set
			{
				WriteAttribute(_formHeader,"lookup", value);
			}
		}

		/// <summary>
		/// Gets the forms description.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public string FormDescription
		{
			get
			{
				return ReadAttribute(_formHeader,"frmdesc","");
			}
		}

		/// <summary>
		/// Gets or Sets the panels default width.
		/// </summary>
		[LocCategory("PANELS")]
		[Lookup("WIDTH")]
		public virtual int PanelWidth
		{
			get
			{
				return Convert.ToInt32(ReadAttribute(_panelHeader,"width","180"));
			}
			set
			{
				WriteAttribute(_panelHeader,"width", value);
			}
		}

		/// <summary>
		/// Gets or Sets the panels brightness.
		/// </summary>
		[LocCategory("PANELS")]
		[Lookup("BRIGHTNESS")]
		public virtual int PanelBrightness
		{
			get
			{
				return Convert.ToInt32(ReadAttribute(_panelHeader,"brightness","50"));
			}
			set
			{
				WriteAttribute(_panelHeader,"brightness", value);
			}
		}

		/// <summary>
		/// Gets or Sets the panels back colour.
		/// </summary>
		[LocCategory("PANELS")]
		[Lookup("BACKCOLOR")]
		public virtual System.Drawing.Color PanelBackColour
		{
			get
			{
				if (_backcolor == System.Drawing.Color.Empty)
				{
					_backcolor = System.Drawing.Color.FromName(ReadAttribute(_panelHeader,"backcolor", "RosyBrown"));
					if (!_backcolor.IsKnownColor)
					{
						_backcolor = System.Drawing.Color.FromArgb(Convert.ToInt32(ReadAttribute(_panelHeader,"backcolor", "-4419697")));
					}
					
				}
				return _backcolor;
			}
			set
			{
				_backcolor = value;
				if (value.IsKnownColor)
					WriteAttribute(_panelHeader,"backcolor", value.Name);
				else
					WriteAttribute(_panelHeader,"backcolor", value.ToArgb());
			}
		}

        /// <summary>
        /// Gets or Sets the panels back colour to Override the Theme Color.
        /// </summary>
        [LocCategory("PANELS")]
        public virtual bool OverrideTheme
        {
            get
            {
                return Convert.ToBoolean(ReadAttribute(_panelHeader, "overridetheme", "False"));
            }
            set
            {
                WriteAttribute(_panelHeader, "overridetheme", Convert.ToString(value));
            }
        }

        public static void RegisterExtendedDataAgainstAllTypes(Type type, string extendedCode, bool active)
        {
            extendedCode = extendedCode.ToUpperInvariant();

            DataTable dt = GetTypes(type, active);

            OMSType omstype = Session.CurrentSession.TypeManager.Create(type) as OMSType;
            if (omstype == null)
                throw new InvalidCastException("Not a valid OMS Type");

            foreach (DataRow row in dt.Rows)
            {
                bool found = false;

                OMSType got = GetOMSType(type, Convert.ToString(row["typecode"]));

                foreach (ExtendedData data in got.ExtData)
                {
                    if (data.Code == extendedCode)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                    continue;

                ExtendedData ed = new ExtendedData(got);
                ed.Code = extendedCode;

                got.ExtData.Add(ed);
                got.Update();


            }
        }


		/// <summary>
		/// Gets the Tab collection.
		/// </summary>
		[LocCategory("TABS")]
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.TabEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
		public OMSType.TabsCollection Tabs
		{
			get
			{
				if (_tabs == null)
				{
					_tabs = new TabsCollection(this, _tabHeader);
					_tabs.Cleared +=new Crownwood.Magic.Collections.CollectionClear(OnDirty);
				}
				return _tabs;
			}
		}

		/// <summary>
		/// Gets the Panels collection.
		/// </summary>
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.PanelEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
		[LocCategory("PANELS")]
		public OMSType.PanelsCollection Panels
		{
			get
			{
				if (_panels == null)
				{
					_panels = new PanelsCollection(this, _panelHeader);
					_panels.Cleared +=new Crownwood.Magic.Collections.CollectionClear(OnDirty);
				}
				return _panels;
			}
		}

		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.ExtDataEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
		[LocCategory("SETTINGS")]
		public OMSType.ExtendedDataCollection ExtData
		{
			get
			{
				if (_extData == null)
				{
					_extData = new ExtendedDataCollection(this, _extendedHeader);
					_extData.Cleared +=new Crownwood.Magic.Collections.CollectionClear(OnDirty);
				}
				return _extData;
			}
		}

		[Browsable(false)]
		public abstract ObjectState State{get;}
		

		#endregion

		#region Methods

		/// <summary>
		/// Gets an OMSType based on the the code and type sent.
		/// </summary>
        /// <param name="omstype"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		public static OMSType GetOMSType(Type omstype, string code)
		{
			OMSType t = null;
			if (omstype == typeof(ClientType))
			{
				return ClientType.GetClientType(code);
			}
			else if (omstype == typeof(ContactType))
			{
				return ContactType.GetContactType(code);
			}
			else if (omstype == typeof(FileType))
			{
				return FileType.GetFileType(code);
			}
            else if (omstype == typeof(AssociateType))
            {
                return AssociateType.GetAssociateType(code);
            }
			else if (omstype == typeof(UserType))
			{
				return UserType.GetUserType(code);
			}
			else if (omstype == typeof(FeeEarnerType))
			{
				return FeeEarnerType.GetFeeEarnerType(code);
			}
			else if (omstype == typeof(CommandCentreType))
			{
				return CommandCentreType.GetCentreType(code);
			}
            else if (omstype == typeof(DocType))
            {
                return DocType.GetDocType(code);
            }
			else
			{
				t = (OMSType)omstype.InvokeMember("", System.Reflection.BindingFlags.CreateInstance,null, null, null);
				t.Fetch(code);
			}
			return t;
		}

		public static DataTable GetTypes(Type omstype, bool active)
		{
			OMSType t = (OMSType)omstype.InvokeMember("", System.Reflection.BindingFlags.CreateInstance,null, null, null);
			return t.GetTypes(active);
		}

		public static string GetTypesSearchList(Type omstype)
		{
			OMSType t = (OMSType)omstype.InvokeMember("", System.Reflection.BindingFlags.CreateInstance,null, null, null);
			return t.SearchListName;
		}

		/// <summary>
		/// String representation of the object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Description;
		}

		//Clears the xml object so it has to be rebuilt.
		private void ClearXML()
		{
			_config = null;
		}

		/// <summary>
		/// Builds a schema of a default type.
		/// </summary>
		protected virtual void BuildXML()
		{
			//Create the document if it does not already exist.
			if (_config == null)
				_config = new XmlDocument();

			_config.PreserveWhitespace = false;
			_config.LoadXml(FetchXmlSource());

			XmlNode root = _config.SelectSingleNode("/Config");
			if (root == null)
			{
				root = _config.CreateElement("Config");
				_config.AppendChild(root);
			}
 
			XmlNode _xmlConfigDialogRoot = root.SelectSingleNode("Dialog");
			if (_xmlConfigDialogRoot == null)
			{
				_xmlConfigDialogRoot = _config.CreateElement("Dialog");
				root.AppendChild(_xmlConfigDialogRoot);
			}			

			_formHeader = _xmlConfigDialogRoot.SelectSingleNode("Form") as XmlElement;
			if (_formHeader == null)
			{
				_formHeader = _config.CreateElement("Form");
				_xmlConfigDialogRoot.AppendChild(_formHeader);
			}	

			_tabHeader = _xmlConfigDialogRoot.SelectSingleNode("Tabs") as XmlElement;
			if (_tabHeader == null)
			{
				_tabHeader = _config.CreateElement("Tabs");
				_xmlConfigDialogRoot.AppendChild(_tabHeader);
			}			

			_panelHeader = _xmlConfigDialogRoot.SelectSingleNode("Panels") as XmlElement;
			if (_panelHeader == null)
			{
				_panelHeader = _config.CreateElement("Panels");
				_xmlConfigDialogRoot.AppendChild(_panelHeader);
			}			

			_extendedHeader = root.SelectSingleNode("ExtendedDataList") as XmlElement;
			if (_extendedHeader == null)
			{
				_extendedHeader = _config.CreateElement("ExtendedDataList");
				root.AppendChild(_extendedHeader);
			}

			_settings = root.SelectSingleNode("Settings") as XmlElement;
			if (_settings == null)
			{
				_settings = _config.CreateElement("Settings");
				root.AppendChild(_settings);
			}

			_defaultTemplatesHeader = root.SelectSingleNode("defaultTemplates") as XmlElement;
			if (_defaultTemplatesHeader == null)
			{
				_defaultTemplatesHeader = _config.CreateElement("defaultTemplates");
				root.AppendChild(_defaultTemplatesHeader);
			}		
		}

		/// <summary>
		/// Gets the root element of the xml configuration.
		/// </summary>
		/// <returns></returns>
		public XmlElement GetConfigRoot()
		{
			return (XmlElement)	_config.SelectSingleNode("/Config");
		}

		/// <summary>
		/// Gets an attribute configuration.
		/// </summary>
		/// <param name="element">Current Element.</param>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="defaultValue">The default value to use.</param>
		/// <returns>A string value, empty string if it does not exists.</returns>
		public virtual string ReadAttribute(XmlElement element, string name, string defaultValue)
		{
			XmlNode att = element.SelectSingleNode(name);
			if (att == null)
				att = element.GetAttributeNode(name);

			string ret = String.Empty;

			if (att != null)
			{
				if (att.NodeType == XmlNodeType.Element)
					ret = att.InnerText;
				else
					ret = att.Value;
			}

			if (ret == String.Empty)
				ret = defaultValue;

			return ret;
		}

		/// <summary>
		/// Writes an attribute to the the specified element.
		/// </summary>
		/// <param name="element">The element to write the attribute to.</param>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="val">The value to set the attribute to.</param>
		public void WriteAttribute(XmlElement element, string name, object val)
		{
			XmlNode att = element.SelectSingleNode(name);
			if (att == null)
				att = element.GetAttributeNode(name);

			if (att != null)
			{
				if (att.NodeType == XmlNodeType.Element)
					att.InnerText = Convert.ToString(val);
				else
					att.Value = Convert.ToString(val);
			}
			else
			{
				if (name == "")
					element.InnerText = Convert.ToString(val);
				else
				{
					XmlAttribute a = _config.CreateAttribute(name);
					a.Value = Convert.ToString(val);
					element.Attributes.Append(a);
				}
				
			}

			OnDirty();
		}

		/// <summary>
		/// Reads a global type setting.
		/// </summary>
		public string GetSetting(string setting, string def)
		{
			return ReadAttribute(_settings, setting, def);
		}

		/// <summary>
		/// Sets a global type setting.
		/// </summary>
		public void SetSetting(string setting, string val)
		{
			WriteAttribute(_settings, setting, val);
		}


		#endregion
	
		#region IDisposable Implementation

		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		protected virtual void Dispose(bool disposing) 
		{
			if (disposing) 
			{
			}

		}


		#endregion
		
		#region Panel Class

		public enum PanelTypes {Property,TimeStatistics,Addin,DataList};
		
		public class Panel : LookupTypeDescriptor
		{
			private XmlElement _info;
			private OMSType _type;
			private bool _isNew = false;
			private CodeLookupDisplay _code = null;
			private NavButtonStyle _glyph;
			private CodeLookupDisplayMulti _usrroles = null;

	
			private Panel (){}

			public Panel(OMSType type)
			{
				_type = type;
				_info = type._config.CreateElement("Panel");
				_glyph = (NavButtonStyle)FWBS.Common.ConvertDef.ToEnum(_type.ReadAttribute(_info, "glyph", "Grey"),NavButtonStyle.Grey);
				_type.WriteAttribute(_info, "lookup", "");
				_isNew = true;
			}

			public Panel(OMSType type, XmlElement info)
			{
				_info = info;
				_type = type;
				_glyph = (NavButtonStyle)FWBS.Common.ConvertDef.ToEnum(_type.ReadAttribute(_info, "glyph", "Grey"),NavButtonStyle.Grey);
			}

			[System.ComponentModel.Browsable(false)]
			public bool IsNew
			{
				get
				{
					return _isNew;
				}
			}

			[System.ComponentModel.Browsable(false)]
			internal XmlElement Element
			{
				get
				{
					return _info;
				}
			}

			public override string ToString()
			{
				return LocalizedCode.Description;
			}


			[LocCategory("DATA")]
			[Lookup("CAPTION")]
			[CodeLookupSelectorTitle("PANELCAP","Panel Captions")]
			public virtual CodeLookupDisplay LocalizedCode
			{
				get
				{
					if (_code == null)
					{
						_code = new CodeLookupDisplay("DLGPNLCAPTION");
						_code.Code = this.Code;
					}
					return _code;
				}
				set
				{
					_code = value;
					this.Code = value.Code;
				}
			}

			[LocCategory("DATA")]
			[RefreshProperties(RefreshProperties.All)]
			public PanelTypes PanelType
			{
				get
				{
					return (PanelTypes)FWBS.Common.ConvertDef.ToEnum(_type.ReadAttribute(_info, "panelType", ""), PanelTypes.Property);
				}
				set
				{
					_type.WriteAttribute(_info, "panelType", value.ToString());
					this.Parameter = "";
				}
			}

			[System.ComponentModel.Browsable(false)]
			public virtual string Code
			{
				get
				{
					return _type.ReadAttribute(_info, "lookup", "");
				}
				set
				{
					_type.WriteAttribute(_info, "lookup", value);
				}

			}

			[System.ComponentModel.Browsable(false)]
			public virtual string Description
			{
				get
				{
					if (_code == null)
						return _type.ReadAttribute(_info, "pnldesc", "");
					else
						return _code.Description;
				}
			}

			[LocCategory("DESIGN")]
			public virtual bool Expanded
			{
				get
				{
					return Common.ConvertDef.ToBoolean(_type.ReadAttribute(_info, "expanded", "true"), true);
				}
				set
				{
					_type.WriteAttribute(_info, "expanded", value.ToString());
				}
			}

            [System.ComponentModel.Browsable(false)]
			[LocCategory("DESIGN")]
			public virtual int Height
			{
				get
				{
					return Common.ConvertDef.ToInt32(_type.ReadAttribute(_info, "height", "150"), 150);
				}
				set
				{
					_type.WriteAttribute(_info, "height", value);
				}
			}

			[LocCategory("Data")]
			[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.PanelParameterEditor,omsadmin",typeof(System.Drawing.Design.UITypeEditor))]
			public virtual string Parameter
			{
				get
				{
					return _type.ReadAttribute(_info, "property", "");
				}
				set
				{
					_type.WriteAttribute(_info, "property", value);
				}
			}


			[LocCategory("DESIGN")]
			[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.PanelGlyphDisplayEditor,omsadmin",typeof(System.Drawing.Design.UITypeEditor))]
			public virtual NavButtonStyle Glyph
			{
				get
				{
					return _glyph;
				}
				set
				{
					_glyph = value;
					_type.WriteAttribute(_info, "glyph", value.ToString());
				}
			}

			[LocCategory("PNLVISIBLE")]
			public string[] Conditional
			{
				get
				{
					return _type.ReadAttribute(_info, "conditional", "").Split(Environment.NewLine.ToCharArray());
				}
				set
				{
					_type.WriteAttribute(_info,"conditional",String.Join(Environment.NewLine,value));
				}
			}

			[Lookup("USERROLES")]
			[LocCategory("PNLVISIBLE")]
			[System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.All)]
			[CodeLookupSelectorTitle("USERROLES","User Roles")]
			public CodeLookupDisplayMulti UserRolesDisplay
			{
				get
				{
					if (_usrroles == null)
					{
						_usrroles = new CodeLookupDisplayMulti("USRROLES");
						_usrroles.Codes = this.UserRoles;
					}
					return _usrroles;
				}
				set
				{
					_usrroles = value;
					this.UserRoles = value.Codes;
				}
			}

			
			[Browsable(false)]
			public string UserRoles
			{
				get
				{
					return _type.ReadAttribute(_info, "userRoles", "");
				}
				set
				{
					_type.WriteAttribute(_info,"userRoles",value);
				}
			}

			[System.ComponentModel.Browsable(false)]
			public OMSType OMSType
			{
				get
				{
					return _type;
				}
			}

			[System.ComponentModel.Browsable(false)]
			public Type OMSObjectType
			{
				get
				{
					return OMSType.OMSObjectType;
				}
			}

		}

		#endregion

		#region Tab Class
		public class Tab : LookupTypeDescriptor
		{
			private XmlElement _info;
			private OMSType _type;
			private bool _isNew = false;
			private CodeLookupDisplay _code = null;
            private CodeLookupDisplay _group = null;
			private CodeLookupDisplayReadOnly _source = null;
			private CodeLookupDisplayMulti _usrroles = null;
	
			private Tab (){}

			public Tab(OMSType type)
			{
				_type = type;
				_info = type._config.CreateElement("Tab");
				_type.WriteAttribute(_info, "lookup", "");
				_isNew = true;
			}

			public Tab(OMSType type, XmlElement info) 
			{
				_info = info;
				_type = type;
			}

			[System.ComponentModel.Browsable(false)]
			public bool IsNew
			{
				get
				{
					return _isNew;
				}
			}

			[System.ComponentModel.Browsable(false)]
			internal XmlElement Element
			{
				get
				{
					return _info;
				}
			}

			public void Assign(Tab tab)
			{
				foreach (XmlAttribute att in tab.Element.Attributes)
					_type.WriteAttribute(_info, att.Name, tab.OMSType.ReadAttribute(tab.Element, att.Name, ""));

			}

			public override string ToString()
			{
				return Session.CurrentSession.Terminology.Parse(this.LocalizedCode.Description,true);
			}

			[System.ComponentModel.Browsable(false)]
			public virtual string Code
			{
				get
				{
					return _type.ReadAttribute(_info, "lookup", "");
				}
				set
				{
					_type.WriteAttribute(_info, "lookup", value);
				}
			}


            [System.ComponentModel.Browsable(false)]
            public virtual string Group
            {
                get
                {
                    return _type.ReadAttribute(_info, "group", "");
                }
                set
                {
                    _type.WriteAttribute(_info, "group", value);
                }
            }


            [Lookup("Group")]
            [LocCategory("GROUP")]
            [CodeLookupSelectorTitle("GROUP", "Group")]
            public virtual CodeLookupDisplay LocalizedGroup
            {
                get
                {
                    if (_group == null)
                    {
                        _group = new CodeLookupDisplay("DLGGROUPCAPTION");
                        _group.Code = this.Group;
                    }
                    return _group;
                }
                set
                {
                    _group = value;
                    this.Group = value.Code;
                }
            }


			[System.ComponentModel.Browsable(false)]
			public virtual string Description
			{
				get
				{
					if (_code == null)
						return _type.ReadAttribute(_info, "tabdesc", "");
					else
						return _code.Description;
				}
			}

			[Lookup("Description")]
			[LocCategory("DATA")]
			[CodeLookupSelectorTitle("TABCAP","Tab Captions")]
			public virtual CodeLookupDisplay LocalizedCode
			{
				get
				{
					if (_code == null)
					{
						_code = new CodeLookupDisplay("DLGTABCAPTION");
						_code.Code = this.Code;
					}
					return _code;
				}
				set
				{
					_code = value;
					this.Code = value.Code;
				}
			}

			public OMSObjectTypes SourceType
			{
				get
				{
					return (OMSObjectTypes)Common.ConvertDef.ToEnum(_type.ReadAttribute(_info, "tabtype", ""), OMSObjectTypes.Enquiry);
				}
			}

            [Lookup("HIDEBUTTONS")]
            [LocCategory("DESIGN")]
            public bool HideCancelSaveButtons
            {
                get
                {
                    return Convert.ToBoolean(_type.ReadAttribute(_info, "hidebuttons", "false"));
                }
                set
                {
                    _type.WriteAttribute(_info, "hidebuttons", value.ToString());
                }
            }

			[Parameter(CodeLookupDisplaySettings.omsObjects)]
			[Lookup("OMSObject")]
			[LocCategory("DESIGN")]
			[System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.All)]
			[CodeLookupSelectorTitle("OMSOBJECTS","OMS Objects")]
			public CodeLookupDisplayReadOnly LocalizedSource
			{
				get
				{
					if (_source == null)
					{
						_source = new CodeLookupDisplayReadOnly("OMSOBJECT");
						_source.Code = this.Source;
					}
					return _source;
				}
				set
				{
					_source = value;
					this.Source = value.Code;
				}
			}

			[Lookup("OMSOBJCODE")]
			[LocCategory("DESIGN")]
			public string OMSObjectCode
			{
				get
				{
					return _type.ReadAttribute(_info, "source", "");
				}
			}

			[LocCategory("TABVISIBLE")]
			public string[] Conditional
			{
				get
				{
					return _type.ReadAttribute(_info, "conditional", "").Split(Environment.NewLine.ToCharArray());
				}
				set
				{
					_type.WriteAttribute(_info,"conditional",String.Join(Environment.NewLine,value));
				}
			}

			[Lookup("USERROLES")]
			[LocCategory("TABVISIBLE")]
			[System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.All)]
			[CodeLookupSelectorTitle("USERROLES","User Roles")]
			public CodeLookupDisplayMulti UserRolesDisplay
			{
				get
				{
					if (_usrroles == null)
					{
						_usrroles = new CodeLookupDisplayMulti("USRROLES");
						_usrroles.Codes = this.UserRoles;
					}
					return _usrroles;
				}
				set
				{
					_usrroles = value;
					this.UserRoles = value.Codes;
				}
			}

			[Browsable(false)]
			public string UserRoles
			{
				get
				{
					return _type.ReadAttribute(_info, "userRoles", "");
				}
				set
				{
					_type.WriteAttribute(_info,"userRoles",value);
				}
			}
			
			[System.ComponentModel.Browsable(false)]
			public string Source
			{
				get
				{
					return _type.ReadAttribute(_info, "source", "");
				}
				set
				{
					_type.WriteAttribute(_info, "source", value);
				
					OmsObject obj = new OmsObject(value);

					if (obj.ObjectType == OMSObjectTypes.Enquiry)
					{
						DataTable dt = EnquiryEngine.Enquiry.HasExtendedData(obj.Windows);
						foreach(DataRow dr in dt.Rows)
						{
							OMSType.ExtendedData ext = new OMSType.ExtendedData(_type);
							ext.Code = Convert.ToString(dr["quExtendedData"]);
							_type.ExtData.Add(ext);
						}

					}
					else if (obj.ObjectType == OMSObjectTypes.ExtData)
					{
						bool exists = false;
						foreach(OMSType.ExtendedData e in _type.ExtData)
						{
							if (e.Code == obj.Windows)
							{
								exists = true;
								break;
							}
						}
						if (exists == false)
						{
							OMSType.ExtendedData ext = new OMSType.ExtendedData(_type);
							ext.Code = obj.Windows;
							_type.ExtData.Add(ext);
						}
					}
					_type.WriteAttribute(_info, "tabtype", obj.ObjectType);
				}
			}

			[System.ComponentModel.Browsable(false)]
			public string SourceParam
			{
				get
				{
					return _type.ReadAttribute(_info, "tabsource", "");
				}
			}

			[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.NavigationTabGlyphDisplayEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
			[System.ComponentModel.TypeConverter("FWBS.OMS.UI.Windows.Design.NavigationIconsLister,omsadmin")]
			[LocCategory("DESIGN")]
			public int Glyph
			{
				get
				{
					return Common.ConvertDef.ToInt32(_type.ReadAttribute(_info, "glyph", "-1"), -1);
				}
				set
				{
					_type.WriteAttribute(_info, "glyph", value);
				}
			}

			[System.ComponentModel.Browsable(false)]
			public OMSType OMSType
			{
				get
				{
					return _type;
				}
			}

			[System.ComponentModel.Browsable(false)]
			public Type OMSObjectType
			{
				get
				{
					return OMSType.OMSObjectType;
				}
			}

		}

		#endregion

		#region DefaultTemplate Class
		public class DefaultTemplate : LookupTypeDescriptor
		{
			private XmlElement _info;
			private OMSType _type;
			private bool _isNew = false;
			private CodeLookupDisplay _template = null;
			private PickAPrecedent _precedent = null;
			

			public DefaultTemplate(OMSType type)
			{
				_type = type;
				_info = type._config.CreateElement("template");
				_precedent = new PickAPrecedent(_type,_info);
				_isNew = true;
			}

			public DefaultTemplate(OMSType type, XmlElement info)
			{
				_type = type;
				_info = info;
				_precedent = new PickAPrecedent(_type,_info);
				_isNew = true;
			}

			[System.ComponentModel.Browsable(false)]
			public bool IsNew
			{
				get
				{
					return _isNew;
				}
			}

			[System.ComponentModel.Browsable(false)]
			public XmlElement Element
			{
				get
				{
					return _info;
				}
			}

			[Lookup("Template")]
			[LocCategory("(DETAILS)")]
			[CodeLookupUIAttribute(CodeLookupUIAttributes.ChangeCode)]
			[CodeLookupSelectorTitle("DEFTEMPLATES","Default Templates")]
			public CodeLookupDisplay TemplateUI
			{
				get
				{
					if (_template == null)
					{
						_template = new CodeLookupDisplayReadOnly("DEFTEMPS");
						_template.Code = this.Template;
					}
					return _template;
				}
				set
				{
					_template = value;
					this.Template = value.Code;
				}
			}

			[LocCategory("DATA")]
			public PickAPrecedent Precedent
			{
				get
				{
					return _precedent;
				}
				set
				{
					_precedent = value;
				}
			}

			[System.ComponentModel.Browsable(false)]
			public string Template
			{
				get
				{
					return Convert.ToString(_type.ReadAttribute(_info, "type", ""));
				}
				set
				{
					_type.WriteAttribute(_info, "type", value);
				}
			}

			[System.ComponentModel.Browsable(false)]
			public OMSType OMSType
			{
				get
				{
					return _type;
				}
			}

			[System.ComponentModel.Browsable(false)]
			public Type OMSObjectType
			{
				get
				{
					return OMSType.OMSObjectType;
				}
			}

			public override string ToString()
			{
				return this.TemplateUI.Description;
			}
		}
		#endregion

		#region PickAPrecedent Class
		[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.DefaultTemplateEditor,omsadmin",typeof(System.Drawing.Design.UITypeEditor))]
		public class PickAPrecedent : LookupTypeDescriptor
		{
			private XmlElement _info;
			private OMSType _type;

			public PickAPrecedent(OMSType type, XmlElement info)
			{
				_info = info;
				_type = type;
			}

			
			public PickAPrecedent(OMSType type, XmlElement info, string title, string library, string category, string subcategory, string minorcategory)
			{
				_info = info;
				_type = type;
				_type.WriteAttribute(_info, "title", title);
				_type.WriteAttribute(_info, "library", library);
				_type.WriteAttribute(_info, "category", category);
				_type.WriteAttribute(_info, "subcategory", subcategory);
                _type.WriteAttribute(_info, "minorcategory", minorcategory);
            }

			public string Title
			{
				get
				{
					return _type.ReadAttribute(_info, "title", "");
				}
			}

			public string Library
			{
				get
				{
					return _type.ReadAttribute(_info, "library", "");
				}
			}

			public string Category
			{
				get
				{
					return _type.ReadAttribute(_info, "category", "");
				}
			}

			public string SubCategory
			{
				get
				{
					return _type.ReadAttribute(_info, "subcategory", "");
				}
			}

            public string MinorCategory
            {
                get
                {
                    return _type.ReadAttribute(_info, "minorcategory", "");
                }
            }

            public override string ToString()
			{
				string tos = Title + "," + Library + "," + Category + "," + SubCategory;
				return tos.Replace(",,",",");
			}

		}
		#endregion

		#region ExtendedData Class

		public class ExtendedData : LookupTypeDescriptor
		{
			private XmlElement _info;
			private OMSType _type;
			private bool _isNew = false;
			private CodeLookupDisplayReadOnly _code = null;
			
			public ExtendedData(OMSType type)
			{
				_type = type;
				_info = type._config.CreateElement("ExtendedData");
				_isNew = true;
			}

			public ExtendedData(OMSType type, XmlElement info)
			{
				_info = info;
				_type = type;
			}

			[System.ComponentModel.Browsable(false)]
			public bool IsNew
			{
				get
				{
					return _isNew;
				}
			}

			[System.ComponentModel.Browsable(false)]
			internal XmlElement Element
			{
				get
				{
					return _info;
				}
			}

			public override string ToString()
			{
				return this.LocalizedCode.Description;
			}

			[System.ComponentModel.Browsable(false)]
			public string Code
			{
				get
				{
					return Convert.ToString(_type.ReadAttribute(_info, "code", ""));
				}
				set
				{
					_type.WriteAttribute(_info, "code", value);
				}
			}

			[Lookup("Code")]
			[LocCategory("DATA")]
			[Parameter(CodeLookupDisplaySettings.ExtendedData)]
			[CodeLookupSelectorTitle("EXTENDEDDATAS","Extended Datas")]
			public virtual CodeLookupDisplayReadOnly LocalizedCode
			{
				get
				{
					if (_code == null)
					{
						_code = new CodeLookupDisplayReadOnly("EXTENDEDDATA");
						_code.Code = this.Code;
					}
					return _code;
				}
				set
				{
					_code = value;
					this.Code = value.Code;
				}
			}

			[System.ComponentModel.Browsable(false)]
			public OMSType OMSType
			{
				get
				{
					return _type;
				}
			}

			[System.ComponentModel.Browsable(false)]
			public Type OMSObjectType
			{
				get
				{
					return OMSType.OMSObjectType;
				}
			}
		}

		#endregion

		#region Tabs OMS Type Collection
		public class TabsCollection : Crownwood.Magic.Collections.CollectionWithEvents
		{
			private XmlElement _info;
			private OMSType _type;
			
			private TabsCollection(){}

			public TabsCollection(OMSType type, XmlElement info)
			{
				_type = type;
				_info = info;

				foreach (XmlNode nd in _info.ChildNodes)
				{
					if (nd is XmlElement)
					{
						Add(new Tab(_type, (XmlElement)nd.Clone()));
					}
				}

			}

			public Tab Add(Tab value)
			{
				// Use base class to process actual collection operation
				base.List.Add(value as object);
				return value;
			}

			public void AddRange(Tab[] values)
			{
				// Use existing method to add each array entry
				foreach(Tab page in values)
					Add(page);
			}

			public void Remove(Tab value)
			{
				// Use base class to process actual collection operation
				base.List.Remove(value as object);
			}

			public void Insert(int index, Tab value)
			{
				// Use base class to process actual collection operation
				base.List.Insert(index, value as object);
			}

			public bool Contains(Tab value)
			{
				// Use base class to process actual collection operation
				return base.List.Contains(value as object);
			}

			public Tab this[int index]
			{
				// Use base class to process actual collection operation
				get { return (base.List[index] as Tab); }
			}

			public int IndexOf(Tab value)
			{
				// Find the 0 based index of the requested entry
				return base.List.IndexOf(value);
			}
		}
		#endregion

		#region Default Template Collection
		public class DefaultTemplateCollection : Crownwood.Magic.Collections.CollectionWithEvents
		{
			private XmlElement _info;
			private OMSType _type;
			
			private DefaultTemplateCollection(){}

			public DefaultTemplateCollection(OMSType type, XmlElement info)
			{
				_type = type;
				_info = info;

				foreach (XmlNode nd in _info.ChildNodes)
				{
					if (nd is XmlElement)
					{
						Add(new DefaultTemplate(_type, (XmlElement)nd.Clone()));
					}
				}

			}

			public DefaultTemplate Add(DefaultTemplate value)
			{
				// Use base class to process actual collection operation
				base.List.Add(value as object);
				return value;
			}

			public void AddRange(DefaultTemplate[] values)
			{
				// Use existing method to add each array entry
				foreach(DefaultTemplate page in values)
					Add(page);
			}

			public void Remove(DefaultTemplate value)
			{
				// Use base class to process actual collection operation
				base.List.Remove(value as object);
			}

			public void Insert(int index, DefaultTemplate value)
			{
				// Use base class to process actual collection operation
				base.List.Insert(index, value as object);
			}

			public bool Contains(DefaultTemplate value)
			{
				// Use base class to process actual collection operation
				return base.List.Contains(value as object);
			}

			public DefaultTemplate this[int index]
			{
				// Use base class to process actual collection operation
				get { return (base.List[index] as DefaultTemplate); }
			}

			public int IndexOf(DefaultTemplate value)
			{
				// Find the 0 based index of the requested entry
				return base.List.IndexOf(value);
			}
		}
		#endregion

		#region Panels OMS Type Collection

		public class PanelsCollection : Crownwood.Magic.Collections.CollectionWithEvents
		{
			private XmlElement _info;
			private OMSType _type;

			private PanelsCollection(){}

			public PanelsCollection(OMSType type, XmlElement info)
			{
				_type = type;
				_info = info;
				foreach (XmlNode nd in _info.ChildNodes)
				{
					if (nd is XmlElement)
					{
						Add(new Panel(_type, (XmlElement)nd.Clone()));
					}
				}
			}

			public Panel Add(Panel value)
			{
				// Use base class to process actual collection operation
				base.List.Add(value as object);

				return value;
			}

			public void AddRange(Panel[] values)
			{
				// Use existing method to add each array entry
				foreach(Panel page in values)
					Add(page);
			}

			public void Remove(Panel value)
			{
				// Use base class to process actual collection operation
				base.List.Remove(value as object);
			}

			public void Insert(int index, Panel value)
			{
				// Use base class to process actual collection operation
				base.List.Insert(index, value as object);
			}

			public bool Contains(Panel value)
			{
				// Use base class to process actual collection operation
				return base.List.Contains(value as object);
			}

			public Panel this[int index]
			{
				// Use base class to process actual collection operation
				get { return (base.List[index] as Panel); }
			}

			public int IndexOf(Panel value)
			{
				// Find the 0 based index of the requested entry
				return base.List.IndexOf(value);
			}
		}

		#endregion

		#region ExtendedData OMS Type Collection

		public class ExtendedDataCollection : Crownwood.Magic.Collections.CollectionWithEvents
		{
			private XmlElement _info;
			private OMSType _type;

			private ExtendedDataCollection(){}

			public ExtendedDataCollection(OMSType type, XmlElement info)
			{
				_type = type;
				_info = info;
				System.Collections.SortedList _ext = new System.Collections.SortedList();
				foreach (XmlNode nd in _info.ChildNodes)
				{
					if (nd is XmlElement)
					{
						if (_ext[nd.OuterXml] == null)
						{
							Add(new ExtendedData(_type, (XmlElement)nd.Clone()));
							_ext[nd.OuterXml] = nd.OuterXml;
						}
					}
				}
			}

			public ExtendedData Add(ExtendedData value)
			{
				// Use base class to process actual collection operation
				base.List.Add(value as object);

				return value;
			}

			public void AddRange(ExtendedData[] values)
			{
				// Use existing method to add each array entry
				foreach(ExtendedData page in values)
					Add(page);
			}

			public void Remove(ExtendedData value)
			{
				// Use base class to process actual collection operation
				base.List.Remove(value as object);
			}

			public void Insert(int index, ExtendedData value)
			{
				// Use base class to process actual collection operation
				base.List.Insert(index, value as object);
			}

			public bool Contains(ExtendedData value)
			{
				// Use base class to process actual collection operation
				return base.List.Contains(value as object);
			}

			public ExtendedData this[int index]
			{
				// Use base class to process actual collection operation
				get { return (base.List[index] as ExtendedData); }
			}

			public int IndexOf(ExtendedData value)
			{
				// Find the 0 based index of the requested entry
				return base.List.IndexOf(value);
			}
		}

		#endregion

	}

	/// <summary>
	/// An OMS type that manipulates the configurable xml of the derived object.
	/// </summary>	
	public abstract class BuiltInOMSType : OMSType
	{
		#region Fields

		/// <summary>
		/// OMS type data source.
		/// </summary>
		protected internal DataSet _omstype = null;

		/// <summary>
		/// SQL statement needed to update the type information.
		/// </summary>
		private string _sql = "select * from %TABLE%";

	
		#endregion

		#region Constructors
		
		/// <summary>
		/// Default constructor not used.
		/// </summary>
		private BuiltInOMSType() : base()
		{
		}


        /// <summary>
        /// Retrieves an existing type by its code.
        /// </summary>
        /// <param name="code">Unique code to filter for.</param>
        protected BuiltInOMSType (string code) : base (code)
		{		
		}



		#endregion

		#region Manipulation Methods

		public override void Fetch(string code)
		{
			fetchcount++;
			_sql = _sql.Replace("%TABLE%", TableName);
			_omstype = GetConfigurableType(code);

			if ((_omstype == null) || (_omstype.Tables[0].Rows.Count == 0 && code != "")) 
			{
				throw GetDoesNotExistException(code);
			}

			if (code == "" && _omstype.Tables[0].Rows.Count == 0)
			{
				DataTable dt = _omstype.Tables[0];
				Global.CreateBlankRecord(ref dt, true);
				_omstype.Tables[0].Columns["typedesc"].ReadOnly = false;
				_omstype.Tables[0].Rows[0][XmlField] = @"<Config/>";
			}
		}

		public override bool Restore()
		{
			try
			{
				SetExtraInfo("typeactive", true);
				Update();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public override bool Delete()
		{
			try
			{
				SetExtraInfo("typeactive", false);
				Update();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public override void Cancel()
		{
			_omstype.RejectChanges();
			IsDirty = false;
			base.Cancel();
		}

		public override void Update()
		{
			base.Update();

			SetExtraInfo(VersionField,  ConvertDef.ToInt64(GetExtraInfo(VersionField),0) + 1);
			SetExtraInfo(XmlField, _config.OuterXml);
			Session.CurrentSession.Connection.Update(_omstype.Tables[0], _sql);
			IsDirty = false;
		}

		public override DataTable GetTypes(bool active)
		{
			string sql = "select *, dbo.GetCodeLookupDesc('%TYPE%', typeCode, @UI) as typeDesc from %TABLE% where typeActive = @ACTIVE";
			sql = sql.Replace("%TABLE%", TableName);
			sql = sql.Replace("%TYPE%", CodeLookupGroup);
			IDataParameter[] pars = new IDataParameter[2];
			pars[0] = Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			pars[1] = Session.CurrentSession.Connection.AddParameter("ACTIVE", SqlDbType.Bit, 1, active);
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(sql, TableName, pars);
			dt.TableName = "OMSTYPES";
			return dt;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the code lookup code for the oms type so that the cultured text
		/// of the OMS type can be stored in code lookups.
		/// </summary>
		public override string Code
		{
			get
			{
				return Convert.ToString(GetExtraInfo("typecode"));
			}
			set
			{
				base.Code = value;
				SetExtraInfo("typecode", base.Code);
			}
		}

		public override string Description
		{
			get
			{
				return Convert.ToString(GetExtraInfo("typedesc"));
			}
			set
			{
				base.Description = value;
				SetExtraInfo("typedesc", base.Description);
			}
		}

		/// <summary>
		/// Gets the current version number of the type.
		/// This is used for caching purposes on the client machine.
		/// </summary>
		public override long Version 
		{
			get
			{
				try
				{
					return Convert.ToInt64(GetExtraInfo(VersionField));
				}
				catch
				{
					return -1;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the types icon index for the internal icon resources
		/// </summary>
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.OMSTypeGlyphDisplayEditor,omsadmin",typeof(System.Drawing.Design.UITypeEditor))]
		[System.ComponentModel.TypeConverter("FWBS.OMS.UI.Windows.Design.EntitiesLister,omsadmin")]
		public override int Glyph 
		{
			get
			{
				try
				{
					return Convert.ToInt32(GetExtraInfo("typeglyph"));
				}
				catch
				{
					return -1;
				}
			}
			set
			{
				SetExtraInfo("typeglyph", value);
			}
		}



		/// <summary>
		/// Gets a flag value indicating whether the current type is being added.
		/// </summary>
		public override bool IsNew
		{
			get
			{
				try
				{
					return (_omstype.Tables[0].Rows[0].RowState == DataRowState.Added);
				}
				catch
				{
					return false;
				}
			}
		}

		public override ObjectState State
		{
			get
			{
				try
				{
					switch (_omstype.Tables[0].Rows[0].RowState)
					{
						case DataRowState.Added:
							return ObjectState.Added;
						case DataRowState.Modified:
							return ObjectState.Modified;
						case DataRowState.Deleted:
							return ObjectState.Deleted;
                        case DataRowState.Unchanged:
                            return ObjectState.Unchanged;
						default:
							return ObjectState.Unitialised;
					}
				}
				catch
				{
					return ObjectState.Unitialised;
				}
			}
		}


		#endregion
		
		#region Abstraction

		/// <summary>
		/// Gets the table name which is used to update from the database.
		/// </summary>
		protected abstract string TableName{get;}

        protected virtual string XmlField
        {
            get
            {
                return "typeXml";
            }
        }

        protected virtual string VersionField
        {
            get
            {
                return "typeVersion";
            }
        }

		/// <summary>
		/// Gets the procedure name to execute to get the type information.
		/// </summary>
		protected abstract string Procedure {get;}
		
		/// <summary>
		/// Gets the caceh folder name.
		/// </summary>
		protected abstract string CacheFolder {get;}
		
		/// <summary>
		/// Gets the Does Not Exist exception from the derived object.
		/// </summary>
		protected Exception GetDoesNotExistException(string code)
		{
			return new OMSTypeException(HelpIndexes.OMSTypeCodeDoesNotExist, code);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Field parses any dynamic property made by a OMSType.
		/// </summary>
		protected internal string ParseDynamicProperty(object obj, string text)
		{
			FieldParser parser = new FieldParser(obj);
			parser.FieldPrefix = "<<<";
			parser.FieldSuffix = ">>>";

			ConditionalParser condp = new ConditionalParser(parser);
			condp.NewLine = @"\par";
			text = condp.ParseString(text);

			text = Session.CurrentSession.Terminology.Parse(parser.ParseString(text), true);

			string checkfor = @"\par" + Environment.NewLine + @"\par" + Environment.NewLine;
			string replace = @"\par" + Environment.NewLine;
			while(text.IndexOf(checkfor) > 0)
				text = text.Replace(checkfor, replace);

			return text;
		}

		/// <summary>
		/// Fetches the xml source that holds the configuration information.
		/// </summary>
		/// <returns></returns>
		protected override string FetchXmlSource()
		{
			return Convert.ToString(GetExtraInfo(XmlField));
		}

		/// <summary>
		/// String representation of the object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Description;
		}

		/// <summary>
		/// Gets the underlying header table.
		/// </summary>
		/// <returns>A data table.</returns>
		internal DataTable GetDataTable()
		{
			return _omstype.Tables[0];
		}


		public override string ReadAttribute(XmlElement element, string name, string defaultValue)
		{
			string ret = base.ReadAttribute(element, name, defaultValue);
			if (ret == defaultValue)
			{
				try
				{
                    if (_omstype.Tables.Contains(element.Name))
                    {
                        if (element.HasAttribute("id"))
                        {
                            _omstype.Tables[element.Name].DefaultView.RowFilter = "id = '" + element.GetAttribute("id").Replace("'", "''") + "'";
                        }
                        else if (element.HasAttribute("lookup"))
                        {
                            _omstype.Tables[element.Name].DefaultView.RowFilter = "lookup = '" + element.GetAttribute("lookup").Replace("'", "''") + "'";
                        }


                        if (_omstype.Tables[element.Name].DefaultView.Count > 0)
                        {
                            ret = Convert.ToString(_omstype.Tables[element.Name].DefaultView[0][name]);
                        }
                    }
                    else
                        ret = String.Empty;
				}
				catch
				{
					ret = String.Empty;
				}
				
			}

			if (ret == String.Empty)
				ret = defaultValue;

			return ret;

		}

		/// <summary>
		/// Sets the raw internal data object with the specified value under the specified field name.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <param name="val">Value to use.</param>
		public void SetExtraInfo (string fieldName, object val)
		{
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			_omstype.Tables[0].Rows[0][fieldName] = val;
			OnDirty();
		}

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public object GetExtraInfo(string fieldName)
		{
            object val = _omstype.Tables[0].Rows[0][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}

		/// <summary>
		/// Checks whether a specified code exists..
		/// </summary>
		/// <returns>True if the code exists.</returns>
		public override bool Exists(string Code)
		{
			int count = Convert.ToInt32(Session.CurrentSession.Connection.ExecuteSQLScalar("select count(*) from " + TableName + " where typeCode = @Code", new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code)}));
			if (count > 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Gets a data set with a few tables explaining how to layout the configurable type.
		/// </summary>
		/// <param name="code">Configurable type code.</param>
		/// <returns>A data set object.</returns>
		private DataSet GetConfigurableType(string code)
		{
			Session.CurrentSession.CheckLoggedIn();

			DataSet ds = null;		//Internal schema used.
			DataSet fds = null;		//Cached schema version.
			long version = 0;		//Version specifier.
			bool local = false;

			//Loads the cached version of the configurable type schema and gets the version
			//from the header information.  If there is an error opening the file
			//then set the version is set to zero.  The enquiry form will then be 
			//completely refreshed from the database.
			try
			{
				fds = Global.GetCache(CacheFolder + @"\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name,  code.ToString() + "." + Global.CacheExt);
                if (fds != null)
                    version = (long)fds.Tables["HEADER"].Rows[0]["typeversion"];
                else
                    version = 0;
			}
			catch
			{
				fds = null;
				version = 0;
			}
		
	
			//Run the stored procedure and pass it the found version 
			//number.  If there is a newer version then cache the newly generated schema.
			IDataParameter [] parlist = new IDataParameter[4];
			parlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.NVarChar, 15, code);
			parlist[1] = Session.CurrentSession.Connection.AddParameter("Version", System.Data.SqlDbType.BigInt, 0, (Session.CurrentSession._designMode ? 0 : version));
			parlist[2] = Session.CurrentSession.Connection.AddParameter("Force", System.Data.SqlDbType.Bit, 0, 0);
			parlist[3] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
			
			//Allow an execution error to escalate through the stack.
			ds = Session.CurrentSession.Connection.ExecuteProcedureDataSet(Procedure, (code == String.Empty), new string[1] {"HEADER"}, parlist);
								
	
			//Make sure that there is a valid configurable type returned.  
			//If not then use the already cached version of the client type.
			if ((ds == null) || (ds.Tables.Count == 0))
			{
				if ((fds == null) || (fds.Tables["HEADER"] == null))
				{
					if (code != String.Empty)
					{
						//The returned data set schema is invalid and there is not cached version
						//to rely on.
						throw GetDoesNotExistException(code);
					}

				}
				else
				{
					//The locally cached version is being used.
					Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Using local version of type '" + code + "'", "BAL.BuiltInOMSTypeGetConfigurableType()");
					ds = fds;
					local = true;
				}
			}
			else
			{
				Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Using database version of type '" + code + "'", "BAL.BuiltInOMSTypeGetConfigurableType()");
		
				//Name the data set for neatnes reasons.
				ds.DataSetName = "OMSTYPE";

				//Name the required schema tables.
				ds.Tables[1].TableName = "FORM";
				ds.Tables[3].TableName = "PANEL";
				ds.Tables[4].TableName = "TAB";
				
			}

			//Loop through the form and terminology parse the text.
			foreach (DataRow ctrl in ds.Tables["FORM"].Rows)
			{
				ctrl["frmdesc"] = Session.CurrentSession.Terminology.Parse(Convert.ToString(ctrl["frmdesc"]), true);
			}

			//Loop through each panel and terminology parse the text.
			foreach (DataRow ctrl in ds.Tables["PANEL"].Rows)
			{
				ctrl["pnldesc"] = Session.CurrentSession.Terminology.Parse(ctrl["pnldesc"].ToString(), true);
				ctrl["pnlhelp"] = Session.CurrentSession.Terminology.Parse(ctrl["pnlhelp"].ToString(), true);
			}

			//Loop through each tab and terminology parse the text.
			foreach (DataRow ctrl in ds.Tables["TAB"].Rows)
			{
				ctrl["tabdesc"] = Session.CurrentSession.Terminology.Parse(ctrl["tabdesc"].ToString(), true);
				ctrl["tabhelp"] = Session.CurrentSession.Terminology.Parse(ctrl["tabhelp"].ToString(), true);
			}


			//Cache the client type item to the users application data folder.
			if (!local) 
			{
				Global.Cache(ds, CacheFolder + @"\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name, code + "." + Global.CacheExt);
			}

			ds.AcceptChanges();
			return ds;
		}

        public override System.Drawing.Icon GetAlternateGlyph()
        {
            return FWBS.OMS.Resource.Users;
        }

		#endregion
	
		#region IDisposable Implementation

		protected override void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if (_omstype != null)
				{
					_omstype.Dispose();
					_omstype = null;
				}

				_config = null;
	
			}

		}

		#endregion
	}

	#region Storage Providers
	[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.StorageProviderEditor,omsadmin",typeof(System.Drawing.Design.UITypeEditor))]
	public class StorageProvider
	{
		
		private DataTable _data = null;
		private short _ID = -1;
		private string _desc = "";
		
		public StorageProvider(short ID)
		{
			if (ID != -1)
			{
				IDataParameter[] paramlist = new IDataParameter[2];
				paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.Int, 0, ID);
				paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
				_data = FWBS.OMS.Session.CurrentSession.Connection.ExecuteSQLTable("select spid, dbo.GetCodeLookupDesc('SLP', spcode, @UI) as spdesc from dbstorageprovider WHERE spID = @Code","STORAGE",false,  paramlist);
				_ID = ID;
				_desc = Convert.ToString(_data.Rows[0]["spdesc"]);
			}
			else
			{
				_ID = -1;
				_desc = Session.CurrentSession.Resources.GetResource("DEFPROV","{Default Provider}","").Text;
			}
		}


		public override string ToString()
		{
			return this.Description;
		}

		public short ID
		{
			get
			{
				return _ID;
			}
		}

		public string Description
		{
			get
			{
				return _desc;
			}
		}

	}
	#endregion


	/// <summary>
	/// An OMS client type that manipulates the configurable xml of the object.
	/// </summary>
	public class ClientType : BuiltInOMSType
	{
		#region Fields

		private StorageProvider _defstgepro = null;

        private FWBS.OMS.Security.TemplateSecurity _security;


		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new client type with properties needing to be set.
		/// </summary>
		public ClientType() : base ("")
		{
            _security = new FWBS.OMS.Security.TemplateSecurity("ClientType", "");
        }

		/// <summary>
		/// Gets an existing client type.
		/// </summary>
		/// <param name="code">A client type code.</param>
		internal ClientType (string code) : base(code)
		{
			//An edit contructor should add the object created to the session memory collection.
			if (IsNew == false) 
			{
				foreach (DataRow row in _omstype.Tables["FORM"].Rows)
					row["frmdesc"] = Convert.ToString(_omstype.Tables["FORM"].Rows[0]["frmdesc"]).Replace("%CLTYPE%", this.Description);
				Session.CurrentSession.CurrentClientTypes.Add(Code, this);
				
				//Call the extensibility event for addins.
				OnObjectEvent(Extensibility.ObjectEvent.Loaded);
			}

            _security = new FWBS.OMS.Security.TemplateSecurity("ClientType", code);
		}
		#endregion

		#region Properties
        public override string Code
        {
            get
            {
                return base.Code;
            }
            set
            {
                base.Code = value;
                if (_security == null)
                    _security = new FWBS.OMS.Security.TemplateSecurity("ClientType", value);
                else
                    _security.Code = value;
            }
        }

        [Lookup("Security")]
        [LocCategory("DATA")]
        public virtual FWBS.OMS.Security.TemplateSecurity Security
        {
            get
            {
                return _security;
            }
            set
            {
                _security = value;
            }
        }

		/// <summary>
		/// The Contact Type for Quick Client Take on.
		/// </summary>
		[LocCategory("CONTACTS")]
		public string ContactType
		{
			get
			{
				return Convert.ToString(ReadAttribute(_settings,"quickContactType","3RDPARTY"));
			}
			set
			{
				WriteAttribute(_settings,"quickContactType", value);
			}
		}
		
		/// <summary>
		/// The maximum number of contacts allowed to be accociated to the contact.
		/// For example a couple will have a miximum of two.
		/// </summary>
		[LocCategory("CONTACTS")]
		[Lookup("MAXCONT")]
		public byte MaximumContactCount
		{
			get
			{
				return Convert.ToByte(ReadAttribute(_settings,"maxContactCount","0"));
			}
			set
			{
				WriteAttribute(_settings,"maxContactCount", value);
			}
		}

		[LocCategory("DEFAULTS")]
		[Lookup("Templates")]
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.DefaultsTemplatesEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
		public OMSType.DefaultTemplateCollection DefaultTemplates
		{
			get
			{
				if (_defaults == null)
				{
					_defaults = new DefaultTemplateCollection(this, _defaultTemplatesHeader);
					_defaults.Cleared +=new Crownwood.Magic.Collections.CollectionClear(OnDirty);
				}
				return _defaults;				
			}
		}

		/// <summary>
		/// The miniimum number of contacts allowed to be accociated to the contact.
		/// For example a couple will have a minimum of two.
		/// </summary>
		[LocCategory("CONTACTS")]
		[Lookup("MINCONT")]
		public byte MinimumContactCount
		{
			get
			{
				return Convert.ToByte(ReadAttribute(_settings,"minContactCount","1"));
			}
			set
			{
				WriteAttribute(_settings,"minContactCount", value);
			}
		}

		/// <summary>
		/// Gets or Sets hte client seed number for client number generation.
		/// </summary>
		[LocCategory("SETTINGS")]
		public string SeedNumber
		{
			get
			{
				return Convert.ToString(GetExtraInfo("typeseed"));
			}
			set
			{
				SetExtraInfo("typeseed", value);
			}
		}


		[LocCategory("Document")]
		[Lookup("DefStorage")]
		public StorageProvider DefaultStorageProviderLoc
		{
			get
			{
				if (_defstgepro == null)
				{
					_defstgepro = new StorageProvider(this.DefaultStorageProvider);
				}
				return _defstgepro;
			}
			set
			{
				_defstgepro = value;
				this.DefaultStorageProvider = _defstgepro.ID;
			}
		}

		/// <summary>
		/// Gets or Sets the default document storage location for the client type.
		/// </summary>
		[Browsable(false)]
		public short DefaultStorageProvider
		{
			get
			{
				return Common.ConvertDef.ToInt16(ReadAttribute(_settings,"typeStorageProvider","-1"), -1);
			}
			set
			{
				if (value < 0)
					WriteAttribute(_settings,"typeStorageProvider", "");
				else
					WriteAttribute(_settings,"typeStorageProvider", value);
			}
		}


        [Browsable(false)]
        public string RetentionFormat
        {
            get
            {
                return ReadAttribute(_settings, "typeRetentionFormat", "");
            }
            set
            {
                if (value == null) value = "";
                WriteAttribute(_settings, "typeRetentionFormat", value);
            }
        }
        private CodeLookupDisplay retFmt;
        [Browsable(true)]
        [DefaultValue("")]
        [LocCategory("Document")]
        [Lookup("RETFMTNAME")]
        [CodeLookupSelectorTitle("RETFMT", "Retention Format")]
        public CodeLookupDisplay RetentionFormatProperty
        {
            get
            {
                if (retFmt == null)
                {
                    retFmt = new CodeLookupDisplay("RETFMT");
                    retFmt.Code = RetentionFormat;
                }
                return retFmt;
            }
            set
            {
                retFmt = value;
                this.RetentionFormat = value.Code;
            }
        }




        [Browsable(false)]
        public string TemplateFormat
        {
            get
            {
                return ReadAttribute(_settings, "typeTemplateFormat", "");
            }
            set
            {
                if (value == null) value = "";
                WriteAttribute(_settings, "typeTemplateFormat", value);
            }
        }
        private CodeLookupDisplay tmpFmt;
        /// <summary>
        /// Property for Template to be used against the File Type
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [LocCategory("Document")]
        [Lookup("TPLTFMATNAME")]
        [CodeLookupSelectorTitle("TMPFMT", "Template Format")]
        public CodeLookupDisplay TemplateFormatProperty
        {
            get
            {
                if (tmpFmt == null)
                {
                    tmpFmt = new CodeLookupDisplay("TMPFMT");
                    tmpFmt.Code = TemplateFormat;
                }
                return tmpFmt;
            }
            set
            {
                tmpFmt = value;
                this.TemplateFormat = value.Code;
            }
        }



		/// <summary>
		/// Gets or Sets whether the client adds a default combined associate when there is more than one contact for the client when creating a new file.
		/// </summary>
		[LocCategory("CONTACTS")]
		[Lookup("ASCOMBINDED")]
		public bool AddCombinedAssociate
		{
			get
			{
				return Common.ConvertDef.ToBoolean(ReadAttribute(_settings,"combinedAssociate", "false"), false);
			}
			set
			{
				WriteAttribute(_settings,"combinedAssociate", value);
			}
		}

		/// <summary>
		/// Gets or Sets whether the client adds a default combined associate when there is more than one contact for the client when creating a new file.
		/// </summary>
		[LocCategory("CONTACTS")]
		[Lookup("ASCONTACTADD")]
		public bool AddAssociateAsContactAddress
		{
			get
			{
				return Common.ConvertDef.ToBoolean(ReadAttribute(_settings,"associateContactAddress", "false"), false);
			}
			set
			{
				WriteAttribute(_settings,"associateContactAddress", value);
			}
		}

		/// <summary>
		/// Gets or Sets whether the client adds a default combined associate when there is more than one contact for the client when creating a new file.
		/// </summary>
		[LocCategory("(DETAILS)")]
		[Lookup("CLIENTDETAILS")]
		[Editor("FWBS.OMS.UI.Windows.Design.CodeDescriptionEditor,OMS.UI",typeof(System.Drawing.Design.UITypeEditor))]
		public string ClientDetailsConfig
		{
			get
			{
				System.Xml.XmlElement el = GetConfigRoot().SelectSingleNode("ClientDetail") as System.Xml.XmlElement;
				if (el == null)
					return "";
				else
					return el.InnerText;
			}
			set
			{
				System.Xml.XmlElement el = GetConfigRoot().SelectSingleNode("ClientDetail") as System.Xml.XmlElement;
				if (el == null)
				{
					el = _config.CreateElement("ClientDetail");
					GetConfigRoot().AppendChild(el);
				}

				if (value == null)
					el.InnerText = "";
				else
					el.InnerText = value;

				OnDirty();
			}
		}
        /// <summary>
        /// Gets or Sets whether the client adds a default combined associate when there is more than one contact for the client when creating a new file.
        /// </summary>
        [LocCategory("(DETAILS)")]
        [Lookup("CLIENTDETAILSIP")]
        [Editor("FWBS.OMS.UI.Windows.Design.CodeDescriptionEditor,OMS.UI", typeof(System.Drawing.Design.UITypeEditor))]
        public string ClientDetailsInfoPanelConfig
        {
            get
            {
                System.Xml.XmlElement el = GetConfigRoot().SelectSingleNode("ClientDetailInfoPanel") as System.Xml.XmlElement;
                if (el == null)
                    return "";
                else
                    return el.InnerText;
            }
            set
            {
                System.Xml.XmlElement el = GetConfigRoot().SelectSingleNode("ClientDetailInfoPanel") as System.Xml.XmlElement;
                if (el == null)
                {
                    el = _config.CreateElement("ClientDetailInfoPanel");
                    GetConfigRoot().AppendChild(el);
                }

                if (value == null)
                    el.InnerText = "";
                else
                    el.InnerText = value;

                OnDirty();
            }
        }
		#endregion

		#region Abstraction
        /// <summary>
        /// Update Security if Advanced Security is Enabled
        /// </summary>
        public override void Update()
        {
            if (_security != null)
                _security.Update();

            base.Update();
        }

		/// <summary>
		/// Clones the OMSType.
		/// </summary>
		/// <returns>New OMSType.</returns>
		public override OMSType Clone()
		{
			ClientType ct = new ClientType();
			ct._omstype = _omstype.Copy();
			ct._omstype.Tables[0].Clear();
			ct._omstype.AcceptChanges();
			ct._omstype.Tables[0].Rows.Add(_omstype.Tables[0].Rows[0].ItemArray);
			ct.SetExtraInfo("typecode", DBNull.Value);
			ct.SetExtraInfo("typedesc", DBNull.Value);
			ct.BuildXML();
			return ct;
		}

		/// <summary>
		/// Gets the code lookup group for the oms type so that the cultured names
		/// of the OMS types can be stored under their own categories.
		/// </summary>
		public override string CodeLookupGroup
		{
			get
			{
				return "CLTYPE";
			}
		}

		public override string SearchListName
		{
			get
			{
				return "ADMCLIENTTYPE";
			}
		}
 

		/// <summary>
		/// Gets the table name which is used to update from the database.
		/// </summary>
		protected override string TableName
		{
			get
			{
				return "dbClientType";;
			}
		}

		
		/// <summary>
		/// Gets the procedure name to execute to get the type information.
		/// </summary>
		protected override string Procedure
		{
			get
			{
				return "sprClientType";
			}
		}

		/// <summary>
		/// Gets the caceh folder name.
		/// </summary>
		protected override string CacheFolder
		{
			get
			{
				return "ClientTypes";
			}
		}

		/// <summary>
		/// Gets an object of the derived type.
		/// </summary>
		/// <param name="id">A unique identifer for the object.</param>
		/// <returns>An IOMSType compaitble object.</returns>
		public override IOMSType GetObject(object id)
		{
			return Client.GetClient((long)id);
		}

        /// <summary>
        /// Creates an instance of the IOMSType.
        /// </summary>
        /// <param name="parameters">Specifies the parameters needed to construct the object.</param>
        /// <returns>An IOMSType object.</returns>
        public override IOMSType CreateObject(object [] parameters)
		{
			if (parameters == null || parameters.Length == 0 )
				return new Client(this, false);
			else
			{
				bool preclient = false;
				Contact defcont = parameters[0] as Contact;
				if (parameters.Length > 1) preclient = unchecked((bool)parameters[1]);
				if (defcont != null)
					return new Client(this, (Contact)parameters[0], preclient);
				else
					return new Client(this, preclient);
			}
		}


		/// <summary>
		/// Gets the object type name.
		/// </summary>
		public override Type OMSObjectType
		{
			get
			{
				return typeof(Client);
			}
		}


		#endregion

		#region Static Methods

		/// <summary>
		/// Gets a specific client type.
		/// <param name="type">Client type code.</param>
		/// </summary>
		/// <returns>A client type object.</returns>
		public static ClientType GetClientType(string type)
		{
			Session.CurrentSession.CheckLoggedIn();

			ClientType ct = Session.CurrentSession.CurrentClientTypes[type] as ClientType;

			if (ct == null)
			{
				ct = new ClientType(type);
			}		
			return ct;
		}

		#endregion

	}

	/// <summary>
	/// An OMS contact type that manipulates the configurable xml of the object.
	/// </summary>
	public class ContactType : BuiltInOMSType
	{
		#region Fields
		private FWBS.OMS.ContactType.AssociatedFormats _associatedformats;
        private FWBS.OMS.Security.TemplateSecurity _security;
		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new contact type with properties needing to be set.
		/// </summary>
		public ContactType() : base ("")
		{
            _security = new FWBS.OMS.Security.TemplateSecurity("ContactType", "");
        }

		/// <summary>
		/// Gets an existing contact type.
		/// </summary>
		/// <param name="code">A contact type code.</param>
		internal ContactType (string code) : base(code)
		{
			//An edit contructor should add the object created to the session memory collection.
            _security = new FWBS.OMS.Security.TemplateSecurity("ContactType", code);
            if (IsNew == false) 
			{
				foreach (DataRow row in _omstype.Tables["FORM"].Rows)
					row["frmdesc"] = Convert.ToString(_omstype.Tables["FORM"].Rows[0]["frmdesc"]).Replace("%CONTTYPE%", this.Description);
				Session.CurrentSession.CurrentContactTypes.Add(Code, this);
				_associatedformats = new AssociatedFormats(code);
			
				//Call the extensibility event for addins.
				OnObjectEvent(Extensibility.ObjectEvent.Loaded);
			}
		}

		#endregion

		#region Abstraction
        /// <summary>
        /// Update Security if Advanced Security is Enabled
        /// </summary>
        public override void Update()
        {
            if (_security != null)
                _security.Update();

            base.Update();
        }

		/// <summary>
		/// Clones the OMSType.
		/// </summary>
		/// <returns>New OMSType.</returns>
		public override OMSType Clone()
		{
			ContactType ct = new ContactType();
			ct._omstype = _omstype.Copy();
			ct._omstype.Tables[0].Clear();
			ct._omstype.AcceptChanges();
			ct._omstype.Tables[0].Rows.Add(_omstype.Tables[0].Rows[0].ItemArray);
			ct.SetExtraInfo("typecode", DBNull.Value);
			ct.SetExtraInfo("typedesc", DBNull.Value);
			ct.BuildXML();
			return ct;
		}

		/// <summary>
		/// Gets the object type name.
		/// </summary>
		public override Type OMSObjectType
		{
			get
			{
				return typeof(Contact);
			}
		}
		
		public override string SearchListName
		{
			get
			{
				return "ADMCONTACTTYPE";
			}
		}

		/// <summary>
		/// Gets the code lookup group for the oms type so that the cultured names
		/// of the OMS types can be stored under their own categories.
		/// </summary>
		public override string CodeLookupGroup
		{
			get
			{
				return "CONTTYPE";
			}
		}

		/// <summary>
		/// Gets the table name which is used to update from the database.
		/// </summary>
		protected override string TableName
		{
			get
			{
				return "dbContactType";;
			}
		}

		/// <summary>
		/// Gets the procedure name to execute to get the type information.
		/// </summary>
		protected override string Procedure
		{
			get
			{
				return "sprContactType";
			}
		}

		/// <summary>
		/// Gets the caceh folder name.
		/// </summary>
		protected override string CacheFolder
		{
			get
			{
				return "ContactTypes";
			}
		}

		/// <summary>
		/// Gets an object of the derived type.
		/// </summary>
		/// <param name="id">A unique identifer for the object.</param>
		/// <returns>An IOMSType compaitble object.</returns>
		public override IOMSType GetObject(object id)
		{
			return Contact.GetContact((long)id);
		}

        /// <summary>
        /// Creates an instance of the IOMSType.
        /// </summary>
        /// <param name="parameters">Specifies the parameters needed to construct the object.</param>
        /// <returns>An IOMSType object.</returns>
        public override IOMSType CreateObject(object [] parameters)
		{
			return new Contact(this);
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets a specific contact type.
		/// <param name="type">Contact type code.</param>
		/// </summary>
		/// <returns>A contact type object.</returns>
		public static ContactType GetContactType(string type)
		{
			Session.CurrentSession.CheckLoggedIn();

			ContactType ct = Session.CurrentSession.CurrentContactTypes[type] as ContactType;

			if (ct == null)
			{
				ct = new ContactType(type);
			}		
			return ct;
		}

		#endregion

		#region Properties
        [Lookup("Security")]
        [LocCategory("DATA")]
        [System.ComponentModel.Editor("FWBS.OMS.Addin.Security.TemplateEditor,FWBS.OMS.Addin.Security", typeof(System.Drawing.Design.UITypeEditor))]
        public virtual FWBS.OMS.Security.TemplateSecurity Security
        {
            get
            {
                return _security;
            }
            set
            {
                _security = value;
            }
        }

		[System.ComponentModel.Browsable(false)]
		public override bool SearchOnCreate
		{
			get
			{
				return Convert.ToBoolean(ReadAttribute(_settings,"searchOnCreate","true"));
			}
			set
			{
				WriteAttribute(_settings,"searchOnCreate", value);
			}
		}

		public override string Code
		{
			get
			{
				return base.Code;
			}
			set
			{
				base.Code = value;
				_associatedformats = new AssociatedFormats(this.Code);
                if (_security == null)
                    _security = new FWBS.OMS.Security.TemplateSecurity("ContactType", value);
                else
                    _security.Code = value;
			}
		}


		/// <summary>
		/// General Contact Type
		/// </summary>
		[LocCategory("Data")]
		public OMSTypeContactGeneralType GeneralType
		{
			get
			{
				return (OMSTypeContactGeneralType)ConvertDef.ToEnum(ReadAttribute(_settings,"GeneralType","Individual"),OMSTypeContactGeneralType.Individual);
			}
			set
			{
				WriteAttribute(_settings,"GeneralType", value.ToString());
			}
		}


		[LocCategory("Associates")]
		public FWBS.OMS.ContactType.AssociatedFormats Formats
		{
			get
			{
				return _associatedformats;
			}
			set
			{
				_associatedformats = value;
				this.OnDirty();
			}
		}
		#endregion

		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.AssociatedFormatsEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
		public class AssociatedFormats
		{
			private string _code;
			private System.Data.DataTable _dt = null;


			public AssociatedFormats(string ContactTypeCode)
			{
				_code = ContactTypeCode;
				_dt = Associate.GetAssociateTypes(ContactTypeCode, false);
				if (_dt.PrimaryKey.Length == 0)
					_dt.PrimaryKey = new DataColumn[2]{_dt.Columns["conttype"], _dt.Columns["assoctype"]};

			}

			public override string ToString()
			{
				return _dt.Rows.Count.ToString() + FWBS.OMS.Session.CurrentSession.Resources.GetResource("ASSFORMCOU"," Associated Formats","").Text;
			}

			public DataTable GetAssociatedTypes
			{
				get
				{
					if (_dt.PrimaryKey.Length == 0)
						_dt.PrimaryKey = new DataColumn[2]{_dt.Columns["conttype"], _dt.Columns["assoctype"]};

					return _dt;
				}
			}

			public string ContactTypeCode
			{
				get
				{
					return _code;
				}
			}

			public DataTable GetAllAssociatedTypes
			{
				get
				{
                    return Associate.GetAllAssociateTypes();
				}
			}

			public void Update()
			{
				try
				{
					Session.CurrentSession.Connection.Update(_dt,"dbAssociatedTypes", false); 
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					_dt = Associate.GetAssociateTypes(_code, false);
				}
			}
		}
	}

	
	//TODO: fileContractGroup
	//TODO: fileDefRiskCode
	//TODO: fileActionToDo
	//TODO: fileLetterhead
	//TODO: fileClientCareLetter
	//TODO: fileTemplateDir
	//TODO: fileConfLetter
	//TODO: fileExpLetter
	//TODO: fileFileSynopsis
	//TODO: fileDefBankCode
	//TODO: fileOffBankCode

	/// <summary>
	/// An OMS file type that manipulates the configurable xml of the object.
	/// </summary>
	public class FileType : BuiltInOMSType
	{
		#region Fields

		/// <summary>
		/// A reference to the multi associates collection.
		/// </summary>
		private FileType.MultiAssociateCollection _multiassocs = null;
		/// <summary>
		/// A reference to the Jobs Collection
		/// </summary>
		private FileType.JobsCollection _jobs = null;

		private CodeLookupDisplayReadOnly _library = null;
		private CodeLookupDisplayReadOnly _filepreccategory = null;
		private CodeLookupDisplayReadOnly _fileprecsubcategory = null;
        private CodeLookupDisplayReadOnly _fileprecminorcategory = null;
        private CodeLookupDisplayReadOnly _filedeptcode = null;
        private string _filefundtype = null;
		private StorageProvider _defstgepro = null;

		/// <summary>
		/// Xml node for job header information.
		/// </summary>
		protected XmlElement _jobHeader;

        private FWBS.OMS.Security.TemplateSecurity _security;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new File type with properties needing to be set.
		/// </summary>
		public FileType() : base ("")
		{
			XmlNode root = _config.SelectSingleNode("/Config");
			XmlNode _xmlConfigDialogRoot = root.SelectSingleNode("Dialog");
			_jobHeader = _xmlConfigDialogRoot.SelectSingleNode("Jobs") as XmlElement;
			if (_jobHeader == null)
			{
				_jobHeader = _config.CreateElement("Jobs");
				_xmlConfigDialogRoot.AppendChild(_jobHeader);
			}
            _security = new FWBS.OMS.Security.TemplateSecurity("FileType","");
        }

		/// <summary>
		/// Gets an existing File type.
		/// </summary>
		/// <param name="code">File type code.</param>
		internal FileType (string code) : base(code)
		{
			//An edit contructor should add the object created to the session memory collection.
			if (IsNew == false) 
			{
				foreach (DataRow row in _omstype.Tables["FORM"].Rows)
					row["frmdesc"] = Convert.ToString(_omstype.Tables["FORM"].Rows[0]["frmdesc"]).Replace("%FILETYPE%", this.Description);
				Session.CurrentSession.CurrentFileTypes.Add(Code, this);
				
				//Call the extensibility event for addins.
				OnObjectEvent(Extensibility.ObjectEvent.Loaded);
			}
            _security = new FWBS.OMS.Security.TemplateSecurity("FileType", code);
        }
		#endregion

		#region Abstraction
	
		/// <summary>
		/// Clones the OMSType.
		/// </summary>
		/// <returns>New OMSType.</returns>
		public override OMSType Clone()
		{
			FileType ft = new FileType();
			ft._omstype = _omstype.Copy();
			ft._omstype.Tables[0].Clear();
			ft._omstype.AcceptChanges();
			ft._omstype.Tables[0].Rows.Add(_omstype.Tables[0].Rows[0].ItemArray);
			ft.SetExtraInfo("typecode", DBNull.Value);
			ft.SetExtraInfo("typedesc", DBNull.Value);
			ft.BuildXML();
			return ft;
		}

		/// <summary>
		/// Gets the object type name.
		/// </summary>
		public override Type OMSObjectType
		{
			get
			{
				return typeof(OMSFile);
			}
		}

		/// <summary>
		/// Gets the code lookup group for the oms type so that the cultured names
		/// of the OMS types can be stored under their own categories.
		/// </summary>
		public override string CodeLookupGroup
		{
			get
			{
				return "FILETYPE";
			}
		}


		public override string SearchListName
		{
			get
			{
				return "ADMFILETYPE";
			}
		}

		/// <summary>
		/// Gets the table name which is used to update from the database.
		/// </summary>
		protected override string TableName
		{
			get
			{
				return "dbFileType";
			}
		}

		
		/// <summary>
		/// Gets the procedure name to execute to get the type information.
		/// </summary>
		protected override string Procedure
		{
			get
			{
				return "sprFileType";
			}
		}

		/// <summary>
		/// Gets the caceh folder name.
		/// </summary>
		protected override string CacheFolder
		{
			get
			{
				return "FileTypes";
			}
		}

		/// <summary>
		/// Gets an object of the derived type.
		/// </summary>
		/// <param name="id">A unique identifer for the object.</param>
		/// <returns>An IOMSType compaitble object.</returns>
		public override IOMSType GetObject(object id)
		{
			return OMSFile.GetFile((long)id);
		}

        /// <summary>
        /// Creates an instance of the IOMSType.
        /// </summary>
        /// <param name="parameters">Specifies the parameters needed to construct the object.</param>
        /// <returns>An IOMSType object.</returns>
        public override IOMSType CreateObject(object [] parameters)
		{
			if (parameters == null || parameters.Length == 0 )
				return new OMSFile(this);
			else
			{
				if (parameters[0] is Client)
					return new OMSFile(this, (Client)parameters[0]);
				else
					return new OMSFile(this);
			}
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets a specific File type.
		/// <param name="type">File type code.</param>
		/// </summary>
		/// <returns>A File type object.</returns>
		public static FileType GetFileType(string type)
		{
			Session.CurrentSession.CheckLoggedIn();
			
			FileType ft = Session.CurrentSession.CurrentFileTypes[type] as FileType;

			if (ft == null)
			{
				ft = new FileType(type);
			}		
			return ft;
		}


		#endregion

		#region Properties
        public override string Code
        {
            get
            {
                return base.Code;
            }
            set
            {
                base.Code = value;
                _security.Code = value;
            }
        }

        [Lookup("Security")]
        [LocCategory("DATA")]
        [System.ComponentModel.Editor("FWBS.OMS.Addin.Security.TemplateEditor,FWBS.OMS.Addin.Security", typeof(System.Drawing.Design.UITypeEditor))]
        public virtual FWBS.OMS.Security.TemplateSecurity Security
        {
            get
            {
                return _security;
            }
            set
            {
                _security = value;
            }
        }
        


        [Lookup("TVTemplate")]
        [DefaultValue("")]
        [System.ComponentModel.Editor("FWBS.OMS.UI.DocumentManagement.Addins.FolderTreeTemplateEditor,OMS.UI", typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(DocFolderTemplateConverter))]
        //Note: Rename to Template Code (?)
        public string TemplateCode
        {
            get
            {
                string code = Convert.ToString(ReadAttribute(_settings, "typeTVTemplate", ""));
                return code;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    WriteAttribute(_settings, "typeTVTemplate", "");
                else
                    WriteAttribute(_settings, "typeTVTemplate", value);
            }
        }

        private bool _migrateWalletsToFoldersOnSave;

        [Browsable(false)]
        public bool MigrateWalletsToFoldersOnSave
        {
            get
            {
                return _migrateWalletsToFoldersOnSave;
            }
            set
            {
                _migrateWalletsToFoldersOnSave = value;
                if (_migrateWalletsToFoldersOnSave)
                {
                    OnDirty();
                }
            }
        }

        internal class DocFolderTemplateConverter : StringConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext ctx, Type sourceType)
            {
                return true;
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                return FWBS.OMS.CodeLookup.GetLookup("DFLDR_TEMPLATE", Convert.ToString(value));
            }

        }


		/// <summary>
		/// Gets or Sets whether the client adds a default combined associate when there is more than one contact for the client when creating a new file.
		/// </summary>
		[LocCategory("(DETAILS)")]
		[Lookup("FILEDETAILS")]
		[Editor("FWBS.OMS.UI.Windows.Design.CodeDescriptionEditor,OMS.UI",typeof(System.Drawing.Design.UITypeEditor))]
		public string FileDetailsConfig
		{
			get
			{
				System.Xml.XmlElement el = GetConfigRoot().SelectSingleNode("FileDetail") as System.Xml.XmlElement;
				if (el == null)
					return "";
				else
					return el.InnerText;
			}
			set
			{
				System.Xml.XmlElement el = GetConfigRoot().SelectSingleNode("FileDetail") as System.Xml.XmlElement;
				if (el == null)
				{
					el = _config.CreateElement("FileDetail");
					GetConfigRoot().AppendChild(el);
				}

				if (value == null)
					el.InnerText = "";
				else
					el.InnerText = value;

				OnDirty();
			}
		}

        /// <summary>
        /// Gets or sets the file type wide Document Versioning option.
        /// </summary>
        [DefaultValue("")]
        [LocCategory("Document")]
        [Lookup("DocVersioning")]
        [Editor("FWBS.OMS.Design.DataListEditor,OMS.UI", typeof(System.Drawing.Design.UITypeEditor))]
        [Design.DataList("DSDOCVERSIONING", NullValue="", UseNull = true, DisplayMember = "cddesc", ValueMember = "cdcode")]
        [TypeConverter(typeof(Design.DataListConverter))]
        public string DocumentVersioning
        {
            get
            {
            
                //O = Overwrite (Standard)
                //N = Creates a brand new document (Save As);
                //V = Automatically creates new version.
                //M = Creates a new root/major version.

                string val = ReadAttribute(_settings, "typeDocVersioning", String.Empty);
                switch (val)
                {
                    case "O":
                        return val;
                    case "N":
                        return val;
                    case "V":
                        return val;
                    case "M":
                        return val;
                    default:
                        return String.Empty;
                }
            }
            set
            {
                if (String.IsNullOrEmpty(value)) value = String.Empty;
                if (this.DocumentVersioning != value)
                {
                    switch (value)
                    {
                        case "O":
                            break;
                        case "N":
                            break;
                        case "V":
                            break;
                        case "M":
                            break;
                        default:
                            value = String.Empty;
                            break;
                    }
                    WriteAttribute(_settings, "typeDocVersioning", value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the file type wide Document locking option.
        /// </summary>
        [DefaultValue("")]
        [LocCategory("Document")]
        [Editor("FWBS.OMS.Design.DataListEditor,OMS.UI", typeof(System.Drawing.Design.UITypeEditor))]
        [Design.DataList("DSDOCLOCKING", NullValue="", UseNull = true, DisplayMember="cddesc", ValueMember="cdcode")]
        [TypeConverter(typeof(Design.DataListConverter))]
        public string DocumentLocking
        {
            get
            {

                //E = Exclusive
                //S = Shared;
                
                string val = ReadAttribute(_settings, "typeDocLocking", String.Empty);
                switch (val)
                {
                    case "E":
                        return val;
                    case "S":
                        return val;
                    default:
                        return String.Empty;
                }
            }
            set
            {
                if (String.IsNullOrEmpty(value)) value = String.Empty;
                if (value != this.DocumentLocking)
                {
                    switch (value)
                    {
                        case "E":
                            break;
                        case "S":
                            break;
                        default:
                            value = String.Empty;
                            break;
                    }

                    WriteAttribute(_settings, "typeDocLocking", value);
                }
            }
        }

		[LocCategory("Document")]
		[Lookup("DefStorage")]
		public StorageProvider DefaultStorageProviderLoc
		{
			get
			{
				if (_defstgepro == null)
				{
					_defstgepro = new StorageProvider(this.DefaultStorageProvider);
				}
				return _defstgepro;
			}
			set
			{
				_defstgepro = value;
				this.DefaultStorageProvider = _defstgepro.ID;
			}
		}	
	
		[LocCategory("DEFAULTS")]
		[Lookup("Templates")]
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.DefaultsTemplatesEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
		public OMSType.DefaultTemplateCollection DefaultTemplates
		{
			get
			{
				if (_defaults == null)
				{
					_defaults = new DefaultTemplateCollection(this, _defaultTemplatesHeader);
					_defaults.Cleared +=new Crownwood.Magic.Collections.CollectionClear(OnDirty);
				}
				return _defaults;				
			}
		}
		
		/// <summary>
		/// Gets or Sets the default document storage location for the file type.
		/// </summary>
		[Browsable(false)]
		public short DefaultStorageProvider
		{
			get
			{
				return Common.ConvertDef.ToInt16(ReadAttribute(_settings,"typeStorageProvider","-1"), -1);
			}
			set
			{
				if (value < 0)
					WriteAttribute(_settings,"typeStorageProvider", "");
				else
					WriteAttribute(_settings,"typeStorageProvider", value);
			}
		}

		/// <summary>
		/// Gets or sets the policy option for what happens to documents after a file is terminated.
		/// </summary>
		[Browsable(true)]
		[DefaultValue("")]
		[LocCategory("Document")]
		[Lookup("RETENTIONPOLICY")]
		public string DocumentRetentionPolicy
		{
			get
			{
				return ReadAttribute(_settings, "typeDocRetentionPolicy", "");				
			}
			set
			{
				if (value == null) value = "";
				WriteAttribute(_settings,"typeDocRetentionPolicy", value);
			}
		}

		/// <summary>
		/// Gets or sets the number of days a document should be archived before after a file is terminated.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(-1)]
		[LocCategory("Document")]
		[Lookup("RETENTIONPERIOD")]
		public int DocumentRetentionPeriod
		{
			get
			{
				return Common.ConvertDef.ToInt32(ReadAttribute(_settings,"typeDocRetentionPeriod","-1"), -1);
			}
			set
			{
				if (value < 0)
					WriteAttribute(_settings,"typeDocRetentionPeriod", -1);
				else
					WriteAttribute(_settings,"typeDocRetentionPeriod", value);
			}
		}

        [Browsable(false)]
        public string RetentionFormat
        {
            get
            {
                return ReadAttribute(_settings, "typeRetentionFormat", "");
            }
            set
            {
                if (value == null) value = "";
                WriteAttribute(_settings, "typeRetentionFormat", value);
            }
        }
        private CodeLookupDisplay retFmt;
        [Browsable(true)]
        [DefaultValue("")]
        [LocCategory("Document")]
        [Lookup("RETFMTNAME")]
        [CodeLookupSelectorTitle("RETFMT", "Retention Format")]
        public CodeLookupDisplay RetentionFormatProperty
        {
            get
            {
                if (retFmt == null)
                {
                    retFmt = new CodeLookupDisplay("RETFMT");
                    retFmt.Code = RetentionFormat;
                }
                return retFmt;
            }
            set
            {
                retFmt = value;
                this.RetentionFormat = value.Code;
            }
        }


        

        [Browsable(false)]
        public string TemplateFormat
        {
            get
            {
                return ReadAttribute(_settings, "typeTemplateFormat", "");
            }
            set
            {
                if (value == null) value = "";
                WriteAttribute(_settings, "typeTemplateFormat", value);
            }
        }
        private CodeLookupDisplay tmpFmt;
        /// <summary>
        /// Property for Template to be used against the File Type
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [LocCategory("Document")]
        [Lookup("TPLTFMATNAME")]
        [CodeLookupSelectorTitle("TMPFMT", "Template Format")]
        public CodeLookupDisplay TemplateFormatProperty
        {
            get
            {
                if (tmpFmt == null)
                {
                    tmpFmt = new CodeLookupDisplay("TMPFMT");
                    tmpFmt.Code = TemplateFormat;
                }
                return tmpFmt;
            }
            set
            {
                tmpFmt = value;
                this.TemplateFormat = value.Code; 
            }
        }

		/// <summary>
		/// Gets the Startup Jobs
		/// </summary>
		[Lookup("StartupDocs")]
		[LocCategory("Document")]
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.JobEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
		public FileType.JobsCollection StartupDocuments
		{
			get
			{
				if (_jobs == null)
				{
					XmlNode root = _config.SelectSingleNode("/Config");
					_jobHeader = root.SelectSingleNode("Jobs") as XmlElement;
					if (_jobHeader == null)
					{
						_jobHeader = _config.CreateElement("Jobs");
						root.AppendChild(_jobHeader);
					}

					_jobs = new JobsCollection(this, _jobHeader);
					_jobs.Cleared +=new Crownwood.Magic.Collections.CollectionClear(OnDirty);

				}
				return _jobs;
			}
		}		
		
		/// <summary>
		/// Gets the multi associate collection of the file type.
		/// </summary>
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.MultiAssociateEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
		[Lookup("MultiAssoc")]
		public FileType.MultiAssociateCollection MultiAssociates
		{
			get
			{
				if (_multiassocs == null)
				{
					_multiassocs = new MultiAssociateCollection(this);
				}
				return _multiassocs;
			}
		}

		/// <summary>
		/// Gets or Sets the property that specifies whether the conflict search is to be used or not.
		/// </summary>
		public override bool SearchOnCreate
		{
			get
			{
				//The session property is checked first so that a file conflict search must be used
				//if this variable is set to true.
				if (Session.CurrentSession.IsFileConflictCheckEnabled)
					return true;
				else
					return base.SearchOnCreate;
			}
			set
			{
				if (Session.CurrentSession.IsFileConflictCheckEnabled)
					throw new OMSException2("864632454646666", "This cannot be changed while 'Is File Conflict Check Enabled' is On");
				else
					base.SearchOnCreate = value;
			}
		}

		[LocCategory("AddInfo")]
		[Lookup("DepCode")]
		[CodeLookupSelectorTitle("DEPARTMENTS","Departments")]
		public CodeLookupDisplayReadOnly DefaultDepartmentLoc
		{
			get
			{
				if (_filedeptcode == null)
				{
					_filedeptcode = new CodeLookupDisplayReadOnly("DEPT");
					_filedeptcode.Code = this.DefaultDepartment;
				}
				return _filedeptcode;
			}
			set
			{
				_filedeptcode = value;
				this.DefaultDepartment = _filedeptcode.Code;
			}
		}
		
		/// <summary>
		/// Gets or Sets the file seed number for file number generation.
		/// </summary>
		[LocCategory("SETTINGS")]
		public string SeedNumber
		{
			get
			{
				return Convert.ToString(GetExtraInfo("typeseed"));
			}
			set
			{
				SetExtraInfo("typeseed", value);
			}
		}

		/// <summary>
		/// Gets or Set the Enabling of the File Review Task
		/// </summary>
		[LocCategory("FILEREVIEW")]
		[Lookup("ENABLE")]
		public bool EnableFileReview
		{
			get
			{
				return ConvertDef.ToBoolean(GetExtraInfo("fileReview"),false);
			}
			set
			{
				SetExtraInfo("fileReview", value);
			}
		}
		
		/// <summary>
		/// Gets or Set the Enabling of the File Review Task
		/// </summary>
		[LocCategory("FILEREVIEW")]
		[Lookup("DAYS")]
		public int FileReviewDays
		{
			get
			{
				return ConvertDef.ToInt32(GetExtraInfo("fileReviewDays"),0);
			}
			set
			{
				SetExtraInfo("fileReviewDays", value);
			}
		}

		/// <summary>
		/// Gets or Set the Security Audit on a new File Takeon
		/// </summary>
		[Lookup("SecAudit")]
		public bool SecurityAudit
		{
			get
			{
				return ConvertDef.ToBoolean(GetExtraInfo("fileSecurity"),true);
			}
			set
			{
				SetExtraInfo("fileSecurity", value);
			}
		}

		/// <summary>
		/// Gets or Set the Renite Default Security Settings
		/// </summary>
		[Lookup("RemoteAccSet")]
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.FlagsEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
		public RemoteAccSettings RemoteAccSettings
		{
			get
			{
				if (GetExtraInfo("fileRemoteAccSet") != DBNull.Value)
					return (RemoteAccSettings)Convert.ToInt64(Convert.ToString(GetExtraInfo("fileRemoteAccSet")),2);
				else
					return 0;
			}
			set
			{
				SetExtraInfo("fileRemoteAccSet", Convert.ToString((Int64)value,2));
			}
		}

		/// <summary>
		/// Gets the default precedent Library for the file type.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public string PrecedentLibrary
		{
			get
			{
				return Convert.ToString(GetExtraInfo("filePrecLibrary"));
			}
			set
			{
				SetExtraInfo("filePrecLibrary", value);
			}
		}

        /// <summary>
        /// Gets the default Precedent Category for the file type.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
		public string PrecedentCategory
		{
			get
			{
				return Convert.ToString(GetExtraInfo("filePrecCategory"));
			}
			set
			{
				SetExtraInfo("filePrecCategory", value);
			}
		}

		/// <summary>
		/// Gets the default precedent Sub Category for the file type.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public string PrecedentSubCategory
		{
			get
			{
				return Convert.ToString(GetExtraInfo("filePrecSubCategory"));
			}
			set
			{
				SetExtraInfo("filePrecSubCategory", value);
			}
		}

        /// <summary>
        /// Gets the default Precedent Minor Category for the file type.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public string PrecedentMinorCategory
        {
            get
            {
                return Convert.ToString(GetExtraInfo("filePrecMinorCategory"));
            }
            set
            {
                SetExtraInfo("filePrecMinorCategory", value);
            }
        }


        /// <summary>
        /// Gets the default precedent Library Codelookupized for the file type.
        /// </summary>
        [LocCategory("PRECEDENT")]
		[Lookup("PRECLIBRARY")]
		[CodeLookupSelectorTitle("PRECLIBRARY","Precedent Libraries")]
		public CodeLookupDisplayReadOnly PrecedentLibraryLoc
		{
			get
			{
				if (_library == null)
				{
					_library = new CodeLookupDisplayReadOnly("PRECLIBRARY");
					_library.Code = this.PrecedentLibrary;
				}
				return _library;
			}
			set
			{
				_library = value;
				this.PrecedentLibrary = _library.Code;
			}
		}

		/// <summary>
		/// Gets the default Precedent Category Codelookupized for the file type.
		/// </summary>
		[LocCategory("PrecedentConfig")]
		[Lookup("PrecCat")]
		[CodeLookupSelectorTitle("PRECCATEGORY","Precedent Categories")]
		public CodeLookupDisplayReadOnly PrecedentCategoryLoc
		{
			get
			{
				if (_filepreccategory == null)
				{
					_filepreccategory = new CodeLookupDisplayReadOnly("PRECCAT");
					_filepreccategory.Code = this.PrecedentCategory;
				}
				return _filepreccategory;
			}
			set
			{
				_filepreccategory = value;
				this.PrecedentCategory = _filepreccategory.Code;
			}
		}

		/// <summary>
		/// Gets the default precedent Sub Category Codelookupized for the file type.
		/// </summary>
		[LocCategory("PrecedentConfig")]
		[Lookup("PrecSubCat")]
		[CodeLookupSelectorTitle("PRECSUBCAT","Precedent Sub Categories")]
		public CodeLookupDisplayReadOnly PrecedentSubCategoryLoc
		{
			get
			{
				if (_fileprecsubcategory == null)
				{
					_fileprecsubcategory = new CodeLookupDisplayReadOnly("PRECSUBCAT");
					_fileprecsubcategory.Code = this.PrecedentSubCategory;
				}
				return _fileprecsubcategory;
			}
			set
			{
				_fileprecsubcategory = value;
				this.PrecedentSubCategory = _fileprecsubcategory.Code;
			}
		}

        /// <summary>
        /// Gets the default precedent Minor Category Codelookupized for the file type.
        /// </summary>
        [LocCategory("PrecedentConfig")]
        [Lookup("PrecMinorCat")]
        [CodeLookupSelectorTitle("PRECMINORCAT", "Precedent Minor Categories")]
        public CodeLookupDisplayReadOnly PrecedentMinorCategoryLoc
        {
            get
            {
                if (_fileprecminorcategory == null)
                {
                    _fileprecminorcategory = new CodeLookupDisplayReadOnly("PRECMINORCAT");
                    _fileprecminorcategory.Code = this.PrecedentMinorCategory;
                }
                return _fileprecminorcategory;
            }
            set
            {
                _fileprecminorcategory = value;
                this.PrecedentMinorCategory = _fileprecminorcategory.Code;
            }
        }

        /// <summary>
        /// Gets or Sets the destroy period number of days to automatically archive the file.
        /// </summary>
        public short DestroyPeriod
		{
			get
			{
				try
				{
					return (short)GetExtraInfo("fileDestroyPeriod");
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				SetExtraInfo("fileDestroyPeriod", value);
			}
		}

		/// <summary>
		/// Gets or Sets the default funding code of the file type / file. 
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public string DefaultFundingCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("filedeffundcode"));
			}
			set
			{
				SetExtraInfo("filedeffundcode", value);
			}
		}

       
        
        [LocCategory("AddInfo")]
        [Lookup("DefFundType")]
        [Editor("FWBS.OMS.Design.DataListEditor,OMS.UI", typeof(System.Drawing.Design.UITypeEditor))]
        [Design.DataList("DFUNDACT+TYPE", NullValue = "", UseNull = false, DisplayMember = "ftDesc", ValueMember = "ftCode")]
        [TypeConverter(typeof(Design.DataListConverter))]
        public string DefaultFundingCodeLoc
        {
            get
            {
                if (_filefundtype == null)
                {
                    _filefundtype = this.DefaultFundingCode;
                }
                return _filefundtype;
            }
            set
            {
                if (_filefundtype != value)
                {
                    _filefundtype = value;
                    this.DefaultFundingCode = _filefundtype;
                }
            }

        }



		/// <summary>
		/// Gets or Sets the default department code for the file type / file.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public string DefaultDepartment
		{
			get
			{
				return Convert.ToString(GetExtraInfo("filedeptcode"));
			}
			set
			{
				SetExtraInfo("filedeptcode", value);
			}
		}

		/// <summary>
		/// Gets or Sets the external account code of the file type which may be used to link
		/// a file type to another instance of the file type in an external database / accounting 
		/// system.
		/// </summary>
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.Elite3ELookupEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
		[Parameter("MattType")]
		public string AccountCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("fileacccode"));
			}
			set
			{
				SetExtraInfo("fileacccode", value);
			}
		}

		/// <summary>
		/// Gets or Sets a flag that specifies whether the file of this type can have a 
		/// list of milestones to pick from.  If false then the specified milestone must be used.
		/// </summary>
		[LocCategory("MILESTONE")]
		[Lookup("ACTIVE")]
		public bool MilestoneActive
		{
			get
			{
				return ConvertDef.ToBoolean(GetExtraInfo("fileMilestoneActive"),false);
			}
			set
			{
				SetExtraInfo("fileMilestoneActive", value);
			}
		}

		/// <summary>
		/// Gets or Sets the mileston plan code of the file type.  This will be what a new file based
		/// on the type will use as a milestone plan.
		/// </summary>
		[LocCategory("MILESTONE")]
		[Lookup("PLAN")]
		[System.ComponentModel.TypeConverter("FWBS.OMS.UI.Windows.Design.MileStonePlanLister,omsadmin")]
		public string MilestonePlan
		{
			get
			{
				return Convert.ToString(GetExtraInfo("fileMilestoneCode"));
			}
			set
			{
				if (value == "")
					SetExtraInfo("fileMilestoneCode", DBNull.Value);
				else
					SetExtraInfo("fileMilestoneCode", value);
			}
		}

		#endregion

		#region Methods

		public override void Update()
		{
            if (_security != null)
                _security.Update();

            if (_jobHeader != null)
			{
				for(int ctr = _jobHeader.ChildNodes.Count -1; ctr >= 0 ; ctr--)
				{
					XmlNode nd = _jobHeader.ChildNodes[ctr];
					if (nd is XmlElement)
						_jobHeader.RemoveChild(nd);
				}
				foreach (Job jb in StartupDocuments)
				{
					_jobHeader.AppendChild(jb.Element.Clone());
				}
			}
			
			base.Update();

			if (_multiassocs != null)
			{
				_multiassocs.Update();
			}
		}

        /// <summary>
		/// Gets a fund type object based on what hasbeen assigned to a file type.
		/// </summary>
		/// <returns>A fund type object.</returns>
		public FundType GetFundType()
		{
			return FundType.GetFundType(DefaultFundingCode, Session.CurrentSession.DefaultCurrency);
		}

		#endregion

		#region Multi Associate Class

		public class MultiAssociate : LookupTypeDescriptor
		{
			private DataRow _info;
			private OMSType _type;
			private Contact _cont = null;
			private CodeLookupDisplay _code = null;
						
			internal MultiAssociate(OMSType type, DataRow info, Contact contact)
			{
				_info = info;
				_type = type;
				SetExtraInfo("filetype", _type.Code);
				if (contact != null)
					SetExtraInfo("contid", contact.ID);
			}

			internal DataRow Info
			{
				get
				{
					return _info;
				}
			}

			public override string ToString()
			{
				if (GetExtraInfo("contid") == DBNull.Value)
					return "";
				else
					return this.Contact.ToString();
			}

			[System.ComponentModel.Browsable(false)]
			public OMSType OMSType
			{
				get
				{
					return _type;
				}
			}

			[System.ComponentModel.Browsable(false)]
			public Type OMSObjectType
			{
				get
				{
					return OMSType.OMSObjectType;
				}
			}

			private object GetExtraInfo(string fieldName)
			{
				object val = _info[fieldName];
                //UTCFIX: DM - 30/11/06 - return local time
                if (val is DateTime)
                    return ((DateTime)val).ToLocalTime();
                else
                    return val;
			}

			private void SetExtraInfo(string fieldName, object value)
			{
                //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
                if (value is DateTime)
                {
                    DateTime dteval = (DateTime)value;
                    if (dteval.Kind == DateTimeKind.Unspecified)
                        value = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
                }

				_info[fieldName] = value;
				_type.OnDirty();
			}

			public string GetAssociateHeading(OMSFile file)
			{
				FieldParser parser = new FieldParser(file);
				System.Text.StringBuilder assocHead = new System.Text.StringBuilder();

				string h1 = "";
				if (DefaultHeadingLine1 != "")
				{
					h1 = DefaultHeadingLine1;
				}
				if (DefaultHeadingField1 != "")
				{
					if (h1 != "") h1 += " ";
					h1 += parser.Parse(DefaultHeadingField1);
				}
				assocHead.Append(h1);

				string h2 = "";
				if (h1 != "") assocHead.Append(Environment.NewLine);
				if (DefaultHeadingLine2 != "")
				{
					h2 = DefaultHeadingLine2;
				}
				if (DefaultHeadingField2 != "")
				{
					if (h2 != "") h2 += " ";
					h2 += parser.Parse(DefaultHeadingField2);
				}
				assocHead.Append(h2);
			
				string h3 = "";
				if (h2 != "") assocHead.Append(Environment.NewLine);
				if (DefaultHeadingLine3 != "")
				{
					h3 = DefaultHeadingLine3;
				}
				if (DefaultHeadingField3 != "")
				{
					if (h3 != "") h3 += " ";
					h3 += parser.Parse(DefaultHeadingField3);
				}
				assocHead.Append(h3);

				return assocHead.ToString();
			}

			public Contact Contact
			{
				get
				{
					if (_cont == null)
					{
						if (GetExtraInfo("contid") == DBNull.Value)
							_cont = null;
						else
							_cont = Contact.GetContact(Common.ConvertDef.ToInt64(GetExtraInfo("contid"), -1));
					}
					return _cont;
				}

			}

			[System.ComponentModel.Browsable(false)]
			public string AssocType
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multiType"));
				}
				set
				{
					if (value == null || value== "")
						throw new OMSException2("ERRASSTYPEREQ","The Associate Type is required");
					else
						SetExtraInfo("multiType", value);
				}
			}

			[Lookup("ASSOCTYPE")]
			[CodeLookupSelectorTitle("ASSTYPES","Associate Types")]
			public CodeLookupDisplay LocalizedAssocType
			{
				get
				{
					if (_code == null)
					{
						_code = new CodeLookupDisplay("SUBASSOC");
						_code.Code = AssocType;
					}
					return _code;
				}
				set
				{
					_code = value;
					AssocType = value.Code;
				}
			}


			[Lookup("REF")]
			public string DefaultRef
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multidefref"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multidefref", DBNull.Value);
					else
						SetExtraInfo("multidefref", value);
				}
			}

			[Lookup("HEAD1")]
			public string DefaultHeadingLine1
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multidefheading1"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multidefheading1", DBNull.Value);
					else
						SetExtraInfo("multidefheading1", value);
				}
			}

			[Lookup("HEADFLD1")]
			public string DefaultHeadingField1
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multidefheadingField1"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multidefheadingField1", DBNull.Value);
					else
						SetExtraInfo("multidefheadingField1", value);
				}
			}

			[Lookup("HEAD2")]
			public string DefaultHeadingLine2
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multidefheading2"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multidefheading2", DBNull.Value);
					else
						SetExtraInfo("multidefheading2", value);
				}
			}

			[Lookup("HEADFLD2")]
			public string DefaultHeadingField2
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multidefheadingField2"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multidefheadingField2", DBNull.Value);
					else
						SetExtraInfo("multidefheadingField2", value);
				}
			}


			[Lookup("HEAD3")]
			public string DefaultHeadingLine3
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multidefheading3"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multidefheading3", DBNull.Value);
					else
						SetExtraInfo("multidefheading3", value);
				}
			}

			[Lookup("HEADFLD3")]
			public string DefaultHeadingField3
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multidefheadingField3"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multidefheadingField3", DBNull.Value);
					else
						SetExtraInfo("multidefheadingField3", value);
				}
			}

		
			[Lookup("SALUTATION")]
			public string DefaultSalutation
			{
				get
				{
					string salut = Convert.ToString(GetExtraInfo("multidefsalut"));
					return salut;
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multidefsalut", DBNull.Value);
					else
						SetExtraInfo("multidefsalut", value);
				}
			}

			[Lookup("ADDRESSEE")]
			public string DefaultAddressee
			{
				get
				{
					string add = Convert.ToString(GetExtraInfo("multidefaddressee"));
					return add;
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multidefaddressee", DBNull.Value);
					else
						SetExtraInfo("multidefaddressee", value);
				}
			}

			[Lookup("DDI")]
			public string DefaultDDI
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multidefddi"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multidefddi", DBNull.Value);
					else
						SetExtraInfo("multidefddi", value);
				}
			}

			[Lookup("MOBILE")]
			public string DefaultMobile
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multidefmobile"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multidefmobile", DBNull.Value);
					else
						SetExtraInfo("multidefmobile", value);
				}
			}

			[Lookup("FAX")]
			public string DefaultFax
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multideffax"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multideffax", DBNull.Value);
					else
						SetExtraInfo("multideffax", value);
				}
			}

			[Lookup("EMAIL")]
			public string DefaultEmail
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multidefemail"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multidefemail", DBNull.Value);
					else
						SetExtraInfo("multidefemail", value);
				}
			}
		}

		#endregion

		#region MultiAssociate OMS Type Collection

		public class MultiAssociateCollection : Crownwood.Magic.Collections.CollectionWithEvents
		{
			private DataTable _info;
			private OMSType _type;

			private const string Table = "MULTIASSOCS";
			private const string Sql = "select * from dbassociatesmulti";

			private MultiAssociateCollection(){}

			internal MultiAssociateCollection(OMSType type)
			{
				_type = type;

				IDataParameter [] pars = new IDataParameter[1];
				pars[0] = Session.CurrentSession.Connection.AddParameter("type", _type.Code);
				_info = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbassociatesmulti where filetype = @type", "MULTIASSOCS", pars);

				foreach (DataRow row in _info.Rows)
				{
					Add(new MultiAssociate(_type, row, null));
				}

				//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
				if (_info.PrimaryKey == null || _info.PrimaryKey.Length == 0)
					_info.PrimaryKey = new DataColumn[1]{_info.Columns["multiID"]};

			}

            protected override void OnRemove(int index, object value)
            {
                base.OnRemove(index, value);

                _type.OnDirty();
            }

            protected override void OnClear()
            {
                base.OnClear();

                _type.OnDirty();
            }

            public MultiAssociate New(Contact contact)
            {
                //UTCFIX: DM - 30/11/06 - Make sure that code is unique to UTC time..
                DataRow row = _info.NewRow();
                row["multiid"] = System.DateTime.UtcNow.Ticks;
                MultiAssociate ass = new MultiAssociate(_type, row, contact);
                ass.AssocType = "CLIENT";
                _type.OnDirty();
                return ass;
            }

			public MultiAssociate Add(MultiAssociate value)
			{
				// Use base class to process actual collection operation
				base.List.Add(value as object);
				return value;
			}

			public void AddRange(MultiAssociate[] values)
			{
				// Use existing method to add each array entry
				foreach(MultiAssociate page in values)
					Add(page);
			}

			public void Remove(MultiAssociate value)
			{
				// Use base class to process actual collection operation
				base.List.Remove(value as object);
			}

			public void Insert(int index, MultiAssociate value)
			{
				// Use base class to process actual collection operation
				base.List.Insert(index, value as object);
			}

			public bool Contains(MultiAssociate value)
			{
				// Use base class to process actual collection operation
				return base.List.Contains(value as object);
			}

			public MultiAssociate this[int index]
			{
				// Use base class to process actual collection operation
				get { return (base.List[index] as MultiAssociate); }
			}

			public int IndexOf(MultiAssociate value)
			{
				// Find the 0 based index of the requested entry
				return base.List.IndexOf(value);
			}

			internal void Update()
			{
				bool added = false;
				foreach (MultiAssociate assoc in this)
				{
					if (assoc.Info.RowState == DataRowState.Detached)
					{
						added = true;
						_info.Rows.Add(assoc.Info);
					}

					//INFO: Adds the associate type to the associate types collection if not already exists.
					//This may later need to be managed better.
					DataTable dt = Associate.GetAssociateTypes(assoc.Contact.ContactTypeCode, false);
					dt.DefaultView.RowFilter= "typecode = '" + Common.SQLRoutines.RemoveRubbish(assoc.AssocType) + "'";
					if (dt.DefaultView.Count == 0)
					{
						AskEventArgs e = new AskEventArgs("ADDASSOCTYPE", "The associated type does not exist for the specified contact type, would you like to add it?", "", AskResult.Yes);
						Session.CurrentSession.OnAsk(this, e);
						if (e.Result == AskResult.Yes)
						{
                            string sql = "INSERT INTO dbAssociatedTypes (conttype, assoctype) values (@CONTTYPE, @ASSOCTYPE)";
							IDataParameter[] pars = new IDataParameter[2];
							pars[0] = Session.CurrentSession.Connection.AddParameter("CONTTYPE", assoc.Contact.ContactTypeCode);
							pars[1] = Session.CurrentSession.Connection.AddParameter("ASSOCTYPE", assoc.AssocType);
							Session.CurrentSession.Connection.ExecuteSQL(sql, pars);
						}
					}
				}

				foreach(DataRow row in _info.Rows)
				{
					bool exists = false;
					foreach(MultiAssociate assoc in this)
					{
						if (assoc.Info == row)
						{
							exists = true;
							break;
						}
					}
					if (exists == false) row.Delete();
				}

				if (_info.GetChanges() != null)
				{
                    Session.CurrentSession.Connection.Update(_info, Sql + " where filetype = '" + Common.SQLRoutines.RemoveRubbish(_type.Code)+ "'");
					if (added)

						_info.AcceptChanges();
				}
			}
		}
		#endregion

		#region Job
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public class Job : LookupTypeDescriptor
		{
			private XmlElement _info;
			private FileType _type;
			private bool _isNew = false;
			private Precedent _precedent;
			private PrecPrintMode _printmode = PrecPrintMode.None;
			private PrecSaveMode _savemode = PrecSaveMode.None;
            private string _hashcode = "";

			private Job(){}

			public Job(FileType type, Precedent precedent)
			{
				_type = type;
				_info = type._config.CreateElement("Job");
				_isNew = true;

				_type.WriteAttribute(_info, "type", precedent.PrecedentType);
				_type.WriteAttribute(_info, "title", precedent.Title);
				_type.WriteAttribute(_info, "library", precedent.Library);
				_type.WriteAttribute(_info, "category", precedent.Category);
				_type.WriteAttribute(_info, "subcategory", precedent.SubCategory);
                _type.WriteAttribute(_info, "minorcategory", precedent.MinorCategory);
                _type.WriteAttribute(_info, "language", precedent.Language);
				_type.WriteAttribute(_info, "printMode", _printmode.ToString());
				_type.WriteAttribute(_info, "saveMode", _savemode.ToString());
				_precedent = precedent;
			}

			
			public Job(FileType type, XmlElement info) 
			{
				_info = info;
				_type = type;
			}

			[System.ComponentModel.Browsable(false)]
			public bool IsNew
			{
				get
				{
					return _isNew;
				}
			}

			[System.ComponentModel.Browsable(false)]
			internal FileType FileType
			{
				get
				{
					return _type;
				}
			}
			
			[System.ComponentModel.Browsable(false)]
			internal XmlElement Element
			{
				get
				{
					return _info;
				}
			}

			[System.ComponentModel.Browsable(false)]
			public Precedent Precedent
			{
				get
				{
                    if (_precedent == null) 
					{
                        string type = _type.ReadAttribute(_info, "type", "");
                        string title = _type.ReadAttribute(_info, "title", "");
                        string library = _type.ReadAttribute(_info, "library", "");
                        string category = _type.ReadAttribute(_info, "category", "");
                        string subcategory = _type.ReadAttribute(_info, "subcategory", "");
                        string minorcategory = _type.ReadAttribute(_info, "minorcategory", "");
                        string language = _type.ReadAttribute(_info, "language", "");
                        
                        _savemode = (PrecSaveMode)Common.ConvertDef.ToEnum(_type.ReadAttribute(_info, "saveMode", PrecSaveMode.None.ToString()), PrecSaveMode.None);
						_printmode = (PrecPrintMode)Common.ConvertDef.ToEnum(_type.ReadAttribute(_info, "printMode", PrecPrintMode.None.ToString()), PrecPrintMode.None);

                        try
                        {
                            _precedent = Precedent.GetPrecedent(title, type, library, category, subcategory,  language, minorcategory);
                        }
                        catch (Exception ex)
                        {
                            _precedent = new Precedent();
                            _precedent.Description = ex.Message;
                        }
					}
					return _precedent;
				}
			}

            [Browsable(false)]
            public string Hashcode
            {
                get
                {
                    string type = _type.ReadAttribute(_info, "type", "");
                    string title = _type.ReadAttribute(_info, "title", "");
                    string library = _type.ReadAttribute(_info, "library", "");
                    string category = _type.ReadAttribute(_info, "category", "");
                    string subcategory = _type.ReadAttribute(_info, "subcategory", "");
                    string minorcategory = _type.ReadAttribute(_info, "minorcategory", "");
                    string language = _type.ReadAttribute(_info, "language", "");
                    _hashcode = library + "/" + type + "/" + category + "/" + subcategory + "/" + minorcategory + "/" + title;

                    return _hashcode.ToUpper();
                }
            }

			public string Title
			{
				get
				{
					return this.Precedent.Title;
				}
			}
			public long PrecID
			{
				get
				{
					return this.Precedent.ID;
				}
			}
			public string Category
			{
				get
				{
					return this.Precedent.Category;
				}
			}
			public string SubCategory
			{
				get
				{
					return this.Precedent.SubCategory;
				}
			}
            public string MinorCategory
            {
                get
                {
                    return this.Precedent.MinorCategory;
                }
            }
            public string Library
			{
				get
				{
					return this.Precedent.Library;
				}
			}
			public string ContactType
			{
				get
				{
					return this.Precedent.ContactType;
				}
			}

			public PrecSaveMode SaveMode
			{
				get
				{
					return _savemode;
					
				}
				set
				{
					_savemode = value;
					_type.WriteAttribute(_info, "saveMode", value.ToString());
				}
			}

			[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.FlagsEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
			public PrecPrintMode PrintMode
			{
				get
				{
					return _printmode;
					
				}
				set
				{
					_printmode = value;
					_type.WriteAttribute(_info, "printMode", value.ToString());
				}
			}

			public override string ToString()
			{
                 return Precedent.Description;
 			}

		}
		#endregion

		#region Jobs OMS Type Collection
		public class JobsCollection : Crownwood.Magic.Collections.CollectionWithEvents
		{
			private XmlElement _info;
			private FileType _type;
			
			private JobsCollection(){}

			public JobsCollection(FileType type, XmlElement info)
			{
				_type = type;
				_info = info;

				foreach (XmlNode nd in _info.ChildNodes)
				{
					if (nd is XmlElement)
					{
						Add(new Job(_type, (XmlElement)nd.Clone()));
					}
				}

			}

			public Job Add(Job value)
			{
				// Use base class to process actual collection operation
				base.List.Add(value as object);
				return value;
			}

			public void AddRange(Job[] values)
			{
				// Use existing method to add each array entry
				foreach(Job page in values)
					Add(page);
			}

			public void Remove(Job value)
			{
				// Use base class to process actual collection operation
				base.List.Remove(value as object);
			}

			public void Insert(int index, Job value)
			{
				// Use base class to process actual collection operation
				base.List.Insert(index, value as object);
			}

			public bool Contains(Job value)
			{
				// Use base class to process actual collection operation
				return base.List.Contains(value as object);
			}

			public Job this[int index]
			{
				// Use base class to process actual collection operation
				get { return (base.List[index] as Job); }
			}

			public int IndexOf(Job value)
			{
				// Find the 0 based index of the requested entry
				return base.List.IndexOf(value);
			}
		}
		#endregion

	}


	/// <summary>
	/// An OMS user type that manipulates the configurable xml of the object.
	/// </summary>
	public class UserType : BuiltInOMSType
	{
		#region Constructors

		/// <summary>
		/// Creates a new user type with properties needing to be set.
		/// </summary>
		public UserType() : base ("")
		{
		}

		/// <summary>
		/// Gets an existing user type.
		/// </summary>
		/// <param name="code">A user type code.</param>
		internal UserType (string code) : base(code)
		{
			//An edit contructor should add the object created to the session memory collection.
			if (IsNew == false) 
			{
				foreach (DataRow row in _omstype.Tables["FORM"].Rows)
					row["frmdesc"] = Convert.ToString(_omstype.Tables["FORM"].Rows[0]["frmdesc"]).Replace("%USERTYPE%", this.Description);
				Session.CurrentSession.CurrentUserTypes.Add(Code, this);
				
				//Call the extensibility event for addins.
				OnObjectEvent(Extensibility.ObjectEvent.Loaded);
			}
		}

		#endregion

		#region Abstraction

		/// <summary>
		/// Clones the OMSType.
		/// </summary>
		/// <returns>New OMSType.</returns>
		public override OMSType Clone()
		{
			UserType ut = new UserType();
			ut._omstype = _omstype.Copy();
			ut._omstype.Tables[0].Clear();
			ut._omstype.AcceptChanges();
			ut._omstype.Tables[0].Rows.Add(_omstype.Tables[0].Rows[0].ItemArray);
			ut.SetExtraInfo("typecode", DBNull.Value);
			ut.SetExtraInfo("typedesc", DBNull.Value);
			ut.BuildXML();
			return ut;
		}

		/// <summary>
		/// Gets the object type name.
		/// </summary>
		public override Type OMSObjectType
		{
			get
			{
				return typeof(User);
			}
		}

		
		public override string SearchListName
		{
			get
			{
				return "ADMUSERTYPE";
			}
		}

		/// <summary>
		/// Gets the code lookup group for the oms type so that the cultured names
		/// of the OMS types can be stored under their own categories.
		/// </summary>
		public override string CodeLookupGroup
		{
			get
			{
				return "USERTYPE";
			}
		}

		/// <summary>
		/// Gets the table name which is used to update from the database.
		/// </summary>
		protected override string TableName
		{
			get
			{
				return "dbUserType";;
			}
		}

		/// <summary>
		/// Gets the procedure name to execute to get the type information.
		/// </summary>
		protected override string Procedure
		{
			get
			{
				return "sprUserType";
			}
		}

		/// <summary>
		/// Gets the caceh folder name.
		/// </summary>
		protected override string CacheFolder
		{
			get
			{
				return "UserTypes";
			}
		}

		/// <summary>
		/// Gets an object of the derived type.
		/// </summary>
		/// <param name="id">A unique identifer for the object.</param>
		/// <returns>An IOMSType compaitble object.</returns>
		public override IOMSType GetObject(object id)
		{
			return User.GetUser((int)id);
		}

        /// <summary>
        /// Creates an instance of the IOMSType.
        /// </summary>
        /// <param name="parameters">Specifies the parameters needed to construct the object.</param>
        /// <returns>An IOMSType object.</returns>
        public override IOMSType CreateObject(object [] parameters)
		{
			return new User(this);
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets a specific user type.
		/// <param name="type">User type code.</param>
		/// </summary>
		/// <returns>A user type object.</returns>
		public static UserType GetUserType(string type)
		{
			Session.CurrentSession.CheckLoggedIn();

			UserType ut = Session.CurrentSession.CurrentUserTypes[type] as UserType;

			if (ut == null)
			{
				ut = new UserType(type);
			}		
			return ut;
		}

		#endregion


	}

	/// <summary>
	/// An OMS fee earner type that manipulates the configurable xml of the object.
	/// </summary>
	public class FeeEarnerType : BuiltInOMSType
	{
		#region Constructors

		/// <summary>
		/// Creates a new FeeEarner type with properties needing to be set.
		/// </summary>
		public FeeEarnerType() : base ("")
		{
		}

		/// <summary>
		/// Gets an existing FeeEarner type.
		/// </summary>
		/// <param name="code">A FeeEarner type code.</param>
		internal FeeEarnerType (string code) : base(code)
		{
			//An edit contructor should add the object created to the session memory collection.
			if (IsNew == false) 
			{
				foreach (DataRow row in _omstype.Tables["FORM"].Rows)
					row["frmdesc"] = Convert.ToString(_omstype.Tables["FORM"].Rows[0]["frmdesc"]).Replace("%FEEEARNERTYPE%", this.Description);
				Session.CurrentSession.CurrentFeeEarnerTypes.Add(Code, this);
				
				//Call the extensibility event for addins.
				OnObjectEvent(Extensibility.ObjectEvent.Loaded);
			}
		}

		#endregion

		#region Abstraction

		/// <summary>
		/// Clones the OMSType.
		/// </summary>
		/// <returns>New OMSType.</returns>
		public override OMSType Clone()
		{
			FeeEarnerType ut = new FeeEarnerType();
			ut._omstype = _omstype.Copy();
			ut._omstype.Tables[0].Clear();
			ut._omstype.AcceptChanges();
			ut._omstype.Tables[0].Rows.Add(_omstype.Tables[0].Rows[0].ItemArray);
			ut.SetExtraInfo("typecode", DBNull.Value);
			ut.SetExtraInfo("typedesc", DBNull.Value);
			ut.BuildXML();
			return ut;
		}

		/// <summary>
		/// Gets the object type name.
		/// </summary>
		public override Type OMSObjectType
		{
			get
			{
				return typeof(FeeEarner);
			}
		}

		/// <summary>
		/// Gets the code lookup group for the oms type so that the cultured names
		/// of the OMS types can be stored under their own categories.
		/// </summary>
		public override string CodeLookupGroup
		{
			get
			{
				return "FEEEARNERTYPE";
			}
		}


		public override string SearchListName
		{
			get
			{
				return "ADMFEETYPE";
			}
		}

		/// <summary>
		/// Gets the table name which is used to update from the database.
		/// </summary>
		protected override string TableName
		{
			get
			{
				return "dbFeeEarnerType";;
			}
		}

		/// <summary>
		/// Gets the procedure name to execute to get the type information.
		/// </summary>
		protected override string Procedure
		{
			get
			{
				return "sprFeeEarnerType";
			}
		}

		/// <summary>
		/// Gets the caceh folder name.
		/// </summary>
		protected override string CacheFolder
		{
			get
			{
				return "FeeEarnerTypes";
			}
		}

		/// <summary>
		/// Gets an object of the derived type.
		/// </summary>
		/// <param name="id">A unique identifer for the object.</param>
		/// <returns>An IOMSType compaitble object.</returns>
		public override IOMSType GetObject(object id)
		{
			return FeeEarner.GetFeeEarner((int)id);
		}

        /// <summary>
        /// Creates an instance of the IOMSType.
        /// </summary>
        /// <param name="parameters">Specifies the parameters needed to construct the object.</param>
        /// <returns>An IOMSType object.</returns>
        public override IOMSType CreateObject(object [] parameters)
		{
			return new FeeEarner(this);
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets a specific FeeEarner type.
		/// <param name="type">FeeEarner type code.</param>
		/// </summary>
		/// <returns>A FeeEarner type object.</returns>
		public static FeeEarnerType GetFeeEarnerType(string type)
		{
			Session.CurrentSession.CheckLoggedIn();

			FeeEarnerType ut = Session.CurrentSession.CurrentFeeEarnerTypes[type] as FeeEarnerType;

			if (ut == null)
			{
				ut = new FeeEarnerType(type);
			}		
			return ut;
		}

		#endregion


	}

	/// <summary>
	/// An OMS command centre type that manipulates the configurable xml of the object.
	/// </summary>
	public class CommandCentreType : BuiltInOMSType
	{
		#region Constructors

		/// <summary>
		/// Creates a new centre type with properties needing to be set.
		/// </summary>
		public CommandCentreType() : base ("")
		{
		}

		/// <summary>
		/// Gets an existing centre type.
		/// </summary>
		internal CommandCentreType (string code) : base(code)
		{
			//An edit contructor should add the object created to the session memory collection.
			if (IsNew == false) 
			{
				foreach (DataRow row in _omstype.Tables["FORM"].Rows)
					row["frmdesc"] = Convert.ToString(_omstype.Tables["FORM"].Rows[0]["frmdesc"]).Replace("%CENTRETYPE%", this.Description);
				Session.CurrentSession.CurrentCommandCentreTypes.Add(Code, this);
				
				//Call the extensibility event for addins.
				OnObjectEvent(Extensibility.ObjectEvent.Loaded);
			}
		}

		#endregion

		#region Abstraction

		/// <summary>
		/// Clones the OMSType.
		/// </summary>
		public override OMSType Clone()
		{
			CommandCentreType ut = new CommandCentreType();
			ut._omstype = _omstype.Copy();
			ut._omstype.Tables[0].Clear();
			ut._omstype.AcceptChanges();
			ut._omstype.Tables[0].Rows.Add(_omstype.Tables[0].Rows[0].ItemArray);
			ut.SetExtraInfo("typecode", DBNull.Value);
			ut.SetExtraInfo("typedesc", DBNull.Value);
			ut.BuildXML();
			return ut;
		}

		/// <summary>
		/// Gets the object type name.
		/// </summary>
		public override Type OMSObjectType
		{
			get
			{
				return typeof(User);
			}
		}

		public override string SearchListName
		{
			get
			{
				return "ADMCMDTYPE";
			}
		}

		/// <summary>
		/// Gets the code lookup group for the oms type so that the cultured names
		/// of the OMS types can be stored under their own categories.
		/// </summary>
		public override string CodeLookupGroup
		{
			get
			{
				return "CENTRETYPE";
			}
		}

		/// <summary>
		/// Gets the table name which is used to update from the database.
		/// </summary>
		protected override string TableName
		{
			get
			{
				return "dbCommandCentreType";;
			}
		}

		/// <summary>
		/// Gets the procedure name to execute to get the type information.
		/// </summary>
		protected override string Procedure
		{
			get
			{
				return "sprCommandCentreType";
			}
		}

		/// <summary>
		/// Gets the caceh folder name.
		/// </summary>
		protected override string CacheFolder
		{
			get
			{
				return "CommandCentreTypes";
			}
		}

		/// <summary>
		/// Gets an object of the derived type.
		/// </summary>
		/// <param name="id">A unique identifer for the object.</param>
		/// <returns>An IOMSType compaitble object.</returns>
		public override IOMSType GetObject(object id)
		{
			return User.GetUser((int)id);
		}

        /// <summary>
        /// Creates an instance of the IOMSType.
        /// </summary>
        /// <param name="parameters">Specifies the parameters needed to construct the object.</param>
        /// <returns>An IOMSType object.</returns>
        public override IOMSType CreateObject(object [] parameters)
		{
			return null;
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets a specific command centre type.
        /// </summary>
		public static CommandCentreType GetCentreType(string type)
		{
			Session.CurrentSession.CheckLoggedIn();

			CommandCentreType ut = Session.CurrentSession.CurrentCommandCentreTypes[type] as CommandCentreType;

			if (ut == null)
			{
				ut = new CommandCentreType(type);
			}		
			return ut;
		}

		#endregion


	}


    /// <summary>
    /// An associate type that manipulates the configurable xml of an associate.
    /// </summary>
    public class AssociateType : BuiltInOMSType
    {
        #region Constructors

        /// <summary>
        /// Creates a new associate type with properties needing to be set.
        /// </summary>
        public AssociateType()
            : base("")
        {
        }

        /// <summary>
        /// Gets an existing associate type.
        /// </summary>
        /// <param name="code">A associate type code.</param>
        internal AssociateType(string code)
            : base(code)
        {
            //An edit contructor should add the object created to the session memory collection.
            if (IsNew == false)
            {
                foreach (DataRow row in _omstype.Tables["FORM"].Rows)
                    row["frmdesc"] = Convert.ToString(_omstype.Tables["FORM"].Rows[0]["frmdesc"]).Replace("%ASSOCTYPE%", this.Description);
                Session.CurrentSession.CurrentAssociateTypes.Add(Code, this);

                //Call the extensibility event for addins.
                OnObjectEvent(Extensibility.ObjectEvent.Loaded);
            }
        }

        #endregion

        #region Abstraction

        /// <summary>
        /// Clones the OMSType.
        /// </summary>
        /// <returns>New OMSType.</returns>
        public override OMSType Clone()
        {
            AssociateType ut = new AssociateType();
            ut._omstype = _omstype.Copy();
            ut._omstype.Tables[0].Clear();
            ut._omstype.AcceptChanges();
            ut._omstype.Tables[0].Rows.Add(_omstype.Tables[0].Rows[0].ItemArray);
            ut.SetExtraInfo("typecode", DBNull.Value);
            ut.SetExtraInfo("typedesc", DBNull.Value);
            ut.BuildXML();
            return ut;
        }

        /// <summary>
        /// Gets the object type name.
        /// </summary>
        public override Type OMSObjectType
        {
            get
            {
                return typeof(Associate);
            }
        }


        public override string SearchListName
        {
            get
            {
                return "ADMASSOCTYPE";
            }
        }

        /// <summary>
        /// Gets the code lookup group for the oms type so that the cultured names
        /// of the OMS types can be stored under their own categories.
        /// </summary>
        public override string CodeLookupGroup
        {
            get
            {
                return "SUBASSOC";
            }
        }

        /// <summary>
        /// Gets the table name which is used to update from the database.
        /// </summary>
        protected override string TableName
        {
            get
            {
                return "dbAssociateType";
            }
        }

        /// <summary>
        /// Gets the procedure name to execute to get the type information.
        /// </summary>
        protected override string Procedure
        {
            get
            {
                return "sprAssociateType";
            }
        }

        /// <summary>
        /// Gets the caceh folder name.
        /// </summary>
        protected override string CacheFolder
        {
            get
            {
                return "AssociateTypes";
            }
        }

        /// <summary>
        /// Gets an object of the derived type.
        /// </summary>
        /// <param name="id">A unique identifer for the object.</param>
        /// <returns>An IOMSType compaitble object.</returns>
        public override IOMSType GetObject(object id)
        {
            return Associate.GetAssociate((long)id);
        }

        /// <summary>
        /// Creates an instance of the IOMSType.
        /// </summary>
        /// <param name="parameters">Specifies the parameters needed to construct the object.</param>
        /// <returns>An IOMSType object.</returns>
        public override IOMSType CreateObject(object[] parameters)
        {
            if (parameters.Length >= 2)
            {
                if (parameters[0] is Contact)
                {
                    if (parameters[1] is OMSFile)
                    {
                        return new Associate((Contact)parameters[0], (OMSFile)parameters[1], this);
                    }
                }
            }

            throw new ArgumentException("There must be at least two arguments, Contact and File");
        }

        #endregion

        /// <summary>
        /// Gets or Sets the external account code of the associate type.
        /// </summary>
        [System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.Elite3ELookupEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
        [Parameter("CftRole")]
        public string AccountCode
        {
            get
            {
                return Convert.ToString(GetExtraInfo("typeAccCode"));
            }
            set
            {
                SetExtraInfo("typeAccCode", value);
            }
        }

        /// <summary>
        /// Gets or Sets the external account relationship code of the associate type.
        /// </summary>
        [System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.Elite3ELookupEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
        [Parameter("CftRelationshipCode")]
        public string AccountRelCode
        {
            get
            {
                return Convert.ToString(GetExtraInfo("typeAccRelationshipCode"));
            }
            set
            {
                SetExtraInfo("typeAccRelationshipCode", value);
            }
        }

        /// <summary>
        /// OMSExport DACPAC adds typeAccCode and typeAccRelationshipCode columns to dbAssociateType table.
        /// Hide corresponding properties in PropertyGrid if these columns don't exist.
        /// </summary>
        protected override PropertyDescriptorCollection FilterProperties(System.Collections.Generic.List<PropertyDescriptor> props)
        {
            DataColumnCollection columns = GetDataTable().Columns;
            var properties = new System.Collections.Generic.Dictionary<string, string>
            {
                { "typeAccCode", "AccountCode" },
                { "typeAccRelationshipCode", "AccountRelCode" }
            };
            foreach (var kvp in properties)
            {
                if (!columns.Contains(kvp.Key))
                {
                    props.RemoveAll(p => p.Name == kvp.Value);
                }
            }
            return base.FilterProperties(props);
        }

        #region Static Methods

        /// <summary>
        /// Gets a specific associate type.
        /// <param name="type">Associate type code.</param>
        /// </summary>
        /// <returns>An associate type object.</returns>
        public static AssociateType GetAssociateType(string type)
        {
            Session.CurrentSession.CheckLoggedIn();

            AssociateType at = Session.CurrentSession.CurrentAssociateTypes[type] as AssociateType;

            if (at == null)
            {
                at = new AssociateType(type);
            }
            return at;
        }

        #endregion


    }
}
