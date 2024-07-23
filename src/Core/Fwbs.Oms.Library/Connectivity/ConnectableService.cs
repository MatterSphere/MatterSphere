using System;
using System.Collections.Generic;

namespace FWBS.OMS.Connectivity
{
    public abstract class ConnectableService : IConnectableService, IDisposable
    {
        #region Fields

        private bool ispolling = false;
        private bool connected = false;
        private TestConnectivityDelegate testMethod = null;
        private System.Threading.Timer timer;
        private string name = Guid.NewGuid().ToString();
        private Guid[] dependencies = new Guid[0];
        private bool forced = false;
        private bool available = false;
        private bool testing = false;
        private Guid id;
        private object lockobj = new object();
        private List<string> messages = new List<string>(5);
        private List<Exception> errors = new List<Exception>(5);
        private bool enabled = true;

        #endregion

        #region Constructors

        protected ConnectableService()
        {
            id = GetType().GUID;
            CheckEnability();
        }

        protected ConnectableService(string name, bool generateId)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.name = name;

            if (generateId)
                id = Guid.NewGuid();
            else
                id = GetType().GUID;

            CheckEnability();
        }

        public ConnectableService(string name, TestConnectivityDelegate method, Guid[] dependencies, bool generateId)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (method == null)
                throw new ArgumentNullException("method");

            if (dependencies == null)
                dependencies = new Guid[0];

            this.dependencies = dependencies;
            this.name = name;
            this.testMethod = method;

            if (generateId)
                id = Guid.NewGuid();
            else
                id = GetType().GUID;

            CheckEnability();
        }

        #endregion

        #region IConnectivity Members

        public event EventHandler Connected;

        public event MessageEventHandler Disconnected;

        public Guid Id
        {
            get
            {
                return id;
            }
        }

        public string ServiceName
        {
            get
            {
                return name;
            }
            protected set
            {
                if (!String.IsNullOrEmpty(value))
                    name = value;
            }
        }

        public bool IsPolling
        {
            get
            {
                return ispolling;
            }
        }

        public bool IsConnected
        {
            get 
            {
                if (enabled)
                    return connected;
                else
                {
                    //if disabled, run like the old system would and expect connectivity to be there.
                    return true;
                }
            }
        }

        public bool IsAvailable
        {
            get
            {
                return available;
            }
        }


        protected bool Enabled
        {
            get
            {
                return enabled;
            }
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<string> Messages
        {
            get
            {
                return messages.AsReadOnly();
            }
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<Exception> Errors
        {
            get
            {
                return errors.AsReadOnly();
            }
        }

        public void Disconnect()
        {
            forced = true;
            OnDisconnected(new MessageEventArgs("Disconnection was forced."));
        }

        public void Connect()
        {
            forced = false;
            Test();
        }

        public void Test()
        {
            if (enabled)
            {
                if (testing == false)
                {
                    if (!System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ThreadedTest)))
                        ThreadedTest(null);
                }
            }
        }


        public bool DependsOn(IConnectableService service)
        {
            if (service == null)
                return false;

            if (service.Id == this.Id)
                return false;

            return InternalDependsOn(service);
        }

        public void OnDependentEvent(IConnectableService service, ConnectivityEvent serviceEvent)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            if (service.Id == this.Id)
                throw new ArgumentException("service");

            InternalOnDependentEvent(service, serviceEvent);
        }

        public virtual IConnectableService[] GetServices()
        {
            return null;
        }

        #endregion

        #region Event Raisers

        protected void OnConnected()
        {
            connected = true;
            EventHandler ev = Connected;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
            }
        }

        protected void OnDisconnected(MessageEventArgs e)
        {
            if (messages.Count >= 5)
                messages.RemoveAt(messages.Count - 1);
            if (errors.Count >= 5)
                errors.RemoveAt(errors.Count - 1);

            messages.Insert(0, e.Message);
            errors.Insert(0, e.Exception);

            connected = false;
            MessageEventHandler ev = Disconnected;
            if (ev != null)
            {
                ev(this, e);
            }
        }

        #endregion

        #region Methods

        private void CheckEnability()
        {
            Common.ApplicationSetting reg = new Common.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "", "DisableConnectivityServices", true);
            if (Convert.ToBoolean(reg.GetSetting()) == false)
                enabled = true;
            else
                enabled = false;

        }

        public void SetTestMethod(TestConnectivityDelegate method)
        {
            testMethod = method;
        }

        public void StartPolling()
        {
            if (enabled)
            {
                if (timer == null)
                {
                    timer = new System.Threading.Timer(new System.Threading.TimerCallback(TimerTest), null, 60000, 60000);
                    ispolling = true;
                }
            }
        }

        public void StopPolling()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
                ispolling = false;
            }
        }

        
        private void ThreadedTest(object state)
        {
            bool waspolling = IsPolling;

            try
            {
                StopPolling();

                testing = true;

                lock (lockobj)
                {
                    if (testMethod != null)
                    {
                        testMethod.Invoke(this);
                    }
                    else
                    {
                        InternalTest();
                    }
                }

                available = true;
                if (!connected)
                    OnConnected();
            }
            catch (Exception ex)
            {
                available = false;
                if (connected)
                    OnDisconnected(new MessageEventArgs(ex));
            }
            finally
            {
                testing = false;

                if (waspolling)
                    StartPolling();
            }
        }

        protected virtual bool InternalDependsOn(IConnectableService service)
        {
            return (Array.IndexOf<Guid>(dependencies, service.Id) > -1);
        }

        protected virtual void InternalOnDependentEvent(IConnectableService service, ConnectivityEvent serviceEvent)
        {
            switch (serviceEvent)
            {
                case ConnectivityEvent.Connected:
                    {
                        if (!IsConnected)
                            Connect();
                    }
                    break;
                case ConnectivityEvent.Disconnected:
                    {
                        if (IsConnected)
                            Disconnect();
                    }
                    break;
            }
        }

        protected abstract void InternalTest();

        protected void ThrowTestFailedException(Exception innerException)
        {
            var exceptionMessage = Session.CurrentSession.Resources.GetMessage("MSGTSTFAILSRVC", "Testing failed for the connectable service of ''%1%''", "", this.ToString()).Text;
            if (innerException == null)
                throw new ConnectivityException(exceptionMessage);
            else
                throw new ConnectivityException(exceptionMessage, innerException);
        }

        private void TimerTest(object state)
        {
            if (forced == false && testing == false)
                ThreadedTest(state);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (timer != null)
                {
                    timer.Dispose();
                    timer = null;
                }
            }
        }

        #endregion
    }
}
