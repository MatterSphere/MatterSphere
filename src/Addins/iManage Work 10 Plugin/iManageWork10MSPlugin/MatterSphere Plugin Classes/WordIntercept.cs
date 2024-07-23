using System;
using iManage.Work.Tools;
using MatterSphereIntercept.MatterSphere_Plugin_Classes;

namespace MatterSphereIntercept
{
    class WordIntercept : General
    {

        IWordPlugInHost word_host;

        private dynamic word = null;

        private dynamic Word
        {
            get
            {
                if (word == null)
                    word = word_host.WordApplication;
                return word;
            }
        }
        
        protected override dynamic ComAddin => Word.ComAddins[AddinConstants.OMS_COM_ADDIN].Object;

        protected override void InternalInitialize(IPlugInHost host)
        {
            word_host = host as IWordPlugInHost;
        }

        protected override void Host_Shutdown(object sender, EventArgs e)
        {
            Shutdown();
            word_host.DocumentOpen -= WordHostOnDocumentOpen;
            word_host.DocumentBeforeClose -= WordHostOnDocumentBeforeClose;
            word_host.DocumentSaveAs -= WordHostOnDocumentSaveAs;
        }

        private void WordHostOnDocumentBeforeClose(object sender, WOfficeEventArgs e)
        {
            ExecuteOperation(() =>
            {
                if (e.DocumentProfile != null)
                {
                    CompleteSilentSaveWorkItem(e.DocumentProfile);
                }
                else
                {
                    string[] mergedData = AddinWrapper.GetMergedFieldDataAsStringArray();

                    if (mergedData != null)
                    {
                        IntegrationMetadata metadata = new IntegrationMetadata(mergedData);
                        SaveNewDocumentSilently(metadata, e.OfficeDocument);
                    }
                }
            });
        }

        private void WordHostOnDocumentOpen(object sender, WOfficeEventArgs e)
        {
            ExecuteOperation(() =>
            {
                StartSilentSaveWorkItem(e.OfficeDocument, e.DocumentProfile);
            });
        }

        protected override void Host_Startup(object sender, EventArgs e)
        {
            word_host = sender as IWordPlugInHost;

            word_host.DocumentOpen += WordHostOnDocumentOpen;
            word_host.DocumentBeforeClose += WordHostOnDocumentBeforeClose;
            word_host.DocumentSaveAs += WordHostOnDocumentSaveAs;
        }

        private void WordHostOnDocumentSaveAs(object sender, WOfficeSaveAsEventArgs e)
        {
            ExecuteOperation(() =>
            {
                StartSilentSaveWorkItem(e.OfficeDocument, e.NewDocumentProfile);
            });
        }

        protected override void SaveForm_OnInitialize(object sender, WFormEventArgs e)
        {
            ExecuteOperation(() =>
            {
                string[] mergedData = AddinWrapper.GetMergedFieldDataAsStringArray();
                if (mergedData != null)
                {
                    IntegrationMetadata metadata = new IntegrationMetadata(mergedData);

                    if (!string.IsNullOrWhiteSpace(metadata.ClientNumber) &&
                        !string.IsNullOrWhiteSpace(metadata.MatterNumber))
                    {
                        WLog.Info(string.Format("Obtained from Word's active document : Client Number : '{0}' ; Matter Number : '{1}'", metadata.ClientNumber, metadata.MatterNumber));

                        SetupSaveForm(e, metadata);
                    }
                    else
                    {
                        WLog.Info("Either this Word document is not a MatterSphere document or it does not contain correctly mapped Client and Matter Number values. iManage Work must handle the document.");
                    }
                }
            });
        }
      
    }
}
