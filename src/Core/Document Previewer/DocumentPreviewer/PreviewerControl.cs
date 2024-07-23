using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Fwbs.Documents.Preview
{
    using Fwbs.Framework.ComponentModel.Composition;
    using Handlers;

    public partial class PreviewerControl : UserControl, IPreviewer
	{
		[Import]
		public IContainer Container
		{ get; set; }

		#region Constants
		private const int WM_DESTROY = 0x0002;
		#endregion

		#region Fields
		private string currentFile = "";
		
		private string resFail = "Unable To Preview";
		private string resProcessing = "Processing ... Please Wait";
		private string resNotSupported = "Preview Not Supported";

		private readonly Dictionary<Guid, IPreviewHandler> handlers = new Dictionary<Guid, IPreviewHandler>();
		private readonly Dictionary<Guid, Control> handlerhosts = new Dictionary<Guid, Control>();

		private bool supportsFullPreviewing = false;
		
		private IPreviewHandler currenthandler;
		private Control currenthandlerhost;
		private string forceUnloadExtensions = ".pdf";
		/// <summary>
		/// List of extensions including . which should have the preview handlers reloaded before viewing a second document
		/// </summary>
		protected string ForceUnloadExtensions
		{
			get
			{
				return forceUnloadExtensions;
			}
			set
			{
				forceUnloadExtensions = value;
			}
		}

		#endregion

		#region Constructors
		public PreviewerControl()
		{
			InitializeComponent();
		}
		#endregion
		
		#region Culturing

		private Dictionary<string, string> cultureProperties;
		public Dictionary<string, string> CultureProperties
		{
			get 
			{
				if (cultureProperties == null)
					cultureProperties = new Dictionary<string, string>();

				return cultureProperties; 
			}
			set { cultureProperties = value; }
		}

		private Dictionary<string, string> additionalProperties;
		public Dictionary<string, string> AdditionalProperties
		{
			get 
			{
				if (additionalProperties == null)
					additionalProperties = new Dictionary<string, string>();
				
				return additionalProperties; 
			}
			set { additionalProperties = value; }
		}

		#endregion

		#region Properties
		[Category("Appearance")]
		[Description("The full path to the file being be previewed.")]
		public string PreviewFileName
		{
			get
			{
				return currentFile;
			}
		}

		private int waitfor = 2000;
		[Description("The number of milliseconds to wait for unloading of a previewer.")]
		[DefaultValue(2000)]
		public int UnloadWaitFor
		{
			get
			{
				return waitfor;
			}
			set
			{
				waitfor = value;
			}
		}
		#endregion

		#region IPreviewer
		public object UIElement
		{
			get
			{
				return this;
			}
		}

		public void PreviewFile(FileInfo file)
		{
			if (DesignMode)
				return;

			if (file == null)
				throw new ArgumentNullException("file");

			file.Refresh();

			if (currentFile != null && file.FullName.ToUpperInvariant() == currentFile.ToUpperInvariant())
			{
				lblMsg.Visible = false;
				lblMsg.SendToBack();
				return;
			}

			if (!file.Exists)
				throw new FileNotFoundException("", file.FullName);
 
			Initialize(file.FullName);
		}

		public void ShowMessage(string message)
		{
			lblMsg.Visible = true;
			lblMsg.Text = message;
			lblMsg.BringToFront();
			Application.DoEvents();            
		}

		private const string UnloadingPreview = "Unloading Preview";
		public void Unload()
		{
			ShowMessage(UnloadingPreview);
			CleanUpHandler(ref currenthandler, true);
		}

		new public void Load()
		{
			if (String.IsNullOrEmpty(currentFile))
				return;

			Initialize(currentFile);
		}
		#endregion

		#region Methods
		protected void ValidateExtension(string fileExtension)
		{
			string file = null;
			var phf = PreviewHandlerFactory.GetPreviewHandlerId(fileExtension, Container);
			ValidateHandler(phf, ref file);           
		}

		private void Initialize(string filePath)
		{
			var originalfile = filePath;
			ApplyScreenCaptions();
			string extension = Path.GetExtension(filePath);
			IPreviewHandler handler = null;
			var info = PreviewHandlerFactory.GetPreviewHandlerId(extension, Container);
			var guid = info.Id;

			try
			{
				ValidateHandler(info, ref filePath);

				CleanUpHandler(ref currenthandler, false);
				currenthandler = null;
				currentFile = null;
				currenthandlerhost = null;

				Control handlerhost = null;
				bool init = false;
			   
				if (handlers.ContainsKey(guid))
				{
					handler = handlers[guid];
					handlerhost = handlerhosts[guid];
				}
				else
				{
					OnProcessing();

					handler = info.ResolvedHandler();
					if (!handlers.ContainsKey(guid)) // To fix the error An item with key already exists
						handlers.Add(guid, handler);
					init = true;

					if (handlerhosts.ContainsKey(guid))
						handlerhost = handlerhosts[guid];
					else
					{

						handlerhost = new Panel();
						handlerhost.Dock = DockStyle.Fill;
						this.Controls.Add(handlerhost);
						handlerhosts.Add(guid, handlerhost);
					}
				}
				
				var initializeFile = handler as IInitializeWithFile;
				var initializeStream = handler as IInitializeWithStream;
				var previewer = handler as IPreviewHandlerInfo;
				if (previewer != null)
				{
					previewer.SetPreviewSupport(supportsFullPreviewing);
					previewer.SetCultureData(CultureProperties);
					previewer.SetAdditionalProperties(AdditionalProperties);
				}
				if (initializeStream != null)
				{
					FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					initializeStream.Initialize(new COMStream(fs), 0);	// When does the COMStream object gets disposed via Dispose()?
				}
				else if (initializeFile != null)
				{
					initializeFile.Initialize(filePath, 0);
				}
				else
					throw new NotSupportedException();

				if (init || guid == new Guid("00020827-0000-0000-c000-000000000046")) // Excel
					SetPreviewBounds(handler, handlerhost);
				if (handler != currenthandler)
				{
					OnProcessing();
				}
				
				handler.DoPreview();
	  
				if (handler != currenthandler)
				{
					if (currenthandlerhost != null)
						currenthandlerhost.Visible = false;

					handlerhost.Visible = true;
					handlerhost.BringToFront();
				}

				currenthandler = handler;
				currenthandlerhost = handlerhost;

				lblMsg.Visible = false;

				currentFile = originalfile;
			}
			catch (NotSupportedException)
			{
				OnNotSupported();
				throw;
			}
			catch (InvalidOperationException ex)
			{
				OnError(ex);
			}
			catch (InvalidComObjectException icex)
			{
				OnError(icex);
				if (handler != null)
					CleanUpHandler(ref handler, false);
			}
			catch (COMException comex)
			{
				OnError(comex);
				if (handler != null)
					CleanUpHandler(ref handler, false);
			}
		}

		private static void ValidateHandler(PreviewHandlerInfo info, ref string filePath)
		{
			if (info.Supported.HasValue && info.Supported.Value == false)
				throw new NotSupportedException(String.Format("Preview Handler for '{0}' is not currently supported.", info.Extension));

			if (!String.IsNullOrWhiteSpace(filePath))
			{
				if (info.CopyFile.GetValueOrDefault() && File.Exists(filePath))
				{
					filePath = MakeTemporaryCopy(filePath);
				}
			}
		}

		private static string CreateTempPath(string file)
		{
			var parent = Directory.GetParent(file);
			var path = Path.Combine(parent.FullName, "Previews");
			var dir = new DirectoryInfo(path);

			if (!dir.Exists)
				dir.Create();

			return Path.Combine(dir.FullName, Path.GetFileNameWithoutExtension(file) + "-preview" + Path.GetExtension(file));
		}

		private static string MakeTemporaryCopy(string file)
		{
			var info = new FileInfo(file);
			string tempPath = CreateTempPath(info.FullName);

            if (!File.Exists(tempPath) || File.GetLastWriteTimeUtc(tempPath) != info.LastWriteTimeUtc)
                info.CopyTo(tempPath, true);

			return tempPath;
		}

		private void SetPreviewBounds(IPreviewHandler handler, Control handlerhost)
		{
			if (handler == null || handlerhost == null)
				return;
		
			var rect = new RECT(handlerhost.ClientRectangle);
			handler.SetWindow(handlerhost.Handle, ref rect);
			handler.SetRect(ref rect);
		  
			var visuals = handler as IPreviewHandlerVisuals;
	  
			if (visuals != null)
			{
				visuals.SetBackgroundColor(new COLORREF(BackColor));
				
				var logFont = new LOGFONT();
				Font.ToLogFont(logFont);
				visuals.SetFont(ref logFont);
				visuals.SetTextColor(new COLORREF(ForeColor));
			}

		}

		private void CleanUpAll()
		{
			// clear up the handlers
			if (this.handlers.Count > 0)
			{
				Guid[] ids = new Guid[handlers.Count];
				handlers.Keys.CopyTo(ids, 0);

				for (int ctr = ids.Length - 1; ctr >= 0; ctr--)
				{
					IPreviewHandler ph = handlers[ids[ctr]];
					this.CleanUpHandler(ref ph, false);
				}

				handlers.Clear();
			}

			// clear up the WinForm panels that is being stored
			if (this.handlerhosts.Count > 0)
			{
				foreach (KeyValuePair<Guid, Control> kvp in this.handlerhosts)
				{
					kvp.Value.Dispose();
				}
				this.handlerhosts.Clear();
			}
		}

		private void CleanUpHandler(ref IPreviewHandler handler, bool wait)
		{
			if (handler != null)
			{
				try
				{
					handler.Unload();
				}
				catch (InvalidComObjectException)
				{
				}
				catch (COMException)
				{
				}
				catch (AccessViolationException)
				{
				}

				// remove the handler from the collection and call its Dispose() if it exists
				foreach (KeyValuePair<Guid, IPreviewHandler> kvp in this.handlers)
				{
					if (kvp.Value == handler)
					{
						// remove from collection
						this.handlers.Remove(kvp.Key);
						// has it got an IDisposable?
						IDisposable idisp = kvp.Value as IDisposable;
						if (idisp != null)
						{
							// yes, get rid of it
							idisp.Dispose();
						}
						break;
					}
				}

				if (Marshal.IsComObject(handler))
					Marshal.FinalReleaseComObject(handler);

				if (wait)
					WaitForRelease();

				handler = null;
			}
		}

		private void ApplyScreenCaptions()
		{
			if (CultureProperties.ContainsKey("NoPreview") && !string.IsNullOrEmpty(CultureProperties["NoPreview"]))
				resFail = CultureProperties["NoPreview"];

			if (CultureProperties.ContainsKey("ProcPlzWait") && !string.IsNullOrEmpty(CultureProperties["ProcPlzWait"]))
				resProcessing = CultureProperties["ProcPlzWait"];

			if (CultureProperties.ContainsKey("NotSupported") && !string.IsNullOrEmpty(CultureProperties["NotSupported"]))
				resNotSupported = CultureProperties["NotSupported"];

		}

		private void OnProcessing()
		{
			ShowMessage(resProcessing);
		}

		private void OnNotSupported()
		{
			ShowMessage(resNotSupported);
		}

		private void OnError(Exception ex)
		{
			ShowMessage(resFail + Environment.NewLine + Environment.NewLine + ex.Message);
		}

		private void WaitForRelease()
		{
			string ext = Path.GetFileName(Utils.FindExecutable(currentFile)).ToUpperInvariant();
			switch (ext)
			{
				case "WINWORD.EXE":
					{
						WaitForProcess("WINWORD");
					}
					break;
				default:
					break;
			}
		}

		private static System.Diagnostics.Process[] GetProcessesByName(string name)
		{
			List<System.Diagnostics.Process> matchedProcesses = new List<System.Diagnostics.Process>();
			int sessionid = System.Diagnostics.Process.GetCurrentProcess().SessionId;
			var procs = System.Diagnostics.Process.GetProcessesByName(name);
			if (procs != null && procs.Length > 0)
			{
				for (int i = 0; i < procs.Length; i++)
				{
					if (procs[i].SessionId == sessionid)
					{
						matchedProcesses.Add(procs[i]);
					}
					else
					{
						// process not needed, get rid of it
						procs[i].Dispose();
					}
				}
			}
			return matchedProcesses.ToArray();
		}

		private void WaitForProcess(string process)
		{
			var procs = GetProcessesByName(process);

			bool returnNow = false;
			bool hidden = false;
			int ctr = 0;
			int max = (int)((double)waitfor / 500.0);
			while (ctr < max)
			{
				if (procs.Length == 0)
					return;

				foreach (var p in procs)
				{
					p.Refresh();
					IntPtr handle = IntPtr.Zero;
					try
					{
						handle = p.MainWindowHandle;
						if (handle == IntPtr.Zero)
						{
							hidden = true;
							if (p.HasExited)
							{
								returnNow = true;
								break;
							}
						}
					}
					catch (InvalidOperationException)
					{
						continue;
					}
				}

				// get rid of Process objects
				for (int i = 0; i < procs.Length; i++ )
				{
					procs[i].Dispose();
				}
				if (returnNow)
					return;

				if (hidden == false)
					return;

				System.Threading.Thread.Sleep(500);

				procs = GetProcessesByName(process);

				ctr++;
			}

			// get rid of Process objects
			for (int i = 0; i < procs.Length; i++)
			{
				procs[i].Dispose();
			}
		}
		#endregion

		#region Captured Events
		private void PreviewHandlerHost_Resize(object sender, EventArgs e)
		{
			if (currenthandler != null)
			{
				var rect = new RECT(ClientRectangle);
				try
				{
					currenthandler.SetRect(ref rect);
				}
				catch (InvalidComObjectException)
				{
					CleanUpHandler(ref currenthandler, false);
				}
				catch (COMException)
				{
					CleanUpHandler(ref currenthandler, false);
				}
				catch (AccessViolationException)
				{
					CleanUpHandler(ref currenthandler, false);
				}
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_DESTROY)
			{
				CleanUpAll();
			}
			base.WndProc(ref m);
		}
		#endregion
	}
}
