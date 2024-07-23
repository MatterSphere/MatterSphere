using System;
using System.Windows.Forms;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS.UI.Windows.Interfaces
{

    public interface ISelectClientFileDialog
    {
        System.Windows.Forms.DialogResult ShowDialog(System.Windows.Forms.IWin32Window owner);
        OMSFile SelectedFile { get;}
        void Dispose();
    }
	/// <summary>
	/// Describes an item that may appear in the oms type dialog tabbed form.
	/// This will expose a refresh and update method as well as a select method
	/// so that the item knows when its tab has been selected. 
	/// This interface also has a IsDirty property to indicate whether there is any unsaved
	/// data.
	/// </summary>
	public interface IOMSItem
	{
		void UpdateItem();
		void RefreshItem();
		void CancelItem();
		void SelectItem();
		bool IsDirty{get;}
		bool ToBeRefreshed{get;set;}
		event EventHandler Dirty;

	}

	/// <summary>
	/// An object that implements this interface can be linked to a OMS configurable type object.
	/// A user control may use this interface to display a list of information based on a client
	/// or file.
	/// </summary>
	public interface IOMSTypeAddin : IOMSItem, IOpenOMSType
	{
		string AddinText {get;}
		void Initialise(IOMSType obj);
		bool Connect(IOMSType obj);
		FWBS.OMS.UI.Windows.ucPanelNav[] Panels {get;}
		FWBS.OMS.UI.Windows.ucPanelNav[] GlobalPanels {get;}
        System.Windows.Forms.Control UIElement { get;}
	}

    public interface IOMSTypeAddin2 : IOMSTypeAddin
    {
        string Code { get; set; }
    }

	/// <summary>
	/// This interface makes it possible for a windows ui user control to open
	/// an OMS type within the oms type dialog form.
	/// </summary>
	public interface IOpenOMSType
	{
		event NewOMSTypeWindowEventHandler NewOMSTypeWindow;
	}



	/// <summary>
	/// Describes the contract for a repeater control to sit within an object selector repeater
	/// container.
	/// </summary>
	public interface ISelectorRepeater
	{
		event EventHandler Closed;
		event EventHandler Selected;
		event System.ComponentModel.CancelEventHandler Selecting;
		event EventHandler UnSelected;
		event System.ComponentModel.CancelEventHandler UnSelecting;

		object Object{get;set;}
		string Text{get;set;}

		bool IsSelected{get;}
		void Select();
		void UnSelect();

		void SetInfo (object [] parameters);

		bool HasMethod(SelectorRepeaterMethods methodType);
		void RunMethod(SelectorRepeaterMethods methodType);
	}

	/// <summary>
	/// Describes the OMS Type window.
	/// </summary>
	public interface IOMSTypeWindow
	{
		void Back();
		void Save();
		void Refresh();
		void Cancel();
		void ShowCommandCentre();
		void ShowSearch();
		void SetTabPage(string name);
        void GotoTab(string Code);
        IOMSItem GetTabsOMSItem(string Code);
	}

	public interface IOMSTypeDisplay : IOMSItem, IOpenOMSType
	{
		event EventHandler SearchManagerVisibleChanged;
		event EventHandler InfoPanelClose;

		void ShowSearchManager(SearchManager Style);
		void HideSearchManager();
		void SetTabPage(string name);
		void GotoNextTab();
		void GotoPreviousTab();
		void Open(FWBS.OMS.Interfaces.IOMSType obj);
        void GotoTab(string Code);
        IOMSItem GetTabsOMSItem(string Code);

		System.Windows.Forms.TabControl TabControl{get;}
		System.Windows.Forms.Panel Panels{get;}
	}
	

    /// <summary>
    /// Introduced to enable switching between the classic view of MatterSphere 
    /// and the new TreeView based navigation approach (V2).
    /// </summary>
    public interface IfrmOMSType : IDisposable
    {
        void Show();
        void SetTabPage(string defaultPage);
        DialogResult ShowDialog(IWin32Window owner);
        System.Windows.Forms.Form Owner { get; set; }
    }


    public interface IShowOMSItem : IDisposable
    {
        string Text { get; set; }
        string FormStorageID { get; set; }
        DialogResult ShowDialog(IWin32Window owner);
        EnquiryForm EnquiryForm { get; }
        System.Drawing.Size Size { get; set; }
        EnquiryFormSettings Settings { get; set; }
    }


    internal interface IEmbeddedOMSTypeDisplay
    {
        bool AlertsVisible { get; set; }
        System.Drawing.Image BackgroundImage { get; set; }
        ImageLayout BackgroundImageLayout {get; set; }
        Control Parent { get; }
        DockStyle Dock { get; set; }
        bool Enabled { get; set; }
        int Height { get; set; }
        bool InfoPanelCloseVisible { get; set; }
        bool InformationPanelVisible { get; set; }
        System.Drawing.Color ipc_BackColor { get; set; }
        int ipc_Width { get; set; }
        bool ipc_Visible { get; set; }
        bool IsDirty { get; }
        int Left { get; set; }
        System.Drawing.Point Location { get; set; }
        string Name { get; set; }
        FWBS.OMS.Interfaces.IOMSType Object { get; }
        string ObjectTypeDescription { get; }
        Padding Padding { get; set; }
        bool SearchManagerCloseVisible { get; set; }
        bool SearchManagerVisible { get; set; }
        string SearchText { get; set; }
        System.Drawing.Size Size { get; set; }
        int TabIndex { get; set; }
        TabAlignment TabPositions { get; set; }
        bool ElasticsearchVisible { get; set; }
        bool ToBeRefreshed { get; set; }
        int Top { get; set; }
        bool Visible { get; set; }
        int Width { get; set; }

        void ApplyFilter(int state);
        void BringToFront();
        void CancelItem();
        void Dispose();
        IOMSItem GetTabsOMSItem(string Code);
        void GotoTab(string Code);
        void Open(FWBS.OMS.Interfaces.IOMSType obj, OMSType omst);
        void RefreshItem();
        void RefreshItem(bool PanelsAlertsOnly);
        void RefreshSearchManager();
        void RefreshElasticsearch();
        void ResumeLayout();
        void SetTabPage(string name);
        void ShowSearchManager(SearchManager Style);
        void SuspendLayout();
        void UpdateItem();

        event EventHandler Dirty;
        event EventHandler InfoPanelClose;
        event NewOMSTypeWindowEventHandler NewOMSTypeWindow;
        event EventHandler SearchCompleted;
        event EventHandler SearchManagerVisibleChanged;
    }

    internal interface IDisplay
    {
        bool IsUser { get; }
        FWBS.OMS.Interfaces.IOMSType Object { get; }
        string SearchManagerHeading { get; }
        bool SearchManagerVisible { get; set; }
        void HideSearchManager();
        void SetElasticsearch();
        void ResetElasticsearchResults();
        void RemoveElasticSearch();
        void RemoveDefaultControls();
        void ShowDefaultControls();
    }

    internal interface IEmbeddedDisplay : IEmbeddedOMSTypeDisplay, IDisplay
    {
    }
}
