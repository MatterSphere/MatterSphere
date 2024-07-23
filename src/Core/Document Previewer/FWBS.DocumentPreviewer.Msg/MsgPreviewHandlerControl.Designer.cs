namespace Fwbs.Documents.Preview.Msg
{
    partial class MsgPreviewHandlerControl
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
            try
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.ucMsgHeader1 = new ucMsgHeader();
            this.subPreviewContainer = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 116);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(491, 225);
            this.webBrowser1.TabIndex = 1;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // ucMsgHeader1
            // 
            this.ucMsgHeader1.BackColor = System.Drawing.SystemColors.Window;
            this.ucMsgHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucMsgHeader1.Location = new System.Drawing.Point(0, 0);
            this.ucMsgHeader1.Name = "ucMsgHeader1";
            this.ucMsgHeader1.Size = new System.Drawing.Size(491, 116);
            this.ucMsgHeader1.TabIndex = 2;
            // 
            // subPreviewContainer
            // 
            this.subPreviewContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subPreviewContainer.Location = new System.Drawing.Point(0, 0);
            this.subPreviewContainer.Name = "subPreviewContainer";
            this.subPreviewContainer.Size = new System.Drawing.Size(491, 341);
            this.subPreviewContainer.TabIndex = 3;
            // 
            // MsgPreviewHandler
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.ucMsgHeader1);
            this.Controls.Add(this.subPreviewContainer);
            this.Name = "MsgPreviewHandler";
            this.Size = new System.Drawing.Size(491, 341);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ToolTip toolTip1;
        private ucMsgHeader ucMsgHeader1;
        private System.Windows.Forms.Panel subPreviewContainer;
        //private Previewer previewer1;

    }
}
