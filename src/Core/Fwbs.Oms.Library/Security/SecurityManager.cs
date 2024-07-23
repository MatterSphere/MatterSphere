using System;
using System.Collections.Generic;
using System.Text;

namespace FWBS.OMS.Security
{
    public sealed class SecurityManager : IService, IDisposable, ISecurityType
    {

        #region Fields

        private bool loaded;
        private ISecurityType securitytype;
        private SecurityType securetype;
        private int cachetimeout;

        #endregion

        #region Constructors

        private SecurityManager()
        {
        }

        #endregion

        #region Properties

        private static SecurityManager current;
        public static SecurityManager CurrentManager
        {
            get
            {
                if (current == null)
                {
                    current = new SecurityManager();
                }
                return current;
            }
        }

        public SecurityType SecurityType
        {
            get
            {
                return securetype;
            }
        }

        #endregion


        #region Checks


        public bool IsGranted(Permissions.Permission permission)
        {
            if (permission == null || !Session.CurrentSession.IsLoggedIn || !IsLoaded)
                return false;

            
            return securitytype.IsGranted(permission);
          
        }

        public bool IsGranted(Permissions.Permission[] permissions)
        {
            return IsGranted(permissions, PermissionComparison.Or);
        }

        public bool IsGranted(Permissions.Permission[] permissions, PermissionComparison comparison)
        {
            return Check(permissions, comparison).Length == 0;
        }

        public Permissions.Permission[] Check(Permissions.Permission[] permissions, PermissionComparison comparison)
        {
            List<Permissions.Permission> failed = new List<FWBS.OMS.Security.Permissions.Permission>();

            if (permissions == null || !Session.CurrentSession.IsLoggedIn || !IsLoaded)
                return failed.ToArray(); 

            switch (comparison)
            {
                case PermissionComparison.And:
                    {
                        foreach (Permissions.Permission perm in permissions)
                        {
                            if (IsGranted(perm) == false)
                            {
                                failed.Add(perm);
                            }
                        }
                    }
                    break;
                case PermissionComparison.Or:
                    {
                        foreach (Permissions.Permission perm in permissions)
                        {
                            if (IsGranted(perm))
                            {
                                failed.Clear();
                                return failed.ToArray();
                            }
                            else
                                failed.Add(perm);
                        }
                    }
                    break;
            }

            return failed.ToArray();
        }

        public void CheckPermission(Permissions.Permission permission)
        {
            if (!IsGranted(permission))
                throw new PermissionsException("EXPERMDENIED2", "Permission Denied", true, permission.ToString());
        }

        public void CheckPermission(Permissions.Permission[] permissions)
        {
            CheckPermission(permissions, PermissionComparison.Or);
        }

        public void CheckPermission(Permissions.Permission[] permissions, PermissionComparison comparison)
        {
            Permissions.Permission[] failedperms = Check(permissions, comparison);
            if (failedperms.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Permissions.Permission perm in failedperms)
                {
                    sb.Append(perm);
                    sb.Append(Environment.NewLine);
                }

                throw new PermissionsException("EXPERMDENIED2", "Permission Denied", true, sb.ToString());
            }
        }

        #endregion

        #region Methods

        public void ApplyDefaultSettings(ISecurable parent, ISecurable objectToSecure)
        {
            securitytype.ApplyDefaultSettings(parent, objectToSecure);
        }

        public void Refresh(ISecurable securedObject)
        {
            securitytype.Refresh(securedObject);
        }

        #endregion

        #region Properties

        public int CacheTimeout
        {
            get
            {
                return cachetimeout;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Unload();
        }

        #endregion

        #region IService Members

        public bool IsLoaded
        {
            get
            {
                return loaded;
            }
        }

        public void Load()
        {
            if (!loaded)
            {
                IDisposable disp = securitytype as IDisposable;
                if (disp != null)
                    disp.Dispose();

                FWBS.Common.ApplicationSetting regmins = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "Memory", "SecurityTimeout", 1);
                cachetimeout = Convert.ToInt32(regmins.GetSetting());

				securetype = SecurityType.Hybrid;
				securitytype = new HybridSecurity();

                loaded = true;
            }
        }

        public void Unload()
        {
            try
            {
                if (loaded)
                {
                    IDisposable disp = securitytype as IDisposable;
                    if (disp != null)
                        disp.Dispose();
                }
            }
            finally
            {
                loaded = false;
            }
        }

        #endregion
    }
}
