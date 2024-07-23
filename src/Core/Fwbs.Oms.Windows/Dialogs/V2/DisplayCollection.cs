using System.Collections;
using System.Collections.Generic;
using FWBS.OMS.UI.Windows.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    internal class DisplayCollection<T> : IEnumerable<T> where T : class, IDisplay
    {
        protected List<T> Displays { get; set; }

        public DisplayCollection()
        {
            Displays = new List<T>();
        }

        public int Count
        {
            get
            {
                return Displays.Count;
            }
        }

        public T FirstDisplay
        {
            get
            {
                return Displays.Count > 0
                    ? Displays[0]
                    : null;
            }
        }

        public T LastDisplay
        {
            get
            {
                return Displays.Count > 0
                    ? Displays[Count - 1]
                    : null;
            }
        }

        public T this[int number]
        {
            get
            {
                if (number >= 0 && number < Displays.Count)
                {
                    return Displays[number];
                }

                return null;
            }
        }

        public void Add(T display)
        {
            Displays.Add(display);
        }

        public void Clear()
        {
            Displays.Clear();
        }

        public void Remove(T display)
        {
            Displays.Remove(display);
        }

        #region IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T display in Displays)
            {
                yield return display;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
