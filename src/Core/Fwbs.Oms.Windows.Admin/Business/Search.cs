using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;
using FWBS.Common;
using FWBS.OMS.Data;
using FWBS.OMS.Script;
using FWBS.OMS.SearchEngine;
using FWBS.OMS.SourceEngine;
using FWBS.OMS.UI.Windows.Admin;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// 30000 Search List Business Object for the Admin Kit
    /// </summary>
    public class SearchListEditor : SearchList
	{
        public event EventHandler CodeChanged;

        public override bool InDesignMode
        {
            get
            {
                return true;
            }
        }

        protected override void RaiseDesignModeException(Exception ex)
        {
            ErrorBox.Show(ex);
        }

        protected override void SetParameter(string name, out object value)
        {
            base.SetParameter(name, out value);
            if ("(DBNULL)".Equals(value))
                value = DBNull.Value;
            else if ("(NULL)".Equals(value))
                value = null;
        }

        #region Constructors
		internal SearchListEditor(SearchListEditor Clone) : base()
		{
            _searchList.Tables[Table].Rows[0].ItemArray = Clone._searchList.Tables[Table].Rows[0].ItemArray;
			SetExtraInfo("schCode", DBNull.Value);



            _db.ResetFields();
			_db.Call ="";
			_db.Parameters.Clear();
			_db.ParametersInx.Clear();
			GetSearchListDesign(Code);

            string question = Session.CurrentSession.Resources.GetMessage("SPECIFYCODE", "Please specify a new Code for this New Search List?", "").Text;
            string title = Session.CurrentSession.Resources.GetResource("CLONE", "Clone", "").Text;
            this.Code = InputBox.Show(question, title, string.Empty, 15);
            if (this.Code == InputBox.CancelText) throw new Exception(Session.CurrentSession.Resources.GetMessage("CANCELEXC", "Cancel", "").Text);
            this.Description = Clone.Description;

            if (this.Script.Code != "")
            {
                FWBS.OMS.UI.Windows.MessageBox show = new FWBS.OMS.UI.Windows.MessageBox(
                    Session.CurrentSession.Resources.GetResource(
                        "SCRIPTEXISTS",
                        "This Search List has a Script. What would you like to do?",
                        ""));
                string new_script = Session.CurrentSession.Resources.GetResource("NEWSCRIPT", "New Script", "").Text;
                string copy_script = Session.CurrentSession.Resources.GetResource("COPYSCRIPT", "Copy Script", "").Text;
                show.Buttons = new string[2] { new_script, copy_script };
                show.Caption = Session.CurrentSession.Resources.GetResource("OMS", "OMS", "").Text;
                show.Icon = MessageBox.MessageBoxIconGear;
                string ret = show.Show();
                if (ret == new_script)
                {
                    this.NewScript();
                    this.ScriptName = "";
                }
                else if(ret == copy_script)
                { 
                    string scriptname = FWBS.OMS.Script.ScriptGen.GenerateUniqueName(this.Code);
                    this.CopyScript(scriptname);
                    this.ScriptName = scriptname;
                }
            } 
            RefreshSearchControl();			
		}

        public void CopyScript(string NewName)
        {
            if (ScriptGen.Exists(NewName))
                ScriptGen.Delete(NewName);
            _script = ScriptGen.GetScript(Convert.ToString(GetExtraInfo("schScript")));
            string oldcode = this.Script.Code;
            _script = _script.Clone();
            _script.Code = NewName;
            if (_script.AdvancedScript)
                _script.RenameClass(oldcode, NewName);
            _script.Update();
            _script = ScriptGen.GetScript(NewName);
        }
			
		public SearchListEditor() : base()
		{
			_db.ResetFields();
			_db.Call ="";
			_db.Parameters.Clear();
			_db.ParametersInx.Clear();
			GetSearchListDesign(Code);
			RefreshSearchControl();
		}
			
			
		public SearchListEditor(string Code) : base(Code,null,new FWBS.Common.KeyValueCollection())
		{
			GetSearchListDesign(Code);
			RefreshSearchControl();
            IfRequiresParentTypeReOpen(Code);
        }

		public SearchListEditor(string Code, bool Clone) : base(Code,null,new FWBS.Common.KeyValueCollection())
		{
			GetSearchListDesign(Code);
			RefreshSearchControl();
            IfRequiresParentTypeReOpen(Code);
		}
		#endregion

		#region Static
		public static SearchListEditor Clone(string type)
		{
			SearchListEditor o = new SearchListEditor(type);
			return new SearchListEditor(o);
		}
		#endregion

		#region Editor Fields
		private DataBuilder _db = new DataBuilder();
		private ColumnsCollection _columns = new ColumnsCollection();
		private ReturnFieldsCollection _returnfields = new ReturnFieldsCollection();
		private ButtonsCollection _buttons = new ButtonsCollection();
		private KeyValueCollection _testparams = new KeyValueCollection();
		private string _description;
		private string _orgcode;
        private List<string> _colindex = new List<string>();

		private Int64 _version = 0;
		private string _SearchType = "";
		private SearchListStyle _style = SearchListStyle.Search;
        private bool _actionButton = false;
        private bool _pagination = false;
        private SaveSearchType _saveSearch = SaveSearchType.Never;

		private string _enquiryform = "";
        private string _helpFileName = "";
        private string _helpKeyword = "";
        private bool _msapiExclude = true;
		private int _seclevel = 0;
		private bool _active = true;
		private XmlDocument _xmlDParams;
		private XmlDocument _xmlDListViews;
		private XmlDocument _xmlDReturnFields;
		private XmlNode _xmlButton;
		private XmlNode _xmlColumn;
		private XmlNode _xmlParameter;
		private XmlNode _xmlReturnField;
		private XmlNode _xmlsearchList;
		private CodeLookupDisplay _captiontext = new CodeLookupDisplay("SLVIEW");
		private CodeLookupDisplay _schgroup = new CodeLookupDisplay("SLGROUP");

		private ImageList _imgList = null;
		private string _imageColumn = "";
		private omsImageLists _omsimagelists = omsImageLists.None;
		private int _imageindex =-1;
		private int _rowheight = 1;

		#endregion

		#region Editor Private

		private void ClearColumns()
		{
            _colindex.Clear();
		}

		private void AddColumns(int index, object value)
		{
			Columns col = (Columns)value;
            if (_colindex.Contains(col.MappingName) == false)
                _colindex.Add(col.MappingName);
            else
                _columns.Remove(col);
		}

		private void AddRet(int index, object value)
		{
			ReturnFields ret = (ReturnFields)value;
            if (_returnfields.IndexOf(ret) > -1)
				_returnfields.Remove(ret);
		}

		private void ClearPanels()
		{
            _colindex.Clear();
		}

		private void AddPanels(int index, object value)
		{
            Columns col = (Columns)value;
            if (_colindex.Contains(col.MappingName) == false)
                _colindex.Add(col.MappingName);
            else
                _columns.Remove(col);
		}

		private void IfRequiresParentTypeReOpen(string Code)
		{
            try
            {
                FWBS.Common.KeyValueCollection ar = new FWBS.Common.KeyValueCollection();
                foreach (XmlNode dr in _xmlParameter.ChildNodes)
                {
                    string __name = "";
                    string __value = "";
                    string __test = "";
                    try { __test = dr.SelectSingleNode("@test").Value; }
                    catch { }
                    try { __name = dr.SelectSingleNode("@name").Value; }
                    catch { }
                    __value = dr.InnerText;
                    if (__test != "")
                        ar.Add(__value.Replace("%", ""), __test);
                }

                if (this.ParentTypeRequired.ToUpper() == "FWBS.OMS.CLIENT")
                {
                    if (Session.CurrentSession.CurrentClient == null)
                        FWBS.OMS.UI.Windows.Services.SelectClient();
                    this.ChangeParent(Session.CurrentSession.CurrentClient);
                    _db.ChangeParent(Session.CurrentSession.CurrentClient);
                    base.SearchListInternal(Code, Session.CurrentSession.CurrentClient, ar);
                }
                else if (this.ParentTypeRequired.ToUpper() == "FWBS.OMS.OMSFILE")
                {
                    if (Session.CurrentSession.CurrentFile == null)
                        FWBS.OMS.UI.Windows.Services.SelectFile();
                    this.ChangeParent(Session.CurrentSession.CurrentFile);
                    _db.ChangeParent(Session.CurrentSession.CurrentFile);
                    base.SearchListInternal(Code, Session.CurrentSession.CurrentFile, ar);
                }
                else if (this.ParentTypeRequired.ToUpper() == "FWBS.OMS.ASSOCIATE")
                {
                    if (Session.CurrentSession.CurrentAssociate == null)
                        FWBS.OMS.UI.Windows.Services.SelectAssociate();
                    this.ChangeParent(Session.CurrentSession.CurrentAssociate);
                    _db.ChangeParent(Session.CurrentSession.CurrentAssociate);
                    base.SearchListInternal(Code, Session.CurrentSession.CurrentAssociate, ar);
                }
            }
            catch
            {
            }
		}

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		private object GetEditExtraInfo(string fieldName)
		{
			return GetExtraInfo(fieldName);
		}
		
		private void WriteAttribute(XmlNode Node, string Name, string Value)
		{
			if (Node.SelectSingleNode("@" + Name) != null)
                Node.SelectSingleNode("@" + Name).Value = Value;
            else
                Node.Attributes.Append(CreateAttribute(Node, Name, Value));
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
				WriteAttribute(Node,Name,Value);
				return Value;
			}

		}


		private void GetSearchListDesign(string Code)
		{
			try
			{
				_code = Convert.ToString(GetEditExtraInfo("schCode"));
				_style = (SearchListStyle)Enum.ToObject(typeof(SearchListStyle),Convert.ToInt64(GetEditExtraInfo("schStyle")));
                _actionButton = Convert.ToBoolean(GetNullableInfo("schActionButton"));
                _pagination = Convert.ToBoolean(GetNullableInfo("schPagination"));
                _SearchType = Convert.ToString(GetEditExtraInfo("schType"));
				_version = Convert.ToInt64(GetEditExtraInfo("schVersion"));
				_enquiryform = Convert.ToString(GetEditExtraInfo("schEnquiry"));
				base.SourceType = (SourceType)FWBS.Common.ConvertDef.ToEnum(Convert.ToString(GetEditExtraInfo("schSourceType")),SourceType.OMS);
				base.Src = Convert.ToString(GetEditExtraInfo("schSource"));
				base.Call = Convert.ToString(GetEditExtraInfo("schSourceCall"));
				_schgroup.Code = Convert.ToString(GetEditExtraInfo("schGroup"));

				_seclevel = Convert.ToInt32(GetEditExtraInfo("schSecurityLevel"));
				_active = Convert.ToBoolean(GetEditExtraInfo("schActive"));
                _msapiExclude = Convert.ToBoolean(GetEditExtraInfo("schApiExclude"));

				//
				// XML Parameters
				//
				_xmlDParams = new XmlDocument();
				_xmlDParams.PreserveWhitespace = false; 
				_xmlDParams.LoadXml(Convert.ToString(GetEditExtraInfo("schSourceParameters")));
				_xmlParameter = _xmlDParams.DocumentElement.SelectSingleNode("/params");
				base.ParentTypeRequired = GetAttribute(_xmlParameter,"parentRequired","");

				//
				// XML Search List Main Heading
				//
				_xmlDListViews = new XmlDocument();
				_xmlDListViews.PreserveWhitespace = false;
				_xmlDListViews.LoadXml(Convert.ToString(GetEditExtraInfo("schListView")));

				//
				// XML If the Base Element searchList does not exist create it.
				//
				_xmlsearchList = _xmlDListViews.DocumentElement.SelectSingleNode("/searchList");
				if (_xmlsearchList == null)
				{
					_xmlsearchList = _xmlDListViews.CreateElement("","searchList","");
					_xmlDListViews.AppendChild(_xmlsearchList);
				}

				//
				// XML Columns
				//
				_xmlColumn = _xmlDListViews.DocumentElement.SelectSingleNode("/searchList/listView");
				if (_xmlColumn == null)
				{
					_xmlColumn = _xmlDListViews.CreateElement("","listView","");
					_xmlsearchList.AppendChild(_xmlColumn);
				}
				_captiontext.Code = GetAttribute(_xmlColumn,"captionLookup","RECCOUNT");
				_imageindex = Convert.ToInt32(GetAttribute(_xmlColumn,"imageIndex","-1"));
				_imageColumn = GetAttribute(_xmlColumn,"imageColumn","");
                _rowheight = FWBS.Common.ConvertDef.ToInt32(GetAttribute(_xmlColumn, "rowHeight", "1"),1);
                ImageResources = (omsImageLists)FWBS.Common.ConvertDef.ToEnum(GetAttribute(_xmlColumn, "imageResouce", "None"), omsImageLists.None);
                _helpFileName = Convert.ToString(GetAttribute(_xmlColumn, "helpFileName", ""));
                _helpKeyword = Convert.ToString(GetAttribute(_xmlColumn, "helpKeyword", ""));
                
                _saveSearch = (SaveSearchType)FWBS.Common.ConvertDef.ToEnum( GetAttribute(_xmlColumn, "saveSearch", "Never"), SaveSearchType.Never);

				//
				// XML Buttons
				//
				_xmlButton = _xmlDListViews.DocumentElement.SelectSingleNode("/searchList/buttons");
				if (_xmlButton == null)
				{
					_xmlButton = _xmlDListViews.CreateElement("","buttons","");
					_xmlsearchList.AppendChild(_xmlButton);
				}

				//
				// XML Return Fields
				//
				_xmlDReturnFields = new XmlDocument();
				_xmlDReturnFields.PreserveWhitespace = false;
				_xmlDReturnFields.LoadXml(Convert.ToString(GetEditExtraInfo("schReturnField")));

				_xmlReturnField = _xmlDReturnFields.DocumentElement.SelectSingleNode("/fields");
				if (_xmlReturnField == null)
				{
					_xmlReturnField = _xmlDReturnFields.CreateElement("","fields","");
					_xmlDReturnFields.AppendChild(_xmlReturnField);
				}

				_columns.Inserted += new Crownwood.Magic.Collections.CollectionChange(AddColumns);
				_columns.Cleared += new Crownwood.Magic.Collections.CollectionClear(ClearColumns);
				_returnfields.Inserting += new Crownwood.Magic.Collections.CollectionChange(AddRet);
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ex);
			}

		}

        private object GetNullableInfo(string fieldName)
        {
            var info = GetEditExtraInfo(fieldName);
            return !Convert.IsDBNull(info) ? info : null; 
        }

        private string GetValue(XmlNode node, string name)
        {
            var xnode = node.SelectSingleNode(name);
            if (xnode != null)
                return xnode.Value;
            else
                return null;
        }

		private void RefreshSearchControl()
		{
			_orgcode = _code;
			_db.Parameters.Clear();
			_db.ParametersInx.Clear();
			_db.ResetFields();
			_testparams.Clear();
			foreach(XmlNode dr in _xmlParameter.ChildNodes)
			{
				string __name = GetValue(dr,"@name");
                string __type = GetValue(dr, "@type");
                string __test = GetValue(dr, "@test");
                string __kind = GetValue(dr, "@kind");
                string __value = dr.InnerText;
                _db.Parameters.Add(new Parameter(_db, __name, __type, __value, __test, (FWBS.OMS.SearchEngine.SearchParameterDateIs)FWBS.Common.ConvertDef.ToEnum(__kind, FWBS.OMS.SearchEngine.SearchParameterDateIs.NotApplicable), dr.Clone()));
				if (__test != "" && __value != "")
					_testparams.Add(__value.Replace("%",""),__test);
			}
			_db.Call = base.Call;
			_db.Source = base.Src;
			_db.SourceType = base.SourceType;
			_db.EnquiryForm = EnquiryForm;
			_db.ChangeParameters(_testparams); // DataBuilder Key Pairs
			base.ChangeParameters(_testparams); // SearchLists Key Pairs

			_columns.Clear();
			foreach(XmlNode dr in _xmlColumn.ChildNodes)
			{
				string __lookup = "";
				string __mappingname = "";
				int __width = 100;
				bool __incquicksearch = true;
				string __nulltext = "";
				string __datalistname = "";
				string __datacodetype = "";
                bool __sortable = true;
                SearchColumnsFormat __format = SearchColumnsFormat.Standard;
				string __conditions = "";
				string __roles = "";
                SearchColumnsDateIs __sourceis = SearchColumnsDateIs.NotApplicable;
                SearchColumnsDateIs __displayas = SearchColumnsDateIs.NotApplicable;

				__lookup = GetValue(dr,"@lookup");
				__mappingname = GetValue(dr,"@mappingName");
				__width = Convert.ToInt32(GetValue(dr,"@width"));
                __sortable = Convert.ToBoolean(GetAttribute(dr, "sortable", true.ToString()));
                __format = (SearchColumnsFormat)FWBS.Common.ConvertDef.ToEnum(GetValue(dr, "@format"), SearchColumnsFormat.Standard);
                __incquicksearch = FWBS.Common.ConvertDef.ToBoolean(GetValue(dr,"@incQuickSearch"),true);
				__nulltext = GetValue(dr,"@nullText");
				__datalistname = GetValue(dr,"@dataListName");
				__datacodetype = GetValue(dr,"@dataCodeType");
				__roles = GetValue(dr,"@roles");
				__conditions = GetValue(dr,"@conditions");
                __displayas = (SearchColumnsDateIs)FWBS.Common.ConvertDef.ToEnum(GetValue(dr,"@displayAs"), SearchColumnsDateIs.NotApplicable);
                __sourceis = (SearchColumnsDateIs)FWBS.Common.ConvertDef.ToEnum(GetValue(dr,"@sourceIs"), SearchColumnsDateIs.NotApplicable);
                _columns.Add(new Columns(this, __lookup, __mappingname, __width, __sortable, __format, __incquicksearch, __nulltext, __datalistname, __datacodetype, __conditions, __roles, dr.Clone(),__sourceis,__displayas));
			}

			_buttons.Clear();
			foreach(XmlNode dr in _xmlButton.ChildNodes)
			{
				string __name = GetValue(dr,"@name");
				string __codelookup = GetValue(dr,"@lookup");
				bool __visible = ConvertDef.ToBoolean(GetValue(dr,"@visible"),true);
				ButtonActions __action = (ButtonActions)FWBS.Common.ConvertDef.ToEnum(GetValue(dr,"@mode"),ButtonActions.None);
				string __parameter = GetValue(dr,"@parameter");
				ButtonStyle __style = (ButtonStyle)ConvertDef.ToEnum(GetValue(dr,"@buttonStyle"),ButtonStyle.Plain);
				int __buttonglyph = ConvertDef.ToInt32(GetValue(dr,"@buttonGlyph"),-1);
				bool __pnlbtnvisible = ConvertDef.ToBoolean(GetValue(dr,"@pnlBtnVisible"),false);
				bool __contextmenuvisible = ConvertDef.ToBoolean(GetValue(dr,"@contextMenuVisible"),false);
				string __pnlbtntext = GetValue(dr,"@pnlBtnText");
                string __parent = GetValue(dr,"@parent");
				int __pnlbtnindex = ConvertDef.ToInt32(GetValue(dr,"@pnlBtnIndex"),-1); 
				bool __enabledwithnorows = ConvertDef.ToBoolean(GetValue(dr,"@enabledWithNoRows"),true);
				bool __enabledwhenmultiselect = ConvertDef.ToBoolean(GetValue(dr,"@enabledMultiSelect"),false);
				string __conditions = GetValue(dr,"@conditions");
				string __roles = GetValue(dr,"@roles");

                Buttons btn = new Buttons(this,__style,__buttonglyph,__parameter,__action,__name,__codelookup,__visible,__pnlbtnvisible,__pnlbtntext,__pnlbtnindex,__contextmenuvisible,__enabledwithnorows,__enabledwhenmultiselect, __conditions, __roles, dr.Clone());
                btn.ParentCode = __parent;
                                
                
                
                
                _buttons.Add(btn);
			}

			_returnfields.Clear();
			foreach(XmlNode dr in _xmlReturnField.ChildNodes)
			{
				string __mappingname = "";
				try{__mappingname = dr.InnerText;}
				catch{}
				_returnfields.Add(new ReturnFields(this,__mappingname, dr.Clone()));
			}	

			DataTable dt = FWBS.OMS.CodeLookup.GetLookups("OMSSEARCH", _code);
			if (dt.Rows.Count > 0)
			{
				_description = Convert.ToString(dt.Rows[0]["cddesc"]);
			}
		}
		#endregion

		#region Editor Properties
		/// <summary>
		/// Gets or Sets the bound image list associated with the column.
		/// </summary>
		[Browsable(false)]
		public ImageList ImageList
		{
			get
			{
				return _imgList;
			}
			set
			{
				_imgList = value;
			}
		}


		[Category("Appearance")]
		[DefaultValue(omsImageLists.None)]
		[RefreshProperties(RefreshProperties.All)]
		public omsImageLists ImageResources
		{
			get
			{
				return _omsimagelists;
			}
			set
			{
				if (_omsimagelists != value)
				{
					switch (value)
					{
						case omsImageLists.AdminMenu16:
						{
							ImageList = Images.AdminMenu16();
							break;
						}
						case omsImageLists.AdminMenu32:
						{
							ImageList = Images.AdminMenu32();
							break;
						}
						case omsImageLists.Arrows:
						{
							ImageList = Images.Arrows;
							break;
						}
                        case omsImageLists.PlusMinus:
                        {
                            ImageList = Images.PlusMinus;
                            break;
                        }
						case omsImageLists.CoolButtons16:
						{
							ImageList = Images.CoolButtons16();
							break;
						}
						case omsImageLists.CoolButtons24:
						{
                            ImageList = Images.GetCoolButtons24();
							break;
						}
						case omsImageLists.Developments16:
						{
							ImageList = Images.Developments();
							break;
						}
						case omsImageLists.Entities16:
						{
							ImageList = Images.Entities();
							break;
						}
						case omsImageLists.Entities32:
						{
							ImageList = Images.Entities32();
							break;
						}
						case omsImageLists.imgFolderForms16:
						{
                            ImageList = Images.GetFolderFormsIcons(Images.IconSize.Size16);
							break;
						}
						case omsImageLists.imgFolderForms32:
						{
                            ImageList = Images.GetFolderFormsIcons(Images.IconSize.Size32);
							break;
						}
						case omsImageLists.None:
						{
							ImageList = null;
							break;
						}
					}
				}
				_omsimagelists = value;
			}
		}


		[Category("Appearance")]
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor(typeof(IconDisplayEditor),typeof(UITypeEditor))]
		[DefaultValue(null)]
		public int ImageIndex
		{
			get
			{
				return _imageindex;
			}
			set
			{
				_imageindex = value;
			}
		}
		
		/// <summary>
		/// Gets or Sets the bound image column to map to an image index.
		/// </summary>
		[Category("Appearance")]
		[TypeConverter(typeof(FieldMappingTypeEditor))]
		public string ImageColumn
		{
			get
			{
				return _imageColumn;
			}
			set
			{
				_imageColumn = value;
			}
		}


		[TypeConverter(typeof(ScriptLister))]
		[ScriptTypeParam("SEARCHLIST")]
		public override string ScriptName
		{
			get
			{
				return base.ScriptName;
			}
			set
			{
				base.ScriptName = value;
			}
		}

		[LocCategory("(Details)")]
		[CodeLookupUIAttribute(CodeLookupUIAttributes.ChangeCode)]
		[CodeLookupSelectorTitle("GROUPS","Groups")]
		public CodeLookupDisplay Group
		{
			get
			{
				return _schgroup;
			}
			set
			{
				_schgroup = value;
				SetExtraInfo("schGroup",value.Code);
			}
		}

		[LocCategory("Design")]
		[Browsable(true)]
		[Description("Sets the Double Click Action of the List Grid")]
		[Lookup("DoubleClickActi")]
		[TypeConverter(typeof(SearchListButtonLister))]
		public override string DoubleClickAction
		{
			get
			{
				return GetAttribute(_xmlColumn,"doubleClickAction","None");
			}
			set
			{
				WriteAttribute(_xmlColumn,"doubleClickAction",value);
			}
		}


		[LocCategory("Caption")]
		[Browsable(true)]
		[Description("Show or Hides the Search List Caption Bar")]
		public override bool CaptionVisible
		{
			get
			{
				return Convert.ToBoolean(GetAttribute(_xmlColumn,"captionVisible",true.ToString()));
			}
			set
			{
				WriteAttribute(_xmlColumn,"captionVisible",value.ToString());
			}
		}
		
		[LocCategory("Design")]
		[Browsable(true)]
		[Description("Show or Hides the Quick Search Bar")]
		public override bool QuickSearch
		{
			get
			{
				return Convert.ToBoolean(GetAttribute(_xmlColumn,"quickSearchVisible",true.ToString()));
			}
			set
			{
				WriteAttribute(_xmlColumn,"quickSearchVisible",value.ToString());
			}
		}

		[LocCategory("Design")]
		[Browsable(true)]
		[Description("Enables Multiple Selection.")]
		public override bool MultiSelect
		{
			get
			{
				return Convert.ToBoolean(GetAttribute(_xmlColumn,"multiSelect",false.ToString()));
			}
			set
			{
				WriteAttribute(_xmlColumn,"multiSelect",value.ToString());
			}
		}

		[LocCategory("Design")]
		[Browsable(true)]
		[Lookup("QUICKSEARCHPRE")]
		[Description("Sets the Quick Search Prefix hidden from the user e.g. %")]
		public override string QuickSearchPrefix
		{
			get
			{
				return Convert.ToString(GetAttribute(_xmlColumn,"quickSearchPrefix","%"));
			}
			set
			{
				WriteAttribute(_xmlColumn,"quickSearchPrefix",value);
			}
		}

		[LocCategory("Data")]
		[Description("Set the Parent Type Required by this search list")]
		[Editor(typeof(ParentLookupEditor),typeof(UITypeEditor))]
		[Browsable(true)]
		public override string ParentTypeRequired
		{
			get
			{
				return base.ParentTypeRequired;
			}
			set
			{
				base.ParentTypeRequired = value;
			}
		}

		[LocCategory("SEARCHGRP")]
		[Browsable(true)]
		[Lookup("SEARCHTYPEVS")]
		[Description("Show the Search Type Selector if available or Hides the Search Type Selector")]
		public bool SearchTypeVisble
		{
			get
			{
				return Convert.ToBoolean(GetAttribute(_xmlColumn,"searchTypeVisible",false.ToString()));
			}
			set
			{
				WriteAttribute(_xmlColumn,"searchTypeVisible",value.ToString());
			}
		}
		
		[LocCategory("Caption")]
		[Description("Change the Search List Caption Bar Text")]
		[CodeLookupSelectorTitle("CAPTIONS","Caption Texts")]
		public CodeLookupDisplay CaptionText
		{
			get
			{
				return _captiontext;
				
			}
			set
			{
				_captiontext = value;
			}
		}

		[LocCategory("Style")]
		[Description("List or Search Sytle")]
		public SearchListStyle ViewStyle
		{
			get
			{
				return _style;
			}
			set
			{
				_style = value;
			}
		}

        [LocCategory("Style")]
        [Description("Action Button option")]
        [Lookup("SCHACTIONBUT")]
        public bool ActionButton
        {
            get
            {
                return _actionButton;
            }
            set
            {
                _actionButton = value;
            }
        }

        [LocCategory("Style")]
        [Description("Pagination option")]
        [Lookup("SCHPAGINATION")]
        public bool Paginataion
        {
            get
            {
                return _pagination;
            }
            set
            {
                _pagination = value;
            }
        }

        [LocCategory("Design")]
        [Description("Save Search Option")]
        public SaveSearchType SaveSearch
        {
            get
            {
                return _saveSearch;
            }
            set
            {
                _saveSearch = value;
            }
        }

   


		[LocCategory("Design")]
		[Description("Enquiry Form used for the Search Criteria")]
		[Editor(typeof(EnquiryFormEditor),typeof(UITypeEditor))]
		public string EnquiryForm
		{
			get
			{
				return _enquiryform;
			}
			set
			{
				_enquiryform = value;
				_db.EnquiryForm = value;
			}
		}

        [LocCategory("Help")]
        [Browsable(true)]
        [Description("HelpFileName")]
        public override string HelpFileName
        {
            get
            {
                return _helpFileName;
            }
            set
            {
                _helpFileName= value;
            }
        }
        [LocCategory("Help")]
        [Browsable(true)]
        [Description("HelpKeyword")]
        public override string HelpKeyword
        {
            get
            {
                return _helpKeyword;
            }
            set
            {
                _helpKeyword = value;
            }
        }

        [LocCategory("MSAPI")]
        [Browsable(true)]
        [Description("MSAPIEXCLUDE")]
        public bool MSApiExclude
        {
            get
            {
                return _msapiExclude;
            }
            set
            {
                _msapiExclude = value;
            }
        }


        /// <summary>
        /// Gets the search list style type.
        /// </summary>
        [LocCategory("Style")]
        [Browsable(true)]
        public new int RowHeight
        {
            get
            {
                return _rowheight;
            }
            set
            {
                if (value < 1)
                    _rowheight = 1;
                else if (value > 10)
                    _rowheight = 10;
                else
                    _rowheight = value;
            }
        }
		
		[LocCategory("SEARCHGRP")]
		[Description("SearchType allow you to create multiple search or listing object for the same Data")]
		[TypeConverter(typeof(SearchTypeEditor))]
		[Lookup("SEARCHTYPECD")]
		public string SearchType
		{
			get
			{
				return _SearchType;
			}
			set
			{
                if (value.Length > 15)
                    throw new Exception(Session.CurrentSession.Resources.GetMessage("CODEGR15", "Group Code cannot be greater than 15 characters", "").Text);
                _SearchType = value;
				RefreshSearchControl();
			}
		}

		[Description("Version"),LocCategory("Design")]
		[Browsable(true)]
		public override int Version
		{
			get
			{
				return base.Version;
			}
		}
		
		[Description("Is the Search List Active"),LocCategory("Security")]
		public bool Active
		{
			get
			{
				return _active;
			}
			set
			{
				_active = value;
			}
		}

		[LocCategory("(Details)")]
		[Browsable(true)]
		[Description("Unique Code used to identify the Search List")]
		[RefreshProperties(RefreshProperties.All)]
		public override string Code
		{
			get{return _code;}
			set
			{
				if (FWBS.OMS.SearchEngine.SearchList.Exists(value))
				{
                    if (value != _orgcode)
                        MessageBox.Show(
                            Session.CurrentSession.Resources.GetResource(
                                "CODEEXISTS",
                                "The Search List Code [''%1%''] already exists..",
                                "",
                                value).Text,
                            ResourceLookup.GetLookupText("OMSAdmin"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                    else
					{
						Description = CodeLookup.GetLookup("OMSSEARCH",value);
						base._code = value; 
					}
				}
				else
				{
					if (IsNew)
					{
						SetExtraInfo("schCode", value);
						base._code = value;
					}
					else
						throw new OMSException2("30000","The Code cannot be changed when set");
				}
                OnCodeChanged();
			}
		}

        private void OnCodeChanged()
        {
            if (CodeChanged != null)
                CodeChanged(this, EventArgs.Empty);
        }


		[LocCategory("(Details)")]
		[Browsable(true)]
		[Description("Description used to identify the Search List")]
		public override string Description
		{
			get
			{
				return _description;
			}
			set
			{
				if (_code == "")
				{
					throw new SearchException(HelpIndexes.SearchNoCode);
				}
				else
				{
					_description = value;
					FWBS.OMS.CodeLookup.Create("OMSSEARCH",_code,value,"",CodeLookup.DefaultCulture,true,true,true);
				}
			}
		}

		
		[LocCategory("Design")]
		[Description("Add, Modify Columns to be show in the Data Grid")]
		[Editor(typeof(ColumnsEditor),typeof(UITypeEditor))]
		public ColumnsCollection Columns
		{
			get
			{
				return _columns;
			}
			set
			{
				_columns = value;
			}
		}

		[LocCategory("Data")]
		[Description("Add, Modify Return Fields that are returned when an item in the Grid is selected")]
		[Editor(typeof(ReturnsEditor),typeof(UITypeEditor))]
		public new ReturnFieldsCollection ReturnFields
		{
			get
			{
				return _returnfields;
			}
			set
			{
				_returnfields = value;
			}
		}

		[LocCategory("CUSBUTTON")]
		[Description("Add / Modify Custom Buttons for the Searh List")]
		[Editor(typeof(ButtonsEditor),typeof(UITypeEditor))]
		public ButtonsCollection Buttons
		{
			get
			{
				return _buttons;
			}
			set
			{
				_buttons = value;
			}
		}

		[Description("Commands to build a Data List"), LocCategory("Data")]
		public DataBuilder DataBuilder
		{
			get
			{
				return _db;
			}
			set
			{
				_db = value;
				for(int ui = _xmlParameter.ChildNodes.Count-1; ui > -1; ui--)
				{
					XmlNode dr = _xmlParameter.ChildNodes[ui];
					_xmlParameter.RemoveChild(dr);
				}	
				base.Src = _db.Source;
				base.SourceType = _db.SourceType;
				base.Call = _db.Call;
				base.ReplacementParameters.Clear();
				foreach (Parameter p in _db.Parameters)
				{
					if (p.Node != null)
					{
						WriteAttribute(p.Node,"name",p.SQLParameter);
						WriteAttribute(p.Node,"type",p.FieldType.ToString());
						WriteAttribute(p.Node,"test",p.TestValue);
						WriteAttribute(p.Node,"kind",p.ParameterDateIs.ToString());
						p.Node.InnerText = p.BoundValue;
						_xmlParameter.AppendChild(p.Node);
					}
					else
					{
						XmlNode _newnode = _xmlParameter.OwnerDocument.CreateNode(XmlNodeType.Element,"param","");
						_newnode.InnerText = p.BoundValue;
						WriteAttribute(_newnode,"name",p.SQLParameter);
						WriteAttribute(_newnode,"type",p.FieldType.ToString());
						WriteAttribute(_newnode,"test",p.TestValue);
                        WriteAttribute(_newnode, "kind", p.ParameterDateIs.ToString());
                        _xmlParameter.AppendChild(_newnode);
					}
					if (p.TestValue != "" && p.BoundValue != "")
						base.ReplacementParameters.Add(p.BoundValue.Replace("%",""),p.TestValue);
				}
			}
		}
		#endregion

		#region Editor Methods
		public override void Update()
		{
            WriteAttribute(_xmlColumn, "rowHeight", _rowheight.ToString());
            WriteAttribute(_xmlColumn, "imageIndex", _imageindex.ToString());
			WriteAttribute(_xmlColumn,"imageColumn",_imageColumn);
			WriteAttribute(_xmlColumn,"imageResouce",_omsimagelists.ToString());
			WriteAttribute(_xmlColumn,"captionLookup",_captiontext.Code);
			WriteAttribute(_xmlParameter,"parentRequired",base.ParentTypeRequired);
            WriteAttribute(_xmlColumn, "helpFileName", _helpFileName.ToString());
            WriteAttribute(_xmlColumn, "helpKeyword", _helpKeyword.ToString());
            WriteAttribute(_xmlColumn, "saveSearch", _saveSearch.ToString());

			for(int ui = _xmlColumn.ChildNodes.Count-1; ui > -1; ui--)
			{
				XmlNode dr = _xmlColumn.ChildNodes[ui];
				_xmlColumn.RemoveChild(dr);
			}
			foreach (Columns p in _columns)
			{
				if (p.Node != null)
				{
					WriteAttribute(p.Node,"lookup",p.Text.Code);
					WriteAttribute(p.Node,"mappingName",p.MappingName);
					WriteAttribute(p.Node,"width",p.Width.ToString());
                    WriteAttribute(p.Node, "sortable", p.Sortable.ToString());
                    WriteAttribute(p.Node,"format",p.Format.ToString());
					WriteAttribute(p.Node,"incQuickSearch",p.IncQuickSearch.ToString());
					WriteAttribute(p.Node,"nullText",p.NullText.Code);
					WriteAttribute(p.Node,"dataListName",p.DataListName.Code);
					WriteAttribute(p.Node,"dataCodeType",p.CodeTypeName);
					WriteAttribute(p.Node,"conditions",String.Join(Environment.NewLine,p.Conditional));
					WriteAttribute(p.Node,"roles",p.UserRoles.Codes);
                    if (p.SourceDateIs != SearchColumnsDateIs.NotApplicable)
                        WriteAttribute(p.Node,"sourceIs", p.SourceDateIs.ToString());
                    if (p.DisplayDateAs != SearchColumnsDateIs.NotApplicable)
                        WriteAttribute(p.Node, "displayAs", p.DisplayDateAs.ToString());
                    _xmlColumn.AppendChild(p.Node);
				}
				else
				{
					XmlNode _new = _xmlColumn;
					XmlNode _newnode = _new.OwnerDocument.CreateNode(XmlNodeType.Element,"column","");
					WriteAttribute(_newnode,"lookup",p.Text.Code);
					WriteAttribute(_newnode,"mappingName",p.MappingName);
					WriteAttribute(_newnode,"width",p.Width.ToString());
                    WriteAttribute(_newnode, "sortable", p.Sortable.ToString());
                    WriteAttribute(_newnode,"format",p.Format.ToString());
					WriteAttribute(_newnode,"incQuickSearch",p.IncQuickSearch.ToString());
					WriteAttribute(_newnode,"nullText",p.NullText.Code);
					WriteAttribute(_newnode,"dataListName",p.DataListName.Code);
					WriteAttribute(_newnode,"dataCodeType",p.CodeTypeName);
					WriteAttribute(_newnode,"conditions",String.Join(Environment.NewLine,p.Conditional));
					WriteAttribute(_newnode,"roles",p.UserRoles.Codes);
                    if (p.SourceDateIs != SearchColumnsDateIs.NotApplicable)
                        WriteAttribute(_newnode, "sourceIs", p.SourceDateIs.ToString());
                    if (p.DisplayDateAs != SearchColumnsDateIs.NotApplicable)
                        WriteAttribute(_newnode, "displayAs", p.DisplayDateAs.ToString());
                    _xmlColumn.AppendChild(_newnode);
				}
			}

			for(int ui = _xmlReturnField.ChildNodes.Count-1; ui > -1; ui--)
			{
				XmlNode dr = _xmlReturnField.ChildNodes[ui];
				_xmlReturnField.RemoveChild(dr);
			}

			foreach (ReturnFields p in _returnfields)
			{
				if (p.Node != null)
				{
					p.Node.InnerText = p.FieldName;
					_xmlReturnField.AppendChild(p.Node);
				}
				else
				{
					XmlNode _new = _xmlReturnField;
					XmlNode _newnode = _new.OwnerDocument.CreateNode(XmlNodeType.Element,"field","");
					_newnode.InnerText = p.FieldName;
					_new.AppendChild(_newnode);
				}
			}

			for(int ui = _xmlButton.ChildNodes.Count-1; ui > -1; ui--)
			{
				XmlNode dr = _xmlButton.ChildNodes[ui];
				_xmlButton.RemoveChild(dr);
			}

			int inx = 0;
			foreach (FWBS.OMS.UI.Windows.Design.Buttons p in _buttons)
			{
				if (p.Node != null)
				{
					WriteAttribute(p.Node,"id",inx.ToString());
					WriteAttribute(p.Node,"name",p.Name);
					WriteAttribute(p.Node,"lookup",p.Caption.Code);
					WriteAttribute(p.Node,"visible",p.Visible.ToString());			
					WriteAttribute(p.Node,"parameter",p.Parameter.Value);			
					WriteAttribute(p.Node,"mode",p.Action.ToString());			
					WriteAttribute(p.Node,"buttonStyle",p.ButtonStyle.ToString());			
					WriteAttribute(p.Node,"buttonGlyph",p.ButtonGlyph.ToString());			
					WriteAttribute(p.Node,"pnlBtnVisible",p.PanelButtonVisible.ToString());			
					WriteAttribute(p.Node,"pnlBtnText",p.PanelButtonCaption.Code);
                    WriteAttribute(p.Node,"parent", p.ParentCode);			
					WriteAttribute(p.Node,"pnlBtnIndex",p.PanelButtonImageIndex.ToString());			
					WriteAttribute(p.Node,"contextMenuVisible",p.ContextMenuVisible.ToString());			
					WriteAttribute(p.Node,"enabledWithNoRows",p.EnabledWhenNoRows.ToString());	
					WriteAttribute(p.Node,"enabledMultiSelect",p.EnabledWhenMultiSelect.ToString());	
					WriteAttribute(p.Node,"conditions",String.Join(Environment.NewLine,p.Conditional));
					WriteAttribute(p.Node,"roles",p.UserRoles.Codes);
					_xmlButton.AppendChild(p.Node);
				}
				else
				{
					XmlNode _new = _xmlButton;
					XmlNode _newnode = _new.OwnerDocument.CreateNode(XmlNodeType.Element,"button","");
					WriteAttribute(_newnode,"id",inx.ToString());
					WriteAttribute(_newnode,"name",p.Name);
					WriteAttribute(_newnode,"lookup",p.Caption.Code);
					WriteAttribute(_newnode,"visible",p.Visible.ToString());		
					WriteAttribute(_newnode,"parameter",p.Parameter.Value);			
					WriteAttribute(_newnode,"mode",p.Action.ToString());			
					WriteAttribute(_newnode,"buttonStyle",p.ButtonStyle.ToString());			
					WriteAttribute(_newnode,"buttonGlyph",p.ButtonGlyph.ToString());			
					WriteAttribute(_newnode,"pnlBtnVisible",p.PanelButtonVisible.ToString());			
					WriteAttribute(_newnode,"pnlBtnText",p.PanelButtonCaption.Code);			
					WriteAttribute(_newnode,"pnlBtnIndex",p.PanelButtonImageIndex.ToString());
                    WriteAttribute(_newnode, "parent", p.ParentCode);			
                    WriteAttribute(_newnode,"contextMenuVisible",p.ContextMenuVisible.ToString());			
					WriteAttribute(_newnode,"enabledWithNoRows",p.EnabledWhenNoRows.ToString());			
					WriteAttribute(_newnode,"enabledMultiSelect",p.EnabledWhenMultiSelect.ToString());			
					WriteAttribute(_newnode,"conditions",String.Join(Environment.NewLine,p.Conditional));
					WriteAttribute(_newnode,"roles",p.UserRoles.Codes);
					_xmlButton.AppendChild(_newnode);
				}
				inx++;
			}
			if (_xmlColumn == null || Convert.ToString(_xmlColumn.InnerXml) == "")
				throw new SearchException(HelpIndexes.SearchNoColumnsHaveBeenSet);
			if (_code == "")
				throw new SearchException(HelpIndexes.SearchNoCode);
			if (Call == "")
				throw new SearchException(HelpIndexes.SearchDataBuilderIncomplete);

			SetExtraInfo("schCode",_code);
			SetExtraInfo("schStyle",_style);
            SetExtraInfo("schActionButton", _actionButton);
            SetExtraInfo("schPagination", _pagination);
            SetExtraInfo("schType",_SearchType);
			if (_enquiryform == "") 
				SetExtraInfo("schEnquiry",DBNull.Value);
			else
				SetExtraInfo("schEnquiry",_enquiryform);
			SetExtraInfo("schSourceType",base.SourceType.ToString());
			SetExtraInfo("schSource",base.Src);
			SetExtraInfo("schSourceCall",base.Call);
			SetExtraInfo("schSecurityLevel",_seclevel);
			SetExtraInfo("schActive",_active);

			SetExtraInfo("schVersion", GetNextVersionNumber(_code, Convert.ToInt64(GetEditExtraInfo("schversion"))));
			SetExtraInfo("schSourceParameters", _xmlDParams.OuterXml);
			SetExtraInfo("schListView", _xmlDListViews.OuterXml);
			SetExtraInfo("schReturnField", _xmlDReturnFields.OuterXml);
            SetExtraInfo("schApiExclude", _msapiExclude);

			base.Update();
		}

        private long GetNextVersionNumber(string code, long currentversion)
        {
            VersionControlSupport vcs = new VersionControlSupport();
            return vcs.IncrementVersionNumber(code, currentversion, "dbSearchListVersionData");
        }

		#endregion
	}

	public class ReturnFields
	{
		#region Fields
		protected string _fieldname;
		protected XmlNode _node;
		private SearchListEditor _parent;
		#endregion
		
		#region Public Methods
		public override string ToString()
		{
			return _fieldname;
		}
		#endregion

		#region Constructors
		public ReturnFields(SearchListEditor Parent, string FieldName, XmlNode Node)
		{
			_parent = Parent;
			_fieldname = FieldName;
			_node = Node;
		}

		public ReturnFields(SearchListEditor Parent, string FieldName)
		{
			_fieldname = FieldName;
			_parent = Parent;
		}

        public override bool Equals(object obj)
        {
            ReturnFields ret = obj as ReturnFields;
            if (ret != null)
            {
                if (ret.FieldName == this.FieldName)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
		#endregion

		#region Properties
		[LocCategory("Design")]
		[TypeConverter(typeof(FieldMappingTypeEditor))]
		public virtual string FieldName
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
		public System.Xml.XmlNode Node
		{
			get
			{
				return _node;
			}
			set
			{
				_node = value;
			}
		}

		[Browsable(false)]
		public SearchListEditor Parent
		{
			get
			{
				return _parent;
			}
		}
		#endregion
	}

	public class Columns
	{
		#region Fields
		private CodeLookupDisplay _codelookup = new CodeLookupDisplay("SLCAPTION");
		private string _mappingname;
		private int _width;
		private XmlNode _node;
        private bool _sortable = true;
        private SearchColumnsFormat _format;
		private bool _incquicksearch = true;
		private CodeLookupDisplay _nulltext = new CodeLookupDisplay("SLCAPTION");
		private SearchListEditor _parent;
		private CodeLookupDisplayReadOnly _datalistname = new CodeLookupDisplayReadOnly("ENQDATALIST");
		private string _codetype = "";
		private string[] _conditions;
		private CodeLookupDisplayMulti _usrroles = new CodeLookupDisplayMulti("USRROLES");
        private SearchColumnsDateIs _sourceis = SearchColumnsDateIs.NotApplicable;
        private SearchColumnsDateIs _displayas = SearchColumnsDateIs.NotApplicable;
        private string[] dt;
        #endregion

		#region Constructors
		public Columns(SearchListEditor Parent, string CodeLookup, string MappingName, int Width, SearchColumnsFormat Format, bool incQuickSearch, string NullText, string DataListName, string CodeType, string Conditions, string Roles)
		{
			_codelookup.Code = CodeLookup;
			_mappingname = MappingName;
			_width = Width;
			_format = Format;
			_incquicksearch = incQuickSearch;
			_parent = Parent;
			_nulltext.Code = NullText;
			_datalistname.Code = DataListName;
			_codetype = CodeType;
			_conditions = Conditions.Split(Environment.NewLine[1]);
			for(int i = 0; i < _conditions.Length; i++)
				_conditions[i] = _conditions[i].Trim(Environment.NewLine[0]);
			_usrroles.Codes = Roles;

		}

		public Columns(SearchListEditor Parent, string CodeLookup, string MappingName, int Width, SearchColumnsFormat Format, bool incQuickSearch, string NullText, string DataListName, string CodeType, string Conditions, string Roles,  XmlNode Node)
		{
			_codelookup.Code = CodeLookup;
			_mappingname = MappingName;
			_width = Width;
			_node = Node;
			_format = Format;
			_incquicksearch = incQuickSearch;
			_parent = Parent;
			_nulltext.Code = NullText;
			_datalistname.Code = DataListName;
			_codetype = CodeType;
			_conditions = Conditions.Split(Environment.NewLine[1]);
			for(int i = 0; i < _conditions.Length; i++)
				_conditions[i] = _conditions[i].Trim(Environment.NewLine[0]);
			_usrroles.Codes = Roles;
		}

        public Columns(SearchListEditor Parent, string CodeLookup, string MappingName, int Width, bool sortable, SearchColumnsFormat Format, bool incQuickSearch, string NullText, string DataListName, string CodeType, string Conditions, string Roles, XmlNode Node, SearchColumnsDateIs SourceIs, SearchColumnsDateIs DisplayAs)
        {
            _codelookup.Code = CodeLookup;
            _mappingname = MappingName;
            _width = Width;
            _node = Node;
            _sortable = sortable;
            _format = Format;
            _incquicksearch = incQuickSearch;
            _parent = Parent;
            _nulltext.Code = NullText;
            _datalistname.Code = DataListName;
            _codetype = CodeType;
            _conditions = Conditions.Split(Environment.NewLine[1]);
            for (int i = 0; i < _conditions.Length; i++)
                _conditions[i] = _conditions[i].Trim(Environment.NewLine[0]);
            _usrroles.Codes = Roles;
            _sourceis = SourceIs;
            _displayas = DisplayAs;
            dt = Parent.DataBuilder.TestAndGetFieldAndTypes();
            if (_sourceis == SearchColumnsDateIs.NotApplicable)
            {
                IsDate(_mappingname);
            }
        }
		#endregion

		#region Public Methods
		public override string ToString()
		{
			return _codelookup.Description;
		}
		#endregion

		#region Properties
		[LocCategory("DISPLAY")]
		[CodeLookupSelectorTitle("COLUMNS","Column Titles")]
		public CodeLookupDisplay Text
		{
			get
			{
				return _codelookup;
			}
			set
			{
				_codelookup = value;
			}
		}

		[TypeConverter(typeof(FieldMappingTypeEditor))]
		[LocCategory("DATA")]
		public string MappingName
		{
			get
			{
				return _mappingname;
			}
			set
			{
				_mappingname = value;
                IsDate(value);
			}
		}

        private void IsDate(string mappingName)
        {
            if (dt == null)
                return;

            foreach (string f in dt)
            {
                if (f.StartsWith(mappingName + ','))
                {
                    if (f.IndexOf("DateTime", mappingName.Length + 1) > -1)
                    {
                        _sourceis = SearchColumnsDateIs.UTC;
                        _displayas = SearchColumnsDateIs.Local;
                    }
                    else
                    {
                        _sourceis = SearchColumnsDateIs.NotApplicable;
                        _displayas = SearchColumnsDateIs.NotApplicable;
                    }
                    return;
                }
            }

        }

        [LocCategory("DISPLAY")]
		public bool Sortable
		{
			get
			{
               return _sortable;
			}
			set
			{
                _sortable = value;
			}
		}

        [LocCategory("DISPLAY")]
        public SearchColumnsFormat Format
        {
            get
            {
                return _format;
            }
            set
            {
                _format = value;
            }
        }

        [LocCategory("DISPLAY")]
		public int Width
		{
			get
			{
				return _width;
			}
			set
			{
				_width = value;
			}
		}

		[LocCategory("DATA")]
		[CodeLookupSelectorTitle("NULLTEXTS","Null Texts")]
		public CodeLookupDisplay NullText
		{
			get
			{
				return _nulltext;
			}
			set
			{
				_nulltext = value;
			}
		}
		
		[Description("If the Column requires a List Source then it set the Data Source here"), LocCategory("DATALISTS")]
		[Parameter("DATALIST")]
		[RefreshProperties(RefreshProperties.All)]
		[CodeLookupSelectorTitle("DATALISTS","Data Lists")]
		public CodeLookupDisplayReadOnly DataListName

		{
			get
			{
				return _datalistname;
			}
			set
			{
				_datalistname = value;
				_codetype = "";
			}
		}

		[Description("The Data Source for this Column is a Code Lookup"), LocCategory("DATALISTS")]
		[RefreshProperties(RefreshProperties.All)]
		public string CodeTypeName
		{
			get
			{
				return _codetype;
			}
			set
			{
				_codetype = value;
				_datalistname.Code = "";
			}
		}
		
		[LocCategory("DISPLAY")]
		public bool IncQuickSearch
		{
			get
			{
				return _incquicksearch;
			}
			set
			{
				_incquicksearch = value;
			}
		}

		[LocCategory("COLUMNVISIBLE")]
		public string[] Conditional
		{
			get
			{
				return _conditions;
			}
			set
			{
				_conditions = value;
			}
		}

        [LocCategory("DISPLAY")]
        public SearchColumnsDateIs SourceDateIs
        {
            get
            {
                return _sourceis;
            }
            set
            {
                if (_sourceis == SearchColumnsDateIs.NotApplicable && value != SearchColumnsDateIs.NotApplicable)
                    throw new OMSException2("ERRNOTDATEFIELD", "This is not a Date Field");
                if (value == SearchColumnsDateIs.NotApplicable && _sourceis != SearchColumnsDateIs.NotApplicable)
                    throw new OMSException2("ERRDATEFIELD", "This is a Date Field you must choose UTC or Local");
                _sourceis = value;
            }
        }

        [LocCategory("DISPLAY")]
        public SearchColumnsDateIs DisplayDateAs
        {
            get
            {
                return _displayas;
            }
            set
            {
                if (_sourceis == SearchColumnsDateIs.NotApplicable && value != SearchColumnsDateIs.NotApplicable)
                    throw new OMSException2("ERRNOTDATEFIELD", "This is not a Date Field");
                if (value == SearchColumnsDateIs.NotApplicable && _sourceis != SearchColumnsDateIs.NotApplicable)
                    throw new OMSException2("ERRDATEFIELD", "This is a Date Field you must choose UTC or Local");
                _displayas = value;
            }
        }

		[LocCategory("COLUMNVISIBLE")]
		[CodeLookupSelectorTitle("USERROLES","User Roles")]
		public CodeLookupDisplayMulti UserRoles
		{
			get
			{
				return _usrroles;
			}
			set
			{
				_usrroles = value;
			}
		}


		[Browsable(false)]
		public System.Xml.XmlNode Node
		{
			get
			{
				return _node;
			}
			set
			{
				_node = value;
			}
		}

		[Browsable(false)]
		public SearchListEditor Parent
		{
			get
			{
				return _parent;
			}
		}
		#endregion
	}

	public class Buttons : LookupTypeDescriptor
	{
		#region Fields
        private ImageList imagelist = null;
        private string _name;
		private CodeLookupDisplay _codelookup = new CodeLookupDisplay("SLBUTTON");
		private bool _visible;
		private XmlNode _node;
		private int _buttonglyph = -1;
		private ButtonStyle _style = ButtonStyle.Graphical;
		private ButtonParameter _parameter;
		private bool _contextvisible = false;

		private bool _panelbuttonvisible = false;
		private CodeLookupDisplay _pnltext = new CodeLookupDisplay("SLPBUTTON");
		private int _pnlimageindex = -1;
		private bool _enabledwhennorows = true;
		private bool _enabledwhenmultiselect = false;

		private string[] _conditions;
		private CodeLookupDisplayMulti _usrroles = new CodeLookupDisplayMulti("USRROLES");
		private SearchListEditor _parent = null;
		#endregion

		#region Public & Contructors
		public override string ToString()
		{
			return _name;
		}

		public Buttons(SearchListEditor Parent, string Name, string CodeLookup, bool Visible) : this (Parent,"",ButtonActions.None,Name,CodeLookup,Visible,null)
		{
		}

		public Buttons(SearchListEditor Parent, string Name, string CodeLookup, bool Visible, XmlNode Node) : this(Parent,"",ButtonActions.None,Name,CodeLookup,Visible,Node)		{
		}

		public Buttons(SearchListEditor Parent, string Parameter, ButtonActions Action, string Name, string CodeLookup, bool Visible, XmlNode Node) : this(Parent,ButtonStyle.Graphical,-1,Parameter,Action,Name,CodeLookup,Visible,false, "",-1,false,true,false, "","",Node)
		{
		}
			
		public Buttons(SearchListEditor Parent, ButtonStyle ButtonStyle, int ButtonGlyph, string Parameter, ButtonActions Action, string Name, string CodeLookup, bool Visible, bool pnlButtonVisible, string pnlButtonText, int pnlButtonIndex, bool ContextVisible, bool EnabledWhenNoRows, bool EnabledWhenMultiSelect, string Conditions, string Roles, XmlNode Node)
		{
            imagelist = FWBS.OMS.UI.Windows.Images.CoolButtons16();
            _parent = Parent;
			_buttonglyph = ButtonGlyph;
			_style = ButtonStyle;
			_name = Name;
			_parameter = new ButtonParameter(Parameter,Action);
			_codelookup.Code = CodeLookup;
			_visible = Visible;
			_node = Node;
			_pnltext.Code = pnlButtonText;
			_pnlimageindex = pnlButtonIndex;
			_panelbuttonvisible = pnlButtonVisible;
			_contextvisible = ContextVisible;
			_enabledwhennorows = EnabledWhenNoRows;
			_enabledwhenmultiselect = EnabledWhenMultiSelect;
			_conditions = Conditions.Split(Environment.NewLine[1]);
			for(int i = 0; i < _conditions.Length; i++)
				_conditions[i] = _conditions[i].Trim(Environment.NewLine[0]);
			_usrroles.Codes = Roles;
		}
		#endregion

		#region Properties
		[Browsable(false)]
		public ImageList ImageList
		{
			get
			{
				return imagelist;
			}
		}

		[LocCategory("Data")]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		[LocCategory("Design")]
		[Lookup("BTNCAP")]
		[CodeLookupSelectorTitle("BUTTONSCAP","Button Captions")]
		public CodeLookupDisplay Caption
		{
			get
			{
				return _codelookup;
			}
			set
			{
				_codelookup = value;
			}
		}
		
		[LocCategory("Design")]
		public FWBS.OMS.UI.Windows.ButtonStyle ButtonStyle
		{
			get
			{
				return _style;
			}
			set
			{
				_style = value;
			}
		}

		[LocCategory("Design")]
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor(typeof(IconDisplayEditor),typeof(UITypeEditor))]
		public int ButtonGlyph
		{
			get
			{
				return _buttonglyph;
			}
			set
			{
				_buttonglyph = value;
			}
		}

		[LocCategory("Design")]
		public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				_visible = value;
			}
		}

        [LocCategory("Design")]
        [Lookup("Parent")]
        public string ParentCode{get;set;}
        

		[LocCategory("Design")]
		[Lookup("EnabledNoRows")]
		public bool EnabledWhenNoRows
		{
			get
			{
				return _enabledwhennorows;
			}
			set
			{
				_enabledwhennorows = value;
			}
		}

		[LocCategory("Design")]
		[Lookup("EnabledMultiSel")]
		public bool EnabledWhenMultiSelect
		{
			get
			{
				return _enabledwhenmultiselect;
			}
			set
			{
				_enabledwhenmultiselect = value;
			}
		}

		[LocCategory("Design")]
		[Lookup("ContextVis")]
		public bool ContextMenuVisible
		{
			get
			{
				return _contextvisible;
			}
			set
			{
				_contextvisible = value;
			}
		}

		[LocCategory("Actions")]
		[RefreshProperties(RefreshProperties.All)]
		public ButtonActions Action
		{
			get
			{
				return _parameter.Action;
			}
			set
			{
				_parameter.Action = value;
                switch (value)
                {
                    case ButtonActions.Add:
                    case ButtonActions.AddFrom:
                        _buttonglyph = 0;
                        break;
                    case ButtonActions.Edit:
                    case ButtonActions.EditWizard:
                    case ButtonActions.EditDialog:
                        _buttonglyph = 24;
                        _enabledwhennorows = false;
                        break;
                    case ButtonActions.Service:
                        break;
                    case ButtonActions.Search:
                        _buttonglyph = 12;
                        _style = Windows.ButtonStyle.Plain;
                        break;
                    case ButtonActions.Select:
                        _buttonglyph = 7;
                        break;
                    case ButtonActions.Delete:
                    case ButtonActions.TrashDelete:
                        _enabledwhennorows = false;
                        _buttonglyph = 6;
                        break;
                    case ButtonActions.Clone:
                        _buttonglyph = 4;
                        _enabledwhennorows = false;
                        break;
                    case ButtonActions.ViewTrash:
                        _buttonglyph = 22;
                        break;
                    case ButtonActions.ViewActive:
                        _buttonglyph = 19;
                        break;
                    case ButtonActions.Restore:
                        _enabledwhennorows = false;
                        _buttonglyph = 8;
                        break;
                    case ButtonActions.Report:
                    case ButtonActions.ReportingServer:
                    case ButtonActions.ReportMulti:
                        _buttonglyph = 60;
                        break;
                    case ButtonActions.Seperator:
                    case ButtonActions.SearchList:
                    case ButtonActions.None:
                    default:
                        break;
                }

                if (String.IsNullOrEmpty(_codelookup.Code))
                {
                    switch (value)
                    {
                        case ButtonActions.Add:
                        case ButtonActions.AddFrom:
                            _codelookup.Code = "BTNADD";
                            break;
                        case ButtonActions.Edit:
                        case ButtonActions.EditWizard:
                        case ButtonActions.EditDialog:
                            _codelookup.Code = "BTNEDIT";
                            break;
                        case ButtonActions.Service:
                            break;
                        case ButtonActions.Search:
                            _codelookup.Code = "BTNSEARCH";
                            break;
                        case ButtonActions.Select:
                            _codelookup.Code = "BTNSELECT";
                            break;
                        case ButtonActions.Delete:
                        case ButtonActions.TrashDelete:
                            _codelookup.Code = "BTNDELETE";
                            break;
                        case ButtonActions.Clone:
                            _codelookup.Code = "BTNCLONE";
                            break;
                        case ButtonActions.ViewTrash:
                            _codelookup.Code = "BTNTRASH";
                            break;
                        case ButtonActions.ViewActive:
                            _codelookup.Code = "BTNACTIVE";
                            break;
                        case ButtonActions.Restore:
                            _codelookup.Code = "BTNRESTORE";
                            break;
                        case ButtonActions.OpenSearch:
                            _codelookup.Code = "BTNOPENSEARCH";
                            break;
                        case ButtonActions.SaveSearch:
                            _codelookup.Code = "BTNSAVESEARCH";
                            break;
                        case ButtonActions.Seperator:
                        case ButtonActions.Report:
                        case ButtonActions.ReportingServer:
                        case ButtonActions.ReportMulti:
                        case ButtonActions.SearchList:
                        case ButtonActions.None:
                        default:
                            break;
                    }
                }

				if (_name == "")
				{
					_name = "cmd" + value.ToString();
				}
			}
		}
		
		[LocCategory("Actions")]
		[RefreshProperties(RefreshProperties.All)]
		public ButtonParameter Parameter
		{
			get
			{
				return _parameter;
			}
			set
			{
				_parameter = value;
			}
		}

		[Category("Panels")]
		[Lookup("PnlVisible")]
		public bool PanelButtonVisible
		{
			get
			{
				return _panelbuttonvisible;
			}
			set
			{
				_panelbuttonvisible = value;
			}
		}

		[Category("Panels")]
		[Lookup("PnlCaption")]
		[CodeLookupSelectorTitle("PNLBTNCAP","Panel Button Captions")]
		public CodeLookupDisplay PanelButtonCaption
		{
			get
			{
				return _pnltext;
			}
			set
			{
				_pnltext = value;
			}
		}

		[Category("Panels")]
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor(typeof(IconDisplayEditor),typeof(UITypeEditor))]
		[Lookup("PnlImgIndex")]
		public int PanelButtonImageIndex
		{
			get
			{
				return _pnlimageindex;
			}
			set
			{
				_pnlimageindex = value;
			}
		}

		[LocCategory("BUTTONVISIBLE")]
		public string[] Conditional
		{
			get
			{
				return _conditions;
			}
			set
			{
				_conditions = value;
			}
		}

		[LocCategory("BUTTONVISIBLE")]
		[CodeLookupSelectorTitle("USERROLES","User Roles")]
		public CodeLookupDisplayMulti UserRoles
		{
			get
			{
				return _usrroles;
			}
			set
			{
				_usrroles = value;
			}
		}

		[Browsable(false)]
		public System.Xml.XmlNode Node
		{
			get
			{
				return _node;
			}
			set
			{
				_node = value;
			}
		}

		[Browsable(false)]
		public SearchListEditor Parent
		{
			get
			{
				return _parent;
			}
		}

  		#endregion
	}

	// **********************************************************************************************************
	// 
	// Property Editors
	// 
	// **********************************************************************************************************

	/// <summary>
	/// Columns Collection Editor
	/// </summary>
	internal class ColumnsEditor : CollectionEditorEx
	{
		public ColumnsEditor() : base (typeof(ColumnsCollection)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (Columns);
		}

		protected override object CreateInstance(System.Type t)
		{
            return new Columns((SearchListEditor)this.Context.Instance, "", "", 100, true, SearchColumnsFormat.Standard, true, "", "", "", "", "", null, SearchColumnsDateIs.NotApplicable, SearchColumnsDateIs.NotApplicable);
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(Columns)};
		}
	}

	/// <summary>
	/// Returns Collection Editor
	/// </summary>
	internal class ReturnsEditor : CollectionEditorEx
	{
		public ReturnsEditor() : base (typeof(ReturnFieldsCollection)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (ReturnFields);
		}

		protected override object CreateInstance(System.Type t)
		{
			return new ReturnFields((SearchListEditor)this.Context.Instance,"");
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(ReturnFields)};
		}
	}

	/// <summary>
	/// Buttons Collection Editor
	/// </summary>
	internal class ButtonsEditor : CollectionEditorEx
	{
		public ButtonsEditor() : base (typeof(ButtonsCollection)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (Buttons);
		}

		protected override object CreateInstance(System.Type t)
		{
			return new Buttons((SearchListEditor)this.Context.Instance,"","",true);
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(Buttons)};
		}
	}

	/// <summary>
	/// Summary description for EnquiryFormEditor.
	/// </summary>
	internal class EnquiryFormEditor : UITypeEditor
	{
		private IWindowsFormsEditorService iWFES;
		public EnquiryFormEditor(){}

		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal ; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			FWBS.OMS.UI.Windows.OpenSaveEnquiry OpenSaveEnquiry1 = new FWBS.OMS.UI.Windows.OpenSaveEnquiry();
			OpenSaveEnquiry1.AllowDelete =false;
			OpenSaveEnquiry1.AllowNewFolder =false;
			OpenSaveEnquiry1.AllowRename =false;
			OpenSaveEnquiry1.Code = Convert.ToString(value);
			Form Me = OpenSaveEnquiry1.OpenSaveForm();
			iWFES.ShowDialog(Me);
			if (Me.DialogResult == DialogResult.OK)
				value = OpenSaveEnquiry1.Code;
			return value;
		}
	}
	
	[Editor(typeof(ButtonParameterEditor),typeof(UITypeEditor))]
	[TypeConverter(typeof(ButtonParameterConverter))]
	public class ButtonParameter
	{
		private string _value = "";
		private ButtonActions _action = ButtonActions.None;
		
		public ButtonParameter(string Value, ButtonActions Action)
		{
			_action = Action;
			_value = Value;
		}

		public ButtonActions Action
		{
			get
			{
				return _action;
			}
			set
			{
				_action = value;
			}
		}

		public string Value
		{
			get
			{
				return _value;
			}
		}
	}


	/// <summary>
	/// Summary description for DataBuilderEditor.
	/// </summary>
	internal class ButtonParameterConverter : TypeConverter
	{
		public override bool CanConvertFrom ( ITypeDescriptorContext ctx , Type sourceType )
		{
			if ( sourceType == typeof ( System.String ) )
				return true ;
			else
				return base.CanConvertFrom ( ctx , sourceType ) ;
		}

		public override object ConvertFrom ( ITypeDescriptorContext ctx , CultureInfo culture , object value )
		{
			if (value != null && value.GetType() == typeof(System.String))
			{
				string data = value as string ;
				if (data == "")
					return new ButtonParameter("",ButtonActions.None);
				else
					return new ButtonParameter(data,((Buttons)ctx.Instance).Action);
			}
			else
				return base.ConvertFrom ( ctx , culture , value ) ;
		}

		public override bool CanConvertTo ( ITypeDescriptorContext ctx , Type destinationType )
		{
			return true ;
		}

		public override object ConvertTo ( ITypeDescriptorContext ctx , System.Globalization.CultureInfo culture , object value , Type destinationType )
		{
			if (value is ButtonParameter)
			{
				ButtonParameter db = (ButtonParameter)value;
				return db.Value;
			}
			else
				return "";
		}
	}
	
	/// <summary>
	/// Summary description for SearchListButtonEditor
	/// </summary>
	internal class ButtonParameterEditor : UITypeEditor
	{
		private IWindowsFormsEditorService iWFES;
		public ButtonParameterEditor(){}

		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal ; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			// Current Action from the Object
			FWBS.OMS.SearchEngine.ButtonActions btnaction = (FWBS.OMS.SearchEngine.ButtonActions)(TypeDescriptor.GetProperties(context.Instance)["Action"].GetValue(context.Instance));
			
			// If Add or Edit it uses the Enquiry Form Selector Dialog
			if (btnaction == ButtonActions.Add || btnaction == ButtonActions.Edit || btnaction == ButtonActions.EditWizard || btnaction == ButtonActions.EditDialog || btnaction == ButtonActions.AddFrom )
			{
				iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
				FWBS.OMS.UI.Windows.OpenSaveEnquiry OpenSaveEnquiry1 = new FWBS.OMS.UI.Windows.OpenSaveEnquiry();
				OpenSaveEnquiry1.AllowDelete =false;
				OpenSaveEnquiry1.AllowNewFolder =false;
				OpenSaveEnquiry1.AllowRename =false;
				OpenSaveEnquiry1.Code = ((ButtonParameter)value).Value;
				Form Me = OpenSaveEnquiry1.OpenSaveForm();
				iWFES.ShowDialog(Me);
				if (Me.DialogResult == DialogResult.OK)
					value = new ButtonParameter(OpenSaveEnquiry1.Code,btnaction);
				return value;
			}
			else if (btnaction == ButtonActions.SearchList)
			{
				FWBS.Common.KeyValueCollection retvals = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(null, Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.SearchListPicker),false, new Size(-1,-1), null,new FWBS.Common.KeyValueCollection());
				if (retvals != null)
					value = new ButtonParameter(Convert.ToString(retvals[0].Value),btnaction);
				return value;
			}
			else if (btnaction == ButtonActions.ReportMulti)
			{
				iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
				FWBS.OMS.UI.Windows.Admin.frmMultiReport _report = new frmMultiReport();
				ButtonParameter _original = value as ButtonParameter;
				if (_original != null) _report.Value = _original.Value;
				iWFES.ShowDialog(_report);
				if (_report.DialogResult == DialogResult.OK)
				{
					value = new ButtonParameter(_report.Value,btnaction);
				}
				return value;
			}
            else if (btnaction == ButtonActions.ReportingServer)
            {
                FWBS.Common.KeyValueCollection retvals = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(null, Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.ReportServerPicker), false, new Size(-1, -1), null, new FWBS.Common.KeyValueCollection());
                if (retvals != null)
                    value = new ButtonParameter(Convert.ToString(retvals[0].Value), btnaction);
                return value;
            }
            else if (btnaction == ButtonActions.Report)
			{
				FWBS.Common.KeyValueCollection retvals = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(null, Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.ReportPicker),false, new Size(-1,-1), null,new FWBS.Common.KeyValueCollection());
				if (retvals != null)
					value = new ButtonParameter(Convert.ToString(retvals[0].Value),btnaction);
				return value;
			}			
			else if (btnaction == ButtonActions.Service)
			{
				iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
				frmListSelector frmListSelector1 = new frmListSelector();
				frmListSelector1.CodeType = "ENQCOMMAND";
				frmListSelector1.List.DataSource = FWBS.OMS.EnquiryEngine.Enquiry.GetEnquiryCommands();
				frmListSelector1.List.DisplayMember = "cmdCode";
				frmListSelector1.List.ValueMember = "cmdCode";
				if (((ButtonParameter)value).Value != "")
					frmListSelector1.List.SelectedValue = ((ButtonParameter)value).Value;

				frmListSelector1.ShowHelp = true;
				iWFES.ShowDialog(frmListSelector1);
				if (frmListSelector1.DialogResult == DialogResult.OK)
					value = new ButtonParameter(frmListSelector1.List.Text,btnaction);
				return value;
			}
            else if (btnaction == ButtonActions.Workflow)
            {
                FWBS.Common.KeyValueCollection retvals = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(null, "SCHWFPICKER", false, new Size(-1, -1), null, new FWBS.Common.KeyValueCollection());
                if (retvals != null)
                    value = new ButtonParameter(Convert.ToString(retvals[0].Value), btnaction);
                return value;
            }
            else if (btnaction == ButtonActions.TrashDelete || btnaction == ButtonActions.Restore || btnaction == ButtonActions.Delete || btnaction == ButtonActions.ViewActive || btnaction == ButtonActions.ViewTrash)
            {
                iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                frmDeleteDesigner frmDeleteDesigner1 = new frmDeleteDesigner(btnaction, ((ButtonParameter)value).Value, ((FWBS.OMS.UI.Windows.Design.Buttons)context.Instance).Parent);
                iWFES.ShowDialog(frmDeleteDesigner1);
                if (frmDeleteDesigner1.DialogResult == DialogResult.OK)
                    value = new ButtonParameter(frmDeleteDesigner1.Output, btnaction);
                return value;
            }
            else
            {
                return new ButtonParameter("", btnaction);
            }
		}
	}

	/// <summary>
	/// Summary description for FieldMappingTypeEditor.
	/// </summary>
	internal class FieldMappingTypeEditor : StringConverter
	{
		public override bool GetStandardValuesSupported(
			ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			string[] dt;
			if (context.Instance is SearchListEditor)
			{
				try
				{
					dt = ((SearchListEditor)context.Instance).DataBuilder.TestAndGetFields();
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ex);
					dt = new string[0]{};
				}
			}
			else if (context.Instance is ReturnFields)
			{
				try
				{
					dt = ((ReturnFields)context.Instance).Parent.DataBuilder.TestAndGetFields();
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ex);
					dt = new string[0]{};
				}
			}
			else if (context.Instance is Columns)
			{
				try
				{
					dt = ((Columns)context.Instance).Parent.DataBuilder.TestAndGetFields();
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ex);
					dt = new string[0]{};
				}
			}
			else if (context.Instance is ExtendedDataEditor)
			{
				try
				{
					dt = ((ExtendedDataEditor)context.Instance).DataBuilder.TestAndGetFields();
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ex);
					dt = new string[0]{};
				}
			}
			else if (context.Instance is ActiveTrashBuilder)
			{
				try
				{
					dt = ((ActiveTrashBuilder)context.Instance).Parent.DataBuilder.TestAndGetFields();
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ex);
					dt = new string[0]{};
				}
			}
			else if (context.Instance is TrashDeleteBuilder)
			{
				try
				{
					dt = ((TrashDeleteBuilder)context.Instance).Parent.DataBuilder.TestAndGetFields();
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ex);
					dt = new string[0]{};
				}
			}
			else if (context.Instance is RestoreBuilder)
			{
				try
				{
					dt = ((RestoreBuilder)context.Instance).Parent.DataBuilder.TestAndGetFields();
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ex);
					dt = new string[0]{};
				}
			}
			else
			{
                MessageBox.ShowInformation("OBJNOTVALID", "'%1%' is not a valid object for the Field list Designer", context.Instance.ToString());
				dt = new string[0]{};
			}
			return new StandardValuesCollection(dt);
		}
	}


	
	/// <summary>
	/// Summary description for SearchTypeEditor.
	/// </summary>
	internal class SearchTypeEditor : StringConverter
	{
		public override bool GetStandardValuesSupported(
			ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			DataTable dt = FWBS.OMS.SearchEngine.SearchList.GetSearchTypes();
			ArrayList ar = new ArrayList();
			foreach(DataRow row in dt.Rows)
				ar.Add(row[0]);
			return new StandardValuesCollection(ar);
		}
	}

	
	/// <summary>
	/// Summary description for ButtonCodeLookupEditor.
	/// </summary>
	internal class CodeLookupEditor : UITypeEditor
	{
		private IWindowsFormsEditorService iWFES;
		public CodeLookupEditor(){}

		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal ; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			frmCodeLookupSelector frmListSelector1 = new frmCodeLookupSelector();
			// Get the ParameterAttribute from the Property
			frmListSelector1.CodeType = Convert.ToString(((ParameterAttribute)context.PropertyDescriptor.Attributes[typeof(ParameterAttribute)]).Value);
			frmListSelector1.LoadCodeTypes();
			if (Convert.ToString(value) != "")
				frmListSelector1.List.SelectedValue= value;
			frmListSelector1.ShowHelp = true;
			iWFES.ShowDialog(frmListSelector1);
			if (frmListSelector1.DialogResult == DialogResult.OK)
				value = frmListSelector1.List.SelectedValue.ToString();
			return value;
		}
	}


	/// <summary>
	/// Summary description for ParentLookupEditor.
	/// </summary>
	internal class ParentLookupEditor : UITypeEditor
	{
		private IWindowsFormsEditorService iWFES;
		public ParentLookupEditor(){}

		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal ; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			frmListSelector frmListSelector1 = new frmListSelector();
			frmListSelector1.List.Sorted=true;
			Type[] rtypes;
			rtypes = FWBS.OMS.EnquiryEngine.Enquiry.GetObjects();
			string[] types = new string[rtypes.Length];
			for(int n = 0; n<rtypes.Length; n++)
				types[n] = rtypes[n].FullName;
			frmListSelector1.List.Items.AddRange(types);
			if (Convert.ToString(value) != "")
				frmListSelector1.List.SelectedValue= value;
			frmListSelector1.ShowHelp = false;
			iWFES.ShowDialog(frmListSelector1);
			if (frmListSelector1.DialogResult == DialogResult.OK)
			{
				value =  frmListSelector1.List.Text;
			}
			return value;
		}
	}


	
	/// <summary>
	/// Summary description for CommandEditor.
	/// </summary>
	internal class CommandEditor : UITypeEditor
	{
		private IWindowsFormsEditorService iWFES;
		public CommandEditor(){}

		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal ; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			frmListSelector frmListSelector1 = new frmListSelector();
			frmListSelector1.CodeType = "ENQCOMMAND";
			frmListSelector1.Text = Session.CurrentSession.Resources.GetResource("PARENTOBJ", "Parent Objects","").Text;
			frmListSelector1.List.DataSource = FWBS.OMS.EnquiryEngine.Enquiry.GetEnquiryCommands();
			frmListSelector1.List.DisplayMember = "cmdCode";
			frmListSelector1.List.ValueMember = "cmdCode";
			if (Convert.ToString(value) != "")
				frmListSelector1.List.SelectedValue = value;

			frmListSelector1.ShowHelp = true;
			iWFES.ShowDialog(frmListSelector1);
			if (frmListSelector1.DialogResult == DialogResult.OK)
				value = frmListSelector1.List.Text;
			return value;
			
		}
	}

	public class ColumnsCollection : Crownwood.Magic.Collections.CollectionWithEvents
	{
		public Columns Add(Columns value)
		{
			// Use base class to process actual collection operation
			base.List.Add(value as object);

			return value;
		}

		public void AddRange(Columns[] values)
		{
			// Use existing method to add each array entry
			foreach(Columns page in values)
				Add(page);
		}

		public void Remove(Columns value)
		{
			// Use base class to process actual collection operation
			base.List.Remove(value as object);
		}

		public void Insert(int index, Columns value)
		{
			// Use base class to process actual collection operation
			base.List.Insert(index, value as object);
		}

		public bool Contains(Columns value)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(value as object);
		}

		public Columns this[int index]
		{
			// Use base class to process actual collection operation
			get { return (base.List[index] as Columns); }
		}

		public int IndexOf(Columns value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}
	}

	public class ReturnFieldsCollection : Crownwood.Magic.Collections.CollectionWithEvents
	{
		public ReturnFields Add(ReturnFields value)
		{
			// Use base class to process actual collection operation
			base.List.Add(value as object);

			return value;
		}

		public void AddRange(ReturnFields[] values)
		{
			// Use existing method to add each array entry
			foreach(ReturnFields page in values)
				Add(page);
		}

		public void Remove(ReturnFields value)
		{
			// Use base class to process actual collection operation
			base.List.Remove(value as object);
		}

		public void Insert(int index, ReturnFields value)
		{
			// Use base class to process actual collection operation
			base.List.Insert(index, value as object);
		}

		public bool Contains(ReturnFields value)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(value as object);
		}

		public ReturnFields this[int index]
		{
			// Use base class to process actual collection operation
			get { return (base.List[index] as ReturnFields); }
		}

		public int IndexOf(ReturnFields value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}
	}

	public class ButtonsCollection : Crownwood.Magic.Collections.CollectionWithEvents
	{
		public Buttons Add(Buttons value)
		{
			// Use base class to process actual collection operation
			base.List.Add(value as object);

			return value;
		}

		public void AddRange(Buttons[] values)
		{
			// Use existing method to add each array entry
			foreach(Buttons page in values)
				Add(page);
		}

		public void Remove(Buttons value)
		{
			// Use base class to process actual collection operation
			base.List.Remove(value as object);
		}

		public void Insert(int index, Buttons value)
		{
			// Use base class to process actual collection operation
			base.List.Insert(index, value as object);
		}

		public bool Contains(Buttons value)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(value as object);
		}

		public Buttons this[int index]
		{
			// Use base class to process actual collection operation
			get { return (base.List[index] as Buttons); }
		}

		public int IndexOf(Buttons value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}
	}
}
