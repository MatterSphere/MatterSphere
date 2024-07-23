using System;
using System.Collections.Generic;
using System.Globalization;
using FWBS.OMS.DocumentManagement;

namespace FWBS.OMS
{
    public class Context : IContext
	{
		public Context()
		{
		}

		protected Context(Context context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

            foreach (var v in context.Values)
            {
                this.Values[v.Key] = v.Value;
            }

            if (context.Data != null)
                this.Data = context.Data;
            if (context.Parent != null)
                this.Parent = Parent.Clone();
        }

		protected readonly Dictionary<string, object> Values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

		public Associate Associate { get {return Get<Associate>();} set {Set<Associate>(value);}}

		public Client Client { get { return Get<Client>(); } set { Set<Client>(value); } }

		public OMSFile File { get { return Get<OMSFile>(); } set { Set<OMSFile>(value); } }

		public Session Session{ get {return Get<Session>();} set {Set<Session>(value);}}

		public User User { get { return Get<User>(); } set { Set<User>(value); } }

		public FeeEarner FeeEarner { get { return Get<FeeEarner>(); } set { Set<FeeEarner>(value); } }

		public FilePhase Phase { get { return Get<FilePhase>(); } set { Set<FilePhase>(value); } }

		public DocumentVersion DocumentVersion { get { return Get<DocumentVersion>(); } set { Set<DocumentVersion>(value); } }

		public Precedent Precedent { get { return Get<Precedent>(); } set { Set<Precedent>(value); } }

		public NumberFormatInfo NumberFormat { get { return Get<NumberFormatInfo>(); } set { Set<NumberFormatInfo>(value); } }

		public DateTimeFormatInfo DateTimeFormat { get { return Get<DateTimeFormatInfo>(); } set { Set<DateTimeFormatInfo>(value); } }

		public Contact Contact { get { return Get<Contact>(); } set { Set<Contact>(value); } }

		public object Data
		{
			get;
			set;
		}

		public OMSDocument Document { get { return Get<OMSDocument>(); } set { Set<OMSDocument>(value); } }

		public IContext Parent
		{
			get;
			set;
		}

		protected static string GetKey<T>(string name, Type type)
		{
			if (String.IsNullOrWhiteSpace(name))
				return type.FullName;
			else
				return name;
		}

		public T Get<T>()
		{
			return Get<T>(typeof(T));
		}

		public T Get<T>(Type type)
		{
			return Get<T>((string)null, type);
		}

		public T Get<T>(string name)
		{
			return Get<T>(name, typeof(T));
		}

		public T Get<T>(string name, Type type)
		{
			object val;

			var key = GetKey<T>(name, type);
			if (Values.TryGetValue(key, out val))
				return (T)val;

			return default(T);
		}

		public void Set<T>(T value)
		{
			Set<T>(null, value);
		}

		public void Set<T>(string name, T value)
		{
			Set<T>(name, value, null);
		}

		public void Set<T>(string name, T value, Type type)
		{
			type = type ?? (value != null ? value.GetType() : typeof(T));

			var key = GetKey<T>(name, type);

			Values[key] = value;
		}

		public virtual IContext Clone()
		{
			var context = (Context)MemberwiseClone();

			foreach (var v in this.Values)
			{
				context.Values[v.Key] = v.Value;
			}

			return context;
		}
	}
}
