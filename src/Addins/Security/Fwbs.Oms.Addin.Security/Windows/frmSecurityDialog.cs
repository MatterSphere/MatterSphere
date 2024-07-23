using System;
using System.Windows.Forms;

namespace FWBS.OMS.Addin.Security.Windows
{
    public partial class frmSecurityDialog : FWBS.OMS.UI.Windows.BaseForm
    {
        public frmSecurityDialog(Int64 docID)
        {
            InitializeComponent();
            OMSDocument doc = OMSDocument.GetDocument(docID);
            ucSecurity1.Connect(doc);
        }

        public frmSecurityDialog(string omstype, string code)
        {
            InitializeComponent();
            ucSecurity1.Connect(omstype, code);
        }

        private bool allowClosure = true;
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (ucSecurity1.CanSaveAfterPermissionCheck(ucSecurity1.GetObjectPermissions()))
                {
                    allowClosure = true;
                    ucSecurity1.UpdateItem();
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
                allowClosure = false;
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void BtnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (ucSecurity1.IsDirty)
                {
                    
                    DialogResult dr = System.Windows.Forms.MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?", "").Text, FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        if (ucSecurity1.CanSaveAfterPermissionCheck(ucSecurity1.GetObjectPermissions()))
                        {
                            allowClosure = true;
                            ucSecurity1.UpdateItem();
                        }
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        allowClosure = false;
                    }
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
                allowClosure = false;
            }
        }

        private void FrmSecurityDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!allowClosure)
            {
                //Reset the flag and prevent the form closing
                allowClosure = true;
                e.Cancel = true;
            }
        }

    }
}