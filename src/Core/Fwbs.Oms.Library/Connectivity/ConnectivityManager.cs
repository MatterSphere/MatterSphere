using System;
using System.Collections.Generic;

namespace FWBS.OMS.Connectivity
{
    public sealed class ConnectivityManager : IDisposable, IService
    {
        #region Fields

        public static readonly Guid NETWORK_SERVICE = typeof(NetworkService).GUID;
        public static readonly Guid OMS_SERVICE = typeof(OMSService).GUID;
        public static readonly Guid BLOB_STORAGE_SERVICE = typeof(DocumentManagement.Storage.Providers.BlobStorageProviderService).GUID;
        public static readonly Guid FS_STORAGE_SERVICE = typeof(DocumentManagement.Storage.Providers.FileSystemStorageProviderService).GUID;
        public static readonly Guid DATABASE_SERVICE = typeof(DatabaseService).GUID;

        #endregion

        #region Fields


        private Dictionary<Guid, IConnectableService> services = null;
        private static ConnectivityManager currentManager = null;
        private bool loaded = false;

        #endregion

        #region Events

        public event EventHandler Connected;
        public event MessageEventHandler Disconnected;

        #endregion

        #region Constructors

        private ConnectivityManager()
        {
        }

        #endregion

        #region Properties

        public static ConnectivityManager CurrentManager
        {
            get
            {
                if (currentManager == null)
                {
                    currentManager = new ConnectivityManager();
                }
                return currentManager;
            }
        }

        #endregion

        #region Methods

        public T[] GetServices<T>() where T : IConnectableService
        {
            List<T> list = new List<T>();
            foreach (IConnectableService service in services.Values)
            {
                if (service.GetType() == typeof(T))
                    list.Add((T)service);
            }

            return list.ToArray();
        }

        public IConnectableService[] GetDependentServices(IConnectableService service)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            List<IConnectableService> list = new List<IConnectableService>();
            foreach (IConnectableService svc in services.Values)
            {
                if (svc.DependsOn(service))
                    list.Add(svc);
            }

            return list.ToArray();

        }

        public void Add(IConnectableService service)
        {
            if (service == null)
                throw new ArgumentNullException("service");


            IConnectableService[] children = service.GetServices();

            if (children != null && children.Length > 0)
            {
                foreach (IConnectableService s in children)
                {
                    if (s == null) continue;
                    Add(s);
                }
                return;
            }

            if (!services.ContainsKey(service.Id))
            {
                service.Connected -= new EventHandler(services_Connected);
                service.Disconnected -= new MessageEventHandler(services_Disconnected);

                services.Add(service.Id, service);
                
                service.Test();

                service.Connected += new EventHandler(services_Connected);
                service.Disconnected += new MessageEventHandler(services_Disconnected);
            }

        }

        public IConnectableService[] GetServices()
        {
            IConnectableService[] ss = new IConnectableService[services.Count];
            services.Values.CopyTo(ss, 0);
            return ss;
        }

        #endregion

        #region Captured Events

        private void services_Disconnected(object sender, MessageEventArgs e)
        {
            try
            {
                MessageEventHandler ev = Disconnected;
                if (ev != null)
                {
                    ev(sender, e);
                }

            }
            finally
            {
                IConnectableService service = (IConnectableService)sender;
                IConnectableService [] deps = GetDependentServices(service);
                foreach (IConnectableService dep in deps)
                {
                    dep.OnDependentEvent(service, ConnectivityEvent.Disconnected);
                }
            }
        }

        private void services_Connected(object sender, EventArgs e)
        {
            try
            {
                EventHandler ev = Connected;
                if (ev != null)
                {
                    ev(sender, e);
                }
            }
            finally
            {
                IConnectableService service = (IConnectableService)sender;
                IConnectableService[] deps = GetDependentServices(service);
                foreach (IConnectableService dep in deps)
                {
                    dep.OnDependentEvent(service, ConnectivityEvent.Connected);
                }
            }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unload();
            }

            loaded = false;
        }

        #endregion

        #region IService

        public bool IsLoaded
        {
            get
            {
                return loaded;
            }
        }

        public void Load()
        {
            if (!loaded)
            {
                if (services == null)
                    services = new Dictionary<Guid, IConnectableService>();

                services.Clear();
                NetworkService nws = new NetworkService();
                OMSService dbs = new OMSService();
                Add(nws);
                Add(dbs);

            }

            loaded = true;

        }

        /// <summary>
        /// Clears any cached information.
        /// </summary>
        public void Unload()
        {
            try
            {
                if (loaded)
                {
                    if (services != null)
                    {
                        foreach (IConnectableService srv in services.Values)
                        {
                            srv.Connected -= new EventHandler(services_Connected);
                            srv.Disconnected -= new MessageEventHandler(services_Disconnected);


                            IDisposable disp = srv as IDisposable;
                            if (disp != null)
                                disp.Dispose();
                        }
                        services.Clear();
                    }

                    currentManager = null;

                }
            }
            finally
            {
                loaded = false;
            }
        }

        #endregion


    }
}
