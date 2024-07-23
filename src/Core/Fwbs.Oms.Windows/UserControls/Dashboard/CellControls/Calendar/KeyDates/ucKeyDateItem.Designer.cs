namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.KeyDates
{
    partial class ucKeyDateItem
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucKeyDateItem));
            this.underLine = new System.Windows.Forms.Panel();
            this.btnActions = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.toolTipLabels = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipFile = new System.Windows.Forms.ToolTip(this.components);
            this.picture = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.CalendarIcon();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.SuspendLayout();
            // 
            // underLine
            // 
            this.underLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.underLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.underLine.Location = new System.Drawing.Point(64, 57);
            this.underLine.Name = "underLine";
            this.underLine.Size = new System.Drawing.Size(326, 1);
            this.underLine.TabIndex = 1;
            // 
            // btnActions
            // 
            this.btnActions.BackColor = System.Drawing.Color.Transparent;
            this.btnActions.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnActions.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnActions.FlatAppearance.BorderSize = 0;
            this.btnActions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnActions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnActions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActions.Font = new System.Drawing.Font("Segoe UI Symbol", 10.5F);
            this.btnActions.ForeColor = System.Drawing.Color.Gray;
            this.btnActions.Location = new System.Drawing.Point(333, 0);
            this.btnActions.Name = "btnActions";
            this.btnActions.Size = new System.Drawing.Size(57, 57);
            this.btnActions.TabIndex = 2;
            this.btnActions.Text = "...";
            this.btnActions.UseVisualStyleBackColor = false;
            this.btnActions.Click += new System.EventHandler(this.btnActions_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoEllipsis = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(269, 30);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblTitle.Paint += new System.Windows.Forms.PaintEventHandler(this.lblTitle_Paint);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoEllipsis = true;
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lblDescription.Location = new System.Drawing.Point(0, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(269, 27);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description";
            this.lblDescription.Paint += new System.Windows.Forms.PaintEventHandler(this.lblDescription_Paint);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(64, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(269, 30);
            this.pnlTop.TabIndex = 5;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lblDescription);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(64, 30);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(269, 27);
            this.pnlBottom.TabIndex = 6;
            // 
            // toolTipLabels
            // 
            this.toolTipLabels.BackColor = System.Drawing.Color.White;
            this.toolTipLabels.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            // 
            // picture
            // 
            this.picture.Day = 0;
            this.picture.Dock = System.Windows.Forms.DockStyle.Left;
            this.picture.Location = new System.Drawing.Point(0, 0);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(64, 58);
            this.picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picture.TabIndex = 0;
            this.picture.TabStop = false;
            this.picture.DpiChangedAfterParent += new System.EventHandler(this.picture_DpiChangedAfterParent);
            // 
            // ucKeyDateItem
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.btnActions);
            this.Controls.Add(this.underLine);
            this.Controls.Add(this.picture);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Name = "ucKeyDateItem";
            this.Size = new System.Drawing.Size(390, 58);
            this.pnlTop.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.CalendarIcon picture;
        private System.Windows.Forms.Panel underLine;
        private System.Windows.Forms.Button btnActions;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.ToolTip toolTipLabels;
        private System.Windows.Forms.ToolTip toolTipFile;
    }
}
