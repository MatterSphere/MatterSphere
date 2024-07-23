namespace FWBS.OMS.FinancialTile
{
    partial class FinancialDashboardItem
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
            _cancellationTokenSource.Cancel();

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
            this.ucDataView = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common.ucDataView();
            this.SuspendLayout();
            // 
            // ucDataView
            // 
            this.ucDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDataView.Location = new System.Drawing.Point(0, 0);
            this.ucDataView.Name = "ucDataView";
            this.ucDataView.Size = new System.Drawing.Size(467, 462);
            this.ucDataView.TabIndex = 1;
            // 
            // FinancialDashboardItem
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ucDataView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Name = "FinancialDashboardItem";
            this.Size = new System.Drawing.Size(467, 462);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.UserControls.Dashboard.CellControls.Common.ucDataView ucDataView;
    }
}
