using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    public partial class frmFullSystemUpdate : BaseForm
    {
        DirectoryInfo di;
        ISystemUpdate parent;
        System.Data.DataView dvPackages;
        Stack<string> forwards = new Stack<string>();

        public frmFullSystemUpdate()
        {
            InitializeComponent();

            ApplyImages();

        }

        public frmFullSystemUpdate(ISystemUpdate parent):this()
        {
            this.parent = parent;
            dvPackages = new System.Data.DataView(FWBS.OMS.Design.Package.Packages.GetImportedPackageList());

        }

        private void ApplyImages()
        {
            var sizeValue = LogicalToDeviceUnits(16);

            this.listView1.SmallImageList = null;
            this.imageList1.Images.Clear();
            this.imageList1.ImageSize = new Size(sizeValue, sizeValue);
            this.imageList1.Images.Add(FWBS.OMS.UI.Windows.Images.GetAdminMenuItem(25, (Images.IconSize)sizeValue));
            this.imageList1.Images.Add(FWBS.OMS.UI.Windows.Images.GetAdminMenuItem(53, (Images.IconSize)sizeValue));
            this.imageList1.Images.Add(FWBS.OMS.UI.Windows.Images.GetAdminMenuItem(0, (Images.IconSize)sizeValue));
            this.imageList1.Images.SetKeyName(0, "error.ico");
            this.imageList1.Images.SetKeyName(1, "preferences.ico");
            this.imageList1.Images.SetKeyName(2, "Gear.ico");
            this.listView1.SmallImageList = imageList1;

            this.btnForward.Image = Images.GetCoolButton(18, (Images.IconSize)sizeValue).ToBitmap();
            this.btnParent.Image = Images.GetCoolButton(17, (Images.IconSize)sizeValue).ToBitmap();
        }
        
        private void btnFolderBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(txtFolderLocation.Text);
                if (dir.Exists)
                    openFileDialog1.InitialDirectory = dir.FullName;
            }
            catch 
            {
            }
            if (btnFolderBrowse.Text == "Set")
            {
                try
                {
                    if (txtFolderLocation.AutoCompleteCustomSource.Contains(txtFolderLocation.Text) == false)
                        txtFolderLocation.AutoCompleteCustomSource.Add(txtFolderLocation.Text);
                    di = new DirectoryInfo(txtFolderLocation.Text);
                    LoadManifests();
                    btnFolderBrowse.Text = "Browse";
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(this, ex);
                }
            }
            else if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    forwards.Clear();
                    btnForward.Enabled = false;
                    txtFolderLocation.Text = openFileDialog1.FileName;
                    if (txtFolderLocation.AutoCompleteCustomSource.Contains(txtFolderLocation.Text) == false)
                        txtFolderLocation.AutoCompleteCustomSource.Add(txtFolderLocation.Text);
                    di = new DirectoryInfo(txtFolderLocation.Text);
                    LoadManifests();
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(this, ex);
                }
            }
        }

        private string GetLastInstalled(string Code)
        {
            dvPackages.RowFilter = String.Format("pkgCode = '{0}'", Code);
            if (dvPackages.Count > 0)
            {
                if (dvPackages[0]["Updated"] != null)
                {
                    DateTime _updated = Convert.ToDateTime(dvPackages[0]["Updated"]);
                    return _updated.ToLocalTime().ToString();
                }
                else
                    return Convert.ToString(dvPackages[0]["Updated"]);
            }
            else
                return "";
        }

        private void LoadManifests()
        {
            FileInfo file = new FileInfo(di.FullName);
            if (file.Exists)
            {
                di = file.Directory;
                txtFolderLocation.Text = di.FullName;
            }

            listView1.Items.Clear();
            var files = di.GetFiles("*.Manifest.xml");
            if (files.Length > 0)
            {
                ListViewItem lvi = listView1.Items.Add(files[0].Name.Replace(".Manifest.xml",""), 2);
                lvi.Tag = files[0];
                lvi.Checked = true;
                lvi.SubItems.Add(GetLastInstalled(files[0].Name.Replace(".Manifest.xml", "")));
                lvi.SubItems.Add("");
            }
            else
            {
                foreach (var i in di.GetDirectories())
                {
                    try
                    {
                        files = i.GetFiles("*.Manifest.xml");
                        if (files.Length > 0)
                        {
                            ListViewItem lvi = listView1.Items.Add(i.Name, 2);
                            lvi.Tag = files[0];
                            lvi.Checked = true;
                            lvi.SubItems.Add(GetLastInstalled(i.Name));
                            lvi.SubItems.Add("");
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public string Filename { get; private set; }

        private void btnRemove_Click(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            frmMain.EnabledAutoStart = chkAutoStart.Checked;
            List<ListViewItem> packages = new List<ListViewItem>();

            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked)
                    packages.Add(item);

            }

            ProcessPackages(packages, true, false);
        }


        public bool somethingSucceeded;
        private void ProcessPackages(List<ListViewItem> packages, bool reconsiderPackageExceptions, bool allowVersionWarning)
        {
            List<ListViewItem> failedPackages = new List<ListViewItem>();
            somethingSucceeded = false;
            foreach (ListViewItem item in packages)
            {
                object ret;
                string filename = ((FileInfo)item.Tag).FullName;

                SystemUpdateReturnStates status = parent.PackageDeployment.ProcessManifest(this, filename, chkCopy.Checked && chkCopy.Visible, chkShowReadme.Checked, chkMakeBackup.Checked, false, out ret, allowVersionWarning);

                Exception ex = ret as Exception;
                if (ex != null)
                {
                    if (reconsiderPackageExceptions && ex is PackageException)
                    {
                        failedPackages.Add(item);
                        continue;
                    }
                    item.SubItems[2].Tag = ret;
                    item.SubItems[2].Text = ex.Message;
                }
                else
                {
                    item.SubItems[2].Text = Convert.ToString(ret);
                }
                if (status == SystemUpdateReturnStates.Success)
                {
                    item.ImageIndex = 1;
                    item.Checked = false;
                }
                else
                    item.ImageIndex = 0;
                if (status == SystemUpdateReturnStates.Cancel)
                    return;

                somethingSucceeded = true;
            }

            if (failedPackages.Count > 0)
            {
                if (!somethingSucceeded && !allowVersionWarning)
                {
                    somethingSucceeded = true;
                    allowVersionWarning = true;
                }
                else
                    allowVersionWarning = false;
  
                                  
                ProcessPackages(failedPackages, somethingSucceeded, allowVersionWarning);
            }


        }


        private void txtFolderLocation_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtFolderLocation.Text) == false)
            {
                btnFolderBrowse.Text = "Set";
                btnParent.Enabled = true;
            }
            else
            {
                btnFolderBrowse.Text = "Browse";
                btnParent.Enabled = false;
            }

        }

        private void txtFolderLocation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnFolderBrowse.PerformClick();
        }

        private void frmFullSystemUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                

                DirectoryInfo check = new DirectoryInfo(txtFolderLocation.Text);
                if (check.Exists == false) txtFolderLocation.Text = "";

                Favourites fav = new Favourites("LASTPACK");
                if (fav.Count > 0)
                {
                    fav.Description(0, txtFolderLocation.Text);
                }
                else
                {
                    fav.AddFavourite(txtFolderLocation.Text, "");
                }
                fav.Update();

            }
            catch (Exception ex)
            {
                Trace.TraceError("frmFullSystemUpdate_FormClosing." + ex.Message);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.Filename = openFileDialog1.FileName;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnUnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listView1.Items)
                i.Checked = false;
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listView1.Items)
                i.Checked = true;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1 && listView1.SelectedItems[0].SubItems[2].Tag != null)
            {
                Exception ex = listView1.SelectedItems[0].SubItems[2].Tag as Exception;
                if (ex != null)
                    ErrorBox.Show(this, ex);
            }
        }

        private void pnlTools_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            label1.Visible = btnBrowse.Visible = btnBrowse.Enabled = !btnBrowse.Visible;
#endif
        }

        private void frmFullSystemUpdate_Load(object sender, EventArgs e)
        {

        }

        private void frmFullSystemUpdate_Shown(object sender, EventArgs e)
        {
            Favourites fav = new Favourites("LASTPACK");
            if (fav.Count > 0)
            {
                txtFolderLocation.Text = fav.Description(0);
                btnFolderBrowse.PerformClick();
            }
        }

        private void btnParent_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(txtFolderLocation.Text);
                if (di.Parent != null)
                {
                    forwards.Push(txtFolderLocation.Text);
                    btnForward.Enabled = true;
                    txtFolderLocation.Text = di.Parent.FullName;
                    btnFolderBrowse.PerformClick();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            try
            {
                if (forwards.Count > 0)
                {
                    txtFolderLocation.Text = forwards.Pop();
                    btnFolderBrowse.PerformClick();
                }
                btnForward.Enabled = forwards.Count > 0;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            ApplyImages();
        }

    }
}
