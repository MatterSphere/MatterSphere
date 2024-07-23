using System;
using System.ComponentModel;
using System.Xml;
using FWBS.Common;
using FWBS.OMS.SourceEngine;
using FWBS.OMS.UI.Windows.Design;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// 34000 DataListEditor for the Admin Kit
    /// </summary>
    public class DataListEditor : FWBS.OMS.EnquiryEngine.DataLists
	{
		#region Fields

		/// <summary>
		/// The DataBuilder that allows the user to Edit the Data Settings
		/// </summary>
		private DataBuilder _db = new DataBuilder();
        private bool _excludeAPI = true;

		#endregion

		#region Constructors

		/// <summary>
		/// Create a New Data List
		/// </summary>
		public DataListEditor() : this("")
		{
			_orgcode = "";
		}
		

		/// <summary>
		/// An Internal Contrutor to accept the DataListEditor Object
		/// </summary>
		/// <param name="Clone">A Data List Object</param>
		internal DataListEditor(DataListEditor Clone) : base("",true)
		{
			_searchlisttb.Rows.Add(Clone._searchlisttb.Rows[0].ItemArray);
			_searchlisttb.Rows[0]["enqTable"] = "";

			DataListEditorInt();
			_description = "";
		}


		/// <summary>
		/// Loads a Data List
		/// </summary>
		/// <param name="Code"></param>
		public DataListEditor(string Code) : base(Code)
		{
		}

        #endregion Constructors

		#region Properties

        protected override bool InDesignMode
        {
            get
            {
                return true;
            }
        }

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


        [LocCategory("MSAPI")]
        [Browsable(true)]
        [Description("MSAPIEXCLUDE")]
        public bool MSAPIEXCLUDE
        {

            get
            {
                return _excludeAPI;
            }



            set
            {
                _excludeAPI = value;
              
            }


        }


        private long NextVersionNumber
        {
            get
            {
                return _version + 1;
            }
        }

		#endregion Properties

		#region Methods

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
            _db.Call = Convert.ToString(_dr["enqCall"]);
            _db.Source = Convert.ToString(_dr["enqSource"]);
            _db.SourceType = (SourceType)Enum.Parse(typeof(SourceType), Convert.ToString(_dr["enqSourceType"]), true);
            _db.EnquiryForm = "";
            _excludeAPI = ConvertDef.ToBoolean(_dr["enqAPIExclude"], true);

        }

        /// <summary>
        /// Clones from a Previous Data List
        /// </summary>
        /// <param name="Code">The Code of the Previous Data List</param>
        /// <returns>Returns a Data List Object</returns>
        public static DataListEditor Clone(string Code)
        {
            DataListEditor o = new DataListEditor(Code);
            return new DataListEditor(o);
        }


		/// <summary>
		/// The Update Method to Commit Changes to the Data List to Disk
		/// </summary>
		public override void Update()
		{
			if (_description == "") 
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(new OMSException(HelpIndexes.OMSDescriptionNotSet));
				return;
			}

            _dr["UpdatedBy"] = Session.CurrentSession.CurrentUser.ID;
            _dr["Updated"] = DateTime.Now;
            _dr["enqApiExclude"] = _excludeAPI;

            long newversionnumber = GetNextVersionNumber(Convert.ToString(_dr["enqTable"]), (long)_version);//NextVersionNumber;
            _dr["enqDLVersion"] = newversionnumber;
            
            base.Update();

            base.Version = newversionnumber;// NextVersionNumber;
		}

        private long GetNextVersionNumber(string code, long currentversion)
        {
            VersionControlSupport vcs = new VersionControlSupport();
            return vcs.IncrementVersionNumber(code, currentversion, "dbDataListVersionData");
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
	
			_dr["enqSource"] = _db.Source;
			_dr["enqSourceType"] = _db.SourceType;
			_dr["enqCall"] = _db.Call;


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
                    WriteAttribute(_newnode, "kind", p.ParameterDateIs.ToString());
                    _xmlParameter.AppendChild(_newnode);
				}
			}
			_searchlisttb.Rows[0]["enqParameters"] = _xmlDParams.InnerXml;
			base.Parameters = _xmlDParams.InnerXml;
			base.Call = _db.Call;
			base.Src = _db.Source;
			base.SourceType = _db.SourceType;
            if (_db.SourceType != SourceEngine.SourceType.Instance)
                base.ReBind();
		}

		#endregion Methods
	}
}
