using System.Diagnostics;
using System.Reflection;
using System.Resources;

namespace FWBS.Common
{
    /// <summary>
    /// Global information on a Common project.
    /// </summary>
    public sealed class Global
    {
        private Global() { }

        /// <summary>
        /// Application key for the registry setting, to be accessed statically.
        /// </summary>
        public const string ApplicationKey = "OMS";

        /// <summary>
        /// Application version key for the registry setting, to be accessed statically.
        /// </summary>
        public const string VersionKey = "2.0";

        /// <summary>
        /// Application data folder.
        /// </summary>
        public const string ApplicationData = @"FWBS\OMS";

        /// <summary>
        /// Gets the resource string specified.
        /// </summary>
        /// <param name="resID">Resource ID.</param>
        /// <param name="param">Parameter replacing arguments.</param>
        /// <returns>Need UI string value.</returns>
        //Internal debugging function for resources.
        public static string GetResString(string resID, params string[] param)
        {
            string text = GetResString(resID);
            for (int ctr = param.GetLowerBound(0); ctr <= param.GetUpperBound(0); ctr++)
                text = text.Replace("%" + (ctr + 1).ToString(), param[ctr].ToString());
            return text;
        }

        public static string GetResString(string resID)
        {
            ResourceManager rm;
            string ret = "";
            rm = new ResourceManager("FWBS.Common.omscom", Assembly.GetExecutingAssembly());
            ret = rm.GetString(resID);
            return ret;
        }
    }
}
