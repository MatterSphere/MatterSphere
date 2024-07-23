namespace FWBS.OMS.UI.Windows
{
    partial class ucFormsOnline
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
            this.btnRequest = new System.Windows.Forms.Button();
            this.labFormOnline = new System.Windows.Forms.Label();
            this.labFormName = new System.Windows.Forms.Label();
            this.labUpdated = new System.Windows.Forms.Label();
            this.labUpdateCaption = new System.Windows.Forms.Label();
            this.picProcessing = new System.Windows.Forms.PictureBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.contextCommands = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCancelRequest = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.picProcessing)).BeginInit();
            this.contextCommands.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRequest
            // 
            this.btnRequest.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRequest.AutoSize = true;
            this.btnRequest.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRequest.Location = new System.Drawing.Point(0, 1);
            this.btnRequest.Margin = new System.Windows.Forms.Padding(0, 1, 3, 1);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Padding = new System.Windows.Forms.Padding(6, 1, 6, 2);
            this.btnRequest.Size = new System.Drawing.Size(69, 26);
            this.btnRequest.TabIndex = 2;
            this.btnRequest.Text = "Request";
            this.btnRequest.UseVisualStyleBackColor = true;
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            // 
            // labFormOnline
            // 
            this.labFormOnline.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labFormOnline.AutoSize = true;
            this.labFormOnline.Location = new System.Drawing.Point(96, 7);
            this.labFormOnline.Name = "labFormOnline";
            this.labFormOnline.Size = new System.Drawing.Size(69, 13);
            this.labFormOnline.TabIndex = 4;
            this.labFormOnline.Text = "Form Online :";
            this.labFormOnline.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labFormName
            // 
            this.labFormName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labFormName.AutoSize = true;
            this.labFormName.Location = new System.Drawing.Point(171, 7);
            this.labFormName.Name = "labFormName";
            this.labFormName.Size = new System.Drawing.Size(85, 13);
            this.labFormName.TabIndex = 5;
            this.labFormName.Text = "Test Form Name";
            this.labFormName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labUpdated
            // 
            this.labUpdated.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labUpdated.AutoSize = true;
            this.labUpdated.Location = new System.Drawing.Point(402, 7);
            this.labUpdated.Name = "labUpdated";
            this.labUpdated.Size = new System.Drawing.Size(95, 13);
            this.labUpdated.TabIndex = 7;
            this.labUpdated.Text = "29/01/2009 16:20";
            this.labUpdated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labUpdateCaption
            // 
            this.labUpdateCaption.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labUpdateCaption.AutoSize = true;
            this.labUpdateCaption.Location = new System.Drawing.Point(339, 7);
            this.labUpdateCaption.Name = "labUpdateCaption";
            this.labUpdateCaption.Size = new System.Drawing.Size(57, 13);
            this.labUpdateCaption.TabIndex = 6;
            this.labUpdateCaption.Text = "Updated : ";
            this.labUpdateCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picProcessing
            // 
            this.picProcessing.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picProcessing.BackColor = System.Drawing.Color.Transparent;
            this.picProcessing.Image = global::FWBS.OMS.Properties.Resources.gears_animated2;
            this.picProcessing.Location = new System.Drawing.Point(502, 1);
            this.picProcessing.Margin = new System.Windows.Forms.Padding(0);
            this.picProcessing.Name = "picProcessing";
            this.picProcessing.Size = new System.Drawing.Size(26, 26);
            this.picProcessing.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picProcessing.TabIndex = 9;
            this.picProcessing.TabStop = false;
            this.picProcessing.Visible = false;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnDown.Font = new System.Drawing.Font("Wingdings 3", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Location = new System.Drawing.Point(72, 1);
            this.btnDown.Margin = new System.Windows.Forms.Padding(0, 1, 3, 1);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(18, 26);
            this.btnDown.TabIndex = 3;
            this.btnDown.Text = "q";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // contextCommands
            // 
            this.contextCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCancelRequest});
            this.contextCommands.Name = "contextCommands";
            this.contextCommands.Size = new System.Drawing.Size(156, 26);
            // 
            // mnuCancelRequest
            // 
            this.mnuCancelRequest.Name = "mnuCancelRequest";
            this.mnuCancelRequest.Size = new System.Drawing.Size(155, 22);
            this.mnuCancelRequest.Text = "Cancel Request";
            this.mnuCancelRequest.Click += new System.EventHandler(this.mnuCancelRequest_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 7;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.flowLayoutPanel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labFormOnline, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labFormName, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.labUpdateCaption, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.labUpdated, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.picProcessing, 6, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(530, 28);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel.Controls.Add(this.btnRequest);
            this.flowLayoutPanel.Controls.Add(this.btnDown);
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(93, 28);
            this.flowLayoutPanel.TabIndex = 1;
            this.flowLayoutPanel.WrapContents = false;
            // 
            // ucFormsOnline
            // 
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "ucFormsOnline";
            this.Size = new System.Drawing.Size(530, 28);
            this.ParentChanged += new System.EventHandler(this.ucFormsOnline_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.picProcessing)).EndInit();
            this.contextCommands.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRequest;
        private System.Windows.Forms.Label labFormOnline;
        private System.Windows.Forms.Label labFormName;
        private System.Windows.Forms.Label labUpdated;
        private System.Windows.Forms.Label labUpdateCaption;
        private System.Windows.Forms.PictureBox picProcessing;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.ContextMenuStrip contextCommands;
        private System.Windows.Forms.ToolStripMenuItem mnuCancelRequest;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
    }
}
