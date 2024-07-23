namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Day
{
    partial class ucDayAppointmentDetails
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
            this.pnlMarkerContainer = new System.Windows.Forms.Panel();
            this.pnlContainer.SuspendLayout();
            this.tlpContainer.SuspendLayout();
            this.pnlMarkerContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // leftMarker
            // 
            this.leftMarker.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.leftMarker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftMarker.Location = new System.Drawing.Point(0, 1);
            this.leftMarker.Name = "leftMarker";
            this.leftMarker.Size = new System.Drawing.Size(5, 49);
            this.leftMarker.TabIndex = 2;
            // 
            // pnlContainer
            // 
            this.pnlContainer.BackColor = System.Drawing.Color.Transparent;
            this.pnlContainer.Controls.Add(this.tlpContainer);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(5, 0);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.pnlContainer.Size = new System.Drawing.Size(285, 50);
            this.pnlContainer.TabIndex = 3;
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
            this.tlpContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpContainer.Location = new System.Drawing.Point(6, 0);
            this.tlpContainer.Margin = new System.Windows.Forms.Padding(0);
            this.tlpContainer.Name = "tlpContainer";
            this.tlpContainer.RowCount = 3;
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpContainer.Size = new System.Drawing.Size(279, 49);
            this.tlpContainer.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoEllipsis = true;
            this.tlpContainer.SetColumnSpan(this.lblTitle, 3);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(273, 17);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTime
            // 
            this.lblTime.AutoEllipsis = true;
            this.tlpContainer.SetColumnSpan(this.lblTime, 2);
            this.lblTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lblTime.Location = new System.Drawing.Point(3, 17);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(213, 16);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "Time";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMarker
            // 
            this.lblMarker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMarker.Font = new System.Drawing.Font("Segoe UI Symbol", 7F);
            this.lblMarker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lblMarker.Location = new System.Drawing.Point(3, 33);
            this.lblMarker.Name = "lblMarker";
            this.lblMarker.Size = new System.Drawing.Size(14, 16);
            this.lblMarker.TabIndex = 2;
            this.lblMarker.Text = "M";
            this.lblMarker.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoEllipsis = true;
            this.tlpContainer.SetColumnSpan(this.lblLocation, 2);
            this.lblLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lblLocation.Location = new System.Drawing.Point(23, 33);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(253, 16);
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
            this.lblJoin.Location = new System.Drawing.Point(222, 17);
            this.lblJoin.Name = "lblJoin";
            this.lblJoin.Size = new System.Drawing.Size(54, 16);
            this.lblJoin.TabIndex = 4;
            this.lblJoin.TabStop = true;
            this.lblJoin.Text = "Join";
            this.lblJoin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblJoin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblJoin_LinkClicked);
            // 
            // pnlMarkerContainer
            // 
            this.pnlMarkerContainer.BackColor = System.Drawing.Color.Transparent;
            this.pnlMarkerContainer.Controls.Add(this.leftMarker);
            this.pnlMarkerContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMarkerContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlMarkerContainer.Name = "pnlMarkerContainer";
            this.pnlMarkerContainer.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.pnlMarkerContainer.Size = new System.Drawing.Size(5, 50);
            this.pnlMarkerContainer.TabIndex = 3;
            // 
            // ucDayAppointmentDetails
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.pnlMarkerContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Name = "ucDayAppointmentDetails";
            this.Size = new System.Drawing.Size(290, 50);
            this.pnlContainer.ResumeLayout(false);
            this.tlpContainer.ResumeLayout(false);
            this.pnlMarkerContainer.ResumeLayout(false);
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
        private System.Windows.Forms.Panel pnlMarkerContainer;
    }
}
