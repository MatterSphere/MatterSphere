namespace MCEPGlobalClasses
{
    public class MCEPGlobal
    {
        public static Aspose.Email.License setAsposeLicense()
        {
            Aspose.Email.License license = new Aspose.Email.License();
            license.SetLicense("Aspose.Total.lic");
            return license;
        }


    }
}
