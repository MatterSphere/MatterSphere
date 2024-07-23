using System;
using System.Collections.Generic;
using System.Linq;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.RestAPI;
using iManageWork10.Shell.RestAPI.RestAPIManagement;

namespace iManageWork10.Shell.Commands
{
    public class CreateWorkspaceCommand : IManageCommand<Workspace>
    {
        private static readonly string CUSTOM1 = "custom1";
        private static readonly string CUSTOM2 = "custom2";

        private readonly Workspace _workspace;

        private WorkspacesManagement _workspacesManagement;
        private CustomsManagement _customsManagement;

        public CreateWorkspaceCommand(Workspace workspace)
        {
            _workspace = workspace;
        }

        public Workspace Execute(IRestApiClient restApiClient)
        {
            _workspacesManagement = new WorkspacesManagement(restApiClient);
            _customsManagement = new CustomsManagement(restApiClient);
            
            ValidateCustom1();
            ValidateCustom2();
            ValidateWorkspaceName();

            return _workspacesManagement.CreateWorkspace(_workspace);
        }

        private void ValidateCustom1()
        {
            List<Custom> customs1 = _customsManagement.GetCustom(CUSTOM1, _workspace.Custom1, _workspace.Database);
            if (!customs1.Any())
            {
                CreateCustom1();
            }
        }

        private void CreateCustom1()
        {
            _customsManagement.CreateCustom(CUSTOM1, new Custom()
            {
                Id = _workspace.Custom1,
                Description = _workspace.Custom1,
                Enabled = true,
                Hipaa = false
            });
        }

        private void ValidateCustom2()
        {
            List<Custom> customs2 = _customsManagement.GetChildCustom(CUSTOM2, _workspace.Custom2, _workspace.Custom1,_workspace.Database);
            if (!customs2.Any())
            {
                CreateCustom2();
            }
        }

        private void CreateCustom2()
        {
            _customsManagement.CreateCustom(CUSTOM2, new Custom()
            {
                Id = _workspace.Custom2,
                Description = _workspace.Custom2,
                Enabled = true,
                Hipaa = false,
                Parent = new Parent() { Id = _workspace.Custom1}
            });
        }

        private void ValidateWorkspaceName()
        {
            if (string.IsNullOrWhiteSpace(_workspace.Name))
            {
                _workspace.Name = $"{_workspace.Custom1} - {_workspace.Custom2}";
            }
        }

    }
}
