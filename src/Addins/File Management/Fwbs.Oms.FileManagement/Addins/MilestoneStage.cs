using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using FWBS.OMS.FileManagement.Milestones;
using FWBS.OMS.StatusManagement;
using FWBS.OMS.StatusManagement.Activities;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.FileManagement.Addins
{
    public class MilestoneStage : System.Windows.Forms.UserControl
	{
		#region Fields

		internal static MilestoneStage LastSelected;

		private static Color cCompleted = Color.FromArgb(210, 252, 210);
		private static Color cOverdue = Color.FromArgb(255, 210, 210);
		private static Color cDue = Color.FromKnownColor(KnownColor.Control);
		private static readonly Font fUnCompleted = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
		private static readonly Font fCompleted = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));

		private const int maxHeight = 171;
		private const int startHeight = 43;

		private Milestones.MilestoneStage _milestoneStage = null;
		private bool resizeafterparentset = false;

		private bool captureEvents = false;
		private bool allowcheck = false;
		private bool selected = false;

		#endregion

		#region Controls

		private System.Windows.Forms.ImageList imlNavigation;
		private System.Windows.Forms.Panel pnlSeperator;
		private System.Windows.Forms.Panel pnlMain;
		private FWBS.Common.UI.Windows.eXPPanel pnlInner;
		private FWBS.Common.UI.Windows.eXPPanel pnlTitle;
		private FWBS.OMS.UI.Windows.ucDateTimePicker dtpMilestoneDue;
		private System.Windows.Forms.Label labMileStoneType;
		private System.Windows.Forms.Label labMilestoneDueDays;
		private System.Windows.Forms.Button btnExpand;
		private FWBS.Common.UI.Windows.eXPPanel pnlTasks;
		private FWBS.Common.UI.Windows.eXPPanel eXPPanel1;
		private System.Windows.Forms.ListView lsvTasks;
		private System.Windows.Forms.ColumnHeader colSubject;
		private System.Windows.Forms.ColumnHeader colDue;
		private System.Windows.Forms.ColumnHeader colCompleted;
		private System.Windows.Forms.CheckBox chkMilestoneDesc;
		private System.Windows.Forms.Label labMilestoneComp;
		private System.Windows.Forms.Label labMilestoneDue;
        private ColumnHeader colTeam;
        private ColumnHeader colAssignedTo;
        private Label lblStageDesc;
        private CultureInfo cultureInfo;

		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructors

		public MilestoneStage()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            cultureInfo = Session.CurrentSession.DefaultCultureInfo;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MilestoneStage));
            this.imlNavigation = new System.Windows.Forms.ImageList(this.components);
            this.pnlSeperator = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlInner = new FWBS.Common.UI.Windows.eXPPanel();
            this.pnlTasks = new FWBS.Common.UI.Windows.eXPPanel();
            this.eXPPanel1 = new FWBS.Common.UI.Windows.eXPPanel();
            this.lsvTasks = new System.Windows.Forms.ListView();
            this.colSubject = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTeam = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAssignedTo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCompleted = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlTitle = new FWBS.Common.UI.Windows.eXPPanel();
            this.labMilestoneDue = new System.Windows.Forms.Label();
            this.lblStageDesc = new System.Windows.Forms.Label();
            this.chkMilestoneDesc = new System.Windows.Forms.CheckBox();
            this.dtpMilestoneDue = new FWBS.OMS.UI.Windows.ucDateTimePicker();
            this.labMileStoneType = new System.Windows.Forms.Label();
            this.labMilestoneComp = new System.Windows.Forms.Label();
            this.labMilestoneDueDays = new System.Windows.Forms.Label();
            this.btnExpand = new System.Windows.Forms.Button();
            this.pnlMain.SuspendLayout();
            this.pnlInner.SuspendLayout();
            this.pnlTasks.SuspendLayout();
            this.eXPPanel1.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // imlNavigation
            // 
            this.imlNavigation.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlNavigation.ImageStream")));
            this.imlNavigation.TransparentColor = System.Drawing.Color.Transparent;
            this.imlNavigation.Images.SetKeyName(0, "");
            this.imlNavigation.Images.SetKeyName(1, "");
            // 
            // pnlSeperator
            // 
            this.pnlSeperator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSeperator.Location = new System.Drawing.Point(0, 32);
            this.pnlSeperator.Name = "pnlSeperator";
            this.pnlSeperator.Size = new System.Drawing.Size(567, 4);
            this.pnlSeperator.TabIndex = 216;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMain.Controls.Add(this.pnlInner);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(1);
            this.pnlMain.Size = new System.Drawing.Size(567, 32);
            this.pnlMain.TabIndex = 217;
            // 
            // pnlInner
            // 
            this.pnlInner.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.Color.White);
            this.pnlInner.Controls.Add(this.pnlTasks);
            this.pnlInner.Controls.Add(this.pnlTitle);
            this.pnlInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInner.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlInner.Location = new System.Drawing.Point(1, 1);
            this.pnlInner.Name = "pnlInner";
            this.pnlInner.Padding = new System.Windows.Forms.Padding(1);
            this.pnlInner.Size = new System.Drawing.Size(565, 30);
            this.pnlInner.TabIndex = 1;
            // 
            // pnlTasks
            // 
            this.pnlTasks.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.Color.White);
            this.pnlTasks.Controls.Add(this.eXPPanel1);
            this.pnlTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTasks.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlTasks.Location = new System.Drawing.Point(1, 27);
            this.pnlTasks.Name = "pnlTasks";
            this.pnlTasks.Padding = new System.Windows.Forms.Padding(20, 3, 0, 1);
            this.pnlTasks.Size = new System.Drawing.Size(563, 2);
            this.pnlTasks.TabIndex = 213;
            this.pnlTasks.TabStop = true;
            // 
            // eXPPanel1
            // 
            this.eXPPanel1.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.Color.White);
            this.eXPPanel1.BorderLine = true;
            this.eXPPanel1.Controls.Add(this.lsvTasks);
            this.eXPPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eXPPanel1.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.eXPPanel1.Location = new System.Drawing.Point(20, 3);
            this.eXPPanel1.Name = "eXPPanel1";
            this.eXPPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.eXPPanel1.Size = new System.Drawing.Size(543, 0);
            this.eXPPanel1.TabIndex = 215;
            // 
            // lsvTasks
            // 
            this.lsvTasks.BackColor = System.Drawing.SystemColors.Control;
            this.lsvTasks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvTasks.CheckBoxes = true;
            this.lsvTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSubject,
            this.colTeam,
            this.colAssignedTo,
            this.colDue,
            this.colCompleted});
            this.lsvTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvTasks.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsvTasks.FullRowSelect = true;
            this.lsvTasks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsvTasks.HoverSelection = true;
            this.lsvTasks.Location = new System.Drawing.Point(1, 1);
            this.lsvTasks.Name = "lsvTasks";
            this.lsvTasks.Scrollable = false;
            this.lsvTasks.ShowItemToolTips = true;
            this.lsvTasks.Size = new System.Drawing.Size(541, 0);
            this.lsvTasks.TabIndex = 0;
            this.lsvTasks.UseCompatibleStateImageBehavior = false;
            this.lsvTasks.View = System.Windows.Forms.View.Details;
            this.lsvTasks.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lsvTasks_ItemCheck);
            this.lsvTasks.SelectedIndexChanged += new System.EventHandler(this.lsvTasks_SelectedIndexChanged);
            this.lsvTasks.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lsvTasks_MouseClick);
            this.lsvTasks.Resize += new System.EventHandler(this.lsvTasks_Resize);
            // 
            // colSubject
            // 
            this.colSubject.Text = "Subject";
            this.colSubject.Width = 200;
            // 
            // colTeam
            // 
            this.colTeam.Text = "Team";
            this.colTeam.Width = 120;
            // 
            // colAssignedTo
            // 
            this.colAssignedTo.Text = "Assigned";
            this.colAssignedTo.Width = 120;
            // 
            // colDue
            // 
            this.colDue.Text = "Due";
            this.colDue.Width = 98;
            // 
            // colCompleted
            // 
            this.colCompleted.Text = "Completed";
            this.colCompleted.Width = 170;
            // 
            // pnlTitle
            // 
            this.pnlTitle.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.Color.White);
            this.pnlTitle.BorderLine = true;
            this.pnlTitle.Controls.Add(this.labMilestoneDue);
            this.pnlTitle.Controls.Add(this.lblStageDesc);
            this.pnlTitle.Controls.Add(this.chkMilestoneDesc);
            this.pnlTitle.Controls.Add(this.dtpMilestoneDue);
            this.pnlTitle.Controls.Add(this.labMileStoneType);
            this.pnlTitle.Controls.Add(this.labMilestoneComp);
            this.pnlTitle.Controls.Add(this.labMilestoneDueDays);
            this.pnlTitle.Controls.Add(this.btnExpand);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlTitle.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlTitle.Location = new System.Drawing.Point(1, 1);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Padding = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.pnlTitle.Size = new System.Drawing.Size(563, 26);
            this.pnlTitle.TabIndex = 214;
            // 
            // labMilestoneDue
            // 
            this.labMilestoneDue.Dock = System.Windows.Forms.DockStyle.Right;
            this.labMilestoneDue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMilestoneDue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labMilestoneDue.Location = new System.Drawing.Point(142, 3);
            this.labMilestoneDue.Name = "labMilestoneDue";
            this.labMilestoneDue.Size = new System.Drawing.Size(105, 20);
            this.labMilestoneDue.TabIndex = 10;
            this.labMilestoneDue.Text = "10/10/2010";
            this.labMilestoneDue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labMilestoneDue.Visible = false;
            // 
            // lblStageDesc
            // 
            this.lblStageDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStageDesc.Location = new System.Drawing.Point(20, 3);
            this.lblStageDesc.Margin = new System.Windows.Forms.Padding(3);
            this.lblStageDesc.Name = "lblStageDesc";
            this.lblStageDesc.Size = new System.Drawing.Size(150, 20);
            this.lblStageDesc.TabIndex = 11;
            this.lblStageDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStageDesc.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MilestoneStage_MouseClick);
            // 
            // chkMilestoneDesc
            // 
            this.chkMilestoneDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkMilestoneDesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMilestoneDesc.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMilestoneDesc.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkMilestoneDesc.Location = new System.Drawing.Point(6, 3);
            this.chkMilestoneDesc.Name = "chkMilestoneDesc";
            this.chkMilestoneDesc.Size = new System.Drawing.Size(150, 20);
            this.chkMilestoneDesc.TabIndex = 0;
            this.chkMilestoneDesc.Text = "Description";
            this.chkMilestoneDesc.CheckedChanged += new System.EventHandler(this.chkMilestoneDesc_CheckedChanged);
            this.chkMilestoneDesc.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MilestoneStage_MouseClick);
            // 
            // dtpMilestoneDue
            // 
            this.dtpMilestoneDue.AllowNull = true;
            this.dtpMilestoneDue.CaptionWidth = 0;
            this.dtpMilestoneDue.DateTimeLayout = FWBS.OMS.UI.Windows.DateTimePickerLayout.dtpSameLine;
            this.dtpMilestoneDue.DateVisible = true;
            this.dtpMilestoneDue.DefaultTime = "09:00:00";
            this.dtpMilestoneDue.DisplayDateIn = System.DateTimeKind.Local;
            this.dtpMilestoneDue.Dock = System.Windows.Forms.DockStyle.Right;
            this.dtpMilestoneDue.GreaterThanToday = false;
            this.dtpMilestoneDue.IsDirty = true;
            this.dtpMilestoneDue.LessThanToday = false;
            this.dtpMilestoneDue.Location = new System.Drawing.Point(247, 3);
            this.dtpMilestoneDue.Name = "dtpMilestoneDue";
            this.dtpMilestoneDue.Size = new System.Drawing.Size(105, 24);
            this.dtpMilestoneDue.SlaveDatePicker = "";
            this.dtpMilestoneDue.SpecialOptionsVisible = true;
            this.dtpMilestoneDue.TabIndex = 6;
            this.dtpMilestoneDue.TimeLabelVisible = false;
            this.dtpMilestoneDue.TimeVisible = false;
            this.dtpMilestoneDue.Value = new System.DateTime(2005, 9, 22, 0, 0, 0, 0);
            this.dtpMilestoneDue.Changed += new System.EventHandler(this.dtpMilestoneDue_Changed);
            // 
            // labMileStoneType
            // 
            this.labMileStoneType.Dock = System.Windows.Forms.DockStyle.Right;
            this.labMileStoneType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMileStoneType.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labMileStoneType.Location = new System.Drawing.Point(352, 3);
            this.labMileStoneType.Name = "labMileStoneType";
            this.labMileStoneType.Size = new System.Drawing.Size(26, 20);
            this.labMileStoneType.TabIndex = 5;
            this.labMileStoneType.Text = "C";
            this.labMileStoneType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labMilestoneComp
            // 
            this.labMilestoneComp.Dock = System.Windows.Forms.DockStyle.Right;
            this.labMilestoneComp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMilestoneComp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labMilestoneComp.Location = new System.Drawing.Point(378, 3);
            this.labMilestoneComp.Name = "labMilestoneComp";
            this.labMilestoneComp.Size = new System.Drawing.Size(117, 20);
            this.labMilestoneComp.TabIndex = 8;
            this.labMilestoneComp.Text = "10/10/2010";
            this.labMilestoneComp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labMilestoneDueDays
            // 
            this.labMilestoneDueDays.Dock = System.Windows.Forms.DockStyle.Right;
            this.labMilestoneDueDays.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMilestoneDueDays.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labMilestoneDueDays.Location = new System.Drawing.Point(495, 3);
            this.labMilestoneDueDays.Name = "labMilestoneDueDays";
            this.labMilestoneDueDays.Size = new System.Drawing.Size(46, 20);
            this.labMilestoneDueDays.TabIndex = 9;
            this.labMilestoneDueDays.Text = "COMP";
            this.labMilestoneDueDays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExpand
            // 
            this.btnExpand.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpand.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExpand.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnExpand.ImageIndex = 1;
            this.btnExpand.ImageList = this.imlNavigation;
            this.btnExpand.Location = new System.Drawing.Point(541, 3);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(19, 20);
            this.btnExpand.TabIndex = 4;
            this.btnExpand.TabStop = false;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // MilestoneStage
            // 
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSeperator);
            this.Name = "MilestoneStage";
            this.Size = new System.Drawing.Size(567, 36);
            this.Load += new System.EventHandler(this.MilestoneStage_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MilestoneStage_MouseClick);
            this.ParentChanged += new System.EventHandler(this.MilestoneStage_ParentChanged);
            this.pnlMain.ResumeLayout(false);
            this.pnlInner.ResumeLayout(false);
            this.pnlTasks.ResumeLayout(false);
            this.eXPPanel1.ResumeLayout(false);
            this.pnlTitle.ResumeLayout(false);
            this.ResumeLayout(false);

		}

 
		#endregion

		#endregion

		#region Private Events
	
		private void btnExpand_Click(object sender, System.EventArgs e)
		{
			try
			{
				ToggleExpanded();
			}
			catch (Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}
		}

		private void lsvTasks_Resize(object sender, System.EventArgs e)
		{
            if (Expanded)
            lsvTasks.Columns[0].Width = (lsvTasks.Width - (lsvTasks.Columns[1].Width + lsvTasks.Columns[2].Width + lsvTasks.Columns[3].Width + lsvTasks.Columns[4].Width + 2));
		}

		#endregion

        #region Public Methods

        internal void SetContextMenu(ContextMenu stage, ContextMenu task)
        {
            chkMilestoneDesc.ContextMenu = stage;
            lblStageDesc.ContextMenu = stage;
            lsvTasks.ContextMenu = task;
        }

        public bool ToggleExpanded()
		{
			switch (btnExpand.ImageIndex)
			{
				case 0: // Already Expanded
				{
					btnExpand.ImageIndex = 1;
					this.Height = pnlTitle.Height + pnlSeperator.Height + pnlInner.DockPadding.Top + pnlInner.DockPadding.Bottom + pnlMain.DockPadding.Top + pnlMain.DockPadding.Bottom;
					break;
				}
				case 1: // Already Contracted
				{
                    int count = _milestoneStage.Tasks.VisibleCount;
					if (count >0)
					{
						btnExpand.ImageIndex = 0;
						this.Height = startHeight + (getItemHeight * count);
					}
					break;
				}
			}
			return (btnExpand.ImageIndex == 0);
		}
		#endregion

		#region Public Properties
		[DefaultValue(false)]
		public bool Expanded
		{
			get
			{
				return (btnExpand.ImageIndex == 0);
			}
			set
			{
				if (value != Expanded)
				{
                    int count = _milestoneStage.Tasks.VisibleCount;
					switch (value)
					{
						case false: // Already Expanded
						{
							if (count >0)
							{
								btnExpand.ImageIndex = 1;
								this.Height = pnlTitle.Height + pnlSeperator.Height + pnlInner.DockPadding.Top + pnlInner.DockPadding.Bottom + pnlMain.DockPadding.Top + pnlMain.DockPadding.Bottom;
							}
							break;
						}
						case true: // Already Contracted
						{
							if (count >0)
							{
								btnExpand.ImageIndex = 0;
								this.Height = startHeight + (getItemHeight * count);
							}
							break;
						}
					}
				}
			}
		}

		[DefaultValue(null)]
		public Milestones.MilestoneStage CurrentMilestoneStage
		{
			get
			{
				return _milestoneStage;
			}
			set
			{
				if (_milestoneStage != value)
				{
					_milestoneStage = value;
					cCompleted = _milestoneStage.Application.Parent.TaskCompletedColour;
					cOverdue = _milestoneStage.Application.Parent.TaskOverdueColour;
					cDue = _milestoneStage.Application.Parent.TaskDueColour;
					LoadStage();
				}
			}
		}
		#endregion

		#region Private Methods

		private void UnSelect()
		{
			try
			{
                pnlMain.BorderStyle = BorderStyle.None;
			}
			finally
			{
				selected = false;
			}
		}

		new public void Select()
		{
			if (LastSelected != null && LastSelected != this)
			{
				try
				{
					LastSelected.UnSelect();
					LoadStageActions();
					LoadTaskActions();
				}
				finally
				{
					selected = true;
				}
			}

            pnlMain.BorderStyle = BorderStyle.FixedSingle;
			LastSelected = this;
		}

		private int getItemHeight
		{
			get
			{
				try
				{
                    return Convert.ToInt32(lsvTasks.Font.Height * 1.3);
				}
				catch
				{
					resizeafterparentset = true;
					return 0;
				}
			}
		}

		#endregion

		#region Data Loading

        public void UnloadStage()
        {
            if (_milestoneStage != null)
            {
                _milestoneStage.TaskChanged -= new TaskChangedEventHandler(_milestoneStage_TaskChanged);
                _milestoneStage = null;
            }

            if (chkMilestoneDesc != null)
                chkMilestoneDesc.ContextMenu = null;
            if (lblStageDesc != null)
                lblStageDesc.ContextMenu = null;
            if (lsvTasks != null)
                lsvTasks.ContextMenu = null;

          
   
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }
		public void LoadStage()
		{
			bool capture = captureEvents;

			try
			{
				captureEvents = false;

				if (_milestoneStage != null)
				{
		
					_milestoneStage.TaskChanged -=new TaskChangedEventHandler(_milestoneStage_TaskChanged);
					_milestoneStage.TaskChanged+=new TaskChangedEventHandler(_milestoneStage_TaskChanged);

                    lblStageDesc.Text = String.Format("{0}.{1}", _milestoneStage.StageNumber, _milestoneStage.Description);
					
					if (_milestoneStage.IsNull(_milestoneStage.Due))
					{
						dtpMilestoneDue.Value = String.Empty;
						labMilestoneDue.Text = Session.CurrentSession.Resources.GetResource("RESNOTSET", "(not set)", "").Text;
					}
					else
					{
						dtpMilestoneDue.Value = _milestoneStage.Due.Value;
						labMilestoneDue.Text = _milestoneStage.Due.Value.ToString(cultureInfo.DateTimeFormat.ShortDatePattern);
					}

					labMilestoneDue.Visible = false;
					dtpMilestoneDue.Visible = true;
					labMileStoneType.Visible = false;
					labMilestoneDueDays.Text = String.Empty;
					labMilestoneComp.Text = String.Empty;
					chkMilestoneDesc.Checked = false;
					btnExpand.Enabled = (_milestoneStage.Tasks.VisibleCount > 0);
					if (btnExpand.Enabled == false) btnExpand.ImageIndex = -1;

					switch (_milestoneStage.Status)
					{
						case StageStatus.Unspecified:
							pnlTitle.Backcolor.SetColor = cDue;
							chkMilestoneDesc.Font = fUnCompleted;
							break;
						case StageStatus.Completed:
							dtpMilestoneDue.Visible = false;
							labMilestoneDue.Visible = true;
							pnlTitle.Backcolor.SetColor = cCompleted;
							labMilestoneComp.Text = _milestoneStage.Achieved.Value.ToString(cultureInfo.DateTimeFormat.ShortDatePattern);
							chkMilestoneDesc.Checked = true;
							chkMilestoneDesc.Font = fCompleted;
							labMilestoneComp.Font = fCompleted;
							labMilestoneDue.Font = fCompleted;
							break;
						case StageStatus.Due:
							pnlTitle.Backcolor.SetColor = cDue;
							labMilestoneDueDays.Text = _milestoneStage.DaysDue.ToString();
							dtpMilestoneDue.Value = _milestoneStage.Due;
							chkMilestoneDesc.Font = fUnCompleted;
							labMilestoneComp.Font = fUnCompleted;
							labMilestoneDue.Font = fUnCompleted;
							break;
						case StageStatus.Overdue:
							pnlTitle.Backcolor.SetColor = cOverdue;
							labMilestoneDueDays.Text = _milestoneStage.DaysDue.ToString();
							dtpMilestoneDue.Value = _milestoneStage.Due;
							chkMilestoneDesc.Font = fUnCompleted;
							labMilestoneComp.Font = fUnCompleted;
							labMilestoneDue.Font = fUnCompleted;
							break;
						case StageStatus.NextDue:
							pnlTitle.Backcolor.SetColor = cDue;
							labMilestoneDueDays.Text = _milestoneStage.DaysDue.ToString();
							dtpMilestoneDue.Value = _milestoneStage.Due;
							chkMilestoneDesc.Font = fUnCompleted;
							labMilestoneComp.Font = fUnCompleted;
							labMilestoneDue.Font = fUnCompleted;
							break;
					}

                    if (_milestoneStage.IsNextDue)
                    {
                        pnlTitle.Backcolor.SetColor = ucPanelNav.ChangeBrightness(pnlTitle.Backcolor.Color, -50);
                        this.Select();
                    }

                    chkMilestoneDesc.Visible = _milestoneStage.Application.Parent.ShowTaskFlowCheckboxes;

                    ToggleDisplayOfMilestoneStageControls(_milestoneStage.Application.Parent.ShowAllTaskFlowColumns);
                }

				LoadTasks(_milestoneStage);
                dtpMilestoneDue.Enabled = new FileActivity(_milestoneStage.CurrentFile, FileStatusActivityType.TaskflowProcessing).IsAllowed();
			}
			finally
			{
				captureEvents = capture;
			}
		}

        private void ToggleDisplayOfMilestoneStageControls(bool visible)
        {
            labMilestoneDue.Visible = visible;
            labMilestoneComp.Visible = visible;
            labMilestoneDue.Visible = visible;
            dtpMilestoneDue.Visible = visible;
            labMilestoneDueDays.Visible = visible;
        }

		private void LoadTasks(Milestones.MilestoneStage stage)
		{
            
            System.Collections.Generic.List<ListViewItem> processed = new System.Collections.Generic.List<ListViewItem>();
			for (int ctr = 0; ctr < stage.Tasks.Count; ctr++)
			{
                ListViewItem n = null;
                FileManagement.Milestones.Task tsk = stage.Tasks[ctr];
                if (lsvTasks.Items.ContainsKey(tsk.GetHashCode().ToString()))
                    n = LoadTask(tsk, lsvTasks.Items[tsk.GetHashCode().ToString()]);
                else
                    n = LoadTask(tsk, null);

                if (n != null)
                    processed.Add(n);
			}

            for (int ctr = lsvTasks.Items.Count - 1; ctr >= 0; ctr-- )
            {
                ListViewItem n = lsvTasks.Items[ctr];
                if (!processed.Contains(n))
                    n.Remove();
            }

            if (lsvTasks.ListViewItemSorter == null)
            {
                lsvTasks.ListViewItemSorter = new DueDateComparer(colDue);
                lsvTasks.Sort();
            }
		}

        private ListViewItem LoadTask(FileManagement.Milestones.Task task, ListViewItem n)
		{
            const string NotAvailable = "n/a";

			bool capture = captureEvents;

			try
			{
				captureEvents = false;

                ListViewItem.ListViewSubItem si_team;
                ListViewItem.ListViewSubItem si_assigned;
				ListViewItem.ListViewSubItem si_due;
				ListViewItem.ListViewSubItem si_complete;

				if (n == null)
				{
                    if (!task.Visible)
                        return null;

                    n = lsvTasks.Items.Add(task.GetHashCode().ToString(), "", "");
                    si_team = n.SubItems.Add(String.Empty);
                    si_assigned = n.SubItems.Add(String.Empty);
                    si_due = n.SubItems.Add(String.Empty);
                    si_complete = n.SubItems.Add(String.Empty);

                }
				else
				{
                    if (!task.Visible)
                    {
                        lsvTasks.Items.Remove(n);
                        return null;
                    }
                    si_team = n.SubItems[1];
                    si_assigned = n.SubItems[2];
					si_due = n.SubItems[3];
					si_complete = n.SubItems[4];

				}

				n.Text = task.Description;
                n.ToolTipText = n.Text;

                if (task.Application.Parent.ShowAllTaskFlowColumns)
                {
                    if (task.AssignedTeam == null)
                        si_team.Text = NotAvailable;
                    else
                        si_team.Text = task.AssignedTeam.Name;

                    if (task.AssignedTo == null)
                        si_assigned.Text = NotAvailable;
                    else
                        si_assigned.Text = task.AssignedTo.FullName;

                    if (task.IsNull(task.Due))
                        si_due.Text = String.Empty;
                    else
                        si_due.Text = task.Due.Value.ToString(cultureInfo.DateTimeFormat.ShortDatePattern);
                }


                if (task.IsCompleted == false)
                {
                    n.Font = fUnCompleted;

                    if (task.Application.Parent.ShowAllTaskFlowColumns)
                        si_complete.Text = String.Empty;

                    if (task.IsOverdue)
                        n.BackColor = cOverdue;
                    else
                        n.BackColor = cDue;
                }
                else
                {
                    n.BackColor = cCompleted;
                    n.Font = fCompleted;

                    if (task.Application.Parent.ShowAllTaskFlowColumns)
                    {
                        if (task.CompletedBy == null)
                            si_complete.Text = String.Format("{0} ({1})", task.Completed.Value.ToString(cultureInfo.DateTimeFormat.ShortDatePattern), NotAvailable);
                        else
                            si_complete.Text = String.Format("{0} ({1})", task.Completed.Value.ToString(cultureInfo.DateTimeFormat.ShortDatePattern), task.CompletedBy.FullName);
                    }
				}

                n.ListView.CheckBoxes = task.Application.Parent.ShowTaskFlowCheckboxes;

                try
				{
					allowcheck = true;
					n.Checked = task.IsCompleted;
				}
				finally
				{
					allowcheck = false;
				}
				n.Tag = task;
                return n;
			}
			finally
			{
				captureEvents = capture;
			}

		}

		private void LoadStageActions()
		{
			Control parent = Parent;
			MilestonePlanOld msplan = null;
			while (parent != null)
			{
				msplan = parent as MilestonePlanOld;
				if (msplan != null)
					break;
				parent = parent.Parent;
			}

			if (msplan != null)
			{
				if (captureEvents)
				{
					msplan.RefreshActions(_milestoneStage);
				}
			}
		}


		private void LoadTaskActions()
		{
			Control parent = Parent;
			MilestonePlanOld msplan = null;
			while (parent != null)
			{
				msplan = parent as MilestonePlanOld;
				if (msplan != null)
					break;
				parent = parent.Parent;
			}

			if (msplan != null)
			{
				if (captureEvents)
				{
                    FileManagement.Milestones.Task tsk = null;

					if (lsvTasks.SelectedItems.Count == 1)
                        tsk = (FileManagement.Milestones.Task)lsvTasks.SelectedItems[0].Tag;
					msplan.RefreshActions(tsk);
				}
			}
		}

		#endregion

		#region Captured Events

		private void MilestoneStage_ParentChanged(object sender, System.EventArgs e)
		{
			if (resizeafterparentset)
			{
				resizeafterparentset = false;
                int count = _milestoneStage.Tasks.VisibleCount;
                switch (btnExpand.ImageIndex)
				{
					case 0: // Already Expanded
					{
						this.Height = startHeight + (getItemHeight * count);
						break;
					}
					case 1: // Already Contracted
					{
						if (count >0)
						{
							this.Height = pnlTitle.Height + pnlSeperator.Height + pnlInner.DockPadding.Top + pnlInner.DockPadding.Bottom + pnlMain.DockPadding.Top + pnlMain.DockPadding.Bottom;
						}
						break;
					}
				}
			}
		}

		private void chkMilestoneDesc_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
                this.chkMilestoneDesc.CheckedChanged -= new System.EventHandler(this.chkMilestoneDesc_CheckedChanged);
                
                Cursor = Cursors.WaitCursor;
				if (captureEvents)
				{
                    new FileActivity(_milestoneStage.CurrentFile, FileStatusActivityType.TaskflowProcessing).Check();

					bool reload = false;
					if (chkMilestoneDesc.Checked)
					{
						if (_milestoneStage.IsCompleted == false)
						{
                            _milestoneStage.Application.CompleteStageUI(_milestoneStage);
							reload = true;
						}
					}
					else
					{
						if (_milestoneStage.IsCompleted)
						{
                            _milestoneStage.Application.UnCompleteStageUI(_milestoneStage);
							reload = true;
						}
					}

					if (reload)
						LoadStage();
				}
			}
			catch(Exception ex)
			{
				Cursor = Cursors.Default;
				ErrorBox.Show(ex);
			}
			finally
			{
                chkMilestoneDesc.Checked = _milestoneStage.IsCompleted;

                this.chkMilestoneDesc.CheckedChanged += new System.EventHandler(this.chkMilestoneDesc_CheckedChanged);

				Cursor = Cursors.Default;
			}
		}

		private void dtpMilestoneDue_Changed(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				if (captureEvents)
				{
                    new FileActivity(_milestoneStage.CurrentFile, FileStatusActivityType.TaskflowProcessing).Check();

					if (Convert.ToString(dtpMilestoneDue.Value) == String.Empty)
					{
						_milestoneStage.Due = null;
					}
					else
					{
						_milestoneStage.Due = Convert.ToDateTime(dtpMilestoneDue.Value);
					}

					LoadStage();
				}
			}
			catch(Exception ex)
			{
				Cursor = Cursors.Default;
				ErrorBox.Show(ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void MilestoneStage_Load(object sender, System.EventArgs e)
		{
			captureEvents = true;
		}

		private void lsvTasks_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				if (captureEvents)
				{
                    new FileActivity(_milestoneStage.CurrentFile, FileStatusActivityType.TaskflowProcessing).Check();

				 	ListViewItem litem = ((ListView)sender).Items[e.Index];
                    FileManagement.Milestones.Task tsk = litem.Tag as FileManagement.Milestones.Task;
					if (tsk != null)
					{
						bool reload = false;
						if (e.NewValue == CheckState.Checked)
						{
								if (tsk.IsCompleted == false)
								{
									tsk.Application.CompleteTaskUI(tsk);
                                    reload = true;
								}
						}
						else
						{
							if (tsk.IsCompleted)
							{
                                tsk.Application.UnCompleteTaskUI(tsk);
                                reload = true;
							}
						}

						if (reload)
							LoadTask(tsk, litem);
					}
				}

			}
			catch(Exception ex)
			{
                if (allowcheck == false)
                {
                    e.NewValue = e.CurrentValue;
                }

				Cursor = Cursors.Default;
				ErrorBox.Show(ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void lsvTasks_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		}

		private void _milestoneStage_TaskChanged(object sender, TaskChangedEventArgs e)
		{
			foreach (ListViewItem litem in lsvTasks.Items)
			{
				if (litem.Tag == e.Task)
				{
					LoadTask(e.Task, litem);
					return;
				}
			}
		}


		#endregion


        private void MilestoneStage_MouseClick(object sender, MouseEventArgs e)
        {
            Select();
        }

        private void lsvTasks_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (this.selected == false)
                {
                    Select();
                }
                LoadTaskActions();

            }
            finally 
            {
                Cursor = Cursors.Default;
            }
        }

        private sealed class DueDateComparer : IComparer
        {
            private ColumnHeader col;
            public DueDateComparer(ColumnHeader col)
            {
                this.col = col;
            }

            #region IComparer Members

            public int Compare(object x, object y)
            {
                ListViewItem itm_x = (ListViewItem)x;
                ListViewItem itm_y = (ListViewItem)y;

                Milestones.Task task_x = itm_x.Tag as Milestones.Task;
                Milestones.Task task_y = itm_y.Tag as Milestones.Task;

                if (task_x == null)
                    return 1;
                else if (task_y == null)
                    return -1;

                DateTime? dte_x = task_x.Due;
                DateTime? dte_y = task_y.Due;

                if (dte_x.HasValue && dte_y.HasValue)
                    return dte_x.Value.CompareTo(dte_y.Value);
                else if (dte_x.HasValue)
                    return -1;
                else if (dte_y.HasValue)
                    return 1;
                else
                    return 0;
            }

            #endregion

        }

	}
}
