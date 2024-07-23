using System;

namespace FWBS.OMS.Script
{
    /// <summary>
    /// Flags a method within a script type object to be overriden within the assembly
    /// inheriting of the object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ScriptMethodOverridableAttribute : Attribute
    {
    }
}
