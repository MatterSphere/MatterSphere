using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Proxies;
using Fwbs.Framework.Extensibility.Proxy;

namespace FWBS.OMS.Proxy
{
    internal class DynamicProxy : RealProxy, IRemotingTypeInfo, IProxy
    {

        private object proxyTarget;

        private bool strict;

        private Type[] supportedTypes;

        private Type baseType;

        private object baseObject;



        protected internal DynamicProxy(object proxyTarget, bool strict, Type baseType, Type[] supportedTypes)
            : base(baseType)
        {
            this.proxyTarget = proxyTarget;
            this.strict = strict;
            this.supportedTypes = supportedTypes;
            this.baseType = baseType;
            this.baseObject = Activator.CreateInstance(baseType);
        }

        public override ObjRef CreateObjRef(System.Type type)
        {
            throw new NotSupportedException("ObjRef for DynamicProxy isn't supported");
        }

        public bool CanCastTo(System.Type toType, object obj)
        {
            // Assume we can (which is the default unless strict is true)
            bool canCast = true;

            if (strict)
            {
                // First check if the proxyTarget supports the cast
                if (toType.IsAssignableFrom(proxyTarget.GetType()))
                {
                    canCast = true;
                }
                else if (supportedTypes != null)
                {
                    canCast = false;
                    // Check if the list of supported interfaces supports the cast
                    foreach (Type type in supportedTypes)
                    {
                        if (toType == type)
                        {
                            canCast = true;
                            break;
                        }
                    }
                }
                else
                {
                    canCast = false;
                }
            }

            return canCast;
        }


        public string TypeName
        {
            get { throw new System.NotSupportedException("TypeName for DynamicProxy isn't supported"); }
            set { throw new System.NotSupportedException("TypeName for DynamicProxy isn't supported"); }
        }

        public override System.Runtime.Remoting.Messaging.IMessage Invoke(System.Runtime.Remoting.Messaging.IMessage message)
        {
            try
            {
                // Convert to a MethodCallMessage
                System.Runtime.Remoting.Messaging.IMethodCallMessage methodMessage = new System.Runtime.Remoting.Messaging.MethodCallMessageWrapper((System.Runtime.Remoting.Messaging.IMethodCallMessage)message);

                // Extract the method being called
                System.Reflection.MethodBase method = methodMessage.MethodBase;

                // Perform the call
                object returnValue = null;

                object[] args = null;

                if (methodMessage.HasVarArgs)
                    args = methodMessage.Args;
                else
                    args = methodMessage.Args;

                if (! CanCastTo(baseType, proxyTarget) && (method.DeclaringType == baseType || !method.DeclaringType.IsAssignableFrom(proxyTarget.GetType())))
                {
                    // Handle IDynamicProxy interface calls on this instance instead of on the proxy target instance
                    returnValue = method.Invoke(baseObject, args);
                }
                else
                {
                    // Delegate to the invocation handler
                    var invoker = baseObject as IInvocationHandler;
                    var invoker2 = proxyTarget as IInvocationHandler;
                    if (invoker != null)
                    {
                        returnValue = invoker.Invoke(proxyTarget, method, args);
                    }

                    else if (invoker2 != null)
                    {
                        returnValue = invoker2.Invoke(baseObject, method, args);
                    }
                    else
                        returnValue = method.Invoke(proxyTarget, args);
                }

                // Create the return message (ReturnMessage)
                System.Runtime.Remoting.Messaging.ReturnMessage returnMessage = new System.Runtime.Remoting.Messaging.ReturnMessage(returnValue, methodMessage.Args, methodMessage.ArgCount, methodMessage.LogicalCallContext, methodMessage);
                return returnMessage;

            }
            catch (Exception ex)
            {
                if (!HandleException(ex))
                    throw;
                return null;
            }
        }

        public object ProxyTarget
        {
            get { return proxyTarget; }
        }


        public bool Strict
        {
            get { return strict; }
        }

        public Type[] SupportedTypes
        {
            get { return supportedTypes; }
        }

        public Type BaseType
        {
            get
            {
                return baseType;
            }
        }

        public object BaseObject
        {
            get
            {
                return baseObject;
            }
        }

        private bool HandleException(Exception ex)
        {
            var tex = ex as TargetInvocationException;
            if (tex != null)
            {
                throw tex.InnerException;
            }

            return false;
        }
    }
}
