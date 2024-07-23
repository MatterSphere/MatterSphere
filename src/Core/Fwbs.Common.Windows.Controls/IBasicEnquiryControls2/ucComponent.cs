using System;
using System.ComponentModel;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A read only enquiry control which generally holds a caption and a label.
    /// This particular control is a way of displaying read only information.
    /// </summary>
    public class eComponent : eBase2, IBasicEnquiryControl2
	{
		#region Constructors

		private object _value;
		private bool _readonly;
		private bool _updating = false;
		/// <summary>
		/// Creates the a new label control as the internal editing control and it to the
		/// custom controls internal controls collection.
		/// </summary>
		public eComponent() : base()
		{
			_ctrl=null;
			base.Visible=false;
			this.VisibleChanged +=new EventHandler(eComponent_VisibleChanged);
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
				return _readonly;
			}
			set
			{
				_readonly = value;
			}
		}


		public override bool omsDesignMode
		{
			set
			{
				base.omsDesignMode =value;
				if (value)
				{
					this.BackColor = System.Drawing.SystemColors.Info;
					this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
					this.Width = 100;
					System.Windows.Forms.PictureBox _myctrl = new System.Windows.Forms.PictureBox();
					_myctrl.Width=16;
					_myctrl.Height=16;
					_myctrl.Anchor = System.Windows.Forms.AnchorStyles.None;
					_myctrl.Dock = System.Windows.Forms.DockStyle.Right;
					_myctrl.TabIndex = 0;
					Controls.Add(_myctrl );
					base.Visible=true;
					this.Paint += new System.Windows.Forms.PaintEventHandler(this.eComponent_Paint);
				}
				else
				{
					Controls.Clear();
					this.Width=0;
					base.Visible=false;
				}
			}
			get
			{
				return base.omsDesignMode;
			}
		}

		/// <summary>
		/// Gets or Sets the value property of the enwuiry control.
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
				if (this.Parent != null)
				{
					IsDirty=true;
					OnChanged();
				}
			}
		}

        #endregion

        #region Properties
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

        private void eComponent_VisibleChanged(object sender, EventArgs e)
		{
			if (!base.omsDesignMode)
			{
				_updating=true;
				this.Visible=false;
				_updating=false;
			}
			else if (this.Visible==false && base.omsDesignMode && _updating==false)
			{
				_updating=true;
				this.Visible=true;
				_updating=false;
			}

		}

		private void eComponent_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
            using (System.Drawing.SolidBrush b1 = new System.Drawing.SolidBrush(this.ForeColor))
            {
                e.Graphics.DrawString(this.Name, this.Font, b1, CaptionWidth, 2);
            }
		}
	}
}
