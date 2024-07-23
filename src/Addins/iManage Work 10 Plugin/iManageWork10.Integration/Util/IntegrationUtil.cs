using System;
using System.Data;
using FWBS.OMS;
using FWBS.OMS.Mappers;

namespace iManageWork10.Integration.Util
{
    public static class IntegrationUtil
    {
        private const string SYSTEM_NAME = "iManageWork10";
        private const string DT_DATA_SOURCE = "DGETIMCLNUMMAP";
        private const string ROOT_URL_SPECIFIC_DATA = "__IM_ROOTURL";
        private const string CLIENT_ID_SPECIFIC_DATA = "__IM_CLIENT_ID";
        private const string EXTENSION_ID_SPECIFIC_DATA = "__IM_EXTENSION_ID";
        private const string WORKSPACE_CREATOR_ROLE = "IMNGWORKSPCRT";
        private const string MAPPING_SYSTEM_NAME = "iManageWork10";
        private const string CLIENT_NUMBER_MAPPING_ID = "ClientNumber";
        private const string MATTER_NUMBER_MAPPING_ID = "MatterNumber";
        private const string LIBRARIES_MAPPING_ID = "Libraries";
        private const string GET_VALUE_MAPPING_ID = "GetValue";

        public static string GetCustomValue(object omsObject, string customName)
        {
            var customMappingValue = GetMappingValueForCustom(customName);
            if (customMappingValue.Equals(CLIENT_NUMBER_MAPPING_ID))
            {
                var omsClient = omsObject as Client;
                if (omsClient != null)
                {
                    return omsClient.ClientNo;
                }

                var omsFile = omsObject as OMSFile;
                if (omsFile != null)
                {
                    return omsFile.Client.ClientNo;
                }
            }

            if (customMappingValue.Equals(MATTER_NUMBER_MAPPING_ID))
            {
                var omsFile = omsObject as OMSFile;
                if (omsFile != null)
                {
                    return omsFile.FileNo;
                }
            }

            throw new ArgumentException($"{customName} is not mapped properly.", customName);
        }

        public static string GetMappingValueForCustom(string internalId)
        {
            return MappingManager.GetMappingManager.GetExternalID(MAPPING_SYSTEM_NAME, internalId, GET_VALUE_MAPPING_ID);
        }

        public static string GetLibrariesScope()
        {
            return MappingManager.GetMappingManager.GetExternalID(MAPPING_SYSTEM_NAME, LIBRARIES_MAPPING_ID, GET_VALUE_MAPPING_ID);
        }

        public static string GetRootUrl()
        {
            var rootUrl = Convert.ToString(Session.CurrentSession.GetSpecificData(ROOT_URL_SPECIFIC_DATA));
            if (string.IsNullOrEmpty(rootUrl))
            {
                throw new ArgumentException($"Specific data {ROOT_URL_SPECIFIC_DATA} not configured.", ROOT_URL_SPECIFIC_DATA);
            }

            return rootUrl;
        }

        public static string GetClientId()
        {
            return Convert.ToString(Session.CurrentSession.GetSpecificData(CLIENT_ID_SPECIFIC_DATA));
        }

        public static string GetExtensionId()
        {
            return Convert.ToString(Session.CurrentSession.GetSpecificData(EXTENSION_ID_SPECIFIC_DATA));
        }

        public static string GetClientNumberMappingEntity()
        {
            FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection();
            kvc.Add("systemName", SYSTEM_NAME);
            DataTable dt = FWBS.OMS.UI.Windows.Services.RunDataList(DT_DATA_SOURCE, kvc);
            if (dt != null && dt.Rows.Count > 0)
                return ((string)dt.Rows[0]["Name"]).ToLower();

            return "Custom1";
        }

        public static bool IsUserInWorkspaceCreatorRole()
        {
            return Session.CurrentSession.CurrentUser.IsInRoles(WORKSPACE_CREATOR_ROLE);
        }
    }
}
