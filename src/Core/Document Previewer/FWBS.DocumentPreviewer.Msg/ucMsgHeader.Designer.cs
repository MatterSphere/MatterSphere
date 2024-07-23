namespace Fwbs.Documents.Preview.Msg
{
    partial class ucMsgHeader
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
            this.subject = new System.Windows.Forms.Label();
            this.From = new System.Windows.Forms.Label();
            this.sent = new System.Windows.Forms.Label();
            this.pnlSent = new System.Windows.Forms.Panel();
            this.SentDate = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listTo = new System.Windows.Forms.FlowLayoutPanel();
            this.To = new System.Windows.Forms.Label();
            this.pnlCC = new System.Windows.Forms.Panel();
            this.lstCC = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlAttach = new System.Windows.Forms.Panel();
            this.lstAttachments = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlSent.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlCC.SuspendLayout();
            this.pnlAttach.SuspendLayout();
            this.SuspendLayout();
            // 
            // subject
            // 
            this.subject.AutoSize = true;
            this.subject.Dock = System.Windows.Forms.DockStyle.Top;
            this.subject.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subject.Location = new System.Drawing.Point(0, 0);
            this.subject.Name = "subject";
            this.subject.Size = new System.Drawing.Size(60, 19);
            this.subject.TabIndex = 0;
            this.subject.Text = "Subject";
            // 
            // From
            // 
            this.From.AutoSize = true;
            this.From.Dock = System.Windows.Forms.DockStyle.Top;
            this.From.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.From.Location = new System.Drawing.Point(0, 19);
            this.From.Name = "From";
            this.From.Size = new System.Drawing.Size(41, 19);
            this.From.TabIndex = 1;
            this.From.Text = "From";
            // 
            // sent
            // 
            this.sent.AutoSize = true;
            this.sent.Dock = System.Windows.Forms.DockStyle.Left;
            this.sent.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sent.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sent.Location = new System.Drawing.Point(0, 0);
            this.sent.Name = "sent";
            this.sent.Size = new System.Drawing.Size(33, 15);
            this.sent.TabIndex = 2;
            this.sent.Text = "Sent:";
            // 
            // pnlSent
            // 
            this.pnlSent.Controls.Add(this.SentDate);
            this.pnlSent.Controls.Add(this.sent);
            this.pnlSent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSent.Location = new System.Drawing.Point(0, 38);
            this.pnlSent.Name = "pnlSent";
            this.pnlSent.Size = new System.Drawing.Size(524, 16);
            this.pnlSent.TabIndex = 3;
            // 
            // SentDate
            // 
            this.SentDate.AutoSize = true;
            this.SentDate.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SentDate.Location = new System.Drawing.Point(81, 0);
            this.SentDate.Name = "SentDate";
            this.SentDate.Size = new System.Drawing.Size(58, 15);
            this.SentDate.TabIndex = 3;
            this.SentDate.Text = "Sent Date";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listTo);
            this.panel2.Controls.Add(this.To);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 54);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(524, 18);
            this.panel2.TabIndex = 4;
            // 
            // listTo
            // 
            this.listTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listTo.AutoScroll = true;
            this.listTo.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listTo.Location = new System.Drawing.Point(79, 0);
            this.listTo.Name = "listTo";
            this.listTo.Size = new System.Drawing.Size(444, 18);
            this.listTo.TabIndex = 3;
            // 
            // To
            // 
            this.To.AutoSize = true;
            this.To.Dock = System.Windows.Forms.DockStyle.Left;
            this.To.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.To.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.To.Location = new System.Drawing.Point(0, 0);
            this.To.Name = "To";
            this.To.Size = new System.Drawing.Size(22, 15);
            this.To.TabIndex = 2;
            this.To.Text = "To:";
            // 
            // pnlCC
            // 
            this.pnlCC.Controls.Add(this.lstCC);
            this.pnlCC.Controls.Add(this.label2);
            this.pnlCC.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCC.Location = new System.Drawing.Point(0, 72);
            this.pnlCC.Name = "pnlCC";
            this.pnlCC.Size = new System.Drawing.Size(524, 18);
            this.pnlCC.TabIndex = 5;
            // 
            // lstCC
            // 
            this.lstCC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstCC.AutoScroll = true;
            this.lstCC.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstCC.Location = new System.Drawing.Point(79, 0);
            this.lstCC.Name = "lstCC";
            this.lstCC.Size = new System.Drawing.Size(444, 18);
            this.lstCC.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Cc:";
            // 
            // pnlAttach
            // 
            this.pnlAttach.Controls.Add(this.lstAttachments);
            this.pnlAttach.Controls.Add(this.label3);
            this.pnlAttach.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAttach.Location = new System.Drawing.Point(0, 90);
            this.pnlAttach.Name = "pnlAttach";
            this.pnlAttach.Size = new System.Drawing.Size(524, 18);
            this.pnlAttach.TabIndex = 6;
            // 
            // lstAttachments
            // 
            this.lstAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstAttachments.AutoScroll = true;
            this.lstAttachments.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstAttachments.Location = new System.Drawing.Point(79, 0);
            this.lstAttachments.Name = "lstAttachments";
            this.lstAttachments.Size = new System.Drawing.Size(444, 18);
            this.lstAttachments.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Attachments:";
            // 
            // ucMsgHeader
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.pnlAttach);
            this.Controls.Add(this.pnlCC);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlSent);
            this.Controls.Add(this.From);
            this.Controls.Add(this.subject);
            this.Name = "ucMsgHeader";
            this.Size = new System.Drawing.Size(524, 114);
            this.Resize += new System.EventHandler(this.ucMsgHeader_Resize);
            this.pnlSent.ResumeLayout(false);
            this.pnlSent.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlCC.ResumeLayout(false);
            this.pnlCC.PerformLayout();
            this.pnlAttach.ResumeLayout(false);
            this.pnlAttach.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label subject;
        private System.Windows.Forms.Label From;
        private System.Windows.Forms.Label sent;
        private System.Windows.Forms.Panel pnlSent;
        private System.Windows.Forms.Label SentDate;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label To;
        private System.Windows.Forms.Panel pnlCC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlAttach;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel listTo;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.FlowLayoutPanel lstCC;
        private System.Windows.Forms.FlowLayoutPanel lstAttachments;
    }
}
