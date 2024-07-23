using FWBS.Common;

namespace FWBS.OMS
{
    public static class CurrentUIVersion
    {
        static CurrentUIVersion()
        {
            bool.TryParse(GetRegistryValue("ImageMso"), out UseOfficeImages);
        }

        #region Properties

        public static readonly bool UseOfficeImages = false;

        public static string Font
        {
            get { return "Segoe UI"; }
        }


        public static float FontSize
        {
            get { return 9; }
        }

        #endregion Properties

        #region Methods

        private static string GetRegistryValue(string keyString)
        {
            var keyValue = new ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", keyString, "");
            return keyValue.GetSetting().ToString();
        }

        #endregion Methods
    }
}
