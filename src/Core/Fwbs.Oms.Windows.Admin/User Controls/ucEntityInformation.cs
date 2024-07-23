using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucEntityInformation.
    /// </summary>
    public class ucEntityInformation : System.Windows.Forms.UserControl
	{
        private System.Windows.Forms.Panel pnlMain;
        protected ResourceLookup resourceLookup1;
        public LinkLabel lnkDelete;
        private Label labCommands;
        public LinkLabel lnkCopy;
        private Panel panel4;
        private Panel panel3;
        public Label Code;
        private Label labCode;
        public Label ObjectType;
        private Label labObjectType;
        public Label OMSObjectCode;
        private Label labObjectCode;
        private IContainer components;

		public ucEntityInformation()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}

		public ucEntityInformation(string Title)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lnkDelete = new System.Windows.Forms.LinkLabel();
            this.labCommands = new System.Windows.Forms.Label();
            this.lnkCopy = new System.Windows.Forms.LinkLabel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Code = new System.Windows.Forms.Label();
            this.labCode = new System.Windows.Forms.Label();
            this.ObjectType = new System.Windows.Forms.Label();
            this.labObjectType = new System.Windows.Forms.Label();
            this.OMSObjectCode = new System.Windows.Forms.Label();
            this.labObjectCode = new System.Windows.Forms.Label();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lnkDelete);
            this.pnlMain.Controls.Add(this.labCommands);
            this.pnlMain.Controls.Add(this.lnkCopy);
            this.pnlMain.Controls.Add(this.panel4);
            this.pnlMain.Controls.Add(this.panel3);
            this.pnlMain.Controls.Add(this.Code);
            this.pnlMain.Controls.Add(this.labCode);
            this.pnlMain.Controls.Add(this.ObjectType);
            this.pnlMain.Controls.Add(this.labObjectType);
            this.pnlMain.Controls.Add(this.OMSObjectCode);
            this.pnlMain.Controls.Add(this.labObjectCode);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMain.Location = new System.Drawing.Point(9, 9);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(5);
            this.pnlMain.Size = new System.Drawing.Size(423, 393);
            this.pnlMain.TabIndex = 1;
            // 
            // lnkDelete
            // 
            this.lnkDelete.AutoSize = true;
            this.lnkDelete.Enabled = false;
            this.lnkDelete.Location = new System.Drawing.Point(42, 290);
            this.resourceLookup1.SetLookup(this.lnkDelete, new FWBS.OMS.UI.Windows.ResourceLookupItem("DelTabFromSel", "Delete this Tab from Selected Types", ""));
            this.lnkDelete.Name = "lnkDelete";
            this.lnkDelete.Size = new System.Drawing.Size(191, 15);
            this.lnkDelete.TabIndex = 19;
            this.lnkDelete.TabStop = true;
            this.lnkDelete.Text = "Delete this Tab from Selected Types";
            this.lnkDelete.Visible = false;
            // 
            // labCommands
            // 
            this.labCommands.AutoSize = true;
            this.labCommands.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labCommands.Location = new System.Drawing.Point(10, 242);
            this.resourceLookup1.SetLookup(this.labCommands, new FWBS.OMS.UI.Windows.ResourceLookupItem("Commands", "Commands", ""));
            this.labCommands.Name = "labCommands";
            this.labCommands.Size = new System.Drawing.Size(68, 15);
            this.labCommands.TabIndex = 20;
            this.labCommands.Text = "Commands";
            // 
            // lnkCopy
            // 
            this.lnkCopy.AutoSize = true;
            this.lnkCopy.Location = new System.Drawing.Point(42, 266);
            this.resourceLookup1.SetLookup(this.lnkCopy, new FWBS.OMS.UI.Windows.ResourceLookupItem("CopyTabToSelctd", "Copy this Tab to Selected Types", ""));
            this.lnkCopy.Name = "lnkCopy";
            this.lnkCopy.Size = new System.Drawing.Size(171, 15);
            this.lnkCopy.TabIndex = 18;
            this.lnkCopy.TabStop = true;
            this.lnkCopy.Text = "Copy this Tab to Selected Types";
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel4.Location = new System.Drawing.Point(10, 230);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(403, 1);
            this.panel4.TabIndex = 17;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel3.Location = new System.Drawing.Point(10, 160);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(403, 1);
            this.panel3.TabIndex = 16;
            // 
            // Code
            // 
            this.Code.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Code.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.Code.Location = new System.Drawing.Point(165, 198);
            this.Code.Name = "Code";
            this.Code.Size = new System.Drawing.Size(245, 20);
            this.Code.TabIndex = 15;
            this.Code.Text = "SCHSYSMAIN";
            this.Code.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labCode
            // 
            this.labCode.Location = new System.Drawing.Point(20, 198);
            this.resourceLookup1.SetLookup(this.labCode, new FWBS.OMS.UI.Windows.ResourceLookupItem("Code", "Code :", ""));
            this.labCode.Name = "labCode";
            this.labCode.Size = new System.Drawing.Size(140, 20);
            this.labCode.TabIndex = 14;
            this.labCode.Text = "Code :";
            this.labCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ObjectType
            // 
            this.ObjectType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ObjectType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.ObjectType.Location = new System.Drawing.Point(165, 173);
            this.ObjectType.Name = "ObjectType";
            this.ObjectType.Size = new System.Drawing.Size(245, 20);
            this.ObjectType.TabIndex = 13;
            this.ObjectType.Text = "Search List";
            this.ObjectType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labObjectType
            // 
            this.labObjectType.Location = new System.Drawing.Point(20, 173);
            this.resourceLookup1.SetLookup(this.labObjectType, new FWBS.OMS.UI.Windows.ResourceLookupItem("ObjectType", "Object Type :", ""));
            this.labObjectType.Name = "labObjectType";
            this.labObjectType.Size = new System.Drawing.Size(140, 20);
            this.labObjectType.TabIndex = 12;
            this.labObjectType.Text = "Object Type :";
            this.labObjectType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OMSObjectCode
            // 
            this.OMSObjectCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OMSObjectCode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.OMSObjectCode.Location = new System.Drawing.Point(165, 135);
            this.OMSObjectCode.Name = "OMSObjectCode";
            this.OMSObjectCode.Size = new System.Drawing.Size(245, 20);
            this.OMSObjectCode.TabIndex = 11;
            this.OMSObjectCode.Text = "OBJECT NAME";
            this.OMSObjectCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labObjectCode
            // 
            this.labObjectCode.Location = new System.Drawing.Point(20, 135);
            this.resourceLookup1.SetLookup(this.labObjectCode, new FWBS.OMS.UI.Windows.ResourceLookupItem("OMSObject", "OMS Object : ", ""));
            this.labObjectCode.Name = "labObjectCode";
            this.labObjectCode.Size = new System.Drawing.Size(140, 20);
            this.labObjectCode.TabIndex = 10;
            this.labObjectCode.Text = "OMS Object : ";
            this.labObjectCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ucEntityInformation
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.pnlMain);
            this.Name = "ucEntityInformation";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.Size = new System.Drawing.Size(441, 411);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

 
	}

	

}
