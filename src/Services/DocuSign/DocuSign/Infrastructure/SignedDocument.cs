namespace FWBS.OMS.DocuSign
{
   public class SignedDocument
    {
        public uint Order { get; internal set; }
        public long Id { get; internal set; }
        public string Description { get; internal set; }
        public byte[] Data { get; internal set; }
        public bool? IsCertificate { get; internal set; }
    }
}
