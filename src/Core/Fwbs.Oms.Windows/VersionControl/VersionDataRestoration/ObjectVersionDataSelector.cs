using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Data;

namespace FWBS.OMS.UI.Windows
{
    public partial class ObjectVersionDataSelector : FWBS.OMS.UI.Windows.BaseForm
    {
        private int version;
        private string type;
        private string code;
        internal IConnection connection;
        private DataTable currentversions; 
        VersionDataRestorationProcessor processor;

        public event EventHandler<RestorationCompletedEventArgs> RestorationCompleted;

        public ObjectVersionDataSelector(string Code, string Type, int Version, DataTable CurrentVersions)
        {
            InitializeComponent();

            code = Code;
            version = Version;
            type = SetObjectVersionType(Type);
            currentversions = CurrentVersions;
            connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            SetSearchListLabels();
            SetVersionHeaderDataSearchList();
            this.FormClosing += new FormClosingEventHandler(ObjectVersionDataSelector_FormClosing);
        }

        void ObjectVersionDataSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(processor != null)
                processor.RestorationCompleted -= new EventHandler<RestorationCompletedEventArgs>(processor_RestorationCompleted);
        }

        private void SetSearchListLabels()
        {
            this.eLabel1.Text = ResourceLookup.GetLookupText("OVDSCISLIST1") + " " + Convert.ToString(version) + " " + ResourceLookup.GetLookupText("OVDSCISLIST2") + " " + code.ToUpper();
            this.eLabel2.Text = ResourceLookup.GetLookupText("OVDSCISOBJECTS");
        }

        private void SetVersionHeaderDataSearchList()
        {
            try
            {
                Common.KeyValueCollection kvc = new Common.KeyValueCollection();
                kvc.Add("code", code);
                kvc.Add("version", version);
                kvc.Add("tablename", type);
                this.sHeader.SetSearchList("LOBJVERSIONDATA", null, kvc);
                sHeader.Search(true, true, true);
            }
            catch (Exception ex) { ErrorBox.Show(this, ex); }
        }

        private string SetObjectVersionType(string Type)
        {
            switch (Type)
            {
                case "EnquiryForm":
                    return VersionTables.EnquiryForm;
                case "SearchList":
                    return VersionTables.SearchList;
                case "DataList":
                    return VersionTables.DataList;
                case "Script":
                    return VersionTables.Script;
                case "Precedent":
                    return "not built yet";
                case "FileManagement":
                    return VersionTables.FileManagement;
            }
            return "";
        }

        private void sHeader_ItemHovered(object sender, EventArgs e)
        {
            if (sHeader.DataTable.Rows.Count > 0 && sHeader.dgSearchResults.VisibleRowCount > 0)
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(sHeader.CurrentItem()["versionID"].Value)))
                    SetObjectSetList(Convert.ToString(sHeader.CurrentItem()["versionID"].Value));
            }
        }

        private void SetObjectSetList(string versionID)
        {
            try
            {
                Common.KeyValueCollection kvc = new Common.KeyValueCollection();
                kvc.Add("versionID", versionID);
                this.sObjectSet.SetSearchList("LOBJVERSIONOBJS", null, kvc);
                this.sObjectSet.Search(false, false, false);
            }
            catch (Exception ex) { ErrorBox.Show(this, ex); }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable restorationtable = null;
                if (!this.chkRevertAll.Checked)
                {
                    if (System.Windows.Forms.MessageBox.Show(ResourceLookup.GetLookupText("OVDSONLYONEOBJ1") + "\n\n" + ResourceLookup.GetLookupText("OVDSONLYONEOBJ2"), ResourceLookup.GetLookupText("OVDSMSGCAPTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        restorationtable = BuildSelectionTable();
                }
                else
                    restorationtable = sObjectSet.DataTable;

                if (restorationtable != null && restorationtable.Rows.Count > 0)
                    ExecuteRestorationProcess(restorationtable);
            }
            catch (Exception ex) { ErrorBox.Show(this, ex); }
        }

        private void ExecuteRestorationProcess(DataTable restorationtable)
        {
            try
            {
                processor = new VersionDataRestorationProcessor(currentversions, restorationtable);
                processor.RestorationCompleted += new EventHandler<RestorationCompletedEventArgs>(processor_RestorationCompleted);
                processor.ProcessVersionData();
            }
            catch (Exception ex) { ErrorBox.Show(this, ex); }
        }

        private void processor_RestorationCompleted(object sender, RestorationCompletedEventArgs e)
        {
            if (this.RestorationCompleted != null)
            {
                UpdateNoticeBoard(e.dtNoticeBoardData); 
                RestorationCompleted(this, e);
            }
            processor.RestorationCompleted -= new EventHandler<RestorationCompletedEventArgs>(processor_RestorationCompleted);
        }

        private DataTable BuildSelectionTable()
        {
            DataTable dt = null;
            if (sObjectSet.SelectedItems.Length > 0)
            {
                dt = sObjectSet.DataTable.Clone();
                foreach (Common.KeyValueCollection kvc in sObjectSet.SelectedItems)
                {
                    dt.Rows.Add(
                                Convert.ToString(kvc["ObjectType"].Value),
                                Convert.ToString(kvc["Code"].Value),
                                Convert.ToString(kvc["VersionNumber"].Value),
                                ConvertObjectTypeToTableName(Convert.ToString(kvc["ObjectType"].Value))
                    );
                }
            }
            return dt;
        }

        private string ConvertObjectTypeToTableName(string objecttype)
        {
            switch(objecttype)
            {
                case "Enquiry Form":
                    return VersionTables.EnquiryForm;
                case "Search List":
                    return VersionTables.SearchList;
                case "Data List":
                    return VersionTables.DataList;
                case "Script":
                    return VersionTables.Script;
                case "File Management":
                    return VersionTables.FileManagement;
                default:
                    return "";
            }
        }

        private void UpdateNoticeBoard(DataTable dtNoticeBoardData)
        {
            this.NoticeBoard.Value = ""; 
            if (dtNoticeBoardData != null && dtNoticeBoardData.Rows.Count > 0)
            {
                this.NoticeBoard.ForeColor = Color.Red;
                foreach (DataRow r in dtNoticeBoardData.Rows)
	            {
                    if (ConvertDef.ToInt32(r["RestoredTo"], 0) == ConvertDef.ToInt32(r["RestoredFrom"], 0))
                        AddNonRestorationMessage(r);
                    else
                        AddRestorationMessage(r);
	            }
            }
        }

        private void AddRestorationMessage(DataRow r)
        {
            this.NoticeBoard.Value += Convert.ToString(r["ObjectType"]) + " - "
                       + Convert.ToString(r["Code"])
                       + " - restored to version "
                       + Convert.ToString(r["RestoredTo"])
                       + " from version "
                       + Convert.ToString(r["RestoredFrom"])
                       + "\r\n";
        }

        private void AddNonRestorationMessage(DataRow r)
        {
            this.NoticeBoard.Value += Convert.ToString(r["ObjectType"]) + " - "
                       + Convert.ToString(r["Code"])
                       + " - was not restored as it is the same version as the current production version."
                       + "\r\n";
        }

        private void ObjectVersionDataSelector_Load(object sender, EventArgs e)
        {

        }
    }
}
