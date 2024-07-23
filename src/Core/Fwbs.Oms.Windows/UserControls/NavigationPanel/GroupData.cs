using System.Collections.Generic;

namespace FWBS.Common.UI.Windows
{
    public class GroupData
    {
        public GroupData(string code)
        {
            Code = code;
            Items = new List<ItemData>();
        }

        public string Code { get; }
        public List<ItemData> Items { get; }

        public void AddItem(string code, string description, int imageIndex)
        {
            Items.Add(new ItemData(code, description, imageIndex));
        }

        public class ItemData
        {
            public ItemData(string code, string description, int imageIndex)
            {
                Code = code;
                Description = description;
                ImageIndex = imageIndex;
            }

            public string Code { get; set; }
            public string Description { get; set; }
            public int ImageIndex { get; set; }
        }
    }
}
