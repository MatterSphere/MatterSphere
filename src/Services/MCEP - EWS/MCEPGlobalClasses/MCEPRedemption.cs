using System;
using System.IO;
using FWBS.OMS;
using FWBS.OMS.DocumentManagement;
using FWBS.OMS.Interfaces;

namespace MCEPGlobalClasses
{
    class MCEPRedemption
    {
        private const string PROPTAG_CUSTOM_PROPERTY = "{00020329-0000-0000-C000-000000000046}";

        internal string AddNamedPropertiesToSavedMsgFile(FileInfo docFile, OMSDocument doc, DocumentVersion ver)
        {
            Redemption.RDOSession rdoSess = Redemption.RedemptionFactory.Default.CreateRDOSession();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            path = System.IO.Path.Combine(path, "Elite", "MCEP", "Temp");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string FileName = DateTime.Now.Ticks.ToString() + ".msg";
            path = System.IO.Path.Combine(path, FileName);
            Redemption.RDOMail msg = rdoSess.CreateMessageFromMsgFile(path, "IPM.Note", 1);
            msg.Import(docFile.FullName, 9);
            AttachDocumentVars(msg, doc, ver);
            msg.SaveAs(docFile.FullName, 9);
            FWBS.Common.COM.DisposeObject(msg);
            msg = null;
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch { }
            FWBS.Common.COM.DisposeObject(rdoSess);
            rdoSess = null;
            return path;
        }

        private void AttachDocumentVars(Redemption.RDOMail obj, OMSDocument doc, DocumentVersion version)
        {
            Associate assoc = doc.Associate;

            if (doc.ID != GetDocVariable(obj, IOMSApp.DOCUMENT, -1))
                SetDocVariable(obj, IOMSApp.DOCUMENT, doc.ID);

            if (doc.ID == GetDocVariable(obj, IOMSApp.DOCUMENT, -1))
            {
                if (HasDocVariable(obj, IOMSApp.PRIVATE))
                    RemoveDocVariable(obj, IOMSApp.PRIVATE);
                if (FWBS.OMS.Session.OMS.CompanyID != GetDocVariable(obj, IOMSApp.COMPANY, -1))
                    SetDocVariable(obj, IOMSApp.COMPANY, FWBS.OMS.Session.OMS.CompanyID);
                if (FWBS.OMS.Session.OMS.DataKey != GetDocVariable(obj, IOMSApp.DATAKEY, String.Empty))
                    SetDocVariable(obj, IOMSApp.DATAKEY, FWBS.OMS.Session.OMS.DataKey);
                if (FWBS.OMS.Session.OMS.SerialNumber != GetDocVariable(obj, IOMSApp.BRANCH, -1))
                    SetDocVariable(obj, IOMSApp.BRANCH, FWBS.OMS.Session.OMS.SerialNumber);
                if (FWBS.OMS.Session.OMS.Edition != GetDocVariable(obj, IOMSApp.EDITION, ""))
                    SetDocVariable(obj, IOMSApp.EDITION, FWBS.OMS.Session.OMS.Edition);
                if (assoc.OMSFile.Client.ClientID != GetDocVariable(obj, IOMSApp.CLIENT, -1))
                    SetDocVariable(obj, IOMSApp.CLIENT, assoc.OMSFile.Client.ClientID);
                if (assoc.OMSFile.ID != GetDocVariable(obj, IOMSApp.FILE, -1))
                    SetDocVariable(obj, IOMSApp.FILE, assoc.OMSFile.ID);
                if (assoc.ID != GetDocVariable(obj, IOMSApp.ASSOCIATE, -1))
                    SetDocVariable(obj, IOMSApp.ASSOCIATE, assoc.ID);
                if (doc.ExternalId != GetDocVariable(obj, IOMSApp.DOCUMENT_EXTERNAL, ""))
                    SetDocVariable(obj, IOMSApp.DOCUMENT_EXTERNAL, doc.ExternalId);
                if (doc.DocumentType != GetDocVariable(obj, IOMSApp.TYPE, ""))
                    SetDocVariable(obj, IOMSApp.TYPE, doc.DocumentType);

                //Set the phase id to the document (if applicable),
                object phaseid = null;
                try { phaseid = doc.GetExtraInfo("phID"); }
                catch { };
                if (phaseid == null || phaseid == DBNull.Value)
                {
                    if (HasDocVariable(obj, IOMSApp.PHASE))
                        RemoveDocVariable(obj, IOMSApp.PHASE);
                }
                else
                    if (phaseid.Equals(GetDocVariable(obj, IOMSApp.PHASE, -1)) == false)
                        SetDocVariable(obj, IOMSApp.PHASE, (long)phaseid);


                Precedent baseprec = doc.BasePrecedent;
                if (baseprec == null)
                {
                    if (HasDocVariable(obj, IOMSApp.BASEPRECEDENT))
                        RemoveDocVariable(obj, IOMSApp.BASEPRECEDENT);
                }
                else
                {
                    if (baseprec.ID != GetDocVariable(obj, IOMSApp.BASEPRECEDENT, -1))
                        SetDocVariable(obj, IOMSApp.BASEPRECEDENT, baseprec.ID);
                }

                Precedent lastprec = doc.LastPrecedent;
                if (lastprec == null)
                {
                    if (HasDocVariable(obj, IOMSApp.PRECEDENT))
                        RemoveDocVariable(obj, IOMSApp.PRECEDENT);
                }
                else
                {
                    if (lastprec.ID != GetDocVariable(obj, IOMSApp.PRECEDENT, -1))
                        SetDocVariable(obj, IOMSApp.PRECEDENT, lastprec.ID);
                }

                //If a particular version is passed then set the document variable.
                if (version == null)
                {
                    if (HasDocVariable(obj, IOMSApp.DOCUMENT_VERSION))
                        RemoveDocVariable(obj, IOMSApp.DOCUMENT_VERSION);
                    if (HasDocVariable(obj, IOMSApp.DOCUMENT_LABEL))
                        RemoveDocVariable(obj, IOMSApp.DOCUMENT_LABEL);
                }
                else
                {
                    Guid stored = new Guid(GetDocVariable(obj, IOMSApp.DOCUMENT_VERSION, Guid.Empty.ToString()));
                    if (version.Id.Equals(stored) == false)
                        SetDocVariable(obj, IOMSApp.DOCUMENT_VERSION, version.Id.ToString());

                    if (version.Label.Equals(GetDocVariable(obj, IOMSApp.DOCUMENT_LABEL, String.Empty)) == false)
                        SetDocVariable(obj, IOMSApp.DOCUMENT_LABEL, version.Label);
                }


            }
        }

        public void RemoveDocVariable(Redemption.RDOMail mail, string name)
        {
            name = Session.CurrentSession.GetExternalDocumentPropertyName(name);

            var prop = mail.UserProperties.Find(name, false);

            if (prop != null)
                prop.Delete();
        }

        public bool HasDocVariable(Redemption.RDOMail mail, string name)
        {
            name = Session.CurrentSession.GetExternalDocumentPropertyName(name);

            return mail.UserProperties.Find(name, false) != null;
        }
        public void SetDocVariable(Redemption.RDOMail mail, string name, long value)
        {
            name = Session.CurrentSession.GetExternalDocumentPropertyName(name);

            Redemption.RDOUserProperty fileProperty = mail.UserProperties.Find(name, false);
            if (fileProperty == null)
            {
                fileProperty = mail.UserProperties.Add(name, Redemption.rdoUserPropertyType.olNumber, false, 0);
            }
            fileProperty.Value = value;

        }
        public void SetDocVariable(Redemption.RDOMail mail, string name, bool value)
        {
            name = Session.CurrentSession.GetExternalDocumentPropertyName(name);

            Redemption.RDOUserProperty fileProperty = mail.UserProperties.Find(name, false);
            if (fileProperty == null)
            {
                fileProperty = mail.UserProperties.Add(name, Redemption.rdoUserPropertyType.olYesNo, false, 0);
            }
            fileProperty.Value = value;

        }
        /// <param name="value"></param>
        public void SetDocVariable(Redemption.RDOMail mail, string name, string value)
        {
            name = Session.CurrentSession.GetExternalDocumentPropertyName(name);

            Redemption.RDOUserProperty fileProperty = mail.UserProperties.Find(name, false);
            if (fileProperty == null)
            {
                fileProperty = mail.UserProperties.Add(name, Redemption.rdoUserPropertyType.olText, false, 0);
            }
            fileProperty.Value = value;

        }
        public object GetDocVariable(Redemption.RDOMail mail, string name, object defaultValue)
        {
            return GetDocVariable<object>(mail, name, defaultValue);
        }
        public T GetDocVariable<T>(Redemption.RDOMail mail, string name, T defaultValue)
        {
            try
            {
                name = Session.CurrentSession.GetExternalDocumentPropertyName(name);

                Redemption.RDOUserProperty prop = mail.UserProperties.Find(name, false);

                if (prop != null)
                    return (T)Convert.ChangeType(prop.Value, typeof(T));
                else
                {
                    var propid = mail.GetIDsFromNames(PROPTAG_CUSTOM_PROPERTY.ToUpperInvariant(), name);
                    object fieldval = mail.get_Fields(propid);

                    if (fieldval != null)
                        return (T)Convert.ChangeType(fieldval, typeof(T));
                    else
                        return defaultValue;
                }
            }
            catch (InvalidCastException)
            {
                return defaultValue;
            }
        }


    }
}
