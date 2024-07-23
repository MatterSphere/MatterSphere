using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A more advanced text editor enquiry control which generally holds a caption and a text box.
    /// This particular control can be used for multi line text editing.
    /// </summary>
    public class eMultiTextBox2 : eTextBox2
	{
		
		#region Constructors

		/// <summary>
		/// Creates a new intance of this control.
		/// </summary>
		public eMultiTextBox2() : base()
		{
			//Get a reference to the already created base textbox.
			TextBox ctrl = (TextBox)_ctrl;
            //Set multiline to true and allow scrollbars.
            ctrl.Multiline = true;
            ctrl.ScrollBars = ScrollBars.Vertical;
            ctrl.AcceptsReturn = true;
            ctrl.AcceptsTab = false;
		}

        #endregion

        #region IBasicEnquiryControl2 Implementation

        /// <summary>
        /// Gets the lock height flag which states whether the control is locked to a certain height.
        /// This is a design mode property and is set to false.
        /// </summary>
        [Browsable(false)]
		[DefaultValue(false)]
		public override bool LockHeight 
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region Properties
		[Category("Appearance")]
		public ScrollBars ScrollBars
		{
			get
			{
				TextBox ctrl = (TextBox)_ctrl;
				return ctrl.ScrollBars;
			}
			set
			{
				TextBox ctrl = (TextBox)_ctrl;
				ctrl.ScrollBars = value;
				if (value == ScrollBars.Both || value == ScrollBars.Horizontal)
					ctrl.WordWrap= false;
			}
		}
        #endregion
    }
}
