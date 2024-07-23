using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using FWBS.OMS.Script;

namespace FWBS.OMS.Design.CodeBuilder
{
    /// <summary>
    /// Summary description for DataGridEvents.
    /// </summary>
    public class DataGridEvents : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private FWBS.OMS.UI.Windows.DataGridEx dgEvents;
		private System.Windows.Forms.DataGridTextBoxColumn dgcMethods;
        private FWBS.OMS.Design.CodeBuilder.DataGridEventColumn dgcVisibleCode;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
        private TextBox txtFilter;
		private System.ComponentModel.Container components = null;

        [Category("OMS")]
        public event EventHandler NewScript;

        public void OnNewScript()
        {
            if (NewScript != null)
                NewScript(this, EventArgs.Empty);
        }

        [Category("OMS")]
		public event EventHandler CodeButtonClick;

		public void OnCodeButtonClick()
		{
            var method = _filtered[dgEvents.CurrentRowIndex];
            var methodname = "";
            if (string.IsNullOrEmpty(method.Delegate))
                methodname = method.Name;
            else
                methodname = string.Format("{0}_{1}", method.Object, method.Name);
            methodname = methodname.Replace("*", "");
            var workflowname = CurrentCodeSurface.ScriptGen.GetWorkflow(methodname);

            if (!String.IsNullOrEmpty(workflowname))
            {
                var result = FWBS.OMS.UI.Windows.MessageBox.ShowYesNoCancel("COVWF2SCRIPT1", "You have chosen to create a Script Method.\n\nIf you want to create a stub to execute the workflow '%1%'. Click Yes otherwise, click No for a blank event.",workflowname);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.No:
                        break;
                    case DialogResult.Yes:
                        CurrentCodeSurface.GenerateHandler(methodname, new GenerateHandlerInfo() { DelegateType = method.Delegate, Workflow = workflowname });
                        break;
                    default:
                        break;
                }
                CurrentCodeSurface.ScriptGen.ClearWorkFlow(methodname);
            }
            
            if (CodeButtonClick != null)
				CodeButtonClick(this,EventArgs.Empty);
		}



		public DataGridEvents()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			dgcVisibleCode.ButtonClick +=new EventHandler(dgcVisibleCode_ButtonClick);
            dgcVisibleCode.WorkflowDeleted += new EventHandler(WorkflowDeleted);
            dgcVisibleCode.WorkflowButtonClick += new EventHandler(WorkFlowSelector);
		}


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.dgEvents = new FWBS.OMS.UI.Windows.DataGridEx();
            this.dgcMethods = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcVisibleCode = new FWBS.OMS.Design.CodeBuilder.DataGridEventColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.ColumnHeadersVisible = false;
            this.dataGridTableStyle1.DataGrid = this.dgEvents;
            this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dgcMethods,
            this.dgcVisibleCode});
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyle1.MappingName = "Methods";
            this.dataGridTableStyle1.PreferredColumnWidth = 100;
            this.dataGridTableStyle1.ReadOnly = true;
            this.dataGridTableStyle1.RowHeadersVisible = false;
            // 
            // dgEvents
            // 
            this.dgEvents.AllowNavigation = false;
            this.dgEvents.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgEvents.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgEvents.CaptionBackColor = System.Drawing.SystemColors.ActiveBorder;
            this.dgEvents.CaptionForeColor = System.Drawing.SystemColors.ControlDark;
            this.dgEvents.CaptionText = "Events";
            this.dgEvents.CaptionVisible = false;
            this.dgEvents.ColumnHeadersVisible = false;
            this.dgEvents.DataMember = "";
            this.dgEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgEvents.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dgEvents.LinkColor = System.Drawing.SystemColors.ControlDark;
            this.dgEvents.Location = new System.Drawing.Point(15, 38);
            this.dgEvents.Name = "dgEvents";
            this.dgEvents.ParentRowsVisible = false;
            this.dgEvents.PreferredRowHeight = 21;
            this.dgEvents.ReadOnly = true;
            this.dgEvents.RowHeadersVisible = false;
            this.dgEvents.Size = new System.Drawing.Size(210, 287);
            this.dgEvents.TabIndex = 1;
            this.dgEvents.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            this.dgEvents.TabStop = false;
            this.dgEvents.Resize += new System.EventHandler(this.dgEvents_Resize);
            // 
            // dgcMethods
            // 
            this.dgcMethods.Format = "";
            this.dgcMethods.FormatInfo = null;
            this.dgcMethods.HeaderText = "Events";
            this.dgcMethods.MappingName = "Name";
            this.dgcMethods.ReadOnly = true;
            this.dgcMethods.Width = 95;
            // 
            // dgcVisibleCode
            // 
            this.dgcVisibleCode.MappingName = "Code";
            this.dgcVisibleCode.NullText = "";
            this.dgcVisibleCode.ReadOnly = true;
            this.dgcVisibleCode.Width = 95;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(1, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(14, 304);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(15, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Events";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFilter
            // 
            this.txtFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtFilter.Location = new System.Drawing.Point(1, 1);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(224, 20);
            this.txtFilter.TabIndex = 6;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // DataGridEvents
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.dgEvents);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtFilter);
            this.Name = "DataGridEvents";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(226, 326);
            ((System.ComponentModel.ISupportInitialize)(this.dgEvents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void dgcVisibleCode_ButtonClick(object sender, EventArgs e)
		{
			OnCodeButtonClick();
            GetObjectEventMethods();
        }

        private void WorkflowDeleted(object sender, EventArgs e)
        {
            if (CurrentCodeSurface == null)
                return;

            var method = _filtered[dgEvents.CurrentRowIndex];
            var methodname = "";
            if (string.IsNullOrEmpty(method.Delegate))
                methodname = method.Name;
            else
                methodname = string.Format("{0}_{1}", method.Object, method.Name);
            methodname = methodname.Replace("*", "");
            var dele = method.Delegate;
            method.Code = null;


            if (!String.IsNullOrEmpty(CurrentCodeSurface.ScriptGen.GetWorkflow(methodname.Replace("*","")))) 
            {
                CurrentCodeSurface.ScriptGen.ClearWorkFlow(methodname);
            }
        }


        private void WorkFlowSelector(object sender, EventArgs e)
        {
            try
            {
                if (CurrentCodeSurface == null)
                    throw new ArgumentNullException("CurrentScriptGen cannot be null");

                FWBS.Common.KeyValueCollection retvals = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(null, Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.WorkflowPicker), false, new Size(-1, -1), null, new FWBS.Common.KeyValueCollection());
                if (retvals != null)
                {
                    var workflowname = Convert.ToString(retvals[0].Value);
                    var method = _filtered[dgEvents.CurrentRowIndex];



                    var methodname = "";
                    if (string.IsNullOrEmpty(method.Delegate))
                        methodname = method.Name;
                    else
                        methodname = string.Format("{0}_{1}", method.Object, method.Name);
                    methodname = methodname.Replace("*", "");

                    if (CurrentCodeSurface.HasMethod(methodname))
                    {
                        //conrad's suggestion...
                        if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("WYRSMWWF1", "Warning you are about replace a scripted method with a workflow. You will need to remove the existing method from your current script. Do you wish to continue?") == DialogResult.No)
                            return;
                    }

                    var dele = method.Delegate;
                    method.Code = workflowname;
                    
                    CurrentCodeSurface.ScriptGen.SetWorkFlow(methodname, workflowname, dele);
                    if (string.IsNullOrEmpty(CurrentCodeSurface.ScriptGen.Code))
                        OnNewScript();
                    GetObjectEventMethods();
                    
                }
            }
            catch (Exception ex)
            {
                UI.Windows.ErrorBox.Show(ParentForm, ex);
            }
        }

		private void dgEvents_Resize(object sender, System.EventArgs e)
		{
			int w = dgEvents.Width;
            if (dgEvents.VerticalScrollBar.Visible)
                w -= dgEvents.VerticalScrollBar.Width;
			dgcMethods.Width = (w/2);
			dgcVisibleCode.Width = (w/2);
		}

        private object _selectedObject;

        [Category("OMS")]
        [DefaultValue(null)]
        public object SelectedObject
        {
            get
            {
                return _selectedObject;
            }
            set
            {
                if (_selectedObject != value)
                {
                        _selectedObject = value;
                    GetObjectEventMethods();
                }
            }
        }

        public EventMethodData SelectedMethodData
        {
            get
            {
                if (_filtered == null)
                    return null;
                else
                    return _filtered[dgEvents.CurrentRowIndex];
            }
        }

        public string SelectedMethodName
        {
            get
            {
                if (_filtered == null)
                    return null;
                else
                {
                    var item = _filtered[dgEvents.CurrentRowIndex];
                    if (string.IsNullOrEmpty(item.Delegate))
                        return item.Name.Replace("*", "");
                     else
                        return string.Format("{0}_{1}", item.Object, item.Name.Replace("*", ""));
                }
            }
        }

        private void GetObjectEventMethods()
        {
            var result = GetControlMethods(_selectedObject, CurrentCodeSurface);
            if (result == null)
                result = GetScriptTypeMethods(_selectedObject, CurrentCodeSurface);
            if (result == null)
                return;
            var sorted = result.OrderBy(n => n.Category).ThenBy(n => n.Name);
            result = new Methods(sorted);
            var index = dgEvents.CurrentRowIndex;
            this.DataSource = result;
            if (index > 0)
                dgEvents.CurrentRowIndex = index;
        }


        private static Methods GetScriptTypeMethods(object value, ICodeSurface current)
        {
            if (value == null)
                return null;
            if (current == null) throw new ArgumentNullException("current");
            ScriptType scripttype = value as ScriptType;
            System.CodeDom.CodeMemberMethod[] meths = scripttype.Methods;
            var _methods = new Methods();
            foreach (var item in meths)
            {
                var methodname = item.Name;
                var dgemd = new EventMethodData();

                dgemd.Name = methodname;

                var workflow = current.ScriptGen.GetWorkflow(methodname);
                if (!string.IsNullOrEmpty(workflow))
                {
                    if (!string.IsNullOrEmpty(workflow))
                    {
                        dgemd.Code = workflow;
                    }
                }
                else if (current.HasMethod(dgemd.Name))
                {
                    dgemd.Code = "[Event Procedure]";
                    dgemd.Name = methodname + "*";
                }

                dgemd.Category = "General";
                dgemd.Object = "{General}";
                dgemd.Type = "{General}";
                _methods.Add(dgemd);
            }
            return _methods;
        }



        private static Methods GetControlMethods(object value, ICodeSurface current)
        {
            if (value == null)
                return null;
            if (current == null) throw new ArgumentNullException("current");
            Control ctrl = value as Control;
            string ctrlname = value.GetType().Name;
            if (ctrl != null)
                ctrlname = ctrl.Name;
            else
                return null;


            var _methods = new Methods();
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
                        var methodname = string.Format("{0}_{1}", ctrlname, dgemd.Name);
                        var workflow = current.ScriptGen.GetWorkflow(methodname);
                        if (!string.IsNullOrEmpty(workflow))
                        {
                            if (!string.IsNullOrEmpty(workflow))
                            {
                                dgemd.Code = workflow;
                            }
                        }
                        else if (current.HasMethod(methodname))
                        {
                            dgemd.Code = "[Event Procedure]";
                            dgemd.Name = dgemd.Name + "*";
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

        private Methods _methods;
        private Methods _filtered;

        private Methods DataSource
        {
            get
            {
                return _methods;
            }
            set
            {
                _methods = value;
                _filtered = _methods;
                ApplyFilter();
            }
        }

        private void RefreshGrid()
        {
            dgEvents.BeginInit();
            dgEvents.DataSource = _filtered;
            dgEvents.EndInit();
            dgEvents_Resize(this, EventArgs.Empty);
        }

        [Category("OMS")]
        [DefaultValue(null)]
        public ICodeSurface CurrentCodeSurface
        {
            get
            {
                return _currentcodesurface;
            }
            set
            {
                _currentcodesurface = value;
                if (_currentcodesurface != null)
                {
                    if (_currentscriptobject != null)
                        _currentscriptobject.DataChanged -=new EventHandler(_currentscriptobject_DataChanged);

                    _currentscriptobject = _currentcodesurface.ScriptGen;

                    if (_currentscriptobject != null)
                        _currentscriptobject.DataChanged += new EventHandler(_currentscriptobject_DataChanged);
                }
            }
        }

        void _currentscriptobject_DataChanged(object sender, EventArgs e)
        {
            GetObjectEventMethods();
        }

        private ICodeSurface _currentcodesurface;
        private ScriptGen _currentscriptobject;

		[Browsable(false)]
		public int CurrentRowIndex
		{
			get
			{
				return dgEvents.CurrentRowIndex;
			}
		}

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
                _filtered = _methods;
            else
            {
                var _search = _methods.Where(n => n.Name != null && n.Name.ToUpperInvariant().Contains(txtFilter.Text.ToUpperInvariant()));
                _filtered = new Methods(_search);
            }
            RefreshGrid();
        }
	}

}
