namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week
{
    partial class ucWeekView
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
            this.tlpContainer = new System.Windows.Forms.TableLayoutPanel();
            this.pnlRightLine = new System.Windows.Forms.Panel();
            this.pnlLeftLine = new System.Windows.Forms.Panel();
            this.weekPickerLine = new System.Windows.Forms.Panel();
            this.pnlWeeekPicker = new System.Windows.Forms.Panel();
            this.tlpWeekPicker = new System.Windows.Forms.TableLayoutPanel();
            this.weekPicker = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week.ucWeekPicker();
            this.tlpHoursContainer = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHeaderLine = new System.Windows.Forms.Panel();
            this.tlpContainer.SuspendLayout();
            this.pnlWeeekPicker.SuspendLayout();
            this.tlpWeekPicker.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpContainer
            // 
            this.tlpContainer.ColumnCount = 9;
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tlpContainer.Controls.Add(this.pnlRightLine, 8, 1);
            this.tlpContainer.Controls.Add(this.pnlLeftLine, 0, 1);
            this.tlpContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpContainer.Location = new System.Drawing.Point(0, 39);
            this.tlpContainer.Margin = new System.Windows.Forms.Padding(0);
            this.tlpContainer.Name = "tlpContainer";
            this.tlpContainer.RowCount = 2;
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContainer.Size = new System.Drawing.Size(350, 80);
            this.tlpContainer.TabIndex = 1;
            // 
            // pnlRightLine
            // 
            this.pnlRightLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.pnlRightLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRightLine.Location = new System.Drawing.Point(330, 38);
            this.pnlRightLine.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRightLine.Name = "pnlRightLine";
            this.pnlRightLine.Size = new System.Drawing.Size(20, 1);
            this.pnlRightLine.TabIndex = 4;
            // 
            // pnlLeftLine
            // 
            this.pnlLeftLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.pnlLeftLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLeftLine.Location = new System.Drawing.Point(0, 38);
            this.pnlLeftLine.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLeftLine.Name = "pnlLeftLine";
            this.pnlLeftLine.Size = new System.Drawing.Size(36, 1);
            this.pnlLeftLine.TabIndex = 3;
            // 
            // weekPickerLine
            // 
            this.weekPickerLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.weekPickerLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.weekPickerLine.Location = new System.Drawing.Point(0, 38);
            this.weekPickerLine.Name = "weekPickerLine";
            this.weekPickerLine.Size = new System.Drawing.Size(350, 1);
            this.weekPickerLine.TabIndex = 1;
            // 
            // pnlWeeekPicker
            // 
            this.pnlWeeekPicker.Controls.Add(this.tlpWeekPicker);
            this.pnlWeeekPicker.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlWeeekPicker.Location = new System.Drawing.Point(0, 0);
            this.pnlWeeekPicker.Margin = new System.Windows.Forms.Padding(0);
            this.pnlWeeekPicker.Name = "pnlWeeekPicker";
            this.pnlWeeekPicker.Size = new System.Drawing.Size(350, 38);
            this.pnlWeeekPicker.TabIndex = 2;
            // 
            // tlpWeekPicker
            // 
            this.tlpWeekPicker.ColumnCount = 3;
            this.tlpWeekPicker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWeekPicker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWeekPicker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWeekPicker.Controls.Add(this.weekPicker, 1, 0);
            this.tlpWeekPicker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWeekPicker.Location = new System.Drawing.Point(0, 0);
            this.tlpWeekPicker.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWeekPicker.Name = "tlpWeekPicker";
            this.tlpWeekPicker.RowCount = 1;
            this.tlpWeekPicker.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWeekPicker.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWeekPicker.Size = new System.Drawing.Size(350, 38);
            this.tlpWeekPicker.TabIndex = 0;
            // 
            // weekPicker
            // 
            this.weekPicker.AutoSize = true;
            this.weekPicker.BackColor = System.Drawing.Color.White;
            this.weekPicker.Cursor = System.Windows.Forms.Cursors.Hand;
            this.weekPicker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.weekPicker.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.weekPicker.FlatAppearance.BorderSize = 0;
            this.weekPicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.weekPicker.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.weekPicker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.weekPicker.Location = new System.Drawing.Point(139, 0);
            this.weekPicker.Margin = new System.Windows.Forms.Padding(0);
            this.weekPicker.Name = "weekPicker";
            this.weekPicker.Size = new System.Drawing.Size(72, 38);
            this.weekPicker.TabIndex = 0;
            this.weekPicker.Text = "Week";
            this.weekPicker.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.weekPicker.UseVisualStyleBackColor = false;
            this.weekPicker.Click += new System.EventHandler(this.weekPicker_Click);
            // 
            // tlpHoursContainer
            // 
            this.tlpHoursContainer.AutoScroll = true;
            this.tlpHoursContainer.ColumnCount = 8;
            this.tlpHoursContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpHoursContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpHoursContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpHoursContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpHoursContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpHoursContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpHoursContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpHoursContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpHoursContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHoursContainer.Location = new System.Drawing.Point(0, 120);
            this.tlpHoursContainer.Name = "tlpHoursContainer";
            this.tlpHoursContainer.RowCount = 24;
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpHoursContainer.Size = new System.Drawing.Size(350, 180);
            this.tlpHoursContainer.TabIndex = 0;
            // 
            // pnlHeaderLine
            // 
            this.pnlHeaderLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.pnlHeaderLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeaderLine.Location = new System.Drawing.Point(0, 119);
            this.pnlHeaderLine.Name = "pnlHeaderLine";
            this.pnlHeaderLine.Size = new System.Drawing.Size(350, 1);
            this.pnlHeaderLine.TabIndex = 2;
            // 
            // ucWeekView
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpHoursContainer);
            this.Controls.Add(this.pnlHeaderLine);
            this.Controls.Add(this.tlpContainer);
            this.Controls.Add(this.weekPickerLine);
            this.Controls.Add(this.pnlWeeekPicker);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Name = "ucWeekView";
            this.Size = new System.Drawing.Size(350, 300);
            this.Tag = "View";
            this.tlpContainer.ResumeLayout(false);
            this.pnlWeeekPicker.ResumeLayout(false);
            this.tlpWeekPicker.ResumeLayout(false);
            this.tlpWeekPicker.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpContainer;
        private System.Windows.Forms.Panel weekPickerLine;
        private System.Windows.Forms.Panel pnlWeeekPicker;
        private System.Windows.Forms.TableLayoutPanel tlpWeekPicker;
        private ucWeekPicker weekPicker;
        private System.Windows.Forms.TableLayoutPanel tlpHoursContainer;
        private System.Windows.Forms.Panel pnlHeaderLine;
        private System.Windows.Forms.Panel pnlRightLine;
        private System.Windows.Forms.Panel pnlLeftLine;
    }
}
