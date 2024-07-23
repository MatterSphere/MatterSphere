using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// An abstract class for a selector control..
    /// </summary>
    public class ucSelectorClass : System.Windows.Forms.UserControl, Interfaces.ISelectorRepeater
	{
		#region Events
		public event EventHandler Closed;
		#endregion

		#region Fields

		protected System.Windows.Forms.Button btnCloseAlert;
		protected FWBS.Common.UI.Windows.eXPPanel pnlTitle;
		protected System.Windows.Forms.Panel border;
		protected System.Windows.Forms.Label labTitle;
		private System.Windows.Forms.Panel pnlSp;

		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructors & Destructors

		public ucSelectorClass()
		{
			InitializeComponent();
			border.ControlAdded +=new ControlEventHandler(ucSelectorClass_ControlAdded);
			border.ControlRemoved +=new ControlEventHandler(ucSelectorClass_ControlRemoved);
		}

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pnlTitle = new FWBS.Common.UI.Windows.eXPPanel();
            this.labTitle = new System.Windows.Forms.Label();
            this.btnCloseAlert = new System.Windows.Forms.Button();
            this.border = new System.Windows.Forms.Panel();
            this.pnlSp = new System.Windows.Forms.Panel();
            this.pnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTitle
            // 
            this.pnlTitle.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.InactiveCaption);
            this.pnlTitle.BorderLine = true;
            this.pnlTitle.Controls.Add(this.labTitle);
            this.pnlTitle.Controls.Add(this.btnCloseAlert);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Padding = new System.Windows.Forms.Padding(2);
            this.pnlTitle.Size = new System.Drawing.Size(424, 20);
            this.pnlTitle.TabIndex = 18;
            // 
            // labTitle
            // 
            this.labTitle.AutoEllipsis = true;
            this.labTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labTitle.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.labTitle.Location = new System.Drawing.Point(2, 2);
            this.labTitle.Name = "labTitle";
            this.labTitle.Size = new System.Drawing.Size(404, 16);
            this.labTitle.TabIndex = 0;
            this.labTitle.Text = "Untitled";
            this.labTitle.Click += new System.EventHandler(this.ClickHandler);
            // 
            // btnCloseAlert
            // 
            this.btnCloseAlert.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCloseAlert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseAlert.Font = new System.Drawing.Font("Webdings", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnCloseAlert.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnCloseAlert.Location = new System.Drawing.Point(406, 2);
            this.btnCloseAlert.Name = "btnCloseAlert";
            this.btnCloseAlert.Size = new System.Drawing.Size(16, 16);
            this.btnCloseAlert.TabIndex = 4;
            this.btnCloseAlert.TabStop = false;
            this.btnCloseAlert.Text = "r";
            this.btnCloseAlert.UseCompatibleTextRendering = true;
            this.btnCloseAlert.TextChanged += new System.EventHandler(this.btnCloseAlert_TextChanged);
            this.btnCloseAlert.Click += new System.EventHandler(this.btnCloseAlert_Click);
            // 
            // border
            // 
            this.border.Dock = System.Windows.Forms.DockStyle.Fill;
            this.border.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.border.Location = new System.Drawing.Point(0, 20);
            this.border.Name = "border";
            this.border.Padding = new System.Windows.Forms.Padding(3);
            this.border.Size = new System.Drawing.Size(424, 84);
            this.border.TabIndex = 19;
            this.border.Paint += new System.Windows.Forms.PaintEventHandler(this.border_Paint);
            this.border.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.border.Resize += new System.EventHandler(this.border_Resize);
            // 
            // pnlSp
            // 
            this.pnlSp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSp.Location = new System.Drawing.Point(0, 104);
            this.pnlSp.Name = "pnlSp";
            this.pnlSp.Size = new System.Drawing.Size(424, 8);
            this.pnlSp.TabIndex = 20;
            // 
            // ucSelectorClass
            // 
            this.Controls.Add(this.border);
            this.Controls.Add(this.pnlTitle);
            this.Controls.Add(this.pnlSp);
            this.Name = "ucSelectorClass";
            this.Size = new System.Drawing.Size(424, 112);
            this.pnlTitle.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region ISelectorRepeater Implementation

	
		/// <summary>
		/// An event that gets raised when the item has been selected.
		/// </summary>
		public event EventHandler Selected = null;

		/// <summary>
		/// An event that gets raised just before the item has been selected.
		/// </summary>
		public event CancelEventHandler Selecting = null;

		/// <summary>
		/// An event that gets raised when the item has been unselected.
		/// </summary>
		public event EventHandler UnSelected = null;

		/// <summary>
		/// An event that gets raised just before the item has been unselected.
		/// </summary>
		public event CancelEventHandler UnSelecting = null;

		/// <summary>
		/// Sets dynaminc parameters to the derived control.
		/// </summary>
		/// <param name="parameters">Parameters to be passed.</param>
		public virtual void SetInfo (object [] parameters)
		{
		}

		/// <summary>
		/// Raises the selected event.
		/// </summary>
		/// <param name="e"></param>
		protected void OnSelected ()
		{
			if (Selected != null)
				Selected(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the selecting event.
		/// </summary>
		/// <param name="e"></param>
		protected void OnSelecting (CancelEventArgs e)
		{
			if (Selecting != null)
				Selecting(this, e);
		}

		/// <summary>
		/// Raises the un selected event.
		/// </summary>
		/// <param name="e"></param>
		protected void OnUnSelected ()
		{
			if (UnSelected != null)
				UnSelected(this, EventArgs.Empty);
		}

		
		/// <summary>
		/// Raises the un selected event.
		/// </summary>
		/// <param name="e"></param>
		protected void OnUnSelecting (CancelEventArgs e)
		{
			if (UnSelecting != null)
				UnSelecting(this, e);
		}

		/// <summary>
		/// Checks to see if this type of selector control supports certain methods.
		/// </summary>
		/// <param name="methodType">Method type to check for.</param>
		/// <returns>A true / false value.</returns>
		public virtual bool HasMethod(SelectorRepeaterMethods methodType)
		{
			return false;
		}

		/// <summary>
		/// Runs the specific type of method.
		/// </summary>
		/// <param name="methodType">>Method type to check for.</param>
		public virtual void RunMethod(SelectorRepeaterMethods methodType){}

		/// <summary>
		/// Gets or sets the current contact object for this control.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public virtual object Object
		{
			get
			{
				return null;
			}
			set
			{;}
		}


		/// <summary>
		/// Overriden to be the Text of the group box container.
		/// </summary>
		[Browsable(true)]
		public override string Text
		{
			get
			{
				return labTitle.Text;
			}
			set
			{
				labTitle.Text = value;
			}
		}

        /// <summary>
        /// Marks the control as selected
        /// </summary>
        new public void Select()
		{
			base.Select();
			if (!IsSelected)
			{
				CancelEventArgs cancel = new CancelEventArgs(false);
				OnSelecting(cancel);
				if (!cancel.Cancel)
				{
					pnlTitle.Backcolor.SetColor = SystemColors.ActiveCaption;
					pnlTitle.ForeColor = SystemColors.ActiveCaptionText;
					OnSelected();
				}
			}
		}


		/// <summary>
		/// Marks the control as unselected.
		/// </summary>
		public void UnSelect()
		{
			if (IsSelected)
			{
				CancelEventArgs cancel = new CancelEventArgs(false);
				OnUnSelecting(cancel);
				if (!cancel.Cancel)
				{
					pnlTitle.Backcolor.SetColor = SystemColors.InactiveCaption;
					pnlTitle.ForeColor = SystemColors.InactiveCaptionText;
					OnUnSelected();
				}
			}
		}

		/// <summary>
		/// Gets a value that indicates that the control was selected.
		/// </summary>
		public bool IsSelected
		{
			get
			{
				return (pnlTitle.BackColor == SystemColors.ActiveCaption);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Catches all click events and raises them through the stack.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ClickHandler(object sender, EventArgs e)
		{
			if (IsSelected)
				UnSelect();
			else
				Select();
		}

		#endregion

		private void btnCloseAlert_Click(object sender, System.EventArgs e)
		{
			if (Closed != null) Closed(this,e);
		}

        private void btnCloseAlert_TextChanged(object sender, EventArgs e)
        {
            btnCloseAlert.Text = "r";
        }

		private void border_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
            Control ctrl = ((Control)sender);
            using (Pen p1 = new Pen(SystemBrushes.ControlDark, 1))
            {
                e.Graphics.DrawLine(p1, 0, 0, 0, ctrl.Height - 1);
                e.Graphics.DrawLine(p1, ctrl.Width - 1, 0, ctrl.Width - 1, ctrl.Height - 1);
                e.Graphics.DrawLine(p1, 0, ctrl.Height - 1, ctrl.Width, ctrl.Height - 1);
            }
		}

		private void border_Resize(object sender, System.EventArgs e)
		{
			border.Invalidate();
		}

		private void ucSelectorClass_ControlAdded(object sender, ControlEventArgs e)
		{
			e.Control.Enter +=new EventHandler(ClickHandler);
		}

		private void ucSelectorClass_ControlRemoved(object sender, ControlEventArgs e)
		{
			e.Control.Enter -=new EventHandler(ClickHandler);
		}

		private void Control_MouseDown(object sender, MouseEventArgs e)
		{
			ClickHandler(sender,EventArgs.Empty);			
		}
	}
}
