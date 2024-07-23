namespace FWBS.OMS.Workflow.Admin
{
	partial class WorkflowList
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
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpList
            // 
            this.tpList.Location = new System.Drawing.Point(42, 25);
            this.tpList.Size = new System.Drawing.Size(412, 311);
            // 
            // tpEdit
            // 
            this.tpEdit.Location = new System.Drawing.Point(3, 4);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Size = new System.Drawing.Size(412, 311);
            // 
            // pnlEdit
            // 
            this.pnlEdit.Size = new System.Drawing.Size(412, 50);
            // 
            // labSelectedObject
            // 
            this.labSelectedObject.Size = new System.Drawing.Size(412, 22);
            // 
            // tbcEdit
            // 
            this.tbcEdit.Size = new System.Drawing.Size(412, 26);
            // 
            // tbSave
            // 
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            // 
            // tbClose
            // 
            this.BresourceLookup1.SetLookup(this.tbClose, new FWBS.OMS.UI.Windows.ResourceLookupItem("Close", "Close", ""));
            // 
            // tbReturn
            // 
            this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
            // 
            // lstList
            // 
            this.lstList.Size = new System.Drawing.Size(412, 311);
            // 
            // pnlToolbarContainer
            // 
            this.pnlToolbarContainer.Size = new System.Drawing.Size(412, 26);
            // 
            // WorkflowList
            // 
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "WorkflowList";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Size = new System.Drawing.Size(470, 377);
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion
	}
}
