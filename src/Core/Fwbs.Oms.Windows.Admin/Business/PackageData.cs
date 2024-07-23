using System;
using System.ComponentModel;
using System.Xml;
using FWBS.OMS.SourceEngine;
using FWBS.OMS.UI.Windows.Design;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// PackageData for the Admin Kit
    /// </summary>
    public class PackageData : FWBS.OMS.Design.Package.PackageData
	{
		#region Fields
		/// <summary>
		/// The DataBuilder that allows the user to Edit the Data Settings
		/// </summary>
		private DataBuilder _db = new DataBuilder();
		private DataBuilder _overrideaselect = new DataBuilder();

		#endregion

		#region Constructors
		/// <summary>
		/// Create a New Data List
		/// </summary>
		public PackageData() : this("")
		{
			_orgcode = "";
		}
		
		/// <summary>
		/// Clones from a Previous PackageData
		/// </summary>
		/// <param name="Code">The Code of the Previous PackageData</param>
		/// <returns>Returns a PackageData Object</returns>
		public static PackageData Clone(string Code)
		{
			PackageData o = new PackageData(Code);
			return new PackageData(o);
		}

		/// <summary>
		/// An Internal Contrutor to accept the PackageData Object
		/// </summary>
		/// <param name="Clone">A PackageData Object</param>
		internal PackageData(PackageData Clone) : base("",true)
		{
			_searchlisttb.Rows.Add(Clone._searchlisttb.Rows[0].ItemArray);
			_searchlisttb.Rows[0]["pkdCode"] = "";
			DataListEditorInt();
			_description = "";
		}

		/// <summary>
		/// Loads a Data List
		/// </summary>
		/// <param name="Code"></param>
		public PackageData(string Code) : base(Code)
		{

		}

		/// <summary>
		/// A Private Method to allow reuse of multi contructors
		/// </summary>
		protected override void DataListEditorInt()
		{
			base.DataListEditorInt();
			_db.Parameters.Clear();
			_db.ParametersInx.Clear();
			_db.ResetFields();
			_db.XMLParameters = _xmlDParams.OuterXml;
			_db.Call = Convert.ToString(_dr["pkdCall"]);
			_db.Source = Convert.ToString(_dr["pkdSource"]);
			_db.SourceType = (SourceType)Enum.Parse(typeof(SourceType),Convert.ToString(_dr["pkdSourceType"]),true);
			_db.EnquiryForm = "";

			if (_dr["pkdUpdateSelect"] != DBNull.Value)
			{
				try
				{
					FWBS.Common.ConfigSetting _data = new FWBS.Common.ConfigSetting(_dr,"pkdUpdateSelect");
					_overrideaselect.SourceType = (FWBS.OMS.SourceEngine.SourceType)FWBS.Common.ConvertDef.ToEnum(_data.GetString("SourceType"),FWBS.OMS.SourceEngine.SourceType.OMS);
					_overrideaselect.Source = _data.GetString("Source","");
					_overrideaselect.Call = _data.GetString("Call","");
				}
				catch
				{
					_overrideaselect = new DataBuilder();
					_dr["pkdUpdateSelect"] = DBNull.Value;
				}

			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Sets if a Parent Object is Required
		/// </summary>
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
		
		/// <summary>
		/// The Data Builder Object used to Edit the Data Settings
		/// </summary>
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
				UpdateParameters();
			}
		}

		[LocCategory("Override")]
		[DataBuilderSourceTypeExclude(SourceType.Class | SourceType.Object | SourceType.Instance)]
		[DataBuilderParametersPanel(false)]
		[Lookup("UPDATESELECT")]
		public new DataBuilder UpdateSelect
		{
			get
			{
				return _overrideaselect;
			}
			set
			{
				_overrideaselect = value;
				FWBS.Common.ConfigSetting _data = new FWBS.Common.ConfigSetting(_dr,"pkdUpdateSelect");
				_data.SetString("SourceType",value.SourceType.ToString());
				_data.SetString("Source",value.Source);
				_data.SetString("Call",value.Call);
				_data.Synchronise();
			}
		}
		#endregion

		#region Public
		/// <summary>
		/// The Update Method to Commit Changes to the Data List to Disk
		/// </summary>
		public override void Update()
		{
			if (_description == "") 
			{
				throw new  OMSException(HelpIndexes.OMSDescriptionNotSet);
			}
            if (this.DataBuilder.Call == "")
            {
                throw new OMSException2("ERRMISSDB", "You must fill in the Data Builder");
            }
            base.Update();
		}

		/// <summary>
		/// Updates the Base Objects Settings from the Data Builder
		/// </summary>
		private void UpdateParameters()
		{
			for(int ui = _xmlParameter.ChildNodes.Count-1; ui > -1; ui--)
			{
				XmlNode dr = _xmlParameter.ChildNodes[ui];
				_xmlParameter.RemoveChild(dr);
			}
	
			_dr["pkdSource"] = _db.Source;
			_dr["pkdSourceType"] = _db.SourceType;
			_dr["pkdCall"] = _db.Call;
			foreach (Parameter p in _db.Parameters)
			{
				if (p.Node != null)
				{
					WriteAttribute(p.Node,"name",p.SQLParameter);
					WriteAttribute(p.Node,"type",p.FieldType.ToString());
					WriteAttribute(p.Node,"test",p.TestValue);
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
					_xmlParameter.AppendChild(_newnode);
				}
			}
			_searchlisttb.Rows[0]["pkdParameters"] = _xmlDParams.InnerXml;
			base.Parameters = _xmlDParams.InnerXml;
			base.Call = _db.Call;
			base.Src = _db.Source;
			base.SourceType = _db.SourceType;
			base.ReBind();
		}
		#endregion
	}
}
