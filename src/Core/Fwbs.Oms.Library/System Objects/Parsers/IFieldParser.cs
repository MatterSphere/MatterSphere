namespace FWBS.OMS.Parsers
{
    public interface IFieldParser
    {
        bool CanHandle(string name);

        FieldValue Parse(IParserContext context, string name);
    }
}
