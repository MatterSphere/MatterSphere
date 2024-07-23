using System.Reflection;

namespace FWBS.OMS.Proxy
{
    internal interface IInvocationHandler
    {
        object Invoke(object proxy, MethodBase method, object[] arguments);
    }
}
