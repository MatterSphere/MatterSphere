using System;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows.Admin
{



    /// <summary>
    /// This form captures an enquiry form screen image and the returns the byte array as property
    /// so that it can be saved into the database.
    /// </summary>
    public class frmPreview : FWBS.OMS.UI.Windows.BaseForm
	{
		#region API

		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		private static extern bool BitBlt(
			IntPtr hdcDest, // handle to destination DC
			int nXDest,  // x-coord of destination upper-left corner
			int nYDest,  // y-coord of destination upper-left corner
			int nWidth,  // width of destination rectangle
			int nHeight, // height of destination rectangle
			IntPtr hdcSrc,  // handle to source DC
			int nXSrc,   // x-coordinate of source upper-left corner
			int nYSrc,   // y-coordinate of source upper-left corner
			System.Int32 dwRop  // raster operation code
			);

		#endregion

		#region Control Fields

		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
		public FWBS.OMS.UI.Windows.EnquiryForm enquiryForm1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnuMode;
		private System.Windows.Forms.MenuItem mnuAction;
		private System.Windows.Forms.MenuItem mnuGet;
		private System.Windows.Forms.MenuItem mnuAdd;
		private System.Windows.Forms.MenuItem mnuEdit;
		private System.Windows.Forms.MenuItem mnuSearch;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.Timer timFlash;
		private System.Windows.Forms.MenuItem mnuSnapshot;

		#endregion

		#region Fields

		/// <summary>
		/// The enquiry form code to be used for a screen capture.
		/// </summary>
		private string _code;
		/// <summary>
		/// Any test parameters that the enquiry form may need to load.
		/// </summary>
		private FWBS.Common.KeyValueCollection _param;
		private System.Windows.Forms.MenuItem mnuWizard;

        /// <summary>
        /// The image byte array to return.
        /// </summary>
        private byte[] _data = null;

		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Constructs the preview form specifying the enquiry to load.
		/// </summary>
		/// <param name="code">The enquiry for code.</param>
		/// <param name="size">The size of the screen capture.</param>
		/// <param name="param">The parameters that the enwuiry form may need to run.</param>
		public frmPreview(string code, Size size, FWBS.Common.KeyValueCollection param)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_code = code;
			_param = param;
			this.ClientSize = size;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPreview));
            this.resourceLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.enquiryForm1 = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuMode = new System.Windows.Forms.MenuItem();
            this.mnuAdd = new System.Windows.Forms.MenuItem();
            this.mnuEdit = new System.Windows.Forms.MenuItem();
            this.mnuSearch = new System.Windows.Forms.MenuItem();
            this.mnuAction = new System.Windows.Forms.MenuItem();
            this.mnuGet = new System.Windows.Forms.MenuItem();
            this.mnuWizard = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.mnuSnapshot = new System.Windows.Forms.MenuItem();
            this.timFlash = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // enquiryForm1
            // 
            this.enquiryForm1.AutoScroll = true;
            this.enquiryForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enquiryForm1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.enquiryForm1.IsDirty = false;
            this.enquiryForm1.Location = new System.Drawing.Point(0, 0);
            this.enquiryForm1.Name = "enquiryForm1";
            this.enquiryForm1.Size = new System.Drawing.Size(684, 490);
            this.enquiryForm1.TabIndex = 1;
            this.enquiryForm1.ToBeRefreshed = false;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuMode,
            this.mnuAction});
            // 
            // mnuMode
            // 
            this.mnuMode.Index = 0;
            this.resourceLookup.SetLookup(this.mnuMode, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuMode", "&Mode", ""));
            this.mnuMode.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuAdd,
            this.mnuEdit,
            this.mnuSearch});
            this.mnuMode.Text = "&Mode";
            // 
            // mnuAdd
            // 
            this.mnuAdd.Checked = true;
            this.mnuAdd.Index = 0;
            this.resourceLookup.SetLookup(this.mnuAdd, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuAdd", "&Add", ""));
            this.mnuAdd.RadioCheck = true;
            this.mnuAdd.Text = "&Add";
            this.mnuAdd.Click += new System.EventHandler(this.mnuModes_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.Index = 1;
            this.resourceLookup.SetLookup(this.mnuEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuEdit", "&Edit", ""));
            this.mnuEdit.RadioCheck = true;
            this.mnuEdit.Text = "&Edit";
            this.mnuEdit.Click += new System.EventHandler(this.mnuModes_Click);
            // 
            // mnuSearch
            // 
            this.mnuSearch.Index = 2;
            this.resourceLookup.SetLookup(this.mnuSearch, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuSearch", "&Search", ""));
            this.mnuSearch.RadioCheck = true;
            this.mnuSearch.Text = "&Search";
            this.mnuSearch.Click += new System.EventHandler(this.mnuModes_Click);
            // 
            // mnuAction
            // 
            this.mnuAction.Index = 1;
            this.resourceLookup.SetLookup(this.mnuAction, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuAction", "&Action", ""));
            this.mnuAction.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuGet,
            this.mnuWizard,
            this.menuItem4,
            this.mnuSnapshot});
            this.mnuAction.Text = "&Action";
            // 
            // mnuGet
            // 
            this.mnuGet.Index = 0;
            this.resourceLookup.SetLookup(this.mnuGet, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuGet", "&Get", ""));
            this.mnuGet.Shortcut = System.Windows.Forms.Shortcut.F5;
            this.mnuGet.Text = "&Get";
            this.mnuGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // mnuWizard
            // 
            this.mnuWizard.Index = 1;
            this.resourceLookup.SetLookup(this.mnuWizard, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuWizard", "&Wizard", ""));
            this.mnuWizard.Shortcut = System.Windows.Forms.Shortcut.F6;
            this.mnuWizard.Text = "&Wizard";
            this.mnuWizard.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // mnuSnapshot
            // 
            this.mnuSnapshot.Index = 3;
            this.resourceLookup.SetLookup(this.mnuSnapshot, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuSnapshot", "&Snapshot", ""));
            this.mnuSnapshot.Text = "&Snapshot";
            this.mnuSnapshot.Click += new System.EventHandler(this.mnuSnapshot_Click);
            // 
            // timFlash
            // 
            this.timFlash.Interval = 1000;
            this.timFlash.Tick += new System.EventHandler(this.timFlash_Tick);
            // 
            // frmPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(684, 490);
            this.Controls.Add(this.enquiryForm1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.resourceLookup.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("PREVIEW", "Preview", ""));
            this.Menu = this.mainMenu1;
            this.Name = "frmPreview";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Preview";
            this.Load += new System.EventHandler(this.frmPreview_Load);
            this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region Event Methods

		private void btnGet_Click(object sender, System.EventArgs e)
		{
            try
			{
				EnquiryMode n;
				if (mnuAdd.Checked) n = EnquiryMode.Add; else if (mnuEdit.Checked) n = EnquiryMode.Edit; else n = EnquiryMode.Search;
                enquiryForm1.Enquiry = Enquiry.GetEnquiry(_code,null,n,false,_param);
            }
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}


		private void mnuCapture_Click(object sender, System.EventArgs e)
		{
			_data = WriteImage(CaptureScreen());
		}

		private void mnuModes_Click(object sender, System.EventArgs e)
		{
			mnuAdd.Checked=false;
			mnuEdit.Checked=false;
			mnuSearch.Checked=false;
			((MenuItem)sender).Checked=true;
		}

		private void timFlash_Tick(object sender, System.EventArgs e)
		{
			timFlash.Enabled=false;
			mnuCapture_Click(sender,e);
		}

		private void mnuSnapshot_Click(object sender, System.EventArgs e)
		{
			timFlash.Enabled=true;
		}	

		#endregion

		#region Methods

		private Image CaptureScreen()
		{
			Graphics g1 = this.CreateGraphics();
			Image MyImage = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height, g1);
			Graphics g2 = Graphics.FromImage(MyImage);
			IntPtr dc1 = g1.GetHdc();
			IntPtr dc2 = g2.GetHdc();
			BitBlt(dc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 0, 13369376);
			g1.ReleaseHdc(dc1);
			g2.ReleaseHdc(dc2);
            g1.Dispose();
            g2.Dispose();
			return MyImage;
		}

		internal static Image ReadImage(byte [] data)
		{
			if (data == null) return null;
			System.IO.MemoryStream mem = new System.IO.MemoryStream(data,0,data.Length);
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter fmt = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			object obj = fmt.Deserialize(mem);
			mem.Close();
			fmt = null;
			mem = null;
			if (obj is Image)
				return (Image)obj;
			else
				return null;
		}

		internal static byte[] WriteImage(Image img)
		{
			System.IO.MemoryStream mem = new System.IO.MemoryStream();
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter fmt = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			fmt.Serialize(mem, img);
			byte [] data = mem.ToArray();
			mem.Close();
			fmt = null;
			mem = null;
			return data;
		}

		#endregion

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			try
			{
				EnquiryMode n;
				if (mnuAdd.Checked) n = EnquiryMode.Add; else if (mnuEdit.Checked) n = EnquiryMode.Edit; else n = EnquiryMode.Search;
				FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(_code,null,n,_param);
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		#region Properties

		/// <summary>
		/// Gets the captured image byte array.
		/// </summary>
		public byte [] ImageStream
		{
			get
			{
				return _data;
			}
		}

		#endregion

        private void frmPreview_Load(object sender, EventArgs e)
        {
            this.Text = Session.CurrentSession.Resources.GetResource("Preview", "Preview","").Text;
            //The menu's need doing
        }
	}
}
