using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.Windows.Reports
{
    public partial class frmLoading : BaseForm
    {
        public frmLoading()
        {
            InitializeComponent();
        }

        public string Description 
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }
    }
}
