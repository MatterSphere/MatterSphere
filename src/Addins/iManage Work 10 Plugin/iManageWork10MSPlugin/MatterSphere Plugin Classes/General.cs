using System;
using System.Linq;
using System.Windows.Forms;
using iManage.Work.Tools;
using iManageWork10.Shell.Exceptions;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.JsonResponses.Enums;
using iManageWork10.Shell.RestAPI;
using iManageWork10.Shell.RestAPI.RestAPIManagement;
using iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties;
using iManageWork10.Shell.Validators;
using MatterSphereIntercept.Exceptions;
using MatterSphereIntercept.MatterSphere_Plugin_Classes;
using MatterSphereIntercept.SilentSave;

namespace MatterSphereIntercept
{
    public abstract class General : PlugInBase
    {

        private RestApiClient _restApiClient;

        public RestApiClient RestApiClient
        {
            get
            {
                if (_restApiClient == null)
                {
                    _restApiClient = new RestApiClient(new PlugInRestApiWorkerProvider(this, PlugInHost));
                }
                return _restApiClient;
            }
        }
        
        public AddinWrapper AddinWrapper
        {
            get
            {
                if (ComAddin == null)
                { 
                    throw new Exception("Could not find MatterSphere add-in.");
                }
                return new AddinWrapper(ComAddin);
            }
        }

        private SilentSaver SilentSaver { get; set; }

        protected abstract dynamic ComAddin { get; }
        
        public override bool Initialize(IPlugInHost host)
        {
            PlugInHost = host;
            PlugInId = "bf973177-e9f7-43c1-906d-28a05af9ae89";
            
            host.Startup += Host_Startup;
            host.Shutdown += Host_Shutdown;
            ((IPlugInHostForms)host).SaveForm_OnInitialize += SaveForm_OnInitialize;
            ((IPlugInHostForms)host).SaveForm_PostOnOK += SaveForm_PostOnOK;

            InternalInitialize(host);
            return true;
        }

        protected abstract void Host_Shutdown(object sender, EventArgs e);

        protected abstract void Host_Startup(object sender, EventArgs e);

        protected abstract void SaveForm_OnInitialize(object sender, WFormEventArgs e);

        protected abstract void InternalInitialize(IPlugInHost host);
        
        protected void SetupSaveForm(WFormEventArgs e, IntegrationMetadata metadata)
        {
            var client = metadata.ClientNumber;
            var matter = metadata.MatterNumber;
            try
            {
                var workspace = GetWorkspace(client, matter);
                var profile = metadata.GetFormattedProfile();
                var destination = workspace.Id;
                try
                {
                    destination = GetFolder(workspace, metadata.DocumentFolder).Id;
                }
                catch (FolderNotFoundException exception)
                {
                    WLog.Info(exception.Message);
                }

                SetSaveFormProperties(e, metadata.DocumentDescription, destination, profile);
            }
            catch (WorkspaceNotFoundException ex)
            {
                e.SetCancel();
                MessageBox.Show(ex.Message);
            }
            catch (AccessDeniedException ex)
            {
                e.SetCancel();
                MessageBox.Show(ex.Message);
            }
        }

        protected void CompleteSilentSaveWorkItem(IWDocumentProfile documentProfile)
        {
            PerformSilentSaveOperation(() =>
            {
                SilentSaver.CompleteWorkItem(documentProfile);
            });
        }

        protected void StartSilentSaveWorkItem(object document, IWDocumentProfile documentProfile)
        {
            PerformSilentSaveOperation(() =>
            {
                SilentSaver.StartWorkItem(document, documentProfile);
            });
        }

        protected void SaveNewDocumentSilently(IntegrationMetadata metadata, dynamic document)
        {
            PerformSilentSaveOperation(() =>
            {
                var client = metadata.ClientNumber;
                var matter = metadata.MatterNumber;
                var workspace = GetWorkspace(client, matter);
                var destination = workspace.Id;
                try
                {
                    destination = GetFolder(workspace, metadata.DocumentFolder).Id;
                }
                catch (FolderNotFoundException exception)
                {
                    WLog.Info(exception.Message);
                }
                SilentSaver.SaveNewDocument(metadata.GetDocumentProfile(), destination, document, workspace.Database);
            });
        }

        protected void Shutdown()
        {
            SilentSaver?.Shutdown();
        }

        protected void ExecuteOperation(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                WLog.Debug(ex.Message);
            }
        }

        private void PerformSilentSaveOperation(Action action)
        {
            try
            {
                ValidateSilentSaver();
                action.Invoke();
            }
            catch (SilentSaveDisabledException)
            {
                WLog.Debug("Silent save option is disabled in MatterSphere.");
            }
        }

        private void ValidateSilentSaver()
        {
            bool enabled;
            var addinWrapper = AddinWrapper;
            if (bool.TryParse(addinWrapper.GetSettingValue(AddinConstants.SILENT_SAVE_ENABLED), out enabled) && enabled)
            {
                if (SilentSaver == null)
                {
                    SilentSaver = new SilentSaver(RestApiClient, WLog);
                    int timeOut;
                    if (int.TryParse(addinWrapper.GetSettingValue(AddinConstants.SILENT_SAVE_TIMEOUT), out timeOut))
                    {
                        SilentSaver.TimerTickInSeconds = timeOut;
                        WLog.Debug($"Silent save timeout is set to {timeOut}.");
                    }
                }
            }
            else
            {
                throw new SilentSaveDisabledException();
            }
        }

        private void SetSaveFormProperties(WFormEventArgs e, string documentDescription, string filingDestination, string strBlankKeysRemoved)
        {
            //use the filing destination to set the StartingLocation on the save form
            e.PropertyChangeSet.Add(WPropertyName.SaveForm.StartingLocation, new WStringProperty(filingDestination));
            e.PropertyChangeSet.Add(WPropertyName.SaveForm.FileName, new WStringProperty(documentDescription));
            e.PropertyChangeSet.Add(WPropertyName.SaveForm.StartingProfile, new WRawJsonProperty(strBlankKeysRemoved));
        }


        private Workspace GetWorkspace(string custom1, string custom2)
        {
            try
            {
                var wm = new WorkspacesManagement(RestApiClient);
                var searchProperties = new SearchWorkspacesProperties
                {
                    Custom1 = custom1,
                    Custom2 = custom2
                };
                var libraryIds = AddinWrapper.GetSettingValue(AddinConstants.ADDITIONAL_LIBRARIES);
                if (!string.IsNullOrEmpty(libraryIds))
                {
                    LibrariesValidator validator = new LibrariesValidator(_restApiClient);
                    try
                    {
                        validator.Validate(libraryIds);
                        searchProperties.Libraries = libraryIds;
                    }
                    catch (ArgumentException ex)
                    {
                        WLog.Warn(ex.Message);
                    }
                }
                var workspaces = wm.SearchWorkspaces(searchProperties);
                if (workspaces?.Count == 0)
                {
                    var message = $"The workspace with search criteria Client={custom1}, Matter={custom2} cannot be found within the database. Or you have insufficient permission to view the workspace.";
                    WLog.Warn(message);
                    throw new WorkspaceNotFoundException(message);
                }

                var workspace = workspaces.First();
                var user = new UserManagement(RestApiClient).GetCurrentUserProfile(workspace.Database);
                var workspaceAccess = wm.GetWorkspaceUserSecurity(workspace.Id, user.Id, workspace.Database).Access;
                try
                {
                    workspaceAccess.ValidateAccess();
                    return workspace;
                }
                catch (AccessDeniedException)
                {
                    var accessMessage = $"The access to workspace for Client={custom1}, Matter={custom2} is denied. User={user.Id}. Access level is '{workspaceAccess}'";
                    WLog.Warn(accessMessage);
                    throw;
                }
            }
            catch (Exception ex)
            {
                WLog.PublishException(WExceptionType.Errors, ex, ex.Message);
                throw;
            }
        }
           
        private Folder GetFolder(Workspace workspace, string folderName)
        {
            var wm = new WorkspacesManagement(RestApiClient);
            var folders =  wm.SearchFolders(workspace.Id, new SearchFoldersProperties { Name = folderName}, workspace.Database);
            if (folders.Any())
            {
                return folders.First();
            }
            else
            {
                var message = $"Folder '{folderName}' not found on Workspace '{workspace.Id}'. Rollback to workspace ID.";
                throw new FolderNotFoundException(message);
            }
            
        }

        private void SaveForm_PostOnOK(object sender, WFormEventArgs e)
        {
            ExecuteOperation(() =>
            {
                var fileIdVariable = AddinWrapper.GetDocVariableN(AddinConstants.FILE_ID);
                if (fileIdVariable != 0)
                {
                    AddinWrapper.RunScript(AddinConstants.SHOW_WIZARD, new string[] { AddinConstants.FILE_ID }, new object[] { fileIdVariable });
                }
            });
        }

    }
   
}