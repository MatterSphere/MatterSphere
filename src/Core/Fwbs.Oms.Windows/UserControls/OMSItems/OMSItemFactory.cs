using System;
using System.Text.RegularExpressions;

namespace FWBS.OMS.UI.Windows
{
    public static class OMSItemFactory
    {
        public const string AppointmentInPlaceWizardCode = "SIPFILAPPT";
        public const string TaskInPlaceWizardCode = "SIPFILTASK";

        public static ucOMSItemBase CreateOMSItem(string code, object parent, EnquiryEngine.EnquiryMode mode, bool offline, Common.KeyValueCollection param)
        {
            ucOMSItemBase omsItem;
            switch (code)
            {
                case "SIPFILAPPT":
                case "SIPFILTASK":
                case "SIPFILUNDERTAKI":
                    string uCode = GetUserDefinedFormCode(code);
                    omsItem = new ucOMSItemV2(uCode, parent, mode, offline, param);
                    break;
                case "SIPDWZNEW":
                    omsItem = new ucOMSItemV2Date(code, parent, mode, offline, param);
                    break;
                default:
                    omsItem = new ucOMSItem(code, parent, mode, offline, param);
                    break;

            }
            return omsItem;
        }

        public static ucOMSItemBase CreateOMSItem(string code, object parent, FWBS.OMS.Interfaces.IEnquiryCompatible businessObject, bool offline, Common.KeyValueCollection param)
        {
            string baseCode = TransformEnquiryCodeToBase(code);
            ucOMSItemBase omsItem;
            switch (baseCode)
            {
                case "SIPFILAPPT":
                case "SIPFILTASK":
                case "SIPFILUNDERTAKI":
                    string uCode = GetUserDefinedFormCode(baseCode);
                    omsItem = new ucOMSItemV2(uCode, parent, businessObject, offline, param);
                    break;
                case "SIPDWZNEW":
                    omsItem = new ucOMSItemV2Date(code, parent, businessObject, offline, param);
                    break;
                default:
                    omsItem = new ucOMSItem(code, parent, businessObject, offline, param);
                    break;
            }
            return omsItem;
        }

        public static FormType FormContainer(string enqCode)
        {
            switch (enqCode)
            {
                case "SIPFILAPPT":
                case "SIPFILTASK":
                case "SIPDWZNEW":
                case "SIPFILUNDERTAKI":
                    return FormType.OMSItem;
                case "SCRFILUNDERTAKI":
                case "SCRCONPICKNEW":
                    return FormType.InPlaceWizard;
                default:
                    return FormType.Wizard;
            }
        }

        public enum FormType
        {
            OMSItem,
            InPlaceWizard,
            Wizard
        }

        /// <summary>
        /// It processes Task and Appointment in-place wizard forms enquiry code
        /// </summary>
        /// <param name="code">Base enquiry code</param>
        /// <returns>User defined enquiry code</returns>
        private static string GetUserDefinedFormCode(string code)
        {
            bool isAppointmentInPlaceWizardCode = code.Equals(AppointmentInPlaceWizardCode, StringComparison.InvariantCultureIgnoreCase);

            bool isTaskInPlaceWizardCode = code.Equals(TaskInPlaceWizardCode, StringComparison.InvariantCultureIgnoreCase);

            SystemForms? systemForms = null;
            if (isAppointmentInPlaceWizardCode)
                systemForms = SystemForms.AppointmentEdit;
            else if (isTaskInPlaceWizardCode)
                systemForms = SystemForms.TaskEdit;

            if (systemForms.HasValue)
            {
                string uCode = Session.CurrentSession.DefaultSystemForm(systemForms.Value);
                return TransformEnquiryCodeToSIP(uCode);
            }
            else
                return code;
        }

        /// <summary>
        /// Transform dialogue enquiry code to in-place one.
        /// As in documentation written that internally exchange...
        /// </summary>
        /// <remarks>
        /// "Depending on where the wizard opens, the standard enquiry form name SCRXYZ may be internally
        /// transformed into SIPXYZ or STPXYZ in order to pick up a form of the intended style."
        /// </remarks>
        /// <param name="enquiryCode">e.g. uSCRFILTASK</param>
        /// <returns>e.g. uSIPFILTASK</returns>
        private static string TransformEnquiryCodeToSIP(string enquiryCode)
        {
            return Regex.Replace(enquiryCode, @"^(fd|ud|u)?SCR(.+)", "$1SIP$2", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        /// <summary>
        /// Return from custom code (uSCRFILTASK) to base one (SIPFILTASK).
        /// </summary>
        /// <param name="customEnquiryCode">custom enquiry code</param>
        /// <returns>Base in-place enquery code</returns>
        private static string TransformEnquiryCodeToBase(string customEnquiryCode)
        {
            return Regex.Replace(customEnquiryCode, @"^(fd|ud|u)?SCR(.+)", "SIP$2", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }
    }
}
