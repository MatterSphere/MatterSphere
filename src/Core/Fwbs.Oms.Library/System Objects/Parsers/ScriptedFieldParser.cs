using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;

namespace FWBS.OMS.Parsers
{
    [Export(typeof(IFieldParser))]
    internal sealed class ScriptedFieldParser : IFieldParser
    {
        private const string FieldPrefix = "SCR_";

        private Dictionary<string, FWBS.OMS.Script.ScriptGen> scripts = new Dictionary<string, FWBS.OMS.Script.ScriptGen>();

        public ScriptedFieldParser()
        {
        }

        #region IFieldParser Members

        public FieldValue Parse(IParserContext context, string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");


            var pars = name.Split(';');

            if (pars.Length == 1)
                throw new ArgumentNullException("There is no specified method.");

            var p1 = pars[0];

            var sname = p1.Substring(FieldPrefix.Length);

            FWBS.OMS.Script.ScriptGen script = null;

            if (String.IsNullOrEmpty(sname))
            {
                script = Session.CurrentSession.Script;
            }
            else
            {
                if (!scripts.TryGetValue(sname, out script))
                {
                    script = FWBS.OMS.Script.ScriptGen.GetScript(sname);
                    scripts.Add(sname, script); 
                }

                script.Load();
            }

            var method = pars[1];

            object ret;

            if (Execute(script, context, method, out ret))
                return new FieldValue(ret);

            throw new InvalidOperationException("Script or Script method not found.");

        }

        private bool Execute(FWBS.OMS.Script.ScriptGen script, IParserContext context, string name, out object val)
        {
            val = null;

            if (script == null)
                return false;

            if (script.Scriptlet == null)
                return false;

            var parser = script.Scriptlet as IFieldParser;
            if (parser != null)
            {
                if (parser.CanHandle(name))
                {
                    val = parser.Parse(context, name);
                    return true;
                }
            }

            var meth = script.Scriptlet.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase, null, new[] { typeof(IParserContext) }, null);

            if (meth != null)
            {
                val = Invoke(script, meth, context);
                return true;
            }

            meth = script.Scriptlet.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase, null, Type.EmptyTypes, null);

            if (meth != null)
            {
                val = meth.Invoke(script.Scriptlet, null);
                return true;
            }

            return false;
        }

        private static object Invoke(FWBS.OMS.Script.ScriptGen script, MethodInfo method, params object[] pars)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            if (method.ReturnType == typeof(void))
                throw new InvalidOperationException("Method must return a value.");

            return method.Invoke(script.Scriptlet, pars);
        }

        public bool CanHandle(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            return name.StartsWith(FieldPrefix);
        }

        #endregion

    }

}
