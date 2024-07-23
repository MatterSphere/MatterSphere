using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.Dialogs
{
    public partial class frmMultiLineText : BaseForm
    {

        public frmMultiLineText(string formCaption, string titleCaption, string content)
        {
            InitializeComponent();
            this.Text = formCaption;
            this.lblTitle.Text = titleCaption;
            this.txtContent.Value = content;
        }
    }
}
