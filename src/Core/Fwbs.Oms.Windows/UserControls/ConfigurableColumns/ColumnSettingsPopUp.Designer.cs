namespace FWBS.OMS.UI.UserControls.ColumnSettings
{
    partial class ColumnSettingsPopUp
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
            this.checkBoxContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // checkBoxContainer
            // 
            this.checkBoxContainer.AutoSize = true;
            this.checkBoxContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.checkBoxContainer.Location = new System.Drawing.Point(0, 0);
            this.checkBoxContainer.Name = "checkBoxContainer";
            this.checkBoxContainer.Padding = new System.Windows.Forms.Padding(8);
            this.checkBoxContainer.Size = new System.Drawing.Size(120, 32);
            this.checkBoxContainer.TabIndex = 0;
            // 
            // ColumnSettingsPopUp
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.checkBoxContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ColumnSettingsPopUp";
            this.Size = new System.Drawing.Size(123, 35);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel checkBoxContainer;
    }
}
