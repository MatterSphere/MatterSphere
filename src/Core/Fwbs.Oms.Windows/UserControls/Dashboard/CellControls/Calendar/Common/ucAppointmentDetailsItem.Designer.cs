namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common
{
    partial class ucAppointmentDetailsItem
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.leftMarker = new System.Windows.Forms.Panel();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.tlpContainer = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblMarker = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblJoin = new System.Windows.Forms.LinkLabel();
            this.pnlContainer.SuspendLayout();
            this.tlpContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // leftMarker
            // 
            this.leftMarker.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.leftMarker.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftMarker.Location = new System.Drawing.Point(1, 1);
            this.leftMarker.Name = "leftMarker";
            this.leftMarker.Size = new System.Drawing.Size(5, 50);
            this.leftMarker.TabIndex = 0;
            // 
            // pnlContainer
            // 
            this.pnlContainer.Controls.Add(this.tlpContainer);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(6, 1);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(6, 4, 0, 4);
            this.pnlContainer.Size = new System.Drawing.Size(203, 50);
            this.pnlContainer.TabIndex = 1;
            // 
            // tlpContainer
            // 
            this.tlpContainer.ColumnCount = 3;
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpContainer.Controls.Add(this.lblTitle, 0, 0);
            this.tlpContainer.Controls.Add(this.lblTime, 0, 1);
            this.tlpContainer.Controls.Add(this.lblMarker, 0, 2);
            this.tlpContainer.Controls.Add(this.lblLocation, 1, 2);
            this.tlpContainer.Controls.Add(this.lblJoin, 2, 1);
            this.tlpContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContainer.Location = new System.Drawing.Point(6, 4);
            this.tlpContainer.Margin = new System.Windows.Forms.Padding(0);
            this.tlpContainer.Name = "tlpContainer";
            this.tlpContainer.RowCount = 3;
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpContainer.Size = new System.Drawing.Size(197, 42);
            this.tlpContainer.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.tlpContainer.SetColumnSpan(this.lblTitle, 3);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(191, 14);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTime
            // 
            this.tlpContainer.SetColumnSpan(this.lblTime, 2);
            this.lblTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lblTime.Location = new System.Drawing.Point(3, 14);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(131, 14);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "Time";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMarker
            // 
            this.lblMarker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMarker.Font = new System.Drawing.Font("Segoe UI Symbol", 7F);
            this.lblMarker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lblMarker.Location = new System.Drawing.Point(3, 28);
            this.lblMarker.Name = "lblMarker";
            this.lblMarker.Size = new System.Drawing.Size(14, 14);
            this.lblMarker.TabIndex = 2;
            this.lblMarker.Text = "M";
            this.lblMarker.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLocation
            // 
            this.tlpContainer.SetColumnSpan(this.lblLocation, 2);
            this.lblLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lblLocation.Location = new System.Drawing.Point(23, 28);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(171, 14);
            this.lblLocation.TabIndex = 3;
            this.lblLocation.Text = "Location";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblJoin
            // 
            this.lblJoin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblJoin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.lblJoin.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblJoin.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.lblJoin.Location = new System.Drawing.Point(140, 14);
            this.lblJoin.Name = "lblJoin";
            this.lblJoin.Size = new System.Drawing.Size(54, 14);
            this.lblJoin.TabIndex = 4;
            this.lblJoin.TabStop = true;
            this.lblJoin.Text = "Join";
            this.lblJoin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblJoin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblJoin_LinkClicked);
            // 
            // ucAppointmentDetailsItem
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.leftMarker);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.MinimumSize = new System.Drawing.Size(210, 52);
            this.Name = "ucAppointmentDetailsItem";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(210, 52);
            this.pnlContainer.ResumeLayout(false);
            this.tlpContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel leftMarker;
        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.TableLayoutPanel tlpContainer;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblMarker;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.LinkLabel lblJoin;
    }
}
