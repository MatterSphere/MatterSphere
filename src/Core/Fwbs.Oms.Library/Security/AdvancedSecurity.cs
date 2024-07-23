using System;
using System.Data;

namespace FWBS.OMS.Security
{
    internal sealed class AdvancedSecurity : ISecurityType, IDisposable
    {
        #region Fields

        private object Synch = new object();

        private const string Table_Permissions = "Permissions";
        private const string Table_SecurityCache = "SecurityCache";
        private DataSet configuration = new DataSet();


        #endregion

        #region Constructors

        public AdvancedSecurity()
        {
            lock (Synch)
            {
                DataTable securitycaching = null;
                if (configuration.Tables.Contains(Table_SecurityCache))
                {
                    securitycaching = configuration.Tables[Table_SecurityCache];
                    securitycaching.Clear();
                }
                else
                {
                    securitycaching = new DataTable(Table_SecurityCache);
                    securitycaching.Columns.Add("SecurableType");
                    securitycaching.Columns.Add("ObjectId");
                    securitycaching.Columns.Add("Cached", typeof(DateTime)).DateTimeMode = DataSetDateTime.Utc;
                    configuration.Tables.Add(securitycaching);
                    securitycaching.PrimaryKey = new DataColumn[] { securitycaching.Columns[0], securitycaching.Columns[1] };
                }

                DataTable permissions = null;

                if (configuration.Tables.Contains(Table_Permissions))
                {
                    permissions = configuration.Tables[Table_Permissions];
                    permissions.Clear();
                }
                else
                {
                    permissions = new DataTable(Table_Permissions);
                    permissions.Columns.Add("SecurableType");
                    permissions.Columns.Add("ObjectId");
                    permissions.Columns.Add("Permission");
                    configuration.Tables.Add(permissions);
                }

                if (HasExpired("SYSTEM", "*"))
                    RefreshPermissions("SYSTEM", "*");
            }
        }

        #endregion

        #region ISecurityType Members

        public void Refresh(ISecurable securedObject)
        {
            if (securedObject == null)
                throw new ArgumentNullException("securedObject");

            GetPermissionConfig(Permissions.Permission.GetSecurableObjectType(securedObject), securedObject.SecurityId, true);
            RefreshPermissions(Permissions.Permission.GetSecurableObjectType(securedObject), securedObject.SecurityId);
        }

        public bool IsGranted(Permissions.Permission permission)
        {
            lock (Synch)
            {
                DataRow r = GetPermissionConfig(permission.SecurableType, permission.SecuredObject.SecurityId, false);

                if (r == null)
                    RefreshPermissions(permission.SecurableType, permission.SecuredObject.SecurityId);

                using (DataView vw = new DataView(configuration.Tables[Table_Permissions]))
                {
                    vw.RowFilter = GetFilter(permission.SecurableType, permission.SecuredObject.SecurityId, permission.Code);
                    if (vw.Count > 0)
                    {
                        //If the permission does not exist then return true for the default.
                        return false;
                    }
                    else
                    {
                        if (Permissions.Permission.StringToStandardType(permission.Code) == FWBS.OMS.Security.Permissions.StandardPermissionType.Unknown)
                        {
                            //Rollback to the old user role system
                            return new BasicSecurity().IsGranted(permission);
                        }
                        else
                            return true;
                    }
                }
            }
        }

        public void ApplyDefaultSettings(ISecurable parent, ISecurable objectToSecure)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (objectToSecure == null)
                throw new ArgumentNullException("objectToSecure");

            string parenttype = Permissions.Permission.GetSecurableObjectType(parent);
            string objecttype = Permissions.Permission.GetSecurableObjectType(objectToSecure);

            IDataParameter[] pars = new IDataParameter[5];
            pars[0] = Session.CurrentSession.Connection.CreateParameter("ParentType", parenttype);
            pars[1] = Session.CurrentSession.Connection.CreateParameter("ParentID", parent.SecurityId);
            pars[2] = Session.CurrentSession.Connection.CreateParameter("ChildID", objectToSecure.SecurityId);
            pars[3] = Session.CurrentSession.Connection.CreateParameter("AdminUserID", Session.CurrentSession.CurrentUser.ID);

            Session.CurrentSession.Connection.ExecuteProcedure("config.ApplyChildSecurity", pars);
            
        }

        #endregion

        #region Methods

        private bool HasExpired(string securableType, string id)
        {
            return GetPermissionConfig(securableType, id, false) == null;
        }

        private string GetFilter(string securableType, string id, string permission)
        {
            string filter = String.Format("SecurableType = '{0}'", securableType.Replace("'", "''"));
            if (!String.IsNullOrEmpty(id))
            {
                if (id.Trim() == "*")
                    filter += String.Format(" and (ObjectId is null or ObjectId = '*')", id.Replace("'", "''"));
                else
                    filter += String.Format(" and ObjectId = '{0}'", id.Replace("'", "''"));
            }

            if (!String.IsNullOrEmpty(permission))
            {
                if (permission.Trim() == "*")
                    filter += String.Format(" and (Permission is null or ObjectId = '*')", permission.Replace("'", "''"));
                else
                    filter += String.Format(" and Permission = '{0}'", permission.Replace("'", "''"));
            }

            return filter;
        }


        private DataRow GetPermissionConfig(string securableType, string id, bool force)
        {
            if (configuration.Tables.Contains(Table_SecurityCache))
            {
                using (DataView vw = new DataView(configuration.Tables[Table_SecurityCache]))
                {
                    vw.RowFilter = GetFilter(securableType, id, null);

                    if (vw.Count > 0)
                    {
                        DataRow r = vw[0].Row;
                        DateTime cache = (DateTime)r["Cached"];
                        TimeSpan ts = DateTime.UtcNow.Subtract(cache);

                        int to = SecurityManager.CurrentManager.CacheTimeout;

                        if ((ts.TotalMinutes > to && to >=0 )  || force)
                        {
                            r.Delete();
                            using (DataView vw2 = new DataView(configuration.Tables[Table_Permissions]))
                            {

                                vw2.RowFilter = GetFilter(securableType, id, null);

                                for (int ctr = vw2.Count - 1; ctr >= 0; ctr--)
                                {
                                    vw2[ctr].Delete();
                                }
                            }
                            configuration.AcceptChanges();

                        }
                        else
                            return r;
                    }

                    return null;
                }
            }
            else
                return null;
        }

        private void RefreshPermissions(string securableType, string id)
        {
            IDataParameter[] pars = new IDataParameter[3];
            pars[0] = Session.CurrentSession.Connection.CreateParameter("SecurableType", securableType);

            long objectid;
            if (long.TryParse(id, out objectid))
                pars[1] = Session.CurrentSession.Connection.CreateParameter("ObjectId", objectid);
            else
                pars[1] = Session.CurrentSession.Connection.CreateParameter("ObjectId", DBNull.Value);

            pars[2] = Session.CurrentSession.Connection.CreateParameter("Children", false);
            DataTable permissions = Session.CurrentSession.Connection.ExecuteProcedureTable("config.GetObjectPermissions", Table_Permissions, pars);
            configuration.Merge(permissions, false, MissingSchemaAction.Ignore);

            configuration.Tables[Table_SecurityCache].Rows.Add(securableType, id, DateTime.UtcNow);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (Synch)
            {
                if (configuration != null)
                {
                    configuration.Dispose();
                }
            }
        }

        #endregion
    }
}
