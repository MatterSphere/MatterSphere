using System;
using System.ComponentModel;

namespace FWBS.OMS.UI.Windows
{

    /// <summary>
    /// Allows the selection a contact. Contacts may be added or created during the process.
    /// </summary>
    public class ucContactSelector : ucSelectorClass 
	{
		#region Controls

		private System.Windows.Forms.TextBox txtContactInfo;

		#endregion
		
		#region Fields

		/// <summary>
		/// The current contact the control is holding.
		/// </summary>
		private ClientContactLink _contact;
		
		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ucContactSelector()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			txtContactInfo.Click += new EventHandler(ClickHandler);
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.txtContactInfo = new System.Windows.Forms.TextBox();
            this.pnlTitle.SuspendLayout();
            this.border.SuspendLayout();
            this.SuspendLayout();
            // 
            // border
            // 
            this.border.Controls.Add(this.txtContactInfo);
            this.border.Padding = new System.Windows.Forms.Padding(8);
            this.border.Text = "Contact (n)";
            // 
            // txtContactInfo
            // 
            this.txtContactInfo.BackColor = System.Drawing.SystemColors.Control;
            this.txtContactInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtContactInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txtContactInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContactInfo.Location = new System.Drawing.Point(8, 8);
            this.txtContactInfo.Multiline = true;
            this.txtContactInfo.Name = "txtContactInfo";
            this.txtContactInfo.ReadOnly = true;
            this.txtContactInfo.Size = new System.Drawing.Size(408, 69);
            this.txtContactInfo.TabIndex = 4;
            this.txtContactInfo.Text = ".....";
            // 
            // ucContactSelector
            // 
            this.Name = "ucContactSelector";
            this.RightToLeftChanged += new System.EventHandler(this.ucContactSelector_RightToLeftChanged);
            this.pnlTitle.ResumeLayout(false);
            this.border.ResumeLayout(false);
            this.border.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion


		#endregion

		#region Methods

		/// <summary>
		/// Captures any change in the right to left visual format.
		/// </summary>
		/// <param name="sender">The currentcontrol instance.</param>
		/// <param name="e">Empt event arguments.</param>
		private void ucContactSelector_RightToLeftChanged(object sender, System.EventArgs e)
		{
			Global.RightToLeftControlConverter(this, ParentForm);
		}


		#endregion

		#region Properties

	
		#endregion

		#region ISelectorRepeater Implementation

	
		/// <summary>
		/// Checks to see if this type of selector control supports certain methods.
		/// </summary>
		/// <param name="methodType">Method type to check for.</param>
		/// <returns>A true / false value.</returns>
		public override bool HasMethod(SelectorRepeaterMethods methodType)
		{
			switch (methodType)
			{
				case SelectorRepeaterMethods.Assign:
					break;
				case SelectorRepeaterMethods.Revoke:
					break;
				case SelectorRepeaterMethods.Find:
					break;
			}
			return true;
		}

		/// <summary>
		/// Runs the specific type of method.
		/// </summary>
		/// <param name="methodType">>Method type to check for.</param>
		public override void RunMethod(SelectorRepeaterMethods methodType)
		{
			switch (methodType)
			{
				case SelectorRepeaterMethods.Assign:
				{
					Contact cont = FWBS.OMS.UI.Windows.Services.Wizards.CreateContact();
					if (cont != null)
						Object = cont;
					else
						Object = null;
						
				}
					break;
				case SelectorRepeaterMethods.Revoke:
				{
					Object = null;
				}
					break;
				case SelectorRepeaterMethods.Find:
				{
					Contact c = FWBS.OMS.UI.Windows.Services.Searches.FindContact();
					if (c != null)
					{
						Object = c;
					}
				}
					break;
			}
			
		}

		/// <summary>
		/// Gets or sets the current contact object for this control.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public override object Object
		{
			get
			{
				if (_contact.Contact == null)
					return null;
				else
					return _contact;
			}
			set
			{
				Contact cont = null;
				if (value == null)
					_contact.Contact = null;
				else if (value is ClientContactLink)
					_contact = (ClientContactLink)value;
				else if (value is Contact)
					_contact = new ClientContactLink(null, (Contact)value);

				cont = _contact.Contact;
				if (cont == null)
					txtContactInfo.Text = ".....";
				else
					txtContactInfo.Text = cont.ContactDescription;
			}
		}

		#endregion
	}
}
