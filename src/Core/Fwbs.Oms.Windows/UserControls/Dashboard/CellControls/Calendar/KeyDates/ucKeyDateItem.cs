using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.ContextMenu;
using FWBS.OMS.UI.Windows;
using Infragistics.Win.Misc;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.KeyDates
{
    internal partial class ucKeyDateItem : UserControl
    {
        private FlowLayoutPanel _popUpContent;
        private UltraPeekPopup _actionPopup;

        public ucKeyDateItem()
        {
            InitializeComponent();
            btnActions.Text = "";
            SetIcon();

            _popUpContent = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Segoe UI", 9),
                AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true
            };
        }

        public EventHandler ActionClicked;
        public int KeyDateId { get; private set; }
        public long FileId { get; private set; }

        public void SetData(KeyDateRow keyDate)
        {
            picture.Day = keyDate.Day;
            lblTitle.Text = keyDate.DateLabel;
            lblDescription.Text = keyDate.Description;

            KeyDateId = keyDate.Id;
            FileId = keyDate.FileId;

            var fileTitle = $"{keyDate.ClientNo}-{keyDate.FileNo}";
            toolTipFile.SetToolTip(picture, fileTitle);
        }

        public void ShowActionPopup(List<ContextMenuButton> buttons)
        {
            _popUpContent.Controls.Clear();

            if (!buttons.Any())
            {
                _popUpContent.Controls.Add(
                    new Label
                    {
                        Text = Session.CurrentSession.Resources.GetResource("NOACTNS", "No actions", "").Text,
                        TextAlign = ContentAlignment.MiddleCenter
                    }, true);
            }
            else
            {
                foreach (var button in buttons)
                {
                    _popUpContent.Controls.Add(button, true);
                }
            }

            _actionPopup = new UltraPeekPopup
            {
                ContentMargin = new Padding(1),
                Content = _popUpContent,
                Appearance = new Infragistics.Win.Appearance
                {
                    BorderColor = Color.FromArgb(234, 234, 234)
                }
            };

            Point location = btnActions.PointToScreen(new Point(btnActions.Width / 2, btnActions.Height / 2));
            _actionPopup.Show(location, Infragistics.Win.Peek.PeekLocation.BelowItem);
        }

        private void SetToolTip(object sender, PaintEventArgs e)
        {
            var label = sender as Label;
            var layoutSize = new SizeF(label.Width, label.Height + 1);
            SizeF fullSize = e.Graphics.MeasureString(label.Text, label.Font, layoutSize);
            if (fullSize.Width > label.Width || fullSize.Height > label.Height)
            {
                toolTipLabels.SetToolTip(label, label.Text);
            }
            else
            {
                toolTipLabels.SetToolTip(label, null);
            }
        }

        private void SetIcon()
        {
            var icon = FWBS.OMS.UI.Windows.Images.GetCommonIcon(DeviceDpi, "calendar");
            picture.Image = icon;
        }

        #region UI events

        private void btnActions_Click(object sender, System.EventArgs e)
        {
            ActionClicked?.Invoke(this, e);
        }

        private void lblDescription_Paint(object sender, PaintEventArgs e)
        {
            SetToolTip(sender, e);
        }

        private void lblTitle_Paint(object sender, PaintEventArgs e)
        {
            SetToolTip(sender, e);
        }

        private void picture_DpiChangedAfterParent(object sender, EventArgs e)
        {
            SetIcon();
        }

        #endregion
    }
}
