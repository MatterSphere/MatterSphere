using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A static global class used for accessing common operations like resource / icon extraction
    /// and right to left form parsing etc...
    /// </summary>
    public static class Global
    {


        /// <summary>
        /// Log switch for the configured trace.
        /// </summary>
        internal static TraceSwitch LogSwitch = new TraceSwitch("WINUI", "OMS Windows UI Services Layer");

        /// <summary>
        /// Uses the terminology parser to change any visual parameters.
        /// </summary>
        /// <param name="frm">Form to be parsed.</param>
        static internal void ControlParser(Form frm)
        {
            if (Session.CurrentSession != null)
            {
                try
                {
                    frm.Text = Session.CurrentSession.Terminology.Parse(frm.Text, true);
                    foreach (Control ctrl in frm.Controls)
                    {
                        ControlParser(ctrl);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Adds the specified control to the control collection.
        /// </summary>
        /// <param name="controls">The control collection.</param>
        /// <param name="control">The System.Windows.Forms.Control to add to the control collection.</param>
        /// <param name="scale">Boolean value indicating whether the control should be scaled or not.</param>
        public static void Add(this Control.ControlCollection controls, Control control, bool scale)
        {
            int deviceDpi = controls.Owner.DeviceDpi;
            if (scale && deviceDpi != 96)
                control.Scale(new System.Drawing.SizeF(deviceDpi / 96F, deviceDpi / 96F));

            controls.Add(control);
        }

        public static void Add(this TableLayoutControlCollection controls, Control control, int column, int row, bool scale)
        {
            int deviceDpi = controls.Owner.DeviceDpi;
            if (scale && deviceDpi != 96)
                control.Scale(new System.Drawing.SizeF(deviceDpi / 96F, deviceDpi / 96F));

            controls.Add(control, column, row);
        }

        public static bool IsDesignerHosted(this Control ctrl)
        {

            Control c = ctrl;

            while (c != null)
            {
                if ((c.Site != null) && c.Site.DesignMode)
                    return true;
                c = ctrl.Parent;
            }
            return false;
        }

        public static bool IsInDesignMode() 
        { 
            return System.Reflection.Assembly.GetExecutingAssembly().Location.Contains("VisualStudio"); 
        }

        public static void RemoveAndDisposeControls(Control control)
        {
            if (control != null && control.HasChildren)
            {
                var controls = control.Controls;
                do
                {   // dispose child control and remove it from parent
                    controls[0].Dispose();
                } while (controls.Count > 0);
            }
        }

        /// <summary>
        /// Loops round all potential containers and parses the terminology aspect of the child controls.
        /// </summary>
        /// <param name="ctrl">Control to be checked for child controls.</param>
        static internal void ControlParser(Control ctrl)
        {
            if (Session.OMS != null)
            {
                try
                {
                    ctrl.Text = Session.CurrentSession.Terminology.Parse(ctrl.Text, true);
                    foreach (Control child in ctrl.Controls)
                    {
                        ControlParser(child);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Finds the Parent Form of any Control
        /// </summary>
        /// <param name="Ctrl">The Control to Find the Parent Form</param>
        /// <returns>The Parent Form</returns>
        static public Form GetParentForm(Control Ctrl)
        {
            Control _parent = Ctrl;
            try
            {
                do
                {
                    _parent = _parent.Parent;
                }
                while (_parent is Form == false);
            }
            catch
            {
                _parent = null;
            }
            return _parent as Form;
        }

        /// <summary>
        /// Converts a form from left to right as best it can.
        /// </summary>
        /// <param name="frm">Passed form object.</param>
        static public void RightToLeftFormConverter(Form frm)
        {
            if (frm.RightToLeft == RightToLeft.No)
                return;

            var parentform = frm as BaseForm;
            try
            {
                frm.SuspendLayout();
                var isrtl = frm as ISupportRightToLeft;
                if (isrtl != null)
                {
                    isrtl.SetRTL(frm);
                    return;
                }

                foreach (Control ctrl in frm.Controls)
                {
                    foreach (Control cctrl in ctrl.Controls)
                    {
                        RightToLeftControlConverter(cctrl, parentform);
                    }
                }
            }
            finally
            {
                frm.ResumeLayout();
            }
        }

        public static void ConvertToolBarRTL(System.Windows.Forms.ToolBar toolbar)
        {
            foreach (var item in toolbar.Buttons.ToArray<ToolBarButton>())
            {
                if (item.ImageIndex == 17 && Convert.ToString(toolbar.ImageList.Tag).Contains("Cool Buttons"))
                    item.ImageIndex = 18;
                toolbar.Buttons.Remove(item);
                toolbar.Buttons.Insert(0, item);
            }
        }



        /// <summary>
        /// Loops round all potential containers and does the right to left conversion.
        /// </summary>
        /// <param name="ctrl">Control to be checked for child controls.</param>
        static public void RightToLeftControlConverter(Control ctrl, Form frm)
        {
            if (ctrl.RightToLeft != RightToLeft.Yes)
                return;

            var parentform = frm as BaseForm;

            if (parentform != null)
            {
                if (parentform.IsControlRightToLeft(ctrl))
                    return;
            }

            AnchorStyles anchor = ctrl.Anchor;
            DockStyle docking = ctrl.Dock;
            if (docking == DockStyle.Right)
                ctrl.Dock = DockStyle.Left;
            else if (docking == DockStyle.Left)
                ctrl.Dock = DockStyle.Right;
            else if (docking == DockStyle.Top || docking == DockStyle.Bottom || docking == DockStyle.Fill)
            {
            }
            else if ((ctrl.Anchor | AnchorStyles.Right | AnchorStyles.Left) != ctrl.Anchor)
            {
                if ((ctrl.Anchor | AnchorStyles.Right) == ctrl.Anchor)
                {
                    anchor -= AnchorStyles.Right;
                    anchor |= AnchorStyles.Left;
                }

                if ((ctrl.Anchor | AnchorStyles.Left) == ctrl.Anchor)
                {
                    anchor -= AnchorStyles.Left;
                    anchor |= AnchorStyles.Right;
                }
                ctrl.Anchor = anchor;
            }
            if (ctrl.Parent != null && ctrl.Dock == DockStyle.None)
                ctrl.Location = new System.Drawing.Point(ctrl.Parent.ClientSize.Width - ctrl.Left - ctrl.Width, ctrl.Top);

            if (parentform != null)
            {
                parentform.AddControlToRTL(ctrl);
            }


            var srtl = ctrl as ISupportRightToLeft;
            if (srtl != null)
            {
                srtl.SetRTL(parentform);
                return;
            }
            if (ctrl.Controls.Count > 0)
            {
                foreach (Control child in ctrl.Controls)
                {
                    RightToLeftControlConverter(child, parentform);
                }
            }
        }

        public static void SetRichTextBoxRightToLeft(System.Windows.Forms.RichTextBox richTextBox)
        {
            if (richTextBox == null)
                throw new ArgumentNullException("richTextBox");

            if (richTextBox.RightToLeft != RightToLeft.Yes)
                return;

            var indexofltrparObject = richTextBox.Rtf.IndexOf(@"\ltrpar", System.StringComparison.Ordinal);
            if (indexofltrparObject != -1)
                richTextBox.Rtf = richTextBox.Rtf.Insert(indexofltrparObject, @"\qr");
        }


        /// <summary>
        /// Gets a picture object from within the resource file.
        /// </summary>
        /// <param name="resID">Resource ID name.</param>
        /// <returns>A picture object.</returns>
        public static object GetResPicture(string resID)
        {
            System.Resources.ResourceManager rm = null;
            object bmp = null;
            rm = new System.Resources.ResourceManager("FWBS.OMS.UI.Resources.omswinui", System.Reflection.Assembly.GetExecutingAssembly());
            bmp = rm.GetObject(resID);
            return bmp;
        }

        /// <summary>
        /// Create Type from Type Name
        /// </summary>
        /// <param name="TypeName">The String Type Name</param>
        /// <returns>The Type</returns>
        public static Type CreateType(string TypeName, string AssemblyName)
        {
            try
            {
                Type type = null;
                type = Session.CurrentSession.TypeManager.Load(TypeName);
                return type;
            }
            catch (Exception ex)
            {

                if (Session.CurrentSession.IsInDebug)
                {
                    try
                    {
                        EventLog evt = new System.Diagnostics.EventLog();
                        // Create the source, if it does not already exist.
                        if (!EventLog.SourceExists("OMSDOTNET"))
                            EventLog.CreateEventSource("OMSDOTNET", "Application");

                        // Create an EventLog instance and assign its source.
                        EventLog myLog = new EventLog();
                        myLog.Source = "OMSDOTNET";
                        myLog.WriteEntry("CreateType - Type : " + TypeName + ", AssemblyName : " + AssemblyName + ", " + ex.Message + Environment.NewLine + "-----------------------------------------------------------------------------------------------" + ex.StackTrace, EventLogEntryType.Error);
                    }
                    catch { }
                }
                throw ex;
            }
        }

    }



}


