using System;
using System.Data;
using FWBS.OMS.Data.Exceptions;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{
    public class PowerProfile : IPowerProfile, IEnquiryCompatible
    {
        private DataTable data = new DataTable();

        //constructor when editing a Power Profile
        [EnquiryUsage(true)]
        public PowerProfile(int ID)
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("id", ID);
            data = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbPowerUserProfiles where id = @id", "PowerProfile", paramlist);
            if (data.Rows.Count > 0)
            {
                description = Convert.ToString(data.Rows[0]["Description"]);
                if (data.Rows[0]["PowerRoles"] != DBNull.Value)
                {
                    powerroles = EncryptionV2.Decrypt(Convert.ToString(data.Rows[0]["PowerRoles"]), "power");
                    powerroles = powerroles.Replace(",", ";");
                }
                if (data.Rows[0]["PowerMenuItem"] != DBNull.Value)
                {
                    powermenuitems = EncryptionV2.Decrypt(Convert.ToString(data.Rows[0]["PowerMenuItem"]), "power");
                }
            }
        }

        //constructor for a new Power Profile
        public PowerProfile()
        {
            data = GetTableSchemas();
            foreach (DataColumn col in data.Columns)
            {
                if (!col.AllowDBNull)
                    col.AllowDBNull = true;
            }
            data.Rows.Add(data.NewRow());
        }


        private static DataTable GetTableSchemas()
        {
            return Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbPowerUserProfiles", "PowerProfile", true, new IDataParameter[0]);
        }

        public int ID
        {
            get
            {
                return Convert.ToInt32(GetExtraInfo("id"));
            }
        }

        private string description;
        [EnquiryEngine.EnquiryUsage(true)]
        internal string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                isdirty = true;
                if (PropertyChanged != null)
                    PropertyChanged(this, new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("Description", value));
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

        public Type GetExtraInfoType(string fieldName)
        {
            return null;
        }

        public void SetExtraInfo(string fieldName, object val)
        {
            data.Rows[0][fieldName] = val;
        }

        public DataSet GetDataset()
        {
            return null;
        }

        public DataTable GetDataTable()
        {
            return data;
        }

        public object GetExtraInfo(string fieldName)
        {
            return data.Rows[0][fieldName];
        }

        #endregion

        #region IUpdateable Members

        public void Update()
        {
            try
            {
                data.Rows[0]["Description"] = description;
                if (this.PowerRoles != null)
                    data.Rows[0]["PowerRoles"] = EncryptionV2.Encrypt(this.PowerRoles, "power");
                if (this.PowerMenuItem != null)
                    data.Rows[0]["PowerMenuItem"] = EncryptionV2.Encrypt(this.PowerMenuItem, "power");
                data.Rows[0]["BranchID"] = Session.CurrentSession.CurrentBranch.ID;


                Session.CurrentSession.Connection.Update(data, "dbPowerUserProfiles", true);
                isdirty = false;
            }
            catch (ConnectionException conex)
            {
                if (conex.InnerException.Message.Contains(description))
                {
                    CreateProfileDescriptionValidationException();
                    return;
                }
                else
                    throw;
            }
        }

        private static void CreateProfileDescriptionValidationException()
        {
            throw new EnquiryValidationFieldException(HelpIndexes.EnquiryDuplicateKey,
                new ValidatedField(Session.CurrentSession.Resources.GetResource("PUPEX1", "Description", "").Text
                                , Session.CurrentSession.Resources.GetResource("PUPEX2", "The profile description must be unique.", "").Text
                                , null));
        }


        public static PowerProfile CloneProfile(int id)
        {
            PowerProfile sourceProfile = new PowerProfile(id);
            PowerProfile clonePowerProfile = new PowerProfile();
            clonePowerProfile.Description = null;
            clonePowerProfile.PowerRoles = sourceProfile.PowerRoles;
            clonePowerProfile.PowerMenuItem = sourceProfile.PowerMenuItem;
            return clonePowerProfile;
        }


        public static void DeleteProfile(int id)
        {
            try
            {
                IDataParameter[] paramlist = new IDataParameter[1];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("id", id);
                Session.CurrentSession.Connection.ExecuteSQLScalar("Delete from dbPowerUserProfiles where id = @id", paramlist);
            }
            catch (ConnectionException conex)
            {
                if (conex.InnerException.Message.Contains("usrPowerUserProfileID"))
                {
                    CreateProfileDeletionException();
                    return;
                }
                else
                    throw;
            }
        }

        private static void CreateProfileDeletionException()
        {
            throw new OMSException2("PUPEX3", "The profile cannot be deleted while it is has been assigned to one or more users.");
        }


        public static void ReassignProfile(FWBS.Common.KeyValueCollection[] SelectedUsers, object ProfileID)
        {
            try
            {
                if (SelectedUsers.Length > 0)
                {
                    foreach (FWBS.Common.KeyValueCollection kvc in SelectedUsers)
                    {
                        FWBS.OMS.User user = FWBS.OMS.User.GetUser(Convert.ToInt32(kvc["usrID"].Value));
                        user.SetExtraInfo("usrPowerUserProfileID", ProfileID);
                        if (ProfileID == DBNull.Value)
                        {
                            user.RemoveRole("POWER");
                        }
                        else
                        {
                            user.AddRole("POWER");
                        }
                        user.Update();
                    }
                }
            }
            catch
            {
                throw new OMSException2("PUPEX4", "An error has occurred during the reassignment of the Power User profile. Please contact the System Administrator");
            }
        }

        #endregion

    }
}
