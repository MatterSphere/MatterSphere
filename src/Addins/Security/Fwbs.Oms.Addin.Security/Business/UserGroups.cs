using System;
using System.Data;
using FWBS.Common;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.Addin.Security
{
    //TODO: Convert this to New Tables

    public class UserGroups : FWBS.OMS.CommonObject
    {
        #region Constructuors
        [EnquiryUsage(true)]
        public UserGroups()
        {
            OMS = new ReportingServer("FWBS Limited 2005");
            Create();
        }

        [EnquiryUsage(true)]
        public UserGroups(string ID)
        {
            OMS = new ReportingServer("FWBS Limited 2005");
            this.Fetch(ID);
        }
        #endregion

        #region Private
        private string _users;
        private FWBS.OMS.ReportingServer OMS = null;
        #endregion

        #region Implement CommonObject
        protected override string DefaultForm
        {
        get { return "SCRSECNEWUSRGRP"; }
        }

        protected override string FieldActive
        {
            get
            {
                return "Active";
            }
        }

        public override string FieldPrimaryKey
        {
            get { return "ID"; }
        }

        public override object Parent
        {
            get { return null; }
        }

        protected override string PrimaryTableName
        {
            get { return "Group"; }
        }

        protected override string SelectStatement
        {
            get { return "SELECT * FROM [Item].[Group]"; }
        }
        #endregion

        #region Public
        public void GetUserGroup(string ID)
        {
            Fetch(ID);
        }
        #endregion

        #region Overrides

        public override void Create()
        {
            base.Create();
            this.ID = Guid.NewGuid().ToString();
            this.PolicyID = DefaultSystemPolicyID;
        }
        #endregion

        #region Default System Policy
        /// <summary>
        /// Returns the Default System Policy Code
        /// </summary>
        /// <returns></returns>
        public string DefaultSystemPolicyID
        {
            get
            {
                string _defaultSysPolicyID = "3cc3bd00-7d7e-4d4a-96c6-44e44e140c5e";
                DataTable _dt = SystemPolicy.DefaultSystemPolicy();
                
                if (_dt != null)
                    _defaultSysPolicyID = _dt.Rows[0]["SystemPolicyID"].ToString();

                System.Diagnostics.Debug.WriteLine(string.Format("UserGroups class. System Policy ID {0}", _defaultSysPolicyID), "ADVSECURITY");
                return _defaultSysPolicyID;
            }
        }
        #endregion

        #region Properties
        public string ID
        {
            get
            {
                return Convert.ToString(GetExtraInfo("ID"));
            }
            private set 
            {
                SetExtraInfo("ID", value);
            }
        }

        public string DistinguishedName
        {
            get
            {
                return Convert.ToString(GetExtraInfo("ADDistinguishedName"));
            }
            set
            {
                SetExtraInfo("ADDistinguishedName", value);
            }
        }

        [EnquiryUsage(true)]
        public string PolicyID
        {
            get
            {
                return Convert.ToString(GetExtraInfo("PolicyID"));
            }
            set
            {
                SetExtraInfo("PolicyID", string.IsNullOrEmpty(value) ? (object)DBNull.Value : value);
            }
        }


        [EnquiryUsage(true)]
        public string GroupName
        {
            get
            {
                return Convert.ToString(GetExtraInfo("Name"));
            }
            set
            {
                SetExtraInfo("Name", value);
            }
        }

        [EnquiryUsage(true)]
        public string GroupDescription
        {
            get
            {
                return Convert.ToString(GetExtraInfo("Description"));
            }
            set
            {
                SetExtraInfo("Description", value);
            }
        }

        [EnquiryUsage(true)]
        public bool Active
        {
            get
            {
                return ConvertDef.ToBoolean(GetExtraInfo("Active"),true);
            }
            set
            {
                SetExtraInfo("Active", value);
            }
        }

        [EnquiryUsage(true)]
        public string Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
            }
        }
        #endregion

        #region Methods
        public DataTable ListActiveUserGroupsAndUsers()
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = OMS.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            DataTable data = OMS.Connection.ExecuteProcedureTable("config.ListActiveUserGroupsAndUsers", "USERGROUPS", paramlist);
            return data;
        }


        public DataTable ListUsersForThisGroup()
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = OMS.Connection.AddParameter("ID", this.ID);
            DataTable dt = OMS.Connection.ExecuteProcedureTable("config.ListUsersForThisGroup", "TABLE", paramlist);
            return dt;
        }
        #endregion

        #region Static Methods
        public static string GetGroupName(string SecID)
        {
            UserGroups ug = new UserGroups();
            ug.GetUserGroup(SecID);
            return  CodeLookup.GetLookup("SECGROUPS",ug.GroupName);
        }
        #endregion
    }
}
