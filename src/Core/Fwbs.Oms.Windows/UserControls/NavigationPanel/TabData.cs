namespace FWBS.Common.UI.Windows
{
    internal class TabData
    {
        public TabData(string code, string description, string group, int imageIndex)
        {
            Code = code;
            Description = description;
            Group = group;
            ImageIndex = imageIndex;
        }

        public string Code { get; }
        public string Description { get; }
        public string Group { get; }
        public int ImageIndex { get; }
    }
}
