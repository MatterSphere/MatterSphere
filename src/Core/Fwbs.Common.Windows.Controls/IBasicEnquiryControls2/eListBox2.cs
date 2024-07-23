using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A list selection enquiry control which generally holds a caption and a list box.  
    /// This particular control can be used for picking fixed items in a list.
    /// </summary>
    public class eListBox2 : eBase2, IBasicEnquiryControl2, IListEnquiryControl
	{
		#region Contructors

		/// <summary>
		/// Creates a new list box control and assigns it to the base control object.
		/// </summary>
		public eListBox2() : base()
		{
			ListBox lst = new ListBox() { IntegralHeight = false };
			// Capture the SelectedIndexChanged event so that it can be re-raised as the changed event.
			lst.SelectedIndexChanged += new System.EventHandler(this.RaiseActiveChangedEvent);
			// Overload the existing Leave event so that it can be re-raised as the leave event.
			lst.Leave += new System.EventHandler(this.RaiseLeaveEvent);
			lst.GotFocus += new System.EventHandler(this.RaiseGotFocusEvent);
            this.VisibleChanged += new EventHandler(eListBox2_VisibleChanged);
            _ctrl = lst;
			_ctrl.TabIndex = 0;
			Controls.Add(_ctrl);
		}

        private void eListBox2_VisibleChanged(object sender, EventArgs e)
        {
            ListBox list = (ListBox)_ctrl;
            if (this.Visible)
            {
                if ((list.SelectedValue != null && list.SelectedValue.Equals(selectedvalue) == false) 
                    || (selectedvalue != null && list.SelectedValue == null))
                    list.SelectedValue = selectedvalue;
            }
        }

        protected override void RaiseActiveChangedEvent(object sender, EventArgs e)
        {
            ListBox list = (ListBox)_ctrl;
            selectedvalue = list.SelectedValue;
            base.RaiseActiveChangedEvent(sender, e);
        }

        #endregion

        #region Design Mode
        public override bool omsDesignMode
		{
			get
			{
				return base.omsDesignMode;
			}
			set
			{
				base.omsDesignMode = value;
				_ctrl.Visible = !value;
			}
		}

		protected override void omsDesignMode_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			System.Windows.Forms.ControlPaint.DrawBorder3D(e.Graphics, _ctrl.Bounds, Border3DStyle.Sunken, Border3DSide.All);
			e.Graphics.FillRectangle(SystemBrushes.Window,_ctrl.Bounds.X + 2,_ctrl.Bounds.Y + 2,_ctrl.Bounds.Width - 4,_ctrl.Bounds.Height - 4);
		}
		#endregion

		#region IListEnquiryControl Implementation

		/// <summary>
		/// Gets or Sets the display text section of the list box.
		/// </summary>
		[Browsable(false)]
		[DefaultValue("")]
		public object DisplayValue 
		{
			get
			{
				return  ((ListBox)_ctrl).Text;
			}
			set
			{
				((ListBox)_ctrl).Text = value.ToString();
			}
		}

		/// <summary>
		/// Assigns an individual bound value and a display value for a list box.
		/// </summary>
		/// <param name="Value">Invisible bound value.</param>
		/// <param name="displayText">Visible display text.</param>
		public void AddItem(object Value, string displayText)
		{
            ListBox lst = (ListBox)_ctrl; 
            lst.BeginUpdate();
			EnquiryListItem itm = new EnquiryListItem(Value, displayText);
			lst.Items.Add(itm);
            lst.EndUpdate();
		}

		/// <summary>
		/// Assigns a data table object to the datasource of the combo box which uses
		/// the first column as bound member, and the first or second column as the display
		/// member.
		/// </summary>
		/// <param name="dataTable">Bound data table.</param>
		public void AddItem(DataTable dataTable)
		{
            ListBox lst = (ListBox)_ctrl; 
            lst.BeginUpdate();
            if (lst.DataSource != null) lst.DataSource = null;
            if (dataTable.Columns.Count > 0)
            {
                lst.ValueMember = dataTable.Columns[0].ColumnName;
                if (dataTable.Columns.Count > 1)
                    lst.DisplayMember = dataTable.Columns[1].ColumnName;
                else
                    lst.DisplayMember = dataTable.Columns[0].ColumnName;
            }
            lst.DataSource = dataTable;
            lst.EndUpdate();

		}

		/// <summary>
		/// Assigns a data table object to the datasource of the list box which uses
		/// the a specified value and display column
		/// </summary>
		/// <param name="dataTable">Bound data table.</param>
		/// <param name="valueMember">Column to be bound.</param>
		/// <param name="displayMember">Column to be displayed.</param>
		public void AddItem(DataTable dataTable, string valueMember, string displayMember)
		{
			ListBox lst = (ListBox)_ctrl;
            lst.BeginUpdate();
			if (dataTable.Columns.Contains(valueMember))
			{
				lst.ValueMember = dataTable.Columns[valueMember].ColumnName;
			}
			if (dataTable.Columns.Contains(displayMember))
			{
				lst.DisplayMember = dataTable.Columns[displayMember].ColumnName;
			}
			lst.DataSource = dataTable;
			lst.EndUpdate();

		}

        /// <summary>
        /// Assigns a data view object to the datasource of the list box which uses
        /// the first column as the bound member, and the first or second column as the display
        /// member.
        /// </summary>
        /// <param name="dataView">Bound data view.</param>
        public void AddItem(DataView dataView)
		{
            ListBox lst = (ListBox)_ctrl;
            lst.BeginUpdate();
			if (dataView.Table != null)
			{
				if (dataView.Table.Columns.Count > 0)
				{
					lst.ValueMember = dataView.Table.Columns[0].ColumnName;
					if (dataView.Table.Columns.Count > 1)
						lst.DisplayMember = dataView.Table.Columns[1].ColumnName;
					else
						lst.DisplayMember = dataView.Table.Columns[0].ColumnName;
				}
				lst.DataSource= dataView;
			}
            lst.EndUpdate();

		}


        /// <summary>
        /// Assigns a data view object to the datasource of the list box which uses
        /// the a specified value and display column
        /// </summary>
        /// <param name="dataView">Bound data view.</param>
        /// <param name="valueMember">Column to be bound.</param>
        /// <param name="displayMember">Column to be displayed.</param>
        public void AddItem(DataView dataView, string valueMember, string displayMember)
		{
            ListBox lst = (ListBox)_ctrl;
            lst.BeginUpdate();
			if (dataView.Table != null)
			{
				if (dataView.Table.Columns.Contains(valueMember))
				{
					lst.ValueMember = dataView.Table.Columns[valueMember].ColumnName;
				}
				if (dataView.Table.Columns.Contains(displayMember))
				{
					lst.DisplayMember = dataView.Table.Columns[displayMember].ColumnName;
				}
				lst.DataSource = dataView;
			}
			lst.EndUpdate();
		}

		/// <summary>
		/// Gets the number of items within the list section of the control.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(0)]
		public int Count 
		{
			get
			{
				return ((ListBox)_ctrl).Items.Count;
			}
		}

		/// <summary>
		/// Filters the bound result set withe the specified Filter String
		/// </summary>
		/// <param name="FilterString">Filter String</param>
		public bool Filter (string FilterString)
		{
			DataTable dt = null;
			ListBox lst = (ListBox)_ctrl;
			if (lst.DataSource is DataView)
			{
				dt = ((DataView)lst.DataSource).Table;
			}
			else if (lst.DataSource is DataTable)
			{
				dt = (DataTable)lst.DataSource;
			}

			if (dt != null)
			{
				DataView dv = new DataView(dt);
				dv.RowFilter = FilterString + " or " + lst.ValueMember + " is null";
				lst.DataSource = dv;
			}
			if (lst.Items.Count > 0)
				lst.SelectedIndex =0;
			return true;
		}

		/// <summary>
		/// Filters the bound result set with the specified value under the specified field.
		/// </summary>
		/// <param name="fieldName">The field name to filter with.</param>
		/// <param name="Value">The value to filter for.</param>
		public bool Filter (string fieldName, object Value)
		{
			if (Value == null || fieldName == null) return false;
			ListBox lst = (ListBox)_ctrl;
			DataTable dt = null;
			if (lst.DataSource is DataView)
			{
				dt = ((DataView)lst.DataSource).Table;
			}
			else if (lst.DataSource is DataTable)
			{
				dt = (DataTable)lst.DataSource;
			}

			if (dt != null)
			{
				DataView dv = new DataView(dt);
				dv.RowFilter = fieldName + " = '" + Convert.ToString(Value).Replace("'", "''") + "' or " + lst.ValueMember + " is null";
				lst.DataSource = dv;
			}
			return true;
		}

		#endregion

		#region IBasicEnquiryControl2 Implementation

        /// <summary>
        /// Gets or Sets the value within the control.
        /// </summary>
        private object selectedvalue;

        [DefaultValue(null)]
        public override object Value
        {
            get
            {
                return selectedvalue;
            }
            set
            {
                ListBox list = (ListBox)_ctrl;
                if (this.Visible)
                    list.SelectedValue = value;
                selectedvalue = value;

                if (this.Parent != null)
                {
                    IsDirty = true;
                    OnChanged();
                }
            }
        }

		/// <summary>
		/// Gets the boolean flag that locks the height of the control.  In this case the list box
		/// can be stretched.
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
