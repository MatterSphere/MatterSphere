using System;
using System.Reflection;

namespace FWBS.OMS
{
    using Fwbs.Framework.Reflection;

    /// <summary>
    /// Workaround to Framework AssemblyManager issue.
    /// </summary>
    class AssemblyManager : IAssemblyManager
    {
        private IAssemblyManager _manager;

        public AssemblyManager(IAssemblyManager manager)
        {
            _manager = manager;
        }

        void IAssemblyManager.Alias(string aliasAssemblyName, string actualAssemblyName)
        {
            _manager.Alias(aliasAssemblyName, actualAssemblyName);
        }

        Assembly IAssemblyManager.Load(string assemblyName)
        {
            Assembly assembly;
            try
            {
                assembly = _manager.Load(assemblyName);
            }
            catch (BadImageFormatException ex)
            {
                assembly = _manager.TryLoadFrom(ex.FileName.Replace("GAC_32", "GAC_64"));
                if (assembly == null)
                    throw;
            }
            return assembly;
        }

        Assembly IAssemblyManager.LoadFrom(string filePath)
        {
            return _manager.LoadFrom(filePath);
        }

        void IAssemblyManager.Register(IAssemblyResolver resolver)
        {
            _manager.Register(resolver);
        }

        Assembly IAssemblyManager.TryLoad(string assemblyName)
        {
            return _manager.TryLoad(assemblyName);
        }

        Assembly IAssemblyManager.TryLoadFrom(string filePath)
        {
            return _manager.TryLoadFrom(filePath);
        }
    }
}
