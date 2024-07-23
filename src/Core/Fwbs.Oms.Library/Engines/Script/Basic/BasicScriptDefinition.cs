namespace FWBS.OMS.Script
{
    internal sealed class BasicScriptDefinition : CodeDomScriptDefinition
    {
        #region Constructors

        public BasicScriptDefinition(ScriptGen gen)
            :base(gen)
        {
        }

        #endregion

        public override IScriptBuilder CreateDefaultBuilder()
        {
            return new BasicScriptBuilder();
        }

        public override IScriptBuilder CreateBuilder()
        {
            if (!Session.CurrentSession.IsLoggedIn)
                return CreateDefaultBuilder();

            var builder = Session.CurrentSession.Container.TryResolve<IScriptBuilder>(TypeCode);
            if (builder != null)
                return builder;

            return Session.CurrentSession.Container.Resolve<IScriptBuilder>("BASIC");
        }
    }
}
