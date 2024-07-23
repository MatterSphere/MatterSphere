using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace FWBS.Controls
{
    /// <summary>
    /// Interaction logic for XPSDocViewer.xaml
    /// </summary>
    public partial class XPSDocViewer : UserControl, IDisposable
	{
		// the current XPSDocument we are processing
		private XpsDocument xpsDoc = null;

		public XPSDocViewer()
		{
			InitializeComponent();
		}

		public void Preview(string xpsDocPath)
		{
			// get rid of previous doc
			if (this.xpsDoc != null)
			{
				this.docViewer.Document = null;
				this.xpsDoc.Close();
				this.xpsDoc = null;
			}

			this.xpsDoc = new XpsDocument(xpsDocPath, System.IO.FileAccess.Read);
            this.docViewer.Document = this.xpsDoc.GetFixedDocumentSequence() as IDocumentPaginatorSource;
            this.docViewer.FitToWidth();
		}

		public void Dispose()
		{
			// get rid of previous doc
			if (this.xpsDoc != null)
			{
				this.docViewer.Document = null;
                // this will unload the package from memory as well - see .NET docs
				this.xpsDoc.Close();
				this.xpsDoc = null;
			}
		}
	}
}
