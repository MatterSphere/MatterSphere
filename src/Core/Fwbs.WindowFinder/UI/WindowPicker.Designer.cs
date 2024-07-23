namespace Fwbs
{
    namespace WinFinder.Internal
    {
        partial class WindowPicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowPicker));
            this.lblApplication = new System.Windows.Forms.Label();
            this.txtApplication = new System.Windows.Forms.TextBox();
            this.lblClass = new System.Windows.Forms.Label();
            this.lblText = new System.Windows.Forms.Label();
            this.lblHandle = new System.Windows.Forms.Label();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.txtWindowText = new System.Windows.Forms.TextBox();
            this.dragPictureBox = new System.Windows.Forms.PictureBox();
            this.txtWindowHandle = new System.Windows.Forms.TextBox();
            this.btnPick = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dragPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // lblApplication
            // 
            resources.ApplyResources(this.lblApplication, "lblApplication");
            this.lblApplication.Name = "lblApplication";
            // 
            // txtApplication
            // 
            resources.ApplyResources(this.txtApplication, "txtApplication");
            this.txtApplication.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtApplication.Name = "txtApplication";
            this.txtApplication.ReadOnly = true;
            // 
            // lblClass
            // 
            resources.ApplyResources(this.lblClass, "lblClass");
            this.lblClass.Name = "lblClass";
            // 
            // lblText
            // 
            resources.ApplyResources(this.lblText, "lblText");
            this.lblText.Name = "lblText";
            // 
            // lblHandle
            // 
            resources.ApplyResources(this.lblHandle, "lblHandle");
            this.lblHandle.Name = "lblHandle";
            // 
            // txtClassName
            // 
            resources.ApplyResources(this.txtClassName, "txtClassName");
            this.txtClassName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.ReadOnly = true;
            // 
            // txtWindowText
            // 
            resources.ApplyResources(this.txtWindowText, "txtWindowText");
            this.txtWindowText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWindowText.Name = "txtWindowText";
            // 
            // dragPictureBox
            // 
            resources.ApplyResources(this.dragPictureBox, "dragPictureBox");
            this.dragPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dragPictureBox.Name = "dragPictureBox";
            this.dragPictureBox.TabStop = false;
            this.dragPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dragPictureBox_MouseDown);
            this.dragPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dragPictureBox_MouseMove);
            this.dragPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dragPictureBox_MouseUp);
            // 
            // txtWindowHandle
            // 
            resources.ApplyResources(this.txtWindowHandle, "txtWindowHandle");
            this.txtWindowHandle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWindowHandle.Name = "txtWindowHandle";
            this.txtWindowHandle.ReadOnly = true;
            // 
            // btnPick
            // 
            resources.ApplyResources(this.btnPick, "btnPick");
            this.btnPick.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPick.Name = "btnPick";
            this.btnPick.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // WindowPicker
            // 
            this.AcceptButton = this.btnPick;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPick);
            this.Controls.Add(this.lblApplication);
            this.Controls.Add(this.txtApplication);
            this.Controls.Add(this.lblClass);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.lblHandle);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.txtWindowText);
            this.Controls.Add(this.dragPictureBox);
            this.Controls.Add(this.txtWindowHandle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "WindowPicker";
            ((System.ComponentModel.ISupportInitialize)(this.dragPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

            #endregion

            private System.Windows.Forms.Label lblApplication;
            private System.Windows.Forms.TextBox txtApplication;
            private System.Windows.Forms.Label lblClass;
            private System.Windows.Forms.Label lblText;
            private System.Windows.Forms.Label lblHandle;
            private System.Windows.Forms.TextBox txtClassName;
            private System.Windows.Forms.TextBox txtWindowText;
            private System.Windows.Forms.PictureBox dragPictureBox;
            private System.Windows.Forms.TextBox txtWindowHandle;
            private System.Windows.Forms.Button btnPick;
            private System.Windows.Forms.Button btnCancel;
            private System.Windows.Forms.Button button1;

        }
    }
}
