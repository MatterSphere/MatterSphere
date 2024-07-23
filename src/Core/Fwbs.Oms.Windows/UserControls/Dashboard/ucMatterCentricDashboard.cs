using System;
using FWBS.OMS.Dashboard;
using static FWBS.OMS.Dashboard.DashboardConfigProviderMC;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    public sealed partial class ucMatterCentricDashboard : ucDashboard
    {
        private OMSFile _parent;

        public ucMatterCentricDashboard()
        {
            InitializeComponent();
            ucNavRichTextSelectedMatter.ControlRich.WordWrap = true;
        }

        protected override object ParentObject
        {
            get
            {
                if (_parent == null)
                {
                    SelectFile();
                }
                return _parent;
            }
        }

        protected override DashboardConfigProvider DashboardConfigProvider
        {
            get
            {
                if (_dashboardConfigProvider == null)
                {
                    _dashboardConfigProvider = new DashboardConfigProviderMC(Code, IsConfigurationMode);
                }

                return _dashboardConfigProvider;
            }
        }

        protected override void SetContent()
        {
            SetMatterDescription(((OMSFile)ParentObject)?.FileDescription);
            base.SetContent();
        }

        protected override void InitializeDashboardSettings()
        {
            try
            {
                var settings = ((DashboardConfigProviderMC)DashboardConfigProvider).GetDashboardSettings();
                _parent = OMSFile.GetFile(settings.ParentObjectId);
                SetMatterDescription(_parent.FileDescription);
            }
            catch (DashboardSettingsNotFoundException)
            { }
        }

        private void ucNavCmdButtonSelectMatter_Click(object sender, EventArgs e)
        {
            _parent = Windows.Services.SelectFile(ParentForm);
            if (_parent != null)
            {
                SetContent();
                ((DashboardConfigProviderMC)DashboardConfigProvider).SetDashboardSettings(new DashboardSettings() { ParentObjectId = _parent.ID });
            }
        }

        private void SelectFile()
        {
            _parent = Windows.Services.SelectFile(ParentForm);
            if (_parent != null)
            {
                ((DashboardConfigProviderMC)DashboardConfigProvider).SetDashboardSettings(new DashboardSettings() { ParentObjectId = _parent.ID });
                SetMatterDescription(_parent.FileDescription);
            }
        }

        private void SetMatterDescription(string text)
        {
            ucNavRichTextSelectedMatter.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(string.Concat(text ?? string.Empty, Environment.NewLine, Environment.NewLine));
            ucNavRichTextSelectedMatter.Refresh();
        }
    }
}
