using System;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;


namespace FWBS.OMS.Design.CodeBuilder
{
    /// <summary>
    /// DataGrid MemoBox object.
    /// </summary>
    public class DataGridMemoBox : Control
	{
		#region Fields

		/// <summary>
		/// Internal text box control for editing.
		/// </summary>
		private DataGridTextBox _txt;
		/// <summary>
		/// Internal button control.
		/// </summary>
		internal Button _btn;
        /// <summary>
        /// Internal workflow button control.
        /// </summary>
        internal Button _wfb;

		/// <summary>
		/// Data Grid control that this control belongs to.
		/// </summary>
		private DataGrid _grid;

        private ToolTip tooltip;
		#endregion
        
        #region Constuctors

        public DataGridMemoBox() : base()
		{
            tooltip = new ToolTip();
			_txt = new DataGridTextBox();
			_txt.Multiline = true;
			_txt.BorderStyle = BorderStyle.None;
			Controls.Add(_txt);

            _wfb = new Button();
            _wfb.Click += new EventHandler(this.ClickEvent);
            tooltip.SetToolTip(_wfb, "Choose WorkFlow");
            _wfb.FlatStyle = FlatStyle.Flat;
            _wfb.BackColor = SystemColors.Control;
            _wfb.Size = new Size(16, 16);
            _wfb.Dock = DockStyle.Right;
            Controls.Add(_wfb,true);
            _btn = new Button();
			_btn.Click += new EventHandler(this.ClickEvent);
			
            tooltip.SetToolTip(_btn, "Edit Script");
            _btn.FlatStyle = FlatStyle.Flat;
            _btn.BackColor = SystemColors.Control;
			_btn.Size = new Size(16, 16);
			_btn.Dock = DockStyle.Right;
            Controls.Add(_btn, true);

            _txt.Dock = DockStyle.Fill;
            ApplyButtonsImage();

            SetRTL();
			this.RightToLeftChanged += new EventHandler(this.RightToLeftChangedEvent);
		}


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (tooltip != null)
                    tooltip.Dispose();
                if (_btn != null)
                {
                    _btn.Click -= new EventHandler(this.ClickEvent);
                    _btn.Dispose();
                    _btn = null;
                }
                if (_wfb != null)
                {
                    _wfb.Click -= new EventHandler(this.ClickEvent);
                    _wfb.Dispose();
                    _wfb = null;
                }
            }
        }
		#endregion

		#region Properties

		/// <summary>
		/// Gets a reference to the button control of the object.
		/// </summary>
		public Button Button
		{
			get
			{
				return _btn;
			}
		}

		/// <summary>
		/// Gets the textbox object of the custom control.
		/// </summary>
		public DataGridTextBox TextBox
		{
			get
			{
				return _txt;
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
			_txt.Focus();
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

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            ApplyButtonsImage();
        }

        private Image WorkflowImage => FWBS.OMS.UI.Windows.Images.GetCoolButton(77,
            (UI.Windows.Images.IconSize)LogicalToDeviceUnits(16)).ToBitmap();

        private Image ScriptImage => FWBS.OMS.UI.Windows.Images.GetCoolButton(12,
            (UI.Windows.Images.IconSize)LogicalToDeviceUnits(16)).ToBitmap();

        private void ApplyButtonsImage()
        {
            _wfb.Image = WorkflowImage;
            _btn.Image = ScriptImage;
        }
    }
	
	/// <summary>
	/// Data Grid MemoBox column style.
	/// </summary>
	public class DataGridEventColumn : DataGridColumnStyle
	{
		#region Fields
		
		/// <summary>
		/// Manipulated memo box control.
		/// </summary>
		private DataGridMemoBox _memo;

		/// <summary>
		/// Original value that the combo box was assigned to.
		/// </summary>
		private object _oldValue = System.DBNull.Value;


		/// <summary>
		/// Current edit mode flag.
		/// </summary>
		private bool _inEdit = false;

		/// <summary>
		/// X coordinate Offset for the positioning of the control within the cell.
		/// </summary>
		private int _xMargin = 2;

		/// <summary>
		/// Y coordinate Offset for the positioning of the control within the cell.
		/// </summary>
		private int _yMargin = 1;



		#endregion

		#region Events
		public event EventHandler ButtonClick;

		public void OnButtonClick(object Sender, EventArgs e)
		{
			if (ButtonClick != null)
				ButtonClick(Sender,e);
		}

        public event EventHandler WorkflowButtonClick;

        public void OnWorkflowButtonClick(object Sender, EventArgs e)
        {
            if (WorkflowButtonClick != null)
                WorkflowButtonClick(Sender, e);
        }

        public event EventHandler WorkflowDeleted;

        public void OnWorkflowDeleted(object Sender, EventArgs e)
        {
            if (WorkflowDeleted != null)
                WorkflowDeleted(Sender, e);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                OnWorkflowDeleted(sender, EventArgs.Empty);
            }
        }

		#endregion
		
		#region Constructors
		/// <summary>
		/// Creates a new memo box column style.
		/// </summary>
		public DataGridEventColumn()
		{
			//Create the memo box and capture the events assigned to below.
			_memo = new DataGridMemoBox();
			_memo.Visible = false;
			_memo.TextBox.ReadOnly =true;
			_memo.TextBox.BackColor = Color.White;
            _memo.TextBox.KeyDown +=new KeyEventHandler(TextBox_KeyUp);
			_memo._btn.Click += new EventHandler(OnButtonClick);
            _memo._wfb.Click += new EventHandler(OnWorkflowButtonClick);
        }

		#endregion

		#region Column Style Override Methods

		/// <summary>
		/// Aborts the specified row edit.
		/// </summary>
		/// <param name="RowNum">Row number to abort.</param>
		protected override void Abort(int RowNum)
		{
			//Rollback to the orignal value and hide the memo box.
			RollBack();
			HideMemoBox();
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
			HideMemoBox();
			if(!_inEdit)
			{
				return true;
			}
			try
			{	
				object Value = _memo.Text;

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
		/// Makes the memo box  invisible when the column has lost focus.
		/// </summary>
		protected override void ConcedeFocus()
		{
			_memo.Visible=false;
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
			Rectangle OriginalBounds = bounds;
			
			_memo.SetDataGrid(this.DataGridTableStyle.DataGrid);
			source.Position = rowNum;
			object _newValue = GetColumnValueAtRow(source, rowNum);
			if(cellIsVisible)
			{
				bounds.Offset(_xMargin, _yMargin);
				bounds.Width -= _xMargin;
				bounds.Height -= _yMargin;
				_memo.Bounds = bounds;
				_memo.Visible = true;
			}
			else
			{
				_memo.Bounds = OriginalBounds;
				_memo.Visible = false;
			}
			

			_memo.Text= GetText(_newValue);

			//Right to left compatibility of the memo box.
			_memo.RightToLeft = this.DataGridTableStyle.DataGrid.RightToLeft;
			
			_memo.SelectAll();

			if(_memo.Visible)
			{
				DataGridTableStyle.DataGrid.Invalidate(OriginalBounds);
			}
			

			_inEdit = true;
		}

		/// <summary>
		/// Returns the minimum height of the column / memo box.
		/// </summary>
		/// <returns>Integer height value.</returns>
		protected override int GetMinimumHeight()
		{
			return _memo.TextBox.PreferredHeight + _yMargin;
		}

		/// <summary>
		/// Returns the default preferred height of the column / memo box.
		/// </summary>
		/// <param name="g">Graphical object of the column.</param>
		/// <param name="Value">Current value set in the graphical space.</param>
		/// <returns>Integer height value.</returns>
		protected override int GetPreferredHeight(Graphics g ,object Value)
		{
			int NewLineIndex  = 0;
			int NewLines = 0;
			string ValueString = this.GetText(Value);
			do
			{
				NewLineIndex = ValueString.IndexOf("r\n", NewLineIndex + 1);
				NewLines += 1;
			}while(NewLineIndex != -1);
			return FontHeight * NewLines + _yMargin;
		}

		/// <summary>
		/// Returns the default preferred size of the column / memo box.
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
			if(_memo.Parent!=Value)
			{
				if(_memo.Parent!=null)
				{
					_memo.Parent.Controls.Remove(_memo);
				}
			}
			if(Value!=null) 
			{
				Value.Controls.Add(_memo);
			}
		}
		
		protected override void UpdateUI(CurrencyManager Source,int RowNum, string InstantText)
		{
			_memo.Text = GetText(GetColumnValueAtRow(Source, RowNum));
			if(InstantText!=null)
			{
				_memo.Text = InstantText;
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
            using (StringFormat Format = new StringFormat())
            {
                if (AlignToRight)
                {
                    Format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                }
                switch (this.Alignment)
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
                Format.FormatFlags = Format.FormatFlags;
                Format.FormatFlags = StringFormatFlags.NoWrap;
                g.FillRectangle(BackBrush, Rect);
                Rect.Offset(0, _yMargin);
                Rect.Height -= _yMargin;
                RectF.Y += 2;
                g.DrawString(Text, this.DataGridTableStyle.DataGrid.Font, ForeBrush, RectF, Format);
            }
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
		private void HideMemoBox()
		{
			if(_memo.Focused)
			{
				this.DataGridTableStyle.DataGrid.Focus();
			}
			_memo.Visible = false;
		}

		/// <summary>
		/// Sets the combo box back to its original value and ends the edit process.
		/// </summary>
		private void RollBack()
		{
			_memo.Text = GetText(_oldValue);
			EndEdit();
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
		public DataGridMemoBox MemoBox
		{
			get
			{
				return _memo;
			}
		}

		#endregion

	}
}

