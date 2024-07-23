#region References
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Elite.Workflow.Framework.Client.Administrator;
using Elite.Workflow.Framework.Client.Administrator.AdminService;
using FWBS.WF.Packaging;
#endregion

namespace FWBS.OMS.Workflow.Admin
{
    internal partial class WorkflowList : FWBS.OMS.UI.Windows.Admin.ucEditBase2
	{
		#region Constants
		private const string SEARCHLISTNAME = "ADMWORKFLOW";	// searchlist in database
		private const string SPECIFICDATA_WORKFLOWADMIN = "WORKFLOWADMIN";
		private const string SQL_COLUMNNAME_WORKFLOWCODE = "wfCode";
		#endregion

		#region Fields
		private FWBS.Logging.TraceLogger logger = FWBS.Logging.TraceSourceFactory.CreateTraceSource(FWBS.OMS.Workflow.Constants.TRACE_SOURCE_NAME, 1000);
		#endregion

		#region Constructor
		public WorkflowList()
		{
			this.logger.TraceVerbose("WorkflowList()");

			InitializeComponent();
			
			// hide toolbar and label
			this.pnlEdit.Visible = false;

            this.lstList.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.SearchButtonCommands);

			this.logger.TraceVerbose("WorkflowList() - End");
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
        }

        #endregion

		#region ucEditBase2
		#region SearchListName
		protected override string SearchListName
		{
			get
			{
				return SEARCHLISTNAME;
			}
		}
		#endregion

		#region NewData
		// Gets called when user clicks 'Add' on main frame
		protected override void NewData()
		{
			this.logger.TraceVerbose("WorkflowList.NewData()");

			this.LoadSingleItem(string.Empty);

			this.logger.TraceVerbose("WorkflowList.NewData() - End");
		}
		#endregion

		#region DeleteData
		protected override void DeleteData(string code)
		{
			FWBS.WF.Packaging.WorkflowXaml current = null;
			try
			{
				current = new FWBS.WF.Packaging.WorkflowXaml();
				current.Fetch(code);
				current.Delete();
				lstList.Search();
			}
			catch (Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}
			finally
			{
				if (current != null)
				{
					current.Dispose();
					current = null;
				}
			}
		}
		#endregion

		#region Clone
		protected override void Clone(string code)
		{
			this.logger.TraceVerbose("WorkflowList.Clone()");

			CodeDescriptionWindow dlg = new CodeDescriptionWindow();
			if (dlg.ShowDialog() == true)
			{
				FWBS.WF.Packaging.WorkflowXaml wfSrc = null;
				FWBS.WF.Packaging.WorkflowXaml wfDest = null;

				try
				{
					// get the source workflow
					wfSrc = new FWBS.WF.Packaging.WorkflowXaml();
					wfSrc.Fetch(code);

					// Set the destination workflow
					wfDest = wfSrc.WorkflowCopy();
					wfDest.Code = dlg.Code;					// set code
					wfDest.Update();						// save to database
					lstList.Search();						// update list
				}
				finally
				{
					if (wfSrc != null)
					{
						wfSrc.Dispose();
						wfSrc = null;
					}
					if (wfDest != null)
					{
						wfDest.Dispose();
						wfDest = null;
					}
				}
			}

			this.logger.TraceVerbose("WorkflowList.Clone() - End");
		}
		#endregion

		#region LoadSingleItem
		protected override void LoadSingleItem(string code)
		{
			this.logger.TraceVerbose("WorkflowList.LoadSingleItem()");

			//
			// we need to check if this is a server or a system workflow and do nothing if it is
			//
			WorkflowXaml wf = null;
			if (!string.IsNullOrWhiteSpace(code))
			{
				wf = new WorkflowXaml();
				wf.Fetch(code);
				if (wf.IsServerWorkflow)
				{
					FWBS.OMS.UI.Windows.MessageBox.ShowInformation(string.Format(FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFAIERR0002", "{0} is a server workflow.", "").Text, code));
				}
				else
				{
					wf = null;
				}
			}

			if (wf == null)
			{
				WorkflowForm wfForm = WorkflowForm.CreateForm(code);
				wfForm.FormClosed += new FormClosedEventHandler(wfForm_FormClosed);
				wfForm.Show();
				wfForm.Focus();
			}

			this.logger.TraceVerbose("WorkflowList.LoadSingleItem() - End");
		}

		void wfForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			var wfForm = (WorkflowForm)sender;
			wfForm.FormClosed -= new FormClosedEventHandler(wfForm_FormClosed);
			lstList.Search();
		}
		#endregion

		#region Handle buttons
		private void SearchButtonCommands(object sender, FWBS.OMS.UI.Windows.SearchButtonEventArgs e)
        {
            if (e.ButtonName == "cmdSync")
            {
				Administration admin = null;
				SplashWindow wnd = new SplashWindow(FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFAISPLH0004", "Synchronising with the workflow server", "").Text);
				wnd.Show();

				try
				{
					// Sync the workflows
					string url = FWBS.OMS.Session.CurrentSession.GetSpecificData(SPECIFICDATA_WORKFLOWADMIN) as string;
					if (url != null)
					{
						admin = new Administration();
						Exception error = admin.CreateFactory(new Uri(url));
						if (error == null)
						{
							WorkflowPackagesRequest request = new WorkflowPackagesRequest()
							{
								DoPaging = false,
								CorrelationID = Guid.NewGuid(),
							};

							wnd.Close();
							wnd = new SplashWindow(FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFAISPLH0001", "Getting data from the workflow server", "").Text);
							wnd.Show();

							WorkflowPackagesResponse response = admin.GetWorkflowPackages(request);
							if (response.Error == ErrorCode.Success)
							{
								wnd.Close();
								wnd = new SplashWindow(FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFAISPLH0002", "Filtering data", "").Text);
								wnd.Show();

								// stick workflow codes in a hashset - comparison is case sensitive!
								HashSet<string> serverCodes = new HashSet<string>(StringComparer.Ordinal);
								for (int i = 0; i < response.Packages.Count; i++)
								{
									// we need to check because we may have different versions of the same code
									if (!serverCodes.Contains(response.Packages[i].Code))
									{
										serverCodes.Add(response.Packages[i].Code);
									}
								}
								// filter out the ones existing
								if ((this.lstList.DataTable != null) && ((this.lstList.DataTable.Rows.Count > 0)))
								{
									for (int i = 0; i < this.lstList.DataTable.Rows.Count; i++)
									{
										serverCodes.Remove((string)this.lstList.DataTable.Rows[i][SQL_COLUMNNAME_WORKFLOWCODE]);
									}
								}

								wnd.Close();
								wnd = null;

								SynchonisationWindow selectCodesDlg = new SynchonisationWindow(serverCodes);
								if (selectCodesDlg.ShowDialog() == true)
								{
									wnd = new SplashWindow(FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFAISPLH0003", "Updating with new data", "").Text);
									wnd.Show();

									// insert the selected
									serverCodes = selectCodesDlg.Selected;
									HashSet<string> failedCodes = new HashSet<string>();
									foreach (string item in serverCodes)
									{
										WorkflowXaml currentWorkflow = new WorkflowXaml()
										{
											Code = item,					// the workflow code
											Xaml = string.Empty,			// no xaml, must be empty string
											Codelookup = string.Empty,		// no code lookup, must be empty string
											Group = string.Empty,			// no group, must be empty string
											IsServerWorkflow = true,		// server workflow
											IsVisibleInToolbox = false,		// can't be seen in the toolbox
											IsVisibleInPicker = true,		// can be picked by default
											IsReadOnly = true,				// read only, can't be modified
											IsSystem = false,				// not system workflow supplied by us
											Notes = string.Empty,
										};

										// update database
										try
										{
											currentWorkflow.Update();
										}
										catch (Exception)
										{
											// error - add item to failed
											failedCodes.Add(item);
										}
									}

									// refresh the list
									this.Refresh();

									if (failedCodes.Count > 0)
									{
										// display failed items
										int count = 0;
										StringBuilder sb = new StringBuilder();
										sb.AppendLine(FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFAIERR0001", "Failed to add:", "").Text);
										foreach (string item in failedCodes)
										{
											sb.AppendLine(item);
											if (count == 20)
											{
												sb.AppendLine("....");
												break;
											}
											count++;
										}
										FWBS.OMS.UI.Windows.MessageBox.ShowInformation(sb.ToString());
									}
								}
							}
							else
							{
								// error
								if (response.Exception != null)
								{
									FWBS.OMS.UI.Windows.ErrorBox.Show(response.Exception);
								}
								else
								{
									FWBS.OMS.UI.Windows.MessageBox.Show(response.Error.ToString());
								}
							}
						}
						else
						{
							FWBS.OMS.UI.Windows.MessageBox.Show(error);
						}
					}
				}
				catch (Exception ex)
				{
					FWBS.OMS.UI.Windows.MessageBox.Show(ex);
				}
				finally
				{
					#region release resources
					// close splash
					if (wnd != null)
					{
						wnd.Close();
					}

					if (admin != null)
					{
						admin.Dispose();
						admin = null;
					}
					#endregion
				}
            }
		}
		#endregion
		#endregion
	}
}
