using System;
using System.Collections.Generic;

namespace FWBS.OMS.Apps
{
    public sealed class ApplicationManager : IService, IDisposable
    {
        #region Fields

        private static ApplicationManager currentManager;

        private Dictionary<short, RegisteredApplication> registeredAppsById = new Dictionary<short,RegisteredApplication>();
        private Dictionary<Guid, RegisteredApplication> registeredAppsByGuid = new Dictionary<Guid, RegisteredApplication>();
        private Dictionary<string, RegisteredApplication> registeredAppsByName = new Dictionary<string, RegisteredApplication>();
        private System.Collections.Generic.Dictionary<string, Interfaces.IOMSApp> appsByName = new System.Collections.Generic.Dictionary<string, Interfaces.IOMSApp>();

        public System.Collections.Generic.Dictionary<string, Interfaces.IOMSApp> AppsByName
        {
            get
            {
                return appsByName;
            }
        }
        
        private bool loaded = false;

        private Interfaces.IOMSApp current;

        #endregion

        #region Constructors

        private ApplicationManager()
        {
        }


        #endregion

        #region Methods
       
        public void InitialiseInstance(string code, Interfaces.IOMSApp application)
        {
            if (String.IsNullOrEmpty(code))
                throw new ArgumentNullException("code");

            if (application != null && appsByName.Count == 0)
                current = application;

            if (appsByName.ContainsKey(code) == false)
            {
                if (application == null)
                    throw new ArgumentNullException("application");

                appsByName.Add(code, application);
            }
            else
            {
                if (application == null)
                    appsByName.Remove(code);
                else
                    appsByName[code] = application;
            }
            
        }

        public Interfaces.IOMSApp GetApplicationInstance(RegisteredApplication registeredApp, bool create)
        {
            Session.CurrentSession.CheckLoggedIn();

            if (registeredApp == null)
                throw new ArgumentNullException("registeredApp");

            if (appsByName.ContainsKey(registeredApp.Code))
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                return appsByName[registeredApp.Code];
            }
            else if (create)
            {
                Interfaces.IOMSApp app = registeredApp.CreateOMSApp();
                appsByName.Add(registeredApp.Code, app);
                return app;
            }
            else
            {
                return null;
            }
        }


        public Interfaces.IOMSApp GetApplicationInstance(short id, bool create)
        {
            RegisteredApplication reg = GetRegisteredApplication(id);
            return GetApplicationInstance(reg, create);
        }

        public Interfaces.IOMSApp GetApplicationInstance(Guid id, bool create)
        {
            RegisteredApplication reg = GetRegisteredApplication(id);
            return GetApplicationInstance(reg, create);
        }

        public Interfaces.IOMSApp GetApplicationInstance(string code, bool create)
        {
            if (String.IsNullOrEmpty(code))
                throw new ArgumentNullException("code");

            RegisteredApplication reg = GetRegisteredApplication(code);
            return GetApplicationInstance(reg, create);

        }

        public System.Data.DataTable GetAvailableRegisteredApplications()
        {
            Session.CurrentSession.CheckLoggedIn();

            return Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbapplication", "APPLICATIONS", null);
        }

        public RegisteredApplication GetRegisteredApplication(short id)
        {
            Session.CurrentSession.CheckLoggedIn();

            if (registeredAppsById.ContainsKey(id) == false)
                throw new RegisteredApplicationException("ERRAPPMISSING", "The registered application '%1%' is not registered.", null, id.ToString());
            else
                return registeredAppsById[id];

        }

        public RegisteredApplication GetRegisteredApplication(Guid id)
        {
            Session.CurrentSession.CheckLoggedIn();

            if (registeredAppsByGuid.ContainsKey(id) == false)
                throw new RegisteredApplicationException("ERRAPPMISSING", "The registered application '%1%' is not registered.", null, id.ToString());
            else
                return registeredAppsByGuid[id];

        }

        public RegisteredApplication GetRegisteredApplication(string code)
        {
            Session.CurrentSession.CheckLoggedIn();

            if (String.IsNullOrEmpty(code))
                throw new ArgumentNullException("code");

            if (registeredAppsByName.ContainsKey(code) == false)
                throw new RegisteredApplicationException("ERRAPPMISSING", "The registered application '%1%' is not registered.", null, code);
            else
                return registeredAppsByName[code];

        }

        public RegisteredApplication GetRegisteredApplicationByExtension(string extension)
        {
            Session.CurrentSession.CheckLoggedIn();

            if (String.IsNullOrEmpty(extension))
                return null;

            foreach (RegisteredApplication reg in registeredAppsById.Values)
            {
                if (reg.Handles(extension))
                    return reg;
            }

            return null;
        }


        #endregion

        #region Properties

        public static ApplicationManager CurrentManager
        {
            get
            {
                if (currentManager == null)
                {
                    currentManager = new ApplicationManager();
                }
                return currentManager;
            }
        }

        public IEnumerable<FWBS.OMS.Interfaces.IOMSApp> LoadedApps
        {
            get
            {
                foreach (FWBS.OMS.Interfaces.IOMSApp app in appsByName.Values)
                    yield return app;
            }
        }

        public FWBS.OMS.Interfaces.IOMSApp CurrentApplication
        {
            get
            {
                string name;

                var app = GetCurrentApplication(out name);

                if (app != null)
                    return app;

                throw new Apps.RegisteredApplicationException("ERRCURRENTAPP", "Cannot determine the current OMS application for process %1%.", null, name);
            }
        }

        private FWBS.OMS.Interfaces.IOMSApp GetCurrentApplication(out string name)
        {
            var proc = System.Diagnostics.Process.GetCurrentProcess();

            name = proc.ProcessName.ToUpperInvariant();

            foreach (var reg in registeredAppsByName.Values)
            {
                if (System.IO.Path.GetFileNameWithoutExtension(reg.Path).ToUpperInvariant() == name)
                {
                    FWBS.OMS.Interfaces.IOMSApp app;
                    if (appsByName.TryGetValue(reg.Code, out app))
                        return app;
                }
            }

            return null;
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
            if (!loaded && Session.CurrentSession.IsLoggedIn)
            {
                Session.CurrentSession.CheckLoggedIn();

                System.Data.DataTable dt = GetAvailableRegisteredApplications();

                if (registeredAppsById == null)
                    registeredAppsById  = new Dictionary<short, RegisteredApplication>();
                else
                    registeredAppsById.Clear();

                if (registeredAppsByGuid == null)
                    registeredAppsByGuid = new Dictionary<Guid, RegisteredApplication>();
                else
                    registeredAppsByGuid.Clear();

                if (registeredAppsByName == null)
                    registeredAppsByName = new Dictionary<string, RegisteredApplication>();
                else
                    registeredAppsByName.Clear();
                

                foreach (System.Data.DataRow r in dt.Rows)
                {
                    RegisteredApplication regapp = new RegisteredApplication(r);
                    registeredAppsById.Add(Convert.ToInt16(r["appid"]), regapp);
                    registeredAppsByGuid.Add((Guid)r["appguid"], regapp);
                    registeredAppsByName.Add(Convert.ToString(r["appcode"]), regapp);
                }

                loaded = true;
            }

           
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
                    if (appsByName != null)
                    {
                        string name;

                        var current = GetCurrentApplication(out name);

                        foreach (var item in appsByName.Values)
                        {
                            if (item == current)
                                continue;

                            var disp = item as IDisposable;
                            if (disp != null)
                                disp.Dispose();
                        }

                        appsByName.Clear();
                    }

                    this.current = null;

                    if (registeredAppsById != null)
                        registeredAppsById.Clear();

                    if (registeredAppsByGuid != null)
                        registeredAppsByGuid.Clear();

                    if (registeredAppsByName != null)
                        registeredAppsByName.Clear();


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
