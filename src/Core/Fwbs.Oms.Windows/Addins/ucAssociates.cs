using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.OMS.Interfaces;
using FWBS.OMS.Security;
using FWBS.OMS.Security.Permissions;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Displays a list of associates contacts that are linked to a specific file.  This is used as
    /// a IConfigurableTypeAddin, to be used in OMSDialogs.
    /// </summary>
    public class ucAssociates : ucBaseAddin, Interfaces.IOMSTypeAddin
	{
		#region Fields

		/// <summary>
		/// The current file to work with.
		/// </summary>
		private OMSFile _file = null;
        private IContainer components;

		#endregion

		#region Controls

		private FWBS.OMS.UI.Windows.ucSearchControl ucSearchControl1;

		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default constructor of the user control.
		/// </summary>
		public ucAssociates()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                if (ucSearchControl1 != null)
                {
                    ucSearchControl1.Dispose();
                }
                if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.ucSearchControl1 = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlActions
            // 
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Visible = true;
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // ucSearchControl1
            // 
            this.ucSearchControl1.BackColor = System.Drawing.Color.White;
            this.ucSearchControl1.BackGroundColor = System.Drawing.Color.White;
            this.ucSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchControl1.DoubleClickAction = "None";
            this.ucSearchControl1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucSearchControl1.GraphicalPanelVisible = true;
            this.ucSearchControl1.Location = new System.Drawing.Point(168, 0);
            this.ucSearchControl1.Name = "ucSearchControl1";
            this.ucSearchControl1.NavCommandPanel = this.navCommands;
            this.ucSearchControl1.Padding = new System.Windows.Forms.Padding(5);
            this.ucSearchControl1.RefreshOnEnquiryFormRefreshEvent = false;
            this.ucSearchControl1.SaveSearch = FWBS.OMS.SearchEngine.SaveSearchType.Never;
            this.ucSearchControl1.SearchListCode = "";
            this.ucSearchControl1.SearchListType = "";
            this.ucSearchControl1.SearchPanelVisible = false;
            this.ucSearchControl1.Size = new System.Drawing.Size(672, 490);
            this.ucSearchControl1.TabIndex = 8;
            this.ucSearchControl1.ToBeRefreshed = false;
            this.ucSearchControl1.TypeSelectorVisible = false;
            this.ucSearchControl1.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.ucSearchControl1_SearchButtonCommands);
            // 
            // ucAssociates
            // 
            this.Controls.Add(this.ucSearchControl1);
            this.Name = "ucAssociates";
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this.ucSearchControl1, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		#endregion

		#region Methods

		/// <summary>
		/// Captures and overrides the search lists action buttons.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ucSearchControl1_SearchButtonCommands(object sender, FWBS.OMS.UI.Windows.SearchButtonEventArgs e)
		{
			try
			{
				switch (e.ButtonName.ToUpper())
				{
                    case "CMDADD":
                        goto case "BTNADD";
                    case "BTNADD":
                        {
                            var wizardForm = OMSApp.CreateModelessWizard("NEWASSOC", WizardStyle.InPlace, _file) as frmWizard;
                            if (wizardForm != null)
                            {
                                var omsItem = new ucOMSItemWizard(wizardForm);
                                ucSearchControl1.OpenOMSItem(omsItem);
                            }
                            e.Cancel = true;
                        }
                        break;
					case "CMDBUSRESTORE":
					{
						Common.KeyValueCollection[] assocs = ucSearchControl1.SelectedItems;
						FWBS.OMS.Associate ass = FWBS.OMS.Associate.GetAssociate(Convert.ToInt64(assocs[0]["ASSOCID"].Value));
						ass.Restore();
                        ass.Update();
                        ucSearchControl1.Search();
						break;
					}
					case "CMDBUSDELETE":
					{
						Common.KeyValueCollection[] assocs = ucSearchControl1.SelectedItems;
						FWBS.OMS.Associate ass = FWBS.OMS.Associate.GetAssociate(Convert.ToInt64(assocs[0]["ASSOCID"].Value));
						ass.Delete();
						ass.Update();
						ucSearchControl1.Search();
						break;
					}
					case "BTNCOPYTO":
					{
						try
						{
							FWBS.OMS.OMSFile source_file = _file;
							if (source_file != null)
							{
								FWBS.OMS.UI.Windows.Services.Searches filesearch = new FWBS.OMS.UI.Windows.Services.Searches(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.FileAssociateCopy));
								filesearch.AsType = false;		
								filesearch.Parent = source_file.Client;
								filesearch.HideButtons = true;
								FWBS.Common.KeyValueCollection ret = filesearch.Show(ParentForm);
								if (!(ret == null || ret.Count <= 0 || ret.Contains("FILEID") == false))
								{
									FWBS.OMS.OMSFile dest_file = FWBS.OMS.OMSFile.GetFile(Convert.ToInt64(ret["FILEID"].Value));
									if (dest_file != null)
									{
										string msg = @"%1% %ASSOCIATES% were Successfully copied.  %3% may already exist so not copied.
Would you like to view the destination %FILE% '%2%'?";
										int count = 0;
										int expected = 0;

										try
										{
											Cursor = Cursors.WaitCursor;
                                            if (e.ButtonName.ToUpper() == "BTNCOPYTO")
                                            {
                                                Common.KeyValueCollection[] assocs = ucSearchControl1.SelectedItems;
                                                if (ret == null || assocs.Length == 0)
                                                    break;
                                                else
                                                {
                                                    ArrayList arr = new ArrayList();
                                                    for (int ctr = 0; ctr < assocs.Length; ctr++)
                                                    {
                                                        arr.Add(assocs[ctr]["ASSOCID"].Value);
                                                    }
                                                    long[] selectedIds = new long[arr.Count];
                                                    arr.CopyTo(selectedIds);
                                                    expected = selectedIds.Length;
                                                    count = source_file.Associates.CopyTo(dest_file, selectedIds);
                                                }
                                            }
                                            else
                                            {
                                                expected = source_file.Associates.Count;
                                                count = source_file.Associates.CopyTo(dest_file);
                                            }
											dest_file.Update();
										}
										finally
										{
											Cursor = Cursors.Default;
										}

										if (source_file.ID != dest_file.ID)
										{
											FWBS.OMS.UI.Windows.MessageBox msgbox = new FWBS.OMS.UI.Windows.MessageBox(MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
											msgbox.Text = Session.CurrentSession.Resources.GetMessage("ASSOCCPYSUCCES2", msg, "", true, count.ToString(), dest_file.ToString(), (expected - count).ToString() );
									
											if (msgbox.Show(ParentForm) == "YES")
											{
												base.OnNewOMSTypeWindow(this, new NewOMSTypeWindowEventArgs(dest_file));
											}
										}
										else
											ucSearchControl1.Search();
									}
								}
							}
		
						}
						catch(Exception ex)
						{
							ErrorBox.Show(ParentForm, ex);
						}
						finally
						{
							Cursor = Cursors.Default;
							e.Cancel = true;
						}
					}
						break;
					case "BTNCOPYALLTO":
						goto case "BTNCOPYTO";
				}

				if (e.ButtonName.ToUpper().StartsWith("TS"))
				{

					Common.KeyValueCollection ret = ucSearchControl1.CurrentItem();

					Associate assoctmp;
					assoctmp = FWBS.OMS.Associate.GetAssociate((long)ret["ASSOCID"].Value);
					if (assoctmp != null)
					{
						DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("ASSOCJOBSUBMIT", "The job has been submitted, would you like to run now?", ""), "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
						if (res != DialogResult.Cancel)
						{
							PrecedentJob job = new PrecedentJob(Precedent.GetDefaultPrecedent(e.ButtonName.Substring(2).ToString(), assoctmp));
							job.Associate = assoctmp;
							switch (res)
							{
								case DialogResult.Yes:
								{
									Services.ProcessJob(null, job);
									if (job.HasError)
										MessageBox.Show(job.ErrorMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
								}
									break;
								case DialogResult.No:
								{
									Session.CurrentSession.CurrentPrecedentJobList.Add(job);
								}
									break;
							}
						}
					}


				}


			}
			catch (Exception ex)
			{
				MessageBox.Show(ParentForm, ex);
			}
		}

		#endregion

		#region IOMSTypeAddin Implementation

		/// <summary>
		/// Allows the calling OMS dialog to connect to the addin for the configurable type object.
		/// </summary>
		/// <param name="obj">OMS Configurable type object to use.</param>
		/// <returns>A flag that tells the dialogthat the connection has been successfull.</returns>
		public override bool Connect(IOMSType obj)
		{
			_file = obj as OMSFile;

			if (obj == null)
				return false;
			else
			{
				ucSearchControl1.NewOMSTypeWindow -=new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
				ucSearchControl1.NewOMSTypeWindow +=new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
				ToBeRefreshed=true;
				return true;
			}

		}

		/// <summary>
		/// Refreshes the addin visual look and data contents.
		/// </summary>
		public override void RefreshItem()
		{
			if (_file != null && ToBeRefreshed)
			{
				if (ucSearchControl1.SearchList == null)
					ucSearchControl1.SetSearchList(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.Associates),_file, null);
				ucSearchControl1.RefreshItem();
				ucSearchControl1.ShowPanelButtons();
				ToBeRefreshed=false;
			}
		}

		public override void UpdateItem()
		{
			ucSearchControl1.UpdateItem();
		}

		public override void CancelItem()
		{
			ucSearchControl1.CancelItem();
		}


		public override bool IsDirty
		{
			get
			{
				return ucSearchControl1.IsDirty;
			}
		}


		#endregion
	}
}
