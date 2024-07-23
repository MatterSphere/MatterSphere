using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A class that uses a picture box control for displaying bitmaps in a data grid, a new DataGridColumnStyle is needed.
    /// </summary>
    public class DataGridImageColumn:DataGridColumnStyle 
	{
		#region Fields
		/// <summary>
		/// The image list that the column will look at.
		/// </summary>
		private ImageList _imgList = null;
		/// <summary>
		/// The image column name.
		/// </summary>
		private string _imageColumn = "";
		/// <summary>
		/// Retains the Resource State
		/// </summary>
		private omsImageLists _omsimagelists = omsImageLists.None;
		private int _imageindex =-1;
        private Image _imagecache = null;
        private Color _backcolor = SystemColors.Window;
		private Color _forecolor = SystemColors.WindowText;


		#endregion

		#region Constructors
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_imagecache != null)
                    {
                        _imagecache.Dispose();
                        _imagecache = null;
                    }

                    if (_imgList != null)
                    {
                        _imgList.Dispose();
                        _imgList = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DataGridImageColumn()
		{
		}

		/// <summary>
		/// Creates a picture box column style and displays the contents of a data table 
		/// inside as a picture based on resources from a specified image list.
		/// </summary>
		/// <param name="imageList">The image list to work off.</param>
		public DataGridImageColumn(ImageList imageList) : this (imageList, "")
		{
		}

		/// <summary>
		/// Creates a picture box column style and displays the contents of a data table 
		/// inside as a picture based on resources from a specified image list.
		/// </summary>
		/// <param name="imageList">The image list to work off.</param>
		/// <param name="boundColumn">The underlying column to bind the image to.</param>
		public DataGridImageColumn(ImageList imageList, string boundColumn) 
		{
			_imgList = imageList;
			_imageColumn = boundColumn;
		}

		#endregion

		#region Overrides
		/// <summary>
		/// Read-only, so nothing must be aborted/
		/// </summary>
		protected override void Abort(int RowNum) 
		{
		}

		/// <summary>
		/// Read-only, so nothing must be committed.
		/// </summary>
		protected override bool Commit(CurrencyManager DataSource,int RowNum) 
		{
			return true;
		}

		/// <summary>
		/// Read-only, so nothing must could be edited
		/// </summary>
		protected override void Edit(CurrencyManager Source ,int Rownum,Rectangle Bounds, bool ReadOnly,string InstantText, bool CellIsVisible) 
		{
		}

		/// <summary>
		/// Returns the minimum height of the picture.
		/// </summary>
		protected override int GetMinimumHeight() 
		{
            return DataGridTableStyle.DataGrid.LogicalToDeviceUnits(16);
		}

		/// <summary>
		/// Returns the preferred height of the picture.
		/// </summary>
		protected override int GetPreferredHeight(Graphics g ,object Value) 
		{
            return DataGridTableStyle.DataGrid.LogicalToDeviceUnits(16);
        }

		/// <summary>
		/// Returns the preferred size of the picture.
		/// </summary>
		protected override Size GetPreferredSize(Graphics g, object Value) 
		{
            return DataGridTableStyle.DataGrid.LogicalToDeviceUnits(new Size(100, 16));
		}

		/// <summary>
		/// The paint method of the 'DataGridColumnStyle' class do the work. There 
		/// exist three overloaded versions of this method.
		/// </summary>
		protected override void Paint(Graphics g,Rectangle Bounds,CurrencyManager Source,int RowNum) 
		{
			Paint(g, Bounds, Source, RowNum, false);
		}

		protected override void Paint(Graphics g,Rectangle Bounds,CurrencyManager Source,int RowNum,bool AlignToRight) 
		{
			if (Source.Position == RowNum) 
			{
				_backcolor = SystemColors.ActiveCaption; 
				_forecolor = SystemColors.Window;
			}
			else 
			{
				_backcolor = SystemColors.Window;
				_forecolor = SystemColors.WindowText;
			}

            using (SolidBrush BackBrush = new SolidBrush(Color.White))
            {
                using (Brush ForeBrush = new SolidBrush(this.DataGridTableStyle.ForeColor))
                {
                    Paint(g, Bounds, Source, RowNum, BackBrush, ForeBrush, AlignToRight);
                }
            }
		}


		protected override void Paint(Graphics g,Rectangle Bounds,CurrencyManager Source,int RowNum, Brush BackBrush ,Brush ForeBrush ,bool AlignToRight) 
		{
			if (Source.Position == RowNum) 
			{
				_backcolor = SystemColors.ActiveCaption; 
				_forecolor = SystemColors.Window;
			}
			else 
			{
				_backcolor = SystemColors.Window;
				_forecolor = SystemColors.WindowText;
			}

            using (SolidBrush backBrush = new SolidBrush(_backcolor))
            {
                g.FillRectangle(backBrush, Bounds);
            }

            StringFormat format = new StringFormat(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center };
			if(AlignToRight) format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

            if (_imgList != null)
            {
                PaintImage(g, ref Bounds, AlignToRight, RowNum);
            }

            using (SolidBrush foreBrush = new SolidBrush(_forecolor))
            {
                object val = GetColumnValueAtRow(Source, RowNum);
                g.DrawString(GetText(val), this.DataGridTableStyle.DataGrid.Font, foreBrush, Bounds, format);
            }
			format.Dispose();
		}

        private void PaintImage(Graphics g, ref Rectangle Bounds, bool AlignToRight, int RowNum)
        {
            int imageSize = DataGridTableStyle.DataGrid.LogicalToDeviceUnits(16);
            int textOffset = DataGridTableStyle.DataGrid.LogicalToDeviceUnits(20);

            int imageIndex = -1;
            if (!string.IsNullOrEmpty(_imageColumn))
            {
                DataTable datsrc = this.DataGridTableStyle.DataGrid.DataSource as DataTable;
                if (datsrc != null)
                {
                    try
                    {
                        imageIndex = FWBS.Common.ConvertDef.ToInt32(datsrc.DefaultView[RowNum][_imageColumn], _imageindex);
                    }
                    catch { }
                }
            }
            else
            {
                imageIndex = _imageindex;
            }

            Rectangle rect;
            if (AlignToRight)
            {
                rect = new Rectangle(Bounds.Right - textOffset + (textOffset - imageSize) / 2, Bounds.Top + (Bounds.Height - imageSize) / 2, imageSize, imageSize);
                Bounds = Rectangle.FromLTRB(Bounds.Left, Bounds.Top, Bounds.Right - textOffset, Bounds.Bottom);
            }
            else
            {
                rect = new Rectangle(Bounds.Left + (textOffset - imageSize) / 2, Bounds.Top + (Bounds.Height - imageSize) / 2, imageSize, imageSize);
                Bounds = Rectangle.FromLTRB(Bounds.Left + textOffset, Bounds.Top, Bounds.Right, Bounds.Bottom);
            }

            if (imageIndex >= 0 && imageIndex < _imgList.Images.Count)
            {
                if (imageSize == _imgList.ImageSize.Width)
                {
                    _imgList.Draw(g, rect.Location, imageIndex);
                }
                else if (_imagecache == null || Convert.ToInt32(_imagecache.Tag) != imageIndex)
                {
                    _imagecache?.Dispose();
                    _imagecache = _imgList.Images[imageIndex];
                    _imagecache.Tag = imageIndex;
                    g.DrawImage(_imagecache, rect);
                }
                else
                {
                    g.DrawImage(_imagecache, rect);
                }
            }
        }

		#endregion

		#region Private
		/// <summary>
		/// Returns a string value from and object and returns NullText of the DataGrid
		/// if the value is DBNull.
		/// </summary>
		/// <param name="Value">Value to be converted to a string representation.</param>
		/// <returns>String object.</returns>
		private string GetText(object Value)
		{
			object newvalue = Value;
			if(Value==System.DBNull.Value)
			{
				return NullText;
			}
			if (Value!=null)
			{
				if (Value is Boolean)
				{
					if (Convert.ToBoolean(Value))
						newvalue = FWBS.OMS.Session.CurrentSession.Resources.GetResource("YES","Yes","").Text;
					else
						newvalue = FWBS.OMS.Session.CurrentSession.Resources.GetResource("NO","No","").Text;
				}
				
				if (this.Format != "")
					return System.String.Format(this.FormatInfo,"{0:" + this.Format + "}",newvalue);
				else
					return Convert.ToString(newvalue);
			}
			else
			{
				return string.Empty;
			}
		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets the bound image list associated with the column.
		/// </summary>
		public ImageList ImageList
		{
			get
			{
				return _imgList;
			}
			set
			{
                _imgList = value;
                if (_imagecache != null)
                {
                    _imagecache.Dispose();
                    _imagecache = null;
                }
			}
		}


		[Category("Appearance")]
		[DefaultValue(omsImageLists.None)]
		[RefreshProperties(RefreshProperties.All)]
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
			}
		}
		
		/// <summary>
		/// Gets or Sets the bound image column to map to an image index.
		/// </summary>
		public string ImageColumn
		{
			get
			{
				return _imageColumn;
			}
			set
			{
				_imageColumn = value;
			}
		}

		private string _format = "";
		public string Format
		{
			get
			{
				return _format;
			}
			set
			{
				_format = value;
			}
		}

		private IFormatProvider _formatinfo = null;
		public IFormatProvider FormatInfo
		{
			get
			{
				return _formatinfo;
			}
			set
			{
				_formatinfo = value;
			}
		}

		#endregion
	}
}