using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    internal class ucCalendarPickerBase : Button
    {
        public ucCalendarPickerBase()
        {
            InitializeComponent();

            chevron.Text = "";
        }

        public virtual void SetTitle(DateTime date)
        {
            throw new NotImplementedException();
        }

        protected void SetForeColor(Color color)
        {
            this.chevron.ForeColor = color;
            this.ForeColor = color;
        }

        private void chevron_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void chevron_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(229, 229, 229);
        }

        private void chevron_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Transparent;
        }

        #region UI

        protected System.Windows.Forms.Label chevron;

        private void InitializeComponent()
        {
            this.chevron = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chevron
            // 
            this.chevron.BackColor = System.Drawing.Color.Transparent;
            this.chevron.Dock = System.Windows.Forms.DockStyle.Right;
            this.chevron.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chevron.Font = new System.Drawing.Font("Segoe UI Symbol", 10.5F);
            this.chevron.Location = new System.Drawing.Point(61, 0);
            this.chevron.Margin = new System.Windows.Forms.Padding(0);
            this.chevron.Name = "chevron";
            this.chevron.Size = new System.Drawing.Size(24, 40);
            this.chevron.TabIndex = 0;
            this.chevron.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chevron.Click += new System.EventHandler(this.chevron_Click);
            this.chevron.MouseLeave += new System.EventHandler(this.chevron_MouseLeave);
            this.chevron.MouseHover += new System.EventHandler(this.chevron_MouseHover);
            // 
            // ucCalendarPickerBase
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chevron);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.FlatAppearance.BorderSize = 0;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(85, 40);
            this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.UseVisualStyleBackColor = false;
            this.ResumeLayout(false);
        }

        #endregion
    }
}
