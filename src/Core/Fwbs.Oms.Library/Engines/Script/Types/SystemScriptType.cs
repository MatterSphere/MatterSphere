namespace FWBS.OMS.Script
{
    public class SystemScriptType : ScriptType
    {
        private ContextFactory factory = new ContextFactory();
        private IContext context = null;


        public override object CurrentObj
        {
            get { return Session.CurrentSession; }
        }

        new protected SystemScriptType CurrentScript
        {
            get
            {
                return (SystemScriptType)base.CurrentScript;
            }
        }

        public override IContext Context
        {
            get 
            {
                if (context == null)
                    context = factory.CreateContext(Session.CurrentSession);

                return context; 
            }
        }

    }
}
