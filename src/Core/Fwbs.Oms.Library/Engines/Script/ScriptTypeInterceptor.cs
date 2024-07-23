using System;
using System.Reflection;

namespace FWBS.OMS.Script
{
    internal sealed class ScriptTypeInterceptor : Proxy.IInvocationHandler
    {
        private ScriptGen gen;
        private ScriptType script;

        public object Invoke(object proxy, MethodBase method, object[] arguments)
        {
            script = (ScriptType)proxy;

            switch (method.Name.ToUpperInvariant())
            {
                case "SETSCRIPTGENERATOROBJECT":
                    gen = (ScriptGen)arguments[0];
                    break;
            }

            return method.Invoke(proxy, arguments);
        }

   

        private static bool IsScriptMethod(MethodBase method)
        {
            return method.Attribute<ScriptMethodOverridableAttribute>() != null;
        }

        private static bool IsEventMethod(MethodBase method)
        {
            var mi = method as MethodInfo;
            if (mi == null)
                return false;

            if (mi.ReturnType != typeof(void))
                return false;

            var pars = mi.GetParameters();

            if (pars.Length != 2)
                return false;

            return typeof(EventArgs).IsAssignableFrom(pars[1].ParameterType);
        }


    }
}
