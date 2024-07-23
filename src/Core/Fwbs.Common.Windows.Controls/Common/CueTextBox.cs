using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows.Common
{
    [DesignerCategory("Code")]
    public class CueTextBox : TextBox
    {
        private const int GWL_STYLE = -16;
        private const int ES_UPPERCASE = 0x0008;
        private const int ES_LOWERCASE = 0x0010;

        private const int WM_PAINT = 0x000F;
        private const int EM_SETCUEBANNER = 0x1501;

        private string _cueText = string.Empty;

        private CharacterCasing _originalCharacterCasing = CharacterCasing.Normal;

        /// <summary>
        /// Occurs when the <see cref="CueText"/> property value changes.
        /// </summary>
        public event EventHandler CueTextChanged;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnCueTextChanged(EventArgs e)
        {
            CueTextChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Gets or sets the text the <see cref="TextBox"/> will display as a cue to the user.
        /// </summary>
        [Description("The text value to be displayed as a cue to the user.")]
        [Category("Appearance"), Localizable(true), DefaultValue("")]
        public virtual string CueText
        {
            get { return _cueText; }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                if (_cueText.Equals(value, StringComparison.CurrentCulture)) return;

                _cueText = value;

                SetCueBanner();
                OnCueTextChanged(EventArgs.Empty);
            }
        }
        
        [Browsable(false)]
        [DefaultValue(false)]
        public bool HandleEnterKey { get; set; }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetCueBanner();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT && Multiline)
            {
                if (!Focused && string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(CueText))
                {
                    using (var g = CreateGraphics())
                    {
                        TextRenderer.DrawText(g, CueText, Font, ClientRectangle, SystemColors.GrayText, BackColor, TextFormatFlags.Top | TextFormatFlags.Left);
                    }
                }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            return (HandleEnterKey && keyData == Keys.Return) || base.IsInputKey(keyData);
        }

        private void SetCueBanner()
        {
            if (IsHandleCreated)
            {
                var msg = new Message
                {
                    HWnd = Handle,
                    Msg = EM_SETCUEBANNER,
                    LParam = Marshal.StringToHGlobalUni(_cueText)
                };
                DefWndProc(ref msg);
                Marshal.FreeHGlobal(msg.LParam);
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (_originalCharacterCasing != CharacterCasing.Normal)
            {
                ModifyCharacterCasing(_originalCharacterCasing);
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (_originalCharacterCasing != CharacterCasing.Normal)
            {
                ModifyCharacterCasing(CharacterCasing.Normal);
            }
        }

        private void ModifyCharacterCasing(CharacterCasing casing)
        {
            int style = GetWindowLong(Handle, GWL_STYLE) & ~(ES_UPPERCASE | ES_LOWERCASE);
            if (casing == CharacterCasing.Upper)
                style |= ES_UPPERCASE;
            else if (casing == CharacterCasing.Lower)
                style |= ES_LOWERCASE;
            SetWindowLong(Handle, GWL_STYLE, style);
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    switch (_originalCharacterCasing)
                    {
                        case CharacterCasing.Upper:
                            value = value.ToUpper();
                            break;
                        case CharacterCasing.Lower:
                            value = value.ToLower();
                            break;
                    } 
                }
                base.Text = value;
            }
        }
        
        public new CharacterCasing CharacterCasing
        {
            get { return _originalCharacterCasing; }
            set { _originalCharacterCasing = value; }
        }

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    }
}
