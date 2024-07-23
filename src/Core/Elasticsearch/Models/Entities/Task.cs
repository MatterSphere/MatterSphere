using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;

namespace Elasticsearch.Models.Entities
{
    public class Task : BaseItem
    {
        [JsonProperty("file-id")]
        public long FileId { get; set; }

        [JsonProperty("taskType")]
        public string TaskType { get; set; }

        [JsonProperty("tskDesc")]
        public string TaskDescription { get; set; }

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

        public ResponseItem GetResponseItem()
        {
            TaskDescription = GetHighlight(TaskDescription, "tskDesc");
            FileDescription = GetHighlight(FileDescription, "fileDesc");
            Title = GetHighlight(Title, "title");
            Summary = GetHighlight(Summary, "summary");

            return new ResponseItem
            {
                ElasticsearchId = ElasticsearchId,
                MatterSphereId = MatterSphereId,
                Name = TaskDescription,
                Title = Title,
                Summary = IsSummaryFieldEnabled && !string.IsNullOrWhiteSpace(Summary) ? Summary : $"{FileDescription}: {TaskDescription} ({TaskType})",
                Author = null,
                ModifiedDate = ModifiedDate,
                ContactId = null,
                ClientId = ClientId,
                FileId = FileId,
                DocumentId = null,
                ObjectType = EntityType.Convert(EntityTypeEnum.Task)
            };
        }
    }
}
