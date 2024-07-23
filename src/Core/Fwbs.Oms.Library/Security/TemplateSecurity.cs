using System;
using System.Data;

namespace FWBS.OMS.Security
{
    [System.ComponentModel.Editor("FWBS.OMS.Addin.Security.TemplateEditor,FWBS.OMS.Addin.Security", typeof(System.Drawing.Design.UITypeEditor))]
    [System.ComponentModel.TypeConverter("FWBS.OMS.Addin.Security.TemplateConverter,FWBS.OMS.Addin.Security")]
    public class TemplateSecurity
    {
        private bool _hassecurity;
        private string _code;
        private string _type;

        public TemplateSecurity(string Type, string Code)
        {
            _code = Code;
            _type = Type;
            if (Session.CurrentSession.AdvancedSecurity)
            {
                IDataParameter[] paramlist = new IDataParameter[2];
                paramlist[0] = FWBS.OMS.Session.CurrentSession.Connection.AddParameter("Type", Type);
                paramlist[1] = FWBS.OMS.Session.CurrentSession.Connection.AddParameter("Code", Code);
                DataTable dt = FWBS.OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT TOP 1 PolicyID FROM Config.ConfigurableTypePolicy_UserGroup WHERE SecurableType = @Type AND SecurableTypeCode = @Code", "YES", paramlist);
                if (dt.Rows.Count > 0)
                    _hassecurity = true;
                else
                    _hassecurity = false;
            }
        }

        public bool HasSecurity
        {
            get
            {
                return _hassecurity;
            }
            set
            {
                _hassecurity = value;
            }
        }
        
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }


        public string Type
        {
            get
            {
                return _type;
            }
        }

        public override string ToString()
        {
            if (Session.CurrentSession.AdvancedSecurity)
            {
                if (_hassecurity)
                    return "{Edit Template}";
                else
                    return "{None Set)";
            }
            else
                return "{Disabled)";
        }

        private bool eraseonsave = false;

        public void ClearSecurity()
        {
            eraseonsave = true;
            _hassecurity = false;
        }

        public void Update()
        {
            if (eraseonsave)
            {
                if (Session.CurrentSession.AdvancedSecurity)
                {
                    IDataParameter[] paramlist = new IDataParameter[2];
                    paramlist[0] = FWBS.OMS.Session.CurrentSession.Connection.AddParameter("SecurableType", Type);
                    paramlist[1] = FWBS.OMS.Session.CurrentSession.Connection.AddParameter("SecurableID", Code);
                    FWBS.OMS.Session.CurrentSession.Connection.ExecuteProcedure("config.RemoveConfigurabeTypePolicy", paramlist);
                }
            }
        }

        internal void ApplySecurity(Int64 SecurableID)
        {
            if (Session.CurrentSession.AdvancedSecurity)
            {
                IDataParameter[] paramlist = new IDataParameter[4];
                paramlist[0] = FWBS.OMS.Session.CurrentSession.Connection.AddParameter("SecurableType", Type);
                paramlist[1] = FWBS.OMS.Session.CurrentSession.Connection.AddParameter("SecurableID", SecurableID);
                paramlist[2] = FWBS.OMS.Session.CurrentSession.Connection.AddParameter("configurableTypeCode", Code);
                paramlist[3] = FWBS.OMS.Session.CurrentSession.Connection.AddParameter("adminUserID", Session.CurrentSession.CurrentUser.ID);
                FWBS.OMS.Session.CurrentSession.Connection.ExecuteProcedure("config.ApplyConfigurableTypePolicy", paramlist);
            }
        }
    }
}
