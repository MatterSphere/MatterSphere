namespace Fwbs.Office.Outlook
{
    partial class OutlookFolder
    {

        #region MAPIFolder Members


        public stdole.StdPicture GetCustomIcon()
        {
            return folder.GetCustomIcon();
        }

        public void SetCustomIcon(stdole.StdPicture Picture)
        {
            folder.SetCustomIcon(Picture);
        }

        #endregion
    }
}
