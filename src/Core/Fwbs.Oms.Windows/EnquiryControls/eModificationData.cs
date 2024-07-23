using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using FWBS.Common.UI;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A custom advanced enquiry control that shows information about creating and last updating.
    /// </summary>
    public class eModificationData : UserControl, IBasicEnquiryControl2, IFormatEnquiryControl, ISupportRightToLeft
	{
		#region Events
		[Category("Action")]
		public virtual event EventHandler Changed;

		[Category("Action")]
		public virtual event EventHandler ActiveChanged;
		#endregion

		#region Fields
		/// <summary>
		/// Holds the value of the control.
		/// </summary>
		private ModificationData _value;

		private bool _isdirty = false;
		#endregion

		#region Controls Specific Controls
		private ResourceLookup _res;
        private IContainer components;
        private TableLayoutPanel tableLayoutPanel1;
        private eTextBox2 _createdBy;
        private eTextBox2 _updatedBy;
        private CultureInfo _cultureInfo;
		#endregion
		
		#region Constructors
		public eModificationData() : base()
		{
			InitializeComponent();
            _cultureInfo = Session.CurrentSession.DefaultCultureInfo;
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._createdBy = new FWBS.Common.UI.Windows.eTextBox2();
            this._updatedBy = new FWBS.Common.UI.Windows.eTextBox2();
            this._res = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this._createdBy, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._updatedBy, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(280, 46);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // _createdBy
            // 
            this._createdBy.CaptionWidth = 150;
            this._createdBy.Dock = System.Windows.Forms.DockStyle.Fill;
            this._createdBy.IsDirty = false;
            this._createdBy.Location = new System.Drawing.Point(0, 0);
            this._res.SetLookup(this._createdBy, new FWBS.OMS.UI.Windows.ResourceLookupItem("CREATEDBY", "Created By :", ""));
            this._createdBy.Margin = new System.Windows.Forms.Padding(0);
            this._createdBy.MaxLength = 0;
            this._createdBy.Name = "_createdBy";
            this._createdBy.ReadOnly = true;
            this._createdBy.Size = new System.Drawing.Size(280, 23);
            this._createdBy.TabIndex = 0;
            this._createdBy.Text = "Created By :";
            // 
            // _updatedBy
            // 
            this._updatedBy.CaptionWidth = 150;
            this._updatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
            this._updatedBy.IsDirty = false;
            this._updatedBy.Location = new System.Drawing.Point(0, 23);
            this._res.SetLookup(this._updatedBy, new FWBS.OMS.UI.Windows.ResourceLookupItem("UPDATEDBY", "Updated By :", ""));
            this._updatedBy.Margin = new System.Windows.Forms.Padding(0);
            this._updatedBy.MaxLength = 0;
            this._updatedBy.Name = "_updatedBy";
            this._updatedBy.ReadOnly = true;
            this._updatedBy.Size = new System.Drawing.Size(280, 23);
            this._updatedBy.TabIndex = 0;
            this._updatedBy.Text = "Updated By :";
            // 
            // eModificationData
            // 
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "eModificationData";
            this.Size = new System.Drawing.Size(280, 46);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
        #endregion

        #region IBasicEnquiryControl2 Implementation
        private bool _omsdesignmode = false;

        [Browsable(false)]
        [DefaultValue(false)]
        public virtual bool omsDesignMode
        {
            get
            {
                return _omsdesignmode;
            }
            set
            {
                _omsdesignmode = value;
            }
        }

        [Browsable(false)]
		[DefaultValue(true)]
		public virtual bool LockHeight 
		{
			get
			{
				return false;
			}
		}

		[Browsable(true)]
		public object Control
		{
			get
			{
				return this;
			}
		}
		
		[DefaultValue(false)]
		[Category("Behavior")]
		public virtual bool Required 
		{
			get
			{
				return false;
			}
			set
			{ }
		}

		[DefaultValue(false)]
		[Category("Behavior")]
		public virtual bool ReadOnly 
		{
            get
            {
                return false;
            }
            set
            {}
		}

		[DefaultValue(150)]
        [Browsable(false)]
		public int CaptionWidth
		{
			get
			{
				return _createdBy.CaptionWidth;
			}
			set
			{
                _createdBy.CaptionWidth = value;
                _updatedBy.CaptionWidth = value;
            }
		}

		[Browsable(false)]
		public bool IsDirty
		{
			get
			{
				return _isdirty;
			}
			set
			{
				_isdirty = value;
			}
		}

		[Browsable(false)]
		public object Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (value is ModificationData)
				{
					_value = (ModificationData)value;

                    if (_value.Created.IsNull)
                        _createdBy.Value = _value.CreatedBy;
                    else
                    {
                        _createdBy.Value = String.Format(_cultureInfo, "{0} \t ({1})"
                            , _value.CreatedBy
                            , _value.Created.Kind == DateTimeKind.Unspecified 
                                ? _value.Created 
                                : _value.Created.ToLocalTime());
                    }

                    if (_value.Updated.IsNull)
                        _updatedBy.Value = _value.UpdatedBy;
                    else
                    {
                        _updatedBy.Value = String.Format(_cultureInfo, "{0} \t ({1})"
                            , _value.UpdatedBy
                            , _value.Created.Kind == DateTimeKind.Unspecified 
                                ? _value.Updated 
                                : _value.Updated.ToLocalTime());
                    }
				}
			}
		}

		public void OnChanged()
		{
            Changed?.Invoke(this, EventArgs.Empty);
        }

		public void OnActiveChanged()
		{
            ActiveChanged?.Invoke(this, EventArgs.Empty);
        }
		#endregion

		#region IFormatEnquiryControl Implementation
		public string Format
		{
			get
			{
                return string.Empty;
			}
			set
			{
			}
		}
        #endregion

        #region ISupportRightToLeft Implementation
        public void SetRTL(Form parentform)
        {
            _updatedBy.SetRTL(parentform);
            _createdBy.SetRTL(parentform);
        }
        #endregion

        /// <summary>
        /// Gets or Set a bool for the Caption location - on the top or not
        /// </summary>
        [Category("OMS Appearance")]
        [DefaultValue(false)]
        public virtual bool CaptionTop
        {
            get
            {
                return _createdBy.CaptionTop;
            }
            set
            {
                _createdBy.CaptionTop = _updatedBy.CaptionTop = value;
                _createdBy.CaptionWidth = _updatedBy.CaptionWidth = value ? 0 : 150;
                ValidateCtrlSizeOnMeasuredTextSize();
                this.Invalidate();
            }
        }

        #region Overrides
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            ValidateCtrlSizeOnMeasuredTextSize();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ValidateCtrlSizeOnMeasuredTextSize();
        }

        private void ValidateCtrlSizeOnMeasuredTextSize()
        {
            if (omsDesignMode)
            {
                var ph = PreferredHeight;
                if (Height < ph)
                {
                    Height = ph;
                }
            }
        }

        private int PreferredHeight
        {
            get
            {
                return _createdBy.PreferredHeight * 2;
            }
        }
        #endregion
    }
}