using System;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// DataGrid TextBox object.
    /// </summary>
    public class DataGridTextBox2 : Control
	{
		#region Fields

		/// <summary>
		/// Internal text box control for editing.
		/// </summary>
		private DataGridTextBox _txt;

		/// <summary>
		/// Data Grid control that this control belongs to.
		/// </summary>
		private DataGrid _grid;

		private CharacterCasing _charcase = CharacterCasing.Normal;

		#endregion

		#region Constuctors

		public DataGridTextBox2() : base()
		{
			_txt = new DataGridTextBox();
			_txt.BorderStyle = BorderStyle.None;
			Controls.Add(_txt);
			_txt.Dock = DockStyle.Fill;
			SetRTL();
			this.RightToLeftChanged += new EventHandler(this.RightToLeftChangedEvent);
		}

		#endregion

		#region Properties
		/// <summary>
		/// Gets the textbox object of the custom control.
		/// </summary>
		public TextBox TextBox
		{
			get
			{
				return _txt;
			}
		}

		/// <summary>
		/// Gets and Sets the text of the Text control.
		/// </summary>
		public override string Text
		{
			get
			{
				if (_charcase == CharacterCasing.Upper)
					return _txt.Text.ToUpper();
				else if (_charcase == CharacterCasing.Lower)
					return _txt.Text.ToLower();
				else
					return _txt.Text;
			}
			set
			{
				if (_charcase == CharacterCasing.Upper)
					_txt.Text = value.ToUpper();
				else if (_charcase == CharacterCasing.Lower)
					_txt.Text = value.ToLower();
				else
					_txt.Text = value;
			}
		}

		public CharacterCasing CharacterCasing
		{
			get
			{
				return _charcase;
			}
			set
			{
				_charcase = value;
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

	}

	/// <summary>
	/// Data Grid TextBox column style.
	/// </summary>
	public class DataGridTextBox2Column	: DataGridColumnStyle
	{
		#region Fields
		
		/// <summary>
		/// Manipulated Text box control.
		/// </summary>
		private DataGridTextBox2 _Text;

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
		private int _yMargin = 0;



		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new Text box column style.
		/// </summary>
		public DataGridTextBox2Column()
		{
			//Create the Text box and capture the events assigned to below.
			_Text = new DataGridTextBox2();
			_Text.Visible = false;
		}
			
							
		#endregion

		#region Column Style Override Methods

		/// <summary>
		/// Aborts the specified row edit.
		/// </summary>
		/// <param name="RowNum">Row number to abort.</param>
		protected override void Abort(int RowNum)
		{
			//Rollback to the orignal value and hide the Text box.
			RollBack();
			HideTextBox();
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
			HideTextBox();
			if(!_inEdit)
			{
				return true;
			}
			try
			{	
				object Value = _Text.Text;

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
		/// Makes the Text box  invisible when the column has lost focus.
		/// </summary>
		protected override void ConcedeFocus()
		{
			_Text.Visible=false;
		}

		
		/// <summary>
		/// Called when the current column on a specified row is being edited.  This allows
		/// for the use of showing the Text box and setting its text to the value being edited.
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
			
			_Text.SetDataGrid(this.DataGridTableStyle.DataGrid);

			object _newValue = GetColumnValueAtRow(source, rowNum);

			if(cellIsVisible)
			{
				bounds.Offset(_xMargin, _yMargin+1);
				bounds.Width -= _xMargin;
				bounds.Height -= (_yMargin+1);
				_Text.Bounds = bounds;
				_Text.Visible = true;
			}
			else
			{
				_Text.Bounds = OriginalBounds;
				_Text.Visible = false;
			}
			
			_Text.TextBox.TextChanged -=new EventHandler(TextBox_TextChanged);
			
			if (_Text.Text != GetText(_newValue))
				_Text.Text= GetText(_newValue);

			if(instantText!=null)
				_Text.Text = instantText;

			_Text.TextBox.TextChanged +=new EventHandler(TextBox_TextChanged);


			//Right to left compatibility of the Text box.
			_Text.RightToLeft = this.DataGridTableStyle.DataGrid.RightToLeft;
			
			_Text.SelectAll();

			if(_Text.Visible)
			{
				DataGridTableStyle.DataGrid.Invalidate(OriginalBounds);
			}
			

			//_inEdit = true;
		}

		/// <summary>
		/// Returns the minimum height of the column / Text box.
		/// </summary>
		/// <returns>Integer height value.</returns>
		protected override int GetMinimumHeight()
		{
			return _Text.TextBox.PreferredHeight + _yMargin;
		}

		/// <summary>
		/// Returns the default preferred height of the column / Text box.
		/// </summary>
		/// <param name="g">Graphical object of the column.</param>
		/// <param name="Value">Current value set in the graphical space.</param>
		/// <returns>Integer height value.</returns>
		protected override int GetPreferredHeight(Graphics g ,object Value)
		{
			return 18;
		}

		/// <summary>
		/// Returns the default preferred size of the column / Text box.
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
			if(_Text.Parent!=Value)
			{
				if(_Text.Parent!=null)
				{
					_Text.Parent.Controls.Remove(_Text);
				}
			}
			if(Value!=null) 
			{
				Value.Controls.Add(_Text);
			}
		}
		
		protected override void UpdateUI(CurrencyManager Source,int RowNum, string InstantText)
		{
			_Text.TextBox.TextChanged -=new EventHandler(TextBox_TextChanged);
			_Text.Text = GetText(GetColumnValueAtRow(Source, RowNum));
			if(InstantText!=null)
			{
				_Text.Text = InstantText;
			}
			_Text.TextBox.TextChanged +=new EventHandler(TextBox_TextChanged);
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
			g.DrawString(Text, this.DataGridTableStyle.DataGrid.Font, ForeBrush, RectF, Format);
			Format.Dispose();
		}

		/// <summary>
		/// Ends the current edit by setting the internal flag to false and redrawing the column.
		/// </summary>
		private void EndEdit()
		{
			_inEdit = false;
			_Text.TextBox.TextChanged -=new EventHandler(TextBox_TextChanged);
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
		private void HideTextBox()
		{
			if(_Text.Focused)
			{
				this.DataGridTableStyle.DataGrid.Focus();
			}
			_Text.Visible = false;
		}

		/// <summary>
		/// Sets the combo box back to its original value and ends the edit process.
		/// </summary>
		private void RollBack()
		{
			_Text.TextBox.TextChanged -=new EventHandler(TextBox_TextChanged);
			_Text.Text = GetText(_oldValue);
			_Text.TextBox.TextChanged +=new EventHandler(TextBox_TextChanged);
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

		public CharacterCasing CharacterCasing
		{
			get
			{
				return _Text.TextBox.CharacterCasing;
			}
			set
			{
				_Text.TextBox.CharacterCasing = value;
			}
		}


		#endregion

		private void TextBox_TextChanged(object sender, EventArgs e)
		{
			_inEdit = true;
		}
	}
}

