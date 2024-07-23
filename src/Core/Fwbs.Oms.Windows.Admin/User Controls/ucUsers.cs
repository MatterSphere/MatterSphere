using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucOMSTypes.
    /// </summary>
    public class ucUsers : ucEditBase2
	{
		#region Fields
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Disposal
		/// </summary>
		private ArrayList _dispose = new ArrayList();
		private FWBS.OMS.UI.Windows.ucOMSTypeDisplay ucOMSTypeDisplay;
		private string _code = "";
		private System.Windows.Forms.ToolBarButton tbrSpacer1;
		private System.Windows.Forms.ToolBarButton tbrUpgToFeeEarner;
		private System.Windows.Forms.ToolBarButton tbImport;
		private FWBS.OMS.User _user = null;

		/// <summary>
		/// Gets the Form OMS Type
		/// </summary>

		#endregion

		#region Constructors
		public ucUsers()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public ucUsers(IMainParent mainparent, Control editparent,FWBS.Common.KeyValueCollection Params) : base(mainparent,editparent,Params)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
            {
                Load();

                if (lstList.GetOMSToolBarButton("cmdConvert") != null)
                    lstList.ButtonEnabledRulesApplied += lstList_ButtonEnabledRulesApplied;
            }

            base.OnParentChanged(e);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ucOMSTypeDisplay = new FWBS.OMS.UI.Windows.ucOMSTypeDisplay();
			this.tbrSpacer1 = new System.Windows.Forms.ToolBarButton();
			this.tbrUpgToFeeEarner = new System.Windows.Forms.ToolBarButton();
			this.tbImport = new System.Windows.Forms.ToolBarButton();
			this.tpEdit.SuspendLayout();
			// 
			// tpList
			// 
			this.tpList.Name = "tpList";
			// 
			// tpEdit
			// 
			this.tpEdit.Controls.Add(this.ucOMSTypeDisplay);
			this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
			this.tpEdit.Name = "tpEdit";
			this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
			this.tpEdit.Controls.SetChildIndex(this.ucOMSTypeDisplay, 0);
			// 
			// pnlEdit
			// 
			this.pnlEdit.Name = "pnlEdit";
			// 
			// labSelectedObject
			// 
			this.labSelectedObject.Name = "labSelectedObject";
			// 
			// tbcEdit
			// 
			this.tbcEdit.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					   this.tbrSpacer1,
																					   this.tbrUpgToFeeEarner});
			this.tbcEdit.Name = "tbcEdit";
			this.tbcEdit.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbcEdit_ButtonClick);
			// 
			// tbSave
			// 
			this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
			// 
			// tbReturn
			// 
			this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
			// 
			// lstList
			// 
			this.lstList.DockPadding.All = 5;
			this.lstList.Name = "lstList";
			this.lstList.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.lstList_SearchButtonCommands);
			// 
			// ucOMSTypeDisplay
			// 
			this.ucOMSTypeDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucOMSTypeDisplay.InformationPanelVisible = true;
			this.ucOMSTypeDisplay.ipc_BackColor = System.Drawing.SystemColors.ControlDark;
			this.ucOMSTypeDisplay.ipc_Visible = true;
			this.ucOMSTypeDisplay.ipc_Width = 175;
			this.ucOMSTypeDisplay.Location = new System.Drawing.Point(0, 49);
			this.ucOMSTypeDisplay.Name = "ucOMSTypeDisplay";
			this.ucOMSTypeDisplay.SearchManagerCloseVisible = false;
            this.ucOMSTypeDisplay.InfoPanelCloseVisible = false;
			this.ucOMSTypeDisplay.SearchManagerVisible = false;
			this.ucOMSTypeDisplay.Size = new System.Drawing.Size(549, 334);
			this.ucOMSTypeDisplay.TabIndex = 2;
			this.ucOMSTypeDisplay.ToBeRefreshed = false;
			this.ucOMSTypeDisplay.Dirty += new System.EventHandler(this.OnDirty);
			// 
			// tbrSpacer1
			// 
			this.tbrSpacer1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbrUpgToFeeEarner
			// 
			this.tbrUpgToFeeEarner.ImageIndex = 52;
			this.BresourceLookup1.SetLookup(this.tbrUpgToFeeEarner, new FWBS.OMS.UI.Windows.ResourceLookupItem("UpgToFeeErnr", "Upgrade to Fee Earner", ""));
			// 
			// ucUsers
			// 
			this.DockPadding.All = 8;
			this.Name = "ucUsers";
			this.tpEdit.ResumeLayout(false);

		}
		#endregion

		#region Overrides
		protected override string SearchListName
		{
			get
			{
				return "ADMUSERS";
			}
		}

		
		protected override void LoadSingleItem(string Code)
		{
			_code = Code.Split('|')[0];
			_user = FWBS.OMS.User.GetUser(Convert.ToInt32(_code));
            tbrUpgToFeeEarner.Visible = "INTERNAL".Equals(_user.GetExtraInfo("AccessType") as string, StringComparison.InvariantCultureIgnoreCase);
            ucOMSTypeDisplay.Open(_user);
			labSelectedObject.Text = _user.FullName + " - " + FWBS.OMS.Session.CurrentSession.Resources.GetResource("USERSMGT","Users Management","").Text;
			ShowEditor(false);
			this.IsDirty=false;
		}

		protected override bool UpdateData()
		{
			ucOMSTypeDisplay.UpdateItem();
			ucOMSTypeDisplay.RefreshItem(true);
			return true;
		}

		protected override void NewData()
		{
			if (Services.Wizards.CreateUser() != null)
				lstList.Search();
		}

		protected override bool CancelData()
		{
			_user.Cancel();
			return true;
		}


		#endregion

		#region Private
		private void OnDirty(object sender, EventArgs e)
		{
			this.IsDirty=true;
		}
		#endregion

		private new void tbcEdit_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
            try
            {

                if (e.Button == tbrUpgToFeeEarner)
                {

                    this.UpdateData();
                    object ret = null;
                    DialogResult result = MessageBox.ShowYesNoQuestion("DOYOUUPGUSR", "Are you sure you wish to upgrade the User to a Fee Earner", false);

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            ret = Services.Wizards.CreateFeeEarner(_user);
                        }
                        catch (Exception ex)
                        {
                            ErrorBox.Show(ParentForm, ex);
                        }

                        if (ret is FeeEarner)
                        {
                            MessageBox.ShowInformation("USERUPGRADED", "User successfully upgraded to Fee Earner.");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
		}

        private void lstList_ButtonEnabledRulesApplied(object sender, System.EventArgs e)
        {
            string usrADID = lstList.CurrentItem()?["usrADID"]?.Value as string;
            if (usrADID == null || usrADID.IndexOf('\\') < 0)
                lstList.GetOMSToolBarButton("cmdConvert").Enabled = false;
        }

		private new void lstList_SearchButtonCommands(object sender, FWBS.OMS.UI.Windows.SearchButtonEventArgs e)
		{
			if(e.ButtonName == "cmdImport")
			{
				DialogResult result = MessageBox.ShowYesNoQuestion("DOYOUADIMPORT","You are about to import users from the network Active Directory. Do you wish to continue?",false);

				if(result == DialogResult.Yes)
				{
					try
					{
						//run the wizard
						DataTable dt = Services.Wizards.ImportADUsers();
						
						if(dt == null)
							return;

                        System.Text.StringBuilder builder = new System.Text.StringBuilder();
                        bool isAAD = Session.CurrentSession.ValidateConditional(null, new string[] { "IsLoginType(\"AAD\")" });

						foreach(DataRow row in dt.Rows)
						{
                            try
                            {
                                string userAlias = row["usrAlias"].ToString();
                                string adusername = row["usrADID"].ToString();
                                string principalName = row["usrPN"].ToString();
                                int recursion = 0;

                                User user = GetThisUser(userAlias, adusername, principalName, ref recursion);

                                if (user != null)
                                    continue;

                                if (recursion > 0)
                                    userAlias = userAlias + recursion.ToString();

                                user = new User(FWBS.OMS.UserType.GetUserType("STANDARD"));

                                //upate property values
                                foreach (DataColumn col in dt.Columns)
                                {
                                    if (col.ColumnName != "usrPN")
                                        user.SetExtraInfo(col.ColumnName, row[col.ColumnName]);
                                }

                                if (isAAD && !string.IsNullOrEmpty(principalName))
                                    user.ActiveDirectoryID = principalName;

                                user.Alias = userAlias;
                                user.Update();
                            }
                            catch (Exception)
                            {
                                builder.AppendLine(row["usrFullName"].ToString());
                            }
							
						}
						lstList.Search();

                        if (builder.Length > 0)
                        {
                            builder.Insert(0, "The following users were unable to be created:" + Environment.NewLine);
                            throw new Exception(builder.ToString());
                        }

					}
					catch(Exception ex)
					{
						ErrorBox.Show(ParentForm, ex);
					}
				}
			}
            else if (e.ButtonName == "cmdConvert")
            {
                try
                {
                    User user = User.GetUser(Convert.ToInt32(lstList.CurrentItem()["usrID"].Value));
                    if ("INTERNAL".Equals(user.GetExtraInfo("AccessType") as string, StringComparison.InvariantCultureIgnoreCase))
                    {
                        FWBS.Common.ActiveDirectoryInfo info = new FWBS.Common.ActiveDirectoryInfo();
                        string userPrincipalName = info.GetUserPrincipalName(user.ActiveDirectoryID);
                        if (userPrincipalName != null)
                        {
                            user.ActiveDirectoryID = userPrincipalName;
                            user.Update();
                            lstList.Search(true, false);
                        }
                    }
                    if (user.ID != Session.CurrentSession.CurrentUser.ID)
                    {
                        Session.CurrentSession.CurrentUsers.Remove(user.ID.ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ParentForm, ex);
                }
            }
		}

        private User GetThisUser(string userInitials, string adUserName, string principalName, ref int recursion)
        {
            try
            {
                User user = User.GetUser(userInitials);
                if (user == null)
                    return null;

                string userADID = user.ActiveDirectoryID;
                if (userADID.Equals(adUserName, StringComparison.InvariantCultureIgnoreCase) || userADID.Equals(principalName, StringComparison.InvariantCultureIgnoreCase))
                    return user;
            }
            catch (Security.InvalidOMSUserException)
            {
                return null;
            }
 
            recursion++;

            if (recursion > 99)
                return null;

            userInitials = userInitials + recursion.ToString();

            return GetThisUser(userInitials, adUserName, principalName, ref recursion);
        }

        // ************************************************************************************************
        //
        // CLOSE
        //
        // ************************************************************************************************

        protected override void CloseAndReturnToList()
        {
            if (base.IsDirty)
            {
                DialogResult? dr = base.IsObjectDirtyDialogResult();
                if (dr != DialogResult.Cancel)
                {
                    base.ShowList();
                }
            }
            else
            {
                ShowList();
            }
        }

	}
}
