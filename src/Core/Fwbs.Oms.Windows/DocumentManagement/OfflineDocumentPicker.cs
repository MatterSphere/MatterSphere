using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    internal partial class OfflineDocumentPicker : BaseForm
    {
        #region Fields

        private string initialfilter = String.Empty;
        private string exludefilter = "([DocType] <> 'RECEIPT' and [DocType] <> 'SMS')";
        private Dictionary<string, string> filters = new Dictionary<string, string>();
        private DataView vw;
        private List<System.IO.FileInfo> selected = new List<System.IO.FileInfo>();
        private ArrayList databases = new ArrayList();
        private ArrayList doctypes = new ArrayList();
        private bool loading = false;

        #endregion

        #region Constructors

        public OfflineDocumentPicker(bool changes) : base()
        {
            InitializeComponent();

            try
            {
                loading = true;

                Cursor = Cursors.WaitCursor;

                if (changes)
                {
                    chkChanged.Checked = true;
                    chkChanged.Tag = true;
                    filters.Add("CHANGES", "[HasChanged] = true");
                }

                PopulateList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loading = false;

                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Captured Events

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            ApplyImages();
        }

        private void OfflineDocumentPicker_Shown(object sender, EventArgs e)
        {
            SetDocumentDetails();
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue >= 21 && e.KeyValue <= 109 && e.KeyData != Keys.Up && e.KeyData != Keys.Down && e.KeyData != Keys.Left && e.KeyData != Keys.Right)
            {

                Char n = (Char)e.KeyValue;

                Application.DoEvents();
                if (txtFilter.Focused == false)
                {
                    txtFilter.Text = n.ToString();
                    txtFilter.Focus();
                    txtFilter.SelectionStart = 1;
                }
            }
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                listView1.Focus();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                listView1.Focus();
                e.Handled = true;
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private string dbvalue = string.Empty; 
        private void cboDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (loading == false)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    if (dbvalue != Convert.ToString(cboDatabase.SelectedValue))
                    {
                        Filter();

                        RefreshList();

                        dbvalue = Convert.ToString(cboDatabase.SelectedValue);
                    }
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private string dtvalue = string.Empty;
        private void cboDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading == false)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;

                    if (dtvalue != Convert.ToString(cboDocType.SelectedValue))
                    {
                        Filter();

                        RefreshList();

                        dtvalue = Convert.ToString(cboDocType.SelectedValue);
                    }
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                cboDatabase.ValueMember = "Key";
                cboDatabase.DisplayMember = "Value";
                cboDatabase.DataSource = databases;

                if (databases.Count <= 2)
                {
                    if (pnlDBFilter.Visible)
                        pnlDBFilter.Visible = false;
                }
                else
                {
                    if (!pnlDBFilter.Visible)
                        pnlDBFilter.Visible = true;
                }

                cboDocType.ValueMember = "Key";
                cboDocType.DisplayMember = "Value";
                cboDocType.DataSource = doctypes;

                if (doctypes.Count <= 2)
                {
                    if (pnlDTFilter.Visible)
                        pnlDTFilter.Visible = false;
                }
                else
                {
                    if (!pnlDTFilter.Visible)
                        pnlDTFilter.Visible = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (vw != null)
            {
                databases.Clear();
                doctypes.Clear();

                List<string> dbs = new List<string>();
                List<string> dts = new List<string>();

                foreach (DataRowView r in vw)
                {
                    string db = String.Format("{0}.{1}", r["server"], r["database"]);

                    if (!dbs.Contains(db))
                        dbs.Add(db);

                    string doctype = Convert.ToString(r["doctypedesc"]) + "|" + Convert.ToString(r["doctype"]);

                    if (!dts.Contains(doctype))
                        dts.Add(doctype);
                }

                dbs.Sort();
                dts.Sort();

                databases.Add(new KeyValuePair<string, string>(String.Empty, "(All)"));
                foreach (string s in dbs)
                {
                    databases.Add(new KeyValuePair<string, string>(s, s));
                }

                doctypes.Add(new KeyValuePair<string, string>(String.Empty, "(All)"));
                foreach (string s in dts)
                {
                    string[] split = s.Split('|');
                    doctypes.Add(new KeyValuePair<string, string>(split[1], split[0]));
                }
            }
        }


        private void chkChanged_CheckedChanged(object sender, EventArgs e)
        {
            if (loading == false)
            {
                bool old = Common.ConvertDef.ToBoolean(chkChanged.Tag, false);
                if (old == false && chkChanged.Checked)
                {
                    if (!filters.ContainsKey("CHANGES"))
                        filters.Add("CHANGES", "[HasChanged] = true");
                    chkChanged.Tag = true;
                    Filter();
                    RefreshList();
                }
                else if (old && chkChanged.Checked == false)
                {
                    if (filters.ContainsKey("CHANGES"))
                        filters.Remove("CHANGES");
                    chkChanged.Tag = false;
                    Filter();
                    RefreshList();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (loading == false)
            {
                bool old = Common.ConvertDef.ToBoolean(checkBox1.Tag, false);
                if (old == false && checkBox1.Checked)
                {
                    if (!filters.ContainsKey("CHECKEDOUT"))
                        filters.Add("CHECKEDOUT", "Len([docCheckedOutByName]) > 0");
                    checkBox1.Tag = true;
                    Filter();
                    RefreshList();
                }
                else if (old && checkBox1.Checked == false)
                {
                    if (filters.ContainsKey("CHECKEDOUT"))
                        filters.Remove("CHECKEDOUT");
                    checkBox1.Tag = false;
                    Filter();
                    RefreshList();
                }
            }

        }

        private void OfflineDocumentPicker_Load(object sender, EventArgs e)
        {
           

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading == false)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;

                    SetDocumentDetails();

                    ModifySelected();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex, "SelectedIndexChanged");
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            try
            {
                e.Item = AddItem(vw[e.ItemIndex].Row);
                if (e.ItemIndex == 0)
                    e.Item.Selected = true;
            }
            catch 
            {
                string[] vals = new string[listView1.Columns.Count];
                for (int ctr = 0; ctr < vals.Length; ctr++ )
                {
                    vals[ctr] = "?";
                }
                e.Item = new ListViewItem(vals);
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (loading == false)
            {
                timer1.Enabled = true;
                timer1.Stop();
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                timer1.Stop();
                timer1.Enabled = false;
                Filter();
                RefreshList();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (vw != null)
                {
                    ColumnHeader col = listView1.Columns[e.Column];

                    if (listView1.Sorting == SortOrder.Ascending)
                    {
                        listView1.Sorting = SortOrder.Descending;
                        vw.Sort = String.Format("{0} desc", Convert.ToString(col.Tag));
                    }
                    else
                    {
                        listView1.Sorting = SortOrder.Ascending;
                        vw.Sort = String.Format("{0} asc", Convert.ToString(col.Tag));
                    }

                    RefreshList();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        private void ListView_VirtualItemsSelectionRangeChanged(object sender, ListViewVirtualItemsSelectionRangeChangedEventArgs e)
        {
            if (loading == false)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;

                    SetDocumentDetails();

                    ModifySelected();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex, "VirtualSelectionRangeChanged");
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (loading == false)
            {
                if (MessageBox.ShowYesNoQuestion(@"Are you sure that you would like to delete the selected documents?
Only the local documents will be deleted.
Checked out documents will not be deleted.") == DialogResult.Yes)
                {
                    foreach (int idx in listView1.SelectedIndices)
                    {
                        DataRow r = listView1.Items[idx].Tag as DataRow;
                        if (r != null)
                        {
                            FWBS.OMS.DocumentManagement.Storage.StorageManager.CurrentManager.LocalDocuments.Remove(new System.IO.FileInfo(Convert.ToString(r["FileLocalPath"])));
                        }
                    }

                    FWBS.OMS.DocumentManagement.Storage.StorageManager.CurrentManager.LocalDocuments.Save();

                    PopulateList();
                    ModifySelected();
                }
            }
        }

        #endregion

        #region Properties

        public System.IO.FileInfo[] SelectedDocuments
        {
            get
            {
                return selected.ToArray();
            }
        }

        #endregion

        #region Methods


        private void ModifySelected()
        {
            selected.Clear();

            foreach (int idx in listView1.SelectedIndices)
            {
                DataRow r = listView1.Items[idx].Tag as DataRow;
                if (r != null)
                {
                    selected.Add(new System.IO.FileInfo(Convert.ToString(r["FileLocalPath"])));
                }
            }

            if (selected.Count > 0)
                btnOK.Text = "&Open";
            else
                btnOK.Text = "&OK";
        }


        private void SetDocumentDetails()
        {
            ucNavRichText1.ControlRich.Clear();

            ListViewItem itm = null;

            if (listView1.SelectedIndices.Count > 0)
                itm = listView1.Items[listView1.SelectedIndices[0]];
            else if (listView1.Items.Count > 0)
                itm = listView1.Items[0];

            if (itm != null)
            {
                DataRow r = itm.Tag as DataRow; 
                if (r != null)
                {
                    var documentDetailsData = GetDocumentDetailsPanelData(r);
                    var documentdetailsformatting = new Version2DocumentDetails();
                    documentdetailsformatting.Set(ucNavRichText1, documentDetailsData);
                    pnDetails.Expanded = true;
                    ucNavRichText1.Refresh();
                    pnDetails.Update();
                }
            }
           
        }


        private Dictionary<string, string> GetDocumentDetailsPanelData(DataRow r)
        {
            var documentDetailsData = new Dictionary<string, string> 
            {
                { "Document Ref",  DisplayableDocID(r) },
                { "Version", Convert.ToString(r["VerLabel"]) },
                { "Our Ref", Convert.ToString(r["ClientFileNo"]) },
                { "Document Type", Convert.ToString(r["docTypedesc"]) },
                { "Created", Convert.ToString(((DateTime)r["Created"]).ToLocalTime()) },
                { "Created By", Convert.ToString(r["CrByFullName"]) },
                { "Database", String.Format("{0}.{1}", Convert.ToString(r["Server"]), Convert.ToString(r["Database"])) }
            };
            return documentDetailsData;
        }




        private void RefreshList()
        {
            if (vw != null)
            {
                listView1.VirtualListSize = vw.Count;
                listView1.Refresh();
                UpdateStats();
                SetDocumentDetails();
            }
        }

        private void Filter()
        {
            if (vw != null)
            {
                string filter = FWBS.Common.Data.SQLRoutines.ConvertToLikeValue(txtFilter.Text);

                StringBuilder fullfilter = new StringBuilder(initialfilter);

                if (initialfilter.Length > 0)
                    fullfilter.Append(" and (");
                
                if (exludefilter.Length > 0)
                {
                    fullfilter.Append(String.Format("{0} and ", exludefilter));
                }

                fullfilter.Append(String.Format("({0})", BuildFilterString()));

                if (initialfilter.Length > 0)
                    fullfilter.Append(")");

                foreach (String f in filters.Values)
                {
                    if (fullfilter.Length > 0)
                        fullfilter.Append(String.Format("and ({0})", f));
                }

                string dbfilter = (cboDatabase.SelectedValue == null ? "" : Convert.ToString(cboDatabase.SelectedValue));
                string dtfilter = (cboDocType.SelectedValue == null ? "" : Convert.ToString(cboDocType.SelectedValue));

                if (dbfilter.Length > 0)
                    fullfilter.Append(String.Format(" and (([server]+ '.' + [database]) = '{0}')", dbfilter));
                if (dtfilter.Length > 0)
                    fullfilter.Append(String.Format(" and ([DocType] = '{0}')", dtfilter));

                vw.RowFilter = String.Format(fullfilter.ToString(), filter);

            }
        }

        private string BuildFilterString()
        {
            StringBuilder sb = new StringBuilder();
            for (int ctr = 0; ctr < vw.Table.Columns.Count; ctr++)
            {
                DataColumn col = vw.Table.Columns[ctr];

                if (col.DataType == typeof(string))
                {
                    if (sb.Length > 0)
                        sb.Append(" or ");

                    sb.AppendFormat("([{0}] Like '%{1}%')", col.ColumnName, "{0}");
                }
            }

            return sb.ToString();
        }

        private void UpdateStats()
        {
            if (vw != null)
            {
                lblCount.Text = String.Format(Convert.ToString(lblCount.Tag), vw.Count);
            }
        }


        private void PopulateList()
        {
            listView1.Items.Clear();

            DataTable dt = FWBS.OMS.DocumentManagement.Storage.StorageManager.CurrentManager.LocalDocuments.GetLocalDocumentInfo();
            vw = new DataView(dt);
            vw.Sort = "CacheDate desc";
            initialfilter = vw.RowFilter;
            Filter();

            backgroundWorker1.RunWorkerAsync();

            RefreshList();

            SetDocumentDetails();
        }

        private ListViewItem AddItem(DataRow r)
        {
            string docid = DisplayableDocID(r);

            ListViewItem itm = new ListViewItem(docid, AddIcon(r));
            foreach (ColumnHeader header in listView1.Columns)
            {
                string colname = Convert.ToString(header.Tag);

                if (colname.ToLower() == "docid")
                    continue;

                ListViewItem.ListViewSubItem sub = itm.SubItems.Add("");


                if (r.Table.Columns.Contains(colname))
                {
                    //UTCFIX: DM - 30/11/06 - Displays the local dates.
                    if (r[colname] is DateTime)
                        sub.Text = Convert.ToString(((DateTime)r[colname]).ToLocalTime());
                    else
                        sub.Text = Convert.ToString(r[colname]);
                }
            }

            itm.Tag = r;

            return itm;
        }

        private static string DisplayableDocID(DataRow r)
        {
            string docid = "";
            if (Session.CurrentSession.ShowExternalDocumentIds && r.Table.Columns.Contains("docidext"))
                docid = Convert.ToString(r["docidext"]);

            if (string.IsNullOrEmpty(docid))
                docid = Convert.ToString(r["docid"]);
            return docid;
        }

        private int AddIcon(DataRow r)
        {
            if (r.Table.Columns.Contains("docextension") && r.Table.Columns.Contains("doccheckedoutby"))
            {
                string file, ext = Convert.ToString(r["docextension"]).Trim('.');

                if (Session.CurrentSession.IsLoggedIn)
                {
                    if (r["doccheckedoutby"] == DBNull.Value)
                    {
                        file = String.Format("checkedin.{0}", ext);
                    }
                    else if (Convert.ToInt32(r["doccheckedoutby"]) == Session.CurrentSession.CurrentUser.ID)
                    {
                        file = String.Format("checkedout.{0}", ext);
                    }
                    else
                    {
                        file = String.Format("locked.{0}", ext);
                    }
                }
                else
                {
                    if (r["doccheckedoutby"] == DBNull.Value)
                    {
                        file = String.Format("checkedin.{0}", ext);
                    }
                    else
                    {
                        file = String.Format("checkedout.{0}", ext);
                    }
                }


                if (imageList1.Images.ContainsKey(file) == false)
                {
                    Image icon = FWBS.Common.IconReader.GetFileIcon(file, FWBS.Common.IconReader.IconSize.Small, false).ToBitmap();


                    if (file.StartsWith("checkedout"))
                    {
                        using (Graphics g = Graphics.FromImage(icon))
                        {
                            try
                            {
                                g.DrawImage(imageList2.Images["Tick"], 0, 0);
                            }
                            catch
                            {
                            }
                        }

                    }
                    else if (file.StartsWith("locked"))
                    {
                        using (Graphics g = Graphics.FromImage(icon))
                        {
                            try
                            {
                                g.DrawImage(imageList2.Images["Person"], 0, 0);
                            }
                            catch { }
                        }
                    }

                    imageList1.Images.Add(file, icon);
                    ApplyImages();
                }

                return imageList1.Images.IndexOfKey(file);
            }

            return -1;
        }

        private void ApplyImages()
        {
            listView1.SmallImageList = null;
            listView1.SmallImageList = (listView1.DeviceDpi == 96) ? imageList1 : Images.ScaleList(imageList1, LogicalToDeviceUnits(new Size(16, 16)));
        }

        #endregion
    }
}