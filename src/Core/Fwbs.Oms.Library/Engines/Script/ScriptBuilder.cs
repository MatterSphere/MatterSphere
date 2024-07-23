using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FWBS.OMS.Script
{
    internal abstract class ScriptBuilder : IScriptBuilder
    {

        #region IScriptBuilder

        public IEnumerable<CodeCompileUnit> Build(IScriptDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");

            var attrunit = CreateAttributeUnit(definition);

            var inherited = CreateInheritedUnit(definition);

            var assemblyunit = CreateAssemblyUnit(definition);

            var wfunit = CreateWorkflowUnit(definition);

            var main = GenerateMainCode(definition);

            var units = new List<CodeCompileUnit>();

            if (main == null)
                main = new CodeSnippetCompileUnit(String.Empty);

            units.Add(main);
            
            if (inherited != null)
                units.Add(inherited);
            if (attrunit != null)
                units.Add(attrunit);
            
            if (wfunit != null)
                units.Add(wfunit);

            if (assemblyunit != null)
                units.Add(assemblyunit);

            return units.AsReadOnly();

        }

        #endregion

        protected abstract CodeCompileUnit GenerateMainCode(IScriptDefinition definition);


        private CodeCompileUnit CreateWorkflowUnit(IScriptDefinition definition)
        {
            var unit = new CodeCompileUnit();

            var ns = new CodeNamespace(definition.Namespace);

            unit.Namespaces.Add(ns);

            var st = definition as ScriptDefinition;
            if (st == null)
                return unit;

            var cl = new CodeTypeDeclaration(definition.Name);
            cl.IsPartial = true;
            ns.Types.Add(cl);

            cl.Members.AddRange(st.WorkflowEventMethods.ToArray());

            return unit;
        }

        private CodeCompileUnit CreateInheritedUnit(IScriptDefinition definition)
        {
            var unit = new CodeCompileUnit();

            var ns = new CodeNamespace(definition.Namespace);

            unit.Namespaces.Add(ns);

            var st = definition as ScriptDefinition;
            if (st == null)
                return unit;
            
            CodeTypeDeclaration cl = new CodeTypeDeclaration(definition.Name);
            cl.IsPartial = true;
            cl.BaseTypes.Add(definition.BaseType);
            cl.TypeAttributes = TypeAttributes.Public;
            ns.Types.Add(cl);

         
            return unit;
        }

        private static CodeCompileUnit CreateAttributeUnit(IScriptDefinition definition)
        {
            var unit = new CodeCompileUnit();
            unit.AssemblyCustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(ScriptGenAssemblyAttribute))));
           return unit;
        }

        protected virtual CodeCompileUnit CreateAssemblyUnit(IScriptDefinition definition)
        {
            var unit = new CodeCompileUnit();
            unit.AssemblyCustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(System.Reflection.AssemblyVersionAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression(String.Format("1.0.0.{0}", definition.Version)))));
            unit.AssemblyCustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(System.Reflection.AssemblyTitleAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression(definition.Description))));
            unit.AssemblyCustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(System.Reflection.AssemblyDescriptionAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression(String.Format("OMS Scriptlet - {0}", definition.TypeDescription)))));
            unit.AssemblyCustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(System.Reflection.AssemblyCompanyAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression(definition.Author))));
            unit.AssemblyCustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(System.Reflection.AssemblyProductAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression("OMS.NET"))));
            return unit;
        }

 
    }
}
