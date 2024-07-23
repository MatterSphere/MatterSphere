using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using FWBS.OMS.Script;
using FWBS.OMS.UI;

namespace FWBS.OMS.Design.CodeBuilder
{
    internal partial class CodeSurfaceV1 : UserControl, ICodeSurface, ICodeSurfaceControls
    {

        #region Fields

        private FWBS.OMS.Script.ScriptType _currentscripttype;
        private FWBS.OMS.Script.ScriptGen _currentscriptobject;
        private DataTable _objects = new DataTable("Objects");
        private bool _noupdate = false;
        private bool _isdirty = false;
        private string findtext = "";

        #endregion

        #region IServiceProvider

        new public object GetService(Type serviceType)
        {
            if (serviceType == typeof(ICodeSurface))
                return this;
            else if (serviceType == typeof(ICodeSurfaceControls))
                return this;

            return base.GetService(serviceType);
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        #endregion

        public CodeSurfaceV1()
        {
            InitializeComponent();
        }

        private Form _parentform;
        public void Init(Form parentForm)
        {
            _objects.Columns.Add("Icon", typeof(Int16));
            DataColumn nm = _objects.Columns.Add("Name", typeof(String));
            DataColumn ctrl = _objects.Columns.Add("Control", typeof(Control));
            _objects.PrimaryKey = new DataColumn[1] { nm };

            cmbObjects.BeginUpdate();
            cmbObjects.DataSource = _objects;
            cmbObjects.ValueMember = "Name";
            cmbObjects.DisplayMember = "Name";
            cmbObjects.EndUpdate();

            isinits = true;
            Application.DoEvents();
            _parentform = parentForm;

        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_parentform != null)
                {
                    _parentform.MainMenuStrip = null;
                    _parentform = null;
                }
            }
            base.Dispose(disposing);
        }


        private void GetObjectEventMethods()
        {
            if (cmbObjects.SelectedItem == null)
                return;
            List<EventMethodData> result;
            var ctrl = ((DataRowView)cmbObjects.SelectedItem)["Control"] as Control;
            if (ctrl == null)
            {
                if (cmbObjects.SelectedIndex == 0)
                {
                    result = GetScriptTypeMethods(this);
                }
                else
                {
                    result = GetStaticMethods(this);
                }
            }
            else
                result = GetControlMethods(ctrl, this);

            var sorted = result.OrderBy(n => n.Category).ThenBy(n => n.Name);
            result = new List<EventMethodData>(sorted);

            cmbEvents.BeginUpdate();
            cmbEvents.DataSource = result;
            cmbEvents.ValueMember = "Name";
            cmbEvents.DisplayMember = "Description";
            cmbEvents.EndUpdate();
        }


        private static List<EventMethodData> GetScriptTypeMethods(ICodeSurface current)
        {
            
            System.CodeDom.CodeMemberMethod[] meths = current.ScriptType.Methods;
            var _methods = new List<EventMethodData>();
            foreach (var item in meths)
            {
                var methodname = item.Name;
                var dgemd = new EventMethodData();

                dgemd.Name = methodname;
                dgemd.Description = methodname;
                if (current.HasMethod(dgemd.Name))
                    dgemd.Code = "[Event Procedure]";
                else
                {
                    var workflow = current.ScriptGen.GetWorkflow(methodname);
                    if (!string.IsNullOrEmpty(workflow))
                    {
                        dgemd.Code = workflow;
                    }
                }

                if (!string.IsNullOrEmpty(dgemd.Code))
                {
                    dgemd.Description = methodname + "*";
                }

                dgemd.Category = "General";
                dgemd.Object = "{General}";
                dgemd.Type = "{General}";
                _methods.Add(dgemd);
            }
            return _methods;
        }

        private static List<EventMethodData> GetStaticMethods(ICodeSurface current)
        {

            var meths = current.ScriptGen.GetStaticProcedures();
            var _methods = new List<EventMethodData>();
            foreach (var item in meths)
            {
                var methodname = item.GetString("name","");
                var dgemd = new EventMethodData();

                dgemd.Name = methodname;
                dgemd.Description = methodname;
                dgemd.Category = "Private";
                dgemd.Object = "{Private}";
                dgemd.Type = "{Private}";
                _methods.Add(dgemd);
            }
            return _methods;
        }


        private static List<EventMethodData> GetControlMethods(object value, ICodeSurface current)
        {
            Control ctrl = value as Control;
            string ctrlname = value.GetType().Name;
            if (ctrl != null)
                ctrlname = ctrl.Name;
            else
                return null;


            var _methods = new List<EventMethodData>();
            EventInfo[] events = value.GetType().GetEvents();
            string catagory = "";
            foreach (EventInfo ev in events)
            {
                bool b = true;
                var browserattrib = ev.Attributes<BrowsableAttribute>().FirstOrDefault();
                if (browserattrib != null)
                    b = browserattrib.Browsable;
                if (b)
                {
                    var catagoryattrib = ev.Attributes<CategoryAttribute>().FirstOrDefault();
                    if (catagoryattrib != null) catagory = catagoryattrib.Category;

                    if (catagory != "Property Changed")
                    {
                        var dgemd = new EventMethodData();
                        dgemd.Name = ev.Name;
                        dgemd.Description = ev.Name;
                        if (current.HasMethod(string.Format("{0}_{1}", ctrlname, dgemd.Name)))
                            dgemd.Code = "[Event Procedure]";
                        else
                        {
                            var workflow = current.ScriptGen.GetWorkflow(dgemd.Name);
                            if (!string.IsNullOrEmpty(workflow))
                            {
                                dgemd.Code = workflow;
                            }
                        }

                        if (!string.IsNullOrEmpty(dgemd.Code))
                        {
                            dgemd.Description = ev.Name + "*";
                        }
                        dgemd.Object = ctrlname;
                        dgemd.Delegate = ev.EventHandlerType.AssemblyQualifiedName;
						
                        dgemd.Category = catagory;
                        dgemd.Type = value.GetType().Name;
                        _methods.Add(dgemd);
                    }
                }
            }
            return _methods;
        }


        private void MergeMenus()
        {
            if (_parentform != null && _parentform.MdiParent != null && _parentform.MdiParent.MainMenuStrip != null)
            {
                IMenuMerge menumerge = _parentform.MdiParent as IMenuMerge;
                if (menumerge != null)
                {
                    _parentform.MainMenuStrip = this.menuStrip1;
                    menumerge.MergeMenus(menuStrip1);
                    this.menuStrip1.Visible = false;
                }
            }
        }

        private void UnMergeMenus()
        {
            if (_parentform != null && _parentform.MdiParent != null && _parentform.MdiParent.MainMenuStrip != null)
            {
                IMenuMerge menumerge = _parentform.MdiParent as IMenuMerge;
                if (menumerge != null)
                {
                    menumerge.UnMergeMenus(menuStrip1);
                    _parentform.MainMenuStrip = null;
                    this.menuStrip1.Visible = true;
                }
            }
        }

        private bool isinits;
        public bool IsInitialized
        {
            get
            {
                return isinits;
            }
        }

        public void Unload()
        {
            UnMergeMenus();
        }


        new public void Load(FWBS.OMS.Script.ScriptType scriptletType, FWBS.OMS.Script.ScriptGen scriptlet)
        {
            if (scriptletType == null)
                throw new ArgumentNullException("scriptletType");

            if (scriptletType == null)
                throw new ArgumentNullException("scriptlet");

            ((ICodeSurfaceControls)this).Clear();
            if (ScriptGen != null)
            {
                ScriptGen.CompileStart -= new EventHandler(Script_CompileStart);
                ScriptGen.CompileFinished -= new EventHandler(_currentscriptobject_CompileFinished);
                ScriptGen.CompileError -= new EventHandler(_currentscriptobject_CompileError);
                ScriptGen.CompileOutput -= new MessageEventHandler(Script_CompileOutput);
            }

            _currentscripttype = scriptletType;
            _currentscriptobject = scriptlet;
              scriptlet.CompileStart += new EventHandler(Script_CompileStart);
            scriptlet.CompileFinished += new EventHandler(_currentscriptobject_CompileFinished);
            scriptlet.CompileError += new EventHandler(_currentscriptobject_CompileError);
            scriptlet.CompileOutput += new MessageEventHandler(Script_CompileOutput);
            switch (scriptlet.Language)
            {
                case FWBS.OMS.Script.ScriptLanguage.CSharp:
                    lblLanguage.Text = "Language : C#";
                    break;
                case FWBS.OMS.Script.ScriptLanguage.VB:
                    lblLanguage.Text = "Language : VB";
                    break;
                default:
                    break;
            }
  
            if (scriptlet.Scriptlet != null)
                propertyGrid1.SelectedObject = _currentscriptobject.Scriptlet.CurrentObj;

            if (cmbObjects.Items.Count > 0)
                cmbObjects.SelectedIndex = 0;
            else
                cmbObjects.SelectedIndex = -1;
            _noupdate = true;
            rcCode.Text = String.Empty;
            _noupdate = false;
            if (cmbEvents.Items.Count > 0)
                cmbEvents.SelectedIndex = 0;
            else
                cmbEvents.SelectedIndex = -1;
            IsDirty = false;
            MergeMenus();
            GetObjectEventMethods();

        }

        public bool SupportsLanguage(FWBS.OMS.Script.ScriptLanguage language)
        {
            return true;
        }

        public bool HasMethod(string name)
        {
            var result = ScriptGen.GetDynamicProcedureCode(name);
            if (string.IsNullOrEmpty(result))
                result = ScriptGen.GetProcedureCode(name);
            if (string.IsNullOrEmpty(result))
                result = ScriptGen.GetStaticProcedureCode(name);
            return !String.IsNullOrEmpty(result);
        }

        public void GenerateHandler(string name, GenerateHandlerInfo info)
        {
            if (_noupdate) return;

            if (String.IsNullOrWhiteSpace(info.DelegateType))
            {
                var ctrlname = FWBS.OMS.UI.Windows.Script.EnquiryFormScriptType.ParseEventMethod(name);
                cmbObjects.SelectedValue = ctrlname.Item1;
                cmbEvents.SelectedValue = ctrlname.Item2;
            }
            else
            {
                cmbObjects.SelectedIndex = 0;
                cmbEvents.SelectedValue = name;
            }

        }

        public void GotoMethod(string name)
        {
            if (_noupdate) return;
            var ctrlname = FWBS.OMS.UI.Windows.Script.EnquiryFormScriptType.ParseEventMethod(name);
            if (ctrlname != null)
            {
                cmbObjects.SelectedValue = ctrlname.Item1;
                cmbEvents.SelectedValue = ctrlname.Item2;
            }
            else
            {
                cmbObjects.SelectedIndex = 0;
                cmbEvents.SelectedValue = name;
            }
        }


        public bool IsDirty
        {
            get
            {
                return _isdirty && ScriptGen.IsDirty;
            }
            set
            {
                _isdirty = value;
                if (_isdirty)
                {
                    statusBar1.Text = "Modified...";
                }
                else
                {
                    statusBar1.Text = "Ready";
                }
            }
        }

        public bool SaveAndCompile()
        {
            var result = _currentscriptobject.Compile(true);
            if (result)
            {
                _currentscriptobject.Update();

                IsDirty = false;
            }
            return result;
        }

        public void Show(bool compileToFile)
        {
            mnuCompileToFile.Checked = compileToFile;

            this.Visible = true;
        }

        public string[] GetMethods()
        {
            List<string> meths = new List<string>();
            foreach (var meth in GetAllMethods())
            {
                meths.Add(meth);
            }

            return meths.ToArray();
        }

        #region Methods

        private IEnumerable<string> GetAllMethods()
        {

            System.CodeDom.CodeMemberMethod[] meths = ScriptType.Methods;
            foreach (var meth in meths)
                yield return meth.Name;

            FWBS.Common.ConfigSettingItem[] dynamprocs = ScriptGen.GetUnAssignedDynamicProcedures();

            for (int i = 0; i <= dynamprocs.Length - 1; i++)
            {
                string cname = dynamprocs[i].GetString("name", "");
                if (!String.IsNullOrEmpty(cname))
                    yield return cname;
            }

        }
        
        private int FindMyText(string searchText, int searchStart, int searchEnd)
        {
            // Initialize the return value to false by default.
            int returnValue = -1;

            // Ensure that a search string and a valid starting point are specified.
            if (searchText.Length > 0 && searchStart >= 0)
            {
                // Ensure that a valid ending value is provided.
                if (searchEnd > searchStart || searchEnd == -1)
                {
                    // Obtain the location of the search string in richTextBox1.
                    int indexToText = rcCode.Find(searchText, searchStart, searchEnd, RichTextBoxFinds.None);
                    // Determine whether the text was found in richTextBox1.
                    if (indexToText >= 0)
                    {
                        // Return the index to the specified search text.
                        returnValue = indexToText;
                    }
                }
            }

            return returnValue;
        }


        private static string RemoveInvalidChars(string input)
        {
            string output = input;
            string test = @"!""£$%^&*()+=|\/<>.,;'[]{}:;@~#` ";
            for (int i = 0; i < test.Length; i++)
                output = output.Replace(test.Substring(i, 1), "");
            return output;
        }


        void ICodeSurfaceControls.Clear()
        {
            _objects.Rows.Clear();
            _objects.RejectChanges();
            DataRow rv = _objects.NewRow();
            rv["Name"] = "{General}";
            _objects.Rows.Add(rv);
            rv = _objects.NewRow();
            rv["Name"] = "{Private}";
            _objects.Rows.Add(rv);
        }

        void ICodeSurfaceControls.Detach(string Name)
        {
            DataView found = new DataView(_objects);
            found.RowFilter = "Name = '" + Name + "'";
            foreach (DataRowView frw in found)
                frw.Delete();

       }

        void ICodeSurfaceControls.Attach(string name)
        {
            if (name == null)
                return;

            DataRow rv = _objects.NewRow();
            rv["Name"] = name;
            try
            {
                _objects.Rows.Add(rv);
            }
            catch { }
        }

        private void indentselection2()
        {
            if (rcCode.SelectionLength == 0)
                return;
            string soep = "";
            int start = rcCode.SelectionStart;
            int end = start + rcCode.SelectionLength;
            soep = rcCode.SelectedText;
            int loc = 0;
            soep = soep.Insert(0, "\t");
            while (loc != -1)
            {
                loc = soep.IndexOf("\n", loc);
                if (loc > 0)
                {
                    if (loc + 1 >= soep.Length)
                        break;
                    soep = soep.Insert(loc + 1, "\t");
                    loc = loc + 1;
                }
            }
            rcCode.Cut();
            rcCode.Text = rcCode.Text.Insert(start, soep);
            rcCode.SelectionStart = start;
            rcCode.SelectionLength = soep.Length;
        }

        private void unindentselection2()
        {
            if (rcCode.SelectionLength == 0)
                return;
            string soep = "";
            int start = rcCode.SelectionStart;
            int end = start + rcCode.SelectionLength;
            soep = rcCode.SelectedText;
            int loc = 0;
            int loc2 = 0;
            int loc3 = 0;
            loc2 = soep.IndexOf("\n", loc);
            loc3 = soep.IndexOf("\t", loc);
            if (loc3 < loc2 || loc2 == -1)
            {
                if (loc3 > -1)
                {
                    soep = soep.Remove(loc3, 1);
                }
            }
            loc = 0;
            bool exitloop = false;
            while (exitloop == false)
            {
                loc = soep.IndexOf("\n", loc);
                loc2 = soep.IndexOf("\n", loc + 1);
                if (loc < 0)
                    exitloop = true;
                if (loc > -1)
                {
                    loc3 = soep.IndexOf("\t", loc);
                    if ((loc3 < loc2 || loc2 == -1))
                    {
                        if (loc3 > 0)
                        {
                            soep = soep.Remove(loc3, 1);
                        }
                    }
                    loc = loc + 1;
                    //
                    if (loc >= soep.Length)
                        exitloop = true;
                }
            }
            rcCode.Cut();
            rcCode.Text = rcCode.Text.Insert(start, soep);
            rcCode.SelectionStart = start;
            rcCode.SelectionLength = soep.Length;
        }
        #endregion

        #region Properties

        public FWBS.OMS.Script.ScriptType ScriptType
        {
            get
            {
                return _currentscripttype;
            }
        }

        public FWBS.OMS.Script.ScriptGen ScriptGen
        {
            get
            {
                return _currentscriptobject;
            }

            set
            {
                if (value != _currentscriptobject)
                    _currentscriptobject = value;
            }
        }

        #endregion

        #region Captured Events

        private void CloseView(object sender, EventArgs e)
        {
            pnlOutput.Visible = false;
            spOutput.Visible = false;
        }

        private void Script_CompileOutput(object sender, MessageEventArgs e)
        {
            txtOutput.Items.Add(e.Message);
            txtOutput.SelectedIndex = txtOutput.Items.Count - 1;
            Application.DoEvents();
        }

        /// <summary>
        /// The Event that Updates the Script XML Field
        /// </summary>
        /// <param name="sender">the rcCode Textbox</param>
        /// <param name="e">Empty</param>
        private void rcCode_TextChanged(object sender, System.EventArgs e)
        {
            if (pnlEvents.Enabled && pnlObjects.Enabled)
            {
                if (_noupdate == false && cmbObjects.SelectedIndex != -1 && cmbEvents.SelectedIndex != -1)
                {
                    EventMethodData method = (EventMethodData)cmbEvents.SelectedItem;
                    if (cmbObjects.SelectedIndex <= 1 && cmbObjects.SelectedIndex > -1)
                    {
                        if (method.Category == "Private")
                        {
                            ScriptGen.SetStaticProcedureCode(Convert.ToString(cmbEvents.SelectedValue), rcCode.Text);
                        }
                        else if (method.Category == "Unlinked")
                            ScriptGen.SetDynamicProcedureCode(Convert.ToString(cmbEvents.SelectedValue), rcCode.Text, method.Delegate);
                        else
                            ScriptGen.SetProcedureCode(Convert.ToString(cmbEvents.SelectedValue), rcCode.Text);
                    }
                    else
                    {
                        ScriptGen.SetDynamicProcedureCode(cmbObjects.Text + "_" + method.Name,rcCode.Text,method.Delegate);
                    }
                    if (!string.IsNullOrEmpty(rcCode.Text))
                    {
                        if (string.IsNullOrEmpty(method.Code))
                        {
                            UpdateEventsCombo();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(method.Code))
                        {
                            UpdateEventsCombo();
                        }
                    }
                    IsDirty = true;
                }
            }
            else
            {
                try
                {
                    ScriptGen.RawXML = rcCode.Text;
                }
                catch (Exception ex)
                {
                    FWBS.OMS.UI.Windows.ErrorBox.Show(ParentForm, ex);
                }
            }

        }

        private void UpdateEventsCombo()
        {
            cmbEvents.SelectedIndexChanged -=new EventHandler(cmbEvents_SelectedIndexChanged);
            try
            {
                var index = cmbEvents.SelectedIndex;
                GetObjectEventMethods();
                if (index < cmbEvents.Items.Count)
                    cmbEvents.SelectedIndex = index;
            }
            finally
            {
                cmbEvents.SelectedIndexChanged += new EventHandler(cmbEvents_SelectedIndexChanged);
            }
        }
        


        /// <summary>
        /// Add an General Method to the Script
        /// </summary>
        /// <param name="sender">The Menu</param>
        /// <param name="e">Emptu</param>
        private void mnuGeneralMethod_Click(object sender, System.EventArgs e)
        {
            if (cmbObjects.Items.Count > 1)
            {
                string strname = FWBS.OMS.UI.Windows.InputBox.Show(Session.CurrentSession.Resources.GetMessage("PENOM", "Please enter the name of the Method", "").Text, "", "", 50);
                strname = RemoveInvalidChars(strname);
                if (strname == FWBS.OMS.UI.Windows.InputBox.CancelText)
                    return;

                ScriptGen.SetStaticProcedureCode(strname, " ");
                cmbObjects.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// Resizes the Distance equaly between The Object Combo and the Events Combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnlTop_Resize(object sender, System.EventArgs e)
        {
            pnlTop.Refresh();
            pnlObjects.Width = pnlTop.Width / 2;
            pnlEvents.Width = pnlTop.Width / 2;
        }

        /// <summary>
        /// Shows the Compiler Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Script_CompileStart(object sender, EventArgs e)
        {
            spOutput.Visible = true;
            pnlOutput.Visible = true;
            txtOutput.Items.Clear();
            statusBar1.Text = "Compiling ...";
        }


        private void mnuSaveCompile_Click(object sender, System.EventArgs e)
        {
            try
            {
                rcCode_TextChanged(sender, e);
                SaveAndCompile();
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ParentForm, ex);
            }
        }

        public void cmbObjects_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            GetObjectEventMethods();
        }


        private void frmCodeBuilder_VisibleChanged(object sender, System.EventArgs e)
        {
            if (this.Visible == true && cmbObjects.Items.Count > 0 && cmbObjects.SelectedIndex == -1) cmbObjects.SelectedIndex = 0;
        }

        private void _currentscriptobject_CompileError(object sender, EventArgs e)
        {
            statusBar1.Text = "Build Errors...";
        }

        private void _currentscriptobject_CompileFinished(object sender, EventArgs e)
        {
            statusBar1.Text = "Build Successful...";
        }


        private void mnuReferences_Click(object sender, EventArgs e)
        {
            using (var frm = new frmCodeBuilder_References())
            {
                frm.References = ScriptGen.GetReferences();

                if (frm.ShowDialog(this) != DialogResult.OK)
                    return;

                ScriptGen.SetReferences(frm.References);
            }

        }


        private void mnuFindNext_Click(object sender, System.EventArgs e)
        {
            FindMyText(findtext, rcCode.SelectionStart + 1, -1);


        }

        private void pnlSnipitSlider_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Gray, pnlSnipitSlider.Width - 2, 1, pnlSnipitSlider.Width - 2, pnlSnipitSlider.Height - 2);
            e.Graphics.DrawString("Code Samples - Click Here", pnlSnipitSlider.Font, SystemBrushes.WindowText, new Point(2, 10), new StringFormat(StringFormatFlags.DirectionVertical));
        }

        private void pnlSnipitSlider_Click(object sender, EventArgs e)
        {
            if (ucSearchControl1.SearchList == null)
            {
                try
                {
                    ucSearchControl1.SetSearchListType("SCHCODESNIPS", null, new FWBS.Common.KeyValueCollection());
                    ucSearchControl1.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(ucSearchControl1_SearchButtonCommands);
                }
                catch
                { }
            }

            if (splitMain.SplitterDistance == 25)
                splitMain.SplitterDistance = 350;
            else
                splitMain.SplitterDistance = 25;
        }



        private void rcCode_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Shift == false && rcCode.SelectedText.Length > 0)
            {
                indentselection2();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Tab && e.Shift == true && rcCode.SelectedText.Length > 0)
            {
                unindentselection2();
                e.Handled = true;
            }
        }

        private void rcCode_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)9 && rcCode.SelectedText.Length > 0) e.Handled = true;
        }

        private void mnuUndo_Click(object sender, System.EventArgs e)
        {
            rcCode.Undo();
        }


        private void mnuFind_Click(object sender, System.EventArgs e)
        {
            findtext = FWBS.OMS.UI.Windows.InputBox.Show(Session.CurrentSession.Resources.GetMessage("TEXTFIND", "Enter text to find", "").Text);
            FindMyText(findtext, 0, -1);
            mnuFindNext.Enabled = true;
        }

       

        private void mnuWizardForm_Click(object sender, System.EventArgs e)
        {
            FWBS.OMS.UI.Windows.OpenSaveEnquiry ops = new FWBS.OMS.UI.Windows.OpenSaveEnquiry();
            if (ops.Execute() == DialogResult.OK)
            {
                rcCode.AppendText(@"object result = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(""" + ops.Code + @""",null,FWBS.OMS.EnquiryEngine.EnquiryMode.Add,new FWBS.Common.KeyValueCollection());");
            }
        }

        private void mnuDeleteProcedure_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show(Session.CurrentSession.Resources.GetMessage("PROCDEL", "Are you sure you wish to Delete this Procedure?", "").Text, Session.CurrentSession.Resources.GetResource("OMSCODEBLD", "OMS Code Builder", "").Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ScriptGen.RemoveStaticProcedureCode(Convert.ToString(cmbEvents.SelectedValue));
                ScriptGen.RemoveDynamicProcedureCode(cmbObjects.Text + "_" + Convert.ToString(cmbEvents.SelectedValue));
                ScriptGen.RemoveProcedureCode(Convert.ToString(cmbEvents.SelectedValue));
                cmbEvents_SelectedIndexChanged(sender, e);
            }
        }

        private void mnuRawView_Click(object sender, System.EventArgs e)
        {
            mnuRawView.Checked = !mnuRawView.Checked;
            pnlObjects.Enabled = !mnuRawView.Checked;
            pnlEvents.Enabled = !mnuRawView.Checked;
            if (mnuRawView.Checked)
            {
                if (MessageBox.Show(Session.CurrentSession.Resources.GetMessage("SCROBJERR", "WARNING DEBUG USE ONLY. After any changes. Please close and reopen the Scripted Object. Do you wish to continue?", "").Text, FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    rcCode.Text = ScriptGen.RawXML;
                }
                else
                {
                    mnuRawView.Checked = !mnuRawView.Checked;
                    pnlObjects.Enabled = !mnuRawView.Checked;
                    pnlEvents.Enabled = !mnuRawView.Checked;
                }
            }
            else
            {
                cmbEvents_SelectedIndexChanged(sender, e);
            }
        }

        private void mnuGetControl_Click(object sender, System.EventArgs e)
        {
            frmCodeBuilder_ControlPicker picker = new frmCodeBuilder_ControlPicker();
            picker.cmbControls.Items.Clear();
            for (int i = 2; i < _objects.Rows.Count; i++)
            {
                picker.cmbControls.Items.Add(Convert.ToString(_objects.Rows[i]["Name"]));
            }
            picker.cmbControls.Sorted = true;
            picker.ShowDialog(this);
            if (picker.DialogResult == DialogResult.OK)
            {
                rcCode.Focus();
                SendKeys.SendWait(@"EnquiryForm.GetControl{(}""" + picker.cmbControls.Text + @""",EnquiryControlMissing." + picker.cmbIfMissing.Text.Split("-".ToCharArray())[0].Trim() + @"{)}");
            }
        }

        private void rcCode_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (cmbEvents.SelectedIndex == -1)
                e.Handled = true;
        }

        private void mnuGetIBasicEnquiryControl_Click(object sender, System.EventArgs e)
        {
            frmCodeBuilder_ControlPicker picker = new frmCodeBuilder_ControlPicker();
            picker.cmbControls.Items.Clear();
            for (int i = 2; i < _objects.Rows.Count; i++)
            {
                picker.cmbControls.Items.Add(Convert.ToString(_objects.Rows[i]["Name"]));
            }
            picker.cmbControls.Sorted = true;
            picker.ShowDialog(this);
            if (picker.DialogResult == DialogResult.OK)
            {
                rcCode.Focus();
                SendKeys.SendWait(@"EnquiryForm.GetIBasicEnquiryControl2{(}""" + picker.cmbControls.Text + @""",EnquiryControlMissing." + picker.cmbIfMissing.Text.Split("-".ToCharArray())[0].Trim() + @"{)}");
            }

        }

        private void mnuCut_Click(object sender, System.EventArgs e)
        {
            rcCode.Cut();
        }

        private void mnuCopy_Click(object sender, System.EventArgs e)
        {
            rcCode.Copy();
        }

        private void mnuPaste_Click(object sender, System.EventArgs e)
        {
            rcCode.Paste();
        }

        private void mnuIndent_Click(object sender, System.EventArgs e)
        {
            indentselection2();
        }



        private void mnuOutdent_Click(object sender, System.EventArgs e)
        {
            unindentselection2();
        }

        private void mnuDrillDown_Click(object sender, System.EventArgs e)
        {
            try
            {
                propertyGrid1.SelectedObject = propertyGrid1.SelectedGridItem.Value;
            }
            catch
            {
                MessageBox.Show(Session.CurrentSession.Resources.GetMessage("ERRORSHOW", "Error", "").Text);
            }
        }


        private void ucSearchControl1_SearchButtonCommands(object sender, FWBS.OMS.UI.Windows.SearchButtonEventArgs e)
        {
            string char10 = ((char)10).ToString();
            if (e.ButtonName == "cmdAdd")
            {
                FWBS.Common.KeyValueCollection keys = new FWBS.Common.KeyValueCollection();
                string unique = "";
                System.Text.StringBuilder key = new System.Text.StringBuilder();

                //UTCFIX: DM - 30/11/06 - Could had been effected if two time zones created at same local time.
                DateTime k = DateTime.UtcNow;
                key.Append(k.Year);
                key.Append(k.Month);
                key.Append(k.Day);
                key.Append(k.Hour);
                key.Append(k.Minute);
                key.Append(k.Second);
                unique = key.ToString();
                keys.Add("CODE", unique);
                string code = rcCode.SelectedText.Replace(char10, Environment.NewLine);
                if (code.Length > 1000)
                {
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("CODESMPL1000", "Code Sample is larger then 1000 characters. Please note trimming to fit.");
                    code = code.Substring(0, 999);
                }
                keys.Add("THECODE", code);
                ucSearchControl1.SetParameters(keys);
            }
            else if (e.ButtonName == "cmdInsert")
            {
                rcCode.Focus();
                string send = Convert.ToString(ucSearchControl1.SelectedItems[0]["cdDesc"].Value);
                send = send.Replace(char10, "");
                send = send.Replace("{", "ÿ{ÿ");
                send = send.Replace("}", "ÿ}ÿ");
                send = send.Replace("(", "{(}");
                send = send.Replace(")", "{)}");
                send = send.Replace("+", "{+}");
                send = send.Replace("^", "{^}");
                send = send.Replace("%", "{%}");
                send = send.Replace("~", "{~}");
                send = send.Replace("[", "{[}");
                send = send.Replace("]", "{]}");
                send = send.Replace("ÿ{ÿ", "{{}");
                send = send.Replace("ÿ}ÿ", "{}}");
                SendKeys.Send(send);
            }
        }

        private void mnuCompileToFile_Click(object sender, EventArgs e)
        {
            FWBS.Common.Reg.ApplicationSetting compileRegKey = new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "CompileToFile");
            compileRegKey.SetSetting(!mnuCompileToFile.Checked);
            mnuCompileToFile.Checked = !mnuCompileToFile.Checked;
        }


        #endregion

        private void mnuConvertToAdvanced_Click(object sender, EventArgs e)
        {
            if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("ADVSCRUPGR", "Are you sure you wish to Upgrade to Advanced Scripting any changes made in Advanced Scripting will not be applied to the old Scripting System. This current script will be the last version for the older clients to work with and converted one way only.") == DialogResult.Yes)
            {
                ScriptGen.ConvertToAdvanced();
            }
        }

        private void cmbEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            _noupdate = true;
            try
            {
                if (cmbObjects.SelectedIndex == 0)
                {
                    rcCode.Text = ScriptGen.GetProcedureCode(Convert.ToString(cmbEvents.SelectedValue)).Trim();
                }
                else if (cmbObjects.SelectedIndex == 1)
                {
                    rcCode.Text = ScriptGen.GetStaticProcedureCode(Convert.ToString(cmbEvents.SelectedValue)).Trim();
                }
                else
                {
                    rcCode.Text = ScriptGen.GetDynamicProcedureCode(string.Format("{0}_{1}", cmbObjects.Text, cmbEvents.SelectedValue)).Trim();
                }
            }
            finally
            {
                _noupdate = false;
            }
        }


        public event EventHandler CloseScriptWindowMenuItem;

        public bool IsCloseScriptWindowMenuItemVisible
        {
            get;
            set;
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.menuStrip1.EnableHotKeys = true;
        }

        protected override void OnLeave(EventArgs e)
        {
            this.menuStrip1.EnableHotKeys = false;
            base.OnLeave(e);
        }
    }

}
