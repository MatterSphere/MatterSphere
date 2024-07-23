using System;
using System.Collections.Generic;

namespace Fwbs.Office
{
    using System.Linq;
    using System.Runtime.InteropServices;
    using MSOffice = Microsoft.Office.Core;

    internal sealed class OfficeCommandBarControls : 
        OfficeObject,
        MSOffice.CommandBarControls
    {
        private static Dictionary<int, OfficeCommandBarControl> allcontrols = new Dictionary<int, OfficeCommandBarControl>();

        private readonly MSOffice.CommandBarControls controls;
        private Dictionary<int, OfficeCommandBarControl> items = new Dictionary<int, OfficeCommandBarControl>();
        private static System.Windows.Forms.Timer _timer;
        private static OfficeCommandBarButton currentbutton;
        private static object synch = new object();
        private OfficeCommandBar bar;
        private OfficeCommandBarPopup popup;

        internal OfficeCommandBarControls(OfficeCommandBarPopup parent, MSOffice.CommandBarControls controls)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (controls == null)
                throw new ArgumentNullException("controls");

            this.popup = parent;
            this.controls = controls;

            Init(controls);

        }

        internal OfficeCommandBarControls(OfficeCommandBar parent, MSOffice.CommandBarControls controls)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (controls == null)
                throw new ArgumentNullException("controls");

            this.bar = parent;
            this.controls = controls;

            Init(controls);
        }

        #region Overrides

        protected override void Init(object obj)
        {
            if (_timer == null)
            {
                _timer = new System.Windows.Forms.Timer();
                _timer.Enabled = false;
                _timer.Interval = 250;
                _timer.Tick += new EventHandler(timer_Tick);
            }

            base.Init(obj);

        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {

                    foreach (var ctrl in items.Values.ToArray())
                    {
                        ctrl.Delete(false);
                    }


                    items.Clear();

                    if (Fwbs.Framework.Stats.GetCount(this.GetType()) == 1)
                    {
                        if (_timer != null)
                        {
                            _timer.Tick -= new EventHandler(timer_Tick);
                            _timer.Dispose();
                            _timer = null;
                        }
                    }
                    
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

     
        #endregion


        #region Captured Events


        internal void OnButtonClick(OfficeCommandBarButton button)
        {
            lock (synch)
            {
                if (currentbutton != null && currentbutton.IsDetached)
                {
                    currentbutton = null;
                }

                if (currentbutton != null && currentbutton.Tag == button.Tag)
                    return;
                currentbutton = button;
                currentbutton.Enabled = false;
            }
            _timer.Enabled = true;
        }

        private static void timer_Tick(object sender, EventArgs e)
        {
            _timer.Enabled = false;


            try
            {
                var btn = currentbutton;
                btn.Enabled = false;
                System.Windows.Forms.Application.DoEvents();
                bool cancel = false;
                btn.OnClick(ref cancel);
            }
            catch (COMException)
            {
                //Just incase the command removed the buttons.
            }
            catch (InvalidComObjectException)
            {
            }
            finally
            {
                try
                {
                    lock (synch)
                    {
                        if (!currentbutton.IsDetached)
                        {
                            //Just incase the command removed the buttons.
                            currentbutton.Enabled = true;
                        }
                        currentbutton = null;
                    }
                }
                catch (COMException) { }
                catch (InvalidComObjectException) { }
            }
        }

        #endregion

        #region CommandBarControls Members

        public Microsoft.Office.Core.CommandBarControl Add(
            [Optional]object Type, [Optional]object Id, [Optional]object Parameter, [Optional]object Before, [Optional]object Temporary)
        {

            try
            {
                var ctrl = controls.Add(Type, Id, Parameter, Before, Temporary);
                return AddControl(ctrl);
            }
            catch
            {
                throw;
            }

            
        }

        public object Application
        {
            get { return controls.Application; }
        }

        public int Count
        {
            get { return items.Count; }
        }

        public int Creator
        {
            get { return controls.Creator; }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            foreach (var ctrl in items.Values)
            {
                yield return ctrl;
            }

            yield break;
            
        }

        public Microsoft.Office.Core.CommandBar Parent
        {
            get { return bar ?? popup.Parent; }
        }

        public Microsoft.Office.Core.CommandBarControl this[object Index]
        {
            get { return GetControl(controls[Index]); }
        }

        #endregion


        internal void RemoveControl(OfficeCommandBarControl ctrl)
        {
            if (items.ContainsKey(ctrl.InstanceId))
                items.Remove(ctrl.InstanceId);
            if (allcontrols.ContainsKey(ctrl.InstanceId))
                allcontrols.Remove(ctrl.InstanceId);
        }

        internal Microsoft.Office.Core.CommandBarControl GetControl(Microsoft.Office.Core.CommandBarControl ctrl)
        {
            if (ctrl == null)
                return null;

            OfficeCommandBarControl octrl;
            if (allcontrols.TryGetValue(ctrl.InstanceId, out octrl))
            {
                if (octrl.IsDetached)
                {
                    RemoveControl(octrl);
                    return AddControl(ctrl);
                }
            }
            return octrl;
           
        }

        private Microsoft.Office.Core.CommandBarControl AddControl(Microsoft.Office.Core.CommandBarControl ctrl)
        {

            OfficeCommandBarControl octrl;

            if (allcontrols.TryGetValue(ctrl.InstanceId, out octrl))
            {
                if (octrl.IsDetached)
                {
                    RemoveControl(octrl);
                }
                else
                    return octrl;
            }

            if (items.TryGetValue(ctrl.InstanceId, out octrl))
            {
                if (octrl.IsDetached)
                {
                    RemoveControl(octrl);
                }
                else
                    return octrl;
            }

            var btn = ctrl as MSOffice.CommandBarButton;
            if (btn != null)
            {
                var obtn = new OfficeCommandBarButton(this, btn);
                items.Add(btn.InstanceId, obtn);
                allcontrols.Add(btn.InstanceId, obtn);
                return obtn;
            }

            var popup = ctrl as MSOffice.CommandBarPopup;
            if (popup != null)
            {
                var opopup = new OfficeCommandBarPopup(this, popup);
                items.Add(popup.InstanceId, opopup);
                allcontrols.Add(popup.InstanceId, opopup);
                return opopup;
            }

            return ctrl;

        }
    }
}
