using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Addin.Security.Business;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.Addin.Security.Windows
{
    public partial class ucPermissions : UserControl, FWBS.Common.UI.IBasicEnquiryControl2
    {
        private DataTable dataPerms;
        private IPermissions policy;
        private bool _isdirty = false;
        private bool _required = false;
        private string _policy = "";
        private bool _omsDesignMode = false;

        public ucPermissions()
        {
            if (Session.CurrentSession.AdvancedSecurity)
            {
                InitializeComponent();
                eInformation1.Text = Session.CurrentSession.Resources.GetResource("SECTIPAVAILPERM", "Context Help connected with the Available Permissions", "").Text;
            }
        }

        private void ShowPermissions()
        {
            if (dataPerms != null)
                dataPerms.RowChanged -= new DataRowChangeEventHandler(Permissions_RowChanged);

            dataPerms = policy.Permissions;
            int i = 0;
            if (pnlPermissions.Controls.Count == 0)
            {
                foreach (DataRowView rv in dataPerms.DefaultView)
                {
                    PermissionItem perm = new PermissionItem(rv, "Permission", "Allow", "Deny", "GroupName", "GroupName", "MajorType");
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
                labDesc.Width = ((PermissionItem)pnlPermissions.Controls[0]).DescriptionWidth;
            dataPerms.RowChanged += new DataRowChangeEventHandler(Permissions_RowChanged);
            pnlPermissions_SizeChanged(this, EventArgs.Empty);
        }

        void perm_PreviousPermission(object sender, EventArgs e)
        {
            if (pnlPermissions.Tag != null)
            {
                int n = Convert.ToInt32(((PermissionItem)pnlPermissions.Tag).Name) - 1;
                if (n >= 0) perm_Click(pnlPermissions.Controls[n.ToString()], e);
            }
        }

        void perm_NextPermission(object sender, EventArgs e)
        {
            if (pnlPermissions.Tag != null)
            {
                int n = Convert.ToInt32(((PermissionItem)pnlPermissions.Tag).Name) + 1;
                if (n < pnlPermissions.Controls.Count) perm_Click(pnlPermissions.Controls[n.ToString()], e);
            }
        }

        private void Permissions_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            dataPerms.RowChanged -= new DataRowChangeEventHandler(Permissions_RowChanged);
            if (cmbPolicy.Value != DBNull.Value)
            {
                cmbPolicy.Value = DBNull.Value;
            }
            policy.PolicyID = _policy = "";

            try
            {
                if (Convert.ToString(e.Row["NodeLevel"]).IndexOf(".00") > -1)
                {
                    foreach (DataRowView rv in dataPerms.DefaultView)
                    {
                        if (rv.Row != e.Row && ConvertDef.ToDouble(rv["NodeLevel"], 0) >= ConvertDef.ToDouble(e.Row["NodeLevel"], 0))
                        {
                            rv["Allow"] = e.Row["Allow"];
                            rv["Deny"] = e.Row["Deny"];
                        }
                    }
                }
                else
                {
                    DataView fc = new DataView(dataPerms);
                    fc.RowFilter = "NodeLevel = " + ConvertDef.ToInt32(e.Row["NodeLevel"],0).ToString() + ".00";
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
            OnChanged();
        }

        private void perm_AllowOrDenyCheckBoxChanged(object sender, EventArgs e)
        {
            IsDirty = true;
            OnActiveChanged();
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
            System.Diagnostics.Debug.WriteLine("cmbPolicy_ActiveChanged(object sender, EventArgs e)", "ADVSECURITY");
            UpdatePermissionsView();
        }

        private void UpdatePermissionsView()
        {
            if (cmbPolicy.Value != DBNull.Value)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("cmbPolicy.Value ({0})", cmbPolicy.Value), "ADVSECURITY");
                policy.PolicyID = Convert.ToString(cmbPolicy.Value);
                _policy = policy.PolicyID;
                ShowPermissions();
                this.IsDirty = true;
                OnActiveChanged();
                OnChanged();
            }
        }

        #region IBasicEnquiryControl2 Members

        public event EventHandler ActiveChanged;

        public event EventHandler Changed;

        public void OnActiveChanged()
        {
            if (ActiveChanged != null)
                ActiveChanged(this, EventArgs.Empty);
        }

        public void OnChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        [Browsable(false)]
        public int CaptionWidth
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }

        public object Control
        {
            get { return this; }
        }

        [Browsable(false)]
        public bool IsDirty
        {
            get
            {
                return _isdirty;
            }
            set
            {
                _isdirty = value;
            }
        }

        [Browsable(false)]
        public bool LockHeight
        {
            get { return false; }
        }
                        
        [Browsable(false)]
        public bool ReadOnly
        {
            get
            {
                return !this.Enabled;
            }
            set
            {
                this.Enabled = !value;
            }
        }

        [Category("Advanced")]
        public bool Required
        {
            get
            {
                return _required;
            }
            set
            {
                _required = value;
            }
        }

        public object Value
        {
            get
            {
                return string.IsNullOrEmpty(_policy) ? (object)DBNull.Value : _policy;
            }
            set
            {
                _policy = Convert.ToString(value);
            }
        }

        [Browsable(false)]
        public bool omsDesignMode
        {
            get
            {
                return _omsDesignMode;
            }
            set
            {
                _omsDesignMode = value;
            }
        }

        [Browsable(false)]
        public bool CaptionTop
        {
            get
            {
                return false;
            }
            set { }
        }

        public void ForceActiveChanged()
        {
            if (Session.CurrentSession.AdvancedSecurity)
                UpdatePermissionsView();
        }

        #endregion

        private FWBS.OMS.UI.Windows.EnquiryForm parent;
        private void pnlPermissions_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                parent = Parent as FWBS.OMS.UI.Windows.EnquiryForm;
                if (parent != null)
                {
                    if (parent.Enquiry.Object is User)
                    {
                        User user = (User)parent.Enquiry.Object;
                        policy = new UserPermissions(user);
                    }
                    else
                    {
                        UserGroups usergrp = parent.Enquiry.Object as UserGroups ?? new UserGroups();
                        policy = new GroupPermissions(usergrp);
                    }

                    SystemPolicy syspolicy = new SystemPolicy();
                    cmbPolicy.AddItem(syspolicy.ListPolicies(true), "PolicyID", "PolicyDescription");
                    cmbPolicy.ActiveChanged += new System.EventHandler(this.cmbPolicy_ActiveChanged);
                    cmbPolicy.Value = policy.PolicyID;

                    ShowPermissions();
                    parent.Enquiry.Updating += new CancelEventHandler(Enquiry_Updating);
                }
            }
            else if (parent != null)
            {
                parent.Enquiry.Updating -= new CancelEventHandler(Enquiry_Updating);
                parent = null;
            }
        }

        private void Enquiry_Updating(object sender, CancelEventArgs e)
        {
            _policy = policy.UpdatePermission();
            OnChanged();
            IsDirty = false;
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