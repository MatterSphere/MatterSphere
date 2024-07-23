using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement.DocumentFolderControls;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// This control will display a list of sub document from a document object.
    /// </summary>
    public class ucSubDocument : UserControl
	{
		#region Fields


		bool isExpanded = false;
		Form _ptparent = null;
		System.Windows.Forms.Splitter sp1 = null;
		ucTreeView tv = null;

		private ListView _list;

		private Label lbl = null;

		private ResourceLookup _res;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// An enquiry form that the control may be sitting on.
		/// </summary>
		private EnquiryForm _enqForm = null;

		/// <summary>
		/// An OMS document that the sub documents are list from.
		/// </summary>
		private OMSDocument _document = null;
		private ColumnHeader colName;
		private ContextMenuStrip mnuOptions;
		private ToolStripMenuItem mnuDetach;
		private ToolStripMenuItem mnuDocFolder;
		private ToolStripMenuItem mnuAttach;
		private ToolStripMenuItem mnuLatest;
		private ImageList smallicons;
		private ImageList largeicons;
		private ColumnHeader colDocId;
		private ColumnHeader colLatest;
		private ColumnHeader colDocDesc;
		private ColumnHeader colDocFolder;
		private ToolStripMenuItem mnuResolve;
		private ColumnHeader colFile;
		private ToolStrip toolStrip1;
		private ToolStripButton btnDetach;
		private ToolStripButton btnAttach;
		private ToolStripButton btnLatest;
		private ToolStripButton btnResolve;
		private ToolStripButton btnSelectAll;
		private ToolStripButton btnDeselectAll;
		private ToolStripButton btnDocFolder;
		private ToolStripSeparator sepDetach;
		private ToolStripSeparator sepAttach;
		private ToolStripSeparator sepResolve;
		private Label lblWarning;
		private ToolStripMenuItem mnuRename;

		/// <summary>
		/// The page name that this control lies on.
		/// </summary>
		private string _pgeName = "";
		private ToolStripButton btnChangeDocDesc;

		private bool folderChanged = false;
		ucTreeViewArgs args;

        #endregion

        #region Constructors 

        public ucSubDocument()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            // Set code lookups
            Res res = Session.CurrentSession.Resources;
            colName.Text = res.GetResource("NAME", "Name", "").Text;
            colDocId.Text = res.GetResource("DOCREF", "Document Ref", "").Text;
            colLatest.Text = res.GetResource("LATEST", "Latest", "").Text;
            colDocDesc.Text = res.GetResource("ID", "Description", "").Text;
            colDocFolder.Text = res.GetResource("FOLDER", "Folder", "").Text;
            colFile.Text = res.GetResource("DETAILS", "Details", "").Text;
            btnAttach.Text = btnAttach.Text;
            mnuAttach.Text = mnuAttach.Text;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSubDocument));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Attached", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Detached - Save as New Document", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Attach - Save and Overwrite", System.Windows.Forms.HorizontalAlignment.Left);
            this.mnuOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuDetach = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAttach = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLatest = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuResolve = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRename = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDocFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.largeicons = new System.Windows.Forms.ImageList(this.components);
            this.smallicons = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSelectAll = new System.Windows.Forms.ToolStripButton();
            this.btnDeselectAll = new System.Windows.Forms.ToolStripButton();
            this.sepDetach = new System.Windows.Forms.ToolStripSeparator();
            this.btnDetach = new System.Windows.Forms.ToolStripButton();
            this.sepAttach = new System.Windows.Forms.ToolStripSeparator();
            this.btnAttach = new System.Windows.Forms.ToolStripButton();
            this.btnLatest = new System.Windows.Forms.ToolStripButton();
            this.sepResolve = new System.Windows.Forms.ToolStripSeparator();
            this.btnResolve = new System.Windows.Forms.ToolStripButton();
            this.btnChangeDocDesc = new System.Windows.Forms.ToolStripButton();
            this.btnDocFolder = new System.Windows.Forms.ToolStripButton();
            this.lblWarning = new System.Windows.Forms.Label();
            this._list = new FWBS.OMS.UI.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDocId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLatest = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDocDesc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDocFolder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._res = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.mnuOptions.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuOptions
            // 
            this.mnuOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDetach,
            this.mnuAttach,
            this.mnuLatest,
            this.mnuResolve,
            this.mnuRename,
            this.mnuDocFolder});
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.Size = new System.Drawing.Size(205, 114);
            this.mnuOptions.Opening += new System.ComponentModel.CancelEventHandler(this.mnuOptions_Opening);
            // 
            // mnuDetach
            // 
            this._res.SetLookup(this.mnuDetach, new FWBS.OMS.UI.Windows.ResourceLookupItem("SAVEASNEWDOCUM", "Save As New Document", ""));
            this.mnuDetach.Name = "mnuDetach";
            this.mnuDetach.Size = new System.Drawing.Size(204, 22);
            this.mnuDetach.Text = "Save As New Document";
            this.mnuDetach.Click += new System.EventHandler(this.MenuClick);
            // 
            // mnuAttach
            // 
            this._res.SetLookup(this.mnuAttach, new FWBS.OMS.UI.Windows.ResourceLookupItem("ATTACHTOVERSION", "Attach To Version", ""));
            this.mnuAttach.Name = "mnuAttach";
            this.mnuAttach.Size = new System.Drawing.Size(204, 22);
            this.mnuAttach.Text = "Attach To Version";
            this.mnuAttach.Click += new System.EventHandler(this.MenuClick);
            // 
            // mnuLatest
            // 
            this._res.SetLookup(this.mnuLatest, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNLATEST", "Flag as Latest", ""));
            this.mnuLatest.Name = "mnuLatest";
            this.mnuLatest.Size = new System.Drawing.Size(204, 22);
            this.mnuLatest.Text = "Flag as Latest";
            this.mnuLatest.Click += new System.EventHandler(this.MenuClick);
            // 
            // mnuResolve
            // 
            this._res.SetLookup(this.mnuResolve, new FWBS.OMS.UI.Windows.ResourceLookupItem("RESOLVE...", "Resolve...", ""));
            this.mnuResolve.Name = "mnuResolve";
            this.mnuResolve.Size = new System.Drawing.Size(204, 22);
            this.mnuResolve.Text = "Resolve...";
            this.mnuResolve.Click += new System.EventHandler(this.MenuClick);
            // 
            // mnuRename
            // 
            this._res.SetLookup(this.mnuRename, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCHNGDOCDESC", "Change Name", "Change the name for the selected document"));
            this.mnuRename.Name = "mnuRename";
            this.mnuRename.Size = new System.Drawing.Size(204, 22);
            this.mnuRename.Text = "Change Name";
            this.mnuRename.Click += new System.EventHandler(this.mnuRename_Click);
            // 
            // mnuDocFolder
            // 
            this._res.SetLookup(this.mnuDocFolder, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNDOCFOLDER", "Assign Document Folder", ""));
            this.mnuDocFolder.Name = "mnuDocFolder";
            this.mnuDocFolder.Size = new System.Drawing.Size(204, 22);
            this.mnuDocFolder.Text = "Assign Document Folder";
            this.mnuDocFolder.Click += new System.EventHandler(this.mnuDocFolder_Click);
            // 
            // largeicons
            // 
            this.largeicons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.largeicons.ImageSize = new System.Drawing.Size(32, 32);
            this.largeicons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // smallicons
            // 
            this.smallicons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.smallicons.ImageSize = new System.Drawing.Size(16, 16);
            this.smallicons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSelectAll,
            this.btnDeselectAll,
            this.sepDetach,
            this.btnDetach,
            this.sepAttach,
            this.btnAttach,
            this.btnLatest,
            this.sepResolve,
            this.btnResolve,
            this.btnChangeDocDesc,
            this.btnDocFolder});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(660, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectAll.Image")));
            this.btnSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._res.SetLookup(this.btnSelectAll, new FWBS.OMS.UI.Windows.ResourceLookupItem("SELECTALL", "Select All", ""));
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(59, 22);
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDeselectAll.Image = ((System.Drawing.Image)(resources.GetObject("btnDeselectAll.Image")));
            this.btnDeselectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._res.SetLookup(this.btnDeselectAll, new FWBS.OMS.UI.Windows.ResourceLookupItem("DESELECTALL", "Deselect All", ""));
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(72, 22);
            this.btnDeselectAll.Text = "Deselect All";
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselect_Click);
            // 
            // sepDetach
            // 
            this.sepDetach.Name = "sepDetach";
            this.sepDetach.Size = new System.Drawing.Size(6, 25);
            this.sepDetach.Visible = false;
            // 
            // btnDetach
            // 
            this.btnDetach.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDetach.Image = ((System.Drawing.Image)(resources.GetObject("btnDetach.Image")));
            this.btnDetach.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._res.SetLookup(this.btnDetach, new FWBS.OMS.UI.Windows.ResourceLookupItem("SAVEASNEWDOCUM", "Save As New Document", ""));
            this.btnDetach.Name = "btnDetach";
            this.btnDetach.Size = new System.Drawing.Size(137, 22);
            this.btnDetach.Text = "Save As New Document";
            this.btnDetach.Click += new System.EventHandler(this.MenuClick);
            // 
            // sepAttach
            // 
            this.sepAttach.Name = "sepAttach";
            this.sepAttach.Size = new System.Drawing.Size(6, 25);
            this.sepAttach.Visible = false;
            // 
            // btnAttach
            // 
            this.btnAttach.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAttach.Image = ((System.Drawing.Image)(resources.GetObject("btnAttach.Image")));
            this.btnAttach.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._res.SetLookup(this.btnAttach, new FWBS.OMS.UI.Windows.ResourceLookupItem("ATTACHTOVERSION", "Attach To Version", ""));
            this.btnAttach.Name = "btnAttach";
            this.btnAttach.Size = new System.Drawing.Size(130, 22);
            this.btnAttach.Text = "Attach To Version";
            this.btnAttach.Click += new System.EventHandler(this.MenuClick);
            // 
            // btnLatest
            // 
            this.btnLatest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLatest.Image = ((System.Drawing.Image)(resources.GetObject("btnLatest.Image")));
            this.btnLatest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._res.SetLookup(this.btnLatest, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNLATEST", "Flag as Latest", ""));
            this.btnLatest.Name = "btnLatest";
            this.btnLatest.Size = new System.Drawing.Size(81, 22);
            this.btnLatest.Text = "Flag as Latest";
            this.btnLatest.Click += new System.EventHandler(this.MenuClick);
            // 
            // sepResolve
            // 
            this.sepResolve.Name = "sepResolve";
            this.sepResolve.Size = new System.Drawing.Size(6, 25);
            this.sepResolve.Visible = false;
            // 
            // btnResolve
            // 
            this.btnResolve.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnResolve.Image = ((System.Drawing.Image)(resources.GetObject("btnResolve.Image")));
            this.btnResolve.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._res.SetLookup(this.btnResolve, new FWBS.OMS.UI.Windows.ResourceLookupItem("RESOLVE...", "Resolve...", ""));
            this.btnResolve.Name = "btnResolve";
            this.btnResolve.Size = new System.Drawing.Size(60, 22);
            this.btnResolve.Text = "Resolve...";
            this.btnResolve.Click += new System.EventHandler(this.MenuClick);
            // 
            // btnChangeDocDesc
            // 
            this.btnChangeDocDesc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnChangeDocDesc.Image = ((System.Drawing.Image)(resources.GetObject("btnChangeDocDesc.Image")));
            this.btnChangeDocDesc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._res.SetLookup(this.btnChangeDocDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCHNGDOCDESC", "Change Name", "Change the name for the selected document"));
            this.btnChangeDocDesc.Name = "btnChangeDocDesc";
            this.btnChangeDocDesc.Size = new System.Drawing.Size(87, 22);
            this.btnChangeDocDesc.Text = "Change Name";
            this.btnChangeDocDesc.Click += new System.EventHandler(this.MenuClick);
            // 
            // btnDocFolder
            // 
            this.btnDocFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDocFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._res.SetLookup(this.btnDocFolder, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNDOCFOLDER", "Assign Document Folder", ""));
            this.btnDocFolder.Name = "btnDocFolder";
            this.btnDocFolder.Size = new System.Drawing.Size(141, 19);
            this.btnDocFolder.Text = "Assign Document Folder";
            this.btnDocFolder.Click += new System.EventHandler(this.btnDocFolder_Click);
            // 
            // lblWarning
            // 
            this.lblWarning.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblWarning.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblWarning.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblWarning.ForeColor = System.Drawing.Color.Red;
            this.lblWarning.Location = new System.Drawing.Point(0, 177);
            this._res.SetLookup(this.lblWarning, new FWBS.OMS.UI.Windows.ResourceLookupItem("SUBDOCATTACHWRN", "Warning - There are documents attached to existing documents under a different %F" +
            "ILE%", ""));
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(660, 35);
            this.lblWarning.TabIndex = 6;
            this.lblWarning.Text = "Warning - There are documents attached to existing documents under a different %F" +
    "ILE%";
            this.lblWarning.Visible = false;
            // 
            // _list
            // 
            this._list.CheckBoxes = true;
            this._list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colDocId,
            this.colLatest,
            this.colDocDesc,
            this.colDocFolder,
            this.colFile});
            this._list.ContextMenuStrip = this.mnuOptions;
            this._list.Dock = System.Windows.Forms.DockStyle.Fill;
            this._list.FullRowSelect = true;
            listViewGroup1.Header = "Attached";
            listViewGroup1.Name = "grpAttachVersion";
            listViewGroup2.Header = "Detached - Save as New Document";
            listViewGroup2.Name = "grpNewDoc";
            listViewGroup3.Header = "Attach - Save and Overwrite";
            listViewGroup3.Name = "grpAttachOverwrite";
            this._list.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this._list.HideSelection = false;
            this._list.LargeImageList = this.largeicons;
            this._list.Location = new System.Drawing.Point(0, 25);
            this._list.Name = "_list";
            this._list.Size = new System.Drawing.Size(660, 152);
            this._list.SmallImageList = this.smallicons;
            this._list.TabIndex = 0;
            this._list.UseCompatibleStateImageBehavior = false;
            this._list.View = System.Windows.Forms.View.Details;
            this._list.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this._list_AfterLabelEdit);
            this._list.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._list_ItemCheck);
            this._list.SelectedIndexChanged += new System.EventHandler(this._list_SelectedIndexChanged);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 200;
            // 
            // colDocId
            // 
            this.colDocId.Text = "Document ID";
            this.colDocId.Width = 100;
            // 
            // colLatest
            // 
            this.colLatest.Text = "Latest";
            this.colLatest.Width = 45;
            // 
            // colDocDesc
            // 
            this.colDocDesc.Text = "Description";
            this.colDocDesc.Width = 200;
            // 
            // colDocFolder
            // 
            this.colDocFolder.Text = "Folder";
            this.colDocFolder.Width = 120;
            // 
            // colFile
            // 
            this.colFile.Text = "Details";
            this.colFile.Width = 200;
            // 
            // ucSubDocument
            // 
            this.Controls.Add(this._list);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lblWarning);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ucSubDocument";
            this.Size = new System.Drawing.Size(660, 212);
            this.VisibleChanged += new System.EventHandler(this.ucSubDocument_VisibleChanged);
            this.ParentChanged += new System.EventHandler(this.ucSubDocument_ParentChanged);
            this.mnuOptions.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#endregion

		#region Captured Events

		private void ucSubDocument_ParentChanged(object sender, System.EventArgs e)
		{
			if (this.Parent is EnquiryForm)
			{
				_enqForm = (EnquiryForm)this.Parent;
				if (_enqForm.Enquiry.InDesignMode == false)
				{
                    _document = _enqForm.Enquiry.Object as OMSDocument;
                    if (_document.Direction == DocumentDirection.Out && colLatest.ListView != null)
                    {
                        _list.Columns.Remove(colLatest);
                        colDocId.Text = Session.CurrentSession.Resources.GetResource("DOCID", "Document ID", "").Text;
                    }
                    _enqForm.PageChanged -= new PageChangedEventHandler(_enqForm_PageChanged);
					_enqForm.PageChanged += new PageChangedEventHandler(_enqForm_PageChanged);
					_enqForm.Finishing -= new CancelEventHandler(_enqForm_Finishing);
					_enqForm.Finishing += new CancelEventHandler(_enqForm_Finishing);
					_enqForm.Cancelled -= new EventHandler(_enqform_Cancelled);
					_enqForm.Cancelled += new EventHandler(_enqform_Cancelled);
					DataRow row = _enqForm.GetSettings(this);
					_pgeName = Convert.ToString(row["quPage"]);
				}
			}
			else if (_enqForm != null && Parent == null)
			{
				_enqForm.PageChanged -=new PageChangedEventHandler(_enqForm_PageChanged);
				_enqForm.Finishing -=new CancelEventHandler(_enqForm_Finishing);
			}
		}


		private void ucSubDocument_VisibleChanged(object sender, System.EventArgs e)
		{
			if (Visible)
			{
				if (_document != null && _document.SubDocuments.GetLength(0) > 0)
				{
					if (lbl != null)
					{
						this.Controls.Remove(lbl);
						lbl = null;
					}

					//CM - WI.4258 - 03/06/14 - If the wallet has been changed, do not remove subdocuments and then re-add as this will lose the wallet in the process
					if (!folderChanged)
					{
						_list.Items.Clear();
						foreach (SubDocument doc in _document.SubDocuments)
						{
							AddItem(doc);
						}
					}
					CheckMenuStatus();
					CheckWarnings();

                    ScaleImageLists();
                }
				else
				{
					if (lbl == null)
					{
						lbl = new Label();
						lbl.Width = this.Width;
						lbl.Height = this.Height;
						lbl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; 
						lbl.TextAlign = ContentAlignment.MiddleCenter;
						lbl.Text = Session.CurrentSession.Resources.GetResource("NOSUBDOCS", "[No Sub Documents]", "").Text;
						this.Controls.Add(lbl);
						lbl.BringToFront();
					}
				}
			}
		}

		private void _enqForm_Finishing(object sender, CancelEventArgs e)
		{
			Close();
			foreach (ListViewItem itm in _list.Items)
			{
				SubDocument doc = (SubDocument)itm.Tag;
				doc.Store = itm.Checked;
				if (itm.SubItems[colDocFolder.Index].Tag != null)
				{
					doc.FolderGUID = new Guid(itm.SubItems[colDocFolder.Index].Tag.ToString());
				}
			}
		}


		private void _enqform_Cancelled(object sender, EventArgs e)
		{
			Close();
		}


		private void _enqForm_PageChanged(object sender, PageChangedEventArgs e)
		{
			if (_pgeName == e.PageName)
			{
				if (_document == null || _document.SubDocuments.GetLength(0) == 0)
				{
					if (e.Direction == EnquiryPageDirection.Back)
					{
						_enqForm.PreviousPage();
						return;
					}
					else if (e.Direction == EnquiryPageDirection.Next)
					{
						_enqForm.NextPage();
						return;
					}
				}
			}
			else
			{
				Close();
			}
		}


		private void btnSelect_Click(object sender, System.EventArgs e)
		{
			foreach (ListViewItem itm in _list.Items)
			{
				itm.Checked = true;
			}
		}


		private void btnDeselect_Click(object sender, System.EventArgs e)
		{
			foreach (ListViewItem itm in _list.Items)
			{
				itm.Checked = false;
			}
		}


		private void btnDocFolder_Click(object sender, System.EventArgs e)
		{
			ShowResults();
		}


		private void _list_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			SubDocument doc = _list.Items[e.Index].Tag as SubDocument;
			if (doc != null)
			{
				if (e.NewValue == CheckState.Checked)
					doc.Store = true;
				else
					doc.Store = false;
			}
			CheckWarnings();
		}

		private void _list_AfterLabelEdit(object sender, System.Windows.Forms.LabelEditEventArgs e)
		{
			SubDocument doc = _list.Items[e.Item].Tag as SubDocument;
			if (doc != null)
			{
				doc.DisplayName = e.Label;
			}
		
		}

        protected override void OnDpiChangedBeforeParent(EventArgs e)
        {
            base.OnDpiChangedBeforeParent(e);
            ScaleImageLists();
            
            InitializeMenuOptions();
        }

        #endregion

        #region Methods

        private void AddItem(SubDocument doc)
		{
			//The user physically acknowledges the sub document.
			doc.Acknowledged = true;

			smallicons.Images.Add(doc.SmallIcon.ToBitmap());
			largeicons.Images.Add(doc.Icon.ToBitmap());
			ListViewItem itm = new ListViewItem();
			itm.Name = Guid.NewGuid().ToString();
			itm.Text = doc.DisplayName;
			itm.ImageIndex = _list.SmallImageList.Images.Count - 1;
			itm.Tag = doc;
			for (int i = 0; i < _list.Columns.Count; i++)
				itm.SubItems.Add(String.Empty);

			_list.Items.Add(itm);
			SetDefaultFolderColumnValue(itm);
			
			ChangeItem(itm);
		}


		private void ChangeItem(ListViewItem item)
		{
			SubDocument doc = item.Tag as SubDocument;
			if (doc != null)
			{
				item.ForeColor = Color.Black;

				switch (doc.StoreAsSetting)
				{
					case SubDocument.StoreAs.None:
					case SubDocument.StoreAs.NewVersion:
						{
							item.Group = _list.Groups["grpAttachVersion"];
							item.SubItems[colDocId.Index].Text = doc.ExistingDocument.DisplayID;
							item.SubItems[colDocDesc.Index].Text = doc.ExistingDocument.ParentDocument.Description;

							if (colLatest.ListView != null)
							{
								if (doc.MarkAsLatest)
									item.SubItems[colLatest.Index].Text = Session.CurrentSession.Resources.GetResource("YES", "Yes", "").Text;
								else
									item.SubItems[colLatest.Index].Text = Session.CurrentSession.Resources.GetResource("No", "No", "").Text;
							}

							SetAttachedSubDocumentDescription(item, doc);

							item.SubItems[colFile.Index].Text = doc.ExistingDocument.ParentDocument.OMSFile.ToString();

							if (doc.IsOwnedByDifferentFile)
								item.ForeColor = Color.Red;
						}
						break;
					case SubDocument.StoreAs.NewDocument:
						{
							item.Group = _list.Groups["grpNewDoc"];
							item.SubItems[colDocId.Index].Text = String.Empty;
							item.SubItems[colDocDesc.Index].Text = String.Empty;

							if (colLatest.ListView != null)
								item.SubItems[colLatest.Index].Text = String.Empty;

							if (item.SubItems[colDocFolder.Index].Tag.ToString() == _document.FolderGUID.ToString())
								item.SubItems[colDocFolder.Index].Text = Session.CurrentSession.Resources.GetResource("SUBDOCDEFWLT", "(default)", "").Text;
						}
						break;
				}
				item.Checked = doc.Store;
			}
		}

        private void SetAttachedSubDocumentDescription(ListViewItem item, SubDocument doc)
        {
            if (tv != null)
            {
                OMSDocument omsdoc = OMSDocument.GetDocument(doc.ExistingDocument.ParentDocument.ID);
                string folderDescription = tv.GetFolderDescriptionByGuid(omsdoc.FolderGUID);
                if (!string.IsNullOrWhiteSpace(folderDescription))
                {
                    item.SubItems[colDocFolder.Index].Text = folderDescription;
                    return;
                }
            }
            item.SubItems[colDocFolder.Index].Text = Session.CurrentSession.Resources.GetResource("SUBDOCDEFWLT", "(default)", "").Text;
        }

        private void CheckWarnings()
		{
			lblWarning.Visible = false;
			foreach (ListViewItem item in _list.Items)
			{
				SubDocument doc = (SubDocument)item.Tag;
				if (doc.IsOwnedByDifferentFile && doc.Store)
					lblWarning.Visible = true;
			}
		}

        private void ScaleImageLists()
        {
            this._list.SmallImageList = Images.ScaleList(this.smallicons, LogicalToDeviceUnits(new Size(16, 16)));
            this._list.LargeImageList = Images.ScaleList(this.largeicons, LogicalToDeviceUnits(new Size(32, 32)));
        }

        private void InitializeMenuOptions()
        {
            this._list.ContextMenuStrip = null;

            if (mnuOptions != null)
            {
                mnuOptions.Items.Clear();
                mnuOptions.Dispose();
            }

            mnuOptions = new ContextMenuStrip();
            this.mnuOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuDetach,
                this.mnuAttach,
                this.mnuLatest,
                this.mnuResolve,
                this.mnuRename,
                this.mnuDocFolder});
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.Opening += new System.ComponentModel.CancelEventHandler(this.mnuOptions_Opening);

            this._list.ContextMenuStrip = mnuOptions;
        }

		private bool CheckMenuStatus()
		{
			mnuDocFolder.Visible = false;
			btnDocFolder.Visible = false;
			btnDetach.Visible = false;
			btnAttach.Visible = false;
			btnLatest.Visible = false;
			btnChangeDocDesc.Visible = false;

			if (_list.SelectedItems.Count == 1)
			{
				ListViewItem item = _list.SelectedItems[0];
				SubDocument doc = item.Tag as SubDocument;

				btnChangeDocDesc.Visible = !doc.AlreadyExists;

				if (doc.AllowsVersions)
				{
					switch (doc.StoreAsSetting)
					{
						case SubDocument.StoreAs.None:
							{
								btnDetach.Visible = true;
							}
							break;
						case SubDocument.StoreAs.NewVersion:
							{
								btnDetach.Visible = true;
								btnAttach.Text = Session.CurrentSession.Resources.GetResource("ATTACHTOVERSION", "Attach To Version", "").Text;
								btnAttach.Visible = true;
								btnLatest.Visible = true;
								btnLatest.Checked = doc.MarkAsLatest;
							}
							break;
						case SubDocument.StoreAs.NewDocument:
							{
								btnDetach.Visible = false;
								btnAttach.Text = Session.CurrentSession.Resources.GetResource("ATTACHTODOC", "Attach To Document", "").Text;
								btnAttach.Visible = true;
							}
							break;
					}
				}

				//New folder options
				switch (doc.StoreAsSetting)
				{
					case SubDocument.StoreAs.None:
					case SubDocument.StoreAs.NewVersion:
						{
							btnDocFolder.Visible = false;
							mnuDocFolder.Visible = false;
						}
						break;
					case SubDocument.StoreAs.NewDocument:
						{
							btnDocFolder.Visible = true;
							mnuDocFolder.Visible = true;
						}
						break;
				}
		
			}
			else if (_list.SelectedItems.Count > 1)
			{
				btnDocFolder.Visible = true;
				mnuDocFolder.Visible = true;
				btnDetach.Visible = true;
				btnAttach.Visible = false;
			}

			mnuDetach.Visible = btnDetach.Visible;
			mnuDetach.Text = btnDetach.Text;
			mnuAttach.Visible = btnAttach.Visible;
			mnuAttach.Text = btnAttach.Text;
			mnuLatest.Visible = btnLatest.Visible;
			mnuLatest.Checked = btnLatest.Checked;
			mnuRename.Visible = btnChangeDocDesc.Visible;
		
			
			return (_list.SelectedItems.Count > 0);

		}

		#endregion

		private void MenuClick(object sender, EventArgs e)
		{
			foreach (ListViewItem item in _list.SelectedItems)
			{

				SubDocument doc = item.Tag as SubDocument;

				if (doc != null)
				{
					if (sender == mnuAttach || sender == btnAttach)
					{
						if (doc.StoreAsSetting == SubDocument.StoreAs.NewVersion)
						{
							DocumentManagement.DocumentVersionPicker version = new DocumentManagement.DocumentVersionPicker();
							version.Document = doc.ExistingDocument.ParentDocument;
							FWBS.OMS.DocumentManagement.Storage.IStorageItemVersion[] versions = version.Show(this);
							if (versions == null)
								return;

							if (versions.Length > 0)
								doc.Attach(versions[0] as FWBS.OMS.DocumentManagement.DocumentVersion);
						}
						else
						{
							DocumentManagement.DocumentPicker picker = new DocumentManagement.DocumentPicker();
							picker.Type = FWBS.OMS.UI.Windows.DocumentManagement.DocumentPickerType.File;
							picker.File = _document.OMSFile;
							OMSDocument[] docs = picker.Show(this);
							if (docs == null)
								return;

							if (docs.Length > 0)
								doc.Attach(docs[0]);
						}
					}
					else if (sender == mnuDetach || sender == btnDetach)
					{
						doc.Detach();
					}
					else if (sender == mnuResolve || sender == btnResolve)
					{
						if (doc.Resolve() == false)
							MessageBox.Show(this, string.Concat(Session.CurrentSession.Resources.GetMessage("MSGCANTRESOLVE", "The document cannot be resolved.", "").Text, Environment.NewLine, doc.DisplayName),
								"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else if (sender == mnuLatest || sender == btnLatest)
					{
						if (doc.StoreAsSetting == SubDocument.StoreAs.NewVersion)
							doc.MarkAsLatest = !doc.MarkAsLatest;
					}
					else if (sender == btnChangeDocDesc)
					{
						//dbDocument.DocDesc is limited to 150 characters
						string newDocName = FWBS.OMS.UI.Windows.InputBox.Show(Session.CurrentSession.Resources.GetResource("CHANGEDOCDESC", "Please enter the document description", "").Text, FWBS.OMS.Branding.APPLICATION_NAME, doc.DisplayName, 150);

						if (newDocName != InputBox.CancelText)
						{
							if (newDocName.Length > 0)
							{
								item.Text = newDocName;
								doc.DisplayName = newDocName;
							}
							else
							{
								FWBS.OMS.UI.Windows.MessageBox.ShowInformation(Session.CurrentSession.Resources.GetResource("CHKLENGTHMIN1", "Please enter at least one character", "").Text);
							}
						}
					}

				}
				ChangeItem(item);
			}
			CheckMenuStatus();
			CheckWarnings();
		}


		private void mnuOptions_Opening(object sender, CancelEventArgs e)
		{
			e.Cancel = !CheckMenuStatus();
		}


		private void _list_SelectedIndexChanged(object sender, EventArgs e)
		{
			CheckMenuStatus();
		}


		private void mnuRename_Click(object sender, EventArgs e)
		{
			btnChangeDocDesc.PerformClick();
		}


		private void mnuDocFolder_Click(object sender, EventArgs e)
		{
			ShowResults();
		}


		private void ShowResults()
		{
			sp1 = new System.Windows.Forms.Splitter();
			sp1.Width = 4;
			sp1.BackColor = Color.LightGray;
			try
			{
				if (tv == null)
				{
                    tv = new ucTreeView(CreateTreeViewArgs());
                    _ptparent = Global.GetParentForm(this);
					if (_ptparent != null)
					{
						tv.Close -= new EventHandler(tv_Close);
						tv.Close += new EventHandler(tv_Close);
						tv.Apply -= new EventHandler(tv_Apply);
						tv.Apply += new EventHandler(tv_Apply);

						AddFolderTreeView();
						IncreaseParentFormWidth();
					}
				}
				else
				{
					AddFolderTreeView();
					IncreaseParentFormWidth();
				}
			}
			catch
			{
			}
			finally
			{
				if (sp1.Visible == false)
					IncreaseParentFormWidth();
				sp1.Visible = true;
				tv.Visible = true;
			}
		}

		private void AddFolderTreeView()
		{
			if (!_ptparent.Controls.Contains(tv))
			{
				sp1.Dock = DockStyle.Right;
				tv.Dock = DockStyle.Right;

                _ptparent.Controls.Add(sp1, true);
				_ptparent.Controls.Add(tv, true);
            }
		}


		private void IncreaseParentFormWidth()
		{
			if (!isExpanded)
			{
				_ptparent.Width = _ptparent.Width + tv.Width + sp1.Width;
				isExpanded = true;
			}
		}


		private void DecreaseParentFormWidth()
		{
			if (isExpanded)
			{
				_ptparent.Width = _ptparent.Width - (tv.Width + sp1.Width);
				isExpanded = false;
			}
		}

		private ucTreeViewArgs CreateTreeViewArgs()
		{
			args = new ucTreeViewArgs();
            args.ApplyToAllText = "Apply to all sub-documents";
            args.IncludeApplyToAllCheckBox = true;
            args.OMSFile = _document.OMSFile;
			args.SelectedFolder = _document.FolderGUID;
			return args;
		}

		private void tv_Close(object sender, EventArgs e)
		{
			Close();
		}


		public void Close()
		{
			if (tv != null && tv.Visible)
			{
				if (tv != null) tv.Visible = false;
				if (sp1 != null) sp1.Visible = false;
				if (_ptparent != null)
					DecreaseParentFormWidth();
			}
		}


		private void tv_Apply(object sender, EventArgs e)
		{
			Apply();
		}


		private void Apply()
		{
			if (args.ApplyToAllValue)
			{
				foreach (ListViewItem item in _list.Items)
				{
					SetFolderColumnValue(item);
				}
			}
			else
			{
				foreach (ListViewItem item in _list.SelectedItems)
				{
					SetFolderColumnValue(item);
				}
			}
			
		}


		private void SetDefaultFolderColumnValue(ListViewItem item)
		{
			item.SubItems[colDocFolder.Index].Tag = _document.FolderGUID.ToString();
			folderChanged = true;
		}


		private void SetFolderColumnValue(ListViewItem item)
		{
			if (args.SelectedFolder != null && item.Group.Name == "grpNewDoc")
			{
				item.SubItems[colDocFolder.Index].Tag = args.SelectedFolder.ToString();
				item.SubItems[colDocFolder.Index].Text = args.SelectedFolderText;
				folderChanged = true;
			}
		}
    }


}
