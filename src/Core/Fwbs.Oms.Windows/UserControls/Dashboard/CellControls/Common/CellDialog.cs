using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    internal class CellDialog : frmNewBrandIdent
    {
        public CellDialog() : base(TitleBarStyle.Large)
        {
        }

        public void AddContentControl(ContentContainer contentContainer)
        {
            var contentControl = contentContainer.GetContent();
            ucSearchControl searchControl = contentControl as ucSearchControl;
            if (searchControl != null)
            {
                searchControl.NavCommandPanel = new ucNavCommands();
                searchControl.NewOMSTypeWindow += CellDialog_NewOMSTypeWindow;
            }
            this.Controls.Add(contentControl, true);
            this.Text = contentContainer.Title;
            contentContainer.UpdateContent(this);
        }

        private void CellDialog_NewOMSTypeWindow(object sender, NewOMSTypeWindowEventArgs e)
        {
            try
            {
                this.UseWaitCursor = true;

                var screen = new OMSTypeScreen(e.OMSObject)
                {
                    DefaultPage = e.DefaultPage,
                    OmsType = e.OMSType
                };

                screen.Show(null);
                this.Close();
            }
            finally
            {
                if (!this.IsDisposed)
                {
                    this.UseWaitCursor = false;
                }
            }
        }
    }
}
