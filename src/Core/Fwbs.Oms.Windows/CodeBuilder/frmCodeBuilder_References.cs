using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FWBS.OMS.Design.CodeBuilder
{
    using System.IO;
    using FWBS.OMS.Script;
    using FWBS.OMS.UI.Windows;

    public partial class frmCodeBuilder_References : Form
    {
        #region Fields

        private ListViewGroup grpSelected = null;
        private IScriptManager manager = null;
        private bool loadall;
        private const int threshold = 500; 

        #endregion

        #region Constructors

        public frmCodeBuilder_References()
        {
            InitializeComponent();

            this.grpSelected = lvwReferences.Groups["grpSelected"];
            this.grpSelected.Header = TranslateGroupName(grpSelected.Header);
        }

        #endregion

        private string TranslateGroupName(string name)
        {
            switch (name)
            {
                case "Selected":
                    return ResourceLookup.GetLookupText("SELECTED", name, null);
                case "System Assemblies":
                    return ResourceLookup.GetLookupText("SYSTMASSEMBLIES", name, null);
                case "Local Assemblies":
                    return ResourceLookup.GetLookupText("LOCALASSEMBLIES", name, null);
                case "Distributed Assemblies":
                    return ResourceLookup.GetLookupText("DISTRASSEMBLIES", name, null);
                case "System Scripts":
                    return ResourceLookup.GetLookupText("SYSTEMSCRIPTS", name, null);
                default:
                    return name;
            }
        }

        #region Properties

        private List<IReference> selected = new List<IReference>();
        
        public IReference[] References
        {
            get
            {
                return selected.ToArray();
            }
            set
            {
                if (value == null)
                    selected.Clear();
                else
                    selected.AddRange(value);
            }
        }

        private IScriptDefinition Definition { get; set; }


        private HashSet<ListViewItem> references = new HashSet<ListViewItem>();
        private Dictionary<ListViewItem, ListViewGroup> itemstogroup = new Dictionary<ListViewItem, ListViewGroup>();

        #endregion

        #region Captured Events

        private void frmCodeBuilder_References_Load(object sender, EventArgs e)
        {
            try
            {
                lvwReferences.SetGroupState(ListViewGroupState.Collapsible);

                manager = Session.CurrentSession.Container.Resolve<IScriptManager>(null);

                foreach (var lib in manager.ReferenceLibraries)
                {
                    var grp = lvwReferences.Groups.Add(lib.Name, TranslateGroupName(lib.Name));
                    grp.Tag = lib;
                    lib.Loaded += new EventHandler(lib_Loaded);
                    lvwReferences.Collapse(grp);
                }

                RefreshLists();
                
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void lib_Loaded(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (InvokeRequired)
            {
                this.Invoke(new EventHandler(lib_Loaded), sender, e);
                return;
            }

            try
            {
                lvwReferences.ItemCheck -= lvwReferences_ItemCheck;
                lvwReferences.BeginUpdate();
                Application.DoEvents();

                var lib = (IReferenceLibrary)sender;
                prgProcessing.Maximum = lib.Count;
                prgProcessing.Value = 0;

                if (lib.Count <= threshold || loadall)
                {
                    foreach (var item in lib.GetByDefinition(Definition))
                    {
                        if (IsClosing)
                            return;

                        var group = lvwReferences.Groups[lib.Name];
                        var litem = AddReference(item, group, false);
                        references.Add(litem);
                        itemstogroup.Add(litem, group);
                        prgProcessing.Value++;
                    }
                }

                lvwReferences.EndUpdate();
                Application.DoEvents();
            }
            finally
            {
                lvwReferences.ItemCheck += lvwReferences_ItemCheck;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer1.Enabled = true;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                lvwReferences.BeginUpdate();
                timer1.Enabled = false;

                var live = lvwReferences.Items.Cast<ListViewItem>();

                var found = from r in live
                            where IsInFilter(r)
                            select r;

                var notincluded = from m in references.Except(live)
                                  where IsInFilter(m)
                                  select m;

                var toremove = live.Except(found);

                foreach (var item in toremove)
                {
                    lvwReferences.Items.Remove(item);
                }

                var toadd = notincluded.Except(live);

                foreach (var item in toadd)
                {
                    lvwReferences.Items.Add(item);
                    item.Group = itemstogroup[item];
                }
                lvwReferences.EndUpdate();

            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                selected.Clear();

                foreach (ListViewItem item in lvwReferences.CheckedItems)
                {
                    if (item.Checked)
                        selected.Add((IReference)item.Tag);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(this.ParentForm, Session.CurrentSession.DefaultSystemForm(SystemForms.AddDistributedAssembly), null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, false, new FWBS.Common.KeyValueCollection());
            if (dt != null)
            {
                try
                {
                    FileInfo fileinfo = new FileInfo(Convert.ToString(dt.Rows[0]["txtFilename"]));
                    if (fileinfo.Exists)
                    {
                        FWBS.OMS.DistributedAssemblies current = new DistributedAssemblies();
                        current.SetSourceFileName(fileinfo.FullName, Convert.ToString(dt.Rows[0]["txtVersion"]));
                        current.Update();
                    }

                    RefreshLists(true);
                    var item = lvwReferences.Items.FirstOrDefault<ListViewItem>(n => fileinfo.Name.Contains(n.Text));
                    if (item != null)
                        item.Checked = true;
                }				
                catch (Exception ex)
                {
                    ErrorBox.Show(this, ex);
                }
            }
        }


        private void frmCodeBuilder_References_FormClosed(object sender, FormClosedEventArgs e)
        {
            references.Clear();
            itemstogroup.Clear();

            foreach (ListViewGroup group in lvwReferences.Groups)
            {
                var lib = group.Tag as IReferenceLibrary;
                if (lib != null)
                    lib.Loaded -= lib_Loaded;
            }
        }


        private void frmCodeBuilder_References_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsClosing = true;
        }

        private void lnkRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                RefreshLists(true);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void lnkIncludeGAC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                loadall = true;

                lnkIncludeGAC.Visible = false;

                BackgroundRefresh(manager.ReferenceLibraries.Where(lib => lib.Count > threshold).ToArray());
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void lvwReferences_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var item = lvwReferences.Items[e.Index];
            var ar = item.Tag as IReference;
            if (ar != null)
                e.NewValue = ar.IsRequired ? CheckState.Checked : e.NewValue;
        }

        #endregion

        #region Methods

        private bool IsInFilter(ListViewItem item)
        {
            return item.Text.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) > -1 || item.Checked;
        }

        private bool IsBusy{get; set;}

        private bool IsClosing{get;set;}

        private void StartBusy()
        {
            IsBusy = true;
            prgProcessing.Visible = true;
            Application.DoEvents();
            //lvwReferences.BeginUpdate();
        }

        private void StopBusy()
        {
            //lvwReferences.EndUpdate();
            Application.DoEvents();
            prgProcessing.Visible = false;
            IsBusy = false;
        }

        private void RefreshLists(bool force = false)
        {
            lvwReferences.ItemCheck -= lvwReferences_ItemCheck;

            try
            {
                lvwReferences.Items.Clear();
                references.Clear();
                itemstogroup.Clear();

                foreach (var sel in selected)
                {
                    var item = AddReference(sel, grpSelected, true);
                    references.Add(item);
                    itemstogroup.Add(item, grpSelected);
                }

                Application.DoEvents();

                BackgroundRefresh(manager.ReferenceLibraries.ToArray(), force);

            }
            finally
            {
                lvwReferences.ItemCheck += lvwReferences_ItemCheck;
            }
        }

        private ListViewItem AddReference(IReference reference, ListViewGroup group, bool selected)
        {
            var item = new ListViewItem();
            item.Checked = selected;
            item.Tag = reference;
            if (selected)
                item.Text = reference.ToString();
            else
                item.Text = reference.Name;

            if (IsInFilter(item))
                lvwReferences.Items.Add(item);

            if (reference.IsRequired)
            {
                item.Font = new Font(item.Font, FontStyle.Italic);
            }

            item.Group = group;
            return item;
        }


        private void BackgroundRefresh(IReferenceLibrary[] libs, bool force = false)
        {
            StartBusy();

            System.Threading.ThreadPool.QueueUserWorkItem((state) =>
            {
                try
                {
                    foreach (var lib in libs)
                    {
                        if (IsClosing)
                            return;

                        if (force)
                            lib.Refresh();
                        else
                            lib.Load();
                    }
                }
                finally
                {
                    if (!IsClosing)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            if (IsClosing)
                                return;
                            StopBusy();
                        }));
                    }
                }
            }, null);
        }
        #endregion



 


    }

}