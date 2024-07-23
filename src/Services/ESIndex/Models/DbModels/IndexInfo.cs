namespace Models.DbModels
{
    public class IndexInfo
    {
        public IndexInfo(short id, string name, string type)
        {
            Id = id;
            Name = name;

            switch (type.ToLower())
            {
                case "user":
                    IndexType = IndexTypeEnum.User;
                    break;
                case "data":
                    IndexType = IndexTypeEnum.Data;
                    break;
                default:
                    IndexType = IndexTypeEnum.Unknown;
                    break;
            }
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public IndexTypeEnum IndexType { get; set; }
    }
}
