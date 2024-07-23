using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A number picker enquiry control which generally holds a caption and a spin button.  
    /// This particular control can be used for picking valid numbers between certain numbers.
    /// </summary>
    public class eSpin2 : eBase2, IBasicEnquiryControl2
	{

        private class Spin : NumericUpDown
        {
            protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
            {
                base.SetBoundsCore(x, System.Math.Max(0, y), width, height, specified);
            }
        }

        #region Constructors

        /// <summary>
        /// Creates a new spin button control and assigns it to the base control object.
        /// </summary>
        public eSpin2() : base()
		{
			NumericUpDown spin = new Spin();
            //Capture the following events and raise it as the changed event.
            spin.ValueChanged += new System.EventHandler(this.RaiseActiveChangedEvent);
			spin.KeyPress +=new KeyPressEventHandler(spin_KeyPress);
			spin.KeyUp +=new KeyEventHandler(spin_KeyUp); 
			spin.Leave +=new EventHandler(this.RaiseLeaveEvent);
			//Capture the following event and raise it as the changed event.
			spin.GotFocus +=new EventHandler(spin_GotFocus);
			spin.Maximum = 3000;
			spin.Minimum = 0;
			spin.Increment = 1;
            _ctrl = spin;
			_ctrl.TabIndex = 0;
			Controls.Add(_ctrl);
		}

        #endregion

        #region IBasicEnquiryControl2 Implementation

        /// <summary>
        /// Gets or Sets the value within the control.
        /// </summary>
        [DefaultValue(0)]
		public override object Value
		{
			get
			{
				return ((NumericUpDown)_ctrl).Value.ToString();;
			}
			set
			{
				((NumericUpDown)_ctrl).ValueChanged -= new System.EventHandler(this.RaiseActiveChangedEvent);
				try
				{
					if (value != DBNull.Value)
						((NumericUpDown)_ctrl).Text = value.ToString();
					else
						((NumericUpDown)_ctrl).Text = "0";
				}
				catch
				{
					((NumericUpDown)_ctrl).Text = "0";
				}
				((NumericUpDown)_ctrl).ValueChanged += new System.EventHandler(this.RaiseActiveChangedEvent);
				if (this.Parent != null)
				{
					IsDirty=true;
					OnChanged();
				}
			}

		}

		[DefaultValue(typeof(decimal),"0")]
		[Category("Data")]
		public decimal Minimum
		{
			get
			{
				return ((NumericUpDown)_ctrl).Minimum;
			}
			set
			{
				((NumericUpDown)_ctrl).ValueChanged -= new System.EventHandler(this.RaiseActiveChangedEvent);
				((NumericUpDown)_ctrl).Minimum = value;
				((NumericUpDown)_ctrl).ValueChanged += new System.EventHandler(this.RaiseActiveChangedEvent);
			}
		}

		[DefaultValue(typeof(decimal),"3000")]
		[Category("Data")]
		public decimal Maximum
		{
			get
			{
				return ((NumericUpDown)_ctrl).Maximum;
			}
			set
			{
				((NumericUpDown)_ctrl).ValueChanged -= new System.EventHandler(this.RaiseActiveChangedEvent);
				((NumericUpDown)_ctrl).Maximum = value;
				((NumericUpDown)_ctrl).ValueChanged += new System.EventHandler(this.RaiseActiveChangedEvent);
			}
		}

		[DefaultValue(typeof(int),"0")]
		[Category("Data")]
		public int DecimalPlaces
		{
			get
			{
				return ((NumericUpDown)_ctrl).DecimalPlaces;
			}
			set
			{
				((NumericUpDown)_ctrl).DecimalPlaces = value;
			}
		}

		[DefaultValue(typeof(decimal),"1")]
		[Category("Data")]
		public decimal Increment
		{
			get
			{
				return ((NumericUpDown)_ctrl).Increment;
			}
			set
			{
				((NumericUpDown)_ctrl).Increment = value;
			}
		}
        #endregion

        private void spin_KeyPress(object sender, KeyPressEventArgs e)
		{
		}

		private void spin_KeyUp(object sender, KeyEventArgs e)
		{
			OnActiveChanged();
		}

		private void spin_GotFocus(object sender, EventArgs e)
		{
			((NumericUpDown)_ctrl).Select(0,_ctrl.Text.Length);
			this.RaiseGotFocusEvent(sender,e);
		}
	}
}
