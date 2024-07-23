namespace FWBS.OMS.HighQ.Models
{
    internal class FolderItem
    {
        public FolderItem(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}
