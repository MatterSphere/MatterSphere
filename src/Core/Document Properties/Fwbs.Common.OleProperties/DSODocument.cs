using System;

namespace Fwbs.Documents
{
    using System.Runtime.InteropServices;
    using DSOFile;

    public sealed class DSODocument : IRawDocument, ICustomPropertiesDocument, IDisposable
    {
        #region Fields

        private OleDocumentProperties dso;
        private System.IO.FileInfo fi;
        private bool supportsprops = true;

        #endregion

        #region Constructors

        public DSODocument()
        {
        }

        #endregion

        #region ICustomPropertiesDocument Members

        public void ReadCustomProperties(CustomPropertyCollection properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            if (!supportsprops)
                return;

            if (!IsOpen)
                throw new FileClosedException();

            try
            {

                DSOPropertyConverter conv = new DSOPropertyConverter();

                System.Collections.IEnumerator enu = dso.CustomProperties.GetEnumerator();
                while (enu.MoveNext())
                {
                    DSOFile.CustomProperty cp = (DSOFile.CustomProperty)enu.Current;

                    //If name is null then it is a link source property
                    //which we are unable to get a value for.  We will have 
                    //to ignore these values for now.
                    if (cp.Name != null)
                    {
                        try
                        {
                            CustomProperty prop = properties.Add(cp.Name);
                            prop.Value = conv.FromSource(cp.get_Value(), cp.Type);
                        }
                        catch (ArgumentException)
                        {
                            //Not a valid property name.
                        }
                    }
                }

                properties.Accept();
            }
            catch (Exception ex)
            {
                supportsprops = false;
                HandleCustomPropertiesError(ex);
            }

        }

        public void WriteCustomProperties(CustomPropertyCollection properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            if (!supportsprops)
                return;

            if (!IsOpen)
                throw new FileClosedException();

            DSOPropertyConverter conv = new DSOPropertyConverter();

            try
            {
                foreach (CustomProperty prop in properties)
                {
                    if (prop.IsDeleted)
                    {
                        try
                        {
                            DSOFile.CustomProperty cp = dso.CustomProperties[prop.Name];
                            object val = conv.FromSource(cp.get_Value(), cp.Type);
                            dso.CustomProperties[prop.Name].Remove();
                        }
                        catch (COMException comex)
                        {
                            HandleCustomPropertiesError(comex);
                        }
                    }
                    else if (prop.HasChanged)
                    {
                        try
                        {
                            DSOFile.CustomProperty cp = dso.CustomProperties[prop.Name];
                            object val = conv.FromSource(cp.get_Value(), cp.Type);
                            if (!val.Equals(prop.Value))
                            {
                                val = conv.ToSource(prop.Value);
                                cp.set_Value(ref val);
                            }
                        }
                        catch (COMException comex)
                        {
                            //Does not exist.
                            if (comex.ErrorCode == -2147217143)
                            {
                                object val = conv.ToSource(prop.Value);
                                try
                                {
                                    dso.CustomProperties.Add(prop.Name, ref val);
                                }
                                catch (COMException comex2) 
                                {
                                    HandleCustomPropertiesError(comex2);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                supportsprops = false;
                HandleCustomPropertiesError(ex);
            }
        }

        #endregion

        #region IRawDocument Members

        public bool IsOpen
        {
            get
            {
                return dso != null;
            }
        }

        public void Open(System.IO.FileInfo file)
        {
            if (!IsOpen)
            {
                if (file == null)
                    throw new ArgumentNullException("file");

                if (!System.IO.File.Exists(file.FullName))
                    throw new System.IO.FileNotFoundException("", file.FullName);

                dso = DSOFileFactory.Default.CreateOleDocumentProperties();

                this.fi = file;

                try
                {
                    dso.Open(file.FullName, false, dsoFileOpenOptions.dsoOptionOpenReadOnlyIfNoWriteAccess);
                }               
                catch (Exception ex)
                {
                    supportsprops = false;
                    HandleCustomPropertiesError(ex);
                    return;
                }
            }

        }

        private void HandleCustomPropertiesError(Exception ex)
        {                       
            string filename = fi == null ? string.Empty : fi.FullName;
            string message = ex == null ? string.Empty : ex.Message;

            System.Diagnostics.Debug.WriteLine(string.Format("There was an error reading or writing to the custom properties for the document {0}. The user may have to manually select the client and matter. Some errors are normal please only report the error if unexpected behaviour occurs. The error returned from the DSODocument is: {1}", filename, message), "DocumentPropertyExtractionError");
        }


        public void Save()
        {
            if (!supportsprops)
                return;

            if (!IsOpen)
                throw new FileClosedException();

            try
            {
                dso.Save();
            }
            catch (COMException comex)
            {
                Exception wrappedException = null;
                switch (comex.ErrorCode)
                {
                    //Read only exception
                    case -2147217147:
                        wrappedException = new System.IO.FileLoadException("The file cannot be opened or saved as it is read only.", fi.FullName, comex);
                        break;
                    case -2147287007:
                        wrappedException = new System.IO.FileLoadException("The file cannot be saved or opened as it is open by another application.", fi.FullName, comex);
                        break;
                    default:
                        wrappedException = new System.IO.FileLoadException(comex.Message, fi.FullName, comex);
                        break;
                }

                HandleCustomPropertiesError(wrappedException);

            }
            catch (Exception ex)
            {
                HandleCustomPropertiesError(ex);
            }
        }

        public void Close()
        {

            if (dso != null)
            {
                try
                {
                    dso.Close(false);
                }
                catch (COMException comex)
                {
                    HandleCustomPropertiesError(comex);
                }

                Marshal.FinalReleaseComObject(dso);

                dso = null;
            }

        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
