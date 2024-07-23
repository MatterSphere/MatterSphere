using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;


namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A list enquiry control which generally holds a caption and a group of radio buttons.  
    /// This particular control can be used for picking valid items from a list.
    /// </summary>
    public class eRadioGroup2 : eBase2, IBasicEnquiryControl2, IListEnquiryControl
	{

		#region Fields
		/// <summary>
		/// Leading between each radio button.
		/// </summary>
		private const int _leading = -3;

		/// <summary>
		/// Holds the underlying DataTable or DataView object that build the option group.
		/// </summary>
		private object _data = null;

		/// <summary>
		/// Holds the bound value member field.
		/// </summary>
		private string _valueMember = "";
		/// <summary>
		/// Holds the display value member field.
		/// </summary>
		private string _displayMember = "";

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new group box control and assigns it to the base control object.
		/// </summary>
		public eRadioGroup2() : base()
		{
			GroupBox grp = new System.Windows.Forms.GroupBox();
			grp.FlatStyle = FlatStyle.System;
			_ctrl = grp;
			_ctrl.Left = 0;
			_ctrl.Dock = DockStyle.Fill;
			_ctrl.TabIndex = 0;
			Controls.Add(_ctrl);
		}

		#endregion

		#region IListEnquiryControl Implementation

		/// <summary>
		/// Assigns an individual bound value and a display value for a radio group.
		/// </summary>
		/// <param name="Value">Invisible bound value.</param>
		/// <param name="displayText">Visible display text.</param>
		public void AddItem(object Value, string displayText)
		{
			this.SuspendLayout();
			//Creates a new radio button.
			System.Windows.Forms.RadioButton Item;
			Item = new System.Windows.Forms.RadioButton();
			//Sets it to system style.
			Item.FlatStyle = System.Windows.Forms.FlatStyle.System;
			Item.Location = new System.Drawing.Point(8, 12);
			//Set the tag value as the bound value.
			Item.Tag = Value;
			//Set the diplay text as the text.
			Item.Text = displayText;
			Item.Width = _ctrl.Width - 16;
			//Capture the following event and raise it as the changed event.
			Item.CheckedChanged += new EventHandler(this.RaiseActiveChangedEvent);
			//Capture the following event and raise it as the leave event.

			//Removed by DJRM
			Item.Leave += new System.EventHandler(this.RaiseLeaveEvent);
			Item.GotFocus += new System.EventHandler(this.RaiseGotFocusEvent);
			
			_ctrl.Controls.Add(Item);
			RebuildControls();
			this.ResumeLayout(true);
		}


		/// <summary>
		/// Assigns a data table object to build a list of radio buttons.
		/// The first column is the bound member, and the first or second column is the display
		/// member.
		/// </summary>
		/// <param name="dataTable">Bound data table.</param>
		public void AddItem(DataTable dataTable)
		{
			_data = dataTable;

			_ctrl.Controls.Clear();
			if (dataTable.Columns.Count > 0)
			{
				if (dataTable.Columns.Count == 1)
				{

					foreach (DataRow row in dataTable.Rows)
					{
						AddItem(row[0], row[0].ToString());
					}
				}
				else if	(dataTable.Columns.Count > 1)
				{

					foreach (DataRow row in dataTable.Rows)
					{
						AddItem(row[0], row[1].ToString());
					}
				}
			}
		}

		/// <summary>
		/// Assigns a data table object to build a list of radio buttons.
		/// </summary>
		/// <param name="dataTable">Bound data table.</param>
		/// <param name="valueMember">Bound column.</param>
		/// <param name="displayMember">Column is view / display.</param>
		public void AddItem(DataTable dataTable, string valueMember, string displayMember)
		{
			_data = dataTable;
			_valueMember = valueMember;
			_displayMember = displayMember;

			_ctrl.Controls.Clear();
			if (dataTable.Columns.Count > 0)
			{
				if (dataTable.Columns.Count == 1)
				{

					foreach (DataRow row in dataTable.Rows)
					{
						AddItem(row[valueMember], row[valueMember].ToString());
					}
				}
				else if	(dataTable.Columns.Count > 1)
				{

					foreach (DataRow row in dataTable.Rows)
					{
						AddItem(row[valueMember], row[displayMember].ToString());
					}
				}
			}
		}

		/// <summary>
		/// Assigns a data view object to build a list of radio buttons.
		/// The first column is the bound member, and the first or second column is the display
		/// member.
		/// </summary>
		/// <param name="dataView">Bound data view.</param>
		public void AddItem(DataView dataView)
		{
			_data = dataView;
			if (dataView.Table != null)
			{
                while (_ctrl.Controls.Count > 0)
                {
                    Control ctrl = _ctrl.Controls[0];
                    _ctrl.Controls.RemoveAt(0);
                    ctrl.Dispose();
                }
                _ctrl.Controls.Clear();

				if (dataView.Table.Columns.Count > 0)
				{
					if (dataView.Table.Columns.Count == 1)
					{
						foreach (DataRowView row in dataView)
						{
							AddItem(row[0], row[0].ToString());
						}
					}
					else if	(dataView.Table.Columns.Count > 1)
					{

						foreach (DataRowView row in dataView)
						{
							AddItem(row[0], row[1].ToString());
						}
					}
				}
			}
		}

		/// <summary>
		/// Assigns a data view object to build a list of radio buttons.
		/// </summary>
		/// <param name="dataView">Bound data view.</param>
		/// <param name="valueMember">Bound column.</param>
		/// <param name="displayMember">Column is view / display.</param>
		public void AddItem(DataView dataView, string valueMember, string displayMember)
		{
			_data = dataView;
			_valueMember = valueMember;
			_displayMember = displayMember;

			if (dataView.Table != null)
			{
				_ctrl.Controls.Clear();
				if (dataView.Table.Columns.Count > 0)
				{
					if (dataView.Table.Columns.Count == 1)
					{
						foreach (DataRowView row in dataView)
						{
							AddItem(row[valueMember], row[valueMember].ToString());
						}
					}
					else if	(dataView.Table.Columns.Count > 1)
					{

						foreach (DataRowView row in dataView)
						{
							AddItem(row[valueMember], row[displayMember].ToString());
						}
					}
				}
			}
		}


		/// <summary>
		/// Gets or Sets the Display member of the radio group.
		/// </summary>
		[Browsable(false)]
		[DefaultValue("")]
		public object DisplayValue 
		{
			get
			{
				object _value = null;
				foreach (System.Windows.Forms.RadioButton Item in _ctrl.Controls)
				{
					if (Item.Checked) 
					{
						_value = Item.Text;
					}
				}
				return _value;
			}
			set
			{
				foreach (System.Windows.Forms.RadioButton Item in _ctrl.Controls)
				{
					if (Item.Tag.ToString()  == value.ToString()) 
					{
						Item.Text = value.ToString();
					}
				}
			}
		}

		/// <summary>
		/// Gets the number of radio group items.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(0)]
		public int Count 
		{
			get
			{
				return _ctrl.Controls.Count;
			}
		}

		/// <summary>
		/// Filters the bound result set with the specified value under the specified field.
		/// </summary>
		/// <param name="fieldName">The field name to filter with.</param>
		/// <param name="Value">The value to filter for.</param>
		public bool Filter (string fieldName, object Value)
		{
			if (Value == null || fieldName == null) return false;
			DataTable dt = null;
			if (_data is DataView)
			{
				dt = ((DataView)_data).Table;
			}
			else if (_data is DataTable)
			{
				dt = (DataTable)_data;
			}

			if (dt != null)
			{
				DataView dv = new DataView(dt);
				dv.RowFilter = fieldName + " = '" + Convert.ToString(Value).Replace("'", "''") + "' or " + _valueMember + " is null";
				if (_valueMember == String.Empty)
					AddItem(dv);
				else
					AddItem(dv, _valueMember, _displayMember);
			}
			return true;
		}


		/// <summary>
		/// Filters the bound result set with the specified value under the specified field.
		/// </summary>
		/// <param name="FilterString">The field name to filter with.</param>
		public bool Filter (string FilterString)
		{
			DataTable dt = null;
			if (_data is DataView)
			{
				dt = ((DataView)_data).Table;
			}
			else if (_data is DataTable)
			{
				dt = (DataTable)_data;
			}

			if (dt != null)
			{
				DataView dv = new DataView(dt);
				dv.RowFilter = FilterString;
				if (_valueMember == String.Empty)
					AddItem(dv);
				else
					AddItem(dv, _valueMember, _displayMember);
			}
			return true;
		}


		#endregion

		#region Methods

		/// <summary>
		/// Rebuilds the controls using the specified leading.
		/// </summary>
		private void RebuildControls()
		{
			int _top=0;
			foreach (System.Windows.Forms.RadioButton Item in _ctrl.Controls)
			{
				Item.Location = new System.Drawing.Point(8,12 + _top);
				_top = _top + (Item.Height + _leading);
			}
			this.Height = _top + 20;
		}


		#endregion

		#region Properties

		/// <summary>
		/// Overrides the base control text property so that the text can be set to the 
		/// group box text in the system style.
		/// </summary>
		[DefaultValue("")]
		public override string Text
		{
			get
			{
				return _ctrl.Text;
			}
			set
			{
				_ctrl.Text = value;
			}
		}

        [Browsable(false)]
        public override bool CaptionTop
        {
            get
            {
                return false;
            }
            set { }
        }

        #endregion

        #region IBasicEnquiryControl2 Implementation

        /// <summary>
        /// Gets or Sets the value within the control.
        /// </summary>
        [DefaultValue(null)]
		public override object Value
		{
			get
			{
				object _value = DBNull.Value;
				foreach (System.Windows.Forms.RadioButton Item in _ctrl.Controls)
				{
					if (Item.Checked) 
					{
						_value = Item.Tag;
						return _value;
					}
				}
				return _value;
			}
			set
			{
				foreach (System.Windows.Forms.RadioButton Item in _ctrl.Controls)
				{
					if (Item.Tag.ToString()  == value.ToString()) 
					{
						Item.Checked = true;
						break;
					}
				}
				if (_ctrl.Controls.Count > 0 && value == DBNull.Value)
				{
					((System.Windows.Forms.RadioButton)_ctrl.Controls[0]).Checked = true;
				}
				if (this.Parent != null)
				{
					IsDirty=true;
					OnChanged();
				}
			}
		}
	

		/// <summary>
		/// Gets or Sets the width of the caption. In this case the caption is located above 
		/// the list.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(150)]
		public override int CaptionWidth
		{
			get
			{
				return 0;
			}
			set
			{
				_ctrl.Left = 0;
				_ctrl.Dock = DockStyle.Fill;
			}
		}

        /// <summary>
        /// Gets the lock height flag which states whether the control is locked to a certain height.
        /// This is a design mode property and is set to false.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public override bool LockHeight
        {
            get
            {
                return false;
            }
        }

        #endregion
    }
}

