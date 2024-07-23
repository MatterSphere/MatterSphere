using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Script;
using swf = System.Windows.Forms;
namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucScripts.
    /// </summary>
    public class ucScripts : FWBS.OMS.UI.Windows.Admin.ucEditBase2, IOBjectDirty, IObjectUnlock
	{
		private string _type = "";
		private OMS.Design.CodeBuilder.CodeSurface codeSurface2;
		private ImageList imageList1;
		private SplitContainer splitContainer1;
		private ListView listView1;
		private ColumnHeader columnHeader1;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private ToolStrip toolStrip1;
		private ToolStripButton tbEditScript;
		private ToolStripButton tbCompile;
		private ToolStripButton tbStop;
        private ToolStripButton tbClose2;
		private IContainer components;
		private IEnumerable<IScriptProcessor> processors;
		private Dictionary<Guid, Tuple<IScriptProcessor, string, ProcessLink>> processorlinks = new Dictionary<Guid, Tuple<IScriptProcessor,string, ProcessLink>>();
        private LockState ls = new LockState();
        private IMainParent _mainparent;
        private string scriptcode;
        private bool objectlocking;

		public ucScripts(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params) : base(mainparent,editparent, Params)
		{
			if (frmMain.PartnerAccess == false)
				Session.CurrentSession.ValidateLicensedFor("SDKALL");
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			tpEdit.Text = FWBS.OMS.Session.CurrentSession.Resources.GetResource("SCRIPTGEN","Script Generator","").Text;
			tpList.Text = tpEdit.Text;
			labSelectedObject.Text = ResourceLookup.GetLookupText("CompileAll", "Compile All", "");
			lstList.SearchListLoad += new EventHandler(lstList_SearchListLoad);
			lstList.CommandExecuted += new CommandExecutedEventHandler(lstList_CommandExecuted);

			processors = Session.CurrentSession.Container.ResolveAll<IScriptProcessor>(null);

            _mainparent = mainparent;

            objectlocking = Session.CurrentSession.ObjectLocking;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}
                if (this != null)
                {
                    if (!string.IsNullOrWhiteSpace(scriptcode))
                    {
                        UnlockCurrentObject();
                    }
                }
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucScripts));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.codeSurface2 = new FWBS.OMS.Design.CodeBuilder.CodeSurface();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new FWBS.OMS.UI.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbEditScript = new System.Windows.Forms.ToolStripButton();
            this.tbCompile = new System.Windows.Forms.ToolStripButton();
            this.tbStop = new System.Windows.Forms.ToolStripButton();
            this.tbClose2 = new System.Windows.Forms.ToolStripButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpList
            // 
            this.tpList.Controls.Add(this.splitContainer1);
            this.tpList.Location = new System.Drawing.Point(56, 21);
            this.tpList.Controls.SetChildIndex(this.lstList, 0);
            this.tpList.Controls.SetChildIndex(this.splitContainer1, 0);
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.codeSurface2);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.codeSurface2, 0);
            // 
            // pnlEdit
            // 
            this.pnlEdit.Size = new System.Drawing.Size(549, 22);
            // 
            // labSelectedObject
            // 
            this.labSelectedObject.Location = new System.Drawing.Point(0, 0);
            this.labSelectedObject.Text = "Compile All";
            // 
            // tbSave
            // 
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            this.tbSave.Visible = false;
            // 
            // tbClose2
            // 
            this.BresourceLookup1.SetLookup(this.tbClose2, new FWBS.OMS.UI.Windows.ResourceLookupItem("Close", "Close", ""));
            // 
            // tbReturn
            // 
            this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
            this.tbReturn.Visible = true;
            // 
            // pnlToolbarContainer
            // 
            this.pnlToolbarContainer.Visible = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "services.ico");
            this.imageList1.Images.SetKeyName(1, "14.ICO");
            this.imageList1.Images.SetKeyName(2, "112_Tick_Green.ico");
            // 
            // codeSurface2
            // 
            this.codeSurface2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeSurface2.IsDirty = false;
            this.codeSurface2.Location = new System.Drawing.Point(0, 22);
            this.codeSurface2.Name = "codeSurface2";
            this.codeSurface2.Size = new System.Drawing.Size(549, 361);
            this.codeSurface2.TabIndex = 2;
            this.codeSurface2.IsCloseScriptWindowMenuItemVisible = true;
            this.codeSurface2.CloseScriptWindowMenuItem += new System.EventHandler(this.codeSurface2_CloseScriptWindowMenuItem);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(549, 383);
            this.splitContainer1.SplitterDistance = 221;
            this.splitContainer1.TabIndex = 201;
            this.splitContainer1.Visible = false;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 25);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(221, 358);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Script Name";
            this.columnHeader1.Width = 200;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbClose2,
            this.tbEditScript,
            this.tbCompile,
            this.tbStop});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(221, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbEditScript
            // 
            this.BresourceLookup1.SetLookup(this.tbEditScript, new FWBS.OMS.UI.Windows.ResourceLookupItem("tbEdit", "Edit", ""));
            this.tbEditScript.Name = "tbEditScript";
            this.tbEditScript.Size = new System.Drawing.Size(31, 22);
            this.tbEditScript.Text = "Edit";
            this.tbEditScript.Click += new System.EventHandler(this.tbEditScript_Click);
            // 
            // tbCompile
            // 
            this.tbCompile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbCompile.Name = "tbCompile";
            this.tbCompile.Size = new System.Drawing.Size(56, 22);
            this.tbCompile.Text = "Compile";
            this.tbCompile.Click += new System.EventHandler(this.tbCompile_Click);
            // 
            // tbStop
            // 
            this.tbStop.Enabled = false;
            this.tbStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbStop.Name = "tbStop";
            this.tbStop.Size = new System.Drawing.Size(35, 22);
            this.tbStop.Text = "Stop";
            this.tbStop.Click += new System.EventHandler(this.tbStop_Click);
            // 
            // tbClose2
            // 
            this.tbClose2.Enabled = true;
            this.tbClose2.Name = "tbClose";
            this.tbClose2.Size = new System.Drawing.Size(51, 22);
            this.tbClose2.Text = "Close";
            this.tbClose2.Click += new System.EventHandler(this.tbClose2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.HideSelection = false;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(324, 383);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // ucScripts
            // 
            this.Name = "ucScripts";
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

		}

        void codeSurface2_CloseScriptWindowMenuItem(object sender, EventArgs e)
        {
            CloseAndReturnToList();
        }

		#endregion

		public string Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
				ApplyButtonStates();
			}
		}

		void lstList_SearchListLoad(object sender, EventArgs e)
		{
			ApplyButtonStates();
		}

		private void ApplyButtonStates()
		{
			if (lstList != null && lstList.GetOMSToolBarButton("cmdAdd") != null)
				lstList.GetOMSToolBarButton("cmdAdd").Visible = true;
		}

		protected override string SearchListName
		{
			get
			{
				return "ADMSCRIPTS";
			}
		}


		protected override void NewData()
		{
            string code = FWBS.OMS.UI.Windows.InputBox.Show(Session.CurrentSession.Resources.GetMessage("ENTERCODE", "Enter a Script Code", "").Text);
			if (code != FWBS.OMS.UI.Windows.InputBox.CancelText)
			{
                if (FWBS.OMS.Script.ScriptGen.Exists(code))
                {
                    swf.MessageBox.Show(Session.CurrentSession.Resources.GetResource("CODEUNIQUE", "You must give your scripts unique codes. The code you have entered is already in use. Please change the code for this script.", "").Text, Session.CurrentSession.Resources.GetResource("SCRIPTCODE", "Script Code", "").Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    NewData();
                }
                else
                {
                    var _script = new FWBS.OMS.Script.ScriptGen(code, _type);
                    Initialise(_script);
                }
			}
		}

		public override bool IsDirty
		{
			get
			{
				return base.IsDirty || codeSurface2.IsDirty;
			}
			set
			{
				base.IsDirty = value;
				codeSurface2.IsDirty = value;
			}
		}

		protected override bool UpdateData()
		{
			try
			{
				codeSurface2.SaveAndCompile();
				return true;
			}
			catch
			{
				return false;
			}
		}

		protected override void ShowList()
		{
			if (splitContainer1.Visible && base.ListMode)
			{
				splitContainer1.Visible = false;
				lstList.Visible = true;
			}
			codeSurface2.Unload();
			base.ShowList();
		}

		private void Initialise(FWBS.OMS.Script.ScriptGen _script)
		{
			ScriptType _scripttype = CreateScriptType(_type);

			ShowEditor(false);
			Application.DoEvents();
			codeSurface2.Init(null);
			codeSurface2.Load(_scripttype, _script);
			labSelectedObject.Text = String.Format("{0} - {1}", ResourceLookup.GetLookupText("EditingScript", "Editing Script", ""), _script.Code);
            codeSurface2.Focus();
		}

		private ListViewItem listviewitem;

		void lstList_CommandExecuted(object sender, CommandExecutedEventArgs e)
		{
			switch (e.ButtonName.ToUpperInvariant())
			{
				case "CMDCOMPILEALL":
				case "CMDTESTCOMPILE":
                    tbClose2.Enabled = true;
					tbStop.Enabled = true;
					tbCompile.Enabled = false;
					tbEditScript.Enabled = false;
					listView1.Items.Clear();
					richTextBox1.Clear();
					richTextBox1.ClearUndo();
					labSelectedObject.Text = ResourceLookup.GetLookupText("CompileAll", "Compile All", "");
					splitContainer1.Visible = true;
					lstList.Visible = false;
					var dt = lstList.SearchList.DataTable.DefaultView;
					foreach (DataRowView row in dt)
					{
						string script = Convert.ToString(row["scrCode"]);
						listviewitem = new ListViewItem() { Text = script, ImageIndex = 0 };
						listView1.Items.Add(listviewitem);
					}
					tbCompile.Enabled = true;
					tbEditScript.Enabled = false;
                    if (listView1.Items.Count > 0)
                    {
                        listView1.Items[0].Selected = true;
                    }
                    break;
				default:
					break;
			}
		}

		private void Compile(string script)
		{
			try
			{
				using (FWBS.OMS.Script.ScriptGen _script = FWBS.OMS.Script.ScriptGen.GetScript(script))
				{
					try
					{
						_script.CompileOutput += new MessageEventHandler(_script_CompileOutput);
						_script.CompileError += new EventHandler(_script_CompileError);

						CompilerResults results;
						if (_script.Compile(true, false, out results))
						{
							listviewitem.ImageIndex = 2;
						}

						foreach (var item in processors)
						{
							try
							{
								var links = item.Compile(_script.Code, results);

								if (links == null || links.Count() == 0)
									continue;

								foreach (var link in links)
								{
									if (link == null)
										continue;

									var guid = Guid.NewGuid();

									richTextBox1.AppendText(Environment.NewLine);
									richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
									richTextBox1.AppendText(string.Format("{0} : ", link.Text));
									richTextBox1.AppendText(String.Format(" http://process?id={0}", guid));

									processorlinks.Add(guid, new Tuple<IScriptProcessor,string,ProcessLink>(item, _script.Code, link));
								}

							}
							catch (Exception ex)
							{
								richTextBox1.AppendText(Environment.NewLine);
								richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
								richTextBox1.AppendText(item.GetType().Name);
								richTextBox1.AppendText(Environment.NewLine);
								richTextBox1.AppendText(ex.Message);
							}
						}
						richTextBox1.AppendText(Environment.NewLine);
						richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
						richTextBox1.ScrollToCaret();
						Application.DoEvents();
					}
					finally
					{
						_script.CompileOutput -= new MessageEventHandler(_script_CompileOutput);
						_script.CompileError -= new EventHandler(_script_CompileError);
					}
				}
				richTextBox1.AppendText(Environment.NewLine);
				richTextBox1.AppendText(Environment.NewLine);
			}
			catch (Exception ex)
			{
				richTextBox1.AppendText(ex.Message);
				richTextBox1.AppendText(Environment.NewLine);
				richTextBox1.AppendText(Environment.NewLine);
				listviewitem.ImageIndex = 1;
			}
		}

		void _script_CompileOutput(object sender, MessageEventArgs e)
		{
			if (e.Message.StartsWith("----- Building started: Scriptlet"))
				richTextBox1.SelectionFont = new Font(richTextBox1.Font,FontStyle.Bold);
			richTextBox1.AppendText(e.Message);
			richTextBox1.AppendText(Environment.NewLine);
			richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
			richTextBox1.ScrollToCaret();
			Application.DoEvents();
		}

		void _script_CompileError(object sender, EventArgs e)
		{
			listviewitem.ImageIndex = 1;
		}

		public override bool ListMode
		{
			get
			{
				if (splitContainer1.Visible)
					return false;
				return base.ListMode;
			}
		}

		
		protected override void DeleteData(string Code)
		{
			FWBS.OMS.Script.ScriptGen.Delete(Code);
		}


		protected override void LoadSingleItem(string Code)
		{
            if (objectlocking)
            {
                if (!ls.CheckObjectLockState(Code, LockableObjects.Script))
                {
                    SetupScript(Code);
                    ls.LockScriptObject(Code);
                    ls.MarkObjectAsOpen(Code,LockableObjects.Script);
                }
            }
            else
                SetupScript(Code);
		}


        private void SetupScript(string Code)
        {
            FWBS.OMS.Script.ScriptGen _script = FWBS.OMS.Script.ScriptGen.GetScript(Code);
            Initialise(_script);
            scriptcode = Code;
        }


		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count != 1)
				return;

			string findtext = "Scriptlet: " + listView1.SelectedItems.First<ListViewItem>().Text;
			int pos = richTextBox1.Find(findtext);
			if (pos > -1)
			{
				richTextBox1.Select(pos, findtext.Length);
				richTextBox1.ScrollToCaret();
			}
		}

		private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			tbEditScript.Enabled = listView1.SelectedItems.Count == 1;
		}

		private void tbEditScript_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count == 1)
			{
				LoadSingleItem(listView1.SelectedItems.First<ListViewItem>().Text);
			}
		}


        private void tbCompile_Click(object sender, EventArgs e)
        {
            tbStop.Enabled = true;
            tbCompile.Enabled = false;
            tbEditScript.Enabled = false;
            richTextBox1.Clear();
            richTextBox1.ClearUndo();
            processorlinks.Clear();
            listView1.HideSelection = false;
            _stop = false;
            if (listView1.SelectedItems.Count == 0)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.ImageIndex < 2)
                    {
                        listviewitem = item;
                        listView1.TopItem = item;
                        item.Selected = true;
                        Compile(item.Text);
                        if (_stop)
                            break;
                        item.Selected = false;
                    }
                }
            }
            else
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    item.ImageIndex = 0;
                }
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    listviewitem = item;
                    Compile(item.Text);
                    if (_stop)
                        break;
                }
            }
            tbStop.Enabled = false;
            tbEditScript.Enabled = true;
            tbCompile.Enabled = true;
        }

		private bool _stop;


		private void listView1_DoubleClick(object sender, EventArgs e)
		{
			LoadSingleItem(listView1.SelectedItems.First<ListViewItem>().Text);
		}

		private void tbStop_Click(object sender, EventArgs e)
		{
			_stop = true;
		}

        private void tbClose2_Click(object sender, EventArgs e)
        {
            lstList.Visible = true;
            splitContainer1.Visible = false;
        }

		private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			foreach (var item in processors)
			{
				if (e.LinkText.IndexOf("process?id=") > -1)
				{
					try
					{
						var pos = e.LinkText.IndexOf("process?id=") + "process?id=".Length;
		  
						var id = Guid.Parse(e.LinkText.Substring(pos, e.LinkText.Length - pos));

						Tuple<IScriptProcessor, string, ProcessLink> link = processorlinks[id];

						if (link.Item1.Process(link.Item2, link.Item3))
						{
							var lpos = richTextBox1.Find(e.LinkText);
							richTextBox1.Rtf = richTextBox1.Rtf.Replace(e.LinkText, "Done");
							if (lpos > -1)
							{
								richTextBox1.Select(lpos, 1);
							}
							return;
						}
					}
					catch (Exception ex)
					{
						ErrorBox.Show(ParentForm, ex);
					}
				}
			}
			System.Diagnostics.Process.Start(e.LinkText);
		}


		private static ScriptType CreateScriptType(string type)
		{
			var t = Session.CurrentSession.TypeManager.Load(ScriptGen.GetScriptTypeName(type));
			return (ScriptType)Activator.CreateInstance(t);
		}

        // ************************************************************************************************
        //
        // CLOSE
        //
        // ************************************************************************************************

        protected override void CloseAndReturnToList()
        {
            if (this.codeSurface2.IsDirty)
            {
                DialogResult? dr = base.IsObjectDirtyDialogResult();
                if (dr == DialogResult.Cancel)
                {
                    this.codeSurface2.Focus();
                    return;
                }
            }

            UnlockCurrentObject();
            ShowList();
        }

        public void UnlockCurrentObject()
        {
            string code = "";
            if (!string.IsNullOrWhiteSpace(scriptcode))
                code = scriptcode;
            else if (codeSurface2.ScriptGen != null && !string.IsNullOrWhiteSpace(codeSurface2.ScriptGen.Code))
                code = codeSurface2.ScriptGen.Code;

            if (objectlocking && !string.IsNullOrWhiteSpace(code))
            {
                ls.MarkObjectAsClosed(code, LockableObjects.Script);
                ls.UnlockScriptObject(code);
            }            
        }
    }


}
