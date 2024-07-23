using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.Addin.Security.Windows
{
    public partial class ucSystemPolicy : FWBS.OMS.UI.Windows.Admin.ucEditBase2
    {
        private SystemPolicy _policy = new SystemPolicy();
        private readonly string _systemPolicyCodeLabel;

        public ucSystemPolicy()
        {
            InitializeComponent();
            eInformation1.Text = Session.CurrentSession.Resources.GetResource("SECTIPAVAILPERM", "Context Help connected with the Available Permissions", "").Text;
            _systemPolicyCodeLabel = Session.CurrentSession.Resources.GetResource("SYSPOLICYCODE", "System Policy Code", "").Text + " : ";
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
        }

        protected override void LoadSingleItem(string Code)
        {
            _policy.GetPolicy(Code);
            txtName.Text = _policy.Name;
            labSelectedObject.Text = _systemPolicyCodeLabel + _policy.Type;

            tbcEdit.Buttons[0].Enabled = true;
            txtActiveFilter.Text = "";
            ShowPermissions("");
            ShowEditor();
            this.IsDirty = false;
            tbSave.Enabled = (_policy.Type.StartsWith("GLOBALSYS") == false);
        }

        private DataTable dataPerms;

        private void ShowPermissions(string Filter)
        {
            if (dataPerms != null)
                dataPerms.RowChanged -= new DataRowChangeEventHandler(Permissions_RowChanged);

            dataPerms = _policy.Permissions;
            int i = 0;
            if (pnlPermissions.Controls.Count == 0)
            {
                foreach (DataRowView rv in dataPerms.DefaultView)
                {
                    PermissionItem perm = new PermissionItem(rv, "Permission", "Allow", "Deny", "GroupName", "Help", "MajorType");
                    perm.Dock = DockStyle.Top;
                    pnlPermissions.Controls.Add(perm, true);
                    perm.Name = i.ToString();
                    perm.BringToFront();
                    perm.Click += new EventHandler(perm_Click);
                    perm.NextPermission += new EventHandler(perm_NextPermission);
                    perm.PreviousPermission += new EventHandler(perm_PreviousPermission);
                    if (pnlPermissions.Tag == null)
                    {
                        perm_Click(perm, EventArgs.Empty);
                    }
                    if (Filter != "")
                    {
                        perm.Visible = (Convert.ToString(rv["Permission"]).ToLower().IndexOf(Filter.ToLower()) != -1);
                    }
                    else
                    {
                        perm.Visible = true;
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
                    if (Filter != "")
                    {
                        perm.Visible = (Convert.ToString(rv["Permission"]).ToLower().IndexOf(Filter.ToLower()) != -1);
                    }
                    else
                        perm.Visible = true;

                }
            }
            if (pnlPermissions.Controls.Count > 0)
                labDesc.Width = ((PermissionItem)pnlPermissions.Controls[0]).DescriptionWidth;
            dataPerms.RowChanged += new DataRowChangeEventHandler(Permissions_RowChanged);
            pnlPermissions_SizeChanged(this, EventArgs.Empty);
        }

        private void perm_PreviousPermission(object sender, EventArgs e)
        {
            if (_policy == null) return;
            if (pnlPermissions.Tag != null)
            {
                int n = 0;
                do
                {
                    n = Convert.ToInt32(((PermissionItem)pnlPermissions.Tag).Name) - 1;
                    if (n >= 0) perm_Click(pnlPermissions.Controls[n.ToString()], e);
                } while (pnlPermissions.Controls[n.ToString()] != null && pnlPermissions.Controls[n.ToString()].Visible == false && n >= 0);
            }
        }

        private void perm_NextPermission(object sender, EventArgs e)
        {
            if (_policy == null) return;
            if (pnlPermissions.Tag != null)
            {
                int n = 0;
                do
                {
                    n = Convert.ToInt32(((PermissionItem)pnlPermissions.Tag).Name) + 1;
                    if (n < pnlPermissions.Controls.Count) perm_Click(pnlPermissions.Controls[n.ToString()], e);
                } while (pnlPermissions.Controls[n.ToString()] != null && pnlPermissions.Controls[n.ToString()].Visible == false && n < pnlPermissions.Controls.Count);
            }
        }

        private void Permissions_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (_policy == null) return;
            if (tbSave.Enabled) IsDirty = true;
            dataPerms.RowChanged -= new DataRowChangeEventHandler(Permissions_RowChanged);
            try
            {
                if (ConvertDef.ToBoolean(e.Row["MajorType"], false) == true)
                {
                    foreach (DataRowView rv in dataPerms.DefaultView)
                    {
                        if (rv.Row != e.Row && Convert.ToString(rv["SecurableType"]).IndexOf(Convert.ToString(e.Row["SecurableType"])) >= 0)
                        {
                            rv["Allow"] = e.Row["Allow"];
                            rv["Deny"] = e.Row["Deny"];
                        }
                    }
                }
                else
                {
                    DataView fc = new DataView(dataPerms);
                    fc.RowFilter = "SecurableType = '" + Convert.ToString(e.Row["SecurableType"]) + "' AND MajorType = true";
                    if (fc.Count > 0)
                    {
                        fc[0]["Allow"] = false;
                        fc[0]["Deny"] = false;
                    }
                    fc.RowFilter = "SecurableType = 'System,' AND MajorType = true";
                    if (fc.Count > 0)
                    {
                        fc[0]["Allow"] = false;
                        fc[0]["Deny"] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
            dataPerms.RowChanged += new DataRowChangeEventHandler(Permissions_RowChanged);
        }

        private void perm_Click(object sender, EventArgs e)
        {
            if (pnlPermissions.Tag != sender)
            {
                PermissionItem p = ((PermissionItem)sender);
                if (pnlPermissions.Tag != null)
                    ((PermissionItem)pnlPermissions.Tag).IsSelected = false;
                pnlPermissions.Tag = p;
                p.IsSelected = true;
                p.Focus();
            }
        }

        protected override string SearchListName
        {
            get
            {
                return "SECSYSPOLICIES";
            }
        }

        protected override void Clone(string Code)
        {
            SystemPolicy poly = new SystemPolicy();
            string typecode = "";
            while (typecode == "")
            {               
                typecode = FWBS.OMS.UI.Windows.InputBox.Show(Session.CurrentSession.Resources.GetMessage("ADVSECNEWPOLCDE", "Please enter a Unique Policy Code (Max 15 Characters)", "").Text, FWBS.OMS.Branding.APPLICATION_NAME, "", 15);
                if (ObjectGroups.PolicyTypeExists(typecode) == true)
                {
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("ADVSECPOLEXISTS", "The Policy Code already exists as a System or an Object Policy. Please Try again");
                    typecode = "";
                }
            }
            if (typecode == FWBS.OMS.UI.Windows.InputBox.CancelText)
            {
                base.ShowList();
                base.Refresh();
                return;
            }

            poly.GetPolicy(Code);
            poly.Type = typecode;
            _policy.Clone(poly);
            txtName.Text = _policy.Name;
            labSelectedObject.Text = _systemPolicyCodeLabel + _policy.Type;
            tbcEdit.Buttons[0].Enabled = true;
            ShowPermissions("");
            ShowEditor();
            this.IsDirty = false;
        }

        protected override void NewData()
        {
            _policy.Create();
            tbSave.Enabled = true;
            txtName.Text = "";
            txtActiveFilter.Text = "";
            string typecode = "";
            while (typecode == "")
            {
                typecode = FWBS.OMS.UI.Windows.InputBox.Show(Session.CurrentSession.Resources.GetMessage("ADVSECNEWPOLCDE", "Please enter a Unique Policy Code (Max 15 Characters)", "").Text, FWBS.OMS.Branding.APPLICATION_NAME, "", 15);
                if (ObjectGroups.PolicyTypeExists(typecode) == true)
                {
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("ADVSECPOLEXISTS", "The Policy Code already exists as a System or an Object Policy. Please Try again");
                    typecode = "";
                }
            }
            if (typecode == FWBS.OMS.UI.Windows.InputBox.CancelText)
            {
                base.ShowList();
                base.Refresh();
                return;
            }

            _policy.Type = typecode;
            labSelectedObject.Text = _systemPolicyCodeLabel + typecode;
            ShowPermissions("");
            ShowEditor();
            this.IsDirty = false;
        }

        protected override void DeleteData(string Code)
        {
            try
            {
                _policy.GetPolicy(Code);
                if (_policy.Type.StartsWith("GLOBALSYS"))
                    throw new Exception("You cannot delete the Global System Policy ...");
                if (_policy.Assigned)
                    throw new Exception("This system policy is assigned to users/groups and cannot be deleted");
                _policy.Delete();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
            base.ShowList();
        }

        protected override bool UpdateData()
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                txtName.Focus();
                throw new Exception("The following required field must be used:\nName");
            }

            txtActiveFilter.Focus();
            _policy.Name = txtName.Text;
            _policy.Update();
            base.ShowList();
            base.Refresh();
            return true;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            labSelectedObject.Text = _systemPolicyCodeLabel + txtName.Text;
            if (tbSave.Enabled) IsDirty = true;
        }

        private void txtActiveFilter_TextChanged(object sender, EventArgs e)
        {
            ShowPermissions(txtActiveFilter.Text);
        }

        private void txtActiveFilter_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (pnlPermissions.Tag != null)
                {
                    PermissionItem perm = pnlPermissions.Tag as PermissionItem;
                    perm.Focus();
                }
            }
        }

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
                base.ShowList();
            }
        }

        protected override void ShowEditor(bool newObject = false)
        {
            base.ShowEditor();
            if (!String.IsNullOrEmpty(txtName.Text))
                base.HostingTab.Text = string.Format("{0} - {1}", OriginalTabText, txtName.Text);
            else
                 base.HostingTab.Text = string.Format("{0}", OriginalTabText);
        }

        private void perm_AllowOrDenyCheckBoxChanged(object sender, EventArgs e)
        {
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


        internal bool CanSaveAfterPermissionCheck()
        {
            if (!AllowOrDenyCheckBoxesCheckedOnEachRow())
            {
                var errorMessage = FWBS.OMS.Session.CurrentSession.Resources.GetMessage("RUSSECREP_2", "All security permissions need to be set to save this configuration.\nPlease revise accordingly.", "").Text;
                var errorCaption = FWBS.OMS.Session.CurrentSession.Resources.GetMessage("FURTHERACTNREQ", "Further Action Required", "").Text;
                FWBS.OMS.UI.Windows.MessageBox.Show(errorMessage, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                SetPermissionItemsDescriptionColour();
                return false;
            }

            SetPermissionItemsDescriptionColour();
            return true;
        }


        private bool AllowOrDenyCheckBoxesCheckedOnEachRow()
        {
            foreach (var item in pnlPermissions.Controls)
            {
                var permissionItem = (PermissionItem)item;

                if (permissionItem.MajorGroup)
                {
                    continue;
                }

                if (!permissionItem.AllowOrDenyChecked)
                {
                    return false;
                }
            }

            return true;
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