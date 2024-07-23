using System;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    public class ucDistributed : ucEditBase2
    {
        public ucDistributed()
        {
            InitializeComponent();
        }

        public ucDistributed(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection param)
            : base(mainparent, editparent, param)
        {
            if (frmMain.PartnerAccess == false) Session.CurrentSession.ValidateLicensedFor("SDKALL");
            // This call is required by the Windows Form Designer.
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
                return "ADMDISTRIB";
            }
        }

        protected override void LoadSingleItem(string Code)
        {
            FWBS.OMS.DistributedAssemblies current = new DistributedAssemblies();
            current.Fetch(Code);
            openFileDialog1.FileName = current.SourceFileName;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                current.SetSourceFileName(openFileDialog1.FileName,current.OMSVersion);
                current.Update();
                lstList.Search();
            }
        }

        protected override bool UpdateData()
        {
            return true;
        }

        protected override void NewData()
        {
            DataTable dt = (DataTable)FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(this.ParentForm, Session.CurrentSession.DefaultSystemForm(SystemForms.AddDistributedAssembly), null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, false, new FWBS.Common.KeyValueCollection());
            if (dt != null)
            {
                try
                {
                    FWBS.OMS.DistributedAssemblies current = new DistributedAssemblies();
                    current.SetSourceFileName(Convert.ToString(dt.Rows[0]["txtFilename"]),Convert.ToString(dt.Rows[0]["txtVersion"]));
                    current.Update();
                    lstList.Search();
                }				
                catch (Exception ex)
                {
                    ErrorBox.Show(ParentForm, ex);
                }
            }
        }


        protected override void DeleteData(string Code)
        {
            try
            {
                FWBS.OMS.DistributedAssemblies current = new DistributedAssemblies();
                current.Fetch(Code);
                current.Delete();
                lstList.Search();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpEdit
            // 
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
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
            this.lstList.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.lstList_SearchButtonCommands_1);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Dynamic DLL\'s (*.dll)|*.dll|All Files (*.*)|*.*";
            this.openFileDialog1.Title = "Select location";
            // 
            // ucDistributed
            // 
            this.Name = "ucDistributed";
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlEdit.PerformLayout();
            this.ResumeLayout(false);

        }

        private OpenFileDialog openFileDialog1;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private const string Refresh = "cmdRefresh";
        private const string NewModule = "cmdNewModule";

        private void lstList_SearchButtonCommands_1(object sender, SearchButtonEventArgs e)
        {
            switch (e.ButtonName)
            {
                case Refresh:
                    try
                    {
                        FWBS.OMS.DistributedAssemblies current = new DistributedAssemblies();
                        current.Fetch(Convert.ToString(lstList.SelectedItems[0][0].Value));

                        if (current.OMSVersion != Session.CurrentSession.EngineVersion.ToString())
                            if (MessageBox.ShowYesNoQuestion("DAVC", "Do you want to Refresh the assembly from %1% against the currently tagged FWBS Framework V%2%. If you wish to assign against another version of the FWBS Framework then click no and then select Add.", current.SourceFileName, current.OMSVersion) == DialogResult.No)
                                return;

                        current.IsDirty = true;
                        current.Update();
                        lstList.Search();
                        MessageBox.ShowInformation("REFRESHASS3", "Refreshed the Assembly from Source. Please Restart the Admin Kit.");
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(ParentForm, ex);
                    }
                    break;
                case NewModule:
                    DataTable dt = (DataTable)FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(this.ParentForm, Session.CurrentSession.DefaultSystemForm(SystemForms.AddDistributedModule), null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, false, new FWBS.Common.KeyValueCollection());
                    if (dt != null)
                    {
                        try
                        {
                            FWBS.OMS.DistributedAssemblies current = new DistributedAssemblies();
                            current.SetSourceFileName(Convert.ToString(dt.Rows[0]["txtFilename"]), Convert.ToString(dt.Rows[0]["txtVersion"]));
                            current.PackageType = DistributedPackageType.Modules;
                            current.Update();
                            lstList.Search();
                            MessageBox.ShowInformation("PLSRESTARTADMIN", "New Module added to use. Please Restart Admin Kit.");
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            if (ex.Number == 2627)
                                ErrorBox.Show(ParentForm, new OMSException2("ERRDISTMDUP", "The Module is already present in the Distributed Assemblies for this Version of OMS. Please find and Refresh instead"));
                            else
                                ErrorBox.Show(ParentForm, ex);
                        }
                        catch (Exception ex)
                        {
                            ErrorBox.Show(ParentForm, ex);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
