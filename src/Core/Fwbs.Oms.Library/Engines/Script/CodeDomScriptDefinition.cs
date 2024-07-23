using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FWBS.OMS.Script
{
    internal abstract class CodeDomScriptDefinition : ScriptDefinition
    {
        #region Fields

        private readonly List<CodeMemberField> fields = new List<CodeMemberField>();
        private readonly List<CodeMemberMethod> methods = new List<CodeMemberMethod>();
        private readonly List<CodeMemberMethod> eventmethods = new List<CodeMemberMethod>();

        #endregion

        #region Constructors

        public CodeDomScriptDefinition(ScriptGen gen)
            : base(gen)
        {
        }

        #endregion

        #region Properties

        public IEnumerable<CodeNamespaceImport> Imports
        {
            get
            {
                return Gen.ScriptType.NamespaceImports;
            }
        }


        public IEnumerable<CodeMemberField> Fields
        {
            get
            {
                Download();

                return fields;
            }
        }

        public IEnumerable<CodeMemberMethod> Methods
        {
            get
            {
                Download();

                return methods;
            }
        }

        #endregion

        #region Methods

        protected override void Download()
        {
            if (IsDownloaded)
                return;

            base.Download();

            fields.Clear();
            methods.Clear();
            eventmethods.Clear();

            BuildFields();
            BuildOverridableMethods();
            BuildEventMethods();
            BuildStaticMethods();
        }

        private void BuildFields()
        {
            var el_script = Xml.Element("script");
            if (el_script == null)
                return;

            var el_fields = Xml.Element("fields");

            if (el_fields == null)
                return;


            foreach (var el in el_fields.Elements("field"))
            {
                var attr_name = el.Attribute("name");
                var attr_type = el.Attribute("type");

                if (attr_name == null || String.IsNullOrWhiteSpace(attr_name.Value))
                    continue;

                if (attr_type == null || String.IsNullOrWhiteSpace(attr_type.Value))
                    continue;

                var fld = new CodeMemberField(attr_type.Value, attr_name.Value);
                fields.Add(fld);
            }
        }

        private void BuildCustomFields()
        {

            var el_script = Xml.Element("script");
            if (el_script == null)
                return;

            var el_customfields = Xml.Element("customFields");

            if (el_customfields == null)
                return;

            foreach (var el in el_customfields.Elements("customField"))
            {
                var attr_name = el.Attribute("name");
                var attr_type = el.Attribute("type");

                if (attr_name == null || String.IsNullOrWhiteSpace(attr_name.Value))
                    continue;

                if (attr_type == null || String.IsNullOrWhiteSpace(attr_type.Value))
                    continue;

                var fld = new CodeMemberField(attr_type.Value, attr_name.Value);
                fields.Add(fld);
            }
        }

        private void BuildOverridableMethods()
        {

            var el_script = Xml.Element("script");
            if (el_script == null)
                return;

            var el_methods = el_script.Element("methods");
            if (el_methods == null)
                return;

            foreach (var method in Gen.ScriptType.Methods)
            {
                var el = el_methods.Elements("method").FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name").Value == method.Name);

                if (el == null || String.IsNullOrWhiteSpace(el.Value))
                    continue;

                method.Statements.Add(new CodeSnippetStatement(el.Value));
                methods.Add(method);
            }

        }

        private void BuildEventMethods()
        {
            var el_script = Xml.Element("script");
            if (el_script == null)
                return;

            var el_methods = el_script.Element("dynamicMethods");
            if (el_methods == null)
                return;

            foreach (var el_method in el_methods.Elements("dynamicMethod"))
            {
                CodeTryCatchFinallyStatement tcf;
                var meth = CreateDelegateMethod(el_method, out tcf);

                if (meth == null)
                    continue;

                tcf.AddCatchErrorBoxHandler();

                methods.Add(meth);
            }

        }


        private void BuildStaticMethods()
        {

            var el_script = Xml.Element("script");
            if (el_script == null)
                return;

            var el_staticMethod = el_script.Element("staticMethods");

            if (el_staticMethod == null)
                return;

            foreach (var el in el_staticMethod.Elements("staticMethod"))
            {
                var attr_name = el.Attribute("name");
                var attr_type = el.Attribute("type");

                if (attr_name == null || String.IsNullOrWhiteSpace(attr_name.Value))
                    continue;

                if (String.IsNullOrWhiteSpace(el.Value))
                    continue;

                var meth = new CodeMemberMethod();
                meth.Name = attr_name.Value;
                meth.ReturnType = new CodeTypeReference(typeof(void));
                meth.Attributes = MemberAttributes.Public;
                meth.Statements.Add(new CodeSnippetStatement(el.Value));
                methods.Add(meth);
            }
        }

        internal static CodeMemberMethod CreateDelegateMethod(XElement el_method, out CodeTryCatchFinallyStatement tcf)
        {
            tcf = null;

            var attr_name = el_method.Attribute("name");

            if (attr_name == null || String.IsNullOrWhiteSpace(attr_name.Value))
                return null;

            if (String.IsNullOrWhiteSpace(el_method.Value))
                return null;

            var attr_del = el_method.Attribute("delegate");

            return CreateDelegateMethod(attr_name.Value, attr_del == null ? null : attr_del.Value, el_method.Value, out tcf);
           
        }

        internal static CodeMemberMethod CreateDelegateMethod(string name, string delegateType, string code, out CodeTryCatchFinallyStatement tcf)
        {
            var meth = new CodeMemberMethod();
            var methType = typeof(EventHandler);
            if (delegateType != null && !String.IsNullOrWhiteSpace(delegateType))
                methType = Session.CurrentSession.TypeManager.Load(delegateType);

            var inv = methType.GetMethod("Invoke");
            meth.Name = String.Format("{0}", name);
            meth.Attributes = MemberAttributes.Private;
            meth.ReturnType = new CodeTypeReference(inv.ReturnType);

            foreach (var par in inv.GetParameters())
            {
                var p = new CodeParameterDeclarationExpression();
                p.Name = par.Name;
                p.Type = new CodeTypeReference(par.ParameterType);
                meth.Parameters.Add(p);
            }

            tcf = new CodeTryCatchFinallyStatement();
            tcf.TryStatements.Add(new CodeSnippetStatement(code));
            meth.Statements.Add(tcf);
            return meth;
        }

 
        internal static CodeMethodInvokeExpression CreateWorkflowInvokeMethod(string workflow)
        {            
            var invoke = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "ExecuteWorkflow"));
            invoke.Parameters.Add(new System.CodeDom.CodePrimitiveExpression(workflow));
            return invoke;
        }


        #endregion

    }


    internal static class CodeDomExtensions
    {
        internal static void AddCatchErroBoxHandlerNoUI(this CodeTryCatchFinallyStatement tcf)
        {
            var clause = new CodeCatchClause("ex", new CodeTypeReference(typeof(Exception)));
            var invoke = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "HandleException"));
            invoke.Parameters.Add(new CodeArgumentReferenceExpression("ex"));
            clause.Statements.Add(invoke);
            tcf.CatchClauses.Add(clause);
        }

        internal static void AddCatchErrorBoxHandler(this CodeTryCatchFinallyStatement tcf)
        {
            var clause = new CodeCatchClause("ex", new CodeTypeReference(typeof(Exception)));
            clause.Statements.Add(new CodeExpressionStatement(new CodeSnippetExpression("FWBS.OMS.UI.Windows.ErrorBox.Show(ex)")));
            tcf.CatchClauses.Add(clause);
        }


    }

}
