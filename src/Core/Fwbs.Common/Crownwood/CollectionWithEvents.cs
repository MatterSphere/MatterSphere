// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Haxey, North Lincolnshire, England and are supplied subject to 
//	licence terms.
// 
//  Magic Version 1.6.1 	www.dotnetmagic.com
// *****************************************************************************

using System;
using System.Collections;

namespace Crownwood.Magic.Collections
{
    // Declare the event signatures
    public delegate void CollectionClear();
    public delegate void CollectionChange(int index, object value);

    /// <summary>
    /// Collection Class wrapper
    /// </summary>
    public class CollectionWithEvents : CollectionBase
    {
        // Collection change events
        /// <summary>
        /// Occurs when [clearing].
        /// </summary>
        public event CollectionClear Clearing;
        /// <summary>
        /// Occurs when [cleared].
        /// </summary>
        public event CollectionClear Cleared;
        /// <summary>
        /// Occurs when [inserting].
        /// </summary>
        public event CollectionChange Inserting;
        /// <summary>
        /// Occurs when [inserted].
        /// </summary>
        public event CollectionChange Inserted;
        /// <summary>
        /// Occurs when [removing].
        /// </summary>
        public event CollectionChange Removing;
        /// <summary>
        /// Occurs when [removed].
        /// </summary>
        public event CollectionChange Removed;
	
        // Overrides for generating events
        protected override void OnClear()
        {
            // Any attached event handlers?
            if (Clearing != null)
                Clearing();
        }

        /// <summary>
        /// Performs additional custom processes after clearing the contents of the <see cref="T:System.Collections.CollectionBase"/> instance.
        /// </summary>
        protected override void OnClearComplete()
        {
            // Any attached event handlers?
            if (Cleared != null)
                Cleared();
        }

        /// <summary>
        /// Performs additional custom processes before inserting a new element into the <see cref="T:System.Collections.CollectionBase"/> instance.
        /// </summary>
        /// <param name="index">The zero-based index at which to insert <paramref name="value"/>.</param>
        /// <param name="value">The new value of the element at <paramref name="index"/>.</param>
        protected override void OnInsert(int index, object value)
        {
            // Any attached event handlers?
            if (Inserting != null)
                Inserting(index, value);
        }

        /// <summary>
        /// Performs additional custom processes after inserting a new element into the <see cref="T:System.Collections.CollectionBase"/> instance.
        /// </summary>
        /// <param name="index">The zero-based index at which to insert <paramref name="value"/>.</param>
        /// <param name="value">The new value of the element at <paramref name="index"/>.</param>
        protected override void OnInsertComplete(int index, object value)
        {
            // Any attached event handlers?
            if (Inserted != null)
                Inserted(index, value);
        }

        /// <summary>
        /// Performs additional custom processes when removing an element from the <see cref="T:System.Collections.CollectionBase"/> instance.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="value"/> can be found.</param>
        /// <param name="value">The value of the element to remove from <paramref name="index"/>.</param>
        protected override void OnRemove(int index, object value)
        {
            // Any attached event handlers?
            if (Removing != null)
                Removing(index, value);
        }

        /// <summary>
        /// Performs additional custom processes after removing an element from the <see cref="T:System.Collections.CollectionBase"/> instance.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="value"/> can be found.</param>
        /// <param name="value">The value of the element to remove from <paramref name="index"/>.</param>
        protected override void OnRemoveComplete(int index, object value)
        {
            // Any attached event handlers?
            if (Removed != null)
                Removed(index, value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The index of <paramref name="value"/> if found in the list; otherwise, -1.
        /// </returns>
        protected int IndexOf(object value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
    }
}
