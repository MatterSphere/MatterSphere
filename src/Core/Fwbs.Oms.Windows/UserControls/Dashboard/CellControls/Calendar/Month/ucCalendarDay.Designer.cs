namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Month
{
    partial class ucCalendarDay
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

            _appointments.Clear();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbDay = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbDay
            // 
            this.lbDay.BackColor = System.Drawing.Color.Transparent;
            this.lbDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDay.Location = new System.Drawing.Point(0, 0);
            this.lbDay.Name = "lbDay";
            this.lbDay.Size = new System.Drawing.Size(68, 54);
            this.lbDay.TabIndex = 0;
            this.lbDay.Text = "Day";
            this.lbDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbDay.Click += new System.EventHandler(this.lbDay_Click);
            this.lbDay.MouseLeave += new System.EventHandler(this.lbDay_MouseLeave);
            this.lbDay.MouseHover += new System.EventHandler(this.lbDay_MouseHover);
            // 
            // ucCalendarDay
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lbDay);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ucCalendarDay";
            this.Size = new System.Drawing.Size(68, 54);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbDay;
    }
}
