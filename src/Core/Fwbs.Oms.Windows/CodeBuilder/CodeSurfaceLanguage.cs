using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.Design.CodeBuilder
{
    public partial class CodeSurfaceLanguage : BaseForm
    {
        public CodeSurfaceLanguage()
        {
            InitializeComponent();
        }

        public bool DontAskAgain 
        {
            get
            {
                return chkDontAsk.Checked;
            }
        }

    }
}
