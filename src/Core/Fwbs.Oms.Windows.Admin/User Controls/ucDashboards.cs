using System;
using System.Data;
using System.Windows.Forms;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.UI.UserControls.Dashboard;

namespace FWBS.OMS.UI.Windows.Admin
{
    public partial class ucDashboards : ucEditBase2
    {
        private const string DASHBOARDS_SEARCHLIST_NAME = "ADMDASHBOARDS";
        private const string LABEL_TEMPLATE = "{0} - [{1}]";

        private Business.Dashboard _currentObj;
        private ucDashboard _dashboardControl;

        public ucDashboards()
        {
            InitializeComponent();
        }

        public ucDashboards(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params) : base(mainparent, editparent, Params)
        {
            InitializeComponent();
        }

        #region ucEditBase2 overrides

        protected override string SearchListName
        {
            get
            {
                return DASHBOARDS_SEARCHLIST_NAME;
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
        }

        protected override void LoadSingleItem(string code)
        {
            if (_currentObj != null)
            {
                DetachEventHandlers(_currentObj);
            }
            labSelectedObject.Text = string.Format(LABEL_TEMPLATE, ResourceLookup.GetLookupText("DASHBOARDS", "Dashboards", ""), code);
            ShowEditor(false);
            _currentObj = new Business.Dashboard(code);
            AttachEventHandlers(_currentObj);
            propertyGrid.SelectedObject = _currentObj;
            propertyGrid.HelpVisible = true;
            SetDashboardControl();
            RefreshLinkState();
        }

        protected override bool UpdateData()
        {
            try
            {
                _currentObj.Update();
                ResetDashboardControl();
                SetDashboardControl();
                lstList.Search();
                return true;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
                return false;
            }
        }

        protected override void NewData()
        {
            if (_currentObj != null)
            {
                DetachEventHandlers(_currentObj);
            }
            labSelectedObject.Text = string.Format(LABEL_TEMPLATE, ResourceLookup.GetLookupText("DASHBOARDS", "Dashboards", ""), ResourceLookup.GetLookupText("Untitled", "Untitled", ""));
            lnkRegDashboardObj.Enabled = false;
            ShowEditor(true);
            _currentObj = new Business.Dashboard();
            AttachEventHandlers(_currentObj);
            propertyGrid.SelectedObject = _currentObj;
            propertyGrid.HelpVisible = true;
        }

        protected override bool CancelData()
        {
            _currentObj.Cancel();
            return true;
        }

        protected override void DeleteData(string code)
        {
            DashboardSysObject.Delete(code);
        }

        protected override bool Restore(string code)
        {
            return DashboardSysObject.Restore(code);
        }

        protected override void Clone(string code)
        {
            if (_currentObj != null)
            {
                DetachEventHandlers(_currentObj);
            }
            labSelectedObject.Text = string.Format(LABEL_TEMPLATE, ResourceLookup.GetLookupText("DASHBOARDS", "Dashboards", ""), ResourceLookup.GetLookupText("Untitled", "Untitled", ""));
            lnkRegDashboardObj.Enabled = false;
            ShowEditor(true);

            _currentObj = Business.Dashboard.Clone(code);
            AttachEventHandlers(_currentObj);
            propertyGrid.SelectedObject = _currentObj;
            propertyGrid.HelpVisible = true;
        }

        protected override void CloseAndReturnToList()
        {
            if (base.IsDirty)
            {
                DialogResult? dr = IsObjectDirtyDialogResult();
                if (dr != DialogResult.Cancel)
                {
                    ResetDashboardControl();
                    base.ShowList();
                }
            }
            else
            {
                ResetDashboardControl();
                base.ShowList();
            }
        }

        #endregion

        private void CurrentObj_CodeChange(object sender, EventArgs e)
        {
            labSelectedObject.Text = string.Format(LABEL_TEMPLATE, ResourceLookup.GetLookupText("DASHBOARDS", "Dashboards", ""), _currentObj.Code);
            RefreshLinkState();
        }

        private void CurrentObj_Change(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            IsDirty = true;
        }

        private void lnkRegDashboardObj_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (IsDirty)
            {
                DialogResult dr = System.Windows.Forms.MessageBox.Show(this.EditParent, Session.CurrentSession.Resources.GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?", "").Text, "OMS Admin", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                    SaveChanges();
                else
                    return;
            }

            var wizardParams = new FWBS.Common.KeyValueCollection
            {
                { "objCode", _currentObj.Code },
                { "objHelp", "[Dashboard]" }
            };

            DataTable objectInfoDt = Services.Wizards.GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.RegisterOMSObject), null, EnquiryMode.Add, false, wizardParams) as DataTable;

            if (objectInfoDt != null)
            {
                var code = Convert.ToString(objectInfoDt.Rows[0]["objCode"]);
                var desc = Convert.ToString(objectInfoDt.Rows[0]["objdescription"]);
                var type = Convert.ToString(objectInfoDt.Rows[0]["objType"]);
                var help = Convert.ToString(objectInfoDt.Rows[0]["objHelp"]);
                var win = _dashboardControl.GetType().FullName;
                var web = code;
                var pda = code;
                try
                {
                    OmsObject.Register(code, OMSObjectTypes.Addin, type, desc, help, win, web, pda, null);
                    RefreshLinkState();
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ParentForm, ex);
                }
            }
        }

        private void AttachEventHandlers(Business.Dashboard _currentObj) 
        {
            _currentObj.CodeChanged += CurrentObj_CodeChange;
            _currentObj.Changed += CurrentObj_Change;
        }

        private void DetachEventHandlers(Business.Dashboard _currentObj)
        {
            _currentObj.CodeChanged -= CurrentObj_CodeChange;
            _currentObj.Changed -= CurrentObj_Change;
        }

        private void RefreshLinkState()
        {
            lnkRegDashboardObj.Enabled = !string.IsNullOrWhiteSpace(_currentObj.Code) && !OmsObject.IsObjectRegistered(_currentObj.Code, OMSObjectTypes.Addin);
        }

        private void lstList_ButtonEnabledRulesAppliedDsh(object sender, EventArgs e)
        {
            OMSToolBarButton tbPDelete = lstList.GetOMSToolBarButton("cmdPDelete");
            if (tbPDelete != null && tbPDelete.Enabled)
            {
                tbPDelete.Enabled = lstList.GetOMSToolBarButton("cmdRestore").Enabled;
            }
        }

        private void lstList_SearchButtonCommandsDsh(object sender, SearchButtonEventArgs e)
        {
            if (e.ButtonName == "cmdPDelete")
            {
                if (MessageBox.ShowYesNoQuestion("AUSPERMDELETE", "Are you sure you wish to Permanently Delete selected item? There is no undo for this operation.") == DialogResult.Yes)
                {
                    DashboardSysObject.PermanentlyDelete(GetCode());
                    lstList.Search();
                }
            }
        }

        private void SetDashboardControl()
        {
            _dashboardControl = NewDashboard(_currentObj.DashboardType);

            _dashboardControl.Dock = System.Windows.Forms.DockStyle.Fill;
            _dashboardControl.IsConfigurationMode = true;
            _dashboardControl.Location = new System.Drawing.Point(0, 0);
            _dashboardControl.Name = "ucDashboard";
            _dashboardControl.TabIndex = 0;
            _dashboardControl.ToBeRefreshed = false;
            _dashboardControl.Code = _currentObj.Code;

            pnlDashboardContainer.Controls.Add(_dashboardControl, true);
            _dashboardControl.Connect(null);
        }

        private static ucDashboard NewDashboard(DashboardTypes dashboardType)
        {
            switch (dashboardType)
            {
                case DashboardTypes.UserCompatible:
                    return new ucDashboard();
                case DashboardTypes.MatterCompatible:
                    return new ucMatterCentricDashboard();
                default:
                    throw new ArgumentOutOfRangeException(nameof(dashboardType), dashboardType, null);
            }
        }

        private void ResetDashboardControl()
        {
            if (_dashboardControl != null)
            {
                _dashboardControl.Reset();
                pnlDashboardContainer.Controls.Remove(_dashboardControl);
                _dashboardControl.Dispose();
                _dashboardControl = null;
            }
        }
    }
}
