using System;
using System.Collections;

namespace FWBS.Common.Collections
{
    // Declare the event signatures
    public delegate void CollectionClear();
    public delegate void CollectionChange(int index, object value);

    public class CollectionWithEvents : CollectionBase
    {
        // Collection change events
        public event CollectionClear Clearing;
        public event CollectionClear Cleared;
        public event CollectionChange Inserting;
        public event CollectionChange Inserted;
        public event CollectionChange Removing;
        public event CollectionChange Removed;
	
        // Overrides for generating events
        protected override void OnClear()
        {
            // Any attached event handlers?
            if (Clearing != null)
                Clearing();
        }	

        protected override void OnClearComplete()
        {
            // Any attached event handlers?
            if (Cleared != null)
                Cleared();
        }	

        protected override void OnInsert(int index, object value)
        {
            // Any attached event handlers?
            if (Inserting != null)
                Inserting(index, value);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            // Any attached event handlers?
            if (Inserted != null)
                Inserted(index, value);
        }

        protected override void OnRemove(int index, object value)
        {
            // Any attached event handlers?
            if (Removing != null)
                Removing(index, value);
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            // Any attached event handlers?
            if (Removed != null)
                Removed(index, value);
        }

        protected int IndexOf(object value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
    }
}
