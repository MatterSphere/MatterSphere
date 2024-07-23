using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace FWBS.OMS.Script
{
    internal abstract class ScriptDefinition : IScriptDefinition
    {
        #region Fields

        private readonly ScriptGen gen;
        private readonly List<IReference> references = new List<IReference>();
        private readonly List<CodeMemberMethod> workflowmethods = new List<CodeMemberMethod>();
 
        private XElement xml;

        #endregion

        #region Constructors

        protected ScriptDefinition(ScriptGen gen)
        {
            if (gen == null)
                throw new ArgumentNullException("gen");

            this.gen = gen;
        }

        #endregion

        #region Properties

        protected ScriptGen Gen
        {
            get
            {
                return gen;
            }
        }

        public string Name
        {
            get {return gen.Code; }
        }

        public string Namespace
        {
            get
            {
                return gen.ScriptType.Namespace;
            }
        }

        public long Version
        {
            get { return gen.Version; }
        }

        public string Author
        {
            get { return gen.Author; }
        }

        public string Description
        {
            get { return gen.ScriptDescription; }
        }

        public string TypeCode
        {
            get { return gen.Usage; }
        }

        public string TypeDescription
        {
            get {return gen.ScriptTypeDescription; }
        }

        public Type BaseType
        {
            get { return gen.ScriptType.GetType(); }
        }

        public ScriptLanguage Language
        {
            get { return gen.Language; }
        }

        public XElement Xml
        {
            get 
            {
                Download();

                return xml; 
            }
            
        }

        public IEnumerable<IReference> References
        {
            get 
            {
                Download();

                return references; 
            }
        }

        public IEnumerable<CodeMemberMethod> WorkflowEventMethods
        {
            get
            {
                Download();

                return workflowmethods;
            }
        }

        public virtual IScriptBuilder CreateBuilder()
        {
            if (!Session.CurrentSession.IsLoggedIn)
                return CreateDefaultBuilder();

            var builder = Session.CurrentSession.Container.TryResolve<IScriptBuilder>(TypeCode);
            if (builder != null)
                return builder;

            return Session.CurrentSession.Container.Resolve<IScriptBuilder>(null);
        }


        public abstract IScriptBuilder CreateDefaultBuilder();


        public IEnumerable<Tuple<string, string>> ProviderOptions
        {
            get
            {
                if (Session.CurrentSession.IsLoggedIn)
                {
                    var ret = Session.CurrentSession.ScriptProviderOptions;

                    return ScriptUtils.SplitProviderOptions(ret);
                }
                else
                    return Enumerable.Empty<Tuple<string, string>>();
            }
        }

       

        public IEnumerable<string> CompilerOptions
        {
            get 
            {
                if (Session.CurrentSession.IsLoggedIn)
                {
                    var ret = Session.CurrentSession.ScriptCompilerOptions;

                    return ret.Split(new char[]{';'} , StringSplitOptions.RemoveEmptyEntries);
                }
                else
                    return Enumerable.Empty<String>();
            }
        }

        public IEnumerable<string> ConditionalCompilationSymbols
        {
            get
            {
                if (Session.CurrentSession.IsLoggedIn)
                {
                    var ret = Session.CurrentSession.ScriptConditionalCompilationSymbols;

                    return ret.Split(new char[]{' ', ',', ';'}, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                    return Enumerable.Empty<String>();
            }
        }


        protected bool IsDownloaded
        {
            get
            {
                return xml != null;
            }
        }

        #endregion

        #region Methods

        protected virtual void Download()
        {
            if (IsDownloaded)
                return;

            references.Clear();
            workflowmethods.Clear();
            this.xml = XElement.Parse(gen.RawXML);
            BuildReferences();
            BuildDistributedReferences();
            BuildScriptReferences();
            BuildWorkflowMethods();
        }

        private static IEnumerable<string> GetAssemblyReferences(ScriptType type)
        {
            return type.AssemblyReferences
                .Union(new string[] { 
                    "System.dll",
					"System.Xml.dll",
					"System.Data.dll",
					"System.Windows.Forms.dll",

                    "OMS.Infrastructure.dll", 
					"OMS.Library.dll",
					"OMS.UI.dll",
					"OMS.Data.dll",
					"FWBS.Common.dll",
                   })
                .Distinct();
        }

        private void BuildReferences()
        {
            var el_script = Xml.Element("script");
            if (el_script == null)
                return;

            var refs = new Dictionary<string, AssemblyReference>(StringComparer.OrdinalIgnoreCase);

            foreach (var r in GetAssemblyReferences(gen.ScriptType))
            {
                if (String.IsNullOrWhiteSpace(r))
                    continue;


                var ass = new AssemblyReference(r);

                ass.IsRequired = true;

                if (refs.ContainsKey(ass.Name))
                    continue;

                refs.Add(ass.Name, ass);
            }

            var el_refs = el_script.Element("references");
            if (el_refs != null)
            {

                foreach (var el in el_refs.Elements())
                {
                    var r = el.Value;

                    if (String.IsNullOrWhiteSpace(r))
                        continue;

                    var ass = new AssemblyReference(r);

                    if (refs.ContainsKey(ass.Name))
                        continue;

                    refs.Add(ass.Name, ass);
                }

            }

            references.AddRange(refs.Values);

        }

        private void BuildDistributedReferences()
        {
            var el_script = Xml.Element("script");
            if (el_script == null)
                return;

            var el_refs = el_script.Element("distributions");
            if (el_refs == null)
                return;

            var output = ScriptUtils.GetOutputName(this);

            foreach (var el in el_refs.Elements())
            {
                var attr_isdist = el.Attribute("isDistributed");
                var attr_name = el.Attribute("filename");
                var attr_modified = el.Attribute("modified");

                FileInfo file = new FileInfo(attr_name.Value);

                if (attr_isdist != null && !String.IsNullOrWhiteSpace(attr_isdist.Value))
                {
                    bool isdist;
                    if (bool.TryParse(attr_isdist.Value, out isdist) && isdist)
                    {
                        var dup = references.FirstOrDefault(n => n.AssemblyName.ToUpperInvariant() == Path.GetFileNameWithoutExtension(file.Name).ToUpperInvariant());
                        if (dup != null)
                            references.Remove(dup);
                        references.Add(new DistributedAssemblyReference(attr_name.Value) { Location = file.FullName });
                        continue;
                    }
                }

                DateTime modified;

                var embedded = new EmbeddedAssemblyReference(attr_name.Value){Location = file.FullName, Data = el.Value };

                if (attr_modified != null && attr_modified.Value != null && DateTime.TryParse(attr_modified.Value, out modified))
                    embedded.Modified = modified;

                ScriptUtils.Extract(embedded, output);

      
                var emdup = references.FirstOrDefault(n => n.AssemblyName.ToUpperInvariant() == Path.GetFileNameWithoutExtension(file.Name).ToUpperInvariant());
                if (emdup != null)
                    references.Remove(emdup);

                references.Add(embedded);
            }

        }




        private void BuildScriptReferences()
        {

            var el_script = Xml.Element("script");
            if (el_script == null)
                return;

            var el_refs = el_script.Element("scriptReferences");
            if (el_refs == null)
                return;

            foreach (var el in el_refs.Elements("reference"))
            {

               if (String.IsNullOrWhiteSpace(el.Value))
                   continue;


                references.Add(new ScriptReference(el.Value));
            }

        }

        private void BuildWorkflowMethods()
        {
            var el_script = Xml.Element("script");
            if (el_script == null)
                return;

            var el_workflows = el_script.Element("workflows");
            if (el_workflows == null)
                return;

            foreach (var el_method in el_workflows.Elements("workflow"))
            {
                var attr_del = el_method.Attribute("delegate");

                if (attr_del == null || String.IsNullOrWhiteSpace(attr_del.Value))
                    continue;

                CodeTryCatchFinallyStatement tcf;

                var meth = CodeDomScriptDefinition.CreateDelegateMethod(el_method, out tcf);

                if (meth == null)
                    continue;


                var invoke = CodeDomScriptDefinition.CreateWorkflowInvokeMethod(el_method.Value);
                tcf.TryStatements.Clear();
                tcf.TryStatements.Add(invoke);
                tcf.AddCatchErroBoxHandlerNoUI();
                workflowmethods.Add(meth);
            }

            foreach (var meth in Gen.ScriptType.Methods)
            {
                var el_method = el_workflows.Elements("workflow").FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name").Value == meth.Name);

                if (el_method == null || String.IsNullOrWhiteSpace(el_method.Value))
                    continue;

                var invoke = CodeDomScriptDefinition.CreateWorkflowInvokeMethod(el_method.Value);
                meth.Statements.Clear();
                meth.Statements.Add(invoke);
                workflowmethods.Add(meth);
            }
        }

        #endregion


 
    }
}
