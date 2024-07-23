using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for frmGrid.
    /// </summary>
    public class frmGrid : BaseForm
	{
		private FWBS.OMS.UI.Windows.DataGridEx dataGrid1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Splitter splitter1;
        private Panel pnlBackground;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmGrid(IListSource Source)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			dataGrid1.DataSource = Source;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.dataGrid1 = new FWBS.OMS.UI.Windows.DataGridEx();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlBackground = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGrid1
            // 
            this.dataGrid1.DataMember = "";
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(0, 0);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(580, 461);
            this.dataGrid1.TabIndex = 1;
            this.dataGrid1.CurrentCellChanged += new System.EventHandler(this.dataGrid1_CurrentCellChanged);
            this.dataGrid1.Navigate += new System.Windows.Forms.NavigateEventHandler(this.dataGrid1_Navigate);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(584, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(240, 461);
            this.propertyGrid1.TabIndex = 3;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(580, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 461);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.dataGrid1);
            this.pnlBackground.Controls.Add(this.splitter1);
            this.pnlBackground.Controls.Add(this.propertyGrid1);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(824, 461);
            this.pnlBackground.TabIndex = 0;
            // 
            // frmGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(824, 461);
            this.Controls.Add(this.pnlBackground);
            this.Name = "frmGrid";
            this.Text = "Debug";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGrid_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.pnlBackground.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void dataGrid1_Navigate(object sender, System.Windows.Forms.NavigateEventArgs ne)
		{
			dataGrid1_CurrentCellChanged(sender,EventArgs.Empty);
		}

		private void dataGrid1_CurrentCellChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (dataGrid1.DataSource is DataSet)
				{
					DataSet ds = dataGrid1.DataSource as DataSet;
					propertyGrid1.SelectedObject =  ds.Tables[dataGrid1.DataMember].Columns[dataGrid1.CurrentCell.ColumnNumber];
				}
				else if (dataGrid1.DataSource is DataTable)
				{
					DataTable dt = dataGrid1.DataSource as DataTable;
					propertyGrid1.SelectedObject =  dt.Columns[dataGrid1.CurrentCell.ColumnNumber];
				}
				else
				{
					propertyGrid1.Visible=false;
					splitter1.Visible = false;
				}
			}
			catch
			{}

		}

        private void frmGrid_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataGrid1.DataSource = null;
            propertyGrid1.SelectedObject = null;
        }

	}
}
