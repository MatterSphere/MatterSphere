using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FWBS.OMS;

namespace Fwbs.Office
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public abstract class OfficeObject : IDisposable
    {
        #region Fields

        private object objref;

        protected readonly bool NeedsReleasing;
        protected object Missing = System.Reflection.Missing.Value;
        private bool areeventsattached;
        private readonly Dictionary<string, object> state = new Dictionary<string, object>();
        private bool disposed;
        private bool isdetached = true;

        #endregion

        #region Constructors

        protected OfficeObject()
        {
        }

        protected OfficeObject(bool needsReleasing)
        {
            this.NeedsReleasing = needsReleasing;
        }

        #endregion

        #region Properties


        public bool AreEventsAttached
        {
            get
            {
                return areeventsattached;
            }
        }

        #endregion

        #region Methods

        public void SetState<T>(string name, T value)
        {
            if (state.ContainsKey(name))
                state.Remove(name);
            state.Add(name, value);
        }

        public T GetState<T>(string name)
        {
            return GetState<T>(name, default(T));
        }

        public T GetState<T>(string name, T defaultValue)
        {
            return GetState<T>(name, defaultValue, false);
        }

        public T GetState<T>(string name, T defaultValue, bool remove)
        {
            try
            {
                object val;
                if (state.TryGetValue(name, out val))
                {
                    if (remove)
                        state.Remove(name);
                    return (T)val;
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public object Unwrap()
        {
            return InternalObject;
        }

        protected virtual void Init(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            Attach(obj);

            AttachEvents();
        }


        protected internal void AttachEvents()
        {
            if (AreEventsAttached)
                return;

            OnAttachEvents();

            areeventsattached = true;
        }

        protected virtual void OnAttachEvents()
        {
        }

        protected void DetachEvents()
        {
            if (!AreEventsAttached)
                return;

            OnDetachEvents();

            areeventsattached = false;
        }

        protected virtual void OnDetachEvents()
        {
        }

        public bool HasProperty(string name)
        {
            CheckIfDetached();

            return HasPropertyEx(InternalObject, name);
        }

        public T GetProperty<T>(string name, params object[] args)
        {
            return (T)WrapValue(GetPropertyEx<T>(InternalObject, name, args));
        }

        public static T GetPropertyEx<T>(object objref, string name, params object[] args)
        {
            try
            {
                return (T)objref.GetType().InvokeMember(name, System.Reflection.BindingFlags.GetProperty, null, objref, args);
            }
            catch (System.Reflection.TargetInvocationException tex)
            {
                throw tex.InnerException;
            }
        }

        public static bool HasPropertyEx(object objref, string name)
        {
            try
            {
                var ret = objref.GetType().InvokeMember(name, System.Reflection.BindingFlags.GetProperty, null, objref, null);
                return true;
            }
            catch (COMException comex)
            {
                if (comex.ErrorCode == HResults.E_DISP_UNKNOWN)
                    return false;
                throw;
            }
            catch (System.Reflection.TargetInvocationException)
            {
                return false;
            }
        }

        public void SetProperty(string name, object value, params object[] args)
        {
            SetPropertyEx(InternalObject, name, UnwrapValue(value), args);
        }

        public static void SetPropertyEx(object objref, string name, object value, params object[] args)
        {
            try
            {
                object existing = null;

                if (args != null && args.Length > 0)
                    existing = GetPropertyEx<object>(objref, name, value);
                else
                {
                    existing = GetPropertyEx<object>(objref, name);

                    if (object.Equals(existing, value))
                        return;
                }

                var list = new List<object>();
                list.Add(value);
                list.AddRange(args);
                objref.GetType().InvokeMember(name, System.Reflection.BindingFlags.SetProperty, null, objref, list.ToArray());
            }
            catch (System.Reflection.TargetInvocationException tex)
            {
                throw tex.InnerException;
            }
        }



        public T Invoke<T>(string name, params object[] args)
        {
            if (args != null)
            {
                for (int ctr = 0; ctr < args.Length; ctr++)
                    args[ctr] = UnwrapValue(args[ctr]);
            }

            return InvokeEx<T>(InternalObject, name, args);
        }
        public static T InvokeEx<T>(object objref, string name, params object[] args)
        {
            try
            {
                return (T)objref.GetType().InvokeMember(name, System.Reflection.BindingFlags.InvokeMethod, null, objref, args);
            }
            catch (System.Reflection.TargetInvocationException tex)
            {
                throw tex.InnerException;
            }

        }

        protected virtual object WrapValue(object value)
        {
            return value;
        }

        protected virtual object UnwrapValue(object value)
        {
            OfficeObject off = value as OfficeObject;
            if (off != null)
                return off.InternalObject;

            return value;
        }

        public object InternalObject
        {
            get
            {
                CheckIfDisposed();
                CheckIfDetached();

                return objref;
            }
        }

        #endregion

        #region Detached

        protected internal void CheckIfDetached()
        {
            if (IsDetached)
                throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("BJISINDTCST", "The object is in a detached state.", "").Text);
        }


        public virtual bool IsDetached
        {
            get
            {
                if (objref == null)
                    return true;

                if (!Marshal.IsComObject(objref))
                    return false;

                return isdetached;
            }
            protected set
            {
                isdetached = value;
            }
        }

        protected virtual bool CanDetach
        {
            get
            {
                return true;
            }
        }

        protected internal void Attach(object obj)
        {
            if (!IsDetached)
                return;


            if (obj == null)
                throw new ArgumentNullException("obj");

            if (!Marshal.IsComObject(obj))
                throw new ArgumentException(Session.CurrentSession.Resources.GetMessage("OBJISNTCMBJ", "The object to be attached is not a com object.", "").Text);

            this.objref = obj;

            this.isdetached = false;

            OnAttach(obj);            
        }

        public void Detach()
        {
            Detach(false);
        }

        public void Detach(bool force)
        {
            if (IsDetached)
                return;

            if (!force)
            {
                if (!CanDetach)
                    return;
            }

            DetachEvents();

            OnDetach();

            if (objref != null)
            {
                if (Marshal.IsComObject(objref))
                {
                    if (isdetached)
                    {
                        objref = null;
                        return;
                    }
                    Marshal.ReleaseComObject(objref);
                }
                isdetached = true;
                objref = null;
            }
            
        }

        protected virtual void OnDetach()
        {
        }

        protected virtual void OnAttach(object obj)
        {
        }

        #endregion

        #region IDisposable Members

        protected internal void CheckIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public bool IsDisposed
        {
            get
            {
                return disposed;
            }
        }

        public void Dispose()
        {
            if (disposed)
                return;

            DetachEvents();
            Dispose(true);
            GC.SuppressFinalize(this);
            disposed = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (objref != null && NeedsReleasing)
            {
                Detach();
            }
        }

        ~OfficeObject()
        {
            Dispose(false);
        }

        #endregion


    }
}
