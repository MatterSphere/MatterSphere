using System;

namespace FWBS.OMS.UI.Windows
{
	/// <summary>
	/// Select The Type of Search to Be Used
	/// </summary>
	public enum SelectClientFileSearchType
	{
		Client,
		File
	}
	
	/// <summary>
	/// Search Manager Settings
	/// </summary>
	public enum SearchManager
	{
		ContactManager,
		ClientManager,
		FileManager,
		None
	}

	/// <summary>
	/// Reason for Closing Child Item
	/// </summary>
	public enum ClosingWhy
	{
        [Obsolete("Please use Cancel or Saved instead of Closing.")]
		Closing,
		Cancel,
		Saved
	}
	
	/// <summary>
	/// An enumeration of ISelectorRepeater methods.
	/// </summary>
	public enum SelectorRepeaterMethods
	{
		New,
		Assign,
		Revoke,
		Find
	}

	/// <summary>
	/// Direction for a wizard page change.
	/// </summary>
	public enum EnquiryPageDirection
	{
		/// <summary>
		/// Specifies that the Next button has been clicked.
		/// </summary>
		Next,
		/// <summary>
		/// Specifies that the Back button has been clicked.
		/// </summary>
		Back,
		/// <summary>
		/// The wizard step has not moved anywhere.
		/// </summary>
		None
	}

	/// <summary>
	/// On Control Missing in the Form Render Base
	/// </summary>
	public enum EnquiryControlMissing
	{
		Exception,
		Create,
		None
	}

	/// <summary>
	/// Wizard page type.
	/// </summary>
	public enum EnquiryPageType
	{
		/// <summary>
		/// The current wizard page type is the welcome / start page.
		/// </summary>
		Start,
		/// <summary>
		/// The current wizard page type should be handled by the programmer.
		/// </summary>
		Custom,
		/// <summary>
		/// The current wizard page type is a enquiry rendered wizard page.
		/// </summary>
		Enquiry
	}

	/// <summary>
	/// Enquiry form styles; wizard, standard etc...
	/// </summary>
	public enum EnquiryStyle
	{
		/// <summary>
		/// Specifies that the enquiry form renders on a form displaying all designed visual controls.
		/// </summary>
		Standard,
		/// <summary>
		/// Specifies that the enquiry form renders in a page by page manner.
		/// </summary>
		Wizard
	}
	
    /// <summary>
    /// Wizard styles: dialog, taks pane, in-place.
    /// </summary>
    public enum WizardStyle
    {
        /// <summary>
        /// Specifies that the wizard runs as a top-level dialog.
        /// </summary>
        Dialog,
        /// <summary>
        /// Specifies that the modeless wizard runs in a task pane.
        /// </summary>
        TaskPane,
        /// <summary>
        /// Specifies that the modeless wizard runs in-place as child control on another form.
        /// </summary>
        InPlace
    }
}
