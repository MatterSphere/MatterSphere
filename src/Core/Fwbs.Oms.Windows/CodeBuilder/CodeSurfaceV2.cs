using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ActiproSoftware.ComponentModel;
using ActiproSoftware.SyntaxEditor;
using ActiproSoftware.SyntaxEditor.Addons.CSharp;
using ActiproSoftware.SyntaxEditor.Addons.Dynamic;
using ActiproSoftware.SyntaxEditor.Addons.VB;
using ActiproSoftware.WinUICore;
using FWBS.OMS.Data;
using FWBS.OMS.UI;
using FWBS.OMS.UI.Windows;



namespace FWBS.OMS.Design.CodeBuilder
{
    internal partial class CodeSurfaceV2 : UserControl, ICodeSurface, ICodeSurfaceControls
	{

        public event EventHandler CloseScriptWindowMenuItem;

        protected void OnCloseScriptWindowMenuItem()
        {
            if (CloseScriptWindowMenuItem != null)
                CloseScriptWindowMenuItem(this, EventArgs.Empty);
        }

        public bool IsCloseScriptWindowMenuItemVisible
        {
            get
            {
                return fileCloseScriptMenuItem.Visible;
            }
            set
            {
                fileCloseScriptMenuItem.Visible = value;
                toolStripSeparator17.Visible = value;
                toolCloseScript.Visible = value;
            }
        }

		#region Fields

		private FWBS.OMS.Script.ScriptType _currentscripttype;
		private FWBS.OMS.Script.ScriptGen _currentscriptobject;
        private readonly string positionPanelTextFmt = "Ln {0}\tCol {1}\tCh {2}";
        private readonly string statePanelTextFmt = "State ID: {0}";

		private List<string> _objects = new List<string>();

        FWBS.OMS.UI.Windows.ScriptVersionDataArchiver scriptVersionDataArchiver;

        VersionComparisonSelector vcs;

		/// <summary>
		/// Indicates a type of application action.
		/// </summary>
		/// <remarks>
		/// This is simply used to have all menu items and toolbars call the same centralized procedure.
		/// </remarks>
		private enum AppAction
		{
			FileNew,
			FileOpen,
			FileSave,
			FileCompile,
			FilePrint,
			FilePageSetup,
			FilePrintPreview,
			EditCut,
			EditCopy,
			EditPaste,
			EditUndo,
			EditRedo,
			EditCommentSelection,
			EditUncommentSelection,
			EditMakeLowercase,
			EditMakeUppercase,
			EditListMembers,
			EditParameterInfo,
			EditQuickInfo,
			EditCompleteWord,
			SearchFindReplace,
			References,
			Revert,
			ToolsHighlightingStyleEditor,
			ToolsTextStatistics,
			ToolsSettingsShowLineNumbers,
			InsertIBasicEnquiryControl,
			InsertIListEnquiryControl,
			InsertControl,
			InsertWorkflow,
			EditWorkflow,
			InsertWorkflowActivity,
			ToolsRegenerateIntellisense,
            VersionControlCheckin,
            VersionControlCompare,
            CloseScript
		}

		/// <summary>
		/// Indicates the types of languages that are available in this sample application.
		/// </summary>
		private enum Language
		{
			None,
			CSharp,
			VBDotNet
		}

		private Language CurrentLanguage;

		/// <summary>
		/// Overide to Foce is Dirty on the Form
		/// </summary>
		private bool _isdirty = false;

		/// <summary>
		/// The find/replace form.
		/// </summary>
		private FindReplaceForm findReplaceForm;

		/// <summary>
		/// Find/replace options.
		/// </summary>
		private FindReplaceOptions findReplaceOptions = new FindReplaceOptions();

		/// <summary>
		/// Gets whether the last language that was loaded was plain text.
		/// </summary>
		private bool lastLanguageWasPlainText = false;

		/// <summary>
		/// Gets whether the last language used auto case correction.
		/// </summary>
		private bool lastLanguageUsedAutoCaseCorrection = false;

		/// <summary>
		/// Code snippet folder for C# snippets.
		/// </summary>
		private CodeSnippetFolder cSharpCodeSnippetFolder;

		/// <summary>
		/// Code snippet folder for VB snippets.
		/// </summary>
		private CodeSnippetFolder vbCodeSnippetFolder;

		/// <summary>
		/// A .NET project resolver.
		/// </summary>
		private ActiproSoftware.SyntaxEditor.Addons.DotNet.Dom.DotNetProjectResolver dotNetProjectResolver;


		/// <summary>
		/// Script Type
		/// </summary>
		public FWBS.OMS.Script.ScriptType ScriptType
		{
			get { return _currentscripttype; }
			set
			{
				if (value != _currentscripttype)
					_currentscripttype = value;
			}

		}
		#endregion

		public CodeSurfaceV2()
		{
			InitializeComponent();
			this.errorsListView.DoubleClick += new System.EventHandler(this.errorsListView_DoubleClick);
            lineColumnHeader.Text = ResourceLookup.GetLookupText("LINE", "Line", "");
            columnColumnHeader.Text = ResourceLookup.GetLookupText("COL", "Col", "");
            messageColumnHeader.Text = ResourceLookup.GetLookupText("MESSAGE", "Message", "");
            positionPanelTextFmt = ResourceLookup.GetLookupText("CS_POSITION_FMT", positionPanelTextFmt, "");
            statePanelTextFmt = ResourceLookup.GetLookupText("CS_STATE_FMT", statePanelTextFmt, "");
            this.SetStatusMessage(ResourceLookup.GetLookupText("READY", "Ready", ""));
		}

		#region IServiceProvider

		new public object GetService(Type serviceType)
		{
			if (serviceType == typeof(ICodeSurface) || serviceType == typeof(ICodeSurfaceControls))
				return this;
			return base.GetService(serviceType);
		}

		public T GetService<T>()
		{
			return (T)GetService(typeof(T));
		}

		#endregion

		#region ICodeSurface Members

		public bool IsInitialized
		{
			get { return isinits; }
		}

		private bool isinits;
		private Form _parentform;

		public void Init(Form parentForm)
		{
			isinits = true;
			uiToolBarDockAreaPanel.Visible = false;
			// Set the main UI thread name (for thread debugging)
			if (System.Threading.Thread.CurrentThread.Name == null)
				System.Threading.Thread.CurrentThread.Name = "User Interface";

			// Get the code snippets
			cSharpCodeSnippetFolder = new CodeSnippetFolder("C#", Global.GetAppDataPath() + @"\Snippets\C#");
			vbCodeSnippetFolder = new CodeSnippetFolder("VB", Global.GetAppDataPath() + @"\Snippets\VB");

			// Start the parser service (only call this once at the start of your application)
			SemanticParserService.Start();

			LoadSettings();

			_parentform = parentForm;
			Application.DoEvents();
		}

		private void MergeToolStrip()
		{
			if (_parentform != null && _parentform.MdiParent != null && _parentform.MdiParent.MainMenuStrip != null)
			{
				IMenuMerge menumerge = _parentform.MdiParent as IMenuMerge;
				if (menumerge != null)
				{
					_parentform.MainMenuStrip = this.menuStrip2;
					menumerge.MergeMenus(menuStrip2);
					this.menuStrip2.Visible = false;
				}
			}
		}

		private void UnMergeToolStrip()
		{
			if (_parentform != null && _parentform.MdiParent != null && _parentform.MdiParent.MainMenuStrip != null)
			{
				IMenuMerge menumerge = _parentform.MdiParent as IMenuMerge;
				if (menumerge != null)
				{
					menumerge.UnMergeMenus(menuStrip2);
					_parentform.MainMenuStrip = null;
					this.menuStrip2.Visible = true;
				}
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
                if (_currentscriptobject != null)
                {
                    _currentscriptobject.CompileStart -= new EventHandler(CurrentscriptObject_CompileStart);
                    _currentscriptobject.CompileFinished -= new EventHandler(CurrentscriptObject_CompileFinished);
                    _currentscriptobject.CompileError -= new EventHandler(CurrentscriptObject_CompileError);
                    _currentscriptobject.CompileOutput -= new MessageEventHandler(CurrentscriptObject_CompileOutput);
                }
				if (_parentform != null)
				{
					_parentform.MainMenuStrip = null;
					_parentform = null;
				}

				// Stop the parser service... note that you should only do this if you are closing your application down
				//   and are sure no other languages are still using the service... in the case of this sample project,
				//   each QuickStart Form is modal so we know another window doesn't have a language that is still accessing the service
				SemanticParserService.Stop();

				if (dotNetProjectResolver != null)
				{
					// Dispose the project resolver... this releases all its resources
					dotNetProjectResolver.Dispose();

					// Prune the cache to remove files that no longer apply... note that you should only do this if you are closing your application down                //   and are sure no other project resolvers are still using the cache... in the case of this sample project, 
					//   each QuickStart Form is modal so we know another window is still not accessing the cache
					dotNetProjectResolver.PruneCache();
				}

				if (components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}


		public new void Load(FWBS.OMS.Script.ScriptType scriptletType, FWBS.OMS.Script.ScriptGen scriptlet)
		{
			if (scriptletType == null)
				throw new ArgumentNullException("scriptletType");

			if (scriptlet == null)
				throw new ArgumentNullException("scriptlet");

			_currentscripttype = scriptletType;
			ScriptGen = scriptlet;


			if (scriptletType is FWBS.OMS.UI.Windows.Script.EnquiryFormScriptType)
			{
				mnuInsertC.Visible = true;
				mnuInsertEC.Visible = true;
				mnuInsertIEC.Visible = true;
			}

			uiToolBarDockAreaPanel.Visible = true;

            bool moreThanObjectOpen = (new LockState().CheckForOpenObjects(this.ScriptGen.Code, LockableObjects.Script)) ? false : true;
            ManageVersionControlOptions(moreThanObjectOpen);
		}

		public void Unload()
		{
			UnMergeToolStrip();
		}

		public bool SupportsLanguage(FWBS.OMS.Script.ScriptLanguage language)
		{
			return false;
		}

		public bool HasMethod(string method)
		{
			if (editor.Document == null)
				return false;
			
			return Array.IndexOf(GetMethods(), method) > -1;
		}

	   
		public void GenerateHandler(string name, GenerateHandlerInfo info)
		{
			if (HasMethod(name))
				GotoMethod(name);
			else
				InsertHandler(name, info);

			editor.Focus();
		}

		private void InsertHandler(string name, GenerateHandlerInfo info)
		{
			string code = "";

			if (String.IsNullOrWhiteSpace(info.DelegateType))
				code = _currentscriptobject.CreateOverrideMethod(name, info.Workflow);
			else
				code = _currentscriptobject.CreateEventHandler(name, info.DelegateType, info.Workflow);

			var offset = GetClassRegion();
			
			if (offset == null) 
				throw new OMSException2("CSV2GENHNDLCRNF", "Could not find class region to insert method. Please check the class name is identical to the script name '%1%'", null, true, ScriptGen.Code);

			InsertAndGotoMethod(name, code, offset);
		}

		private string InsertAndGotoMethod(string name, string code, ClassRegion offset)
		{
			code = String.Format("\t\t{0}\r\n", code.Replace("\r\n", "\r\n\t\t"));
			editor.Document.InsertText(DocumentModificationType.InsertCodeSnippetTemplate, offset.End.StartOffset, code);
			GotoMethod(name, offset);
			IsDirty = true;
			return code;
		}

		#region Class Region

		public class ClassRegion
		{
			public TextRange Start { get; set; }

			public TextRange End { get; set; }
		}

		private ClassRegion GetClassRegion()
		{

			if (editor.Document.SpanIndicatorLayers.Contains(SpanIndicatorLayer.ReadOnlyDisplayPriority))
			{
				var ro_layer = editor.Document.SpanIndicatorLayers[SpanIndicatorLayer.ReadOnlyDisplayPriority];

				if (ro_layer != null)
				{
					return new ClassRegion() { Start = ro_layer[0].TextRange, End = ro_layer[1].TextRange };
				}
			}

			switch(CurrentLanguage)
			{
				case Language.CSharp:
					return GetClassRegionCS();
				case  Language.VBDotNet:
					return GetClassRegionVB();
			}


			return null;
		}

		private ClassRegion GetClassRegionCS()
		{

			var ret = new ClassRegion();

			using (var strm = editor.Document.GetTextStream(0))
			{
				while (strm.GoToNextTokenWithID(CSharpTokenID.Class))
				{
					while (strm.GoToNextTokenWithID(CSharpTokenID.Identifier))
					{
						if (!strm.TokenText.Equals(ScriptGen.Code, StringComparison.OrdinalIgnoreCase))
							continue;

						int p = strm.DocumentLine.StartOffset;

						strm.GoToNextTokenWithID(CSharpTokenID.OpenCurlyBrace);

						ret.Start = new TextRange(p, strm.Token.EndOffset);

						var token = strm.Token;

						if (strm.GoToNextMatchingToken(token))
						{
							ret.End = new TextRange(strm.DocumentLine.StartOffset, strm.DocumentLine.EndOffset);
							return ret;
						}
					}
				}

			}

			return null;

		}

		private ClassRegion GetClassRegionVB()
		{

			var ret = new ClassRegion();

			using (var strm = editor.Document.GetTextStream(0))
			{
				while (strm.GoToNextTokenWithID(VBTokenID.Class)) 
				{
					while (strm.GoToNextTokenWithID(VBTokenID.Identifier)) 
					{
						if (!strm.TokenText.Equals(ScriptGen.Code, StringComparison.OrdinalIgnoreCase))
							continue;

						int p = strm.DocumentLine.StartOffset;

						strm.GoToNextTokenWithID(VBTokenID.LineTerminator);

						ret.Start = new TextRange(p, strm.Token.EndOffset);

						var token = strm.Token;

						int indent = 0;

						while (strm.GoToNextTokenWithID(new int[] { VBTokenID.Class, VBTokenID.End })) 
						{
							if (strm.Token.ID == VBTokenID.Class)
							{
								indent++;
							}
							else if (strm.Token.ID == VBTokenID.End)
							{
								strm.GoToNextNonWhitespaceOrCommentToken();
								if (strm.Token.ID == VBTokenID.Class)
								{
									indent--;
								}
							}
							if (indent < 0)
							{
								ret.End = new TextRange(strm.DocumentLine.StartOffset, strm.DocumentLine.EndOffset);
								return ret;
							}
						}
					}
				}

			}

			return null;

		}

		#endregion

		#region GotoMethod

		public void GotoMethod(string name)
		{
			GotoMethod(name, GetClassRegion());
		}

		private void GotoMethod(string name,ClassRegion region)
		{
			if (region == null)
				return;


			switch (CurrentLanguage)
			{
				case Language.CSharp:
					GotoMethodCS(name, region);
					break;
				case Language.VBDotNet:
					GotoMethodVB(name, region);
					break;
			}
		}

		private void GotoMethodCS(string name, ClassRegion region)
		{
			using (var strm = editor.Document.GetTextStream(region.Start.EndOffset))
			{
				foreach (var item in GetMethodsCS(strm))
				{
					if (!item.Equals(name))
						continue;

					editor.SelectedView.GoToLine(editor.Document.Lines.Count - 1);
					editor.SelectedView.GoToLine(strm.DocumentLineIndex + 1);
				}
			}
		}

		private bool IsDataTypeTokenCS(IToken token)
		{
			switch (token.ID)
			{
				case CSharpTokenID.Void:
				
				case CSharpTokenID.ULong:
				case CSharpTokenID.UInt:
				case CSharpTokenID.UShort:
				case CSharpTokenID.Byte:
				case CSharpTokenID.Bool:
				case CSharpTokenID.Long:
				case CSharpTokenID.Int:
				case CSharpTokenID.Short:
				case CSharpTokenID.SByte:
				case CSharpTokenID.String:
				case CSharpTokenID.Char:
				case CSharpTokenID.Identifier:
				case CSharpTokenID.Decimal:
				case CSharpTokenID.Double:
				case CSharpTokenID.Dynamic:
				case CSharpTokenID.Float:
				case CSharpTokenID.Object:
					return true;
			}

			return false;
		}

		private void GotoMethodVB(string name, ClassRegion region)
		{
			using (var strm = editor.Document.GetTextStream(region.Start.EndOffset))
			{
				foreach (var item in GetMethodsVB(strm))
				{
					 if (!item.Equals(name))
						continue;
				
					editor.SelectedView.GoToLine(editor.Document.Lines.Count - 1);
					editor.SelectedView.GoToLine(strm.DocumentLineIndex + 1);
				}

			}
		}

		#endregion

		#region GetMethods
		
		public string[] GetMethods()
		{
			var region = GetClassRegion();

			if (region != null)
			{
				using (var strm = editor.Document.GetTextStream(region.Start.EndOffset))
				{
					switch (CurrentLanguage)
					{
						case Language.CSharp:
							return GetMethodsCS(strm).ToArray();
						case Language.VBDotNet:
							return GetMethodsVB(strm).ToArray();
					}
				}
			}

			return new string[0];
		}

		private IEnumerable<string> GetMethodsCS(TextStream strm)
		{
			while (strm.GoToNextTokenWithID(CSharpTokenID.Identifier))
			{
				var name = strm.TokenText;

				strm.GoToNextNonWhitespaceOrCommentToken();
				if (strm.Token.ID != CSharpTokenID.OpenParenthesis)
				{
					continue;
				}

				strm.GoToPreviousNonWhitespaceOrCommentToken();
				strm.GoToPreviousNonWhitespaceOrCommentToken();

				if (!IsDataTypeTokenCS(strm.Token))
				{
					strm.GoToNextNonWhitespaceOrCommentToken();
					continue;
				}

				yield return name;

				strm.GoToNextNonWhitespaceOrCommentToken();
			}   
		}

		private IEnumerable<string> GetMethodsVB(TextStream strm)
		{
			while (strm.GoToNextTokenWithID(VBTokenID.Identifier))
			{
				var name = strm.TokenText;

				strm.GoToPreviousNonWhitespaceOrCommentToken();

				if (strm.Token.ID == VBTokenID.Sub || strm.Token.ID == VBTokenID.Function)
				{
					yield return name;
				}

				strm.GoToNextNonWhitespaceOrCommentToken();
			}
		}

		#endregion

		public void ShowCode()
		{
			FWBS.Common.Reg.ApplicationSetting compileRegKey = new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "CompileToFile");
			if (ScriptGen.AdvancedScript)
			{
				mnuCompileToFile.Checked = Convert.ToBoolean(compileRegKey.GetSetting(false));
			}
			else
			{
				mnuCompileToFile.Checked = Convert.ToBoolean(compileRegKey.GetSetting(false));
			}
		}

		#endregion

		#region Methods

		#endregion

		#region Menu / Toolbar Events
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// MENU/TOOLBAR EVENT HANDLERS
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		private void workflowActivityToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ExecuteAppAction(AppAction.InsertWorkflowActivity);
		}

		private void toolsWorkflowEditMenuItem_Click(object sender, EventArgs e)
		{
			// Execute the AppAction
			this.ExecuteAppAction(AppAction.EditWorkflow);
		}

		private void toolsWorkflowInsertMenuItem_Click(object sender, EventArgs e)
		{
			// Execute the AppAction
			this.ExecuteAppAction(AppAction.InsertWorkflow);
		}

		private void mnuRevert_Click(object sender, EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.Revert);
		}

		private void mnuReferences_Click(object sender, EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.References);
		}

		private void editCopyMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditCopy);
		}

		private void editCutMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditCut);
		}

		private void editInsertCodeSnippetMenuItem_Click(object sender, System.EventArgs e)
		{
			if (editor.SelectedView.GetCurrentLanguageForContext().IntelliPromptCodeSnippetsSupported)
			{
				if (editor.SelectedView.GetCurrentLanguageForContext().ShowIntelliPromptInsertSnippetPopup(editor, Session.CurrentSession.Resources.GetResource("INSRTSNPT", "Insert Snippet", "").Text + ":", CodeSnippetTypes.Expansion))
					return;
			}
			FWBS.OMS.UI.Windows.MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("NOSNIPPET", "There are no snippets currently loaded for the current language.", "").Text, Session.CurrentSession.Resources.GetResource("INSRTSNPT", "Insert Snippet", "").Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void mnuInsertEC_Click(object sender, EventArgs e)
		{
			ExecuteAppAction(AppAction.InsertIBasicEnquiryControl);
		}

		private void mnuInsertIEC_Click(object sender, EventArgs e)
		{
			ExecuteAppAction(AppAction.InsertIListEnquiryControl);
		}

		private void mnuInsertC_Click(object sender, EventArgs e)
		{
			ExecuteAppAction(AppAction.InsertControl);
		}

		private void editPasteMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditPaste);
		}

		private void editRedoMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditRedo);
		}

		private void editUndoMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.EditUndo);
		}

		private void fileExitMenuItem_Click(object sender, System.EventArgs e)
		{
			// Close the form
			//this.Close();
		}

		private void filePageSetupMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.FilePageSetup);
		}

		private void filePrintMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.FilePrint);
		}

		private void filePrintPreviewMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.FilePrintPreview);
		}

        private void fileCloseScriptMenuItem_Click(object sender, System.EventArgs e)
        {
            OnCloseScriptWindowMenuItem();
        }

		private void fileSaveMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.FileSave);
		}

		private void findReplaceForm_StatusChanged(object sender, FindReplaceStatusChangeEventArgs e)
		{
			switch (e.ChangeType)
			{
				case FindReplaceStatusChangeType.Find:
					this.SetStatusMessage(string.Format("{0}: \"{1}\"", ResourceLookup.GetLookupText("FIND", "Find", ""), e.Options.FindText));
					break;
				case FindReplaceStatusChangeType.PastDocumentEnd:
					this.SetStatusMessage(ResourceLookup.GetLookupText("PASTENDOFDOC", "Past the end of the document", ""));
					break;
				case FindReplaceStatusChangeType.Ready:
					this.SetStatusMessage(ResourceLookup.GetLookupText("READY", "Ready", ""));
					break;
				case FindReplaceStatusChangeType.Replace:
					this.SetStatusMessage(ResourceLookup.GetLookupText("REPLACE:WITH:", "Replace: \"%1%\", with: \"%2%\"", "", false, e.Options.FindText, e.Options.ReplaceText));
					break;
			}
		}

		private void redoNamesMenuItem_Popup(object sender, System.EventArgs e)
		{
			// Get the number of items on the redo stack
			int redoCount = editor.Document.UndoRedo.RedoStack.Count;

			if (redoCount == 0)
			{
				// There are no items on the redo stack
				redoName1MenuItem.Visible = true;
				redoName1MenuItem.Enabled = false;
				redoName1MenuItem.Text = string.Format("({0})", ResourceLookup.GetLookupText("NOACTNS", "No actions", ""));
				redoName2MenuItem.Visible = redoName3MenuItem.Visible = redoName4MenuItem.Visible = redoName5MenuItem.Visible = false;
			}
			else
			{
				// Make the menu items visible if there are enough items on the stack
				redoName1MenuItem.Visible = (redoCount >= 1);
				redoName1MenuItem.Enabled = true;
				redoName2MenuItem.Visible = (redoCount >= 2);
				redoName3MenuItem.Visible = (redoCount >= 3);
				redoName4MenuItem.Visible = (redoCount >= 4);
				redoName5MenuItem.Visible = (redoCount >= 5);

				// Update the menu item text
				if (redoCount >= 1) redoName1MenuItem.Text = editor.Document.UndoRedo.RedoStack.GetName(redoCount - 1);
				if (redoCount >= 2) redoName2MenuItem.Text = editor.Document.UndoRedo.RedoStack.GetName(redoCount - 2);
				if (redoCount >= 3) redoName3MenuItem.Text = editor.Document.UndoRedo.RedoStack.GetName(redoCount - 3);
				if (redoCount >= 4) redoName4MenuItem.Text = editor.Document.UndoRedo.RedoStack.GetName(redoCount - 4);
				if (redoCount >= 5) redoName5MenuItem.Text = editor.Document.UndoRedo.RedoStack.GetName(redoCount - 5);
			}
		}

		private void redoName1MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 1 level off the undo stack
			for (int i = 0; i < 1; i++)
				editor.Document.UndoRedo.Redo();
		}

		private void redoName2MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 2 levels off the undo stack
			for (int i = 0; i < 2; i++)
				editor.Document.UndoRedo.Redo();
		}

		private void redoName3MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 3 levels off the undo stack
			for (int i = 0; i < 3; i++)
				editor.Document.UndoRedo.Redo();
		}

		private void redoName4MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 4 levels off the undo stack
			for (int i = 0; i < 4; i++)
				editor.Document.UndoRedo.Redo();
		}

		private void redoName5MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 5 levels off the undo stack
			for (int i = 0; i < 5; i++)
				editor.Document.UndoRedo.Redo();
		}

		private void searchFindMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			ExecuteAppAction(AppAction.SearchFindReplace);
		}

		private void searchGoToLineMenuItem_Click(object sender, System.EventArgs e)
		{
			// Show the goto line form
			new GoToLineForm(editor).ShowDialog(this);
			// NOTE: Alternatively this works to use a built-in dialog:  editor.ShowGoToLineForm(this);
		}

		private void searchIncrementalSearchMenuItem_Click(object sender, System.EventArgs e)
		{
			// Perform another incremental search
			editor.SelectedView.FindReplace.IncrementalSearch.PerformSearch(false);
		}

		private void searchReverseIncrementalSearchMenuItem_Click(object sender, System.EventArgs e)
		{
			// Perform another incremental search
			editor.SelectedView.FindReplace.IncrementalSearch.PerformSearch(true);
		}

		private void smartTagContextMenuItem_Click(object sender, EventArgs e)
		{
			OwnerDrawMenuItem menuItem = (OwnerDrawMenuItem)sender;
			if (menuItem.Text == ResourceLookup.GetLookupText("UNDOPASTEOPER", "Undo the Paste Operation", ""))
				editor.Document.UndoRedo.Undo();
		}

		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			AppAction action = (AppAction)Enum.Parse(typeof(AppAction), e.Button.Tag.ToString());
			this.ExecuteAppAction(action);
		}
		private void toolBar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
            if (e.ClickedItem.Tag != null)
            {
                AppAction action = (AppAction)Enum.Parse(typeof(AppAction), e.ClickedItem.Tag.ToString());
                this.ExecuteAppAction(action);
            }
		}

		private void toolsHighlightingStyleEditorMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			this.ExecuteAppAction(AppAction.ToolsHighlightingStyleEditor);
		}

		private void toolsTextStatisticsMenuItem_Click(object sender, System.EventArgs e)
		{
			// Execute the AppAction
			this.ExecuteAppAction(AppAction.ToolsTextStatistics);
		}

		private void undoNamesMenuItem_Popup(object sender, System.EventArgs e)
		{
			// Get the number of items on the undo stack
			int undoCount = editor.Document.UndoRedo.UndoStack.Count;

			if (undoCount == 0)
			{
				// There are no items on the undo stack
				undoName1MenuItem.Visible = true;
				undoName1MenuItem.Enabled = false;
				undoName1MenuItem.Text = string.Format("({0})", ResourceLookup.GetLookupText("NOACTNS", "No actions", ""));
                undoName2MenuItem.Visible = undoName3MenuItem.Visible = undoName4MenuItem.Visible = undoName5MenuItem.Visible = false;
			}
			else
			{
				// Make the menu items visible if there are enough items on the stack
				undoName1MenuItem.Visible = (undoCount >= 1);
				undoName1MenuItem.Enabled = true;
				undoName2MenuItem.Visible = (undoCount >= 2);
				undoName3MenuItem.Visible = (undoCount >= 3);
				undoName4MenuItem.Visible = (undoCount >= 4);
				undoName5MenuItem.Visible = (undoCount >= 5);

				// Update the menu item text
				if (undoCount >= 1) undoName1MenuItem.Text = editor.Document.UndoRedo.UndoStack.GetName(undoCount - 1);
				if (undoCount >= 2) undoName2MenuItem.Text = editor.Document.UndoRedo.UndoStack.GetName(undoCount - 2);
				if (undoCount >= 3) undoName3MenuItem.Text = editor.Document.UndoRedo.UndoStack.GetName(undoCount - 3);
				if (undoCount >= 4) undoName4MenuItem.Text = editor.Document.UndoRedo.UndoStack.GetName(undoCount - 4);
				if (undoCount >= 5) undoName5MenuItem.Text = editor.Document.UndoRedo.UndoStack.GetName(undoCount - 5);
			}
		}

		private void undoName1MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 1 level off the undo stack
			for (int i = 0; i < 1; i++)
				editor.Document.UndoRedo.Undo();
		}

		private void undoName2MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 2 levels off the undo stack
			for (int i = 0; i < 2; i++)
				editor.Document.UndoRedo.Undo();
		}

		private void undoName3MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 3 levels off the undo stack
			for (int i = 0; i < 3; i++)
				editor.Document.UndoRedo.Undo();
		}

		private void undoName4MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 4 levels off the undo stack
			for (int i = 0; i < 4; i++)
				editor.Document.UndoRedo.Undo();
		}

		private void undoName5MenuItem_Click(object sender, System.EventArgs e)
		{
			// Undo 5 levels off the undo stack
			for (int i = 0; i < 5; i++)
				editor.Document.UndoRedo.Undo();
		}

		private void mnuSaveCompile_Click(object sender, EventArgs e)
		{
			ExecuteAppAction(AppAction.FileSave);
		}

		private void mnuShowLineNumbers_Click(object sender, EventArgs e)
		{
			ExecuteAppAction(AppAction.ToolsSettingsShowLineNumbers);
		}

        private void mnuCheckin_Click(object sender, EventArgs e)
        {
            ExecuteAppAction(AppAction.VersionControlCheckin);
        }

        private void mnuCompare_Click(object sender, EventArgs e)
        {
            ExecuteAppAction(AppAction.VersionControlCompare);
        }

		#endregion

		#region Editor Events
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// EDITOR EVENT HANDLERS
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Occurs after a delay following the raising of the <see cref="SyntaxEditor.DocumentTextChanged"/> and
		/// <see cref="SyntaxEditor.DocumentSemanticParseDataChanged"/> events, allowing for updating of user interface controls external to SyntaxEditor.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void editor_UserInterfaceUpdate(object sender, EventArgs e)
		{
			this.UpdateSemanticUI();
		}


		/// <summary>
		/// Occurs after a language is loaded
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void editor_DocumentSyntaxLanguageLoaded(object sender, ActiproSoftware.SyntaxEditor.SyntaxLanguageEventArgs e)
		{
			if (e.Language is ActiproSoftware.SyntaxEditor.Addons.DotNet.Dom.DotNetSyntaxLanguage)
			{
				// Update the language data
				dotNetProjectResolver.CodeSnippetFolders.Clear();
				if (e.Language.Key == "C#")
					dotNetProjectResolver.CodeSnippetFolders.Add(cSharpCodeSnippetFolder);
				else
					dotNetProjectResolver.CodeSnippetFolders.Add(vbCodeSnippetFolder);
				editor.Document.LanguageData = dotNetProjectResolver;
			}
			else
			{
				// Clear the language data
				editor.Document.LanguageData = null;
			}

			// Update other UI
			editCommentSelectionButton.Enabled = (e.Language.LineCommentDelimiter != null);
			editUncommentSelectionButton.Enabled = (e.Language.LineCommentDelimiter != null);

			// Alter the user interface of the window based on the language
			if (e.Language is ActiproSoftware.SyntaxEditor.Addons.DotNet.Dom.DotNetSyntaxLanguage)
			{
				typeMemberDropDownList.SyntaxEditor = editor;
				typeMemberDropDownList.Visible = true;
			}
			else
			{
				typeMemberDropDownList.SyntaxEditor = null;
				typeMemberDropDownList.Visible = false;
			}

			// Get all the languages
			SyntaxLanguageCollection languages = SyntaxLanguage.GetLanguageCollection(e.Language);

			// Add status message
			this.WriteLine("SyntaxLanguageLoaded: " + languages.ToString());
		}

		/// <summary>
		/// Occurs after the undo or redo stack is changed so that application user interface enabled states, such as toolbar buttons, can be updated.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void editor_DocumentUndoRedoStateChanged(object sender, ActiproSoftware.SyntaxEditor.UndoRedoStateChangedEventArgs e)
		{
			if (e.EnabledStatesChanged)
			{
				editUndoMenuItem.Enabled = editor.Document.UndoRedo.CanUndo;
				editUndoButton.Enabled = editor.Document.UndoRedo.CanUndo;
				editRedoMenuItem.Enabled = editor.Document.UndoRedo.CanRedo;
				editRedoButton.Enabled = editor.Document.UndoRedo.CanRedo;
			}
		}

		/// <summary>
		/// Occurs when an incremental search event occurs.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void editor_IncrementalSearch(object sender, ActiproSoftware.SyntaxEditor.IncrementalSearchEventArgs e)
		{
			switch (e.EventType)
			{
				case IncrementalSearchEventType.Activated:
				case IncrementalSearchEventType.Search:
					string text = editor.SelectedView.FindReplace.IncrementalSearch.SearchUp ?
						ResourceLookup.GetLookupText("MNUREVINCSEARCH", "&Reverse Incremental Search", "") :
						ResourceLookup.GetLookupText("MNUINCSEARCH", "&Incremental Search", "");
					text = text.Replace("&", "") + ": " + editor.SelectedView.FindReplace.IncrementalSearch.FindText;
					if (!(editor.SelectedView.FindReplace.IncrementalSearch.FindText.Length == 0 || e.ResultSet.Count > 0))
					{
						text += " (" + ResourceLookup.GetLookupText("NOTFOUND", "Not Found", "").ToLower() + ")";
					}
					this.SetStatusMessage(text);
					break;
				case IncrementalSearchEventType.Deactivated:
					this.SetStatusMessage(ResourceLookup.GetLookupText("READY", "Ready", ""));
					break;
				case IncrementalSearchEventType.CharacterIgnored:
					// This happens if a character is ignored because the current find text was not found in the last search
					break;
			}
		}



		/// <summary>
		/// Occurs when the currently active smart tag is clicked.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void editor_IntelliPromptSmartTagClicked(object sender, EventArgs e)
		{
			switch (editor.IntelliPrompt.SmartTag.ActiveSmartTag.Key)
			{
				case "Paste":
					{
						// Show a post-paste context menu in response to the smart tag being clicked
						OwnerDrawContextMenu contextMenu = new OwnerDrawContextMenu();
						contextMenu.ImageList = imageList;
						contextMenu.MenuItems.Add(new OwnerDrawMenuItem(ResourceLookup.GetLookupText("UNDOPASTEOPER", "Undo the Paste Operation", ""), editUndoButton.ImageIndex,
							new EventHandler(smartTagContextMenuItem_Click), Shortcut.CtrlZ, editor.IntelliPrompt.SmartTag.ActiveSmartTag));
						contextMenu.MenuItems.Add(new OwnerDrawMenuItem("-"));
						contextMenu.MenuItems.Add(new OwnerDrawMenuItem(ResourceLookup.GetLookupText("CANCEL", "Cancel", "")));
						contextMenu.Show(editor, editor.PointToClient(new Point(editor.IntelliPrompt.SmartTag.Bounds.Left, editor.IntelliPrompt.SmartTag.Bounds.Bottom)));
						break;
					}
			}

			// Hide the smart tag
			editor.IntelliPrompt.SmartTag.Hide();
		}

		/// <summary>
		/// Occurs before a key is typed.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void editor_KeyTyping(object sender, ActiproSoftware.SyntaxEditor.KeyTypingEventArgs e)
		{

			if (e.KeyChar == '(')
			{
				// Get the offset
				int offset = editor.SelectedView.Selection.EndOffset;

				// Get the text stream
				TextStream stream = editor.Document.GetTextStream(offset);

				// Get the language
				SyntaxLanguage language = stream.Token.Language;

				// If in C#...
				if ((language is DynamicSyntaxLanguage) && (language.Key == "C#"))
				{
					if ((offset >= 10) && (editor.Document.GetSubstring(offset - 10, 10) == "Invalidate"))
					{
						// Show parameter info
						editor.IntelliPrompt.ParameterInfo.Info.Clear();
						editor.IntelliPrompt.ParameterInfo.Info.Add(@"void <b>Control.Invalidate</b>()<br/>" +
							@"Invalidates the entire surface of the control and causes the control to be redrawn.");
						editor.IntelliPrompt.ParameterInfo.Info.Add(@"void Control.Invalidate(<b>System.Drawing.Rectangle rc</b>, bool invalidateChildren)<br/>" +
							@"<b>rc:</b> A System.Drawing.Rectangle object that represents the region to invalidate.");
						editor.IntelliPrompt.ParameterInfo.Show(offset - 10);
					}
				}
			}
		}

		/// <summary>
		/// Occurs when a paste or drag/drop operation occurs over the control, allowing for customization of the text to be inserted.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void editor_PasteDragDrop(object sender, ActiproSoftware.SyntaxEditor.PasteDragDropEventArgs e)
		{
			// Allow file name drops from Windows Explorer
			if (e.DataObject.GetDataPresent(DataFormats.FileDrop))
			{
				object files = e.DataObject.GetData(DataFormats.FileDrop);
				if ((files is string[]) && (((string[])files).Length > 0))
				{
					string filename = ((string[])files)[0];

					// If performing a drop of a .snippet file, see if it contains any code snippets and if so, 
					//    activate the first one that is found
					if ((e.Source == PasteDragDropSource.DragDrop) && (Path.GetExtension(filename).ToLower() == ".snippet"))
					{
						CodeSnippet[] codeSnippets = CodeSnippet.LoadFromXml(filename);
						if (codeSnippets.Length > 0)
						{
							e.Text = String.Empty;
							editor.IntelliPrompt.CodeSnippets.Activate(codeSnippets[0]);
							return;
						}
					}

					// Simply insert the path of the file
					e.Text = filename;
				}
			}

			// If a paste occurred, show a smart tag for more options
			if ((e.Source == PasteDragDropSource.PasteComplete) && (editor.Caret.Offset > 0))
			{
				editor.IntelliPrompt.SmartTag.Add(new SmartTag("Paste", imageList.Images[editPasteButton.ImageIndex],
                    Session.CurrentSession.Resources.GetMessage("OPTPOSTPASTOPER", "Options for <b>post-paste</b> operations.", "").Text),
					new TextRange(Math.Max(0, editor.Caret.Offset - e.Text.Length), editor.Caret.Offset));
			}
		}

		/// <summary>
		/// Occurs when the selection changes.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void editor_SelectionChanged(object sender, SelectionEventArgs e)
		{
			// Add status message

			// Update the position in the status bar (uses 1-based line character specifications... better for end users)
			positionPanel.Text = string.Format(positionPanelTextFmt, e.DisplayCaretDocumentPosition.Line,
				e.DisplayCaretCharacterColumn, e.DisplayCaretDocumentPosition.Character);

			// Use this for debugging
			// positionPanel.Text = "Off " + editor.Caret.Offset +
			//    	"\tLn " + e.DisplayCaretDocumentPosition.Line + 
			//     	"\tCh " + e.DisplayCaretDocumentPosition.Character;

			// Get information about the current token
			IToken token = editor.Document.Tokens.GetTokenAtOffset(editor.Caret.Offset);

			// Update the panels
			languagePanel.Text = (token.Language != null ? token.Language.Key : editor.Document.Language.Key);
			statePanel.Text = (token.LexicalState != null ? token.LexicalState.Key : string.Format(statePanelTextFmt, token.LexicalStateID));
			tokenPanel.Text = token.Key + (editor.Caret.Offset == token.StartOffset ? " (*)" : String.Empty);

			if (token.HasFlag(LexicalParseFlags.LanguageStart))
				hierarchyLevelPanel.Text = token.NestingDepth + ">";
			else if (token.HasFlag(LexicalParseFlags.LanguageEnd))
				hierarchyLevelPanel.Text = "<" + token.NestingDepth;
			else if (token.IsDocumentEnd)
				hierarchyLevelPanel.Text = "!" + token.NestingDepth;
			else
				hierarchyLevelPanel.Text = token.NestingDepth.ToString();
		}

		/// <summary>
		/// Occurs when smart indent is to be performed.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void editor_SmartIndent(object sender, ActiproSoftware.SyntaxEditor.SmartIndentEventArgs e)
		{
			// By default, SyntaxEditor initializes the e.IndentAmount properly for Block formatting.
			// You can change e.IndentAmount to provide "smart" indenting based on language context.

			if (editor.Document.Language is DynamicSyntaxLanguage)
			{
				// Increment indent if pressing ENTER after a curly brace
				switch (editor.Document.Language.Key)
				{
					case "C#":
					case "Java":
					case "JScript":
					case "PHP":
						{
							TextStream stream = editor.Document.GetTextStream(editor.SelectedView.Selection.FirstOffset);
							bool exitLoop = false;
							while (stream.Offset > 0)
							{
								stream.GoToPreviousToken();
								switch (stream.Token.Key)
								{
									case "WhitespaceToken":
										// Ignore whitespace
										break;
									case "OpenCurlyBraceToken":
										e.IndentAmount++;
										exitLoop = true;
										break;
									default:
										exitLoop = true;
										break;
								}
								if (exitLoop)
									break;
							}
							break;
						}
				}
			}
		}


		/// <summary>
		/// Occurs after a trigger is activated.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void editor_TriggerActivated(object sender, ActiproSoftware.SyntaxEditor.TriggerEventArgs e)
		{
			switch (editor.Document.Language.Key)
			{
				case "C#":
					{
						switch (e.Trigger.Key)
						{
							case "MemberListTrigger":
								{
									// Construct full name of item to see if reflection can be used... iterate backwards through the token stream
									TokenStream stream = editor.Document.GetTokenStream(editor.Document.Tokens.IndexOf(
										editor.SelectedView.Selection.EndOffset - 1));
									string fullName = String.Empty;
									int periods = 0;
									while (stream.Position > 0)
									{
										IToken token = stream.ReadReverse();
										switch (token.Key)
										{
											case "IdentifierToken":
											case "NativeTypeToken":
												fullName = editor.Document.GetTokenText(token) + fullName;
												break;
											case "PunctuationToken":
												if ((token.Length == 1) && (editor.Document.GetTokenText(token) == "."))
												{
													fullName = editor.Document.GetTokenText(token) + fullName;
													periods++;
												}
												else
													stream.Position = 0;
												break;
											default:
												stream.Position = 0;
												break;
										}
									}

									// Convert common types
									if ((fullName.Length > 0) && (periods == 0))
									{
										switch (fullName)
										{
											case "bool":
												fullName = "System.Boolean";
												break;
											case "byte":
												fullName = "System.Byte";
												break;
											case "char":
												fullName = "System.Char";
												break;
											case "decimal":
												fullName = "System.Decimal";
												break;
											case "double":
												fullName = "System.Double";
												break;
											case "short":
												fullName = "System.Int16";
												break;
											case "int":
												fullName = "System.Int32";
												break;
											case "long":
												fullName = "System.Int64";
												break;
											case "object":
												fullName = "System.Object";
												break;
											case "sbyte":
												fullName = "System.SByte";
												break;
											case "float":
												fullName = "System.Single";
												break;
											case "string":
												fullName = "System.String";
												break;
											case "ushort":
												fullName = "System.UInt16";
												break;
											case "uint":
												fullName = "System.UInt32";
												break;
											case "ulong":
												fullName = "System.UInt64";
												break;
											case "void":
												fullName = "System.Void";
												break;
										}
									}

									// If a full name is found...
									if (fullName.Length > 0)
									{
										// Get the member list
										IntelliPromptMemberList memberList = editor.IntelliPrompt.MemberList;
										memberList.ResetAllowedCharacters();

										// Set IntelliPrompt ImageList
										memberList.ImageList = imageList;

										// Add items to the list
										memberList.Clear();

										// Find a type that matches the full name
										Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
										foreach (Assembly assemblyData in assemblies)
										{
											Type type = assemblyData.GetType(fullName, false, false);
											if (type != null)
											{
												memberList.AddReflectionForTypeMembers(type, IntelliPromptTypeMemberFlags.AllMemberTypes |
													IntelliPromptTypeMemberFlags.AllAccessTypes | IntelliPromptTypeMemberFlags.Static);
												break;
											}
										}

										// If no specific type was found... 
										if (memberList.Count == 0)
										{
											// Add namespace to examine
											System.Collections.Specialized.StringCollection namespaceNames = new System.Collections.Specialized.StringCollection();
											namespaceNames.Add(fullName);

											// Create the array of flags for each Assembly... this generic example will assume we only
											//   want namespaces and types that are public
											IntelliPromptNamespaceAndTypeFlags[] flags = new IntelliPromptNamespaceAndTypeFlags[assemblies.Length];
											for (int index = 0; index < flags.Length; index++)
												flags[index] = IntelliPromptNamespaceAndTypeFlags.NamespacesAndTypes | IntelliPromptNamespaceAndTypeFlags.Public;

											// Use the reflection helper method
											memberList.AddReflectionForAssemblyNamespacesAndTypes(assemblies, flags, namespaceNames, null, false);

											// Loop through the items that were created and add some descriptions
											foreach (IntelliPromptMemberListItem item in memberList)
											{
												if (item.ImageIndex == (int)ActiproSoftware.Products.SyntaxEditor.IconResource.Namespace)
													item.Description = String.Format("namespace <b>{0}</b>", item.Tag.ToString());
												else if (item.Tag is Type)
												{
													Type type = (Type)item.Tag;
													if (type.IsEnum)
														item.Description = String.Format("enum <b>{0}</b>", type.FullName);
													else if (type.IsInterface)
														item.Description = String.Format("interface <b>{0}</b>", type.FullName);
													else if (type.IsValueType)
														item.Description = String.Format("struct <b>{0}</b>", type.FullName);
													else if (type.IsSubclassOf(typeof(Delegate)))
														item.Description = String.Format("delegate <b>{0}</b>", type.FullName);
													else
														item.Description = String.Format("class <b>{0}</b>", type.FullName);
												}
											}
										}

										// Show the list
										if (memberList.Count > 0)
											memberList.Show();
									}
									break;
								}
						}
						break;
					}
			}
		}

		/// <summary>
		/// Occurs when a mouse button is pressed over an <c>EditorView</c>.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void editor_ViewMouseDown(object sender, EditorViewMouseEventArgs e)
		{
			switch (e.HitTestResult.Target)
			{
				case SyntaxEditorHitTestTarget.IndicatorMargin:
					{
						if (Control.ModifierKeys == Keys.Shift)
						{
							// Remove all span indicators from the line
							editor.SelectedView.CurrentDocumentLine.SpanIndicators.Clear();
							return;
						}

						// Make sure text is selected
						if (editor.SelectedView.Selection.Length == 0)
						{
                            FWBS.OMS.UI.Windows.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("BREAKPOINT", "Please select some text and re-click within the indicator margin to set a breakpoint over the selected text.", "").Text);
							return;
						}


						if (Control.ModifierKeys == Keys.Control)
						{
							// Add a current statement indicator to the line if it doesn't overlap with another indicator (indicators within a layer cannot overlap)
							SpanIndicatorLayer layer = editor.Document.SpanIndicatorLayers[SpanIndicatorLayer.CurrentStatementKey];
							if (layer == null)
							{
								layer = new SpanIndicatorLayer(SpanIndicatorLayer.CurrentStatementKey, SpanIndicatorLayer.CurrentStatementDisplayPriority);
								editor.Document.SpanIndicatorLayers.Add(layer);
							}
							if (!layer.OverlapsWith(editor.SelectedView.Selection.TextRange))
								layer.Add(new CurrentStatementSpanIndicator(), editor.SelectedView.Selection.TextRange);
						}
						else
						{
							// Add a breakpoint to the line if it doesn't overlap with another breakpoint (indicators within a layer cannot overlap)
							SpanIndicatorLayer layer = editor.Document.SpanIndicatorLayers[SpanIndicatorLayer.BreakpointKey];
							if (layer == null)
							{
								layer = new SpanIndicatorLayer(SpanIndicatorLayer.BreakpointKey, SpanIndicatorLayer.BreakpointDisplayPriority);
								editor.Document.SpanIndicatorLayers.Add(layer);
							}
							if (!layer.OverlapsWith(editor.SelectedView.Selection.TextRange))
								layer.Add(new BreakpointSpanIndicator(), editor.SelectedView.Selection.TextRange);
						}
						break;
					}
				default:
					if (e.HitTestResult.Token != null)
					{
						// See if the token is a URL token
						switch (e.HitTestResult.Token.Key)
						{
							case "CommentURLToken":
							case "MultiLineCommentURLToken":
							case "XMLCommentURLToken":
								// If the CTRL key is pressed, navigate to the URL
								if ((e.Button == MouseButtons.Left) && (Control.ModifierKeys == Keys.Control))
								{
									e.Cancel = true;
									System.Diagnostics.Process.Start(editor.Document.GetTokenText(e.HitTestResult.Token));
								}
								break;
						}
					}
					break;
			}
		}

		/// <summary>
		/// Occurs when the mouse is hovered over an <c>EditorView</c>.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void editor_ViewMouseHover(object sender, ActiproSoftware.SyntaxEditor.EditorViewMouseEventArgs e)
		{
			if (e.HitTestResult.Token != null)
			{
				// Get the language
				SyntaxLanguage language = e.HitTestResult.Token.Language;

				// If in C#...
				if ((language is DynamicSyntaxLanguage) && (language.Key == "C#"))
				{
					switch (e.HitTestResult.Token.Key)
					{
						case "CommentURLToken":
						case "MultiLineCommentURLToken":
						case "XMLCommentURLToken":
							// This shows how to provide a popup for when the mouse is over a URL in a comment
							e.ToolTipText = editor.Document.GetTokenText(e.HitTestResult.Token) + "<br/><b>CTRL + click to follow link</b>";
							break;
						case "IdentifierToken":
							// This simple example shows how to display quick info when hovering over an identifier "System"
							if (editor.Document.GetTokenText(e.HitTestResult.Token) == "System")
								e.ToolTipText = "namespace <b>System</b>";
							break;
					}
				}
			}
		}


		#endregion

		#region Private Procedures
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// NON-PUBLIC PROCEDURES
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Execute an application action.
		/// </summary>
		/// <param name="action">
		/// An <c>AppAction</c> specifying the type of action to take.
		/// </param>
		/// <remarks>
		/// This procedure is a centralized place for handling menu and toolbar commands.
		/// </remarks>
		private void ExecuteAppAction(AppAction action)
		{
			try
			{
				// Based on the action passed, do something...
				switch (action)
				{
					case AppAction.InsertWorkflowActivity:
						{
							FWBS.OMS.Design.CodeBuilder.NewWFActivityForm dlg = new FWBS.OMS.Design.CodeBuilder.NewWFActivityForm(this.CurrentLanguage == Language.CSharp);
							if (dlg.ShowDialog() == DialogResult.OK)
							{
								this.editor.Document.InsertText(DocumentModificationType.Typing, this.editor.Caret.Offset, dlg.TemplateCode + Environment.NewLine);
							}
							dlg.Dispose();
						}
						break;

					case AppAction.EditWorkflow:
						{
							// let user select the workflow
							FWBS.Common.KeyValueCollection retvals = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(null, Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.WorkflowPicker), false, new Size(-1, -1), null, new FWBS.Common.KeyValueCollection());
							if (retvals != null)
							{
								var code = Convert.ToString(retvals[0].Value);
								// create the workflow designer passing the workflow code to constructor - this way this assembly is NOT dependent on the designer assembly!
								try
								{
									Type t = FWBS.OMS.Session.CurrentSession.TypeManager.Load("FWBS.OMS.Workflow.Admin.WorkflowForm,FWBS.OMS.Workflow");
									object obj = t.InvokeMember("CreateForm", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[] { code });
									if (obj != null)
									{
										// we have a designer form
										Form form = obj as Form;
										if (form != null)
										{
											// show designer
											form.Show();
											form.Focus();
										}
									}
								}
								catch (Exception ex)
								{
									// we cannot create workflow designer
									ErrorBox.Show(ParentForm, ex);
								}
							}
						}
						break;

					case AppAction.InsertWorkflow:
						{
							FWBS.Common.KeyValueCollection retvals = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(null, Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.WorkflowPicker), false, new Size(-1, -1), null, new FWBS.Common.KeyValueCollection());
							if (retvals != null)
							{
								var code = Convert.ToString(retvals[0].Value);

								// Insert workflow runtime calling code snippet
								if (this.CurrentLanguage == Language.VBDotNet)
								{
									// Insert snippet for VB language
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "' TODO: Create and add your input parameters if the workflow has input arguments defined" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "' This is the collection which gets mapped to the input arguments of the workflow" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "' The name of the dictionary entry must match the name of the workflow input argument" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "' for the entry value to be assigned to the workflow input argument." + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "Dim inParams As Dictionary(Of String, Object)" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "' This is the collection which is returned after the workflow is executed" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "' The name of the entry matches the name of the workflow output argument." + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "Dim outParams As IDictionary(Of String, Object)" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "' The code of the workflow to invoke." + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "Dim workflowName = \"" + code + "\"" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "' The Context that is passed to the workflow." + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "Dim context As FWBS.OMS.IContext = Me.Context" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "' This is the time limit within which the workflow execution must complete." + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "' Otherwise workflow execution will get aborted." + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "Dim timeOut = TimeSpan.FromMilliseconds(Int32.MaxValue)" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "Try" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "\t' For server-side workflows, use the appropriate Execute() method overload." + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "\toutParams = FWBS.OMS.Workflow.WFRuntime.Execute(workflowName, timeOut, inParams, context)" + Environment.NewLine);	
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "\t' Workflow execution has been successful" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "Catch ex As Exception" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "\t' An error occured during workflow execution, display the error" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "\tFWBS.OMS.UI.Windows.ErrorBox.Show(ex)" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "End Try" + Environment.NewLine);
								}
								else
								{
									// Insert snippet for C# language
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "#region Invoke workflow '" + code + "'" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "// TODO: Create and add your input parameters if the workflow has input arguments defined" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "// This is the collection which gets mapped to the input arguments of the workflow" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "// The name of the dictionary entry must match the name of the workflow input argument" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "// for the entry value to be assigned to the workflow input argument." + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "System.Collections.Generic.Dictionary<String, Object> inParams = null;" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "// This is the collection which is returned after the workflow is executed" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "// The name of the entry matches the name of the workflow output argument." + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "System.Collections.Generic.IDictionary<String, Object> outParams = null;" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "// The code of the workflow to invoke." + Environment.NewLine);                                                                        
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "String workflowName = \"" + code + "\";" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "// The Context that is passed to the workflow." + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "FWBS.OMS.IContext context = this.Context;" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "// This is the time limit within which the workflow execution must complete." + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "// Otherwise workflow execution will get aborted." + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "TimeSpan timeOut = TimeSpan.FromMilliseconds(Int32.MaxValue);" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "try" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "{" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "\t// For server-side workflows, use the appropriate Execute() method overload." + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "\toutParams = FWBS.OMS.Workflow.WFRuntime.Execute(workflowName, timeOut, inParams, context);" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "\t// Workflow execution has been successful" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "}" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "catch (Exception ex)" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "{" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "\t// An error occured during workflow execution, display the error" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "\tFWBS.OMS.UI.Windows.ErrorBox.Show(ex);" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "}" + Environment.NewLine);
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, "#endregion" + Environment.NewLine);
								}
								editor.Document.Modified = true;
							}
						}
						break;

					case AppAction.InsertIBasicEnquiryControl:
						{
							frmCodeBuilder_ControlPicker picker = new frmCodeBuilder_ControlPicker();

							picker.cmbControls.Items.Clear();
							for (int i = 0; i < _objects.Count; i++)
							{
								picker.cmbControls.Items.Add(_objects[i]);
							}
							picker.cmbControls.Sorted = true;
							picker.ShowDialog(this);
							if (picker.DialogResult == DialogResult.OK)
							{
								if (picker.cmbIfMissing.Text != "")
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, @"EnquiryForm.GetIBasicEnquiryControl2(""" + picker.cmbControls.Text + @""",EnquiryControlMissing." + picker.cmbIfMissing.Text.Split("-".ToCharArray())[0].Trim() + @")");
								else
									editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, @"EnquiryForm.GetIBasicEnquiryControl2(""" + picker.cmbControls.Text + @""")");
							}
							break;
						}

					case AppAction.InsertControl:
						{
							frmCodeBuilder_ControlPicker picker = new frmCodeBuilder_ControlPicker();

							picker.cmbControls.Items.Clear();
							picker.MissingVisible = false;
							for (int i = 0; i < _objects.Count; i++)
							{
								picker.cmbControls.Items.Add(_objects[i]);
							}
							picker.cmbControls.Sorted = true;
							picker.ShowDialog(this);
							if (picker.DialogResult == DialogResult.OK)
							{
								editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, @"EnquiryForm.GetControl(""" + picker.cmbControls.Text + @""")");
							}
							break;
						}
					case AppAction.InsertIListEnquiryControl:
						{
							frmCodeBuilder_ControlPicker picker = new frmCodeBuilder_ControlPicker();

							picker.cmbControls.Items.Clear();
							picker.MissingVisible = false;
							for (int i = 0; i < _objects.Count; i++)
							{
								picker.cmbControls.Items.Add(_objects[i]);
							}
							picker.cmbControls.Sorted = true;
							picker.ShowDialog(this);
							if (picker.DialogResult == DialogResult.OK)
							{
								editor.Document.InsertText(DocumentModificationType.Typing, editor.Caret.Offset, @"EnquiryForm.GetIListEnquiryControl(""" + picker.cmbControls.Text + @""")");
							}
							break;
						}
					case AppAction.References:
						{
							using (var frm = new frmCodeBuilder_References())
							{
								frm.References = ScriptGen.GetReferences();

								if (frm.ShowDialog(this) != DialogResult.OK)
									return;

								ScriptGen.SetReferences(frm.References);
								ScriptGen.ExtractDistribution(ScriptGen.OutputDir);
								BuildIntelisenseAssemblies();
							}
						}
						break;
					case AppAction.ToolsRegenerateIntellisense:
						{
							var refs = ScriptGen.GetReferences();
							ScriptGen.SetReferences(refs);
							ScriptGen.ExtractDistribution(ScriptGen.OutputDir);
							BuildIntelisenseAssemblies();
						}
						break;
					case AppAction.EditCommentSelection:
						// Comment the currently selected text
						editor.SelectedView.CommentLines();
						break;
					case AppAction.EditCompleteWord:
						// Show a member list
						if (editor.SelectedView.GetCurrentLanguageForContext().IntelliPromptMemberListSupported)
							editor.SelectedView.GetCurrentLanguageForContext().IntelliPromptCompleteWord(editor);
						break;
					case AppAction.EditCopy:
						// Copy the currently selected text
						editor.SelectedView.CopyToClipboard();
						break;
					case AppAction.EditCut:
						// Cut the currently selected text
						editor.SelectedView.CutToClipboard();
						break;
					case AppAction.EditListMembers:
						// Show a member list
						if (editor.SelectedView.GetCurrentLanguageForContext().IntelliPromptMemberListSupported)
							editor.SelectedView.GetCurrentLanguageForContext().ShowIntelliPromptMemberList(editor);
						break;
					case AppAction.EditPaste:
						// Paste text from the clipboard
						editor.SelectedView.PasteFromClipboard();
						break;
					case AppAction.EditQuickInfo:
						// Show quick info
						if (editor.SelectedView.GetCurrentLanguageForContext().IntelliPromptQuickInfoSupported)
							editor.SelectedView.GetCurrentLanguageForContext().ShowIntelliPromptQuickInfo(editor);
						break;
					case AppAction.EditRedo:
						// Perform a redo action
						editor.Document.UndoRedo.Redo();
						break;
					case AppAction.EditUncommentSelection:
						// Uncomment the currently selected text
						editor.SelectedView.UncommentLines();
						break;
					case AppAction.EditUndo:
						// Perform an undo action
						editor.Document.UndoRedo.Undo();
						break;
					case AppAction.FilePageSetup:
						{
							// Show the page setup
							editor.ShowPageSetupForm(this);
							break;
						}
					case AppAction.FilePrint:
						try
						{
							// Show the print dialog and print
							editor.Print(true);
						}
						catch (Exception ex)
						{
							ErrorBox.Show(ParentForm, ex);
						}
						break;
					case AppAction.FilePrintPreview:
						// Implement to use the DocumentTitle property to make the current filename appear at the top of each page
						// editor.PrintSettings.DocumentTitle = "<filename>";  

						// Show the print preview dialog
						editor.PrintPreview();
						break;
                    case AppAction.CloseScript:
                        CloseScript();
                        break;
					case AppAction.FileSave:
						InternalSaveAndCompile();
						break;
					case AppAction.FileCompile:
						InternalCompile();
						break;

					case AppAction.SearchFindReplace:
						{
							// Show the find/replace form 
							if (findReplaceForm == null)
                            {
                                findReplaceForm = new FindReplaceForm(editor, findReplaceOptions);
                                if (findReplaceForm.DeviceDpi != 96)
                                {
                                    findReplaceForm.Font = new Font(findReplaceForm.Font.FontFamily, findReplaceForm.Font.Size * findReplaceForm.DeviceDpi / 96F);
                                }
                                findReplaceForm.StatusChanged += new FindReplaceStatusChangeEventHandler(findReplaceForm_StatusChanged);
							}
							//Need to verify changin owner to parent

							findReplaceForm.Owner = this.FindForm();
							if (findReplaceForm.Visible)
								findReplaceForm.Activate();
							else
								findReplaceForm.Show();
							break;
						}
					case AppAction.ToolsHighlightingStyleEditor:
						{
							// Show the style editor
							string init = "";
							StyleEditorForm form = new StyleEditorForm(editor);
							if (form.ShowDialog(this) == DialogResult.OK)
							{
								if (ScriptGen.Language == Script.ScriptLanguage.CSharp) init = "C#";
								else if (ScriptGen.Language == Script.ScriptLanguage.VB) init = "VB";
								SaveStyle(editor.Document.Language.HighlightingStyles, init);
							}
							break;
						}
					case AppAction.ToolsTextStatistics:
						{
							// Show the text statistics
							if (editor.SelectedView.Selection.Length > 0)
								editor.SelectedView.GetTextStatisticsForSelectedText().Show(this, ResourceLookup.GetLookupText("SELTEXTSTATS", "Selected Text Statistics", ""));
							else
                                editor.Document.GetTextStatistics().Show(this, ResourceLookup.GetLookupText("DOCUMENTSTATS", "Document Statistics", ""));
							break;
						}
					case AppAction.ToolsSettingsShowLineNumbers:
						{
							mnuShowLineNumbers.Checked = !mnuShowLineNumbers.Checked;
							editor.LineNumberMarginVisible = mnuShowLineNumbers.Checked;
							Favourites fav = new Favourites("CBLINENUM");
							if (fav.Count > 0)
								fav.Description(0, mnuShowLineNumbers.Checked.ToString());
							else
								fav.AddFavourite(mnuShowLineNumbers.Checked.ToString(), "");
							fav.Update();
							break;
						}
                    case AppAction.VersionControlCheckin:
                        {
                            CreateAndArchiveData(true);
                            break;
                        }
                    case AppAction.VersionControlCompare:
                        {
                            if (this.IsDirty || CheckObjectInIfNecessary())
                            {
                                CreateAndArchiveData(false);
                            }
                            else
                                OpenComparisonTool();
                            break;
                        }
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
		}


        // ************************************************************************************************
        //
        // CHECK IN
        //
        // ************************************************************************************************

        private void CreateAndArchiveData(bool CheckInOnly)
        {
            var scriptVersionData = GetScriptVersionDataSet();
            scriptVersionDataArchiver = new FWBS.OMS.UI.Windows.ScriptVersionDataArchiver();
            CheckForVCS();
            vcs.RestorationCompleted += new EventHandler<RestorationCompletedEventArgs>(vcs_RestorationCompleted);
            if(!CheckInOnly)
                scriptVersionDataArchiver.Saved += new EventHandler(scriptVersionDataArchiver_Saved);
            if (!string.IsNullOrWhiteSpace(_currentscriptobject.Code) && FWBS.Common.ConvertDef.ToInt64(_currentscriptobject.Version, 0) != 0)
                scriptVersionDataArchiver.SaveData(scriptVersionData, _currentscriptobject.Code, _currentscriptobject.Version, versionID: Guid.NewGuid());
            else
                System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("SAVESCRIPT", "Please save the script before attempting to access Version Control functions.", "").Text, FWBS.OMS.Branding.APPLICATION_NAME,MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void scriptVersionDataArchiver_Saved(object sender, EventArgs e)
        {
            scriptVersionDataArchiver.Saved -= new EventHandler(scriptVersionDataArchiver_Saved);
            CheckForVCS();
            OpenComparisonTool();
        }

        private DataSet GetScriptVersionDataSet()
        {
            var versionDataToSave = new DataSet();
            var script = BuildVersionDataTable();
            versionDataToSave.Tables.Add(script);
            return versionDataToSave;
        }

        // Methods to build the DataSet for the Archiver objects to use.
        private DataTable BuildVersionDataTable()
        {
            string sql = @"select * from dbScript where scrCode = @code";
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("code", _currentscriptobject.Code));
            System.Data.DataTable dt = connection.ExecuteSQL(sql, parList);
            dt.TableName = "dbScript";
            return dt;
        }

        // ************************************************************************************************
        //
        // COMPARE
        //
        // ************************************************************************************************

        private void OpenComparisonTool()
        {
            CheckForVCS();
            var result = vcs.ShowDialog();
            vcs.StartPosition = FormStartPosition.CenterScreen;
        }

        private void vcs_RestorationCompleted(object sender, RestorationCompletedEventArgs e)
        {
            FWBS.OMS.Script.ScriptGen _script = FWBS.OMS.Script.ScriptGen.GetScript(_currentscriptobject.Code);
            try
            {
                this.Load(_currentscripttype, _script);
                ScriptGen = _script;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("USRINTRFMSG", "The restoration process has completed however 3E MatterSphere has been unable to update the user interface. Please close the script window and re-open the script.", "").Text,Session.CurrentSession.Resources.GetResource("USRINTRFERR", "User Interface Update Error", "").Text, MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            finally
            {

                vcs.RestorationCompleted -= new EventHandler<RestorationCompletedEventArgs>(vcs_RestorationCompleted);
            }
        }

        private bool CheckObjectInIfNecessary()
        {
            string sql = "select * from dbScriptVersionData where Code = @code and Version = @version";
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("code", _currentscriptobject.Code));
            parList.Add(connection.CreateParameter("version", _currentscriptobject.Version));
            System.Data.DataTable dt = connection.ExecuteSQL(sql, parList);
            if (dt != null && dt.Rows.Count > 0)
                return false;
            else
                return true;
        }

        private void CheckForVCS()
        {
            if (vcs == null || !vcs.Visible)
                InstantiateVersionComparisonSelector();
        }

        private void InstantiateVersionComparisonSelector()
        {
            vcs = new VersionComparisonSelector(_currentscriptobject.Code, UI.Windows.LockableObjects.Script);
            vcs.RestorationCompleted += new EventHandler<RestorationCompletedEventArgs>(vcs_RestorationCompleted);
        }

        // ************************************************************************************************

        private void CloseScript()
        {
        }

        private bool InternalSaveAndCompile()
		{
            bool isNewScript = this._currentscriptobject.IsNew;

			// Save a document
			editor.Document.SaveFile(_currentfilename, LineTerminator.CarriageReturnNewline);
			editor.Document.Modified = false;
			bottomTabControl.SelectedIndex = 0;
			consoleTextBox.Clear();
			ScriptGen.SetSourceFile(_currentfilename);
			CompilerResults compilerResults;
			if (ScriptGen.Compile(true, out compilerResults))
			{
				ScriptGen.Update();
				_isdirty = false;
				editor.Focus();
                if (isNewScript)
                {
                    if (Session.CurrentSession.ObjectLocking)
                    {
                        LockState ls = new LockState();
                        if (!ls.CheckObjectLockState(this._currentscriptobject.Code, LockableObjects.Script))
                        {
                            ls.LockScriptObject(this._currentscriptobject.Code);
                            ls.MarkObjectAsOpen(this._currentscriptobject.Code, LockableObjects.Script);
                        }
                    }
                }
				return true;
			}
			else
			{
				_isdirty = true;
				PopulateErrorsList(compilerResults);
				editor.Focus();
				return false;
			}
		}

		private bool InternalCompile()
		{
			// Save a document
			editor.Document.SaveFile(_currentfilename, LineTerminator.CarriageReturnNewline);
			editor.Document.Modified = false;
			bottomTabControl.SelectedIndex = 0;
			consoleTextBox.Clear();
			ScriptGen.SetSourceFile(_currentfilename);
			CompilerResults compilerResults;
			if (ScriptGen.Compile(true, out compilerResults))
			{
				editor.Focus();
				return true;
			}
			else
			{
				_isdirty = true;
				PopulateErrorsList(compilerResults);
				editor.Focus();
				return false;
			}
		}

		private void LoadSettings()
		{
			languageNotSpecifiedMenuItem.Checked = true;
			FWBS.OMS.Favourites favs = new Favourites("SCRIPTLANG");
			if (favs.Count > 0)
			{
				var language = (Script.ScriptLanguage)FWBS.Common.ConvertDef.ToEnum(favs.Param1(0), Script.ScriptLanguage.CSharp);
				languageNotSpecifiedMenuItem.Checked = false;
				languageVBDotNetMenuItem.Checked = false;
				languageNotSpecifiedMenuItem.Checked = false;
				switch (language)
				{
					case FWBS.OMS.Script.ScriptLanguage.CSharp:
						languageCSharpMenuItem.Checked = true;
						break;
					case FWBS.OMS.Script.ScriptLanguage.VB:
						languageVBDotNetMenuItem.Checked = true;
						break;
					default:
						break;
				}
			}

			Favourites fav = new Favourites("CBLINENUM");
			if (fav.Count > 0)
			{
				bool val = Convert.ToBoolean(fav.Description(0));
				mnuShowLineNumbers.Checked = val;
				editor.LineNumberMarginVisible = val;
			}
		}

		private void LoadStyle(HighlightingStyleCollection styles, string LangInits)
		{
			foreach (HighlightingStyle style in styles)
			{
				Favourites fav = new Favourites(LangInits + style.Key);
				if (fav.Count > 0)
				{
					DataView dv = fav.GetDataView();
					foreach (DataRowView dr in dv)
					{
						string name = Convert.ToString(dr["usrFavDesc"]);
						string value = Convert.ToString(dr["usrFavObjParam1"]);
						if (name == "BackColor") style.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(value));
						if (name == "ForeColor") style.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(value));
						if (name == "FontFamilyName") style.FontFamilyName = value;
						if (name == "FontSize") style.FontSize = (float)Convert.ToDouble(value);
						if (name == "Bold") style.Bold = (Convert.ToBoolean(value) ? DefaultableBoolean.True : DefaultableBoolean.Default);
						if (name == "Italic") style.Italic = (Convert.ToBoolean(value) ? DefaultableBoolean.True : DefaultableBoolean.Default);
					}
				}
			}
		}

		private void SaveStyle(HighlightingStyleCollection styles, string LangInits)
		{
			foreach (HighlightingStyle style in styles)
			{
				Favourites fav = new Favourites(LangInits + style.Key);
				fav.ApplyFilter("usrFavDesc = 'BackColor'");
				if (fav.Count == 0)
					fav.AddFavourite("BackColor", "", style.BackColor.ToArgb().ToString());
				else
					fav.Param1(0, style.BackColor.ToArgb().ToString());

				fav.ApplyFilter("usrFavDesc = 'ForeColor'");
				if (fav.Count == 0)
					fav.AddFavourite("ForeColor", "", style.ForeColor.ToArgb().ToString());
				else
					fav.Param1(0, style.ForeColor.ToArgb().ToString());

				if (style.FontFamilyName != null)
				{
					fav.ApplyFilter("usrFavDesc = 'FontFamilyName'");
					if (fav.Count == 0)
						fav.AddFavourite("FontFamilyName", "", style.FontFamilyName);
					else
						fav.Param1(0, style.FontFamilyName);
				}
				if (style.FontSize != 0)
				{
					fav.ApplyFilter("usrFavDesc = 'FontSize'");
					if (fav.Count == 0)
						fav.AddFavourite("FontSize", "", style.FontSize.ToString());
					else
						fav.Param1(0, style.FontSize.ToString());
				}
				if (style.Bold.ToString() != "Default")
				{
					fav.ApplyFilter("usrFavDesc = 'Bold'");
					if (fav.Count == 0)
						fav.AddFavourite("Bold", "", style.Bold.ToString());
					else
						fav.Param1(0, style.Bold.ToString());
				}
				if (style.Italic.ToString() != "Default")
				{
					fav.ApplyFilter("usrFavDesc = 'Italic'");
					if (fav.Count == 0)
						fav.AddFavourite("Italic", "", style.Italic.ToString());
					else
						fav.Param1(0, style.Bold.ToString());
				}
				fav.Update();
			}
		}

		/// <summary>
		/// Loads a language definition into the editor.
		/// </summary>
		/// <param name="language">A <c>Language</c> specifying the type of language to load.</param>
		private void LoadLanguageDefinition(Language language)
		{
			// Store whether auto-case correction was enabled
			if (editor.Document.Language is ActiproSoftware.SyntaxEditor.Addons.VB.VBSyntaxLanguage)
				editor.Document.AutoCaseCorrectEnabled = lastLanguageUsedAutoCaseCorrection;
			else
				lastLanguageUsedAutoCaseCorrection = editor.Document.AutoCaseCorrectEnabled;

			try
			{
				// Based on the specified language, load a language definition from a file
				switch (language)
				{
					case Language.CSharp:
						// Load the advanced C# language add-on
						BuildIntelisenseAssemblies();
						editor.Document.Language = new ActiproSoftware.SyntaxEditor.Addons.CSharp.CSharpSyntaxLanguage();
						LoadStyle(editor.Document.Language.HighlightingStyles, "C#");
						CurrentLanguage = language;
						break;

					case Language.VBDotNet:
						// Load the advanced VB.NET language add-on
						BuildIntelisenseAssemblies();
						editor.Document.Language = new ActiproSoftware.SyntaxEditor.Addons.VB.VBSyntaxLanguage();
						LoadStyle(editor.Document.Language.HighlightingStyles, "VB");
						editor.Document.AutoCaseCorrectEnabled = true;
						CurrentLanguage = language;
						break;

					default:
						// Plain text
						editor.Document.ResetLanguage();
						break;
				}

				// Only enable lexical/semantic parsing and outlining if the language is not Plain Text...
				//   Note that the Plain Text language still does lexical parsing of text into words but for this sample we want to demo
				//   fast performance for large files so we are turning off lexical/semantic parsing and outlining options in the Document
				if (language == Language.None)
					editor.Document.Outlining.Mode = OutliningMode.None;
				else if (lastLanguageWasPlainText)
					editor.Document.Outlining.Mode = OutliningMode.Automatic;
				editor.Document.SemanticParsingEnabled = (language != Language.None);
				editor.Document.LexicalParsingEnabled = (language != Language.None);
				lastLanguageWasPlainText = (language == Language.None);
			}
			catch (Exception ex)
			{
				WriteLine("AN EXCEPTION OCCURRED WHILE LOADING A LANGUAGE DEFINITION:");
				WriteLine(ex.Message);
				if (!(ex is InvalidLanguageDefinitionException))
					WriteLine(ex.StackTrace);
				editor.Document.ResetLanguage();
			}
		}

		/// <summary>
		/// Sets a status bar message.
		/// </summary>
		/// <param name="text">The text to display.</param>
		public void SetStatusMessage(string text)
		{
			messagePanel.Text = text;
		}

		public void AddAllAssembliesInAppDomainAsExternalReferences()
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				try
				{
					if (!string.IsNullOrEmpty(assembly.Location))
						dotNetProjectResolver.AddExternalReference(assembly);
				}
				catch (Exception)
				{
				}
			}
		}


		private void BuildIntelisenseAssemblies()
		{
			CacheSplashForm splashForm = new CacheSplashForm();
			splashForm.Show();
			splashForm.Refresh();
			try
			{
				dotNetProjectResolver = new ActiproSoftware.SyntaxEditor.Addons.DotNet.Dom.DotNetProjectResolver();
				DirectoryInfo dir = new DirectoryInfo(Global.GetDBAppDataPath() + @"\Intellisense Cache\" + Session.CurrentSession.AssemblyVersion);
				dir.Create();

				dotNetProjectResolver.CachePath = dir.FullName;

				AddCoreSystemReferences();

				if (ScriptGen != null)
				{
					foreach (var ss in ScriptGen.GetReferences())
					{
						
						try
						{
							if (string.IsNullOrEmpty(ss.Location) && ss is Script.ScriptReference)
							{
								using (Script.ScriptGen script = Script.ScriptGen.GetScript(ss.Name))
								{
									script.Load();
									((Script.ScriptReference)ss).Location = script.OutputName;
								}
							}

							if (!string.IsNullOrWhiteSpace(ss.Location) && File.Exists(ss.Location))
							{
								dotNetProjectResolver.AddExternalReference(ss.Location);
							}
							else
							{
								var ass = Session.CurrentSession.AssemblyManager.Load(ss.AssemblyName);
								if (File.Exists(ass.Location))
									dotNetProjectResolver.AddExternalReference(ass.Location);
								else
									Trace.WriteLine(String.Format("Intellisense cannot find reference '{0}'", ass.Location));
							}
						}
						catch (Exception ex)
						{
							Trace.WriteLine(string.Format("Intellisense cannot find reference '{0}' because '{1}'", ss.AssemblyName, ex.Message));
						}
					}
				}

				if (ScriptType != null)
				{
					foreach (string s in ScriptType.Assemblies)
					{
						try
						{
							var ass = Session.CurrentSession.AssemblyManager.Load(s);
							if (File.Exists(ass.Location))
								dotNetProjectResolver.AddExternalReference(ass.Location);
							else
								Trace.WriteLine(String.Format("Intellisense cannot find reference '{0}'", ass.Location));
						}
						catch (Exception ex)
						{
							Trace.WriteLine(string.Format("Intellisense cannot find reference '{0}' because '{1}'", s, ex.Message));
						}
					}
				}
				editor.Document.LanguageData = dotNetProjectResolver;
			}
			catch
			{
				throw;
			}
			finally
			{
				splashForm.Dispose();
			}
		}

		private void AddCoreSystemReferences()
		{
			dotNetProjectResolver.AddExternalReferenceForMSCorLib();
		}

		private void ApplyReadOnlyZones()
		{

			var region = GetClassRegion();

			if (region != null)
			{
				var diff = region.End.StartOffset - region.Start.EndOffset;

				if (diff <= 1)
				{
					InsertLine(region.Start.EndOffset);
				}


				region = GetClassRegion(); 
				var layer = new SpanIndicatorLayer(SpanIndicatorLayer.ReadOnlyKey, SpanIndicatorLayer.ReadOnlyDisplayPriority);
				editor.Document.SpanIndicatorLayers.Add(layer);
				layer.Add(new ReadOnlySpanIndicator(), region.Start);
				layer.Add(new ReadOnlySpanIndicator(), region.End);
			}
			
		}
		#endregion

		#region Properties
		/// <summary>
		/// Shows the Usual Message for a Dirty Item
		/// </summary>
		/// <returns></returns>
		public bool IsFormDirty()
		{
			if (IsDirty)
			{
                DialogResult dr = FWBS.OMS.UI.Windows.MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?", "").Text, Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (dr == DialogResult.Yes) ExecuteAppAction(AppAction.FileSave);
				if (dr == DialogResult.No)
				{
					IsDirty = false;
				}
				if (dr == DialogResult.Cancel) return false;
			}
			return true;
		}

		public bool IsDirty
		{
			get
			{
				try
				{
					if (editor.Document != null)
						return (editor.Document.Modified || _isdirty || ScriptGen.IsDirty);
					else
						return _isdirty || ScriptGen.IsDirty;
				}
				catch
				{
					return _isdirty;
				}
			}
			set
			{
				if (IsDirty != value)
				{
                    if (editor.Document != null)
                        editor.Document.Modified = value;
                    ScriptGen.IsDirty = value;
					_isdirty = value;
				}
			}
		}

		public bool SaveAndCompile()
		{
			return InternalSaveAndCompile();
		}

		/// <summary>
		/// Returns the Current Script Object
		/// </summary>
		public FWBS.OMS.Script.ScriptGen ScriptGen
		{
			get
			{
				return _currentscriptobject;
			}
			set
			{
				_currentscriptobject = value;
				switch (ScriptGen.Language)
				{
					case FWBS.OMS.Script.ScriptLanguage.CSharp:
						LoadLanguageDefinition(Language.CSharp);
						break;
					case FWBS.OMS.Script.ScriptLanguage.VB:
						LoadLanguageDefinition(Language.VBDotNet);
						break;
					default:
						break;
				}
				Open(_currentscriptobject.SourceFile);
				_currentscriptobject.CompileStart -= new EventHandler(CurrentscriptObject_CompileStart);
				_currentscriptobject.CompileFinished -= new EventHandler(CurrentscriptObject_CompileFinished);
				_currentscriptobject.CompileError -= new EventHandler(CurrentscriptObject_CompileError);
				_currentscriptobject.CompileOutput -= new MessageEventHandler(CurrentscriptObject_CompileOutput);
				_currentscriptobject.CompileStart += new EventHandler(CurrentscriptObject_CompileStart);
				_currentscriptobject.CompileFinished += new EventHandler(CurrentscriptObject_CompileFinished);
				_currentscriptobject.CompileError += new EventHandler(CurrentscriptObject_CompileError);
				_currentscriptobject.CompileOutput += new MessageEventHandler(CurrentscriptObject_CompileOutput);
			}
		}

		

		private void CurrentscriptObject_CompileStart(object sender, EventArgs e)
		{
			errorsListView.Items.Clear();
		}

		private void CurrentscriptObject_CompileFinished(object sender, EventArgs e)
		{
			statusBar.Panels[0].Text = Session.CurrentSession.Resources.GetResource("INFOBILDSUCCESS", "Build Successful...", "").Text;
		}

		private void CurrentscriptObject_CompileError(object sender, EventArgs e)
		{
			statusBar.Panels[0].Text = Session.CurrentSession.Resources.GetResource("INFOBILDFAILURE", "Build Errors...", "").Text;
		}

		private void CurrentscriptObject_CompileOutput(object sender, MessageEventArgs e)
		{
			WriteLine(e.Message);
			Application.DoEvents();
		}

		#endregion

		#region Public
		/// <summary>
		/// Updates the semantic-related user interface controls.
		/// </summary>
		public void UpdateSemanticUI()
		{
		}

		/// <summary>
		/// Writes a line to the console textbox.
		/// </summary>
		/// <param name="text">The text to write.</param>
		public void WriteLine(string text)
		{
			// Append the text
			consoleTextBox.Text += (text + "\r\n");

			// Scroll
			consoleTextBox.SelectionStart = consoleTextBox.Text.Length - 2;
			consoleTextBox.ScrollToCaret();
		}

		private string _currentfilename;
		private int CurrentOffset;
		public void Open(string source)
		{
			consoleTextBox.Text = string.Empty;
			errorsListView.Items.Clear(); 
			editor.Document.LoadFile(source);
			editor.Document.Reparse();
			_currentfilename = source;
			ApplyReadOnlyZones();
			MergeToolStrip();
			ApplyCaretPosition();
		}

		private void ApplyCaretPosition()
		{
			if (CurrentOffset > 0)
				editor.Caret.Offset = CurrentOffset;
			CurrentOffset = 0;

		}

		private void InsertLine(int offset)
		{
			const string v = "\r\n\t\t";
			editor.Document.InsertText(DocumentModificationType.Typing, offset, v);
			CurrentOffset = editor.Caret.Offset;
		}

		private void PopulateErrorsList(CompilerResults compilerResults)
		{
			if (compilerResults.Errors.Count == 0)
				return;

			bottomTabControl.SelectedTab = errorsTabPage;

			errorsListView.BeginUpdate();
			errorsListView.Items.Clear();

			foreach (CompilerError error in compilerResults.Errors)
			{
				// Add the error to the listview					
				ListViewItem listViewItem = new ListViewItem(new string[] { error.Line.ToString(), error.Column.ToString(), error.ErrorText });
				listViewItem.Tag = error;
				errorsListView.Items.Add(listViewItem);
			}

			errorsListView.EndUpdate();
		}

		private void errorsListView_DoubleClick(object sender, EventArgs e)
		{
			if (errorsListView.SelectedItems.Count == 0)
				return;

			CompilerError error = errorsListView.SelectedItems[0].Tag as CompilerError;
			if (error == null)
				return;

			editor.SelectedView.GoToLine(error.Line);
			editor.SelectedView.Selection.MoveLeft();

			editor.Focus();
		}

		#endregion

		private void mnuCompileToFile_Click(object sender, EventArgs e)
		{
			FWBS.Common.Reg.ApplicationSetting compileRegKey = new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "CompileToFile");
			compileRegKey.SetSetting(!mnuCompileToFile.Checked);
			mnuCompileToFile.Checked = !mnuCompileToFile.Checked;
		}

		private void SetDefaultLangage(Script.ScriptLanguage? language)
		{
			if (language != null)
			{
				FWBS.OMS.Favourites favs = new Favourites("SCRIPTLANG");
				if (favs.Count > 0)
					favs.Param1(0, language.ToString());
				else
					favs.AddFavourite("Language", "", language.ToString());
				favs.Update();
			}
			else
			{
				FWBS.OMS.Favourites favs = new Favourites("SCRIPTLANG");
				if (favs.Count > 0)
				{
					favs.RemoveFavourite(0);
					favs.Update();
				}
			}

			languageCSharpMenuItem.Checked = false;
			languageVBDotNetMenuItem.Checked = false;
			languageNotSpecifiedMenuItem.Checked = false;

            string caption = Session.CurrentSession.Resources.GetResource("LANGCAP", "Scripting", "").Text;
			switch (language)
			{
				case FWBS.OMS.Script.ScriptLanguage.CSharp:
					languageCSharpMenuItem.Checked = true;
                    FWBS.OMS.UI.Windows.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("LANGSHARP", "The Default language is now C-Sharp this will take effect when you create a new Script", "").Text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				case FWBS.OMS.Script.ScriptLanguage.VB:
					languageVBDotNetMenuItem.Checked = true;
                    FWBS.OMS.UI.Windows.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("LANGBASIC", "The Default language is now Visual Basic this will take effect when you create a new Script", "").Text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				default:
                    FWBS.OMS.UI.Windows.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("LANGNOTSPEC", "The Default language is now Not Specified you will be asked to choose a language the next time you create a new script.", "").Text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
					languageNotSpecifiedMenuItem.Checked = true;
					break;
			}
		}
		
		private void languageCSharpMenuItem_Click(object sender, EventArgs e)
		{
			SetDefaultLangage(Script.ScriptLanguage.CSharp);
		}
		
		private void languageVBDotNetMenuItem_Click(object sender, EventArgs e)
		{
			SetDefaultLangage(Script.ScriptLanguage.VB);
		}

		private void languageNotSpecifiedMenuItem_Click(object sender, EventArgs e)
		{
			SetDefaultLangage(null);
		}

		public void Attach(string name)
		{
			if (name == null)
				return;

			_objects.Add(name);
		}

		public void Clear()
		{
			_objects.Clear();
		}

		public void Detach(string name)
		{
			_objects.Remove(name);
		}

		private void regenerateIntellisenseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ExecuteAppAction(AppAction.ToolsRegenerateIntellisense);
		}

		private void CompileMenuItem_Click(object sender, EventArgs e)
		{
			ExecuteAppAction(AppAction.FileCompile);
		}

        private void ManageVersionControlOptions(bool state)
        {
            this.mnuCheckin.Enabled = state;
            this.mnuCompare.Enabled = state;
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.menuStrip2.EnableHotKeys = true;
        }

        protected override void OnLeave(EventArgs e)
        {
            this.menuStrip2.EnableHotKeys = false;
            base.OnLeave(e);
        }

    }
}
