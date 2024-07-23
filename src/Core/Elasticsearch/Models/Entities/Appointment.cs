using System;
using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;

namespace Elasticsearch.Models.Entities
{
    public class Appointment : BaseItem
    {
        [JsonProperty("file-id")]
        public long FileId { get; set; }

        [JsonProperty("appointmentType")]
        public string AppointmentType { get; set; }

        [JsonProperty("appLocation")]
        public string AppointmentLocation { get; set; }

        [JsonProperty("appDesc")]
        public string AppointmentDescription { get; set; }

        #region File
        [JsonProperty("client-Id")]
        public long ClientId { get; set; }

        [JsonProperty("fileStatus")]
        public string FileStatus { get; set; }

        [JsonProperty("fileType")]
        public string FileType { get; set; }

        [JsonProperty("fileDesc")]
        public string FileDescription { get; set; }
        #endregion

        #region Client
        [JsonProperty("clientNum")]
        public string ClientNumber { get; set; }

        [JsonProperty("name"), Obsolete("This will be renamed to clientName.")]
        private string Name { get; set; }

        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        [JsonProperty("clientType")]
        public string ClientType { get; set; }
        #endregion

        public ResponseItem GetResponseItem()
        {
            AppointmentDescription = GetHighlight(AppointmentDescription, "appDesc");
            ClientNumber = GetHighlight(ClientNumber, "clientNum");
            ClientName = GetHighlight(ClientName, "clientName");
            // this is workaround for renaming ClientName source field
            if (string.IsNullOrEmpty(ClientName))
            {
                ClientName = GetHighlight(Name, "name");
            }
            FileDescription = GetHighlight(FileDescription, "fileDesc");
            Title = GetHighlight(Title, "title");
            Summary = GetHighlight(Summary, "summary");

            return new ResponseItem
            {
                ElasticsearchId = ElasticsearchId,
                MatterSphereId = MatterSphereId,
                Name = AppointmentDescription,
                Title = Title,
                Summary = IsSummaryFieldEnabled && !string.IsNullOrWhiteSpace(Summary) ? Summary : $"{ClientNumber}:{ClientName}/{FileDescription}: {AppointmentDescription} ({AppointmentType}). Location: {AppointmentLocation}",
                Author = null,
                ModifiedDate = ModifiedDate,
                ContactId = null,
                ClientId = ClientId,
                FileId = FileId,
                DocumentId = null,
                ObjectType = EntityType.Convert(EntityTypeEnum.Appointment)
            };
        }
    }
}
