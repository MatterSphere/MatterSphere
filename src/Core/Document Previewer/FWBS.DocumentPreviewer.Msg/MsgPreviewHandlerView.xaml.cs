using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Fwbs.Documents.Preview.Msg
{
    /// <summary>
    /// Interaction logic for MsgPreviewHandlerView.xaml
    /// </summary>
    public partial class MsgPreviewHandlerView : UserControl, IDisposable
	{
		public MsgPreviewHandlerView()
		{
			InitializeComponent();
		}

		internal IPreviewer attachmentPreviewer;
		internal void SetPreviewer(IPreviewer previewer)
		{
			if (previewer == null)
				return;

			attachmentPreviewer = previewer;
			var ctrl =  (System.Windows.Forms.Control)previewer.UIElement;
			FormsHost.Child = ctrl;
		}

		void WebBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
		{
			if (e.Uri != null) //Stop navigation from web Page (Potential Security issues)
				e.Cancel = true;
		}

		private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var pc = e.OldValue as System.ComponentModel.INotifyPropertyChanged;
			if (pc != null)
				pc.PropertyChanged -= pc_PropertyChanged;

			pc = e.NewValue as System.ComponentModel.INotifyPropertyChanged;

			if (pc != null) 
				pc.PropertyChanged += pc_PropertyChanged;
		}

		void pc_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName != "AttachmentLocation")
				return;

			if (attachmentPreviewer == null)
				return;

			var vm = sender as MessagePreviewerVM;
			if (vm == null || string.IsNullOrWhiteSpace(vm.AttachmentLocation))
			{
				attachmentPreviewer.Unload();
				return;
			}

			System.Diagnostics.Debug.WriteLine("Loading Item Preview");
			attachmentPreviewer.Load();
			attachmentPreviewer.PreviewFile(new System.IO.FileInfo(vm.AttachmentLocation));
			FormsHost.Child.Refresh();
		}

		public void Dispose()
		{
			// get rid of host control
			if (FormsHost != null)
			{
				if (FormsHost.Child != null)
				{ 
					FormsHost.Child = null;
					System.Diagnostics.Debug.WriteLine("Clearing host");
				}

				FormsHost.Dispose();
			}
			// get rid of the previewer
			attachmentPreviewer.Unload();
			this.attachmentPreviewer = null;

            // unattach event (attached in .xaml)
            this.webBrowser.Navigating -= this.WebBrowser_Navigating;
            this.webBrowser.Dispose();
            this.webBrowser = null;
		}
	}
}
