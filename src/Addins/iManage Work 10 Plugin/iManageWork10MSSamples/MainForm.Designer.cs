namespace iManageWork10MSSamples
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGetWorkspace = new System.Windows.Forms.Button();
            this.texbBoxLog = new System.Windows.Forms.RichTextBox();
            this.btnGetRoles = new System.Windows.Forms.Button();
            this.btnGetLoggedUser = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageRest = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControlRest = new System.Windows.Forms.TabControl();
            this.tabPageRolesManagement = new System.Windows.Forms.TabPage();
            this.tabPageWorkspacesManagement = new System.Windows.Forms.TabPage();
            this.checkBoxMultiDbSearch = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxWorkspaceLibraries = new System.Windows.Forms.TextBox();
            this.checkBoxOpenWorkspaceInBrowser = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSearchFolders = new System.Windows.Forms.Button();
            this.textBoxFolderName = new System.Windows.Forms.TextBox();
            this.btnGetWorkspaceUserSecurity = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxWorkspaceId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCustom2 = new System.Windows.Forms.TextBox();
            this.textBoxCustom1 = new System.Windows.Forms.TextBox();
            this.btnSearchWorkspaces = new System.Windows.Forms.Button();
            this.tabPageFoldersMangement = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxDocumentName = new System.Windows.Forms.TextBox();
            this.btnUploadNewDocument = new System.Windows.Forms.Button();
            this.textBoxFolderId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnGetFolderDocuments = new System.Windows.Forms.Button();
            this.tabPageDocumentsManagement = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxLocation = new System.Windows.Forms.TextBox();
            this.textBoxComments = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxDueDate = new System.Windows.Forms.TextBox();
            this.btnPostDocumentLock = new System.Windows.Forms.Button();
            this.btnDeleteDocumentLock = new System.Windows.Forms.Button();
            this.btnGetDocumentCheckOut = new System.Windows.Forms.Button();
            this.btnReplaceDocumentContent = new System.Windows.Forms.Button();
            this.btnGetDocumentProfile = new System.Windows.Forms.Button();
            this.textBoxDocmentId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnGetDocumentOperations = new System.Windows.Forms.Button();
            this.tabPageMisc = new System.Windows.Forms.TabPage();
            this.buttonGetLibraries = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnCreateWorkspace = new System.Windows.Forms.Button();
            this.tabControlMain.SuspendLayout();
            this.tabPageRest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlRest.SuspendLayout();
            this.tabPageRolesManagement.SuspendLayout();
            this.tabPageWorkspacesManagement.SuspendLayout();
            this.tabPageFoldersMangement.SuspendLayout();
            this.tabPageDocumentsManagement.SuspendLayout();
            this.tabPageMisc.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetWorkspace
            // 
            this.btnGetWorkspace.Location = new System.Drawing.Point(26, 85);
            this.btnGetWorkspace.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetWorkspace.Name = "btnGetWorkspace";
            this.btnGetWorkspace.Size = new System.Drawing.Size(156, 25);
            this.btnGetWorkspace.TabIndex = 1;
            this.btnGetWorkspace.Text = "Get workspace";
            this.btnGetWorkspace.UseVisualStyleBackColor = true;
            this.btnGetWorkspace.Click += new System.EventHandler(this.btnGetWorkspace_Click);
            // 
            // texbBoxLog
            // 
            this.texbBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texbBoxLog.Location = new System.Drawing.Point(0, 0);
            this.texbBoxLog.Margin = new System.Windows.Forms.Padding(2);
            this.texbBoxLog.Name = "texbBoxLog";
            this.texbBoxLog.Size = new System.Drawing.Size(964, 278);
            this.texbBoxLog.TabIndex = 2;
            this.texbBoxLog.Text = "";
            this.texbBoxLog.WordWrap = false;
            // 
            // btnGetRoles
            // 
            this.btnGetRoles.Location = new System.Drawing.Point(5, 5);
            this.btnGetRoles.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetRoles.Name = "btnGetRoles";
            this.btnGetRoles.Size = new System.Drawing.Size(125, 25);
            this.btnGetRoles.TabIndex = 4;
            this.btnGetRoles.Text = "Get roles";
            this.btnGetRoles.UseVisualStyleBackColor = true;
            this.btnGetRoles.Click += new System.EventHandler(this.btnGetRoles_Click);
            // 
            // btnGetLoggedUser
            // 
            this.btnGetLoggedUser.Location = new System.Drawing.Point(5, 5);
            this.btnGetLoggedUser.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetLoggedUser.Name = "btnGetLoggedUser";
            this.btnGetLoggedUser.Size = new System.Drawing.Size(125, 25);
            this.btnGetLoggedUser.TabIndex = 6;
            this.btnGetLoggedUser.Text = "Get logged-in user";
            this.btnGetLoggedUser.UseVisualStyleBackColor = true;
            this.btnGetLoggedUser.Click += new System.EventHandler(this.btnGetLoggedUser_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(9, 10);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(398, 21);
            this.comboBox1.TabIndex = 6;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(this.tabPageRest);
            this.tabControlMain.Controls.Add(this.tabPage2);
            this.tabControlMain.Enabled = false;
            this.tabControlMain.Location = new System.Drawing.Point(9, 34);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(976, 593);
            this.tabControlMain.TabIndex = 7;
            // 
            // tabPageRest
            // 
            this.tabPageRest.Controls.Add(this.splitContainer1);
            this.tabPageRest.Location = new System.Drawing.Point(4, 22);
            this.tabPageRest.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageRest.Name = "tabPageRest";
            this.tabPageRest.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageRest.Size = new System.Drawing.Size(968, 567);
            this.tabPageRest.TabIndex = 0;
            this.tabPageRest.Text = "REST";
            this.tabPageRest.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(2, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControlRest);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.texbBoxLog);
            this.splitContainer1.Size = new System.Drawing.Size(964, 563);
            this.splitContainer1.SplitterDistance = 281;
            this.splitContainer1.TabIndex = 10;
            // 
            // tabControlRest
            // 
            this.tabControlRest.Controls.Add(this.tabPageRolesManagement);
            this.tabControlRest.Controls.Add(this.tabPageWorkspacesManagement);
            this.tabControlRest.Controls.Add(this.tabPageFoldersMangement);
            this.tabControlRest.Controls.Add(this.tabPageDocumentsManagement);
            this.tabControlRest.Controls.Add(this.tabPageMisc);
            this.tabControlRest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlRest.Location = new System.Drawing.Point(0, 0);
            this.tabControlRest.Name = "tabControlRest";
            this.tabControlRest.SelectedIndex = 0;
            this.tabControlRest.Size = new System.Drawing.Size(964, 281);
            this.tabControlRest.TabIndex = 9;
            // 
            // tabPageRolesManagement
            // 
            this.tabPageRolesManagement.Controls.Add(this.btnGetRoles);
            this.tabPageRolesManagement.Location = new System.Drawing.Point(4, 22);
            this.tabPageRolesManagement.Name = "tabPageRolesManagement";
            this.tabPageRolesManagement.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRolesManagement.Size = new System.Drawing.Size(956, 255);
            this.tabPageRolesManagement.TabIndex = 1;
            this.tabPageRolesManagement.Text = "Roles Management";
            this.tabPageRolesManagement.UseVisualStyleBackColor = true;
            // 
            // tabPageWorkspacesManagement
            // 
            this.tabPageWorkspacesManagement.Controls.Add(this.btnCreateWorkspace);
            this.tabPageWorkspacesManagement.Controls.Add(this.checkBoxMultiDbSearch);
            this.tabPageWorkspacesManagement.Controls.Add(this.label11);
            this.tabPageWorkspacesManagement.Controls.Add(this.textBoxWorkspaceLibraries);
            this.tabPageWorkspacesManagement.Controls.Add(this.checkBoxOpenWorkspaceInBrowser);
            this.tabPageWorkspacesManagement.Controls.Add(this.label4);
            this.tabPageWorkspacesManagement.Controls.Add(this.btnSearchFolders);
            this.tabPageWorkspacesManagement.Controls.Add(this.textBoxFolderName);
            this.tabPageWorkspacesManagement.Controls.Add(this.btnGetWorkspaceUserSecurity);
            this.tabPageWorkspacesManagement.Controls.Add(this.label3);
            this.tabPageWorkspacesManagement.Controls.Add(this.textBoxWorkspaceId);
            this.tabPageWorkspacesManagement.Controls.Add(this.btnGetWorkspace);
            this.tabPageWorkspacesManagement.Controls.Add(this.label2);
            this.tabPageWorkspacesManagement.Controls.Add(this.label1);
            this.tabPageWorkspacesManagement.Controls.Add(this.textBoxCustom2);
            this.tabPageWorkspacesManagement.Controls.Add(this.textBoxCustom1);
            this.tabPageWorkspacesManagement.Controls.Add(this.btnSearchWorkspaces);
            this.tabPageWorkspacesManagement.Location = new System.Drawing.Point(4, 22);
            this.tabPageWorkspacesManagement.Name = "tabPageWorkspacesManagement";
            this.tabPageWorkspacesManagement.Size = new System.Drawing.Size(956, 255);
            this.tabPageWorkspacesManagement.TabIndex = 2;
            this.tabPageWorkspacesManagement.Text = "Workspaces Management";
            this.tabPageWorkspacesManagement.UseVisualStyleBackColor = true;
            // 
            // checkBoxMultiDbSearch
            // 
            this.checkBoxMultiDbSearch.AutoSize = true;
            this.checkBoxMultiDbSearch.Location = new System.Drawing.Point(199, 50);
            this.checkBoxMultiDbSearch.Name = "checkBoxMultiDbSearch";
            this.checkBoxMultiDbSearch.Size = new System.Drawing.Size(113, 17);
            this.checkBoxMultiDbSearch.TabIndex = 14;
            this.checkBoxMultiDbSearch.Text = "Search in Multi Db";
            this.checkBoxMultiDbSearch.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(407, 53);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "Libraries";
            // 
            // textBoxWorkspaceLibraries
            // 
            this.textBoxWorkspaceLibraries.Location = new System.Drawing.Point(459, 50);
            this.textBoxWorkspaceLibraries.Name = "textBoxWorkspaceLibraries";
            this.textBoxWorkspaceLibraries.Size = new System.Drawing.Size(53, 20);
            this.textBoxWorkspaceLibraries.TabIndex = 12;
            // 
            // checkBoxOpenWorkspaceInBrowser
            // 
            this.checkBoxOpenWorkspaceInBrowser.AutoSize = true;
            this.checkBoxOpenWorkspaceInBrowser.Location = new System.Drawing.Point(199, 27);
            this.checkBoxOpenWorkspaceInBrowser.Name = "checkBoxOpenWorkspaceInBrowser";
            this.checkBoxOpenWorkspaceInBrowser.Size = new System.Drawing.Size(163, 17);
            this.checkBoxOpenWorkspaceInBrowser.TabIndex = 11;
            this.checkBoxOpenWorkspaceInBrowser.Text = "Open Workspace In Browser";
            this.checkBoxOpenWorkspaceInBrowser.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(564, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Folder Name";
            // 
            // btnSearchFolders
            // 
            this.btnSearchFolders.Location = new System.Drawing.Point(25, 143);
            this.btnSearchFolders.Name = "btnSearchFolders";
            this.btnSearchFolders.Size = new System.Drawing.Size(157, 23);
            this.btnSearchFolders.TabIndex = 9;
            this.btnSearchFolders.Text = "Search Folders";
            this.btnSearchFolders.UseVisualStyleBackColor = true;
            this.btnSearchFolders.Click += new System.EventHandler(this.btnSearchFolders_Click);
            // 
            // textBoxFolderName
            // 
            this.textBoxFolderName.Location = new System.Drawing.Point(637, 133);
            this.textBoxFolderName.Name = "textBoxFolderName";
            this.textBoxFolderName.Size = new System.Drawing.Size(100, 20);
            this.textBoxFolderName.TabIndex = 8;
            // 
            // btnGetWorkspaceUserSecurity
            // 
            this.btnGetWorkspaceUserSecurity.Location = new System.Drawing.Point(26, 115);
            this.btnGetWorkspaceUserSecurity.Name = "btnGetWorkspaceUserSecurity";
            this.btnGetWorkspaceUserSecurity.Size = new System.Drawing.Size(156, 23);
            this.btnGetWorkspaceUserSecurity.TabIndex = 7;
            this.btnGetWorkspaceUserSecurity.Text = "Get workspace user security";
            this.btnGetWorkspaceUserSecurity.UseVisualStyleBackColor = true;
            this.btnGetWorkspaceUserSecurity.Click += new System.EventHandler(this.btnGetWorkspaceUserSecurity_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(377, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "WorkspaceId";
            // 
            // textBoxWorkspaceId
            // 
            this.textBoxWorkspaceId.Location = new System.Drawing.Point(459, 86);
            this.textBoxWorkspaceId.Multiline = true;
            this.textBoxWorkspaceId.Name = "textBoxWorkspaceId";
            this.textBoxWorkspaceId.Size = new System.Drawing.Size(100, 75);
            this.textBoxWorkspaceId.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(571, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Custom2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(405, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Custom1";
            // 
            // textBoxCustom2
            // 
            this.textBoxCustom2.Location = new System.Drawing.Point(625, 20);
            this.textBoxCustom2.Multiline = true;
            this.textBoxCustom2.Name = "textBoxCustom2";
            this.textBoxCustom2.Size = new System.Drawing.Size(100, 21);
            this.textBoxCustom2.TabIndex = 2;
            // 
            // textBoxCustom1
            // 
            this.textBoxCustom1.Location = new System.Drawing.Point(459, 22);
            this.textBoxCustom1.Multiline = true;
            this.textBoxCustom1.Name = "textBoxCustom1";
            this.textBoxCustom1.Size = new System.Drawing.Size(100, 21);
            this.textBoxCustom1.TabIndex = 1;
            // 
            // btnSearchWorkspaces
            // 
            this.btnSearchWorkspaces.Location = new System.Drawing.Point(26, 25);
            this.btnSearchWorkspaces.Name = "btnSearchWorkspaces";
            this.btnSearchWorkspaces.Size = new System.Drawing.Size(156, 23);
            this.btnSearchWorkspaces.TabIndex = 0;
            this.btnSearchWorkspaces.Text = "Search Workspaces";
            this.btnSearchWorkspaces.UseVisualStyleBackColor = true;
            this.btnSearchWorkspaces.Click += new System.EventHandler(this.btnSearchWorkspaces_Click);
            // 
            // tabPageFoldersMangement
            // 
            this.tabPageFoldersMangement.Controls.Add(this.label13);
            this.tabPageFoldersMangement.Controls.Add(this.textBoxDocumentName);
            this.tabPageFoldersMangement.Controls.Add(this.btnUploadNewDocument);
            this.tabPageFoldersMangement.Controls.Add(this.textBoxFolderId);
            this.tabPageFoldersMangement.Controls.Add(this.label5);
            this.tabPageFoldersMangement.Controls.Add(this.btnGetFolderDocuments);
            this.tabPageFoldersMangement.Location = new System.Drawing.Point(4, 22);
            this.tabPageFoldersMangement.Name = "tabPageFoldersMangement";
            this.tabPageFoldersMangement.Size = new System.Drawing.Size(956, 255);
            this.tabPageFoldersMangement.TabIndex = 3;
            this.tabPageFoldersMangement.Text = "Folders Management";
            this.tabPageFoldersMangement.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(468, 45);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(81, 13);
            this.label13.TabIndex = 5;
            this.label13.Text = "Name (optional)";
            // 
            // textBoxDocumentName
            // 
            this.textBoxDocumentName.Location = new System.Drawing.Point(556, 40);
            this.textBoxDocumentName.Name = "textBoxDocumentName";
            this.textBoxDocumentName.Size = new System.Drawing.Size(100, 20);
            this.textBoxDocumentName.TabIndex = 4;
            // 
            // btnUploadNewDocument
            // 
            this.btnUploadNewDocument.Location = new System.Drawing.Point(13, 40);
            this.btnUploadNewDocument.Name = "btnUploadNewDocument";
            this.btnUploadNewDocument.Size = new System.Drawing.Size(146, 23);
            this.btnUploadNewDocument.TabIndex = 3;
            this.btnUploadNewDocument.Text = "Upload document to folder";
            this.btnUploadNewDocument.UseVisualStyleBackColor = true;
            this.btnUploadNewDocument.Click += new System.EventHandler(this.btnUploadNewDocument_Click);
            // 
            // textBoxFolderId
            // 
            this.textBoxFolderId.Location = new System.Drawing.Point(324, 13);
            this.textBoxFolderId.Multiline = true;
            this.textBoxFolderId.Name = "textBoxFolderId";
            this.textBoxFolderId.Size = new System.Drawing.Size(100, 50);
            this.textBoxFolderId.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(258, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Folder Id";
            // 
            // btnGetFolderDocuments
            // 
            this.btnGetFolderDocuments.Location = new System.Drawing.Point(13, 11);
            this.btnGetFolderDocuments.Name = "btnGetFolderDocuments";
            this.btnGetFolderDocuments.Size = new System.Drawing.Size(146, 23);
            this.btnGetFolderDocuments.TabIndex = 0;
            this.btnGetFolderDocuments.Text = "Get Folder Documents";
            this.btnGetFolderDocuments.UseVisualStyleBackColor = true;
            this.btnGetFolderDocuments.Click += new System.EventHandler(this.btnGetFolderDocuments_Click);
            // 
            // tabPageDocumentsManagement
            // 
            this.tabPageDocumentsManagement.Controls.Add(this.label10);
            this.tabPageDocumentsManagement.Controls.Add(this.label9);
            this.tabPageDocumentsManagement.Controls.Add(this.textBoxLocation);
            this.tabPageDocumentsManagement.Controls.Add(this.textBoxComments);
            this.tabPageDocumentsManagement.Controls.Add(this.label8);
            this.tabPageDocumentsManagement.Controls.Add(this.textBoxPath);
            this.tabPageDocumentsManagement.Controls.Add(this.label7);
            this.tabPageDocumentsManagement.Controls.Add(this.textBoxDueDate);
            this.tabPageDocumentsManagement.Controls.Add(this.btnPostDocumentLock);
            this.tabPageDocumentsManagement.Controls.Add(this.btnDeleteDocumentLock);
            this.tabPageDocumentsManagement.Controls.Add(this.btnGetDocumentCheckOut);
            this.tabPageDocumentsManagement.Controls.Add(this.btnReplaceDocumentContent);
            this.tabPageDocumentsManagement.Controls.Add(this.btnGetDocumentProfile);
            this.tabPageDocumentsManagement.Controls.Add(this.textBoxDocmentId);
            this.tabPageDocumentsManagement.Controls.Add(this.label6);
            this.tabPageDocumentsManagement.Controls.Add(this.btnGetDocumentOperations);
            this.tabPageDocumentsManagement.Location = new System.Drawing.Point(4, 22);
            this.tabPageDocumentsManagement.Name = "tabPageDocumentsManagement";
            this.tabPageDocumentsManagement.Size = new System.Drawing.Size(956, 255);
            this.tabPageDocumentsManagement.TabIndex = 4;
            this.tabPageDocumentsManagement.Text = "Documents Management";
            this.tabPageDocumentsManagement.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(829, 165);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Location";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(686, 165);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Comments";
            // 
            // textBoxLocation
            // 
            this.textBoxLocation.Location = new System.Drawing.Point(881, 162);
            this.textBoxLocation.Name = "textBoxLocation";
            this.textBoxLocation.Size = new System.Drawing.Size(72, 20);
            this.textBoxLocation.TabIndex = 13;
            // 
            // textBoxComments
            // 
            this.textBoxComments.Location = new System.Drawing.Point(748, 162);
            this.textBoxComments.Name = "textBoxComments";
            this.textBoxComments.Size = new System.Drawing.Size(75, 20);
            this.textBoxComments.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(574, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Path";
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(609, 162);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(71, 20);
            this.textBoxPath.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(415, 164);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Due Date";
            // 
            // textBoxDueDate
            // 
            this.textBoxDueDate.Location = new System.Drawing.Point(474, 161);
            this.textBoxDueDate.Name = "textBoxDueDate";
            this.textBoxDueDate.Size = new System.Drawing.Size(94, 20);
            this.textBoxDueDate.TabIndex = 8;
            // 
            // btnPostDocumentLock
            // 
            this.btnPostDocumentLock.AutoSize = true;
            this.btnPostDocumentLock.Location = new System.Drawing.Point(15, 159);
            this.btnPostDocumentLock.Name = "btnPostDocumentLock";
            this.btnPostDocumentLock.Size = new System.Drawing.Size(117, 23);
            this.btnPostDocumentLock.TabIndex = 7;
            this.btnPostDocumentLock.Text = "Post Document Lock";
            this.btnPostDocumentLock.UseVisualStyleBackColor = true;
            this.btnPostDocumentLock.Click += new System.EventHandler(this.btnPostDocumentLock_Click);
            // 
            // btnDeleteDocumentLock
            // 
            this.btnDeleteDocumentLock.AutoSize = true;
            this.btnDeleteDocumentLock.Location = new System.Drawing.Point(15, 130);
            this.btnDeleteDocumentLock.Name = "btnDeleteDocumentLock";
            this.btnDeleteDocumentLock.Size = new System.Drawing.Size(127, 23);
            this.btnDeleteDocumentLock.TabIndex = 6;
            this.btnDeleteDocumentLock.Text = "Delete Document Lock";
            this.btnDeleteDocumentLock.UseVisualStyleBackColor = true;
            this.btnDeleteDocumentLock.Click += new System.EventHandler(this.btnDeleteDocumentLock_Click);
            // 
            // btnGetDocumentCheckOut
            // 
            this.btnGetDocumentCheckOut.AutoSize = true;
            this.btnGetDocumentCheckOut.Location = new System.Drawing.Point(15, 101);
            this.btnGetDocumentCheckOut.Name = "btnGetDocumentCheckOut";
            this.btnGetDocumentCheckOut.Size = new System.Drawing.Size(140, 23);
            this.btnGetDocumentCheckOut.TabIndex = 5;
            this.btnGetDocumentCheckOut.Text = "Get Document Check Out";
            this.btnGetDocumentCheckOut.UseVisualStyleBackColor = true;
            this.btnGetDocumentCheckOut.Click += new System.EventHandler(this.btnGetDocumentCheckOut_Click);
            // 
            // btnReplaceDocumentContent
            // 
            this.btnReplaceDocumentContent.AutoSize = true;
            this.btnReplaceDocumentContent.Location = new System.Drawing.Point(15, 72);
            this.btnReplaceDocumentContent.Name = "btnReplaceDocumentContent";
            this.btnReplaceDocumentContent.Size = new System.Drawing.Size(149, 23);
            this.btnReplaceDocumentContent.TabIndex = 4;
            this.btnReplaceDocumentContent.Text = "Replace Document Content";
            this.btnReplaceDocumentContent.UseVisualStyleBackColor = true;
            this.btnReplaceDocumentContent.Click += new System.EventHandler(this.btnReplaceDocumentContent_Click);
            // 
            // btnGetDocumentProfile
            // 
            this.btnGetDocumentProfile.AutoSize = true;
            this.btnGetDocumentProfile.Location = new System.Drawing.Point(15, 15);
            this.btnGetDocumentProfile.Name = "btnGetDocumentProfile";
            this.btnGetDocumentProfile.Size = new System.Drawing.Size(118, 23);
            this.btnGetDocumentProfile.TabIndex = 3;
            this.btnGetDocumentProfile.Text = "Get Document Profile";
            this.btnGetDocumentProfile.UseVisualStyleBackColor = true;
            this.btnGetDocumentProfile.Click += new System.EventHandler(this.btnGetDocumentProfile_Click);
            // 
            // textBoxDocmentId
            // 
            this.textBoxDocmentId.Location = new System.Drawing.Point(306, 15);
            this.textBoxDocmentId.Multiline = true;
            this.textBoxDocmentId.Name = "textBoxDocmentId";
            this.textBoxDocmentId.Size = new System.Drawing.Size(100, 135);
            this.textBoxDocmentId.TabIndex = 2;
            this.textBoxDocmentId.Text = "WS10!342.1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(215, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Document Id";
            // 
            // btnGetDocumentOperations
            // 
            this.btnGetDocumentOperations.AutoSize = true;
            this.btnGetDocumentOperations.Location = new System.Drawing.Point(15, 43);
            this.btnGetDocumentOperations.Name = "btnGetDocumentOperations";
            this.btnGetDocumentOperations.Size = new System.Drawing.Size(140, 23);
            this.btnGetDocumentOperations.TabIndex = 0;
            this.btnGetDocumentOperations.Text = "Get Document Operations";
            this.btnGetDocumentOperations.UseVisualStyleBackColor = true;
            this.btnGetDocumentOperations.Click += new System.EventHandler(this.btnGetDocumentOperations_Click);
            // 
            // tabPageMisc
            // 
            this.tabPageMisc.Controls.Add(this.buttonGetLibraries);
            this.tabPageMisc.Controls.Add(this.btnGetLoggedUser);
            this.tabPageMisc.Location = new System.Drawing.Point(4, 22);
            this.tabPageMisc.Name = "tabPageMisc";
            this.tabPageMisc.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMisc.Size = new System.Drawing.Size(956, 255);
            this.tabPageMisc.TabIndex = 0;
            this.tabPageMisc.Text = "Misc";
            this.tabPageMisc.UseVisualStyleBackColor = true;
            // 
            // buttonGetLibraries
            // 
            this.buttonGetLibraries.Location = new System.Drawing.Point(5, 35);
            this.buttonGetLibraries.Name = "buttonGetLibraries";
            this.buttonGetLibraries.Size = new System.Drawing.Size(125, 23);
            this.buttonGetLibraries.TabIndex = 7;
            this.buttonGetLibraries.Text = "Get libraries";
            this.buttonGetLibraries.UseVisualStyleBackColor = true;
            this.buttonGetLibraries.Click += new System.EventHandler(this.buttonGetLibraries_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.webBrowser1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(968, 567);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Explorer";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(2, 2);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(2);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(15, 16);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(964, 563);
            this.webBrowser1.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(412, 7);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 25);
            this.btnConnect.TabIndex = 7;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(493, 7);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 25);
            this.btnDisconnect.TabIndex = 8;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnCreateWorkspace
            // 
            this.btnCreateWorkspace.Location = new System.Drawing.Point(25, 173);
            this.btnCreateWorkspace.Name = "btnCreateWorkspace";
            this.btnCreateWorkspace.Size = new System.Drawing.Size(157, 23);
            this.btnCreateWorkspace.TabIndex = 15;
            this.btnCreateWorkspace.Text = "Create Workspace";
            this.btnCreateWorkspace.UseVisualStyleBackColor = true;
            this.btnCreateWorkspace.Click += new System.EventHandler(this.btnCreateWorkspace_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 630);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnConnect);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iManage Work 10 samples";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageRest.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlRest.ResumeLayout(false);
            this.tabPageRolesManagement.ResumeLayout(false);
            this.tabPageWorkspacesManagement.ResumeLayout(false);
            this.tabPageWorkspacesManagement.PerformLayout();
            this.tabPageFoldersMangement.ResumeLayout(false);
            this.tabPageFoldersMangement.PerformLayout();
            this.tabPageDocumentsManagement.ResumeLayout(false);
            this.tabPageDocumentsManagement.PerformLayout();
            this.tabPageMisc.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnGetWorkspace;
        private System.Windows.Forms.RichTextBox texbBoxLog;
        private System.Windows.Forms.Button btnGetRoles;
        private System.Windows.Forms.Button btnGetLoggedUser;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageRest;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TabControl tabControlRest;
        private System.Windows.Forms.TabPage tabPageMisc;
        private System.Windows.Forms.TabPage tabPageRolesManagement;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.TabPage tabPageWorkspacesManagement;
        private System.Windows.Forms.Button btnSearchWorkspaces;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCustom2;
        private System.Windows.Forms.TextBox textBoxCustom1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxWorkspaceId;
        private System.Windows.Forms.Button btnGetWorkspaceUserSecurity;
        private System.Windows.Forms.Button btnSearchFolders;
        private System.Windows.Forms.TextBox textBoxFolderName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPageFoldersMangement;
        private System.Windows.Forms.TextBox textBoxFolderId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnGetFolderDocuments;
        private System.Windows.Forms.TabPage tabPageDocumentsManagement;
        private System.Windows.Forms.TextBox textBoxDocmentId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnGetDocumentOperations;
        private System.Windows.Forms.Button btnGetDocumentProfile;
        private System.Windows.Forms.Button btnReplaceDocumentContent;
        private System.Windows.Forms.Button btnGetDocumentCheckOut;
        private System.Windows.Forms.Button btnDeleteDocumentLock;
        private System.Windows.Forms.CheckBox checkBoxOpenWorkspaceInBrowser;
        private System.Windows.Forms.Button btnPostDocumentLock;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxDueDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.TextBox textBoxLocation;
        private System.Windows.Forms.TextBox textBoxComments;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxWorkspaceLibraries;
        private System.Windows.Forms.CheckBox checkBoxMultiDbSearch;
        private System.Windows.Forms.Button buttonGetLibraries;
        private System.Windows.Forms.Button btnUploadNewDocument;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxDocumentName;
        private System.Windows.Forms.Button btnCreateWorkspace;
    }
}

