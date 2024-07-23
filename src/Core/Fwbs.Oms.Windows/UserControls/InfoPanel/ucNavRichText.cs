using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucNavRichText.
    /// </summary>
    public class ucNavRichText : ucNavPanel, ISupportRightToLeft
	{
		private FWBS.OMS.UI.RichTextBox RichText;

		public ucNavRichText()
		{
			/// <summary>
			/// Required for Windows.Forms Class Composition Designer support
			/// </summary>
			InitializeComponent();
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.RichText = new FWBS.OMS.UI.RichTextBox();
			this.SuspendLayout();
			// 
			// RichText
			// 
			this.RichText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.RichText.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.RichText.Location = new System.Drawing.Point(17, 17);
			this.RichText.Name = "RichText";
			this.RichText.ReadOnly = true;
			this.RichText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.RichText.TabIndex = 0;
			this.RichText.Text = "richTextBox1";
			this.RichText.WordWrap = false;
			this.RichText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.lbl_LinkClicked);
			// 
			// ucNavRichText
			// 
			this.Controls.Add(this.RichText);
			this.DockPadding.All = 3;
			this.SizeChanged += new System.EventHandler(this.ucNavRichText_SizeChanged);
			this.BackColorChanged += new System.EventHandler(this.ucNavRichText_BackColorChanged);
			this.ParentChanged += new System.EventHandler(this.ucNavRichText_ParentChanged);
			this.ResumeLayout(false);

		}
        #endregion

        private void lbl_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.LinkText);
		}

		private void ucNavRichText_SizeChanged(object sender, System.EventArgs e)
		{
			if (DesignMode)
			{
				RichText.Location = new Point(5,3);
				RichText.Size = new Size(this.Width-10,this.Height-6);
			}
		}

        ucPanelNav parent;
		private void ucNavRichText_ParentChanged(object sender, System.EventArgs e)
		{
			if (Parent != null)
			{
				ucPanelNav p = (ucPanelNav)Parent;
                parent = p;
				p.Load += new EventHandler(ucNavRichText_Load);
			}
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (parent != null)
                    parent.Load -= new EventHandler(ucNavRichText_Load);
            }
            base.Dispose(disposing);
        }

		private void ucNavRichText_Load(object sender, System.EventArgs e)
		{
			Refresh();
		}

		private void ucNavRichText_BackColorChanged(object sender, System.EventArgs e)
		{
			RichText.BackColor = this.BackColor;
		}

		public new void Refresh()
		{
            ucPanelNav panelNav = Parent as ucPanelNav;
            if (panelNav != null)
			{
                panelNav.Height = RichText.GetPreferredSize(Size.Empty).Height + panelNav.pnlSpace.Height + panelNav.labHeader.Height + panelNav.DockPadding.Top + panelNav.DockPadding.Bottom + LogicalToDeviceUnits(11);
                panelNav.ExpandedHeight = panelNav.Height;
			}

            RichText.Anchor = AnchorStyles.None;
            RichText.Location = new Point(LogicalToDeviceUnits(5), LogicalToDeviceUnits(5));
			RichText.Size = new Size(this.Width - LogicalToDeviceUnits(10), this.Height - LogicalToDeviceUnits(6));
			this.RichText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
                if (ModernStyle)
                {
                    base.BackColor = value;
                }
			}
		}

		[Browsable(false)]
		public string Rtf
		{
			get
			{
				return RichText.Rtf;
			}
			set
			{
				try
				{
					RichText.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(value);
                    if (RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                        Global.SetRichTextBoxRightToLeft(RichText);
				}
				catch
				{
					RichText.Text = value;
				}
			}
		}

		[Browsable(true)]
		public override string Text
		{
			get
			{
				return RichText.Text;
			}
			set
			{
                RichText.Text = value;
                RichText.SelectAll();
                RichText.SelectionFont = RichText.Font;
                RichText.Select(0, 0);
            }
		}

		public FWBS.OMS.UI.RichTextBox ControlRich
		{
			get
			{
				return RichText;
			}
			set
			{
				RichText = value;
			}
		}


        public void SetRTL(Form parentform)
        {

        }
    }
}
