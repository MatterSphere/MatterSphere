using System.ComponentModel;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucValidatePageChange.
    /// </summary>
    public class ucValidatePageChange : FWBS.Common.UI.Windows.eBase2
	{
		#region Fields
		/// <summary>
		/// The Design Time View
		/// </summary>
		private System.Windows.Forms.PictureBox picDesignMode;
        #endregion

        #region	Properties
        [Browsable(false)]
        public override bool CaptionTop
        {
            get
            {
                return false;
            }
            set { }
        }
        #endregion

        #region Constructors
        public ucValidatePageChange() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			base.CaptionWidth = 0;
			_ctrl = this.picDesignMode;
			this.picDesignMode.Image = FWBS.OMS.UI.Windows.Images.AdminMenu48().Images[3];
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.picDesignMode = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// picDesignMode
			// 
			this.picDesignMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picDesignMode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picDesignMode.Location = new System.Drawing.Point(0, 0);
			this.picDesignMode.Name = "picDesignMode";
			this.picDesignMode.Size = new System.Drawing.Size(61, 57);
			this.picDesignMode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picDesignMode.TabIndex = 1;
			this.picDesignMode.TabStop = false;
			// 
			// ucValidatePageChange
			// 
			this.Controls.Add(this.picDesignMode);
			this.Size = new System.Drawing.Size(61, 57);
			this.ResumeLayout(false);

		}
		#endregion
		#endregion


	}
}
