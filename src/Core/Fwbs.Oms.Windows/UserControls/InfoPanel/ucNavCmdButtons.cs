using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucFavorites.
    /// </summary>
    [Serializable()] 
	public class ucNavCmdButtons : Control
	{
		private ImageList _imagelist;
		private int _imageindex =-1;
		private string _key = "";
		private string _text = "";
		private Rectangle _activehover;
		private bool _underline = false;
		private bool _mousedown = false;
        private bool _visible = true;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public ucNavCmdButtons()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.UpdateStyles();
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}

		public event EventHandler LinkClicked;

		protected void OnLinkClicked(object sender, EventArgs e) 
		{
            try
            {
                LinkClicked?.Invoke(this, EventArgs.Empty);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex);
            }
		}

        public void AttachButton(Button button)
        {
            button.Click += OnLinkClicked;
            button.Tag = Tag;
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
			}
		}
	
		[DefaultValue("")]
		[Category("Data")]
		public string Key
		{
			get
			{
				return _key;
			}
			set
			{
				_key = value;
			}
		}
		
		[DefaultValue(null)]
		public ImageList ImageList
		{
			get
			{
				return _imagelist;
			}
			set
			{
				_imagelist = value;
				this.Invalidate();
			}
		}

		[Category("Appearance")]
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor(typeof(IconDisplayEditor),typeof(UITypeEditor))]
		[DefaultValue(null)]
		public int ImageIndex
		{
			get
			{
				return _imageindex;
			}
			set
			{
				_imageindex = value;
				this.Invalidate();

			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		public override string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
				this.Invalidate();
				OnTextChanged(new EventArgs());
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// ucNavCmdButtons
			// 
			this.Size = new System.Drawing.Size(240, 22);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.picFavorite_Paint);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ucNavCmdButtons_MouseDown);
			this.MouseLeave += new System.EventHandler(this.ucNavCmdButtons_MouseLeave);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ucNavCmdButtons_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ucNavCmdButtons_MouseUp);
			this.ResumeLayout(false);

		}
		#endregion

		
		public override string ToString()
		{
			return this.Key;
		}

		private void picFavorite_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
            string text = _text;
            StringFormat sf = new StringFormat(StringFormatFlags.NoWrap)
            {
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter
            };
            if (this.RightToLeft == RightToLeft.Yes)
                sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

            try
            {
                Image image = (_imagelist != null && _imageindex > -1 && _imageindex < _imagelist.Images.Count) ? _imagelist.Images[_imageindex] : null;
				if (image != null)
				{
                    text = '\t' + _text;
                    sf.SetTabStops(0, new float[] { LogicalToDeviceUnits(20) });
                }

                Color c = this.ForeColor;
                if (_underline && _mousedown)
                    c = Color.Red;
                else if (!this.Enabled)
                    c = SystemColors.GrayText;

                Size textSize = Size.Ceiling(e.Graphics.MeasureString(text, this.Font, ClientSize, sf));
                _activehover = new Rectangle(this.RightToLeft != RightToLeft.Yes ? 0 : this.Width - textSize.Width,
                    (this.Height - textSize.Height) / 2, textSize.Width, textSize.Height);

                using (SolidBrush br = new SolidBrush(c))
                using (Font f = new Font(this.Font, _underline ? FontStyle.Underline : FontStyle.Regular))
                {
                    e.Graphics.DrawString(text, f, br, ClientRectangle, sf);
                }

                if (image != null)
                {
                    int x = (this.RightToLeft != RightToLeft.Yes) ? LogicalToDeviceUnits(2) : this.Width - LogicalToDeviceUnits(18);
                    int y = LogicalToDeviceUnits(2), size = LogicalToDeviceUnits(16);

                    if (this.Enabled)
                        e.Graphics.DrawImage(image, x, y, size, size);
                    else
                        Images.DrawGrayedImage(e.Graphics, image, x, y, size, size);
                }
            }
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
            finally
            {
                sf.Dispose();
            }
		}

		private void ucNavCmdButtons_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (_activehover.IntersectsWith(new Rectangle(e.Location,new Size(1,1))))
			{
				if (_underline == false) 
				{
                    _underline = !ModernStyle;
					this.Cursor = Cursors.Hand;
					this.Invalidate();
				}
			}
			else
			{
				if (_underline == true || ModernStyle) 
				{
					_underline = false;
					_mousedown = false;
					this.Invalidate();
					this.Cursor = Cursors.Default;
				}
			}
					
		}

        [DefaultValue(false)]
        public bool ModernStyle { get; set; }

        public new bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
                base.Visible = value;
            }
        }

		private void ucNavCmdButtons_MouseLeave(object sender, System.EventArgs e)
		{
			_underline = false;
			this.Invalidate();
		}

		private void ucNavCmdButtons_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if ((_underline  || ModernStyle) && e.Button == MouseButtons.Left && _mousedown == false)
			{
				_mousedown = true;
				this.Invalidate();
			}
		}

		private void ucNavCmdButtons_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if ((_underline || ModernStyle) && _mousedown)
			{
				OnLinkClicked(sender, e);
			}
			_mousedown = false;
			this.Invalidate();
		}
	}

	internal class IconDisplayEditor : UITypeEditor
	{
		public IconDisplayEditor()
		{
		}

		public override bool GetPaintValueSupported ( ITypeDescriptorContext ctx )
		{
			return true ;
		}

		public override void PaintValue ( PaintValueEventArgs e )
		{
			if (e != null && e.Context != null) 
			{
				// Find the ImageList property on the parent...
				//
				PropertyDescriptor imageProp = null;
				foreach(PropertyDescriptor pd in
					TypeDescriptor.GetProperties(e.Context.Instance)) 
				{
					if (typeof(ImageList).IsAssignableFrom(pd.PropertyType)) 
					{
						imageProp = pd;
						break;
					}
				}
				if (imageProp != null) 
				{
					try
					{
						ImageList imageList = (ImageList)imageProp.GetValue(e.Context.Instance);
						Image image = imageList.Images[Convert.ToInt32(Convert.ChangeType(e.Value,typeof(System.Int32)))];
						e.Graphics.DrawImage(image, e.Bounds, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
					}
					catch
					{}
				}
			}
		}
	}

}


