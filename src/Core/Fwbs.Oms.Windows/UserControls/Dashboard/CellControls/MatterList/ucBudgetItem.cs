using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.MatterList
{
    public partial class ucBudgetItem : UserControl
    {
        public ucBudgetItem()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        public string Value
        {
            get { return lblValue.Text; }
            set { lblValue.Text = value; }
        }

        public void SetHighlight()
        {
            this.BackColor = Color.FromArgb(208, 2, 27);
            lblTitle.ForeColor = Color.White;
            lblValue.ForeColor = Color.White;
        }
    }
}
