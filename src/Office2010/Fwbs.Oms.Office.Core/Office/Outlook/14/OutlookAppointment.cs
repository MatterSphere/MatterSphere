namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookAppointment
    {

        #region _AppointmentItem Members


        public MSOutlook.AppointmentItem CopyTo(MSOutlook.MAPIFolder DestinationFolder, MSOutlook.OlAppointmentCopyOptions CopyOptions)
        {
            var ofolder = DestinationFolder as OutlookFolder;
            if (ofolder != null)
                DestinationFolder = ofolder.InternalItem;

            return (OutlookAppointment)Application.GetItem(InternalItem.CopyTo(DestinationFolder, CopyOptions));
        }


        public MSOutlook.AddressEntry GetOrganizer()
        {
            return InternalItem.GetOrganizer();
        }



        #endregion
    }
}
