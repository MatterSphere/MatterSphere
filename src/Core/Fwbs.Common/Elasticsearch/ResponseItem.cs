using System;

namespace FWBS.Common.Elasticsearch
{
    public class ResponseItem
    {
        public string ElasticsearchId { get; set; }
        public string MatterSphereId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string DocumentType { get; set; }
        public string ObjectType { get; set; }
        public string Extension { get; set; }
        public long? ContactId { get; set; }
        public long? ClientId { get; set; }
        public long? FileId { get; set; }
        public long? DocumentId { get; set; }
        public long? AssociateId { get; set; }
    }
}
