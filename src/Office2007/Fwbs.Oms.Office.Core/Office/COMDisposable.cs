using System;
using System.Runtime.InteropServices;

namespace Fwbs.Office
{
    [System.ComponentModel.Browsable(false)]
    public sealed class COMDisposable : IDisposable
    {
        private object[] objects;

        public COMDisposable(params object[] objects)
        {
            this.objects = objects;

        }

        #region IDisposable Members

        public void Dispose()
        {
            if (objects == null)
                return;

            for (int ctr = 0; ctr < objects.Length; ctr++)
            {
                object obj = objects[ctr];

                if (obj == null)
                    continue;

                if (Marshal.IsComObject(obj))
                    Marshal.ReleaseComObject(obj);

                objects[ctr] = null;
                obj = null;
            }

            objects = null;
        }

        #endregion
    }

    [System.ComponentModel.Browsable(false)]
     public sealed class COMDisposable<T> : IDisposable
       where T : class
    {
        private T item;
        private bool keepAlive;

        public COMDisposable(T item, bool keepAlive)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            this.keepAlive = keepAlive;
            this.item = item;
        }

        public T Item
        {
            get
            {
                return item;
            }
        }

        #region IDisposable Members

        public void Release()
        {
            if (Marshal.IsComObject(item))
                Marshal.ReleaseComObject(item);
            item = null;
        }

        public void Dispose()
        {
            if (keepAlive)
                return;

            Release();

        }

        #endregion
    }

    [System.ComponentModel.Browsable(false)] 
    public sealed class COMDisposable<T1, T2> : IDisposable
      where T1 : class
      where T2 : class
    {
        private T1 item1;
        private T2 item2;
        private bool keepAlive;

        public COMDisposable(T1 item1, T2 item2, bool keepAlive)
        {
            if (item1 == null)
                throw new ArgumentNullException("item1");

            if (item2 == null)
                throw new ArgumentNullException("item2");

            this.keepAlive = keepAlive;
            this.item1 = item1;
            this.item2 = item2;
        }

        public T1 Item1
        {
            get
            {
                return item1;
            }
        }

        public T2 Item2
        {
            get
            {
                return item2;
            }
        }

        #region IDisposable Members

        public void Release()
        {
            if (item1 != null)
            {
                if (Marshal.IsComObject(item1))
                    Marshal.ReleaseComObject(item1);
                item1 = null;
            }

            if (item2 != null)
            {
                if (Marshal.IsComObject(item2))
                    Marshal.ReleaseComObject(item2);
                item2 = null;
            }
        }

        public void Dispose()
        {
            if (keepAlive)
                return;

            Release();

        }

        #endregion
    }
}
