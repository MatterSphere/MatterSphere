using System;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    public partial class ucEditForm : UserControl
    {
        private const string _closeSymbol = "╳";
        private readonly string _saveTitle = CodeLookup.GetLookup("DASHBOARD", "SAVE", "Save");
        private readonly string _addTitle = CodeLookup.GetLookup("DASHBOARD", "ADD", "Add");
        private IEditFormContent _editFormContent;

        public ucEditForm()
        {
            InitializeComponent();

            this.btnClose.Text = _closeSymbol;
        }

        public event EventHandler Canceled;
        public event EventHandler Closed;
        public event EventHandler Added;

        public bool IsNew
        {
            set
            {
                btnAdd.Text = value
                    ? _addTitle
                    : _saveTitle;
            }
        }

        public void InsertContent(EnquiryForm enquiryForm)
        {
            var content = new EnquiryFormContent(enquiryForm);
            InsertContent(content);
        }

        public void InsertContent(ucOmsItem omsItem)
        {
            var content = new OmsItemContent(omsItem);
            InsertContent(content);
        }

        public void InsertTitle(string title)
        {
            lblTitle.Text = title;
        }

        public bool UpdateItem()
        {
            try
            {
                _editFormContent.UpdateContent();
                return true;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
            return false;
        }

        #region UI Events

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Added?.Invoke(this, EventArgs.Empty);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CheckChanges(Canceled);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CheckChanges(Closed);
        }

        #endregion

        private void CheckChanges(EventHandler handler)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (_editFormContent.IsDirty)
                {
                    DialogResult res = Windows.MessageBox.Show(Session.CurrentSession.Resources
                        .GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?", "")
                        .Text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                    switch (res)
                    {
                        case DialogResult.Yes:
                            Added?.Invoke(this, EventArgs.Empty);
                            return;
                        case DialogResult.No:
                            handler?.Invoke(this, EventArgs.Empty);
                            break;
                        case DialogResult.Cancel:
                            return;
                    }
                }
                else
                {
                    handler?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void InsertContent(IEditFormContent editFormContent)
        {
            editFormContent.Location = new Point(0, 0);
            editFormContent.AutoScroll = true;
            pnlContent.AutoScroll = false;
            editFormContent.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(editFormContent.Content);
            _editFormContent = editFormContent;
        }
    }
}
