using System;
using Fwbs.Framework.Interop;

namespace DSOFile
{
    public class DSOFileFactory : COMFactory
    {
        #region Static

        private static readonly DSOFileFactory def = new DSOFileFactory();

        public static DSOFileFactory Default
        {
            get
            {
                return def;
            }
        }

        #endregion

        #region Constants

        public const string Name64 = "dsofile64.dll";
        public const string Name32 = "dsofile.dll";

        #endregion

        #region Constructors

        public DSOFileFactory()
            : base(CreateFile())
        {
        }

        public DSOFileFactory(DllInfo file)
            : base(file)
        {
        }

        private static DllInfo CreateFile()
        {
            return new DllInfo(Name32, Name64);
        }

        #endregion

        #region Factory Methods

        public OleDocumentProperties CreateOleDocumentProperties()
        {
            return (OleDocumentProperties)Create(new Guid("58968145-CF05-4341-995F-2EE093F6ABA3")) ?? new OleDocumentProperties();
        }

        #endregion
    }
}
