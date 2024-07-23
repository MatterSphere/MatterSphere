namespace FWBS.OMS.Models.Client
{
    public class ClientDetails
    {
        public ClientDetails(string clientName, Associate defaultAssociate)
        {
            Contact contact = defaultAssociate.Contact;
            Name = clientName;
            Contact = contact.Name;
            Salutation = defaultAssociate.Salutation;
            Telephone = defaultAssociate.DefaultTelephoneNumber;
            Fax = defaultAssociate.DefaultFaxNumber;
            Mobile = defaultAssociate.DefaultMobile;
            Email = defaultAssociate.DefaultEmail;
            AddressLine1 = defaultAssociate.DefaultAddress.Line1;
            AddressLine2 = defaultAssociate.DefaultAddress.Line2;
            WorkEmail = contact.DefaultWorkEmail;
            HomeEmail = contact.DefaultHomeEmail;
            TelephoneWork = contact.DefaultWorkTelephoneNumber;
            TelephoneHome = contact.DefaultHomeTelephoneNumber;
        }

        public string Name { get; private set; }
        public string Contact { get; private set; }
        public string Salutation { get; private set; }
        public string Telephone { get; private set; }
        public string Fax { get; private set; }
        public string Mobile { get; private set; }
        public string Email { get; private set; }
        public string WorkEmail { get; private set; }
        public string HomeEmail { get; private set; }
        public string AddressLine1 { get; private set; }
        public string AddressLine2 { get; private set; }
        public string TelephoneWork { get; private set; }
        public string TelephoneHome { get; private set; }
    }
}