namespace Fwbs.Documents
{
    public interface ICustomPropertiesDocument : IRawDocument
    {
        void ReadCustomProperties(CustomPropertyCollection properties);
        void WriteCustomProperties(CustomPropertyCollection properties);
    }
}
