using System.Windows.Forms;
using ActiproSoftware.SyntaxEditor;

namespace FWBS.OMS.Design.CodeBuilder
{
    partial class CodeSurfaceV2
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeSurfaceV2));
            ActiproSoftware.SyntaxEditor.Document document1 = new ActiproSoftware.SyntaxEditor.Document();
            this.toolCloseScript = new System.Windows.Forms.ToolStripButton();
            this.redoNames = new System.Windows.Forms.ToolStripDropDown();
            this.redoName1MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoName2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoName3MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoName4MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoName5MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editRedoButton = new System.Windows.Forms.ToolStripSplitButton();
            this.undoNames = new System.Windows.Forms.ToolStripDropDown();
            this.undoName1MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoName2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoName3MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoName4MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoName5MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editUndoButton = new System.Windows.Forms.ToolStripSplitButton();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.uiToolBarDockAreaPanel = new System.Windows.Forms.Panel();
            this.controlContainerPanel = new System.Windows.Forms.Panel();
            this.editor = new ActiproSoftware.SyntaxEditor.SyntaxEditor();
            this.horizontalSplitter = new System.Windows.Forms.Splitter();
            this.bottomTabControl = new FWBS.OMS.UI.TabControl();
            this.eventsTabPage = new System.Windows.Forms.TabPage();
            this.consoleTextBox = new System.Windows.Forms.TextBox();
            this.errorsTabPage = new System.Windows.Forms.TabPage();
            this.errorsListView = new FWBS.OMS.UI.ListView();
            this.lineColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.messageColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeMemberDropDownList = new ActiproSoftware.SyntaxEditor.Addons.DotNet.Dom.TypeMemberDropDownList();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.fileSaveButton = new System.Windows.Forms.ToolStripButton();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.filePrintButton = new System.Windows.Forms.ToolStripButton();
            this.filePrintPreviewButton = new System.Windows.Forms.ToolStripButton();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.editCutButton = new System.Windows.Forms.ToolStripButton();
            this.editCopyButton = new System.Windows.Forms.ToolStripButton();
            this.editPasteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.searchFindReplaceButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.editCommentSelectionButton = new System.Windows.Forms.ToolStripButton();
            this.editUncommentSelectionButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.editListMembersButton = new System.Windows.Forms.ToolStripButton();
            this.editCompleteWordButton = new System.Windows.Forms.ToolStripButton();
            this.statusBar = new FWBS.OMS.UI.StatusBar();
            this.positionPanel = new System.Windows.Forms.StatusBarPanel();
            this.languagePanel = new System.Windows.Forms.StatusBarPanel();
            this.statePanel = new System.Windows.Forms.StatusBarPanel();
            this.tokenPanel = new System.Windows.Forms.StatusBarPanel();
            this.hierarchyLevelPanel = new System.Windows.Forms.StatusBarPanel();
            this.messagePanel = new System.Windows.Forms.StatusBarPanel();
            this.menuStrip2 = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuStrip();
            this.fileMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.fileNewMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.mnuSaveCompile = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.CompileMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.filePrintMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.filePageSetupMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.filePrintPreviewMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.fileCloseScriptMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.editMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.editUndoMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.editRedoMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.editCutMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.editCopyMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.editPasteMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolsWorkflowEditMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripMenuItem1 = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolsWorkflowInsertMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.workflowActivityToolStripMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuInsertEC = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.mnuInsertIEC = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.mnuInsertC = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.editInsertCodeSnippetMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolsMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolsHighlightingStyleEditorMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolsTextStatisticsMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.mnuSettings = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.mnuShowLineNumbers = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.languageMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.languageCSharpMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.languageVBDotNetMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.languageNotSpecifiedMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRevert = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCompileToFile = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuReferences = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.regenerateIntellisenseToolStripMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.tsVersionControl = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.mnuCheckin = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.mnuCompare = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.searchMenu = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.searchFindMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.searchIncrementalSearchMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.searchReverseIncrementalSearchMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.searchGoToLineMenuItem = new FWBS.OMS.Design.CodeBuilder.CodeSurfaceMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.resourceLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.redoNames.SuspendLayout();
            this.undoNames.SuspendLayout();
            this.uiToolBarDockAreaPanel.SuspendLayout();
            this.controlContainerPanel.SuspendLayout();
            this.bottomTabControl.SuspendLayout();
            this.eventsTabPage.SuspendLayout();
            this.errorsTabPage.SuspendLayout();
            this.toolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.languagePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tokenPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hierarchyLevelPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.messagePanel)).BeginInit();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolCloseScript
            // 
            this.toolCloseScript.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolCloseScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resourceLookup.SetLookup(this.toolCloseScript, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_CLOSE", "Close", "Close Script"));
            this.toolCloseScript.Name = "toolCloseScript";
            this.toolCloseScript.Size = new System.Drawing.Size(40, 22);
            this.toolCloseScript.Text = "Close";
            this.toolCloseScript.Click += new System.EventHandler(this.fileCloseScriptMenuItem_Click);
            // 
            // redoNames
            // 
            this.redoNames.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.redoName1MenuItem,
            this.redoName2MenuItem,
            this.redoName3MenuItem,
            this.redoName4MenuItem,
            this.redoName5MenuItem});
            this.redoNames.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.redoNames.Name = "redoNames";
            this.redoNames.OwnerItem = this.editRedoButton;
            this.redoNames.Size = new System.Drawing.Size(46, 109);
            this.redoNames.Click += new System.EventHandler(this.redoNamesMenuItem_Popup);
            // 
            // redoName1MenuItem
            // 
            this.redoName1MenuItem.Name = "redoName1MenuItem";
            this.redoName1MenuItem.Size = new System.Drawing.Size(44, 21);
            this.redoName1MenuItem.Tag = 0;
            this.redoName1MenuItem.Text = "Redo1";
            this.redoName1MenuItem.Click += new System.EventHandler(this.redoName1MenuItem_Click);
            // 
            // redoName2MenuItem
            // 
            this.redoName2MenuItem.Name = "redoName2MenuItem";
            this.redoName2MenuItem.Size = new System.Drawing.Size(44, 21);
            this.redoName2MenuItem.Tag = 1;
            this.redoName2MenuItem.Text = "Redo2";
            this.redoName2MenuItem.Click += new System.EventHandler(this.redoName2MenuItem_Click);
            // 
            // redoName3MenuItem
            // 
            this.redoName3MenuItem.Name = "redoName3MenuItem";
            this.redoName3MenuItem.Size = new System.Drawing.Size(44, 21);
            this.redoName3MenuItem.Tag = 2;
            this.redoName3MenuItem.Text = "Redo3";
            this.redoName3MenuItem.Click += new System.EventHandler(this.redoName3MenuItem_Click);
            // 
            // redoName4MenuItem
            // 
            this.redoName4MenuItem.Name = "redoName4MenuItem";
            this.redoName4MenuItem.Size = new System.Drawing.Size(44, 21);
            this.redoName4MenuItem.Tag = 3;
            this.redoName4MenuItem.Text = "Redo4";
            this.redoName4MenuItem.Click += new System.EventHandler(this.redoName4MenuItem_Click);
            // 
            // redoName5MenuItem
            // 
            this.redoName5MenuItem.Name = "redoName5MenuItem";
            this.redoName5MenuItem.Size = new System.Drawing.Size(44, 21);
            this.redoName5MenuItem.Tag = 4;
            this.redoName5MenuItem.Text = "Redo5";
            this.redoName5MenuItem.Click += new System.EventHandler(this.redoName5MenuItem_Click);
            // 
            // editRedoButton
            // 
            this.editRedoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editRedoButton.DropDown = this.redoNames;
            this.editRedoButton.Enabled = false;
            this.editRedoButton.ImageIndex = 10;
            this.resourceLookup.SetLookup(this.editRedoButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_REDO", "Redo", "Redo"));
            this.editRedoButton.Name = "editRedoButton";
            this.editRedoButton.Size = new System.Drawing.Size(32, 22);
            this.editRedoButton.Tag = "EditRedo";
            this.editRedoButton.ToolTipText = "Redo";
            this.editRedoButton.DropDownOpening += new System.EventHandler(this.redoNamesMenuItem_Popup);
            // 
            // undoNames
            // 
            this.undoNames.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoName1MenuItem,
            this.undoName2MenuItem,
            this.undoName3MenuItem,
            this.undoName4MenuItem,
            this.undoName5MenuItem});
            this.undoNames.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.undoNames.Name = "undoNames";
            this.undoNames.OwnerItem = this.editUndoButton;
            this.undoNames.Size = new System.Drawing.Size(48, 109);
            this.undoNames.Click += new System.EventHandler(this.undoNamesMenuItem_Popup);
            // 
            // undoName1MenuItem
            // 
            this.undoName1MenuItem.Name = "undoName1MenuItem";
            this.undoName1MenuItem.Size = new System.Drawing.Size(46, 21);
            this.undoName1MenuItem.Tag = 0;
            this.undoName1MenuItem.Text = "Undo1";
            this.undoName1MenuItem.Click += new System.EventHandler(this.undoName1MenuItem_Click);
            // 
            // undoName2MenuItem
            // 
            this.undoName2MenuItem.Name = "undoName2MenuItem";
            this.undoName2MenuItem.Size = new System.Drawing.Size(46, 21);
            this.undoName2MenuItem.Tag = 1;
            this.undoName2MenuItem.Text = "Undo2";
            this.undoName2MenuItem.Click += new System.EventHandler(this.undoName2MenuItem_Click);
            // 
            // undoName3MenuItem
            // 
            this.undoName3MenuItem.Name = "undoName3MenuItem";
            this.undoName3MenuItem.Size = new System.Drawing.Size(46, 21);
            this.undoName3MenuItem.Tag = 2;
            this.undoName3MenuItem.Text = "Undo3";
            this.undoName3MenuItem.Click += new System.EventHandler(this.undoName3MenuItem_Click);
            // 
            // undoName4MenuItem
            // 
            this.undoName4MenuItem.Name = "undoName4MenuItem";
            this.undoName4MenuItem.Size = new System.Drawing.Size(46, 21);
            this.undoName4MenuItem.Tag = 3;
            this.undoName4MenuItem.Text = "Undo4";
            this.undoName4MenuItem.Click += new System.EventHandler(this.undoName4MenuItem_Click);
            // 
            // undoName5MenuItem
            // 
            this.undoName5MenuItem.Name = "undoName5MenuItem";
            this.undoName5MenuItem.Size = new System.Drawing.Size(46, 21);
            this.undoName5MenuItem.Tag = 4;
            this.undoName5MenuItem.Text = "Undo5";
            this.undoName5MenuItem.Click += new System.EventHandler(this.undoName5MenuItem_Click);
            // 
            // editUndoButton
            // 
            this.editUndoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editUndoButton.DropDown = this.undoNames;
            this.editUndoButton.Enabled = false;
            this.editUndoButton.ImageIndex = 9;
            this.resourceLookup.SetLookup(this.editUndoButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_UNDO", "Undo", "Undo"));
            this.editUndoButton.Name = "editUndoButton";
            this.editUndoButton.Size = new System.Drawing.Size(32, 22);
            this.editUndoButton.Tag = "EditUndo";
            this.editUndoButton.ToolTipText = "Undo";
            this.editUndoButton.DropDownOpening += new System.EventHandler(this.undoNamesMenuItem_Popup);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            this.imageList.Images.SetKeyName(4, "");
            this.imageList.Images.SetKeyName(5, "");
            this.imageList.Images.SetKeyName(6, "");
            this.imageList.Images.SetKeyName(7, "");
            this.imageList.Images.SetKeyName(8, "");
            this.imageList.Images.SetKeyName(9, "");
            this.imageList.Images.SetKeyName(10, "");
            this.imageList.Images.SetKeyName(11, "");
            this.imageList.Images.SetKeyName(12, "");
            this.imageList.Images.SetKeyName(13, "");
            this.imageList.Images.SetKeyName(14, "");
            this.imageList.Images.SetKeyName(15, "");
            this.imageList.Images.SetKeyName(16, "");
            this.imageList.Images.SetKeyName(17, "");
            this.imageList.Images.SetKeyName(18, "");
            this.imageList.Images.SetKeyName(19, "");
            this.imageList.Images.SetKeyName(20, "");
            this.imageList.Images.SetKeyName(21, "");
            this.imageList.Images.SetKeyName(22, "");
            this.imageList.Images.SetKeyName(23, "");
            this.imageList.Images.SetKeyName(24, "");
            this.imageList.Images.SetKeyName(25, "");
            this.imageList.Images.SetKeyName(26, "");
            this.imageList.Images.SetKeyName(27, "");
            this.imageList.Images.SetKeyName(28, "");
            this.imageList.Images.SetKeyName(29, "");
            this.imageList.Images.SetKeyName(30, "");
            // 
            // uiToolBarDockAreaPanel
            // 
            this.uiToolBarDockAreaPanel.Controls.Add(this.controlContainerPanel);
            this.uiToolBarDockAreaPanel.Controls.Add(this.toolBar);
            this.uiToolBarDockAreaPanel.Controls.Add(this.statusBar);
            this.uiToolBarDockAreaPanel.Controls.Add(this.menuStrip2);
            this.uiToolBarDockAreaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiToolBarDockAreaPanel.Location = new System.Drawing.Point(0, 0);
            this.uiToolBarDockAreaPanel.Name = "uiToolBarDockAreaPanel";
            this.uiToolBarDockAreaPanel.Size = new System.Drawing.Size(800, 424);
            this.uiToolBarDockAreaPanel.TabIndex = 13;
            // 
            // controlContainerPanel
            // 
            this.controlContainerPanel.Controls.Add(this.editor);
            this.controlContainerPanel.Controls.Add(this.horizontalSplitter);
            this.controlContainerPanel.Controls.Add(this.bottomTabControl);
            this.controlContainerPanel.Controls.Add(this.typeMemberDropDownList);
            this.controlContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlContainerPanel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.controlContainerPanel.Location = new System.Drawing.Point(0, 49);
            this.controlContainerPanel.Name = "controlContainerPanel";
            this.controlContainerPanel.Size = new System.Drawing.Size(800, 353);
            this.controlContainerPanel.TabIndex = 10;
            // 
            // editor
            // 
            this.editor.AllowDrop = true;
            this.editor.BracketHighlightingVisible = true;
            this.editor.Dock = System.Windows.Forms.DockStyle.Fill;
            document1.Filename = "Sample.txt";
            document1.LineModificationMarkingEnabled = true;
            document1.Outlining.Mode = ActiproSoftware.SyntaxEditor.OutliningMode.Automatic;
            this.editor.Document = document1;
            this.editor.IndicatorMarginVisible = false;
            this.editor.IntelliPrompt.DropShadowEnabled = true;
            this.editor.IntelliPrompt.SmartTag.ClearOnDocumentModification = true;
            this.editor.IntelliPrompt.SmartTag.MultipleSmartTagsEnabled = false;
            this.editor.LineNumberMarginVisible = true;
            this.editor.Location = new System.Drawing.Point(0, 25);
            this.editor.Name = "editor";
            this.editor.Size = new System.Drawing.Size(800, 176);
            this.editor.TabIndex = 0;
            this.editor.DocumentSyntaxLanguageLoaded += new ActiproSoftware.SyntaxEditor.SyntaxLanguageEventHandler(this.editor_DocumentSyntaxLanguageLoaded);
            this.editor.DocumentUndoRedoStateChanged += new ActiproSoftware.SyntaxEditor.UndoRedoStateChangedEventHandler(this.editor_DocumentUndoRedoStateChanged);
            this.editor.IncrementalSearch += new ActiproSoftware.SyntaxEditor.IncrementalSearchEventHandler(this.editor_IncrementalSearch);
            this.editor.IntelliPromptSmartTagClicked += new System.EventHandler(this.editor_IntelliPromptSmartTagClicked);
            this.editor.KeyTyping += new ActiproSoftware.SyntaxEditor.KeyTypingEventHandler(this.editor_KeyTyping);
            this.editor.PasteDragDrop += new ActiproSoftware.SyntaxEditor.PasteDragDropEventHandler(this.editor_PasteDragDrop);
            this.editor.SelectionChanged += new ActiproSoftware.SyntaxEditor.SelectionEventHandler(this.editor_SelectionChanged);
            this.editor.SmartIndent += new ActiproSoftware.SyntaxEditor.SmartIndentEventHandler(this.editor_SmartIndent);
            this.editor.TriggerActivated += new ActiproSoftware.SyntaxEditor.TriggerEventHandler(this.editor_TriggerActivated);
            this.editor.UserInterfaceUpdate += new System.EventHandler(this.editor_UserInterfaceUpdate);
            this.editor.ViewMouseDown += new ActiproSoftware.SyntaxEditor.EditorViewMouseEventHandler(this.editor_ViewMouseDown);
            this.editor.ViewMouseHover += new ActiproSoftware.SyntaxEditor.EditorViewMouseEventHandler(this.editor_ViewMouseHover);
            // 
            // horizontalSplitter
            // 
            this.horizontalSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.horizontalSplitter.Location = new System.Drawing.Point(0, 201);
            this.horizontalSplitter.Name = "horizontalSplitter";
            this.horizontalSplitter.Size = new System.Drawing.Size(800, 6);
            this.horizontalSplitter.TabIndex = 10;
            this.horizontalSplitter.TabStop = false;
            // 
            // bottomTabControl
            // 
            this.bottomTabControl.Controls.Add(this.eventsTabPage);
            this.bottomTabControl.Controls.Add(this.errorsTabPage);
            this.bottomTabControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomTabControl.Location = new System.Drawing.Point(0, 207);
            this.bottomTabControl.Name = "bottomTabControl";
            this.bottomTabControl.SelectedIndex = 0;
            this.bottomTabControl.Size = new System.Drawing.Size(800, 146);
            this.bottomTabControl.TabIndex = 9;
            // 
            // eventsTabPage
            // 
            this.eventsTabPage.Controls.Add(this.consoleTextBox);
            this.eventsTabPage.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup.SetLookup(this.eventsTabPage, new FWBS.OMS.UI.Windows.ResourceLookupItem("OUTPUT", "Output", ""));
            this.eventsTabPage.Name = "eventsTabPage";
            this.eventsTabPage.Size = new System.Drawing.Size(792, 118);
            this.eventsTabPage.TabIndex = 0;
            this.eventsTabPage.Text = "Output";
            this.eventsTabPage.UseVisualStyleBackColor = true;
            // 
            // consoleTextBox
            // 
            this.consoleTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.consoleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleTextBox.Location = new System.Drawing.Point(0, 0);
            this.consoleTextBox.Multiline = true;
            this.consoleTextBox.Name = "consoleTextBox";
            this.consoleTextBox.ReadOnly = true;
            this.consoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.consoleTextBox.Size = new System.Drawing.Size(792, 118);
            this.consoleTextBox.TabIndex = 7;
            this.consoleTextBox.WordWrap = false;
            // 
            // errorsTabPage
            // 
            this.errorsTabPage.Controls.Add(this.errorsListView);
            this.errorsTabPage.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup.SetLookup(this.errorsTabPage, new FWBS.OMS.UI.Windows.ResourceLookupItem("ERRORS", "Errors", ""));
            this.errorsTabPage.Name = "errorsTabPage";
            this.errorsTabPage.Size = new System.Drawing.Size(792, 118);
            this.errorsTabPage.TabIndex = 1;
            this.errorsTabPage.Text = "Errors";
            this.errorsTabPage.UseVisualStyleBackColor = true;
            // 
            // errorsListView
            // 
            this.errorsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lineColumnHeader,
            this.columnColumnHeader,
            this.messageColumnHeader});
            this.errorsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorsListView.FullRowSelect = true;
            this.errorsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.errorsListView.Location = new System.Drawing.Point(0, 0);
            this.errorsListView.MultiSelect = false;
            this.errorsListView.Name = "errorsListView";
            this.errorsListView.Size = new System.Drawing.Size(792, 118);
            this.errorsListView.TabIndex = 0;
            this.errorsListView.UseCompatibleStateImageBehavior = false;
            this.errorsListView.View = System.Windows.Forms.View.Details;
            // 
            // lineColumnHeader
            // 
            this.lineColumnHeader.Text = "Line";
            this.lineColumnHeader.Width = 40;
            // 
            // columnColumnHeader
            // 
            this.columnColumnHeader.Text = "Col";
            this.columnColumnHeader.Width = 40;
            // 
            // messageColumnHeader
            // 
            this.messageColumnHeader.Text = "Message";
            this.messageColumnHeader.Width = 430;
            // 
            // typeMemberDropDownList
            // 
            this.typeMemberDropDownList.Dock = System.Windows.Forms.DockStyle.Top;
            this.typeMemberDropDownList.Location = new System.Drawing.Point(0, 0);
            this.typeMemberDropDownList.Name = "typeMemberDropDownList";
            this.typeMemberDropDownList.Size = new System.Drawing.Size(800, 25);
            this.typeMemberDropDownList.TabIndex = 11;
            this.typeMemberDropDownList.Tag = "";
            // 
            // toolBar
            // 
            this.toolBar.ImageList = this.imageList;
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolCloseScript,
            this.fileSaveButton,
            this.separator1,
            this.filePrintButton,
            this.filePrintPreviewButton,
            this.separator2,
            this.editCutButton,
            this.editCopyButton,
            this.editPasteButton,
            this.toolStripSeparator11,
            this.editUndoButton,
            this.editRedoButton,
            this.toolStripSeparator12,
            this.searchFindReplaceButton,
            this.toolStripSeparator13,
            this.editCommentSelectionButton,
            this.editUncommentSelectionButton,
            this.toolStripSeparator14,
            this.editListMembersButton,
            this.editCompleteWordButton});
            this.toolBar.Location = new System.Drawing.Point(0, 24);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(800, 25);
            this.toolBar.TabIndex = 9;
            this.toolBar.Text = "Close";
            this.toolBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolBar_ItemClicked);
            // 
            // fileSaveButton
            // 
            this.fileSaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fileSaveButton.ImageIndex = 2;
            this.resourceLookup.SetLookup(this.fileSaveButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_SAVE", "Save", "Save Script"));
            this.fileSaveButton.Name = "fileSaveButton";
            this.fileSaveButton.Size = new System.Drawing.Size(23, 22);
            this.fileSaveButton.Tag = "FileSave";
            this.fileSaveButton.ToolTipText = "Save Script";
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(6, 25);
            // 
            // filePrintButton
            // 
            this.filePrintButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.filePrintButton.ImageIndex = 3;
            this.resourceLookup.SetLookup(this.filePrintButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_PRINT", "Print", "Print"));
            this.filePrintButton.Name = "filePrintButton";
            this.filePrintButton.Size = new System.Drawing.Size(23, 22);
            this.filePrintButton.Tag = "FilePrint";
            this.filePrintButton.ToolTipText = "Print";
            // 
            // filePrintPreviewButton
            // 
            this.filePrintPreviewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.filePrintPreviewButton.ImageIndex = 4;
            this.resourceLookup.SetLookup(this.filePrintPreviewButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_PRINTPREVIEW", "Print Preview", "Print Preview"));
            this.filePrintPreviewButton.Name = "filePrintPreviewButton";
            this.filePrintPreviewButton.Size = new System.Drawing.Size(23, 22);
            this.filePrintPreviewButton.Tag = "FilePrintPreview";
            this.filePrintPreviewButton.ToolTipText = "Print Preview";
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(6, 25);
            // 
            // editCutButton
            // 
            this.editCutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editCutButton.ImageIndex = 5;
            this.resourceLookup.SetLookup(this.editCutButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_CUT", "Cut", "Cut"));
            this.editCutButton.Name = "editCutButton";
            this.editCutButton.Size = new System.Drawing.Size(23, 22);
            this.editCutButton.Tag = "EditCut";
            this.editCutButton.ToolTipText = "Cut";
            // 
            // editCopyButton
            // 
            this.editCopyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editCopyButton.ImageIndex = 6;
            this.resourceLookup.SetLookup(this.editCopyButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_COPY", "Copy", "Copy"));
            this.editCopyButton.Name = "editCopyButton";
            this.editCopyButton.Size = new System.Drawing.Size(23, 22);
            this.editCopyButton.Tag = "EditCopy";
            this.editCopyButton.ToolTipText = "Copy";
            // 
            // editPasteButton
            // 
            this.editPasteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editPasteButton.ImageIndex = 7;
            this.resourceLookup.SetLookup(this.editPasteButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_PASTE", "Paste", "Paste"));
            this.editPasteButton.Name = "editPasteButton";
            this.editPasteButton.Size = new System.Drawing.Size(23, 22);
            this.editPasteButton.Tag = "EditPaste";
            this.editPasteButton.ToolTipText = "Paste";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
            // 
            // searchFindReplaceButton
            // 
            this.searchFindReplaceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.searchFindReplaceButton.ImageIndex = 11;
            this.resourceLookup.SetLookup(this.searchFindReplaceButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_FINDREPLACE", "Find", "Find/Replace"));
            this.searchFindReplaceButton.Name = "searchFindReplaceButton";
            this.searchFindReplaceButton.Size = new System.Drawing.Size(23, 22);
            this.searchFindReplaceButton.Tag = "SearchFindReplace";
            this.searchFindReplaceButton.ToolTipText = "Find/Replace";
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
            // 
            // editCommentSelectionButton
            // 
            this.editCommentSelectionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editCommentSelectionButton.ImageIndex = 15;
            this.resourceLookup.SetLookup(this.editCommentSelectionButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_COMMENT", "Comment", "Comment Selection"));
            this.editCommentSelectionButton.Name = "editCommentSelectionButton";
            this.editCommentSelectionButton.Size = new System.Drawing.Size(23, 22);
            this.editCommentSelectionButton.Tag = "EditCommentSelection";
            this.editCommentSelectionButton.ToolTipText = "Comment Selection";
            // 
            // editUncommentSelectionButton
            // 
            this.editUncommentSelectionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editUncommentSelectionButton.ImageIndex = 16;
            this.resourceLookup.SetLookup(this.editUncommentSelectionButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_UNCOMMENT", "Uncomment", "Uncomment Selection"));
            this.editUncommentSelectionButton.Name = "editUncommentSelectionButton";
            this.editUncommentSelectionButton.Size = new System.Drawing.Size(23, 22);
            this.editUncommentSelectionButton.Tag = "EditUncommentSelection";
            this.editUncommentSelectionButton.ToolTipText = "Uncomment Selection";
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
            // 
            // editListMembersButton
            // 
            this.editListMembersButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editListMembersButton.ImageIndex = 27;
            this.resourceLookup.SetLookup(this.editListMembersButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_LISTMEMBERS", "List Members", "Display an Object Member List"));
            this.editListMembersButton.Name = "editListMembersButton";
            this.editListMembersButton.Size = new System.Drawing.Size(23, 22);
            this.editListMembersButton.Tag = "EditListMembers";
            this.editListMembersButton.ToolTipText = "Display an Object Member List";
            // 
            // editCompleteWordButton
            // 
            this.editCompleteWordButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editCompleteWordButton.ImageIndex = 30;
            this.resourceLookup.SetLookup(this.editCompleteWordButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("CS_WORDCOMPLETN", "Word Completion", "Display Word Completion"));
            this.editCompleteWordButton.Name = "editCompleteWordButton";
            this.editCompleteWordButton.Size = new System.Drawing.Size(23, 22);
            this.editCompleteWordButton.Tag = "EditCompleteWord";
            this.editCompleteWordButton.ToolTipText = "Display Word Completion";
            // 
            // statusBar
            // 
            this.statusBar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusBar.Location = new System.Drawing.Point(0, 402);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.positionPanel,
            this.languagePanel,
            this.statePanel,
            this.tokenPanel,
            this.hierarchyLevelPanel,
            this.messagePanel});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(800, 22);
            this.statusBar.TabIndex = 3;
            // 
            // positionPanel
            // 
            this.positionPanel.Name = "positionPanel";
            this.positionPanel.Width = 150;
            // 
            // languagePanel
            // 
            this.languagePanel.Name = "languagePanel";
            this.languagePanel.Width = 50;
            // 
            // statePanel
            // 
            this.statePanel.Name = "statePanel";
            this.statePanel.Width = 150;
            // 
            // tokenPanel
            // 
            this.tokenPanel.Name = "tokenPanel";
            this.tokenPanel.Width = 250;
            // 
            // hierarchyLevelPanel
            // 
            this.hierarchyLevelPanel.Name = "hierarchyLevelPanel";
            this.hierarchyLevelPanel.Width = 25;
            // 
            // messagePanel
            // 
            this.messagePanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.messagePanel.Name = "messagePanel";
            this.messagePanel.Text = "Ready";
            this.messagePanel.Width = 158;
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.editMenuItem,
            this.toolStripMenuItem1,
            this.toolsMenuItem,
            this.tsVersionControl,
            this.searchMenu});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(800, 24);
            this.menuStrip2.TabIndex = 11;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileNewMenuItem,
            this.mnuSaveCompile,
            this.CompileMenuItem,
            this.toolStripSeparator1,
            this.filePrintMenuItem,
            this.filePageSetupMenuItem,
            this.filePrintPreviewMenuItem,
            this.toolStripSeparator17,
            this.fileCloseScriptMenuItem});
            this.resourceLookup.SetLookup(this.fileMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuFile", "&File", ""));
            this.fileMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileMenuItem.Tag = 0;
            this.fileMenuItem.Text = "&File";
            // 
            // fileNewMenuItem
            // 
            this.resourceLookup.SetLookup(this.fileNewMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuNew", "&New", ""));
            this.fileNewMenuItem.Name = "fileNewMenuItem";
            this.fileNewMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.fileNewMenuItem.Size = new System.Drawing.Size(199, 22);
            this.fileNewMenuItem.Tag = 0;
            this.fileNewMenuItem.Text = "&New";
            this.fileNewMenuItem.Visible = false;
            // 
            // mnuSaveCompile
            // 
            this.resourceLookup.SetLookup(this.mnuSaveCompile, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuSaveCompile", "Save && &Compile", ""));
            this.mnuSaveCompile.Name = "mnuSaveCompile";
            this.mnuSaveCompile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuSaveCompile.Size = new System.Drawing.Size(199, 22);
            this.mnuSaveCompile.Tag = 1;
            this.mnuSaveCompile.Text = "Save && &Compile";
            this.mnuSaveCompile.Click += new System.EventHandler(this.mnuSaveCompile_Click);
            // 
            // CompileMenuItem
            // 
            this.resourceLookup.SetLookup(this.CompileMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuCompile", "Compile", ""));
            this.CompileMenuItem.Name = "CompileMenuItem";
            this.CompileMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.CompileMenuItem.Size = new System.Drawing.Size(199, 22);
            this.CompileMenuItem.Text = "Compile";
            this.CompileMenuItem.Click += new System.EventHandler(this.CompileMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(196, 6);
            // 
            // filePrintMenuItem
            // 
            this.resourceLookup.SetLookup(this.filePrintMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuPrint", "&Print...", ""));
            this.filePrintMenuItem.Name = "filePrintMenuItem";
            this.filePrintMenuItem.Size = new System.Drawing.Size(199, 22);
            this.filePrintMenuItem.Tag = 3;
            this.filePrintMenuItem.Text = "&Print...";
            this.filePrintMenuItem.Click += new System.EventHandler(this.filePrintMenuItem_Click);
            // 
            // filePageSetupMenuItem
            // 
            this.resourceLookup.SetLookup(this.filePageSetupMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuPageSetup", "Page Se&tup...", ""));
            this.filePageSetupMenuItem.Name = "filePageSetupMenuItem";
            this.filePageSetupMenuItem.Size = new System.Drawing.Size(199, 22);
            this.filePageSetupMenuItem.Tag = 4;
            this.filePageSetupMenuItem.Text = "Page Se&tup...";
            this.filePageSetupMenuItem.Click += new System.EventHandler(this.filePageSetupMenuItem_Click);
            // 
            // filePrintPreviewMenuItem
            // 
            this.resourceLookup.SetLookup(this.filePrintPreviewMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuPrintPreview", "Print Previe&w...", ""));
            this.filePrintPreviewMenuItem.Name = "filePrintPreviewMenuItem";
            this.filePrintPreviewMenuItem.Size = new System.Drawing.Size(199, 22);
            this.filePrintPreviewMenuItem.Tag = 5;
            this.filePrintPreviewMenuItem.Text = "Print Previe&w...";
            this.filePrintPreviewMenuItem.Click += new System.EventHandler(this.filePrintPreviewMenuItem_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(196, 6);
            // 
            // fileCloseScriptMenuItem
            // 
            this.resourceLookup.SetLookup(this.fileCloseScriptMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuCloseScript", "Close Script", ""));
            this.fileCloseScriptMenuItem.Name = "fileCloseScriptMenuItem";
            this.fileCloseScriptMenuItem.Size = new System.Drawing.Size(199, 22);
            this.fileCloseScriptMenuItem.Tag = 5;
            this.fileCloseScriptMenuItem.Text = "Close Script";
            this.fileCloseScriptMenuItem.Visible = false;
            this.fileCloseScriptMenuItem.Click += new System.EventHandler(this.fileCloseScriptMenuItem_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editUndoMenuItem,
            this.editRedoMenuItem,
            this.toolStripSeparator3,
            this.editCutMenuItem,
            this.editCopyMenuItem,
            this.editPasteMenuItem,
            this.toolStripSeparator5,
            this.toolsWorkflowEditMenuItem});
            this.resourceLookup.SetLookup(this.editMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuEdit", "&Edit", ""));
            this.editMenuItem.MergeIndex = 2;
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editMenuItem.Tag = 1;
            this.editMenuItem.Text = "&Edit";
            // 
            // editUndoMenuItem
            // 
            this.resourceLookup.SetLookup(this.editUndoMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuUndo", "&Undo", ""));
            this.editUndoMenuItem.Name = "editUndoMenuItem";
            this.editUndoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.editUndoMenuItem.Size = new System.Drawing.Size(174, 22);
            this.editUndoMenuItem.Tag = 0;
            this.editUndoMenuItem.Text = "&Undo";
            this.editUndoMenuItem.Click += new System.EventHandler(this.editUndoMenuItem_Click);
            // 
            // editRedoMenuItem
            // 
            this.resourceLookup.SetLookup(this.editRedoMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuRedo", "&Redo", ""));
            this.editRedoMenuItem.Name = "editRedoMenuItem";
            this.editRedoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
            this.editRedoMenuItem.Size = new System.Drawing.Size(174, 22);
            this.editRedoMenuItem.Tag = 1;
            this.editRedoMenuItem.Text = "&Redo";
            this.editRedoMenuItem.Click += new System.EventHandler(this.editRedoMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(171, 6);
            // 
            // editCutMenuItem
            // 
            this.resourceLookup.SetLookup(this.editCutMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuCut", "Cu&t", ""));
            this.editCutMenuItem.Name = "editCutMenuItem";
            this.editCutMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.editCutMenuItem.Size = new System.Drawing.Size(174, 22);
            this.editCutMenuItem.Tag = 3;
            this.editCutMenuItem.Text = "Cu&t";
            this.editCutMenuItem.Click += new System.EventHandler(this.editCutMenuItem_Click);
            // 
            // editCopyMenuItem
            // 
            this.resourceLookup.SetLookup(this.editCopyMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuCopy", "&Copy", ""));
            this.editCopyMenuItem.Name = "editCopyMenuItem";
            this.editCopyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.editCopyMenuItem.Size = new System.Drawing.Size(174, 22);
            this.editCopyMenuItem.Tag = 4;
            this.editCopyMenuItem.Text = "&Copy";
            this.editCopyMenuItem.Click += new System.EventHandler(this.editCopyMenuItem_Click);
            // 
            // editPasteMenuItem
            // 
            this.resourceLookup.SetLookup(this.editPasteMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuPaste", "&Paste", ""));
            this.editPasteMenuItem.Name = "editPasteMenuItem";
            this.editPasteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.editPasteMenuItem.Size = new System.Drawing.Size(174, 22);
            this.editPasteMenuItem.Tag = 5;
            this.editPasteMenuItem.Text = "&Paste";
            this.editPasteMenuItem.Click += new System.EventHandler(this.editPasteMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(171, 6);
            // 
            // toolsWorkflowEditMenuItem
            // 
            this.resourceLookup.SetLookup(this.toolsWorkflowEditMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuEditWorkflow", "Edit &Workflow", ""));
            this.toolsWorkflowEditMenuItem.Name = "toolsWorkflowEditMenuItem";
            this.toolsWorkflowEditMenuItem.Size = new System.Drawing.Size(174, 22);
            this.toolsWorkflowEditMenuItem.Text = "Edit &Workflow";
            this.toolsWorkflowEditMenuItem.Click += new System.EventHandler(this.toolsWorkflowEditMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsWorkflowInsertMenuItem,
            this.workflowActivityToolStripMenuItem,
            this.toolStripSeparator16,
            this.mnuInsertEC,
            this.mnuInsertIEC,
            this.mnuInsertC,
            this.toolStripSeparator4,
            this.editInsertCodeSnippetMenuItem});
            this.resourceLookup.SetLookup(this.toolStripMenuItem1, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuInsert", "&Insert", ""));
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(48, 20);
            this.toolStripMenuItem1.Text = "&Insert";
            // 
            // toolsWorkflowInsertMenuItem
            // 
            this.resourceLookup.SetLookup(this.toolsWorkflowInsertMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuWorkflow", "Workflow", ""));
            this.toolsWorkflowInsertMenuItem.Name = "toolsWorkflowInsertMenuItem";
            this.toolsWorkflowInsertMenuItem.Size = new System.Drawing.Size(221, 22);
            this.toolsWorkflowInsertMenuItem.Tag = 9;
            this.toolsWorkflowInsertMenuItem.Text = "Workflow";
            this.toolsWorkflowInsertMenuItem.Click += new System.EventHandler(this.toolsWorkflowInsertMenuItem_Click);
            // 
            // workflowActivityToolStripMenuItem
            // 
            this.resourceLookup.SetLookup(this.workflowActivityToolStripMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuWorkflowActv", "Workflow Activity ...", ""));
            this.workflowActivityToolStripMenuItem.Name = "workflowActivityToolStripMenuItem";
            this.workflowActivityToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.workflowActivityToolStripMenuItem.Text = "Workflow Activity ...";
            this.workflowActivityToolStripMenuItem.Click += new System.EventHandler(this.workflowActivityToolStripMenuItem_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(218, 6);
            // 
            // mnuInsertEC
            // 
            this.mnuInsertEC.Name = "mnuInsertEC";
            this.mnuInsertEC.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.mnuInsertEC.Size = new System.Drawing.Size(221, 22);
            this.mnuInsertEC.Tag = 0;
            this.mnuInsertEC.Text = "IBasicEnquiryControl";
            this.mnuInsertEC.Visible = false;
            this.mnuInsertEC.Click += new System.EventHandler(this.mnuInsertEC_Click);
            // 
            // mnuInsertIEC
            // 
            this.mnuInsertIEC.Name = "mnuInsertIEC";
            this.mnuInsertIEC.Size = new System.Drawing.Size(221, 22);
            this.mnuInsertIEC.Tag = 1;
            this.mnuInsertIEC.Text = "IListEnquiryControl";
            this.mnuInsertIEC.Visible = false;
            this.mnuInsertIEC.Click += new System.EventHandler(this.mnuInsertIEC_Click);
            // 
            // mnuInsertC
            // 
            this.mnuInsertC.Name = "mnuInsertC";
            this.mnuInsertC.Size = new System.Drawing.Size(221, 22);
            this.mnuInsertC.Tag = 2;
            this.mnuInsertC.Text = "GetControl";
            this.mnuInsertC.Visible = false;
            this.mnuInsertC.Click += new System.EventHandler(this.mnuInsertC_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(218, 6);
            this.toolStripSeparator4.Visible = false;
            // 
            // editInsertCodeSnippetMenuItem
            // 
            this.resourceLookup.SetLookup(this.editInsertCodeSnippetMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuInsSnippet", "Insert Code Snippet", ""));
            this.editInsertCodeSnippetMenuItem.Name = "editInsertCodeSnippetMenuItem";
            this.editInsertCodeSnippetMenuItem.Size = new System.Drawing.Size(221, 22);
            this.editInsertCodeSnippetMenuItem.Tag = 7;
            this.editInsertCodeSnippetMenuItem.Text = "Insert Code Snippet";
            this.editInsertCodeSnippetMenuItem.Visible = false;
            this.editInsertCodeSnippetMenuItem.Click += new System.EventHandler(this.editInsertCodeSnippetMenuItem_Click);
            // 
            // toolsMenuItem
            // 
            this.toolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsHighlightingStyleEditorMenuItem,
            this.toolStripSeparator10,
            this.toolsTextStatisticsMenuItem,
            this.mnuSettings,
            this.toolStripSeparator9,
            this.mnuRevert,
            this.toolStripSeparator8,
            this.mnuCompileToFile,
            this.toolStripSeparator15,
            this.mnuReferences,
            this.toolStripSeparator2,
            this.regenerateIntellisenseToolStripMenuItem});
            this.resourceLookup.SetLookup(this.toolsMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuTools", "&Tools", ""));
            this.toolsMenuItem.MergeIndex = 5;
            this.toolsMenuItem.Name = "toolsMenuItem";
            this.toolsMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsMenuItem.Tag = 4;
            this.toolsMenuItem.Text = "&Tools";
            // 
            // toolsHighlightingStyleEditorMenuItem
            // 
            this.resourceLookup.SetLookup(this.toolsHighlightingStyleEditorMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuHighlighting", "&Highlighting Style Editor...", ""));
            this.toolsHighlightingStyleEditorMenuItem.Name = "toolsHighlightingStyleEditorMenuItem";
            this.toolsHighlightingStyleEditorMenuItem.Size = new System.Drawing.Size(235, 22);
            this.toolsHighlightingStyleEditorMenuItem.Tag = 0;
            this.toolsHighlightingStyleEditorMenuItem.Text = "&Highlighting Style Editor...";
            this.toolsHighlightingStyleEditorMenuItem.Click += new System.EventHandler(this.toolsHighlightingStyleEditorMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(232, 6);
            // 
            // toolsTextStatisticsMenuItem
            // 
            this.resourceLookup.SetLookup(this.toolsTextStatisticsMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuTextStats", "Text Statistics...", ""));
            this.toolsTextStatisticsMenuItem.Name = "toolsTextStatisticsMenuItem";
            this.toolsTextStatisticsMenuItem.Size = new System.Drawing.Size(235, 22);
            this.toolsTextStatisticsMenuItem.Tag = 2;
            this.toolsTextStatisticsMenuItem.Text = "Text Statistics...";
            this.toolsTextStatisticsMenuItem.Click += new System.EventHandler(this.toolsTextStatisticsMenuItem_Click);
            // 
            // mnuSettings
            // 
            this.mnuSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowLineNumbers,
            this.languageMenuItem});
            this.resourceLookup.SetLookup(this.mnuSettings, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuSettings", "&Settings", ""));
            this.mnuSettings.Name = "mnuSettings";
            this.mnuSettings.Size = new System.Drawing.Size(235, 22);
            this.mnuSettings.Tag = 3;
            this.mnuSettings.Text = "&Settings";
            // 
            // mnuShowLineNumbers
            // 
            this.mnuShowLineNumbers.Checked = true;
            this.mnuShowLineNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resourceLookup.SetLookup(this.mnuShowLineNumbers, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuShowLineNumb", "Show Line Numbers", ""));
            this.mnuShowLineNumbers.Name = "mnuShowLineNumbers";
            this.mnuShowLineNumbers.Size = new System.Drawing.Size(180, 22);
            this.mnuShowLineNumbers.Tag = 0;
            this.mnuShowLineNumbers.Text = "Show Line Numbers";
            this.mnuShowLineNumbers.Click += new System.EventHandler(this.mnuShowLineNumbers_Click);
            // 
            // languageMenuItem
            // 
            this.languageMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.languageCSharpMenuItem,
            this.languageVBDotNetMenuItem,
            this.languageNotSpecifiedMenuItem});
            this.resourceLookup.SetLookup(this.languageMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuDefaultLang", "&Default Language", ""));
            this.languageMenuItem.MergeIndex = 3;
            this.languageMenuItem.Name = "languageMenuItem";
            this.languageMenuItem.Size = new System.Drawing.Size(180, 22);
            this.languageMenuItem.Tag = 2;
            this.languageMenuItem.Text = "&Default Language";
            // 
            // languageCSharpMenuItem
            // 
            this.languageCSharpMenuItem.Name = "languageCSharpMenuItem";
            this.languageCSharpMenuItem.Size = new System.Drawing.Size(145, 22);
            this.languageCSharpMenuItem.Tag = 0;
            this.languageCSharpMenuItem.Text = "&C#";
            this.languageCSharpMenuItem.Click += new System.EventHandler(this.languageCSharpMenuItem_Click);
            // 
            // languageVBDotNetMenuItem
            // 
            this.languageVBDotNetMenuItem.Name = "languageVBDotNetMenuItem";
            this.languageVBDotNetMenuItem.Size = new System.Drawing.Size(145, 22);
            this.languageVBDotNetMenuItem.Tag = 1;
            this.languageVBDotNetMenuItem.Text = "&VB.NET";
            this.languageVBDotNetMenuItem.Click += new System.EventHandler(this.languageVBDotNetMenuItem_Click);
            // 
            // languageNotSpecifiedMenuItem
            // 
            this.resourceLookup.SetLookup(this.languageNotSpecifiedMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuNotSpecified", "Not Specified", ""));
            this.languageNotSpecifiedMenuItem.Name = "languageNotSpecifiedMenuItem";
            this.languageNotSpecifiedMenuItem.Size = new System.Drawing.Size(145, 22);
            this.languageNotSpecifiedMenuItem.Text = "Not Specified";
            this.languageNotSpecifiedMenuItem.Click += new System.EventHandler(this.languageNotSpecifiedMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(232, 6);
            // 
            // mnuRevert
            // 
            this.resourceLookup.SetLookup(this.mnuRevert, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuRevertScript", "Revert to Old Script System", ""));
            this.mnuRevert.Name = "mnuRevert";
            this.mnuRevert.Size = new System.Drawing.Size(235, 22);
            this.mnuRevert.Tag = 5;
            this.mnuRevert.Text = "Revert to Old Script System";
            this.mnuRevert.Visible = false;
            this.mnuRevert.Click += new System.EventHandler(this.mnuRevert_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(232, 6);
            this.toolStripSeparator8.Visible = false;
            // 
            // mnuCompileToFile
            // 
            this.resourceLookup.SetLookup(this.mnuCompileToFile, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuCompileFile", "Compile To File", ""));
            this.mnuCompileToFile.Name = "mnuCompileToFile";
            this.mnuCompileToFile.Size = new System.Drawing.Size(235, 22);
            this.mnuCompileToFile.Tag = 7;
            this.mnuCompileToFile.Text = "Compile To File";
            this.mnuCompileToFile.Click += new System.EventHandler(this.mnuCompileToFile_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(232, 6);
            // 
            // mnuReferences
            // 
            this.resourceLookup.SetLookup(this.mnuReferences, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuReferences", "References...", ""));
            this.mnuReferences.Name = "mnuReferences";
            this.mnuReferences.Size = new System.Drawing.Size(235, 22);
            this.mnuReferences.Tag = 3;
            this.mnuReferences.Text = "References...";
            this.mnuReferences.Click += new System.EventHandler(this.mnuReferences_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(232, 6);
            // 
            // regenerateIntellisenseToolStripMenuItem
            // 
            this.resourceLookup.SetLookup(this.regenerateIntellisenseToolStripMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuIntellisense", "Regenerate Intellisense", ""));
            this.regenerateIntellisenseToolStripMenuItem.Name = "regenerateIntellisenseToolStripMenuItem";
            this.regenerateIntellisenseToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.regenerateIntellisenseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.regenerateIntellisenseToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.regenerateIntellisenseToolStripMenuItem.Text = "Regenerate Intellisense";
            this.regenerateIntellisenseToolStripMenuItem.Click += new System.EventHandler(this.regenerateIntellisenseToolStripMenuItem_Click);
            // 
            // tsVersionControl
            // 
            this.tsVersionControl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCheckin,
            this.mnuCompare});
            this.resourceLookup.SetLookup(this.tsVersionControl, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuVersioning", "&Version Control", ""));
            this.tsVersionControl.Name = "tsVersionControl";
            this.tsVersionControl.Size = new System.Drawing.Size(100, 20);
            this.tsVersionControl.Text = "&Version Control";
            // 
            // mnuCheckin
            // 
            this.resourceLookup.SetLookup(this.mnuCheckin, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuCheckin", "&Check in", ""));
            this.mnuCheckin.Name = "mnuCheckin";
            this.mnuCheckin.Size = new System.Drawing.Size(194, 22);
            this.mnuCheckin.Text = "&Check in";
            this.mnuCheckin.Click += new System.EventHandler(this.mnuCheckin_Click);
            // 
            // mnuCompare
            // 
            this.resourceLookup.SetLookup(this.mnuCompare, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuCompare", "Version &Administration", ""));
            this.mnuCompare.Name = "mnuCompare";
            this.mnuCompare.Size = new System.Drawing.Size(194, 22);
            this.mnuCompare.Text = "Version &Administration";
            this.mnuCompare.Click += new System.EventHandler(this.mnuCompare_Click);
            // 
            // searchMenu
            // 
            this.searchMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchFindMenuItem,
            this.toolStripSeparator6,
            this.searchIncrementalSearchMenuItem,
            this.searchReverseIncrementalSearchMenuItem,
            this.toolStripSeparator7,
            this.searchGoToLineMenuItem});
            this.resourceLookup.SetLookup(this.searchMenu, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuSearch", "&Search", ""));
            this.searchMenu.MergeIndex = 4;
            this.searchMenu.Name = "searchMenu";
            this.searchMenu.Size = new System.Drawing.Size(54, 20);
            this.searchMenu.Tag = 3;
            this.searchMenu.Text = "&Search";
            // 
            // searchFindMenuItem
            // 
            this.resourceLookup.SetLookup(this.searchFindMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuFindReplace", "&Find/Replace...", ""));
            this.searchFindMenuItem.Name = "searchFindMenuItem";
            this.searchFindMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.searchFindMenuItem.Size = new System.Drawing.Size(293, 22);
            this.searchFindMenuItem.Tag = 0;
            this.searchFindMenuItem.Text = "&Find/Replace...";
            this.searchFindMenuItem.Click += new System.EventHandler(this.searchFindMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(290, 6);
            // 
            // searchIncrementalSearchMenuItem
            // 
            this.resourceLookup.SetLookup(this.searchIncrementalSearchMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuIncSearch", "&Incremental Search", ""));
            this.searchIncrementalSearchMenuItem.Name = "searchIncrementalSearchMenuItem";
            this.searchIncrementalSearchMenuItem.Size = new System.Drawing.Size(293, 22);
            this.searchIncrementalSearchMenuItem.Tag = 2;
            this.searchIncrementalSearchMenuItem.Text = "&Incremental Search";
            this.searchIncrementalSearchMenuItem.Click += new System.EventHandler(this.searchIncrementalSearchMenuItem_Click);
            // 
            // searchReverseIncrementalSearchMenuItem
            // 
            this.resourceLookup.SetLookup(this.searchReverseIncrementalSearchMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuRevIncSearch", "&Reverse Incremental Search", ""));
            this.searchReverseIncrementalSearchMenuItem.Name = "searchReverseIncrementalSearchMenuItem";
            this.searchReverseIncrementalSearchMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.searchReverseIncrementalSearchMenuItem.Size = new System.Drawing.Size(293, 22);
            this.searchReverseIncrementalSearchMenuItem.Tag = 3;
            this.searchReverseIncrementalSearchMenuItem.Text = "&Reverse Incremental Search";
            this.searchReverseIncrementalSearchMenuItem.Click += new System.EventHandler(this.searchReverseIncrementalSearchMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(290, 6);
            // 
            // searchGoToLineMenuItem
            // 
            this.resourceLookup.SetLookup(this.searchGoToLineMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuGoToLine", "&Go To Line...", ""));
            this.searchGoToLineMenuItem.Name = "searchGoToLineMenuItem";
            this.searchGoToLineMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.searchGoToLineMenuItem.Size = new System.Drawing.Size(293, 22);
            this.searchGoToLineMenuItem.Tag = 5;
            this.searchGoToLineMenuItem.Text = "&Go To Line...";
            this.searchGoToLineMenuItem.Click += new System.EventHandler(this.searchGoToLineMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Title = "Select File ...";
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.UniqueID = "";
            this.ucFormStorage1.Version = ((long)(0));
            // 
            // CodeSurfaceV2
            // 
            this.Controls.Add(this.uiToolBarDockAreaPanel);
            this.Name = "CodeSurfaceV2";
            this.Size = new System.Drawing.Size(800, 424);
            this.redoNames.ResumeLayout(false);
            this.undoNames.ResumeLayout(false);
            this.uiToolBarDockAreaPanel.ResumeLayout(false);
            this.uiToolBarDockAreaPanel.PerformLayout();
            this.controlContainerPanel.ResumeLayout(false);
            this.bottomTabControl.ResumeLayout(false);
            this.eventsTabPage.ResumeLayout(false);
            this.eventsTabPage.PerformLayout();
            this.errorsTabPage.ResumeLayout(false);
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.languagePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tokenPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hierarchyLevelPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.messagePanel)).EndInit();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);

		}




		#endregion

		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
		private ToolStripDropDown redoNames;
		private ToolStripMenuItem redoName1MenuItem;
		private ToolStripMenuItem redoName2MenuItem;
		private ToolStripMenuItem redoName3MenuItem;
		private ToolStripMenuItem redoName4MenuItem;
		private ToolStripMenuItem redoName5MenuItem;
		private SyntaxEditor editor;
		private Panel controlContainerPanel;
		private Splitter horizontalSplitter;
		private TabPage eventsTabPage;
		private TextBox consoleTextBox;
		private TabPage errorsTabPage;
		private ColumnHeader lineColumnHeader;
		private ColumnHeader columnColumnHeader;
		private ColumnHeader messageColumnHeader;
		private ActiproSoftware.SyntaxEditor.Addons.DotNet.Dom.TypeMemberDropDownList typeMemberDropDownList;
		private Panel uiToolBarDockAreaPanel;
		private ToolStrip toolBar;
		private ToolStripButton fileSaveButton;
		private ToolStripSeparator separator1;
		private ToolStripButton filePrintButton;
        private ToolStripButton filePrintPreviewButton;
		private ToolStripSeparator separator2;

		private ToolStripButton editCutButton;
		private ToolStripButton editCopyButton;
		private ToolStripButton editPasteButton;
		private ToolStripSplitButton editUndoButton;
		private ToolStripDropDown undoNames;
		private ToolStripMenuItem undoName1MenuItem;
		private ToolStripMenuItem undoName2MenuItem;
		private ToolStripMenuItem undoName3MenuItem;
		private ToolStripMenuItem undoName4MenuItem;
		private ToolStripMenuItem undoName5MenuItem;
		private ToolStripSplitButton editRedoButton;
		private ToolStripButton searchFindReplaceButton;
		private ToolStripButton editCommentSelectionButton;
		private ToolStripButton editUncommentSelectionButton;
		private ToolStripButton editListMembersButton;
		private ToolStripButton editCompleteWordButton;
		private ImageList imageList;
        private FWBS.OMS.UI.StatusBar statusBar;
		private StatusBarPanel positionPanel;
		private StatusBarPanel languagePanel;
		private StatusBarPanel statePanel;
		private StatusBarPanel tokenPanel;
		private StatusBarPanel hierarchyLevelPanel;
		private StatusBarPanel messagePanel;
		private OpenFileDialog openFileDialog;
		protected FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;
		private CodeSurfaceMenuItem fileNewMenuItem;
		private CodeSurfaceMenuItem mnuSaveCompile;
		private CodeSurfaceMenuItem filePrintMenuItem;
		private CodeSurfaceMenuItem filePageSetupMenuItem;
        private CodeSurfaceMenuItem filePrintPreviewMenuItem;
        private CodeSurfaceMenuItem fileCloseScriptMenuItem; 
        private CodeSurfaceMenuItem fileMenuItem;
		private CodeSurfaceMenuItem editUndoMenuItem;
		private CodeSurfaceMenuItem editRedoMenuItem;
		private CodeSurfaceMenuItem editCutMenuItem;
		private CodeSurfaceMenuItem editCopyMenuItem;
        private CodeSurfaceMenuItem editPasteMenuItem;
        private CodeSurfaceMenuItem editMenuItem;
		private CodeSurfaceMenuItem searchFindMenuItem;
		private CodeSurfaceMenuItem searchIncrementalSearchMenuItem;
		private CodeSurfaceMenuItem searchReverseIncrementalSearchMenuItem;
		private CodeSurfaceMenuItem searchGoToLineMenuItem;
		private CodeSurfaceMenuItem searchMenu;
		private CodeSurfaceMenuItem toolsHighlightingStyleEditorMenuItem;
		private CodeSurfaceMenuItem toolsTextStatisticsMenuItem;
		private CodeSurfaceMenuItem mnuShowLineNumbers;
		private CodeSurfaceMenuItem mnuSettings;
		private CodeSurfaceMenuItem mnuRevert;
		internal CodeSurfaceMenuItem mnuCompileToFile;
		private CodeSurfaceMenuItem toolsMenuItem;
		private CodeSurfaceMenuStrip menuStrip2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator3;
		private ToolStripSeparator toolStripSeparator6;
		private ToolStripSeparator toolStripSeparator7;
		private ToolStripSeparator toolStripSeparator10;
		private ToolStripSeparator toolStripSeparator9;
		private ToolStripSeparator toolStripSeparator8;
		private ToolStripSeparator toolStripSeparator11;
		private ToolStripSeparator toolStripSeparator12;
		private ToolStripSeparator toolStripSeparator13;
		private ToolStripSeparator toolStripSeparator14;
        private ToolStripSeparator toolStripSeparator15;
        private ToolStripSeparator toolStripSeparator5;
        private CodeSurfaceMenuItem toolsWorkflowEditMenuItem;
        private CodeSurfaceMenuItem toolStripMenuItem1;
        private CodeSurfaceMenuItem toolsWorkflowInsertMenuItem;
        private CodeSurfaceMenuItem mnuInsertEC;
        private CodeSurfaceMenuItem mnuInsertIEC;
        private CodeSurfaceMenuItem mnuInsertC;
        private CodeSurfaceMenuItem editInsertCodeSnippetMenuItem;
        private CodeSurfaceMenuItem mnuReferences;
        private ToolStripSeparator toolStripSeparator4;
        private CodeSurfaceMenuItem languageMenuItem;
        private CodeSurfaceMenuItem languageCSharpMenuItem;
        private CodeSurfaceMenuItem languageVBDotNetMenuItem;
        private CodeSurfaceMenuItem languageNotSpecifiedMenuItem;
		private CodeSurfaceMenuItem workflowActivityToolStripMenuItem;
        private CodeSurfaceMenuItem regenerateIntellisenseToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private CodeSurfaceMenuItem CompileMenuItem;
		private ToolStripSeparator toolStripSeparator16;
		private UI.TabControl bottomTabControl;
		private UI.ListView errorsListView;
        private CodeSurfaceMenuItem tsVersionControl;
        private CodeSurfaceMenuItem mnuCheckin;
        private CodeSurfaceMenuItem mnuCompare;
        private ToolStripSeparator toolStripSeparator17;
        private ToolStripButton toolCloseScript;
	}
}
