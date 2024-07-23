using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    public partial class frmWarningV5 : Form
    {
        public frmWarningV5()
        {
            InitializeComponent();
        }

        public static void ShowWarning()
        {
            FWBS.OMS.Favourites fav = new Favourites("V5WARNING");
            if (fav.Count == 0)
            {
                using (frmWarningV5 frm = new frmWarningV5())
                {
                    frm.ShowDialog();
                    if (frm.chkDontShowAgain.Checked)
                    {
                        fav.AddFavourite("DONTSHOWAGAIN", "");
                        fav.Update();
                    }
                }
            }
        }
    }
}
