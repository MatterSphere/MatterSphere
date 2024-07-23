using System;
using System.Collections;

namespace FWBS.OMS.Extensibility
{
	public interface OMSAddin
	{
		Guid RegisteredId{get;}
		string Name{get;}
		string Description{get;}
		Exception[] Errors{get;}
		LoadBehaviour LoadBehaviour{get;}
		AddinStatus Status {get;}
		OMSExtensibility Instance{get;}

	}

	internal class AddinConfiguration: CommonObject, OMSAddin
	{
		#region Fields

		private Type[] registeredTypes;
		readonly private ArrayList errors = new ArrayList();
		private AddinStatus status = AddinStatus.Unloaded;
		private OMSExtensibility instance = null;
		private CodeLookupDisplay clu = null;
		
		#endregion

		#region Constructors

		internal AddinConfiguration()
		{
		}

		#endregion

		#region CommonObject

		protected override void Fetch(object id)
		{
			base.Fetch (id);
			errors.Clear();
			status = AddinStatus.Unloaded;
		}

		internal void Fetch(Guid id)
		{
			base.Fetch (id);

		}

		protected override string DefaultForm
		{
			get
			{
				return String.Empty;
			}
		}

		public override object Parent
		{
			get
			{
				return null;
			}
		}

		public override string FieldPrimaryKey
		{
			get
			{
				return "id";
			}
		}


		protected override string PrimaryTableName
		{
			get
			{
				return "dbextensibility";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "select * from dbextensibility";
			}
		}


		#endregion

		#region Properties

	
		public string Code
		{
			get
			{
				return Convert.ToString(GetExtraInfo("code"));
			}
		}

		internal string TypeName
		{
			get
			{
				return Convert.ToString(GetExtraInfo("type"));
			}
		}

		internal Type Type
		{
			get
			{
                return Session.CurrentSession.TypeManager.Load(TypeName);
			}
		}


		#endregion

		#region OMSAddin

		public Guid RegisteredId
		{
			get
			{
				return (Guid)GetExtraInfo(FieldPrimaryKey);
			}
		}

		public LoadBehaviour LoadBehaviour
		{
			get
			{
				return (LoadBehaviour)Common.ConvertDef.ToEnum(GetExtraInfo("behaviour"), LoadBehaviour.Inactive);
			}
		}

		public string Name
		{
			get
			{
				if (clu == null)
				{
					clu = new CodeLookupDisplay("OMSADDIN");
					clu.Code = Code;
				}
				return Session.CurrentSession.Terminology.Parse(clu.Description, true);
			}
		}

		public string Description
		{
			get
			{
				if (clu == null)
				{
					clu = new CodeLookupDisplay("OMSADDIN");
					clu.Code = Code;
				}
				return Session.CurrentSession.Terminology.Parse(clu.Help, true);
			}
		}

		public AddinStatus Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value;
			}
		}

		public Exception [] Errors
		{
			get
			{
				Exception[] err = new Exception[errors.Count];
				errors.CopyTo(err, 0);
				return err;
			}
		}

		public OMSExtensibility Instance
		{
			get
			{
				if (instance == null)
				{
					if (LoadBehaviour == LoadBehaviour.Startup || LoadBehaviour == LoadBehaviour.OnDemand)
						Load();
				}
				return instance;
			}
		}

		#endregion

		#region Methods

		internal void Load()
		{
			try
			{
				switch (LoadBehaviour)
				{
					case LoadBehaviour.Inactive:
                        throw new Exception(Session.CurrentSession.Resources.GetMessage("ADDCNTBELDINDS", "Addin cannot be loaded when it is inactive/disabled", "").Text);
					default:
						break;
				}
				instance = CreateInstance();
				instance.OnConnection();
				instance.OnStartupComplete(out registeredTypes);
				status = AddinStatus.Loaded;
			}
			catch(Exception ex)
			{
				errors.Add(ex);
				status = AddinStatus.Errors;
			}
		}

		internal void Unload()
		{
			try
			{
				if (instance != null)
				{
					instance.OnBeforeShutdown();
					instance.OnDisconnection();
				}
				status = AddinStatus.Unloaded;
			}
			catch(Exception ex)
			{
				errors.Add(ex);
				status = AddinStatus.Errors;
			}
		}

		private OMSExtensibility CreateInstance()
		{
            bool designMode = Session.CurrentSession._designMode;

			try
			{
				Session.CurrentSession.ValidateAPIClient(Type.Assembly);
			}
			finally
			{
				Session.CurrentSession._designMode = designMode;
			}

			return (OMSExtensibility)Type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
		}

		internal object GetObjectExtender(object obj)
		{
			if (registeredTypes == null || obj == null || instance == null)
				return null;

            Type t = obj as Type;
            if (t == null)
            {
                t = obj.GetType();

                if (Array.IndexOf<Type>(registeredTypes, t) > -1)
                {
                    return instance.GetObjectExtender(obj);
                }
            }
            else
            {
                if (Array.IndexOf<Type>(registeredTypes, t) > -1)
                {
                    return instance.GetObjectExtender(t);
                }
            }

			return null;
		}

		internal void OnObjectEvent(ObjectEventArgs e)
		{
			if (registeredTypes == null || e == null || e.Sender == null || instance == null)
				return;
			
			bool cancel = e.Cancel;

            if (Array.IndexOf<Type>(registeredTypes, e.Sender.GetType()) > -1)
            {
                instance.OnObjectEvent(e);

                if (e.Arguments is System.ComponentModel.CancelEventArgs)
                {
                    System.ComponentModel.CancelEventArgs cea = (System.ComponentModel.CancelEventArgs)e.Arguments;
                    if (cea.Cancel)
                        e.Cancel = true;
                }
                cancel = e.Cancel;
            }

			e.Cancel = cancel;
		}

		public override string ToString()
		{
			return String.Format("{0} ({1})", Name, Status);
		}


		#endregion


	}

}
