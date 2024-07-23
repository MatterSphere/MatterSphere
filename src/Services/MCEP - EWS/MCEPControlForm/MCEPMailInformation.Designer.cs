namespace MCEPControlForm
{
    partial class MCEPMailInformation
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtStoreID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGetInformation = new System.Windows.Forms.Button();
            this.txtMailInformation = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtStoreID
            // 
            this.txtStoreID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStoreID.Location = new System.Drawing.Point(12, 19);
            this.txtStoreID.Name = "txtStoreID";
            this.txtStoreID.Size = new System.Drawing.Size(892, 20);
            this.txtStoreID.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "StoreID";
            // 
            // btnGetInformation
            // 
            this.btnGetInformation.Location = new System.Drawing.Point(13, 44);
            this.btnGetInformation.Name = "btnGetInformation";
            this.btnGetInformation.Size = new System.Drawing.Size(148, 27);
            this.btnGetInformation.TabIndex = 2;
            this.btnGetInformation.Text = "GetMailInformation";
            this.btnGetInformation.UseVisualStyleBackColor = true;
            this.btnGetInformation.Click += new System.EventHandler(this.btnGetInformation_Click);
            // 
            // txtMailInformation
            // 
            this.txtMailInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMailInformation.Location = new System.Drawing.Point(14, 82);
            this.txtMailInformation.Multiline = true;
            this.txtMailInformation.Name = "txtMailInformation";
            this.txtMailInformation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMailInformation.Size = new System.Drawing.Size(889, 162);
            this.txtMailInformation.TabIndex = 3;
            // 
            // MCEPMailInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 261);
            this.Controls.Add(this.txtMailInformation);
            this.Controls.Add(this.btnGetInformation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtStoreID);
            this.Name = "MCEPMailInformation";
            this.Text = "MCEP Mail Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStoreID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGetInformation;
        private System.Windows.Forms.TextBox txtMailInformation;
    }
}