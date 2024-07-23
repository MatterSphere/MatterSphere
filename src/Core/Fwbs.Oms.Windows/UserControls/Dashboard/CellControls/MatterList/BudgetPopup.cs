using System;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.MatterList
{
    public partial class BudgetPopup : UserControl
    {
        public BudgetPopup()
        {
            InitializeComponent();
        }

        public event EventHandler FileClicked;

        public string OMSFileNo
        {
            get { return lblMatterNo.Text; }
            set { lblMatterNo.Text = value; }
        }

        public long OMSFileId { get; set; }

        public void AddBudgetItem(ucBudgetItem budgetItem, bool onTop)
        {
            budgetItem.Dock = onTop ? DockStyle.Top : DockStyle.Bottom;
            pnlItems.Controls.Add(budgetItem, true);
        }

        private void lblMatterNo_Click(object sender, System.EventArgs e)
        {
            if (OMSFileId == 0)
            {
                return;
            }

            FWBS.OMS.OMSFile file = FWBS.OMS.OMSFile.GetFile(OMSFileId);
            var eventArgs = new NewOMSTypeWindowEventArgs(file);
            var screen = new OMSTypeScreen(eventArgs.OMSObject)
            {
                DefaultPage = eventArgs.DefaultPage,
                OmsType = eventArgs.OMSType
            };

            FileClicked?.Invoke(this, EventArgs.Empty);
            screen.Show(null);
        }
    }
}
