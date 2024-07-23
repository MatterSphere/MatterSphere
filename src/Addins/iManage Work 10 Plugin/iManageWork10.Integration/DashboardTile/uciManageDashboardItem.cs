using System;
using System.Windows.Forms;
using FWBS.OMS;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;
using iManageWork10.Integration.Util;
using iManageWork10.Shell.Commands;
using iManageWork10.Shell.Exceptions;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.RestAPI;
using iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties;
using iManageWork10HostRestApi;

namespace iManageWork10.Integration.DashboardTile
{
    public partial class uciManageDashboardItem : BaseContainerPage
    {

        private string _baseUrl = string.Empty;
        private string _clientId;
        private string _extensionId;
        private RestApiClient _apiClient;
        private object _parentObject;

        public uciManageDashboardItem()
        {
            InitializeComponent();
        }

        public override object ParentObject
        {
            get
            {
                return _parentObject;
            }
            set
            {
                if (value == null || value is User || value is OMSFile)
                {
                    if (_parentObject != null)
                    {
                        if (_parentObject != value)
                        {
                            _parentObject = value;
                            UpdateData();
                        }
                    }
                    else
                    {
                        _parentObject = value;
                    }
                }
                else
                {
                    throw new Exception($"iManage tile is not compatible with {value.GetType()}");
                }
            }
        }

        public override void UpdateData(bool withScale = false)
        {
            if (string.IsNullOrEmpty(_baseUrl))
            {
                _baseUrl = IntegrationUtil.GetRootUrl();
            }
            if (string.IsNullOrEmpty(_clientId))
            {
                _clientId = IntegrationUtil.GetClientId();
            }
            if (string.IsNullOrEmpty(_extensionId))
            {
                _extensionId = IntegrationUtil.GetExtensionId();
            }

            string url = _baseUrl;
            if (_apiClient == null)
            {
                _apiClient = new RestApiClient(new HostRestApiWorkerProvider(_baseUrl, _clientId, _extensionId));
            }
            if (!_apiClient.Connect())
            {
                browser.Url = "about:blank";
                return;
            }

            if (!browser.IsInitialized)
            {
                browser.Initialized += Browser_Initialized;
                browser.Url = "about:blank";
                return;
            }

            var omsFile = ParentObject as OMSFile;
            if (omsFile != null)
            {
                var custom1Value = IntegrationUtil.GetCustomValue(ParentObject, "Custom1");
                var custom2Value = IntegrationUtil.GetCustomValue(ParentObject, "Custom2");
                var libraries = IntegrationUtil.GetLibrariesScope();

                try
                {
                    var workspaceSearchProperties = new SearchWorkspacesProperties()
                    {
                        Custom1 = custom1Value,
                        Custom2 = custom2Value,
                        Libraries = libraries
                    };
                    var command = new FindWorkspaceCommand(workspaceSearchProperties);
                    var workspace = new CommandsExecuter(_apiClient).Execute(command);
                    url = UriComposer.ComposeWorkspaceUrl(_baseUrl ,workspace);
                }
                catch (WorkspaceNotFoundException)
                {
                    url = HandleWorkspaceNotFoundException(custom1Value, custom2Value, omsFile.FileDescription);
                }
            }

            var user = ParentObject as User;
            if (user != null)
            {
                url = UriComposer.ComposeUserBaseUrl(_baseUrl);
            }

            browser.SetCookie(_baseUrl, "imToken", _apiClient.AuthToken);
            browser.Url = url;
        }

        private void Browser_Initialized(object sender, EventArgs e)
        {
            UpdateData();
            browser.Initialized -= Browser_Initialized;
        }

        private string HandleWorkspaceNotFoundException(string custom1Value, string custom2Value, string workspaceName)
        {
            if (IntegrationUtil.IsUserInWorkspaceCreatorRole())
            {
                if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion(this, "IMWSPNTFOUNDQST",
                    "Workspace not found. Would you like to create workspace?") == DialogResult.Yes)
                {
                    var workspace = new Workspace()
                    {
                        Custom1 = custom1Value,
                        Custom2 = custom2Value,
                        Name = workspaceName
                    };
                    var command = new CreateWorkspaceCommand(workspace);
                    var createdWorkspace = new CommandsExecuter(_apiClient).Execute(command);
                    return UriComposer.ComposeWorkspaceUrl(_baseUrl, createdWorkspace);
                }
            }
            else
            {
                FWBS.OMS.UI.Windows.MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("IMWSPNTFOUND", "Workspace not found", ""));
            }
            return UriComposer.ComposeUserBaseUrl(_baseUrl);
        }

    }
}