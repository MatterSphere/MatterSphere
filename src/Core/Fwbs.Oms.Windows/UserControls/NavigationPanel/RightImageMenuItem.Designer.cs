using System.Drawing;

namespace FWBS.Common.UI.Windows
{
    partial class RightImageMenuItem
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
            // RightImageMenuItem
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chevron);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.FlatAppearance.BorderSize = 0;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Size = new System.Drawing.Size(85, 40);
            this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.UseVisualStyleBackColor = false;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label chevron;
    }
}
