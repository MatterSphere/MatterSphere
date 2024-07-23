using System;
using System.Collections.Generic;
using System.Reflection;


namespace FWBS.OMS.Licensing
{
    using Fwbs.Framework.Licensing;
    using Fwbs.Framework.Licensing.API;

    internal sealed class APILicensingManager : ILicensingManager, IDisposable
    {

        #region Fields


        private readonly ConsumerProvider defaultapiprovider = new ConsumerProvider();
        private APILicensingProvider apiprovider;

        #endregion

        #region Constructors

        public APILicensingManager()
        {
            Session.CurrentSession.Disconnected += new EventHandler(CurrentSession_Disconnected);
            Session.CurrentSession.Connecting +=new EventHandler(CurrentSession_Connecting);

            OnStatusChanged();
        }

        #endregion

        #region IAPIProvider

        public bool IsAllowed(Assembly assembly)
        {
            IConsumerInfo consumer;
            return IsAllowed(assembly, out consumer);
        }

        public bool IsAllowed(Assembly assembly, out IConsumerInfo consumer)
        {

            if (assembly == null)
                throw new ArgumentNullException("assembly");

            consumer = null;

            if (UseLocalProvider)
            {
                return defaultapiprovider.IsAllowed(assembly, out consumer);
            }

            return apiprovider.IsAllowed(assembly, out consumer);
        }

        public IConsumerInfo Validate(Assembly assembly)
        {
            if (UseLocalProvider)
            {
                return defaultapiprovider.Validate(assembly);
            }

            return apiprovider.Validate(assembly);
        }

        public IConsumerInfo GetConsumer(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            if (UseLocalProvider)
            {
                return defaultapiprovider.GetConsumer(assembly);
            }

            return apiprovider.GetConsumer(assembly);
        }

        public IEnumerable<IConsumerInfo> GetConsumers()
        {

            if (UseLocalProvider)
            {
                return defaultapiprovider.GetConsumers();
            }

            return apiprovider.GetConsumers();
        }

        #endregion

        #region Properties

        private bool UseLocalProvider
        {
            get
            {
                return apiprovider == null;
            }
        }

        #endregion

       

        #region Captured Events


        private void CurrentSession_Disconnected(object sender, EventArgs e)
        {
            OnStatusChanged();
        }


        private void CurrentSession_Connecting(object sender, EventArgs e)
        {
            OnStatusChanged();
        }

        private void OnStatusChanged()
        {
            if (Session.CurrentSession.IsConnected || Session.CurrentSession.IsConnecting)
            {
                if (apiprovider == null)
                    apiprovider = new APILicensingProvider(defaultapiprovider);
            }
            else
            {
                if (apiprovider != null)
                    apiprovider = null;
            }
        }

        #endregion

        public void Dispose()
        {
            Session.CurrentSession.Disconnected -= new EventHandler(CurrentSession_Disconnected);
            Session.CurrentSession.Connecting -= new EventHandler(CurrentSession_Connecting);
        }
    }
}
