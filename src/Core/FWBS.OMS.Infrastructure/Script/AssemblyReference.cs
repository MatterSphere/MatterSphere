using System;
using System.IO;

namespace FWBS.OMS.Script
{
    public class AssemblyReference : IReference
    {
        public AssemblyReference(string name)
        {
            Name = GetFileNameWithExtension(name);
            AssemblyName = GetFileNameWithoutExtension(name);
        }

        internal static string GetFileNameWithExtension(String name)
        {
            switch (Path.GetExtension(name).ToUpperInvariant())
            {
                case ".DLL":
                case ".EXE":
                    return Path.GetFileName(name);
                default:
                    return Path.GetFileName(String.Format("{0}.{1}", name, "dll"));
            }

        }
        internal static string GetFileNameWithoutExtension(String name)
        {
            switch (Path.GetExtension(name).ToUpperInvariant())
            {
                case ".DLL":
                case ".EXE":
                    return Path.GetFileNameWithoutExtension(name);
                default:
                    return name;
            }            
        }

        public string Name { get; private set; }

        public string AssemblyName { get; private set; }

        public string Location { get; set; }

        public bool IsGlobal{get;set;}

        public bool IsRequired { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

   
}
