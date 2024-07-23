using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public partial class ucAlertLabel : UserControl
    {
        public ucAlertLabel()
        {
            InitializeComponent();
        }


        public ucAlertLabel(Label alertLabel, Image alertIcon)
            : this()
        {
            this.BackColor = alertLabel.BackColor;
            lblAlert.Font = alertLabel.Font;
            lblAlert.Text = alertLabel.Text;
            lblAlert.ForeColor = alertLabel.ForeColor;
            pbAlertIcon.Image = alertIcon;
        }


        public Label AlertLabel 
        { 
            get
            {
                return lblAlert;
            }
            set
            {
                lblAlert = value;
            }
        }


        public Image AlertImage
        { 
            get
            {
                return pbAlertIcon.Image;
            }
            set
            {
                pbAlertIcon.Image = value;
            }
        }
    }
}
