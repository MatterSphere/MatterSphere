using System;

namespace FWBS.OMS
{
    /// <summary>
    /// An object that exposes and deals with Departments.
    /// </summary>
    public class Department : CommonObject
	{

		#region Constructors

		/// <summary>
		/// Creates a new billing information item from scratch.
		/// </summary>
		[EnquiryEngine.EnquiryUsage(true)]
        public Department(string code)
            : base(code)
		{
		}


        public Department()
            : base()
		{
            _data.Rows[0][FieldPrimaryKey] = "";
			Session.CurrentSession.CheckLoggedIn();
			
		}

        protected override void Fetch(object id)
        {
            base.Fetch(id);
            deptDesc = FWBS.OMS.CodeLookup.GetLookup("DEPT", Code);
        }

        public override void Update()
        {
            if (IsDirty)
            {
                if (xmlprops != null)
                    xmlprops.Update();
            }

            FWBS.OMS.CodeLookup.Create("DEPT", Code, deptDesc, "", "{default}", true, true, true);
            base.Update();

            
        }

		#endregion

		#region CommonObject Implementation

		protected override string DefaultForm
		{
			get
			{
				return null;
			}
		}

		public override string FieldPrimaryKey
		{
			get
			{
				return "deptCode";
			}
		}

		public override object Parent
		{
			get
			{
				return null;
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "DEPARTMENT";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "select * from dbDepartment";
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the unique identifier of the department.
		/// </summary>
        [EnquiryEngine.EnquiryUsage(true)]
		public string Code
		{
			get
			{
				return Convert.ToString(GetExtraInfo(FieldPrimaryKey));
			}
            set
			{
				SetExtraInfo(FieldPrimaryKey, value);
			}

		}

		/// <summary>
		/// Gets or Sets the departments email
		/// </summary>
        [EnquiryEngine.EnquiryUsage(true)]
        public string Email
		{
			get
			{
				return Convert.ToString(GetExtraInfo("deptEmail"));
			}
            set
            {
                SetExtraInfo("deptEmail", value);
            }
		}

		/// <summary>
		/// Gets or Sets if the department is active.
		/// </summary>
        [EnquiryEngine.EnquiryUsage(true)]
        public bool Active
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("deptActive"));
			}
			set
			{
                SetExtraInfo("deptActive", value);
			}
		}

		/// <summary>
		/// Gets or Sets the departments account code.
		/// </summary>
        [EnquiryEngine.EnquiryUsage(true)]
        public string AccountCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("deptAccCode"));
			}
			set
			{
				if (value == null && value == "")
                    SetExtraInfo("deptAccCode", DBNull.Value);
				else
                    SetExtraInfo("deptAccCode", value);
			}
		}

        string deptDesc;
        [EnquiryEngine.EnquiryUsage(true)]
        public string Description
        {
            get { return deptDesc; }
            set { deptDesc = value; }
        }

        /// <summary>
        /// The Action types that are displayed in the Context menu within TaskFlow
        /// Valid values are 0 = FileAndTask, 1 = FileOnly, 2= TaskOnly, 4 = None
        /// </summary>
        [EnquiryEngine.EnquiryUsage(true)]
        public int ActionsInContextMenu
        {
            get
            {
                return Common.ConvDef.ToInt32(GetXmlProperty("ActionsInContextMenu", null),0);
            }
            set
            {
                if (!Enum.IsDefined(typeof(FileManagement.ActionsToDisplay), value))
                    throw new Exception(string.Format("{0} is not a valid value for ActionsInContextMenu", Convert.ToString(value)));
                SetXmlProperty("ActionsInContextMenu", value);
            }
        }

        /// <summary>
        /// The order that the available Actions are displayed in the Context menu within TaskFlow
        /// Valid values are 0= FileActions1st, 1 = TaskActions1st
        /// </summary>
        [EnquiryEngine.EnquiryUsage(true)]
        public int ActionsOrderContextMenu
        {
            get
            {
                return Common.ConvDef.ToInt32(GetXmlProperty("ActionsOrderContextMenu", null),0);
            }
            set
            {
                if (!Enum.IsDefined(typeof(FileManagement.ActionsOrderType), value))
                    throw new Exception(string.Format("{0} is not a valid value for ActionsOrderContextMenu",Convert.ToString(value)));
                SetXmlProperty("ActionsOrderContextMenu", value);
            }
        }

        /// <summary>
        /// Gets or Sets the task list code for the tasks search list that will be displayed on the tasks tab for users in this department
        /// If a value is specified at the user level then that will take precedence.
        /// </summary>
        [EnquiryEngine.EnquiryUsage(true)]
        public string TasksAddinCommandCentreSearchListOverride
        {
            get
            {
                return Convert.ToString(GetXmlProperty("TasksAddinCommandCentreSearchListOverride", null));
            }
            set
            {
                SetXmlProperty("TasksAddinCommandCentreSearchListOverride", value);
            }
        }

		#endregion

        #region XML Settings Methods

        private XmlProperties xmlprops = null;

        private void BuildXML()
        {
            //Create the document if it does not already exist.
            if (xmlprops == null)
                xmlprops = new XmlProperties(this, "deptXML");
          
        }

        public object GetXmlProperty(string name, object defaultValue)
        {
            BuildXML();
            return xmlprops.GetProperty(name, defaultValue);
        }

        public void SetXmlProperty(string name, object val)
        {
            BuildXML();
            if (xmlprops.SetProperty(name, val))
                IsDirty = true;
        }


        #endregion

	}

}
