using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucTimeStat.
    /// </summary>
    public class ucTimeStat : System.Windows.Forms.UserControl
	{
		#region Fields
		private omsImageLists _omsimagelists = omsImageLists.None;
		private System.Windows.Forms.Label txtText;
		private System.Windows.Forms.Label txtPrice;
		private System.Windows.Forms.Label labPicture;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;
		#endregion

		#region Constructors


		public ucTimeStat()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            if (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
            {
                txtPrice.Dock = DockStyle.Left;
                labPicture.Dock = DockStyle.Right;
            }
            else
            {
                txtPrice.Dock = DockStyle.Right;
                labPicture.Dock = DockStyle.Left;
            }
            txtText.BringToFront();
        }

		public ucTimeStat(ImageList imagelist, int imageindex, string text, string price, string help) : this()
		{
			this.ImageIndex = imageindex;
			this.ImageList = imagelist;
			this.Price = price;
			this.Text = text;
			toolTip1.SetToolTip(txtText,help);
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
            this.txtText = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.Label();
            this.labPicture = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // txtText
            // 
            this.txtText.AutoSize = true;
            this.txtText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtText.Location = new System.Drawing.Point(22, 0);
            this.txtText.Name = "txtText";
            this.txtText.Padding = new System.Windows.Forms.Padding(10, 0, 2, 0);
            this.txtText.Size = new System.Drawing.Size(40, 13);
            this.txtText.TabIndex = 2;
            this.txtText.Text = "Text";
            this.txtText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPrice
            // 
            this.txtPrice.AutoSize = true;
            this.txtPrice.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtPrice.Location = new System.Drawing.Point(135, 0);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Padding = new System.Windows.Forms.Padding(2, 0, 10, 0);
            this.txtPrice.Size = new System.Drawing.Size(73, 13);
            this.txtPrice.TabIndex = 3;
            this.txtPrice.Text = "£10,000.00";
            this.txtPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labPicture
            // 
            this.labPicture.AutoSize = true;
            this.labPicture.Dock = System.Windows.Forms.DockStyle.Left;
            this.labPicture.Location = new System.Drawing.Point(0, 0);
            this.labPicture.Name = "labPicture";
            this.labPicture.Size = new System.Drawing.Size(22, 13);
            this.labPicture.TabIndex = 1;
            this.labPicture.Text = "     ";
            this.labPicture.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labPicture.Visible = false;
            // 
            // ucTimeStat
            // 
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.labPicture);
            this.Name = "ucTimeStat";
            this.Size = new System.Drawing.Size(208, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Properties
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		public new string Text
		{
			get
			{
				return txtText.Text;
			}
			set
			{
				txtText.Text = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string Price
		{
			get
			{
				return txtPrice.Text;
			}
			set
			{
				txtPrice.Text = value;
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
				return labPicture.ImageIndex;
			}
			set
			{
				labPicture.ImageIndex = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public ImageList ImageList
		{
			get
			{
				return labPicture.ImageList;
			}
			set
			{
				labPicture.ImageList = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(omsImageLists.None)]
		public omsImageLists Resources
		{
			get
			{
				return _omsimagelists;
			}
			set
			{
				if (_omsimagelists != value)
				{
					switch (value)
					{
						case omsImageLists.AdminMenu16:
						{
							ImageList = Images.AdminMenu16();
							break;
						}
						case omsImageLists.AdminMenu32:
						{
							ImageList = Images.AdminMenu32();
							break;
						}
						case omsImageLists.Arrows:
						{
							ImageList = Images.Arrows;
							break;
						}
                        case omsImageLists.PlusMinus:
                        {
                            ImageList = Images.PlusMinus;
                            break;
                        }
						case omsImageLists.CoolButtons16:
						{
							ImageList = Images.CoolButtons16();
							break;
						}
						case omsImageLists.CoolButtons24:
						{
                            ImageList = Images.GetCoolButtons24();
							break;
						}
						case omsImageLists.Developments16:
						{
							ImageList = Images.Developments();
							break;
						}
						case omsImageLists.Entities16:
						{
							ImageList = Images.Entities();
							break;
						}
						case omsImageLists.Entities32:
						{
							ImageList = Images.Entities32();
							break;
						}
						case omsImageLists.imgFolderForms16:
						{
                            ImageList = Images.GetFolderFormsIcons(Images.IconSize.Size16);
							break;
						}
						case omsImageLists.imgFolderForms32:
						{
                            ImageList = Images.GetFolderFormsIcons(Images.IconSize.Size32);
							break;
						}
						case omsImageLists.None:
						{
							ImageList = null;
							break;
						}
					}
				}
				_omsimagelists = value;
			}
		}
		#endregion
	}
}
