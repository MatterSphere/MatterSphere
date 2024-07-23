using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace FWBS.OMS.Script
{
    [Export("WORKFLOW", typeof(IScriptBuilder))]
	internal class WorkflowScriptBuilder : IScriptBuilder
	{
		public IEnumerable<CodeCompileUnit> Build(IScriptDefinition definition)
		{
			var codeunits = definition.CreateDefaultBuilder().Build(definition);

            var main = codeunits.First();

            var ns = main.Namespaces.Cast<CodeNamespace>().FirstOrDefault(a=>a.Name.Equals(definition.Namespace));

            if (ns != null)
            {
                var type = ns.Types.Cast<CodeTypeDeclaration>().FirstOrDefault(a=>a.Name.Equals(definition.Name));
                if (type != null)
                {
                    ns.Types.Remove(type);
                }
            }

            return codeunits;
		}
	}

	public class WorkflowScriptType : ScriptType
	{
		private ContextFactory factory = new ContextFactory();
		private IContext context = null;

		internal protected override CodeNamespaceImport[] NamespaceImports
		{
			get
			{
				CodeNamespaceImport[] ns = new CodeNamespaceImport[]
				{
					new CodeNamespaceImport("System"),
					new CodeNamespaceImport("System.Activities"),
					new CodeNamespaceImport("System.Activities.Statements"),
					new CodeNamespaceImport("System.Activities.Expressions"),
					new CodeNamespaceImport("FWBS.Common"),
                    new CodeNamespaceImport("System.Linq"),
				};
				return ns;
			}
		}

        protected internal override string Namespace
        {
            get
            {
                return "OMS.Activities";
            }
        }

		protected internal override string[] AssemblyReferences
		{
			get
			{
				return new string[] {"System.Activities.dll" , "System.Core"};
			}
		}

		public override object CurrentObj
		{
			get { return Session.CurrentSession; }
		}

		new protected WorkflowScriptType CurrentScript
		{
			get
			{
				return (WorkflowScriptType)base.CurrentScript;
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
