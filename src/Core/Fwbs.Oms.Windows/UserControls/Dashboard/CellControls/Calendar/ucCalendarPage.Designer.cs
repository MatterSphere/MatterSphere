namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    partial class ucCalendarPage
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
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.topLine = new System.Windows.Forms.Panel();
            this.ucCalendar = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Month.ucMonthView();
            this.monthPicker = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.ucMonthPicker();
            this.rbDay = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common.ucViewSelector();
            this.rbWeek = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common.ucViewSelector();
            this.rbMonth = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common.ucViewSelector();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.monthPicker);
            this.pnlFilter.Controls.Add(this.rbDay);
            this.pnlFilter.Controls.Add(this.rbWeek);
            this.pnlFilter.Controls.Add(this.rbMonth);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 0);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pnlFilter.Size = new System.Drawing.Size(350, 50);
            this.pnlFilter.TabIndex = 0;
            // 
            // topLine
            // 
            this.topLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.topLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.topLine.Location = new System.Drawing.Point(0, 50);
            this.topLine.Name = "topLine";
            this.topLine.Size = new System.Drawing.Size(350, 1);
            this.topLine.TabIndex = 1;
            // 
            // ucCalendar
            // 
            this.ucCalendar.BackColor = System.Drawing.Color.White;
            this.ucCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucCalendar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucCalendar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ucCalendar.Location = new System.Drawing.Point(0, 51);
            this.ucCalendar.Name = "ucCalendar";
            this.ucCalendar.Size = new System.Drawing.Size(350, 299);
            this.ucCalendar.TabIndex = 2;
            // 
            // monthPicker
            // 
            this.monthPicker.AutoSize = true;
            this.monthPicker.BackColor = System.Drawing.Color.White;
            this.monthPicker.Cursor = System.Windows.Forms.Cursors.Hand;
            this.monthPicker.Dock = System.Windows.Forms.DockStyle.Left;
            this.monthPicker.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.monthPicker.FlatAppearance.BorderSize = 0;
            this.monthPicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.monthPicker.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.monthPicker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.monthPicker.Location = new System.Drawing.Point(0, 0);
            this.monthPicker.Margin = new System.Windows.Forms.Padding(4);
            this.monthPicker.Name = "monthPicker";
            this.monthPicker.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.monthPicker.Size = new System.Drawing.Size(109, 50);
            this.monthPicker.TabIndex = 3;
            this.monthPicker.Text = "Month Year";
            this.monthPicker.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.monthPicker.UseVisualStyleBackColor = false;
            this.monthPicker.Click += new System.EventHandler(this.monthPicker_Click);
            // 
            // rbDay
            // 
            this.rbDay.BackColor = System.Drawing.Color.White;
            this.rbDay.Dock = System.Windows.Forms.DockStyle.Right;
            this.rbDay.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.rbDay.Location = new System.Drawing.Point(150, 0);
            this.rbDay.Name = "rbDay";
            this.rbDay.Size = new System.Drawing.Size(55, 50);
            this.rbDay.TabIndex = 2;
            this.rbDay.Text = "Day";
            this.rbDay.UseVisualStyleBackColor = false;
            this.rbDay.CheckedChanged += new System.EventHandler(this.rbDay_CheckedChanged);
            // 
            // rbWeek
            // 
            this.rbWeek.BackColor = System.Drawing.Color.White;
            this.rbWeek.Dock = System.Windows.Forms.DockStyle.Right;
            this.rbWeek.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.rbWeek.Location = new System.Drawing.Point(205, 0);
            this.rbWeek.Name = "rbWeek";
            this.rbWeek.Size = new System.Drawing.Size(64, 50);
            this.rbWeek.TabIndex = 1;
            this.rbWeek.Text = "Week";
            this.rbWeek.UseVisualStyleBackColor = false;
            this.rbWeek.CheckedChanged += new System.EventHandler(this.rbWeek_CheckedChanged);
            // 
            // rbMonth
            // 
            this.rbMonth.BackColor = System.Drawing.Color.White;
            this.rbMonth.Checked = true;
            this.rbMonth.Dock = System.Windows.Forms.DockStyle.Right;
            this.rbMonth.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.rbMonth.Location = new System.Drawing.Point(269, 0);
            this.rbMonth.Name = "rbMonth";
            this.rbMonth.Size = new System.Drawing.Size(71, 50);
            this.rbMonth.TabIndex = 0;
            this.rbMonth.TabStop = true;
            this.rbMonth.Text = "Month";
            this.rbMonth.UseVisualStyleBackColor = false;
            this.rbMonth.CheckedChanged += new System.EventHandler(this.rbMonth_CheckedChanged);
            // 
            // ucCalendarPage
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ucCalendar);
            this.Controls.Add(this.topLine);
            this.Controls.Add(this.pnlFilter);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Name = "ucCalendarPage";
            this.Size = new System.Drawing.Size(350, 350);
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlFilter;
        private FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common.ucViewSelector rbMonth;
        private FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common.ucViewSelector rbDay;
        private FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common.ucViewSelector rbWeek;
        private System.Windows.Forms.Panel topLine;
        private FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Month.ucMonthView ucCalendar;
        private FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.ucMonthPicker monthPicker;
    }
}
