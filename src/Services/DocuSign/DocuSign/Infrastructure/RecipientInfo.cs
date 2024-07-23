namespace FWBS.OMS.DocuSign
{
    public class RecipientInfo
    {
        public uint Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public RecipientType Type { get; set; }
        public string Tag { get; set; }
    }
}