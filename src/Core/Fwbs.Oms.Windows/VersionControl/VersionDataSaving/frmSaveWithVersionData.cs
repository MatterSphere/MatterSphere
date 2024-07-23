using System;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public partial class frmSaveWithVersionData : BaseForm
    {
        string comments = string.Empty;
        string destination;
        IVersionDataArchiver dataarchiver;
        string code;
        long version;
        ObjectLinkRetriever objectLinkRetriever;
        string objecttype;

        public event EventHandler<frmSaveWithVersionDataEventArgs> Closing;


        private frmSaveWithVersionData()
        {
            InitializeComponent();
        }


        public frmSaveWithVersionData(string destination, string code, long version, IVersionDataArchiver DataArchiver)
            : this()
        {
            this.destination = destination;
            this.dataarchiver = DataArchiver;
            this.code = code;
            this.version = version;
            objectLinkRetriever = new ObjectLinkRetriever();
            ProcessLinkedItems();
        }


        private void ProcessLinkedItems()
        {
            DataTable links = null;

            if (dataarchiver is EnquiryFormVersionDataArchiver)
            {
                links = GetLinks(new EnquiryFormComparer(code));
                objecttype = "EnquiryForm";
            }

            if (dataarchiver is ScriptVersionDataArchiver)
            {
                links = GetLinks(new ScriptComparer(code));
                objecttype = "Script";
            }

            if (dataarchiver is SearchListVersionDataArchiver)
            {
                links = GetLinks(new SearchListComparer(code));
                objecttype = "SearchList";
            }

            if (dataarchiver is PrecedentVersionDataArchiver)
            {
                links = GetLinks(new PrecedentComparer(code));
                objecttype = "Precedent";
            }

            if (dataarchiver is FileManagementVersionDataArchiver)
            {
                links = GetLinks(new FileManagementComparer(code));
                objecttype = "FileManagement";
            }

            if (links != null && links.Rows.Count > 0)
                PopulateLinkedItemList(links);
        }


        private DataTable GetLinks(IObjectComparison objectType)
        {
            var links = objectLinkRetriever.RetrieveObjectLinks(objectType, includeDataLists: false);
            return links;
        }


        private void PopulateLinkedItemList(DataTable dtLinks)
        {
            var linkeditemlist = new System.Text.StringBuilder();
            string version = ResourceLookup.GetLookupText("VERSION", "Version", "");
            foreach (DataRow r in dtLinks.Rows)
            {
                if (Convert.ToString(r["objectcode"]) != code || Convert.ToString(r["objecttype"]) != objecttype)
                {
                    linkeditemlist.AppendFormat("{0} - {1} - {2} : {3}\r\n",
                        FormatObjectType(Convert.ToString(r["objecttype"])), Convert.ToString(r["objectcode"]),
                        version, Convert.ToString(r["objectversion"]));
                }
            }
            if(linkeditemlist.Length > 0)
                this.LinkedItemList.Value = linkeditemlist.ToString();
        }


        private string FormatObjectType(string type)
        {
            string result = "";
            switch(type.ToUpper())
            {
                case "ENQUIRYFORM":
                    result = ResourceLookup.GetLookupText("ENQFORM", "Enquiry Form", "");
                    break;
                case "SEARCHLIST":
                    result = ResourceLookup.GetLookupText("SEARCHLIST", "Search List", "");
                    break;
                case "DATALIST":
                    result = ResourceLookup.GetLookupText("DATALIST", "Data List", "");
                    break;
                case "SCRIPT":
                    result = ResourceLookup.GetLookupText("SCRIPT", "Script", "");
                    break;
                case "PRECEDENT":
                    result = ResourceLookup.GetLookupText("PRECEDENT", "Precedent", "");
                    break;
            }
            return result;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            var saveEventArgs = new frmSaveWithVersionDataEventArgs { CancelClicked = true };
            OnClosing(saveEventArgs);
            this.Close();
        }


        private void OnClosing(frmSaveWithVersionDataEventArgs e)
        {
            if (Closing != null)
            {
                Closing(this, e);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable results;
            if (VersionDataSaver.VersionDataExistsInDB(destination, code, version, out results))
            {
                DialogResult answer = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DATAEXISTSMSG", "Data for this version (%1%) already exists in the tables which store version data.", "", version.ToString()).Text, Session.CurrentSession.Resources.GetResource("DATAEXISTS", "Version Data Already Exists", "").Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                btnCancel_Click(this, EventArgs.Empty);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtComments.Text))
            {
                MessageBox.Show(Session.CurrentSession.Resources.GetMessage("COMMREQMSG", "Please provide a comment advising of the changes made.", "").Text, Session.CurrentSession.Resources.GetResource("COMMREQ", "Comments Required", "").Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            var saveEventArgs = new frmSaveWithVersionDataEventArgs
            {
                CancelClicked = false,
                OKClicked = true,
                Comments = txtComments.Text
            };

            OnClosing(saveEventArgs);
            this.Close();
        }
    }


    public class frmSaveWithVersionDataEventArgs : EventArgs
    {
        public bool CancelClicked { get; set; }
        public bool OKClicked { get; set; }
        public string Comments { get; set; }
    }
}
