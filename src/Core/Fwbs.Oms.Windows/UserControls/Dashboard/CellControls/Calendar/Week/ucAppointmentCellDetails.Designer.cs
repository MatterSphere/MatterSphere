namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week
{
    partial class ucAppointmentCellDetails
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
            this.lblTime = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // leftMarker
            // 
            this.leftMarker.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.leftMarker.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftMarker.Location = new System.Drawing.Point(0, 0);
            this.leftMarker.Name = "leftMarker";
            this.leftMarker.Size = new System.Drawing.Size(5, 54);
            this.leftMarker.TabIndex = 1;
            // 
            // lblTime
            // 
            this.lblTime.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTime.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.lblTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lblTime.Location = new System.Drawing.Point(5, 40);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(49, 14);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "Time";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoEllipsis = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 8F);
            this.lblTitle.Location = new System.Drawing.Point(5, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(49, 40);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Title";
            // 
            // ucAppointmentCellDetails
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.leftMarker);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Name = "ucAppointmentCellDetails";
            this.Size = new System.Drawing.Size(54, 54);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel leftMarker;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblTitle;
    }
}
