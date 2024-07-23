using System;
using System.Collections;
using System.Data;

namespace FWBS.OMS.Extensibility
{
	public class AddinManager : IEnumerable, IService
	{
		#region Fields

		private readonly Hashtable addins = new Hashtable();
		private bool loaded = false;

		#endregion

		#region Constructors

		internal AddinManager()
		{
			Session.CurrentSession.CheckLoggedIn();
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

        void IService.Load()
        {
            if (loaded == false)
            {
                addins.Clear();
                AddinConfiguration ac = new AddinConfiguration();
                DataTable dt = ac.GetList();
                foreach (DataRow r in dt.Rows)
                {
                    Guid id = (Guid)r["id"];
                    AddinConfiguration addin = new AddinConfiguration();
                    addin.Fetch(id);
                    LoadBehaviour lb = addin.LoadBehaviour;

                    addins.Add(id, addin);

                    switch (lb)
                    {
                        case LoadBehaviour.Inactive:
                            break;
                        case LoadBehaviour.OnDemand:
                            break;
                        case LoadBehaviour.Startup:
                            addin.Load();
                            break;
                    }
                }
            }

            loaded = true;

        }

        void IService.Unload()
		{
			try
			{
				if (loaded)
				{
					foreach (AddinConfiguration addin in addins.Values)
					{
						addin.Unload();
					}
				}
			}
			finally
			{
				loaded = false;
			}
		}

        #endregion

        #region Methods

        internal void OnObjectEvent(ObjectEventArgs e)
		{
			foreach (AddinConfiguration addin in addins.Values)
			{
				addin.OnObjectEvent(e);
			}
		}

		internal object[] GetObjectExtenders(object obj)
		{
			ArrayList extenders = new ArrayList();
			foreach (AddinConfiguration addin in addins.Values)
			{
				object o = addin.GetObjectExtender(obj);
				if (o != null)
					extenders.Add(o);
			}

			object[] exts = new object[extenders.Count];
			extenders.CopyTo(exts, 0);
			return exts;
		}

		#endregion

		#region Indexers

		public OMSAddin this[Guid registeredId]
		{
			get
			{
				return (OMSAddin)addins[registeredId];
			}
		}

		public OMSAddin this [int index]
		{
			get
			{
				IEnumerator enu = addins.GetEnumerator();
				int ctr = 0;
				while(enu.MoveNext())
				{
					if (ctr == index)
						return (OMSAddin)enu.Current;
					ctr++;
				}
				throw new ArgumentOutOfRangeException("index");
			}
		}

		#endregion

		#region Properties

		public int Count
		{
			get
			{
				return addins.Count;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return addins.Values.GetEnumerator();
		}

		#endregion
	}
}
