using System;
using System.Collections.Generic;

namespace Fwbs
{
    namespace WinFinder
    {
        using System.Text.RegularExpressions;

        public sealed class WindowList : IEnumerable<Window>
        {
            #region Fields

            private Window m_parent;
            private Dictionary<IntPtr, Window> m_list;
            private WindowFilter m_filterCriteria;
            private WindowFilterCallback m_callback;
            private bool m_recursive;

            #endregion

            #region Constructors

            public WindowList()
                : this(null)
            {
            }

            public WindowList(Window parent)
                : this(parent, null, null)
            {
            }

            public WindowList(Window parent, WindowFilter filterCriteria)
                : this(parent, filterCriteria, null)
            {

            }

            public WindowList(Window parent, WindowFilter filterCriteria, WindowFilterCallback callback)
            {
                this.m_parent = parent;
                this.m_filterCriteria = filterCriteria;
                this.m_callback = callback;
                this.m_list = new Dictionary<IntPtr, Window>();
            }

            #endregion

            #region Properties

            public Window Parent
            {
                get
                {
                    return m_parent;
                }
            }

            public int Count
            {
                get
                {
                    return m_list.Count;
                }
            }

            public Window this[int index]
            {
                get
                {
                    Window[] windows = new Window[m_list.Count];
                    m_list.Values.CopyTo(windows, 0);
                    return windows[index];
                }
            }

            public bool IsFiltered
            {
                get
                {
                    return m_filterCriteria != null;
                }
            }

            #endregion

            #region Static

            public static WindowList Find(string className)
            {
                return Find(null, new WindowFilter(className), null, false);
            }

            public static WindowList Find(Window parent, string className)
            {
                return Find(parent, new WindowFilter(className), null, false);
            }

            public static WindowList Find(Window parent, string className, bool recursive)
            {
                return Find(parent, new WindowFilter(className), null, recursive);
            }

            public static WindowList Find(string className, string title)
            {
                return Find(null, new WindowFilter(className, title), null, false);
            }

            public static WindowList Find(Window parent, string className, string title)
            {
                return Find(parent, new WindowFilter(className, title), null, false);
            }

            public static WindowList Find(Window parent, string className, string title, bool recursive)
            {
                return Find(parent, new WindowFilter(className, title), null, recursive);
            }

            public static WindowList Find(WindowFilter filter)
            {
                return Find(null, filter, null, false);
            }

            public static WindowList Find(WindowFilter filter, bool recursive)
            {
                return Find(null, filter, null, recursive);
            }

            public static WindowList Find(Window parent, WindowFilter filter)
            {
                return Find(parent, filter, null, false);
            }

            public static WindowList Find(Window parent, WindowFilter filter, bool recursive)
            {
                return Find(parent, filter, null, recursive);
            }

            public static WindowList Find(WindowFilter filter, WindowFilterCallback callback)
            {
                return Find(null, filter, callback, false);
            }

            public static WindowList Find(WindowFilter filter, WindowFilterCallback callback, bool recursive)
            {
                return Find(null, filter, callback, recursive);
            }

            public static WindowList Find(Window parent, WindowFilter filter, WindowFilterCallback callback)
            {
                return Find(parent, filter, callback, false);
            }

            public static WindowList Find(Window parent, WindowFilter filter, WindowFilterCallback callback, bool recursive)
            {
                WindowList win = new WindowList(parent, filter, callback);
                win.m_recursive = recursive;
                win.Refresh();
                return win;
            }


            #endregion

            #region Methods

            public void Refresh()
            {
                InternalFind();
            }

            public void Reset()
            {
                this.m_filterCriteria = null;
                InternalFind();
            }


            public void Filter(string className)
            {
                Filter(className, null);
            }

            public void Filter(string className, string title)
            {
                Filter(new WindowFilter(className, title));
            }

            public void Filter(WindowFilter filterCriteria)
            {
                Filter(filterCriteria, null);
            }

            public void Filter(WindowFilter filterCriteria, WindowFilterCallback callback)
            {
                if (filterCriteria == null)
                    throw new ArgumentNullException("filterCriteria");
                this.m_filterCriteria = filterCriteria;
                this.m_callback = callback;
                InternalFind();
            }

            private void InternalFind()
            {

                lock (m_list)
                {
                    m_list.Clear();

                    if (m_parent != null)
                    {
                        FindWindows(m_parent.Handle, this.m_filterCriteria ?? new WindowFilter() , this.m_recursive);
                    }
                    else
                    {
                        FindWindows(IntPtr.Zero, this.m_filterCriteria ?? new WindowFilter(), this.m_recursive);
                    }
                }
            }

            private void FindWindows(IntPtr parentHandle, WindowFilter filter, bool recursive)
            {




                IntPtr handle = IntPtr.Zero;
                IntPtr nexthandle = IntPtr.Zero;

                string winclass = String.IsNullOrEmpty(filter.Class) ? null : filter.Class;
                string wintitle = String.IsNullOrEmpty(filter.Title) ? null : filter.Title;

                handle = NativeMethods.FindWindowEx(parentHandle, IntPtr.Zero, winclass, wintitle);
                if (handle == IntPtr.Zero)
                {
                    if (parentHandle == IntPtr.Zero)
                    {
                        if (recursive)
                            throw new InvalidOperationException(Properties.Resources.ExceptionRecursiveWindowsFromRoot);

                        NativeMethods.EnumWindows(new NativeMethods.EnumWindowsProc(EnumWindowCallback), filter);
                    }
                    else
                        NativeMethods.EnumChildWindows(parentHandle, new NativeMethods.EnumWindowsProc(EnumWindowCallback), filter);

                }
                else
                {
                    while (handle != IntPtr.Zero)
                    {
                        AddWindow(handle, filter);

                        if (recursive)
                            FindWindows(handle, filter, true);

                        nexthandle = NativeMethods.FindWindowEx(parentHandle, handle, winclass, wintitle);

                        if (nexthandle == IntPtr.Zero || nexthandle == handle)
                            break;

                        handle = nexthandle;

                    }
                }
            }

            private bool EnumWindowCallback(IntPtr handle, WindowFilter filter)
            {
                AddWindow(handle, filter);
                return true;
            }

            private void AddWindow(IntPtr handle, WindowFilter filter)
            {
                Window win = WindowFactory.GetWindow(handle);

                if (m_list.ContainsKey(win.Handle))
                    return;

                if (filter.Parent != IntPtr.Zero)
                {
                    if (NativeMethods.GetParent(handle) != filter.Parent)
                        return;
                }

                if (m_callback != null)
                {
                    bool? ret = m_callback(win, filter);
                    if (ret.HasValue)
                    {
                        if (!ret.Value)
                        {
                            win.Dispose();
                            return;
                        }
                        else if (ret.Value)
                        {
                            m_list.Add(win.Handle, win);
                            return;
                        }
                    }
                }
                else
                {
                    if (Compare(filter, win))
                        m_list.Add(win.Handle, win);
                    else
                        win.Dispose();
                }
            }


            private static bool IsLikeMatch(string input, string pattern)
            {
                pattern = pattern.Replace("*", @".*");
                pattern = pattern.Replace("%", @".*");
                pattern = "^" + pattern + @"\z";
                return Regex.Match(input, pattern).Success;
            }

            private static bool Compare(WindowFilter filter, Window win)
            {

                if (CompareBoolean(filter.Visible, win.IsVisible))
                {
                    if (String.IsNullOrEmpty(filter.Class) || CompareText(filter.Class, win.Class))
                    {
                        if (String.IsNullOrEmpty(filter.Title) || CompareText(filter.Title, win.Text))
                        {
                            if (String.IsNullOrEmpty(filter.FileName) || CompareText(filter.FileName, win.FileName))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }

            private static bool CompareText(string text, string wintext)
            {
                if (wintext == null)
                    wintext = String.Empty;

                wintext = wintext.ToUpperInvariant();

                if (text == null)
                    text = String.Empty;
                text = text.ToUpperInvariant();

                

                return (string.Compare(wintext, text, StringComparison.OrdinalIgnoreCase) == 0 ||
                    IsLikeMatch(wintext, text));
                   
            }

            private static bool CompareBoolean(BooleanFilter filter, bool winval)
            {
                switch (filter)
                {
                    case BooleanFilter.Both:
                        return true;
                    case BooleanFilter.False:
                        return !winval;
                    case BooleanFilter.True:
                        return winval;
                }

                return false;
            }
            public override string ToString()
            {
                return Count.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            #endregion

            #region IEnumerable<Window> Members

            public IEnumerator<Window> GetEnumerator()
            {
                return m_list.Values.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion


        }
    }
}