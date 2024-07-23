namespace Fwbs.Office.Outlook
{
    partial class OutlookApplication
    {
        #region _Application Members


        public Microsoft.Office.Core.PickerDialog PickerDialog
        {
            get { return app.PickerDialog; }
        }

        public void RefreshFormRegionDefinition(string RegionName)
        {
            app.RefreshFormRegionDefinition(RegionName);
        }

        #endregion
    }
}
