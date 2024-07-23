namespace FWBS.OMS.Parsers
{
    public sealed class ParserContext : Context, IParserContext
	{
		public ParserContext()
		{
		}

		internal ParserContext(Context context) : base(context)
		{
  
		}

		public override IContext Clone()
		{
			return new ParserContext(this);
		}
	}
}
