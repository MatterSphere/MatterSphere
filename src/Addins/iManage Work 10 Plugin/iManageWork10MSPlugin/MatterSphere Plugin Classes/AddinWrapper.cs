using System;

namespace MatterSphereIntercept.MatterSphere_Plugin_Classes
{
    public class AddinWrapper
    {

        private readonly dynamic _addin;

        public AddinWrapper(dynamic addin)
        {
            _addin = addin;
        }
        
        public string[] GetMergedFieldDataAsStringArray()
        {
            ValidateOmsAddin();
            var documentMetadata = GetOmsMetadata();
            string[] names = { AddinConstants.FILE_ID, AddinConstants.ASSOCIATE_ID, AddinConstants.BASE_PRECEDENT_ID };
            object[] values = { documentMetadata.FileId, documentMetadata.AssociateId,
                documentMetadata.BasePrecedentId };
            return _addin.RunScript(AddinConstants.GET_PROFILE_FIELDS, names, values).Split('|');
        }

        public string GetSettingValue(string settingName)
        {
            ValidateOmsAddin();
            string[] names = new string[0];
            object[] values = new object[0];
            return _addin.RunScript(settingName, names, values);
        }

        public void RunScript(string methodName, string[] parameterNames, object[] parameters)
        {
            ValidateOmsAddin();
            _addin.RunScript(methodName, parameterNames, parameters);
        }

        public long GetDocVariableN(string variableName)
        {
            ValidateOmsAddin();
            return _addin.GetDocVariableN(variableName, 0);
        }

        private DocumentMetadata GetOmsMetadata()
        {
            var fileIdVariable = GetDocVariableN(AddinConstants.FILE_ID);
            ValidateRequiredVariable(AddinConstants.FILE_ID, fileIdVariable);
            var associateIdVariable = GetDocVariableN(AddinConstants.ASSOCIATE_ID);
            ValidateRequiredVariable(AddinConstants.ASSOCIATE_ID, associateIdVariable);
            var basePrecedentIdVariable = GetDocVariableN(AddinConstants.PRECEDENT_ID);
            if (basePrecedentIdVariable == 0)
                basePrecedentIdVariable = GetDocVariableN(AddinConstants.BASE_PRECEDENT_ID);
            ValidateRequiredVariable(AddinConstants.BASE_PRECEDENT_ID, basePrecedentIdVariable);

            return new DocumentMetadata
            {
                FileId = fileIdVariable,
                AssociateId = associateIdVariable,
                BasePrecedentId = basePrecedentIdVariable
            };
        }

        private void ValidateRequiredVariable(string variableName, long value)
        {
            if (value == 0)
            {
                throw new Exception($"{variableName} is not found.");
            }
        }

        private void ValidateOmsAddin()
        {
            if (!_addin.online)
            {
                throw new Exception("MatterSphere add-in is not connected.");
            }
        }

    }

    public static class AddinConstants
    {
        public static readonly string GET_PROFILE_FIELDS = "GetProfileFields";
        public static readonly string FILE_ID = "FILEID";
        public static readonly string ASSOCIATE_ID = "ASSOCID";
        public static readonly string BASE_PRECEDENT_ID = "BASEPRECID";
        public static readonly string PRECEDENT_ID = "PRECID";
        public static readonly string OMS_COM_ADDIN = "OMSOffice2007";
        public static readonly string SHOW_WIZARD = "iManageWork10Wizard";
        public static readonly string SILENT_SAVE_ENABLED = "iManageWork10SilentSave";
        public static readonly string SILENT_SAVE_TIMEOUT = "iManageWork10SilentSaveTimeOut";
        public static readonly string ADDITIONAL_LIBRARIES = "iManageGetLibraries";
    }

}
