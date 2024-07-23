namespace FWBS.OMS.HighQ.Models
{
    internal class ColumnInfo
    {
        public ColumnInfo(int id, string title, string type)
        {
            Id = id;
            Title = title;
            Type = type;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
    }
}
