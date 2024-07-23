namespace FWBS.OMS.Parsers
{
    public sealed class ParserContextFactory
    {
        public IParserContext CreateContext(object data)
        {
            var factory = new ContextFactory();

            var context = factory.CreateContext(data);

            return new ParserContext(context);

        }
    }
}
