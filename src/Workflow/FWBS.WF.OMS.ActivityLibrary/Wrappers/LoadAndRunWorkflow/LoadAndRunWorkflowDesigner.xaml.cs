#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class LoadAndRunWorkflowDesigner
	{
		#region Fields
		// Command object wired up in .xaml
		public System.Windows.Input.ICommand ViewWorkflow { get; set; }
		#endregion

		#region Constructor
		public LoadAndRunWorkflowDesigner()
		{
			InitializeComponent();

			// set delegate
			this.ViewWorkflow = new DelegateCommand(x => this.ViewWorkflowCmdExecuted(), x => this.ViewWorkflowCmdCanExecute());

			this.Loaded += new System.Windows.RoutedEventHandler(LoadAndRunWorkflowDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string InTooltip2 { get; set; }
		public string OutTooltip { get; set; }
		#endregion

		#region LoadAndRunWorkflowDesigner_Loaded
		void LoadAndRunWorkflowDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTLARWBDTT", "Load and Run Workflow", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTLARWINTT", "A Workflow Code string as input", "").Text;
				this.InTooltip2 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTLARWINTT2", "A Dictionary as input", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTLARWOTTT", "A Dictionary output result", "").Text;
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
						// if the workflow is already open in a designer then use that, otherwise create new designer ...
						Type t = Type.GetType("FWBS.OMS.Workflow.Admin.WorkflowForm,FWBS.OMS.Workflow");
						object obj = t.InvokeMember("CreateForm",
							System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.InvokeMethod,
							null, null, new object[] { code });
						if (obj != null)
						{
							// we have a designer form
							System.Windows.Forms.Form form = obj as System.Windows.Forms.Form;
							if (form != null)
							{
								// show designer
								form.Show();
								form.Focus();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}
		}

		internal bool ViewWorkflowCmdCanExecute()
		{
			return (this.textBoxWorkflowCode.Expression != null) && (this.textBoxWorkflowCode.Expression.ItemType == typeof(System.Activities.Expressions.Literal<string>));
		}
		#endregion
	}
}
