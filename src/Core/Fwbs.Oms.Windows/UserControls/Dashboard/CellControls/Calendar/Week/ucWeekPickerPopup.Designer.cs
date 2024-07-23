using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week
{
    partial class ucWeekPickerPopup
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

            while (this.flpContainer.Controls.Count > 0)
            {
                var control = this.flpContainer.Controls[0];
                var item = control as ucWeekPickerPopupItem;
                if (item != null)
                {
                    item.ItemClicked -= OnItemClick;
                }
                
                this.flpContainer.Controls.Remove(control);
                control.Dispose();
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
            this.flpContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpContainer
            // 
            this.flpContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpContainer.Location = new System.Drawing.Point(0, 0);
            this.flpContainer.Margin = new System.Windows.Forms.Padding(0);
            this.flpContainer.Name = "flpContainer";
            this.flpContainer.Size = new System.Drawing.Size(150, 155);
            this.flpContainer.TabIndex = 0;
            // 
            // ucWeekPickerPopup
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.flpContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.MinimumSize = new System.Drawing.Size(150, 0);
            this.Name = "ucWeekPickerPopup";
            this.Size = new System.Drawing.Size(150, 155);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpContainer;
    }
}
