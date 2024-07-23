using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    internal class WizardPageTracker
    {
        private readonly ProgressBar progressBar;
        private readonly EnquiryForm enquiryForm;
        private readonly EnquiryForm customForm;


        internal WizardPageTracker(ProgressBar progressBar, EnquiryForm enquiryFormInWizard, EnquiryForm customFormInWizard = null)
        {
            this.progressBar = progressBar;
            this.enquiryForm = enquiryFormInWizard;
            this.customForm = customFormInWizard;
            System.Diagnostics.Debug.Assert(InWizardMode());
        }

        internal void ApplyPageTrackingLogic(EnquiryForm sender, PageChangedEventArgs e)
        {
            if (!OnCustomWelcomePage(sender, e))
                SetPageProgress(e.PageNumber + 1, sender == customForm);
        }

        private bool InWizardMode()
        {
            return enquiryForm.Style == EnquiryStyle.Wizard;
        }

        private bool OnCustomWelcomePage(EnquiryForm sender, PageChangedEventArgs e)
        {
            return sender == customForm && e.PageType == EnquiryPageType.Start;
        }

        private void SetPageProgress(int pageNumber, bool custom)
        {
            int totalPages = enquiryForm.PageCount;
            if (customForm != null)
            {
                if (custom)
                    pageNumber += totalPages;

                if (customForm.PageCount > 0)
                    totalPages += customForm.PageCount;
                else
                    totalPages += totalPages; // Workaround to improve progress perception when custom form page count isn't known yet.
            }

            progressBar.Maximum = totalPages;
            progressBar.Value = pageNumber;
        }

        #region ProgressBar

        internal class ProgressBar : System.Windows.Forms.ProgressBar
        {
            public ProgressBar()
            {
                SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                using (Brush brBack = new SolidBrush(BackColor))
                    e.Graphics.FillRectangle(brBack, 0, 0, Width, Height);

                using (Brush brFore = new SolidBrush(ForeColor))
                    e.Graphics.FillRectangle(brFore, 0, 0, Width * ((float)Value / Maximum), Height);
            }
        }

        #endregion
    }
}
