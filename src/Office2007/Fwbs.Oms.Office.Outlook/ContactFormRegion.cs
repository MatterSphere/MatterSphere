using System;
using Fwbs.Office.Outlook;
using FWBS.OMS.Interfaces;

namespace Fwbs.Oms.Office.Outlook
{
    partial class ContactFormRegion
    {
        //C
        #region Form Region Factory

        [Microsoft.Office.Tools.Outlook.FormRegionMessageClass(Microsoft.Office.Tools.Outlook.FormRegionMessageClassAttribute.Contact)]
        [Microsoft.Office.Tools.Outlook.FormRegionName("Outlook.ContactFormRegion")]
        public partial class ContactFormRegionFactory
        {

            // Occurs before the form region is initialized.
            // To prevent the form region from appearing, set e.Cancel to true.
            // Use e.OutlookItem to get a reference to the current Outlook item.
            private void ContactFormRegionFactory_FormRegionInitializing(object sender, Microsoft.Office.Tools.Outlook.FormRegionInitializingEventArgs e)
            {
                if (!FWBS.OMS.Session.CurrentSession.IsLoggedIn)
                {
                    e.Cancel = true;
                    return;
                }

                // Code to check the current item to see whether there is a Client/Matter/Associate linked.
                var curContact = Globals.ThisAddIn.OmsApp.HasDocVariable(e.OutlookItem,"CONTACTID");
                if (!curContact)
                    e.Cancel = true;
            }
        }

        #endregion

        // Occurs before the form region is displayed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void ContactFormRegion_FormRegionShowing(object sender, System.EventArgs e)
        {
            

            var curContact = Convert.ToInt32(Globals.ThisAddIn.OmsApp.GetDocVariable(this.OutlookItem, "CONTACTID",0));

            if (curContact != 0)
            {
                try
                {
                    this.ucOMSTypeEmbeded1.Connect(FWBS.OMS.Contact.GetContact(curContact) as IOMSType);

                    Fwbs.Office.Outlook.OutlookApplication.GetApplication(this.OutlookFormRegion.Application).BeforeCloseItem += new EventHandler<BeforeItemEventArgs>(ContactFormRegion_BeforeCloseItem);
                }
                catch { }
            }
        }

        void ContactFormRegion_BeforeCloseItem(object sender, BeforeItemEventArgs e)
        {
            if (ucOMSTypeEmbeded1.IsDirty == true)
            {
                if (!ucOMSTypeEmbeded1.CanClose(false))
                {
                    e.Cancel = true;
                    e.Handled = true; // I am handling and Cancelling.
                }
            }
            else
            {
                e.Cancel = false;
                e.Handled = false;
            }
        }

  
        // Occurs when the form region is closed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void ContactFormRegion_FormRegionClosed(object sender, System.EventArgs e)
        {
            
            
        }

        private void ContactFormRegion_Click(object sender, EventArgs e)
        {
        }
    }
}
