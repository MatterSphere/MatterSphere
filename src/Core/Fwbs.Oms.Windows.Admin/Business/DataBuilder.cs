using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;
using FWBS.OMS.SearchEngine;
using FWBS.OMS.SourceEngine;

namespace FWBS.OMS.UI.Windows.Design
{
    public enum DataBuilderMode {ListMode,ExtendedDataMode,EnquiryMode}
	[Editor(typeof(DataBuilderEditor),typeof(UITypeEditor))]
	[TypeConverter(typeof(DataBuilderConverter))]
	public class DataBuilder : FWBS.OMS.SourceEngine.Source
	{
		private ParameterCollection _Parameters = new ParameterCollection();
		private Hashtable _Parametersinx = new Hashtable();
		private ReturnFieldsCollection _fieldscol = new ReturnFieldsCollection();
        private string[] _fields = null;
        private string[] _fieldnames = null;
        private string _enquiryform = "";
		public string TableName = "";
		public string Where = "";

		/// <summary>
		/// Static Binding flags used when getting type information on instance methods and properties.
		/// </summary>
		private const BindingFlags _memberBindingStatic = BindingFlags.Static | BindingFlags.Public;

		public DataBuilder()
		{
		}

		protected override void SetParameter(string name, out object value)
		{
			value = DBNull.Value;
			foreach (Parameter kvi in this.Parameters)
			{
				if (kvi.BoundValue.Replace("%","").ToUpper() == name.ToUpper())
				{
					if (kvi.TestValue.ToUpper() == "(DBNULL)")
						value = DBNull.Value;
                    else if (kvi.TestValue.ToUpper() == "(NULL)")
                        value = null;
                    else
						value = kvi.TestValue;
					return;
				}
			}
		}

		public void ResetFields()
		{
            _fields = null;
            _fieldnames = null;
		}

        public string[] TestAndGetFields()
        {
            try
            {
                if (_fieldnames != null)
                    return _fieldnames;

                string lastcall = this.Call;
                if (this.TableName != "")
                    this.Call = "SELECT TOP 1 * FROM " + TableName;

                base.Parameters = XMLParameters;
                object returned = this.Run(false, false);
                if (returned is DataTable)
                {
                    DataTable dt = returned as DataTable;
                    _fieldnames = GetColumnList(false, dt);
                }
                else if (returned is FWBS.OMS.Interfaces.IExtraInfo)
                {
                    FWBS.OMS.Interfaces.IExtraInfo iei = returned as FWBS.OMS.Interfaces.IExtraInfo;
                    DataTable dt = iei.GetDataTable() as DataTable;
                    _fieldnames = GetColumnList(false, dt);
                }
                this.Call = lastcall;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("ERR:" + ex.Message, "DataBuilder");
            }
            return _fieldnames;
        }

        
        public string[] TestAndGetFieldAndTypes()
		{
            try
            {
                if (_fields != null)
                    return _fields;

                string lastcall = this.Call;
                if (this.TableName != "")
                    this.Call = "SELECT TOP 1 * FROM " + TableName;

                base.Parameters = XMLParameters;
                object returned = this.Run(false, false);
                if (returned is DataTable)
                {
                    DataTable dt = returned as DataTable;
                    _fields = GetColumnList(true, dt);
                }
                else if (returned is FWBS.OMS.Interfaces.IExtraInfo)
                {
                    FWBS.OMS.Interfaces.IExtraInfo iei = returned as FWBS.OMS.Interfaces.IExtraInfo;
                    DataTable dt = iei.GetDataTable() as DataTable;
                    _fields = GetColumnList(true, dt);
                }
                this.Call = lastcall;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("ERR:" + ex.Message, "DataBuilder");
            }
			return _fields;
		}



        private string[] GetColumnList(bool IncludeType, DataTable dt)
        {
            List<string> fields = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ExtendedProperties.ContainsKey("InVisible") == false)
                {
                    if (IncludeType)
                        fields.Add(dc.ColumnName + "," + dc.DataType.Name);
                    else
                        fields.Add(dc.ColumnName);
                }
            }
            return fields.ToArray();
        }
		
		public DataBuilder(string Call, ParameterCollection Parameters, string EnquiryForm)
		{
			_Parameters = Parameters;
			_enquiryform = EnquiryForm;
			base.Call = Call;
			BuildIndex(Parameters);
		}

		public void BuildIndex()
		{
			BuildIndex(_Parameters);
		}

		private void BuildIndex(ParameterCollection source)
		{
			_Parametersinx.Clear();
			foreach (Parameter p in source)
				_Parametersinx.Add(p.SQLParameter,p.SQLParameter);
		}

		[Browsable(false)]
		public string EnquiryForm
		{
			get
			{
				return _enquiryform;
			}
			set
			{
				_enquiryform = value;
			}
		}	
		
		public ReturnFieldsCollection Fields
		{
			get
			{
				return _fieldscol;
			}
			set
			{
				_fieldscol = value;
			}
		}	
		
		[ReadOnly(true)]
		public new SourceType SourceType
		{
			get
			{
				return base.SourceType;
			}
			set
			{
				base.SourceType = value;
			}
		}	

		[ReadOnly(true)]
		public string Source
		{
			get
			{
				return base.Src;
			}
			set
			{
				base.Src = value;
			}
		}	

		[Editor(typeof(ParameterEditor),typeof(UITypeEditor))]
		public new ParameterCollection Parameters
		{
			get
			{
				return _Parameters;
			}
			set
			{
				_Parameters = value;
				base.Parameters = XMLParameters;
			}
		}

		[Browsable(false)]
		public Hashtable ParametersInx
		{
			get
			{
				return _Parametersinx;
			}
			set
			{
				_Parametersinx = value;
			}
		}

		[ReadOnly(true)]
		public new string Call
		{
			get
			{
				return base.Call;
			}
			set
			{
				base.Call = value;
			}
		}

		[Browsable(false)]
		public string XMLParameters
		{
			get
			{
				FWBS.Common.ConfigSetting _param = new FWBS.Common.ConfigSetting("");
				_param.Current = "params";
				foreach (Parameter p in _Parameters)
				{
					FWBS.Common.ConfigSettingItem itm = _param.AddChildItem("param");
					itm.SetString("name",p.SQLParameter);
					itm.SetString("type",p.FieldType);
					itm.SetString("test",p.TestValue);
                    itm.SetString("kind",p.ParameterDateIs.ToString());
                    itm.SetString(p.BoundValue);
				}
				return _param.DocObject.OuterXml;
			}
			set
			{
				this.Parameters.Clear();
				//
				// XML Parameters
				//
				XmlDocument _xmlDParams = new XmlDocument();
				_xmlDParams.PreserveWhitespace = false;
				_xmlDParams.LoadXml(value);
				XmlNode _xmlParameter = _xmlDParams.DocumentElement.SelectSingleNode("/params");

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
                    this.Parameters.Add(new Parameter(this, __name, __type, __value, __test, (SearchParameterDateIs)FWBS.Common.ConvertDef.ToEnum(__kind, SearchParameterDateIs.NotApplicable), dr.Clone()));
				}
			}
		}
	}
	public class Parameter
	{
		private System.Xml.XmlNode _node = null;
		private string _SQLParameter = "";
		private string _fieldtype = "";
		private string _BoundValue = "";
		private string _testvalue = "";
		private DataBuilder _db = null;
        private SearchParameterDateIs _dateis = SearchParameterDateIs.NotApplicable;

        public Parameter(DataBuilder db, string SQLParameter, string FieldType, string BoundValue, string TestValue, SearchParameterDateIs DateIs)
		{
			IntParameter(db,SQLParameter,FieldType,BoundValue,TestValue, DateIs, null);
		}

        public Parameter(DataBuilder db, string SQLParameter, string FieldType, string BoundValue, string TestValue, SearchParameterDateIs DateIs, System.Xml.XmlNode Node)
		{
			IntParameter(db,SQLParameter,FieldType,BoundValue,TestValue,DateIs,Node);
		}

        private void IntParameter(DataBuilder db, string SQLParameter, string FieldType, string BoundValue, string TestValue, SearchParameterDateIs DateIs, System.Xml.XmlNode Node)
		{
			_db = db;
			_SQLParameter = SQLParameter;
			_fieldtype = FieldType;
			_BoundValue = BoundValue;
			_testvalue = TestValue;
            _dateis = DateIs;
			_node=Node;
            if (_fieldtype.ToLower().IndexOf("datetime") > -1 && _dateis == SearchParameterDateIs.NotApplicable)
                _dateis = SearchParameterDateIs.UTC;

		}

		public override string ToString()
		{
			return _SQLParameter;
		}

        [LocCategory("SQL")]
        [DefaultValue(SearchColumnsDateIs.NotApplicable)]
        [Description("If the Parameter is Date type, please choose the kind of date it is. UTC or LOCAL")]
        public SearchParameterDateIs ParameterDateIs
        {
            get
            {
                return _dateis;
            }
            set
            {
                if (_dateis == SearchParameterDateIs.NotApplicable && value != SearchParameterDateIs.NotApplicable)
                    throw new OMSException2("ERRNOTDATEFIELD", "This is not a Date Field");
                if (value == SearchParameterDateIs.NotApplicable && _dateis != SearchParameterDateIs.NotApplicable)
                    throw new OMSException2("ERRDATEFIELD", "This is a Date Field you must choose UTC or Local");
                _dateis = value;
            }
        }

		[DefaultValue("")]
		[Category("SQL")]
		[Description("The SQL Paremeter name used in the Stored Procedure or Select Statement")]
		public string SQLParameter
		{
			get
			{
				return _SQLParameter;
			}
			set
			{
				_SQLParameter = value;
			}
		}

		[DefaultValue("")]
		[Category("SQL")]
		[Description("The Type of SQL Parameter used")]
		[TypeConverter(typeof(EnquryControlTypeTypeEditor))]
		public string FieldType
		{
			get
			{
				return _fieldtype;
			}
			set
			{
				_fieldtype = value;
                if (_fieldtype.ToLower().IndexOf("datetime") > -1 && _dateis == SearchParameterDateIs.NotApplicable)
                    _dateis = SearchParameterDateIs.UTC;
                else
                    _dateis = SearchParameterDateIs.NotApplicable;
			}
		}

		[DefaultValue("")]
		[Category("OMS")]
		[Description("The Bound Value used by OMS which must be surrounded by % symbols it may also be a field from the Enquiry Form")]
		[TypeConverter(typeof(EnquryControlNameTypeEditor))]
		public string BoundValue
		{
			get
			{
				return _BoundValue;
			}
			set
			{
				_BoundValue = value;
			}
		}


		[DefaultValue("")]
		[Category("OMS")]
		[Description("This is the Test Literal Value used to populate the Bound Value Variable for Testing")]
		[TypeConverter(typeof(DefaultTestValueEditor))]
		public string TestValue
		{
			get
			{
				return _testvalue;
			}
			set
			{
				_testvalue = value;
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
		public DataBuilder DataBuilderParent
		{
			get
			{
				return _db;
			}
		}
	}


	/// <summary>
	/// Summary description for DataBuilderEditor.
	/// </summary>
	internal class DataBuilderConverter : TypeConverter
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
				string enqform = "";
				if (ctx.Instance is SearchListEditor)
					enqform = ((SearchListEditor)ctx.Instance).EnquiryForm;
				if (ctx.Instance is ReportListEditor)
					enqform = ((ReportListEditor)ctx.Instance).EnquiryForm;
				if (data == "")
					return new DataBuilder("",new ParameterCollection(),enqform);
				else
					return base.ConvertFrom ( ctx , culture , value );
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
			if (value is DataBuilder)
			{
				DataBuilder db = (DataBuilder)value;
				if (db.Source == "" && db.Parameters.Count==0 && (db.Call == "" || db.Call == null))
					return "Data Builder (Empty)";
				else
					return "Data Builder";
			}
			else
				return "";
		}
	}
		
	/// <summary>
	/// Summary description for DataBuilderEditor.
	/// </summary>
	internal class DataBuilderEditor : UITypeEditor
	{
		private IWindowsFormsEditorService iWFES;
		public DataBuilderEditor(){}


		public override UITypeEditorEditStyle GetEditStyle (ITypeDescriptorContext ctx)
		{
			return UITypeEditorEditStyle.Modal ; 
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			DataBuilder inew = new DataBuilder();
			if (value != null)
			{
				DataBuilder db = (DataBuilder)value;
				inew.SourceType = db.SourceType;
				inew.Source = (string)db.Source.Clone();
				inew.Call = (string)db.Call.Clone();
				inew.Parameters = (ParameterCollection)db.Parameters.Clone();
				inew.EnquiryForm = db.EnquiryForm;
				inew.ParametersInx = (Hashtable)db.ParametersInx.Clone();
				inew.TableName = (string)db.TableName.Clone();
				inew.Where = (string)db.Where.Clone();
				inew.ChangeParent(db.Parent);
			}
			FWBS.OMS.UI.Windows.Design.frmDataBuilder frmDataBuilder1 = new FWBS.OMS.UI.Windows.Design.frmDataBuilder(inew);

			DataBuilderAttribute mode = context.PropertyDescriptor.Attributes[typeof(DataBuilderAttribute)] as DataBuilderAttribute;
			if (mode != null) frmDataBuilder1.Mode = mode.Value;

			DataBuilderSourceTypeExcludeAttribute exclude = context.PropertyDescriptor.Attributes[typeof(DataBuilderSourceTypeExcludeAttribute)] as DataBuilderSourceTypeExcludeAttribute;
			if (exclude != null) frmDataBuilder1.ExcludeSourceType = exclude.Value;

			DataBuilderParametersPanelAttribute parpnl = context.PropertyDescriptor.Attributes[typeof(DataBuilderParametersPanelAttribute)] as DataBuilderParametersPanelAttribute;
			if (parpnl != null) frmDataBuilder1.ParametersPanel = parpnl.Value;

			iWFES.ShowDialog(frmDataBuilder1);
			if (frmDataBuilder1.DialogResult == DialogResult.OK)
				value = frmDataBuilder1.Value;
			return value;
		}
	}


	/// <summary>
	/// Summary description for EnquryControlTypeTypeEditor.
	/// </summary>
	internal class EnquryControlTypeTypeEditor : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			string[] vals;
			Array arr = Enum.GetValues(typeof(SqlDbType));
			vals = new string[arr.Length];
			for (int ctr = 0; ctr < arr.Length; ctr++)
			{
				vals[ctr] = Enum.GetName(typeof(SqlDbType), arr.GetValue(ctr));
			}
			return new StandardValuesCollection(vals);
		}
	}
	
	/// <summary>
	/// Summary description for EnquryControlNameTypeEditor.
	/// </summary>
	internal class EnquryControlNameTypeEditor : StringConverter
	{
		public override bool GetStandardValuesSupported(
			ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			if (((Parameter)context.Instance).DataBuilderParent.EnquiryForm == "ßEXTENDEDDATAß")
			{
				string[] ar = "%fileID%,%fileAccCode%,%fileNo%,%clID%,%clNo%,%clAccCode%,%contID%".Split(",".ToCharArray());
				return new StandardValuesCollection(ar);
			}
			else
			{
				DataTable dt = EnquiryEngine.Enquiry.GetControlNames(((Parameter)context.Instance).DataBuilderParent.EnquiryForm,true);
				ArrayList ar = new ArrayList();
				foreach(DataRow row in dt.Rows)
					ar.Add(row[0]);
				return new StandardValuesCollection(ar);
			}
		}
	}

	internal class DefaultTestValueEditor : StringConverter
	{
		public override bool GetStandardValuesSupported(
			ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			string[] ar = "(DBNULL),en-gb,(NULL)".Split(",".ToCharArray());
			return new StandardValuesCollection(ar);
		}
	}
	

	/// <summary>
	/// Parameter Collection Editor
	/// </summary>
	internal class ParameterEditor : CollectionEditorEx
	{
		public ParameterEditor() : base (typeof(ParameterCollection)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (Parameter);
		}

		protected override object CreateInstance(System.Type t)
		{
            return new Parameter((DataBuilder)this.Context.Instance, "", "NVarChar", "", "", SearchParameterDateIs.NotApplicable);
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(Parameter)};
		}
	}

	public class ParameterCollection : Crownwood.Magic.Collections.CollectionWithEvents
	{
		public ParameterCollection Clone()
		{
			ParameterCollection clone = new ParameterCollection();
			foreach(Parameter page in base.List)
			{
				Parameter p = new Parameter(page.DataBuilderParent, page.SQLParameter,page.FieldType,page.BoundValue,page.TestValue,page.ParameterDateIs);
				clone.Add(p);
			}
			return clone;
		}

		public Parameter Add(Parameter value)
		{
			// Use base class to process actual collection operation
			base.List.Add(value as object);
			return value;
		}

		public void AddRange(Parameter[] values)
		{
			// Use existing method to add each array entry
			foreach(Parameter page in values)
				Add(page);
		}

		public void Remove(Parameter value)
		{
			// Use base class to process actual collection operation
			base.List.Remove(value as object);
		}

		public void Insert(int index, Parameter value)
		{
			// Use base class to process actual collection operation
			base.List.Insert(index, value as object);
		}

		public bool Contains(Parameter value)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(value as object);
		}

		public Parameter this[int index]
		{
			// Use base class to process actual collection operation
			get { return (base.List[index] as Parameter); }
		}

		public int IndexOf(Parameter value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}
	}

	[AttributeUsage(AttributeTargets.All)]
	public class DataBuilderAttribute : Attribute 
	{
		DataBuilderMode _value;
		public DataBuilderAttribute(DataBuilderMode Value)
		{
			_value = Value;
		}

		public DataBuilderMode Value
		{
			get
			{
				return _value;
			}
		}
	}

	[AttributeUsage(AttributeTargets.All)]
	public class DataBuilderSourceTypeExcludeAttribute : Attribute 
	{
		SourceType _value;
		public DataBuilderSourceTypeExcludeAttribute(SourceType Value)
		{
			_value = Value;
		}

		public SourceType Value
		{
			get
			{
				return _value;
			}
		}
	}

	[AttributeUsage(AttributeTargets.All)]
	public class DataBuilderParametersPanelAttribute : Attribute 
	{
		bool _value;
		public DataBuilderParametersPanelAttribute(bool Value)
		{
			_value = Value;
		}

		public bool Value
		{
			get
			{
				return _value;
			}
		}
	}
}
