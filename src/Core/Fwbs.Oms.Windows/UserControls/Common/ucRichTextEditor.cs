using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// The Output Text Mode for the ucRectTextEditor2
    /// </summary>
    public enum TextMode {Text,RTF}

	/// <summary>
	/// Summary description for ucRichTextEditor2.
	/// </summary>
	public class ucRichTextEditor : System.Windows.Forms.UserControl, IBasicEnquiryControl2
	{
		#region Events

		/// <summary>
		/// The changed event is used to determine when a major change has happended within the
		/// user control.  This will tend to be used when the internal editing control has changed
		/// in some way or another.
		/// </summary>
		[Category("Action")]
		public event EventHandler Changed;

		[Category("Action")]
		public event EventHandler ActiveChanged;

		[Category("Design")]
		public event EventHandler TextModeChanged;

		#endregion
		
		#region Fields
		private System.Windows.Forms.MenuItem rcxmnuCopy;
		private System.Windows.Forms.MenuItem rcxmnuAbout;
		private System.Windows.Forms.MenuItem rcxmnuAlignment;
		private System.Windows.Forms.MenuItem rcxmnuLeft;
		private System.Windows.Forms.MenuItem rcxmnuCentre;
		private System.Windows.Forms.MenuItem rcxmnuRight;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.FontDialog fontDialog1;
		private System.Windows.Forms.MenuItem rcxmnuStyle;
		private System.Windows.Forms.MenuItem rcxmnuBold;
		private System.Windows.Forms.MenuItem rcxmnuItalic;
		private System.Windows.Forms.MenuItem rcxmnuUnderline;
		private System.Windows.Forms.MenuItem rcxmnuFormat;
		private System.Windows.Forms.MenuItem rcxmnuFont;
		private System.Windows.Forms.MenuItem rcxmnuSp3;
		private System.Windows.Forms.MenuItem rcxmnuEdit;
		private System.Windows.Forms.MenuItem rcxmnuCut;
		private System.Windows.Forms.MenuItem rcxmnuPaste;
		private System.Windows.Forms.MenuItem rcxmnuFile;
		private System.Windows.Forms.MenuItem rcxmnuNew;
		private System.Windows.Forms.MenuItem rcxmnuOpen;
		private System.Windows.Forms.MenuItem rcxmnuSaveAs;
		private System.Windows.Forms.MenuItem rcxmnuSave;
		private System.Windows.Forms.MenuItem rcxmnuSp1;
		private System.Windows.Forms.MenuItem rcxmnuClose;
		private System.Windows.Forms.ToolStripButton tbLeft;
		private System.Windows.Forms.ToolStripButton tbRight;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem rcxmnuHelp;
		private System.Windows.Forms.ToolStrip tbControls;
		private System.Windows.Forms.ToolStripComboBox cmbSize;
		private System.Windows.Forms.ToolStripComboBox cmbFont;
		private System.Windows.Forms.ToolStripButton tbBold;
		private System.Windows.Forms.ToolStripButton tbItalic;
		private System.Windows.Forms.ToolStripButton tbUnderline;
		private System.Windows.Forms.ToolStripSeparator tbSp1;
		private System.Windows.Forms.ToolStripButton tbCentre;
		private FWBS.OMS.UI.RichTextBox richTextBox1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		

		private string title = "RichText Editor - [%1%]";
		private System.IO.FileInfo currentfile = new System.IO.FileInfo("doc1");
		private bool Dirty = false;
		private bool _omsdesignmode = false;
		private bool? _parenthasmenu = null;
		private TextMode _textmode = TextMode.Text;

		/// <summary>
		/// Stores how wide the caption should be.
		/// </summary>
		private int _captionWidth = 0;
        private IContainer components;

        /// <summary>
        /// Holds the boolean value for the read only attribute.
        /// </summary>
        private bool _required;
		#endregion

		#region Constructors
		public ucRichTextEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.SetStyle(ControlStyles.ResizeRedraw|ControlStyles.AllPaintingInWmPaint,true);
			this.tbControls.ImageList = Images.GetRichTextIcons((Images.IconSize)LogicalToDeviceUnits(16));
		}

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            this.tbControls.ImageList = Images.GetRichTextIcons((Images.IconSize)LogicalToDeviceUnits(16));
            base.OnDpiChangedAfterParent(e);
        }

		#endregion

		#region IBasicEnquiryControl2 Implementation

		[Browsable(true)]
		public object Control
		{
			get
			{
				return richTextBox1;
			}
		}

		[Browsable(false)]
		public bool omsDesignMode
		{
			get
			{
				return _omsdesignmode;
			}
			set
			{
				_omsdesignmode = value;
				if (value)
				{
					this.Controls.Remove(richTextBox1);
					this.Paint += new System.Windows.Forms.PaintEventHandler(this.omsDesignMode_Paint);
					this.Resize += new EventHandler(this.omsDesignMode_Resize);

				}
			}
		}

		private void omsDesignMode_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int y = this.ShowToolBar ? tbControls.Height : 0;
			Rectangle rect = new Rectangle(0,y,this.ClientSize.Width,this.ClientSize.Height-y);
			System.Windows.Forms.ControlPaint.DrawBorder3D(e.Graphics,rect,Border3DStyle.Sunken);
			rect = new Rectangle(rect.X+2,rect.Y+2,rect.Width-4,rect.Height-4);
			e.Graphics.FillRectangle(SystemBrushes.Window,rect);
		}

		private void omsDesignMode_Resize(object sender, EventArgs e)
		{
			this.Invalidate();
		}


		/// <summary>
		/// Gets whether the current control can be stretched by its Y co-ordinate.
		/// This is a design mode property and is set to true.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(true)]
		public bool LockHeight 
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets or Sets the value of the control.  In this case it should be True of False.
		/// </summary>
		[DefaultValue(0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Value
		{
			get
			{
				if (_textmode == TextMode.RTF)
					return richTextBox1.Rtf;
				else
					return richTextBox1.Text;
			}
			set
			{
				if (value != null && value != System.DBNull.Value)
				{
					try
					{
						richTextBox1.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(Convert.ToString(value));
						Global.SetRichTextBoxRightToLeft(richTextBox1);
						this.TextMode = TextMode.RTF;
					}
					catch
					{
						richTextBox1.Text = Convert.ToString(value);
						this.TextMode = TextMode.Text;
					}
				}
			}
		}

		/// <summary>
		/// Gets or Sets the flag that tells the rendering form that this control is required
		/// to be filled incase the underlying value is DBNull.
		/// </summary>
		[DefaultValue(false)]
		[Category("Behavior")]
		public bool Required 
		{
			get
			{
				return _required;
			}
			set
			{
				_required = value;
			}
		}

		/// <summary>
		/// Gets or Sets the editable format of the control.
		/// </summary>
		[Category("RichText")]
		public new Color BackColor 
		{
			get
			{
				return richTextBox1.BackColor;
			}
			set
			{
				richTextBox1.BackColor = value;
			}
		}
		
		public string RichText
		{
			get
			{
				return richTextBox1.Rtf;
			}
		}

		public string RawText
		{
			get
			{
				return richTextBox1.Text;
			}
		}

		/// <summary>
		/// Gets or Sets the editable format of the control.
		/// </summary>
		[DefaultValue(false)]
		[Category("RichText")]
		public bool ReadOnly 
		{
			get
			{
				return richTextBox1.ReadOnly;
			}
			set
			{
				richTextBox1.ReadOnly = value;
			}
		}

		[DefaultValue(BorderStyle.Fixed3D)]
		[Category("RichText")]
		public BorderStyle Borderstyle
		{
			get
			{
				return richTextBox1.BorderStyle;
			}
			set
			{
				richTextBox1.BorderStyle = value;
			}
		}


		/// <summary>
		/// Gets or Sets the caption width of a control, leaving the rest of the width of the control
		/// to be the width of the internal editing control.
		/// </summary>
		[DefaultValue(150)]
        [Browsable(false)]
		public virtual int CaptionWidth
		{
			get
			{
				return _captionWidth;
			}
			set
			{
				_captionWidth = value;
			}
		}

        [Browsable(false)]
        public bool CaptionTop
        {
            get
            {
                return false;
            }
            set { }
        }

		/// <summary>
		/// Executes the changed event.
		/// </summary>
		public void OnChanged()
		{
			if (Changed!= null)
				Changed(this, EventArgs.Empty);
		}

		public void OnActiveChanged()
		{
			if (ActiveChanged!= null)
				ActiveChanged(this, EventArgs.Empty);
		}
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.rcxmnuCopy = new System.Windows.Forms.MenuItem();
            this.rcxmnuAbout = new System.Windows.Forms.MenuItem();
            this.rcxmnuAlignment = new System.Windows.Forms.MenuItem();
            this.rcxmnuLeft = new System.Windows.Forms.MenuItem();
            this.rcxmnuCentre = new System.Windows.Forms.MenuItem();
            this.rcxmnuRight = new System.Windows.Forms.MenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.rcxmnuStyle = new System.Windows.Forms.MenuItem();
            this.rcxmnuBold = new System.Windows.Forms.MenuItem();
            this.rcxmnuItalic = new System.Windows.Forms.MenuItem();
            this.rcxmnuUnderline = new System.Windows.Forms.MenuItem();
            this.rcxmnuFormat = new System.Windows.Forms.MenuItem();
            this.rcxmnuFont = new System.Windows.Forms.MenuItem();
            this.rcxmnuSp3 = new System.Windows.Forms.MenuItem();
            this.rcxmnuEdit = new System.Windows.Forms.MenuItem();
            this.rcxmnuCut = new System.Windows.Forms.MenuItem();
            this.rcxmnuPaste = new System.Windows.Forms.MenuItem();
            this.rcxmnuFile = new System.Windows.Forms.MenuItem();
            this.rcxmnuNew = new System.Windows.Forms.MenuItem();
            this.rcxmnuOpen = new System.Windows.Forms.MenuItem();
            this.rcxmnuSaveAs = new System.Windows.Forms.MenuItem();
            this.rcxmnuSave = new System.Windows.Forms.MenuItem();
            this.rcxmnuSp1 = new System.Windows.Forms.MenuItem();
            this.rcxmnuClose = new System.Windows.Forms.MenuItem();
            this.tbLeft = new System.Windows.Forms.ToolStripButton();
            this.tbRight = new System.Windows.Forms.ToolStripButton();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.rcxmnuHelp = new System.Windows.Forms.MenuItem();
            this.tbControls = new System.Windows.Forms.ToolStrip();
            this.cmbFont = new System.Windows.Forms.ToolStripComboBox();
            this.cmbSize = new System.Windows.Forms.ToolStripComboBox();
            this.tbBold = new System.Windows.Forms.ToolStripButton();
            this.tbItalic = new System.Windows.Forms.ToolStripButton();
            this.tbUnderline = new System.Windows.Forms.ToolStripButton();
            this.tbSp1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbCentre = new System.Windows.Forms.ToolStripButton();
            this.richTextBox1 = new FWBS.OMS.UI.RichTextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tbControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // rcxmnuCopy
            // 
            this.rcxmnuCopy.Index = 1;
            this.rcxmnuCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.rcxmnuCopy.Text = "Copy";
            this.rcxmnuCopy.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // rcxmnuAbout
            // 
            this.rcxmnuAbout.Index = 0;
            this.rcxmnuAbout.Text = "About";
            // 
            // rcxmnuAlignment
            // 
            this.rcxmnuAlignment.Index = 2;
            this.rcxmnuAlignment.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.rcxmnuLeft,
            this.rcxmnuCentre,
            this.rcxmnuRight});
            this.rcxmnuAlignment.Text = "Alignment";
            // 
            // rcxmnuLeft
            // 
            this.rcxmnuLeft.Index = 0;
            this.rcxmnuLeft.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
            this.rcxmnuLeft.Text = "Left";
            this.rcxmnuLeft.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // rcxmnuCentre
            // 
            this.rcxmnuCentre.Index = 1;
            this.rcxmnuCentre.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.rcxmnuCentre.Text = "Centre";
            this.rcxmnuCentre.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // rcxmnuRight
            // 
            this.rcxmnuRight.Index = 2;
            this.rcxmnuRight.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            this.rcxmnuRight.Text = "Right";
            this.rcxmnuRight.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "rtf";
            this.openFileDialog1.Filter = "RichText files (*.rtf)|*.rtf|Text files (*.txt)|*.txt|All files (*.*)|*.*\"";
            this.openFileDialog1.Title = "Open ...";
            // 
            // rcxmnuStyle
            // 
            this.rcxmnuStyle.Index = 3;
            this.rcxmnuStyle.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.rcxmnuBold,
            this.rcxmnuItalic,
            this.rcxmnuUnderline});
            this.rcxmnuStyle.Text = "Sytle";
            // 
            // rcxmnuBold
            // 
            this.rcxmnuBold.Index = 0;
            this.rcxmnuBold.Shortcut = System.Windows.Forms.Shortcut.CtrlB;
            this.rcxmnuBold.Text = "Bold";
            this.rcxmnuBold.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // rcxmnuItalic
            // 
            this.rcxmnuItalic.Index = 1;
            this.rcxmnuItalic.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
            this.rcxmnuItalic.Text = "Italic";
            this.rcxmnuItalic.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // rcxmnuUnderline
            // 
            this.rcxmnuUnderline.Index = 2;
            this.rcxmnuUnderline.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
            this.rcxmnuUnderline.Text = "Underline";
            this.rcxmnuUnderline.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // rcxmnuFormat
            // 
            this.rcxmnuFormat.Index = 2;
            this.rcxmnuFormat.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.rcxmnuFont,
            this.rcxmnuSp3,
            this.rcxmnuAlignment,
            this.rcxmnuStyle});
            this.rcxmnuFormat.Text = "Format";
            this.rcxmnuFormat.Visible = false;
            // 
            // rcxmnuFont
            // 
            this.rcxmnuFont.Index = 0;
            this.rcxmnuFont.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
            this.rcxmnuFont.Text = "Font";
            this.rcxmnuFont.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // rcxmnuSp3
            // 
            this.rcxmnuSp3.Index = 1;
            this.rcxmnuSp3.Text = "-";
            // 
            // rcxmnuEdit
            // 
            this.rcxmnuEdit.Index = 1;
            this.rcxmnuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.rcxmnuCut,
            this.rcxmnuCopy,
            this.rcxmnuPaste});
            this.rcxmnuEdit.Text = "Edit";
            this.rcxmnuEdit.Visible = false;
            // 
            // rcxmnuCut
            // 
            this.rcxmnuCut.Index = 0;
            this.rcxmnuCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.rcxmnuCut.Text = "Cut";
            this.rcxmnuCut.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // rcxmnuPaste
            // 
            this.rcxmnuPaste.Index = 2;
            this.rcxmnuPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.rcxmnuPaste.Text = "Paste";
            this.rcxmnuPaste.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // rcxmnuFile
            // 
            this.rcxmnuFile.Index = 0;
            this.rcxmnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.rcxmnuNew,
            this.rcxmnuOpen,
            this.rcxmnuSaveAs,
            this.rcxmnuSave,
            this.rcxmnuSp1,
            this.rcxmnuClose});
            this.rcxmnuFile.Text = "&File";
            this.rcxmnuFile.Visible = false;
            // 
            // rcxmnuNew
            // 
            this.rcxmnuNew.Index = 0;
            this.rcxmnuNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.rcxmnuNew.Text = "&New";
            this.rcxmnuNew.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // rcxmnuOpen
            // 
            this.rcxmnuOpen.Index = 1;
            this.rcxmnuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.rcxmnuOpen.Text = "&Open";
            this.rcxmnuOpen.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // rcxmnuSaveAs
            // 
            this.rcxmnuSaveAs.Index = 2;
            this.rcxmnuSaveAs.Text = "Save &As";
            this.rcxmnuSaveAs.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // rcxmnuSave
            // 
            this.rcxmnuSave.Index = 3;
            this.rcxmnuSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.rcxmnuSave.Text = "&Save";
            this.rcxmnuSave.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // rcxmnuSp1
            // 
            this.rcxmnuSp1.Index = 4;
            this.rcxmnuSp1.Text = "-";
            // 
            // rcxmnuClose
            // 
            this.rcxmnuClose.Index = 5;
            this.rcxmnuClose.Text = "&Close";
            this.rcxmnuClose.Visible = false;
            this.rcxmnuClose.Click += new System.EventHandler(this.rcxmnuMenuButtons_Click);
            // 
            // tbLeft
            // 
            this.tbLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbLeft.ImageIndex = 4;
            this.tbLeft.Name = "tbLeft";
            this.tbLeft.Size = new System.Drawing.Size(23, 22);
            // 
            // tbRight
            // 
            this.tbRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbRight.ImageIndex = 6;
            this.tbRight.Name = "tbRight";
            this.tbRight.Size = new System.Drawing.Size(23, 22);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.rcxmnuFile,
            this.rcxmnuEdit,
            this.rcxmnuFormat,
            this.rcxmnuHelp});
            // 
            // rcxmnuHelp
            // 
            this.rcxmnuHelp.Index = 3;
            this.rcxmnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.rcxmnuAbout});
            this.rcxmnuHelp.Text = "Help";
            this.rcxmnuHelp.Visible = false;
            // 
            // tbControls
            // 
            this.tbControls.CanOverflow = false;
            this.tbControls.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tbControls.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmbFont,
            this.cmbSize,
            this.tbBold,
            this.tbItalic,
            this.tbUnderline,
            this.tbSp1,
            this.tbLeft,
            this.tbCentre,
            this.tbRight});
            this.tbControls.Location = new System.Drawing.Point(0, 0);
            this.tbControls.Name = "tbControls";
            this.tbControls.Size = new System.Drawing.Size(447, 25);
            this.tbControls.TabIndex = 1;
            this.tbControls.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tbControls_ButtonClick);
            // 
            // cmbFont
            // 
            this.cmbFont.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbFont.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.cmbFont.Name = "cmbFont";
            this.cmbFont.Size = new System.Drawing.Size(170, 25);
            this.cmbFont.SelectedIndexChanged += new System.EventHandler(this.cmbFont_SelectedIndexChanged);
            this.cmbFont.Enter += new System.EventHandler(this.cmbFont_Enter);
            // 
            // cmbSize
            // 
            this.cmbSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbSize.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24",
            "26",
            "28",
            "36",
            "48",
            "72"});
            this.cmbSize.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.cmbSize.Name = "cmbSize";
            this.cmbSize.Size = new System.Drawing.Size(80, 25);
            this.cmbSize.SelectedIndexChanged += new System.EventHandler(this.cmbSize_SelectedIndexChanged);
            // 
            // tbBold
            // 
            this.tbBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbBold.ImageIndex = 0;
            this.tbBold.Name = "tbBold";
            this.tbBold.Size = new System.Drawing.Size(23, 22);
            // 
            // tbItalic
            // 
            this.tbItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbItalic.ImageIndex = 1;
            this.tbItalic.Name = "tbItalic";
            this.tbItalic.Size = new System.Drawing.Size(23, 22);
            // 
            // tbUnderline
            // 
            this.tbUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbUnderline.ImageIndex = 2;
            this.tbUnderline.Name = "tbUnderline";
            this.tbUnderline.Size = new System.Drawing.Size(23, 22);
            // 
            // tbSp1
            // 
            this.tbSp1.Name = "tbSp1";
            this.tbSp1.Size = new System.Drawing.Size(6, 25);
            // 
            // tbCentre
            // 
            this.tbCentre.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbCentre.ImageIndex = 5;
            this.tbCentre.Name = "tbCentre";
            this.tbCentre.Size = new System.Drawing.Size(23, 22);
            // 
            // richTextBox1
            // 
            this.richTextBox1.AcceptsTab = true;
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.HideSelection = false;
            this.richTextBox1.Location = new System.Drawing.Point(0, 25);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox1.ShowSelectionMargin = true;
            this.richTextBox1.Size = new System.Drawing.Size(447, 306);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            this.richTextBox1.SelectionChanged += new System.EventHandler(this.richTextBox1_SelectionChanged);
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            this.richTextBox1.VisibleChanged += new System.EventHandler(this.richTextBox1_SelectionChanged);
            this.richTextBox1.Enter += new System.EventHandler(this.richTextBox1_SelectionChanged);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "rtf";
            this.saveFileDialog1.FileName = "doc1";
            this.saveFileDialog1.Filter = "RichText files (*.rtf)|*.rtf|Text files (*.txt)|*.txt|All files (*.*)|*.*\"";
            this.saveFileDialog1.Title = "Save As...";
            // 
            // ucRichTextEditor
            // 
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.tbControls);
            this.Name = "ucRichTextEditor";
            this.Size = new System.Drawing.Size(447, 331);
            this.tbControls.ResumeLayout(false);
            this.tbControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region RichText Editor
		private void tbControls_ButtonClick(object sender, ToolStripItemClickedEventArgs e)
		{
            ToolStripButton button = e.ClickedItem as ToolStripButton;
            if (button == tbLeft)
			{
				tbLeft.Checked = rcxmnuLeft.Checked = true;
                tbCentre.Checked = rcxmnuCentre.Checked = false;
                tbRight.Checked = rcxmnuRight.Checked = false;
				richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
			}
			if (button == tbCentre)
			{
				tbLeft.Checked = rcxmnuLeft.Checked = false;
                tbCentre.Checked = rcxmnuCentre.Checked = true;
                tbRight.Checked = rcxmnuRight.Checked = false;
				richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
			}
			if (button == tbRight)
			{
                tbLeft.Checked = rcxmnuLeft.Checked = false;
                tbCentre.Checked = rcxmnuCentre.Checked = false;
                tbRight.Checked = rcxmnuRight.Checked = true;
                richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
			}

			if (button == tbBold || button == tbItalic || button == tbUnderline)
			{
				if (sender != tbControls)
				{
					if (sender == rcxmnuBold)
					{
                        tbBold.Checked = rcxmnuBold.Checked = !rcxmnuBold.Checked;
					}
					if (sender == rcxmnuItalic)
					{
                        tbItalic.Checked = rcxmnuItalic.Checked = !rcxmnuItalic.Checked;
					}
					if (sender == rcxmnuUnderline)
					{
                        tbUnderline.Checked = rcxmnuUnderline.Checked = !rcxmnuUnderline.Checked;
					}
				}
				else
				{
                    if (button == tbBold)
                    {
                        rcxmnuBold.Checked = tbBold.Checked = !tbBold.Checked;
                    }
                    if (button == tbItalic)
                    {
                        rcxmnuItalic.Checked = tbItalic.Checked = !tbItalic.Checked;
                    }
                    if (button == tbUnderline)
                    {
                        rcxmnuUnderline.Checked = tbUnderline.Checked = !tbUnderline.Checked;
                    }
				}

				if (richTextBox1.SelectionFont != null)
				{
					System.Drawing.Font currentFont = richTextBox1.SelectionFont;
					System.Drawing.FontStyle newFontStyle = FontStyle.Regular;
					if (tbBold.Checked) newFontStyle = newFontStyle | FontStyle.Bold;
					if (tbItalic.Checked) newFontStyle = newFontStyle | FontStyle.Italic;
					if (tbUnderline.Checked) newFontStyle = newFontStyle | FontStyle.Underline;
					
					richTextBox1.SelectionFont = new Font(
						currentFont.FontFamily, 
						currentFont.Size, 
						newFontStyle
						);
				}

				if (!richTextBox1.Visible)
					richTextBox1.Visible = true;
			}
		}

		private void richTextBox1_SelectionChanged(object sender, System.EventArgs e)
		{
			if (Parent == null || richTextBox1.Visible == false) return;
			if (richTextBox1.SelectionFont != null)
			{
				tbBold.Checked = richTextBox1.SelectionFont.Bold;
				tbItalic.Checked = richTextBox1.SelectionFont.Italic;
				tbUnderline.Checked = richTextBox1.SelectionFont.Underline;
				cmbFont.Text = richTextBox1.SelectionFont.FontFamily.GetName(0);
				cmbSize.Text = richTextBox1.SelectionFont.Size.ToString();
				rcxmnuBold.Checked = tbBold.Checked;
				rcxmnuItalic.Checked = tbItalic.Checked;
				rcxmnuUnderline.Checked = tbUnderline.Checked;
			}
			tbLeft.Checked = (richTextBox1.SelectionAlignment == HorizontalAlignment.Left);
			tbCentre.Checked = (richTextBox1.SelectionAlignment == HorizontalAlignment.Center);
			tbRight.Checked = (richTextBox1.SelectionAlignment == HorizontalAlignment.Right);
			rcxmnuLeft.Checked = tbLeft.Checked;
			rcxmnuCentre.Checked = tbCentre.Checked;
			rcxmnuRight.Checked = tbRight.Checked;
		}

		private void cmbFont_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			System.Drawing.Font currentFont = richTextBox1.SelectionFont;
			try
			{
				richTextBox1.SelectionFont = new Font(cmbFont.Text, currentFont.Size);
				if (!richTextBox1.Visible)
					richTextBox1.Visible = true;
			}
			catch
			{
			}

		}

		private void rcxmnuMenuButtons_Click(object sender, System.EventArgs e)
		{
			if (sender == rcxmnuBold)
				tbControls_ButtonClick(sender,new ToolStripItemClickedEventArgs(tbBold));
			if (sender == rcxmnuItalic)
				tbControls_ButtonClick(sender,new ToolStripItemClickedEventArgs(tbItalic));
			if (sender == rcxmnuUnderline)
				tbControls_ButtonClick(sender,new ToolStripItemClickedEventArgs(tbUnderline));
			if (sender == rcxmnuLeft)
				tbControls_ButtonClick(sender,new ToolStripItemClickedEventArgs(tbLeft));
			if (sender == rcxmnuCentre)
				tbControls_ButtonClick(sender,new ToolStripItemClickedEventArgs(tbCentre));
			if (sender == rcxmnuRight)
				tbControls_ButtonClick(sender,new ToolStripItemClickedEventArgs(tbRight));
			if (sender == rcxmnuCut) richTextBox1.Cut();
			if (sender == rcxmnuCopy) richTextBox1.Copy();
			if (sender == rcxmnuPaste) richTextBox1.Paste();
			if (sender == rcxmnuFont)
			{
				fontDialog1.Font = richTextBox1.SelectionFont;
				if (fontDialog1.ShowDialog(this) == DialogResult.OK)
				{
					try
					{
						richTextBox1.SelectionFont = fontDialog1.Font;
						richTextBox1_SelectionChanged(sender,e);
					}
					catch
					{}
				}
			}
			if (sender == rcxmnuNew && rcxmnuFile.Visible) 
			{
				if (IsDirty==false) return;
				richTextBox1.Clear();
				richTextBox1.ClearUndo();
				SetFileName(new System.IO.FileInfo("doc1"));
				Dirty=false;
			}
			if (sender == rcxmnuOpen && rcxmnuFile.Visible)
			{
				if (IsDirty==false) return;
				if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					richTextBox1.LoadFile(openFileDialog1.FileName);
					SetFileName(new System.IO.FileInfo(openFileDialog1.FileName));
					Dirty=false;

				}
			}
			if (sender == rcxmnuSaveAs && rcxmnuFile.Visible)
			{
				if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					richTextBox1.SaveFile(saveFileDialog1.FileName);
					SetFileName(new System.IO.FileInfo(saveFileDialog1.FileName));
					Dirty=false;
				}
			}
			if (sender == rcxmnuSave && rcxmnuFile.Visible)
			{
				if (currentfile.Name == "doc1")
				{
					rcxmnuMenuButtons_Click(rcxmnuSaveAs,new ToolBarButtonClickEventArgs(null));
					return;
				}
				richTextBox1.SaveFile(currentfile.FullName);
				Dirty=false;

			}
			if (sender == rcxmnuClose && rcxmnuFile.Visible)
			{
				if (IsDirty==false) return;
				this.ParentForm.Close();
			}
		}

		private void cmbSize_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				System.Drawing.Font currentFont = richTextBox1.SelectionFont;
				richTextBox1.SelectionFont = new Font(currentFont.FontFamily, Convert.ToSingle(cmbSize.Text), currentFont.Style);
				if (!richTextBox1.Visible)
					richTextBox1.Visible = true;
			}
			catch{}
		}

		private void SetFileName(System.IO.FileInfo filename)
		{
			this.ParentForm.Text = title.Replace("%1%",filename.Name);
			currentfile = filename;
		}

		[Browsable(false)]
		[DefaultValue(false)]
		public bool IsDirty
		{
			get
			{
				try
				{
					if (Dirty)
					{
						DialogResult dr = System.Windows.Forms.MessageBox.Show(this,Session.CurrentSession.Resources.GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?","").Text,ParentForm.Text,MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
						if (dr == DialogResult.Yes) rcxmnuMenuButtons_Click(rcxmnuSave, EventArgs.Empty);
						if (dr == DialogResult.No)
						{
							Dirty=false;
						}
						if (dr == DialogResult.Cancel) return false;
					}
				}
				catch
				{}
				return true;
			}
			set
			{
				Dirty = value;
			}
		}

		private void richTextBox1_TextChanged(object sender, System.EventArgs e)
		{
			Dirty=true;
			if (Dirty) OnChanged();
		}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if (Parent != null)
			try
			{
				FWBS.OMS.UI.Windows.Global.RightToLeftControlConverter(this, ParentForm);
				Dirty=false;
				rcxmnuMenuButtons_Click(rcxmnuNew, EventArgs.Empty);
				if (_parenthasmenu==null)
					_parenthasmenu = (this.ParentForm.Menu != null);
				if (_showmenu)
				{
					SetMenus();
				}
			}
			catch
			{}
		}

		private void SetMenus()
		{
			try
			{
				if (omsDesignMode == false)
					if (_parenthasmenu == true)
					{
						if (this.ParentForm.Menu.MenuItems.Contains(rcxmnuFile) == false)
						{
							this.ParentForm.Menu.MenuItems.Add(rcxmnuFile);
							this.ParentForm.Menu.MenuItems.Add(rcxmnuEdit);
							this.ParentForm.Menu.MenuItems.Add(rcxmnuFormat);
							this.ParentForm.Menu.MenuItems.Add(rcxmnuHelp);
						}
					}
					else
					{
						this.ParentForm.Menu = mainMenu1;
					}
			}
			catch
			{}
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			if (!_showmenu) SetMenus();
		}

		protected override void OnLeave(EventArgs e)
		{
			try
			{
				if (omsDesignMode == false)
					if (_parenthasmenu == true)
					{
						this.ParentForm.Menu.MenuItems.Remove(rcxmnuFile);
						this.ParentForm.Menu.MenuItems.Remove(rcxmnuEdit);
						this.ParentForm.Menu.MenuItems.Remove(rcxmnuFormat);
						this.ParentForm.Menu.MenuItems.Remove(rcxmnuHelp);
					}
					else
					{
						this.ParentForm.Menu = null;
					}
			}
			catch{}
			base.OnLeave(e);
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			richTextBox1.Enabled=this.Enabled;
			rcxmnuFile.Enabled=this.Enabled;
			rcxmnuEdit.Enabled=this.Enabled;
			rcxmnuFormat.Enabled=this.Enabled;
			rcxmnuHelp.Enabled=this.Enabled;
            tbControls.Enabled=this.Enabled;
		}
		#endregion

		#region Private
		private bool _showmenu = false;

		private void richTextBox1_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.LinkText);
		}

		#endregion

		private void cmbFont_Enter(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				if (tbControls.Visible && !omsDesignMode && cmbFont.Items.Count == 0)
				{
					System.Drawing.Text.InstalledFontCollection fonts = new System.Drawing.Text.InstalledFontCollection(); 
					for(int i =0;i < fonts.Families.Length;i++)
						cmbFont.Items.Add(fonts.Families[i].GetName(0));
				}
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		#region Properties
		[Category("RichText")]
		[DefaultValue(false)]
		[Description("Always show the Menu overwise it will only become visible when the richtext box has focus")]
		public bool AlwaysShowMenu
		{
			get
			{
				return _showmenu;
			}
			set
			{
				_showmenu = value;
			}
		}
		
		[Category("RichText")]
		[DefaultValue(true)]
		[Description("Shows the Tool Bar")]
		public bool ShowToolBar
		{
			get
			{
				return tbControls.Visible;
			}
			set
			{
                SuspendLayout();
                tbControls.Visible = value;
                richTextBox1.Anchor = AnchorStyles.None;
                richTextBox1.Top = value ? tbControls.Height : 0;
                richTextBox1.Height = this.Height - richTextBox1.Top;
                richTextBox1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                ResumeLayout();
                Invalidate();
			}
		}

		[Category("RichText")]
		[DefaultValue(false)]
		[Description("Shows the Close option in the File Menu")]
		public bool ShowClose
		{
			get
			{
				return rcxmnuClose.Visible;
			}
			set
			{
				rcxmnuClose.Visible=value;
			}
		}

		
		[Category("RichText")]
		[DefaultValue(false)]
		[Description("Shows the File Menu")]
		public bool ShowFileMenu
		{
			get
			{
				return rcxmnuFile.Visible;
			}
			set
			{
				rcxmnuFile.Visible = value;
			}
		}

		[Category("RichText")]
		[DefaultValue(false)]
		[Description("Shows the Edit Menu")]
		public bool ShowEditMenu
		{
			get
			{
				return rcxmnuEdit.Visible;
			}
			set
			{
				rcxmnuEdit.Visible = value;
			}
		}

		[Category("RichText")]
		[DefaultValue(false)]
		[Description("Shows the Help Menu")]
		public bool ShowHelpMenu
		{
			get
			{
				return rcxmnuHelp.Visible;
			}
			set
			{
				rcxmnuHelp.Visible = value;
			}
		}

		[Category("RichText")]
		[DefaultValue(false)]
		[Description("Shows the Format Menu")]
		public bool ShowFormatMenu
		{
			get
			{
				return rcxmnuFormat.Visible;
			}
			set
			{
				rcxmnuFormat.Visible = value;
			}
		}

		[Category("RichText")]
		[DefaultValue(TextMode.Text)]
		[Description("The Text Mode Output")]
		public TextMode TextMode
		{
			get
			{
				return _textmode;
			}
			set
			{
				if (_textmode != value)
				{
					_textmode = value;
					if (TextModeChanged != null)
						TextModeChanged(this, EventArgs.Empty);
				}
			}
		}
        #endregion
    }
}