namespace FWBS.OMS.HighQ.Converters.TokenPathProviders
{
    internal interface ITokenPathProvider
    {
        string GetItemIdPath(int itemIndex);
        string GetColumnIdPath(int itemIndex, int columnIndex);
        string GetColumnValuePath(int itemIndex, int columnIndex);
    }
}
