using System;

namespace Fwbs.Office
{
    using MSOffice = Microsoft.Office.Core;

    public sealed partial class OfficeCommandBar : 
        OfficeObject,
        MSOffice.CommandBar
    {
        private readonly MSOffice.CommandBar bar;
        private readonly OfficeCommandBarControls controls;
        

        public OfficeCommandBar(MSOffice.CommandBar bar)
        {
            if (bar == null)
                throw new ArgumentNullException("bar");

            this.bar = bar;
            this.controls = new OfficeCommandBarControls(this, bar.Controls);

            Init(bar);
        }

        #region Overrides

     
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (controls != null)
                        controls.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion

        #region CommandBar Members

        public bool AdaptiveMenu
        {
            get
            {
                return bar.AdaptiveMenu;
            }
            set
            {
                bar.AdaptiveMenu = value;
            }
        }

        public object Application
        {
            get { return bar.Application; }
        }

        public bool BuiltIn
        {
            get { return bar.BuiltIn; }
        }

        public string Context
        {
            get
            {
                return bar.Context;
            }
            set
            {
                bar.Context = value;
            }
        }

        public MSOffice.CommandBarControls Controls
        {
            get
            {
                return controls;
            }
        }
    
        public int Creator
        {
            get { return bar.Creator; }
        }

        public void Delete()
        {
            bar.Delete();
        }

        public bool Enabled
        {
            get
            {
                return bar.Enabled;
            }
            set
            {
                bar.Enabled = value;
            }
        }

        public MSOffice.CommandBarControl FindControl(object Type, object Id, object Tag, object Visible, object Recursive)
        {
            
            var ctrl = bar.FindControl(Type, Id, Tag, Visible, Recursive);

            return controls.GetControl(ctrl);
        }

        public int Height
        {
            get
            {
                return bar.Height;
            }
            set
            {
                bar.Height = value;
            }
        }

        public int Id
        {
            get { return bar.Id; }
        }

        public int Index
        {
            get { return bar.Index; }
        }

        public int InstanceId
        {
            get { return bar.InstanceId; }
        }

        public int Left
        {
            get
            {
                return bar.Left;
            }
            set
            {
                bar.Left = value;
            }
        }

        public string Name
        {
            get
            {
                return bar.Name;
            }
            set
            {
                bar.Name = value;
            }
        }

        public string NameLocal
        {
            get
            {
                return bar.NameLocal;
            }
            set
            {
                bar.NameLocal = value;
            }
        }

        public object Parent
        {
            get { return bar.Parent; }
        }

        public MSOffice.MsoBarPosition Position
        {
            get
            {
                return bar.Position;
            }
            set
            {
                bar.Position = value;
            }
        }

        public MSOffice.MsoBarProtection Protection
        {
            get
            {
                return bar.Protection;
            }
            set
            {
                bar.Protection = value;
            }
        }

        public void Reset()
        {
            bar.Reset();
        }

        public int RowIndex
        {
            get
            {
                return bar.RowIndex;
            }
            set
            {
                bar.RowIndex = value;
            }
        }

        public void ShowPopup(object x, object y)
        {
            bar.ShowPopup(x, y);
        }

        public int Top
        {
            get
            {
               return bar.Top;
            }
            set
            {
                bar.Top = value;
            }
        }

        public MSOffice.MsoBarType Type
        {
            get { return bar.Type; }
        }

        public bool Visible
        {
            get
            {
                return bar.Visible;
            }
            set
            {
                bar.Visible = value;
            }
        }

        public int Width
        {
            get
            {
                return bar.Width;
            }
            set
            {
                bar.Width = value;
            }
        }

        public int accChildCount
        {
            get { return bar.accChildCount; }
        }

        public void accDoDefaultAction(object varChild)
        {
            bar.accDoDefaultAction(varChild);
        }

        public object accFocus
        {
            get { return bar.accFocus; }
        }

        public object accHitTest(int xLeft, int yTop)
        {
            return bar.accHitTest(xLeft, yTop);
        }

        public void accLocation(out int pxLeft, out int pyTop, out int pcxWidth, out int pcyHeight, object varChild)
        {
            bar.accLocation(out pxLeft, out pyTop, out pcxWidth, out pcyHeight, varChild);
        }

        public object accNavigate(int navDir, object varStart)
        {
            return bar.accNavigate(navDir, varStart);
        }

        public object accParent
        {
            get { return bar.accParent; }
        }

        public void accSelect(int flagsSelect, object varChild)
        {
            bar.accSelect(flagsSelect, varChild);
        }

        public object accSelection
        {
            get { return bar.accSelection; }
        }

        public object get_accChild(object varChild)
        {
            return bar.get_accChild(varChild);
        }

        public string get_accDefaultAction(object varChild)
        {
            return bar.get_accDefaultAction(varChild);
        }

        public string get_accDescription(object varChild)
        {
            return bar.get_accDescription(varChild);
        }

        public string get_accHelp(object varChild)
        {
            return bar.get_accHelp(varChild);
        }

        public int get_accHelpTopic(out string pszHelpFile, object varChild)
        {
            return bar.get_accHelpTopic(out pszHelpFile, varChild);
        }

        public string get_accKeyboardShortcut(object varChild)
        {
            return bar.get_accKeyboardShortcut(varChild);
        }

        public string get_accName(object varChild)
        {
            return bar.get_accName(varChild);
        }

        public object get_accRole(object varChild)
        {
            return bar.get_accRole(varChild);
        }

        public object get_accState(object varChild)
        {
            return bar.get_accState(varChild);
        }

        public string get_accValue(object varChild)
        {
            return bar.get_accValue(varChild);
        }

        public void set_accName(object varChild, string pszName)
        {
            bar.set_accName(varChild, pszName);
        }

        public void set_accValue(object varChild, string pszValue)
        {
            bar.set_accValue(varChild, pszValue);
        }

        #endregion

    }
}
