using System;
using System.Data;

namespace FWBS.OMS.Addin.Security.Windows
{
    public partial class ucObjectGroups : FWBS.OMS.UI.Windows.Admin.ucEditBase2
    {
        private string _code = "";
        
        public ucObjectGroups()
        {
            InitializeComponent();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
        }

        protected override string SearchListName
        {
            get
            {
                return "SECOBJECTS";
            }
        }

        protected override void NewData()
        {
            DataTable data = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("SCRSECNEWUSRGRP", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, true, new FWBS.Common.KeyValueCollection()) as DataTable;
            if (data != null)
            {
                FWBS.OMS.ReportingServer rep = new ReportingServer("FWBS Limited 2005");

                IDataParameter[] paramlist = new IDataParameter[6];
                paramlist[0] = rep.Connection.AddParameter("groupDescription", data.Rows[0]["txtGroupDescription"]);
                paramlist[1] = rep.Connection.AddParameter("groupName", data.Rows[0]["txtGroupName"]);
                paramlist[2] = rep.Connection.AddParameter("groupIsDefault", data.Rows[0]["chkGroupIsDefault"]);
                paramlist[3] = rep.Connection.AddParameter("groupActive", data.Rows[0]["chkGroupActive"]);
                paramlist[4] = rep.Connection.AddParameter("flag", "1");
                paramlist[5] = rep.Connection.AddParameter("return", SqlDbType.Int,ParameterDirection.ReturnValue,8,null);
                rep.Connection.ExecuteProcedureScalar("secGroupListing", paramlist);
                object ret = paramlist[5].Value;

                string users = Convert.ToString(data.Rows[0]["UserSelections"]);
                
                foreach(string s in users.Split(";".ToCharArray()))
                {
                    paramlist = new IDataParameter[2];
                    paramlist[0] = rep.Connection.AddParameter("usrID", s);
                    paramlist[1] = rep.Connection.AddParameter("secGroupID", ret);
                    rep.Connection.ExecuteSQL("INSERT secGroup_dbUser ( secGroupID , usrID ) VALUES ( @secGroupID , @usrID )", paramlist);
                }
                base.Load();
            }

        }

        protected override void DeleteData(string Code)
        {
            FWBS.OMS.ReportingServer rep = new ReportingServer("FWBS Limited 2005");

            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = rep.Connection.AddParameter("groupID", Code);
            rep.Connection.ExecuteSQL("DELETE FROM secGroup WHERE secGroupID = @groupID", paramlist);
            base.Load();
        }

        protected override void LoadSingleItem(string Code)
        {
            labSelectedObject.Text = string.Format("{0} - {1}", Code, UI.Windows.ResourceLookup.GetLookupText("UserGroup", "User Group", ""));
            FWBS.Common.KeyValueCollection p = new FWBS.Common.KeyValueCollection();
            p.Add("Code", Code);
            _code = Code;
                 
            enquiryForm1.Enquiry = FWBS.OMS.EnquiryEngine.Enquiry.GetEnquiry("SCRSECNEWUSRGRP", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, false, p);


            FWBS.OMS.ReportingServer rep = new ReportingServer("FWBS Limited 2005");

            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = rep.Connection.AddParameter("groupID", Code);

            DataTable dt = rep.Connection.ExecuteSQLTable("SELECT usrID FROM secGroup_dbUser WHERE secGroupID = @groupID", "TABLE",paramlist);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (DataRow d in dt.Rows)
            {
                s.Append(Convert.ToInt32(d[0]));
                s.Append(";");
            }
            if (s.Length > 0)
                enquiryForm1.GetIBasicEnquiryControl2("UserSelections").Value = s.ToString().TrimEnd(";".ToCharArray());
            base.ShowEditor();
        }

        protected override bool UpdateData()
        {
            enquiryForm1.Update();
            FWBS.OMS.ReportingServer rep = new ReportingServer("FWBS Limited 2005");

            string users = Convert.ToString(enquiryForm1.GetIBasicEnquiryControl2("UserSelections").Value);

            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = rep.Connection.AddParameter("groupID", _code);
            rep.Connection.ExecuteSQL("DELETE FROM secGroup_dbUser WHERE secGroupID = @groupID", paramlist);

            foreach (string s in users.Split(";".ToCharArray()))
            {
                paramlist = new IDataParameter[2];
                paramlist[0] = rep.Connection.AddParameter("usrID", s);
                paramlist[1] = rep.Connection.AddParameter("secGroupID", _code);
                rep.Connection.ExecuteSQL("INSERT secGroup_dbUser ( secGroupID , usrID ) VALUES ( @secGroupID , @usrID )", paramlist);
            }
            base.ShowList();
            return true;
        }
    }
}
