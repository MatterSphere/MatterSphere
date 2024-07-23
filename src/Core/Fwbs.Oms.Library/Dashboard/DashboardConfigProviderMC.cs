using System;
using System.Data;

namespace FWBS.OMS.Dashboard
{
    public class DashboardConfigProviderMC : DashboardConfigProvider
    {
        #region Config examples
        /* Dashboard
        <config>
            <tiles>
                <tile row="0" column="0" omsObjCode="SCHFILTASKS" dshbType="TLOMSOBJ" guid="6ec94fc7-501d-46a8-af15-5db968a4b9e6" width="3" height="1" />
                <tile row="1" column="0" omsObjCode="SCHFILAPPOINTS" dshbType="TLOMSOBJ" guid="1bdcb323-e0d1-4184-9bdf-3ac533529a59" width="3" height="1" />
            </tiles>
            <settings object_id="1" />
        </config>
        */
        #endregion

        private const string OBJECT_ID_ATTRIBUTE = "object_id";
        private const string SETTINGS_PATH = "settings";

        public DashboardConfigProviderMC(string dashboardCode, bool isConfigurationMode = false) : base(dashboardCode, isConfigurationMode)
        {
        }

        public DashboardSettings GetDashboardSettings()
        {
            var configSetting = GetUserDashboardConfigSetting();
            configSetting.Current = SETTINGS_PATH;
            long parentObjectId = 0;
            if (long.TryParse(configSetting.GetString(OBJECT_ID_ATTRIBUTE, string.Empty), out parentObjectId))
            {
                return new DashboardSettings()
                {
                    ParentObjectId = parentObjectId
                };
            }

            throw new DashboardSettingsNotFoundException();
        }

        public void SetDashboardSettings(DashboardSettings settings)
        {
            var configSetting = GetUserDashboardConfigSetting();
            configSetting.Current = SETTINGS_PATH;
            configSetting.SetString(OBJECT_ID_ATTRIBUTE, settings.ParentObjectId.ToString());
            SetUserDashboardConfigSetting(configSetting);
        }

        protected override bool IsCompatibleWithDashboardType(DataRow row)
        {
            return row[3].ToString().Equals("FWBS.OMS.OmsFile", StringComparison.OrdinalIgnoreCase);
        }

        #region Classes

        public class DashboardSettings
        {
            public long ParentObjectId { get; set; }
        }

        public class DashboardSettingsNotFoundException : Exception { }

        #endregion
    }
}
