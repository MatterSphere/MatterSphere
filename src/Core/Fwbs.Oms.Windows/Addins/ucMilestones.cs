using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Displays the milestone information as an intelligent addin control.
    /// </summary>
    public class ucMilestones : ucBaseAddin, Interfaces.IOMSTypeAddin
	{
        private Hashtable EnquiryControls = new Hashtable();
        private IContainer components;

		/// <summary>
		/// The current client to work with.
		/// </summary>
		private OMSFile _omsfile = null;
		private bool _isdirty = false;
		private Milestones_OMS2K _milestone = null;
		private bool _toolbarvisible = true;
		private bool _disableevents = false;
		private bool _deleting = false;
		
		/// <summary>
		/// The enquiry form that the control may be sitting on.
		/// </summary>
		private EnquiryForm _enqForm = null;
		public FWBS.OMS.UI.Windows.eToolbars menu;
        private Timer timRunFirst;
        private Panel pnlToolbarContainer;
        private EnquiryForm enquiryForm1;

        /// <summary>
        /// The page name that this control lies on.
        /// </summary>
        private string _pgeName = "";


		public ucMilestones()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            this.menu.ImageList = FWBS.OMS.UI.Windows.Images.Windows8();
        }


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMilestones));
            this.menu = new FWBS.OMS.UI.Windows.eToolbars();
            this.timRunFirst = new System.Windows.Forms.Timer(this.components);
            this.pnlToolbarContainer = new System.Windows.Forms.Panel();
            this.enquiryForm1 = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDesign
            // 
            this.pnlDesign.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDesign.Location = new System.Drawing.Point(667, 5);
            this.pnlDesign.Size = new System.Drawing.Size(168, 480);
            // 
            // pnlActions
            // 
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Visible = true;
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // menu
            // 
            this.menu.BottomDivider = false;
            this.menu.ButtonsXML = resources.GetString("menu.ButtonsXML");
            this.menu.Divider = false;
            this.menu.DropDownArrows = true;
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.NavCommandPanel = null;
            this.menu.ShowToolTips = true;
            this.menu.Size = new System.Drawing.Size(662, 26);
            this.menu.TabIndex = 10;
            this.menu.TopDivider = false;
            this.menu.OMSButtonClick += new FWBS.OMS.UI.Windows.OMSToolBarButtonClickEventHandler(this.ButtonClick);
            // 
            // timRunFirst
            // 
            this.timRunFirst.Interval = 1000;
            this.timRunFirst.Tick += new System.EventHandler(this.timRunFirst_Tick);
            // 
            // pnlTolbarContainer
            // 
            this.pnlToolbarContainer.Controls.Add(this.menu);
            this.pnlToolbarContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbarContainer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlToolbarContainer.Location = new System.Drawing.Point(5, 5);
            this.pnlToolbarContainer.Name = "pnlToolbarContainer";
            this.pnlToolbarContainer.Size = new System.Drawing.Size(662, 26);
            this.pnlToolbarContainer.TabIndex = 11;
            // 
            // enquiryForm1
            // 
            this.enquiryForm1.AutoScroll = true;
            this.enquiryForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enquiryForm1.IsDirty = false;
            this.enquiryForm1.Location = new System.Drawing.Point(5, 31);
            this.enquiryForm1.Name = "enquiryForm1";
            this.enquiryForm1.Size = new System.Drawing.Size(662, 454);
            this.enquiryForm1.TabIndex = 12;
            this.enquiryForm1.ToBeRefreshed = false;
            this.enquiryForm1.RefreshedControls += new System.EventHandler(this.enquiryForm1_Rendered);
            this.enquiryForm1.NewOMSTypeWindow += new FWBS.OMS.UI.Windows.NewOMSTypeWindowEventHandler(this.enquiryForm1_NewOMSTypeWindow);
            // 
            // ucMilestones
            // 
            this.Controls.Add(this.enquiryForm1);
            this.Controls.Add(this.pnlToolbarContainer);
            this.Name = "ucMilestones";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ParentChanged += new System.EventHandler(this.ucMilestones_ParentChanged);
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this.pnlToolbarContainer, 0);
            this.Controls.SetChildIndex(this.enquiryForm1, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.ResumeLayout(false);

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
			_omsfile = obj as OMSFile;
			if (_omsfile.MilestonePlan != null)
				_milestone = _omsfile.MilestonePlan;
			else
				_milestone = new Milestones_OMS2K(_omsfile);
			if (obj == null)
				return false;
			else
			{
				ToBeRefreshed =true;
				return true;
			}
		}

		/// <summary>
		/// Updates the contents of the addin, if there is any at all.
		/// </summary>
		public override void UpdateItem()
		{
            if (enquiryForm1 != null)
            {
                enquiryForm1.UpdateItem();
                if (_omsfile != null)
                    _omsfile.Update();
                _isdirty = false;
                enquiryForm1.IsDirty = false;
                ToBeRefreshed = true;
                RefreshItem();
            }
            else if (_milestone.MSPlan != "")
            {
                _milestone.RemoveMileStonePlan();
                _isdirty = false;
            }
		}

		public override bool IsDirty
		{
			get
			{
                if (enquiryForm1 == null && _milestone != null && _milestone.MSPlan != "")
                    return true;
                else if (enquiryForm1 == null)
                    return false;
                else if (_isdirty || enquiryForm1.IsDirty)
                    return true;
                else
                    return false;
			}
		}

		public override void CancelItem()
		{
            if (enquiryForm1 != null)
            {
                enquiryForm1.CancelItem();
                _isdirty = false;
                base.CancelItem();
            }
		}



		/// <summary>
		/// Refreshes the addin visual look and data contents.
		/// </summary>
		public override void RefreshItem()
		{
			if (_omsfile != null)
			{
				try
				{
					if (ToBeRefreshed)
					{
						if (_milestone.MSPlan != "")
						{
                            if (enquiryForm1.Enquiry == null)
                            {
								_disableevents = true;
                                enquiryForm1.Dirty -= new EventHandler(OnDirty);
                                enquiryForm1.Enquiry = _milestone.Edit(null);
								enquiryForm1.Dirty +=new EventHandler(OnDirty);
								enquiryForm1.IsDirty=false;
								_isdirty=false;
								_disableevents = false;
							}
							else
							{
								if (((Milestones_OMS2K)enquiryForm1.Enquiry.Object).MSPlan != "")
								{
									enquiryForm1.RefreshItem();
									_omsfile.MilestonePlan = (Milestones_OMS2K)enquiryForm1.Enquiry.Object;
                                }

								if (enquiryForm1.Enquiry.Object is Milestones_OMS2K && ((Milestones_OMS2K)enquiryForm1.Enquiry.Object).OMSFile.IsNew)
								{
									menu.GetButton("btnPrint").Enabled = false;
								}
							}
                            if (enquiryForm1.Enquiry != null && enquiryForm1.Enquiry.Object is Milestones_OMS2K && ((Milestones_OMS2K)enquiryForm1.Enquiry.Object).OMSFile.IsNew)
                            {
                                menu.GetButton("btnPrint").Enabled = false;
                            }
                        }
						else
						{
							menu.GetButton("btnPrint").Enabled = false;
							menu.GetButton("Button2").Enabled = false;
                            if (enquiryForm1 != null)
                            {
                                enquiryForm1.Enquiry = null;
                            }
                        }
                    }
					


					ToBeRefreshed = false;
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}
			}
		}

        public override ucPanelNav[] Panels
        {
            get
            {
                return null;
            }
        }

		#endregion

        [DefaultValue(true)]
		public bool ShowToolBar
		{
			get
			{
				return _toolbarvisible;
			}
			set
			{
				_toolbarvisible = value;
                pnlToolbarContainer.Visible = _toolbarvisible;
            }
		}

		private void enquiryForm1_Rendered(object sender, System.EventArgs e)
		{
			for (int i = 1; i<=30; i++)
			{
				CheckColorChange(i);
				enquiryForm1.GetControl("chkAchieved" + i.ToString(),EnquiryControlMissing.Exception).Click -=new EventHandler(Changed);
				enquiryForm1.GetControl("chkAchieved" + i.ToString(),EnquiryControlMissing.Exception).Click +=new EventHandler(Changed);
				enquiryForm1.GetIBasicEnquiryControl2("dteMSStage" + i.ToString() + "Due",EnquiryControlMissing.Exception).Changed -=new EventHandler(Changed);
				enquiryForm1.GetIBasicEnquiryControl2("dteMSStage" + i.ToString() + "Due",EnquiryControlMissing.Exception).Changed +=new EventHandler(Changed);
			}
			if (enquiryForm1.Enquiry.Object is Milestones_OMS2K)
			{
				menu.GetButton("Button2").Enabled = !((Milestones_OMS2K)enquiryForm1.Enquiry.Object).IsClear;
				menu.GetButton("btnPrint").Enabled = !((Milestones_OMS2K)enquiryForm1.Enquiry.Object).IsClear;
				menu.GetButton("Button1").Enabled = ((Milestones_OMS2K)enquiryForm1.Enquiry.Object).IsClear;
			}
			else
			{
				menu.GetButton("Button1").Enabled = true;
				menu.GetButton("Button2").Enabled = false;
				menu.GetButton("btnPrint").Enabled = false;
			}

			if (enquiryForm1.Enquiry.Object is Milestones_OMS2K && ((Milestones_OMS2K)enquiryForm1.Enquiry.Object).OMSFile.IsNew)
			{
				menu.GetButton("btnPrint").Enabled = false;
			}
							

		}

		private void Changed(object sender, EventArgs e)
		{
			if (_disableevents==false)
			{
				if (((Control)sender).Name.StartsWith("chk"))
				{
					string io = ((Control)sender).Name.Substring(11);
					int i = Convert.ToInt32(io);
					enquiryForm1.GetControl("txtDaysLeft" + i.ToString() + "",EnquiryControlMissing.Exception).Focus();
					CheckColorChange(i);
				}
				if (((Control)sender).Name.StartsWith("dte"))
				{
					string io = ((Control)sender).Name.Substring(10);
					io = io.Replace("Due","");
					int i = Convert.ToInt32(io);
					CheckColorChange(i);
				}
			}
		}

		private void CheckColorChange(int i)
		{
			if (_deleting) return;
			if (Convert.ToString(enquiryForm1.GetIBasicEnquiryControl2("lblMSStage" + i.ToString() + "Achieved",EnquiryControlMissing.Exception).Value) == "")
			{
				enquiryForm1.GetControl("dteMSStage" + i.ToString() + "Due",EnquiryControlMissing.Exception).Enabled = true;
			}
			else 
			{
				enquiryForm1.GetControl("dteMSStage" + i.ToString() + "Due",EnquiryControlMissing.Exception).Enabled = false;
			}

			if (Convert.ToString(enquiryForm1.GetIBasicEnquiryControl2("lblMSStage" + i.ToString() + "Desc",EnquiryControlMissing.Exception).Value) == "") 
			{
				enquiryForm1.GetControl("lblMSStage" + i.ToString() + "Desc",EnquiryControlMissing.Exception).Visible=false;
				enquiryForm1.GetControl("dteMSStage" + i.ToString() + "Due",EnquiryControlMissing.Exception).Visible=false;
				enquiryForm1.GetControl("lblMSStage" + i.ToString() + "Achieved",EnquiryControlMissing.Exception).Visible=false;
				enquiryForm1.GetControl("chkAchieved" + i.ToString(),EnquiryControlMissing.Exception).Visible=false;
				enquiryForm1.GetControl("txtDaysLeft" + i.ToString(),EnquiryControlMissing.Exception).Visible=false;
				enquiryForm1.GetControl("txtCalc" + i.ToString(),EnquiryControlMissing.Exception).Visible=false;
				return;
			}
			if (ConvertDef.ToInt32(enquiryForm1.GetIBasicEnquiryControl2("txtDaysLeft" + i.ToString(),EnquiryControlMissing.Exception).Value,0) < 0) 
			{
				enquiryForm1.GetControl("lblMSStage" + i.ToString() + "Desc",EnquiryControlMissing.Exception).ForeColor = System.Drawing.Color.Red;
				enquiryForm1.GetControl("dteMSStage" + i.ToString() + "Due",EnquiryControlMissing.Exception).ForeColor = System.Drawing.Color.Red;
				enquiryForm1.GetControl("lblMSStage" + i.ToString() + "Achieved",EnquiryControlMissing.Exception).ForeColor = System.Drawing.Color.Red;
				enquiryForm1.GetControl("chkAchieved" + i.ToString(),EnquiryControlMissing.Exception).ForeColor = System.Drawing.Color.Red;
				enquiryForm1.GetControl("txtDaysLeft" + i.ToString(),EnquiryControlMissing.Exception).ForeColor = System.Drawing.Color.Red;
			}
			else
			{
				if (enquiryForm1.GetControl("lblMSStage" + i.ToString() + "Desc",EnquiryControlMissing.Exception).Visible)
				{
					enquiryForm1.GetControl("lblMSStage" + i.ToString() + "Desc",EnquiryControlMissing.Exception).ForeColor = System.Drawing.SystemColors.ControlText;
					enquiryForm1.GetControl("dteMSStage" + i.ToString() + "Due",EnquiryControlMissing.Exception).ForeColor = System.Drawing.SystemColors.ControlText;
					enquiryForm1.GetControl("lblMSStage" + i.ToString() + "Achieved",EnquiryControlMissing.Exception).ForeColor = System.Drawing.SystemColors.ControlText;
					enquiryForm1.GetControl("chkAchieved" + i.ToString(),EnquiryControlMissing.Exception).ForeColor = System.Drawing.SystemColors.ControlText;
					enquiryForm1.GetControl("txtDaysLeft" + i.ToString(),EnquiryControlMissing.Exception).ForeColor = System.Drawing.SystemColors.ControlText;
				}
			}
		}

		private void CheckColorChange()
		{
			try
			{
				for (int i = 1; i<=30; i++)
				{
					CheckColorChange(i);

				}
			}
			catch{}
		}

		private void ButtonClick(object sender, OMSToolBarButtonClickEventArgs e)
		{
			try
			{
				bool msanswer;
				if (Convert.ToString(e.Button.Tag) == "NEW")
				{
					FWBS.Common.KeyValueCollection items = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(OMS.Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.SelectMilestone) ,new Size(300,400),null,null);
					if (items != null)
					{
						if (MessageBox.Show(Session.CurrentSession.Resources.GetMessage("NEWMILESTONE","Would you like to base the Milestone start from today?  Select No to create from the File Created date.","").Text,FWBS.OMS.Branding.APPLICATION_NAME,MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes) 
						{
							//Base Milestone creation off todays date (ie Yes!)
							msanswer = true ;

						}
						else
						{
							//Base Milestone creation off File Creation date
							msanswer = false;
						}
					
						_milestone.SetMileStonePlan(Convert.ToString(items["MSCode"].Value),msanswer);
						_omsfile.MilestonePlan = _milestone;
						ToBeRefreshed=true;
						RefreshItem();
						_isdirty = true;
					}
					
				}
				else if (Convert.ToString(e.Button.Tag) == "DELETE")
				{	
					if (Convert.ToBoolean(Session.CurrentSession.GetXmlProperty("EnableDeleteMilstoneRole",false)) &&
					    Session.CurrentSession.CurrentUser.IsInRoles("DELETEMILSTONE") == false)
                            throw new FWBS.OMS.Security.PermissionsException("EXPERMDENIED2", "Permission Denied", true, "");

                    if (MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DELETEMILESTONE","Are you sure you would like to remove the milestone plan from this file?  This will cause any entered information to be lost.  Are you sure you want to remove this plan?","").Text,FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes)
					{
						try
						{
							_deleting = true;
                            _isdirty = true;
                            _milestone.RemoveMileStonePlan();
                            Application.DoEvents();
                            enquiryForm1.Enquiry = null;
							menu.GetButton("Button1").Enabled = true;
							menu.GetButton("Button2").Enabled = false;
							menu.GetButton("btnPrint").Enabled = false;
						}
						finally
						{
							_deleting = false;
						}
					}	
				}
				else if (Convert.ToString(e.Button.Tag) == "PRINT")
				{
					if (enquiryForm1.Enquiry.Object is Milestones_OMS2K && ((Milestones_OMS2K)enquiryForm1.Enquiry.Object).OMSFile.IsNew)
					{
						MessageBox.ShowInformation("NOPRINT","You cannot print the milestone plan until you have created the %FILE%.","");
						return;
					}

					if (IsDirty)
					{
						DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DIRTYDATAPRM", "Changes have been detected to %1%, would you like to save?","",_milestone.MSDescription), FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
						switch (res)
						{
							case DialogResult.Yes:
								this.UpdateItem();
								break;
							case DialogResult.No:
								this.CancelItem();
								enquiryForm1.RenderControls();
								break;
							case DialogResult.Cancel:
								return;
						}					
					}
					FWBS.OMS.UI.Windows.Services.Reports.OpenReport("RPTFILMSTPLAN",_omsfile,null,true,this.ParentForm);
				}
				else if (Convert.ToString(e.Button.Tag) == "REFRESH")
				{
					if (IsDirty)
					{
						DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DIRTYDATAPRM", "Changes have been detected to %1%, would you like to save?","",_milestone.MSDescription), FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
						switch (res)
						{
							case DialogResult.Yes:
                                this.UpdateItem();
								break;
							case DialogResult.No:
								this.CancelItem();
								enquiryForm1.RenderControls();
								break;
							case DialogResult.Cancel:
								return;
						}					
					}
					
					this.ToBeRefreshed = true;
					this.RefreshItem();
				}
				else if (Convert.ToString(e.Button.Tag) == "RECALCULATE")
				{
					_milestone.Recalculate();
				}
			}
			catch(Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}

		}

		private void ucMilestones_ParentChanged(object sender, System.EventArgs e)
		{
			menu.GetButton("Button3").Visible = false;
			if (Parent is FWBS.OMS.UI.Windows.EnquiryForm)
			{
				_enqForm = Parent as EnquiryForm;
				if (_enqForm.Enquiry.Object is OMSFile)
				{
					Connect((OMSFile)_enqForm.Enquiry.Object);
					_enqForm.PageChanged -=new PageChangedEventHandler(_enqForm_PageChanged2);
					_enqForm.PageChanged -=new PageChangedEventHandler(_enqForm_PageChanged);
					_enqForm.PageChanged += new PageChangedEventHandler(_enqForm_PageChanged);
					RefreshItem();
				}
				else if (_enqForm.Enquiry.Object is OMSDocument)
				{
					Connect(((OMSDocument)_enqForm.Enquiry.Object).OMSFile);
					_enqForm.PageChanged -=new PageChangedEventHandler(_enqForm_PageChanged);
					_enqForm.PageChanged -=new PageChangedEventHandler(_enqForm_PageChanged2);
					_enqForm.PageChanged += new PageChangedEventHandler(_enqForm_PageChanged2);
					RefreshItem();
					menu.GetButton("Button3").Visible = true;
					menu.GetButton("Button2").Visible = false;
				}
				DataRow row = _enqForm.GetSettings(this);
				_pgeName = Convert.ToString(row["quPage"]);
			}
			else
			{
				if (_enqForm != null)
				{
					_enqForm.PageChanged -=new PageChangedEventHandler(_enqForm_PageChanged);
				}
			}
		}

		private void _enqForm_PageChanged2(object sender, PageChangedEventArgs e)
		{
			if (_pgeName == e.PageName)
			{
				try
				{
					if (_omsfile.MilestonePlan == null || _omsfile.MilestonePlan.MSPlan == "")
					{
						if (e.Direction == EnquiryPageDirection.Back)
						{
							_enqForm.PreviousPage();
							return;
						}
						else if (e.Direction == EnquiryPageDirection.Next)
						{
							_enqForm.NextPage();
							return;
						}
					}
				}
				catch{}
			}
		}
		
		private void _enqForm_PageChanged(object sender, PageChangedEventArgs e)
		{
			if (_pgeName == e.PageName)
			{
				try
				{
					if (_omsfile.CurrentFileType.MilestoneActive == false)
					{
						if (e.Direction == EnquiryPageDirection.Back)
						{
							_enqForm.PreviousPage();
							return;
						}
						else if (e.Direction == EnquiryPageDirection.Next)
						{
							_enqForm.NextPage();
							return;
						}
					}
				}
				catch{}
			}
		}

        private void timRunFirst_Tick(object sender, EventArgs e)
        {
        }

        private void enquiryForm1_Rendered_1(object sender, EventArgs e)
        {
            Application.DoEvents();
            timRunFirst.Enabled = true;
        }

        private void enquiryForm1_NewOMSTypeWindow(object sender, NewOMSTypeWindowEventArgs e)
        {
            OnNewOMSTypeWindow(e);
        }
    }
}
