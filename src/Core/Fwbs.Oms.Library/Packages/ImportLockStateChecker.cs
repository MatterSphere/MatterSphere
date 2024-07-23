using System;
using System.Reflection;

namespace FWBS.OMS.Design.Import
{
    public class ImportLockStateChecker
    {
        private Assembly assembly = null;

        public bool CheckObjectLockState(string code, string type)
        {
            if (assembly == null)
                assembly = Assembly.Load("OMS.UI, Version=" + Session.CurrentSession.AssemblyVersion.ToString() + ", Culture=neutral, PublicKeyToken=7212801a92a1726d");

            Type lockState = assembly.GetType("FWBS.OMS.UI.Windows.LockState");
            var lockstateinstance = Activator.CreateInstance(lockState);
            MethodInfo method = lockState.GetMethod("ObjectLockStateCheck");
            object ret = method.Invoke(lockstateinstance, new[] { code, type });
            return (Boolean)ret;
        }
    }
}