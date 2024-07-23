using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FWBS.Common.UI.Windows.Common;
using FWBS.OMS;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A list selection enquiry control which generally holds a caption and a combo box.  
    /// This particular control can be used for picking items in a combo box style list or using it like a text box.
    /// </summary>
    public class eComboBox2 : eBase2, IBasicEnquiryControl2, IListEnquiryControl, ITextEditorEnquiryControl
	{

        private class CustomComboBox : CueComboBox
        {
            protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
            {
                base.SetBoundsCore(x, System.Math.Max(0, y), width, height, specified);
            }
        }

        private const string CUETEXT_CODELOOKUPGROUPNAME = "ENQQUESTCUETXT";

        private CodeLookupDisplay _cueText;

        private string _cueTextCode;

        #region Constructors
        /// <summary>
        /// Creates a new combo box control and assigns it to the base control object.
        /// </summary>
        public eComboBox2() : base()
        {
            CustomComboBox cbo = new CustomComboBox();
			//Captures the SelectedIndexChanged event to be raised as the changed event.
			cbo.Leave +=new EventHandler(cbo_Leave);
            cbo.TextChanged += new EventHandler(this.RaiseActiveChangedEvent);
            cbo.SelectionChangeCommitted += new EventHandler(this.RaiseActiveChangedEvent);
            cbo.LostFocus += new EventHandler(cbo_LostFocus);
			cbo.GotFocus += new System.EventHandler(this.RaiseGotFocusEvent);
			_ctrl = cbo;
			_ctrl.TabIndex = 0;
            this.Height = 23;
            this.VisibleChanged += new EventHandler(eComboBox2_VisibleChanged);
            this.Controls.Add(_ctrl);
        }

        #region Properties

        [Category("OMS Appearance")]
        [CodeLookupSelectorTitle("CUETEXT", "Cue Text")]
        [DefaultValue(null)]
        [Description("Localised code of the Controls CueText"), LocCategory("Design")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MergableProperty(false)]
        public virtual CodeLookupDisplay CueText
        {
            get { return _cueText ?? (_cueText = new CodeLookupDisplay(CUETEXT_CODELOOKUPGROUPNAME)); }

            set
            {
                if (_cueText != value)
                {
                    _cueText = value;
                    ((CustomComboBox)_ctrl).CueText = _cueText.Description;
                    IsDirty = true;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string CueTextCode
        {
            get { return _cueTextCode; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !value.Equals(_cueTextCode))
                {
                    _cueTextCode = value;
                    CueText = new CodeLookupDisplay(CUETEXT_CODELOOKUPGROUPNAME)
                    {
                        Code = value,
                        Description = CodeLookup.GetLookup(CUETEXT_CODELOOKUPGROUPNAME, value),
                        UICulture = Thread.CurrentThread.CurrentCulture.Name,
                        Help = CodeLookup.GetLookupHelp(CUETEXT_CODELOOKUPGROUPNAME, value)
                    };
                }
            }
        }

        #endregion

        private void SetDataSource(object data)
        {
            if (data == null) return;
            ComboBox cbo = (ComboBox)_ctrl;
            cbo.DataSource = data;
            cbo.TextChanged -= new EventHandler(this.RaiseActiveChangedEvent);
        }

        private bool IsSelectedValueDifferentFromselectedvalue()
        {
            ComboBox cbo = (ComboBox)_ctrl;
            if (cbo.DropDownStyle == ComboBoxStyle.DropDown && cbo.DataSource == null)
            {
                if ((cbo.SelectedValue != null && cbo.SelectedValue.Equals(selectedvalue) == false)
                    || (selectedvalue != null && cbo.SelectedValue == null))
                    return true;
                else
                    return false;
            }
            else
            {
                if ((cbo.Text != null && cbo.Text.Equals(selectedvalue) == false)
                    || (selectedvalue != null && cbo.Text == null))
                    return true;
                else
                    return false;
            }
        }

        private void eComboBox2_VisibleChanged(object sender, EventArgs e)
        {
            ComboBox cbo = (ComboBox)_ctrl;
            if (this.Visible)
            {
                if (IsSelectedValueDifferentFromselectedvalue())
                {
                    if (cbo.Items.Count == 0)
                    {
                        if (cbo.DropDownStyle == ComboBoxStyle.DropDown)
                        {
                            if (cbo.DataSource == null)
                                cbo.Text = selectedvalue.ToString();
                            else
                                cbo.SelectedValue = selectedvalue;
                        }
                    }
                    else
                    {
                        if (cbo.DataSource == null)
                        {
                            if (selectedvalue != null)
                            {
                                cbo.Text = selectedvalue.ToString(); 
                            }
                        }
                        else
                        {
                            try
                            {
                                cbo.SelectedValue = selectedvalue;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        protected override void RaiseActiveChangedEvent(object sender, EventArgs e)
        {
            ComboBox cbo = (ComboBox)_ctrl;
            if (cbo.DataSource == null)
                selectedvalue = cbo.Text;
            else
                selectedvalue = cbo.SelectedValue;
            base.RaiseActiveChangedEvent(sender, e);
        }
        #endregion

        #region Design Mode
		protected override void omsDesignMode_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			System.Windows.Forms.ControlPaint.DrawBorder3D(e.Graphics, _ctrl.Bounds, Border3DStyle.Sunken, Border3DSide.All);
            e.Graphics.FillRectangle(SystemBrushes.Window,_ctrl.Bounds.X + 2,_ctrl.Bounds.Y + 2,_ctrl.Bounds.Width - 4,_ctrl.Bounds.Height - 4);
			System.Windows.Forms.ControlPaint.DrawComboButton(e.Graphics,_ctrl.Bounds.X + _ctrl.Bounds.Width - 19,_ctrl.Bounds.Y +2,17,_ctrl.Bounds.Height - 4,ButtonState.Normal);
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
                ComboBox cbo = (ComboBox)_ctrl;
                selectedvalue = value;

                if (this.Visible)
                {
                    if (cbo.Items.Count == 0)
                    {
                        if (cbo.DropDownStyle == ComboBoxStyle.DropDown)
                            cbo.Text = value.ToString();
                        else
                        {
                            if (cbo.DataSource == null)
                                cbo.Text = value.ToString();
                            else
                                cbo.SelectedValue = value;
                        }
                    }
                    else
                    {
                        if (cbo.DataSource == null)
                            cbo.Text = value.ToString();
                        else
                            cbo.SelectedValue = value;
                    }
                }

                if (this.Parent != null)
                {
                    IsDirty = true;
                    OnChanged();
                }
            }
        }
 		#endregion

		#region IListEnquiryControl Implementation
        /// <summary>
        /// Gets the ValueMember of the combo box.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get
            {
                ComboBox cbo = (ComboBox)_ctrl;
                return cbo.SelectedIndex;
            }
            set
            {
                ComboBox cbo = (ComboBox)_ctrl;
                if (cbo.SelectedIndex != value)
                    cbo.SelectedIndex = value;
            }
        }

        /// <summary>
        /// Gets the ValueMember of the combo box.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedValue
        {
            get
            {
                ComboBox cbo = (ComboBox)_ctrl;
                return cbo.SelectedValue;
            }
            set
            {
                ComboBox cbo = (ComboBox)_ctrl;
                if (cbo.SelectedValue != value)
                {
                    cbo.SelectedValue = value;
                }
            }
        }


        /// <summary>
        /// Gets the ValueMember of the combo box.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedItem
        {
            get
            {
                ComboBox cbo = (ComboBox)_ctrl;
                return cbo.SelectedItem;
            }
            set
            {
                ComboBox cbo = (ComboBox)_ctrl;
                if (cbo.SelectedItem != value)
                {
                    cbo.SelectedItem = value;
                }
            }
        }

		/// <summary>
		/// Gets or Sets the display text section of the combo box.
		/// </summary>
		[Browsable(false)]
		[DefaultValue("")]
		public object DisplayValue 
		{
			get
			{
				return  ((ComboBox)_ctrl).Text;
			}
			set
			{
				((ComboBox)_ctrl).Text = Convert.ToString(value);
			}
		}

		/// <summary>
		/// Assigns an individual bound value and a display value for a combo box.
		/// </summary>
		/// <param name="Value">Invisible bound value.</param>
		/// <param name="displayText">Visible display text.</param>
		public void AddItem(object Value, string displayText)
		{
			ComboBox cbo = (ComboBox)_ctrl;
			cbo.BeginUpdate();
			EnquiryListItem itm = new EnquiryListItem(Value, displayText);
			cbo.Items.Add(itm);
			cbo.EndUpdate();
		}

		/// <summary>
		/// Assigns a data table object to the datasource of the combo box which uses
		/// the first column as bound member, and the first or second column as the display
		/// member.
		/// </summary>
		/// <param name="dataTable">Bound data table.</param>
		public void AddItem(DataTable dataTable)
		{
			ComboBox cbo = (ComboBox)_ctrl;
			cbo.BeginUpdate();
			if (dataTable.Columns.Count > 0)
			{
				cbo.ValueMember = dataTable.Columns[0].ColumnName;
				if (dataTable.Columns.Count > 1)
					cbo.DisplayMember = dataTable.Columns[1].ColumnName;
				else
					cbo.DisplayMember = dataTable.Columns[0].ColumnName;
			}
			SetDataSource(dataTable);
			cbo.EndUpdate();
		}

		/// <summary>
		/// Assigns a data table object to the datasource of the combo box which uses
		/// the a specified value and display column
		/// </summary>
		/// <param name="dataTable">Bound data table.</param>
		/// <param name="valueMember">Column to be bound.</param>
		/// <param name="displayMember">Column to be displayed.</param>
		public void AddItem(DataTable dataTable, string valueMember, string displayMember)
		{
			ComboBox cbo = (ComboBox)_ctrl;
			cbo.BeginUpdate();
			if (dataTable.Columns.Contains(valueMember))
			{
				cbo.ValueMember = dataTable.Columns[valueMember].ColumnName;
			}
			if (dataTable.Columns.Contains(displayMember))
			{
				cbo.DisplayMember = dataTable.Columns[displayMember].ColumnName;
			}
            SetDataSource(dataTable);
            cbo.EndUpdate();
		}

        /// <summary>
        /// Assigns a data view object to the datasource of the combo box which uses
        /// the first column as the bound member, and the first or second column as the display
        /// member.
        /// </summary>
        /// <param name="dataView">Bound data view.</param>
        public void AddItem(DataView dataView)
		{
			
			if (dataView.Table != null)
			{
				ComboBox cbo = (ComboBox)_ctrl;
				cbo.BeginUpdate();
				if (dataView.Table.Columns.Count > 0)
				{
					cbo.ValueMember = dataView.Table.Columns[0].ColumnName;
					if (dataView.Table.Columns.Count > 1)
						cbo.DisplayMember = dataView.Table.Columns[1].ColumnName;
					else
						cbo.DisplayMember = dataView.Table.Columns[0].ColumnName;
				}
                SetDataSource(dataView);
                cbo.EndUpdate();
			}
		}

        /// <summary>
        /// Assigns a data view object to the datasource of the combo box which uses
        /// the a specified value and display column
        /// </summary>
        /// <param name="dataView">Bound data view.</param>
        /// <param name="valueMember">Column to be bound.</param>
        /// <param name="displayMember">Column to be displayed.</param>
        public void AddItem(DataView dataView, string valueMember, string displayMember)
		{
			
			if (dataView.Table != null)
			{
				ComboBox cbo = (ComboBox)_ctrl;
				cbo.BeginUpdate();
				if (dataView.Table.Columns.Contains(valueMember))
				{
					cbo.ValueMember = dataView.Table.Columns[valueMember].ColumnName;
				}
				if (dataView.Table.Columns.Contains(displayMember))
				{
					cbo.DisplayMember = dataView.Table.Columns[displayMember].ColumnName;
				}
                SetDataSource(dataView);
                cbo.EndUpdate();
			}
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
				return ((ComboBox)_ctrl).Items.Count;
			}
		}

		/// <summary>
		/// Filters the bound result set withe the specified Filter String
		/// </summary>
		/// <param name="FilterString">Filter String</param>
		public bool Filter (string FilterString)
		{
			DataTable dt = null;
			ComboBox cbo = (ComboBox)_ctrl;
			if (cbo.DataSource is DataView)
			{
				dt = ((DataView)cbo.DataSource).Table;
			}
			else if (cbo.DataSource is DataTable)
			{
				dt = (DataTable)cbo.DataSource;
			}

			if (dt != null)
			{
				DataView dv = new DataView(dt);
				dv.RowFilter = FilterString + " or " + cbo.ValueMember + " is null";;
				cbo.DataSource = dv;
			}
			if (cbo.Items.Count > 0)
				cbo.SelectedIndex = 0;
			return true;

		}

		/// <summary>
		/// Filters the bound result set with the specified value under the specified field.
		/// </summary>
		/// <param name="fieldName">The field name to filter with.</param>
		/// <param name="Value">The value to filter for.</param>
		public bool Filter (string fieldName, object Value)
		{
			// Removed Value == null from get out clause MNW
			if (fieldName == null) return false;
			ComboBox cbo = (ComboBox)_ctrl;
			DataTable dt = null;
			if (cbo.DataSource is DataView)
			{
				dt = ((DataView)cbo.DataSource).Table;
			}
			else if (cbo.DataSource is DataTable)
			{
				dt = (DataTable)cbo.DataSource;
			}

			if (dt != null)
			{
				try
				{
					DataView dv = new DataView(dt);
					string filter;
					// Check for NULL Value 
					if (Value != System.DBNull.Value)
						filter = fieldName + " = '" + Convert.ToString(Value).Replace("'", "''") + "' or " + cbo.ValueMember + " is null";
					else
						filter = fieldName + " is null";

					//Re-apply the value after the filter has been performed.
					object val = this.Value;
					dv.RowFilter = filter;
					cbo.DataSource = dv;
					this.Value = val;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message,Session.CurrentSession.Resources.GetMessage("ERRORSHOW","Error","").Text,MessageBoxButtons.OK,MessageBoxIcon.Error);
				}
			}
			return true;
		}

		#endregion

		#region ITextEditorEnquiryControl Implementation

		/// <summary>
		/// Gets or Sets the maximum length of the combo box text editor part of the control.
		/// </summary>
		[DefaultValue(255)]
		[Category("Behavior")]
		public int MaxLength
		{
			get
			{
				return ((ComboBox)_ctrl).MaxLength;
			}
			set
			{
				((ComboBox)_ctrl).MaxLength = value;
			}
		}

		#endregion

        #region Private

        private void cbo_LostFocus(object sender, EventArgs e)
        {
            if (fireleave == false)
            {
                cbo_Leave(sender, e);
            }
        }

        private void cbo_Leave(object sender, EventArgs e)
		{
            fireleave = true;
            // DMB 23/02/2004 added this clause to respond to the tabbing out of the control
			if(((ComboBox)_ctrl).DroppedDown)
			{
				((ComboBox)_ctrl).DroppedDown = false;
				this.RaiseActiveChangedEvent(sender,e);
			}
			this.RaiseLeaveEvent(sender,e);
        }
        #endregion
    }
}
