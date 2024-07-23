using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A command button enquiry control which generally holds a command button only.  
    /// This particular control can be used for running commands only.
    /// </summary>
    public class eButton : Button, ICommandEnquiryControl
	{
		#region Events
		/// <summary>
		/// If the control has been marked with command arguments then this event will be raised
		/// when the button has been clicked, passing the command parameters to the form that is rendering the control.
		/// </summary>
		[Category("Action")]
		public virtual event EventHandler ExecuteCommand;

		#endregion

		#region Constructors

		public eButton()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			// 
			// eButton
			// 
			this.FlatStyle = System.Windows.Forms.FlatStyle.System;

		}

		#endregion

		#region ICommandEnquiryControl Implementation

		/// <summary>
		/// Gets the command button of the control so that the rendering form can use it to assign
		/// tooltips to it or manipulate it in other ways.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		[Category("Command")]
		public Button CommandButton
		{
			get
			{
				return this;
			}
		}

		/// <summary>
		/// Assigns a command to the control.
		/// </summary>
		/// <param name="on">Swithches the command button on or off.</param>
		public void SetCommand (bool on)
		{
			if (on)
			{
				this.Click -= new EventHandler(this.CommandClick);
				this.Click += new EventHandler(this.CommandClick);
			}
			else
				this.Click -= new EventHandler(this.CommandClick);
		}

		
		/// <summary>
		/// Executes the execute command event.
		/// </summary>
		public void OnExecuteCommand()
		{
			if (ExecuteCommand != null)
				ExecuteCommand(this, EventArgs.Empty);
		}


		#endregion

		#region Methods

		/// <summary>
		/// Capture the click of the command button then raise it in the form of the ExecuteCommand event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CommandClick(object sender, System.EventArgs e)
		{
			OnExecuteCommand();
		}

		#endregion

	}
}
