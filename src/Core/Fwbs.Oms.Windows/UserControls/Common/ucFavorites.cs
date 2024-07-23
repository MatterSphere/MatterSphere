using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// Summary description for ucFavorites.
    /// </summary>

    public class ucFavorites : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.PictureBox picFavorite;
		static ImageList _imagelist;
		private string _codelookup = "";
		private string _mcid = "";
		private int _imageindex =-1;
		private int _clicked =0;
		private System.Windows.Forms.LinkLabel lnkLabel;
		private System.Windows.Forms.Panel panel1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ucFavorites()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

		}

		public ucFavorites(ImageList imagelist, int imageindex, string linklabel, string mcid, int ClickCount, string CodeLookup)
		{
			InitializeComponent();
			_clicked = ClickCount;
			ImageList = imagelist;
			ImageIndex = imageindex;
			LinkLabel = linklabel;
			_mcid = mcid;
			this.Dock = DockStyle.Top;
			_codelookup = CodeLookup;
		}

		public event EventHandler LinkClicked;
		public event ButtonEventHandler ButtonClick;

		protected virtual void OnButtonClick(MenuButtonEventArgs e) 
		{
			if (ButtonClick != null)
				ButtonClick(this, e);
		}

		protected virtual void OnLinkClicked(EventArgs e) 
		{
			if (LinkClicked != null)
				LinkClicked(this, e);
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

		
		[Category("Appearance")]
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
				if (_imageindex > -1 && _imagelist != null)
				{
					Console.WriteLine(_imagelist.ToString() + " " + _imageindex.ToString());
					picFavorite.Image = _imagelist.Images[_imageindex];
				}
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
				if (_imageindex > -1 && _imagelist != null)
				{
					Console.WriteLine(_imagelist.ToString() + " " + _imageindex.ToString());
					picFavorite.Image = _imagelist.Images[_imageindex];
				}
			}
		}

		public class IconDisplayEditor : UITypeEditor
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
				if (_imagelist != null && (int)e.Value > -1)
					_imagelist.Draw(e.Graphics,e.Bounds.Left,e.Bounds.Top,e.Bounds.Width,e.Bounds.Height,(int)e.Value);
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string LinkLabel
		{
			get
			{
				return lnkLabel.Text;
			}
			set
			{
				lnkLabel.Text = value;
			}
		}

		[Category("Buttons")]
		[DefaultValue("")]
		public string MenuCollectionID
		{
			get
			{
				return _mcid;
			}
			set
			{
				_mcid = value;
			}
		}

		[Category("Other")]
		[DefaultValue(0)]
		public int ClickCount
		{
			get
			{
				return _clicked;
			}
			set
			{
				_clicked = value;
			}
		}		

		[Browsable(false)]
		public string CodeLookup
		{
			get
			{
				return _codelookup;
			}
		}		

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.picFavorite = new System.Windows.Forms.PictureBox();
			this.lnkLabel = new System.Windows.Forms.LinkLabel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// picFavorite
			// 
			this.picFavorite.Dock = System.Windows.Forms.DockStyle.Left;
			this.picFavorite.Location = new System.Drawing.Point(4, 4);
			this.picFavorite.Name = "picFavorite";
			this.picFavorite.Size = new System.Drawing.Size(16, 16);
			this.picFavorite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picFavorite.TabIndex = 7;
			this.picFavorite.TabStop = false;
			// 
			// lnkLabel
			// 
			this.lnkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lnkLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lnkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.lnkLabel.LinkColor = System.Drawing.Color.Navy;
			this.lnkLabel.Location = new System.Drawing.Point(23, 4);
			this.lnkLabel.Name = "lnkLabel";
			this.lnkLabel.Size = new System.Drawing.Size(209, 16);
			this.lnkLabel.TabIndex = 10;
			this.lnkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lnkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLabel_LinkClicked);
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(20, 4);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(3, 16);
			this.panel1.TabIndex = 11;
			// 
			// ucFavorites
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lnkLabel,
																		  this.panel1,
																		  this.picFavorite});
			this.DockPadding.All = 4;
			this.Name = "ucFavorites";
			this.Size = new System.Drawing.Size(236, 24);
			this.ResumeLayout(false);

		}
		#endregion

		private void lnkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			_clicked++;
			OnLinkClicked(EventArgs.Empty);
			MenuButtonEventArgs ee = new MenuButtonEventArgs(_mcid,lnkLabel.Text,_imageindex,true,_codelookup);
			OnButtonClick(ee);		
		}
	}
}


