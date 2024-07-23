using System;
using System.Data;
using CrystalDecisions.Shared;
using FWBS.Common;

namespace FWBS.OMS.UI.Windows.Reports
{
    internal enum AutomationMode { Print, Export }

    public class AutomationExecute
    {
        private FWBS.OMS.Report _report = null;

        public AutomationExecute()
        {
        }

        private AutomationMode _mode;
        private string _exportdestination;
        private int _copies;
        private aExportFormatType _exportformattype;
        private aPageMargins? _pagemargins;
        private string _printerName;

        public void Print(string reportCode, FWBS.Common.KeyValueCollection param, Factory.OMSObjectFactoryItem parent, int copies, aPageMargins? margins, string printerName)
        {
            if (String.IsNullOrEmpty(reportCode))
                throw new ArgumentNullException("reportName");

            var pparent = FWBS.OMS.UI.Factory.OMSObjectFactory.Get(parent);
            _copies = copies;
            _pagemargins = margins;
            _printerName = printerName;

            _mode = AutomationMode.Print;
            _report = FWBS.OMS.Report.GetReport(reportCode, pparent, param);
            _report.SearchList.Searched += new FWBS.OMS.SearchEngine.SearchedEventHandler(SearchList_Searched);
            _report.SearchList.Search(false);

        }

        public void Export(string reportCode, FWBS.Common.KeyValueCollection kvc, Factory.OMSObjectFactoryItem parent, aExportFormatType exportFormatType, string destination)
        {
            if (String.IsNullOrEmpty(reportCode))
                throw new ArgumentNullException("reportName");

            var pparent = FWBS.OMS.UI.Factory.OMSObjectFactory.Get(parent);
            _mode = AutomationMode.Export;
            _exportdestination = destination;
            _exportformattype = exportFormatType;
            _report = FWBS.OMS.Report.GetReport(reportCode, pparent, kvc);
            _report.SearchList.Searched += new FWBS.OMS.SearchEngine.SearchedEventHandler(SearchList_Searched);
            _report.SearchList.Search(false);

        }


        void SearchList_Searched(object sender, FWBS.OMS.SearchEngine.SearchedEventArgs e)
        {
            try
            {
                using (CrystalDecisions.CrystalReports.Engine.ReportDocument crpe = new CrystalDecisions.CrystalReports.Engine.ReportDocument())
                {
                    using (CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer())
                    {
                        //Load the report
                        crpe.Load(_report.ReportLocation.FullName);
                        if (e.DataSet != null)
                        {
                            using (DataSet data = e.DataSet.Clone())
                            {
                                foreach (DataTable dt in data.Tables)
                                {
                                    foreach (DataColumn col in dt.Columns)
                                    {
                                        if (col.DataType == typeof(DateTime))
                                        {
                                            col.DateTimeMode = DataSetDateTime.Local;
                                        }
                                    }
                                }
                                data.Merge(e.DataSet, false, MissingSchemaAction.Ignore);
                                data.AcceptChanges();
                                string[] _tables = _report.SearchList.Tables;
                                for (int i = 0; i < data.Tables.Count; i++)
                                {
                                    try
                                    {
                                        data.Tables[i].TableName = _tables[i];
                                    }
                                    catch
                                    {
                                        data.Tables[i].TableName = "REPORTS" + (i + 1).ToString();
                                    }
                                    try
                                    {
                                        crpe.Database.Tables[i].SetDataSource(data.Tables[i]);
                                    }
                                    catch
                                    { }
                                }
                            }
                        }
                        else
                        {
                            using (DataTable data = e.Data.Clone())
                            {
                                foreach (DataColumn col in data.Columns)
                                {
                                    if (col.DataType == typeof(DateTime))
                                    {
                                        col.DateTimeMode = DataSetDateTime.Local;
                                    }
                                }
                                data.Merge(e.Data, false, MissingSchemaAction.Ignore);
                                data.AcceptChanges();
                                string[] _tables = _report.SearchList.Tables;
                                try
                                {
                                    data.TableName = _tables[0];
                                    crpe.Database.Tables[0].SetDataSource(data);
                                }
                                catch
                                {
                                    data.TableName = "REPORTS1";
                                    crpe.Database.Tables[0].SetDataSource(data);
                                }

                            }
                        }

                        //Replace fields beginning with @ in report objects 
                        FWBS.OMS.FieldParser fparse = new FWBS.OMS.FieldParser(_report.SearchList.Parent);

                        foreach (CrystalDecisions.CrystalReports.Engine.ReportObject repObj in crpe.ReportDefinition.ReportObjects)
                        {
                            // Text Field so parse for update.
                            if (repObj is CrystalDecisions.CrystalReports.Engine.TextObject)
                            {
                                CrystalDecisions.CrystalReports.Engine.TextObject txtObj = repObj as CrystalDecisions.CrystalReports.Engine.TextObject;
                                if (txtObj.Text.StartsWith("@"))
                                {
                                    txtObj.Text = txtObj.Text.Substring(1);
                                    txtObj.Text = FWBS.OMS.Session.CurrentSession.Terminology.Parse(txtObj.Text, true);

                                    txtObj.Text = fparse.ParseString(txtObj.Text);
                                }
                            }
                        }

                        crystalReportViewer.ReportSource = crpe;
                        switch (_mode)
                        {
                            case AutomationMode.Print:
                                if (_pagemargins != null)
                                    crpe.PrintOptions.ApplyPageMargins(new PageMargins(_pagemargins.Value.leftMargin, _pagemargins.Value.topMargin, _pagemargins.Value.rightMargin, _pagemargins.Value.bottomMargin));
                                if (_printerName != null)
                                    crpe.PrintOptions.PrinterName = _printerName;
                                crpe.PrintToPrinter(_copies, false, 0, 0);
                                break;
                            case AutomationMode.Export:
                                try
                                {
                                    ExportOptions CrExportOptions;
                                    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                                    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                                    CrDiskFileDestinationOptions.DiskFileName = _exportdestination;
                                    CrExportOptions = crpe.ExportOptions;
                                    {
                                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                                        CrExportOptions.ExportFormatType = (ExportFormatType)ConvertDef.ToEnum(_exportformattype, ExportFormatType.PortableDocFormat);
                                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                                    }
                                    crpe.Export();

                                }
                                catch (Exception ex)
                                {
                                    ErrorBox.Show(ex);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
        }
    }
}
