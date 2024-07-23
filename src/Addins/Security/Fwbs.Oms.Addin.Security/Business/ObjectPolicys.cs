using System;
using System.Data;
using System.Text;
using System.Threading;
using FWBS.Common;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.Addin.Security
{
    /// <summary>
    /// Policy Business Object used by the Admiu Kit and ucPolicy
    /// </summary>
    public class ObjectPolicy : FWBS.OMS.CommonObject
    {
        public const string SystemSecurityCode = "f1c719f2-6c08-4b91-8beb-caf391fb6a8e";
        public const string SystemPolicyType = "GLOBALOBJDEF";

        #region Constructuors
        [EnquiryUsage(true)]
        public ObjectPolicy()
        {
            data = null;
            OMS = new ReportingServer("FWBS Limited 2005");
            Create();
        }

        [EnquiryUsage(true)]
        public ObjectPolicy(string ID)
        {
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
            get { return "ObjectPolicy"; }
        }

        protected override string SelectStatement
        {
            get { return "SELECT * FROM Config.ObjectPolicy"; }
        }
        #endregion

        #region Public
        /// <summary>
        /// Admin Kit List Policies
        /// </summary>
        /// <returns></returns>
        public DataTable ListPolicies(bool IncludeExp)
        {
            IDataParameter[] paramlist = new IDataParameter[4];
            paramlist[0] = OMS.Connection.AddParameter("UI", Thread.CurrentThread.CurrentCulture.Name);
            paramlist[1] = OMS.Connection.AddParameter("IsSystem", false);
            paramlist[2] = OMS.Connection.AddParameter("includeexp", IncludeExp);

            return OMS.Connection.ExecuteProcedureTable("Config.ListPolicyType", "PERMISSIONS", paramlist);
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

        /// <summary>
        /// List Object Policies
        /// </summary>
        /// <param name="Remote">True to List remote user policies, False for internal</param>
        /// <param name="IncludeNull">Include a null row</param>
        /// <returns>A list of</returns>
        public DataTable ListPolicies(bool Remote, bool IncludeNull)
        {
            string filter = "IsRemote = 'false' OR IsRemote IS NULL";
            if (Remote == true)
                filter = "IsRemote = 'true'";
            else
                filter = "IsRemote = 'false' OR IsRemote IS NULL";

            DataTable table = ListPolicies(false);
            try
            {
                table.DefaultView.RowFilter = filter;
                table = table.DefaultView.ToTable();

                if (IncludeNull)
                {
                    DataRow row = table.NewRow();
                    row["PolicyTypeCode"] = DBNull.Value;
                    row["PolicyDescription"] = Session.CurrentSession.Resources.GetResource("NOACCESS", "No Access", "").Text;
                    table.Rows.InsertAt(row, 0);
                }
            }
            catch (System.Data.EvaluateException evex)
            {
                if (evex.Message.Contains("Cannot find column [IsRemote]"))
                {
                    throw new Exception("Stored procedure 'ListPolicyType' is not up to date, it does not contain the column 'IsRemote'", evex);
                }
            }
            return table.DefaultView.ToTable();
        }

        public DataTable ListInternalPolicies(bool IncludeNull)
        {
            return ListPolicies(false, IncludeNull);
        }

        public DataTable ListRemotePolicies(bool IncludeNull)
        {
            return ListPolicies(true, IncludeNull);
        }




        protected override void Fetch(object id)
        {
            base.Fetch(id);
            name = CodeLookup.GetLookup("POLICY", this.Type);
        }
        #endregion

        #region Overrides
        public override void Create()
        {
            base.Create();
            this.ID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Admin Kit Clone Policy
        /// </summary>
        /// <param name="policy"></param>
        public void Clone(ObjectPolicy policy)
        {
            OMS = new ReportingServer("FWBS Limited 2005");
            Create();
            this.Name = "";
            this.Type = policy.Type;
            IDataParameter[] paramlist = new IDataParameter[2];
            paramlist[0] = OMS.Connection.AddParameter("policyTypeCode", DBNull.Value);
            paramlist[1] = OMS.Connection.AddParameter("PolicyID", policy.ID);
            data = OMS.Connection.ExecuteProcedureTable("Config.GetObjectPolicyPermissions", "PERMISSIONS", paramlist);
        }

        public bool ValidateUniqueType(string TypeCode)
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = OMS.Connection.AddParameter("Type", TypeCode);
            DataTable data = OMS.Connection.ExecuteSQLTable("SELECT * FROM Config.ObjectPolicy WHERE Type = @Type", "PERMISSIONS", paramlist);
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
            paramlist = new IDataParameter[9];
            paramlist[0] = OMS.Connection.AddParameter("policyTypeCode", this.Type);
            paramlist[2] = OMS.Connection.AddParameter("isSystemPolicy", false);
            paramlist[3] = OMS.Connection.AddParameter("name", this.Name);
            paramlist[4] = OMS.Connection.AddParameter("usrID", Session.CurrentSession.CurrentUser.ID);
            paramlist[5] = OMS.Connection.AddParameter("allowMask", "0x" + allowbinary);
            paramlist[6] = OMS.Connection.AddParameter("denyMask", "0x" + denybinary);
            paramlist[7] = OMS.Connection.AddParameter("ID", this.ID);
            paramlist[8] = OMS.Connection.AddParameter("IsRemote", this.IsRemote);

            if (this.IsNew)
            {
                OMS.Connection.ExecuteProcedure("Config.CreatePolicyTemplate", paramlist);
            }
            else
            {
                OMS.Connection.ExecuteProcedure("Config.UpdatePolicyTemplate", paramlist);
            }
            CodeLookup.Create("POLICY", this.Type, this.Name, "", CodeLookup.DefaultCulture, true, true, true);
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

        /// <summary>
        /// Is this policy for remote users
        /// </summary>
        [EnquiryUsage(true)]
        public bool IsRemote
        {
            get
            {
                return ConvDef.ToBoolean(GetExtraInfo("IsRemote"), false);
            }
            set
            {
                SetExtraInfo("IsRemote", value);
            }
        }

        public override void Delete()
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = OMS.Connection.AddParameter("policyID", this.ID);
            OMS.Connection.ExecuteProcedure("Config.DeleteObjectPolicy", paramlist);
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
                        paramlist[0] = OMS.Connection.AddParameter("policyTypeCode", SystemPolicyType);
                        paramlist[1] = OMS.Connection.AddParameter("PolicyID", SystemSecurityCode);
                    }
                    else
                    {
                        paramlist[0] = OMS.Connection.AddParameter("policyTypeCode", DBNull.Value);
                        paramlist[1] = OMS.Connection.AddParameter("PolicyID", this.ID);
                    }
                    data = OMS.Connection.ExecuteProcedureTable("Config.GetObjectPolicyPermissions", "PERMISSIONS", paramlist);
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
    }
}
