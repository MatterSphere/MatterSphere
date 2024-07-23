using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.MatterList;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    public class BaseContainerPage : UserControl
    {
        private bool _omsWindowOpening;

        protected BaseContainerPage()
        {
            ActionItems = new List<ActionItem>();
            FilterOptions = new List<ActionItem>();
            PageSize = 50;
        }

        public event EventHandler<int> QueryCompleted;
        public event EventHandler PagesReset;
        public event EventHandler<bool> DeleteEnableStatusChanged;
        public event EventHandler<EditFormParameters> EditFormOpening;
        public event EventHandler<bool> EnableChanged;
        public event EventHandler ActionsEnabled;

        public Guid DashboardCellGuid { get; set; }
        public string DashboardCode { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int PageSize { get; set; }
        public virtual object ParentObject { get; set; }
        public List<ActionItem> ActionItems { get; set; }
        public List<ActionItem> FilterOptions { get; set; }
        public string DefaultFilter { get; set; }

        public bool AllowColumnChange { get; set; }
        public ColumnData[] Columns { get; set; }

        public bool ViewMode { get; set; }
        public bool IsConfigurationMode { get; internal set; }
        public bool HideBottomPanel { get; set; }
        public bool HideSearchButton { get; set; }

        public FavoritesProvider FavoritesProvider { get; private set; }
        public void SetFavoritesProvider(FavoritesProvider provider)
        {
            FavoritesProvider = provider;
            FavoritesProvider.FilesUpdated += UpdateFileFavorites;
        }

        protected void OnQueryCompleted(int total)
        {
            QueryCompleted?.Invoke(this, total);
        }

        protected void ResetPages()
        {
            PagesReset?.Invoke(this, EventArgs.Empty);
        }

        protected void OnDeleteEnableStatusChanged(bool enable)
        {
            DeleteEnableStatusChanged?.Invoke(this, enable);
        }

        protected void OnEditFormOpening(string code, EnquiryMode mode, FWBS.Common.KeyValueCollection param)
        {
            EditFormOpening?.Invoke(this, new EditFormParameters(code, mode, param));
        }

        protected void ChangeEnable(bool enable)
        {
            EnableChanged?.Invoke(this, enable);
        }

        protected void EnableActions()
        {
            ActionsEnabled?.Invoke(this, EventArgs.Empty);
        }

        public virtual void ChangePage(object sender, int page)
        {
            throw new NotImplementedException();
        }

        public virtual void StartSearch(object sender, string query)
        {
            throw new NotImplementedException();
        }

        public virtual void AddNew(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateData(bool withScale = false)
        {
            throw new NotImplementedException();
        }

        public virtual void CallAction(object sender, string code)
        {
            throw new NotImplementedException();
        }

        public virtual void ChangeColumnVisibility(string name, bool visibility)
        {
            throw new NotImplementedException();
        }

        public virtual void SetColumnSettings()
        {
        }

        protected virtual void UpdateFileFavorites(object sender, EventArgs e)
        {
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

        protected void OpenNewOMSTypeWindow(IOMSType omsType)
        {
            if (_omsWindowOpening)
                return;
            try
            {
                _omsWindowOpening = true;
                this.UseWaitCursor = true;
                OnNewOMSTypeWindow(omsType);
            }
            finally
            {
                _omsWindowOpening = false;
                this.UseWaitCursor = false;
            }
        }
    }
}
