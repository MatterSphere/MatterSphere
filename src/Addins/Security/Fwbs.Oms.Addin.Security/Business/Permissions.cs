using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using FWBS.OMS.Interfaces;
using FWBS.OMS.Security.Permissions;

namespace FWBS.OMS.Addin.Security
{
    public class Permissions
    {
        private ReportingServer OMS = null;
        private DataTable _permissions = null;
        private DataTable _permissionsparties = null;
        public const string Everyone = "Everyone";
        public const string FullControl = "Full Control";
        internal IExtraInfo ObjType;
        private object ObjectID;
        internal string ObjectType;

        /// <summary>
        /// For filtering Permissions by user access type
        /// If nothing is specified the stored procedure defaults to INTERNAL
        /// </summary>
        public string AccessType { get; set; }

        internal Permissions(string Type, string Code)
        {
            OMS = new ReportingServer("FWBS Limited 2005");
            ObjectType = Type;
            ObjectID = Code;
        }

        public Permissions(IExtraInfo Object)
        {
            OMS = new ReportingServer("FWBS Limited 2005");
            ObjType = Object;
            ObjectType = ObjType.GetType().Name.Replace("OMS", "");
            switch (ObjectType)
            {
                case "Client":
                    ObjectID = Convert.ToInt64(((FWBS.OMS.Client)ObjType).ClientID);
                    break;
                case "File":
                    ObjectID = Convert.ToInt64(((FWBS.OMS.OMSFile)ObjType).ID);
                    break;
                case "Document":
                    ObjectID = Convert.ToInt64(((FWBS.OMS.OMSDocument)ObjType).ID);
                    break;
                case "Contact":
                    ObjectID = Convert.ToInt64(((FWBS.OMS.Contact)ObjType).ID);
                    break;
            }
        }

        public DataTable ListParties()
        {
            if (_permissionsparties == null || _permissionsparties.Namespace != ObjectID + ObjectType)
            {
                IDataParameter[] paramlist = new IDataParameter[4];

                if (!string.IsNullOrEmpty(AccessType))
                    paramlist[3] = OMS.Connection.AddParameter("AccessType", AccessType);

                switch (ObjectType)
                {
                    case "Client":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("clID", ObjectID);
                            paramlist[1] = OMS.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                            _permissionsparties = OMS.Connection.ExecuteProcedureTable("Config.ListClientAssociatedUserGroups", "PARTIES", paramlist);
                            break;
                        }
                    case "File":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("fileID", ObjectID);
                            paramlist[1] = OMS.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                            _permissionsparties = OMS.Connection.ExecuteProcedureTable("Config.ListFileAssociatedUserGroups", "PARTIES", paramlist);
                            break;
                        }
                    case "Document":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("documentID", ObjectID);
                            paramlist[1] = OMS.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                            _permissionsparties = OMS.Connection.ExecuteProcedureTable("Config.ListDocumentAssociatedUserGroups", "PARTIES", paramlist);
                            break;
                        }
                    case "Contact":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("contactID", ObjectID);
                            paramlist[1] = OMS.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                            _permissionsparties = OMS.Connection.ExecuteProcedureTable("Config.ListContactAssociatedUserGroups", "PARTIES", paramlist);
                            break;
                        }
                    case "FileType":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("securableType", ObjectType);
                            paramlist[1] = OMS.Connection.AddParameter("configurableTypeCode", ObjectID);
                            paramlist[2] = OMS.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                            _permissionsparties = OMS.Connection.ExecuteProcedureTable("Config.ListConfigurableTypeUserGroups", "PARTIES", paramlist);
                            break;
                        }
                    case "ClientType":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("securableType", ObjectType);
                            paramlist[1] = OMS.Connection.AddParameter("configurableTypeCode", ObjectID);
                            paramlist[2] = OMS.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                            _permissionsparties = OMS.Connection.ExecuteProcedureTable("Config.ListConfigurableTypeUserGroups", "PARTIES", paramlist);
                            break;
                        }
                    case "ContactType":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("securableType", ObjectType);
                            paramlist[1] = OMS.Connection.AddParameter("configurableTypeCode", ObjectID);
                            paramlist[2] = OMS.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                            _permissionsparties = OMS.Connection.ExecuteProcedureTable("Config.ListConfigurableTypeUserGroups", "PARTIES", paramlist);
                            break;
                        }
                }
                _permissionsparties.Columns.Add("Modified");
                _permissionsparties.Namespace = ObjectID + ObjectType;
            }
            return _permissionsparties;
        }



        public DataTable ListPermissions(string GroupID)
        {
            if (_permissions != null) _permissions.DefaultView.RowFilter = "";
            if (_permissions == null || _permissions.TableName != ObjectID + ObjectType)
            {
                IDataParameter[] paramlist = new IDataParameter[3];
                switch (ObjectType)
                {
                    case "Client":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("clid", ObjectID);
                            _permissions = OMS.Connection.ExecuteProcedureTable("Config.GetClientSecurity", ObjectID + ObjectType, paramlist);
                            break;
                        }
                    case "File":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("fileID", ObjectID);
                            _permissions = OMS.Connection.ExecuteProcedureTable("Config.GetFileSecurity", ObjectID + ObjectType, paramlist);
                            break;
                        }
                    case "Document":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("documentID", ObjectID);
                            _permissions = OMS.Connection.ExecuteProcedureTable("Config.GetDocumentSecurity", ObjectID + ObjectType, paramlist);
                            break;
                        }
                    case "Contact":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("contactID", ObjectID);
                            _permissions = OMS.Connection.ExecuteProcedureTable("Config.GetContactSecurity", ObjectID + ObjectType, paramlist);
                            break;
                        }
                    case "FileType":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("securableType", "FileType");
                            paramlist[1] = OMS.Connection.AddParameter("configurableTypeCode", ObjectID);
                            _permissions = OMS.Connection.ExecuteProcedureTable("Config.GetConfigurableTypeSecurity", ObjectID + ObjectType, paramlist);
                            break;
                        }
                    case "ClientType":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("securableType", "ClientType");
                            paramlist[1] = OMS.Connection.AddParameter("configurableTypeCode", ObjectID);
                            _permissions = OMS.Connection.ExecuteProcedureTable("Config.GetConfigurableTypeSecurity", ObjectID + ObjectType, paramlist);
                            break;
                        }
                    case "ContactType":
                        {
                            paramlist[0] = OMS.Connection.AddParameter("securableType", "ContactType");
                            paramlist[1] = OMS.Connection.AddParameter("configurableTypeCode", ObjectID);
                            _permissions = OMS.Connection.ExecuteProcedureTable("Config.GetConfigurableTypeSecurity", ObjectID + ObjectType, paramlist);
                            break;
                        }
                }
            }
            switch (ObjectType)
            {
                case "Client":
                    {
                        _permissions.DefaultView.RowFilter = "NodeLevel >= 1";
                        break;
                    }
                case "File":
                    {
                        _permissions.DefaultView.RowFilter = "NodeLevel >= 2";
                        break;
                    }
                case "Document":
                    {
                        _permissions.DefaultView.RowFilter = "NodeLevel >= 3";
                        break;
                    }
                case "Contact":
                    {
                        _permissions.DefaultView.RowFilter = "NodeLevel >= 0.10 and NodeLevel <= 0.15";
                        break;
                    }
                case "FileType":
                    {
                        _permissions.DefaultView.RowFilter = "NodeLevel >= 2";
                        break;
                    }
                case "ContactType":
                    {
                        _permissions.DefaultView.RowFilter = "NodeLevel >= 0.10 and NodeLevel <= 0.15";
                        break;
                    }
                case "ClientType":
                    {
                        _permissions.DefaultView.RowFilter = "NodeLevel >= 1";
                        break;
                    }
            }
            if (GroupID != "")
            {
                if (_permissions.DefaultView.RowFilter == "")
                    _permissions.DefaultView.RowFilter = "ID = '" + GroupID + "'";
                else
                    _permissions.DefaultView.RowFilter = _permissions.DefaultView.RowFilter + " AND ID = '" + GroupID + "'";
            }
            return _permissions;
        }

        public void Refresh()
        {
            _permissions = null;
            _permissionsparties = null;
            ListObjectParties();
            ListObjectPartiesPermission(Convert.ToString(_permissionsparties.Rows[0]["GroupID"]));
        }

        internal DataTable ListObjectParties()
        {
            DataTable data = ListParties();
            return data;
        }

        internal DataTable ListObjectPartiesPermission(string GroupID)
        {
            return ListPermissions(GroupID);
        }

        public void ModifiedPolicy(string GroupID)
        {
            DataView po = new DataView(_permissionsparties, "GroupID = '" + GroupID + "'", "", DataViewRowState.CurrentRows);
            po[0]["Modified"] = DateTime.Now;
            _permissionsparties.AcceptChanges();
        }

        public void ChangePolicy(string GroupID, string NewPolciyID)
        {
            DataView po = new DataView(_permissionsparties, "GroupID = '" + GroupID + "'", "", DataViewRowState.CurrentRows);
            if (NewPolciyID == "")
                po[0]["Policy"] = DBNull.Value;
            else
                po[0]["Policy"] = NewPolciyID;
            _permissionsparties.AcceptChanges();
        }


        public void DeletePermission(string GroupID)
        {
            //CM - Permission Parties does include External Users as AF amended the 'List...AssociatedUserGroups' stored procs
            DataView view = new DataView(_permissions);
            view.RowFilter = "ID = '" + GroupID + "'";
            view.RowStateFilter = DataViewRowState.OriginalRows;
            foreach (DataRowView rv in view)
                rv.Delete();

            view = new DataView(_permissionsparties);
            view.RowFilter = "GroupID = '" + GroupID + "'";
            view.RowStateFilter = DataViewRowState.OriginalRows;
            foreach (DataRowView rv in view)
                rv.Delete();
        }

        public bool UpdatePermission()
        {
            return UpdatePermission(false);
        }

        public bool UpdatePermission(bool overwrite)
        {
            switch (ObjectType)
            {
                case "Client":
                    new ClientPermission((Client)ObjType, StandardPermissionType.UpdatePermissions).Check();
                    break;
                case "Contact":
                    new ContactPermission((Contact)ObjType, StandardPermissionType.UpdatePermissions).Check();
                    break;
                case "File":
                    new FilePermission((OMSFile)ObjType, StandardPermissionType.UpdatePermissions).Check();
                    break;
                case "Document":
                    new DocumentPermission((OMSDocument)ObjType, StandardPermissionType.UpdatePermissions).Check();
                    break;
            }

            IDataParameter[] paramlist;
            DataView view = new DataView(_permissionsparties);
            view.RowStateFilter = DataViewRowState.Deleted;
            bool permrestore = false;

            System.Diagnostics.Debug.WriteLine(string.Format("UpdatePermission() - Deleted row count:{0}", view.Count),"ADVSECURITY");

            if (view.Count > 0)
            {
                foreach (DataRowView rv in view)
                {
                    string GroupID = Convert.ToString(rv["GroupID"]);
                    IDataParameter[] clearparamlist = new IDataParameter[5];
                    switch (ObjectType)
                    {
                        case "File":
                            clearparamlist[0] = OMS.Connection.AddParameter("ParentsecurableID", ((OMSFile)ObjType).ClientID);
                            break;
                        case "Document":
                            clearparamlist[0] = OMS.Connection.AddParameter("ParentsecurableID", ((OMSDocument)ObjType).OMSFile.ID);
                            break;
                    }
                    clearparamlist[1] = OMS.Connection.AddParameter("securableID", ObjectID);
                    clearparamlist[2] = OMS.Connection.AddParameter("securableType", ObjectType);
                    clearparamlist[3] = OMS.Connection.AddParameter("UserGroupID", GroupID);
                    clearparamlist[4] = OMS.Connection.AddParameter("adminUserID", Session.CurrentSession.CurrentUser.ID);
                    try
                    {
                        OMS.Connection.ExecuteProcedure("Config.DeleteUserGroupFromObject", clearparamlist);
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        if (ex.Message == "PERMRESTORE")
                            permrestore = true;
                        else
                            throw;
                    }
                }
            }

            DataView partyview;
            if (overwrite == false)
                partyview = new DataView(_permissionsparties, "[Modified] <> ''", "", DataViewRowState.CurrentRows);
            else
                partyview = new DataView(_permissionsparties, "", "", DataViewRowState.CurrentRows);

            foreach (DataRowView rw in partyview)
            {
                view = new DataView(_permissions);
                view.RowStateFilter = DataViewRowState.CurrentRows;
                view.RowFilter = "ID = '" + Convert.ToString(rw["GroupID"]) + "'";
                view.Sort = "Byte ASC";
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
                paramlist[0] = OMS.Connection.AddParameter("policyID", rw["policy"]);
                switch (ObjectType)
                {
                    case "Contact":
                        {
                            Contact contact = ObjType as Contact;
                            if (contact != null)
                            {
                                paramlist[1] = OMS.Connection.AddParameter("SecurableType", "Contact");
                                paramlist[2] = OMS.Connection.AddParameter("SecurableID", contact.ID);
                            }
                            break;
                        }
                    case "Client":
                        {
                            Client client = ObjType as Client;
                            if (client != null)
                            {
                                paramlist[1] = OMS.Connection.AddParameter("SecurableType", "Client");
                                paramlist[2] = OMS.Connection.AddParameter("SecurableID", client.ClientID);
                            }
                            break;
                        }
                    case "File":
                        {
                            OMSFile file = ObjType as OMSFile;
                            if (file != null)
                            {
                                paramlist[1] = OMS.Connection.AddParameter("SecurableType", "File");
                                paramlist[2] = OMS.Connection.AddParameter("SecurableID", file.ID);
                            }
                            break;
                        }
                    case "Document":
                        {
                            OMSDocument doc = ObjType as OMSDocument;
                            if (doc != null)
                            {
                                paramlist[1] = OMS.Connection.AddParameter("SecurableType", "Document");
                                paramlist[2] = OMS.Connection.AddParameter("SecurableID", doc.ID);
                            }
                            break;
                        }
                    default:
                        {
                            paramlist[1] = OMS.Connection.AddParameter("SecurableType", ObjectType);
                            paramlist[2] = OMS.Connection.AddParameter("SecurableID", ObjectID);
                            break;
                        }
                }

                paramlist[3] = OMS.Connection.AddParameter("userGroupID", rw["GroupID"]);
                paramlist[4] = OMS.Connection.AddParameter("adminUserID", Session.CurrentSession.CurrentUser.ID);
                paramlist[5] = OMS.Connection.AddParameter("allowMask", "0x" + allowbinary);
                paramlist[6] = OMS.Connection.AddParameter("denyMask", "0x" + denybinary);
                OMS.Connection.ExecuteProcedure("Config.ApplyObjectPolicy", paramlist);
            }
            return permrestore;
        }

        public void AddPermission(string users)
        {
            SecurityPrincipalCollection secCollection = new SecurityPrincipalCollection();
            secCollection.AddUsersFromDelimitedString(users);
            AddPermission(secCollection);
        }

        internal void AddPermission(SecurityPrincipalCollection Users)
        {
            if (_permissions.Rows.Count == 0)
            {
                _permissionsparties.Clear();
                _permissionsparties.AcceptChanges();
            }

            foreach (SecurityPrincipal secPrincipal in Users)
            {
                ObjectPolicy p = new ObjectPolicy();
                p.GetPolicy(ObjectPolicy.SystemSecurityCode);
                DataTable def = p.Permissions;

                foreach (DataRow rw in def.Rows)
                {
                    DataRow nrw = _permissions.NewRow();
                    foreach (DataColumn c in def.Columns)
                    {
                        nrw[c.ColumnName] = rw[c.ColumnName];
                    }
                    nrw["ID"] = secPrincipal.GroupId;
                    _permissions.Rows.Add(nrw);
                }

                DataRow np = _permissionsparties.NewRow();
                np["GroupID"] = secPrincipal.GroupId;
                np["ImageIndex"] = secPrincipal.Type;
                np["GroupNameDesc"] = secPrincipal.GroupName;
                np["policy"] = ObjectPolicy.SystemSecurityCode;
                np["Modified"] = DateTime.Now;
                
                if (secPrincipal.Type == "94")
                    np["AccessType"] = "EXTERNAL";
                else
                    np["AccessType"] = "INTERNAL";

                _permissionsparties.Rows.Add(np);
            }

        }

        public Guid? GetSecurityID(User user)
        {
            string SecurityID = Convert.ToString(user.GetExtraInfo("SecurityID"));
            if (!string.IsNullOrEmpty(SecurityID))
                return new Guid(SecurityID);
            else
                return null;
        }


        public void AddPermission(User user)
        {
            if (_permissions.Rows.Count == 0)
            {
                _permissionsparties.Clear();
                _permissionsparties.AcceptChanges();
            }

            ObjectPolicy p = new ObjectPolicy();
            p.GetPolicy(ObjectPolicy.SystemSecurityCode);
            DataTable def = p.Permissions;

            foreach (DataRow rw in def.Rows)
            {
                DataRow nrw = _permissions.NewRow();
                foreach (DataColumn c in def.Columns)
                {
                    nrw[c.ColumnName] = rw[c.ColumnName];
                }
                nrw["ID"] = GetSecurityID(user);
                _permissions.Rows.Add(nrw);
            }

            DataRow np = _permissionsparties.NewRow();
            np["GroupID"] = GetSecurityID(user);
            np["ImageIndex"] = 51;
            np["GroupNameDesc"] = user.FullName;
            np["policy"] = ObjectPolicy.SystemSecurityCode;
            np["Modified"] = DateTime.Now;
            np["AccessType"] = "INTERNAL";
            _permissionsparties.Rows.Add(np);

        }

        public void ResetChildPermissions()
        {
            IDataParameter[] paramlist;
            switch (ObjectType)
            {
                case "Client":
                    paramlist = new IDataParameter[1];
                    paramlist[0] = OMS.Connection.AddParameter("securableID", ObjectID);
                    OMS.Connection.ExecuteProcedure("Config.ResetClientSecurity", paramlist);
                    break;
                case "File":
                    paramlist = new IDataParameter[1];
                    paramlist[0] = OMS.Connection.AddParameter("securableID", ObjectID);
                    OMS.Connection.ExecuteProcedure("Config.ResetFileSecurity", paramlist);
                    break;
            }
        }

        public string GetPolicyID(Guid UserGroupID)
        {
            string ret = string.Empty;

            DataTable table = this.ListPermissions(Convert.ToString(UserGroupID));

            if (table.DefaultView.Count > 0)
                ret = Convert.ToString(table.DefaultView[0]["PolicyID"]);

            return ret;
        }

    }

    public class SecurityPrincipalCollection : List<SecurityPrincipal>
    {
        public SecurityPrincipalCollection()
        {

        }

        public void AddFromDelimitedString(string delimitedUsers, bool clearExisting)
        {
            if (clearExisting)
                this.Clear();

            foreach (string user in delimitedUsers.Split(':'))
            {
                this.Add(SecurityPrincipal.CreateFromString(user));
            }
        }

        public void AddUsersFromDelimitedString(string users)
        {
            AddFromDelimitedString(users, false);
        }
    }

    /// <summary>
    /// Holds user or group info that is passed from the collection control when adding permissions to an object
    /// </summary>
    public class SecurityPrincipal
    {
        #region Properties

        public string Type { get; set; }
        public string GroupName { get; set; }
        public string GroupId { get; set; }
        public string ContactId { get; set; }

        public long? ContID
        {
            get
            {
                if (String.IsNullOrEmpty(ContactId))
                    return null;
                else
                    return Convert.ToInt64(ContactId);
            }
        }

        public Contact Contact
        {
            get
            {
                if (ContID.HasValue)
                    return Contact.GetContact(ContID.Value);
                else
                    return null;
            }
        }

        #endregion

        #region Constructors
        public SecurityPrincipal()
        {
        }

        /// <summary>
        /// Create a security principal
        /// </summary>
        /// <param name="userID">The ID of a User to create a security principal from</param>
        /// <returns></returns>
        public static SecurityPrincipal Create(int userID)
        {
            SecurityPrincipal principal = new SecurityPrincipal();

            return principal;
        }

        /// <summary>
        /// Create a security principal
        /// </summary>
        /// <param name="user">A delimeted string with in the form of groupId;groupName;Type</param>
        /// <returns>Newly created Security Principal</returns>
        public static SecurityPrincipal CreateFromString(string user)
        {
            SecurityPrincipal principal = new SecurityPrincipal();
            string[] param = user.Split(";".ToCharArray());

            if (param.Length < 3)
                throw new ArgumentException("The user string is invalid, expected string in format groupId;groupName;Type;ContactID", "user");

            principal.GroupId = param[0];
            principal.GroupName = CodeLookup.GetLookup("SECGROUPS", param[1]);
            if (principal.GroupName == "")
                principal.GroupName = param[1];

            principal.Type = param[2];

            if (param.Length > 3)
                principal.ContactId = param[3];

            return principal;
        }

        #endregion

    }

}
