using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Fwbs.Documents.Preview.Image
{
    using System.Runtime.InteropServices;
    using Handlers;

    [Guid(PicturePreviewHandlerFactory.ClassID)]
	internal partial class PicturePreviewHandlerControl : PreviewHandlerFromFile
	{
		public PicturePreviewHandlerControl()
		{
			InitializeComponent();
		}

		public override void DoPreview()
		{
			if (!ShowPicture(file.FullName))
				throw new NotSupportedException("Cannot display, file is not a picture.");

			this.Visible = true;
			this.BringToFront();
		}

		public override void Unload()
		{            
            GC.Collect();
            // get rid of any bitmap object we may have
            if (this.pictureBox1.Image != null)
			{ 
				Bitmap bmp = this.pictureBox1.Image as Bitmap;
				if (bmp != null)
				{
					this.pictureBox1.Image = null;
					bmp.Dispose();
				}
			}
            GC.WaitForPendingFinalizers();
            GC.Collect();
            base.Unload();
		}

		public bool ShowPicture(string path)
		{
			if (!System.IO.File.Exists(path))
				throw new System.IO.FileNotFoundException("Image File Not Found", path);

			bool imageDisplayed = true;
			try
			{
				System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(path);

				this.pictureBox1.Image = bmp;
				this.SetImageMode();
			}
			catch (ArgumentException)
			{
				imageDisplayed = false;
			}

			return imageDisplayed;

		}

		public void ShowProcessingImage()
		{
			this.menuStrip1.Visible = false;
			this.pictureBox1.Image = global::FWBS.DocumentPreviewer.Image.Properties.Resources.LoadingProgressBar;
			this.pictureBox1.Refresh();
		}

		private void ShowFullScreen()
		{
			Type type = Type.GetType("FWBS.Common.UI.Windows.frmFullScreen, FWBS.Common.UI", false);
			if (type != null)
			{
				using (Form fScreen = (Form)Activator.CreateInstance(type))
				{
					type.InvokeMember("DisplayImage", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, fScreen, new object[] { pictureBox1.Image });
					fScreen.ShowDialog(this);
				}
			}
		}

		private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowFullScreen();
		}

		private void pictureBox1_DoubleClick(object sender, EventArgs e)
		{
			ShowFullScreen();
		}

		private void pictureBox1_Resize(object sender, EventArgs e)
		{
			SetImageMode();
		}

		private void SetImageMode()
		{

			if (this.pictureBox1.Image == null)
				return;

			this.menuStrip1.Visible = true;

			if (this.pictureBox1.Image.Size.Height > this.pictureBox1.Size.Height || this.pictureBox1.Image.Size.Width > this.pictureBox1.Size.Width)
				this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			else
				this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;


		}

	}
}
