using System;

namespace Fwbs
{
    namespace WinFinder
    {
        using System.Runtime.InteropServices;


        [StructLayout(LayoutKind.Sequential)]
        public sealed class WindowFilter
        {
            #region Fields

            private IntPtr parent;
            [MarshalAs(UnmanagedType.LPWStr)]
            private string title;
            [MarshalAs(UnmanagedType.LPWStr)]
            private string filename;
            [MarshalAs(UnmanagedType.LPWStr)]
            private string classname;
            private BooleanFilter visible;

            #endregion

            #region Constructors

            public WindowFilter()
            {
            }

            public WindowFilter(bool visible)
            {
                if (visible)
                    this.visible = BooleanFilter.True;
                else
                    this.visible = BooleanFilter.False;
            }

            public WindowFilter(string className)
                : this(className, null)
            {
            }

            public WindowFilter(string className, string title)
            {
                this.classname = className;
                this.title = title;
            }

            public WindowFilter(string className, string title, bool visible)
                : this(className, title)
            {

                if (visible)
                    this.visible = BooleanFilter.True;
                else
                    this.visible = BooleanFilter.False;
            }

            #endregion

            #region Properties

            internal IntPtr Parent
            {
                get
                {
                    return parent;
                }
                set
                {
                    parent = value;
                }
            }

            public string FileName
            {
                get
                {
                    return filename;
                }
                set
                {
                    filename = value;
                }
            }

            public string Title
            {
                get
                {
                    return title;
                }
                set
                {
                    title = value;
                }
            }

            public string Class
            {
                get
                {
                    return classname;
                }
                set
                {
                    classname = value;
                }
            }

            public BooleanFilter Visible
            {
                get
                {
                    return visible;
                }
                set
                {
                    visible = value;
                }
            }

            #endregion

            #region Methods

            public WindowFilter Clone()
            {
                WindowFilter filter = (WindowFilter)MemberwiseClone();
                filter.title = title;
                filter.classname = classname;
                filter.filename = filename;
                return filter;
            }

            #endregion
        }



    }

}
