using System;
using System.Data;
using System.Text;
using System.Threading;
using FWBS.Common;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.Addin.Security
{
    /// <summary>
    /// Policy Business Object used by the Admin Kit and ucPolicy
    /// </summary>
    public class SystemPolicy : FWBS.OMS.CommonObject
    {
        public string SystemPolicyID = "3cc3bd00-7d7e-4d4a-96c6-44e44e140c5e";
        public string SystemPolicyCode = "GLOBALSYSDEF";

        #region Constructuors
        [EnquiryUsage(true)]
        public SystemPolicy()
        {
            GetDefaultSystemPolicy();

            data = null;
            OMS = new ReportingServer("FWBS Limited 2005");
            Create();
        }

        [EnquiryUsage(true)]
        public SystemPolicy(string ID)
        {
            GetDefaultSystemPolicy();

            OMS = new ReportingServer("FWBS Limited 2005");
            this.Fetch(ID);
        }

        #endregion

        #region Private
        private ReportingServer OMS = null;
        private DataTable data = null;
        private string name;
        #endregion

        #region Implement CommonObject
        protected override string DefaultForm
        {
        get { return ""; }
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
            get { return "SystemPolicy"; }
        }

        protected override string SelectStatement
        {
            get { return "SELECT * FROM Config.SystemPolicy"; }
        }
        #endregion

        #region Public
        /// <summary>
        /// Admin Kit List Policies
        /// </summary>
        /// <returns></returns>
        public DataTable ListPolicies(bool IncludeExp)
        {
            IDataParameter[] paramlist = new IDataParameter[3];
            paramlist[0] = OMS.Connection.AddParameter("UI", Thread.CurrentThread.CurrentCulture.Name);
            paramlist[1] = OMS.Connection.AddParameter("IsSystem", true);
            paramlist[2] = OMS.Connection.AddParameter("includeexp", IncludeExp);
            return OMS.Connection.ExecuteProcedureTable("Config.ListPolicyType", "PERMISSIONS", paramlist);
        }

        protected override void Fetch(object id)
        {
            base.Fetch(id);
            name = CodeLookup.GetLookup("POLICY", this.Type);
        }

        /// <summary>
        /// Admin Kit Get Policy
        /// </summary>
        /// <param name="ID"></param>
        public void GetPolicy(string ID)
        {
            data = null;
            Fetch(ID);
        }
        #endregion

        #region Overrides
        public override void Create()
        {
            data = null;
            base.Create();
            this.ID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Admin Kit Clone Policy
        /// </summary>
        /// <param name="policy"></param>
        public void Clone(SystemPolicy policy)
        {
            OMS = new ReportingServer("FWBS Limited 2005");
            Create();
            this.Name = "";
            this.Type = policy.Type;
            IDataParameter[] paramlist = new IDataParameter[2];
            paramlist[0] = OMS.Connection.AddParameter("policyTypeCode", DBNull.Value);
            paramlist[1] = OMS.Connection.AddParameter("PolicyID", policy.ID);
            data = OMS.Connection.ExecuteProcedureTable("Config.GetSystemPolicyPermissions", "PERMISSIONS", paramlist);

        }

        public bool ValidateUniqueType(string TypeCode)
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = OMS.Connection.AddParameter("Type", TypeCode);
            DataTable data = OMS.Connection.ExecuteSQLTable("SELECT * FROM Config.SystemPolicy WHERE Type = @Type", "PERMISSIONS", paramlist);
            return (data.Rows.Count == 0);
        }
        
        /// <summary>
        /// Admin Kit Update Policy
        /// </summary>
        public override void Update()
        {
            IDataParameter[] paramlist = new IDataParameter[6];
            DataView view;
            view = new DataView(this.Permissions);
            System.Text.StringBuilder build = new StringBuilder();
            build.Append("|");
            int allowmask = 0;
            int denymask = 0;
            string allowbinary = "";
            string denybinary = "";
            int mbyte = 0;
            foreach (DataRowView dr in view)
            {
                if (mbyte == 0)
                {
                    while (Convert.ToInt32(dr["Byte"]) > mbyte + 1)
                    {
                        allowbinary += "00";
                        denybinary += "00";
                        mbyte++;
                    }
                    mbyte = Convert.ToInt32(dr["Byte"]);
                }
                if (mbyte != Convert.ToInt32(dr["Byte"]))
                {
                    string AX = allowmask.ToString("X");
                    allowbinary += AX.PadLeft(2, '0');
                    string DX = denymask.ToString("X");
                    denybinary += DX.PadLeft(2, '0');
                    allowmask = 0;
                    denymask = 0;
                    while (Convert.ToInt32(dr["Byte"]) > mbyte + 1)
                    {
                        allowbinary += "00";
                        denybinary += "00";
                        mbyte++;
                    }
                    mbyte = Convert.ToInt32(dr["Byte"]);
                }

                if (Convert.ToBoolean(dr["Allow"]))
                    allowmask += Convert.ToInt32(dr["BitValue"]);
                if (Convert.ToBoolean(dr["Deny"]))
                    denymask += Convert.ToInt32(dr["BitValue"]);
            }

            string AX2 = allowmask.ToString("X");
            allowbinary += AX2.PadLeft(2, '0');
            string DX2 = denymask.ToString("X");
            denybinary += DX2.PadLeft(2, '0');

            while (mbyte <= 32)
            {
                allowbinary += "00";
                denybinary += "00";
                mbyte++;
            }
            paramlist = new IDataParameter[8];
            paramlist[0] = OMS.Connection.AddParameter("policyTypeCode", this.Type);
            paramlist[2] = OMS.Connection.AddParameter("isSystemPolicy", true);
            paramlist[3] = OMS.Connection.AddParameter("name", this.Name);
            paramlist[4] = OMS.Connection.AddParameter("usrID", Session.CurrentSession.CurrentUser.ID);
            paramlist[5] = OMS.Connection.AddParameter("allowMask", "0x" + allowbinary);
            paramlist[6] = OMS.Connection.AddParameter("denyMask", "0x" + denybinary);
            if (this.IsNew)
            {
                paramlist[7] = OMS.Connection.AddParameter("ID",this.ID );
                OMS.Connection.ExecuteProcedure("Config.CreatePolicyTemplate", paramlist);
            }
            else
            {
                paramlist[7] = OMS.Connection.AddParameter("ID", this.ID);
                OMS.Connection.ExecuteProcedure("Config.UpdatePolicyTemplate", paramlist);
            }
            CodeLookup.Create("POLICY", this.Type, this.Name, "", CodeLookup.DefaultCulture, true, true, true);
            this.Fetch(ID);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Admin Kit Policy ID
        /// </summary>
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
                

        /// <summary>
        /// Admin Kit Policy Name
        /// </summary>
        [EnquiryUsage(true)]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        
        /// <summary>
        /// Admin Kit Policy Name
        /// </summary>
        [EnquiryUsage(true)]
        public string Type
        {
            get
            {
                return Convert.ToString(GetExtraInfo("Type"));
            }
            set
            {
                SetExtraInfo("Type", value);
            }
        }
        

        public override void Delete()
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = OMS.Connection.AddParameter("policyID", this.ID);
            OMS.Connection.ExecuteProcedure("Config.DeleteSystemPolicy", paramlist);
        }

       
        /// <summary>
        /// Returns the Default System Policy ID and Code
        /// </summary>
        /// <returns></returns>
        public static DataTable DefaultSystemPolicy()
        {
            ReportingServer OMS = new ReportingServer("FWBS Limited 2005");
            return OMS.Connection.ExecuteProcedureTable("Config.GetDefaultSystemPolicy", "DEFAULTPOLICY", null);            
        }

        
        /// <summary>
        /// Sets the Default System Policy properties
        /// </summary>
        private void GetDefaultSystemPolicy()
        {
            DataTable _dt = DefaultSystemPolicy();

            if (_dt != null)
            {
                SystemPolicyCode = _dt.Rows[0]["SystemPolicyCode"].ToString();
                SystemPolicyID =_dt.Rows[0]["SystemPolicyID"].ToString();
                System.Diagnostics.Debug.WriteLine(string.Format("Default System Policy found : [{0}][{1}]", SystemPolicyCode, SystemPolicyID), "ADVSECURITY");
            }
        }

                   
        /// <summary>
        /// List Policys for the Addin
        /// </summary>
        public DataTable Permissions
        {
            get
            {
                if (data == null)
                {
                    IDataParameter[] paramlist = new IDataParameter[2];
                    if (IsNew)
                    {
                        paramlist[0] = OMS.Connection.AddParameter("policyTypeCode", SystemPolicyCode);
                        paramlist[1] = OMS.Connection.AddParameter("PolicyID", SystemPolicyID);
                    }
                    else
                    {
                        paramlist[0] = OMS.Connection.AddParameter("policyTypeCode", this.Type);
                        paramlist[1] = OMS.Connection.AddParameter("PolicyID", this.ID);
                    }
                    data = OMS.Connection.ExecuteProcedureTable("Config.GetSystemPolicyPermissions", "PERMISSIONS", paramlist);
                    if (IsNew)
                    {
                        foreach (DataRow dr in data.Rows)
                        {
                            dr["Allow"] = false;
                            dr["Deny"] = false;
                        }
                    }
                }
                return data;
            }
        }
        #endregion

        #region Users
        public void GetPolicyByUserID(int UserID)
        {
            IDataParameter[] paramlist = new IDataParameter[2];
            paramlist[0] = OMS.Connection.AddParameter("userID", UserID);
            _data = OMS.Connection.ExecuteProcedureTable("Config.GetSystemPolicy", PrimaryTableName, paramlist);
            if (_data.Rows.Count == 0)
                Create();
        }

        public void GetPolicyByGroupID(string groupid)
        {
            IDataParameter[] paramlist = new IDataParameter[2];
            paramlist[0] = OMS.Connection.AddParameter("groupid", groupid);
            _data = OMS.Connection.ExecuteProcedureTable("Config.GetSystemPolicy", PrimaryTableName, paramlist);
            if (_data.Rows.Count == 0)
                Create();
        }

        #endregion


        /// <summary>
        /// Has the policy been assigned to a user or group
        /// </summary>
        public bool Assigned
        {
            get
            {
                IDataParameter[] paramlist = new IDataParameter[1];
                paramlist[0] = OMS.Connection.AddParameter("policyID", this.ID);
                return ConvertDef.ToBoolean(OMS.Connection.ExecuteProcedureScalar(@"config.IsSystemPolicyAssigned", paramlist), false);
            }
        }


        /// <summary>
        /// A flag that indicates whether the System Policy is the Default or not.
        /// </summary>
        public bool Default
        {
            get
            {
                return ConvertDef.ToBoolean(GetExtraInfo("isDefault") , false);
            }
        }
        

        /// <summary>
        /// Flag this System Policy as the Default
        /// </summary>
        public void FlagAsDefault()
        {
            if (this.ID == null)
                throw new Exception("System Policy required");

            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = OMS.Connection.AddParameter("policyID", this.ID);
            OMS.Connection.ExecuteProcedureScalar(@"config.SetDefaultSystemPolicy", paramlist);
        }



    }
}
