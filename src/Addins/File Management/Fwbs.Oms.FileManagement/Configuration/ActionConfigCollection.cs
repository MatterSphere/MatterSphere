using System;
using System.Xml;

namespace FWBS.OMS.FileManagement.Configuration
{
	public sealed class ActionConfigCollection : Crownwood.Magic.Collections.CollectionWithEvents
	{
		#region Fields

		private XmlElement _info;
		private FMApplication _app;

		#endregion

		#region Constructors

		private ActionConfigCollection(){}
		

		internal ActionConfigCollection(FMApplication app, XmlElement info)
		{
			if (app == null)
				throw new ArgumentNullException("app");

			if (info == null)
				throw new ArgumentNullException("info");

			_app = app;
			_info = info;

			foreach (XmlNode nd in _info.ChildNodes)
			{
				if (nd is XmlElement)
				{
					Add(new ActionConfig(_app, (XmlElement)nd.Clone()));
				}
			}

		}


		#endregion

		#region Collection Specifics

		internal ActionConfig Add(ActionConfig value)
		{
			// Use base class to process actual collection operation
			base.List.Add(value as object);
			return value;
		}

		internal void AddRange(ActionConfig[] values)
		{
			// Use existing method to add each array entry
			foreach(ActionConfig action in values)
			{
				if (action != null)Add(action);
			}
		}

		internal void Remove(ActionConfig value)
		{
			// Use base class to process actual collection operation
			base.List.Remove(value as object);
		}

		internal void Insert(int index, ActionConfig value)
		{
			// Use base class to process actual collection operation
			base.List.Insert(index, value as object);
		}

		internal bool Contains(ActionConfig value)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(value as object);
		}

		public ActionConfig this[int index]
		{
			// Use base class to process actual collection operation
			get { return (base.List[index] as ActionConfig); }
		}

		internal int IndexOf(ActionConfig value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}

		#endregion

		#region Methods

		public override string ToString()
		{
			return String.Format("Actions: {0}", Count);
		}

		#endregion
	}
}
