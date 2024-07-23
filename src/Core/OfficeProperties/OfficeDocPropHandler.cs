using System;
using System.Collections.Generic;

namespace Fwbs.Documents
{
    using FWBS.Common;
    using Fwbs.Framework.ComponentModel.Composition;

    [Export(typeof(IDocPropHandler))]
    public sealed class OfficeDocPropHandler : IDocPropHandler
    {
        // See OMSAppBase - Document Property Constants
        private readonly string[] DocumentProperties = new string[]
        {
            "ASSOCID",
            "BASEPRECID",
            "BASEPRECTYPE",
            "CLIENTID",
            "COMPANYID",
            "CUSTOMFORM",
            "DATAKEY",
            "DOCID",
            "DOCIDEX",
            "EDITION",
            "FILEID",
            "ISPREC",
            "ISPRIVATE",
            "PHASEID",
            "PRECID",
            "RETENTIONPERIOD",
            "RETENTIONPOLICY",
            "SERIALNO",
            "TEXTONLY",
            "VERSIONID",
            "VERSIONLABEL",
            "VIEWMODE"
        };

        internal IEnumerable<string> KnownProperties
        {
            get { return DocumentProperties; }
        }

        internal LimitCollection CachedPasswords { get; private set; }

        public OfficeDocPropHandler()
        {
            Type type = Type.GetType("FWBS.OMS.Session, OMS.Library", false, true);
            if (type != null)
            {
                object currentSession = type.GetProperty("CurrentSession").GetValue(null);
                if ((bool)type.GetProperty("AdvancedDocPropHandler").GetValue(currentSession))
                {
                    var GetExternalDocumentPropertyName = type.GetMethod("GetExternalDocumentPropertyName");
                    for (int i = 0; i < DocumentProperties.Length; i++)
                    {
                        DocumentProperties[i] = (string)GetExternalDocumentPropertyName.Invoke(currentSession, new object[] { DocumentProperties[i] });
                    }

                    CachedPasswords = new LimitCollection(50, 15);
                }
                else
                {
                    DocumentProperties = new string[0];
                }
            }
            else
            {
                DocumentProperties = new string[0];
            }
        }

        internal bool PasswordRequest(IPasswordProtected sender)
        {
            var args = new System.ComponentModel.CancelEventArgs(true);
            if (Environment.UserInteractive)
            {
                Type type = Type.GetType("FWBS.OMS.Session, OMS.Library", true, true);
                object currentSession = type.GetProperty("CurrentSession").GetValue(null);
                var OnPasswordRequest = type.GetMethod("OnPasswordRequest", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                OnPasswordRequest.Invoke(currentSession, new object[] { sender, args });
            }
            return !args.Cancel;
        }

        internal void ThrowPasswordRequestCancelled()
        {
            var PasswordRequestCancelled = Enum.ToObject(Type.GetType("FWBS.OMS.HelpIndexes, OMS.Library", true, true), 82);
            Type type = Type.GetType("FWBS.OMS.Security.SecurityException, OMS.Library", true, true);
            Exception ex = (Exception)Activator.CreateInstance(type, PasswordRequestCancelled);
            throw ex;
        }

        #region IDocPropHandler Members
        public bool Handles(System.IO.FileInfo file)
        {
            var extensions = new string[]
            {
                ".DOC", ".DOCX", ".DOCM", ".DOT", ".DOTX", ".DOTM", ".XLS", ".XLSX", ".XLSM", ".XLT", ".XLTX", ".XLTM"
            };

            return DocumentProperties.Length > 0 && file != null && Array.IndexOf(extensions, file.Extension.ToUpperInvariant()) >= 0;
        }

        public IRawDocument CreateDocument(System.IO.FileInfo file)
        {
            string ext = (file != null) ? file.Extension.ToUpperInvariant() : null;
            switch (ext)
            {
                case ".DOC":
                case ".DOCX":
                case ".DOCM":
                case ".DOT":
                case ".DOTX":
                case ".DOTM":
                    return new OfficeWordDocument(this);
                case ".XLS":
                case ".XLSX":
                case ".XLSM":
                case ".XLT":
                case ".XLTX":
                case ".XLTM":
                    return new OfficeExcelDocument(this);
                default:
                    return null;
            }
        }
        #endregion
    }
}
