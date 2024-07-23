using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucNavPanel.
    /// </summary>
    [ToolboxItem(false)]
	public class ucNavPanel : System.Windows.Forms.ContainerControl
	{
		#region Fields
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// The Color of the Line drawn around the Panel to give the alutsion of one object
		/// </summary>
		private System.Drawing.Color _linecolor;
		/// <summary>
		/// Color of the Panel to override the Parent leave empty to accept parent color
		/// </summary>
		private System.Drawing.Color _panelcolor = Color.Empty;
		/// <summary>
		/// Set the Brightness of the the line compared to the Background color
		/// </summary>
		private int _brightness = 20;
		/// <summary>
		/// Autosizes Parent to fit the Panels Content all content must be docked to the bottom this
		/// is automatically done
		/// </summary>
		private bool _autosize = false;

		private ToolTip _toolTip = null;

        private Control _parent = null;
		#endregion

		#region Contructors

		/// <summary>
		/// Set the Hooks to capture any content added to the Panel and set the style to 
		/// stop the dam thing flickering when resizing
		/// </summary>
		public ucNavPanel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.components = new Container();
			this._toolTip = new ToolTip(components);

			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.ControlAdded += new ControlEventHandler(myControlAdded);
			this.ControlRemoved +=new ControlEventHandler(ucNavPanel_ControlRemoved);
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            try
            {
                if (disposing)
                {
                    if (_parent != null)
                        _parent.BackColorChanged -= new EventHandler(Parent_BackColorChanged);

                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
		}
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ucNavPanel
			// 
			this.DockPadding.All = 5;
			this.Resize += new System.EventHandler(this.ucNavPanel_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ucNavPanel_Paint);
			this.ParentChanged += new System.EventHandler(this.ucNavPanel_ParentChanged);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Overrides Parent color is the _panelcolor different for Empty
		/// </summary>
		/// <param name="sender">The Object</param>
		/// <param name="e">Empty</param>
		private void Parent_BackColorChanged(object sender, EventArgs e)
		{
			if (_panelcolor == Color.Empty)
			{
				if (Parent != null)
					this.BackColor = Parent.BackColor;
				else
					this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
			}
		}

		/// <summary>
		/// Foreces a refresh of the Parent AutoResize if the controls added to the panel
		/// are resized
		/// </summary>
		/// <param name="sender">The Control that has resized</param>
		/// <param name="e">Empty</param>
		private void Control_Resize(object sender, EventArgs e)
		{
			
            Refresh();
		}

		/// <summary>
		/// Removes its dirty little hooks from the Controls if they are removed from the Panel
		/// </summary>
		/// <param name="sender">This Object</param>
		/// <param name="e">The Control that has been removed</param>
		private void ucNavPanel_ControlRemoved(object sender, ControlEventArgs e)
		{
			e.Control.Resize -=new EventHandler(Control_Resize);
		}
		
		/// <summary>
		/// Draws the Line around the panel hiding the top bar of Panel
		/// </summary>
		/// <param name="sender">This Object</param>
		/// <param name="e">Graphics Property</param>
		private void ucNavPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			_linecolor = ModernStyle ? Color.FromArgb(244, 244, 244) : ucPanelNav.ChangeBrightness(this.BackColor,_brightness);
            using (Pen pen = new System.Drawing.Pen(_linecolor,2))
            {
			    e.Graphics.DrawRectangle(pen,1,-1,this.Width-2,this.Height);
            }
		}

		/// <summary>
		/// Set the Anchor to the Bottom Left if no Dock Style is set and Hook in the Controls Resize Event
		/// </summary>
		/// <param name="sender">This Object</param>
		/// <param name="e">The Control that have added</param>
		private void myControlAdded(object sender,ControlEventArgs e)
		{
			if (e.Control.Dock == DockStyle.None)
				e.Control.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			e.Control.Resize +=new EventHandler(Control_Resize);
			Refresh();
		}

		/// <summary>
		/// If the Panel is resized Invalidate the drawing area
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ucNavPanel_Resize(object sender, System.EventArgs e)
		{
			this.Invalidate();
		}

		/// <summary>
		/// When the Parent is changed get details from the Parent and set the backcolor and hook for 
		/// back color changes
		/// </summary>
		/// <param name="sender">The Object</param>
		/// <param name="e">Empty</param>
		private void ucNavPanel_ParentChanged(object sender, System.EventArgs e)
		{
            if (this.Parent != null) _parent = this.Parent;
            Refresh();
			if (_panelcolor == Color.Empty)
			{
				if (_parent != null)
				{
                    this.BackColor = _parent.BackColor;
                    _parent.BackColorChanged += new EventHandler(Parent_BackColorChanged);
				}
				else
					this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
			}
			else
			{
                if (_parent != null) _parent.BackColorChanged -= new EventHandler(Parent_BackColorChanged);
			}
		}
		#endregion

		#region Public
		/// <summary>
		/// Override the Refresh to Resize of the Content Changes
		/// </summary>
		public new void Refresh()
		{
			if (Parent != null && _autosize)
			{
				Parent_BackColorChanged(this,EventArgs.Empty);
				int h =0;
				ucPanelNav n = (ucPanelNav)Parent;
                if (n.Animating) return;
				h= n.labHeader.Height + this.DockPadding.Top + this.DockPadding.Bottom + n.pnlSpace.Height;
				if (this.Controls.Count > 0)
				{
					foreach(Control ctrl in this.Controls)
					{
						h=h + ctrl.Height;
						ctrl.Dock = DockStyle.Bottom;
					}
				}
				else
					h = n.labHeader.Height + n.pnlSpace.Height;

				if (n.Expanded)
				{
					n.ExpandedHeight = h;
					n.Height =h;
				}
				else
				{
					n.ExpandedHeight = h;
				}
			}
			base.Refresh();
		}
        #endregion

        #region Properites

        [DefaultValue(false)]
        [Browsable(false)]
        public bool ModernStyle { get; set; }

		/// <summary>
		/// Get or Set the Auto Size Property
		/// </summary>
		[Category("Panel")]
		[DefaultValue(false)]
		new public bool AutoSize
		{
			get
			{
				return _autosize;
			}
			set
			{
				if (value != _autosize)
				{
					_autosize = value;
					if (_autosize)
					{
						Refresh();
					}
				}
			}
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

		/// <summary>
		/// Hides the Dock Property
		/// </summary>
		[Browsable(false)]
		public override System.Windows.Forms.DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				if (value == System.Windows.Forms.DockStyle.Fill)
					base.Dock = value;
			}
		}

		/// <summary>
		/// Get or Set the Overriding Panel Back Color
		/// </summary>
		[Category("Panel")]
		[DefaultValue(typeof(System.Drawing.Color),"Empty")]
		public Color PanelBackColor
		{
			get
			{
				return _panelcolor;
			}
			set
			{
				_panelcolor = value;
				this.BackColor = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or Set the Brightness of the Line around the Panel based on the backcolor
		/// </summary>
		[Category("Panel")]
		[DefaultValue(20)]
		public int Brightness
		{
			get
			{
				return _brightness;
			}
			set
			{
				_brightness = value;
				this.Invalidate();
			}
		}

		public ToolTip ToolTip
		{
			get
			{
				return _toolTip;
			}
		}
		#endregion
	}
}
