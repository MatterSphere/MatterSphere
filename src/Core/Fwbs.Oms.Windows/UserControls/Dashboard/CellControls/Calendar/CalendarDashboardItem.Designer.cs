namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    partial class CalendarDashboardItem
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

            ucCalendarPage.ViewChanged -= OnCalendarViewChange;
            ucCalendarPage.MonthChanged -= OnSelectedMonthChange;
            ucCalendarPage.WeekChanged -= OnWeekChanged;
            ucCalendarPage.DayChanged -= OnDayChanged;

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ucCalendarPage = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.ucCalendarPage();
            this.SuspendLayout();
            // 
            // ucCalendarPage
            // 
            this.ucCalendarPage.BackColor = System.Drawing.Color.White;
            this.ucCalendarPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucCalendarPage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucCalendarPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ucCalendarPage.Location = new System.Drawing.Point(0, 0);
            this.ucCalendarPage.Name = "ucCalendarPage";
            this.ucCalendarPage.Size = new System.Drawing.Size(350, 350);
            this.ucCalendarPage.TabIndex = 0;
            // 
            // CalendarDashboardItem
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ucCalendarPage);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Name = "CalendarDashboardItem";
            this.Size = new System.Drawing.Size(350, 350);
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.ucCalendarPage ucCalendarPage;
    }
}
