using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;
using FWBS.OMS.UI.UserControls.ContextMenu;
using FWBS.OMS.UI.Windows;
using Infragistics.Win.Misc;

namespace FWBS.OMS.UI.DataGridViewControls
{
    public delegate IEnumerable<ContextMenuButton> DataGridViewActionsColumnDelegate(DataGridViewActionsCell cell);

    public class DataGridViewActionsColumn : DataGridViewButtonColumn, IBeforeCellDisplayable
    {
        /// <summary>
        /// Popup element which activated on the cell click
        /// </summary>
        private UltraPeekPopup _actionPopup;

        private FlowLayoutPanel _popUpContent;
        
        /// <summary>
        /// Font that is used to draw action button dots
        /// </summary>
        private System.Drawing.Font _font;

        /// <summary>
        /// Font that is used to display popup content
        /// </summary>
        private System.Drawing.Font _contentFont;

        [Browsable(false)]
        public DataGridViewActionsColumnDelegate GetActions { get; set; }

        public event EventHandler<CellDisplayEventArgs> BeforeCellDisplayEvent;

        public CellDisplayEventArgs OnBeforeCellDisplayEvent(int rowNum, CurrencyManager source, string text, string columnName)
        {
            EventHandler<CellDisplayEventArgs> ev = BeforeCellDisplayEvent;
            if (ev != null)
            {
                System.Data.DataView dv = source.List as System.Data.DataView;
                if (dv != null)
                {
                    CellDisplayEventArgs e = new CellDisplayEventArgs(rowNum, dv[rowNum], text, columnName);
                    ev(this, e);
                    return e;
                }
            }
            return null;
        }

        public DataGridViewActionsColumn()
        {
            CellTemplate = new DataGridViewActionsCell
            {
                Style = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.BottomLeft,
                    ForeColor = System.Drawing.Color.Gray,
                    SelectionForeColor = System.Drawing.Color.Gray
                }
            };

            Text = "...";
            UseColumnTextForButtonValue = true;
            FlatStyle = FlatStyle.Flat;
        }

        private void InitializePopupContent()
        {
            _font = new System.Drawing.Font(DataGridView.Font.FontFamily, 24);
            _contentFont = new System.Drawing.Font(DataGridView.Font.FontFamily, 12);
            _popUpContent = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Font = _contentFont,
                AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true
            };
        }

        protected override void OnDataGridViewChanged()
        {
            base.OnDataGridViewChanged();
            if (DataGridView != null)
            {
                InitializePopupContent();

                DataGridView.DpiChangedAfterParent -= AdjustFont;
                DataGridView.DpiChangedAfterParent += AdjustFont;
            }
        }

        /// <summary>
        /// Adjusts font depending on DPI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdjustFont(object sender, System.EventArgs e)
        {
            using (Graphics g = DataGridView.CreateGraphics())
            {
                float scaleFactor = DataGridView.DeviceDpi / g.DpiX;
                _font = new Font(DataGridView.Font.FontFamily, 24 * scaleFactor);
                _contentFont = new Font(DataGridView.Font.FontFamily, 12 * scaleFactor);
            }
        }

        private void PopulatePanel(IEnumerable<ContextMenuButton> buttons)
        {
            Windows.Global.RemoveAndDisposeControls(_popUpContent);

            if (buttons != null && buttons.Any())
            {
                foreach (var button in buttons)
                {
                    _popUpContent.Controls.Add(button, true);
                }
            }
            else
            {
                _popUpContent.Controls.Add(
                    new Label
                    {
                        Text = Session.CurrentSession.Resources.GetResource("NOACTNS", "No actions", "").Text,
                        TextAlign = ContentAlignment.MiddleCenter
                    }, true);
            }
        }

        /// <summary>
        /// Font applied to action cell painting
        /// </summary>
        [Browsable(false)]
        public System.Drawing.Font ActionFont
        {
            get
            {
                return _font;
            }
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                var type = typeof(DataGridViewActionsCell);
                if (value?.GetType().IsAssignableFrom(type) == false)
                {
                    ThrowInvalidCastException(type);
                }

                base.CellTemplate = value;
            }
        }

        private void ThrowInvalidCastException(Type target)
        {
            throw new OMSException2(
                "COLUMNCELLTYPE",
                "Cell must be a '%1%' type",
                new InvalidCastException(),
                true,
                target.Name);
        }

        /// <summary>
        /// Shows actions pupup
        /// </summary>
        /// <param name="actionsCell"></param>
        /// <param name="location"></param>
        public void ShowActionsPopup(DataGridViewActionsCell actionsCell, Point location)
        {
            CloseActionsPopup();
            PopulatePanel(GetActions != null ? GetActions(actionsCell) : null);

            _actionPopup = new UltraPeekPopup
            {
                ContentMargin = new Padding(1),
                Content = _popUpContent,
                Appearance = new Infragistics.Win.Appearance
                {
                    BorderColor = Color.FromArgb(234, 234, 234)
                }
            };

            _actionPopup.Show(location, Infragistics.Win.Peek.PeekLocation.BelowItem);
        }

        /// <summary>
        /// Closes actions popup
        /// </summary>
        public void CloseActionsPopup()
        {
            if (_actionPopup != null)
            {
                _actionPopup.Close();
                _actionPopup.Dispose();
                _actionPopup = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CloseActionsPopup();
                if (_popUpContent != null)
                {
                    Windows.Global.RemoveAndDisposeControls(_popUpContent);
                    _popUpContent.Dispose();
                    _popUpContent = null;
                }
                if (_font != null)
                {
                    _font.Dispose();
                    _font = null;
                }
                if (_contentFont != null)
                {
                    _contentFont.Dispose();
                    _contentFont = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
