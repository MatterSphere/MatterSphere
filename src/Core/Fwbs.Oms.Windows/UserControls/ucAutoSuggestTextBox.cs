using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using FWBS.Common.UI;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    public class ucAutoSuggestTextBox : eTextBox2, IListEnquiryControl
    {
        #region Events
        public event EventHandler AutoSuggestCompleted;
        #endregion

        #region Fields
        private ListBox _listBoxSuggestions;
        private DataTable _dataSource;
        private int _columnIndexInSource = 0;
        private int _indexBefore = -1;
        #endregion

        #region Consts
        private const string TextForSizing = "Gg";
        #endregion

        #region Constructors
        public ucAutoSuggestTextBox()
        {
            this.SuggestedDataRow = null;

            KeyUp += textBoxInput_KeyUp;
            KeyDown += textBoxInput_KeyDown;
            _ctrl.LostFocus += _ctrl_LostFocus;

            AddListBox();
        }
        #endregion

        #region Properties
        [Browsable(false)]
        public DataRow SuggestedDataRow { get; private set; }

        [Browsable(false)]
        public object DisplayValue
        {
            get { return _ctrl.Text; }

            set { _ctrl.Text = Convert.ToString(value); }
        }

        [Browsable(false)]
        public int Count => _dataSource?.Rows.Count ?? 0;

        #endregion

        #region Methods
        private void AddListBox()
        {
            _listBoxSuggestions = new ListBox
            {
                DrawMode = DrawMode.OwnerDrawVariable,
                IntegralHeight = false,
                Height = 100,
                Visible = false,
            };

            _listBoxSuggestions.Font = this.Font;
            _listBoxSuggestions.TabStop = false;

            _listBoxSuggestions.DrawItem += _listBoxSuggestions_DrawItem;
            _listBoxSuggestions.KeyDown += _listBoxSuggestions_KeyDown;
            _listBoxSuggestions.LostFocus += _listBoxSuggestions_LostFocus;
            _listBoxSuggestions.MeasureItem += _listBoxSuggestions_MeasureItem;
            _listBoxSuggestions.MouseClick += _listBoxSuggestions_MouseClick;
            _listBoxSuggestions.MouseMove += _listBoxSuggestions_MouseMove;
            _listBoxSuggestions.SelectedIndexChanged += _listBoxSuggestions_SelectedIndexChanged;
        }

        private void ShowSuggestionsList()
        {
            if (_listBoxSuggestions == null)
                return;

            if(!this.Parent.Controls.Contains(_listBoxSuggestions))
                this.Parent.Controls.Add(_listBoxSuggestions);

            _listBoxSuggestions.Location = new Point(this.Left + (base.CaptionTop ? 0 : _ctrl.Location.X), this.Top + _ctrl.Bottom - 1);
            _listBoxSuggestions.Width = _ctrl.Width;
            _listBoxSuggestions.Visible = true;
            _listBoxSuggestions.BringToFront();
        }

        private void HideSuggestionsList()
        {
            if (_listBoxSuggestions == null)
                return;

            _listBoxSuggestions.Visible = false;
            _listBoxSuggestions.Items.Clear();
        }

        public void AddItem(object Value, string displayText)
        {
        }

        public void AddItem(DataTable dataTable)
        {
            if (dataTable.Columns.Count > 0)
            {
                _columnIndexInSource = dataTable.Columns.Count > 1 ? 1 : 0;
            }

            _dataSource = dataTable;
        }

        public void AddItem(DataTable dataTable, string valueMember, string displayMember)
        {
        }

        public void AddItem(DataView dataView)
        {
        }

        public void AddItem(DataView dataView, string valueMember, string displayMember)
        {
        }

        public bool Filter(string fieldName, object Value)
        {
            return false;
        }

        public bool Filter(string FilterString)
        {
            return false;
        }

        private DataRow GetSelectedDataRow()
        {
            var rows = _dataSource.Rows.ToList<DataRow>().Where(r => r[_columnIndexInSource].ToString().StartsWith(_ctrl.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (rows.Count == 0)
            {
                return null;
            }

            int index = _listBoxSuggestions.SelectedIndex;
            if (index < 0)
            {
                index = 0;
            }
            return rows[index];
        }
        #endregion

        #region Event Methods
        private void textBoxInput_KeyUp(object sender, KeyEventArgs e)
        {
            SuggestedDataRow = null;
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                return;
            }

            var textboxInput = (TextBox)_ctrl;
            if (textboxInput.ReadOnly)
            {
                return;
            }

            _listBoxSuggestions.Items.Clear();

            AddDataToSuggestionsList(_dataSource, textboxInput.Text);

            var count = _listBoxSuggestions.Items.Count;
            if (count == 0)
            {
                HideSuggestionsList();
                return;
            }
            if (!_listBoxSuggestions.Visible && count > 0)
            {
                ShowSuggestionsList();
            }
        }

        private void textBoxInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (_listBoxSuggestions.Items.Count == 0)
            {
                return;
            }

            if (e.KeyCode == Keys.Down)
            {
                SuggestedDataRow = null;
                _listBoxSuggestions.Select();
                _listBoxSuggestions.SetSelected(0, true);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (_listBoxSuggestions.Visible)
                {
                    HideSuggestionsList();
                }
                SuggestedDataRow = GetSelectedDataRow();
            }
        }

        private void _ctrl_LostFocus(object sender, EventArgs e)
        {
            OnLostFocus(e);

            if (!_listBoxSuggestions.Focused)
                HideSuggestionsList();
        }

        private void _listBoxSuggestions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OnAutoSuggestCompleted(sender, e);
                _ctrl.Focus();
            }
        }

        private void _listBoxSuggestions_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)Math.Ceiling(e.Graphics.MeasureString(TextForSizing, this.Font).Height);
        }

        private void _listBoxSuggestions_MouseClick(object sender, MouseEventArgs e)
        {
            OnAutoSuggestCompleted(sender, e);
            _ctrl.Focus();
        }

        private void _listBoxSuggestions_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = _listBoxSuggestions.PointToClient(Cursor.Position);
            int index = _listBoxSuggestions.IndexFromPoint(point);

            if (index >= 0 && _indexBefore != index)
            {
                _listBoxSuggestions.SelectedIndex = index;
                _indexBefore = index;
            }
        }

        private void _listBoxSuggestions_LostFocus(object sender, EventArgs e)
        {
            if (_listBoxSuggestions.Visible)
            {
                HideSuggestionsList();
            }
        }

        private void _listBoxSuggestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnChanged();
        }

        private void _listBoxSuggestions_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,
                    e.Font,
                    e.Bounds,
                    e.Index,
                    e.State ^ DrawItemState.Selected,
                    e.ForeColor,
                    Color.FromArgb(208, 224, 242));

            e.DrawBackground();
            e.Graphics.DrawString(_listBoxSuggestions.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            _listBoxSuggestions.Font = this.Font;
        }

        protected override void OnDpiChangedBeforeParent(EventArgs e)
        {
            _ctrl.Focus();
            base.OnDpiChangedBeforeParent(e);
        }

        private void OnAutoSuggestCompleted(object sender, EventArgs e)
        {
            this.SuggestedDataRow = GetSelectedDataRow();
            _ctrl.Text = Convert.ToString(_listBoxSuggestions.SelectedItem);
            AutoSuggestCompleted?.Invoke(sender, e);
        }

        private void AddDataToSuggestionsList(DataTable data, string filterString)
        {
            if (data == null) return;

            foreach (DataRow dataRow in data.Rows)
            {
                var itemName = dataRow[_columnIndexInSource].ToString();

                if (itemName.StartsWith(filterString, StringComparison.CurrentCultureIgnoreCase))
                {
                    _listBoxSuggestions.Items.Add(itemName);
                }
            }
        }
        #endregion
    }
}
