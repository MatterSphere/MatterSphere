using System;
using FWBS.Controls;

namespace Fwbs.Documents.Preview.Xps
{
    using System.Runtime.InteropServices;
    using Handlers;

    [Guid(XpsPreviewHandlerFactory.ClassID)]
	public partial class XpsPreviewHandlerControl : PreviewHandlerFromFile
	{
		public XpsPreviewHandlerControl()
		{
			InitializeComponent();
			elementHost1.Child = new XPSDocViewer();

		}

		public override void DoPreview()
		{

            ((XPSDocViewer)elementHost1.Child).Preview(file.FullName);
		}

		public override void Unload()
		{           
            GC.Collect();
            XPSDocViewer docViewerCtrl = this.elementHost1.Child as XPSDocViewer;
			if (docViewerCtrl != null)
			{
                this.elementHost1.Child = null;     // detach from host
				docViewerCtrl.Dispose();            // get rid of resources
			}
            GC.WaitForPendingFinalizers();
            GC.Collect();
            base.Unload();
		}
	}
}
