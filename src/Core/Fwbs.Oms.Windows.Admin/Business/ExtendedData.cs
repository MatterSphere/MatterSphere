using System;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Xml;
using FWBS.Common;
using FWBS.OMS.SourceEngine;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for ExtendedData.
    /// </summary>
    public class ExtendedDataEditor : FWBS.OMS.ExtendedData
	{
		protected XmlDocument _xmlDParams;
		protected XmlNode _xmlParameter;

		protected XmlDocument _xmlDReturnFields;
		protected XmlNode _xmlCReturnFields = null;
		protected XmlNode _xmlReturnField;

		protected XmlNode _xmlExclusionsField;
        protected XmlNode _xmlLocalDates;
        
		private DataBuilder _db = new DataBuilder();
		private string _orgcode="";
		private string _description ="";      

		private ReturnFieldsCollection _exclusionsfields = new ReturnFieldsCollection();
		private FWBS.OMS.SourceEngine.SourceType _sourcetype;
		
		public ExtendedDataEditor() : base()
		{
			RefreshExtendedDataEditor();
		}

		public ExtendedDataEditor(string code) : base(code)
		{
			
			RefreshExtendedDataEditor();
			try
			{
				base.FetchExternalData();
			}
			catch (Exception ex)
			{
                ErrorBox.Show(ex);
			}
		}

		public ExtendedDataEditor(ExtendedDataEditor Clone) : base()
		{
			_extended.Rows[0].ItemArray = Clone._extended.Rows[0].ItemArray;
			_extended.Rows[0]["extCode"] = DBNull.Value;

			RefreshExtendedDataEditor();
			try
			{
				base.FetchExternalData();
			}
			catch
			{
			}
		}

		public void BuidDBFieldsRows()
		{
			string[] fields = this.DataBuilder.TestAndGetFields();
			DataTable dbfields = this.GetDBFields();
			foreach (string field in fields)
			{
				string excludefields =  "fileid,clid,contid,usrid,associd,rowguid";
                if (!excludefields.Contains(field.ToLower()))
				{
					dbfields.DefaultView.RowFilter = "fldName = '" + field + "'";
					if (dbfields.DefaultView.Count == 0)
						this.AddDBField(field);
				}
			}
		}

		private void WriteAttribute(XmlNode Node, string Name, string Value)
		{
			if (Node.SelectSingleNode("@" + Name) != null)
                Node.SelectSingleNode("@" + Name).Value = Value;
            else
                Node.Attributes.Append(CreateAttribute(Node, Name,Value));
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
			catch //(Exception ex)
			{
				WriteAttribute(Node,Name,Value);
				return Value;
			}

		}
		
		private void RefreshExtendedDataEditor()
		{
			//
			// XML Parameters
			//
			_xmlDParams = new XmlDocument();
			_xmlDParams.PreserveWhitespace = false;
			_xmlDParams.LoadXml(Convert.ToString(GetExtraInfo("extParameters")));
			_xmlParameter = _xmlDParams.DocumentElement.SelectSingleNode("/params");

			//
			// XML Return Fields
			//
			_xmlDReturnFields = new XmlDocument();
			_xmlDReturnFields.PreserveWhitespace = false;
			try
			{
				_xmlDReturnFields.LoadXml(Convert.ToString(GetExtraInfo("extFields")));
				_xmlCReturnFields = _xmlDReturnFields.DocumentElement.SelectSingleNode("/config");
			}
			catch{}
			if (_xmlCReturnFields == null)
			{
				_xmlCReturnFields = _xmlDReturnFields.CreateElement("","config","");
				_xmlDReturnFields.AppendChild(_xmlCReturnFields);
			}
			_xmlReturnField = _xmlDReturnFields.DocumentElement.SelectSingleNode("/config/fields");
			if (_xmlReturnField == null)
			{
                _xmlReturnField = _xmlDReturnFields.CreateElement("", "fields", "");
				_xmlCReturnFields.AppendChild(_xmlReturnField);
			}

            
                if (_xmlReturnField.SelectSingleNode("@utc") == null)
                    WriteAttribute(_xmlReturnField, "utc", "true");
            


			_xmlExclusionsField = _xmlDReturnFields.DocumentElement.SelectSingleNode("/config/exclusions");
			if (_xmlExclusionsField == null)
			{
				_xmlExclusionsField = _xmlDReturnFields.CreateElement("","exclusions","");
				_xmlCReturnFields.AppendChild(_xmlExclusionsField);
			}

            _xmlLocalDates = _xmlDReturnFields.DocumentElement.SelectSingleNode("/config/localdates");
            if (_xmlLocalDates == null)
            {
                _xmlLocalDates = _xmlDReturnFields.CreateElement("", "localdates", "");
                _xmlCReturnFields.AppendChild(_xmlLocalDates);
            }

			_orgcode = base.Code;
			_db.Parameters.Clear();
			_db.ParametersInx.Clear();
			_db.ResetFields();
			_db.EnquiryForm = "ßEXTENDEDDATAß";
			foreach(XmlNode dr in _xmlParameter.ChildNodes)
			{
                string __name = "";
                string __type = "";
                string __value = "";
                string __test = "";
                string __kind = "";
                try { __name = dr.SelectSingleNode("@name").Value; }
                catch { }
                try { __type = dr.SelectSingleNode("@type").Value; }
                catch { }
                try { __test = dr.SelectSingleNode("@test").Value; }
                catch { }
                try { __kind = dr.SelectSingleNode("@kind").Value; }
                catch { }
                __value = dr.InnerText;
                _db.Parameters.Add(new Parameter(_db, __name, __type, __value, __test, (FWBS.OMS.SearchEngine.SearchParameterDateIs)FWBS.Common.ConvertDef.ToEnum(__kind, FWBS.OMS.SearchEngine.SearchParameterDateIs.NotApplicable), dr.Clone()));
  
            }
			_db.Call = Convert.ToString(GetExtraInfo("extCall")) + " " + Convert.ToString(GetExtraInfo("extWhere"));
			_db.Call = _db.Call.Trim();
			_db.Source = Convert.ToString(GetExtraInfo("extSource"));
			
			try{_sourcetype = (SourceType)Enum.Parse(typeof(SourceType),Convert.ToString(GetExtraInfo("extSourceType")),true);}
			catch{}
			_db.SourceType = _sourcetype;

			_db.Fields.Clear();
			foreach(XmlNode dr in _xmlReturnField.ChildNodes)
			{
				string __mappingname = "";
				try{__mappingname = dr.InnerText;}
				catch{}
				_db.Fields.Add(new ReturnFieldsExclusion(this,__mappingname, dr.Clone()));
			}	
			
			_exclusionsfields.Clear();
			foreach(XmlNode dr in _xmlExclusionsField.ChildNodes)
			{
				string __mappingname = "";
				try{__mappingname = dr.InnerText;}
				catch{}
				_exclusionsfields.Add(new ReturnFieldsExclusion(this,__mappingname, dr.Clone()));
			}


			DataTable dt = FWBS.OMS.CodeLookup.GetLookups("EXTENDEDDATA", base.Code);
			if (dt.Rows.Count > 0)
			{
				_description =  Convert.ToString(dt.Rows[0]["cddesc"]);
			}
		}

		[LocCategory("Link")]
		[TypeConverter(typeof(FieldMappingTypeEditor))]
		public string SourceLink
		{
			get
			{
				return Convert.ToString(GetExtraInfo("extSourceLink"));
			}
			set
			{
				SetExtraInfo("extSourceLink",value);
			}
		}

		[LocCategory("Link")]
		public string DestinationLink
		{
			get
			{
				return Convert.ToString(GetExtraInfo("extDestLink"));
			}
			set
			{
				SetExtraInfo("extDestLink",value);
			}
		}
		
		[LocCategory("(Details)")]
		[Description("Description used to identify the Extended Data")]
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				if (Convert.ToString(GetExtraInfo("extCode")) == "")
				{
					throw new ExtendedDataException(HelpIndexes.ExtendedDataNoCode);
				}
				else
				{
					_description = value;
					FWBS.OMS.CodeLookup.Create("EXTENDEDDATA",Convert.ToString(GetExtraInfo("extCode")),value,"",CodeLookup.DefaultCulture,true,true,true);
				}
			}
		}

        [LocCategory("MSAPI")]
        [Browsable(true)]
        [Description("MSAPIEXCLUDE")]
        public bool MSApiExclude
        {
            get
            {

                return ConvertDef.ToBoolean(GetExtraInfo("extApiExclude"), true);           
            }
            set
            {             
                SetExtraInfo("extApiExclude", value);
               
            }
        }

        [LocCategory("MSAPI")]      
        [Description("MSAPIEXCLUWHERE")]
        [Lookup("MSAPIEXCLUWHERE")]
        public string MSApiSpecial
        {
            get
            {
             
                return Convert.ToString(GetExtraInfo("extWhereMsApi"));
            }
            set
            {     
                SetExtraInfo("extWhereMsApi", value);             
               
            }
        }
        

        [LocCategory("Data")]
        [Description("Set the Parent Type Required by this search list")]
        [Browsable(false)]
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
        
        [LocCategory("Data")]
		[Description("Are the Dates used in this Extended Data UTC?")]
		public bool UTCDates
		{
			get
			{
                return Convert.ToBoolean(_xmlReturnField.SelectSingleNode("@utc").Value);
			}
			set
			{
                WriteAttribute(_xmlReturnField, "utc", value.ToString());
			}
		}


		[LocCategory("Data")]
		[Description("Add, Modify Exclusions Fields that are exluded from the return returned fields")]
		[Editor(typeof(ReturnsExclusionsEditor),typeof(UITypeEditor))]
		public ReturnFieldsCollection Exclusions
		{
			get
			{
				return _exclusionsfields;
			}
			set
			{
				_exclusionsfields = value;
			}
		}

		[Description("Commands to build an Extended Data Source"), LocCategory("Data")]
		[DataBuilderAttribute(DataBuilderMode.ExtendedDataMode)]
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
				SetExtraInfo("extSource",_db.Source);
				SetExtraInfo("extSourceType",_db.SourceType);
				if (_db.Call.ToUpper().IndexOf("WHERE") != -1)
					SetExtraInfo("extCall",_db.Call.Substring(0,_db.Call.ToUpper().IndexOf("WHERE")-1));
				else
					SetExtraInfo("extCall",_db.Call);
				if (_db.Call.ToUpper().IndexOf("WHERE") != -1)
					SetExtraInfo("extWhere",_db.Call.Substring(_db.Call.ToUpper().IndexOf("WHERE")));

				foreach (Parameter p in _db.Parameters)
				{
					if (p.Node != null)
					{
						WriteAttribute(p.Node,"name",p.SQLParameter);
						WriteAttribute(p.Node,"type",p.FieldType.ToString());
						WriteAttribute(p.Node,"test",p.TestValue);
                        WriteAttribute(p.Node, "kind", p.ParameterDateIs.ToString());
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
				}
			}
		}

		[Editor(typeof(FWBS.OMS.UI.Windows.Design.FlagsEditor),typeof(UITypeEditor))]
		public override ExtendedDataMode Modes
		{
			get
			{
				return base.Modes;
			}
			set
			{
				SetExtraInfo("extModes", value);
			}
		}
 

		public override void Update()
		{
			WriteAttribute(_xmlParameter,"parentRequired",base.ParentTypeRequired);

            for (int ui = _xmlLocalDates.ChildNodes.Count - 1; ui > -1; ui--)
            {
                XmlNode dr = _xmlLocalDates.ChildNodes[ui];
                _xmlLocalDates.RemoveChild(dr);
            }

            string[] fields = this.DataBuilder.TestAndGetFieldAndTypes();

            if (this.UTCDates == false)
            {
                foreach (string p in fields)
                {
                    if (p.ToLower().IndexOf("datetime") > -1)
                    {
                        string[] fieldname = p.Split(",".ToCharArray());
                        XmlNode _newnode = _xmlLocalDates.OwnerDocument.CreateNode(XmlNodeType.Element, "field", "");
                        _newnode.InnerText = fieldname[0];
                        _xmlLocalDates.AppendChild(_newnode);
                    }
                }
            }
            for (int ui = _xmlExclusionsField.ChildNodes.Count - 1; ui > -1; ui--)
			{
				XmlNode dr = _xmlExclusionsField.ChildNodes[ui];
				_xmlExclusionsField.RemoveChild(dr);
			}
			foreach (ReturnFields p in _exclusionsfields)
			{
				if (p.Node != null)
				{
					p.Node.InnerText = p.FieldName;
					_xmlExclusionsField.AppendChild(p.Node);
				}
				else
				{
					XmlNode _new = _xmlExclusionsField;
					XmlNode _newnode = _new.OwnerDocument.CreateNode(XmlNodeType.Element,"field","");
					_newnode.InnerText = p.FieldName;
					_new.AppendChild(_newnode);
				}
			}
			for(int ui = _xmlReturnField.ChildNodes.Count-1; ui > -1; ui--)
			{
				XmlNode dr = _xmlReturnField.ChildNodes[ui];
				_xmlReturnField.RemoveChild(dr);
			}
			foreach (ReturnFields p in _db.Fields)
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
			SetExtraInfo("extParameters", _xmlDParams.OuterXml);
			SetExtraInfo("extFields", _xmlDReturnFields.OuterXml);
			base.Update();

		}

		/// <summary>
		/// An abstract method which must be implemented so that each parameter in the paramater
		/// list has its value populated.
		/// </summary>
		/// <param name="name">The name of the parameter being set.</param>
		/// <param name="value">The value to be returned to the parameter list.</param>
		protected override void SetParameter (string name, out object value)
		{
			foreach(Parameter p in _db.Parameters)
			{
				if (p.BoundValue.IndexOf(name) != -1)
				{
					value = p.TestValue;
					return;
				}
			}
			value = null;
		}

        [MethodImpl(MethodImplOptions.NoInlining)]
        private new void SetExtraInfo(string fieldName, object val)
        {
            base.SetExtraInfo(fieldName, val);
        }

		#region Static
		public static ExtendedDataEditor Clone(string code)
		{
			ExtendedDataEditor o = new ExtendedDataEditor(code);
			return new ExtendedDataEditor(o);
		}
		#endregion

	}

	/// <summary>
	/// Returns Collection Editor
	/// </summary>
	internal class ReturnsExclusionsEditor : CollectionEditorEx
	{
		public ReturnsExclusionsEditor() : base (typeof(ReturnFieldsCollection)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (ReturnFieldsExclusion);
		}

		protected override object CreateInstance(System.Type t)
		{
			return new ReturnFieldsExclusion((ExtendedDataEditor)this.Context.Instance,"",null);
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(ReturnFieldsExclusion)};
		}
	}

	public class ReturnFieldsExclusion : ReturnFields
	{
		private ExtendedDataEditor _parent = null;

		public ReturnFieldsExclusion(ExtendedDataEditor Parent, string FieldName, XmlNode Node) : base (null, FieldName,Node)
		{
			_parent = Parent;
		}

		[LocCategory("Design")]
		[TypeConverter(typeof(FieldMappingExclusionTypeEditor))]
		public override string FieldName
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
		public new ExtendedDataEditor Parent
		{
			get
			{
				return _parent;
			}
		}
	}
    
	/// <summary>
	/// Summary description for FieldMappingTypeEditor.
	/// </summary>
	internal class FieldMappingExclusionTypeEditor : StringConverter
	{
		public override bool GetStandardValuesSupported(
			ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			string[] dt;
			try
			{
				dt = ((FWBS.OMS.UI.Windows.Design.ReturnFieldsExclusion)context.Instance).Parent.DataBuilder.TestAndGetFields();
			}
			catch
			{
				dt = new string[0]{};
			}
			return new StandardValuesCollection(dt);
		}
	}

}
