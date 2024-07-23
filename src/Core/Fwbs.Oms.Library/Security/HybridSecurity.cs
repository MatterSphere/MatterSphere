namespace FWBS.OMS.Security
{
    internal sealed class HybridSecurity : ISecurityType
    {
        #region Fields

		private BasicSecurity basic;
		private AdvancedSecurity advanced;

        private BasicSecurity Basic
        {
            get
            {
                if (basic == null)
                    basic = new BasicSecurity();

                return basic;
            }
        }

        private AdvancedSecurity Advanced
        {
            get
            {
                if (advanced == null)
                    advanced = new AdvancedSecurity();

                return advanced;
            }
        }        

        #endregion

        #region Constructors

		public HybridSecurity()
        {            
        }

        #endregion

        #region ISecurityType Members

        public void Refresh(ISecurable securedObject)
        {
			if (Session.CurrentSession.AdvancedSecurityEnabled)
                Advanced.Refresh(securedObject);
			else 
			   Basic.Refresh(securedObject); 		      
        }

        public bool IsGranted(Permissions.Permission permission)
        {
			if (Session.CurrentSession.AdvancedSecurityEnabled)
                return Advanced.IsGranted(permission);
			else
                return Basic.IsGranted(permission);
        }

        public void ApplyDefaultSettings(ISecurable parent, ISecurable objectToSecure)
        {
			if (Session.CurrentSession.AdvancedSecurityEnabled || Session.CurrentSession.MatterSphereSecurityEnabled)
                Advanced.ApplyDefaultSettings(parent, objectToSecure);
			else
                Basic.ApplyDefaultSettings(parent, objectToSecure);             
        }

        #endregion 
      
    }
}
