using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public partial class ucOMSItemWizard : ucOMSItemBase
    {
        private frmWizard _form;
        private EntityImageProvider _provider;

        public ucOMSItemWizard()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Inherit;
        }

        internal ucOMSItemWizard(frmWizard form) : this()
        {
            SetupForm(form);
            _form.Show();
        }

        private void SetupForm(frmWizard form)
        {
            lblTypeCreated.SizeChanged += CenterHeaderText;
            InitializeWizard(form);
            Setup(form.EnquiryForm.Code);
        }

        private void Setup(string enquiryCode)
        {
            _provider = new EntityImageProvider(enquiryCode);
            SetHeaderText();
            SetIcons();
        }

        private void InitializeWizard(frmWizard form)
        {
            _form = form;
            _form.SuspendLayout();
            _form.BackColor = Color.FromArgb(237, 243, 250);
            _form.FormClosed += WizardClosed;
            _form.FormBorderStyle = FormBorderStyle.None;
            Controls.Add(_form);
            _form.BringToFront();
            _form.Dock = DockStyle.Fill;
            _form.ResumeLayout();
        }

        private void SetHeaderText()
        {
            var entity = _provider.Entity;
            var codeLookUp = entity.ToString().ToUpper();
            var formattedEntity = entity.ToString();
            lblHeaderText.Text = Session.CurrentSession.Resources.GetResource($"ADD{codeLookUp}", $"Add New {formattedEntity}", "").Text;
            lblTypeCreated.Text = $@"{Session.CurrentSession.Resources.GetResource($"NEW{codeLookUp}", $"New {formattedEntity}", "").Text} {
                Session.CurrentSession.Resources.GetResource("CREATED", "Created", "").Text}";
        }

        private void ShowLastPage()
        {
            pnlSecondPage.Visible = true;
            pnlSecondPage.BringToFront();
        }

        private void SetIcons()
        {
            btnClose.Image = Images.GetCommonIcon(DeviceDpi, "close");
            pictureBox1.Image = _provider.Image;
        }

        private void CenterHeaderText(object sender, EventArgs e)
        {
            lblTypeCreated.Left = (ClientSize.Width - lblTypeCreated.Size.Width) / 2;
        }

        private void WizardClosed(object sender, FormClosedEventArgs args)
        {
            if (_form.DialogResult == DialogResult.OK)
            {
                ShowLastPage();
            }
            else
            {
                OnClose(ClosingWhy.Cancel);
            }
        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(216, 216, 216), 1))
            {
                e.Graphics.DrawLine(pen, new Point(e.ClipRectangle.Left, e.ClipRectangle.Bottom - pnlHeader.Padding.Bottom),
                    new Point(e.ClipRectangle.Right, e.ClipRectangle.Bottom - pnlHeader.Padding.Bottom));
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (!IsDirty)
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
                Dispose();
            }
        }
    }
}
