using System;
using System.Xml;

namespace FWBS.OMS.FileManagement.Configuration
{

	public sealed class MilestoneTaskConfigCollection : Crownwood.Magic.Collections.CollectionWithEvents
	{
		#region Fields

		private XmlElement _info;
		private FMApplication _app;

		#endregion

		#region Constructors

		private MilestoneTaskConfigCollection(){}
		

		internal MilestoneTaskConfigCollection(FMApplication app, XmlElement info)
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
					Add(new MilestoneTaskConfig(_app, (XmlElement)nd.Clone()));
				}
			}

		}


		#endregion

		#region Collection Specifics

		internal MilestoneTaskConfig Add(MilestoneTaskConfig value)
		{
			// Use base class to process actual collection operation
			base.List.Add(value as object);
			return value;
		}

		internal void AddRange(MilestoneTaskConfig[] values)
		{
			// Use existing method to add each array entry
			foreach(MilestoneTaskConfig action in values)
			{
				if (action != null)Add(action);
			}
		}

		internal void Remove(MilestoneTaskConfig value)
		{
			// Use base class to process actual collection operation
			base.List.Remove(value as object);
		}

		internal void Insert(int index, MilestoneTaskConfig value)
		{
			// Use base class to process actual collection operation
			base.List.Insert(index, value as object);
		}

		internal bool Contains(MilestoneTaskConfig value)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(value as object);
		}

		public MilestoneTaskConfig this[int index]
		{
			// Use base class to process actual collection operation
			get { return (base.List[index] as MilestoneTaskConfig); }
		}

		internal int IndexOf(MilestoneTaskConfig value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}

		#endregion

		#region Methods

		public override string ToString()
		{
			return String.Format("Tasks: {0}", Count);
		}

		#endregion
	}
}
