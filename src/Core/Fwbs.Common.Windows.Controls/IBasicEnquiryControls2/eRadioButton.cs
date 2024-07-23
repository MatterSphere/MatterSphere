using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{

    /// <summary>
    /// A check box enquiry control which generally holds a radio button, the caption is already built
    /// into the radio buttoncontrol.  This control uses a radio button as its base control rather than eBase.
    /// This particular control is a way of displaying True / False flag information.
    /// </summary>
    public class eRadioButton : RadioButton,  IBasicEnquiryControl2
	{
		#region Events

		/// <summary>
		/// The changed event is used to determine when a major change has happended within the
		/// user control.  This will tend to be used when the internal editing control has changed
		/// in some way or another.
		/// </summary>
		[Category("Action")]
		public event EventHandler Changed;

        /// <summary>
        /// Occurs when [active changed].
        /// </summary>
		[Category("Action")]
		public event EventHandler ActiveChanged;

		#endregion

		#region Fields

		/// <summary>
		/// Marks the editing control as required.
		/// </summary>
		private bool _required = false;

		/// <summary>
		/// Stores how wide the caption should be.
		/// </summary>
		protected int _captionWidth = 150;

        /// <summary>
        /// Radio button size on standard DPI (96)
        /// </summary>
        private const int _defaultRadioButtonSize = 14;

        /// <summary>
        /// Holds the boolean value for the read only attribute.
        /// </summary>
        private bool _readonly = false;

		private bool _isdirty = false;

        #endregion

		#region Design Mode
		private bool _omsdesignmode = false;
		[Browsable(false)]
		public bool omsDesignMode
		{
			get
			{
				return _omsdesignmode;
			}
			set
			{
				_omsdesignmode = value;
				if (value)
				{
					this.FlatStyle=FlatStyle.Standard;
					this.Paint += this.omsDesignMode_Paint;
					this.Resize += this.omsDesignMode_Resize;
				}
			}
		}

        /// <summary>
        /// Paints overlapping radio button in design mode only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void omsDesignMode_Paint(object sender, PaintEventArgs e)
		{
            if (this.CheckAlign == ContentAlignment.MiddleLeft || this.CheckAlign == ContentAlignment.MiddleRight)
            {
                var radioButtonSize = LogicalToDeviceUnits(_defaultRadioButtonSize);
                int y = (this.Height - radioButtonSize) / 2;
                int x = this.Width - radioButtonSize;
                if (this.CheckAlign == ContentAlignment.MiddleLeft) x = 0;
                ControlPaint.DrawRadioButton(e.Graphics, x, y, radioButtonSize, radioButtonSize, ButtonState.Normal);
            }
        }

		private void omsDesignMode_Resize(object sender, EventArgs e)
		{
			this.Invalidate();
		}
		#endregion

		#region Properties
		[Browsable(true)]
		public object Control
		{
			get
			{
				return this;
			}
		}

        /// <summary>
        /// Overrides RadioButton alignment. Only MiddleLeft or MiddleRight allowed
        /// </summary>
        public new ContentAlignment CheckAlign
        {
            get
            {
                return base.CheckAlign;
            }
            set
            {
                if (value == ContentAlignment.MiddleLeft || value == ContentAlignment.MiddleRight)
                {
                    base.CheckAlign = value;
                }
            }
        }

        #endregion

        #region Constructors

        public eRadioButton() : base()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
            // 
            // eRadioButton
            // 
            this.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Click += this.RaiseActiveChangedEvent;
            this.Leave += this.RaiseChangedEvent;
        }

		#endregion

		#region IBasicEnquiryControl2 Implementation

		/// <summary>
		/// Gets whether the current control can be stretched by its Y co-ordinate.
		/// This is a design mode property and is set to true.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(true)]
		public bool LockHeight 
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets or Sets the value of the control.  In this case it should be True of False.
		/// </summary>
		[DefaultValue(false)]
		public object Value
		{
			get
			{
				return Checked;
			}
			set
			{
				try
				{
					Checked = Convert.ToBoolean(value);
				}
				catch
				{
					Checked = false;
				}
				if (this.Parent != null)
				{
					IsDirty=true;
					OnChanged();
				}
			}
		}

		/// <summary>
		/// Gets or Sets the flag that tells the rendering form that this control is required
		/// to be filled incase the underlying value is DBNull.
		/// </summary>
		[DefaultValue(false)]
		[Category("Behavior")]
		public bool Required 
		{
			get
			{
				return _required;
			}
			set
			{
				_required = value;
			}
		}

		/// <summary>
		/// Gets or Sets the editable format of the control.
		/// </summary>
		[DefaultValue(false)]
		[Category("Behavior")]
		public bool ReadOnly 
		{
			get
			{
				return _readonly;
			}
			set
			{
				_readonly = value;
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

		/// <summary>
		/// Gets or Sets the caption width of a control, leaving the rest of the width of the control
		/// to be the width of the internal editing control.
		/// </summary>
		[DefaultValue(150)]
        [Browsable(false)]
		public virtual int CaptionWidth
		{
			get
			{
				return _captionWidth;
			}
			set
			{
			}
		}

        [Browsable(false)]
        public bool CaptionTop
        {
            get
            {
                return false;
            }
            set { }
        }

		/// <summary>
		/// Executes the changed event.
		/// </summary>
		public void OnChanged()
		{
			if (Changed!= null && IsDirty)
				Changed(this, EventArgs.Empty);
			IsDirty=false;
		}

		public void OnActiveChanged()
		{
			IsDirty = true;
			if (ActiveChanged != null)
				ActiveChanged(this,EventArgs.Empty);
		}

        #endregion

        #region Methods

        /// <summary>
        /// Raises the changed event within the base control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RaiseActiveChangedEvent(object sender, EventArgs e)
		{
			if (_readonly && this.Focused) 
			{
				if (this.Checked == false) this.Checked = true; else this.Checked=false;
			}
			else
			{
				OnActiveChanged();
			}
		}

		private void RaiseChangedEvent(object sender, EventArgs e)
		{
			OnChanged();
		}

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (omsDesignMode && this.Height < this.PreferredSize.Height)
            {
                this.Height = this.PreferredSize.Height;
            }
        }

        /// <summary>
        /// Raises the leave event within the base control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RaiseLeaveEvent(object sender, EventArgs e)
		{
		}


		#endregion

	}
}
