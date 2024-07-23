using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fwbs.Documents
{
    using Fwbs.Framework.ComponentModel.Composition;

    public sealed class DocumentInfo : IDisposable
    {
        #region Static

        private static Fwbs.Framework.Session session = new Framework.Session();

        static DocumentInfo()
        {
           SetSession(null);
        }

        private static void session_Initialize(object sender, Framework.SessionInitializeEventArgs e)
        {
            AddCatalogs(e,null);
        }

        public static void AddCatalogs(Framework.SessionInitializeEventArgs e, string baseApplicationLocation)
        {
            if (string.IsNullOrWhiteSpace(baseApplicationLocation))
                baseApplicationLocation = AppDomain.CurrentDomain.BaseDirectory;
            

            e.Catalogs.Add(new DirectoryCatalog(baseApplicationLocation, "Fwbs.Documents.*.dll", e.Session));
            e.Catalogs.Add(new DirectoryCatalog(System.IO.Path.Combine(baseApplicationLocation, "Modules"), e.Session));
        }

        public static void SetSession(Framework.Session newSession)
        {
            if (newSession == null)
            {
                session = new Framework.Session();

                session.Initialize += new EventHandler<Framework.SessionInitializeEventArgs>(session_Initialize);
            }
            else
            {
                session = newSession;
            }
        }

        #endregion

        #region Fields

        private readonly FileInfo file;
        private readonly CustomPropertyCollection customprops;
        private IRawDocument doc;
        

        #endregion

        #region Constructors

        public DocumentInfo(string file)
        {
            if (String.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");

            this.file = new FileInfo(file);
            this.customprops = new CustomPropertyCollection();
            this.doc = Resolve();

            this.Open();
        }

        public DocumentInfo(FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            
            this.file = file;
            this.customprops = new CustomPropertyCollection();
            this.doc = Resolve();

            this.Open();
        }

        #endregion

        #region Imports

        internal IEnumerable<IDocPropHandler> Handlers { get; set; }

        #endregion

        #region Properties

        public string FullName
        {
            get
            {
                return file.FullName;
            }
        }

        public string Name
        {
            get
            {
                return file.Name;
            }
        }

        public CustomPropertyCollection CustomProperties
        {
            get
            {
                return customprops;
            }
        }

        #endregion

        #region Methods

        public static bool IsZipFile(string file)
        {
            return IsZipFile(new System.IO.FileInfo(file));
        }

        public static bool IsZipFile(System.IO.FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            using (FileStream fs = file.OpenRead())
            {
                byte[] header = new byte[4];
                if (fs.Read(header, 0, 4) == 4)
                    return BitConverter.ToInt32(header, 0) == 67324752;
            }

            return false;
        }

        private IRawDocument Resolve()
        {
            Handlers = session.GetService<IContainer>().ResolveAll<IDocPropHandler>().ToArray();

            if (Handlers != null)
            {
                foreach (IDocPropHandler dph in Handlers)
                {
                    if (dph.Handles(file))
                    {
                        IRawDocument doc = dph.CreateDocument(file);
                        if (doc != null)
                            return doc;
                    }
                }
            }

            throw new NotSupportedException(String.Format("Property handler not supported or properly installed for file '{0}'", file.FullName));

        }

        public void Open()
        {
            if (!File.Exists(file.FullName))
                throw new FileNotFoundException("File not found", file.FullName);

            try
            {
                doc.Open(file);

                ICustomPropertiesDocument cust = doc as ICustomPropertiesDocument;
                if (cust != null)
                {
                    CustomProperties.Clear();
                    cust.ReadCustomProperties(CustomProperties);
                }

            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                if (!(ex.InnerException is UnauthorizedAccessException))
                {
                    throw;
                }
            }
            finally
            {
                Close();
            }

        }

        public void Refresh()
        {
            Close();
            Open();
        }


        public void Cancel()
        {
            CustomProperties.Cancel();
        }

        public void Save()
        {
            if (CustomProperties.HasChanged)
            {
                try
                {
                  
                    doc.Open(file);

                    ICustomPropertiesDocument cust = doc as ICustomPropertiesDocument;
                    if (cust != null)
                    {
                        cust.WriteCustomProperties(CustomProperties);
                    }
                    doc.Save();

                    CustomProperties.Accept();

                }
                catch (IOException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);

                    if (!(e.InnerException is UnauthorizedAccessException))
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    throw new FileLoadException(ex.Message, file.FullName, ex);
                }
                finally
                {
                    Close();
                }

            }
        }

        public bool Exists
        {
            get
            {
                file.Refresh();
                return file.Exists;
            }
        }

        public void Close()
        {
            Cancel();
            doc.Close();
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
