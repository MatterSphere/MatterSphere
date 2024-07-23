using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucVertialContactDisplay.
    /// </summary>
    public class ucVerticalContact : System.Windows.Forms.UserControl
	{
		protected System.Windows.Forms.Panel pnlMain;
		protected FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.ComponentModel.IContainer components;
		private Color _headercolor = SystemColors.Control;

		#region Constructors & Dispose
	
		public ucVerticalContact()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(5, 5);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(151, 180);
            this.pnlMain.TabIndex = 0;
            // 
            // ucVerticalContact
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.pnlMain);
            this.Name = "ucVerticalContact";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(161, 190);
            this.ResumeLayout(false);

		}
		#endregion

		#region Properties
		[Browsable(false)]
		public bool Selected
		{
			get
			{
				return (pnlMain.BorderStyle != BorderStyle.None);
			}
			set
			{
                pnlMain.BorderStyle = value ? BorderStyle.FixedSingle : BorderStyle.None;
			}
		}

		public Color HeaderColor
		{
			get
			{
				return _headercolor;
			}
			set
			{
				_headercolor = value;
			}
		}
		#endregion

		#region Public Methods
		public void Add(string Caption, object Value, string Format, int Width)
		{
			if (Convert.ToString(Value) != "")
			{
				if (pnlMain.Controls.Count == 0)
				{
					Label item = new Label();
					item.TextAlign = ContentAlignment.MiddleLeft;
                    item.UseMnemonic = false;
					item.BackColor = this.HeaderColor;
					item.Height = 20;
					item.Text = Convert.ToString(Value);
					item.Dock = DockStyle.Top;
					pnlMain.Controls.Add(item);
					item.BringToFront();

					item = new Label();
					item.Height = 4;
					item.Dock = DockStyle.Top;
					pnlMain.Controls.Add(item);
					item.BringToFront();
				
				}
				else if (Caption == "" && Format == "Standard")
				{
					Label item = new Label();
					item.TextAlign = ContentAlignment.MiddleLeft;
                    item.UseMnemonic = false;
					item.Text = Convert.ToString(Value);
					item.Height = 16;
					item.Dock = DockStyle.Top;
					pnlMain.Controls.Add(item);
					item.BringToFront();
				}
				else
				{
					FWBS.Common.UI.Windows.eLabel2 item = new FWBS.Common.UI.Windows.eLabel2();
					item.Text = Caption + ":";
					item.Format = ApplyFormat(Format);
					item.Value = Value;
					item.CaptionWidth = (Caption != "") ? Width : 0;
					item.Height = 16;
					item.Dock = DockStyle.Top;
					pnlMain.Controls.Add(item);
					item.BringToFront();
				}
			}
		}

		public void SizeToFit()
		{
			int height = 0;
			foreach (Control ctrl in pnlMain.Controls)
			{
				if (ctrl.Visible)
				{
					height += ctrl.Height;
					ctrl.Click -=new EventHandler(BoxToggle_Click);
					ctrl.Click +=new EventHandler(BoxToggle_Click);
					ctrl.DoubleClick -= new EventHandler(BoxToggle_DoubleClick);
					ctrl.DoubleClick += new EventHandler(BoxToggle_DoubleClick);
				}
			}
            this.Height = height + LogicalToDeviceUnits(14);
        }
		#endregion

		#region Private
		private string ApplyFormat(string format)
		{
			try
			{
				if (format == "Number")
				{
					return "F";
				}
				else if (format == "Currency")
				{
					return "c";
				}
				else if (format == "DateTimeRgt")
				{
					return "d t";
				}
				else if (format == "DateTimeLft")
				{
					return "d t";
				}
				else if (format == "DateLft")
				{
					return "d";
				}
				else if (format == "DateLongLft")
				{
					return "D";
				}
				else if (format == "Date" || format == "DateRgt")
				{
					return "d";
				}
				else if (format == "Time" || format == "TimeRgt")
				{
					return "t";
				}
				else
				{
					return "";
				}
			}
			catch
			{
				return "";
			}
		}

		private void BoxToggle_Click(object sender, System.EventArgs e)
		{
			this.Selected=true;
			this.OnClick(e);
		}

		private void BoxToggle_DoubleClick(object sender, System.EventArgs e)
		{
			this.OnDoubleClick(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			SizeToFit();
		}
		#endregion
	}
}
