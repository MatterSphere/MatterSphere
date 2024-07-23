using System;

namespace FWBS.OMS.Extensibility
{
	
	public class ObjectEvent
	{
		internal readonly bool CanCancel;
		public readonly string Name;

		private ObjectEvent(){}
		public ObjectEvent(string name, bool canCancel)
		{
			Name = name;
			CanCancel = canCancel;
		}

		public override string ToString()
		{
			return Name;
		}


		public static readonly ObjectEvent Loaded = new ObjectEvent("Loaded", false);
		public static readonly ObjectEvent Creating = new ObjectEvent("Creating", true);
		public static readonly ObjectEvent Created = new ObjectEvent("Created", false);
		public static readonly ObjectEvent Deleting = new ObjectEvent("Deleting", true);
		public static readonly ObjectEvent Deleted = new ObjectEvent("Deleted", false);
		public static readonly ObjectEvent Updating = new ObjectEvent("Updating", true);
		public static readonly ObjectEvent Updated = new ObjectEvent("Updated", false);
		public static readonly ObjectEvent ValueChanging = new ObjectEvent("ValueChanging", true);
		public static readonly ObjectEvent ValueChanged = new ObjectEvent("ValueChanged", false);
		public static readonly ObjectEvent Refreshing = new ObjectEvent("Refreshing", true);
		public static readonly ObjectEvent Refreshed = new ObjectEvent("Refreshed", false);


	}

	public class ObjectEventArgs : EventArgs
	{
		public readonly object Sender;
		public readonly ObjectEvent Event;
		public readonly bool CanCancel;
		public bool Cancel = false;
		public readonly EventArgs Arguments;

		public ObjectEventArgs(Object sender, ObjectEvent ev) : this(sender, ev, ev.CanCancel, EventArgs.Empty)
		{
		}

		public ObjectEventArgs(Object sender, ObjectEvent ev, bool canCancel) : this(sender, ev, canCancel, EventArgs.Empty)
		{
		}

		public ObjectEventArgs(Object sender, ObjectEvent ev, bool canCancel, EventArgs arguments)
		{
			Sender = sender;
			Event = ev;
			CanCancel = canCancel;
			Arguments = arguments;
		}

		public override string ToString()
		{
			string val = @"Sender:{0}
Event: {1}
Can Cancel: {2}
Cancelled: {3}
Arguments: {4}";
			return String.Format(val, Sender, Event, CanCancel, Cancel, Arguments);
		}

	}
}
