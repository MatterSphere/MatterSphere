using System;

namespace FWBS.OMS.Security
{
    internal sealed class BasicSecurity : ISecurityType
    {
        #region ISecurityType Members

        public void Refresh(ISecurable securedObject)
        {
            if (securedObject == null)
                throw new ArgumentNullException("securedObject");

        }

        public bool IsGranted(Permissions.Permission permission)
        {
            //Use the old fashioned roles based system
            if (permission.SecurableType == "SYSTEM" && LegacyRoleExist(permission.Code))
                return LegacyIsInRoles(permission.Code);
            else
                return true;
        }

        public void ApplyDefaultSettings(ISecurable parent, ISecurable objectToSecure)
        {
        }

        #endregion

        #region Methods

        private bool LegacyRoleExist(string role)
        {
            using (System.Data.DataView vw = new System.Data.DataView(CodeLookup.GetLookups("USRROLES")))
            {
                vw.RowFilter = string.Format("cdcode = '{0}'", role.Replace("'", "''"));
                if (vw.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        private bool LegacyIsInRoles(string code)
        {
            if (String.IsNullOrEmpty(code))
                return true;

            string[] vals = Session.CurrentSession.CurrentUser.Roles.Split(',');

            return Array.IndexOf(vals, code) > -1;
        }

        #endregion
    }
}
