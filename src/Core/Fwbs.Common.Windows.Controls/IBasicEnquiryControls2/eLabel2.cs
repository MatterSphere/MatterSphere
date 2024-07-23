using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A read only enquiry control which generally holds a caption and a label.
    /// This particular control is a way of displaying read only information.
    /// </summary>
    public class eLabel2 : eBase2, IBasicEnquiryControl2, IFormatEnquiryControl
	{
		#region Fields

		/// <summary>
		/// The format string.
		/// </summary>
		private string _format = "";
		private object _value = null;
		private string _text = "";
		private bool _nowrap = false;
		
		#endregion

		#region Constructors

		/// <summary>
		/// Creates the a new label control as the internal editing control and it to the
		/// custom controls internal controls collection.
		/// </summary>
		public eLabel2() : base()
		{
		}

		#endregion

		#region IBasicEnquiryControl2 Implementation

		/// <summary>
		/// Gets or Sets the readonly state of the label.  Although a label is read only anyway, 
		/// this property still toggles the enabled state of the label rather than the whole custom control.
		/// This will appear greyed out.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
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
		/// Gets or Sets the value property of the enquiry control.
		/// </summary>
		[DefaultValue("")]
		public override object Value 
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;

                //DM - 30/11/06
                //UTC Date Fix
                if (_value is DateTime)
                {
                    DateTime dtevalue = (DateTime)_value;
                    if (dtevalue.Kind != DateTimeKind.Unspecified)
                        _value = dtevalue.ToLocalTime();
                }
                else if (_value is DateTimeNULL)
                {
                    DateTimeNULL dtevalue = (DateTimeNULL)_value;
                    if (dtevalue.Kind != DateTimeKind.Unspecified)
                        _value = dtevalue.ToLocalTime();
                }


				try
				{
					_text = FWBS.Common.Format.GetFormattedValue(value, _format);
				}
				catch
				{
					_text = Convert.ToString(value);
				}
				if (this.Parent != null)
				{
					IsDirty=true;
					OnChanged();
					this.Invalidate();
				}
			}
		}

		#endregion

		#region IFormatEnquiryControl Implementation

		/// <summary>
		/// Gets or Sets the Format string of the contorl.
		/// </summary>
		[DefaultValue("")]
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

        #endregion

        #region	Properties
        /// <summary>
        /// Gets or Sets the flag which disables the text wrapping between lines.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool NoWrap
        {
            get
            {
                return _nowrap;
            }
            set
            {
                _nowrap = value;
                if (omsDesignMode)
                    Invalidate();
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

        public override int PreferredHeight
        {
            get
            {
                using (var graphics = CreateGraphics())
                {
                    return graphics.MeasureString(Text, Font).ToSize().Height;
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!string.IsNullOrWhiteSpace(_text))
            {
                // ********************************************************************************************
                // * Sets the Text Format Settings
                // ********************************************************************************************
                StringFormat sf = new StringFormat() { FormatFlags = StringFormatFlags.NoClip };
                if (NoWrap)
                {
                    sf.FormatFlags |= StringFormatFlags.NoWrap;
                }
                if (this.RightToLeft == RightToLeft.Yes)
                {
                    sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                }
                sf.Trimming = StringTrimming.EllipsisWord;
                // ********************************************************************************************
                // * Calculate rectangle label
                // ********************************************************************************************
                int labelStartX = LogicalToDeviceUnits(_captionWidth);
                int labelWidth = this.Width - labelStartX;
                Rectangle pntarea = new Rectangle(this.RightToLeft == RightToLeft.Yes ? 0 : labelStartX, 0, labelWidth, this.Height);
                // ********************************************************************************************
                // * Draw caption string
                // ********************************************************************************************
                using (SolidBrush br = new SolidBrush(this.ForeColor))
                {
                    e.Graphics.DrawString(_text, this.Font, br, pntarea, sf);
                }
            }
        }
	}
}
