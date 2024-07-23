namespace FWBS.OMS.DocuSign
{
    public class EnvelopeInfo
    {
        public string Subject { get; internal set; }
        public string EmailBlurb { get; internal set; }
        public DocumentInfo[] Documents { get; internal set; }
        public RecipientInfo[] Recipients { get; internal set; }
        public Reminder Reminder { get; internal set; }
        public Expiration Expiration { get; internal set; }
        public StatusCode StatusCode { get; internal set; }
    }
}