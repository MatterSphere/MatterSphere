using System;
using System.Data;
using System.Windows.Forms;

namespace FWBS.Common.UI
{

	/// <summary>
	/// An interface that specifies that the control implementing the interface can format
	/// text within its value contents.
	/// </summary>
	public interface IFormatEnquiryControl
	{
		string Format{get;set;}
		
	}

	/// <summary>
	/// Flags a control / object to be of a group / container type
	/// </summary>
	public interface IContainerEnquiryControl
	{
	}

	/// <summary>
	/// Basic enquiry control interface.
	/// </summary>
	public interface IBasicEnquiryControl2
	{
		event EventHandler ActiveChanged;
		event EventHandler Changed;
		event EventHandler Leave;

		object Control {get;} 
		int CaptionWidth {get;set;} 
        bool CaptionTop {get;set;}
		bool LockHeight {get;}
		bool Required {get;set;}
		bool ReadOnly {get;set;}
		bool omsDesignMode {get;set;}
		object Value {get; set;}
		string Text {get; set;}
		bool IsDirty {get; set;}

		void OnChanged();
		void OnActiveChanged();

	}

	

	/// <summary>
	/// Button type control that raises a command event when clicked.
	/// </summary>
	public interface ICommandEnquiryControl
	{
		event EventHandler ExecuteCommand;
		void SetCommand (bool on);
		System.Windows.Forms.Button CommandButton {get;}
		void OnExecuteCommand();
	}

	/// <summary>
	/// List style enquiry control.
	/// </summary>
	public interface IListEnquiryControl :IBasicEnquiryControl2
	{
		void AddItem (object Value, string displayText);
		void AddItem (DataTable dataTable);
		void AddItem (DataTable dataTable, string valueMember, string displayMember);
		void AddItem (DataView dataView);
		void AddItem (DataView dataView, string valueMember, string displayMember);
		object DisplayValue {get;set;}
		bool Filter (string fieldName, object Value);
		bool Filter (string FilterString);
		int Count {get;}
	}

	/// <summary>
	/// Text editor controls.
	/// </summary>
	public interface ITextEditorEnquiryControl
	{
		int MaxLength {get;set;}
	}

	/// <summary>
	/// Controls the character casing of a particular control that implements this interface.
	/// </summary>
	public interface ICharacterCasingControl
	{
		System.Windows.Forms.CharacterCasing Casing{get;set;}
	}

	/// <summary>
	/// Updateable interface.
	/// </summary>
	public interface IUpdateableEnquiryControl
	{
		void Update();
	}

	/// <summary>
	/// Positions the required stars next to the right controls.
	/// </summary>
	public interface IUsesRequiredStars
	{
		void RequiredIconsOn(bool on);
		void ErrorIconsOn(bool on);
	}


	/// <summary>
	/// Value key pair object item.
	/// </summary>
	public  class EnquiryListItem
	{
		public object Value;
		public string Description;

		public EnquiryListItem(object val, string description)
		{
			Value = val;
			Description = description;
		}

		public override string ToString()
		{
			return this.Description;
		}
	}


    public interface IOverrideTooltip
    {
        System.Windows.Forms.Control ToolTipControl { get; }
    }

	public interface IWebBrowserControl
	{
		event EventHandler Initialized;
		event WebBrowserNavigatingEventHandler Navigating;
		event WebBrowserNavigateErrorEventHandler NavigateError;
		event WebBrowserDocumentCompletedEventHandler DocumentCompleted;

		bool IsInitialized { get; }
		bool SetCookie(string baseUrl, string cookieName, string data);
		void Navigate(string url);
		void Stop();
	}
}

