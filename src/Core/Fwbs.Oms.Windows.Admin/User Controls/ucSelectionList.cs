using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucSelectionList.
    /// </summary>
    public class ucSelectionList : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.GroupBox grpContainer;
		private System.Windows.Forms.Panel pnlSelected;
		private System.Windows.Forms.Label labSelected;
		private System.Windows.Forms.ListBox lstSelected;
		private FWBS.Common.UI.Windows.eXPPanel pnlSelectedHelp;
		private System.Windows.Forms.Label labSelectedHelpDesc;
		private System.Windows.Forms.Label labSelectedHelpCaption;
		private System.Windows.Forms.Splitter splSelected;
		private System.Windows.Forms.Panel pnlAvailable;
		private System.Windows.Forms.ListBox lstAvailable;
		private System.Windows.Forms.Label labAvailable;
		private System.Windows.Forms.Splitter splAvailable;
		private FWBS.Common.UI.Windows.eXPPanel pnlAvailableHelp;
		private System.Windows.Forms.Label labAvailableHelpDesc;
		private System.Windows.Forms.Label labAvailableHelpCaption;
		private System.Windows.Forms.TableLayoutPanel pnlContainer;
		private System.Windows.Forms.Panel pnlNavButtons;
		private System.Windows.Forms.Button btnSelectItems;
		private System.Windows.Forms.Button btnUnselectAll;
		private System.Windows.Forms.Button btnUnselectItems;
		private System.Windows.Forms.Button btnSelectAll;

		private DataTable _inputdata = new DataTable("Available");
		private DataTable _outputdata = new DataTable("Selected");

		private Point tbDragAvl;
		private Point tbDragSel;
		private enum _dragsources {lstSelected,lstAvailable};
		private _dragsources _dragsource;
		private DataTable _inputdatatable;
		private System.Xml.XmlNodeList _inputdatanodes;
		private DataTable _outputdatatable;
		private System.Xml.XmlNodeList _outputdatanodes;
		private DataView _outputdataview;
		private DataView _inputdataview;
		private enum _datatypes {dataview,datatable,xml,none};
		private _datatypes _idatatype = _datatypes.none;
		private _datatypes _odatatype = _datatypes.none;
		private DataTable _codelookups = null;
		private System.Windows.Forms.Panel pnlFilterAvailable;
		private System.Windows.Forms.Label labSearchAvailable;
		private System.Windows.Forms.TextBox txtFilterAvailable;
		private System.Windows.Forms.Panel pnlFilterSelected;
		private System.Windows.Forms.TextBox txtFilterSelected;
		private System.Windows.Forms.Label labFilterSelected;
        protected ResourceLookup resourceLookup1;
        private IContainer components;

        public event EventHandler Changed;
        public event EventHandler<BeforeSelectionChangeEventArgs> Changing;

		public ucSelectionList()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			_inputdata.Columns.Add("Value");
			_inputdata.Columns.Add("Display");
			_outputdata.Columns.Add("Value");
			_outputdata.Columns.Add("Display");
			RefreshDataSources();
		}

		private void AddItem(DataTable dt, object Value, string Display)
		{
			DataRow dr = dt.NewRow();
			dr["Value"] = Value;
			dr["Display"] = Display;
			dt.Rows.Add(dr);
		}

		private void AddItem(DataTable dt, DataRow newdr)
		{
			dt.Rows.Add(newdr.ItemArray);
		}
		
		private void RemoveItem(DataTable dt, int i)
		{
			dt.DefaultView[i].Row.Delete();
		}

		private void ClearList(DataTable dt)
		{
			dt.RejectChanges();
		}

        protected virtual void OnChanging(BeforeSelectionChangeEventArgs e)
        {
            if (Changing != null)
                Changing(this, e);
        }

		// Invoke the Changed event; when changed
		protected virtual void OnChanged(EventArgs e) 
		{
			if (Changed != null)
				Changed(this, e);
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
            this.grpContainer = new System.Windows.Forms.GroupBox();
            this.pnlContainer = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSelected = new System.Windows.Forms.Panel();
            this.lstSelected = new System.Windows.Forms.ListBox();
            this.pnlFilterSelected = new System.Windows.Forms.Panel();
            this.txtFilterSelected = new System.Windows.Forms.TextBox();
            this.labFilterSelected = new System.Windows.Forms.Label();
            this.labSelected = new System.Windows.Forms.Label();
            this.splSelected = new System.Windows.Forms.Splitter();
            this.pnlSelectedHelp = new FWBS.Common.UI.Windows.eXPPanel();
            this.labSelectedHelpDesc = new System.Windows.Forms.Label();
            this.labSelectedHelpCaption = new System.Windows.Forms.Label();
            this.pnlNavButtons = new System.Windows.Forms.Panel();
            this.btnSelectItems = new System.Windows.Forms.Button();
            this.btnUnselectItems = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnUnselectAll = new System.Windows.Forms.Button();
            this.pnlAvailable = new System.Windows.Forms.Panel();
            this.lstAvailable = new System.Windows.Forms.ListBox();
            this.pnlFilterAvailable = new System.Windows.Forms.Panel();
            this.txtFilterAvailable = new System.Windows.Forms.TextBox();
            this.labSearchAvailable = new System.Windows.Forms.Label();
            this.labAvailable = new System.Windows.Forms.Label();
            this.splAvailable = new System.Windows.Forms.Splitter();
            this.pnlAvailableHelp = new FWBS.Common.UI.Windows.eXPPanel();
            this.labAvailableHelpDesc = new System.Windows.Forms.Label();
            this.labAvailableHelpCaption = new System.Windows.Forms.Label();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.grpContainer.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            this.pnlSelected.SuspendLayout();
            this.pnlFilterSelected.SuspendLayout();
            this.pnlSelectedHelp.SuspendLayout();
            this.pnlNavButtons.SuspendLayout();
            this.pnlAvailable.SuspendLayout();
            this.pnlFilterAvailable.SuspendLayout();
            this.pnlAvailableHelp.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpContainer
            // 
            this.grpContainer.BackColor = System.Drawing.Color.White;
            this.grpContainer.Controls.Add(this.pnlContainer);
            this.grpContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpContainer.Location = new System.Drawing.Point(0, 0);
            this.grpContainer.Name = "grpContainer";
            this.grpContainer.Padding = new System.Windows.Forms.Padding(15);
            this.grpContainer.Size = new System.Drawing.Size(646, 402);
            this.grpContainer.TabIndex = 8;
            this.grpContainer.TabStop = false;
            this.grpContainer.Text = "Select What Extended Data Object are Compatable with the %1% Type";
            // 
            // pnlContainer
            // 
            this.pnlContainer.ColumnCount = 3;
            this.pnlContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.pnlContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlContainer.Controls.Add(this.pnlSelected, 2, 0);
            this.pnlContainer.Controls.Add(this.pnlNavButtons, 1, 0);
            this.pnlContainer.Controls.Add(this.pnlAvailable, 0, 0);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.pnlContainer.Location = new System.Drawing.Point(15, 28);
            this.pnlContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.RowCount = 1;
            this.pnlContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlContainer.Size = new System.Drawing.Size(616, 359);
            this.pnlContainer.TabIndex = 12;
            // 
            // pnlSelected
            // 
            this.pnlSelected.Controls.Add(this.lstSelected);
            this.pnlSelected.Controls.Add(this.pnlFilterSelected);
            this.pnlSelected.Controls.Add(this.labSelected);
            this.pnlSelected.Controls.Add(this.splSelected);
            this.pnlSelected.Controls.Add(this.pnlSelectedHelp);
            this.pnlSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSelected.Location = new System.Drawing.Point(337, 0);
            this.pnlSelected.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSelected.Name = "pnlSelected";
            this.pnlSelected.Size = new System.Drawing.Size(279, 359);
            this.pnlSelected.TabIndex = 9;
            // 
            // lstSelected
            // 
            this.lstSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSelected.IntegralHeight = false;
            this.lstSelected.Location = new System.Drawing.Point(0, 38);
            this.lstSelected.Name = "lstSelected";
            this.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelected.Size = new System.Drawing.Size(279, 235);
            this.lstSelected.TabIndex = 3;
            this.lstSelected.SelectedIndexChanged += new System.EventHandler(this.lstSelected_SelectedIndexChanged);
            this.lstSelected.DoubleClick += new System.EventHandler(this.btnUnselectItems_Click);
            this.lstSelected.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstSelected_MouseDown);
            this.lstSelected.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstSelected_MouseMove);
            // 
            // pnlFilterSelected
            // 
            this.pnlFilterSelected.Controls.Add(this.txtFilterSelected);
            this.pnlFilterSelected.Controls.Add(this.labFilterSelected);
            this.pnlFilterSelected.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterSelected.Location = new System.Drawing.Point(0, 16);
            this.pnlFilterSelected.Name = "pnlFilterSelected";
            this.pnlFilterSelected.Size = new System.Drawing.Size(279, 22);
            this.pnlFilterSelected.TabIndex = 12;
            // 
            // txtFilterSelected
            // 
            this.txtFilterSelected.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtFilterSelected.Location = new System.Drawing.Point(40, 0);
            this.txtFilterSelected.Name = "txtFilterSelected";
            this.txtFilterSelected.Size = new System.Drawing.Size(239, 20);
            this.txtFilterSelected.TabIndex = 12;
            this.txtFilterSelected.TextChanged += new System.EventHandler(this.txtFilterSelected_TextChanged);
            // 
            // labFilterSelected
            // 
            this.labFilterSelected.Dock = System.Windows.Forms.DockStyle.Left;
            this.labFilterSelected.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup1.SetLookup(this.labFilterSelected, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblFilter", "Filter : ", ""));
            this.labFilterSelected.Name = "labFilterSelected";
            this.labFilterSelected.Size = new System.Drawing.Size(40, 22);
            this.labFilterSelected.TabIndex = 0;
            this.labFilterSelected.Text = "Filter : ";
            this.labFilterSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labSelected
            // 
            this.labSelected.Dock = System.Windows.Forms.DockStyle.Top;
            this.labSelected.Location = new System.Drawing.Point(0, 0);
            this.labSelected.Name = "labSelected";
            this.labSelected.Size = new System.Drawing.Size(279, 16);
            this.labSelected.TabIndex = 2;
            this.labSelected.Text = "Selected Extended Data Objects";
            // 
            // splSelected
            // 
            this.splSelected.BackColor = System.Drawing.SystemColors.Control;
            this.splSelected.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splSelected.Location = new System.Drawing.Point(0, 273);
            this.splSelected.Name = "splSelected";
            this.splSelected.Size = new System.Drawing.Size(279, 4);
            this.splSelected.TabIndex = 10;
            this.splSelected.TabStop = false;
            // 
            // pnlSelectedHelp
            // 
            this.pnlSelectedHelp.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.pnlSelectedHelp.BorderLine = true;
            this.pnlSelectedHelp.Controls.Add(this.labSelectedHelpDesc);
            this.pnlSelectedHelp.Controls.Add(this.labSelectedHelpCaption);
            this.pnlSelectedHelp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSelectedHelp.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlSelectedHelp.Location = new System.Drawing.Point(0, 277);
            this.pnlSelectedHelp.Name = "pnlSelectedHelp";
            this.pnlSelectedHelp.Padding = new System.Windows.Forms.Padding(3);
            this.pnlSelectedHelp.Size = new System.Drawing.Size(279, 82);
            this.pnlSelectedHelp.TabIndex = 9;
            // 
            // labSelectedHelpDesc
            // 
            this.labSelectedHelpDesc.BackColor = System.Drawing.Color.White;
            this.labSelectedHelpDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labSelectedHelpDesc.Location = new System.Drawing.Point(3, 17);
            this.labSelectedHelpDesc.Name = "labSelectedHelpDesc";
            this.labSelectedHelpDesc.Size = new System.Drawing.Size(273, 62);
            this.labSelectedHelpDesc.TabIndex = 1;
            // 
            // labSelectedHelpCaption
            // 
            this.labSelectedHelpCaption.BackColor = System.Drawing.Color.White;
            this.labSelectedHelpCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.labSelectedHelpCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labSelectedHelpCaption.Location = new System.Drawing.Point(3, 3);
            this.labSelectedHelpCaption.Name = "labSelectedHelpCaption";
            this.labSelectedHelpCaption.Size = new System.Drawing.Size(273, 14);
            this.labSelectedHelpCaption.TabIndex = 0;
            // 
            // pnlNavButtons
            // 
            this.pnlNavButtons.BackColor = System.Drawing.Color.White;
            this.pnlNavButtons.Controls.Add(this.btnSelectItems);
            this.pnlNavButtons.Controls.Add(this.btnUnselectItems);
            this.pnlNavButtons.Controls.Add(this.btnSelectAll);
            this.pnlNavButtons.Controls.Add(this.btnUnselectAll);
            this.pnlNavButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNavButtons.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.pnlNavButtons.Location = new System.Drawing.Point(279, 0);
            this.pnlNavButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlNavButtons.Name = "pnlNavButtons";
            this.pnlNavButtons.Size = new System.Drawing.Size(58, 359);
            this.pnlNavButtons.TabIndex = 0;
            // 
            // btnSelectItems
            // 
            this.btnSelectItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectItems.Location = new System.Drawing.Point(14, 38);
            this.btnSelectItems.Name = "btnSelectItems";
            this.btnSelectItems.Size = new System.Drawing.Size(32, 23);
            this.btnSelectItems.TabIndex = 11;
            this.btnSelectItems.Text = ">";
            this.btnSelectItems.Click += new System.EventHandler(this.btnSelectItems_Click);
            // 
            // btnUnselectItems
            // 
            this.btnUnselectItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUnselectItems.Location = new System.Drawing.Point(14, 70);
            this.btnUnselectItems.Name = "btnUnselectItems";
            this.btnUnselectItems.Size = new System.Drawing.Size(32, 23);
            this.btnUnselectItems.TabIndex = 12;
            this.btnUnselectItems.Text = "<";
            this.btnUnselectItems.Click += new System.EventHandler(this.btnUnselectItems_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectAll.Location = new System.Drawing.Point(14, 102);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(32, 23);
            this.btnSelectAll.TabIndex = 13;
            this.btnSelectAll.Text = ">>";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnUnselectAll
            // 
            this.btnUnselectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUnselectAll.Location = new System.Drawing.Point(14, 134);
            this.btnUnselectAll.Name = "btnUnselectAll";
            this.btnUnselectAll.Size = new System.Drawing.Size(32, 23);
            this.btnUnselectAll.TabIndex = 14;
            this.btnUnselectAll.Text = "<<";
            this.btnUnselectAll.Click += new System.EventHandler(this.btnUnselectAll_Click);
            // 
            // pnlAvailable
            // 
            this.pnlAvailable.Controls.Add(this.lstAvailable);
            this.pnlAvailable.Controls.Add(this.pnlFilterAvailable);
            this.pnlAvailable.Controls.Add(this.labAvailable);
            this.pnlAvailable.Controls.Add(this.splAvailable);
            this.pnlAvailable.Controls.Add(this.pnlAvailableHelp);
            this.pnlAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAvailable.Location = new System.Drawing.Point(0, 0);
            this.pnlAvailable.Margin = new System.Windows.Forms.Padding(0);
            this.pnlAvailable.Name = "pnlAvailable";
            this.pnlAvailable.Size = new System.Drawing.Size(279, 359);
            this.pnlAvailable.TabIndex = 11;
            // 
            // lstAvailable
            // 
            this.lstAvailable.AllowDrop = true;
            this.lstAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAvailable.IntegralHeight = false;
            this.lstAvailable.Location = new System.Drawing.Point(0, 38);
            this.lstAvailable.Name = "lstAvailable";
            this.lstAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstAvailable.Size = new System.Drawing.Size(279, 235);
            this.lstAvailable.TabIndex = 3;
            this.lstAvailable.SelectedIndexChanged += new System.EventHandler(this.lstAvailable_SelectedIndexChanged);
            this.lstAvailable.DoubleClick += new System.EventHandler(this.btnSelectItems_Click);
            this.lstAvailable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstAvailable_MouseDown);
            this.lstAvailable.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstAvailable_MouseMove);
            // 
            // pnlFilterAvailable
            // 
            this.pnlFilterAvailable.Controls.Add(this.txtFilterAvailable);
            this.pnlFilterAvailable.Controls.Add(this.labSearchAvailable);
            this.pnlFilterAvailable.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterAvailable.Location = new System.Drawing.Point(0, 16);
            this.pnlFilterAvailable.Name = "pnlFilterAvailable";
            this.pnlFilterAvailable.Size = new System.Drawing.Size(279, 22);
            this.pnlFilterAvailable.TabIndex = 11;
            // 
            // txtFilterAvailable
            // 
            this.txtFilterAvailable.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtFilterAvailable.Location = new System.Drawing.Point(40, 0);
            this.txtFilterAvailable.Name = "txtFilterAvailable";
            this.txtFilterAvailable.Size = new System.Drawing.Size(239, 20);
            this.txtFilterAvailable.TabIndex = 12;
            this.txtFilterAvailable.TextChanged += new System.EventHandler(this.txtFilterAvailable_TextChanged);
            // 
            // labSearchAvailable
            // 
            this.labSearchAvailable.Dock = System.Windows.Forms.DockStyle.Left;
            this.labSearchAvailable.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup1.SetLookup(this.labSearchAvailable, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblFilter", "Filter : ", ""));
            this.labSearchAvailable.Name = "labSearchAvailable";
            this.labSearchAvailable.Size = new System.Drawing.Size(40, 22);
            this.labSearchAvailable.TabIndex = 0;
            this.labSearchAvailable.Text = "Filter : ";
            this.labSearchAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labAvailable
            // 
            this.labAvailable.Dock = System.Windows.Forms.DockStyle.Top;
            this.labAvailable.Location = new System.Drawing.Point(0, 0);
            this.labAvailable.Name = "labAvailable";
            this.labAvailable.Size = new System.Drawing.Size(279, 16);
            this.labAvailable.TabIndex = 2;
            this.labAvailable.Text = "Selected Extended Data Objects";
            // 
            // splAvailable
            // 
            this.splAvailable.BackColor = System.Drawing.SystemColors.Control;
            this.splAvailable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splAvailable.Location = new System.Drawing.Point(0, 273);
            this.splAvailable.Name = "splAvailable";
            this.splAvailable.Size = new System.Drawing.Size(279, 4);
            this.splAvailable.TabIndex = 10;
            this.splAvailable.TabStop = false;
            // 
            // pnlAvailableHelp
            // 
            this.pnlAvailableHelp.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.pnlAvailableHelp.BorderLine = true;
            this.pnlAvailableHelp.Controls.Add(this.labAvailableHelpDesc);
            this.pnlAvailableHelp.Controls.Add(this.labAvailableHelpCaption);
            this.pnlAvailableHelp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAvailableHelp.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlAvailableHelp.Location = new System.Drawing.Point(0, 277);
            this.pnlAvailableHelp.Name = "pnlAvailableHelp";
            this.pnlAvailableHelp.Padding = new System.Windows.Forms.Padding(3);
            this.pnlAvailableHelp.Size = new System.Drawing.Size(279, 82);
            this.pnlAvailableHelp.TabIndex = 9;
            // 
            // labAvailableHelpDesc
            // 
            this.labAvailableHelpDesc.BackColor = System.Drawing.Color.White;
            this.labAvailableHelpDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labAvailableHelpDesc.Location = new System.Drawing.Point(3, 17);
            this.labAvailableHelpDesc.Name = "labAvailableHelpDesc";
            this.labAvailableHelpDesc.Size = new System.Drawing.Size(273, 62);
            this.labAvailableHelpDesc.TabIndex = 1;
            // 
            // labAvailableHelpCaption
            // 
            this.labAvailableHelpCaption.BackColor = System.Drawing.Color.White;
            this.labAvailableHelpCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.labAvailableHelpCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labAvailableHelpCaption.Location = new System.Drawing.Point(3, 3);
            this.labAvailableHelpCaption.Name = "labAvailableHelpCaption";
            this.labAvailableHelpCaption.Size = new System.Drawing.Size(273, 14);
            this.labAvailableHelpCaption.TabIndex = 0;
            // 
            // ucSelectionList
            // 
            this.Controls.Add(this.grpContainer);
            this.Name = "ucSelectionList";
            this.Size = new System.Drawing.Size(646, 402);
            this.SizeChanged += new System.EventHandler(this.ucSelectionList_SizeChanged);
            this.grpContainer.ResumeLayout(false);
            this.pnlContainer.ResumeLayout(false);
            this.pnlSelected.ResumeLayout(false);
            this.pnlFilterSelected.ResumeLayout(false);
            this.pnlFilterSelected.PerformLayout();
            this.pnlSelectedHelp.ResumeLayout(false);
            this.pnlNavButtons.ResumeLayout(false);
            this.pnlAvailable.ResumeLayout(false);
            this.pnlFilterAvailable.ResumeLayout(false);
            this.pnlFilterAvailable.PerformLayout();
            this.pnlAvailableHelp.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public DataTable SelectedItems
		{
			get
			{
				return _outputdata;
			}
		}
		
		public string AvailableHeading
		{
			get
			{
				return labAvailable.Text;
			}
			set
			{
				labAvailable.Text = value;
			}
		}

		public string SelectedHeading
		{
			get
			{
				return labSelected.Text;
			}
			set
			{
				labSelected.Text = value;
			}
		}

		public string SelectionListTitle
		{
			get
			{
				return grpContainer.Text;
			}
			set
			{
				grpContainer.Text = value;
			}
		}
		
		private string _codetype;
		public string CodeType
		{
			get
			{
				return _codetype;
			}
			set
			{
				if (value != _codetype)
				{
					_codetype = value;
					_codelookups = FWBS.OMS.CodeLookup.GetLookups(_codetype);
				}
			}
		}

		private void btnSelectItems_Click(object sender, System.EventArgs e)
		{
			lstSelected.SelectionMode = SelectionMode.One;
			object last = DBNull.Value;
			ArrayList toberemoved = new ArrayList();
            List<DataRow> proposedChangedItems = new List<DataRow>();
			for (int i = lstAvailable.Items.Count-1; i > -1; i--)
			{
				if (lstAvailable.GetSelected(i) || sender == btnSelectAll)
				{
                    proposedChangedItems.Add(_inputdata.DefaultView[i].Row);
				}
			}

            var ea = new BeforeSelectionChangeEventArgs();
            ea.DataRows = proposedChangedItems.ToArray();
            ea.Direction = Direction.Add;
            ea.Cancel = false;
            OnChanging(ea);
            if (ea.Cancel)
                return;

            for (int i = lstAvailable.Items.Count - 1; i > -1; i--)
            {
                if (lstAvailable.GetSelected(i) || sender == btnSelectAll)
                {
                    last = _inputdata.DefaultView[i]["Value"];
                    AddItem(_outputdata, _inputdata.DefaultView[i].Row);
                    toberemoved.Add(i);
                }
            }

            foreach (Int32 i in toberemoved)
				_inputdata.DefaultView[i].Delete();
			
			try{lstSelected.SelectedValue = last;}
			catch{}
			lstSelected.SelectionMode = SelectionMode.MultiExtended;
			if (lstSelected.Items.Count >0)
			{
				lstSelected.DisplayMember = "Display";
				lstSelected.ValueMember = "Value";
			}
			OnChanged(e);
		}

		private void btnUnselectItems_Click(object sender, System.EventArgs e)
		{
			lstAvailable.SelectionMode = SelectionMode.One;
			ArrayList toberemoved = new ArrayList();
            List<DataRow> proposedChangedItems = new List<DataRow>();
            object last = DBNull.Value;
            for (int i = lstSelected.Items.Count - 1; i > -1; i--)
            {
                if (lstSelected.GetSelected(i) || sender == btnUnselectAll)
                {
                    proposedChangedItems.Add(_outputdata.DefaultView[i].Row);
                }
            }

            var ea = new BeforeSelectionChangeEventArgs();
            ea.DataRows = proposedChangedItems.ToArray();
            ea.Direction = Direction.Remove;
            ea.Cancel = false;
            OnChanging(ea);
            if (ea.Cancel)
                return;

            for (int i = lstSelected.Items.Count - 1; i > -1; i--)
			{
				if (lstSelected.GetSelected(i) || sender == btnUnselectAll)
				{
					last = _outputdata.DefaultView[i]["Value"];
					AddItem(_inputdata,_outputdata.DefaultView[i].Row);
                    toberemoved.Add(i);
				}
			}

			foreach (Int32 i in toberemoved)
				_outputdata.DefaultView[i].Delete();

			try{lstAvailable.SelectedValue = last;}
			catch{}
			lstAvailable.SelectionMode = SelectionMode.MultiExtended;
			if (lstAvailable.Items.Count >0)
			{
				lstAvailable.DisplayMember = "Display";
				lstAvailable.ValueMember = "Value";
			}
			OnChanged(e);
		}
		
		private void btnSelectAll_Click(object sender, System.EventArgs e)
		{
			btnSelectItems_Click(sender,e);
			OnChanged(e);
		}

		private void btnUnselectAll_Click(object sender, System.EventArgs e)
		{
			btnUnselectItems_Click(sender,e);
			OnChanged(e);
		}	
		
		private void RefreshDataSources()
		{
			labAvailableHelpCaption.Text="";
			labAvailableHelpDesc.Text="";
			labSelectedHelpCaption.Text="";
			labSelectedHelpDesc.Text="";
			lstSelected.DataSource = null;
			lstAvailable.DataSource = null;
			lstSelected.DataSource = _outputdata;
			lstAvailable.DataSource = _inputdata;
			_inputdata.DefaultView.Sort = "Display";
			_outputdata.DefaultView.Sort = "Display";
			
			if (lstAvailable.Items.Count >0)
			{
				lstAvailable.DisplayMember = "Display";
				lstAvailable.ValueMember = "Value";
			}
			if (lstSelected.Items.Count >0)
			{
				lstSelected.DisplayMember = "Display";
				lstSelected.ValueMember = "Value";
			}
		}

		public void RefreshData()
		{
			_outputdata.RejectChanges();
			_inputdata.RejectChanges();
			if (_idatatype == _datatypes.datatable)
			{
				foreach(DataRow rw in _inputdatatable.Rows)
				{
					AddItem(_inputdata,rw[_inputvaluemember],Convert.ToString(rw[_inputdisplaymember]));
				}
			}
			else if (_idatatype == _datatypes.xml)
			{
			}
			else if (_idatatype == _datatypes.dataview)
			{
				foreach(DataRowView rw in _inputdataview)
				{
					AddItem(_inputdata,rw[_inputvaluemember],Convert.ToString(rw[_inputdisplaymember]));
				}
			}

			if (_odatatype == _datatypes.datatable)
			{
				foreach(DataRow rw in _outputdatatable.Rows)
				{
					for(int i = 0; i < _inputdata.Rows.Count; i++)
					{
						if (Convert.ToString(rw[_outputvaluemember]) == Convert.ToString(_inputdata.Rows[i]["Value"]))
						{
							AddItem(_outputdata,_inputdata.Rows[i]);
							_inputdata.Rows[i].Delete();
							break;
						}
					}
				}
			}
			else if (_odatatype == _datatypes.xml)
			{
				foreach(XmlNode dr in _outputdatanodes)
				{
					for(int i = 0; i < _inputdata.Rows.Count; i++)
					{
						if (dr.SelectSingleNode(_outputvaluemember).Value == Convert.ToString(_inputdata.Rows[i]["Value"]))
						{
							AddItem(_outputdata,_inputdata.Rows[i]);
							_inputdata.Rows[i].Delete();
							break;
						}
					}
				}
			}
			else if (_odatatype == _datatypes.dataview)
			{
				foreach(DataRowView rw in _outputdataview)
				{
					for(int i = 0; i < _inputdata.Rows.Count; i++)
					{
						if (Convert.ToString(rw[_outputvaluemember]) == Convert.ToString(_inputdata.Rows[i]["Value"]))
						{
							AddItem(_outputdata,_inputdata.Rows[i]);
							_inputdata.Rows[i].Delete();
							break;
						}
					}
				}
			}
			RefreshDataSources();
		}

		public object InputData
		{
			set
			{
				if (value is DataView)
				{
					_inputdataview = (DataView)value;
					_idatatype = _datatypes.dataview;
				}
				else if (value is DataTable)
				{
					_inputdatatable = (DataTable)value;
					_idatatype = _datatypes.datatable;
				}
				else if (value is System.Xml.XmlNodeList)
				{
					_inputdatanodes = (System.Xml.XmlNodeList)value;
					_idatatype = _datatypes.xml;
				}
			}
		}

		public object OutputData
		{
			set
			{
				if (value is DataView)
				{
					_outputdataview = (DataView)value;
					_idatatype = _datatypes.dataview;
				}
				else if (value is DataTable)
				{
					_outputdatatable = (DataTable)value;
					_odatatype = _datatypes.datatable;
				}
				else if (value is System.Xml.XmlNodeList)
				{
					_outputdatanodes = (System.Xml.XmlNodeList)value;
					_odatatype = _datatypes.xml;
				}
			}
		}

		private string _inputdisplaymember;
		public string InputDisplayMember
		{
			get
			{
				return _inputdisplaymember;
			}
			set
			{
				_inputdisplaymember = value;
			}
		}

		private string _inputvaluemember;
		public string InputValueMember
		{
			get
			{
				return _inputvaluemember;
			}
			set
			{
				_inputvaluemember = value;
			}
		}

		private string _outputdisplaymember;
		public string OutputDisplayMember
		{
			get
			{
				return _outputdisplaymember;
			}
			set
			{
				_outputdisplaymember = value;
			}
		}

		private string _outputvaluemember;
		public string OutputValueMember
		{
			get
			{
				return _outputvaluemember;
			}
			set
			{
				_outputvaluemember = value;
			}
		}


		private void lstAvailable_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && ((Math.Abs(tbDragAvl.X - e.X) > 3) || (Math.Abs(tbDragAvl.Y - e.Y) > 3)))
			{
				_dragsource=_dragsources.lstAvailable;
				lstAvailable.DoDragDrop(lstAvailable, DragDropEffects.Copy | DragDropEffects.Move);
			}
		}

		private void lstAvailable_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				tbDragAvl = new Point(e.X,e.Y);
		}

		private void lstSelected_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				tbDragSel = new Point(e.X,e.Y);
		}

		private void lstSelected_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && ((Math.Abs(tbDragSel.X - e.X) > 3) || (Math.Abs(tbDragSel.Y - e.Y) > 3)))
			{
				_dragsource=_dragsources.lstSelected;
				lstAvailable.DoDragDrop(lstSelected, DragDropEffects.Copy | DragDropEffects.Move);
			}

		}

		private void lstSelected_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Console.WriteLine(_dragsource);
			btnSelectItems_Click(sender,EventArgs.Empty);
		}

		private void lstAvailable_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Console.WriteLine(_dragsource);
			btnUnselectItems_Click(sender,EventArgs.Empty);
		}

		private void lstAvailable_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (_dragsource == _dragsources.lstSelected) e.Effect = DragDropEffects.Copy;
		}

		private void lstSelected_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (_dragsource == _dragsources.lstAvailable) e.Effect = DragDropEffects.Copy;

		}

		private void ucSelectionList_SizeChanged(object sender, System.EventArgs e)
		{
			pnlAvailable.Width = ((this.Width - 40)-this.pnlNavButtons.Width) / 2;
		}

		private void lstAvailable_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (_codelookups != null && lstAvailable.SelectedValue != null)
			{
				_codelookups.DefaultView.RowFilter = "[cdcode] = '" + lstAvailable.SelectedValue.ToString() + "'";
				if (_codelookups.DefaultView.Count > 0)
				{
					try{labAvailableHelpCaption.Text = Session.CurrentSession.Terminology.Parse(_codelookups.DefaultView[0]["cddesc"].ToString(),true);}
					catch{labAvailableHelpCaption.Text = Session.CurrentSession.Resources.GetResource("UNKNOWN","Unknown","").Text;}

					try{labAvailableHelpDesc.Text = Session.CurrentSession.Terminology.Parse(_codelookups.DefaultView[0]["cdhelp"].ToString(),true);}
					catch{labAvailableHelpDesc.Text = Session.CurrentSession.Resources.GetResource("UNKNOWN","Unknown","").Text;}
				}
				else
				{
					labAvailableHelpDesc.Text = Session.CurrentSession.Resources.GetResource("UNKNOWN","Unknown","").Text;
					labAvailableHelpCaption.Text = Session.CurrentSession.Resources.GetResource("UNKNOWN","Unknown","").Text;
				}
			}
		}

		private void lstSelected_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (_codelookups.DefaultView != null && lstSelected.SelectedValue != null)
			{
				_codelookups.DefaultView.RowFilter = "[cdcode] = '" + lstSelected.SelectedValue.ToString() + "'";
				if (_codelookups.DefaultView.Count > 0)
				{
					try{labSelectedHelpCaption.Text = Session.CurrentSession.Terminology.Parse(_codelookups.DefaultView[0]["cddesc"].ToString(),true);}
					catch{labSelectedHelpCaption.Text = Session.CurrentSession.Resources.GetResource("UNKNOWN","Unknown","").Text;}

					try{labSelectedHelpDesc.Text = Session.CurrentSession.Terminology.Parse(_codelookups.DefaultView[0]["cdhelp"].ToString(),true);}
					catch{labSelectedHelpDesc.Text = Session.CurrentSession.Resources.GetResource("UNKNOWN","Unknown","").Text;}
				}
				else
				{
					labSelectedHelpCaption.Text = Session.CurrentSession.Resources.GetResource("UNKNOWN","Unknown","").Text;
					labSelectedHelpDesc.Text = Session.CurrentSession.Resources.GetResource("UNKNOWN","Unknown","").Text;
				}
			}
		}


		private void txtFilterAvailable_TextChanged(object sender, System.EventArgs e)
		{
			string searchstring = FWBS.Common.SQLRoutines.RemoveRubbish(txtFilterAvailable.Text);
			searchstring = searchstring.Replace("[","ÿþýÊ");
			searchstring = searchstring.Replace("]","Êÿýþ");
			searchstring = searchstring.Replace("ÿþýÊ","[[]");
			searchstring = searchstring.Replace("Êÿýþ","[]]");
			searchstring = searchstring.Replace("%","[%]");
			_inputdata.DefaultView.RowFilter = "Display Like '" + searchstring + "*'";
		}

		private void txtFilterSelected_TextChanged(object sender, System.EventArgs e)
		{
			string searchstring = FWBS.Common.SQLRoutines.RemoveRubbish(txtFilterSelected.Text);
			searchstring = searchstring.Replace("[","ÿþýÊ");
			searchstring = searchstring.Replace("]","Êÿýþ");
			searchstring = searchstring.Replace("ÿþýÊ","[[]");
			searchstring = searchstring.Replace("Êÿýþ","[]]");
			searchstring = searchstring.Replace("%","[%]");
			_outputdata.DefaultView.RowFilter = "Display Like '" + searchstring + "*'";
		}
	}


}
