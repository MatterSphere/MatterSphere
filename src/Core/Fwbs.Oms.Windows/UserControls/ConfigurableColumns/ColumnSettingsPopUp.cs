using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.ColumnSettings
{
    public partial class ColumnSettingsPopUp : UserControl
    {
        private const int CS_DROPSHADOW = 0x00020000;
        private IEnumerable<DataGridViewColumn> _columns;
        private ColumnSettingsBuilder _columnsBuilder;
        private LinkLabel _resetAllLabel;

        public ColumnSettingsPopUp(IEnumerable<DataGridViewColumn> columns, string searchListCode)
        {
            InitializeComponent();
            _columns = columns.Where(x => !string.IsNullOrEmpty(x.DataPropertyName) && x.Width > DataGridViewEx.MinimumColumnWidth);
            _columnsBuilder = new ColumnSettingsBuilder(searchListCode);
            _columnsBuilder.LoadNodes(columns);
            BuildContent();
        }

        public event EventHandler ResetColumns;

        // Draw DrapShadow around the control
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public void BuildContent()
        {
            checkBoxContainer.Controls.Clear();
            foreach (var column in _columns.OrderBy(x => x.DisplayIndex))
            {
                var text = column.HeaderText;
                BlueCheckBox checkBox = new BlueCheckBox
                {
                    CheckAlign = ContentAlignment.MiddleLeft,
                    Checked = column.Visible,
                    Text = string.IsNullOrEmpty(text) ? " " : text,
                    Tag = column.DataPropertyName,
                    AutoSize = true,
                    Dock = DockStyle.Top
                };
                checkBox.MouseUp += checkbox_MouseUp;
                checkBoxContainer.Controls.Add(checkBox);
            }

            if (_columns.Any())
            {
                _resetAllLabel = new LinkLabel
                {
                    ActiveLinkColor = Color.FromArgb(108, 169, 216),
                    AutoSize = true,
                    DisabledLinkColor = Color.FromArgb(165, 195, 230),
                    Dock =  DockStyle.Top,
                    Font = new System.Drawing.Font("Segoe UI", 9F),
                    LinkBehavior = LinkBehavior.NeverUnderline,
                    LinkColor = Color.FromArgb(21, 101, 192),
                    Text = Session.CurrentSession.Resources.GetResource("RESETALL", "Reset All", "").Text,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                _resetAllLabel.Links[0].Enabled = _columnsBuilder.IsColumnsCustomized();
                _resetAllLabel.Click += _resetAllLabel_Click;

                checkBoxContainer.Controls.Add(_resetAllLabel);
            }
        }

        public void Update(object sender, EventArgs e)
        {
           _columnsBuilder.UpdateColumnSettings();   
        }

        public void ResetColumnsSettings()
        {
            int defaultIndex = 0;
            foreach (var column in _columns)
            {
                column.Visible = column.Width > DataGridViewEx.MinimumColumnWidth;
                column.DisplayIndex = defaultIndex++;
            }
            Update(this, EventArgs.Empty);

            (Parent as ToolStripDropDown)?.Close(ToolStripDropDownCloseReason.CloseCalled);
        }

        protected virtual void OnResetColumns()
        {
            ResetColumns?.Invoke(this, EventArgs.Empty);
        }

        private void checkbox_MouseUp(object sender, MouseEventArgs e)
        {
            var checkbox = (BlueCheckBox)sender;
            var col = _columns.First(x => x.DataPropertyName == checkbox.Tag.ToString());
            col.Visible = checkbox.Checked && col.Width > DataGridViewEx.MinimumColumnWidth;
            _resetAllLabel.Links[0].Enabled = _columnsBuilder.IsColumnsCustomized();
        }

        private void _resetAllLabel_Click(object sender, EventArgs e)
        {
            OnResetColumns();
        }

        private class BlueCheckBox : CheckBox
        {
            protected override void OnPaint(PaintEventArgs pevent)
            {
                var checkMarkArea = LogicalToDeviceUnits(16);
                var rect = ClientRectangle;
                pevent.Graphics.FillRectangle(Brushes.White, rect);
                int indent = rect.X = (rect.Height - checkMarkArea) / 2;

                var stringFormat = new StringFormat(StringFormatFlags.NoWrap) {LineAlignment = StringAlignment.Center};

                if (Checked)
                {
                    using (var blueBrush = new SolidBrush(Color.FromArgb(21, 101, 192)))
                    using (var checkMarkFont = new Font("Segoe UI Symbol", Font.Size, FontStyle.Regular))
                    {
                        pevent.Graphics.FillRectangle(blueBrush, indent, indent, checkMarkArea, checkMarkArea);
                        pevent.Graphics.DrawString("\u2713", checkMarkFont, Brushes.White, rect, stringFormat);
                    }
                }
                else
                {
                    using (var pen = new Pen(Color.FromArgb(51, 51, 51)))
                    {
                        pevent.Graphics.FillRectangle(Brushes.White, indent, indent, checkMarkArea, checkMarkArea);
                        pevent.Graphics.DrawRectangle(pen, indent, indent, checkMarkArea, checkMarkArea);
                    }
                }

                using (var textBrush = new SolidBrush(Color.FromArgb(51, 51, 51)))
                { 
                    rect.X += (checkMarkArea + indent);
                    pevent.Graphics.DrawString(Text, Font, textBrush, rect, stringFormat);
                }

                stringFormat.Dispose();
            }
        }
    }
}
