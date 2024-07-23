using FWBS.OMS.Interfaces;
using System;
using System.Linq;

namespace FWBS.OMS
{
    public class Power
    {
        internal Power(IPowerProfile powerProfile)
        {
            this.PowerRoles = powerProfile.PowerRoles;
            this.PowerMenuItem = powerProfile.PowerMenuItem;
        }

        public static readonly Power Empty = new Power();

        private Power()
        {

        }

        public bool IsConfigured
        {
            get
            {
                return (String.IsNullOrEmpty(PowerMenuItem) == false);
            }
        }

        public bool CanRunAction(string ActionName)
        {
            if (Session.CurrentSession.CurrentUser.IsInRoles("ADMIN"))
                return true;

            string cra = PowerMenuItem + ";";
            return cra.IndexOf(ActionName + ";") > -1;
        }

        private string powermenuitems;
        internal string PowerMenuItem
        {
            get
            {
                return powermenuitems;
            }
            set
            {
                powermenuitems = value;
            }
        }

        private string powerroles;
        internal string PowerRoles
        {
            get
            {
                return powerroles;
            }
            set
            {
                powerroles = value;
            }
        }

        public bool IsInRoles(string role)
        {
            return powerroles
                .Split(';')
                .Any(r => r.Equals(role, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool IsInRoles(params string[] roles)
        {
            return roles.Any(IsInRoles);
        }
    }



}
