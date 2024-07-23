using System;

namespace FWBS.OMS.Extensibility
{
	public abstract class OMSExtensibility : IDisposable
	{
		#region Methods


		public virtual void OnConnection ()
		{
		}

		public virtual void OnStartupComplete(out Type[] registeredTypes)
		{
			registeredTypes = null;
		}

		public virtual void OnBeforeShutdown()
		{
		}

		public virtual void OnDisconnection()
		{
		}

		public virtual void OnObjectEvent(ObjectEventArgs e)
		{
		}

        public virtual object GetObjectExtender(Type type)
        {
            return null;
        }

		public virtual object GetObjectExtender(object obj)
		{
			return null;
		}

		#endregion

		#region Poperties

		public Session CurrentSession
		{
			get
			{
				return Session.OMS;
			}
		}

		#endregion

		#region IDisposable Members

		protected virtual void Dispose(bool disposing) 
		{
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}


		#endregion
	}
}
