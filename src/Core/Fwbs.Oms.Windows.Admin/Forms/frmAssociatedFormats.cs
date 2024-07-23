using System;
using System.ComponentModel;
using System.Linq;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for frmAssociatedFormats.
    /// </summary>
    public class frmAssociatedFormats : FWBS.OMS.UI.Windows.BaseForm
	{
		public FWBS.OMS.UI.Windows.ucSelectionList SelectionList;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        protected ResourceLookup resourceLookup1;
        private IContainer components;

		public frmAssociatedFormats()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            //if this is specified in initialize components the form wont load
            this.SelectionList.CodeType = "SUBASSOC";
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAssociatedFormats));
            this.SelectionList = new FWBS.OMS.UI.Windows.ucSelectionList();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectionList
            // 
            this.SelectionList.AvailableHeading = "Available %ASSOCIATE% Formats";
            this.SelectionList.CodeType = null;
            this.SelectionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectionList.InputDisplayMember = null;
            this.SelectionList.InputValueMember = null;
            this.SelectionList.Location = new System.Drawing.Point(5, 5);
            this.SelectionList.Name = "SelectionList";
            this.SelectionList.OutputDisplayMember = null;
            this.SelectionList.OutputValueMember = null;
            this.SelectionList.SelectedHeading = "Selected %ASSOCIATE% Formats";
            this.SelectionList.SelectionListTitle = "Select what Associated Formats are available for the Contact Type";
            this.SelectionList.Size = new System.Drawing.Size(648, 391);
            this.SelectionList.TabIndex = 0;
            this.SelectionList.Changing += new System.EventHandler<FWBS.OMS.UI.Windows.BeforeSelectionChangeEventArgs>(this.SelectionList_Changing);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(5, 396);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(648, 32);
            this.panel1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(571, 4);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cance&l";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(491, 4);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            // 
            // frmAssociatedFormats
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(658, 433);
            this.Controls.Add(this.SelectionList);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAssociatedFormats";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Associated Formats";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        void SelectionList_Changing(object sender, BeforeSelectionChangeEventArgs e)
        {
            if (e.Direction == Direction.Add)
                return;

            var result = e.DataRows.Any(n => 
                Convert.ToString(n["Value"]).ToUpperInvariant() == "CLIENT" || 
                Convert.ToString(n["Value"]).ToUpperInvariant() == "SOURCE");
            if (result)
            {
                MessageBox.ShowInformation("You cannot remove Client or Source of Business");
                e.Cancel = true;
            }
        }
		#endregion

	}
}
