using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmMenu.
    /// </summary>
    public class frmMenu : BaseForm
	{
		#region Fields
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// The Menu User Control
		/// 
		private FWBS.OMS.UI.Windows.ucHome ucHome1;
		private System.Windows.Forms.Label labLic;
		/// <summary>
		/// Stores the Parent Form
		/// </summary>
		private frmMain _mainparent = null;
		#endregion

		#region Events
		public event OpenFolderEventHandler FolderOpening;

		protected virtual void OnFolderOpening(object sender, OpenFolderEventArgs e)
		{
			if (FolderOpening != null)
				FolderOpening(sender,e);
		}
		
		public event HandledEventHandler GoClick;

		public void OnGoClick(HandledEventArgs e)
		{
			if (GoClick!= null)
				GoClick(this,e);
		}


		/// <summary>
		/// Passed from the Menu User Control
		/// </summary>
		public event EventHandler MenuChanged;

		public void OnMenuChanged(object sender, EventArgs e)
		{
			if (MenuChanged != null)
				MenuChanged(sender,e);
		}

		/// <summary>
		/// Passed from the Menu User Control
		/// </summary>
		public event MenuEventHandler MenuActioned;

		public void OnMenuActioned(object sender, MenuEventArgs e)
		{
			if (MenuActioned != null)
				MenuActioned(sender,e);
		}

		/// <summary>
		/// Passed from the Menu User Control
		/// </summary>
		public event EventHandler ParentEnabledChanged;

		public void OnParentEnabledChanged(object sender, EventArgs e)
		{
			if (ParentEnabledChanged != null)
				ParentEnabledChanged(sender,e);
		}

		#endregion

		#region Contructors
		public FWBS.OMS.UI.Windows.ucHome ucHome
		{
			get
			{
				if (this.ucHome1 == null) 
					this.ucHome1 = new FWBS.OMS.UI.Windows.ucHome();

				return ucHome1;
			}
		}


		public frmMenu(string MenuCode, frmMain mainparent)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_mainparent = mainparent;
			ucHome1.InitalizeSetup(MenuCode, mainparent);
			ucHome1.MenuChanged +=new EventHandler(OnMenuChanged);
			ucHome1.MenuActioned +=new MenuEventHandler(OnMenuActioned);
			ucHome1.ParentEnabledChanged +=new EventHandler(OnParentEnabledChanged);
			ucHome1.FolderOpening +=new OpenFolderEventHandler(OnFolderOpening);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (ucHome1 != null)
				{
					ucHome1.Dispose();
				}
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
            this.ucHome1 = new FWBS.OMS.UI.Windows.ucHome();
            this.labLic = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ucHome1
            // 
            this.ucHome1.AddEditItem = "";
            this.ucHome1.AddEditMenuFolder = "";
            this.ucHome1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucHome1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucHome1.Location = new System.Drawing.Point(0, 0);
            this.ucHome1.MenuCode = "";
            this.ucHome1.Name = "ucHome1";
            this.ucHome1.ShowMenuDeveloper = false;
            this.ucHome1.Size = new System.Drawing.Size(784, 554);
            this.ucHome1.TabIndex = 0;
            this.ucHome1.GoClick += new FWBS.OMS.UI.Windows.HandledEventHandler(this.ucHome1_GoClick);
            this.ucHome1.ParentEnabledChanged += new System.EventHandler(this.ucHome1_ParentEnabledChanged);
            // 
            // labLic
            // 
            this.labLic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labLic.BackColor = System.Drawing.SystemColors.Window;
            this.labLic.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labLic.ForeColor = System.Drawing.Color.Red;
            this.labLic.Location = new System.Drawing.Point(348, 509);
            this.labLic.Name = "labLic";
            this.labLic.Size = new System.Drawing.Size(429, 36);
            this.labLic.TabIndex = 1;
            this.labLic.Text = "This Terminal is not registered. Please add {0} as a Licensed Terminal in License Manager";
            this.labLic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(784, 554);
            this.Controls.Add(this.labLic);
            this.Controls.Add(this.ucHome1);
            this.Name = "frmMenu";
            this.Text = "Main Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.frmMenu_Activated);
            this.Deactivate += new System.EventHandler(this.frmMenu_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMenu_FormClosed);
            this.ResumeLayout(false);

		}
		#endregion
		#endregion

		#region Private
		/// <summary>
		/// Wires up the Parents Main Tool Bar and the Mouses Back Button to the Parent Control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmMenu_Activated(object sender, System.EventArgs e)
		{
			if (_mainparent != null)
				_mainparent.OMSToolbars.OMSButtonClick +=new OMSToolBarButtonClickEventHandler(OMSToolbars_OMSButtonClick);
			FWBS.OMS.UI.Windows.Services.BackForwardMouse.BackButtonClicked +=new EventHandler(BackForwardMouse_BackButtonClicked);
			labLic.Text = labLic.Text.Replace("{0}",Session.CurrentSession.CurrentTerminal.TerminalName);
			labLic.Visible = !Session.CurrentSession.CurrentTerminal.IsRegistered;
		}

		/// <summary>
		/// Fires the Menu Usercontrols Parent Method
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OMSToolbars_OMSButtonClick(object sender, OMSToolBarButtonClickEventArgs e)
		{
			if (e.Button.Name == "tbParent" && ucHome1 != null) 
				ucHome1.ParentFolder();
		}

		/// <summary>
		/// Deactivates the the Toolbar Click and Mouse Back Button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmMenu_Deactivate(object sender, System.EventArgs e)
		{
			if (_mainparent != null)
			_mainparent.OMSToolbars.OMSButtonClick -=new OMSToolBarButtonClickEventHandler(OMSToolbars_OMSButtonClick);
			FWBS.OMS.UI.Windows.Services.BackForwardMouse.BackButtonClicked -=new EventHandler(BackForwardMouse_BackButtonClicked);
		}

		/// <summary>
		/// If the Parent button is clicked this is fired inform the UI of the Enabled State of the Parent
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ucHome1_ParentEnabledChanged(object sender, System.EventArgs e)
		{
			_mainparent.OMSToolbars.GetButton("tbParent").Enabled = ucHome1.IsParentEnabled;
		}

		/// <summary>
		/// Removes or Links to the Parent if the Form is Closed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void frmMenu_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (_mainparent != null)
				_mainparent.OMSToolbars.OMSButtonClick -= new OMSToolBarButtonClickEventHandler(OMSToolbars_OMSButtonClick);
			_mainparent = null;
		}

		/// <summary>
		/// Fires the Parent Folder Action 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackForwardMouse_BackButtonClicked(object sender, EventArgs e)
		{
			if (ucHome1 != null)
				ucHome1.ParentFolder();
		}

		private void ucHome1_GoClick(object sender, HandledEventArgs e)
		{
			OnGoClick(e);
		}
		#endregion
	}
}
