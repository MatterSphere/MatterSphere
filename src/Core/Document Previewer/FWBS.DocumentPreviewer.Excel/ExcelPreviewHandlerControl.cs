using System;
using System.IO;
using FWBS.Controls;

namespace Fwbs.Documents.Preview.Excel
{
    using Handlers;

    [System.Runtime.InteropServices.Guid(ExcelPreviewHandlerFactory.ClassID)]
	public partial class ExcelPreviewHandlerControl : PreviewHandlerFromFile
	{
		public ExcelPreviewHandlerControl()
		{
			InitializeComponent();

			var lic = new Aspose.Cells.License();
			lic.SetLicense("Aspose.Total.lic");
		}

        public override void DoPreview()
        {
            var xpsLocation = CreateTempPath(".xps");

            if (HasChanged && (!File.Exists(xpsLocation) || File.GetLastWriteTimeUtc(xpsLocation) != file.LastWriteTimeUtc))
            {
                ConvertExcelSpreadsheetToXPS(xpsLocation);
            }

            if (!File.Exists(xpsLocation))
            {
                throw new InvalidOperationException(string.Empty);
            }

            ((XPSDocViewer)elementHost1.Child).Preview(xpsLocation);
        }

        private void ConvertExcelSpreadsheetToXPS(string xpsLocation)
        {            
            var previewFile = CreateTempPath(file.Extension);
            file.CopyTo(previewFile, true);

            try
            {
                using (Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(previewFile))
                {
                    foreach (Aspose.Cells.Worksheet ws in wb.Worksheets)
                    {
                        Aspose.Cells.PageSetup pageSetup = ws.PageSetup;
                        pageSetup.BottomMargin = Math.Min(pageSetup.BottomMargin, 1.0);
                        pageSetup.TopMargin = Math.Min(pageSetup.TopMargin, 1.0);
                        pageSetup.LeftMargin = Math.Min(pageSetup.LeftMargin, 1.0);
                        pageSetup.RightMargin = Math.Min(pageSetup.RightMargin, 1.0);
                    }

                    wb.Save(xpsLocation, new Aspose.Cells.XpsSaveOptions() { OnePagePerSheet = true });
                }
                File.SetLastWriteTimeUtc(xpsLocation, file.LastWriteTimeUtc);
            }
            catch (Aspose.Cells.CellsException ex)
            {
                if (ex.Code == Aspose.Cells.ExceptionType.IncorrectPassword)
                    throw new InvalidOperationException(string.Empty);
                else
                    throw;
            }
        }

		public override void Unload()
		{
            GC.Collect();
            if (this.xpsDocViewer1 != null)
			{
				this.elementHost1.Child = null;     // detach from host
				this.xpsDocViewer1.Dispose();       // get rid of resources
				this.xpsDocViewer1 = null;          // GC should collect
			}
            GC.WaitForPendingFinalizers();
            GC.Collect();
            base.Unload();
		}
	}
}
