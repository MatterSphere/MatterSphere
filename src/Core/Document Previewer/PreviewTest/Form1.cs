using System;
using System.Windows.Forms;
using Fwbs.Framework.ComponentModel.Composition;

namespace PreviewTest
{
    using Fwbs.Documents.Preview;

    public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			SetupContainer();
		}
		private Fwbs.Framework.Session internalsession;
		private Fwbs.Framework.Session InternalSession
		{
			get
			{
				return internalsession;
			}
		}
		new public Fwbs.Framework.ComponentModel.Composition.IContainer Container
		{
			get
			{
				return InternalSession.GetService<Fwbs.Framework.ComponentModel.Composition.IContainer>();
			}
		}
		private void SetupContainer()
		{
			if (internalsession != null)
				return;

			internalsession = new Fwbs.Framework.Session();

			internalsession.Initialize += new EventHandler<Fwbs.Framework.SessionInitializeEventArgs>(sessioncontainer_Initialize);
		}

		private void sessioncontainer_Initialize(object sender, Fwbs.Framework.SessionInitializeEventArgs e)
		{
			e.Catalogs.Add(new AssemblyCatalog(typeof(PreviewerControl).Assembly, e.Session));
			e.Catalogs.Add(new FileCatalog (@"c:\omscoreassemblies\oms.infrastructure.dll", e.Session));
			e.Catalogs.Add(new DirectoryCatalog(System.IO.Path.Combine(@"c:\OMSCoreAssemblies", "Modules"),e.Session));

			internalsession.Initialize -= new EventHandler<Fwbs.Framework.SessionInitializeEventArgs>(sessioncontainer_Initialize);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			var container = internalsession.GetService<Fwbs.Framework.ComponentModel.Composition.IContainer>();
			container.Compose(previewHandlerHost1);

			MessageBox.Show("Loading");

			listBox1.DataSource = new System.IO.DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).GetFiles();

			this.FormClosing += Form1_FormClosing;
		}

		void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			var container = internalsession.GetService<Fwbs.Framework.ComponentModel.Composition.IContainer>();
			container.Release(this.previewHandlerHost1);
			this.previewHandlerHost1.Unload();
			this.internalsession = null;
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			System.IO.FileInfo file = (System.IO.FileInfo)listBox1.SelectedItem;

			try
			{
				previewHandlerHost1.Unload();
				((IPreviewer)previewHandlerHost1).PreviewFile(file);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void listBox1_DoubleClick(object sender, EventArgs e)
		{

		}

		private void sheToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.IO.FileInfo file = (System.IO.FileInfo)listBox1.SelectedItem;

			try
			{
				System.Diagnostics.Process.Start(file.FullName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void automateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.IO.FileInfo file = (System.IO.FileInfo)listBox1.SelectedItem;

			try
			{
				object missing = Type.Missing;
				object filename = file.FullName;

				previewHandlerHost1.Unload();

				Microsoft.Office.Interop.Word.Application word = null; 
				try
				{
					word = (Microsoft.Office.Interop.Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
				}
				catch(System.Runtime.InteropServices.COMException)
				{
					if (word == null)
						word = new Microsoft.Office.Interop.Word.ApplicationClass();
				}
				if (word.Documents.Count == 0)
					word.Documents.Add(ref missing, ref missing, ref missing, ref missing);

				word.Documents.Open(ref filename, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
				word.ShowMe();
				word.Visible = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				previewHandlerHost1.Load();
			}
		}
	}
}
