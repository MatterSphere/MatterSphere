using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    partial class ucSearchConflicts
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblCaption = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.tableCaption = new System.Windows.Forms.TableLayoutPanel();
            this.lblConflicts = new System.Windows.Forms.Label();
            this.pnlResults = new System.Windows.Forms.Panel();
            this.pnlLabels = new System.Windows.Forms.Panel();
            this.pnlOpens = new System.Windows.Forms.Panel();
            this.pnlGroup = new System.Windows.Forms.Panel();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            this.tableCaption.SuspendLayout();
            this.pnlResults.SuspendLayout();
            this.pnlGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(60, 0);
            this.groupBox1.TabIndex = 6;
            // 
            // lblCaption
            // 
            this.lblCaption.AutoSize = true;
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCaption.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCaption.Location = new System.Drawing.Point(106, 0);
            this.lblCaption.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.lblCaption.MinimumSize = new System.Drawing.Size(150, 25);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(190, 25);
            this.lblCaption.TabIndex = 3;
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWarning
            // 
            this.lblWarning.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblWarning.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblWarning.Location = new System.Drawing.Point(0, 14);
            this.resourceLookup1.SetLookup(this.lblWarning, new FWBS.OMS.UI.Windows.ResourceLookupItem("PersonExists", "This person may already be in the system. Try re-entering a name or use an existi" +
            "ng contact from this list to continue.", ""));
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Padding = new System.Windows.Forms.Padding(18, 43, 18, 18);
            this.lblWarning.Size = new System.Drawing.Size(536, 92);
            this.lblWarning.TabIndex = 5;
            this.lblWarning.Visible = false;
            // 
            // tableCaption
            // 
            this.tableCaption.AutoSize = true;
            this.tableCaption.ColumnCount = 2;
            this.tableCaption.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableCaption.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableCaption.Controls.Add(this.lblCaption, 1, 0);
            this.tableCaption.Controls.Add(this.lblConflicts, 0, 0);
            this.tableCaption.Location = new System.Drawing.Point(0, 14);
            this.tableCaption.Name = "tableCaption";
            this.tableCaption.RowCount = 1;
            this.tableCaption.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableCaption.Size = new System.Drawing.Size(308, 25);
            this.tableCaption.TabIndex = 4;
            // 
            // lblConflicts
            // 
            this.lblConflicts.AutoSize = true;
            this.lblConflicts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConflicts.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblConflicts.Location = new System.Drawing.Point(12, 0);
            this.lblConflicts.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblConflicts.Name = "lblConflicts";
            this.lblConflicts.Size = new System.Drawing.Size(94, 25);
            this.lblConflicts.TabIndex = 4;
            this.lblConflicts.Text = "Conflicts search:";
            this.lblConflicts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlResults
            // 
            this.pnlResults.AutoScroll = true;
            this.pnlResults.AutoSize = true;
            this.pnlResults.Controls.Add(this.pnlLabels);
            this.pnlResults.Controls.Add(this.pnlOpens);
            this.pnlResults.Controls.Add(this.pnlGroup);
            this.pnlResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResults.Location = new System.Drawing.Point(0, 106);
            this.pnlResults.Margin = new System.Windows.Forms.Padding(0);
            this.pnlResults.Name = "pnlResults";
            this.pnlResults.Size = new System.Drawing.Size(536, 186);
            this.pnlResults.TabIndex = 7;
            // 
            // pnlLabels
            // 
            this.pnlLabels.AutoSize = true;
            this.pnlLabels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLabels.Location = new System.Drawing.Point(37, 0);
            this.pnlLabels.Name = "pnlLabels";
            this.pnlLabels.Size = new System.Drawing.Size(433, 186);
            this.pnlLabels.TabIndex = 8;
            // 
            // pnlOpens
            // 
            this.pnlOpens.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlOpens.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlOpens.Location = new System.Drawing.Point(470, 0);
            this.pnlOpens.Name = "pnlOpens";
            this.pnlOpens.Padding = new System.Windows.Forms.Padding(0, 0, 22, 0);
            this.pnlOpens.Size = new System.Drawing.Size(66, 186);
            this.pnlOpens.TabIndex = 9;
            // 
            // pnlGroup
            // 
            this.pnlGroup.Controls.Add(this.groupBox1);
            this.pnlGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlGroup.Location = new System.Drawing.Point(0, 0);
            this.pnlGroup.Name = "pnlGroup";
            this.pnlGroup.Size = new System.Drawing.Size(37, 186);
            this.pnlGroup.TabIndex = 7;
            // 
            // ucSearchConflicts
            // 
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(252)))));
            this.Controls.Add(this.tableCaption);
            this.Controls.Add(this.pnlResults);
            this.Controls.Add(this.lblWarning);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(340, 240);
            this.Name = "ucSearchConflicts";
            this.Padding = new System.Windows.Forms.Padding(0, 14, 0, 14);
            this.Size = new System.Drawing.Size(536, 306);
            this.MouseLeave += new System.EventHandler(this.ucSearchConflicts_MouseLeave);
            this.ParentChanged += new System.EventHandler(this.ucSearchConflicts_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            this.tableCaption.ResumeLayout(false);
            this.tableCaption.PerformLayout();
            this.pnlResults.ResumeLayout(false);
            this.pnlResults.PerformLayout();
            this.pnlGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableCaption;
        private System.Windows.Forms.Label lblConflicts;
        private System.Windows.Forms.Label lblWarning;
        private Infragistics.Win.Misc.UltraGroupBox groupBox1;
        private Panel pnlResults;
        private Panel pnlLabels;
        private Panel pnlOpens;
        private Panel pnlGroup;
        private ResourceLookup resourceLookup1;
    }
}
