using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using FWBS.OMS.SearchEngine;
using FWBS.OMS.UI.Windows;

namespace FWBS.Common.UI.Windows
{

    /// <summary>
    /// Data Grid Label column style.
    /// </summary>
    public class DataGridLabelColumn: DataGridColumnStyle
	{
		#region Fields

		/// <summary>
		/// X coordinate Offset for the positioning of the control within the cell.
		/// </summary>
		private int _xMargin = 2;

		private Color _backcolor = SystemColors.Window;
		private Color _forecolor = SystemColors.WindowText;


		/// <summary>
		/// *******************************************************************
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
        private Image pic = null;
        private SearchColumnsDateIs _sourceis = SearchColumnsDateIs.NotApplicable;
        private SearchColumnsDateIs _displayas = SearchColumnsDateIs.NotApplicable;

		/// ******************************************************************

		/// <summary>
		/// Create a Data List from the DataListName Property
		/// </summary>
		private FWBS.OMS.EnquiryEngine.DataLists _datalist = null;

		/// <summary>
		/// The Name of the DataList
		/// </summary>
		private string _datalistname = "";

		/// <summary>
		/// The Name of the Data Code Lookup Type
		/// </summary>
		private string _datacodetype = "";

		/// <summary>
		/// The Parent Search List object for the Column
		/// </summary>
		private FWBS.OMS.SearchEngine.SearchList _searchlist = null;

		/// <summary>
		/// Contains the Data Table for the Data List;
		/// </summary>
		private DataTable _run = null;
		
		/// <summary>
		/// Allows multi select.
		/// </summary>
		private bool _multiselect = false;

        private CultureInfo cultureInfo;

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
                    if (pic != null)
                    {
                        pic.Dispose();
                        pic = null;
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
		/// Creates a new memo box column style.
		/// </summary>
		public DataGridLabelColumn()
		{
            cultureInfo = OMS.Session.CurrentSession.DefaultCultureInfo;
		}
			
							
		#endregion

		#region Column Style Override Methods

		public override bool ReadOnly
		{
			get
			{
				return true;
			}
			set
			{
				
			}
		}


		/// <summary>
		/// Aborts the specified row edit.
		/// </summary>
		/// <param name="RowNum">Row number to abort.</param>
		protected override void Abort(int RowNum)
		{
					
		}

		/// <summary>
		/// Commits the new value to the data source.
		/// </summary>
		/// <param name="dataSource">Data source to commit to the new value to.</param>
		/// <param name="rowNum">Row number to update.</param>
		/// <returns>Commit success flag.</returns>
		protected override bool Commit(CurrencyManager dataSource,int rowNum)
		{
			return false;
		}

		/// <summary>
		/// Makes the memo box  invisible when the column has lost focus.
		/// </summary>
		protected override void ConcedeFocus()
		{
		}

		
		/// <summary>
		/// Called when the current column on a specified row is being edited.  This allows
		/// for the use of showing the memo box and setting its text to the value being edited.
		/// </summary>
		/// <param name="source">Data source being edited.</param>
		/// <param name="rowNum">Row number being edited.</param>
		/// <param name="bounds">Graphical bounds of the grid item.</param>
		/// <param name="readOnly">Specifies whether the column is read only.</param>
		/// <param name="instantText">Text to be set if no other value is specified.</param>
		/// <param name="cellIsVisible">Specified whether the column is to be displayed or not.</param>
		protected override void Edit(CurrencyManager source ,int rowNum, Rectangle bounds, bool readOnly,string instantText, bool cellIsVisible)
		{
		}

		/// <summary>
		/// Returns the minimum height of the column / memo box.
		/// </summary>
		/// <returns>Integer height value.</returns>
		protected override int GetMinimumHeight()
		{
            return DataGridTableStyle.DataGrid.LogicalToDeviceUnits(18);
		}

		/// <summary>
		/// Returns the default preferred height of the column / memo box.
		/// </summary>
		/// <param name="g">Graphical object of the column.</param>
		/// <param name="Value">Current value set in the graphical space.</param>
		/// <returns>Integer height value.</returns>
		protected override int GetPreferredHeight(Graphics g ,object Value)
		{
            return DataGridTableStyle.DataGrid.LogicalToDeviceUnits(18);
		}

		/// <summary>
		/// Returns the default preferred size of the column.
		/// </summary>
		/// <param name="g">Graphical object of the column.</param>
		/// <param name="Value">Current value set in the graphical space.</param>
		/// <returns>Size object.</returns>
		protected override Size GetPreferredSize(Graphics g, object Value)
		{
			Size Extents = Size.Ceiling(g.MeasureString(GetText(Value), this.DataGridTableStyle.DataGrid.Font));
            int extraWidth = _xMargin * 2 + DataGridTableGridLineWidth;
            if (_imageindex != -1 || _imageColumn != "") extraWidth += 16;
            Extents.Width += DataGridTableStyle.DataGrid.LogicalToDeviceUnits(extraWidth);
			Extents.Height = System.Math.Max(Extents.Height, GetMinimumHeight());
			return Extents;
		}

		protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum)
		{
			Paint(g, bounds, source, rowNum, false);
		}

		protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, bool alignToRight)
		{
			string Text = GetText(GetColumnValueAtRow(source, rowNum));
			PaintText(g, bounds, Text, alignToRight, source, rowNum);
		}

		protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source,int rowNum, Brush backBrush, Brush foreBrush , bool alignToRight)
		{
            var columnColors = GetColumnColours();

			if (_multiselect == false)
			{
				if (source.Position == rowNum) 
				{
                    _backcolor = columnColors.SingleSelectedRow.SelectedRowBackColor;
                    _forecolor = columnColors.SingleSelectedRow.SelectedRowForeColor;
				}
				else 
				{
                    _backcolor = columnColors.SingleSelectedRow.UnselectedRowBackColor;
                    _forecolor = columnColors.SingleSelectedRow.UnselectedRowForeColor;
				}
			}
			else
			{
				if (this.DataGridTableStyle.DataGrid.IsSelected(rowNum) || source.Position == rowNum)
				{
					if (source.Position == rowNum)
					{
                        _backcolor = columnColors.MultiSelectedRows.LastSelectedRowBackColor;
                        _forecolor = columnColors.MultiSelectedRows.LastSelectedRowForeColor;
					}
					else
					{
                        _backcolor = columnColors.MultiSelectedRows.SelectedRowsBackColor;
                        _forecolor = columnColors.MultiSelectedRows.SelectedRowsForeColor;
					}
				}
				else 
				{
                    _backcolor = columnColors.MultiSelectedRows.UnselectedRowsBackColor;
                    _forecolor = columnColors.MultiSelectedRows.UnselectedRowsForeColor;
				}
			}

            string Text = GetText(GetColumnValueAtRow(source, rowNum));
			PaintText(g, bounds, Text, backBrush, foreBrush, alignToRight, source, rowNum);
		}


        private ColumnColours GetColumnColours()
        {
            return new ColumnColours(new Version2SingleSelectedRowColors()
                                     , new Version2MultiSelectedRowColors());
        }
		
		#endregion

        internal event EventHandler<CellDisplayEventArgs> BeforeCellDisplayEvent;

        internal CellDisplayEventArgs OnBeforeCellDisplayEvent(int rownum, CurrencyManager source, string text, string columnname)
        {
            EventHandler<CellDisplayEventArgs> ev = BeforeCellDisplayEvent;
            if (ev != null)
            {
                DataView dv = source.List as DataView;
                if (dv != null)
                {
                    CellDisplayEventArgs e = new CellDisplayEventArgs(rownum, dv[rownum], text, columnname);
                    ev(this, e);
                    return e;
                }
            }
            return null;
        }
	
		#region Methods
        private void PaintText(Graphics g, Rectangle bounds, string text, bool alignToRight, CurrencyManager Source, int RowNum)
        {
            using (Brush BackBrush = new SolidBrush(this.DataGridTableStyle.BackColor))
            {
                using (Brush ForeBrush = new SolidBrush(this.DataGridTableStyle.ForeColor))
                {
                    PaintText(g, bounds, text, BackBrush, ForeBrush, alignToRight, Source, RowNum);
                }
            }
        }

		private void PaintText(Graphics g , Rectangle Bounds, string Text, Brush BackBrush,Brush ForeBrush,bool AlignToRight, CurrencyManager Source, int RowNum)
		{
            Color backcolor = _backcolor;
            Color forecolor = _forecolor;

            CellDisplayEventArgs ee = OnBeforeCellDisplayEvent(RowNum, Source, Text, this.MappingName);
            if (ee != null)
            {
                if (Source.Position != RowNum && (this.DataGridTableStyle.DataGrid.IsSelected(RowNum) == false || _multiselect == false))
                {
                    if (ee.BackColor != Color.Empty)
                        backcolor = ee.BackColor;
                    if (ee.ForeColor != Color.Empty)
                        forecolor = ee.ForeColor;
                }
                Text = ee.Text;
            }

            using (SolidBrush backBrush = new SolidBrush(backcolor))
            {
                g.FillRectangle(backBrush, Bounds);
            }

            StringFormat format = new StringFormat(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center };
            if (AlignToRight) format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

            if (_imgList != null && (_imageindex != -1 || _imageColumn != ""))
            {
                PaintImage(g, ref Bounds, AlignToRight, RowNum);
            }
            else
            {
                switch (this.Alignment)
                {
                    case HorizontalAlignment.Left:
                        format.Alignment = StringAlignment.Near;
                        break;
                    case HorizontalAlignment.Right:
                        format.Alignment = StringAlignment.Far;
                        break;
                    case HorizontalAlignment.Center:
                        format.Alignment = StringAlignment.Center;
                        break;
                }
            }

            using (SolidBrush foreBrush = new SolidBrush(forecolor))
            {
                g.DrawString(Text, this.DataGridTableStyle.DataGrid.Font, foreBrush, Bounds, format);
            }
            format.Dispose();
		}

        private void PaintImage(Graphics g, ref Rectangle Bounds, bool AlignToRight, int RowNum)
        {
            int imageSize = DataGridTableStyle.DataGrid.LogicalToDeviceUnits(16);
            int textOffset = DataGridTableStyle.DataGrid.LogicalToDeviceUnits(20);

            int imageIndex = -1;
            if (_imageColumn != string.Empty)
            {
                DataTable datsrc = this.DataGridTableStyle.DataGrid.DataSource as DataTable;
                if (datsrc != null)
                {
                    try
                    {
                        object idx = datsrc.DefaultView[RowNum][_imageColumn];
                        if (idx != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(idx as string))
                            {
                                imageIndex = _imgList.Images.IndexOfKey((string)idx);
                            }
                            else if (idx is int)
                            {
                                imageIndex = (int)idx;
                            }
                        }
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
                else if (pic == null || Convert.ToInt32(pic.Tag) != imageIndex)
                {
                    pic?.Dispose();
                    pic = _imgList.Images[imageIndex];
                    pic.Tag = imageIndex;
                    g.DrawImage(pic, rect);
                }
                else
                {
                    g.DrawImage(pic, rect);
                }
            }
        }

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
                //UTCFIX: DM - 27/11/06 - Make sure that a UTC date is converted to local time for displaying to user.

				if (Value is Boolean)
				{
					if (Convert.ToBoolean(Value))
						newvalue = FWBS.OMS.Session.CurrentSession.Resources.GetResource("SLYES","Yes","").Text;
					else
						newvalue = FWBS.OMS.Session.CurrentSession.Resources.GetResource("SLNO","No","").Text;
				}
				
				if (_datalist != null)
				{
					string _filter = "";
					try
					{
						if (_run == null)
						{
							_run = _datalist.Run() as DataTable;
							if (_run == null) _datalist = null;
						}
						if (_run != null)
						{
                            try
                            {
                                _filter = _run.Columns[0].ColumnName + " = " + Convert.ToString(Value);
                                _run.DefaultView.RowFilter = _filter;
                            }
                            catch
                            {
                                _filter = _run.Columns[0].ColumnName + " = '" + Convert.ToString(Value) + "'";
                                _run.DefaultView.RowFilter = _filter;
                            }
							if (_run.DefaultView.Count > 0)
								newvalue = _run.DefaultView[0][1];
						}
					}
					catch (Exception ex)
					{
						ErrorBox.Show(new FWBS.OMS.OMSException2("ERRBADCOLDATA1", "Error using DataList '%1%' on Column '%2%'. Applying Filter '%3%'",ex,true,_datalistname,this.MappingName,_filter));
						_datalist = null;
					}
				}
				else if (_datacodetype != "" && _run != null)
				{
					string _filter = "";
					try
					{
						_filter = "cdCode = '" + Convert.ToString(Value) + "'";
						_run.DefaultView.RowFilter = _filter;
						if (_run.DefaultView.Count > 0)
							newvalue = _run.DefaultView[0]["cddesc"];
					}
					catch (Exception ex)
					{
						ErrorBox.Show(new FWBS.OMS.OMSException2("ERRBADCOLDATA2", "Error using Code Lookup Type '%1%' on Column '%2%'. Applying Filter '%3%'",ex,true,_datacodetype,this.MappingName,_filter));
						_datalist = null;
					}

				}

                if (_displayas == SearchColumnsDateIs.Local)
                {
                    if (Value is DateTime)
                    {
                        DateTime datedata = (DateTime)Value;
                        if (datedata.Kind == DateTimeKind.Unspecified)
                            newvalue = datedata;
                        else
                            newvalue = datedata.ToLocalTime();
                    }
					return System.String.Format(cultureInfo,"{0:" + this.Format + "}",newvalue);
                }
                else if (_displayas == SearchColumnsDateIs.UTC)
                {
                    if (Value is DateTime)
                    {
                        DateTime datedata = (DateTime)Value;
                        if (datedata.Kind == DateTimeKind.Unspecified)
                            newvalue = datedata;
                        else
                            newvalue = datedata.ToUniversalTime();
                    }
					return System.String.Format(cultureInfo, "{0:" + this.Format + "}",newvalue) + " (UTC)";
                }
                else if (this.Format != "")
                {
                    var ci = this.Format.Equals("c", StringComparison.OrdinalIgnoreCase) ? FormatInfo : cultureInfo;                    
                    return System.String.Format(ci, "{0:" + this.Format + "}", newvalue);
                }
                else
                    return Convert.ToString(newvalue, cultureInfo);
			}
			else
			{
				return string.Empty;
			}
		}
		#endregion

		#region Properties
        public DataTable DataListTable
        {
            get
            {
                if (_datalist != null)
                {
                    if (_run == null)
                    {
                        _run = _datalist.Run() as DataTable;
                        if (_run == null) _datalist = null;
                    }
                }
                return _run;
            }
        }

        public SearchColumnsDateIs SourceDateIs
        {
            get
            {
                return _sourceis;
            }
            set
            {
                _sourceis = value;
            }
        }

        public SearchColumnsDateIs DisplayDateAs
        {
            get
            {
                return _displayas;
            }
            set
            {
                _displayas = value;
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

		/// <summary>
		/// Gets the grid line width depending on the grid table style line style.
		/// </summary>
		private int DataGridTableGridLineWidth
		{
			get
			{
                return (this.DataGridTableStyle.GridLineStyle == DataGridLineStyle.Solid) ? 1 : 0;
			}
		}

		/// ******************************************************************************************
		/// The Image Icon Properties
		/// ******************************************************************************************

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
                if (pic != null)
                {
                    pic.Dispose();
                    pic = null;
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

		/// ******************************************************************************************
		/// ******************************************************************************************
		/// ******************************************************************************************

		[Category("Data")]
		[DefaultValue("")]
		public string DataListName
		{
			get
			{
				return _datalistname;
			}
			set
			{
				if (_datalistname != value)
				{
					_datalistname = value;
					try
					{
						_datacodetype = "";
						_datalist = new FWBS.OMS.EnquiryEngine.DataLists(_datalistname);
						_datalist.ChangeParent(_searchlist.Parent);
					}
					catch (Exception ex)
					{
						_datalistname = "";
						_datalist = null;
						_run = null;
						ErrorBox.Show(new FWBS.OMS.OMSException2("ERRINVDATALS","Error invalid Data List name '%1%'",ex,true,_datalistname));
					}
				}
			}
		}

		[Category("Data")]
		[DefaultValue("")]
		public string DataCodeType
		{
			get
			{
				return _datacodetype;
			}
			set
			{
				try
				{
					if (_datacodetype != value)
					{
						_datalistname = "";
						_datacodetype = value;
						_run = FWBS.OMS.CodeLookup.GetLookups(_datacodetype);
					}
				}
				catch (Exception ex)
				{
					_datacodetype = "";
					_run = null;
					ErrorBox.Show(new FWBS.OMS.OMSException2("ERRINVCODETYP","Error invalid code lookup type name '%1%'",ex,true,_datacodetype));
				}
			}
		}

		[Browsable(false)]
		public FWBS.OMS.SearchEngine.SearchList SearchList
		{
			get
			{
				return _searchlist;
			}
			set
			{
				_searchlist = value;
			}
		}

		public bool AllowMultiSelect
		{
			get
			{
				return _multiselect;
			}
			set
			{
				_multiselect = value;
			}
		}
		#endregion
	}
}
