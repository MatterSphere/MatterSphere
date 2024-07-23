namespace Horizon.Common.Models.Repositories.IndexStructure
{
    public class IndexEntity
    {
        public IndexEntity(short id, string name, string table, string key)
        {
            Id = id;
            Name = name;
            TableName = table;
            Key = key;
            IndexingEnabled = true;
            IsDefault = true;
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
        public string Key { get; set; }
        public bool IndexingEnabled { get; set; }
        public bool IsDefault { get; set; }
        public string SummaryTemplate { get; set; }
    }
}
