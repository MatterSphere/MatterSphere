using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using iManageWork10.Shell.Commands;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.JsonResponses.Enums;
using iManageWork10.Shell.RestAPI;
using iManageWork10.Shell.RestAPI.RestAPIManagement;
using iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties;
using iManageWork10.Shell.Validators;
using iManageWork10HostRestApi;

namespace iManageWork10MSSamples
{
    public partial class MainForm : Form
    {

        private readonly BrowserHelper _browserHelper;

        private string Url
        {
            get
            {
                return comboBox1.SelectedItem.ToString();
            }
        }
        

        private UserManagement _userManagement;

        private RolesManagement _rolesManagement;

        private DocumentsManagement _documentsManagement;

        private WorkspacesManagement _workspacesManagement;

        private FoldersManagement _foldersManagement;

        private LibrariesManagement _librariesManagement;

        private CommandsExecuter _commandsExecuter;

        private RestApiClient _restApiClient;

        public MainForm()
        {
            InitializeComponent();
            _browserHelper = new BrowserHelper();
            comboBox1.Items.Add("https://test1.NET");
            comboBox1.Items.Add("https://test2.NET");
            comboBox1.Items.Add("https://cloudimanage.com");
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = comboBox1.Items[0];
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }
        
        private void btnConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void ShowUrl(string baseUrl, string token)
        {
            _browserHelper.SetCookie(baseUrl, "imToken", token);
            webBrowser1.Navigate(baseUrl);
        }

        private void ExecuteOperation(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (HttpException exception)
            {
                Log(exception.GetHttpCode().ToString());
                Log(exception.Message);
                Log(exception.StackTrace);
            }
            catch (Exception exception)
            {
                Log(exception.Message);
                Log(exception.StackTrace);
            }
        }

        private void Connect()
        {
            ExecuteOperation(() =>
            {
                _restApiClient = new RestApiClient(new HostRestApiWorkerProvider(Url));
                _userManagement = new UserManagement(_restApiClient);
                _rolesManagement = new RolesManagement(_restApiClient);
                _workspacesManagement = new WorkspacesManagement(_restApiClient);
                _foldersManagement = new FoldersManagement(_restApiClient);
                _documentsManagement = new DocumentsManagement(_restApiClient);
                _librariesManagement = new LibrariesManagement(_restApiClient);
                _commandsExecuter = new CommandsExecuter(_restApiClient);
                tabControlMain.Enabled = true;
                ShowUrl(Url, _restApiClient.AuthToken);
                Log("Connected.");
            });
        }

        private void Disconnect()
        {
            if (_restApiClient != null)
            {
                _restApiClient.Disconnect();
                _restApiClient = null;
                webBrowser1.Navigate(string.Empty);
                tabControlMain.Enabled = false;
                Log("Disconnected.");
            }
        }
        
        private void btnGetLoggedUser_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                var currentUser = _userManagement.GetCurrentUserProfile();
                Log(currentUser.ToString());
            });
        }

        private void btnGetRoles_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                foreach (var role in _rolesManagement.GetRoles())
                {
                    Log(role.ToString());
                }
            });
        }

        private void Log(string message)
        {
            texbBoxLog.AppendText($"{DateTime.Now:s} - {message}{Environment.NewLine}");
            texbBoxLog.Select(texbBoxLog.TextLength - 1,0);
            texbBoxLog.ScrollToCaret();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void btnSearchWorkspaces_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                var properties = new SearchWorkspacesProperties
                {
                    Custom1 = textBoxCustom1.Text,
                    Custom2 = textBoxCustom2.Text
                };

                var libraryIds = textBoxWorkspaceLibraries.Text;
                if (!string.IsNullOrEmpty(libraryIds))
                {
                    LibrariesValidator validator = new LibrariesValidator(_restApiClient);
                    try
                    {
                        validator.Validate(libraryIds);
                        properties.Libraries = libraryIds;
                    }
                    catch (ArgumentException ex)
                    {
                        Log(ex.Message);
                    }
                    
                }

                var workspaces = _workspacesManagement.SearchWorkspaces(properties);
                if (checkBoxOpenWorkspaceInBrowser.Checked)
                {
                    var workspace = workspaces.First();
                    var currentUser = _userManagement.GetCurrentUserProfile();
                    var workspaceAccess = _workspacesManagement.GetWorkspaceUserSecurity(workspace.Id, currentUser.Id)
                        .Access;
                    workspaceAccess.ValidateAccess();
                    var url = string.IsNullOrEmpty(textBoxCustom2.Text)
                            ? $"{Url}/work/web/r/custom1/{textBoxCustom1.Text}?exclude_email=true&scope={_restApiClient.PreferredLibrary}"
                            : $"{Url}/work/web/r/libraries/{_restApiClient.PreferredLibrary}/workspaces/{workspace.Id}";

                        Log(url);
                        ShowUrl(url, _restApiClient.AuthToken);
                }
                foreach (var workspace in workspaces)
                {
                    Log(workspace.ToString());
                }
            });
        }

        private void btnGetWorkspace_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                string workspaceId = textBoxWorkspaceId.Text;
                if (string.IsNullOrEmpty(workspaceId))
                {
                    throw new Exception("WorkspaceId couldn't be empty.");
                }

                Log(_workspacesManagement.GetWorkspace(workspaceId).ToString());
            });
        }

        private void btnGetWorkspaceUserSecurity_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                string workspaceId = textBoxWorkspaceId.Text;
                if (string.IsNullOrEmpty(workspaceId))
                {
                    throw new Exception("WorkspaceId couldn't be empty.");
                }

                var currentUser = _userManagement.GetCurrentUserProfile();
                Log(_workspacesManagement.GetWorkspaceUserSecurity(workspaceId, currentUser.Id).ToString());
            });
        }

        private void btnSearchFolders_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                string workspaceId = textBoxWorkspaceId.Text;
                if (string.IsNullOrEmpty(workspaceId))
                {
                    throw new Exception("WorkspaceId couldn't be empty.");
                }

                string folderName = textBoxFolderName.Text;

                foreach (var folder in _workspacesManagement.SearchFolders(workspaceId, new SearchFoldersProperties() { Name = folderName }))
                {
                    Log(folder.ToString());
                }
            });
        }

        private void btnGetFolderDocuments_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                string folderId = textBoxFolderId.Text;
                if (string.IsNullOrEmpty(folderId))
                {
                    throw new Exception("FolderId couldn't be empty.");
                }

                foreach (var document in _foldersManagement.GetFolderDocuments(folderId, null))
                {
                    Log(document.ToString());
                }
            });
        }

        private void btnGetDocumentOperations_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                string documentId = textBoxDocmentId.Text;
                if (string.IsNullOrEmpty(documentId))
                {
                    throw new Exception("DocumentId couldn't be empty.");
                }

                Log(_documentsManagement.GetDocumentOperations(documentId).ToString());

            });
        }

        private void btnGetDocumentProfile_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                string documentId = textBoxDocmentId.Text;
                if (string.IsNullOrEmpty(documentId))
                {
                    throw new Exception("DocumentId couldn't be empty.");
                }

                Log(_documentsManagement.GetDocumentProfile(documentId).ToString());

            });
        }

        private void btnReplaceDocumentContent_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                string documentId = textBoxDocmentId.Text;
                if (string.IsNullOrEmpty(documentId))
                {
                    throw new Exception("DocumentId couldn't be empty.");
                }

                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(openFileDialog.FileName);
                    Log(fi.FullName);
                    Log(_documentsManagement.ReplaceDocumentContent(documentId, fi.FullName).ToString());
                }

            });
        }

        private void btnGetDocumentCheckOut_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                string documentId = textBoxDocmentId.Text;
                if (string.IsNullOrEmpty(documentId))
                {
                    throw new Exception("DocumentId couldn't be empty.");
                }

                var documentCheckOut = _documentsManagement.GetDocumentCheckOut(documentId);
                textBoxDueDate.Text = documentCheckOut.DueDate;
                textBoxComments.Text = documentCheckOut.Comments;
                textBoxPath.Text = documentCheckOut.Path;
                textBoxLocation.Text = documentCheckOut.Location;

                Log(documentCheckOut.ToString());
            });
        }

        private void btnDeleteDocumentLock_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                string documentId = textBoxDocmentId.Text;
                if (string.IsNullOrEmpty(documentId))
                {
                    throw new Exception("DocumentId couldn't be empty.");
                }

                _documentsManagement.DeleteDocumentLock(documentId);

                try
                {
                    _documentsManagement.GetDocumentCheckOut(documentId);
                }
                catch (HttpException exception)
                {
                    if (exception.GetHttpCode() == 404)
                    {
                        Log("Document lock deleted.");
                    }
                }
            });
        }

        private void btnPostDocumentLock_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                string documentId = textBoxDocmentId.Text;
                if (string.IsNullOrEmpty(documentId))
                {
                    throw new Exception("DocumentId couldn't be empty.");
                }
                
                _documentsManagement.PostDocumentLock(documentId, new PostDocumentLockProperties
                {
                    Comments = textBoxComments.Text,
                    DueDate = textBoxDueDate.Text,
                    Location = textBoxLocation.Text,
                    Path = textBoxPath.Text
                });
            });
        }

        private void buttonGetLibraries_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                foreach (var library in _librariesManagement.GetLibraries())
                {
                    Log(library.ToString());
                }
            });
        }

        private void btnUploadNewDocument_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                string folderId = textBoxFolderId.Text;
                if (string.IsNullOrEmpty(folderId))
                {
                    throw new Exception("FolderId couldn't be empty.");
                }

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var fileInfo = new FileInfo(openFileDialog.FileName);
                        var documentProfile = _foldersManagement.PostFolderDocument(
                            folderId,
                            new DocumentProfile
                            {
                                Name = textBoxDocumentName.Text,
                                Extension = fileInfo.Extension.TrimStart('.')
                            }, 
                            fileInfo.FullName);
                        Log(documentProfile.ToString());
                    }
                }
            });
        }

        private void btnCreateWorkspace_Click(object sender, EventArgs e)
        {
            ExecuteOperation(() =>
            {
                var custom1 = textBoxCustom1.Text;
                var custom2 = textBoxCustom2.Text;
                var libraryIds = textBoxWorkspaceLibraries.Text;
                var workspace = new Workspace()
                {
                    Custom1 = custom1,
                    Custom2 = custom2,
                    Database = libraryIds
                };

                var command = new CreateWorkspaceCommand(workspace);
                _commandsExecuter.Execute(command);
            });
        }
    }
}
