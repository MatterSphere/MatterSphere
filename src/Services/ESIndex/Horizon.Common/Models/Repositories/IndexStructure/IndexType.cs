namespace Horizon.Common.Models.Repositories.IndexStructure
{
    public static class IndexType
    {
        public static IndexTypeEnum GetIndexType(string type)
        {
            switch (type.ToLower())
            {
                case "user":
                    return IndexTypeEnum.User;
                case "data":
                    return IndexTypeEnum.Data;
            }

            return IndexTypeEnum.Unknown;
        }

        public static string GetIndexType(IndexTypeEnum type)
        {
            switch (type)
            {
                case IndexTypeEnum.User:
                    return "user";
                case IndexTypeEnum.Data:
                    return "data";
            }

            return "error";
        }
    }
}
