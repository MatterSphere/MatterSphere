using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using FWBS.OMS;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A simple text editor enquiry control which generally holds a caption and a text box.  
    /// This particular control can be used for simple text editing.
    /// </summary>
    public class eTextBox2 : eBase2, IBasicEnquiryControl2, ITextEditorEnquiryControl, ICharacterCasingControl
	{

        private const string CUETEXT_CODELOOKUPGROUPNAME = "ENQQUESTCUETXT";

        #region Fields

        private bool _readonly = false;
		private string _acceptedChars = "";
        private CodeLookupDisplay _cueText;
        private string _cueTextCode;

        #endregion

        #region Constructors

        /// <summary>
        /// Calls the base constructor and applies its own properties after creating the internal 
        /// editing control, which in this case is a textbox.
        /// </summary>
        public eTextBox2() : base()
        {
            //New textbox.
            MaskedEdit txt = new MaskedEdit();
            //Use the lost focus event as the change event.
            txt.BackColor = SystemColors.Window;
            txt.TextChanged +=new EventHandler(RaiseActiveChangedEvent);
            txt.InputMask = "";
            txt.InputChar = '_';
            //User the exisitng leave event as the new leave event.
            txt.Leave += new System.EventHandler(this.RaiseLeaveEvent);
            txt.GotFocus += new System.EventHandler(this.RaiseGotFocusEvent);
            txt.KeyDown +=new KeyEventHandler(txt_KeyDown);
            txt.KeyPress +=new KeyPressEventHandler(txt_KeyPress);
            txt.KeyUp +=new KeyEventHandler(txt_KeyUp);
            txt.ReadOnlyChanged += new EventHandler(txt_ReadOnlyChanged);
            txt.BackColorChanged += new EventHandler(txt_BackColorChanged);
            //Assign the protected base item to the new text box.
			_ctrl = txt;
			//Give default values and then add control to the base controls collection.
			_ctrl.TabIndex = 0;
			Controls.Add(_ctrl);
		}

        protected override void RaiseActiveChangedEvent(object sender, System.EventArgs e)
        {
            base.OnActiveChanged();
            base.OnChanged();
        }

        void txt_BackColorChanged(object sender, EventArgs e)
        {
            TextBox txt = _ctrl as TextBox;

            if (txt == null)
                return;

            if (this.Enabled && !txt.ReadOnly)
                _ctrl.BackColor = SystemColors.Window;
            else if (txt.ReadOnly)
                _ctrl.BackColor = Color.FromArgb(244, 244, 244);
            else
                _ctrl.BackColor = SystemColors.ControlLight;
        }

        void txt_ReadOnlyChanged(object sender, EventArgs e)
        {
            TextBox txt = _ctrl as TextBox;

            if (txt != null)
            {
                txt.BackColor = txt.ReadOnly ? SystemColors.ControlDark : SystemColors.ControlLight;
            }
        }

        #endregion

        #region Masked

        [Category("Mask")]
		[DefaultValue("")]
		public string InputMask
		{
			get
			{
				return ((MaskedEdit)_ctrl).InputMask;
			}
			set
			{
				((MaskedEdit)_ctrl).InputMask = value ?? string.Empty;
				((MaskedEdit)_ctrl).StdInputMask = string.IsNullOrEmpty(value)
                    ? MaskedEdit.InputMaskType.None
                    : MaskedEdit.InputMaskType.Custom;
			}
		}

		[Category("Mask")]
		[DefaultValue('_')]
		public char InputChar
		{
			get
			{
				return ((MaskedEdit)_ctrl).InputChar;
			}
			set
			{
				((MaskedEdit)_ctrl).InputChar = value;
			}
		}

		[Category("Mask")]
		[DefaultValue("")]
		public string AcceptedChars
		{
			get
			{
				return _acceptedChars;
			}
			set
			{
				_acceptedChars = value;
                MaskedEdit txt = _ctrl as MaskedEdit;
                if (txt != null)
                {
                    txt.TextPaste -= txt_TextPaste;
                    if (!string.IsNullOrEmpty(_acceptedChars))
                        txt.TextPaste += txt_TextPaste;
                }
			}
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
				return Convert.ToString(((MaskedEdit) _ctrl).Value);
			}
			set
			{
				_ctrl.TextChanged -= new EventHandler(RaiseActiveChangedEvent);
				try
				{
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

					((MaskedEdit)_ctrl).Value = Convert.ToString(value);
				}
				catch
				{
					((MaskedEdit)_ctrl).Value = "";
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
				return ((MaskedEdit)_ctrl).CharacterCasing;
			}
			set
			{
				((MaskedEdit)_ctrl).CharacterCasing=value;
			}
		}

        #endregion

        #region CueText Implementation

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
                    ((MaskedEdit)_ctrl).CueText = _cueText.Description;
                    IsDirty = true;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string CueTextCode
        {
            get
            {
                return _cueTextCode;
            }
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

        #region Key Events
        [Category("Key")]
		[Browsable(true)]
		public new event KeyEventHandler KeyDown;
		[Category("Key")]
		[Browsable(true)]
		public new event KeyEventHandler KeyUp;
		[Category("Key")]
		[Browsable(true)]
		public new event KeyPressEventHandler KeyPress;

		private void txt_KeyDown(object sender, KeyEventArgs e)
		{
			if (KeyDown != null) KeyDown(this,e);
		}

		private void txt_KeyPress(object sender, KeyPressEventArgs e)
		{
            //"CTRL + V" (Key Character 22) to be ignored from accepted characters routine to allow Paste. 
            if (e.KeyChar >= 20 && e.KeyChar != 22)
			{
				if (_acceptedChars.Length > 0)
				{
					if (_acceptedChars.IndexOf(e.KeyChar) < 0)
					{
						e.Handled = true;
						return;
					}
				}
			}

			if (KeyPress != null) KeyPress(this,e);
		}

		private void txt_KeyUp(object sender, KeyEventArgs e)
		{
			if (KeyUp != null) KeyUp(this,e);
		}
        #endregion

        private void txt_TextPaste(object sender, CancelEventArgs e)
        {
            string clipText;
            try { clipText = Clipboard.GetText(); } catch { clipText = string.Empty; }

            if (clipText.Length > 0)
            {
                e.Cancel = true;
                // Check if data to be pasted is acceptable, filter out unwanted characters.
                var acceptedChars = new System.Collections.Generic.HashSet<char>(_acceptedChars);
                clipText = string.Concat(clipText.Where(c => acceptedChars.Contains(c)));

                if (clipText.Length != 0)
                {
                    MaskedEdit edit = (MaskedEdit)sender;
                    int selStart = edit.SelectionStart;
                    int selLen = edit.SelectionLength;
                    string text = edit.Text;

                    // Assemble new text
                    text = text.Substring(0, selStart) + clipText + text.Substring(selStart + selLen);

                    if (text.Length > edit.MaxLength)
                        text = text.Remove(edit.MaxLength);

                    edit.Text = text;
                }
            }
        }
    }
}
