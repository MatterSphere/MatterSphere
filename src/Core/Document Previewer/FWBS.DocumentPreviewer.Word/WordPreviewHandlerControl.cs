using System;
using System.IO;
using FWBS.Controls;

namespace Fwbs.Documents.Preview.Word
{
    using Handlers;

    [System.Runtime.InteropServices.Guid(WordPreviewHandlerFactory.ClassID)]
	public partial class WordPreviewHandlerControl : PreviewHandlerFromFile
	{
		public WordPreviewHandlerControl()
		{
			InitializeComponent();

			var lic = new Aspose.Words.License();
			lic.SetLicense("Aspose.Total.lic");
		}

		public override void DoPreview()
		{
			var xpsLocation = CreateTempPath(".xps");

			if (HasChanged && (!File.Exists(xpsLocation) || File.GetLastWriteTimeUtc(xpsLocation) != file.LastWriteTimeUtc))
			{
				ConvertWordDocumentToXPS(xpsLocation);
			}

			if (!File.Exists(xpsLocation))
			{
				throw new InvalidOperationException(string.Empty);
			}

			((XPSDocViewer)elementHost1.Child).Preview(xpsLocation);
		}

		private void ConvertWordDocumentToXPS(string xpsLocation)
		{
			var previewFile = CreateTempPath(file.Extension);
			file.CopyTo(previewFile, true);

			try
			{
				Aspose.Words.Document d = new Aspose.Words.Document(previewFile);
				d.AcceptAllRevisions();
				d.Save(xpsLocation, Aspose.Words.SaveFormat.Xps);
				File.SetLastWriteTimeUtc(xpsLocation, file.LastWriteTimeUtc);
			}
			catch (Aspose.Words.IncorrectPasswordException)
			{
				throw new InvalidOperationException(string.Empty);
			}
		}

		public override void Unload()
		{
            GC.Collect();
            if (System.Threading.Thread.CurrentThread.GetApartmentState() == System.Threading.ApartmentState.STA)
            {
                // get rid of the wpf control
                if (this.xpsDocViewer1 != null)
                {
                    this.elementHost1.Child = null;		// detach from host
                    this.xpsDocViewer1.Dispose();		// get rid of it
                    this.xpsDocViewer1 = null;          // GC should collect
                }
            }
            GC.WaitForPendingFinalizers();
            GC.Collect();
			base.Unload();
		}
	}
}
