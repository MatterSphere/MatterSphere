using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// Summary description for eComboBoxForm.
    /// </summary>
    public class eComboBoxForm : System.Windows.Forms.Form
	{
		public System.Windows.Forms.ListBox lstList;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Control _sender;

		public eComboBoxForm(Control sender)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_sender = sender;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            try
            {
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lstList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstList
            // 
            this.lstList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstList.IntegralHeight = false;
            this.lstList.Location = new System.Drawing.Point(1, 1);
            this.lstList.Name = "lstList";
            this.lstList.Size = new System.Drawing.Size(194, 87);
            this.lstList.TabIndex = 0;
            this.lstList.Enter += new System.EventHandler(this.lstList_Enter);
            this.lstList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstList_MouseDown);
            // 
            // eComboBoxForm
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(196, 89);
            this.ControlBox = false;
            this.Controls.Add(this.lstList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "eComboBoxForm";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "eComboBoxForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.eComboBoxForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.eComboBoxForm_Paint);
            this.ResumeLayout(false);

		}
		#endregion

		private void eComboBoxForm_Load(object sender, System.EventArgs e)
		{
			
		}

		private void eComboBoxForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
            using (Pen p1 = new Pen(Color.Black, 1))
            {
                e.Graphics.DrawRectangle(p1, 0, 0, this.Width - 1, this.Height - 1);
            }
		}

		private void lstList_Enter(object sender, System.EventArgs e)
		{
			if (_sender != null)
				_sender.Focus();
		}

		private void lstList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (_sender != null)
				_sender.Focus();
		}
	}
}
