namespace Horizon.Common.Models.Repositories.IndexStructure
{
    public class IndexInfo
    {
        public IndexInfo(short id, string name, IndexTypeEnum type)
        {
            Id = id;
            Name = name;
            IndexType = type;
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public IndexTypeEnum IndexType { get; set; }
    }
}
