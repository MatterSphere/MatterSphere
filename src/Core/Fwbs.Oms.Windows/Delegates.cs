using System;

namespace FWBS.OMS.UI.Windows
{

	/// <summary>
	/// Wizard after changed page event arguments. 
	/// </summary>
	public class PageChangedEventArgs : EventArgs
	{
		private string _pageName = "";
		/// <summary>
		/// The page number that the wizard is currently on.
		/// The value -1 being the welcome page.
		/// </summary>
		private short _pageNumber = -1;
		/// <summary>
		/// An enumeration member describing the page type, this could be custom, the
		/// start welcom page etc..
		/// </summary>
		private EnquiryPageType _pageType = EnquiryPageType.Start;
		/// <summary>
		/// The direction the user has chosen, this depends on whether the user has
		/// clicked on the back or next button.
		/// </summary>
		private EnquiryPageDirection _direction = EnquiryPageDirection.None;

		/// <summary>
		/// Default constructor not used.
		/// </summary>
		private PageChangedEventArgs (){}

		/// <summary>
		/// Creates an instance of the event arguments.
		/// </summary>
		/// <param name="page">Page number that the wizard is moving to.</param>
		/// <param name="custom">Indicates whether the page is a custom handled page or not.</param>
		/// <param name="direction">The direction of flow.</param>
		internal PageChangedEventArgs (string pageName, short page, bool custom, EnquiryPageDirection direction)
		{
			_pageName = pageName;
			_pageNumber = page;
			if (page ==-1)
				_pageType = EnquiryPageType.Start;
			else if (custom)
				_pageType = EnquiryPageType.Custom;
			else
				_pageType = EnquiryPageType.Enquiry;
			_direction = direction;
		}

		/// <summary>
		/// Gets the page name that the wizard is currently on.
		/// </summary>
		public string PageName
		{
			get
			{
				return _pageName;
			}
		}
		
		/// <summary>
		/// Gets the page number that the wizard is currently on.
		/// The value -1 being the welcome page.
		/// </summary>
		public short PageNumber
		{
			get
			{
				return _pageNumber;
			}
		}

		/// <summary>
		/// Gets an enumeration member describing the page type, this could be custom or the
		/// start welcome page etc..
		/// </summary>
		public EnquiryPageType PageType
		{
			get
			{
				return _pageType;
			}
		}

		/// <summary>
		/// Gets the direction the user has chosen, this depends on whether the user has
		/// clicked on the back or next button.
		/// </summary>
		public EnquiryPageDirection Direction 
		{
			get
			{
				return _direction;
			}
		}
	}

	/// <summary>
	/// Wizard before changed page event arguments. 
	/// </summary>
	public class PageChangingEventArgs : PageChangedEventArgs
	{
		/// <summary>
		/// Cancels the page changed event for some reason or another.
		/// </summary>
		private bool _cancel = false;

		/// <summary>
		/// Creates an instance of the event arguments.
		/// </summary>
		/// <param name="page">Page number that the wizard is moving to.</param>
		/// <param name="custom">Indicates whether the page is a custom handled page or not.</param>
		/// <param name="direction">The direction of flow.</param>
		/// <param name="cancel">Default value for the canceled page move option.</param>
		internal PageChangingEventArgs (string pageName, short page, bool custom, EnquiryPageDirection direction, bool cancel) : base (pageName, page, custom, direction)
		{
			_cancel = cancel;
		}

		/// <summary>
		/// Gets or Sets the cancel flag to cancel the page change.
		/// </summary>
		public bool Cancel
		{
			get
			{
				return _cancel;
			}
			set
			{
				_cancel = value;
			}
		}

	}

	
	/// <summary>
	/// Delegate method signature for the wizards before page change event.
	/// </summary>
	public delegate void PageChangingEventHandler (object sender, PageChangingEventArgs e);
	/// <summary>
	/// Delegate method signature for the wizards after page change event.
	/// </summary>
	public delegate void PageChangedEventHandler (object sender, PageChangedEventArgs e);



	/// <summary>
	/// Delegate for Closing a new OMS type window in the OMS dialogs form
	/// </summary>
	public delegate void NewOMSTypeCloseEventHander (object sender, NewOMSTypeCloseEventArgs e);

	/// <summary>
	/// Event arguments for Closing a new OMS Type Window
	/// </summary>
	public class NewOMSTypeCloseEventArgs : EventArgs
	{
		/// <summary>
		/// Reason for Closing the New OMS Type Window
		/// </summary>
		private ClosingWhy _why;

		/// <summary>
		/// Create an instance of the NewOMSTypeCloseEventArgs
		/// </summary>
		/// <param name="Why">The Reason Why</param>
		public NewOMSTypeCloseEventArgs(ClosingWhy Why)
		{
			_why = Why;
		}

		/// <summary>
		/// Returns the Reason why the New OMS Type window was closed
		/// </summary>
		public ClosingWhy Why
		{
			get
			{
				return _why;
			}
		}
	}
		
		
	/// <summary>
	/// Delegate for creating a new OMS type windows in the OMS dialogs form.
	/// </summary>
	public delegate void NewOMSTypeWindowEventHandler (object sender, NewOMSTypeWindowEventArgs e);

	/// <summary>
	/// Event arguments for creating a new OMS type windows in the OMS dialogs form.
	/// </summary>
	public class NewOMSTypeWindowEventArgs : EventArgs
	{
		/// <summary>
		/// A reference to the OMS library object that is OMSType configurable.
		/// </summary>
		private FWBS.OMS.Interfaces.IOMSType _obj = null;

		/// <summary>
		/// A reference to the OMS type object to be used.
		/// </summary>
		private FWBS.OMS.OMSType _omst = null;

		/// <summary>
		/// The default page to show in the dialog.
		/// </summary>
		private string _defaultPage = null;

		/// <summary>
		/// Default constructor not used.
		/// </summary>
		private NewOMSTypeWindowEventArgs (){}

		/// <summary>
		/// Creates an instance of the event arguments.
		/// </summary>
		/// <param name="obj">A reference to the OMS library object that is OMSType configurable.</param>
		public NewOMSTypeWindowEventArgs  (FWBS.OMS.Interfaces.IOMSType obj)
		{
			_obj = obj;
		}

		
		/// <summary>
		/// Creates an instance of the event arguments.
		/// </summary>
		/// <param name="obj">A reference to the OMS library object that is OMSType configurable.</param>
		/// <param name="defaultPage">The default page to display.</param>
		public NewOMSTypeWindowEventArgs  (FWBS.OMS.Interfaces.IOMSType obj, string defaultPage)
		{
			_obj = obj;
			_defaultPage = defaultPage;
		}

		public NewOMSTypeWindowEventArgs  (FWBS.OMS.Interfaces.IOMSType obj, FWBS.OMS.OMSType omst, string defaultPage)
		{
			_obj = obj;
			_omst = omst;
			_defaultPage = defaultPage;
		}

		/// <summary>
		/// A reference to the OMS library object that is OMSType configurable.
		/// </summary>
		public FWBS.OMS.Interfaces.IOMSType OMSObject
		{
			get
			{
				return _obj;
			}
		}

		/// <summary>
		/// Gets the OMSType object that is to be used.
		/// </summary>
		public FWBS.OMS.OMSType OMSType
		{
			get
			{
				if (_omst == null)
					_omst = _obj.GetOMSType();
				return _omst;
			}
		}

		/// <summary>
		/// Gets the default tab page name to display.
		/// </summary>
		public string DefaultPage
		{
			get
			{
				return _defaultPage;
			}
		}
	}
}
