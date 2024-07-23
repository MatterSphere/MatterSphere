using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows.Reports
{
    /// <summary>
    /// Summary description for ucReportManager.
    /// </summary>
    internal class ucReportsManagerRS : System.Windows.Forms.UserControl
	{
		#region Auto Fields
		private System.Windows.Forms.Panel pnlHeading;
        private System.Windows.Forms.Label labHeading;
		private FWBS.Common.UI.Windows.eXPPanel pnlBack;
		private FWBS.Common.UI.Windows.eXPFrame pnlMain;
		#endregion

		#region Fields
		/// <summary>
		/// The Search List Object used for search the Contacts
		/// </summary>
		private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Panel panel1;
        private Panel pnlForm;

		#endregion

		#region Contructors


		public ucReportsManagerRS()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            pnlMain.FrameBackColor.SetColor = Color.Transparent;
		}
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pnlHeading = new System.Windows.Forms.Panel();
            this.labHeading = new System.Windows.Forms.Label();
            this.pnlBack = new FWBS.Common.UI.Windows.eXPPanel();
            this.pnlMain = new FWBS.Common.UI.Windows.eXPFrame();
            this.pnlForm = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.pnlHeading.SuspendLayout();
            this.pnlBack.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeading
            // 
            this.pnlHeading.Controls.Add(this.labHeading);
            this.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeading.Location = new System.Drawing.Point(0, 0);
            this.pnlHeading.Name = "pnlHeading";
            this.pnlHeading.Padding = new System.Windows.Forms.Padding(3);
            this.pnlHeading.Size = new System.Drawing.Size(222, 22);
            this.pnlHeading.TabIndex = 99;
            // 
            // labHeading
            // 
            this.labHeading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labHeading.Location = new System.Drawing.Point(3, 3);
            this.labHeading.Name = "labHeading";
            this.labHeading.Size = new System.Drawing.Size(216, 16);
            this.labHeading.TabIndex = 0;
            this.labHeading.Text = "Report Manager";
            this.labHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBack
            // 
            this.pnlBack.Backcolor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.TaskPainBackColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.pnlBack.Controls.Add(this.pnlMain);
            this.pnlBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBack.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlBack.Location = new System.Drawing.Point(0, 22);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Padding = new System.Windows.Forms.Padding(10);
            this.pnlBack.Size = new System.Drawing.Size(222, 402);
            this.pnlBack.TabIndex = 0;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlForm);
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Controls.Add(this.btnApply);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.pnlMain.FrameBackColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.SearchPainBackColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.pnlMain.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.Color.White);
            this.pnlMain.Location = new System.Drawing.Point(10, 10);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(8, 45, 8, 12);
            this.pnlMain.Size = new System.Drawing.Size(202, 382);
            this.pnlMain.TabIndex = 0;
            // 
            // pnlForm
            // 
            this.pnlForm.AutoScroll = true;
            this.pnlForm.BackColor = System.Drawing.Color.Transparent;
            this.pnlForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(8, 45);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(186, 325);
            this.pnlForm.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(8, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 1);
            this.panel1.TabIndex = 5;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnApply.Location = new System.Drawing.Point(111, 10);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(83, 23);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "View Report";
            // 
            // ucReportsManagerRS
            // 
            this.Controls.Add(this.pnlBack);
            this.Controls.Add(this.pnlHeading);
            this.Name = "ucReportsManagerRS";
            this.Size = new System.Drawing.Size(222, 424);
            this.pnlHeading.ResumeLayout(false);
            this.pnlBack.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        #region Properties
        public Panel Panel
        {
            get
            {
                return pnlForm;
            }
        }

        public Button SearchButton
        {
            get
            {
                return btnApply;
            }
        }
        #endregion
    }
}
