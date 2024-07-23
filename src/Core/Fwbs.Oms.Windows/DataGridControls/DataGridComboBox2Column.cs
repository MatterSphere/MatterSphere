using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// DataGrid ComboBox2 object.
    /// </summary>
    public class DataGridComboBox2 : System.Windows.Forms.Panel
	{
		#region Events
		public event EventHandler DataSourceChanged = null;
		public event EventHandler SelectionChangeCommitted = null;

		public void OnDataSourceChanged(object sender,EventArgs e)
		{
			if (DataSourceChanged != null)
				DataSourceChanged(sender,e);
		}
		
		public void OnSelectionChangeCommitted(object sender,EventArgs e)
		{
			if (SelectionChangeCommitted != null)
				SelectionChangeCommitted(sender,e);
		}

		#endregion

		#region Fields

		/// <summary>
		/// Y coordinate Offset for the positioning of the control within the cell.
		/// </summary>
		private bool _keyskip = false;

		/// <summary>
		/// 
		/// </summary>
		private ButtonState _butstate = ButtonState.Normal;

		/// <summary>
		/// Internal text box control for editing.
		/// </summary>
		private DataGridTextBox _txt;
		/// <summary>
		/// Internal button control.
		/// </summary>
		private System.Windows.Forms.Button _btn;

		/// <summary>
		/// Data Grid control that this control belongs to.
		/// </summary>
		public DataGrid _grid;

		private FWBS.Common.UI.Windows.eComboBoxForm _dropdown = null;

		private object _preselect = null;

		private bool _enableautoclosedrop = true;

		#endregion

		#region Constuctors

		public DataGridComboBox2() : base()
		{
			_txt = new DataGridTextBox();
			_txt.BorderStyle = BorderStyle.None;
			Controls.Add(_txt);
			this.DockPadding.Top=1;
			this.DockPadding.Bottom=1;
			_dropdown = new eComboBoxForm(null);
			_dropdown.lstList.DataSourceChanged +=new EventHandler(OnDataSourceChanged);
			_dropdown.lstList.Click +=new EventHandler(lstList_Click);
			_dropdown.lstList.SelectedIndexChanged +=new EventHandler(lstList_SelectedIndexChanged);
			_dropdown.lstList.KeyUp +=new KeyEventHandler(lstList_KeyUp);
			_btn = new System.Windows.Forms.Button();
			_btn.Click += new EventHandler(this.ClickEvent);
			_dropdown.Deactivate +=new EventHandler(dropdown_Deactivate);
			Controls.Add(_btn);
			_btn.Paint +=new PaintEventHandler(btn_Paint);
			_txt.KeyPress +=new KeyPressEventHandler(DataGridComboBox2_KeyPress);
			_txt.KeyDown +=new KeyEventHandler(DataGridComboBox2_KeyDown);
			_btn.MouseUp +=new MouseEventHandler(btn_MouseUp);
			_btn.MouseDown +=new MouseEventHandler(btn_MouseDown);
			_btn.BackColor = SystemColors.Control;
			_btn.Size = new Size(16, 16);
			_btn.Dock = DockStyle.Right;
			_txt.Dock = DockStyle.Fill;
			SetRTL();
			this.RightToLeftChanged += new EventHandler(this.RightToLeftChangedEvent);
		}

		#endregion

		#region Properties
		[Browsable(false)]
		public bool EnableAutoDropDownClose
		{
			get
			{
				return _enableautoclosedrop;
			}
			set
			{
				_enableautoclosedrop = value;
			}
		}

		public FWBS.Common.UI.Windows.eComboBoxForm DropDown
		{
			get
			{
				return _dropdown;
			}
		}

		[Category("DATA")]
		public object DataSource
		{
			get
			{
				return _dropdown.lstList.DataSource;
			}
			set
			{
				_dropdown.lstList.DataSource = value;
			}
		}

		[Category("DATA")]
		public string DisplayMember
		{
			get
			{
				return _dropdown.lstList.DisplayMember;
			}
			set
			{
				_dropdown.lstList.DisplayMember = value;
			}
		}

		[Category("DATA")]
		public string ValueMember
		{
			get
			{
				return _dropdown.lstList.ValueMember;
			}
			set
			{
				_dropdown.lstList.ValueMember = value;
			}
		}

		[Category("DATA")]
		public object SelectedValue
		{
			get
			{
				return _dropdown.lstList.SelectedValue;
			}
			set
			{
				_dropdown.lstList.SelectedValue = value;
				_txt.Text = _dropdown.lstList.Text;
			}

		}
		
		[Category("DATA")]
		public int SelectedIndex
		{
			get
			{
				return _dropdown.lstList.SelectedIndex;
			}
			set
			{
				_dropdown.lstList.SelectedIndex = value;
				_txt.Text = _dropdown.lstList.Text;
			}
		}
		
		[Category("DATA")]
		public object SelectedItem
		{
			get
			{
				return _dropdown.lstList.SelectedItem;
			}
			set
			{
				_dropdown.lstList.SelectedItem = value;
				_txt.Text = _dropdown.lstList.Text;
			}
		}

		/// <summary>
		/// Gets and Sets the text of the memo control.
		/// </summary>
		public override string Text
		{
			get
			{
				return _txt.Text;
			}
			set
			{
				_txt.Text = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets the text boxes data grid parent to thes passed data grid.
		/// </summary>
		/// <param name="parentGrid">Parent data grid.</param>
		public void SetDataGrid(DataGrid parentGrid)
		{
			if (_grid == null)
			{
				_grid = parentGrid;
				_txt.SetDataGrid(parentGrid);
			}
		}

		/// <summary>
		/// Selects all the text within the internal text box.
		/// </summary>
		public void SelectAll()
		{
			_txt.Focus();
			_txt.SelectAll();
		}

		/// <summary>
		/// Selects a portion of text within the internal text box control.
		/// </summary>
		/// <param name="start">Starting index.</param>
		/// <param name="length">Length of text from the starting index.</param>
		public void Select(int start, int length)
		{
			_txt.Focus();
			_txt.Select(start, length);
		}

		/// <summary>
		/// Captures the click event of the corresponding button.
		/// </summary>
		/// <param name="sender">Button instance that raised the event.</param>
		/// <param name="e">Empty event arguments.</param>
		private void ClickEvent (object sender, EventArgs e)
		{
			if (_dropdown.Visible == false)
			{
				OnSelectionChangeCommitted(this,e);
				_preselect = this.SelectedValue;
				_dropdown.Show();
			}
			else
				_dropdown.Hide();
		}

		/// <summary>
		/// Captures the right to left property changed event.
		/// </summary>
		/// <param name="sender">Control that raises this event.</param>
		/// <param name="e">Empty event arguments.</param>
		private void RightToLeftChangedEvent (object sender, EventArgs e)
		{
			SetRTL();
		}

		/// <summary>
		/// Positions the conmtrols correctly when in Right to Left mode.
		/// </summary>
		private void SetRTL ()
		{
            FWBS.OMS.UI.Windows.Global.RightToLeftControlConverter(this, null);
		}

		#endregion

		#region Private
		private void btn_Paint(object sender, PaintEventArgs e)
		{
			System.Windows.Forms.ControlPaint.DrawComboButton(e.Graphics,e.ClipRectangle,_butstate);
		}

		private void btn_MouseDown(object sender, MouseEventArgs e)
		{
			_butstate = ButtonState.Pushed;
			_btn.Invalidate();
		}

		private void btn_MouseUp(object sender, MouseEventArgs e)
		{
			_butstate = ButtonState.Normal;
			_btn.Invalidate();
		}

		private void dropdown_Deactivate(object sender, EventArgs e)
		{
			if (_enableautoclosedrop)
				_dropdown.Hide();
		}

		private void lstList_Click(object sender, EventArgs e)
		{
			_txt.Text = _dropdown.lstList.Text;
			_dropdown.Hide();
			this.Invalidate();
			_preselect = this.SelectedValue;
		}


		private void DataGridComboBox2_Enter(object sender, EventArgs e)
		{
			_btn.Focus();
		}

		private void lstList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_dropdown.Visible)
			{
				_txt.Text = _dropdown.lstList.Text;
				OnSelectionChangeCommitted(this,e);
			}
		}

		private void lstList_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F4)
			{
				if (_keyskip)
				{
					_keyskip = false;
					return;
				}
				lstList_Click(sender,EventArgs.Empty);
			}
			else if (e.KeyCode == Keys.Escape)
			{
				this.SelectedValue = _preselect;
				_dropdown.Hide();
			}
		}

		private void DataGridComboBox2_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F4)
			{
				_keyskip = true;
				ClickEvent(sender,EventArgs.Empty);
			}
			else if (e.KeyCode != Keys.F4) 
			{
				e.Handled=true;
			}
		}

		private void DataGridComboBox2_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled=true;
		}
		#endregion
	}

	/// <summary>
	/// Data Grid ComboBox column style.
	/// </summary>
	public class DataGridComboBox2Column : DataGridColumnStyle
	{
		#region Fields
		
		/// <summary>
		/// Manipulated combo box control.
		/// </summary>
		private DataGridComboBox2 _combo;

		/// <summary>
		/// Data table or array to hold the combo boxes contents.
		/// </summary>
		private object dt;

		/// <summary>
		/// X coordinate Offset for the positioning of the control within the cell.
		/// </summary>
		private int _xMargin = 0;

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
		public DataGridComboBox2Column()
		{
			//Create the combo box and capture the events assigned to below.
			_combo = new DataGridComboBox2();
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
		public DataGridComboBox2Column(DataTable dataSource, int valueMember, int displayMember) : this(dataSource, dataSource.Columns[valueMember].ColumnName, dataSource.Columns[displayMember].ColumnName)
		{
		}

						
		/// <summary>
		/// Creates a combo box column style and displays the contents of a data table 
		/// inside a combo box using column names.
		/// </summary>
		/// <param name="DataSource">Data table as the list.</param>
		/// <param name="DisplayMember">Bound column name.</param>
		/// <param name="ValueMember">Display column name.</param>
		public DataGridComboBox2Column(DataTable dataSource,string valueMember, string displayMember)
		{
			_combo = new DataGridComboBox2();
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
		public DataGridComboBox2Column(object [] array, string valueMember, string displayMember)
		{
			_combo = new DataGridComboBox2();
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
		
			_combo.SetDataGrid(this.DataGridTableStyle.DataGrid);
			Rectangle OriginalBounds = bounds;
					
			object _newValue = GetColumnValueAtRow(source, rowNum);
			if (_newValue == null || _newValue == DBNull.Value)
				_newValue = _oldValue;
			else
				_oldValue = _newValue;

			if(cellIsVisible)
			{
				_combo.DropDown.Location = this.DataGridTableStyle.DataGrid.PointToScreen(bounds.Location);
				_combo.DropDown.Top += bounds.Height;
				_combo.DropDown.Width = bounds.Width;
				_combo.DropDown.Closing +=new CancelEventHandler(DropDown_Closing);


				bounds.Offset(_xMargin, _yMargin);
				bounds.Width -= _xMargin;
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
			return 18;
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
			PaintText(g, bounds, GetColumnValueAtRow(source, rowNum), alignToRight);
		}

		protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source,int rowNum, Brush backBrush, Brush foreBrush , bool alignToRight)
		{
			PaintText(g, bounds, GetColumnValueAtRow(source, rowNum), backBrush, foreBrush, alignToRight);
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

		private void PaintText(Graphics g ,Rectangle bounds,object text,bool alignToRight)
		{
            using (Brush BackBrush = new SolidBrush(this.DataGridTableStyle.BackColor))
            {
                using (Brush ForeBrush = new SolidBrush(this.DataGridTableStyle.ForeColor))
                {
                    PaintText(g, bounds, text, BackBrush, ForeBrush, alignToRight);
                }
            }
		}

		private void PaintText(Graphics g , Rectangle TextBounds, object Text, Brush BackBrush,Brush ForeBrush,bool AlignToRight)
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
			Format.FormatFlags =Format.FormatFlags;
			Format.FormatFlags =StringFormatFlags.NoWrap;
			g.FillRectangle(BackBrush, Rect);
			Rect.Offset(0, _yMargin);
			Rect.Height -= _yMargin;
			RectF.Y += 2;
			_combo.DropDown.lstList.SelectedValue = Text;
			if (Text != DBNull.Value && _combo.DropDown.lstList.SelectedIndex != -1)
				g.DrawString(_combo.DropDown.lstList.Text, this.DataGridTableStyle.DataGrid.Font, ForeBrush, RectF, Format);
			Format.Dispose();
		}

		/// <summary>
		/// Ends the current edit by setting the internal flag to false and redrawing the column.
		/// </summary>
		private void EndEdit()
		{
			_combo.DropDown.Closing -=new CancelEventHandler(DropDown_Closing);
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
			_combo.DropDown.Closing -=new CancelEventHandler(DropDown_Closing);
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
			_combo.EnableAutoDropDownClose = false;
			ColumnStartedEditing(_combo);
			_oldValue = _combo.SelectedValue;
			_combo.EnableAutoDropDownClose = true;
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
		[Category("DATA")]
		public object DataSource
		{
			get
			{
				return _combo.DataSource;
			}
			set
			{
				_combo.DataSource = value;
			}
		}

		[Category("DATA")]
		public string DisplayMember
		{
			get
			{
				return _combo.DisplayMember;
			}
			set
			{
				_combo.DisplayMember = value;
			}
		}

		[Category("DATA")]
		public string ValueMember
		{
			get
			{
				return _combo.ValueMember;
			}
			set
			{
				_combo.ValueMember = value;
			}
		}

		[Category("DATA")]
		public object SelectedValue
		{
			get
			{
				return _combo.SelectedValue;
			}
			set
			{
				_combo.SelectedValue = value;
			}

		}
		
		[Category("DATA")]
		public int SelectedIndex
		{
			get
			{
				return _combo.SelectedIndex;
			}
			set
			{
				_combo.SelectedIndex = value;
			}
		}
		
		[Category("DATA")]
		public object SelectedItem
		{
			get
			{
				return _combo.SelectedItem;
			}
			set
			{
				_combo.SelectedItem = value;
			}
		}

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
		public DataGridComboBox2 ComboBox
		{
			get
			{
				return _combo;
			}
		}

		private void DropDown_Closing(object sender, CancelEventArgs e)
		{
			e.Cancel=true;
		}
		#endregion

	}

}
