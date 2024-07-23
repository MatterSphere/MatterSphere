using System;
using System.Data;
using System.Text;

namespace FWBS.OMS.Addin.Security.Business
{
    public class GroupPermissions : IPermissions
    {
        private ReportingServer OMS = null;
        private UserGroups current = null;
        private string policyid;
        private System.Data.DataTable permissions = null;

        internal GroupPermissions(UserGroups group)
        {
            OMS = new ReportingServer("FWBS Limited 2005");
            current = group;
            SystemPolicy policy = new SystemPolicy();
            policy.GetPolicyByGroupID(current.ID);
            permissions = policy.Permissions;
            if (policy.Type == "")
                policyid = group.PolicyID;
            else if (policy.Type != "EXPLICITSYS")
                policyid = policy.ID;
            else
                policyid = "";
        }

        public string PolicyID
        {
            get { return policyid; }
            set
            {
                policyid = value;
                if (value != "")
                {
                    SystemPolicy policy = new SystemPolicy();
                    policy.GetPolicy(value);
                    permissions = policy.Permissions;
                }
            }
        }

        public UserGroups UserGroup
        {
            get
            {
                return current;
            }
        }

        public System.Data.DataTable Permissions
        {
            get
            {
                return permissions;
            }
        }


        public string UpdatePermission()
        {
            IDataParameter[] paramlist;
            DataView view = new DataView(this.Permissions);
            view.RowStateFilter = DataViewRowState.CurrentRows;
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

            paramlist = new IDataParameter[6];
            paramlist[0] = OMS.Connection.AddParameter("policyID", SqlDbType.Char, ParameterDirection.InputOutput, 36, PolicyID != "" ? (object)PolicyID : DBNull.Value);
            paramlist[1] = OMS.Connection.AddParameter("groupID", current.ID);
            paramlist[3] = OMS.Connection.AddParameter("adminUserID", Session.CurrentSession.CurrentUser.ID);
            paramlist[4] = OMS.Connection.AddParameter("allowMask", "0x" + allowbinary);
            paramlist[5] = OMS.Connection.AddParameter("denyMask", "0x" + denybinary);
            OMS.Connection.ExecuteProcedure("Config.ApplySystemPolicy", paramlist);
            FWBS.OMS.Security.SecurityManager.CurrentManager.Refresh(FWBS.OMS.Session.CurrentSession);
            current.PolicyID = Convert.ToString(paramlist[0].Value).ToLowerInvariant();
            return current.PolicyID;
        }
    }
}
