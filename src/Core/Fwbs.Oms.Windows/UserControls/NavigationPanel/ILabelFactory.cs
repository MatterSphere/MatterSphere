namespace FWBS.Common.UI.Windows
{
    internal interface ILabelFactory
    {
        ucNavigationItem Create(string code, string description, int imageIndex = 0);
    }
}
