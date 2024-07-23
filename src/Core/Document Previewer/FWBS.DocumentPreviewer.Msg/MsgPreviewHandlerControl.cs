using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Fwbs.Documents.Preview.Msg
{
    using Aspose.Email;
    using Handlers;


    [System.Runtime.InteropServices.Guid(MsgPreviewHandlerFactory.ClassID)]
	public partial class MsgPreviewHandlerControl : PreviewHandlerFromFile , IPreviewHandlerInfo
	{
		
		private MailMessage msg;
		private IPreviewer previewer;
	   
		public MsgPreviewHandlerControl(IPreviewer previewer)
		{
			var lic = new Aspose.Email.License();
			lic.SetLicense("Aspose.Total.lic");

			this.previewer = previewer;
			InitializeComponent();
			var ucPreviewer = previewer.UIElement as Control ?? previewer as Control;
			if (ucPreviewer != null)
			{
				subPreviewContainer.Controls.Add(ucPreviewer);
				ucPreviewer.Dock = DockStyle.Fill;
			}
		}

		#region IPreviewHandler Members

		public override void DoPreview()
		{
			if (msg.IsSigned)
			{
				string msgFailMessage = "Unable to preview - Email has been secured. Please Open In Outlook";
				if (cultureData.ContainsKey("SecureEmail"))
					msgFailMessage = cultureData["SecureEmail"];

				throw new NotSupportedException(msgFailMessage);
			}    

			//HTML Body?!
			webBrowser1.DocumentText = msg.HtmlBody;
			ucMsgHeader1.SetupControl(msg);
			AttachEvents();
		}

		public override void Unload()
		{            
            GC.Collect();
            this.DetachEvents();
			if (this.msg != null)
			{
				this.msg.Dispose();
				this.msg = null;
			}
			if (this.previewer != null)
			{
				// do we need to release this here?
				this.previewer.Unload();
				this.previewer = null;
			}

			// remove and get rid of web browser control
			this.Controls.Remove(this.webBrowser1);
			this.webBrowser1.Dispose();
			this.webBrowser1 = null;

            GC.WaitForPendingFinalizers();
            GC.Collect();
            base.Unload();
		}
		#endregion

		#region IInitializeWithFile Members

		public override void Initialize(string pszFilePath, uint grfMode)
		{

			base.Initialize(pszFilePath, grfMode);

			if (this.msg != null)
			{
				this.msg.Dispose();
				this.msg = null;
			}
            this.msg = Aspose.Email.MailMessage.Load(file.FullName, new Aspose.Email.MsgLoadOptions());
		}

	  
		#endregion

		private void AttachEvents()
		{
			DetachEvents();

			this.ucMsgHeader1.AttatchmentClicked += ucMsgHeader1_AttatchmentClicked;
			this.ucMsgHeader1.MessageClicked += ucMsgHeader1_MessageClicked;
		}

		void ucMsgHeader1_MessageClicked(object sender, EventArgs e)
		{
			this.webBrowser1.BringToFront();
		}

		void ucMsgHeader1_AttatchmentClicked(object sender, EventArgs e)
		{
			if (previewer == null)
				return;

			this.subPreviewContainer.BringToFront();

			if (cultureData != null)
				previewer.CultureProperties = cultureData;

			Label lbl = sender as Label;

			if (lbl == null)
			{
			   
				previewer.PreviewFile(null);
				return;
			}
			string filename = null;
			if (lbl.Tag is string)
				filename= ((string)lbl.Tag).Trim();

			if (filename == null)
			{
				previewer.PreviewFile(null);
				return;
			}

			try
			{
				previewer.PreviewFile(new System.IO.FileInfo(filename));
			}
			catch { }
		}

		private void DetachEvents()
		{
			//Events were never being detached.  Add code to detach when called and see how this impacts memory
			this.ucMsgHeader1.AttatchmentClicked -= ucMsgHeader1_AttatchmentClicked;
			this.ucMsgHeader1.MessageClicked -= ucMsgHeader1_MessageClicked;
		}

		private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			if (msg.LinkedResources.Count > 0 && webBrowser1.Document.Images.Count > 0)
			{
				System.IO.DirectoryInfo dir = CreateTempDirectory(file.FullName);
				
				foreach (HtmlElement img in webBrowser1.Document.Images)
				{
					string url = img.GetAttribute("src");

					foreach (var att in msg.LinkedResources)
					{
						if (url.IndexOf(att.ContentId) > -1)
						{
							string attfile = GetLocalAttatchmentFile(dir, att);
							img.SetAttribute("src", new Uri(attfile).AbsoluteUri);
							img.SetAttribute("alt", att.ContentId);

							ucMsgHeader1.RemoveAttatchmentDisplayList(att.ContentId);

						}
					}
				}
			}
		}

		private static string GetLocalAttatchmentFile(System.IO.DirectoryInfo dir,  Aspose.Email.LinkedResource att)
		{
			string attfile = System.IO.Path.Combine(dir.FullName, att.ContentId);
			if (!System.IO.File.Exists(attfile))
				att.Save(attfile);
			return attfile;
		}

		private static System.IO.DirectoryInfo CreateTempDirectory(string file)
		{
			string checksum = GenerateChecksum(file);
			string temp = System.IO.Path.GetTempPath();
			temp =System.IO.Path.Combine(temp, checksum);
			if (!System.IO.Directory.Exists(temp))
				System.IO.Directory.CreateDirectory(temp);
			
			return new System.IO.DirectoryInfo(temp);
		}

		private static string GenerateChecksum(string text)
		{
			System.Text.UnicodeEncoding enc = new System.Text.UnicodeEncoding();
			byte[] input = enc.GetBytes(text);

			using (System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
			{
				byte[] hash = sha1.ComputeHash(input);
				System.Text.StringBuilder buff = new System.Text.StringBuilder();
				foreach (byte hashByte in hash)
				{
					buff.Append(String.Format("{0:X1}", hashByte));
				}

				return buff.ToString();
			}
		}

		#region IPreviewHandlerInfo Members
		private Dictionary<string, string> cultureData;

		public void SetCultureData(Dictionary<string, string> cultureData)
		{
			this.cultureData = cultureData;
			ucMsgHeader1.Culture(cultureData);
		}
		private bool supportsFullPreview = true;
		public void SetPreviewSupport(bool fullPreviewSupport)
		{
			this.supportsFullPreview = fullPreviewSupport;
		}

		public void SetAdditionalProperties(Dictionary<string, string> additionalProperties)
		{
			ucMsgHeader1.AdditionalProperties(additionalProperties);
		}
		#endregion
	}
}
