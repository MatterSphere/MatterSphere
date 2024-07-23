using System;
using System.Runtime.InteropServices;
using iManage.Work.Tools;
using MatterSphereIntercept.MatterSphere_Plugin_Classes;


namespace MatterSphereIntercept
{
    public class ExcelIntercept : General
    {

        IExcelPlugInHost excel_host;

        private dynamic excel = null;

        private dynamic Excel
        {
            get
            {
                if (excel == null)
                    excel = excel_host.ExcelApplication;
                return excel;
            }
        }

        protected override dynamic ComAddin => Excel.ComAddins[AddinConstants.OMS_COM_ADDIN].Object;
        
        protected override void InternalInitialize(IPlugInHost host)
        {
            excel_host = host as IExcelPlugInHost;
        }
        
        protected override void Host_Shutdown(object sender, EventArgs e)
        {
            Shutdown();
            excel_host.WorkbookOpen -= ExcelHostOnWorkbookOpen;
            excel_host.WorkbookBeforeClose -= ExcelHostOnWorkbookBeforeClose;
            excel_host.WorkbookSaveAs -= ExcelHostOnWorkbookSaveAs;
        }
        
        protected override void Host_Startup(object sender, EventArgs e)
        {
            excel_host = sender as IExcelPlugInHost;

            excel_host.WorkbookOpen += ExcelHostOnWorkbookOpen;
            excel_host.WorkbookBeforeClose += ExcelHostOnWorkbookBeforeClose;
            excel_host.WorkbookSaveAs += ExcelHostOnWorkbookSaveAs;
        }

        private void ExcelHostOnWorkbookBeforeClose(object sender, WOfficeEventArgs e)
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

                    if (mergedData != null && mergedData.Length > 1)
                    {
                        IntegrationMetadata metadata = new IntegrationMetadata(mergedData);
                        SaveNewDocumentSilently(metadata, e.OfficeDocument);
                    }
                }
            });
        }

        private void ExcelHostOnWorkbookSaveAs(object sender, WOfficeSaveAsEventArgs e)
        {
            ExecuteOperation(() => { StartSilentSaveWorkItem(e.OfficeDocument, e.NewDocumentProfile); });
        }

        private void ExcelHostOnWorkbookOpen(object sender, WOfficeEventArgs e)
        {
            ExecuteOperation(() =>
            {
                StartSilentSaveWorkItem(e.OfficeDocument, e.DocumentProfile);
            });
        }

        protected override void SaveForm_OnInitialize(object sender, WFormEventArgs e)
        {
            ExecuteOperation(() =>
            {
                bool matterSphereWorkbook = false;
                string[] mergedData = AddinWrapper.GetMergedFieldDataAsStringArray();

                if (mergedData != null)
                {
                    matterSphereWorkbook = true;

                    IntegrationMetadata metadata = new IntegrationMetadata(mergedData);

                    if (!string.IsNullOrWhiteSpace(metadata.ClientNumber) && !string.IsNullOrWhiteSpace(metadata.MatterNumber))
                    {
                        WLog.Info(string.Format("Obtained from Excel's active workbook : Client Number : '{0}' ; Matter Number : '{1}'", metadata.ClientNumber, metadata.MatterNumber));

                        SetupSaveForm(e, metadata);
                    }
                    else
                    {
                        WLog.Info("Either this Excel workbook is not a MatterSphere document or it does not contain correctly mapped Client and Matter Number values.  Let iManage Work handle document.");
                    }
                }

                if (!matterSphereWorkbook)
                    WLog.Info("Active workbook is not a MatterSphere Workbook - iManage Work must handle the workbook.");
            });
        }

        

    }
}
