using System;
using System.Collections.Generic;

namespace FWBS.OMS.DocumentManagement
{
    using Storage;
    using Document = OMSDocument;

    public class SendDocumentViaEmailCommand : FWBS.OMS.Commands.Command
    {
        public SendDocumentViaEmailCommand()
        {
            EmailTemplate = Precedent.GetPrecedent("DOCSEND", "EMAIL", "", "SYSTEM", "", "");
        }

        #region Properties

        public string To { get; set; }

        public string BCC { get; set; }

        public string CC { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string HtmlBody { get; set; }

        public bool AttachOutgoingVariables { get; set; }

        public Precedent EmailTemplate { get; set; }

        public bool ConvertToPDF { get; set; }

        public bool AsPDF { get; set; }

        public bool SendLink { get; set; }



        private List<string> documentIds = new List<string>();
        public List<string> DocumentIdsToDisplay 
        {
            get
            {
                return documentIds;
            }

        }

        private readonly List<FWBS.OMS.DocumentManagement.Storage.IStorageItem> attdocs = new List<FWBS.OMS.DocumentManagement.Storage.IStorageItem>();
        public List<FWBS.OMS.DocumentManagement.Storage.IStorageItem> DocumentsToAttach
        {
            get
            {
                return attdocs;
            }
        }

        private readonly List<System.IO.FileInfo> attfiles = new List<System.IO.FileInfo>();
        public List<System.IO.FileInfo> FilesToAttach
        {
            get
            {
                return attfiles;
            }
        }


        public Associate ToAssociate { get; set; }

        public bool AdditionalCCs { get; set; }

        public bool AdditionalBCCs { get; set; }

        public bool AdditionalTos { get; set; }

        

        private PrecedentJob job; 
        public PrecedentJob JobResult
        {
            get
            {
                return job;
            }
        }
        #endregion


        #region IProcess Members

        protected InvalidOperationException CreateAssociateMissingException()
        {
            throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("MSGASCSNMTBESPC", "Associate to send to must be specified.", "").Text);
        }

        protected InvalidOperationException CreateEmailTemplateMissingException()
        {
            throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("MSGEMLMSTBSPC", "Email precedent must be specified.", "").Text);
        }

         public override FWBS.OMS.Commands.ExecuteResult Execute()
        {
            job = null;

            if (!Session.CurrentSession.IsMailEnabled)
                throw new MailDisabledException();

            if (EmailTemplate == null)
                throw CreateEmailTemplateMissingException();

            if (ToAssociate == null)
                throw CreateAssociateMissingException();

            FWBS.OMS.Commands.ExecuteResult res = new FWBS.OMS.Commands.ExecuteResult();

            if (AttachOutgoingVariables)
            {
                for (int ctr = DocumentsToAttach.Count-1; ctr>=0; ctr--)
                {
                    IStorageItem item = DocumentsToAttach[ctr];

                    try
                    {
                        Document doc = item as Document;
                        DocumentVersion version = item as DocumentVersion;
                        if (version != null)
                            doc = version.ParentDocument;

                        StorageProvider provider = item.GetStorageProvider();

                        StorageSettingsCollection settings = item.GetSettings();
                        if (settings == null)
                            settings = provider.GetDefaultSettings(item, SettingsType.Fetch);

                        LockableFetchSettings locksettings = settings.GetSettings<LockableFetchSettings>();
                        VersionFetchSettings versettings = settings.GetSettings<VersionFetchSettings>();

                        if (locksettings != null)
                            locksettings.CheckOut = false;

                        if (versettings != null)
                            versettings.Version = VersionFetchSettings.FetchAs.Current;

                        FetchResults fres = provider.Fetch(item, true, settings);

                        System.IO.FileInfo file = fres.LocalFile;

                        FWBS.OMS.Interfaces.IOMSApp.AttachOutboundDocumentVars(file, fres.Item);

                        FilesToAttach.Add(file);
                        if (!DocumentIdsToDisplay.Contains(item.DisplayID))
                            DocumentIdsToDisplay.Add(item.DisplayID);
                        DocumentsToAttach.Remove(item);
                    }

                    catch (Exception ex)
                    {
                        if (ContinueOnError)
                        {
                            res.Errors.Add(ex);
                        }
                        else
                            throw;
                    }
                }
            }

            job = CreateJob();

            res.Status = FWBS.OMS.Commands.CommandStatus.Success;

            return res;
        }

        private PrecedentJob CreateJob()
        {
            PrecedentJob job = new PrecedentJob(EmailTemplate);
            job.SaveMode = PrecSaveMode.None;
            job.PrintMode = PrecPrintMode.None;
            job.Associate = ToAssociate;

            job.Params.Add("TO", AdditionalTos);
            job.Params.Add("CC", AdditionalCCs);
            job.Params.Add("BCC", AdditionalBCCs);

            job.Params.Add("SENDLINK", SendLink);

            if (!String.IsNullOrEmpty(To))
                job.Params.Add("TO_ADD", To);

            if (!String.IsNullOrEmpty(CC))
                job.Params.Add("CC_ADD", CC);

            if (!String.IsNullOrEmpty(BCC))
                job.Params.Add("BCC_ADD", BCC);

            if (!String.IsNullOrEmpty(Body))
                job.Params.Add("BODY", Body);

            if (!String.IsNullOrEmpty(HtmlBody))
                job.Params.Add("HTMLBODY", HtmlBody);

            if (!String.IsNullOrEmpty(Subject))
                job.Params.Add("SUBJECT", Subject);

            List<string> docids = new List<string>();

            if (DocumentsToAttach.Count > 0)
            {
                job.Params.Add("ATTACH_ADD", DocumentsToAttach.ToArray());

                foreach (var doc in DocumentsToAttach)
                {
                    if (!DocumentIdsToDisplay.Contains(doc.DisplayID))
                        DocumentIdsToDisplay.Add(doc.DisplayID);
                }
            }

            if (FilesToAttach.Count > 0)
            {
                job.Params.Add("ATTACH_FILES", FilesToAttach.ToArray());
            }

            if (DocumentIdsToDisplay.Count > 0)
            {
                job.Params.Add("ATTACHDOCID", String.Join(", ", DocumentIdsToDisplay.ToArray()));
            }

            //Add property for PDF
            if (ConvertToPDF)
                job.Params.Add("CONVERTPDF", "True");

            if (AsPDF)
                job.Params.Add("ASPDF", "True");          

            job.ApplyParams();

            return job;
        }


       


        #endregion
    }
}
