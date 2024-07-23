using System.Windows.Forms;
using FWBS.Common.UI;

namespace FWBS.OMS.UI.Windows
{
    public class RequiredFieldRenderer
    {
        private const string _requiredAsterisk = "* ";
        private readonly ErrorProvider _req;

        public string Description { get; private set; }

        public RequiredFieldRenderer(ErrorProvider req)
        {
            _req = req;
            Description = Session.CurrentSession.Resources?.GetResource("REQFIELD", "This field is required.", "").Text;
        }

        public void MarkRequiredControl(Control ctrl)
        {
            if ((ctrl is IBasicEnquiryControl2) && ((IBasicEnquiryControl2)ctrl).CaptionTop)
            {
                if (!ctrl.Text.StartsWith(_requiredAsterisk))
                {
                    ctrl.Text = _requiredAsterisk + ctrl.Text;
                }
            }
            else
            {
                _req.SetError(ctrl, Description);
            }
        }
    }
}
