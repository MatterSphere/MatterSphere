using FWBS.OMS.HighQ.Converters.TokenPathProviders;

namespace FWBS.OMS.HighQ.Converters
{
    internal class PluralItemsTokenPathProvider : ITokenPathProvider
    {
        public string GetColumnIdPath(int itemIndex, int columnIndex)
        {
            return $"isheet.data.item[{itemIndex}].column[{columnIndex}].attributecolumnid";
        }

        public string GetColumnValuePath(int itemIndex, int columnIndex)
        {
            return $"isheet.data.item[{itemIndex}].column[{columnIndex}]";
        }

        public string GetItemIdPath(int itemIndex)
        {
            return $"isheet.data.item[{itemIndex}].itemid";
        }
    }
}
