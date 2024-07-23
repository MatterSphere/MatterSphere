using System;
using System.Collections;


namespace FWBS.Common.UI.Windows
{
	public class ChangeEventArgs : EventArgs 
	{  
		private object _item;
		private string _what;
		public ChangeEventArgs(object Item, string What) 
		{_item = Item;_what = What;}
		public object Item{get{return _item;}}
		public string What{get{return _what;}}
	}

	// A delegate type for hooking up change notifications.
	public delegate void ChangedEventHandler(object sender, ChangeEventArgs e);
	
	/// <summary>
	/// Summary description for EventArrayList.
	/// </summary>
	public class EventArrayList: ArrayList
	{	
		// An event that clients can use to be notified whenever the
		// elements of the list change.
		public event ChangedEventHandler Changed;
		// Invoke the Changed event; called whenever list changes
		protected virtual void OnChanged(ChangeEventArgs e) 
		{
			if (Changed != null)
				Changed(this, e);
		}

		// Override some of the methods that can change the list;
		// invoke event after each
		public override int Add(object value) 
		{
			int i = base.Add(value);
			ChangeEventArgs e = new ChangeEventArgs(value,"add");
			OnChanged(e);
			return i;
		}

		public override void Clear() 
		{
			base.Clear();
			ChangeEventArgs e = new ChangeEventArgs(null,"clear");
			OnChanged(e);
		}
	}
}
