using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public class eNumbersOnly : eBase2, IBasicEnquiryControl2, ITextEditorEnquiryControl, ICharacterCasingControl
    {
        private class NumbersTextBox : TextBox
        {
            protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
            {
                base.SetBoundsCore(x, System.Math.Max(0, y), width, height, specified);
            }
        }

        #region Fields
        private bool _readonly = false;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Calls the base constructor and applies its own properties after creating the internal 
		/// editing control, which in this case is a textbox.
		/// </summary>
        public eNumbersOnly()
            : base()
		{
			//New textbox.
            TextBox txt = new NumbersTextBox();
			//Use the lost focus event as the change event.
			txt.BackColor = SystemColors.Window;
			txt.TextChanged +=new EventHandler(RaiseActiveChangedEvent);
            txt.TextChanged += new EventHandler(txt_TextChanged);
            txt.KeyPress += new KeyPressEventHandler(txt_KeyPress);
			//User the existing leave event as the new leave event.
			txt.Leave += new System.EventHandler(this.RaiseLeaveEvent);
			txt.GotFocus += new System.EventHandler(this.RaiseGotFocusEvent);
            txt.ReadOnlyChanged += new EventHandler(txt_ReadOnlyChanged);
            txt.BackColorChanged += new EventHandler(txt_BackColorChanged);
			//Assign the protected base item to the new text box.
			_ctrl = txt;
			//Give default values and then add control to the base controls collection.
			_ctrl.TabIndex = 0;
			Controls.Add(_ctrl);
		}

        void txt_TextChanged(object sender, EventArgs e)
        {
            string vals = "";
            if (afterpaste)
            {
                foreach (char item in ((TextBox)_ctrl).Text.ToCharArray())
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(item.ToString(), "\\d+"))
                        vals += item.ToString();
                }
                ((TextBox)_ctrl).Text = vals;
                ((TextBox)_ctrl).SelectionStart = ((TextBox)_ctrl).Text.Length;
                afterpaste = false;
            }
        }

        private const Char backspace = (Char)8;
        private const Char copy = (Char)3;
        private const Char paste = (Char)22;
        private bool afterpaste = false;
        
        void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case copy:
                case backspace:
                    return;
                case paste:
                    afterpaste = true;
                    return;
                default:
                    if (System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), "\\d+"))
                        return;
                    e.Handled = true;
                    return;
            }

        }

        void txt_BackColorChanged(object sender, EventArgs e)
        {
            TextBox txt = _ctrl as TextBox;

            if (txt == null)
                return;

            if (this.Enabled && !txt.ReadOnly)
                _ctrl.BackColor = SystemColors.Window;
            else if(txt.ReadOnly)
                _ctrl.BackColor = Color.FromArgb(240,240,240);
            else
                _ctrl.BackColor = SystemColors.ControlLight;
        }

        void txt_ReadOnlyChanged(object sender, EventArgs e)
        {
            TextBox txt = _ctrl as TextBox;

            if (txt == null)
                return;

            if (txt.ReadOnly)
                txt.BackColor = SystemColors.ControlDark;
            else
                txt.BackColor = SystemColors.ControlLight;
        }

        #endregion

        #region	Properties
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

		#region IBasicEnquiryControl2 Implementation

		/// <summary>
		/// Gets or Sets the readonly property.  For the eTextBox, the readonly property is toggled.
		/// </summary>
		[Category("Behavior")]
		public override bool ReadOnly
		{
			get
			{
				return _readonly;
			}
			set
			{
				_readonly = value;
				((TextBox)_ctrl).ReadOnly = value;

			}
		}


		/// <summary>
		/// Gets or Sets the value of the control.
		/// </summary>
		[DefaultValue("")]
		public override object Value 
		{
			get
			{
				return Convert.ToString(((TextBox) _ctrl).Text);
			}
			set
			{
				_ctrl.TextChanged -= new EventHandler(RaiseActiveChangedEvent);
				try
				{
                    //DM - 30/11/06
                    //UTC Date Fix
                    if (value is DateTime)
                    {
                        DateTime dtevalue = (DateTime)value;
                        if (dtevalue.Kind != DateTimeKind.Unspecified)
                            value = dtevalue.ToLocalTime();
                    }
                    else if (value is DateTimeNULL)
                    {
                        DateTimeNULL dtevalue = (DateTimeNULL)value;
                        if (dtevalue.Kind != DateTimeKind.Unspecified)
                            value= dtevalue.ToLocalTime();
                    }

					((TextBox)_ctrl).Text = Convert.ToString(value);
				}
				catch
				{
					((TextBox)_ctrl).Text= "";
				}
				_ctrl.TextChanged += new EventHandler(RaiseActiveChangedEvent);
				if (this.Parent != null)
				{
					IsDirty=true;
					OnChanged();
				}
			}
		}

		#endregion

		#region ITextEnquiryControl Implementation

		/// <summary>
		/// Gets or Sets the maximum length property of the eTextBox.
		/// </summary>
		[Category("Behavior")]	
		public int MaxLength
		{
			get
			{
				return ((TextBox)_ctrl).MaxLength;
			}
			set
			{
				((TextBox)_ctrl).MaxLength = value;
			}
		}



		#endregion

		#region ICharacterCasingControl Implementation

		/// <summary>
		/// Gets or Sets the character casing of the control.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(CharacterCasing.Normal)]
		public CharacterCasing Casing
		{
			get
			{
				return ((TextBox)_ctrl).CharacterCasing;
			}
			set
			{
				((TextBox)_ctrl).CharacterCasing=value;
			}
		}

		#endregion

    }
}
