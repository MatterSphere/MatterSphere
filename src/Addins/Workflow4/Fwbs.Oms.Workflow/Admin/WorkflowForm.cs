#region References
using System;
using System.Diagnostics;
using System.Windows.Forms;
#endregion

namespace FWBS.OMS.Workflow.Admin
{
    public partial class WorkflowForm : Form
	{
		#region Constants
		// TODO: Should be code lookups
		private const string UNTITLED_WORKFLOW = "Untitled - Workflow Routine";	// text to display for new workflow
		private const string MODIFIED_WORKFLOW = " [Modified]";	// text to append to workflow name when modified
		#endregion

		#region Fields
		private string workflowCode = string.Empty;		// the code of the workflow
		private bool isDirty = false;					// workflow modified?

		// Diagnostic tracing
		private TraceSource traceSource = new TraceSource(FWBS.OMS.Workflow.Constants.TRACE_SOURCE_NAME);
		#endregion

		#region CreateForm
		//
		// IMPORTANT NOTE:
		//	This static method gets called via reflection by name in Core. If you change the name of the method
		//	then you must change the method literal string name in Core!
		//
		public static WorkflowForm CreateForm(string code)
		{
			WorkflowForm form = null;

			// search current forms to see if the workflow is already open
			for (int i = 0; i < Application.OpenForms.Count; i++)
			{
				form = Application.OpenForms[i] as WorkflowForm;
				if (form != null)
				{
					// we have a workflow mdi child, check if code is the same
					if (form.Code == code)
					{
						// same code, break to return the mdi child
						break;
					}
					form = null;
				}
			}

			if (form == null)
			{
				form = new WorkflowForm(code);
			}

			return form;
		}
		#endregion

		#region Constructors
		public WorkflowForm()
		{
			this.traceSource.TraceEvent(TraceEventType.Verbose, 1000, "WorkflowForm()");
			InitializeComponent();
			this.elementHost1.Child = new WFControl();

			// Find the MDI parent (we assume there is only ONE MDI container) and add ourselves to it
			for (int i = 0; i < Application.OpenForms.Count; i++)
			{
				if (Application.OpenForms[i].IsMdiContainer)
				{
					// found MDI parent
					this.MdiParent = Application.OpenForms[i];
					break;
				}
			}

			this.traceSource.TraceEvent(TraceEventType.Verbose, 1000, "WorkflowForm() - End");
		}

		public WorkflowForm(string code) :
			this()
		{
			this.workflowCode = code;
		}
		#endregion

		#region Properties
		public string Code
		{
			get { return this.workflowCode; }
		}
		#endregion

		#region Load event
		private void WorkflowForm_Load(object sender, EventArgs e)
		{
			try
			{
				// hook into the window closing events
				this.FormClosing += new FormClosingEventHandler(WorkflowForm_FormClosing);
				this.FormClosed += new FormClosedEventHandler(WorkflowForm_FormClosed);

				// hook in to the workflow control events
				WFControl ctrl = this.elementHost1.Child as WFControl;
				System.Diagnostics.Trace.Assert(ctrl != null);
				ctrl.WorkflowModelChanged += new EventHandler<EventArgs>(ctrl_WorkflowModelChanged);
				ctrl.WorkflowModelSaved += new EventHandler<StringEventArgs>(ctrl_WorkflowModelSaved);
				// load the workflow
				ctrl.Load(this.workflowCode);

				if (string.IsNullOrEmpty(this.workflowCode))
				{
					this.Text = UNTITLED_WORKFLOW;
				}
				else
				{
					// TODO: should use code lookup
					this.Text = this.workflowCode + " - Workflow Routine";
				}
			}
			catch (Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}
		}
		#endregion

		#region Closing event
		void WorkflowForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			WFControl ctrl = this.elementHost1.Child as WFControl;

			if (ctrl != null)
			{
				if (ctrl.IsWorkflowRunning)
				{
					// cancel closing if we are running workflow
					e.Cancel = true;
				}
				else if (this.isDirty)
				{
					switch (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoCancel("DIRTYDATAMSG", "Changes have been detected, would you like to save?", this.workflowCode))
					{
						case System.Windows.Forms.DialogResult.Yes:
							if (ctrl.UpdateData() == false)
							{
								// cancel closing if we failed to save
								e.Cancel = true;
							}
							else
							{
								this.isDirty = false;
							}
							break;

						case System.Windows.Forms.DialogResult.No:
							this.isDirty = false;
							break;

						case System.Windows.Forms.DialogResult.Cancel:
							e.Cancel = true;
							break;
					}
				}

				// save settings
				if (!e.Cancel)
				{
					ctrl.SaveSettings();
				}
			}
		}
		#endregion

		#region Close event
		void WorkflowForm_FormClosed(object sender, FormClosedEventArgs e)
		{
            Controls.Remove(elementHost1);
            elementHost1.Dispose();
            elementHost1 = null;
			traceSource.Close();
		}
		#endregion

		#region WorkflowModelSaved event
		void ctrl_WorkflowModelSaved(object sender, StringEventArgs e)
		{
			this.isDirty = false;

			if (!string.IsNullOrEmpty(e.Str))
			{
				// set workflow code
				this.workflowCode = e.Str;
				// set title
				this.Text = "Workflow - " + this.workflowCode;
			}
			else
			{
				// no code supplied ...
				// remove "[Modified]" from title
				this.Text = this.Text.Replace(MODIFIED_WORKFLOW, string.Empty);
			}
		}
		#endregion

		#region WorkflowModelChanged event
		void ctrl_WorkflowModelChanged(object sender, EventArgs e)
		{
			this.traceSource.TraceEvent(TraceEventType.Verbose, 1000, "WorkflowForm.ctrl_WorkflowModelChanged()");
			// Add modified to title
			if (!this.isDirty)
			{
				this.Text += MODIFIED_WORKFLOW;
				this.isDirty = true;
			}
		}
		#endregion
	}
}
