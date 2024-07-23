using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;
using FWBS.OMS.UI.UserControls.ContextMenu;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.KeyDates
{
    public partial class KeyDatesDashboardItem : BaseContainerPage
    {
        private string _query;
        private int _currentPage = 1;
        private List<KeyDateRow> _keyDates;
        private ContextMenuItemBuilder _cmiBuilder;
        
        public KeyDatesDashboardItem()
        {
            InitializeComponent();

            _cmiBuilder = new ContextMenuItemBuilder();
        }

        #region Actions popup

        private void ActionPopupOpen(object sender, EventArgs e)
        {
            var item = sender as ucKeyDateItem;
            var buttons = new List<ContextMenuButton>();

            var viewFileButton = _cmiBuilder.CreateTextButton(
                title: CodeLookup.GetLookup("DASHBOARD", "VWFL", "View Matter"),
                clickHandler: delegate { OpenFile(item.FileId); });
            buttons.Add(viewFileButton);

            var deleteButton = _cmiBuilder.CreateTextButton(
                title: CodeLookup.GetLookup("DASHBOARD", "DELETE", "Delete"),
                clickHandler: delegate
                {
                    var wasDeleted = DateWizard.DeleteKeyDate(item.FileId, item.KeyDateId);
                    if (wasDeleted)
                    {
                        UpdateData();
                    }
                });
            buttons.Add(deleteButton);

            item.ShowActionPopup(buttons);
        }

        #endregion

        #region Private methods

        private void CheckData(string query, int page, out int total)
        {
            _keyDates = DashboardTileDataProvider.GetKeyDates(query, page, out total, PageSize);
        }

        private void OpenFile(long id)
        {
            FWBS.OMS.OMSFile file = FWBS.OMS.OMSFile.GetFile(id);
            OnNewOMSTypeWindow(file);
        }

        private void OnNewOMSTypeWindow(IOMSType omsType)
        {
            var eventArgs = new NewOMSTypeWindowEventArgs(omsType);
            var screen = new OMSTypeScreen(eventArgs.OMSObject)
            {
                DefaultPage = eventArgs.DefaultPage,
                OmsType = eventArgs.OMSType
            };

            screen.Show(null);
        }

        #endregion

        #region BaseContainerPage

        public override void UpdateData(bool withScale = false)
        {
            int total;
            CheckData(_query, _currentPage, out total);
            OnQueryCompleted(total);

            while (pnlContainer.Controls.Count > 0)
            {
                var control = pnlContainer.Controls[0];
                if (control is ucKeyDateItem)
                {
                    ((ucKeyDateItem)control).ActionClicked -= ActionPopupOpen;
                }

                control.Dispose();
            }

            pnlContainer.Controls.Clear();

            for (int i = _keyDates.Count-1; i >=0 ; i--)
            {
                var listItem = new ucKeyDateItem
                {
                    Dock = DockStyle.Top
                };
                listItem.SetData(_keyDates[i]);
                listItem.ActionClicked += ActionPopupOpen;
                pnlContainer.Controls.Add(listItem, withScale);
            }
        }

        public override void ChangePage(object sender, int e)
        {
            _currentPage = e;
            UpdateData(true);
        }

        public override void StartSearch(object sender, string e)
        {
            _query = e;
            _currentPage = 1;
            UpdateData(true);
        }

        public override void AddNew(object sender, EventArgs e)
        {
            base.OnEditFormOpening("DSHBKDADD", EnquiryMode.Add, null);
        }

        #endregion
    }
}
