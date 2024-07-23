using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FWBS.OMS.Workflow
{
    public class SortedObservableCollection<T>  : ObservableCollection<T>
		where T : IComparable
	{
		#region Constructors
		public SortedObservableCollection()
			: base()
		{ }

		public SortedObservableCollection(IEnumerable<T> collection)
		{
			foreach (T item in collection)
			{
				this.Add(item);
			}
		}

		public SortedObservableCollection(List<T> collection)
		{
			collection.Sort();
			this.AddRange(collection);
		}
		#endregion

		#region Properties
		public bool Distinct { get;  set; }
		#endregion

		#region Add
		public new void Add(T item)
		{
			for (int i = 0; i < this.Count; i++)
			{
				switch (Math.Sign(this[i].CompareTo(item)))
				{
					case 0:
						if (this.Distinct)
						{
							throw new InvalidOperationException("Cannot insert duplicated items");
						}
						else
						{
							base.InsertItem(i, item);
						}
						return;
					case 1:
						base.InsertItem(i, item);
						return;
					case -1:
						break;
				}
			}
			base.InsertItem(this.Count, item);
		}
		#endregion

		#region InsertItem override
		protected override void InsertItem(int index, T item)
		{
			// NOTE: This algorithm does not perform as it is a straight forward linear loop
			//	For large items we need a better search algorithm like binary search since the items are sorted
			for (int i = 0; i < this.Count; i++)
			{
				switch (Math.Sign(this[i].CompareTo(item)))
				{
					case 0:
						if (this.Distinct)
						{
							throw new InvalidOperationException("Cannot insert duplicated items");
						}
						else
						{
							base.InsertItem(i, item);
						}
						return;
					case 1:
						base.InsertItem(i, item);
						return;
					case -1:
						break;
				}
			}
			
			base.InsertItem(this.Count, item);
		}
		#endregion
	}
}
