namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week
{
    partial class ucCalendarMultiItem
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
            this.lblMeetings = new System.Windows.Forms.Label();
            this.lblNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMeetings
            // 
            this.lblMeetings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMeetings.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblMeetings.Location = new System.Drawing.Point(2, 35);
            this.lblMeetings.Name = "lblMeetings";
            this.lblMeetings.Size = new System.Drawing.Size(48, 15);
            this.lblMeetings.TabIndex = 0;
            this.lblMeetings.Text = "Meetings";
            this.lblMeetings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNumber
            // 
            this.lblNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNumber.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            this.lblNumber.Location = new System.Drawing.Point(2, 0);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(48, 35);
            this.lblNumber.TabIndex = 1;
            this.lblNumber.Text = "2";
            this.lblNumber.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ucCalendarMultiItem
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblNumber);
            this.Controls.Add(this.lblMeetings);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Name = "ucCalendarMultiItem";
            this.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.Size = new System.Drawing.Size(50, 50);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMeetings;
        private System.Windows.Forms.Label lblNumber;
    }
}
