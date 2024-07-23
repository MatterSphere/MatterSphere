namespace FWBS.OMS.DocuSign
{
    public class DocumentInfo
    {
        public uint Order { get; set; }
        public long Id { get; set; }
        public string Description { get; set; }
        public byte[] Data { get; set; }
        public string FileExtension { get; set; }
    }
}