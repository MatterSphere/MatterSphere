using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Interfaces;
using FWBS.OMS.Security.Permissions;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.Addin.Security.Windows
{
    public partial class ucSecurity : FWBS.OMS.UI.Windows.ucBaseAddin
    {
        private string CurrentGroupID;
        private ObjectPolicy policy;

        public ucSecurity()
        {
            InitializeComponent();

            ucSearchControl1.SearchCompleted += new SearchCompletedEventHandler(ucSearchControl1_SearchCompleted);
            this.Dirty += new EventHandler(ucSecurity_Dirty);
            if (Session.CurrentSession.IsConnected)
            {
                policy = new ObjectPolicy();
                eInformation1.Text = Session.CurrentSession.Resources.GetResource("SECTIPAVAILPERM", "Context Help connected with the Available Permissions", "").Text;
            }
        }

        void ucSecurity_Dirty(object sender, EventArgs e)
        {
            _isdirty = true;
        }

        private void ucSearchControl1_SearchCompleted(object sender, SearchCompletedEventArgs e)
        {
            if (ucSearchControl1.SearchList.ResultCount > 0)
                ucSearchControl1_ItemHovered(sender, e);
            else
                pnlPermissions.Controls.Clear();

            if (Convert.ToString(pnlPermissions.Tag) == "OFF")
                ucSearchControl1.ToolBar.Enabled = false;
        }

        private Permissions perms = null;
        private Client _client;
        private OMSFile _file;
        private Contact _contact;
        private OMSDocument _document;
        private object _obj;
        private FWBS.OMS.UI.Windows.Services.Wizards wz;
        private DataTable dataPerms;
        private string oldGroupName;
        private bool _isdirty = false;


        #region IOMSTypeAddin Implementation
        public override bool IsDirty
        {
            get
            {
                return _isdirty;
            }
        }

        public override void SelectItem()
        {
            base.SelectItem();
            ucSearchControl1.Search();
        }

        public bool Connect(OMSDocument obj)
        {
            _document = obj;
            perms = new Permissions(obj);
            if (_document == null)
                return false;
            else
            {
                ToBeRefreshed = true;
                ucSearchControl1.Tag = obj;
                _obj = obj;
                RefreshItem();
                return true;
            }
        }

        private string _type;
        private string _code;
        public bool Connect(string Type, string Code)
        {
            _type = Type;
            _code = Code;
            perms = new Permissions(Type, Code);
            ToBeRefreshed = true;
            ucSearchControl1.Tag = Type;
            chkOverright.Visible = false;
            RefreshItem();
            return true;
        }

        public override bool Connect(IOMSType obj)
        {
            _client = obj as Client;
            _file = obj as OMSFile;
            _contact = obj as Contact;
            _document = obj as OMSDocument;
            perms = new Permissions(obj);

            if (obj != null)
            {
                try
                {
                    labLocked.Visible = false;
                    switch (obj.GetType().Name)
                    {
                        case "Client":
                            chkOverright.Visible = true;
                            new ClientPermission((Client)obj, StandardPermissionType.UpdatePermissions).Check();
                            break;
                        case "Contact":
                            chkOverright.Visible = false;
                            new ContactPermission((Contact)obj, StandardPermissionType.UpdatePermissions).Check();
                            break;
                        case "OMSFile":
                            chkOverright.Visible = true;
                            new FilePermission((OMSFile)obj, StandardPermissionType.UpdatePermissions).Check();
                            break;
                        case "OMSDocument":
                            chkOverright.Visible = false;
                            new DocumentPermission((OMSDocument)obj, StandardPermissionType.UpdatePermissions).Check();
                            break;
                    }
                }
                catch
                {
                    cmbPolicy.Enabled = false;
                    pnlPermissions.Tag = "OFF";
                    labLocked.Visible = true;
                }

                chkOverright.Enabled = FWBS.OMS.Security.SecurityManager.CurrentManager.IsGranted(new SystemPermission(StandardPermissionType.SecurityAdmin));
            }

            if (_client == null && _file == null && _contact == null && _document == null)
                return false;
            else
            {
                ucSearchControl1.Tag = obj;
                _obj = obj;
                ToBeRefreshed = true;
                return true;
            }

        }


        private string GetDisplayedObjectTypes(string objectType)
        {
            switch (objectType.ToUpper())
            {
                case "CLIENT":
                case "CLIENTTYPE":
                    return "'~CLIENT~' OR SecurableType = '~FILE~' OR SecurableType = '~DOCUMENT~'";
                case "CONTACT":
                case "CONTACTTYPE":
                    return "'~CONTACT~'";
                case "FILE":
                case "FILETYPE":
                    return "'~FILE~' OR SecurableType = '~DOCUMENT~'";
                case "DOCUMENT":
                    return "'~DOCUMENT~'";
                default:
                    throw new Exception("Object type not found"); //Should never happen
            }
        }

        internal bool CanSaveAfterPermissionCheck(DataRow[] objectPermissions)
        {
            if (objectPermissions != null && objectPermissions.Length > 0)
            {
                StringBuilder sb = PermissionsCheckFeedback(objectPermissions);
                sb.AppendLine(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("RUSSECREP_2", "All security permissions need to be set to save this configuration.\nPlease revise accordingly.", "").Text);

                SetPermissionItemsDescriptionColour();

                throw new Exception(sb.ToString());
            }

            SetPermissionItemsDescriptionColour();
            return true;
        }


        private void SetPermissionItemsDescriptionColour()
        {
            foreach (var control in pnlPermissions.Controls)
            {
                var permissionItem = (PermissionItem)control;

                if (permissionItem.MajorGroup)
                {
                    continue;
                }

                permissionItem.DescriptionColour = permissionItem.AllowOrDenyChecked
                                                            ? Color.FromArgb(0, 0, 0)
                                                            : Color.FromArgb(255, 0, 0);
            }
        }


        public override void UpdateItem()
        {
            if (objectExists() && !CanSaveAfterPermissionCheck(GetObjectPermissions()))
            {
                return;
            }
         
            if (chkOverright.Checked && objectExists())
            {
                DialogResult n = FWBS.OMS.UI.Windows.MessageBox.ShowYesNoCancel("RUSSECREP", "Are you sure you wish to Replace all the Children Permissions with this Policy Change?");
                if (n == DialogResult.Yes)
                {
                    perms.ResetChildPermissions();
                }
                else if (n == DialogResult.Cancel)
                {
                    return;
                }
            }

            if (perms != null)
            {
                if (objectExists())
                {
                    AllInternalGroupManagement();
                }

                if (perms.UpdatePermission(chkOverright.Checked) == true)
                {
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("SECRESTORE", "You have attempted to remove a User/Group that was inherited from its parent security. The group will be restored to the Parents Settings. If you want to remove inherited User/Group then please do this on the Parent. Otherwise Deny the User/Group access.");
                }

                this.ToBeRefreshed = true;
                this.RefreshItem();
                _isdirty = false;
            }

            chkOverright.Checked = false;

            base.UpdateItem();
            this.RefreshItem();
        }

        private static StringBuilder PermissionsCheckFeedback(DataRow[] rows)
        {
            StringBuilder sb = new StringBuilder("The following Users or Groups have empty permissions:\n").AppendLine();
            foreach (DataRow r in rows)
            {
                string name = String.IsNullOrEmpty(FWBS.OMS.CodeLookup.GetLookup("SECGROUPS", r["GroupName"].ToString())) ? r["GroupName"].ToString() : FWBS.OMS.CodeLookup.GetLookup("SECGROUPS", r["GroupName"].ToString());
                if (!sb.ToString().Contains(name))
                    sb.AppendLine(name);
            }
            sb.Append("\n");
            return sb;
        }

        private bool objectExists()
        {
            return _obj != null;
        }


        #region "All Internal Group Management"

        /// <summary>
        /// Checks the permission parties on the object and decides if the AllInternal group should be added or removed
        /// </summary>
        private void AllInternalGroupManagement()
        {
            DataTable dt = perms.ListObjectParties();

            //Scenario 1 - ADD 'All Internal' IF only External Users exist
            if (dt.Select("AccessType = 'EXTERNAL'").Length > 0 && dt.Select("AccessType = 'INTERNAL'").Length == 0)
            {
                //Scenario 1b - Do not add if we only have inherited external users 
                if (!(dt.Select("(Inherited = 'C' OR Inherited = 'F') AND AccessType = 'EXTERNAL'").Length == TotalNumberOfPartiesAfterDeletion(dt)))
                {
                    System.Diagnostics.Debug.WriteLine("Scenario 1b - ADD 'All Internal' IF only External Users exist");

                    if (dataPerms != null) dataPerms.RowChanged -= new DataRowChangeEventHandler(Permissions_RowChanged);
                    perms.AddPermission("DF08633D-9262-489F-B7F1-9A4FC56B41BC;AllInternal;51");
                    if (dataPerms != null) dataPerms.RowChanged += new DataRowChangeEventHandler(Permissions_RowChanged);
                }
            }
            //Scenario 2 - REMOVE 'All Internal' IF 'All Internal' exists with Internal User/Group(s)
            else if (dt.Select("AccessType = 'INTERNAL'").Length > 1 && dt.Select("GroupID = 'DF08633D-9262-489F-B7F1-9A4FC56B41BC'").Length > 0)
            {
                System.Diagnostics.Debug.WriteLine("Scenario 2 - REMOVE 'All Internal' IF 'All Internal' exists with Internal User/Group(s)");

                if (dataPerms != null) dataPerms.RowChanged -= new DataRowChangeEventHandler(Permissions_RowChanged);
                perms.DeletePermission("DF08633D-9262-489F-B7F1-9A4FC56B41BC");
                if (dataPerms != null) dataPerms.RowChanged += new DataRowChangeEventHandler(Permissions_RowChanged);
                oldGroupName = "";
            }

            //Scenario 3 - ADD 'All Internal' IF only External users and 'Selected Matters Only' items exist in the list
            if (NumberOfSelectedMattersOnlyItems(dt) > 0 && TotalNumberOfPartiesAfterDeletion(dt) > 0 && dt.Select("AccessType = 'EXTERNAL'").Length > 0 && (TotalNumberOfPartiesAfterDeletion(dt) == NumberOfSelectedMattersOnlyItems(dt) + dt.Select("AccessType = 'EXTERNAL'").Length))
            {
                System.Diagnostics.Debug.WriteLine("Scenario 3 - ADD 'All Internal' IF only External users and 'Selected Matters Only' items exist in the list");

                if (dataPerms != null) dataPerms.RowChanged -= new DataRowChangeEventHandler(Permissions_RowChanged);
                perms.AddPermission("DF08633D-9262-489F-B7F1-9A4FC56B41BC;AllInternal;51");
                if (dataPerms != null) dataPerms.RowChanged += new DataRowChangeEventHandler(Permissions_RowChanged);
            }

            //Scenario 4 - REMOVE 'All Internal' IF it is the only item in the list
            if ((dt.Rows.Count == 1 || TotalNumberOfPartiesAfterDeletion(dt) == 1) && dt.Select("GroupID = 'DF08633D-9262-489F-B7F1-9A4FC56B41BC'").Length > 0)
            {
                System.Diagnostics.Debug.WriteLine("Scenario 4 - REMOVE 'All Internal' IF it is the only item in the list");

                if (dataPerms != null) dataPerms.RowChanged -= new DataRowChangeEventHandler(Permissions_RowChanged);
                perms.DeletePermission("DF08633D-9262-489F-B7F1-9A4FC56B41BC");
                if (dataPerms != null) dataPerms.RowChanged += new DataRowChangeEventHandler(Permissions_RowChanged);
                oldGroupName = "";
            }

        }


        private int NumberOfSelectedMattersOnlyItems(DataTable Parties)
        {
            if (!Parties.Columns.Contains("SelectedMattersOnly"))
                return 0;
            else
                return Parties.Select("SelectedMattersOnly = 1").Length;
        }

        private int DeletedParties(DataView Parties)
        {
            Parties.RowStateFilter = DataViewRowState.Deleted;
            return Parties.Count;
        }

        private int TotalNumberOfPartiesAfterDeletion(DataTable Parties)
        {
            return Parties.Rows.Count - DeletedParties(Parties.DefaultView);
        }

        #endregion





        public override void CancelItem()
        {
            _isdirty = false;
        }

        public override void RefreshItem()
        {
            if ((_client != null || _contact != null || _document != null || _file != null || _type != "") && ToBeRefreshed)
            {
                KeyValueCollection param = new KeyValueCollection();
                if (ucSearchControl1.Tag != null && perms != null)
                    ucSearchControl1.SetSearchList("SCHSECACTUSERS", false, perms, param);
                ucSearchControl1.Tag = null;
                perms.Refresh();
                ucSearchControl1.Search(false, true, true);
                ucSearchControl1.ShowPanelButtons();

                cmbPolicy.AddItem(policy.ListPolicies(true), "PolicyID", "PolicyDescription");
                ToBeRefreshed = false;
            }
        }
        #endregion

        private void ucSearchControl1_SearchListLoad(object sender, EventArgs e)
        {
            ucSearchControl1.SearchButtonCommands += new SearchButtonEventHandler(ucSearchControl1_SearchButtonCommands);
        }

        private void ucSearchControl1_SearchButtonCommands(object sender, SearchButtonEventArgs e)
        {
            try
            {
                if (perms == null) return;
                if (e.ButtonName == "cmdAdd")
                {
                    if (!CanSaveAfterPermissionCheck(GetObjectPermissions()))
                        return;

                    UserGroups ug = new UserGroups();
                    DataTable data = ug.ListActiveUserGroupsAndUsers();

                    FWBS.OMS.EnquiryEngine.Enquiry enq = FWBS.OMS.EnquiryEngine.Enquiry.GetEnquiry("SCRSELUSRGRP", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, new FWBS.Common.KeyValueCollection());
                    wz = new FWBS.OMS.UI.Windows.Services.Wizards(enq);
                    wz.EnquiryForm.GetIListEnquiryControl("lstGroups").AddItem(data);
                    DataTable ret = wz.Show() as DataTable;
                    if (ret != null)
                    {
                        if (dataPerms != null) dataPerms.RowChanged -= new DataRowChangeEventHandler(Permissions_RowChanged);
                        perms.AddPermission(Convert.ToString(ret.Rows[0]["lstGroups"]));
                        if (dataPerms != null) dataPerms.RowChanged += new DataRowChangeEventHandler(Permissions_RowChanged);
                    }

                    wz = null;
                    ucSearchControl1.Search();
                    OnDirty(this, EventArgs.Empty);
                    this.UpdateItem();
                }
                else if (e.ButtonName == "cmdDelete")
                {
                    if (!CanSaveAfterPermissionCheck(GetObjectPermissions(false)))
                        return;

                    bool canDelete = true;
                    
                    //Check needed for configurable type level
                    if (objectExists())
                    {
                        bool isClientLevel = _obj.GetType().Name.ToUpper() == "CLIENT";
                        if (isClientLevel)
                        {
                            DataTable dt = perms.ListObjectParties();
                            bool isInternalUserOrGroup = (ucSearchControl1.CurrentItem()["AccessType"].Value.ToString().ToUpper() == "INTERNAL" && ucSearchControl1.CurrentItem()["SelectedMattersOnly"].Value.ToString().ToUpper() != "1");
                            bool isDeletingInternalUserOrGroupLeavingSelectedMattersOnly = _obj.GetType().Name.ToUpper() == "CLIENT" && isInternalUserOrGroup && NumberOfSelectedMattersOnlyItems(dt) > 0 && ((dt.Rows.Count - 1) == NumberOfSelectedMattersOnlyItems(dt));
                            bool isDeletingLastInternalUserOrGroup = dt.Rows.Count == 1 && isInternalUserOrGroup;

                            //Prompt needed IF deletion of user/group will leave 'Selected Matters Only' entries on Client Level OR IF deletion user/group leaves no entries (Everyone - Internal Users)
                            if (isDeletingLastInternalUserOrGroup || isDeletingInternalUserOrGroupLeavingSelectedMattersOnly)
                            {
                                DialogResult warning = FWBS.OMS.UI.Windows.MessageBox.ShowYesNoCancel("ADVSECDELGRPMTR", "Deleting this user/group will also delete it from any matters it is on.\nThis may cause some matters to have no users able to access them.\nIf you don’t want this to happen you should add another user or group first.\n\nSelect Yes to continue with the deletion or No to resolve the issue first.");
                                if (warning == DialogResult.No || warning == DialogResult.Cancel)
                                    canDelete = false;
                            }
                        }
                    }


                    if (canDelete)
                    {
                        dataPerms.RowChanged -= new DataRowChangeEventHandler(Permissions_RowChanged);
                        perms.DeletePermission(Convert.ToString(ucSearchControl1.SelectedItems[0]["GroupID"].Value));
                        dataPerms.RowChanged += new DataRowChangeEventHandler(Permissions_RowChanged);

                        oldGroupName = "";
                        ucSearchControl1.Search();
                        OnDirty(this, EventArgs.Empty);
                        this.UpdateItem();
                    }

                   
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }


        internal DataRow[] GetObjectPermissions(bool includeSelected = true)
        {
            string objectType;
            objectType = perms.ObjType == null ? perms.ObjectType.ToString() : perms.ObjType.GetType().Name.Replace("OMS", "");

            DataTable dt = dataPerms;
            string selectQuery = string.Format("MajorType is null AND (SecurableType = {0}) AND (Allow = 0 AND Deny = 0)", GetDisplayedObjectTypes(objectType));
            if (!includeSelected)
                selectQuery += (string.Format(" AND ID <> '{0}'", ucSearchControl1.CurrentItem()[0].Value.ToString()));

            DataRow[] rows = dt.Select(selectQuery);
            return rows;
        }

        private void ucSearchControl1_ItemHovered(object sender, EventArgs e)
        {
            try
            {
                if (ucSearchControl1.SelectedItems.Length > 0)
                {
                    // Variables
                    bool isAllInternal = ucSearchControl1.CurrentItem()["GroupName"].Value.ToString().ToUpper() == "ALLINTERNAL";
                    bool isExternal = ucSearchControl1.CurrentItem()["AccessType"].Value.ToString().ToUpper() == "EXTERNAL";
                    bool isEveryone = ucSearchControl1.CurrentItem()["GroupName"].Value.ToString().ToUpper() == "EVERYONE";

                    //Internal/Remote Policy (WI.7273)
                    if (isExternal)
                        cmbPolicy.AddItem(policy.ListRemotePolicies(false), "PolicyID", "PolicyDescription");
                    else
                    {
                        string filter = "IsRemote = 0 OR IsRemote IS NULL";
                        DataTable dt = policy.ListPolicies(true);
                        dt.DefaultView.RowFilter = filter;
                        dt = dt.DefaultView.ToTable();
                        cmbPolicy.AddItem(dt, "PolicyID", "PolicyDescription");
                    }

                    //Display the object policy value and permissions
                    CurrentGroupID = Convert.ToString(ucSearchControl1.SelectedItems[0]["GroupID"].Value);
                    ShowPermissions(CurrentGroupID);
                    this.cmbPolicy.ActiveChanged -= new System.EventHandler(this.cmbPolicy_ActiveChanged);
                    cmbPolicy.Value = Convert.ToString(ucSearchControl1.SelectedItems[0]["Policy"].Value);
                    this.cmbPolicy.ActiveChanged += new System.EventHandler(this.cmbPolicy_ActiveChanged);

                    //validation required on searchcontrol buttons and permission panel (WI.7272 & 7279)
                    if (isEveryone)
                    {
                        ucSearchControl1.ToolBar.Buttons["CMDDELETE"].Enabled = false;
                        cmbPolicy.Enabled = false;
                    }
                    else
                    {
                        bool canEdit = !(isAllInternal | isExternal);
                        ucSearchControl1.ToolBar.Buttons["CMDDELETE"].Enabled = canEdit;
                        cmbPolicy.Enabled = canEdit;
                        pnlPermissions.Enabled = canEdit;
                    }

                    //Ensures permissions are always hidden for the 'Internal Users' entry (Everyone)
                    foreach (Control c in pnlPermissions.Controls)
                        c.Visible = !isEveryone;
                }

            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
        }

        private void ShowPermissions(string GroupID)
        {
            if (perms == null) return;
            if (GroupID != oldGroupName)
            {
                oldGroupName = GroupID;
                if (dataPerms != null)
                    dataPerms.RowChanged -= new DataRowChangeEventHandler(Permissions_RowChanged);

                dataPerms = perms.ListObjectPartiesPermission(GroupID);
                int i = 0;
                if (pnlPermissions.Controls.Count == 0)
                {
                    foreach (DataRowView rv in dataPerms.DefaultView)
                    {
                        PermissionItem perm = new PermissionItem(rv, "Permission", "Allow", "Deny", "GroupName", "GroupName", "MajorType");
                        perm.Dock = DockStyle.Top;
                        pnlPermissions.Controls.Add(perm);
                        perm.Name = i.ToString();
                        perm.BringToFront();
                        perm.Click += new EventHandler(perm_Click);
                        perm.NextPermission += new EventHandler(perm_NextPermission);
                        perm.PreviousPermission += new EventHandler(perm_PreviousPermission);
                        if (pnlPermissions.Tag == null)
                        {
                            perm_Click(perm, EventArgs.Empty);
                        }
                        perm.AllowOrDenyCheckBoxChanged += new EventHandler(perm_AllowOrDenyCheckBoxChanged);
                        i++;
                    }
                }
                else
                {
                    foreach (DataRowView rv in dataPerms.DefaultView)
                    {
                        PermissionItem perm = pnlPermissions.Controls[i.ToString()] as PermissionItem;
                        if (perm != null)
                        {
                            perm.DataRowView = rv;
                            i++;
                        }
                    }
                }
                if (pnlPermissions.Controls.Count > 0)
                {
                    labDesc.Width = ((PermissionItem)pnlPermissions.Controls[0]).DescriptionWidth;
                    cmbPolicy.Enabled = true;
                    SetPermissionItemsDescriptionColour();
                }
                else
                    cmbPolicy.Enabled = false;
                dataPerms.RowChanged += new DataRowChangeEventHandler(Permissions_RowChanged);
                pnlPermissions_SizeChanged(this, EventArgs.Empty);
            }
        }


        private void perm_AllowOrDenyCheckBoxChanged(object sender, EventArgs e)
        {
            SetPermissionItemsDescriptionColour();
        }


        void perm_PreviousPermission(object sender, EventArgs e)
        {
            if (perms == null) return;
            if (pnlPermissions.Tag != null)
            {
                int n = Convert.ToInt32(((PermissionItem)pnlPermissions.Tag).Name) - 1;
                if (n >= 0) perm_Click(pnlPermissions.Controls[n.ToString()], e);
            }
        }

        void perm_NextPermission(object sender, EventArgs e)
        {
            if (perms == null) return;
            if (pnlPermissions.Tag != null)
            {
                int n = Convert.ToInt32(((PermissionItem)pnlPermissions.Tag).Name) + 1;
                if (n < pnlPermissions.Controls.Count) perm_Click(pnlPermissions.Controls[n.ToString()], e);
            }
        }

        private void Permissions_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (perms == null) return;
            OnDirty(this, EventArgs.Empty);
            dataPerms.RowChanged -= new DataRowChangeEventHandler(Permissions_RowChanged);
            perms.ModifiedPolicy(CurrentGroupID);
            if (cmbPolicy.Value != DBNull.Value)
            {
                cmbPolicy.Value = DBNull.Value;
                perms.ChangePolicy(CurrentGroupID, "");
            }
            try
            {
                //Original rule to set child permissions for a major type
                if (Convert.ToString(e.Row["NodeLevel"]).IndexOf(".00") > -1)
                {
                    foreach (DataRowView rv in dataPerms.DefaultView)
                    {
                        if (rv.Row != e.Row && Convert.ToDouble(rv["NodeLevel"]) >= Convert.ToDouble(e.Row["NodeLevel"]))
                        {
                            rv["Allow"] = e.Row["Allow"];
                            rv["Deny"] = e.Row["Deny"];
                        }
                    }
                }
                else
                {
                    DataView fc = new DataView(dataPerms);
                    fc.RowFilter = "NodeLevel = " + Convert.ToInt32(e.Row["NodeLevel"]).ToString() + ".00";
                    if (fc.Count > 0)
                    {
                        fc[0]["Allow"] = false;
                        fc[0]["Deny"] = false;
                    }
                }

                //New permission rules
                if (Convert.ToBoolean(e.Row["Deny"]) && !Convert.ToBoolean(e.Row["Allow"]))
                {
                    SetFileAndDocumentControlToDenyIfViewClientIsDeny(e, 1.40, Convert.ToDouble(e.Row["NodeLevel"]));
                    SetDocumentControlToDenyIfViewFileIsDeny(e, 2.40, 3.00);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
            dataPerms.RowChanged += new DataRowChangeEventHandler(Permissions_RowChanged);
        }

        #region Permission control - Checkbox rules
        private void SetFileAndDocumentControlToDenyIfViewClientIsDeny(DataRowChangeEventArgs e, double initialNodeLevelToCheck, double startNodeLevelToDeny)
        {
            SetPermissionCheckboxesToDenyOnLowerLevels(e, 1.40, Convert.ToDouble(e.Row["NodeLevel"]));
        }

        private void SetDocumentControlToDenyIfViewFileIsDeny(DataRowChangeEventArgs e, double initialNodeLevelToCheck, double startNodeLevelToDeny)
        {
            SetPermissionCheckboxesToDenyOnLowerLevels(e, 2.40, 3.00);
        }
        
        private void SetPermissionCheckboxesToDenyOnLowerLevels(DataRowChangeEventArgs e, double initialNodeLevelToCheck, double startNodeLevelToDeny)
        {
            if (Convert.ToDouble(e.Row["NodeLevel"]) == initialNodeLevelToCheck) // && Convert.ToBoolean(e.Row["Deny"]))
            {
                foreach (DataRowView rv in dataPerms.DefaultView)
                {
                    if (rv.Row != e.Row && Convert.ToDouble(rv["NodeLevel"]) >= startNodeLevelToDeny)
                    {
                        rv["Allow"] = false;
                        rv["Deny"] = true;
                    }
                }
            }
        }
        #endregion


        private void perm_Click(object sender, EventArgs e)
        {
            if (pnlPermissions.Tag != sender)
            {
                PermissionItem p = ((PermissionItem)sender);
                if (pnlPermissions.Tag != null)
                    ((PermissionItem)pnlPermissions.Tag).IsSelected = false;
                pnlPermissions.Tag = p;
                p.IsSelected = true;
                this.eInformation1.Text = p.Help;
                p.Focus();
            }
        }

        private void ucSecurity_SizeChanged(object sender, EventArgs e)
        {
            if (pnlPermissions.Controls.Count > 0)
                labDesc.Width = ((PermissionItem)pnlPermissions.Controls[0]).DescriptionWidth;
        }

        private void cmbPolicy_ActiveChanged(object sender, EventArgs e)
        {
            if (!(cmbPolicy.Value == DBNull.Value || cmbPolicy.Value == null))
            {
                policy.GetPolicy(Convert.ToString(cmbPolicy.Value));
                perms.ModifiedPolicy(CurrentGroupID);
                perms.ChangePolicy(CurrentGroupID, Convert.ToString(cmbPolicy.Value));
                DataTable dt = policy.Permissions;
                dataPerms.RowChanged -= new DataRowChangeEventHandler(Permissions_RowChanged);
                foreach (DataRow row in dt.Rows)
                {
                    DataView dv = new DataView(dataPerms);
                    dv.RowFilter = "ID = '" + CurrentGroupID + "' AND Byte = " + Convert.ToString(row["Byte"]) + " and BitValue = " + Convert.ToString(row["BitValue"]);
                    dv[0]["Allow"] = row["Allow"];
                    dv[0]["Deny"] = row["Deny"];
                }
                dataPerms.RowChanged += new DataRowChangeEventHandler(Permissions_RowChanged);
                _isdirty = true;
            }
        }

        private void chkOverright_CheckedChanged(object sender, EventArgs e)
        {
            _isdirty = true;
        }

        /// <summary>
        /// Manage  headers panel layout to match columns with headers.
        /// </summary>
        private void pnlPermissions_SizeChanged(object sender, EventArgs e)
        {
            pnlFakeScroolBar.Visible = pnlPermissions.VerticalScroll.Visible;
        }
    }
}

