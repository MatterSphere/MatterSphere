using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using Horizon.Common.Models.Repositories.ProcessingStatus;
using Horizon.Models.ReportProvider;
using Microsoft.Win32;

namespace Horizon.Providers
{
    public class ReportProvider
    {
        public void GenerateDocumentsReport(List<ReportItem> documents)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = $"{DateTime.Now.ToString("yyyy-M-d dddd HH-mm-ss")}.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(saveFileDialog.FileName))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(documents);
                }
            }
        }

        public void GenerateErrorsReport(List<DocumentErrorInfo> errors)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = $"Process Errors {DateTime.Now.ToString("yyyy-M-d dddd HH-mm-ss")}.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(saveFileDialog.FileName))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(errors);
                }
            }
        }
    }
}
