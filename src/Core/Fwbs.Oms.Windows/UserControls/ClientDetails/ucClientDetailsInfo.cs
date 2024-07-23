using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Common;

namespace FWBS.OMS.UI.UserControls.ClientDetails
{
    public partial class ucClientDetailsInfo : UserControl
    {
        public ucClientDetailsInfo()
        {
            InitializeComponent();
        }

        public ucClientDetailsInfo(FWBS.OMS.Models.Client.ClientDetails details) : this()
        {
            ClientName.SetTextProperty(FWBS.OMS.CodeLookup.GetLookup("SLCAPTION", "-1658590780"), details.Name);
            DefaultContact.SetTextProperty(FWBS.OMS.CodeLookup.GetLookup("RESOURCE", "DEFCONTACT"), details.Contact);
            Salutation.SetTextProperty(FWBS.OMS.CodeLookup.GetLookup("RESOURCE", "SALUTATION"), details.Salutation);
            Telephone.SetTextProperty(FWBS.OMS.CodeLookup.GetLookup("RESOURCE", "TELEPHONE"), details.Telephone);
            Fax.SetTextProperty(FWBS.OMS.CodeLookup.GetLookup("SLCAPTION", "193443354"), details.Fax);
            Mobile.SetTextProperty(FWBS.OMS.CodeLookup.GetLookup("RESOURCE", "MOBILE"), details.Mobile);
            Email.SetTextProperty(FWBS.OMS.CodeLookup.GetLookup("RESOURCE", "EMAIL"), details.Email, ucLabelField.LabelStyle.Link);

            LayoutPanel.Refresh();
        }
    }
}
