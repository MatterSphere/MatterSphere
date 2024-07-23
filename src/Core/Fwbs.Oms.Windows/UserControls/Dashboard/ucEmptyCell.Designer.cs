namespace FWBS.OMS.UI.UserControls.Dashboard
{
    partial class ucEmptyCell
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
            this.panel = new System.Windows.Forms.Panel();
            this.tlpContainer = new System.Windows.Forms.TableLayoutPanel();
            this.plus = new System.Windows.Forms.PictureBox();
            this.lblAddModule = new System.Windows.Forms.Label();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.panel.SuspendLayout();
            this.tlpContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plus)).BeginInit();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panel.Controls.Add(this.tlpContainer);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(4, 4);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(332, 332);
            this.panel.TabIndex = 0;
            // 
            // tlpContainer
            // 
            this.tlpContainer.BackColor = System.Drawing.Color.Transparent;
            this.tlpContainer.ColumnCount = 3;
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContainer.Controls.Add(this.plus, 1, 1);
            this.tlpContainer.Controls.Add(this.lblAddModule, 1, 2);
            this.tlpContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContainer.Location = new System.Drawing.Point(0, 0);
            this.tlpContainer.Name = "tlpContainer";
            this.tlpContainer.RowCount = 4;
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContainer.Size = new System.Drawing.Size(332, 332);
            this.tlpContainer.TabIndex = 0;
            this.tlpContainer.MouseLeave += new System.EventHandler(this.tlpContainer_MouseLeave);
            this.tlpContainer.MouseHover += new System.EventHandler(this.tlpContainer_MouseHover);
            // 
            // plus
            // 
            this.plus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plus.Location = new System.Drawing.Point(126, 136);
            this.plus.Margin = new System.Windows.Forms.Padding(0, 0, 0, 13);
            this.plus.Name = "plus";
            this.plus.Size = new System.Drawing.Size(79, 32);
            this.plus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.plus.TabIndex = 0;
            this.plus.TabStop = false;
            this.plus.Click += new System.EventHandler(this.plus_Click);
            this.plus.DpiChangedAfterParent += new System.EventHandler(this.plus_DpiChangedAfterParent);
            // 
            // lblAddModule
            // 
            this.lblAddModule.AutoSize = true;
            this.lblAddModule.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAddModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddModule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.lblAddModule.Location = new System.Drawing.Point(129, 181);
            this.resourceLookup1.SetLookup(this.lblAddModule, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblAddModule", "Add Module", ""));
            this.lblAddModule.Name = "lblAddModule";
            this.lblAddModule.Size = new System.Drawing.Size(73, 15);
            this.lblAddModule.TabIndex = 1;
            this.lblAddModule.Text = "Add Module";
            this.lblAddModule.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAddModule.Click += new System.EventHandler(this.lblAddModule_Click);
            // 
            // ucEmptyCell
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ucEmptyCell";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(340, 340);
            this.panel.ResumeLayout(false);
            this.tlpContainer.ResumeLayout(false);
            this.tlpContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.TableLayoutPanel tlpContainer;
        private System.Windows.Forms.PictureBox plus;
        private System.Windows.Forms.Label lblAddModule;
        private Windows.ResourceLookup resourceLookup1;
    }
}
