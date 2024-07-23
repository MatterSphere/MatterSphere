using System;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Xml;
using FWBS.Common;
using FWBS.OMS.SearchEngine;
using FWBS.OMS.SourceEngine;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Report List Business Object for the Admin Kit
    /// </summary>
    public class ReportListEditor : SearchList
	{
		#region Constructors
		internal ReportListEditor(ReportListEditor Clone) : base()
		{
            this.ParentTypeRequired = Clone.ParentTypeRequired;
            _searchList.Tables[Table].Rows[0].ItemArray = Clone._searchList.Tables[Table].Rows[0].ItemArray;
			SetExtraInfo("schCode", DBNull.Value);
			_db.ResetFields();
			_db.Call ="";
			_db.Parameters.Clear();
			_db.ParametersInx.Clear();
			GetSearchListDesign(Code);
			this.ReportObject.DataRow.ItemArray = Clone.ReportObject.DataRow.ItemArray;
			this.ReportObject.Refresh();
			RefreshSearchControl();
            try
            {
                LoadSQLVersion(Code);
            }
            catch
            {
            }
        }

		public ReportListEditor() : base()
		{
			SetExtraInfo("schReturnField", "RESULTS");
			_db.ResetFields();
			_db.Call ="";
			_db.Parameters.Clear();
			_db.ParametersInx.Clear();
			_report = new Report();
			GetSearchListDesign(Code);
			RefreshSearchControl();
		}
			
			
		public ReportListEditor(string Code) : base(Code,null,new FWBS.Common.KeyValueCollection())
		{
			GetSearchListDesign(Code);
			RefreshSearchControl();
			try
			{
				LoadSQLVersion(Code);
			}
			catch
			{
			}
		}

		public ReportListEditor(string Code, bool Clone) : base(Code,null,new FWBS.Common.KeyValueCollection())
		{
			GetSearchListDesign(Code);
            RefreshSearchControl();
			try
			{
				LoadSQLVersion(Code);
			}
			catch
			{
			}
		}
		#endregion

		#region Static
		public static ReportListEditor Clone(string type)
		{
			ReportListEditor o = new ReportListEditor(type);
			return new ReportListEditor(o);
		}
		#endregion

		#region Editor Fields
		private KeyValueCollection _testparams = new KeyValueCollection();
		internal static DataBuilder _db = new DataBuilder();
		private ColumnsCollection _columns = new ColumnsCollection();
		private string _description;
		private string _orgcode;

		private Int64 _version = 0;
		private string _SearchType = "";
		private SearchListStyle _style = SearchListStyle.Search;
		private string _enquiryform = "";
		private int _seclevel = 0;
		private bool _active = true;
		private XmlDocument _xmlDParams;
		private XmlDocument _xmlDListViews;
		private XmlNode _xmlColumn;
		private XmlNode _xmlParameter;
		private XmlNode _xmlsearchList;
		private FWBS.OMS.Report _report = null;
		private string[] _tables;
		private CodeLookupDisplay _schgroup = new CodeLookupDisplay("SLGROUP");
		private string _detailed = "";

        private SaveSearchType _saveSearch = SaveSearchType.Never;

		#endregion

		#region Editor Private
		private void LoadSQLVersion(string Code)
		{
			FWBS.Common.KeyValueCollection ar = new FWBS.Common.KeyValueCollection();
			
			if (this.ParentTypeRequired.ToUpper() == "FWBS.OMS.CLIENT")
			{
				if (Session.CurrentSession.CurrentClient == null)
					FWBS.OMS.UI.Windows.Services.SelectClient();
				_db.ChangeParent(Session.CurrentSession.CurrentClient);
				this.ReportObject.SearchList.ChangeParent(Session.CurrentSession.CurrentClient);
				base.SearchListInternal(Code,Session.CurrentSession.CurrentClient,ar);

			}
			else if (this.ParentTypeRequired.ToUpper() == "FWBS.OMS.OMSFILE")
			{
				if (Session.CurrentSession.CurrentFile == null)
					FWBS.OMS.UI.Windows.Services.SelectFile();
				_db.ChangeParent(Session.CurrentSession.CurrentFile);
				this.ReportObject.SearchList.ChangeParent(Session.CurrentSession.CurrentFile);
				base.SearchListInternal(Code,Session.CurrentSession.CurrentFile,ar);
			}
			else
				base.SearchListInternal(Code,null,ar);
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
			try{Node.SelectSingleNode("@" + Name).Value = Value;}
			catch{Node.Attributes.Append(CreateAttribute(Node, Name,Value));}
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
			_code = Convert.ToString(GetEditExtraInfo("schCode"));
			_style = (SearchListStyle)Enum.ToObject(typeof(SearchListStyle),Convert.ToInt64(GetEditExtraInfo("schStyle")));
			_SearchType = Convert.ToString(GetEditExtraInfo("schType"));
			_version = Convert.ToInt64(GetEditExtraInfo("schVersion"));
			_enquiryform = Convert.ToString(GetEditExtraInfo("schEnquiry"));
			base.SourceType = (SourceType)FWBS.Common.ConvertDef.ToEnum(Convert.ToString(GetEditExtraInfo("schSourceType")),SourceType.OMS);
			base.Src = Convert.ToString(GetEditExtraInfo("schSource"));
			base.Call = Convert.ToString(GetEditExtraInfo("schSourceCall"));
			_tables = Convert.ToString(GetExtraInfo("schReturnField")).Split(',');
			_schgroup.Code = Convert.ToString(GetEditExtraInfo("schGroup"));


			_seclevel = Convert.ToInt32(GetEditExtraInfo("schSecurityLevel"));
			_active = Convert.ToBoolean(GetEditExtraInfo("schActive"));


			//
			// XML Parameters
			//
			_xmlDParams = new XmlDocument();
			_xmlDParams.PreserveWhitespace = false; 
			_xmlDParams.LoadXml(Convert.ToString(GetEditExtraInfo("schSourceParameters")));
			_xmlParameter = _xmlDParams.DocumentElement.SelectSingleNode("/params");


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
                _xmlColumn = _xmlDListViews.CreateElement("", "listView", "");
                _xmlsearchList.AppendChild(_xmlColumn);
            }

             _saveSearch = (SaveSearchType)FWBS.Common.ConvertDef.ToEnum(GetAttribute(_xmlColumn, "saveSearch", "Never"), SaveSearchType.Never);


			if (_code == "")
				_report = new FWBS.OMS.Report();
			else
			{
				try
				{
					_report = FWBS.OMS.Report.GetReport(Code,null,null);
				}
				catch
				{}
			}

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
				string __name = "";
				string __type = "";
				string __value = "";
				string __test = "";
                string __kind = "";
				try{__name = dr.SelectSingleNode("@name").Value;}
				catch{}
				try{__type = dr.SelectSingleNode("@type").Value;}
				catch{}
				try{__test = dr.SelectSingleNode("@test").Value;}
				catch{}
                try { __kind = dr.SelectSingleNode("@kind").Value; }
                catch { }
                __value = dr.InnerText;
                _db.Parameters.Add(new Parameter(_db, __name, __type, __value, __test, (SearchParameterDateIs)FWBS.Common.ConvertDef.ToEnum(__kind, SearchParameterDateIs.NotApplicable), dr.Clone()));
                if (__test != "" && __value != "")
					_testparams.Add(__value.Replace("%",""),__test);
			}
			_db.Call = base.Call;
			_db.Source = base.Src;
			_db.SourceType = base.SourceType;
			_db.EnquiryForm = EnquiryForm;
			_db.ChangeParameters(_testparams); // DataBuilder Key Pairs
			base.ChangeParameters(_testparams); // SearchLists Key Pairs


			DataTable dt = FWBS.OMS.CodeLookup.GetLookups("OMSSEARCH", _code);
			if (dt.Rows.Count > 0)
			{
				_description = Convert.ToString(dt.Rows[0]["cddesc"]);
			}
		}
		#endregion

		#region Editor Properties
		[TypeConverter(typeof(ScriptLister))]
		[ScriptTypeParam("Report")]
		[LocCategory("Script")]
		public override string ScriptName
		{
			get
			{
				return _report.ScriptName;
			}
			set
			{
				_report.ScriptName = value;
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
		
		[Editor(typeof(ReportsFileName),typeof(UITypeEditor))]
		[LocCategory("Reports")]
		public string Report
		{
			get
			{
				if (_report.ReportLocation != null)
					return "[Embeded Report]";
				else
					return "[No Report]";
			}
			set
			{
				_report.ReportLocation = new System.IO.FileInfo(value);
			}
		}
		
		[LocCategory("Design")]
		[Description("Enquiry Form used for the Filter Criteria")]
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
                        Description = CodeLookup.GetLookup("OMSSEARCH", value);
                        base._code = value;
                        _report.Code = value;
                    }
                }
				else
				{
					if (IsNew)
					{
						SetExtraInfo("schCode", value);
						base._code = value;
						_report.Code = value;
					}
					else
						throw new OMSException2("30000","The Code cannot be changed when set");
				}
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
		[Description("Description used to identify the Report Data List")]
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
					FWBS.OMS.CodeLookup.Create("OMSSEARCH",_code,value,_detailed,CodeLookup.DefaultCulture,true,true,true);
				}
			}
		}


        [LocCategory("Design")]
        [Description("Version")]
        [Browsable(true)]
        public long Version
        {
            get
            {
                return _report.Version;
            }
            set
            {
                _report.Version = value;
            }
        }
		
		[LocCategory("Design")]
		[Description("Table Names")]
		[Browsable(true)]
		public new string[] Tables
		{
			get
			{
				return _tables;
			}
			set
			{
				_tables = value;
				this.DataBuilder  = this.DataBuilder;
				
			}
		}
				
		[LocCategory("Search")]
		[Description("Key Words")]
		[Browsable(true)]
		public string[] Keywords
		{
			get
			{
				return _report.Keywords;
			}
			set
			{
				_report.Keywords = value;
			}
		}

		[Editor(typeof(CodeDescriptionEditor),typeof(UITypeEditor))]
		[LocCategory("Search")]
		public string Notes
		{
			get
			{
				return _report.Notes;
			}
			set
			{
				_report.Notes = value;
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
                        WriteAttribute(p.Node,"kind", p.ParameterDateIs.ToString());
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
                        WriteAttribute(_newnode,"kind", p.ParameterDateIs.ToString());
                        _xmlParameter.AppendChild(_newnode);
					}
					if (p.TestValue != "" && p.BoundValue != "")
						base.ReplacementParameters.Add(p.BoundValue.Replace("%",""),p.TestValue);
				}
			}
		}

		[Browsable(false)]
		public FWBS.OMS.Report ReportObject
		{
			get
			{
				if (_report == null)
				{
					_report = new FWBS.OMS.Report();
				}
				return _report;
			}
		}
		#endregion

		#region Editor Methods
		public override void Update()
		{
			WriteAttribute(_xmlParameter,"parentRequired",base.ParentTypeRequired);
            WriteAttribute(_xmlColumn, "saveSearch", _saveSearch.ToString());

			if (_code == "")
				throw new SearchException(HelpIndexes.SearchNoCode);
			if (Call == "")
				throw new SearchException(HelpIndexes.SearchDataBuilderIncomplete);

			if (_tables != null)
				SetExtraInfo("schReturnField", string.Join(",",_tables));
			SetExtraInfo("schIsReport",true);
			SetExtraInfo("schCode",_code);
			SetExtraInfo("schStyle",_style);
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
			_report.Code = this.Code;

			SetExtraInfo("schVersion",  Convert.ToInt64(GetEditExtraInfo("schversion")) + 1);
			SetExtraInfo("schSourceParameters", _xmlDParams.OuterXml);
			SetExtraInfo("schListView", _xmlDListViews.OuterXml);
                 

			base.Update();
			_report.Update();
		}
		#endregion

		#region Public
		public void EditReport()
		{
			DataSet ds = new DataSet(this.Code);
			_db.Parameters = _db.Parameters;
			object d = _db.Run(false,true);
			ds = (DataSet)d;

			for(int i = 0; i < ds.Tables.Count; i++)
			{
				try
				{
					ds.Tables[i].TableName = _tables[i];
				}
				catch
				{
					ds.Tables[i].TableName = "REPORTS"+ (i+1).ToString();
				}
			}
			ds.Namespace=this.Code;
			ds.DataSetName=this.Code;

			ds.WriteXml(_report.XMLLocation.FullName,XmlWriteMode.WriteSchema);
			_report.XMLLocation = new System.IO.FileInfo(_report.XMLLocation.FullName);
			_report.EditReport();
		}
		#endregion
	}

	public class ReportsFileName : System.Windows.Forms.Design.FileNameEditor
	{
		protected override void InitializeDialog(OpenFileDialog openFileDialog)
		{
			openFileDialog.Filter = "Crystal Reports (*.rpt)|*.rpt|All Files (*.*)|*.*";
		}

	}
}
