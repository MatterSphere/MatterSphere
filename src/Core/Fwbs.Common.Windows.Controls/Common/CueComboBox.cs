using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows.Common
{
    [DesignerCategory("Code")]
    public class CueComboBox : ComboBox
    {

        private const int CB_SETCUEBANNER = 0x1703;

        private const int VALUE_MEMBER_COLUMN_NUMBER = 0;

        private string _cueText = string.Empty;

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
        /// Gets or sets the text the <see cref="ComboBox"/> will display as a cue to the user.
        /// </summary>
        [Description("The text value to be displayed as a cue to the user.")]
        [Category("Appearance"), Localizable(true), DefaultValue("")]
        public string CueText
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

        public bool IsCueTextShown
        {
            get
            {
                return (SelectedIndex == -1 && HandleDbNullValue);
            }
        }

        public new object SelectedValue
        {
            get
            {
                if (SelectedIndex == -1 && HandleDbNullValue)
                {
                    return DBNull.Value;
                }
                return base.SelectedValue;
            }
            set
            {
                if (value == DBNull.Value && HandleDbNullValue)
                {
                    base.SelectedIndex = NullItemIndex;
                    base.ResetText();
                }
                else
                {
                    base.SelectedValue = value;
                }
            }
        }

        private int NullItemIndex
        {
            get
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (((DataRowView)Items[i]).Row[VALUE_MEMBER_COLUMN_NUMBER] == DBNull.Value)
                    {
                        return i;
                    }
                }

                return -1;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetCueBanner();
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (SelectedIndex == -1 && HandleDbNullValue)
            {
                SelectedValue = DBNull.Value;
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (MustShowPlaceHolder)
            {
                SelectedIndex = -1;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (MustShowPlaceHolder)
            {
                SelectedIndex = -1;
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (ShouldDrawCueText(e))
            {
                TextRenderer.DrawText(e.Graphics, CueText, Font, e.Bounds, SystemColors.GrayText, BackColor, TextFormatFlags.Top | TextFormatFlags.Left);
            }
            else
            {
                base.OnDrawItem(e);
            }
        }

        private void SetCueBanner()
        {
            if (IsHandleCreated)
            {
                var msg = new Message
                {
                    HWnd = Handle,
                    Msg = CB_SETCUEBANNER,
                    LParam = Marshal.StringToHGlobalUni(_cueText)
                };
                DefWndProc(ref msg);
                Marshal.FreeHGlobal(msg.LParam);
            }
        }

        private bool HandleDbNullValue
        {
            get
            {
                if (!string.IsNullOrEmpty(CueText) && DataSource != null)
                {
                    if (DataSourceContainsLessThanTwoRowsWithDbNullValue)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private bool MustShowPlaceHolder
        {
            get
            {
                if (SelectedValue is DBNull && !string.IsNullOrEmpty(CueText) && SelectedIndex == 0)
                {
                    return DataSourceContainsLessThanTwoRowsWithDbNullValue;
                }

                return false;
            }
        }

        private bool DataSourceContainsLessThanTwoRowsWithDbNullValue
        {
            get
            {
                var dv = DataSource as DataView;
                {
                    if (dv != null && ContainsLessThanTwoRowsWithDbNullValue(dv.Table))
                    {
                        return true;
                    }
                }
                var dt = DataSource as DataTable;
                {
                    if (dt != null && ContainsLessThanTwoRowsWithDbNullValue(dt))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private bool ContainsLessThanTwoRowsWithDbNullValue(DataTable dataTable)
        {
            return dataTable.Rows.Cast<DataRow>()
                .Count(r => r[VALUE_MEMBER_COLUMN_NUMBER] == DBNull.Value) <= 1;
        }

        private bool ShouldDrawCueText(DrawItemEventArgs e)
        {
            return DrawMode == DrawMode.OwnerDrawFixed && !Focused && SelectedIndex == -1 && e.Index == -1 && !string.IsNullOrEmpty(CueText);
        }
    }
}
