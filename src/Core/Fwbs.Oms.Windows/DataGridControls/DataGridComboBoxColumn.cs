using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// DataGrid ComboBox object.
    /// </summary>
    public class DataGridComboBox : ComboBox
	{
	}

	/// <summary>
	/// Data Grid ComboBox column style.
	/// </summary>
	public class DataGridComboBoxColumn	: DataGridColumnStyle
	{
		#region Fields
		
		/// <summary>
		/// Manipulated combo box control.
		/// </summary>
		private DataGridComboBox _combo;

		/// <summary>
		/// Data table or array to hold the combo boxes contents.
		/// </summary>
		private object dt;

		/// <summary>
		/// X coordinate Offset for the positioning of the control within the cell.
		/// </summary>
		private int _xMargin = 2;

		/// <summary>
		/// Y coordinate Offset for the positioning of the control within the cell.
		/// </summary>
		private int _yMargin = 1;

		/// <summary>
		/// Original value that the combo box was assigned to.
		/// </summary>
		private object _oldValue = System.DBNull.Value;

		/// <summary>
		/// Current edit mode flag.
		/// </summary>
		private bool _inEdit = false;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new combo box column style.
		/// </summary>
		public DataGridComboBoxColumn()
		{
			//Create the combo box and capture the events assigned to below.
			_combo = new DataGridComboBox();
			_combo.SelectionChangeCommitted += new EventHandler(this.ChangeEvent);
			_combo.DataSourceChanged += new EventHandler(this.DataSourceEvent);
			_combo.Visible = false;
		}
			
		/// <summary>
		/// Creates a combo box column style and displays the contents of a data table 
		/// inside a combo box using column ordinal values.
		/// </summary>
		/// <param name="DataSource">Dat table as the list.</param>
		/// <param name="DisplayMember">Display column ordinal value.</param>
		/// <param name="ValueMember">Display column ordinal value.</param>
		public DataGridComboBoxColumn(DataTable dataSource, int valueMember, int displayMember) : this(dataSource, dataSource.Columns[valueMember].ColumnName, dataSource.Columns[displayMember].ColumnName)
		{
		}

						
		/// <summary>
		/// Creates a combo box column style and displays the contents of a data table 
		/// inside a combo box using column names.
		/// </summary>
		/// <param name="DataSource">Data table as the list.</param>
		/// <param name="DisplayMember">Bound column name.</param>
		/// <param name="ValueMember">Display column name.</param>
		public DataGridComboBoxColumn(DataTable dataSource,string valueMember, string displayMember)
		{
			_combo = new DataGridComboBox();
			_combo.SelectionChangeCommitted += new EventHandler(this.ChangeEvent);
			_combo.DataSourceChanged += new EventHandler(this.DataSourceEvent);
			_combo.Visible = false;
			_combo.DataSource = dataSource;
			_combo.DisplayMember = displayMember;
			_combo.ValueMember = valueMember;
			dt = dataSource;
		}

		/// <summary>
		/// Creates a combo box column style and displays the contents of an array list
		/// inside a combo box.
		/// </summary>
		/// <param name="array">Array list to display.</param>
		/// <param name="DisplayMember">Bound column name.</param>
		/// <param name="ValueMember">Display column name.</param>
		public DataGridComboBoxColumn(object [] array, string valueMember, string displayMember)
		{
			_combo = new DataGridComboBox();
			_combo.SelectionChangeCommitted += new EventHandler(this.ChangeEvent);
			_combo.DataSourceChanged += new EventHandler(this.DataSourceEvent);
			_combo.Visible = false;
			_combo.DataSource = array;
			_combo.DisplayMember = displayMember;
			_combo.ValueMember = valueMember;
			dt = array;
		}

		#endregion

		#region Column Style Override Methods

		/// <summary>
		/// Aborts the specified row edit.
		/// </summary>
		/// <param name="RowNum">Row number to abort.</param>
		protected override void Abort(int RowNum)
		{
			//Rollback to the orignal value and hide the combo box.
			RollBack();
			HideComboBox();
			EndEdit();
		}

		/// <summary>
		/// Commits the new value to the data source.
		/// </summary>
		/// <param name="dataSource">Data source to commit to the new value to.</param>
		/// <param name="rowNum">Row number to update.</param>
		/// <returns>Commit success flag.</returns>
		protected override bool Commit(CurrencyManager dataSource,int rowNum)
		{
			HideComboBox();
			if(!_inEdit)
			{
				return true;
			}
			try
			{	
				object Value = _combo.SelectedValue;

				if(NullText.Equals(Value))
				{
					Value = System.Convert.DBNull; 
				}
				SetColumnValueAtRow(dataSource, rowNum, Value);
				
				return true;
			}
			catch
			{
				RollBack();
				return false;	
			}
			finally
			{
				this.EndEdit();
			}
			
		}

		/// <summary>
		/// Makes the combo box  invisible when the column has lost focus.
		/// </summary>
		protected override void ConcedeFocus()
		{
			_combo.Visible=false;
		}

		
		/// <summary>
		/// Called when the current column on a specified row is being edited.  This allows
		/// for the use of showing the combo box and setting its value to the value being edited.
		/// </summary>
		/// <param name="source">Data source being edited.</param>
		/// <param name="rowNum">Row number being edited.</param>
		/// <param name="bounds">Graphical bounds of the grid item.</param>
		/// <param name="readOnly">Specifies whether the column is read only.</param>
		/// <param name="instantText">Text to be set if no other value is specified.</param>
		/// <param name="cellIsVisible">Specified whether the column is to be displayed or not.</param>
		protected override void Edit(CurrencyManager source ,int rowNum, Rectangle bounds, bool readOnly,string instantText, bool cellIsVisible)
		{
		
			Rectangle OriginalBounds = bounds;
					
			object _newValue = GetColumnValueAtRow(source, rowNum);
			if (_newValue == null || _newValue == DBNull.Value)
				_newValue = _oldValue;
			else
				_oldValue = _newValue;

			if(cellIsVisible)
			{
				bounds.Offset(_xMargin, _yMargin);
				bounds.Width -= _xMargin * 2;
				bounds.Height -= _yMargin;
				_combo.Bounds = bounds;
				_combo.Visible = true;
			}
			else
			{
				_combo.Bounds = OriginalBounds;
				_combo.Visible = false;
			}
			
			if (_combo.SelectedValue != _oldValue)
			{
				_combo.SelectedValue = _oldValue;
			}

			if(instantText!=null)
			{
				_combo.SelectedValue = instantText;
			}

			//Right to left compatibility of the combo.
			_combo.RightToLeft = this.DataGridTableStyle.DataGrid.RightToLeft;
			
			if(instantText==null)
			{
				_combo.SelectAll();
				
			}
			else
			{
				int end = _combo.Text.Length;
				_combo.Select(end, 0);
			}
			
			if(_combo.Visible)
			{
				DataGridTableStyle.DataGrid.Invalidate(OriginalBounds);
			}

			_inEdit = true;

		}

		/// <summary>
		/// Returns the minimum height of the column / combo box.
		/// </summary>
		/// <returns>Integer height value.</returns>
		protected override int GetMinimumHeight()
		{
			return _combo.PreferredHeight + _yMargin;
		}

		/// <summary>
		/// Returns the default preferred height of the column / combo box.
		/// </summary>
		/// <param name="g">Graphical object of the column.</param>
		/// <param name="Value">Current value set in the graphical space.</param>
		/// <returns>Integer height value.</returns>
		protected override int GetPreferredHeight(Graphics g ,object Value)
		{
			return 18;
		}

		/// <summary>
		/// Returns the default preferred size of the column / combo box.
		/// </summary>
		/// <param name="g">Graphical object of the column.</param>
		/// <param name="Value">Current value set in the graphical space.</param>
		/// <returns>Size object.</returns>
		protected override Size GetPreferredSize(Graphics g, object Value)
		{
			Size Extents = Size.Ceiling(g.MeasureString(GetText(Value), this.DataGridTableStyle.DataGrid.Font));
			Extents.Width += _xMargin * 2 + DataGridTableGridLineWidth ;
			Extents.Height += _yMargin;
			return Extents;
		}

		//Paint Overrides.
		protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum)
		{
			Paint(g, bounds, source, rowNum, false);
		}

		protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, bool alignToRight)
		{
			string Text = GetText(GetColumnValueAtRow(source, rowNum));
			PaintText(g, bounds, Text, alignToRight);
		}

		protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source,int rowNum, Brush backBrush, Brush foreBrush , bool alignToRight)
		{
			string Text = GetText(GetColumnValueAtRow(source, rowNum));
			PaintText(g, bounds, Text, backBrush, foreBrush, alignToRight);
		}
		
		protected override void SetDataGridInColumn(DataGrid Value)
		{
			base.SetDataGridInColumn(Value);
			if(_combo.Parent!=Value)
			{
				if(_combo.Parent!=null)
				{
					_combo.Parent.Controls.Remove(_combo);
				}
			}
			if(Value!=null) 
			{
				Value.Controls.Add(_combo);
			}
		}
		
		protected override void UpdateUI(CurrencyManager Source,int RowNum, string InstantText)
		{
			_combo.SelectedValue = GetColumnValueAtRow(Source, RowNum);
			if(InstantText!=null)
			{
				_combo.SelectedValue = InstantText;
			}
		}			
		
	
		#endregion
	
		#region Methods

		private void PaintText(Graphics g ,Rectangle bounds,string text,bool alignToRight)
		{
            using (Brush BackBrush = new SolidBrush(this.DataGridTableStyle.BackColor))
            {
                using (Brush ForeBrush = new SolidBrush(this.DataGridTableStyle.ForeColor))
                {
                    PaintText(g, bounds, text, BackBrush, ForeBrush, alignToRight);
                }
            } 
		}

		private void PaintText(Graphics g , Rectangle TextBounds, string Text, Brush BackBrush,Brush ForeBrush,bool AlignToRight)
		{	
			Rectangle Rect = TextBounds;
			RectangleF RectF  = Rect; 
			StringFormat Format = new StringFormat();
			if(AlignToRight)
			{
				Format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
			}
			switch(this.Alignment)
			{
				case HorizontalAlignment.Left:
					if (this.DataGridTableStyle.DataGrid.RightToLeft == RightToLeft.No)
						Format.Alignment = StringAlignment.Near;
					else
						Format.Alignment = StringAlignment.Far;
					break;
				case HorizontalAlignment.Right:
					if (this.DataGridTableStyle.DataGrid.RightToLeft == RightToLeft.No)
						Format.Alignment = StringAlignment.Far;
					else
						Format.Alignment = StringAlignment.Near;
					break;
				case HorizontalAlignment.Center:
					Format.Alignment = StringAlignment.Center;
					break;
			}
			Format.FormatFlags |= StringFormatFlags.NoWrap;
			g.FillRectangle(BackBrush, Rect);
			Rect.Offset(0, _yMargin);
			Rect.Height -= _yMargin;
			RectF.Y += 2;
			//Display the longer text.
			if (dt != null)
			{
				if (dt is DataTable)
				{
					DataTable dat = (DataTable)dt;
					if (dat.Columns.Count > 1)
					{
						DataView vw = new DataView(dat);
						vw.RowFilter = dat.Columns[0].ColumnName + " = '" + Text.Replace("'", "''") + "'";
						if (vw.Count > 0)
							Text = vw[0].Row[1].ToString();
					}
				}
			}
			g.DrawString(Text, this.DataGridTableStyle.DataGrid.Font, ForeBrush, RectF, Format);
			Format.Dispose();
		}

		/// <summary>
		/// Ends the current edit by setting the internal flag to false and redrawing the column.
		/// </summary>
		private void EndEdit()
		{
			_inEdit = false;
			Invalidate();
		}

		/// <summary>
		/// Returns a string value from and object and returns NullText of the DataGrid
		/// if the value is DBNull.
		/// </summary>
		/// <param name="Value">Value to be converted to a string representation.</param>
		/// <returns>String object.</returns>
		private string GetText(object Value)
		{
			if(Value==System.DBNull.Value)
			{
				return NullText;
			}
			if(Value!=null)
			{
				return Value.ToString();
			}
			else
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Hides the combo box.
		/// </summary>
		private void HideComboBox()
		{
			if(_combo.Focused)
			{
				this.DataGridTableStyle.DataGrid.Focus();
			}
			_combo.Visible = false;
		}

		/// <summary>
		/// Sets the combo box back to its original value and ends the edit process.
		/// </summary>
		private void RollBack()
		{
			_combo.SelectedValue = _oldValue;
			EndEdit();
		}

		/// <summary>
		/// Captures the change event of the combo box.
		/// </summary>
		/// <param name="sender">The combo box raising this method.</param>
		/// <param name="e">Empty event arguments/</param>
		private void ChangeEvent (object sender, EventArgs e)
		{
			_oldValue = _combo.SelectedValue;
			ColumnStartedEditing(_combo);
		}

		/// <summary>
		/// Captures the Changed Data Source event of the combo box.
		/// </summary>
		/// <param name="sender">The combo box raising this method.</param>
		/// <param name="e">Empty event arguments/</param>
		private void DataSourceEvent (object sender, EventArgs e)
		{
			dt = _combo.DataSource;
		}


		#endregion

		#region Properties

		/// <summary>
		/// Gets the grid line width depending on the grid table style line style.
		/// </summary>
		private int DataGridTableGridLineWidth
		{
			get
			{
				if(this.DataGridTableStyle.GridLineStyle == DataGridLineStyle.Solid) 
				{ 
					return 1;
				}
				else
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// Gets a reference to the combo box being used within the grid.
		/// </summary>
		public DataGridComboBox ComboBox
		{
			get
			{
				return _combo;
			}
		}

		#endregion

	}
}

