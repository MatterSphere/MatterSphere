namespace Fwbs.Documents
{
    public interface IDocPropHandler
    {
        bool Handles(System.IO.FileInfo file);
        IRawDocument CreateDocument(System.IO.FileInfo file);
    }
}
