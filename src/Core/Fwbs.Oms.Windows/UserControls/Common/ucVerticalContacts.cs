using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmVerticalContacts.
    /// </summary>
    public class ucVerticalContacts : System.Windows.Forms.UserControl
	{
        #region CloseButton
        private class CloseButton : Button
        {
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                bool pressed = (Control.MouseButtons == MouseButtons.Left) && RectangleToScreen(ClientRectangle).Contains(Control.MousePosition);
                ControlPaint.DrawCaptionButton(e.Graphics, ClientRectangle, CaptionButton.Close, pressed ? ButtonState.Pushed : ButtonState.Flat);
            }
        }
        #endregion CloseButton

		#region Auto Fields
		private System.Windows.Forms.Panel pnlMain;
		private CloseButton btnClose;
		#endregion

		#region Fields
		private VerticalContactsCollection _contactdisplay;
        private ToolStrip tbTools;
        private ToolStripButton btAccept;
		private ucVerticalContact _lastselected = null;
		#endregion

		#region Events
		public event EventHandler Close;
		public event EventHandler Accept;
		#endregion

		#region Constructors & Desctructors
		public ucVerticalContacts()
		{
			InitializeComponent();
            this.btAccept.Text = Session.CurrentSession.Resources.GetResource("ACCEPT", "Accept", "").Text;
            _contactdisplay = new VerticalContactsCollection();
            _contactdisplay.Inserted +=new Crownwood.Magic.Collections.CollectionChange(_contactdisplay_Inserted);
			_contactdisplay.Removing +=new Crownwood.Magic.Collections.CollectionChange(_contactdisplay_Removing);
			_contactdisplay.Clearing +=new Crownwood.Magic.Collections.CollectionClear(_contactdisplay_Clearing);
		}



		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnClose = new FWBS.OMS.UI.Windows.ucVerticalContacts.CloseButton();
            this.tbTools = new System.Windows.Forms.ToolStrip();
            this.btAccept = new System.Windows.Forms.ToolStripButton();
            this.tbTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.BackColor = System.Drawing.SystemColors.Window;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 27);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(200, 373);
            this.pnlMain.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(175, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(19, 19);
            this.btnClose.TabIndex = 6;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tbTools
            // 
            this.tbTools.CanOverflow = false;
            this.tbTools.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tbTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btAccept});
            this.tbTools.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.tbTools.Location = new System.Drawing.Point(0, 0);
            this.tbTools.Name = "tbTools";
            this.tbTools.Size = new System.Drawing.Size(200, 27);
            this.tbTools.TabIndex = 0;
            // 
            // btAccept
            // 
            this.btAccept.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btAccept.Enabled = false;
            this.btAccept.Margin = new System.Windows.Forms.Padding(4);
            this.btAccept.Name = "btAccept";
            this.btAccept.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.btAccept.Size = new System.Drawing.Size(48, 19);
            this.btAccept.Text = "Accept";
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // ucVerticalContacts
            // 
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.tbTools);
            this.Name = "ucVerticalContacts";
            this.Size = new System.Drawing.Size(200, 400);
            this.tbTools.ResumeLayout(false);
            this.tbTools.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		#endregion

		#region Properties
		public ucVerticalContact Selected
		{
			get
			{
				return _lastselected;
			}
		}

		[Category("OMS")]
		public VerticalContactsCollection Contacts
		{
			get
			{
				return _contactdisplay;
			}
			set
			{
				_contactdisplay = value;
			}
		}

		public bool MainPanelVisible
		{
			get
			{
				return pnlMain.Visible;
			}
			set
			{
				pnlMain.Visible = value;
			}
		}
		#endregion

		#region Private
		private void _contactdisplay_Inserted(int index, object value)
		{
			ucVerticalContact ctrl = value as ucVerticalContact;
			if (ctrl != null)
			{
				ctrl.Dock = DockStyle.Top;
				ctrl.Click +=new EventHandler(ctrl_SelectChanged);
				ctrl.DoubleClick +=new EventHandler(ctrl_DoubleClick);
				pnlMain.Controls.Add(ctrl);
				ctrl.BringToFront();
			}

		}

		private void _contactdisplay_Removed(int index, object value)
		{

		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			if (Close != null)
				Close(this,EventArgs.Empty);
		}

		private void ctrl_SelectChanged(object sender, EventArgs e)
		{
			if (_lastselected == sender) return;
			if (_lastselected != null)
				_lastselected.Selected=false;
			_lastselected = sender as ucVerticalContact;
			btAccept.Enabled=true;
			this.OnClick(e);
		}

		private void ctrl_DoubleClick(object sender, EventArgs e)
		{
			if (Accept != null)
				Accept(this,EventArgs.Empty);			
		}

        private void btAccept_Click(object sender, EventArgs e)
        {
            if (Accept != null)
                Accept(this, EventArgs.Empty);
        }

		private void _contactdisplay_Clearing()
		{
			foreach(ucVerticalContact ctr in pnlMain.Controls)
			{
				ctr.Dock = DockStyle.None;
				ctr.Click -=new EventHandler(ctrl_SelectChanged);
				ctr.DoubleClick -=new EventHandler(ctrl_DoubleClick);
			}
			Global.RemoveAndDisposeControls(pnlMain);
			btAccept.Enabled=false;
		}

		private void _contactdisplay_Removing(int index, object value)
		{
			ucVerticalContact ctrl = value as ucVerticalContact;
			ctrl.Click -=new EventHandler(ctrl_SelectChanged);
			ctrl.DoubleClick -=new EventHandler(ctrl_DoubleClick);
			pnlMain.Controls.Remove(ctrl);

		}
		#endregion

        public ToolStripButton AddToolbarButton(string name, string text, EventHandler onClick)
        {
            ToolStripButton tbButton = new ToolStripButton();
            tbButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tbButton.Margin = new Padding(4);
            tbButton.Name = name;
            tbButton.Overflow = ToolStripItemOverflow.Never;
            tbButton.Text = text;
            tbButton.Click += onClick;
            this.tbTools.Items.Add(tbButton);
            return tbButton;
        }

	}

	#region Collection
	public class VerticalContactsCollection : Crownwood.Magic.Collections.CollectionWithEvents
	{
		public ucVerticalContact Add(ucVerticalContact value)
		{
			// Use base class to process actual collection operation
			base.List.Add(value as object);

			return value;
		}

		public void AddRange(ucVerticalContact[] values)
		{
			// Use existing method to add each array entry
			foreach(ucVerticalContact page in values)
				Add(page);
		}

		public void Remove(ucVerticalContact value)
		{
			// Use base class to process actual collection operation
			base.List.Remove(value as object);
		}

		public void Insert(int index, ucVerticalContact value)
		{
			// Use base class to process actual collection operation
			base.List.Insert(index, value as object);
		}

		public bool Contains(ucVerticalContact value)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(value as object);
		}

		public ucVerticalContact this[int index]
		{
			// Use base class to process actual collection operation
			get { return (base.List[index] as ucVerticalContact); }
		}

		public int IndexOf(ucVerticalContact value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}
	}
	#endregion
}
