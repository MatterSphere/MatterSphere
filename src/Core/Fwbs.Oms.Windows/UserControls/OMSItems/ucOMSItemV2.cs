using System;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows
{
    public partial class ucOMSItemV2 : ucOMSItemBase
    {

        #region Constructors & Destructors

        private EntityImageProvider _provider;
        private Entity _entity;
        private EnquiryMode _mode;
        private object _parent;
        private string _code;
        private Common.KeyValueCollection _param;

        /// <summary>
        /// Default constructor of the control.
        /// </summary>
        public ucOMSItemV2()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Inherit;
            lblTypeCreated.SizeChanged += CenterHeaderText;
        }

        /// <summary>
        /// New entity enquiry form with a specified edit mode, also specifying whether to only have it as offline
        /// so that the database does not get updated at this moment in time.
        /// </summary>
        /// <param name="code">Unique enquiry form code.</param>
        /// <param name="parent">The parent to use for the enquiry form.</param>
        /// <param name="mode">Enquiry edit mode option.</param>
        /// <param name="offline">Offline option, if true then the database will not be updated.</param>
        /// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
        internal ucOMSItemV2(string code, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param) : this()
        {
            enquiryForm1.Enquiry = Enquiry.GetEnquiry(code, parent, mode, offline, param);
            Setup(code, mode, parent, param);
        }

        /// <summary>
        /// Edits an existing object with a specified enquiry form.
        /// </summary>
        /// <param name="code">Unique enquiry form code.</param>
        /// <param name="parent">The parent to use for the enquiry form.</param>
        /// <param name="obj">Enquiry compatible object that is to be edited by the wizard.</param>
        /// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
        internal ucOMSItemV2(string code, object parent, FWBS.OMS.Interfaces.IEnquiryCompatible obj, bool offline, Common.KeyValueCollection param) : this()
        {
            enquiryForm1.Enquiry = Enquiry.GetEnquiry(code, parent, obj, param);
            enquiryForm1.Enquiry.Offline = offline;
            var mode = obj.IsNew ? EnquiryMode.Add : EnquiryMode.Edit;
            Setup(code, mode, parent, param);
        }

        #endregion

        #region Methods

        protected virtual void Setup(string enquiryCode, EnquiryMode mode, object parent, Common.KeyValueCollection param)
        {
            enquiryForm1.Dirty += new EventHandler(OnDirty);
            enquiryForm1.NewOMSTypeWindow += new NewOMSTypeWindowEventHandler(OnNewOMSTypeWindow);
            enquiryForm1.SetCanvasSize();
            _provider = new EntityImageProvider(enquiryCode);
            _entity = _provider.Entity;
            _code = enquiryCode;
            _mode = mode;
            _parent = parent;
            _param = param;
            SetupWithMode();
            enquiryForm1.Dirty += ValidateFormCompleted;
            SetIcons();
        }

        private void SetupWithMode()
        {
            var codeLookUp = _entity.ToString().ToUpper();
            var formattedEntity = _entity.ToString();
            if (_mode == EnquiryMode.Add)
            {
                lblHeaderText.Text = Session.CurrentSession.Resources.GetResource($"ADD{codeLookUp}", $"Add New {formattedEntity}", "").Text;
                lblTypeCreated.Text = $@"{Session.CurrentSession.Resources.GetResource($"NEW{codeLookUp}", $"New {formattedEntity}", "").Text} {
                    Session.CurrentSession.Resources.GetResource("CREATED", "Created", "").Text}";
            }
            else if (_mode == EnquiryMode.Edit)
            {
                lblHeaderText.Text = Session.CurrentSession.Resources.GetResource($"EDIT{codeLookUp}", $"Edit {formattedEntity}", "").Text;
                btnAdd.Visible = false;
                btnSave.Visible = true;
                btnSave.BringToFront();
            }
        }

        /// <summary>
        /// Refreshes any data change to the underlying enquiry form.
        /// </summary>
        public override void RefreshItem()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (_mode == EnquiryMode.Edit)
                {
                    enquiryForm1.RefreshItem();
                }
                //If it's creation mode than reinitialize underlying enquiry
                else if (_mode == EnquiryMode.Add)
                {
                    enquiryForm1.Enquiry = Enquiry.GetEnquiry(_code, _parent, _mode, _param);
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            SetIcons();
        }

        private void SetIcons()
        {
            btnClose.Image = Images.GetCommonIcon(DeviceDpi, "close");
            pictureBox1.Image = _provider.Image;
        }

        protected virtual void ValidateFormCompleted(object sender, EventArgs e)
        {
            if (enquiryForm1.RequiredFieldsComplete())
            {
                EnableFinish();
            }
            else
            {
                DisableFinish();
            }
        }

        protected void DisableFinish()
        {
            btnAdd.Enabled = false;
            btnAdd.BackColor = Color.FromArgb(244, 244, 244);
            btnAdd.ForeColor = Color.FromArgb(121, 121, 121);
            btnAdd.FlatAppearance.BorderColor = Color.Black;
        }

        protected void EnableFinish()
        {
            btnAdd.Enabled = true;
            btnAdd.BackColor = Color.FromArgb(21, 101, 192);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatAppearance.BorderColor = Color.FromArgb(21, 101, 192);
        }

        /// <summary>
        /// Attempts to dave the enquiry form data to the database and then closes the form.
        /// </summary>
        /// <param name="sender">OK button object.</param>
        /// <param name="e">Empty event arguments.</param>
        protected virtual void btnAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                btnAdd.Focus();
                Cursor = Cursors.WaitCursor;
                enquiryForm1.UpdateItem();
                if (btnAdd.Enabled)
                {
                    enquiryForm1.Visible = false;
                    pnlButtons.Visible = false;
                    pnlSecondPage.Visible = true;
                    pnlSecondPage.BringToFront();
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

        private void btnDone_Click(object sender, System.EventArgs e)
        {
            try
            {
                btnDone.Focus();
                Cursor = Cursors.WaitCursor;
                if (!enquiryForm1.IsFormDirty)
                {
                    OnClose(ClosingWhy.Saved);
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

        /// <summary>
        /// Allows a Public Cancel Item with all the UI Accouterments
        /// http://dictionary.reference.com/search?r=2&q=Accouterments
        /// </summary>
        public override void CancelUIItem()
        {
            btnCancel.Focus();
            Application.DoEvents();
            cmdCancel_Click(this, EventArgs.Empty);
        }

        /// <summary>
        /// Closes the enquiry form.
        /// </summary>
        /// <param name="sender">Close button.</param>
        /// <param name="e">Empty event arguments.</param>
        private void cmdCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (IsDirty)
                {
                    DialogResult res = Windows.MessageBox.Show(Session.CurrentSession.Resources
                        .GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?", "")
                        .Text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                    switch (res)
                    {
                        case DialogResult.Yes:
                            if (_mode == EnquiryMode.Add)
                            {
                                btnAdd_Click(sender, e);
                            }
                            else if (_mode == EnquiryMode.Edit)
                            {
                                btnSave_Click(sender, e);
                            }
                            return;
                        case DialogResult.No:
                            CancelItem();
                            break;
                        case DialogResult.Cancel:
                            return;
                    }
                }
                OnClose(ClosingWhy.Cancel);
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

        private void CenterHeaderText(object sender, EventArgs e)
        {
            lblTypeCreated.Left = (this.ClientSize.Width - lblTypeCreated.Size.Width) / 2;
        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(216, 216, 216), 1))
            {
                e.Graphics.DrawLine(pen, new Point(e.ClipRectangle.Left, e.ClipRectangle.Bottom - pnlHeader.Padding.Bottom),
                    new Point(e.ClipRectangle.Right, e.ClipRectangle.Bottom - pnlHeader.Padding.Bottom));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Focus();
                Cursor = Cursors.WaitCursor;
                enquiryForm1.UpdateItem();
                if (!enquiryForm1.IsFormDirty)
                {
                    OnClose(ClosingWhy.Saved);
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

        #endregion
    }
}
