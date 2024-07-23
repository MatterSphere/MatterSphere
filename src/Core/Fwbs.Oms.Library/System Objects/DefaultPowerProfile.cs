using FWBS.Common;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace FWBS.OMS
{
    public class DefaultPowerProfile : IPowerProfile, IEnquiryCompatible
    {
        private DataTable data = new DataTable();

        public DefaultPowerProfile()
        {
            data.Columns.Add("cdAddLink");
            data.Columns.Add("PowerMenuItem");
            data.Columns.Add("PowerRoles");
            data.Rows.Add(data.NewRow());

            if (Convert.ToBoolean(Session.CurrentSession.GetXmlProperty("PowerUpgrade", false)) == false)
            {
                IDataParameter[] paramlist = new IDataParameter[2];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("cdType", "POWER");
                paramlist[1] = Session.CurrentSession.Connection.AddParameter("cdCode", "SETTINGS");
                DataTable _data = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbCodeLookup where cdType = @cdType AND cdCode = @cdCode", "Import", paramlist);
                if (_data.Rows.Count > 0)
                {
                    if (_data.Rows[0]["cdNotes"] != DBNull.Value)
                    {
                        powerroles = Encryption.Decrypt(Convert.ToString(_data.Rows[0]["cdHelp"]));
                        powermenuitems = Encryption.Decrypt(Convert.ToString(_data.Rows[0]["cdDesc"]));
                        powerroles = powerroles.Replace(",", ";");
                        powermenuitems = powermenuitems.Replace(",", ";");
                    }
                }
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(Convert.ToString(Session.CurrentSession.GetXmlProperty("PowerRoles", null))))
                {
                    var roles = EncryptionV2.Decrypt(Convert.ToString(Session.CurrentSession.GetXmlProperty("PowerRoles", null)), "power");
                    if (roles.Contains(","))
                        this.PowerRoles = roles.Replace(",", ";");
                    else
                        this.PowerRoles = roles;
                }
                else
                {
                    this.PowerRoles = null;
                }

                if (!String.IsNullOrWhiteSpace(Convert.ToString(Session.CurrentSession.GetXmlProperty("PowerMenuItem", null))))
                {
                    var menu = EncryptionV2.Decrypt(Convert.ToString(Session.CurrentSession.GetXmlProperty("PowerMenuItem", null)), "power");
                    this.PowerMenuItem = menu;
                }
                else
                {
                    this.PowerMenuItem = null;
                }

                this.isdirty = false;
            }
        }


        private string powermenuitems;
        [EnquiryEngine.EnquiryUsage(true)]
        public string PowerMenuItem
        {
            get
            {
                return powermenuitems;
            }
            set
            {
                powermenuitems = value;
                isdirty = true;
                if (PropertyChanged != null)
                    PropertyChanged(this, new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("PowerMenuItem", value));
            }
        }

        private string powerroles;
        [EnquiryEngine.EnquiryUsage(true)]
        public string PowerRoles
        {
            get
            {
                return powerroles;
            }
            set
            {
                powerroles = value;
                isdirty = true;
                if (PropertyChanged != null)
                    PropertyChanged(this, new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("PowerRoles", value));
            }
        }

        #region IEnquiryCompatible Members

        public event FWBS.OMS.EnquiryEngine.PropertyChangedEventHandler PropertyChanged;

        public FWBS.OMS.EnquiryEngine.Enquiry Edit(FWBS.Common.KeyValueCollection param)
        {
            return null;
        }

        public FWBS.OMS.EnquiryEngine.Enquiry Edit(string customForm, FWBS.Common.KeyValueCollection param)
        {
            return null;
        }

        #endregion

        #region IExtraInfo Members


        public Type GetExtraInfoType(string fieldName)
        {
            return null;
        }

        public void SetExtraInfo(string fieldName, object val)
        {

        }

        public DataSet GetDataset()
        {
            return null;
        }

        public DataTable GetDataTable()
        {
            return data;
        }

        #endregion

        #region IUpdateable Members


        public void Refresh()
        {
        }

        public void Refresh(bool applyChanges)
        {
        }

        public void Cancel()
        {
        }

        private bool isdirty = false;

        public bool IsDirty
        {
            get
            {
                return isdirty;
            }
        }

        public bool IsNew
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region IParent Members

        public object Parent
        {
            get { return null; }
        }

        #endregion

        #region IExtraInfo Members

        public object GetExtraInfo(string fieldName)
        {
            switch (fieldName.ToLowerInvariant())
            {
                case "cdaddlink":
                    return 2;
                default:
                    return null;
            }
        }

        #endregion

        #region IUpdateable Members

        public void Update()
        {
            if (this.PowerRoles != null)
                Session.CurrentSession.SetXmlProperty("PowerRoles", EncryptionV2.Encrypt(this.PowerRoles, "power"));
            if (this.PowerMenuItem != null)
                Session.CurrentSession.SetXmlProperty("PowerMenuItem", EncryptionV2.Encrypt(this.PowerMenuItem, "power"));

            Session.CurrentSession.SetXmlProperty("PowerUpgrade", true);
            Session.CurrentSession.Update();
            isdirty = false;
        }

        #endregion

    }
}
