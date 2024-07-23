namespace System.IO
{
    public sealed class OleFileInfo : IDisposable
    {
        #region Fields

        private Fwbs.Documents.DocumentInfo doc;

        #endregion

        #region Constructors

        public OleFileInfo(string file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            doc = new Fwbs.Documents.DocumentInfo(file);
        }

        #endregion

        #region File Opretations

        public void Cancel()
        {
            doc.Cancel();
        }

        public void Save()
        {
            doc.Save();
        }


        #endregion

        #region Property Methods

        public void SetProperty(string name, object value)
        {
            doc.CustomProperties[name].Value = value;
        }

        public object GetProperty(string name)
        {
            if (HasProperty(name))
                return doc.CustomProperties[name].Value;
            else
                return null;
        }

        public bool HasProperty(string name)
        {
            return doc.CustomProperties.Contains(name);
        }

        public void RemoveProperty(string name)
        {
            doc.CustomProperties.Delete(name);
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            doc.Dispose();
        }

        #endregion
    }

    public static class OleFile
    {
        public static void SetProperty(string file, string name, object value)
        {
            using (OleFileInfo ole = new OleFileInfo(file))
            {
                ole.SetProperty(name, value);
                ole.Save();
            }
        }

        public static object GetProperty(string file, string name, object value)
        {
            using (OleFileInfo ole = new OleFileInfo(file))
            {
                return ole.GetProperty(name);
            }
        }

        public static bool HasProperty(string file, string name, object value)
        {
            using (OleFileInfo ole = new OleFileInfo(file))
            {
                return ole.HasProperty(name);
            }
        }

        public static void RemoveProperty(string file, string name, object value)
        {
            using (OleFileInfo ole = new OleFileInfo(file))
            {
                ole.RemoveProperty(name);
                ole.Save();
            }
        }


    }
}
