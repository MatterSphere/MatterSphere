using System;
using Fwbs.Office.Outlook;
using FWBS.OMS.Interfaces;

namespace Fwbs.Oms.Office.Outlook
{
    partial class ClientFormRegion
    {
        //C
        #region Form Region Factory

        [Microsoft.Office.Tools.Outlook.FormRegionMessageClass(Microsoft.Office.Tools.Outlook.FormRegionMessageClassAttribute.Appointment)]
        [Microsoft.Office.Tools.Outlook.FormRegionMessageClass(Microsoft.Office.Tools.Outlook.FormRegionMessageClassAttribute.Note)]
        [Microsoft.Office.Tools.Outlook.FormRegionName("Outlook.ClientFormRegion")]
        public partial class ClientFormRegionFactory
        {

            // Occurs before the form region is initialized.
            // To prevent the form region from appearing, set e.Cancel to true.
            // Use e.OutlookItem to get a reference to the current Outlook item.
            private void ClientFormRegionFactory_FormRegionInitializing(object sender, Microsoft.Office.Tools.Outlook.FormRegionInitializingEventArgs e)
            {
                if (Globals.ThisAddIn.OmsApp == null)
                {
                    e.Cancel = true;
                    return;
                }

                if (!FWBS.OMS.Session.CurrentSession.IsLoggedIn)
                {
                    e.Cancel = true;
                    return;
                }


                // Code to check the current item to see whether there is a Client/Matter/Associate linked.
                var curAssoc = Globals.ThisAddIn.OmsApp.GetCurrentAssociate(e.OutlookItem);
                if (curAssoc == null)
                    e.Cancel = true;
            }
        }

        #endregion

        // Occurs before the form region is displayed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void ClientFormRegion_FormRegionShowing(object sender, System.EventArgs e)
        {
            if (Globals.ThisAddIn.OmsApp == null)
            {
                return;
            }

            var curAssoc = Globals.ThisAddIn.OmsApp.GetCurrentAssociate(this.OutlookItem);

            if (curAssoc != null)
            {
                try
                {
                    this.ucOMSTypeEmbeded1.Connect(curAssoc.OMSFile.Client as IOMSType);

                    Fwbs.Office.Outlook.OutlookApplication.GetApplication(this.OutlookFormRegion.Application).BeforeCloseItem += new EventHandler<BeforeItemEventArgs>(ClientFormRegion_BeforeCloseItem);
                }
                catch { }
            }
        }

        void ClientFormRegion_BeforeCloseItem(object sender, BeforeItemEventArgs e)
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
        private void ClientFormRegion_FormRegionClosed(object sender, System.EventArgs e)
        {
            
            
        }

        private void ClientFormRegion_Click(object sender, EventArgs e)
        {
        }
    }
}
