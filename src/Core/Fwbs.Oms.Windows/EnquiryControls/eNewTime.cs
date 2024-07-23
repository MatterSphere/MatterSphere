using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.StatusManagement;
using FWBS.OMS.StatusManagement.Activities;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for eNewTime.
    /// </summary>
    public class eNewTime : System.Windows.Forms.UserControl,FWBS.Common.UI.IBasicEnquiryControl2
	{
		#region Fields
		/// <summary>
		/// Holds the hit test information for the current row chosen.
		/// </summary>
		private DataGrid.HitTestInfo _currentItem = null;

		/// <summary>
		/// The Flash Color Position from Red to White
		/// </summary>
		private int flashcolorpos = 0;
		/// <summary>
		/// If True Move from Red to White else White to Red
		/// </summary>
		private bool flashcolorfwd = true;
		// None Auto Build Fields
		private OMSFile _omsfile;
		private OMSDocument _omsdocument;
		private decimal _creditlimit = -1;
		private decimal _workinprogress = -1;
		private decimal _available = -1;
		private Int64 _clientid = -1;
		private Int64 _fileid = -1;
		private System.Globalization.NumberFormatInfo _currencyformat = null;
		private bool _required = false;
		private bool _omsdesignmode = false;
		private bool _runonce = false;
		private TimeRecord _currenttime = null;
		private int _position = -1;
		private FWBS.Common.UI.IListEnquiryControl cmbActivities;
		private object _value = "OK";
		private TimeCollection _currenttimelist = null;
		private bool _createonload = false;
		private bool _isdirty = false;
		private TabPage doctime = null;
		private ucSearchControl ucsDocTime=null;
		private Control cboFileLACategory = null;
		private FWBS.Common.DateTimeNULL _lastdate = DateTime.Today;
		private FWBS.OMS.FeeEarner _lastfee = null;
		private int _lastlegalaid = -1; 
		//
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolBarButton tbNew;
		private System.Windows.Forms.ToolBarButton tbDelete;
		private System.Windows.Forms.ImageList imgTools;
		private System.Windows.Forms.TabPage tpTimeEntry;
		private System.Windows.Forms.Label label3;
        private FWBS.OMS.UI.Windows.DataGridEx dgTimeSheet;
		private System.Windows.Forms.DataGridTableStyle dgsTimeRecording;
		private System.Windows.Forms.DataGridTextBoxColumn dgcDescription;
		private System.Windows.Forms.DataGridTextBoxColumn gdcActivity;
		private System.Windows.Forms.Label labCreditLimit;
		private FWBS.Common.UI.Windows.ToolBar tbTime;
		private FWBS.OMS.UI.Windows.EnquiryForm enqNewTime;
		private System.Windows.Forms.DataGridTextBoxColumn dgSp;
		private System.Windows.Forms.ToolBarButton tbEdit;
		private System.Windows.Forms.ToolBarButton tbCancel;
		private System.Windows.Forms.CheckBox chkSkipTime;
		private FWBS.OMS.UI.Windows.ucTimeRecording ucTimeRecording1;
		private System.Windows.Forms.TabPage tpConfirmed;
		private TabControl tcTimeRecording;
		private System.Windows.Forms.TabPage tpTimeSheet;
		private System.Windows.Forms.Label labWIP;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labAvailable;
		private System.Windows.Forms.Timer timFlash;
		private System.Windows.Forms.Label lblAvailable;
		private System.Windows.Forms.Panel pnlStatusBar;
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
        private FWBS.Common.UI.Windows.DataGridLabelColumn dgcDate;
        private Label labWarning;
        private ucAlert alertTimeActivity;
        private Panel pnlToolbar;
		private System.Windows.Forms.DataGridTextBoxColumn dgcUnits;
		#endregion

		#region Constructors
        public eNewTime()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            tbTime.ImageList = ImageListSelector.GetImageList();
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
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(eNewTime));
            this.imgTools = new System.Windows.Forms.ImageList(this.components);
            this.pnlStatusBar = new System.Windows.Forms.Panel();
            this.chkSkipTime = new System.Windows.Forms.CheckBox();
            this.labAvailable = new System.Windows.Forms.Label();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labWIP = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labCreditLimit = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timFlash = new System.Windows.Forms.Timer(this.components);
            this.labWarning = new System.Windows.Forms.Label();
            this.tbTime = new FWBS.Common.UI.Windows.ToolBar();
            this.tbNew = new System.Windows.Forms.ToolBarButton();
            this.tbEdit = new System.Windows.Forms.ToolBarButton();
            this.tbDelete = new System.Windows.Forms.ToolBarButton();
            this.tbCancel = new System.Windows.Forms.ToolBarButton();
            this.alertTimeActivity = new FWBS.OMS.UI.Windows.ucAlert();
            this.tcTimeRecording = new FWBS.OMS.UI.TabControl();
            this.tpTimeEntry = new System.Windows.Forms.TabPage();
            this.enqNewTime = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.tpConfirmed = new System.Windows.Forms.TabPage();
            this.dgTimeSheet = new FWBS.OMS.UI.Windows.DataGridEx();
            this.dgsTimeRecording = new System.Windows.Forms.DataGridTableStyle();
            this.dgSp = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcDate = new FWBS.Common.UI.Windows.DataGridLabelColumn();
            this.dgcDescription = new System.Windows.Forms.DataGridTextBoxColumn();
            this.gdcActivity = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcUnits = new System.Windows.Forms.DataGridTextBoxColumn();
            this.tpTimeSheet = new System.Windows.Forms.TabPage();
            this.ucTimeRecording1 = new FWBS.OMS.UI.Windows.ucTimeRecording();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.pnlStatusBar.SuspendLayout();
            this.tcTimeRecording.SuspendLayout();
            this.tpTimeEntry.SuspendLayout();
            this.tpConfirmed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTimeSheet)).BeginInit();
            this.tpTimeSheet.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgTools
            // 
            this.imgTools.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTools.ImageStream")));
            this.imgTools.TransparentColor = System.Drawing.Color.Transparent;
            this.imgTools.Images.SetKeyName(0, "");
            this.imgTools.Images.SetKeyName(1, "");
            this.imgTools.Images.SetKeyName(2, "");
            this.imgTools.Images.SetKeyName(3, "");
            // 
            // pnlStatusBar
            // 
            this.pnlStatusBar.Controls.Add(this.chkSkipTime);
            this.pnlStatusBar.Controls.Add(this.labAvailable);
            this.pnlStatusBar.Controls.Add(this.lblAvailable);
            this.pnlStatusBar.Controls.Add(this.label4);
            this.pnlStatusBar.Controls.Add(this.labWIP);
            this.pnlStatusBar.Controls.Add(this.label2);
            this.pnlStatusBar.Controls.Add(this.label1);
            this.pnlStatusBar.Controls.Add(this.labCreditLimit);
            this.pnlStatusBar.Controls.Add(this.label3);
            this.pnlStatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatusBar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlStatusBar.Location = new System.Drawing.Point(0, 410);
            this.pnlStatusBar.Name = "pnlStatusBar";
            this.pnlStatusBar.Padding = new System.Windows.Forms.Padding(5);
            this.pnlStatusBar.Size = new System.Drawing.Size(689, 30);
            this.pnlStatusBar.TabIndex = 13;
            // 
            // chkSkipTime
            // 
            this.chkSkipTime.AutoSize = true;
            this.chkSkipTime.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSkipTime.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkSkipTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.chkSkipTime.Location = new System.Drawing.Point(603, 5);
            this.resourceLookup1.SetLookup(this.chkSkipTime, new FWBS.OMS.UI.Windows.ResourceLookupItem("SkipTime", "Skip Time", ""));
            this.chkSkipTime.Name = "chkSkipTime";
            this.chkSkipTime.Size = new System.Drawing.Size(81, 20);
            this.chkSkipTime.TabIndex = 4;
            this.chkSkipTime.Text = "Skip Time";
            this.chkSkipTime.CheckedChanged += new System.EventHandler(this.chkSkipTime_CheckedChanged);
            // 
            // labAvailable
            // 
            this.labAvailable.AutoSize = true;
            this.labAvailable.Dock = System.Windows.Forms.DockStyle.Left;
            this.labAvailable.Location = new System.Drawing.Point(370, 5);
            this.labAvailable.Name = "labAvailable";
            this.labAvailable.Size = new System.Drawing.Size(44, 15);
            this.labAvailable.TabIndex = 7;
            this.labAvailable.Text = "£100.00";
            this.labAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoSize = true;
            this.lblAvailable.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblAvailable.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAvailable.Location = new System.Drawing.Point(295, 5);
            this.resourceLookup1.SetLookup(this.lblAvailable, new FWBS.OMS.UI.Windows.ResourceLookupItem("AVAILABLE", "   Available : ", ""));
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(75, 15);
            this.lblAvailable.TabIndex = 8;
            this.lblAvailable.Text = "   Available : ";
            this.lblAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(281, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 20);
            this.label4.TabIndex = 10;
            // 
            // labWIP
            // 
            this.labWIP.AutoSize = true;
            this.labWIP.Dock = System.Windows.Forms.DockStyle.Left;
            this.labWIP.Location = new System.Drawing.Point(237, 5);
            this.labWIP.Name = "labWIP";
            this.labWIP.Size = new System.Drawing.Size(44, 15);
            this.labWIP.TabIndex = 5;
            this.labWIP.Text = "£100.00";
            this.labWIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(144, 5);
            this.resourceLookup1.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("FILEWIP", "   %FILE% WIP : ", ""));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "   %FILE% WIP : ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(130, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 20);
            this.label1.TabIndex = 9;
            // 
            // labCreditLimit
            // 
            this.labCreditLimit.AutoSize = true;
            this.labCreditLimit.Dock = System.Windows.Forms.DockStyle.Left;
            this.labCreditLimit.Location = new System.Drawing.Point(86, 5);
            this.labCreditLimit.Name = "labCreditLimit";
            this.labCreditLimit.Size = new System.Drawing.Size(44, 15);
            this.labCreditLimit.TabIndex = 2;
            this.labCreditLimit.Text = "£100.00";
            this.labCreditLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(5, 5);
            this.resourceLookup1.SetLookup(this.label3, new FWBS.OMS.UI.Windows.ResourceLookupItem("CREDITLIMIT", "Credit Limit : ", ""));
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Credit Limit : ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timFlash
            // 
            this.timFlash.Tick += new System.EventHandler(this.timFlash_Tick);
            // 
            // labWarning
            // 
            this.labWarning.BackColor = System.Drawing.Color.SandyBrown;
            this.labWarning.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labWarning.Location = new System.Drawing.Point(0, 391);
            this.resourceLookup1.SetLookup(this.labWarning, new FWBS.OMS.UI.Windows.ResourceLookupItem("TIMEWARNING", "Time recording has already been recorded against this document", ""));
            this.labWarning.Name = "labWarning";
            this.labWarning.Size = new System.Drawing.Size(689, 19);
            this.labWarning.TabIndex = 15;
            this.labWarning.Text = "Time recording has already been recorded against this document";
            this.labWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labWarning.Visible = false;
            // 
            // tbTime
            // 
            this.tbTime.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbTime.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbNew,
            this.tbEdit,
            this.tbDelete,
            this.tbCancel});
            this.tbTime.Divider = false;
            this.tbTime.DropDownArrows = true;
            this.tbTime.ImageList = this.imgTools;
            this.tbTime.Location = new System.Drawing.Point(0, 0);
            this.tbTime.Name = "tbTime";
            this.tbTime.ShowToolTips = true;
            this.tbTime.Size = new System.Drawing.Size(689, 26);
            this.tbTime.TabIndex = 10;
            this.tbTime.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.tbTime.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbTime_ButtonClick);
            // 
            // tbNew
            // 
            this.tbNew.ImageIndex = 0;
            this.resourceLookup1.SetLookup(this.tbNew, new FWBS.OMS.UI.Windows.ResourceLookupItem("CreateNew2", "Create && Ne&w", ""));
            this.tbNew.Name = "tbNew";
            this.tbNew.Text = "Create && Ne&w";
            // 
            // tbEdit
            // 
            this.tbEdit.Enabled = false;
            this.tbEdit.ImageIndex = 2;
            this.resourceLookup1.SetLookup(this.tbEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "&Edit", ""));
            this.tbEdit.Name = "tbEdit";
            this.tbEdit.Text = "&Edit";
            // 
            // tbDelete
            // 
            this.tbDelete.Enabled = false;
            this.tbDelete.ImageIndex = 1;
            this.resourceLookup1.SetLookup(this.tbDelete, new FWBS.OMS.UI.Windows.ResourceLookupItem("Delete", "&Delete", ""));
            this.tbDelete.Name = "tbDelete";
            this.tbDelete.Text = "&Delete";
            // 
            // tbCancel
            // 
            this.tbCancel.Enabled = false;
            this.tbCancel.ImageIndex = 3;
            this.resourceLookup1.SetLookup(this.tbCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("Undo", "&Undo", ""));
            this.tbCancel.Name = "tbCancel";
            this.tbCancel.Text = "&Undo";
            // 
            // alertTimeActivity
            // 
            this.alertTimeActivity.BackColor = System.Drawing.Color.Transparent;
            this.alertTimeActivity.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.alertTimeActivity.Location = new System.Drawing.Point(0, 318);
            this.alertTimeActivity.Name = "alertTimeActivity";
            this.alertTimeActivity.Size = new System.Drawing.Size(689, 73);
            this.alertTimeActivity.TabIndex = 1;
            this.alertTimeActivity.Visible = false;
            // 
            // tcTimeRecording
            // 
            this.tcTimeRecording.Controls.Add(this.tpTimeEntry);
            this.tcTimeRecording.Controls.Add(this.tpConfirmed);
            this.tcTimeRecording.Controls.Add(this.tpTimeSheet);
            this.tcTimeRecording.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTimeRecording.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tcTimeRecording.Location = new System.Drawing.Point(0, 28);
            this.tcTimeRecording.Name = "tcTimeRecording";
            this.tcTimeRecording.SelectedIndex = 0;
            this.tcTimeRecording.Size = new System.Drawing.Size(689, 363);
            this.tcTimeRecording.TabIndex = 12;
            this.tcTimeRecording.SelectedIndexChanged += new System.EventHandler(this.tcTimeRecording_SelectedIndexChanged);
            // 
            // tpTimeEntry
            // 
            this.tpTimeEntry.Controls.Add(this.enqNewTime);
            this.tpTimeEntry.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpTimeEntry, new FWBS.OMS.UI.Windows.ResourceLookupItem("TimeEntry", "Time Entry", ""));
            this.tpTimeEntry.Name = "tpTimeEntry";
            this.tpTimeEntry.Size = new System.Drawing.Size(681, 335);
            this.tpTimeEntry.TabIndex = 1;
            this.tpTimeEntry.Text = "Time Entry";
            // 
            // enqNewTime
            // 
            this.enqNewTime.AutoScroll = true;
            this.enqNewTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enqNewTime.IsDirty = false;
            this.enqNewTime.Location = new System.Drawing.Point(0, 0);
            this.enqNewTime.Name = "enqNewTime";
            this.enqNewTime.Size = new System.Drawing.Size(681, 335);
            this.enqNewTime.TabIndex = 0;
            this.enqNewTime.ToBeRefreshed = false;
            this.enqNewTime.Rendered += new System.EventHandler(this.enqNewTime_Rendered);
            // 
            // tpConfirmed
            // 
            this.tpConfirmed.Controls.Add(this.dgTimeSheet);
            this.tpConfirmed.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpConfirmed, new FWBS.OMS.UI.Windows.ResourceLookupItem("ToBeConfirmed", "To Be Confirmed", ""));
            this.tpConfirmed.Name = "tpConfirmed";
            this.tpConfirmed.Size = new System.Drawing.Size(681, 335);
            this.tpConfirmed.TabIndex = 0;
            this.tpConfirmed.Text = "To Be Confirmed";
            // 
            // dgTimeSheet
            // 
            this.dgTimeSheet.AllowNavigation = false;
            this.dgTimeSheet.CaptionVisible = false;
            this.dgTimeSheet.DataMember = "";
            this.dgTimeSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgTimeSheet.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dgTimeSheet.Location = new System.Drawing.Point(0, 0);
            this.dgTimeSheet.Name = "dgTimeSheet";
            this.dgTimeSheet.RowHeaderWidth = 16;
            this.dgTimeSheet.Size = new System.Drawing.Size(681, 335);
            this.dgTimeSheet.TabIndex = 0;
            this.dgTimeSheet.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dgsTimeRecording});
            this.dgTimeSheet.CurrentCellChanged += new System.EventHandler(this.dgTimeSheet_CurrentCellChanged);
            this.dgTimeSheet.DoubleClick += new System.EventHandler(this.dgTimeSheet_DoubleClick);
            this.dgTimeSheet.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgTimeSheet_MouseDown);
            // 
            // dgsTimeRecording
            // 
            this.dgsTimeRecording.DataGrid = this.dgTimeSheet;
            this.dgsTimeRecording.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dgSp,
            this.dgcDate,
            this.dgcDescription,
            this.gdcActivity,
            this.dgcUnits});
            this.dgsTimeRecording.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None;
            this.dgsTimeRecording.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dgsTimeRecording.MappingName = "TIMERECORDS";
            this.dgsTimeRecording.ReadOnly = true;
            this.dgsTimeRecording.SelectionForeColor = System.Drawing.SystemColors.Window;
            // 
            // dgSp
            // 
            this.dgSp.Format = "";
            this.dgSp.FormatInfo = null;
            this.dgSp.ReadOnly = true;
            this.dgSp.Width = 0;
            // 
            // dgcDate
            // 
            this.dgcDate.AllowMultiSelect = false;
            this.dgcDate.DisplayDateAs = FWBS.OMS.SearchEngine.SearchColumnsDateIs.Local;
            this.dgcDate.Format = "d";
            this.dgcDate.FormatInfo = null;
            this.dgcDate.HeaderText = "Date";
            this.dgcDate.ImageColumn = "";
            this.dgcDate.ImageIndex = -1;
            this.dgcDate.ImageList = null;
            this.dgcDate.MappingName = "timeRecorded";
            this.dgcDate.ReadOnly = true;
            this.dgcDate.SearchList = null;
            this.dgcDate.SourceDateIs = FWBS.OMS.SearchEngine.SearchColumnsDateIs.NotApplicable;
            this.dgcDate.Width = 75;
            // 
            // dgcDescription
            // 
            this.dgcDescription.Format = "";
            this.dgcDescription.FormatInfo = null;
            this.dgcDescription.HeaderText = "Description";
            this.dgcDescription.MappingName = "timedesc";
            this.dgcDescription.ReadOnly = true;
            this.dgcDescription.Width = 240;
            // 
            // gdcActivity
            // 
            this.gdcActivity.Format = "";
            this.gdcActivity.FormatInfo = null;
            this.gdcActivity.HeaderText = "Activity";
            this.gdcActivity.MappingName = "timeActivityCodeDesc";
            this.gdcActivity.ReadOnly = true;
            this.gdcActivity.Width = 120;
            // 
            // dgcUnits
            // 
            this.dgcUnits.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.dgcUnits.Format = "";
            this.dgcUnits.FormatInfo = null;
            this.dgcUnits.HeaderText = "Units";
            this.dgcUnits.MappingName = "timeUnits";
            this.dgcUnits.ReadOnly = true;
            this.dgcUnits.Width = 75;
            // 
            // tpTimeSheet
            // 
            this.tpTimeSheet.Controls.Add(this.ucTimeRecording1);
            this.tpTimeSheet.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpTimeSheet, new FWBS.OMS.UI.Windows.ResourceLookupItem("TimeLedger", "Time Ledger", ""));
            this.tpTimeSheet.Name = "tpTimeSheet";
            this.tpTimeSheet.Size = new System.Drawing.Size(681, 335);
            this.tpTimeSheet.TabIndex = 2;
            this.tpTimeSheet.Text = "Time Ledger";
            // 
            // ucTimeRecording1
            // 
            this.ucTimeRecording1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTimeRecording1.Location = new System.Drawing.Point(0, 0);
            this.ucTimeRecording1.Name = "ucTimeRecording1";
            this.ucTimeRecording1.Size = new System.Drawing.Size(681, 335);
            this.ucTimeRecording1.TabIndex = 0;
            this.ucTimeRecording1.ToBeRefreshed = false;
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.Controls.Add(this.tbTime);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlToolbar.Location = new System.Drawing.Point(0, 0);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(689, 28);
            this.pnlToolbar.TabIndex = 16;
            // 
            // eNewTime
            // 
            this.Controls.Add(this.alertTimeActivity);
            this.Controls.Add(this.tcTimeRecording);
            this.Controls.Add(this.labWarning);
            this.Controls.Add(this.pnlStatusBar);
            this.Controls.Add(this.pnlToolbar);
            this.Name = "eNewTime";
            this.Size = new System.Drawing.Size(689, 440);
            this.Load += new System.EventHandler(this.eNewTime_Load);
            this.VisibleChanged += new System.EventHandler(this.eNewTime_VisibleChanged);
            this.ParentChanged += new System.EventHandler(this.eNewTime_ParentChanged);
            this.pnlStatusBar.ResumeLayout(false);
            this.pnlStatusBar.PerformLayout();
            this.tcTimeRecording.ResumeLayout(false);
            this.tpTimeEntry.ResumeLayout(false);
            this.tpConfirmed.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgTimeSheet)).EndInit();
            this.tpTimeSheet.ResumeLayout(false);
            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region IBasicEnquiryControl2
		/// <summary>
		/// Executes the changed event.
		/// </summary>
		public void OnChanged()
		{
			if (Changed!= null && IsDirty)
				Changed(this, EventArgs.Empty);
			IsDirty = false;
		}
		
		public void OnActiveChanged()
		{
			IsDirty = true;
			if (ActiveChanged!= null)
				ActiveChanged(this, EventArgs.Empty);
		}
		
		[Category("Action")]
		public event EventHandler Changed;

		[Category("Action")]
		public event EventHandler ActiveChanged;

        [Browsable(false)]
        [DefaultValue(false)]
		public bool omsDesignMode
		{
			get
			{
				return _omsdesignmode;
			}
			set
			{
				_omsdesignmode = value;
			}
		}
		
		public int CaptionWidth
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

        [Browsable(false)]
        public bool CaptionTop
        {
            get
            {
                return false;
            }
            set { }
        }

		public object Control
		{
			get
			{
				return null;
			}
		}

        [Browsable(false)]
		public bool LockHeight
		{
			get
			{
				return false;
			}
		}

		public bool ReadOnly
		{
			get
			{
				return !this.Enabled;
			}
			set
			{
				this.Enabled = !value;
			}
		}

		public bool Required
		{
			get
			{
				return _required;
			}
			set
			{
				_required = value;
			}
		}

		public object Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		[Browsable(false)]
		public bool IsDirty
		{
			get
			{
				return _isdirty;
			}
			set
			{
				_isdirty = value;
			}
		}

		#endregion

		#region Properties
		[Category("OMS")]
		public bool ShowSkipTime
		{
			get
			{
				return chkSkipTime.Visible;
			}
			set
			{
				chkSkipTime.Visible = value;
			}
		}

		[Category("OMS")]
		public int PaddingAll
		{
			get
			{
				return this.DockPadding.All;
			}
			set
			{
				this.DockPadding.All = value;
			}
		}

		[Browsable(false)]
		public Int64 ClientID
		{
			get
			{
				return _clientid;
			}
			set
			{
				_clientid = value;
			}
		}

		[Browsable(false)]
		public Int64 FileID
		{
			get
			{
				return _fileid;
			}
			set
			{
				_fileid = value;
			}
		}

		[Browsable(false)]
		public decimal CreditLimit
		{
			get
			{
				return _creditlimit;
			}
			set
			{
				_creditlimit = value;
				labCreditLimit.Text = String.Format(this.CurrencyFormat, "{0:C}", this.CreditLimit);
			}
		}

		[Browsable(false)]
		public decimal WorkInProgress
		{
			get
			{
				return _workinprogress;
			}
			set
			{
				_workinprogress = value;
				labWIP.Text = String.Format(this.CurrencyFormat, "{0:C}", this.WorkInProgress);
			}
		}

		[Browsable(false)]
		public decimal Available
		{
			get
			{
				return _available;
			}
			set
			{
				_available = value;
				labAvailable.Text = String.Format(this.CurrencyFormat, "{0:C}", this.Available);
			}
		}

		/// <summary>
		/// Returns the Number Format Info for the Current File.
		/// </summary>
		[Browsable(false)]
		public System.Globalization.NumberFormatInfo CurrencyFormat
		{
			get
			{
				return _currencyformat;
			}
			set
			{
				_currencyformat = value;
			}
		}

		/// <summary>
		/// Create a New Time entry on Load
		/// </summary>
		[Category("OMS")]
        public bool CreateOnLoad
        {
            get
            {
                return _createonload;
            }
            set
            {
                _createonload = value;
            }
        }

        #endregion

        #region Private

        private FWBS.OMS.UI.Windows.EnquiryForm _enqform;
        /// <summary>
        /// Grabs the Enquiry Form and thus the business object it contains
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eNewTime_ParentChanged(object sender, System.EventArgs e)
        {
            if (this.Parent is FWBS.OMS.UI.Windows.EnquiryForm)
            {
               _enqform = (FWBS.OMS.UI.Windows.EnquiryForm)this.Parent;
                var obj = _enqform.Enquiry.Object;
                if(!_enqform.Enquiry.InDesignMode)
                    SetBusinessObject(obj, false);
                _enqform.Finishing += new CancelEventHandler(enqNewTime_Finishing);
                _enqform.Finished += new EventHandler(_enqform_Finished);
            }

            if (Parent == null)
            {
                if (_omsdocument != null)
                {
                    _omsdocument.PropertyChanged -= new FWBS.OMS.EnquiryEngine.PropertyChangedEventHandler(_omsdocument_PropertyChanged);
                }
                _enqform.Finishing -= new CancelEventHandler(enqNewTime_Finishing);
                _enqform.Finished -= new EventHandler(_enqform_Finished);
                _enqform = null;
                _omsfile = null;
                _omsdocument = null;
                if (_currenttimelist != null)
                {
                    _currenttimelist.SetBusinessObject(null);
                    _currenttimelist.Clear();
                }
            }

            if (_omsdocument != null && _omsdocument.IsNew == false)
                CancelItem(false);
        }

        private void SetBusinessObject(object obj, bool clearTime)
        {
            if (obj is OMSFile)
                _omsfile = (OMSFile)obj;
            if (obj is OMSDocument)
            {
                _omsdocument = (OMSDocument)obj;
                if (_omsdocument.IsNew == false)
                {
                    doctime = new TabPage(FWBS.OMS.Session.CurrentSession.Resources.GetResource("PREDOCTIME", "Previous Document Time", "").Text);
                    ucsDocTime = new ucSearchControl();
                    ucsDocTime.SetSearchList(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.DocumentTimeRecording), _omsdocument, new FWBS.Common.KeyValueCollection());
                    ucsDocTime.SearchCompleted += new SearchCompletedEventHandler(ucsDocTime_SearchCompleted);
                    ucsDocTime.Search();
                    ucsDocTime.Dock = DockStyle.Fill;
                    doctime.Controls.Add(ucsDocTime);
                    tcTimeRecording.TabPages.Add(doctime);
                }
                _omsdocument.PropertyChanged += new FWBS.OMS.EnquiryEngine.PropertyChangedEventHandler(_omsdocument_PropertyChanged);
            }

            if (_currenttimelist != null && clearTime)
            {
                _currenttimelist.Clear();
                CancelItem(false);
            }
            SetDefaults();
        }

        public void UpdateBusinessObject(object obj)
        {
            SetBusinessObject(obj, true);
        }


        void ucsDocTime_SearchCompleted(object sender, SearchCompletedEventArgs e)
        {
            labWarning.Visible = (e.Count > 0);
        }



		private void SetDefaults()
		{

			if (_omsdocument != null && _runonce == false)
			{
				this.CurrencyFormat = _omsdocument.OMSFile.CurrencyFormat;
				this.CreditLimit = _omsdocument.OMSFile.CreditLimit;
				this.ClientID = _omsdocument.OMSFile.ClientID;
				this.FileID = _omsdocument.OMSFile.ID;
				this.WorkInProgress = _omsdocument.OMSFile.TimeWIP;
				this.Available = _omsdocument.OMSFile.CreditLimit - (_omsdocument.OMSFile.TimeWIP + _omsdocument.OMSFile.TimeBilled);
				_omsfile = _omsdocument.OMSFile;
				_currenttimelist = _omsdocument.TimeRecords;
				chkSkipTime.Checked = _currenttimelist.SkipTime;

                DetachEvents();

                if (_currenttimelist.Count == 0)
					_currenttime = new TimeRecord(_omsdocument,true);
				else
				{
					_position = 0;
					_currenttime = _currenttimelist[_position];
				}
                AttachEvents();

				enqNewTime.Enquiry = _currenttime.Edit(null);

				cboFileLACategory = enqNewTime.GetControl("cboFileLACategory");
				cboFileLACategory.Visible = _omsfile.FundingType.LegalAidCharged;

				dgTimeSheet.DataSource = _currenttimelist.GetTimeRecords();
				_currenttimelist.GetTimeRecords().DefaultView.AllowNew = false;
				cmbActivities = enqNewTime.GetIListEnquiryControl("cmbActivities");

				if (cmbActivities != null && _currenttime != null) 
				{
					cmbActivities.AddItem(_currenttime.Activities.GetDataTable());
					enqNewTime.GetIBasicEnquiryControl2("cmbActivities").Value = enqNewTime.Enquiry.Source.Tables["DATA"].Rows[0]["cmbActivities"];
				}

				if (_omsdocument.OMSFile.IsCreditWarning)
				{
					lblAvailable.BackColor = Color.Red;
					labAvailable.BackColor = Color.Red;
					timFlash.Enabled=true;
				}
				else
				{
					timFlash.Enabled=false;
					lblAvailable.BackColor = this.BackColor;
					labAvailable.BackColor = this.BackColor;

				}
				enqNewTime.Dirty -= new EventHandler(_currenttime_Dirty);
				enqNewTime.Dirty += new EventHandler(_currenttime_Dirty);
			}
			else if (_omsfile != null && _runonce == false)
			{
				_currenttimelist = new TimeCollection(_omsfile);
				this.CurrencyFormat = _omsfile.CurrencyFormat;
				this.CreditLimit = _omsfile.CreditLimit;
				this.ClientID = _omsfile.ClientID;
				this.FileID = _omsfile.ID;
				this.WorkInProgress = _omsfile.TimeWIP;
				this.Available = _omsfile.CreditLimit - (_omsfile.TimeWIP + _omsfile.TimeBilled);

				if (_omsfile.IsCreditWarning)
				{
					lblAvailable.BackColor = Color.Red;
					labAvailable.BackColor = Color.Red;
					timFlash.Enabled=true;
				}
				else
				{
					timFlash.Enabled=false;
					lblAvailable.BackColor = this.BackColor;
					labAvailable.BackColor = this.BackColor;
				}
			}

            //Client and Matter Status - Time Entry check
            if (!new FileActivity(FWBS.OMS.OMSFile.GetFile(_fileid), FileStatusActivityType.TimeEntry).IsAllowed())
            {
                chkSkipTime.Checked = true;
                chkSkipTime.Enabled = false;
                tcTimeRecording.Enabled = false;
                tbTime.Enabled = false;

                Alert timeAlert = new Alert(Session.CurrentSession.Resources.GetResource("ERRCLMSTDENYTME", "Activity Denied", "").Text, Alert.AlertStatus.Red);
                this.alertTimeActivity.SetAlerts(new Alert [] {timeAlert});
                alertTimeActivity.Visible = true; 
            }

		}

		private void tbTime_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			tcTimeRecording.Focus();
			Application.DoEvents();
			if (e.Button == tbNew)
			{
				tcTimeRecording.SelectedTab = tpTimeEntry;
				AddAndCreate(Session.CurrentSession.Resources.GetResource("TIMERECMANT", "MANUALTIME:","").Text);
				enqNewTime.Focus();
			}
			else if (e.Button == tbEdit)
			{
				enqNewTime.Visible=false;
				enqNewTime.Enquiry = null;
				
                Global.RemoveAndDisposeControls(enqNewTime);
				
                _position = dgTimeSheet.CurrentRowIndex;

                DetachEvents();

				_currenttime = _currenttimelist[_position];

                AttachEvents();

				enqNewTime.Enquiry = _currenttime.Edit(null);
				
				cboFileLACategory = enqNewTime.GetControl("cboFileLACategory");
				cboFileLACategory.Visible = _omsfile.FundingType.LegalAidCharged;

				cmbActivities = enqNewTime.GetIListEnquiryControl("cmbActivities");
				if (cmbActivities != null)
					cmbActivities.AddItem(_currenttime.Activities.GetDataTable());
				enqNewTime.GetIBasicEnquiryControl2("cmbActivities").Value = _currenttime.ActivityCode;
				tcTimeRecording.SelectedTab = tpTimeEntry;
				enqNewTime.Focus();
				enqNewTime.Visible=true;
				enqNewTime.Dirty -= new EventHandler(_currenttime_Dirty);
				enqNewTime.Dirty += new EventHandler(_currenttime_Dirty);
				_currenttime.IsDirty=false;
				tbCancel.Enabled = false;
			}
			else if (e.Button == tbDelete)
			{
				if (_currenttime != null && _currenttime.Index == dgTimeSheet.CurrentRowIndex)
				{
					enqNewTime.Enquiry = null;

					Global.RemoveAndDisposeControls(enqNewTime);

                    DetachEvents();

                    _currenttime = null;
					tbCancel.Enabled=false;
				}
				_position = dgTimeSheet.CurrentRowIndex;
				_currenttimelist.RemoveAt(_position);
				dgTimeSheet.Refresh();
				tbDelete.Enabled = (_currenttimelist.Count !=0);
				tbEdit.Enabled = (_currenttimelist.Count !=0);
			}
			else if (e.Button == tbCancel)
			{
                CancelItem();
			}
		}

        private void CancelItem()
        {
            CancelItem(true);
        }

        private void CancelItem(bool tabMove)
        {
            enqNewTime.Enquiry = null;

            Global.RemoveAndDisposeControls(enqNewTime);

            DetachEvents();

            _currenttime = null;
            tbCancel.Enabled = false;
            if (tabMove)
                tcTimeRecording.SelectedTab = tpConfirmed;
        }

        private void enqNewTime_Finishing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_currenttimelist.SkipTime == false && Session.CurrentSession.IsLicensedFor("TIMEREC") && _currenttime != null && _currenttime.IsDirty)
            {
                try
                {
                    AddAndCreate(null);
                    enqNewTime.ReBind();
                    enqNewTime.UpdateItem();
                    this.Value = "OK";
                    OnChanged();
                }
                catch (OMSException2 ex)
                {
                    ((EnquiryForm)sender).GotoControl(this.Name);
                    enqNewTime.GetControlByProperty(ex.Property).Focus();
                    ErrorBox.Show(ParentForm, ex);
                    e.Cancel = true;
                    _currenttime.IsDirty = true;
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ParentForm, ex);
                    e.Cancel = true;
                    _currenttime.IsDirty = true;
                }
            }
            else
            {
                this.Value = "OK";
                OnChanged();
            }
        }


		private void AddAndCreate(object NewTimeDescription)
		{
			try
			{
				if (_currenttime != null && _currenttime.IsDirty)
				{
                    //UTCFIX: DM - 30/11/06
					if (_currenttime.TimeDate.ToLocalTime() > DateTime.Now)
                        if (MessageBox.Show(
                                Session.CurrentSession.Resources.GetMessage("MSGBKTIMFUT", "Are you sure you wish to book time for the future?", "").Text,
                                Session.CurrentSession.Resources.GetResource("TIMEREC", "Time Recording", "").Text, 
                                MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
						{
							this.Value = DBNull.Value;
							return;
						}

					_lastdate = _currenttime.TimeDate;
					_lastfee = _currenttime.FeeEarner;
					_lastlegalaid = _currenttime.LegalAidCategory;

					if (chkSkipTime.Checked==false)
						_currenttime.Update();
					
					
					if (_currenttime.TimeParent == null)
						_currenttimelist.Add(_currenttime);
					else
						_currenttimelist[_currenttime.Index] = _currenttime;
						dgTimeSheet.DataSource = _currenttimelist.GetTimeRecords();


					dgTimeSheet.Refresh();
				}
				if (NewTimeDescription != null)
				{
                    DetachEvents();

					_currenttime = new TimeRecord(_omsfile,true,_lastdate,_lastfee,_lastlegalaid);
                    
                    AttachEvents();
						
					_currenttime.TimeDescription = Convert.ToString(NewTimeDescription);
					cmbActivities = null;
					enqNewTime.Enquiry = _currenttime.Edit(null);
					
					cboFileLACategory = enqNewTime.GetControl("cboFileLACategory",FWBS.OMS.UI.Windows.EnquiryControlMissing.Exception);
					cboFileLACategory.Visible = _omsfile.FundingType.LegalAidCharged;

					enqNewTime.Dirty -= new EventHandler(_currenttime_Dirty);
					enqNewTime.Dirty += new EventHandler(_currenttime_Dirty);
					_currenttime_Dirty(this,EventArgs.Empty);
					cmbActivities = enqNewTime.GetIListEnquiryControl("cmbActivities");
					if (cmbActivities != null) cmbActivities.AddItem(_currenttime.Activities.GetDataTable());
				}
			}
			catch(Exception ex)
			{
				if (NewTimeDescription != null) 
					ErrorBox.Show(ParentForm, ex);
				else
					throw ex;
			}
		}

		private void eNewTime_VisibleChanged(object sender, System.EventArgs e)
		{
			if (Visible) enqNewTime.Focus();
		}

		private void dgTimeSheet_CurrentCellChanged(object sender, System.EventArgs e)
		{
			dgTimeSheet.CurrentCell = new DataGridCell(dgTimeSheet.CurrentRowIndex,-1);
			dgTimeSheet.Select(dgTimeSheet.CurrentRowIndex); 
		}

		private void tcTimeRecording_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (tcTimeRecording.SelectedTab == doctime)
			{
				ucsDocTime.Search();
				tbDelete.Enabled = false;
				tbEdit.Enabled = false;
			}
			else if (tcTimeRecording.SelectedTab == tpConfirmed)
			{
				if (_currenttime != null && _currenttime.IsDirty)
				{
					try
					{
						AddAndCreate(null);
						tbCancel.Enabled = false;
					}
					catch (Exception ex)
					{
						ErrorBox.Show(ParentForm, ex);
						tcTimeRecording.SelectedTab = tpTimeEntry;
						return;
					}
				}
				tbDelete.Enabled = (_currenttimelist.Count !=0);
				tbEdit.Enabled = (_currenttimelist.Count !=0);
			}
			else if (tcTimeRecording.SelectedTab == tpTimeSheet)
			{
				tbDelete.Enabled = false;
				tbEdit.Enabled = false;
				if (ucTimeRecording1.IsConnected == false)
					ucTimeRecording1.Connect(_omsfile);
				ucTimeRecording1.SelectItem();
			}
			else
			{
				#if NODATABIND
					tbTime_ButtonClick(this,new ToolBarButtonClickEventArgs(tbEdit));
				#endif
				tbDelete.Enabled = false;
				tbEdit.Enabled = false;
			}
		}

		private void dgTimeSheet_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_currentItem = dgTimeSheet.HitTest(e.X, e.Y);

		}

		private void dgTimeSheet_DoubleClick(object sender, System.EventArgs e)
		{
			if (_currentItem != null)
				tbTime_ButtonClick(sender,new ToolBarButtonClickEventArgs(tbEdit));
		}

		private void _enqform_Finished(object sender, EventArgs e)
		{
			_currenttimelist.Update();
		}
		
		private void _currenttime_Dirty(object sender, EventArgs e)
		{
			if (_currenttimelist.GetTimeRecords().Rows.Count > 0)
				tbCancel.Enabled = enqNewTime.IsDirty;
		}

		private void _currenttime_CancelTime(object sender, EventArgs e)
		{
			enqNewTime.ReBind();
		}

		private void chkSkipTime_CheckedChanged(object sender, System.EventArgs e)
		{
			_currenttimelist.SkipTime = chkSkipTime.Checked;
			tcTimeRecording.Enabled = !chkSkipTime.Checked;
		}

		private void eNewTime_Load(object sender, System.EventArgs e)
		{
            if (Session.CurrentSession.IsLoggedIn)
            {
                label3.Visible = !Session.CurrentSession.HideBalances;
                labCreditLimit.Visible = label3.Visible;
                label2.Visible = label3.Visible;
                labWIP.Visible = label3.Visible;
                lblAvailable.Visible = label3.Visible;
                labAvailable.Visible = label3.Visible;


                if (Session.CurrentSession.HideTimeLedgerTab)
                    tcTimeRecording.TabPages.Remove(tpTimeSheet);
            }
            if (_createonload && omsDesignMode == false)
				tbTime_ButtonClick(sender,new ToolBarButtonClickEventArgs(tbNew));
		}

		private void timFlash_Tick(object sender, System.EventArgs e)
		{
			if (flashcolorfwd) flashcolorpos+=20; else flashcolorpos-=20;
			if (flashcolorpos > 255)
			{
				flashcolorpos = 255;
				flashcolorfwd = false;
			}
			if (flashcolorpos < 0)
			{
				flashcolorpos = 0;
				flashcolorfwd = true;
			}
			lblAvailable.BackColor = Color.FromArgb(255,flashcolorpos,flashcolorpos);
			labAvailable.BackColor = Color.FromArgb(255,flashcolorpos,flashcolorpos);
		}
		#endregion

		private void enqNewTime_Rendered(object sender, System.EventArgs e)
		{
			try
			{
                if (Session.CurrentSession.HideCalculatedChargeCost) // Test
                {
                    Control _currency1 = enqNewTime.GetControl("Currency1", EnquiryControlMissing.Create);
                    Control _currency2 = enqNewTime.GetControl("Currency2", EnquiryControlMissing.Create);
                    Control _label1 = enqNewTime.GetControl("Label1", EnquiryControlMissing.Create);
                    Control _btnUpdate = enqNewTime.GetControl("btnUpdate",EnquiryControlMissing.Create);
                    _currency1.Visible = false;
                    _currency2.Visible = false;
                    _label1.Visible = false;
                    _btnUpdate.Visible = false;
                }

                FWBS.Common.UI.IBasicEnquiryControl2 _feeearner = enqNewTime.GetIBasicEnquiryControl2("cmbFeeEarner",EnquiryControlMissing.Exception);
				_feeearner.ActiveChanged +=new EventHandler(ActivePropertyChanged_ActiveChanged);
				
				FWBS.Common.UI.IBasicEnquiryControl2 _activities = enqNewTime.GetIBasicEnquiryControl2("cmbActivities",EnquiryControlMissing.Exception);
				_activities.ActiveChanged +=new EventHandler(ActivePropertyChanged_ActiveChanged);

				FWBS.Common.UI.IBasicEnquiryControl2 _lacategory = enqNewTime.GetIBasicEnquiryControl2("cboFileLACategory",EnquiryControlMissing.None);
				if (_lacategory != null) _lacategory.ActiveChanged +=new EventHandler(ActivePropertyChanged_ActiveChanged);
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}

		}

		private void ActivePropertyChanged_ActiveChanged(object sender, EventArgs e)
		{
			((FWBS.Common.UI.IBasicEnquiryControl2)sender).OnChanged();
			if (sender == enqNewTime.GetControl("cboFileLACategory"))
			{
				try
				{
					FWBS.Common.UI.IListEnquiryControl c = enqNewTime.GetIListEnquiryControl("cmbActivities");
					c.AddItem(_currenttime.Activities.GetDataTable());
				}
				catch
				{
				}
			}
		}

        private void AttachEvents()
        {
            if (_currenttime != null)
            {
                _currenttime.CancelTime += new EventHandler(_currenttime_CancelTime);
            }	
        }

        private void DetachEvents()
        {
            if (_currenttime != null)
            {
                _currenttime.CancelTime -= new EventHandler(_currenttime_CancelTime);
            }
        }




        private void _omsdocument_PropertyChanged(object sender, FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs e)
        {
            if (e.Property.ToUpperInvariant() == "DESCRIPTION")
            {
                if (e.PreviousValue == Type.Missing || e.PreviousValue == null)
                    return;

                string pv = Convert.ToString(e.PreviousValue);

                if (_currenttime != null && pv.Equals(_currenttime.TimeDescription))
                    _currenttime.TimeDescription = Convert.ToString(e.Value);

            }
        }
	}
}
