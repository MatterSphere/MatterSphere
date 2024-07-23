namespace TestApp

{
    partial class frmTest
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
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPickInstructionFile = new System.Windows.Forms.Button();
            this.lblKey = new System.Windows.Forms.Label();
            this.txtTestKey = new System.Windows.Forms.TextBox();
            this.btnValidateKey = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(93, 117);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(188, 23);
            this.btnTestConnection.TabIndex = 0;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(93, 21);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(188, 20);
            this.txtServer.TabIndex = 2;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(93, 47);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(188, 20);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "9999";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Port";
            // 
            // btnPickInstructionFile
            // 
            this.btnPickInstructionFile.Location = new System.Drawing.Point(93, 169);
            this.btnPickInstructionFile.Name = "btnPickInstructionFile";
            this.btnPickInstructionFile.Size = new System.Drawing.Size(188, 23);
            this.btnPickInstructionFile.TabIndex = 9;
            this.btnPickInstructionFile.Text = "Pick Instruction File";
            this.btnPickInstructionFile.UseVisualStyleBackColor = true;
            this.btnPickInstructionFile.Click += new System.EventHandler(this.btnPickInstructionFile_Click);
            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.Location = new System.Drawing.Point(13, 81);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(25, 13);
            this.lblKey.TabIndex = 11;
            this.lblKey.Text = "Key";
            // 
            // txtTestKey
            // 
            this.txtTestKey.Location = new System.Drawing.Point(93, 78);
            this.txtTestKey.Name = "txtTestKey";
            this.txtTestKey.Size = new System.Drawing.Size(188, 20);
            this.txtTestKey.TabIndex = 10;
            this.txtTestKey.Text = "9999";
            // 
            // btnValidateKey
            // 
            this.btnValidateKey.Location = new System.Drawing.Point(93, 143);
            this.btnValidateKey.Name = "btnValidateKey";
            this.btnValidateKey.Size = new System.Drawing.Size(188, 23);
            this.btnValidateKey.TabIndex = 12;
            this.btnValidateKey.Text = "Validate Key";
            this.btnValidateKey.UseVisualStyleBackColor = true;
            this.btnValidateKey.Click += new System.EventHandler(this.btnValidateKey_Click);
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 205);
            this.Controls.Add(this.btnValidateKey);
            this.Controls.Add(this.lblKey);
            this.Controls.Add(this.txtTestKey);
            this.Controls.Add(this.btnPickInstructionFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTestConnection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmTest";
            this.Text = "3E MatterSphere PDF Bundler Service Test";
            this.Load += new System.EventHandler(this.frmTest_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPickInstructionFile;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.TextBox txtTestKey;
        private System.Windows.Forms.Button btnValidateKey;
    }
}

