using System;

namespace Fwbs.Office
{
    using System.Runtime.InteropServices;
    using MSOffice = Microsoft.Office.Core;

    internal abstract class OfficeCommandBarControl : 
        OfficeObject,
        MSOffice.CommandBarControl
    {
        private readonly MSOffice.CommandBarControl ctrl;
        private readonly OfficeCommandBarControls parent;
        private int instanceid;
        private string tag;

        internal OfficeCommandBarControl(OfficeCommandBarControls parent, MSOffice.CommandBarControl ctrl)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (ctrl == null)
                throw new ArgumentNullException("ctrl");
            
            this.parent = parent;
            this.ctrl = ctrl;
            this.instanceid = ctrl.InstanceId;
            this.tag = ctrl.Tag;
        }


        #region _CommandBarButton Members

        public object Application
        {
            get
            {
                return ctrl.Application;
            }
        }

        public bool BeginGroup
        {
            get
            {
                return ctrl.BeginGroup;
            }
            set
            {
                ctrl.BeginGroup = value;
            }
        }

        public bool BuiltIn
        {
            get
            {
                return ctrl.BuiltIn;
            }
        }

        public string Caption
        {
            get
            {
                return ctrl.Caption;
            }
            set
            {
                ctrl.Caption = value;
            }
        }

        public object Control
        {
            get { return ctrl.Control; }
        }

        public Microsoft.Office.Core.CommandBarControl Copy(object Bar, object Before)
        {
            throw new NotImplementedException();
        }

 
        public int Creator
        {
            get { return ctrl.Creator; }
        }

        public virtual void Delete(object Temporary)
        {
            parent.RemoveControl(this);
            DetachEvents();

            try
            {
                ctrl.Delete(Temporary);
            }
            catch (COMException)
            {
            }
            catch (System.Runtime.InteropServices.InvalidComObjectException)
            {
            }

            Dispose();
        }

        public string DescriptionText
        {
            get
            {
                return ctrl.DescriptionText;
            }
            set
            {
                ctrl.DescriptionText = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return ctrl.Enabled;
            }
            set
            {
                ctrl.Enabled = value;
            }
        }

        public void Execute()
        {
            ctrl.Execute();
        }



        public int Height
        {
            get
            {
                return ctrl.Height;
            }
            set
            {
                ctrl.Height = value;
            }
        }

        public int HelpContextId
        {
            get
            {
                return ctrl.HelpContextId;
            }
            set
            {
                ctrl.HelpContextId = value;
            }
        }

        public string HelpFile
        {
            get
            {
                return ctrl.HelpFile;
            }
            set
            {
                ctrl.HelpFile = value;
            }
        }

        public int Id
        {
            get
            {
                return ctrl.Id;
            }
        }

        public int Index
        {
            get
            {
                return ctrl.Index;
            }
        }

        public int InstanceId
        {
            get
            {
                try
                {
                    return ctrl.InstanceId;
                }
                catch (COMException)
                {
                    return instanceid;
                }
            }
        }

        public bool IsPriorityDropped
        {
            get
            {
                return ctrl.IsPriorityDropped;
            }
        }

        public int Left
        {
            get
            {
                return ctrl.Left;
            }
        }


        public Microsoft.Office.Core.CommandBarControl Move(object Bar, object Before)
        {
            throw new NotImplementedException();
        }

        public Microsoft.Office.Core.MsoControlOLEUsage OLEUsage
        {
            get
            {
                return ctrl.OLEUsage;
            }
            set
            {
                ctrl.OLEUsage = value;
            }
        }

        public string OnAction
        {
            get
            {
                return ctrl.OnAction;
            }
            set
            {
                ctrl.OnAction = value;
            }
        }


        public string Parameter
        {
            get
            {
                return ctrl.Parameter;
            }
            set
            {
                ctrl.Parameter = value;
            }
        }

        public Microsoft.Office.Core.CommandBar Parent
        {
            get 
            { 
                return parent.Parent; 
            }
        }

        public int Priority
        {
            get
            {
                return ctrl.Priority;
            }
            set
            {
                ctrl.Priority = value;
            }
        }

        public void Reserved1()
        {
            throw new NotImplementedException();
        }

        public void Reserved2()
        {
            throw new NotImplementedException();
        }

        public void Reserved3()
        {
            throw new NotImplementedException();
        }

        public void Reserved4()
        {
            throw new NotImplementedException();
        }

        public void Reserved5()
        {
            throw new NotImplementedException();
        }

        public void Reserved6()
        {
            throw new NotImplementedException();
        }

        public void Reserved7()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            ctrl.Reset();
        }

        public void SetFocus()
        {
            ctrl.SetFocus();
        }



        public string Tag
        {
            get
            {
                return tag;
            }
            set
            {
                try
                {
                    tag = value;
                    ctrl.Tag = tag;
                }
                catch (COMException)
                {
                    throw;
                }
            }
        }

        public string TooltipText
        {
            get
            {
                return ctrl.TooltipText;
            }
            set
            {
                ctrl.TooltipText = value;
            }
        }

        public int Top
        {
            get { return ctrl.Top; }
        }

        public Microsoft.Office.Core.MsoControlType Type
        {
            get { return ctrl.Type; }
        }

        public bool Visible
        {
            get
            {
                return ctrl.Visible;
            }
            set
            {
                ctrl.Visible = value;
            }
        }

        public int Width
        {
            get
            {
                return ctrl.Width;
            }
            set
            {
                ctrl.Width = value;
            }
        }

        public int accChildCount
        {
            get { return ctrl.accChildCount; }
        }

        public void accDoDefaultAction(object varChild)
        {
            ctrl.accDoDefaultAction(varChild) ;
        }

        public object accFocus
        {
            get { return ctrl.accFocus; }
        }

        public object accHitTest(int xLeft, int yTop)
        {
            return ctrl.accHitTest(xLeft, yTop);
        }

        public void accLocation(out int pxLeft, out int pyTop, out int pcxWidth, out int pcyHeight, object varChild)
        {
            ctrl.accLocation(out pxLeft, out pyTop, out pcxWidth, out pcyHeight, varChild);
        }

        public object accNavigate(int navDir, object varStart)
        {
            return ctrl.accNavigate(navDir, varStart);
        }

        public object accParent
        {
            get { return ctrl.accParent ; }
        }

        public void accSelect(int flagsSelect, object varChild)
        {
            ctrl.accSelect(flagsSelect, varChild);
        }

        public object accSelection
        {
            get { return ctrl.accSelection; }
        }

        public object get_accChild(object varChild)
        {
            return ctrl.get_accChild(varChild);
        }

        public string get_accDefaultAction(object varChild)
        {
            return ctrl.get_accDefaultAction(varChild);
        }

        public string get_accDescription(object varChild)
        {
            return ctrl.get_accDescription(varChild);
        }

        public string get_accHelp(object varChild)
        {
            return ctrl.get_accHelp(varChild);
        }

        public int get_accHelpTopic(out string pszHelpFile, object varChild)
        {
            return ctrl.get_accHelpTopic(out pszHelpFile, varChild);
        }

        public string get_accKeyboardShortcut(object varChild)
        {
            return ctrl.get_accKeyboardShortcut(varChild);
        }

        public string get_accName(object varChild)
        {
            return ctrl.get_accName(varChild);
        }

        public object get_accRole(object varChild)
        {
            return ctrl.get_accRole(varChild);
        }

        public object get_accState(object varChild)
        {
            return ctrl.get_accValue(varChild);
        }

        public string get_accValue(object varChild)
        {
            return ctrl.get_accValue(varChild) ;
        }

        public void set_accName(object varChild, string pszName)
        {
            ctrl.set_accName(varChild, pszName);
        }

        public void set_accValue(object varChild, string pszValue)
        {
            ctrl.set_accValue(varChild, pszValue);
        }

        public virtual MSOffice.CommandBarControls Controls
        {
            get
            {
                return null;
            }
        }

        #endregion

        public override bool IsDetached
        {
            get
            {
                //Make sure this gets called as the Dispose lower down will call IsDetached proeprty again.
                if (base.IsDetached)
                    return true;

                try
                {
                    var tag = ctrl.Tag;
                }
                catch (System.Runtime.InteropServices.InvalidComObjectException)
                {
                    IsDetached = true;
                    Dispose();
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    IsDetached = true;
                    Dispose();
                }
                catch (AccessViolationException)
                {
                    IsDetached = true;
                    Dispose();

                    System.Diagnostics.Debug.WriteLine("OfficeCommandBarButton.IsDetached -> Tag -> AccessViolation");
                }

                return base.IsDetached;
            }
            protected set
            {
                base.IsDetached = value;
            }
        }
 
    }
}
