using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ThreeDPanel.
    /// </summary>
    public enum ThreeDBorder3DStyle
	{
        Transparent = 0,
        None = Border3DStyle.Adjust,
		RaisedOuter = Border3DStyle.RaisedOuter,
		SunkenOuter = Border3DStyle.SunkenOuter,
		RaisedInner = Border3DStyle.RaisedInner,
		SunkenInner = Border3DStyle.SunkenInner,
		Raised = Border3DStyle.Raised,
		Sunken = Border3DStyle.Sunken,
		Etched = Border3DStyle.Etched,
		Bump = Border3DStyle.Bump,
		Flat = Border3DStyle.Flat
	};
	public enum ThreeDBorder3DSide
	{
		Left = Border3DSide.Left,
		Top = Border3DSide.Top,
		Right = Border3DSide.Right,
		Bottom = Border3DSide.Bottom,
		All = Border3DSide.All
	}	
	
	[Designer(typeof(ThreedParentControlDesigner))]
	public class ThreeDPanel : UserControl
	{
		protected ThreeDBorder3DStyle _borderstyle = ThreeDBorder3DStyle.Etched;
		protected ThreeDBorder3DSide _borderside = ThreeDBorder3DSide.All;
		public ThreeDPanel()
		{
			/// <summary>
			/// Required for Windows.Forms Class Composition Designer support
			/// </summary>
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			InitializeComponent();
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ThreeDPanel
			// 
			this.Name = "ThreeDPanel";
			this.Size = new System.Drawing.Size(155, 156);
		}
		#endregion

		[DefaultValue(ThreeDBorder3DSide.All)]
		[Category("Appearance")]
		public ThreeDBorder3DSide BorderSide
		{
			get
			{
				return _borderside;
			}
			set
			{
				_borderside = value;
				Invalidate();
			}
		}
		
		
        [Browsable(false)]
		new public ThreeDBorder3DStyle BorderStyle
		{
            //CM : 28.11.13 - Pass through values into 'ThreeDBorderStyle' (Bug 2497)
            get
            {
                return this.ThreeDBorderStyle;
            }
            set
            {
                this.ThreeDBorderStyle = value;
            }
		}

        [DefaultValue(ThreeDBorder3DStyle.Etched)]
        [Category("Appearance")]
        public ThreeDBorder3DStyle ThreeDBorderStyle
        {
            get
            {
                return _borderstyle;
            }
            set
            {
                _borderstyle = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_borderstyle != ThreeDBorder3DStyle.Transparent)
            {
                ControlPaint.DrawBorder3D(e.Graphics, 1, 1, this.Width - 3, this.Height - 3, (Border3DStyle)_borderstyle, (Border3DSide)_borderside);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

    }

	public class ThreedParentControlDesigner : System.Windows.Forms.Design.ParentControlDesigner
	{

		protected override bool DrawGrid
		{
			get { return false; }
		}
	}
}
