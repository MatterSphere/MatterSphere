using System;

namespace FWBS.OMS.Utils.Commands
{
    public class ViewContactCommand : RunCommand
    {
        public override string Name
        {
            get { return "VIEWCONTACT"; }
        }

        public override void Execute(MainWindow main)
        {
            if (Param.Length > 0)
            {
                Contact contact = Contact.GetContact(Convert.ToInt64(Param));
                FWBS.OMS.UI.OMSTypeScreen screen = new FWBS.OMS.UI.OMSTypeScreen(contact);
                screen.OmsType = contact.CurrentContactType;
                screen.DefaultPage = Param2;
                screen.Show(Parent);
            }
        }
    }
}
