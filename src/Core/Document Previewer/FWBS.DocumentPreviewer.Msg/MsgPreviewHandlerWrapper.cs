using System.Collections.Generic;

using System.ComponentModel.Composition;

namespace Fwbs.Documents.Preview.Msg
{
    using Handlers;


    public partial class MsgPreviewHandlerWrapper : PreviewHandlerFromFile , IPreviewHandlerInfo
	{
		private MessagePreviewerVM vm;
		 [ImportingConstructor]
		public MsgPreviewHandlerWrapper(IPreviewer previewer)
		{
			var lic = new Aspose.Email.License();
			lic.SetLicense("Aspose.Total.lic");

			InitializeComponent();

			msgPreviewHandlerView1.SetPreviewer(previewer);
			vm = new MessagePreviewerVM();
			this.msgPreviewHandlerView1.DataContext = vm;
		}

		public override void DoPreview()
		{
			vm.DoPreview(file.FullName);
		}

		public override void Unload()
		{
			if (this.msgPreviewHandlerView1 != null)
			{
				this.elementHost1.Child = null;                     // Detach from host
				this.msgPreviewHandlerView1.DataContext = null;     // Detach data which is 'vm'
				vm.Dispose();                                       // get rid of resources
				this.msgPreviewHandlerView1.Dispose();              // get rid of resources
				this.msgPreviewHandlerView1 = null;                 // GC should collect
			}
		}


		#region IPreviewHandlerInfo

		public void SetCultureData(Dictionary<string, string> cultureData)
		{
			vm.SetCultureData(cultureData);
			msgPreviewHandlerView1.attachmentPreviewer.CultureProperties = cultureData;

		}

		public void SetPreviewSupport(bool fullPreviewSupport)
		{
			vm.SetPreviewSupport(fullPreviewSupport);
		}

		public void SetAdditionalProperties(Dictionary<string, string> additionalProperties)
		{
			vm.SetAdditionalProperties(additionalProperties);
		}
#endregion
	}
}
