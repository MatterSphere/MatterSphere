using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// Summary description for ucCaptionLine.
    /// </summary>

    [Designer(typeof(CTRLPanelDesigner))]
    public class eCaptionLine2 : Panel, IeCaptionLine
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Color linecolor;
        private System.ComponentModel.Container components = null;
        private ExtColor _forecolor;
        private ExtColor _fraforecolor;
        private Color backcolor;
        private bool _fontStyleBold = true;

        /// <summary>
        /// Gets or Set the string for the Controls Caption
        /// </summary>
        [Category("Appearance")]
        [Browsable(true)]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.Invalidate();
            }
        }

        [Browsable(false)]
        public new bool TabStop
        {
            get
            {
                return false;
            }
            set
            {

            }
        }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool FontStyleBold
        {
            get
            {
                return _fontStyleBold;
            }
            set
            {
                _fontStyleBold = value;
                this.Invalidate();
            }
        }

        [Browsable(false)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
            }
        }

        [Category("Appearance")]
        public ExtColor FontColor
        {
            get
            {
                return _forecolor;
            }
            set
            {
                _forecolor.SettingsChanged -= new EventHandler(SettingsChanged);
                _forecolor = value;
                _forecolor.SettingsChanged += new EventHandler(SettingsChanged);
            }
        }

        [Category("Appearance")]
        public ExtColor FrameForeColor
        {
            get
            {
                return _fraforecolor;
            }
            set
            {
                _fraforecolor.SettingsChanged -= new EventHandler(SettingsChanged);
                _fraforecolor = value;
                _fraforecolor.SettingsChanged += new EventHandler(SettingsChanged);
            }
        }

        [Category("Appearance")]
        public new System.Drawing.Color BackColor
        {
            get
            {
                return backcolor;
            }
            set
            {
                backcolor = value;
                this.Invalidate();
            }
        }


        [Category("Appearance")]
        public System.Drawing.Color LineColor
        {
            get
            {
                return linecolor;
            }
            set
            {
                linecolor = value;
                this.Invalidate();
            }
        }

        public eCaptionLine2()
        {
            // Set the value of the double-buffering style bits to true.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            _forecolor = new ExtColor(ExtColorPresets.FrameForeColor, ExtColorTheme.Auto);
            _fraforecolor = new ExtColor(ExtColorPresets.FrameLineForeColor, ExtColorTheme.Auto);
            base.BackColor = System.Drawing.Color.WhiteSmoke;
            backcolor = base.BackColor;
            linecolor = Color.FromArgb(218, 222, 214);
        }


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // eCaptionLine
            // 
            this.Name = "ucCaptionLine";
            this.Size = new System.Drawing.Size(516, 25);
            this.Resize += new System.EventHandler(this.ucCaptionLine_Resize);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ucCaptionLine_Paint);
            this.ParentChanged += new EventHandler(eCaptionLine_ParentChanged);
        }
        #endregion

        private void ucCaptionLine_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            PaintCaptionLine(e);
        }


        public void PaintCaptionLine(System.Windows.Forms.PaintEventArgs e)
        {
            eCaptionLinePainter painter = new eCaptionLinePainter(this);
            painter.PaintCaptionLine(e);
        }


        private void ucCaptionLine_Resize(object sender, System.EventArgs e)
        {
            this.Invalidate();
        }


        private void eCaptionLine_ParentChanged(object sender, EventArgs e)
        {
            base.TabIndex = 9999;
            base.TabStop = false;
        }

        private void SettingsChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
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


        public void DrawCaptionLine(DrawingParameters pars)
        {
            pars.b2 = new SolidBrush(backcolor);                          //used for background
            pars.p1.Color = linecolor;                                    //used for the line

            if (this.RightToLeft == RightToLeft.Yes)
                DrawCaptionLineRTL(pars);
            else
                DrawCaptionLineLTR(pars);

        }

        private void DrawCaptionLineRTL(DrawingParameters pars)
        {
            pars.args.Graphics.FillRectangle(pars.b2, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            int width = this.Width - (pars.textsize.Width + 3);
            pars.args.Graphics.DrawString(this.Text, pars.f, pars.b1, new Point(this.Width, (this.Height - pars.textsize.Height) / 2), new StringFormat(StringFormatFlags.DirectionRightToLeft));
            pars.args.Graphics.DrawLine(pars.p1, new Point(0, this.Height - 1), new Point(width, this.Height - 1));
        }


        private void DrawCaptionLineLTR(DrawingParameters pars)
        {
            pars.args.Graphics.FillRectangle(pars.b2, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            pars.args.Graphics.DrawString(this.Text, pars.f, pars.b1, new Point(0, (this.Height - pars.textsize.Height) / 2));
            pars.args.Graphics.DrawLine(pars.p1, new Point(pars.textsize.Left, this.Height - 1), new Point(this.Width, this.Height - 1));
        }


    }

}
