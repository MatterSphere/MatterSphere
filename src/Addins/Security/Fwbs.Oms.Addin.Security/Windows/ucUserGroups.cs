using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Data.Exceptions;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.Addin.Security.Windows
{
    public partial class ucUserGroups : FWBS.OMS.UI.Windows.Admin.ucEditBase2
    {
        private string _code = "";
        private UserGroups _usergroups = new UserGroups();

        public ucUserGroups()
        {
            InitializeComponent();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
        }

        protected override string SearchListName
        {
            get
            {
                return "SECGROUPS";
            }
        }

        protected override void DeleteData(string Code)
        {
            _usergroups.GetUserGroup(Code);
            _usergroups.Delete();
            base.Load();
        }

        protected override bool Restore(string Code)
        {
            _usergroups.GetUserGroup(Code);
            _usergroups.Restore();
            base.Load();
            return true;
        }

        protected override bool UpdateData()
        {
            enquiryForm1.UpdateItem();
            IsDirty = false;
            return true;
        }

        protected override void LoadSingleItem(string Code)
        {
            KeyValueCollection myparams = new KeyValueCollection();
            _code = Code;
            labSelectedObject.Text = string.Format("{0} - {1}", UserGroups.GetGroupName(Code), ResourceLookup.GetLookupText("UserGroup", "User Group", ""));
            myparams.Add("id", Code);
            enquiryForm1.Enquiry = EnquiryEngine.Enquiry.GetEnquiry("SCREDADGROUP", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, myparams);
            enquiryForm1.GetControl("btnSync").Click += new EventHandler(ucUserGroups_Click);
            ShowEditor();
            IsDirty = false;
        }

        private void OnDirty(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void ucUserGroups_Click(object sender, EventArgs e)
        {
            try
            {
                UserGroups groups = enquiryForm1.Enquiry.Object as UserGroups;
                if (groups != null)
                {
                    string policy = Convert.ToString(enquiryForm1.GetIBasicEnquiryControl2("ApplySecUserPol1", EnquiryControlMissing.Exception).Value);
                    ActiveDirectoryServices.AddGroup(groups.DistinguishedName, policy);
                    enquiryForm1.GetIListEnquiryControl("UserSelections").AddItem(groups.ListUsersForThisGroup());
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("SYNCSUCC", "Users successfully synchronised with Active Directory...");
                }
            }
			catch (ConnectionException cex)
			{
				SqlException sqlex = cex.InnerException as SqlException;
				if (sqlex != null)
					ErrorBox.Show(new OMSException2("ERRSYNCERROR", "Cannot synchronise this Group with Active Directory", sqlex));
				else	
					ErrorBox.Show(cex);
			}
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
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

            if (!String.IsNullOrEmpty(UserGroups.GetGroupName(_code)))
                base.HostingTab.Text = string.Format("{0} - {1}", OriginalTabText, UserGroups.GetGroupName(_code));
            else
                base.HostingTab.Text = string.Format("{0}", OriginalTabText);

        }

    }
}