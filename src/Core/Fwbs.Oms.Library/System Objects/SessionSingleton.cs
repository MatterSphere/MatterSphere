using System;
using System.Collections.Generic;

namespace FWBS.OMS
{
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Xml;

    internal sealed class SessionSingleton : IDisposable
    {
        public sealed class XmlDomWrapper : IDisposable
        {
            private bool watcherenabled = false;
            private XmlDocument doc = null;
            private SessionSingleton session;
            private string originalhash = String.Empty;

            public XmlDomWrapper(SessionSingleton session)
            {
                this.session = session;

                if (session.IsAttached)
                {
                    watcherenabled = session.watcher.EnableRaisingEvents;

                    session.watcher.EnableRaisingEvents = false;
                    session.file.Position = 0;

                    session.Lock();

                    doc = new XmlDocument();

                    try
                    {
                        doc.Load(session.file);
                        originalhash = GetHash(doc.OuterXml);
                    }
                    catch (XmlException)
                    {
                        originalhash = String.Empty;
                    }
                }
            }

            public XmlDocument Document
            {
                get
                {
                    return doc;
                }
            }

            public bool HasChanged
            {
                get
                {
                    if (doc == null)
                        return false;
                    else
                        return (originalhash != GetHash(doc.OuterXml));

                }
            }

            public void Save()
            {
                if (HasChanged)
                {
                    session.file.SetLength(0);
                    byte[] data = UnicodeEncoding.UTF8.GetBytes(doc.OuterXml);
                    session.file.Write(data, 0, data.Length);
                    session.file.Flush();
                    originalhash = GetHash(doc.OuterXml);
                }
            }

            private string GetHash(string xml)
            {
                using (System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
                {
                    byte[] hash;

                    using (MemoryStream mem = new MemoryStream(UnicodeEncoding.UTF8.GetBytes(xml)))
                    {
                        hash = sha1.ComputeHash(mem);
                    }
                    return BitConverter.ToString(hash);
                }
            }

            #region IDisposable Members

            public void Dispose()
            {
                session.Unlock();

                session.watcher.EnableRaisingEvents = watcherenabled;
            }

            #endregion
        }

        #region Fields

        private FileStream file = null;
        private bool islocked = true;
        private FileSystemWatcher watcher = new FileSystemWatcher();
        private const int LOCK_SIZE = 1000000;


        #endregion

        #region Events

        public event EventHandler DisconnectRequest;
        private void OnDisconnectRequest()
        {
            if (DisconnectRequest != null)
                DisconnectRequest(this, EventArgs.Empty);
        }


        #endregion

        #region Connection Methods

        public void Connect()
        {
            try
            {
                if (!IsAttached)
                {
                    InternalAttach();
                    AttachProcess();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

      
        private void InternalAttach()
        {
            if (IsAttached)
                InternalDisconnect();

            if (FileLocation.Directory.Exists == false)
                FileLocation.Directory.Create();

            file = FileLocation.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            watcher.EnableRaisingEvents = false;
            watcher.Changed -= new FileSystemEventHandler(watcher_Changed);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Path = FileLocation.Directory.FullName;
            watcher.Filter = FileLocation.Name;
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            watcher.EnableRaisingEvents = true;
        }


        public void Disconnect()
        {
            Disconnect(false);
        }


        public void Disconnect(bool global)
        {
            if (IsAttached)
            {
                XmlDomWrapper dom = new XmlDomWrapper(this);

                try
                {
                    if (global)
                    {
                        XmlElement root = (XmlElement)dom.Document.SelectSingleNode(String.Format(@"/Session"));
                        root.SetAttribute("GlobalDisconnect", global.ToString());
                    }

                    XmlElement el = (XmlElement)dom.Document.SelectSingleNode(String.Format(@"/Session/Clients/Client[@id={0}]", ProcessId));
                    if (el != null)
                    {
                        el.ParentNode.RemoveChild(el);
                    }

                    dom.Save();

                }
                finally
                {
                    dom.Dispose();
                }

            }


            InternalDisconnect();
        }

        private void InternalDisconnect()
        {
            watcher.EnableRaisingEvents = false;
            watcher.Changed -= new FileSystemEventHandler(watcher_Changed);

            if (IsAttached)
            {
                file.Flush();
                file.Close();

                try
                {
                    File.Delete(file.Name);
                }
                catch (IOException)
                {
                }

                file = null;
            }
        }

        #endregion

        #region Methods

        public string GetProperty(string name, string defaultValue)
        {
            if (IsAttached)
            {
                XmlDomWrapper dom = new XmlDomWrapper(this);

                try
                {
                    XmlAttribute attr = (XmlAttribute)dom.Document.SelectSingleNode(String.Format(@"/Session/@{0}", XmlConvert.EncodeName(name)));
                    if (attr == null)
                        return defaultValue;
                    else
                        return attr.Value;

                }
                finally
                {
                    dom.Dispose();
                }

            }
            return defaultValue;
        }

        public void SetProperties(Dictionary<string, string> values)
        {
            if (IsAttached)
            {
                XmlDomWrapper dom = new XmlDomWrapper(this);

                try
                {
                    Dictionary<string, string>.Enumerator enu = values.GetEnumerator();
                    XmlElement root = (XmlElement)dom.Document.SelectSingleNode(String.Format(@"/Session"));

                    while (enu.MoveNext())
                    {
                        string val = enu.Current.Value;
                        if (root.GetAttribute(XmlConvert.EncodeName(enu.Current.Key)) != val)
                            root.SetAttribute(XmlConvert.EncodeName(enu.Current.Key), val);
                    }

                    dom.Save();

                }
                finally
                {
                    dom.Dispose();
                }

            }
        }


        private void Lock()
        {
            if (islocked == false)
            {
                bool locked = false;
                while (locked == false)
                {
                    try
                    {
                        file.Lock(0, LOCK_SIZE);
                        locked = true;
                        islocked = true;
                    }
                    catch (IOException)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }

        private void Unlock()
        {
            try
            {
                if (file != null)
                {
                    file.Unlock(0, LOCK_SIZE);
                    islocked = false;
                }
            }
            catch { }
        }

        private bool GetGlobalDisconnect()
        {
            if (IsAttached)
            {
                try
                {
                    file.Position = 0;

                    if (file.Length == 0)
                        return false;

                    Lock();

                    using (XmlReader r = XmlReader.Create(file))
                    {
                        while (r.Read())
                        {
                            if (r.Name.ToLower() == "session")
                            {
                                if (r.MoveToAttribute("GlobalDisconnect"))
                                {
                                    bool ret;
                                    if (bool.TryParse(r.Value, out ret))
                                        return ret;
                                    break;

                                }
                            }
                        }
                    }
                }
                catch (XmlException)
                {
                    throw;
                }
                finally
                {
                    Unlock();
                }
            }

            return false;
        }


        #endregion

        #region Client

        private void AttachProcess()
        {
            if (IsAttached)
            {
                XmlDomWrapper dom = new XmlDomWrapper(this);

                try
                {

                    XmlElement n_session = (XmlElement)dom.Document.SelectSingleNode(String.Format(@"Session"));
                    if (n_session == null)
                    {
                        n_session = dom.Document.CreateElement("Session");
                        dom.Document.AppendChild(n_session);
                    }

                    XmlElement n_clients = (XmlElement)n_session.SelectSingleNode(String.Format(@"Clients"));
                    if (n_clients == null)
                    {
                        n_clients = dom.Document.CreateElement("Clients");
                        n_session.AppendChild(n_clients);
                    }

                    XmlElement n_client = (XmlElement)n_clients.SelectSingleNode(String.Format(@"Client[@id='{0}']", ProcessId));
                    if (n_client == null)
                    {
                        n_client = dom.Document.CreateElement("Client");
                        n_clients.PrependChild(n_client);
                    }

                    if (n_client.GetAttribute("id") != ProcessId.ToString() || n_client.GetAttribute("name") != ProcessName || n_client.GetAttribute("location") != ProcessFileName)
                    {
                        n_client.SetAttribute("id", ProcessId.ToString());
                        n_client.SetAttribute("name", ProcessName);
                        n_client.SetAttribute("location", ProcessFileName);
                    }

                    TidyUp(dom);

                    dom.Save();
                }
                finally
                {
                    dom.Dispose();
                }

            }
        }

        public ConnectedClientInfo[] GetConnectedClients()
        {
            List<ConnectedClientInfo> list = new List<ConnectedClientInfo>();

            bool attached = IsAttached;

            if (attached == false)
                InternalAttach();

            if (IsAttached)
            {
                XmlDomWrapper dom = new XmlDomWrapper(this);

                try
                {

                    list = TidyUp(dom);

                    dom.Save();

                }
                finally
                {
                    dom.Dispose();
                }

            }

            if (attached == false)
                InternalDisconnect();

            return list.ToArray();
        }

        private List<ConnectedClientInfo> TidyUp(XmlDomWrapper dom)
        {
            List<ConnectedClientInfo> list = new List<ConnectedClientInfo>();

            XmlNodeList nodes = dom.Document.SelectNodes(String.Format(@"/Session/Clients/Client"));

            for (int ctr = nodes.Count - 1; ctr >= 0; ctr--)
            {
                XmlElement el = nodes[ctr] as XmlElement;
                if (el == null)
                    nodes[ctr].ParentNode.RemoveChild(nodes[ctr]);


                int procid = Convert.ToInt32((el.GetAttribute("id") ?? "0"));
                string name = (el.GetAttribute("name") ?? "?");
                string location = (el.GetAttribute("location") ?? "?");


                try
                {
                    Process proc = Process.GetProcessById(procid);
                    if (proc.ProcessName.ToLower() == name.ToLower())
                    {
                        list.Add(new ConnectedClientInfo(proc, location));
                        continue;
                    }
                }
                catch (ArgumentException)
                {
                }

                el.ParentNode.RemoveChild(el);
            }

            return list;
        }

        #endregion

        #region Captured Events

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (IsAttached)
                {
                    try
                    {

                        if (file.Length == 0)
                            return;

                        watcher.EnableRaisingEvents = false;

                        if (GetGlobalDisconnect())
                        {
                            OnDisconnectRequest();
                        }
                    }
                    finally
                    {
                        watcher.EnableRaisingEvents = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        #endregion

        #region Properties

        [System.ComponentModel.Browsable(false)]
        public bool IsLastConnection
        {
            get
            {
                bool can = false;

                System.IO.FileInfo sessionfile = FileLocation;

                if (sessionfile.Exists)
                {

                    string tempfile = System.IO.Path.Combine(sessionfile.Directory.FullName, String.Format("{0}.tmp", Guid.NewGuid().ToString()));

                    try
                    {
                        sessionfile.CopyTo(tempfile, true);
                        InternalDisconnect();

                        try
                        {
                            sessionfile.Delete();
                            System.IO.File.Move(tempfile, sessionfile.FullName);
                            can = true;
                        }
                        catch (IOException)
                        {
                            can = false;
                        }

                        InternalAttach();
                    }
                    finally
                    {
                        if (File.Exists(tempfile))
                        {
                            try
                            {
                                File.Delete(tempfile);
                            }
                            catch { }
                        }
                    }
                }
                else
                    can = true;

                return can;
            }
        }

        public bool IsAttached
        {
            get
            {
                return (file != null);
            }
        }

        private System.IO.FileInfo FileLocation
        {
            get
            {
                Common.Directory dir = Global.GetAppDataPath();
                Common.FilePath filename = dir.ToString() + @"\Session." + Global.SessionExt;
                return new System.IO.FileInfo(filename);
            }
        }

        private int ProcessId
        {
            get
            {
                return Process.GetCurrentProcess().Id;
            }
        }

        private string ProcessName
        {
            get
            {
                return Process.GetCurrentProcess().ProcessName;
            }
        }

        private string ProcessFileName
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.FileName;
            }
        }

        public bool AllowMultiLogin
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, String.Empty, "AllowMultiLogin", false);
                return reg.ToBoolean();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Disconnect(false);

            if (watcher != null)
                watcher.Dispose();
        }

        #endregion
    }

    public sealed class ConnectedClientInfo
    {
        internal ConnectedClientInfo(Process process, string location)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            this.process = process;
            this.location = (location ?? "?");
        }

        public int ProcessId
        {
            get
            {
                return process.Id;
            }
        }

        public string Name
        {
            get
            {
                return process.ProcessName;
            }
        }

        private Process process;
        public Process Process
        {
            get
            {
                return process;
            }
        }

        private string location;
        public FileInfo Location
        {
            get
            {
                return new FileInfo(location);
            }
        }
    }
}

