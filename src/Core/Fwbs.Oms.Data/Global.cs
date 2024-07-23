using System.Diagnostics;
using System.Reflection;
using System.Resources;
using FWBS.Common;

namespace FWBS.OMS.Data
{
    /// <summary>
    /// Global information on a data layer.
    /// </summary>
    sealed internal class Global
	{
		private Global(){}

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
		/// The default RowGuid column name.
		/// </summary>
		public const string RowGuidCol = "rowguid";

		/// <summary>
		/// Help Registry key.
		/// </summary>
		internal static ApplicationSetting Help = new ApplicationSetting(ApplicationKey, VersionKey, "", "Help", "http://www.fwbs.net/helpme.asp?");


		/// <summary>
		/// Log switch for the configured trace.
		/// </summary>
		internal static TraceSwitch LogSwitch = new TraceSwitch("DAL", "OMS Data Application Layer");

		/// <summary>
		/// Gets the resource string specified.
		/// </summary>
		/// <param name="resID">Resource ID.</param>
		/// <param name="param">Parameter replacing arguments.</param>
		/// <returns>Need UI string value.</returns>
		//Internal debugging function for resources.
		internal static string GetResString(string resID, params string [] param)
		{
			string text = GetResString(resID);
			for (int ctr = param.GetLowerBound(0); ctr <= param.GetUpperBound(0); ctr++)
				text = text.Replace("%" + (ctr+1).ToString(), param[ctr].ToString());
			return text;
		}

		internal static string GetResString(string resID)
		{
			ResourceManager rm;
			string ret = "";
			rm = new ResourceManager("FWBS.OMS.Data.omsdl", Assembly.GetExecutingAssembly());
			ret = rm.GetString(resID);
			return ret;
		}


        public static bool AllowAssertions
        {
            get
            {
                return FWBS.Common.ConvertDef.ToBoolean(new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "Debug", "AllowAssertions").GetSetting(false), false);
            }
        }

        private static bool? _executeReaderUseExecuteParameters;
        public static bool ExecuteReaderUseExecuteParameters
        {
            get
            {
                if (_executeReaderUseExecuteParameters == null)
                    _executeReaderUseExecuteParameters = FWBS.Common.ConvertDef.ToBoolean(new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "Data", "ExecuteReaderUseExecuteParameters").GetSetting(true), true);

                return _executeReaderUseExecuteParameters.Value;
            }
        }
	}
}
