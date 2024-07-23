#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	// Interaction logic for LoadWorkflowDesigner.xaml
	internal sealed partial class LoadWorkflowDesigner
	{
		#region Fields
		// Command object wired up in .xaml
		public System.Windows.Input.ICommand ViewWorkflow { get; set; }
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string OutTooltip { get; set; }
		#endregion

		#region Constructor
		public LoadWorkflowDesigner()
		{
			InitializeComponent();

			// set delegate
			this.ViewWorkflow = new DelegateCommand(x => this.ViewWorkflowCmdExecuted(), x => this.ViewWorkflowCmdCanExecute());

			this.Loaded += new System.Windows.RoutedEventHandler(LoadWorkflowDesigner_Loaded);
		}
		#endregion

		#region LoadWorkflowDesigner_Loaded
		void LoadWorkflowDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTLDWKBDTT", "Load a Workflow As an Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTLDWKINTT", "A Workflow Code string as input", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTLDWKOTTT", "An Activity output result", "").Text;
			}
			catch (Exception)
			{
				// error
				;
			}			
		}
		#endregion

		#region Handle ViewWorkflow command
		internal void ViewWorkflowCmdExecuted()
		{
			try
			{
				// if code is literal string ...
				if ((this.textBoxWorkflowCode.Expression != null) &&
					(this.textBoxWorkflowCode.Expression.ItemType == typeof(System.Activities.Expressions.Literal<string>)))
				{
					// get code value
					string code = this.textBoxWorkflowCode.Expression.GetCurrentValue().ToString().Trim();
					if (!string.IsNullOrEmpty(code))
					{
						// create the designer form
						object obj = AppDomain.CurrentDomain.CreateInstanceAndUnwrap("FWBS.OMS.Workflow", "FWBS.OMS.Workflow.Admin.WorkflowForm", false, System.Reflection.BindingFlags.CreateInstance, null, new object[] { code }, null, null);
						if (obj != null)
						{
							// we have a designer form, show it
							System.Windows.Forms.Form form = obj as System.Windows.Forms.Form;
							form.Show();
						}
					}
				}
			}
			catch (Exception)
			{
				// TODO: handle error
			}
		}

		internal bool ViewWorkflowCmdCanExecute()
		{
			return (this.textBoxWorkflowCode.Expression != null) && (this.textBoxWorkflowCode.Expression.ItemType == typeof(System.Activities.Expressions.Literal<string>));
		}
		#endregion
	}
}
