using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Core.Presentation.Factories;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.Services;
using System.Activities.Presentation.Toolbox;
using System.Activities.Presentation.Validation;
using System.Activities.Presentation.View;
using System.Activities.Statements;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Activities;
using System.ServiceModel.Activities.Presentation.Factories;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using FWBS.WF.Packaging;
using Microsoft.Win32;

namespace FWBS.OMS.Workflow.Admin
{
    // This class is used to pass a string to the caller - string value is context dependent
    internal class StringEventArgs : EventArgs
	{
		internal string Str;
		internal StringEventArgs(string str)
		{
			this.Str = str;
		}
	}

	/// <summary>
	/// Workflow Designer control
	/// </summary>
	internal partial class WFControl : UserControl, IValidationErrorService, IDisposable
	{
		private const string NEW_WORKFLOW_DIRECTORY = "T_M_P_"; // sub-directory for cached/copied assemblies new workflow with no code
		private const string STARTUP_CODE = "STARTUP"; // subdirectory for startup assemblies as well as config code in database
		private const string ROOTACTIVITY_NAME = "ActivityBuilder"; // Name of the empty ActivityBuilder - this workflow serialiser uses this name as the class name for arguments! 

		//
		// Running a workflow
		//
		private const string APPDOMAIN_NAME = "Workflow_Run";
		private const string APPDOMAIN_ASSEMBLY_TOEXECUTE = "FWBS.OMS.WFRuntime.exe";

		//
		// User Settings
		//
		// Main key
		private const string USERSET_KEYNAME = "WorkflowDesigner";
		// Sections
		private const string USERSET_LEFTCONTAINER_SECTION = "ToolboxWindow";
		private const string USERSET_RIGHTCONTAINER_SECTION = "PropertiesWindow";
		private const string USERSET_BOTTOMCONTAINER_SECTION = "StatusWindow";
		// Values
		private const string USERSET_WIDTH = "Width";
		private const string USERSET_HEIGHT = "Height";
		private const string USERSET_SELECTEDTAB = "Selected";
		private const string USERSET_AUTOHIDE_TAB1 = "AutoHideTab1";
		private const string USERSET_AUTOHIDE_TAB2 = "AutoHideTab2";

		// Default category for workflows where there is no group
		private const string WORKFLOWS_CATEGORY_DEFAULT = "Workflows";

		// Role name for modifying system items
		private const string ROLE_ADMIN = "ADMIN";

		// Raised when model changes
		internal event EventHandler<EventArgs> WorkflowModelChanged;
		// Raised when model has been saved
		internal event EventHandler<StringEventArgs> WorkflowModelSaved;
		

		private delegate void MessageDelegate(string msg); // delegate for displaying messages on UI thread

		private WorkflowDesigner workflowDesigner = null; // workflow designer control - created everytime a new workflow is to be loaded since it cannot be 'reset'

		private string workflowFileName = string.Empty; // name of file used in loading the workflow - used to delete temporary file later on
		private HashSet<string> toolboxBuiltinCategories = new HashSet<string>(); // list of builtin activity categories - used when resetting (deleting custom activities) the toolbox
		private Dictionary<string, string> toolboxStartupCategories = new Dictionary<string, string>(); // list of startup activity categories - used when deleting custom activities from the toolbox

		private HashSet<string> dllDistribution = new HashSet<string>(); // Assemblies referenced by this workflow and managed by 'Distributed Assemblies' - Property
		private HashSet<string> scriptCodeDistribution = new HashSet<string>(); // Workflow script codes referenced by this workflow - Property
		private HashSet<string> referencesDistribution = new HashSet<string>(); // Assemblies referenced by this workflow - Property

		private WorkflowXaml currentWorkflow = new WorkflowXaml(); // Object representing the workflow - includes database interface
		private WorkflowStartupAssemblies startupConfig = new WorkflowStartupAssemblies(); // Object representing the toolbox global startup configuration
		private WorkflowStartupAssemblies startupUserConfig = new WorkflowStartupAssemblies(); // Object representing the toolbox user startup configuration

		private FWBS.Logging.TraceLogger logger = FWBS.Logging.TraceSourceFactory.CreateTraceSource(FWBS.OMS.Workflow.Constants.TRACE_SOURCE_NAME, 1000); // logger to file

		private System.Threading.Thread runThread = null; // thread to run the workflow on
		private object lockObject = new object(); // lock object to stop running multiple instances of this workflow
		private bool isRunThreadAborting = false; // indicates whether Abort() has been called on the runThread

		private ExpressionEditorService expressionEditorService;	// Actipro syntax editor for expressions

		private string[] fwbsBuiltinActivityLibrarires = // FWBS libraries to be loaded as builtin
		{
			"FWBS.WF.OMS.ActivityLibrary.dll",
		};

		private bool isDirty = false; // indicator for the SAVE button
		private StoreFactorySettings reset_layout; // Restore the Layout to Defualt;

		//
		// Caching of workflow code and its xaml
		//	Used for dragging workflow to designer surface. It is updated everytime a new workflow designer is started or the workflow toolbox explicitly reset.
		//
		static private ConcurrentDictionary<string, string> wfCodeXamlCache = new ConcurrentDictionary<string, string>();

		internal WFControl()
		{
			this.logger.TraceVerbose("WFControl()");

			// no startup is loaded yet...
			this.IsStartupLoaded = false;

			InitializeComponent();
			
			// set script cache directory
			this.ScriptCacheDirectory = Global.GetCachePath().ToString() + FWBS.OMS.Workflow.Constants.CACHE_DIRECTORY + @"\";

			// Initialise XAML ActivityBuilder string - gets written to empty file
			//	NOTE: This way of getting the xaml will always ensure that it is the latest xml representation rather than a const xml string
			{
				WorkflowDesigner wfd = new WorkflowDesigner();
				ActivityBuilder ab = new ActivityBuilder();
				// It is IMPORTANT to set the 'Name' property, otherwise loading an xaml with an InArgument that has a default value will fail!
				//	This string will be output instead of '{x:Null}' for 'x:Class' attribute of the top level 'Activity'
				// This is also the string that gets displayed at the top of the designer
				ab.Name = ROOTACTIVITY_NAME;

				// Add 'System' namespace for VB expressions - this allows 'TimeSpan.FromSeconds(2)' instead of full name 'System.TimeSpan.FromSeconds(2)'!
				this.AddNamespace(ab, typeof(string));

				wfd.Load(ab);
				wfd.Flush();
				this.ActivityBuilderXaml = wfd.Text;
			}

			// Register the designer metadata to add designer support for all the built-in activities.
			//	This enables you to drop activities from the toolbox onto the original Sequence activity in the Workflow Designer
			DesignerMetadata dm = new DesignerMetadata();
			dm.Register();

			//
			// Create WorkflowDesigner
			//
			this.workflowDesigner = new WorkflowDesigner();
			this.designerBorder.Content = this.workflowDesigner.View;	// Add the designer canvas to the grid.

			// Set category items to 'collapsed' at start up
			Style newStyle = new Style(typeof(TreeViewItem), this.toolBoxCtrl.CategoryItemStyle)
			{
				Setters = { new Setter(TreeViewItem.IsExpandedProperty, false) }
			};
			this.toolBoxCtrl.CategoryItemStyle = newStyle;
			this.workflowToolBoxCtrl.CategoryItemStyle = newStyle;

			//
			// Create the PropertyGrid
			//
			propertyBorder.Content = this.workflowDesigner.PropertyInspectorView;
			
			// handle workflow designer errors
			this.workflowDesigner.Context.Services.Publish<IValidationErrorService>(this);
			// handle expression editor
			this.expressionEditorService = ExpressionEditorService.CreateExpressionEditorService();
			this.workflowDesigner.Context.Services.Publish<IExpressionEditorService>(this.expressionEditorService);
			// handle model changed
			this.workflowDesigner.ModelChanged += new EventHandler(workflowDesigner_ModelChanged);

			// set the relevant codes for config
			this.startupConfig.Code = STARTUP_CODE;
			this.startupUserConfig.Code = FWBS.OMS.Session.CurrentSession.CurrentUser.ID.ToString();

			// handle Loaded() event
			this.Loaded += new RoutedEventHandler(WFControl_Loaded);

			this.logger.TraceVerbose("WFControl() - End");
		}

		private string ActivityBuilderXaml { get; set; } // Xaml for empty workflow
		private string AssemblyCacheDirectory { get; set; } // Directory name for this workflow's assemblies
		private string AssemblyCacheStartupDirectory { get; set; } // Directory name for this startup assemblies
		private string ScriptCacheDirectory { get; set; } // Root Directory to put scripts in
		private bool IsStartupLoaded { get; set; } // Indicates whether the builtin and startup assemblies have been loaded and set in Toolbox - one time initial use

		// Indicates whether the workflow is being executed
		internal bool IsWorkflowRunning
		{
			get { return ((this.runThread != null) && this.runThread.IsAlive); }
		}

		// Indicates whether there is a validation error with the workflow
		internal bool IsValidationError
		{
			get { return (this.textBoxErrors != null ? this.textBoxErrors.Text.Trim().Length > 0 : true); }
		}

		internal HashSet<string> Distribution
		{
			get
			{
				return this.dllDistribution;
			}
			set
			{
				if (value == null)
				{
					value = new HashSet<string>();
				}
				this.dllDistribution = value;
			}
		}

		internal HashSet<string> References
		{
			get
			{
				return this.referencesDistribution;
			}
			set
			{
				if (value == null)
				{
					value = new HashSet<string>();
				}
				this.referencesDistribution = value;
			}
		}

		internal HashSet<string> ScriptCodes
		{
			get
			{
				return this.scriptCodeDistribution;
			}
			set
			{
				if (value == null)
				{
					value = new HashSet<string>();
				}
				this.scriptCodeDistribution = value;
			}
		}
		
		

		//
		// It may be important to realise that the Load event may be called more than once in certain scenarios.
		//	For example the same control on two different tabs of a tabcontrol will have its Load event called twice!
		//	The benefit of using Load event is that the control is fully initialised and ready to go ...
		private void WFControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.logger.TraceVerbose("WFControl_Loaded()");

			reset_layout = new StoreFactorySettings(this);
			FWBS.Configuration.RegistryConfig config = new Configuration.RegistryConfig(USERSET_KEYNAME);
			if (config.HasSection(USERSET_LEFTCONTAINER_SECTION))
			{
				this.leftContainer.Width = config.GetValue(USERSET_LEFTCONTAINER_SECTION, USERSET_WIDTH, this.leftContainer.Width);
				if (config.GetValue(USERSET_LEFTCONTAINER_SECTION, USERSET_SELECTEDTAB, 0) == 0)
				{
					this.leftContainerTab1.IsSelected = true;
				}
				else
				{
					this.leftContainerTab2.IsSelected = true;
				}
				this.leftContainerTab1.IsPinned = !config.GetValue(USERSET_LEFTCONTAINER_SECTION, USERSET_AUTOHIDE_TAB1, !this.leftContainerTab1.IsPinned);
				this.leftContainerTab2.IsPinned = !config.GetValue(USERSET_LEFTCONTAINER_SECTION, USERSET_AUTOHIDE_TAB2, !this.leftContainerTab2.IsPinned);
			}

			if (config.HasSection(USERSET_RIGHTCONTAINER_SECTION))
			{
				this.rightContainer.Width = config.GetValue(USERSET_RIGHTCONTAINER_SECTION, USERSET_WIDTH, this.rightContainer.Width);
				if (config.GetValue(USERSET_RIGHTCONTAINER_SECTION, USERSET_SELECTEDTAB, 0) == 0)
				{
					this.rightContainerTab1.IsSelected = true;
				}
				else
				{
					this.rightContainerTab2.IsSelected = true;
				}
				this.rightContainerTab2.IsPinned = !config.GetValue(USERSET_RIGHTCONTAINER_SECTION, USERSET_AUTOHIDE_TAB2, !this.rightContainerTab2.IsPinned);
			}

			if (config.HasSection(USERSET_BOTTOMCONTAINER_SECTION))
			{
				this.bottomContainer.Height = config.GetValue(USERSET_BOTTOMCONTAINER_SECTION, USERSET_HEIGHT, this.bottomContainer.Height);
				this.bottomContainerTab1.IsPinned = !config.GetValue(USERSET_BOTTOMCONTAINER_SECTION, USERSET_AUTOHIDE_TAB1, !this.bottomContainerTab1.IsPinned);
			}

			RoutedCommand runRoutedCommand = new RoutedCommand();
			this.menuItemSave.Command = runRoutedCommand;
			this.CommandBindings.Add(new CommandBinding(runRoutedCommand, SaveCmdExecuted, SaveCmdCanExecute));

			runRoutedCommand = new RoutedCommand();
			this.menuItemClear.Command = runRoutedCommand;
			this.CommandBindings.Add(new CommandBinding(runRoutedCommand, ClearCmdExecuted, ClearCmdCanExecute));

			runRoutedCommand = new RoutedCommand();
			this.menuItemClose.Command = runRoutedCommand;
			this.CommandBindings.Add(new CommandBinding(runRoutedCommand, CloseCmdExecuted, CloseCmdCanExecute));

			runRoutedCommand = new RoutedCommand();
			this.menuItemLoadFromFile.Command = runRoutedCommand;
			this.CommandBindings.Add(new CommandBinding(runRoutedCommand, LoadFromFileCmdExecuted, LoadFromFileCmdCanExecute));

			runRoutedCommand = new RoutedCommand();
			this.menuItemSaveToFile.Command = runRoutedCommand;
			this.CommandBindings.Add(new CommandBinding(runRoutedCommand, SaveToFileCmdExecuted, SaveToFileCmdCanExecute));

			runRoutedCommand = new RoutedCommand();
			this.menuItemRun.Command = runRoutedCommand;
			this.CommandBindings.Add(new CommandBinding(runRoutedCommand, RunCmdExecuted, RunCmdCanExecute));

			runRoutedCommand = new RoutedCommand();
			this.menuItemStop.Command = runRoutedCommand;
			this.CommandBindings.Add(new CommandBinding(runRoutedCommand, StopCmdExecuted, StopCmdCanExecute));

			runRoutedCommand = new RoutedCommand();
			this.menuItemRemoveSettings.Command = runRoutedCommand;
			this.CommandBindings.Add(new CommandBinding(runRoutedCommand, RemoveSettingsCmdExecuted, RemoveSettingsCmdCanExecute));

			// main menu
			ContextMenu contextMenu = new ContextMenu();
			// Choose Items menu item
			MenuItem menuItem = new MenuItem();
			menuItem.Header = "Choose Items...";
			runRoutedCommand = new RoutedCommand();
			menuItem.Command = runRoutedCommand;
			menuItem.CommandBindings.Add(new CommandBinding(runRoutedCommand, this.ChooseItemsCmdExecuted, this.ChooseItemsCmdCanExecute));
			contextMenu.Items.Add(menuItem);

			// Reset Toolbox menu item
			menuItem = new MenuItem();
			menuItem.Header = "Reset Toolbox";
			runRoutedCommand = new RoutedCommand();
			menuItem.Command = runRoutedCommand;
			menuItem.CommandBindings.Add(new CommandBinding(runRoutedCommand, this.ResetToolboxCmdExecuted, this.ResetToolboxCmdCanExecute));
			contextMenu.Items.Add(menuItem);

			this.toolBoxCtrl.ContextMenu = contextMenu;

			// Reset Workflow Toolbox menu item
			ContextMenu contextWorkflowMenu = new ContextMenu();
			menuItem = new MenuItem();
			menuItem.Header = "Reset Workflows Toolbox";
			runRoutedCommand = new RoutedCommand();
			menuItem.Command = runRoutedCommand;
			menuItem.CommandBindings.Add(new CommandBinding(runRoutedCommand, this.ResetWorkflowToolboxCmdExecuted, this.ResetWorkflowToolboxCmdCanExecute));
			contextWorkflowMenu.Items.Add(menuItem);

			this.workflowToolBoxCtrl.ContextMenu = contextWorkflowMenu;

			

			// load startup if not loaded
			if (!this.IsStartupLoaded)
			{
				// load built-in activites
				this.LoadBuiltInActivities(this.toolBoxCtrl);
				
				// load startup activities
				this.LoadReferencedAssemblies();

				//  Load built-in workflows
				this.LoadBuiltInWorkflows();

				this.IsStartupLoaded = true;
			}

			this.logger.TraceVerbose("WFControl_Loaded() - End");
		}

		private void ReloadWorkflowDesigner()
		{
			this.logger.TraceVerbose("WFControl.ReloadWorkflowDesigner()");

			if (this.workflowDesigner != null)
			{
				this.workflowDesigner.ModelChanged -= new EventHandler(workflowDesigner_ModelChanged);
			}

			// Create a new one
			this.workflowDesigner = new WorkflowDesigner();
			this.workflowDesigner.Context.Items.GetValue<System.Activities.Presentation.Hosting.ReadOnlyState>().IsReadOnly = false;
			this.designerBorder.Content = this.workflowDesigner.View;	// Add the designer canvas.
			this.isDirty = false;

			// Create the PropertyGrid
			this.propertyBorder.Content = this.workflowDesigner.PropertyInspectorView;

			// handle errors
			this.workflowDesigner.Context.Services.Publish<IValidationErrorService>(this);
			// handle expression editor
			this.workflowDesigner.Context.Services.Publish<IExpressionEditorService>(this.expressionEditorService);
			// handle model changed
			this.workflowDesigner.ModelChanged += new EventHandler(workflowDesigner_ModelChanged);

			this.logger.TraceVerbose("WFControl.ReloadWorkflowDesigner() - End");
		}

		private void LoadWorkflowFile(string fileName)
		{
			this.logger.TraceVerbose("WFControl.LoadWorkflowFile({0})", new object[] { fileName });

			if (!string.IsNullOrEmpty(fileName))
			{
				if (File.Exists(fileName))
				{
					// load workflow
					this.workflowDesigner.Load(fileName);
					// Set readOnly to false
					this.workflowDesigner.Context.Items.GetValue<System.Activities.Presentation.Hosting.ReadOnlyState>().IsReadOnly = this.currentWorkflow.IsReadOnly;
					// save filename for deletion later
					this.workflowFileName = fileName;
					// Add 'System' namespace for VB expressions
					this.AddNamespace(this.GetRootInstance(), typeof(string));
				}
				else
				{
					FWBS.OMS.UI.Windows.ErrorBox.Show(new ApplicationException(string.Format("Cannot load workflow! Workflow file '{0}' no longer exists!", fileName)));
				}
			}

			this.logger.TraceVerbose("WFControl.LoadWorkflowFile() - End");
		}

		private void LoadWorkflowFromXaml(string xaml)
		{
			this.logger.TraceVerbose("WFControl.LoadWorkflowFromXaml()");

			if (!string.IsNullOrEmpty(xaml))
			{
				string fileName = this.GetFileName(xaml);
				this.LoadWorkflowFile(fileName);
			}

			this.logger.TraceVerbose("WFControl.LoadWorkflowFromXaml() - End");
		}

		private void NewWorkflow()
		{
			this.logger.TraceVerbose("WFControl.NewWorkflow()");

			// reset bits and pieces
			try
			{
				// create new designer
				this.ReloadWorkflowDesigner();

				// load file with an empty Activity Builder
				string fileName = this.GetFileName(this.ActivityBuilderXaml);
				this.LoadWorkflowFile(fileName);
			}
			catch (Exception ex)
			{
				this.logger.TraceError("{0} {1}", new object[] { ex.Message, ex.StackTrace });
			}

			this.logger.TraceVerbose("WFControl.NewWorkflow() - End");
		}

		private bool SaveWorkflowToFile(string fileName)
		{
			this.logger.TraceVerbose("WFControl.SaveWorkflowToFile({0})", new object[] { fileName });

			bool retValue = false;

			try
			{
				this.workflowDesigner.Save(fileName);
				retValue = true;
			}
			catch (Exception ex)
			{
				this.logger.TraceError("Exception {0} {1}", new object[] { ex.Message, ex.StackTrace });
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}

			this.logger.TraceVerbose("WFControl.SaveWorkflowToFile() - End");

			return retValue;
		}

		private string SaveWorkflowToXaml()
		{
			this.logger.TraceVerbose("WFControl.SaveWorkflowToXaml()");

			string retValue = string.Empty;

			// Save any work on screen - any exceptions are passed on to the caller of this property...
			this.workflowDesigner.Flush();
			retValue = this.workflowDesigner.Text;

			this.logger.TraceVerbose("WFControl.SaveWorkflowToXaml() - End");

			return retValue;
		}

		private void LoadBuiltInActivities(ToolboxControl ctrl)
		{
			this.logger.TraceVerbose("WFControl.LoadBuiltInActivities()");

			ToolboxCategory category = new ToolboxCategory("Control Flow");
			category.Add(new ToolboxItemWrapper(typeof(DoWhile)));
			category.Add(new ToolboxItemWrapper(typeof(ForEachWithBodyFactory<>), "ForEach"));
			category.Add(new ToolboxItemWrapper(typeof(If)));
			category.Add(new ToolboxItemWrapper(typeof(Parallel)));
			category.Add(new ToolboxItemWrapper(typeof(ParallelForEachWithBodyFactory<>), "ParallelForEach"));
			category.Add(new ToolboxItemWrapper(typeof(Pick)));
			category.Add(new ToolboxItemWrapper(typeof(PickBranch)));
			category.Add(new ToolboxItemWrapper(typeof(Sequence)));
			category.Add(new ToolboxItemWrapper(typeof(Switch<>), "Switch"));
			category.Add(new ToolboxItemWrapper(typeof(While)));

			// Add the category to the ToolBox control.
			ctrl.Categories.Add(category);
			this.toolboxBuiltinCategories.Add(category.CategoryName);

			category = new ToolboxCategory("Flowchart");
			category.Add(new ToolboxItemWrapper(typeof(Flowchart)));
			category.Add(new ToolboxItemWrapper(typeof(FlowDecision)));
			category.Add(new ToolboxItemWrapper(typeof(FlowSwitch<>), "FlowSwitch"));

			ctrl.Categories.Add(category);		// Add the category to the ToolBox control.
			this.toolboxBuiltinCategories.Add(category.CategoryName);

			// StateMachine is in Platform Update 1 - if it is not installed on machine then we cannot find the type
			//	If we find it then load ...
			try
			{
				Type type;
				type = FWBS.OMS.Session.CurrentSession.TypeManager.TryLoad("System.Activities.Statements.StateMachine, System.Activities");
				if (type != null)
				{
					category = new ToolboxCategory("State Machine");
					category.Add(new ToolboxItemWrapper(type));
					type = FWBS.OMS.Session.CurrentSession.TypeManager.TryLoad("System.Activities.Statements.State, System.Activities");
					category.Add(new ToolboxItemWrapper(type));
					type = FWBS.OMS.Session.CurrentSession.TypeManager.TryLoad("System.Activities.Core.Presentation.FinalState, System.Activities.Core.Presentation");
					category.Add(new ToolboxItemWrapper(type));

					ctrl.Categories.Add(category);		// Add the category to the ToolBox control.
					this.toolboxBuiltinCategories.Add(category.CategoryName);
				}
			}
			catch (Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}

			category = new ToolboxCategory("Messaging");
			category.Add(new ToolboxItemWrapper(typeof(CorrelationScope)));
			category.Add(new ToolboxItemWrapper(typeof(InitializeCorrelation)));
			category.Add(new ToolboxItemWrapper(typeof(Receive)));
			category.Add(new ToolboxItemWrapper(typeof(ReceiveAndSendReplyFactory)));
			category.Add(new ToolboxItemWrapper(typeof(Send)));
			category.Add(new ToolboxItemWrapper(typeof(SendAndReceiveReplyFactory)));
			category.Add(new ToolboxItemWrapper(typeof(TransactedReceiveScope)));

			ctrl.Categories.Add(category);		// Add the category to the ToolBox control.
			this.toolboxBuiltinCategories.Add(category.CategoryName);

			category = new ToolboxCategory("Primitives");
			category.Add(new ToolboxItemWrapper(typeof(Assign)));
			category.Add(new ToolboxItemWrapper(typeof(Delay)));
			category.Add(new ToolboxItemWrapper(typeof(InvokeMethod)));
			category.Add(new ToolboxItemWrapper(typeof(WriteLine)));

			// Add the category to the ToolBox control.
			ctrl.Categories.Add(category);
			this.toolboxBuiltinCategories.Add(category.CategoryName);

			category = new ToolboxCategory("Runtime");
			category.Add(new ToolboxItemWrapper(typeof(Persist)));
			category.Add(new ToolboxItemWrapper(typeof(TerminateWorkflow)));

			// Add the category to the ToolBox control.
			ctrl.Categories.Add(category);
			this.toolboxBuiltinCategories.Add(category.CategoryName);

			category = new ToolboxCategory("Transaction");
			category.Add(new ToolboxItemWrapper(typeof(CancellationScope)));
			category.Add(new ToolboxItemWrapper(typeof(CompensableActivity)));
			category.Add(new ToolboxItemWrapper(typeof(Compensate)));
			category.Add(new ToolboxItemWrapper(typeof(Confirm)));
			category.Add(new ToolboxItemWrapper(typeof(TransactionScope)));

			// Add the category to the ToolBox control.
			ctrl.Categories.Add(category);
			this.toolboxBuiltinCategories.Add(category.CategoryName);

			category = new ToolboxCategory("Collection");
			category.Add(new ToolboxItemWrapper(typeof(AddToCollection<>), "AddToCollection"));
			category.Add(new ToolboxItemWrapper(typeof(ClearCollection<>), "ClearCollection"));
			category.Add(new ToolboxItemWrapper(typeof(ExistsInCollection<>), "ExistsInCollection"));
			category.Add(new ToolboxItemWrapper(typeof(RemoveFromCollection<>), "RemoveFromCollection"));

			// Add the category to the ToolBox control.
			ctrl.Categories.Add(category);
			this.toolboxBuiltinCategories.Add(category.CategoryName);

			category = new ToolboxCategory("Error Handling");
			category.Add(new ToolboxItemWrapper(typeof(Rethrow)));
			category.Add(new ToolboxItemWrapper(typeof(Throw)));
			category.Add(new ToolboxItemWrapper(typeof(TryCatch)));

			// Add the category to the ToolBox control.
			ctrl.Categories.Add(category);
			this.toolboxBuiltinCategories.Add(category.CategoryName);

			FileInfo fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
			foreach (string library in this.fwbsBuiltinActivityLibrarires)
			{
				string filePath = fileInfo.DirectoryName + @"\" + library;
				if (this.LoadActivitiesFromFile(filePath))
				{
					// get the assembly just loaded
					var assemblies = AppDomain.CurrentDomain.GetAssemblies().Select(x => x).Where(x => !x.IsDynamic && (x.Location == filePath));
					foreach (Assembly ass in assemblies)
					{
						// save its category name in the built-in so that 'Reset Toolbox' won't remove it
						this.toolboxBuiltinCategories.Add(this.MakeCategoryName(ass.GetName()));
					}
				}
			}
			

			this.logger.TraceVerbose("WFControl.LoadBuiltInActivities() - End");
		}

		private bool LoadBuiltInWorkflows()
		{
			// get available workflows
			FWBS.WF.Packaging.WorkflowXaml wfXaml = new WF.Packaging.WorkflowXaml();
			DataTable dt = wfXaml.GetToolboxList();

			// setup lookup dictionary 1st
			Dictionary<string, ToolboxCategory> categories = new Dictionary<string, ToolboxCategory>();
			foreach(ToolboxCategory category in  this.workflowToolBoxCtrl.Categories)
			{
				categories.Add(category.CategoryName, category);
			}

			// iterate
			foreach (DataRow row in dt.Rows)
			{
				string catName = row[WorkflowXaml.GroupColumnName].ToString();

				CodeLookupDisplay group = new CodeLookupDisplay(Constants.CODELOOKUP_GROUPTYPE);
				group.Code = catName;
				catName = group.Description;

				ToolboxCategory category = null;

				// set category name based on group
				if (string.IsNullOrWhiteSpace(catName))
				{
					catName = WORKFLOWS_CATEGORY_DEFAULT;
				}

				if (categories.ContainsKey(catName))
				{
					// exists
					category = categories[catName];
				}
				else
				{
					// create new category
					category = new ToolboxCategory(catName);
					categories[catName] = category;
					this.workflowToolBoxCtrl.Categories.Add(category);
				}

				// add workflow to category
				string code = row[WorkflowXaml.CodeColumnName].ToString();
				ToolboxItemWrapper tool = new ToolboxItemWrapper(typeof(WorkflowDesignActivity), null, code);
				category.Add(tool);

				// add/update to cache
				wfCodeXamlCache[code] = row[WorkflowXaml.XamlColumnName].ToString();
			}

			this.workflowToolBoxCtrl.ToolSelected += new RoutedEventHandler(workflowToolBoxCtrl_ToolSelected);

			wfXaml.Dispose();
			return true;
		}

		void workflowToolBoxCtrl_ToolSelected(object sender, RoutedEventArgs e)
		{
			var designer = sender as ToolboxControl;

			// display name is the workflow code
			// there should only be a single instance
			if (this.workflowDesigner.Context.Items.Contains<ContextItemWorkflow>())
			{
				// found it, set properties
				ContextItemWorkflow ciw = this.workflowDesigner.Context.Items.GetValue<ContextItemWorkflow>();
				ciw.Code = designer.SelectedTool.DisplayName;
				ciw.Xaml = wfCodeXamlCache[ciw.Code];
			}
			else
			{
				// does not exist, create a new one
				string code = designer.SelectedTool.DisplayName;
				this.workflowDesigner.Context.Items.SetValue(new ContextItemWorkflow(code, wfCodeXamlCache[code]));
			}
		}

		private bool LoadActivitiesFromFile(string filePath, string scriptCode = null)
		{
			this.logger.TraceVerbose("WFControl.LoadActivitiesFromFile({0})", new object[] { filePath });

			bool retValue = false;

			try
			{
				// load file
				Assembly ass = this.LoadFromOrLoadAssembly(filePath);
				if (ass != null)
				{
					// load any activities
					this.LoadActivitiesFromAssembly(ass, scriptCode);
					retValue = true;
				}
				else
				{
					// TODO: Code lookup for globalization
					string errMsg = string.Format("Failed to load activities from '{0}'", filePath);
					this.logger.TraceError(errMsg);
					FWBS.OMS.UI.Windows.MessageBox.Show(errMsg);
				}
			}
			catch (Exception ex)
			{
				this.logger.TraceError("Exception {0} {1}", new object[] { ex.Message, ex.StackTrace });
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}

			this.logger.TraceVerbose("WFControl.LoadActivitiesFromFile() - End");

			return retValue;
		}

		private void LoadActivitiesFromAssembly(Assembly ass, string scriptCode)
		{
			this.logger.TraceVerbose("WFControl.LoadActivitiesFromAssembly({0})", new object[] { ass.FullName });

			// Check whether this is already loaded as a category
			string categoryName = string.IsNullOrWhiteSpace(scriptCode) ? this.MakeCategoryName(ass.GetName()) : this.MakeCategoryName(scriptCode, ass.GetName());
			bool alreadyLoaded = false;

			for (int i = 0; i < this.toolBoxCtrl.Categories.Count; i++)
			{
				if (string.Compare(this.toolBoxCtrl.Categories[i].CategoryName, categoryName, true) == 0)
				{
					alreadyLoaded = true;
					break;
				}
			}

			if (alreadyLoaded == false)
			{
				ToolboxCategory category = null;
				string assName = ass.GetName().Name;		// assembly short name - used in stripping namespace from activity name further down...

				// Extract custom activities - must be public AND non-abstract!
				var orderedList = from type in ass.GetTypes().Where(t => (t.IsSubclassOf(typeof(System.Activities.Activity)) || (t.GetInterface("System.Activities.Presentation.IActivityTemplateFactory") != null)) && t.IsPublic && !t.IsAbstract && !t.IsNested && !t.ContainsGenericParameters)
										orderby type.Name ascending
										select type;

				foreach (Type type in orderedList)
				{
					if (category == null)
					{
						category = new ToolboxCategory(categoryName);
					}

					string itemName = type.FullName;
					if (type.FullName.StartsWith(assName + "."))
					{
						itemName = itemName.Replace(assName + ".", string.Empty);
					}
					else if (type.FullName.StartsWith(assName))
					{
						itemName = itemName.Replace(assName, string.Empty);
					}

					ToolboxItemWrapper tool = new ToolboxItemWrapper(type, null, itemName);

					//  Get activity description as the Tooltip                    
					Attribute[] attrs = Attribute.GetCustomAttributes(type);
					string tooltipName = itemName;

					foreach (Attribute att in attrs)
					{
						if (att is DescriptionAttribute)
						{
							DescriptionAttribute attribute = (DescriptionAttribute)att;
							if (attribute != null)
							{
								tooltipName = attribute.Description;
							}
						}
					}

					ToolTipConverter.ToolTipDic.Add(tool, tooltipName);
					category.Add(tool);
				}

				if (category != null)
				{
					this.toolBoxCtrl.Categories.Add(category);
				}
			}

			this.logger.TraceVerbose("WFControl.LoadActivitiesFromAssembly() - End");
		}

		private string MakeCategoryName(AssemblyName assName)
		{
			if (assName == null)
			{
				throw (new ArgumentNullException());
			}

			return string.Format("{0} ({1})", assName.Name, assName.Version.ToString());
		}

		private string MakeCategoryName(string scriptCode, AssemblyName assName)
		{
			if (assName == null)
			{
				throw (new ArgumentNullException());
			}

			return string.Format("{0} ({1})", scriptCode, assName.Version.ToString());
		}

		public void ShowValidationErrors(IList<ValidationErrorInfo> errors)
		{
			this.textBoxErrors.Clear();
			foreach (var error in errors)
			{
				this.textBoxErrors.AppendText(error.Message + Environment.NewLine);
			}
		}

		private void workflowDesigner_ModelChanged(object sender, EventArgs e)
		{
			this.isDirty = true;
			this.OnWorkflowModelChanged();
		}

		protected virtual void OnWorkflowModelChanged()
		{
			if (this.WorkflowModelChanged != null)
			{
				this.WorkflowModelChanged(this, new EventArgs());
			}
		}

		private void AsyncCompleted(IAsyncResult asyncResult)
		{
		}

		void ChooseItemsCmdExecuted(object target, ExecutedRoutedEventArgs e)
		{
			this.logger.TraceVerbose("WFControl.ChooseItemsCmdExecuted()");

			ReferencesWindow dlg = new ReferencesWindow(this.startupConfig, this.startupUserConfig);
			if (dlg.ShowDialog() == true)
			{
				HashSet<string> distributions = dlg.Distribution;
				HashSet<string> references = dlg.References;
				HashSet<string> scriptCodes = dlg.Scripts;
				HashSet<string> sysDistributions = dlg.SysDistribution;
				HashSet<string> sysReferences = dlg.SysReferences;
				HashSet<string> sysScriptCodes = dlg.SysScripts;
				HashSet<string> removedItems = dlg.RemovedItems;

				bool isChanged = false;
				if (!sysDistributions.SetEquals(this.startupConfig.GetDistributions()))
				{
					this.startupConfig.SetDistribution(sysDistributions);
					isChanged = true;
				}
				if (!sysReferences.SetEquals(this.startupConfig.GetReferences()))
				{
					this.startupConfig.SetReferences(sysReferences);
					isChanged = true;
				}
				if (!sysScriptCodes.SetEquals(this.startupConfig.GetScriptCodes()))
				{
					this.startupConfig.SetScriptCodes(sysScriptCodes);
					isChanged = true;
				}
				if (isChanged)
				{
					this.startupConfig.Update();
				}

				isChanged = false;
				if (!distributions.SetEquals(this.startupUserConfig.GetDistributions()))
				{
					this.startupUserConfig.SetDistribution(distributions);
					isChanged = true;
				}
				if (!references.SetEquals(this.startupUserConfig.GetReferences()))
				{
					this.startupUserConfig.SetReferences(references);
					isChanged = true;
				}
				if (!scriptCodes.SetEquals(this.startupUserConfig.GetScriptCodes()))
				{
					this.startupUserConfig.SetScriptCodes(scriptCodes);
					isChanged = true;
				}
				if (isChanged)
				{
					this.startupUserConfig.Update();
				}

				// iterate through removed items
				foreach (string str in removedItems)
				{
					// get category name
					string categoryName = null;
					if (this.toolboxStartupCategories.TryGetValue(str, out categoryName))
					{
						// iterate toolbox categories to find it
						for (int j = 0; j < this.toolBoxCtrl.Categories.Count; j++)
						{
							if (string.Compare(this.toolBoxCtrl.Categories[j].CategoryName, categoryName, true) == 0)
							{
								// found it, remove from toolbox
								this.toolBoxCtrl.Categories.Remove(this.toolBoxCtrl.Categories[j]);
								break;
							}
						}
						// remove from dictionary
						this.toolboxStartupCategories.Remove(str);
					}
				}

				this.LoadReferencedAssembliesHelper(this.startupConfig, this.AssemblyCacheStartupDirectory);
				this.LoadScriptAssemblies(sysScriptCodes);
				this.LoadReferencedAssembliesHelper(this.startupUserConfig, this.AssemblyCacheStartupDirectory);
				this.LoadScriptAssemblies(scriptCodes);
				
			}

			e.Handled = true;

			this.logger.TraceVerbose("WFControl.ChooseItemsCmdExecuted() - End");
		}

		void ChooseItemsCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			// can add anytime except while running
			e.CanExecute = !this.IsWorkflowRunning;
		}

		void ResetToolboxCmdExecuted(object target, ExecutedRoutedEventArgs e)
		{
			this.logger.TraceVerbose("WFControl.ResetToolboxCmdExecuted()");

			bool reset = MessageBox.Show("Are you sure to reset the Toolbox?\n\nAny activities other than builtin activities will NOT be available next time OMS AdminKit is started!\n\nIf you are an ADMIN, 'System' activities will not be available for ALL users!", "Reset Toolbox", MessageBoxButton.YesNo) == MessageBoxResult.Yes;

			if (reset)
			{
				// clear startup/workflow dictionary maps
				this.toolboxStartupCategories.Clear();

				// iterate top down so that the deleted item does not interfere with the iteration
				for (int i = this.toolBoxCtrl.Categories.Count; i > 0; i--)
				{
					if (!this.toolboxBuiltinCategories.Contains(this.toolBoxCtrl.Categories[i - 1].CategoryName))
					{
						// clear the items
						this.toolBoxCtrl.Categories[i - 1].Tools.Clear();
						// remove the category - note that the assembly will still be loaded!
						this.toolBoxCtrl.Categories.Remove(this.toolBoxCtrl.Categories[i - 1]);
					}
				}

				HashSet<string> emptyList = new HashSet<string>();
				this.startupUserConfig.SetDistribution(emptyList);
				this.startupUserConfig.SetReferences(emptyList);
				this.startupUserConfig.SetScriptCodes(emptyList);
				this.startupUserConfig.Update();

				// check whether user has privilige to manipulate system/global items
				string[] roles = FWBS.OMS.Session.CurrentSession.CurrentUser.Roles.Split(',');
				foreach (string str in roles)
				{
					if (str == ROLE_ADMIN)
					{
						this.startupConfig.SetDistribution(emptyList);
						this.startupConfig.SetReferences(emptyList);
						this.startupConfig.SetScriptCodes(emptyList);
						this.startupConfig.Update();
						break;
					}
				}
			}

			this.logger.TraceVerbose("WFControl.ResetToolboxCmdExecuted() - End");
		}

		void ResetToolboxCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			// can add anytime except while running
			e.CanExecute = !this.IsWorkflowRunning;
		}

		void ResetWorkflowToolboxCmdExecuted(object target, ExecutedRoutedEventArgs e)
		{
			this.logger.TraceVerbose("WFControl.ResetWorkflowToolboxCmdExecuted()");

			bool reset = MessageBox.Show("Are you sure to reset the Workflow Toolbox?", "Reset Workflow Toolbox", MessageBoxButton.YesNo) == MessageBoxResult.Yes;

			if (reset)
			{
				this.workflowToolBoxCtrl.Categories.Clear();
				this.LoadBuiltInWorkflows();
			}

			this.logger.TraceVerbose("WFControl.ResetWorkflowToolboxCmdExecuted() - End");
		}

		void ResetWorkflowToolboxCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			// can add anytime except while running
			e.CanExecute = !this.IsWorkflowRunning;
		}

		private void Message(string msg)
		{
			FWBS.OMS.UI.Windows.MessageBox.Show(msg);
		}

		private object GetRootInstance()
		{
			this.logger.TraceVerbose("WFControl.GetRootInstance()");

			object retValue = null;

			ModelService modelService = this.workflowDesigner.Context.Services.GetService<ModelService>();
			if (modelService != null)
			{
				retValue = modelService.Root.GetCurrentValue();
			}

			if (retValue == null)
			{
				this.logger.TraceWarning("WFControl.GetRootInstance - Returning (null)");
			}

			this.logger.TraceVerbose("WFControl.GetRootInstance() - End");

			return retValue;
		}

		private string GetFileName(string fileContent)
		{
			// "this.AssemblyCacheDirectory" is set early to the cache directory
			DirectoryInfo dirInfo = new DirectoryInfo(this.AssemblyCacheDirectory);
			string fileName = dirInfo.FullName + "/" + dirInfo.Name + ".xaml";
			try
			{
				// write the contents
				if (!string.IsNullOrEmpty(fileContent))
				{
					StreamWriter sw = File.CreateText(fileName);
					sw.Write(fileContent);
					sw.Close();
				}
			}
			catch (Exception ex)
			{
				this.logger.TraceError("Exception {0} {1}", new object[] { ex.Message, ex.StackTrace });
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}

			return fileName;
		}

		private void AddNamespace(object activity, Type type)
		{
			/*
			 * The way to get all distinct namespaces within an assembly
				Assembly.GetTypes().Select(t => t.Namespace).Distinct(); 
			 */

			string assName = type.Assembly.GetName().Name;
			string ns = type.Namespace;

			Microsoft.VisualBasic.Activities.VisualBasicSettings vbSettings = Microsoft.VisualBasic.Activities.VisualBasic.GetSettings(activity);

			if (vbSettings == null)
			{
				vbSettings = new Microsoft.VisualBasic.Activities.VisualBasicSettings();
				vbSettings.ImportReferences.Add(new Microsoft.VisualBasic.Activities.VisualBasicImportReference
				{
					Assembly = assName,
					Import = ns,
				});
				Microsoft.VisualBasic.Activities.VisualBasic.SetSettings(activity, vbSettings);
			}
			else
			{
				vbSettings.ImportReferences.Add(new Microsoft.VisualBasic.Activities.VisualBasicImportReference
				{
					Assembly = assName,
					Import = ns,
				});
			}
		}

		private void LoadReferencedAssemblies()
		{
			this.logger.TraceVerbose("WFControl.LoadReferencedAssemblies()");

			string dirPath = this.ScriptCacheDirectory + STARTUP_CODE;
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}
			this.AssemblyCacheStartupDirectory = dirPath;

			// check whether the entry exists
			if (this.startupConfig.Exists(STARTUP_CODE))
			{
				this.startupConfig.Fetch(STARTUP_CODE);
				this.LoadReferencedAssembliesHelper(this.startupConfig, dirPath);
				this.LoadScriptAssemblies(this.startupConfig.GetScriptCodes());
			}
			this.logger.TraceVerbose("WFControl.LoadReferencedAssemblies - loaded STARTUP");

			string userStartupCode = FWBS.OMS.Session.CurrentSession.CurrentUser.ID.ToString();
			if (this.startupUserConfig.Exists(userStartupCode))
			{
				this.startupUserConfig.Fetch(userStartupCode);
				this.LoadReferencedAssembliesHelper(this.startupUserConfig, dirPath);
				this.LoadScriptAssemblies(this.startupUserConfig.GetScriptCodes());
			}
			this.logger.TraceVerbose("WFControl.LoadReferencedAssemblies - loaded USER STARTUP");

			this.logger.TraceVerbose("WFControl.LoadReferencedAssemblies() - End");
		}

		private void LoadReferencedAssembliesHelper(WorkflowStartupAssemblies startup, string dirPath)
		{
			this.logger.TraceVerbose("WFControl.LoadReferencedAssembliesHelper()");

			// Extract DA Assemblies 
			startup.DistributedErrors += new EventHandler<DistributedEventArgs>(currentWorkflow_DistributedErrors);
			startup.ExtractDistribution();
			startup.DistributedErrors -= new EventHandler<DistributedEventArgs>(currentWorkflow_DistributedErrors);

			// for each referenced distributed assembly ...
			this.toolboxStartupCategories.Clear();
			foreach (string str in startup.GetDistributions())
			{
				this.logger.TraceVerbose("WFControl.LoadReferencedAssembliesHelper - Processing {0}", new object[] { str });

				// form the full filename we expect for a distributed assembly
				FileInfo fileInfo = new FileInfo(str);
				string destFileName = string.Format(@"{0}\{1}", FWBS.OMS.Session.CurrentSession.DistributedAssemblyManager.DistributedAssembliesDirectory, fileInfo.Name);

				// load
				if (this.LoadActivitiesFromFile(destFileName))
				{
					// save distribution name with the category name
					Assembly ass = this.LoadFromOrLoadAssembly(destFileName);	// should be loaded already
					string categoryName = this.MakeCategoryName(ass.GetName());
					this.toolboxStartupCategories.Add(str, categoryName);
				}
			}
			// load referenced assemblies
			foreach (string str in startup.GetReferences())
			{
				this.logger.TraceVerbose("WFControl.LoadReferencedAssembliesHelper - Processing {0}", new object[] { str });

				// form a FileInfo class to extract the filename
				FileInfo fileInfo1 = new FileInfo(Assembly.GetExecutingAssembly().Location);
				FileInfo fileInfo = new FileInfo(fileInfo1.DirectoryName + @"\" + str);
				// load
				if (this.LoadActivitiesFromFile(fileInfo.FullName))
				{
					// save distribution name with the category name
					Assembly ass = this.LoadFromOrLoadAssembly(fileInfo.FullName);	// should be loaded already
					string categoryName = this.MakeCategoryName(ass.GetName());
					this.toolboxStartupCategories.Add(str, categoryName);
				}
			}

			this.LoadScriptAssemblies(startup.GetScriptCodes());

			this.logger.TraceVerbose("WFControl.LoadReferencedAssembliesHelper() - End");
		}

		private void LoadDistributedAssemblies(HashSet<string> assemblies)
		{
			this.logger.TraceVerbose("WFControl.LoadDistributedAssemblies()");

			// for each distributed assembly ...
			foreach (string str in assemblies)
			{
				this.logger.TraceVerbose("WFControl.LoadDistributedAssemblies - Processing {0}", new object[] { str });

				// form the full filename we expect
				FileInfo fileInfo = new FileInfo(str);
				// NOTE: The path format depends on source file DistributedAssemblyManager.cs
				// It is used in WFRuntime.cs, LoadWorkflow.cs and WFControl.xaml.cs
				string destFileName = Path.Combine(FWBS.OMS.Session.CurrentSession.DistributedAssemblyManager.DistributedAssembliesDirectory.FullName, fileInfo.Name, fileInfo.Name);

				if (this.LoadActivitiesFromFile(destFileName))
				{
					// save distribution name with the category name
					Assembly ass = this.LoadFromOrLoadAssembly(destFileName);		// should be loaded already
					string categoryName = this.MakeCategoryName(ass.GetName());
					this.toolboxStartupCategories[str] = categoryName;
					this.logger.TraceVerbose("WFControl.LoadDistributedAssemblies - Added {0} to toolbox", new object[] { categoryName });
				}
			}

			this.logger.TraceVerbose("WFControl.LoadDistributedAssemblies() - End");
		}	

		private void LoadReferencedAssemblies(HashSet<string> assemblies)
		{
			this.logger.TraceVerbose("WFControl.LoadReferencedAssemblies()");
			FileInfo fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
			foreach (string fileName in assemblies)
			{
				// try where this assembly is 1st
				Assembly ass = FWBS.OMS.Session.CurrentSession.AssemblyManager.TryLoadFrom(fileInfo.DirectoryName + @"\" + fileName);
				if (ass == null)
				{
					// try default locations
					ass = this.LoadFromOrLoadAssembly(fileName);
				}
			}

			this.logger.TraceVerbose("WFControl.LoadReferencedAssemblies() - End");
		}

		private void LoadScriptAssemblies(HashSet<string> scriptCodes)
		{
			this.logger.TraceVerbose("WFControl.LoadScriptAssemblies()");

			foreach (string scriptCode in scriptCodes)
			{
				this.logger.TraceVerbose("WFControl.LoadScriptAssemblies - Processing {0}", new object[] { scriptCode });

				Script.ScriptGen sc = new Script.ScriptGen(scriptCode);
				sc.Load();
				if (!this.toolboxStartupCategories.ContainsKey(scriptCode))
				{
					if (this.LoadActivitiesFromFile(sc.OutputName, scriptCode))
					{
						// save script code with the category name
						Assembly ass = FWBS.OMS.Session.CurrentSession.AssemblyManager.LoadFrom(sc.OutputName);	// should be loaded already via LoadActivitiesFromFile()
						string categoryName = this.MakeCategoryName(scriptCode, ass.GetName());
						this.toolboxStartupCategories[scriptCode] = categoryName;
					}
				}
				sc.Dispose();
			}

			this.logger.TraceVerbose("WFControl.LoadScriptAssemblies() - End");
		}

		private void SaveCmdExecuted(object target, ExecutedRoutedEventArgs e)
		{
			this.isDirty = !this.UpdateData();
		}

		private void SaveCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			// enable if there is no workflow running
			e.CanExecute = this.isDirty && !(this.IsWorkflowRunning || this.IsValidationError);
		}

		private void ClearCmdExecuted(object target, ExecutedRoutedEventArgs e)
		{
			// TODO: Use codelook up to get the message
			if(MessageBox.Show("Are you sure? The Current Workflow will be cleared.", "Clear Workflow", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
				return;
			this.NewCmd();
			this.OnWorkflowModelChanged();
		}

		private void ClearCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			// enable if there is no workflow running
			e.CanExecute = !this.IsWorkflowRunning;
		}

		private void CloseCmdExecuted(object target, ExecutedRoutedEventArgs e)
		{
			Window wpfParentWnd = Window.GetWindow(this);
			if (wpfParentWnd == null)
			{
				// Hosted in Winforms?
				System.Windows.Interop.HwndSource presentationSource = (System.Windows.Interop.HwndSource)PresentationSource.FromVisual(this);
				if (presentationSource != null)
				{
					System.Windows.Forms.Control ctl = System.Windows.Forms.Control.FromChildHandle(presentationSource.Handle);
					if (ctl != null)
					{
						System.Windows.Forms.Form parentWnd = ctl.FindForm();
						if (parentWnd != null)
						{
							parentWnd.Close();
						}
					}
				}
			}
			else
			{
				// Hosted in WPF
				wpfParentWnd.Close();
			}
		}

		private void CloseCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = false;
			Window wpfParentWnd = Window.GetWindow(this);
			if (wpfParentWnd == null)
			{
				// Hosted in Winforms?
				System.Windows.Interop.HwndSource presentationSource = (System.Windows.Interop.HwndSource)PresentationSource.FromVisual(this);
				if (presentationSource != null)
				{
					System.Windows.Forms.Control ctl = System.Windows.Forms.Control.FromChildHandle(presentationSource.Handle);
					if (ctl != null)
					{
						System.Windows.Forms.Form parentWnd = ctl.FindForm();
						if (parentWnd != null)
						{
							e.CanExecute = true;
						}
					}
				}
			}
			else
			{
				// Hosted in WPF
				e.CanExecute = true;
			}
		}

		private void LoadFromFileCmdExecuted(object target, ExecutedRoutedEventArgs e)
		{
			if (this.LoadCmd())
			{
				//  Make the Save Button active
				this.isDirty = true;

				this.OnWorkflowModelChanged();
			}
		}

		private void LoadFromFileCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			// enable if there is no workflow running
			e.CanExecute = !this.IsWorkflowRunning;
		}

		private void SaveToFileCmdExecuted(object target, ExecutedRoutedEventArgs e)
		{
			this.SaveCmd();
		}

		private void SaveToFileCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			// enable if there is no workflow running
			e.CanExecute = !this.IsWorkflowRunning;
		}

		private void RunCmdExecuted(object target, ExecutedRoutedEventArgs e)
		{
			this.logger.TraceVerbose("WFControl.RunCmdExecuted()");
			try
			{
				if (!this.IsValidationError)
				{
					SplashWindow wnd = new SplashWindow("Checking" + Environment.NewLine + "environment");
					wnd.Show();
					System.Threading.Thread.Sleep(500);

					try
					{
						lock (this.lockObject)
						{
							// check if we are already running
							if ((this.runThread != null) && this.runThread.IsAlive)
							{
								FWBS.OMS.UI.Windows.MessageBox.ShowInformation("Workflow already running");
								return;
							}

							if (this.runThread == null || !this.runThread.IsAlive)
							{
								this.runThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RunWorkflow));
								this.runThread.SetApartmentState(System.Threading.ApartmentState.STA);
							}

							string xaml = this.SaveWorkflowToXaml();
							this.AddStartupToWorkflowUsingWorkflowXaml(xaml);	// update distribution/references/script property lists

							this.runThread.Start( new object[]
								{
									this.currentWorkflow.Code,
									xaml,
									new HashSet<string>(this.Distribution),		// pass copies since we will be in a different thread
									new HashSet<string>(this.References),
									new HashSet<string>(this.ScriptCodes)
								});
						}
					}
					finally
					{
						wnd.Close();
					}
				}
			}
			catch (Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}
			this.logger.TraceVerbose("WFControl.RunCmdExecuted() - End");
		}

		private void RunCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			// enable if there is no workflow running
			e.CanExecute = !(this.IsWorkflowRunning || this.IsValidationError);
		}

		private void RunWorkflow(object data)
		{
			this.logger.TraceVerbose("WFControl.RunWorkflow()");

			SplashWindow wnd = new SplashWindow("Setting up environment");
			wnd.Show();
			System.Threading.Thread.Sleep(500);

			AppDomain wfRunAppDomain = null;
			try
			{
				// NOTE: Argument count and order MUST match the call!
				object[] args = data as object[];
				string wfCode = (string)args[0];
				string wfXaml = (string)args[1];
				HashSet<string> distribution = (HashSet<string>)args[2];
				HashSet<string> references = (HashSet<string>)args[3];
				HashSet<string> scriptCodes = (HashSet<string>)args[4];

				wfRunAppDomain = AppDomain.CreateDomain(APPDOMAIN_NAME);

				// set workflow code - runs from database using workflow code if non empty
				wfRunAppDomain.SetData(FWBS.OMS.Workflow.Constants.APPDOMAIN_WFCODE, wfCode);

				wfRunAppDomain.SetData(FWBS.OMS.Workflow.Constants.APPDOMAIN_WFXAML, wfXaml);
				wfRunAppDomain.SetData(FWBS.OMS.Workflow.Constants.APPDOMAIN_WFDISTRIBUTIONS, distribution);
				wfRunAppDomain.SetData(FWBS.OMS.Workflow.Constants.APPDOMAIN_WFREFERENCES, references);
				wfRunAppDomain.SetData(FWBS.OMS.Workflow.Constants.APPDOMAIN_WFSCRIPTCODES, scriptCodes);

				wnd.Close();
				wnd = null;
				// execute - get directory from current executing assembly
				FileInfo fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
				wfRunAppDomain.ExecuteAssembly(fileInfo.DirectoryName + @"\" + APPDOMAIN_ASSEMBLY_TOEXECUTE);
			}
			catch (System.Threading.ThreadAbortException)
			{
				// abort thread
				// Unloading appdomain is brutal when the thread is aborted, there may be a better way of aborting...
				System.Threading.Thread.ResetAbort();
			}
			catch (Exception ex)
			{
				// Some error has occurred!
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}
			finally
			{
				// close splash
				if (wnd != null)
				{
					wnd.Close();
				}

				// unload appdomain
				if (wfRunAppDomain != null)
				{
					AppDomain.Unload(wfRunAppDomain);
					wfRunAppDomain = null;
				}
				// finished
				MessageDelegate md = new MessageDelegate(this.Message);
				this.Dispatcher.Invoke(md, new object[] { "Completed" });
			}

			this.isRunThreadAborting = false;
			this.logger.TraceVerbose("WFControl.RunWorkflow() - End");
		}

		private void StopCmdExecuted(object target, ExecutedRoutedEventArgs e)
		{
			if ((this.runThread != null) && (!this.isRunThreadAborting))
			{
				this.runThread.Abort();
				this.isRunThreadAborting = true;
			}
		}

		private void StopCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			// enable if there is no workflow running
			e.CanExecute = this.IsWorkflowRunning && !this.isRunThreadAborting;
		}

		private void UserSettingsCmdExecuted(object target, ExecutedRoutedEventArgs e)
		{
			this.SaveSettings();
		}

		private void UserSettingsCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		internal void SaveSettings()
		{
			FWBS.Configuration.RegistryConfig config = new Configuration.RegistryConfig(USERSET_KEYNAME);
			config.SetValue(USERSET_LEFTCONTAINER_SECTION, USERSET_WIDTH, this.leftContainer.Width);
			config.SetValue(USERSET_LEFTCONTAINER_SECTION, USERSET_HEIGHT, this.leftContainer.Height);
			config.SetValue(USERSET_LEFTCONTAINER_SECTION, USERSET_SELECTEDTAB, this.leftContainerTab1.IsSelected ? 0 : 1);
			config.SetValue(USERSET_LEFTCONTAINER_SECTION, USERSET_AUTOHIDE_TAB1, !this.leftContainerTab1.IsPinned);
			config.SetValue(USERSET_LEFTCONTAINER_SECTION, USERSET_AUTOHIDE_TAB2, !this.leftContainerTab2.IsPinned);

			config.SetValue(USERSET_RIGHTCONTAINER_SECTION, USERSET_WIDTH, this.rightContainer.Width);
			config.SetValue(USERSET_RIGHTCONTAINER_SECTION, USERSET_HEIGHT, this.rightContainer.Height);
			config.SetValue(USERSET_RIGHTCONTAINER_SECTION, USERSET_SELECTEDTAB, this.rightContainerTab1.IsSelected ? 0 : 1);
			config.SetValue(USERSET_RIGHTCONTAINER_SECTION, USERSET_AUTOHIDE_TAB1, !this.rightContainerTab1.IsPinned);
			config.SetValue(USERSET_RIGHTCONTAINER_SECTION, USERSET_AUTOHIDE_TAB2, !this.rightContainerTab2.IsPinned);

			config.SetValue(USERSET_BOTTOMCONTAINER_SECTION, USERSET_WIDTH, this.bottomContainer.Width);
			config.SetValue(USERSET_BOTTOMCONTAINER_SECTION, USERSET_HEIGHT, this.bottomContainer.Height);
			config.SetValue(USERSET_BOTTOMCONTAINER_SECTION, USERSET_SELECTEDTAB, 0);
			config.SetValue(USERSET_BOTTOMCONTAINER_SECTION, USERSET_AUTOHIDE_TAB1, !this.bottomContainerTab1.IsPinned);
		}

		private void RemoveSettingsCmdExecuted(object target, ExecutedRoutedEventArgs e)
		{
			FWBS.Configuration.RegistryConfig config = new Configuration.RegistryConfig(USERSET_KEYNAME);
			config.Remove();
			reset_layout.Restore(this);
		}

		private void RemoveSettingsCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private bool NewCmd()
		{
			this.logger.TraceVerbose("WFControl.NewCmd()");

			bool retValue = false;

			if (!this.IsWorkflowRunning)
			{
				this.NewWorkflow();
				retValue = true;
			}

			this.logger.TraceVerbose("WFControl.NewCmd() - End");

			return retValue;
		}

		private bool LoadCmd()
		{
			this.logger.TraceVerbose("WFControl.LoadCmd()");

			bool retValue = false;

			if (!this.IsWorkflowRunning)
			{
				OpenFileDialog dlg = new OpenFileDialog();
				dlg.CheckFileExists = true;
				dlg.Multiselect = false;
				dlg.DereferenceLinks = true;
				dlg.ValidateNames = true;
				dlg.Filter = "XAML Files | *.xaml";
				if (dlg.ShowDialog() == true)
				{
					this.ReloadWorkflowDesigner();
					string fileName;
					try
					{
						// Copy file to file in cache first
						fileName = this.GetFileName(string.Empty);
						File.Copy(dlg.FileName, fileName);
						this.logger.TraceVerbose("WFControl.LoadCmd() - copied {0} to {1}", new object[] { dlg.FileName, fileName });
					}
					catch (Exception ex)
					{
						// otherwise use the original!
						this.logger.TraceVerbose("Exception {0} {1}", new object[] { ex.Message, ex.StackTrace });
						fileName = dlg.FileName;
					}
					this.logger.TraceVerbose("WFControl.LoadCmd() - using {0}", new object[] { fileName });
					this.LoadWorkflowFile(fileName);

					retValue = true;
				}
			}

			this.logger.TraceVerbose("WFControl.LoadCmd() - End");

			return retValue;
		}

		private void LoadXamlCmd(string xaml)
		{
			this.logger.TraceVerbose("WFControl.LoadXamlCmd()");

			if (string.IsNullOrEmpty(xaml))
			{
				this.NewCmd();
			}
			else
			{
				// NOTE: This routine will get called before the Loaded() event and thus
				//	the startup assemblies would not have been loaded to the toolbox at this stage!
				if (!this.IsStartupLoaded)
				{
					// load built-in activites
					this.LoadBuiltInActivities(this.toolBoxCtrl);
					
					// load startup activities
					this.LoadReferencedAssemblies();

					//  Load built-in workflows
					this.LoadBuiltInWorkflows();

					this.IsStartupLoaded = true;
				}

				// Any assemblies/scripts the workflow needs should get loaded as part of the workflow...
				// Attempt to load the assemblies this workflow requires
				this.LoadDistributedAssemblies(this.Distribution);
				this.LoadReferencedAssemblies(this.References);
				this.LoadScriptAssemblies(this.ScriptCodes);

				this.ReloadWorkflowDesigner();

				// load file with the xaml as contents
				this.LoadWorkflowFromXaml(xaml);
			}

			this.logger.TraceVerbose("WFControl.LoadXamlCmd() - End");
		}

		private bool SaveCmd()
		{
			this.logger.TraceVerbose("WFControl.SaveCmd()");

			bool retValue = false;

			if (!this.IsWorkflowRunning)
			{
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.CheckFileExists = false;
				dlg.CheckPathExists = true;
				dlg.DereferenceLinks = true;
				dlg.ValidateNames = true;
				dlg.Filter = "XAML Files | *.xaml";
				if (dlg.ShowDialog() == true)
				{
					retValue = this.SaveWorkflowToFile(dlg.FileName);
				}
			}

			this.logger.TraceVerbose("WFControl.SaveCmd() - End");

			return retValue;
		}

		/// <summary>
		/// This routine adds/removes any startup 'distributed assembly' and 'scripts' to the workflow's one to ensure it will contain
		///		all the assemblies it requires - the runtime engine also uses this information.
		///	This is achieved by parsing the xaml for the assemblies used, mapping the assembly name to the short filename
		///		and finding the short filename in the startup list.
		///	String comparisons are either case-insensitive or in lower case
		/// </summary>
		/// <param name="xaml">The workflow xaml</param>
		private void AddStartupToWorkflowUsingWorkflowXaml(string xaml)
		{
			this.logger.TraceVerbose("WFControl.AddStartupToWorkflowUsingWorkflowXaml()");

			HashSet<string> scriptCodes = new HashSet<string>();

			// format of assembly name is as xml namespace -  xmlns:a1="clr-namespace:Activity2;assembly=Activity2"
			HashSet<string> assemblies = new HashSet<string>();
			string prefix = ";assembly=";
			int foundIndex = 0;

			// Extract the assembly names out of the xaml - they are namespace attributes at root xml node
			XElement rootElement = XElement.Parse(xaml);
			foreach (XAttribute attribute in rootElement.Attributes().Where(
				a => a.IsNamespaceDeclaration && a.Name.LocalName != "xmlns"))
			{
				string nm = attribute.Value;
				if (nm.StartsWith("clr-namespace"))
				{
					// ignore culture and case sensitivity!
					foundIndex = nm.IndexOf(prefix);
					if (foundIndex != -1)
					{
						int beginIndex = foundIndex + prefix.Length;
						string assName = nm.Substring(beginIndex, nm.Length - beginIndex).ToLower();
						// check if it is a script assembly
						FWBS.OMS.Script.ScriptFileName sfn;
						if (FWBS.OMS.Script.ScriptFileName.TryParse(assName, out sfn))
						{
							// it is a script assembly
							if (!scriptCodes.Contains(sfn.Name))
							{
								scriptCodes.Add(sfn.Name);
							}
						}
						else
						{
							if (!assemblies.Contains(assName))
							{
								// add if it is not already added...
								assemblies.Add(assName);
							}
						}
					}
				}
			}

			// Map the assembly name to the filename (they maybe different) - Our distributed assembly systems works on file names
			HashSet<string> fileNames = new HashSet<string>();
			foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (!ass.IsDynamic)
				{
					AssemblyName assName = ass.GetName();
					if (assemblies.Contains(assName.Name.ToLower()))
					{
						// get the filename - format is "file:///c:/OMScoreassemblies/FWBS.OMS.Workflow.DLL"
						int index = assName.CodeBase.LastIndexOf('/') + 1;
						if (index >= 0)
						{
							string fileName = assName.CodeBase.Substring(index, assName.CodeBase.Length - index).ToLower();
							if (!fileNames.Contains(fileName))
							{
								// add if it is not already added...
								fileNames.Add(fileName);
							}
						}
					}
				}
			}

			// We now have a list of the assembly filenames used within the workflow
			// check if the filename exists within the current distribution list
			HashSet<string> daToRemove = new HashSet<string>();
			foreach (string str in this.Distribution)
			{
				// form a FileInfo class to extract the filename
				FileInfo fileInfo = new FileInfo(str);

				// remove from list if it exists
				if (!fileNames.Remove(fileInfo.Name.ToLower()))
				{
					// remove from distribution list
					daToRemove.Add(str);
				}
			}
			this.Distribution.ExceptWith(daToRemove);
			// check if the filename exists within the current references list
			HashSet<string> drToRemove = new HashSet<string>();
			foreach (string str in this.References)
			{
				// form a FileInfo class to extract the filename
				FileInfo fileInfo = new FileInfo(str);

				// remove from list if it exists
				if (!fileNames.Remove(fileInfo.Name.ToLower()))
				{
					// remove from references list
					drToRemove.Add(str);
				}
			}
			this.References.ExceptWith(drToRemove);

			// check if the filename exists within the current script list
			HashSet<string> dsToRemove = new HashSet<string>();
			foreach (string str in this.ScriptCodes)
			{
				// remove from list if it exists
				if (!scriptCodes.Remove(str.ToLower()))
				{
					// remove from script list
					dsToRemove.Add(str);
				}
			}
			this.ScriptCodes.ExceptWith(dsToRemove);

			HashSet<string> daToAdd = new HashSet<string>();
			foreach (string str in this.startupConfig.GetDistributions())
			{
				// form a FileInfo class to extract the filename
				FileInfo fileInfo = new FileInfo(str);
				if (fileNames.Remove(fileInfo.Name.ToLower()))
				{
					// add to the workflow distribution
					daToAdd.Add(str);
				}
			}
			
			foreach (string str in this.startupUserConfig.GetDistributions())
			{
				// form a FileInfo class to extract the filename
				FileInfo fileInfo = new FileInfo(str);
				if (fileNames.Remove(fileInfo.Name.ToLower()))
				{
					// add to the workflow distribution
					daToAdd.Add(str);
				}
			}

			// check if it exists within the startup script list
			HashSet<string> dsToAdd = new HashSet<string>();
			foreach (string str in this.startupConfig.GetScriptCodes())
			{
				if (scriptCodes.Remove(str.ToLower()))
				{
					// add to the workflow distribution
					dsToAdd.Add(str);
				}
			}
			
			foreach (string str in this.startupUserConfig.GetScriptCodes())
			{
				if (scriptCodes.Remove(str.ToLower() + ".dll"))
				{
					// add to the workflow distribution
					dsToAdd.Add(str);
				}
			}

			fileNames.Remove("mscorlib.dll");

			// Merge any additions to the workflow references list
			// add all files left over as references
			this.References.UnionWith(fileNames);

			// Merge any additions to the workflow distribution list
			if (daToAdd.Count > 0)
			{
				this.Distribution.UnionWith(daToAdd);
			}

			// Merge any additions to the workflow script list
			if (dsToAdd.Count > 0)
			{
				this.ScriptCodes.UnionWith(dsToAdd);
			}
			if (scriptCodes.Count > 0)
			{
				this.ScriptCodes.UnionWith(scriptCodes);
			}
			

			this.logger.TraceVerbose("WFControl.AddStartupToWorkflowUsingWorkflowXaml() - End");
		}

		internal bool UpdateData()
		{
			this.logger.TraceVerbose("WFControl.UpdateData()");

			bool retValue = false;

			try
			{
				// save if we are not in an error state and the workflow is valid
				if (!(this.workflowDesigner.IsInErrorState() || this.IsValidationError))
				{
					// check whether the top activity has a name - do it via ModelItem so that it gets serialised!
					ModelItem item = this.workflowDesigner.Context.Services.GetService<ModelService>().Root;
					if (item != null)
					{
						if (string.IsNullOrWhiteSpace(item.Properties["Name"].Value.GetCurrentValue().ToString()))
						{
							// set ModelItem name
							item.Properties["Name"].SetValue(string.IsNullOrWhiteSpace(this.currentWorkflow.Code) ? ROOTACTIVITY_NAME : this.currentWorkflow.Code);
						}
					}

					PropertyGridData workData = this.WorkFlowPropertyGrid.Item as PropertyGridData;
					bool save = true;

					// check if user has entered code/description for new workflow - Code property should be empty for new workflow
					if (string.IsNullOrEmpty(this.currentWorkflow.Code))
					{
						string code = workData.Code;

						if (string.IsNullOrEmpty(code))
						{
							//  Make the Workflow Properties tab active (visible) for the user to see
							rightContainerTab2.IsSelected = true;
							
							// TODO: Codelookup for literal string for globalisation?
							MessageBox.Show("Please enter the workflow code and description!");
							save = false;
						}
						else
						{
							FWBS.WF.Packaging.WorkflowXaml chkWfXaml = new WF.Packaging.WorkflowXaml();
							if (chkWfXaml.Exists(code))
							{
								// exists, don't overwrite, abort Save!
								// TODO: Codelookup for literal string for globalisation?
								MessageBox.Show(string.Format("Workflow code {0} already exists in database", code), "Workflow", MessageBoxButton.OK, MessageBoxImage.Error);
								save = false;
							}
							else
							{
								this.currentWorkflow.Code = code;
							}
						}
					}

					if (save)
					{
						//  Bind data with Workflow Properties Grid
						this.currentWorkflow.Codelookup = workData.Description.Code;
						this.currentWorkflow.Group = workData.Group.Code;
						this.currentWorkflow.IsServerWorkflow = workData.IsServerWorkflow;
						this.currentWorkflow.IsVisibleInToolbox = workData.IsVisibleInToolbox;
						this.currentWorkflow.IsVisibleInPicker = workData.IsVisibleInPicker;
						this.currentWorkflow.IsReadOnly = workData.IsReadOnly;
						this.currentWorkflow.IsSystem = workData.IsSystem;
						this.currentWorkflow.Notes = (workData.Notes == null) ? string.Empty : String.Join(Environment.NewLine, workData.Notes);
						
						// get xaml from the workflow designer
						this.currentWorkflow.Xaml = this.SaveWorkflowToXaml();	// xaml;
						// merge any startup distributed assembly with the workflow one ensuring the workflow distributed list will ALWAYS contain the assemblies it requires
						this.AddStartupToWorkflowUsingWorkflowXaml(this.currentWorkflow.Xaml);
						// set distribution assemblies - must be done after the merging above as the 'Distribution', 'References' and 'ScriptCodes' properties get set!
						this.currentWorkflow.SetDistribution(this.Distribution);
						this.currentWorkflow.SetReferences(this.References);
						this.currentWorkflow.SetScriptCodes(this.ScriptCodes);

						// update database
						this.currentWorkflow.Update();

						retValue = true;
					}
				}
			}
			catch (Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}

			// invoke saved event
			if (retValue)
			{
				if (this.WorkflowModelSaved != null)
				{
					// pass the workflow code - may be used to display in window title ...
					this.WorkflowModelSaved(this, new StringEventArgs(this.currentWorkflow.Code));
				}
			}

			this.logger.TraceVerbose("WFControl.UpdateData() - End");

			return retValue;
		}

		internal void Load(string code)
		{
			this.logger.TraceVerbose("WFControl.Load({0})", new object[] { code });

			try
			{
				PropertyGridData workData = new PropertyGridData();
				if (string.IsNullOrEmpty(code))
				{
					this.currentWorkflow = new WorkflowXaml();
				}
				else
				{
					this.currentWorkflow = new WorkflowXaml();
					// check if it exists - may have been deleted ...
					if (this.currentWorkflow.Exists(code))
					{
						this.currentWorkflow.Fetch(code);

						//  Create a populated Property Grid
						workData.Code = code;
						workData.Description = new CodeLookupDisplay(FWBS.OMS.Workflow.Constants.CODELOOKUPTYPE);
						workData.Description.Code = this.currentWorkflow.Codelookup;
						workData.IsServerWorkflow = this.currentWorkflow.IsServerWorkflow;
						workData.IsVisibleInToolbox = this.currentWorkflow.IsVisibleInToolbox;
						workData.IsVisibleInPicker = this.currentWorkflow.IsVisibleInPicker;
						workData.IsReadOnly = this.currentWorkflow.IsReadOnly;
						workData.IsSystem = this.currentWorkflow.IsSystem;
						workData.Group = new CodeLookupDisplay(FWBS.OMS.Workflow.Constants.CODELOOKUP_GROUPTYPE);
						workData.Group.Code = this.currentWorkflow.Group;
						workData.Notes = string.IsNullOrWhiteSpace(this.currentWorkflow.Notes) ? null : this.currentWorkflow.Notes.Split(Environment.NewLine.ToCharArray());
						workData.Updated = this.currentWorkflow.UpdatedDate;
						workData.UpdatedBy = this.currentWorkflow.UpdatedBy;

						//  Set Edit Mode to OFF
						workData.isEditMode = false;

						//
						// disable property grid if it is a system FWBS workflow
						//	NOTE: we could possibly individually make the properties readonly by adding the ReadOnlyAttribute at run time before binding the object
						//	to the property grid above rather than disabling the whole grid which stops the subfields from 'expanding' ...
						if (this.currentWorkflow.IsSystem)
						{
							this.WorkFlowPropertyGrid.IsEnabled = false;
							workData.IsReadOnly = true;
						}
					}
					else
					{
						// TODO: Code lookup for message
						FWBS.OMS.UI.Windows.MessageBox.ShowInformation(string.Format("Workflow {0} does not exist!", code));
					}
					
				}
				workData.PropertyChanged += new PropertyChangedEventHandler(workData_PropertyChanged);
				this.WorkFlowPropertyGrid.Item = workData;

				// extract the required assemblies - create the cache path if required
				string dirPath = string.Format(@"{0}{1}\{2}", Global.GetCachePath().ToString(), FWBS.OMS.Workflow.Constants.CACHE_DIRECTORY,
												string.IsNullOrEmpty(this.currentWorkflow.Code) ? NEW_WORKFLOW_DIRECTORY + DateTime.Now.Ticks.ToString() : this.currentWorkflow.Code);
				if (!Directory.Exists(dirPath))
				{
					Directory.CreateDirectory(dirPath);
				}
				this.currentWorkflow.DistributedErrors += new EventHandler<DistributedEventArgs>(currentWorkflow_DistributedErrors);
				this.currentWorkflow.ExtractDistribution();
				// scripts normally get extracted to this directory...
				// extraction will also compile the script, any errors will be handled via DistributedErrors event handler.
				this.currentWorkflow.ExtractScripts();
				this.currentWorkflow.DistributedErrors -= new EventHandler<DistributedEventArgs>(currentWorkflow_DistributedErrors);
				// Set workflow assembly location 1st
				this.AssemblyCacheDirectory = dirPath;
				// set distributed references and scripts 2nd
				this.Distribution = this.currentWorkflow.GetDistributions();
				this.References = this.currentWorkflow.GetReferences();
				this.ScriptCodes = this.currentWorkflow.GetScriptCodes();
				// load the xaml LAST!
				this.LoadXamlCmd(this.currentWorkflow.Xaml);
			}
			catch (Exception ex)
			{
				this.logger.TraceError("Exception {0} {1}", new object[] { ex.Message, ex.StackTrace });
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}

			this.logger.TraceVerbose("WFControl.Load() - End");
		}

		void workData_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// we need saving
			this.isDirty = true;
			// toggle workflow designer IsReadOnly state
			if (e.PropertyName == "IsReadOnly")
			{
				PropertyGridData data = this.WorkFlowPropertyGrid.Item as PropertyGridData;
				if (data != null)
				{
					if (this.workflowDesigner.Context.Items.GetValue<System.Activities.Presentation.Hosting.ReadOnlyState>().IsReadOnly != data.IsReadOnly)
					{
						this.workflowDesigner.Context.Items.GetValue<System.Activities.Presentation.Hosting.ReadOnlyState>().IsReadOnly = data.IsReadOnly;
					}
				}
			}
			this.OnWorkflowModelChanged();
		}

		void currentWorkflow_DistributedErrors(object sender, DistributedEventArgs e)
		{
			// display message
			StringBuilder sb = new StringBuilder();
			Exception ex = e.Exception;
			do
			{
				sb.Append(ex.Message);
				ex = ex.InnerException;
			}
			while (ex != null);

			string errMsg = sb.ToString();
			this.logger.TraceError("Exception {0}", new object[] { errMsg });

			MessageDelegate md = new MessageDelegate(this.Message);
			this.Dispatcher.Invoke(md, new object[] { errMsg });
		}

		private Assembly LoadFromOrLoadAssembly(string assemblyFileName)
		{
			return WFRuntime.LoadFromOrLoadAssembly(assemblyFileName);
		}

		public void Dispose()
		{
			this.logger.TraceVerbose("WFControl.Dispose()");

			if (this.expressionEditorService != null)
			{
				this.expressionEditorService.CloseExpressionEditors();
				this.expressionEditorService.Dispose();
				this.expressionEditorService = null;
			}

			this.logger.TraceVerbose("WFControl.Dispose() - End");

			// close logger
			this.logger.Close();
		}
		
	}

	internal class StoreFactorySettings
	{
		internal StoreFactorySettings(WFControl current)
		{
			LeftContainer = new System.Windows.Size(current.leftContainer.Width, current.leftContainer.Height);
			LeftContainerTab1_Selected = true;
			LeftContainerTab1_IsPinned = current.leftContainerTab1.IsPinned;
			LeftContainerTab2_IsPinned = current.leftContainerTab2.IsPinned;

			RightContainer = new System.Windows.Size(current.rightContainer.Width,current.rightContainer.Height);
			RightContainerTab1_Selected = true;
			RightContainerTab1_IsPinned = current.rightContainerTab1.IsPinned;
			RightContainerTab2_IsPinned = current.rightContainerTab2.IsPinned;

			BottomContainer = new System.Windows.Size(current.bottomContainer.Width,current.bottomContainer.Height);
			BottomContainerTab1_IsPinned = current.bottomContainerTab1.IsPinned;
		}

		internal void Restore(WFControl current)
		{
			current.leftContainer.Width = LeftContainer.Width;
			current.leftContainer.Height = LeftContainer.Height;
			if (LeftContainerTab1_Selected)
				current.leftContainerTab1.IsSelected = true;
			else
				current.leftContainerTab2.IsSelected = true;
			current.leftContainerTab1.IsPinned = LeftContainerTab1_IsPinned;
			current.leftContainerTab2.IsPinned = LeftContainerTab2_IsPinned;

			current.rightContainer.Width = RightContainer.Width;
			current.rightContainer.Height = RightContainer.Height;
			if (RightContainerTab1_Selected)
				current.rightContainerTab1.IsSelected = true;
			else
				current.rightContainerTab2.IsSelected = true;
			current.rightContainerTab1.IsPinned = RightContainerTab1_IsPinned;
			current.rightContainerTab2.IsPinned = RightContainerTab2_IsPinned;

			current.bottomContainer.Width = BottomContainer.Width;
			current.bottomContainer.Height = BottomContainer.Height;
			current.bottomContainerTab1.IsPinned = BottomContainerTab1_IsPinned;
		}

		private System.Windows.Size LeftContainer { get; set; }

		private bool LeftContainerTab1_Selected { get; set; }
		private bool LeftContainerTab1_IsPinned { get; set; }
		private bool LeftContainerTab2_IsPinned { get; set; }

		private System.Windows.Size RightContainer { get; set; }
		private bool RightContainerTab1_Selected { get; set; }
		private bool RightContainerTab1_IsPinned { get; set; }
		private bool RightContainerTab2_IsPinned { get; set; }

		private System.Windows.Size BottomContainer { get; set; }
		private bool BottomContainerTab1_IsPinned { get; set; }
	}
}
