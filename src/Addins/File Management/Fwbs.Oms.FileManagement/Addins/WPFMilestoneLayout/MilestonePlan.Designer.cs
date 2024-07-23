using System.ComponentModel;
namespace FWBS.OMS.FileManagement.Addins
{
    partial class MilestonePlan
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
            if (disposing )
            {                
                if (milestonePlan1 != null)
                {
                    milestonePlan1.Shutdown();
                }

                if (vm != null)
                {
                    vm.PropertyChanged -= new PropertyChangedEventHandler(vm_PropertyChanged);
                    vm.Dispose();
                }
               
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.pnlFileActions = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.fileActions = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.pnlFileActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDesign
            // 
            this.pnlDesign.Controls.Add(this.pnlFileActions);
            this.pnlDesign.Controls.SetChildIndex(this.pnlFileActions, 0);
            this.pnlDesign.Controls.SetChildIndex(this.pnlActions, 0);
            // 
            // pnlActions
            // 
            this.pnlActions.Location = new System.Drawing.Point(8, 39);
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(168, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(672, 490);
            this.elementHost1.TabIndex = 9;
            this.elementHost1.Text = "elementHost1";
            // 
            // pnlFileActions
            // 
            this.pnlFileActions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(254)))));
            this.pnlFileActions.BlendColor1 = System.Drawing.Color.Empty;
            this.pnlFileActions.BlendColor2 = System.Drawing.Color.Empty;
            this.pnlFileActions.Controls.Add(this.fileActions);
            this.pnlFileActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFileActions.ExpandedHeight = 31;
            this.pnlFileActions.GlobalScope = true;
            this.pnlFileActions.HeaderColor = System.Drawing.Color.Empty;
            this.pnlFileActions.Location = new System.Drawing.Point(8, 8);
            this.resourceLookup1.SetLookup(this.pnlFileActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("FILEACTIONS", "%FILE% Actions", ""));
            this.pnlFileActions.ModernStyle = FWBS.OMS.UI.Windows.ucPanelNav.NavStyle.ModernHeader;
            this.pnlFileActions.Name = "pnlFileActions";
            this.pnlFileActions.Size = new System.Drawing.Size(152, 31);
            this.pnlFileActions.TabIndex = 2;
            this.pnlFileActions.TabStop = false;
            this.pnlFileActions.Tag = "FMFILEACTIONS";
            this.pnlFileActions.Text = "%FILE% Actions";
            // 
            // fileActions
            // 
            this.fileActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileActions.Location = new System.Drawing.Point(0, 19);
            this.fileActions.ModernStyle = true;
            this.fileActions.Name = "fileActions";
            this.fileActions.Padding = new System.Windows.Forms.Padding(5);
            this.fileActions.PanelBackColor = System.Drawing.Color.Empty;
            this.fileActions.Size = new System.Drawing.Size(152, 5);
            this.fileActions.TabIndex = 15;
            this.fileActions.TabStop = false;
            // 
            // MilestonePlan
            // 
            this.Controls.Add(this.elementHost1);
            this.Name = "MilestonePlan";
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this.elementHost1, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.pnlFileActions.ResumeLayout(false);
            this.pnlFileActions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        
        private OMS.UI.Windows.ucPanelNav pnlFileActions;
        private OMS.UI.Windows.ucNavCommands fileActions;

    }
}
