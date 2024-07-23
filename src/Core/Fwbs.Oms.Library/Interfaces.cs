using System;
using System.Data;

namespace FWBS.OMS
{
    public interface IService
    {
        bool IsLoaded { get;}
        void Load();
        void Unload();
    }



}

namespace FWBS.OMS.Interfaces
{

	/// <summary>
	/// This is an interface that wraps up common editable functionality for simple database objects.
	/// </summary>
	public interface ICommonObject : IExtraInfo, IEnquiryCompatible, IUpdateable, IDisposable
	{
		event EventHandler UniqueIDChanged;
		event EventHandler DataChanged;

		//TODO: Perhaps put the following events in the IUpdateable interface.
		event System.ComponentModel.CancelEventHandler Updating;
		event EventHandler Updated;

		object UniqueID{get;}
		void Restore();
		void Delete();
        bool IsDeleted { get; }
		bool Exists(object id);
		DataTable GetList();
		DataTable GetList(bool active);
		void Create();
		string FieldPrimaryKey{get;}
	}

	/// <summary>
	/// OMS Type Tabs Conditionals
	/// </summary>
	public interface IConditional
	{
		string Conditionals{get;}
	}


	/// <summary>
	/// An object implenting this interface could be used for extra type configurable
	/// information.  This can be used by the user interface layer to render a omsDialog 
	/// to display the object depending on the XML returned.  
	/// For instance the client uses this in the form of a client type.
	/// </summary>
	public interface IOMSType : IEnquiryCompatible
	{
		OMSType GetOMSType();
		object LinkValue {get;}
        string DefaultTab { get;set;}
        void SetCurrentSessions();
    }


	/// <summary>
	/// This interface ensures that an object must have a parent property.
	/// </summary>
	public interface IParent
	{
		object Parent{get;}
	}

	/// <summary>
	/// Compatible interfaces for enquiries to edit and update objects.
	/// </summary>
	public interface IEnquiryCompatible : IExtraInfo, IUpdateable, IParent
	{
		event EnquiryEngine.PropertyChangedEventHandler PropertyChanged;
		EnquiryEngine.Enquiry Edit(Common.KeyValueCollection param);
		EnquiryEngine.Enquiry Edit(string customForm, Common.KeyValueCollection param);
	}

	/// <summary>
	/// Ensures that an implemented object gets extra information based on a data set.
	/// </summary>
	public interface IExtraInfo
	{
		/// <summary>
		/// Gets any extra information from a dataset based on the fieldname.
		/// </summary>
		/// <param name="fieldName">Fieldname within database</param>
		/// <returns>Any object value depending on field type.</returns>
		object GetExtraInfo(string fieldName);

		/// <summary>
		/// Gets any extra information from a dataset based on the fieldname.
		/// </summary>
		/// <param name="fieldName">Fieldname within database</param>
		/// <returns>Any object value depending on field type.</returns>
		Type GetExtraInfoType(string fieldName);

		/// <summary>
		/// Sets any information within a dataset based on its field name.
		/// </summary>
		/// <param name="fieldName">Field name within dataset.</param>
		/// <param name="val">Value with the type depending on the field type.</param>
		void SetExtraInfo(string fieldName, object val);

		/// <summary>
		/// Returns a reference to the internal dataset of the object inmplementing the interface.
		/// </summary>
		DataSet GetDataset();


		/// <summary>
		/// Returns a datatable version of the internal recordet.
		/// </summary>
		DataTable GetDataTable();
	}


	/// <summary>
	/// Object that can be updated when implementing this interface.
	/// </summary>
	public interface IUpdateable
	{
		void Update();
		void Refresh();
		void Refresh(bool applyChanges);
		void Cancel();
		bool IsDirty{get;}
		bool IsNew{get;}
	}



	/// <summary>
	/// An interface that if implemented makes a specific object compatible to expose
	/// multiple extended data objects.
	/// </summary>
	public interface IExtendedDataCompatible
	{
		/// <summary>
		/// Gets the extended data list indexer which will expose
		/// each of the extended data objects on the particular object.
		/// </summary>
		ExtendedDataList ExtendedData {get;}

	}


	/// <summary>
	/// An interface that exposes alert functionality.
	/// </summary>
	public interface IAlert
	{
		Alert [] Alerts{get;}
        void AddAlert(Alert alert);
        void ClearAlerts();
	}

    public interface IVirtualDrive
    {
        bool IsMounted { get; }
        void Mount(string mountPoint, Data.Connection dbConnection);
        void Unmount();
    }

    internal interface IPowerProfile
    {
        string PowerMenuItem { get; set; }
        string PowerRoles { get; set; }
    }

}

