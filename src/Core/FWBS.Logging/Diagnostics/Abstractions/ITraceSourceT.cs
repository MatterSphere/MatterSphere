﻿namespace FWBS.Logging
{
    /// <summary>
    /// Defines a set of methods and properties that enable applications to trace the 
    /// execution of code and associate trace messages with a source related to
    /// a specific class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The source is usually related to the specified class based on the class
    /// name or other properties, such as the assembly the class is from.
    /// </para>
    /// <para>
    /// For a suitable default implementation, which bases the source on the
    /// assembly the class is from see AssemblyTraceSource.
    /// </para>
    /// </remarks>
    public interface ITraceSource<T> : ITraceSource
    {
    }
}
