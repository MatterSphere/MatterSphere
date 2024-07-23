using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;
using FWBS.Common;
using FWBS.OMS.Data;
using FWBS.OMS.Interfaces;
using FWBS.OMS.Script;
using FWBS.OMS.SourceEngine;

namespace FWBS.OMS.EnquiryEngine
{


    /// <summary>
    /// An object that retrieves a data set schema from the database and sets it up to be used 
    /// to render a dynamic form to the screen.  This engine allows dynamic data retrieval 
    /// from multiple of different data sources in different binding styles.  An enquiry 
    /// form can add or edit data to and from a data source directly using SQL, or through 
    /// a .NET assembly object.  This enables the use of structured exception handling and 
    /// applied business logic before the underlying data source is updated and committed.
    /// </summary>
    public sealed class Enquiry : SourceEngine.Source, IDisposable
	{
		#region Fields
		/// <summary>
		/// The core internal dataset schema that stores multiple of relevant tables that need
		/// to be used to carry out the rendering process for whatever type of UI that uses it.
		/// </summary>
		private DataSet _enquiry = null;

		/// <summary>
		/// An IEnquiryCompatible object if set to business mapping binding.  This object
		/// may be constructed to add or edit the underlying data source.  The 
		/// intelligence of the update is built into the object, but a contract
		/// of methods and properties is guaranteed through the implementation of the 
		/// IEnquiryCompatible interface.
		/// </summary>
		private IEnquiryCompatible _obj = null;

        /// <summary>
        /// Property bag of the currently bound enquiry compatible object.
        /// </summary>
        private EnquiryPropertyCollection _props = null;

		/// <summary>
		/// The unique code name for the underlying enquiry form.  This is the form
		/// header code that is also localized through code lookups.
		/// </summary>
		private string _code = "";

		/// <summary>
		/// A flag that specifies whether the enquiry is currently in design mode.
		/// This can only be set through the constructor of an object, and only the
		/// designer tool can flag this to true for licensing and protection reasons.
		/// </summary>
		private bool _designMode = false;

		/// <summary>
		/// The mode of the enquiry form can currently be Add or Edit.  If editing
		/// extra parameters in the constructor will need to be specified as a record
		/// will have to be found to edit within the data source.
		/// </summary>
		private EnquiryMode _mode = EnquiryMode.Add;

		/// <summary>
		/// The binding type specifies where the form is to get the data from.  Business
		/// mapping binding will constrcut a .NET object and use the IEnquiryCompatible
		/// interface to get, set and update the information within. The bound binding
		/// type is used for direct SQL maipulation to the database.  There is also 
		/// unbound binding which enables a form to be rendered with no changes made to a 
		/// database.
		/// </summary>
		private EnquiryBinding _binding = EnquiryBinding.Unbound;

		/// <summary>
		/// A reference to a Session object that the enquiry engine uses.  This is used as
		/// a short cut to the static session object on 'FWBS.OMS.Session.OMS'.  Again, 
		/// this can only be set within the constructor of the enquiry object.
		/// </summary>
		private FWBS.OMS.Session _session = null;

		/// <summary>
		/// A boolean flag that is used to check whether the underlying enquiry form 
		/// dataset schema was retrieved on the local machine or from the database.  Every
		/// time the form is called there is a quick version check made.  If the version
		/// is unchanged within the database then a local cached version is used as it
		/// is already flat filed.  This reduces the load on the server, there is no need
		/// to execute many joins to return back exactly the same data schema.  There are triggers 
		/// on the enquiry related tables to update the version if any data changes are
		/// made.  The client will then fetch the new data schema from the database and renew the 
		/// local cache.
		/// </summary>
		private bool _local = false;


		/// <summary>
		/// A flag that specifies whether the data gets updated to the database
		/// or the  business mapped object gets its Update method called. This flag
		/// allows the capability of unbound binding for the other types of binding.
		/// A business mapped object still gets constructed but no update method is
		/// called.
		/// </summary>
		private bool _offline = false;

	
		/// <summary>
		/// Holds a reference to the script object for performing form and control events
		/// for the form when rendered to screen.
		/// </summary>
		private Script.ScriptGen _script = null;

        /// <summary>
        /// Settings
        /// </summary>
        private FWBS.Common.ConfigSetting _xmlSetting;
		#endregion

		#region Static Fields

		/// <summary>
		/// The enquiry header internal table name.
		/// </summary>
		private const string Table_Header = "ENQUIRY";
		
		/// <summary>
		/// Table name for the update order table.
		/// </summary>
		private const string Table_UpdateOrder= "UPDATEORDER";

		/// <summary>
		/// Table name for the pages table.
		/// </summary>
		private const string Table_Page = "PAGES";

		/// <summary>
		/// Table name for the questions table.
		/// </summary>
		private const string Table_Question = "QUESTIONS";

		/// <summary>
		/// Table name for the methods table.
		/// </summary>
		private const string Table_Method = "METHODS";

		/// <summary>
		/// Table name for the table that holds the added or edited data from a
		/// specified data source.
		/// </summary>
		internal const string Table_Data = "DATA";

		/// <summary>
		/// Table name for the methods table (Design Mode Only).
		/// </summary>
		private const string Table_Control = "CONTROLS";

		/// <summary>
		/// Table name for the lists table (Design Mode Only).
		/// </summary>
		private const string Table_List = "LISTS";

		/// <summary>
		/// Table name for the tables table (Design Mode Only).
		/// </summary>
		private const string Table_Table = "TABLES";

		/// <summary>
		/// Table name for the forms table (Design Mode Only).
		/// </summary>
		private const string Table_Form = "FORMS";

		/// <summary>
		/// Table name for the commands table (Design Mode Only).
		/// </summary>
		private const string Table_Command = "COMMANDS";



		//*********************************
		//The Following SQL statements are only used in design mode of the enquiry object
		//as they are used to update schema information back to the database when
		//the save method is called.
		//*********************************

		/// <summary>
		/// SQL statement for updating the enquiry header.
		/// </summary>
		private const string Sql_Header = "select * from dbenquiry";

		/// <summary>
		/// SQL statement for updating the enquiry questions.
		/// </summary>
		private const string Sql_Question = "select * from dbenquiryquestion";

		/// <summary>
		/// SQL statement for updating the update order.
		/// </summary>
		private const string Sql_UpdateOrder = "select * from dbenquirydatasource";

		/// <summary>
		/// SQL statement for updating the wizard pages.
		/// </summary>
		private const string Sql_Page = "select * from dbenquirypage";

		/// <summary>
		/// SQL statement for updating the methods.
		/// </summary>
		private const string Sql_Method = "select * from dbenquirymethod";

		/// <summary>
		/// SQL statement for updating the valid list of controls.
		/// </summary>
		private const string Sql_Control = "select * from dbenquirycontrol";

		/// <summary>
		/// SQL statement for updating the data lists table.
		/// </summary>
		private const string Sql_List = "select * from dbenquirydatalist";

		/// <summary>
		/// SQL statement for updating the command methods.
		/// </summary>
		private const string Sql_Command = "select * from dbenquirycommand";

		/// <summary>
		/// Binding flags used when getting type information on instance methods and properties.
		/// </summary>
		internal const BindingFlags MemberBinding = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		/// <summary>
		/// The engine version to be used for form compatiblity.
		/// </summary>
		public const long EngineVersion = 2;

		#endregion

		#region Events

		/// <summary>
		/// An event that gets raised when the script has be changed within the object.
		/// </summary>
		public event EventHandler ScriptChanged = null;


		/// <summary>
		/// This event gets raised when the add or edit mode changes from one another.
		/// </summary>
		public event EventHandler ModeChanged = null;
			
		/// <summary>
		/// This event gets raised when the underlying data source gets successfully updated.
		/// If this does not get raised when the update method is ran, then an exception
		/// will occur.
		/// </summary>
		public event EventHandler Updated = null;

		/// <summary>
		/// This event gets raised just before the underlying data source gets updated.
		/// </summary>
		public event CancelEventHandler Updating = null;

		/// <summary>
		/// This event gets raised when the underlying data source is changed to a 
		/// different data source (not if the data within it changes).
		/// </summary>
		public event EventHandler DataChanged = null;

		/// <summary>
		/// This event gets raised when the the refresh method is executed.
		/// </summary>
		public event EventHandler Refreshed = null;

		/// <summary>
		/// An event that gets raised when a property changes within the object.
		/// </summary>
		public event EnquiryEngine.PropertyChangedEventHandler FormPropertyChanged = null;

		/// <summary>
		/// Raises the property changed event with the specified event arguments.
		/// </summary>
        /// <param name="sender"></param>
		/// <param name="e">Property Changed Event Arguments.</param>
		public void OnFormPropertyChanged(object sender, EnquiryEngine.PropertyChangedEventArgs e)
		{
			if (FormPropertyChanged != null)
				FormPropertyChanged(sender, e);
		}
		
		/// <summary>
		/// Calls the script changed event.
		/// </summary>
		private void OnScriptChanged() 
		{
			if (ScriptChanged != null)
				ScriptChanged(this, EventArgs.Empty);
		}	

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a brand new enquiry object with a blank schema template.
		/// This can only be run in design mode.
		/// </summary>
		public Enquiry()
		{
			Session.CurrentSession.CheckLoggedIn();
			
			//Check if allowed to enter Design Mode
			if (Session.CurrentSession._designMode)
				_designMode = true;
		
			//Only in design mode.
			if (!_designMode)
				throw new EnquiryException(HelpIndexes.EnquiryOnlyInDesignMode);
			
			//Initialize fields.
			_code = "";
			_mode = EnquiryMode.Add;
			_binding = EnquiryBinding.Unbound;
			_offline = false;
			_obj = null;
            _props = null;

			_session = Session.OMS;
			
			//Fetch the enquiry schema.
			GetEnquiryData();  
		}

		/// <summary>
		/// Creates an enquiry form based on a Enquiry XML Cache File.  This can only
		/// be ran in design mode.
		/// </summary>
		/// <param name="Filename">XML FileName.</param>
		private Enquiry(string Filename)
		{
			Session.CurrentSession.CheckLoggedIn();

			//Check if allowed to enter Design Mode
			if (Session.CurrentSession._designMode)
				_designMode = true;
		
			//Only in design mode.
			if (!_designMode)
				throw new EnquiryException(HelpIndexes.EnquiryOnlyInDesignMode);

			GetEnquiryHeader("",Filename);


			
			foreach(DataRow rw in _enquiry.Tables[5].Rows)
			{
				string cl = FWBS.OMS.CodeLookup.GetLookup("ENQQUESTIONS",Convert.ToString(rw["qucode"]));
				if (cl.StartsWith("~"))
					FWBS.OMS.CodeLookup.Create("ENQQUESTIONS",Convert.ToString(rw["qucode"]),Convert.ToString(rw["qudesc"]),Convert.ToString(rw["quhelp"]),"{default}",true,false,true);

				if (FWBS.OMS.EnquiryEngine.DataLists.Exists(Convert.ToString(rw["qudatalist"]))==false)
				{
				}
			}
		}
		
		/// <summary>
		/// Creates an enquiry form based on a passed code and an IEnquiryCompatible 
		/// object type.  This object will be used to expose the underlying data source
		/// to edit only.
		/// </summary>
		/// <param name="code">Unique enquiry code.</param>
		/// <param name="parent">The parent object to extract information from.</param>
		/// <param name="obj">IEnquiryCompatible object.</param>
		/// <param name="offline">Flag to indicate wether a new record is created.</param>
		/// <param name="param">Named parameter collection.</param>
		private Enquiry(string code, object parent, IEnquiryCompatible obj, bool offline, Common.KeyValueCollection param)
		{
	
			//Initialize fields.
			_code = code;
			_mode = EnquiryMode.Edit;
			_obj = obj;
            _props = new EnquiryPropertyCollection(_obj, true);
			_offline = offline;
			_session = Session.OMS;
			_designMode = false;

			//If the overriding parent is null then set the passed objects parent.
			if (parent == null) parent = obj.Parent;

			//Set the parameter list.
			base.ChangeParameters(param);
			//Set the parent.
			base.ChangeParent(_obj);
			
			//Fetch the enquiry form schema.
			GetEnquiryData();  
		}

		/// <summary>
		/// Fetches the enquiry form schema based on the unique code passed.  
		/// This constructor also specifies whether the it is to add or edit a record
		/// or an IEnquiryCompatible object.
		/// </summary>
		/// <param name="code">Unique enquiry form code.</param>
		/// <param name="parent">The parent object to extract information from.</param>
		/// <param name="mode">Editing mode of the enquiry.</param>
		/// <param name="offline">Carries out the specified binding type but does not run the commands.</param>
		/// <param name="param">Parameter array.</param>
		private Enquiry(string code, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param)
		{
			Session.CurrentSession.CheckLoggedIn();

			//Initialize fields.
			_code = code;
			_mode = mode;
			_obj = null;
            _props = null;
			_offline = offline;
			_session = Session.OMS;
			_designMode = false;

			
			//Set the parameter list.
			base.ChangeParameters(param);
			//Set the parent.
			base.ChangeParent(parent);

			//Fetch the enquiry form schema.
			GetEnquiryData();
		}

		/// <summary>
		/// Fetches the enquiry form schema based on the unique code passed.  
		/// This constructor also specifies whether the it is to add or edit a record
		/// or an IEnquiryCompatible object. and possibly allow Design Mode if License Permits
		/// </summary>
		/// <param name="code">Unique enquiry form code.</param>
		/// <param name="parent">The parent object to extract information from.</param>
		/// <param name="mode">Editing mode of the enquiry.</param>
		/// <param name="offline">Carries out the specified binding type but does not run the commands.</param>
		/// <param name="param">Parameter array.</param>
		/// <param name="designMode">Allow Design Mode if License Permits</param>
		private Enquiry(string code, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param, bool designMode)
		{
			Session.CurrentSession.CheckLoggedIn();

			//Initialize fields.
			_code = code;
			_mode = mode;
			_obj = null;
            _props = null;
			_offline = offline;
			_session = Session.OMS;

			
			//Set the parameter list.
			base.ChangeParameters(param);
			//Set the parent.
			base.ChangeParent(parent);

			if (Session.CurrentSession._designMode)
				_designMode = designMode;

			//Fetch the enquiry form schema.
			GetEnquiryData();
		}
		

		#endregion

		#region Destructors & IDisposable Implementation


		/// <summary>
		/// Disposes the object immediately without waiting for the garbage collector.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		/// <summary>
		/// Disposes all internal objects used by this object.
		/// </summary>
		/// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
		private void Dispose(bool disposing) 
		{
			//Un-assign the property changed event.
			if (disposing)
			{
				_local = false;

				if (_obj != null)
				{
					_obj.PropertyChanged -= new PropertyChangedEventHandler(this.PropertyChanged);

                    //24910 - Duplication Check
                    if (_obj is OMSDocument && _obj.IsDirty)
				    {
				        _obj.Cancel();
				    }

					_obj = null;
                    _props = null;
				}

				_session = null;


				if (_enquiry != null)
				{
					_enquiry.Dispose();
					_enquiry = null;
				}

				if (_script != null)
				{
					_script.Dispose();
					_script = null;
				}
			}

			//Dispose any unmanaged objects.
		}
	
		#endregion

		#region Static Methods

		/// <summary>
		/// Deletes a specified enquiry form from the enquiry engine.
		/// </summary>
		/// <param name="code">The enquiry form code to delete.</param>
		public static bool Delete(string code)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.NVarChar, 15, code);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("Overwrite", System.Data.SqlDbType.Bit, 15, 0);
			int _rowsefected = Session.CurrentSession.Connection.ExecuteProcedure("sprEnquiryDelete", paramlist); 
			if (_rowsefected == -1) return false; else return true;
		}

		/// <summary>
		/// Overwrites an existing enquiry form.
		/// </summary>
		/// <param name="code">The code lookup code to overwrite.</param>
		public static bool Overwrite(string code)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] paramlist = new IDataParameter[2];			
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.NVarChar, 15, code);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("Overwrite", System.Data.SqlDbType.Bit, 15, 1);
			int _rowsefected = Session.CurrentSession.Connection.ExecuteProcedure("sprEnquiryDelete", paramlist); 
			if (_rowsefected == -1) return false; else return true;
		}

		/// <summary>
		/// Imports a sepcified enquiry through an existing cached file.
		/// </summary>
		/// <param name="fileName">The file name of the cacehed enquiry form.</param>
		public static void ImportEnquiry(string fileName)
		{
			Session.CurrentSession.CheckLoggedIn();
			Enquiry enq = new Enquiry(fileName);
			enq.Save();
		}


		/// <summary>
		/// Gets an enquiry form based on a passed code and an IEnquiryCompatible 
		/// object type.  This object will be used to expose the underlying data source
		/// to edit only.  In most cases the parameter array must have at least one value 
		/// as this will be needed to construct the IEnquiryCompatible object with the 
		/// value, so that it can use it to uniquely identify the underlying data entity
		/// (example: the user ID of a user object).
		/// </summary>
		/// <param name="code">Unique enquiry code.</param>
		/// <param name="parent">The parent object to extract information from.</param>
		/// <param name="obj">IEnquiryCompatible object.</param>
		/// <param name="param">Named parameter collection.</param>
		public static Enquiry GetEnquiry(string code, object parent, IEnquiryCompatible obj, Common.KeyValueCollection param)
		{
			Session.CurrentSession.CheckLoggedIn();
			return new Enquiry(code, parent, obj, false, param);
		}
		
		public static Enquiry GetEnquiry(string code, object parent, IEnquiryCompatible obj, bool offline, Common.KeyValueCollection param)
		{
			Session.CurrentSession.CheckLoggedIn();
			return new Enquiry(code,parent,obj,offline,param);
		}


		/// <summary>
		/// Fetches the enquiry form schema based on the unique code passed. This
		/// constructor allows to specify the adding and editing mode.
		/// </summary>
		/// <param name="code">Unique enquiry form code.</param>
		/// <param name="parent">The parent object to extract information from.</param>
		/// <param name="mode">Editing mode of the enquiry.</param>
		/// <param name="param">Named parameter collection.</param>
		public static Enquiry GetEnquiry(string code, object parent, EnquiryMode mode, Common.KeyValueCollection param)
		{
			Session.CurrentSession.CheckLoggedIn();
			return new Enquiry(code, parent, mode, false, param);
		}

		/// <summary>
		/// Fetches the enquiry form schema based on the unique code passed. This
		/// constructor loads the Enquiry in Design Mode is license permits
		/// </summary>
		/// <param name="code">Unique enquiry form code.</param>
		public static Enquiry GetEnquiryInDesign(string code)
		{
			Session.CurrentSession.CheckLoggedIn();
			return new Enquiry(code, null, EnquiryMode.Add, true, null,true);
		}

		/// <summary>
		/// Fetches the enquiry form schema based on the unique code passed.  
		/// This constructor also specifies whether the it is to add or edit a record
		/// or an IEnquiryCompatible object.
		/// </summary>
		/// <param name="code">Unique enquiry form code.</param>
		/// <param name="parent">The parent object to extract information from.</param>
		/// <param name="mode">Editing mode of the enquiry.</param>
		/// <param name="offline">Runs the enquiry form in a normally but will not update the underlying data source.</param>
		/// <param name="param">Parameter array.</param>
		public static Enquiry GetEnquiry(string code, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param)
		{
			Session.CurrentSession.CheckLoggedIn();
			return new Enquiry(code, parent, mode, offline, param);
		}

		/// <summary>
		/// Retrieves a list of Enquiry Headers
		/// </summary>
		/// <returns>A data table to list the enquiry headers.</returns>
		public static DataTable GetEnquiryHeaders()
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Table", SqlDbType.NVarChar, 25, "FORMS");
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
			DataTable dt = Session.CurrentSession.Connection.ExecuteProcedureTable("sprEnquiryTableInformation", "FORMS", paramlist);
			return dt;
		}

		/// <summary>
		/// Retrieves a list of Enquiry Commands
		/// </summary>
		/// <returns>A data table to list the enquiry Commands.</returns>
		public static DataTable GetEnquiryCommands()
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Table", SqlDbType.NVarChar, 25, "COMMANDS");
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
			DataTable dt = Session.CurrentSession.Connection.ExecuteProcedureTable("sprEnquiryTableInformation", "COMMANDS", paramlist);
			return dt;
		}
		
		/// <summary>
		/// Retrieves a list of Enquiry Commands
		/// </summary>
		/// <returns>A data table to list the enquiry Commands.</returns>
		public static DataTable GetEnquiryFormsByDataList(string dataList)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("DL", SqlDbType.NVarChar, 25, dataList);
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("select c.enqcode from dbenquiry c join dbenquiryquestion q on c.enqid = q.enqid where q.qudatalist = @DL","dbenquiry", paramlist);
			return dt;
		}

		/// <summary>
		/// Gets a list of Enquiry controls
		/// </summary>
		/// <returns>dataview of the dbEnquiryControl table</returns>
		public static DataView GetEnquiryControls()
		{
			Session.CurrentSession.CheckLoggedIn();
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbEnquiryControl","CONTROLS",null);
			return dt.DefaultView;
		}

		/// <summary>
		/// Gets an enquiry picture of the form in the form of a byte array.
		/// </summary>
		/// <param name="code">The enquiry code to get the picture from.</param>
		/// <returns>An image in the for of a byte array.</returns>
		public static byte[] GetEnquiryPicture(string code)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, code);
			object val  = Session.CurrentSession.Connection.ExecuteSQLScalar("select enqimage from dbenquiry where enqcode = @Code", paramlist);
			if (val is byte[])
				return (byte[])val;
			else
				return null;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the unique enquiry form code.
		/// </summary>
		public string Code
		{
			get
			{
				return _code;
			}
		}

        Image image = null;
        public Image WelcomePageImage
        {
            get
            {
                if (image == null && String.IsNullOrEmpty(_xmlSetting.GetSetting("Wizard","Image","")) == false)
                {
                    string base64 = _xmlSetting.GetSetting("Wizard", "Image", "");
                    try
                    {
                        using (System.IO.MemoryStream reader = new System.IO.MemoryStream())
                        {
                            byte[] buffer = Convert.FromBase64String(base64);
                            reader.Write(buffer, 0, buffer.Length);
                            reader.Position = 0;
                            image = Image.FromStream(reader);
                        }
                    }
                    catch { }
                }
                return image;
            }
        }

        private int _glyph = -1;
        public int Glyph
        {
            get
            {
                if (_glyph < 0 && _xmlSetting != null)
                    _glyph = Convert.ToInt32(_xmlSetting.GetSetting("View", "Glyph", "-1"));
                return _glyph;
            }
        }

		/// <summary>
		/// Gets the localized descriptive name of the enquiry form.
		/// </summary>
		public string EnquiryName
		{
			get
			{
				return FWBS.OMS.CodeLookup.GetLookup(EnquiryCodeLookupType.EnqHeader.ToString(), _code);
			}
		}
		

		/// <summary>
		/// Gets the local cached flag. True if the item was retrieved from a cached 
		/// version on the local machine, False if a new version was fetched from the
		/// database.
		/// </summary>
		public bool Local
		{
			get
			{
				return _local;
			}
		}


		/// <summary>
		/// Gets the version number of the form (-1 if not a valid form or the form is not present).
		/// </summary>
		public long Version
		{
			get
			{
				try
				{
					return  (long)GetHeaderInfo("enqVersion");
				}
				catch
				{
					return -1;
				}
			}
		}


		/// <summary>
		/// Gets or Sets the edit mode of the enquiry form.
		/// </summary>
		public EnquiryMode Mode
		{
			get
			{
				return _mode;
			}
			set
			{
				_session.CheckLoggedIn();
				if (value != _mode)
				{
					_mode = value;

					GetEnquiryData();
					
					if (ModeChanged != null)
						ModeChanged(this, EventArgs.Empty);
				}
			}
		}
		

		
		/// <summary>
		/// Gets or Sets the binding mode of the enquiry.
		/// </summary>
		public EnquiryBinding Binding
		{
			get
			{
				return _binding;
			}
		}

		/// <summary>
		/// Gets the underlying enquiry dataset schema.
		/// </summary>
		public DataSet Source
		{
			get
			{
				return _enquiry;
			}
		}


		/// <summary>
		/// Gets the design mode status of the enquiry form.
		/// </summary>
		public bool InDesignMode
		{
			get
			{
				return _designMode;
			}
		}


		/// <summary>
		/// Gets the object that has been updated or created.  
		/// If binded using business mapping then the IEnquiryCompatible object is returned.
		/// Otherwise, the underlying data table is returned.
		/// </summary>
		public object Object
		{
			get
			{
				if (_obj != null)
					return _obj;
				else
					return GetDATATable();
			}
            set
            {
                IEnquiryCompatible end = value as IEnquiryCompatible;
                _obj = end;
                _props = new EnquiryPropertyCollection(_obj, true);
            }
		}

		/// <summary>
		/// Gets or Sets the flag that specifies the offline status of the enquiry object.
		/// This allows you to return IEnquiryCompatible object or the underlying data table
		/// without any actual physical update to the database.
		/// </summary>
		public bool Offline
		{
			get
			{
				return _offline;
			}
			set
			{
				_offline = value;
			}
		}

		/// <summary>
		/// Gets a boolean value that indicates whether the enquiry form ahas a script assopciated
		/// with it.
		/// </summary>
		public bool HasScript
		{
			get
			{
				try
				{
					if (Convert.ToString(GetHeaderInfo("enqScript")) == "")
						return false;
					else
						return true;
				}
				catch
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Gets the enquiry script type of the of the current enquiry form.
		/// </summary>
		public ScriptGen Script
		{
			get
			{
				if (_script == null)
				{
					if (HasScript && ScriptGen.Exists(Convert.ToString(GetHeaderInfo("enqScript"))))
					{
						_script = ScriptGen.GetScript(Convert.ToString(GetHeaderInfo("enqScript")));
						OnScriptChanged();
					}
					else
					{
                        _script = new ScriptGen(Convert.ToString(GetHeaderInfo("enqScript")), "ENQUIRYFORM");
                        OnScriptChanged();
					}
				}
				return _script;
			}
		}

		public System.Drawing.Size CanvasSize
		{
			get
			{
				return new System.Drawing.Size(Convert.ToInt32(GetHeaderInfo("enqCanvasWidth")),Convert.ToInt32(GetHeaderInfo("enqCanvasHeight")));
			}
		}

		public string Helpfilename
		{
			get
			{
				return Convert.ToString(GetHeaderInfo("enqHelp"));
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Create a New Script object for the Enquiry Form
		/// </summary>
		public void NewScript()
		{
			_script = null;
		}

        /// <summary>
        /// Create a New Script object for the Enquiry Form
        /// </summary>
        public void CopyScript(string NewName)
        {
            if (ScriptGen.Exists(NewName))
                ScriptGen.Delete(NewName);
            string oldcode = _script.Code;
            _script = _script.Clone();
            _script.Code = NewName;
            _script.Update();
            _script = ScriptGen.GetScript(NewName);
            if (_script.AdvancedScript)
                _script.RenameClass(oldcode, NewName);
            _script.Compile(true);
            _script.Update();
        }

		/// <summary>
		/// Sets a value to the underlying data table.  This should be use to set the raw data directly.
		/// Any data binding applied should update any controls bound to it in the UI layers.
		/// </summary>
		/// <param name="control">The control name.</param>
		/// <param name="val">The value to set it to.</param>
		public void SetValue (string control, object val)
		{
			DataTable data = GetDATATable();
			if (data.Columns.Contains(control))
			{
				data.Rows[0][control] = val;
			}
		}

        /// <summary>
        /// Refreshes the underlying data list for the specific question.
        /// </summary>
        /// <param name="question">The question requesting a refresh.</param>
		
        public void RefreshDataList(string question)
        {
            if (String.IsNullOrEmpty(question))
                return;

            DataView dv = new DataView(Source.Tables[Table_Question], String.Format("[quName] = '{0}'", question), "", DataViewRowState.CurrentRows);
            if (dv.Count > 0)
                RefreshDataList(dv[0].Row);
        }


        public void RefreshDataList (DataRow question)
		{
			GetDataList(question, true);
		}

		/// <summary>
		/// Refreshes the specified datatable from the database, incase any wizard or enquiry form
		/// has added, updated or deleted items within one of the static tables.
		/// This is a Design Mode Only method.
		/// </summary>
		/// <param name="dt">Data table reference within the source to refresh.</param>
		public void RefreshTable(DataTable dt)
		{
			if (!InDesignMode)
				throw new EnquiryException(HelpIndexes.EnquiryOnlyInDesignMode);
			
			IDataParameter [] paramlist = new IDataParameter[2];
			paramlist[0] = _session.Connection.AddParameter("Table", SqlDbType.NVarChar, 25, dt.TableName);
			paramlist[1] = _session.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
			DataTable data = _session.Connection.ExecuteProcedureTable("sprEnquiryTableInformation", dt.TableName, paramlist);

			DataColumn [] cols = new DataColumn[dt.PrimaryKey.GetUpperBound(0) + 1];
			int ctr = 0;
			foreach (DataColumn col in dt.PrimaryKey)
			{
				cols[ctr] = data.Columns[col.ColumnName];
				ctr++;
			}
												   
			data.PrimaryKey = cols;

			if (dt != null)
			{
				_enquiry.Merge(data);
				dt.AcceptChanges();
			}
			else
			{
				AddError("BAL.Enquiry.RefreshTable()", "INFO", String.Format("Table '{0}' cannot be refreshed.", dt.TableName)); 
			}

		}

		/// <summary>
		/// Get the enquiry header of the enquiry form and names all the other schema 
		/// tables used to render the form.  This method checks the the version of
		/// the cached XML interpretation of the enquiry from schema with the relational
		/// version in the databse.
		/// </summary>
		/// <param name="code">Unique enquiry code.</param>
		private void GetEnquiryHeader(string code)
		{
			 GetEnquiryHeader(code,"");
		}
		
		/// <summary>
		/// Get the enquiry header of the enquiry form and names all the other schema 
		/// tables used to render the form.  This method checks the the version of
		/// the cached XML interpretation of the enquiry from schema with the relational
		/// version in the databse.
		/// </summary>
		/// <param name="code">Unique enquiry code.</param>
        /// <param name="Filename"></param>
		private void GetEnquiryHeader(string code, string Filename)
		{
			DataSet ds = null;		//Internal schema used.
			DataSet fds = null;		//Cached schema version.
			long version = 0;		//Version specifier.

			//Loads the cached version of the enquiry schema and gets the version
			//from the header information.  If there is an error opening the file
			//then set the version to zero.  The enquiry form will then be completely
			//refreshed from the databse.
			try
			{
                if (String.IsNullOrEmpty(Filename))
                    fds = Global.GetCache(@"enquiries\" + _session.Edition + @"\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name, code + "." + Global.CacheExt);
                else
                {
                    FileInfo filenameinfo = new FileInfo(Filename);
                    if (filenameinfo.Exists)
                        fds.ReadXml(Filename, XmlReadMode.ReadSchema);
                }
                if (fds != null)
                    version = (long)fds.Tables[Table_Header].Rows[0]["enqversion"];
                else
                    version = 0;
			}
			catch
			{
				fds = null;
				version = 0;
			}
			
		
			IDataParameter [] paramlist = new IDataParameter[6];
			//Run the sprEnquiryBuilder stored procedure and pass it the found version 
			//number.  If there is a newer version then cache the newly generated schema.
			paramlist[0] = _session.Connection.AddParameter("Code", System.Data.SqlDbType.NVarChar, 15, code);
			paramlist[1] = _session.Connection.AddParameter("Version", System.Data.SqlDbType.BigInt, 0, (Session.CurrentSession._designMode ? 0 : version));
			paramlist[2] = _session.Connection.AddParameter("Force", System.Data.SqlDbType.Bit, 0, 0);
			paramlist[3] = _session.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
			paramlist[4] = _session.Connection.AddParameter("Edition", System.Data.SqlDbType.VarChar, 2, _session.Edition);
			paramlist[5] = _session.Connection.AddParameter("Designer", System.Data.SqlDbType.Bit, 0, InDesignMode);

			//Allow an execution error to escalate through the stack.
			ds = _session.Connection.ExecuteProcedureDataSet("sprEnquiryBuilder", new string[1]{Table_Header}, paramlist);
			
			
	
			//Make sure that there is a valid enquiry form returned.  
			//If not then use the already cached version of the enquiry form.
			if ((ds == null) || (ds.Tables.Count == 0))
			{
				if ((fds == null) || (fds.Tables[Table_Header] == null))
				{
					//The returned data set chema is invalid and there is not cached version
					//to rely on.
					throw new EnquiryException(HelpIndexes.EnquiryDoesNotExist, code); 
				}
				else
				{
					//The locally cached version is being used.
					Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Using local version of enquiry form '" + _code + "'", "BAL.Enquiry.GetEnquiryHeader()");
					ds = fds;
					_local = true;
				}
			}
			else
			{
				Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Using database version of enquiry form '" + _code + "'", "BAL.Enquiry.GetEnquiryHeader()");
				
				//Name the required schema tables by using the Table_? constants declared
				//at the top of the class.
				//TODO: The insertion order table will be for bound binding types where it needs to insert into two or more different tables.
				ds.Tables[1].TableName = Table_UpdateOrder;
				ds.Tables[2].TableName = Table_Page;
				ds.Tables[3].TableName = Table_Method;
				ds.Tables[4].TableName = Table_Question;
			}

			//Set the object scoped internal data set schema.
			_enquiry = ds;

			//DESIGN MODE ONLY  
			//*******************
			//Name all the required design time tables.  These tables are only returned
			//back when the design mode flag was sent to the stored procedure.
			//ALSO: Apply referential integrity on the tables so that the designer cannot
			//enter invalid data which will just error out on the server level anyway.
			if (InDesignMode)
			{
				_enquiry.Tables[5].TableName = Table_Control;
				_enquiry.Tables[6].TableName = Table_List;
				_enquiry.Tables[7].TableName = Table_Table;
				_enquiry.Tables[8].TableName = CodeLookupLocalized.Table;
				_enquiry.Tables[9].TableName = Table_Form;
				_enquiry.Tables[10].TableName = Table_Command;
				_enquiry.Tables[11].TableName = ExtendedData.Table;
				
				//Terminology parse the data lists descriptions.
				foreach (DataRow row in ds.Tables[Table_List].Rows)
				{
					row["enqTableDesc"] = _session.Terminology.Parse(row["enqTableDesc"].ToString(), true);
				}

				AddConstraintsAndDefaults();				

			
			}

		}


		/// <summary>
		/// Adds the contraints to all of the tables.
		/// </summary>
		private void AddConstraintsAndDefaults()
		{
			//Set the referential PK and FK links.
			_enquiry.Tables[Table_UpdateOrder].ParentRelations.Add(_enquiry.Tables[Table_Header].Columns["enqid"], _enquiry.Tables[Table_UpdateOrder].Columns["enqid"]);
			_enquiry.Tables[Table_Page].ParentRelations.Add(_enquiry.Tables[Table_Header].Columns["enqid"], _enquiry.Tables[Table_Page].Columns["enqid"]);
			_enquiry.Tables[Table_Method].ParentRelations.Add(_enquiry.Tables[Table_Header].Columns["enqid"], _enquiry.Tables[Table_Method].Columns["enqid"]);
			_enquiry.Tables[Table_Question].ParentRelations.Add(_enquiry.Tables[Table_Header].Columns["enqid"], _enquiry.Tables[Table_Question].Columns["enqid"]);
			_enquiry.Tables[Table_Question].ParentRelations.Add(_enquiry.Tables[Table_Control].Columns["ctrlid"], _enquiry.Tables[Table_Question].Columns["quctrlid"]);
			_enquiry.Tables[Table_Question].ParentRelations.Add(_enquiry.Tables[Table_List].Columns["enqTable"], _enquiry.Tables[Table_Question].Columns["qudatalist"]);
			_enquiry.Tables[Table_Question].ParentRelations.Add(_enquiry.Tables[Table_Command].Columns["cmdcode"], _enquiry.Tables[Table_Question].Columns["qucommand"]);
			_enquiry.Tables[Table_Question].ParentRelations.Add(_enquiry.Tables[ExtendedData.Table].Columns["extCode"], _enquiry.Tables[Table_Question].Columns["quextendeddata"]);
			
			//Set any primary keys to the information list tables.
			_enquiry.Tables[Table_Header].PrimaryKey = new DataColumn[]{_enquiry.Tables[Table_Header].Columns["enqid"]};
			_enquiry.Tables[Table_UpdateOrder].PrimaryKey = new DataColumn[]{_enquiry.Tables[Table_UpdateOrder].Columns["enqid"], _enquiry.Tables[Table_UpdateOrder].Columns["enqorder"]};
			_enquiry.Tables[Table_Page].PrimaryKey = new DataColumn[]{_enquiry.Tables[Table_Page].Columns["enqid"], _enquiry.Tables[Table_Page].Columns["pgeorder"]};
			_enquiry.Tables[Table_Method].PrimaryKey = new DataColumn[]{_enquiry.Tables[Table_Method].Columns["enqid"], _enquiry.Tables[Table_Method].Columns["enqorder"]};
			_enquiry.Tables[Table_Control].PrimaryKey = new DataColumn[]{_enquiry.Tables[Table_Control].Columns["ctrlid"]};
			_enquiry.Tables[Table_List].PrimaryKey = new DataColumn[]{_enquiry.Tables[Table_List].Columns["enqtable"]};
			_enquiry.Tables[Table_Command].PrimaryKey = new DataColumn[]{_enquiry.Tables[Table_Command].Columns["cmdid"]};
			_enquiry.Tables[Table_Question].PrimaryKey = new DataColumn[]{_enquiry.Tables[Table_Question].Columns["quid"]};
			_enquiry.Tables[ExtendedData.Table].PrimaryKey = new DataColumn[]{_enquiry.Tables[ExtendedData.Table].Columns["extcode"]};
			_enquiry.Tables[Table_Form].PrimaryKey = new DataColumn[]{_enquiry.Tables[Table_Form].Columns["enqcode"]};
			

			//Make sure that the questions control name is unique.
			_enquiry.Tables[Table_Question].Constraints.Add("name", _enquiry.Tables[Table_Question].Columns["quname"], true);

			//If the enquiry item is a new one then set the header table's defaults.
			//Create a new base row for the designer to work with.
			//WARNING: This int.MaxValue logic may be different to the databses maximum integer value.  This will work though with SQL server 2000 and windows client machines.
			if (_code == String.Empty)
			{

				Random rnd = new Random();

				_enquiry.Tables[Table_Header].Columns["enqID"].DefaultValue = rnd.Next(int.MaxValue);
				_enquiry.Tables[Table_Header].Columns["enqCode"].DefaultValue = String.Empty;
				_enquiry.Tables[Table_Header].Columns["enqSourceType"].DefaultValue = SourceEngine.SourceType.OMS.ToString().ToUpper();
				_enquiry.Tables[Table_Header].Columns["enqDesc"].DefaultValue = "untitled";
				_enquiry.Tables[Table_Header].Columns["enqVersion"].DefaultValue = 0;
				_enquiry.Tables[Table_Header].Columns["enqPaddingX"].DefaultValue = 0;
				_enquiry.Tables[Table_Header].Columns["enqPaddingY"].DefaultValue = 0;
				_enquiry.Tables[Table_Header].Columns["enqLeadingX"].DefaultValue = 0;
				_enquiry.Tables[Table_Header].Columns["enqLeadingY"].DefaultValue = 0;
				_enquiry.Tables[Table_Header].Columns["enqModes"].DefaultValue = 7;
				_enquiry.Tables[Table_Header].Columns["enqBindings"].DefaultValue = 1;
				_enquiry.Tables[Table_Header].Columns["enqCanvasHeight"].DefaultValue = 394;
				_enquiry.Tables[Table_Header].Columns["enqCanvasWidth"].DefaultValue = 506;
				_enquiry.Tables[Table_Header].Columns["enqWizardHeight"].DefaultValue = 394;
				_enquiry.Tables[Table_Header].Columns["enqWizardWidth"].DefaultValue = 506;
				_enquiry.Tables[Table_Header].Rows.Add(_enquiry.Tables[Table_Header].NewRow());
			}

			//Set the default values of the questions table.
			DataTable questions = GetQUESTIONSTable();
			questions.Columns["quid"].AutoIncrement = true;
			questions.Columns["enqid"].DefaultValue = GetHeaderInfo("enqid");
			questions.Columns["qucode"].DefaultValue = String.Empty;
			questions.Columns["qudesc"].DefaultValue = "untitled";
			questions.Columns["quhelp"].DefaultValue = String.Empty;
			questions.Columns["qupage"].DefaultValue = 0;
			questions.Columns["quorder"].DefaultValue = 0;
			questions.Columns["qutype"].DefaultValue  = "System.String";
			questions.Columns["quctrlid"].DefaultValue  = 0;
			questions.Columns["quadd"].DefaultValue  = true;
			questions.Columns["quedit"].DefaultValue  = true;
			questions.Columns["quaddseclevel"].DefaultValue  = 0;
			questions.Columns["queditseclevel"].DefaultValue  = 0;
			questions.Columns["quunique"].DefaultValue  = false;
			questions.Columns["quhidden"].DefaultValue  = false;
			questions.Columns["qurequired"].DefaultValue  = false;
			questions.Columns["qureadonly"].DefaultValue = false;
			questions.Columns["quminlength"].DefaultValue  = 0;
			questions.Columns["qumaxlength"].DefaultValue  = 255;
			questions.Columns["quwidth"].DefaultValue  = 300;
			questions.Columns["quheight"].DefaultValue  = 22;
			questions.Columns["qux"].DefaultValue  = 0;
			questions.Columns["quy"].DefaultValue  = 0;
			questions.Columns["qucaptionwidth"].DefaultValue  = 150;
			questions.Columns["qusystem"].DefaultValue = false;
			questions.Columns["qusearch"].DefaultValue = false;
			questions.Columns["qucommandRetVal"].DefaultValue = true;
			questions.Columns["qucolumn"].DefaultValue = 0;
			questions.Columns["qufilter"].DefaultValue = "<filters/>";
			questions.Columns["qucasing"].DefaultValue  = "Normal";


			//Set the default link values of the methods table.
			_enquiry.Tables[Table_Method].Columns["enqID"].DefaultValue = GetHeaderInfo("enqid");
			_enquiry.Tables[Table_Method].Columns["enqOrder"].DefaultValue = 0;
			_enquiry.Tables[Table_Method].Columns["enqParameters"].DefaultValue = "<params></params>";

			//Set the default linking values of the pages table.
			_enquiry.Tables[Table_Page].Columns["enqID"].DefaultValue = GetHeaderInfo("enqid");
			_enquiry.Tables[Table_Page].Columns["pgeOrder"].DefaultValue = 0;
			_enquiry.Tables[Table_Page].Columns["pgeCustom"].DefaultValue = false;

			//Set the default linking values of the update order table.
			_enquiry.Tables[Table_UpdateOrder].Columns["enqID"].DefaultValue = GetHeaderInfo("enqid");
			_enquiry.Tables[Table_UpdateOrder].Columns["enqOrder"].DefaultValue = 0;
	
		}



		/// <summary>
		/// Clears the contraints on all the tables used.
		/// </summary>
		private void ClearContraints()
		{
			_enquiry.Tables[Table_UpdateOrder].ParentRelations.Clear();
			_enquiry.Tables[Table_UpdateOrder].Constraints.Clear();
			_enquiry.Tables[Table_Page].ParentRelations.Clear();
			_enquiry.Tables[Table_Page].Constraints.Clear();
			_enquiry.Tables[Table_Method].ParentRelations.Clear();
			_enquiry.Tables[Table_Method].Constraints.Clear();
			_enquiry.Tables[Table_Question].ParentRelations.Clear();
			_enquiry.Tables[Table_Question].ParentRelations.Clear();
			_enquiry.Tables[Table_Question].ParentRelations.Clear();
			_enquiry.Tables[Table_Question].ParentRelations.Clear();
			_enquiry.Tables[Table_Question].ParentRelations.Clear();
			_enquiry.Tables[Table_Question].Constraints.Clear();
			_enquiry.Tables[Table_Header].ParentRelations.Clear();
			_enquiry.Tables[Table_Header].Constraints.Clear();
		}

		/// <summary>
		/// Builds the questions based on the internal information and adds the underlying
		/// data source needed.  This data source is used to bind straight to the rendered
		/// controls.
		/// </summary>
		private void GetEnquiryData()
		{

			//Keep the connection open.
			_session.Connection.Connect(true);

			//Get the header dataset schema.
			GetEnquiryHeader(_code);
			
			//Set the base objects properties.
			base.SourceType = (SourceEngine.SourceType)Enum.Parse(typeof(SourceEngine.SourceType), Convert.ToString(GetHeaderInfo("enqSourceType")),true);
			base.Src = Convert.ToString(GetHeaderInfo("enqSource"));
			base.Call = Convert.ToString(GetHeaderInfo("enqCall"));
			base.Parameters = Convert.ToString(GetHeaderInfo("enqParameters"));
            
            if (_enquiry.Tables[Table_Header].Columns.Contains("enqSettings"))
                _xmlSetting = new FWBS.Common.ConfigSetting(_enquiry.Tables[Table_Header].Rows[0], "enqSettings");
            else
                _xmlSetting = new FWBS.Common.ConfigSetting("");

			//Gets the enquiry binding value.
			try
			{
				if (InDesignMode)
					_binding = EnquiryBinding.Unbound;
				else
					_binding = (EnquiryBinding)Enum.ToObject(typeof(EnquiryBinding), Convert.ToInt64(GetHeaderInfo("enqBindings")));
			}
			catch
			{
				throw new EnquiryException(HelpIndexes.EnquiryDoesNotSupportBinding, _code);
			}

			//Name the schema data set (not really required but nice to name).
			_enquiry.DataSetName = "EnquiryForm";
	
			//Check the compatibility issues for modes and bindings.  This does not
			//need to be checked design mode though, the designer needs to be flexible.
			if (!InDesignMode) CheckCompatibilityIssues();

			//Terminoly parse the enquiry header and welcome texts.
			_enquiry.Tables[Table_Header].Rows[0]["enqDesc"] = _session.Terminology.Parse(GetHeaderInfo("enqDesc").ToString(), true);
			_enquiry.Tables[Table_Header].Rows[0]["enqWelcomeHeader"] = _session.Terminology.Parse(GetHeaderInfo("enqWelcomeHeader").ToString(), true);
			_enquiry.Tables[Table_Header].Rows[0]["enqWelcomeText"] =  _session.Terminology.Parse(GetHeaderInfo("enqWelcomeText").ToString(), true);

			//Terminolgy parse each of the page descriptions.
			if (_enquiry.Tables.Contains(Table_Page))
			{
				foreach (DataRow page in _enquiry.Tables[Table_Page].Rows)
				{
					page["pgeDesc"] = _session.Terminology.Parse(page["pgeDesc"].ToString(), true);
					page["pgeShortDesc"] = _session.Terminology.Parse(page["pgeShortDesc"].ToString(), true);
				}
			}

			//Do not persist the underlying data structure within the cached version.
			//Remove this table if it already exists.
			if (_enquiry.Tables[Table_Data] != null)
				_enquiry.Tables.Remove(Table_Data);

			//Cache the enquiry item to the users application data folder, but only if it was taken
			//from the databse.
			//(e.g., "%APPDATA%\fwbs\oms\enquiries\user.xml").
			if (!_local && !InDesignMode)
				Global.Cache(_enquiry, @"enquiries\" + _session.Edition + @"\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name , _code + "." + Global.CacheExt);


			//Remove the questions which do not apply to the add edit or user security level.
			//Make sure that this only happens in live mode, not design mode.  Also delete these
			//records after saving to the cache so that the full set of questions are in the cached
			//file.  These questions must appear in the designer so that it can still be designed
			//as if the questions are there.
			if (!InDesignMode)
			{
				//TODO: Security level needs to be linked to a users role level.
				DataView exclusion = new DataView(_enquiry.Tables[Table_Question]);
				if (Mode == EnquiryMode.Add)
					exclusion.RowFilter = "quadd=false"; //queditseclevel
				else if (Mode == EnquiryMode.Edit)
					exclusion.RowFilter = "quedit=false"; //queditseclevel
				else 
					exclusion.RowFilter = "qusearch=false";

				for (int ctr = (exclusion.Count - 1); ctr > -1; ctr--)
				{
					exclusion[ctr].Delete();
				}
				_enquiry.Tables[Table_Question].AcceptChanges();
			}

			//Builds the actual questions and gets the actual data.
			GetData();

			if (this.InDesignMode == false)
			{
				// Loop through the pages and remove any that failed the conditions
				for (int i= this.Source.Tables[Table_Page].Rows.Count-1; i > -1; i--)
				{
					DataRow rw = this.Source.Tables[Table_Page].Rows[i];
					if (Convert.ToString(rw["pgeCondition"]) != "")
					{
						string[] conditions = Convert.ToString(rw["pgeCondition"]).Split(Environment.NewLine.ToCharArray());
						if (Session.CurrentSession.ValidateConditional(this.Object,conditions) == false)
							rw.Delete();		
					}
					try
					{
						if (Convert.ToString(rw["pgeRole"]) != "")
						{
							if (Session.CurrentSession.CurrentUser.IsInRoles(Convert.ToString(rw["pgeRole"]).Split(",".ToCharArray())) == false)
								rw.Delete();		
						}
					}
					catch
					{}
				}
				this.Source.Tables[Table_Page].AcceptChanges();

				// Loop through the questions and remove any that failed the conditions
				for (int i= this.Source.Tables[Table_Question].Rows.Count-1; i > -1; i--)
				{
					DataRow rw = this.Source.Tables[Table_Question].Rows[i];
					if (Convert.ToString(rw["quCondition"]) != "")
					{
						string[] conditions = Convert.ToString(rw["quCondition"]).Split(Environment.NewLine.ToCharArray());
						if (Session.CurrentSession.ValidateConditional(this.Object,conditions) == false)
							rw.Delete();		
					}
				}
				this.Source.Tables[Table_Question].AcceptChanges();
			}

			
			//Reference any script that maybe available to the enquiry.
			if (HasScript || this.InDesignMode)
			{
				try
				{
					Script.Load(this.InDesignMode);
				}
				catch (Exception ex)
				{
					if (this.InDesignMode)
						AddError("BAL.ScriptGen.Load", "ERROR", String.Format(ex.Message));
					else
						throw ex;
				}
				EnquiryScriptType enqscr =  Script.Scriptlet as EnquiryScriptType;
				if (enqscr != null)
				{
					enqscr.SetEnquiryObject(this);
				}
			}

			_session.Connection.Disconnect(true);
		}

		/// <summary>
		/// Changes the currently set parameters without refetching the whole enquiry 
		/// form.
		/// </summary>
		/// <param name="mode">Changes the edit mode of the form.</param>
		/// <param name="param">The new parameters to be replaced.</param>
		public void ChangeParameters(EnquiryMode mode, Common.KeyValueCollection param)
		{
			_session.CheckLoggedIn();

			base.ChangeParameters(param);

			_obj = null;
            _props = null;

			if (_mode != mode)
			{
				Mode = mode;
			}
			else
				GetData();
		}


		/// <summary>
		/// Saves /Persists the designed enquiry form to the database.  
		/// This is different to update, update updates the actual data.
		/// This is a Design Mode Only method.
		/// </summary>
		public void Save()
		{
			_session.CheckLoggedIn();

			if (InDesignMode)
			{
				
				if (Convert.ToString(_enquiry.Tables[Table_Header].Rows[0]["enqCode"]) == "")
					throw new EnquiryException(HelpIndexes.EnquiryNoCode);

				string where = " where enqid = " + Convert.ToString(this.GetHeaderInfo("enqid"));

				//TODO: Better exception raising if a duplicate enquiry is added.
				string [] tables = new string[3] 
					{
						Table_Header, 
						Table_Page, 
						Table_Question
					};
				string [] sql = new string [3] 
					{
						Sql_Header + where, 
						Sql_Page + where, 
						Sql_Question + where
					};


                _enquiry.Tables[Table_Header].Rows[0]["enqversion"] = IncrementVersionNumber(_enquiry);
                _enquiry.Tables[Table_Header].Rows[0]["updatedby"] = Session.CurrentSession.CurrentUser.ID;
                _enquiry.Tables[Table_Header].Rows[0]["updated"] = DateTime.Now;
                _session.Connection.Update(_enquiry, tables, sql);
				_enquiry.AcceptChanges();

			}
			else
				throw new EnquiryException(HelpIndexes.EnquiryOnlyInDesignMode);
		}

        private long IncrementVersionNumber(DataSet _enquiry)
        {
            long highestversion = GetHighestCheckedInVersion(_enquiry);
            long currentversion = (long)_enquiry.Tables[Table_Header].Rows[0]["enqversion"];

            if (highestversion != 0 && highestversion > currentversion)
                return highestversion + 1;
            else
                return currentversion + 1;
        }

        private long GetHighestCheckedInVersion(DataSet _enquiry)
        {
            string sql = "select MAX(Version) as Version from dbEnquiryVersionData where Code = '" + Convert.ToString(_enquiry.Tables[Table_Header].Rows[0]["enqCode"]) + "'";
            List<IDataParameter> parList = new List<IDataParameter>();
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            DataTable dt = connection.ExecuteSQL(sql, parList);
            connection.Disconnect();
            return ConvertDef.ToInt64(dt.Rows[0]["Version"],0);
        }

		/// <summary>
		/// Saves an existing enquiry form and copies it under a new header leaving
		/// the existing one unchanged.  This is a design mode only command.
		/// </summary>
		/// <param name="newCode">New enquiry header code.</param>
		public void SaveAs(string newCode)
		{
			_session.CheckLoggedIn();
			_code= newCode;
	
			if (InDesignMode)
			{
				ClearContraints();

				Enquiry enq = Enquiry.GetEnquiryInDesign(String.Empty);
					
				DataSet ds = enq.Source;
				
				ClearContraints();

				object num = enq.GetHeaderInfo("enqID");

				//Duplicate the header info.
				_enquiry.Tables[Table_Header].Rows[0]["enqcode"] = newCode;
				foreach (DataColumn col in _enquiry.Tables[Table_Header].Columns)
				{
					if (col.ColumnName.ToLower() != "enqid")
						ds.Tables[Table_Header].Rows[0][col.ColumnName] = _enquiry.Tables[Table_Header].Rows[0][col.ColumnName];
						
				}
				
				//Duplicate the question rows and update the new enquiry id.
				foreach (DataRow row in _enquiry.Tables[Table_Question].Rows)
				{
					row["enqid"] = num;
					ds.Tables[Table_Question].LoadDataRow(row.ItemArray, false);
				}

				//Duplicate the method rows and update the new enquiry id.
				foreach (DataRow row in _enquiry.Tables[Table_Method].Rows)
				{
					row["enqid"] = num;
					ds.Tables[Table_Method].LoadDataRow(row.ItemArray, false);
				}

				//Duplicate the update order rows and update the new enquiry id.
				foreach (DataRow row in _enquiry.Tables[Table_UpdateOrder].Rows)
				{
					row["enqid"] = num;
					ds.Tables[Table_UpdateOrder].LoadDataRow(row.ItemArray, false);
				}

				//Duplicate the page rows and update the new enquiry id.
				foreach (DataRow row in _enquiry.Tables[Table_Page].Rows)
				{
					row["enqid"] = num;
					ds.Tables[Table_Page].LoadDataRow(row.ItemArray, false);
				}

				//Save the new enquiry form.
				enq.Save();

				_enquiry.Dispose();
				_enquiry = enq.Source;
				enq.Dispose();
				enq = null;

				Refresh();

			}
			else
				throw new EnquiryException(HelpIndexes.EnquiryOnlyInDesignMode);
		}

		public DataTable ConvertToV2(string APIPassword)
		{
			if (APIPassword != "FWBS Ltd © 2005")
				throw new Exception("You cannot use this Method");

			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("@Code", System.Data.SqlDbType.NVarChar, 15, Code);
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT quID, quCustom, quFilter FROM dbEnquiryQuestion WHERE NOT quCustom is null","CONVERT",false,paramlist);
			return dt;
		}

		public void ConvertToV2(string APIPassword, DataTable dt)
		{
			if (APIPassword != "FWBS Ltd © 2005")
				throw new Exception("You cannot use this Method");

			Session.CurrentSession.Connection.Update(dt,"SELECT quID, quCustom, quFilter FROM dbEnquiryQuestion");
		}

		/// <summary>
		/// Builds the data table based on the binding mode and edit mode.
		/// </summary>
		private void GetData()
		{
			ApplicationSetting debugset = new ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Debug", "Enabled", "false");
			bool debug = debugset.ToBoolean();

			DataTable questions = GetQUESTIONSTable();
			DataTable data = new DataTable(Table_Data);

			//Retrieve the data for each binding type.
			switch (_binding)
			{
				case EnquiryBinding.Unbound:

					//Loop through each question and build the DATA source structure.
					foreach (DataRow ubrow in questions.Rows)
					{
						//Add the column name into the DATA table by using the unique control name.
						DataColumn ubcol = data.Columns.Add((string)ubrow["quName"]);
					
						//Allow nulls.
						ubcol.AllowDBNull = true;

						//Set the caption of the column to the localized descriptive
						//question.  The only place this would be used is if the table
						//is displayed in a table.
						if (SQLRoutines.IsNullString(ubrow["quDesc"]))
							ubcol.Caption = "";
						else
							ubcol.Caption = (string)ubrow["quDesc"];

						//Set the data type of the column to whats stored under the question.
						if (SQLRoutines.IsNullString(ubrow["qutype"]))
							ubrow["qutype"] = "System.String";

                        ubcol.DataType = Session.CurrentSession.TypeManager.Load((string)ubrow["quType"]);

						//If there is a default value then replace any of the parameter
						//constants and convert the type to the typoe it is expecting.
						//Set the columns default value to the converted value.
						if (Convert.ToString(ubrow["quDefault"]).Length > 0)
						{
							try
							{
								string def = ParseString(ubrow["quDefault"].ToString());
                                if (ubcol.DataType == typeof(DateTime))
                                {
                                    DateTime dt = Convert.ToDateTime(def);
                                    if (dt.Kind == DateTimeKind.Unspecified)
                                        dt = DateTime.SpecifyKind(dt,DateTimeKind.Local);
                                    ubcol.DefaultValue = dt;
                                }
                                else
                                {
                                    ubcol.DefaultValue = Convert.ChangeType(def, ubcol.DataType);
                                }

							}
							catch
							{
							}
						}

						//Terminology parse any strings that are to appear to the user.
						ubrow["quDesc"] = _session.Terminology.Parse(ubrow["quDesc"].ToString(), true);
						ubrow["quHelp"] = _session.Terminology.Parse(ubrow["quHelp"].ToString(), true);
						ubrow["qucmddesc"] = _session.Terminology.Parse(ubrow["qucmddesc"].ToString(), true);
						ubrow["qucmdhelp"] = _session.Terminology.Parse(ubrow["qucmdhelp"].ToString(), true);

                        //UTCFIX: DM - 01/12/06 - Make sure the underlying data columns is date time kind is specified.
                        if (ubcol.DataType == typeof(DateTime))
                            ubcol.DateTimeMode = DataSetDateTime.Utc;

						//Retrieve from the database any list that a question uses.
						//This might include for example a list of active OMS users.
						GetDataList(ubrow);
					}

					//Add a default row in either modes.
					data.Rows.Add(data.NewRow());

					break;

				case EnquiryBinding.Bound:

					//Get the underlying data source SQL statement used to update, delete, select
					//or insert.  The command builder in the update command in the data layer will
					//need this.
					string sql = "SELECT " + GetHeaderInfo("enqFields").ToString() + " FROM " + GetHeaderInfo("enqCall").ToString(); 
				
					//On the edit of an enquiry replace sql parameters with the passed arguments.
					//on the where filter of the above select statement given.
					if (_mode == EnquiryMode.Edit)
						sql += ' ' + GetHeaderInfo("enqWhere").ToString();

					//Make sure the call method is the whole sql statement.
					base.Call = sql;
					
					//If the record is beeing added then just get the schema of the database.
					//Otherwise get the data which is being filtered for.
					if (_mode == EnquiryMode.Add)
					{
						data = (DataTable)Run(true);
					}
					else
						data = (DataTable)Run(false);

					data.TableName = Table_Data;

					//Loop through each question and build the DATA source structure.
					//In bound binding, the data table is the actual data returned from
					//ExecuteSQL command above.
					foreach (DataRow brow in questions.Rows)
					{
						DataColumn bcol = null;
							
						//Grab a reference to the column if it already exists, otherwise create the new column.
						//Set the old column name to the caption, for a valid unique control name for the renderer to deal with.
						string fieldName = brow["qufieldname"].ToString();
						if (data.Columns.Contains(fieldName))
						{
							bcol = data.Columns[fieldName];
							bcol.ColumnName = (string)brow["quname"];
							bcol.Caption = fieldName;
						}
						else
						{
							bcol = data.Columns.Add((string)brow["quname"]);
							if (fieldName != string.Empty)
								bcol.Caption = fieldName;
						}

						bcol.AllowDBNull = true;
							
						//If there is a default value then replace any of the parameter
						//constants and convert the type to the type it is expecting.
						//Set the columns default value to the converted value.
						if (Convert.ToString(brow["quDefault"]).Length > 0)
						{
							try
							{
								string def = ParseString(brow["quDefault"].ToString());
								bcol.DefaultValue =  Convert.ChangeType(def, bcol.DataType);
							}
							catch
							{
							}
						}

							
						//Terminology parse any strings that are to appear to the user.
						brow["quDesc"] = _session.Terminology.Parse(brow["quDesc"].ToString(), true);
						brow["quHelp"] = _session.Terminology.Parse(brow["quHelp"].ToString(), true);
						brow["qucmddesc"] = _session.Terminology.Parse(brow["qucmddesc"].ToString(), true);
						brow["qucmdhelp"] = _session.Terminology.Parse(brow["qucmdhelp"].ToString(), true);

						//Retrieve from the database any list that a question uses.
						//This might include for example a list of active OMS users.
						GetDataList(brow);
					}

					//Add a default row in add mode, edit mode was fetched from the database below.
					if (Mode == EnquiryMode.Add) data.Rows.Add(data.NewRow());

							
					break;

				case EnquiryBinding.BusinessMapping:
					
					//Loop through each question and build the DATA source structure based on reflection
					//from the given properties of the object type used.

					//Type of object to construct.
					Type objtype = null;
					

					//If the internal IEnquiryCompatible object has not been already set
					//for editing purposes then construct the object.
					if (_obj == null)
					{
						
						//If the object does not exist create a new object based on the specified type.
						if (SQLRoutines.IsNullString(GetHeaderInfo("enqSource")))
							throw new EnquiryException(HelpIndexes.EnquiryInvalidBusinessObjectType, "NULL");

                        objtype = Session.CurrentSession.TypeManager.Load((string)GetHeaderInfo("enqSource"));
						
						//If the type cannot be automatically from the given type string then
						//raise an exception and allow the user to know.
						if (objtype == null)
							throw new EnquiryException(HelpIndexes.EnquiryInvalidBusinessObjectType, GetHeaderInfo("enqSource").ToString()); 
						
						//In edit mode make sure you specify a constructor that creates your object 
						//with a unique value of a given data type..
						if (_mode == EnquiryMode.Edit)
						{
							base.Parameters = Convert.ToString(GetHeaderInfo("enqparameters"));

							try
							{
								//Cast the object that returns from the invoke into a IEnquiryCompatible object.
								_obj = (IEnquiryCompatible)Run();
                                _props = new EnquiryPropertyCollection(_obj, true);
							}
							catch(Exception ex)
							{
								if (ex.InnerException is OMSException)
									throw ex.InnerException;
								else
									throw new EnquiryException(ex, HelpIndexes.EnquiryConstructorInvokeError, GetHeaderInfo("enqSource").ToString()); 
							}
						
						}
						else
						{
							base.Parameters = "<params/>";
							
							try
							{
								//Cast the object that returns from the invoke into a IEnquiryCompatible object.
								_obj = (IEnquiryCompatible)Run();
                                _props = new EnquiryPropertyCollection(_obj, true);
							}

							catch(Exception ex)
							{
								if (ex.InnerException is OMSException)
									throw ex.InnerException;
								else
									throw new EnquiryException(ex, HelpIndexes.EnquiryConstructorInvokeError, GetHeaderInfo("enqSource").ToString()); 
							}

						}
					}


					//Just incase the object did not need to be created then get the
					//type of the IEnquiryCompatible object anyway.
					objtype = _obj.GetType();

  					//Holds the internal data table of the created object.
					DataTable objdata = _obj.GetDataTable();

					//Loop through each of the questions and build the data table.
					foreach (DataRow bmrow in questions.Rows)
					{	
						//Create the column based on the control name.
						DataColumn bmcol = new DataColumn();
						
						bmcol.ColumnName = (string)bmrow["quName"];

						//Sort out the property that the enquiry is reading from, if it can.
						EnquiryProperty prop = _props[Convert.ToString(bmrow["quproperty"])];

						//If the question is to manipulate a proeprty on the object
						//then make the data type of the column the same as the properties
						//return value, otherwise set the data type to whatever is specified 
						//in the questions table.
						if (SQLRoutines.IsNullString(bmrow["qutype"]))
							bmrow["qutype"] = "System.String";

						if (SQLRoutines.IsNullString(bmrow["quproperty"]))
						{
							try
							{
								if (bmrow["quExtendedData"] == DBNull.Value)
								{
									if (FWBS.Common.Data.SQLRoutines.IsNullString(bmrow["quFieldName"]) == false)
									{
										try
										{
											bmcol.DataType = _obj.GetExtraInfoType((string)bmrow["quFieldName"]);
										}
										catch(Exception ex)
										{
											AddError("BAL.GetData()", "ERROR", String.Format("Control: {0}' Field: '{1}' - Msg: {2}", bmrow["quname"].ToString(), bmrow["qufieldname"].ToString(), ex.Message));
											bmrow.Delete();
											continue;	
										}
									}
								}
								else
								{
									if (_obj is IExtendedDataCompatible)
									{
										if (FWBS.Common.Data.SQLRoutines.IsNullString(bmrow["quFieldName"]) == false)
										{
											try
											{
												bmcol.DataType = ((IExtendedDataCompatible)_obj).ExtendedData[Convert.ToString(bmrow["quExtendedData"])].GetExtendedDataType((string)bmrow["quFieldName"]);
											}
											catch(Exception ex)
											{
												AddError("BAL.GetData()", "ERROR", String.Format("Control: {0}' Field: '{1}' - Msg: {2}", bmrow["quname"].ToString(), bmrow["qufieldname"].ToString(), ex.Message));
												bmrow.Delete();
												continue;	
											}
										}
									}
									else
										AddError("BAL.GetData()", "WARNING", String.Format("Extended data object '{0}' does not exist. The object is not IExtendedDataCompatible.", (string)bmrow["quextendeddata"]));
								}
							}
							catch
							{
                                bmcol.DataType = Session.CurrentSession.TypeManager.Load((string)bmrow["quType"]);
							}
						}
						else
						{
							if (bmrow["quExtendedData"] != DBNull.Value)
							{
								if (_obj is IExtendedDataCompatible)
								{
									//Gets the property of the internal object within the extended data collection.
									ExtendedData ext = ((IExtendedDataCompatible)_obj).ExtendedData [(string)bmrow["quextendeddata"]];
									if (ext.Object != null)
										prop = new EnquiryPropertyCollection(ext.Object, true)[Convert.ToString(bmrow["quproperty"])];
								}
								else
									AddError("BAL.GetData()", "WARNING", String.Format("Extended data object '{0}' does not exist. The object is not IExtendedDataCompatible.", (string)bmrow["quextendeddata"]));
							}
							
							if (prop == null || prop.IsValid == false) 
							{
								AddError("BAL.GetData()", "ERROR", String.Format("Property '{0}' does not exist on control '{1}'.", bmrow["quProperty"].ToString(), bmrow["quname"].ToString()));
								bmrow.Delete();
								continue;							
							}
							else
								bmcol.DataType = prop.PropertyType;

							//If there is no writeable value for the property then make the question read only.
							if (prop == null || prop.IsValid == false || prop.CanWrite == false)
								bmrow["quReadOnly"] = true;

						}
						
						//Set the caption of the column to the localized descriptive
						//question.  The only place this would be used is if the table
						//is displayed in a table.
						if (SQLRoutines.IsNullString(bmrow["quDesc"]))
							bmcol.Caption = "N/A";
						else
							bmcol.Caption = (string)bmrow["quDesc"];
							
						//If there is a default value then replace any of the parameter
						//constants and convert the type to the typoe it is expecting.
						//Set the columns default value to the converted value.
						if (Convert.ToString(bmrow["quDefault"]).Length > 0)
						{
							string def = ParseString(bmrow["quDefault"].ToString());
							try
							{
								bmcol.DefaultValue =  Convert.ChangeType(def, bmcol.DataType);
							}
							catch
							{
								// Problem with DBNULL fix overload to default type
                                bmcol.DataType = typeof(string);
								bmcol.DefaultValue = (string)bmrow["quDefault"];
							}							
						}


						//Terminology parse any strings that are to appear to the user.
						bmrow["quDesc"] = _session.Terminology.Parse(bmrow["quDesc"].ToString(), true);
						bmrow["quHelp"] = _session.Terminology.Parse(bmrow["quHelp"].ToString(), true);
						bmrow["qucmddesc"] = _session.Terminology.Parse(bmrow["qucmddesc"].ToString(), true);
						bmrow["qucmdhelp"] = _session.Terminology.Parse(bmrow["qucmdhelp"].ToString(), true);

                        //UTCFIX: DM - 01/12/06 - Make sure the underlying data columns is date time kind is specified.
                        if (bmcol.DataType == typeof(DateTime))
                            bmcol.DateTimeMode = DataSetDateTime.Utc;

						data.Columns.Add(bmcol);
						//Retrieve from the database any list that a question uses.
						//This might include for example a list of active OMS users.
						GetDataList(bmrow);
					}

					//Accept any dynamic changes to the questions table.
					questions.AcceptChanges();

					//On an edit create a new data row for the data to be filled in, which will be looped 
					//round again to set the information to the object.
					if (_mode == EnquiryMode.Edit)
					{
						//***********************************
						//The reason why this is in a separate loop is because in the initial
						//loop above not all the columns would have been created if a new row to
						//hold the data is added.
						//***********************************
						for (int ctr = 1;  ctr <= objdata.Rows.Count; ctr++)
						{
							//Add a new data row using the created column schema.
							DataRow datrow = data.NewRow();

							//Loop through the questions agin and get the actual data 
							//from the constructed IEnquiryCompatible object.
							foreach (DataRow qrow in questions.Rows)
							{
								try
								{
									//If no fieldname is specified then the question is classed
									//as unbound and no data is retrieved or set later on in the update method.
									if (SQLRoutines.IsNullString(qrow["qufieldname"]) == false || SQLRoutines.IsNullString(qrow["quproperty"]) == false)
									{
										//If no property is specified then get the objects field value.
										if (SQLRoutines.IsNullString(qrow["quproperty"]))
										{
											//Make sure no null values are set, as this causes an error.  The datatable
											//must be expecting DBNull not null.
											object objprop = null;

											//If there is no extended data to look at then get the field value
											//using the IEnquiryCompatible GetExtraInfo method.
											//Otherwise, use the GetExtendedData of the IExtendedDataCompatible interface
											//method.
											if (SQLRoutines.IsNullString(qrow["quextendeddata"]))
											{
												objprop =  _obj.GetExtraInfo((string)qrow["qufieldname"]);
											}
											else
											{
												if (_obj is IExtendedDataCompatible)
												{
													ExtendedData ext = ((IExtendedDataCompatible)_obj).ExtendedData[(string)qrow["quextendeddata"]];
													if (ext != null)
													{
														objprop = ext.GetExtendedData((string)qrow["qufieldname"]);
													}
													else
														AddError("BAL.GetData()", "WARNING", String.Format("Extended data object '{0}' does not exist. The object is not IExtendedDataCompatible.", (string)qrow["quextendeddata"]));
					
												}
												else
													AddError("BAL.GetData()", "WARNING", String.Format("Extended data object '{0}' does not exist. The object is not IExtendedDataCompatible.", (string)qrow["quextendeddata"]));

											}

											//Set the field to the value returned.
											if (objprop != null)
												datrow[(string)qrow["quName"]] = objprop;
										}
										else
										{

											//If the property is going to be used then through reflection on the 
											//IEnquiryCompatible object to get the data needed from the property.
											EnquiryProperty prop = _props[Convert.ToString(qrow["quproperty"])];
											ExtendedData ext = null;

											//If extended data is not specified then get the property from the
											//object itself, otherwise get the object within the extended data
											//collection and get the property of that instead.
											if (!SQLRoutines.IsNullString(qrow["quextendeddata"]))
											{
												if (_obj is IExtendedDataCompatible)
												{
													//Gets the property of the internal object within the extended data collection.
													ext = ((IExtendedDataCompatible)_obj).ExtendedData [(string)qrow["quextendeddata"]];
													if (ext.Object != null)
														prop = new EnquiryPropertyCollection(ext.Object, true)[Convert.ToString(qrow["quproperty"])];
												}
												else
													AddError("BAL.GetData()", "WARNING", String.Format("Extended data object '{0}' does not exist. The object is not IExtendedDataCompatible.", (string)qrow["quextendeddata"]));
											}					
										
											if (ext != null && ext.Object != null)
											{
												//Assign to the property changed event.
												((IEnquiryCompatible)ext.Object).PropertyChanged -= new PropertyChangedEventHandler(this.PropertyChanged);
												((IEnquiryCompatible)ext.Object).PropertyChanged += new PropertyChangedEventHandler(this.PropertyChanged);
											}
		
											//If no property exists then throw up.
											if (prop == null || prop.IsValid == false) 
												throw new EnquiryException(HelpIndexes.EnquiryPropertyInvokeError, qrow["quProperty"].ToString(), GetHeaderInfo("enqSource").ToString(),  _code);
							
											try
											{
												//Again make sure that the value is not null but DBNull, or in this case
												//do not assign it at all.
												object objprop = prop.GetValue();

												if (objprop != null)
													datrow[(string)qrow["quname"]] = objprop;
											}

											catch(Exception ex)
											{
												if (ex.InnerException is OMSException)
													throw ex.InnerException;
												else
													throw new EnquiryException(ex, HelpIndexes.EnquiryPropertyInvokeError, prop.Name, GetHeaderInfo("enqSource").ToString(),  _code);
											}

										}
									}
								}
								catch (Exception ex)
								{
									AddError("BAL.Enquiry.GetData()", "ERROR", ex.Message);
									qrow["qucontrol"] = "Default";
									qrow["qurequired"] = false;
									datrow[(string)qrow["quname"]] = "*** err ***";
								}
							}
							//Make sure the row is added and changes accepted.
							data.Rows.Add(datrow);
						}
						// Moved by Danny this code does not need to be run
						// after every loop
						data.AcceptChanges();

					}
					else if (_mode == EnquiryMode.Add)
					{
						//Add a default row in add mode, edit mode was done above.
						data.Rows.Add(data.NewRow());
					}

                    //Assign to the property changed event.
                    _obj.PropertyChanged -= new PropertyChangedEventHandler(this.PropertyChanged);
                    _obj.PropertyChanged += new PropertyChangedEventHandler(this.PropertyChanged);

					break;
				default:
					goto case EnquiryBinding.Unbound;
			}

			//Make sure that the data table is in the _enquiry dataset.
			data.TableName = Table_Data;

			if (_enquiry.Tables.Contains(Table_Data))
				_enquiry.Tables.Remove(Table_Data);
			
			_enquiry.Tables.Add(data);

			//Assign to the column change event of the data table.
			data.ColumnChanged -= new DataColumnChangeEventHandler(this.DataColumnChanged);
			data.ColumnChanged += new DataColumnChangeEventHandler(this.DataColumnChanged);

			//Raise the underlying data source changed event.
			if (DataChanged != null)
				DataChanged (this, EventArgs.Empty);

		}

	
	

		/// <summary>
		/// This private method loops round each question within the enquiry and
		/// Populates a data table with data for combo boxes and lists to potentially use.
		/// </summary>
		/// <param name="row">Data row that holds all of the relevent question information.</param>
		private void GetDataList(DataRow row)
		{
			GetDataList(row, false);
		}

		/// <summary>
		/// This private method loops round each question within the enquiry and
		/// Populates a data table with data for combo boxes and lists to potentially use.
		/// </summary>
		/// <param name="row">Data row that holds all of the relevent question information.</param>
		/// <param name="force">If true then the data for the list is refetched.</param>
		private void GetDataList(DataRow row, bool force)
		{

			string tableName = Convert.ToString(row["qudatalist"]);

			//If the select / data list field is not empty then fill get the data.
			//Currently only a select statement.
			if (tableName.Length > 0 && InDesignMode==false)
			{
				//Do not rerun the same query if two lists use the same data table.
				if (!_enquiry.Tables.Contains(tableName) || force == true)
				{
					
					try
					{
						//Execute the SQL or stored procedure.
						DataTable qu = null;
						
						SourceEngine.Source src = new SourceEngine.Source(Convert.ToString(row["qusourcetype"]), Convert.ToString(row["qusource"]), Convert.ToString(row["qucall"]), Convert.ToString(row["quparameters"]));
						if (_obj != null)
							src.ChangeParent(_obj);
						else
							src.ChangeParent(Parent);
						src.ChangeParameters(base.ReplacementParameters);
						object ret = src.Run();
						if (ret is DataTable)
						{
							qu = (DataTable)ret;
							qu.TableName = tableName;
							//Parse the display column of thedata table, but only if there is morethan one column.
							if (qu.Columns.Count > 1)
							{
								foreach (DataRow r in qu.Rows)
									r[1] = _session.Terminology.Parse(Convert.ToString(r[1]), true);
							}
						}
						else
							AddError("BAL.GetDataList()", "WARNING", String.Format("Returned data list item was not a data table object but a '{0}'.", ret.GetType().FullName ));
	
											
						//Add the new table to the _enquiry dataset.
						if (_enquiry.Tables.Contains(tableName) && qu != null)
						{
							_enquiry.Tables[tableName].Rows.Clear();
							qu.TableName = tableName;
                            _enquiry.Merge(qu, false,MissingSchemaAction.Ignore);

						}
						else
						{
							if (qu != null)
							{
								_enquiry.Tables.Add(qu);
							}
						}
					}
					catch (System.Data.SqlClient.SqlException ex)
					{
						AddError("BAL.GetDataList()", "ERROR", String.Format(ex.Message));
						if (ex.InnerException != null)
							AddError("BAL.GetDataList()", "ERROR", String.Format(ex.InnerException.Message));
					}
					catch(Exception ex)
					{
						AddError("BAL.GetDataList()", "ERROR", String.Format(ex.Message));
					}
				}
			}			
		}


		/// <summary>
		/// Checks to see if the enquiry form is compatible with the specified mode and binding.
		/// </summary>
		private void CheckCompatibilityIssues()
		{
			EnquiryMode modes = (EnquiryMode)Convert.ToInt32(GetHeaderInfo("enqModes").ToString());

			if ((modes | _mode) != modes)
			{
				throw new EnquiryException(HelpIndexes.EnquiryDoesNotSupportMode, _code, _mode.ToString());
			}

			if (EngineVersion > ConvertDef.ToInt64(GetHeaderInfo("enqEngineVersion"),EngineVersion))
			{
				Session.CurrentSession.OnWarning(this, new EnquiryException(HelpIndexes.EnquiryEngineVersionDated, _code));
			}

			if (_binding == EnquiryBinding.Bound && _mode == EnquiryMode.Search)
				throw new EnquiryException(HelpIndexes.EnquiryDoesNotSupportModeAndBinding, _mode.ToString(), _binding.ToString());

			if (_binding == EnquiryBinding.BusinessMapping && _mode == EnquiryMode.Search)
				throw new EnquiryException(HelpIndexes.EnquiryDoesNotSupportModeAndBinding, _mode.ToString(), _binding.ToString());

		}
			  	

		/// <summary>
		/// Captures a property changed event on the current business mapped object.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">Property Changed specific arguments.</param>
		private void PropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			// If enquiry form captures the Form Property Changed Event then update
			
			// The Business Mapped Object is stilled mapped to the Data Table in the enquiry object
			// You must set both the Underling Data and the Propery if you wish to value to be set
			// this only accures within a wizard because of the page changing which reset to the binded
			// data table
			
			if (FormPropertyChanged != null)
			{
				OnFormPropertyChanged(sender,e);
			}
			else
			{
				DataTable questions = GetQUESTIONSTable();
				DataTable data = GetDATATable();
				DataView vw = new DataView(questions);
				vw.RowFilter = "quproperty = '" + e.Property.Replace("'", "''") + "'";
				if (vw.Count > 0)
				{
					foreach (DataRowView row in vw)
					{	
						if (data.Columns.Contains(Convert.ToString(row["quname"])))
						{
							if (e.Value != data.Rows[0][Convert.ToString(row["quname"])])
							{
								data.ColumnChanged -= new DataColumnChangeEventHandler(this.DataColumnChanged);
								data.Rows[0][Convert.ToString(row["quname"])] = e.Value;
							
								data.AcceptChanges();
								data.ColumnChanged += new DataColumnChangeEventHandler(this.DataColumnChanged);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Captures a change to the underlying data tables column change event.  
		/// This will keep the underlying object in synch.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A DataColumnChangeEventArgs that contains the event data.</param>
		private void DataColumnChanged (object sender, DataColumnChangeEventArgs e)
		{
			if (_obj != null)
			{
				DataTable questions = GetQUESTIONSTable();
				DataTable data = GetDATATable();
				DataView vw = new DataView(questions);
				vw.RowFilter = "quname = '" + e.Column.ColumnName + "'";
				if (vw.Count > 0)
				{	
					EnquiryProperty prop = null;
					if (vw[0]["quExtendedData"] != DBNull.Value)
					{
						if (_obj is IExtendedDataCompatible)
						{
							//Gets the property of the internal object within the extended data collection.
							ExtendedData ext = ((IExtendedDataCompatible)_obj).ExtendedData [(string)vw[0]["quextendeddata"]];
							if (ext.Object != null)
								prop = new EnquiryPropertyCollection(ext.Object, true)[Convert.ToString(vw[0]["quproperty"])];
							
							if (prop != null && prop.CanRead && prop.CanWrite)
							{
								object ret = prop.GetValue();
								if (Convert.ToString(ret) != Convert.ToString(e.ProposedValue))
								{
									//DJRM - 07/08/03
									//I added this so that DBNulls can be set to a string property as an empty string.
									if (e.ProposedValue is DBNull)
									{
										if (prop.PropertyType == typeof (string))
										{
											prop.SetValue("");
											return;
										}
									}
									prop.SetValue(e.ProposedValue);
								}
							}
						}
						else
							AddError("BAL.GetData()", "WARNING", String.Format("Extended data object '{0}' does not exist. The object is not IExtendedDataCompatible.", (string)vw[0]["quextendeddata"]));
					}
					else
					{
						prop = _props[Convert.ToString(vw[0]["quProperty"])];
						
                        if (prop != null && prop.CanRead && prop.CanWrite)
						{
							object ret = prop.GetValue();
							if (Convert.ToString(ret) != Convert.ToString(e.ProposedValue))
							{
								//DJRM - 07/08/03
								//I added this so that DBNulls can be set to a string property as an empty string.
								if (e.ProposedValue is DBNull)
								{
									if (prop.PropertyType == typeof(FWBS.OMS.Address))
										return;
									else if (prop.PropertyType == typeof(string))
									{
										prop.SetValue("");
										return;
									}
									else if (prop.PropertyType == typeof(FWBS.Common.TriState))
									{
										//DJRM - 09/09/04
										//I added this to allow for tri-state properties.
										prop.SetValue(TriState.Null);
										return;
									}
								}
								prop.SetValue(e.ProposedValue);
							}
						}
					}
				}
			}
		}

		#endregion

		#region Action Methods
		/// <summary>
		/// Cancels any changes made to the underlying data.
		/// </summary>
		public void Cancel()
		{
			_session.CheckLoggedIn();

			DataTable data = GetDATATable();
			data.RejectChanges();

			if (Binding == EnquiryBinding.BusinessMapping)
			{
				if (_obj != null)
					_obj.Cancel();
			}
		}

		/// <summary>
		/// Forces the form to be refreshed which in turns raises the refresh event.
		/// </summary>
		public void Refresh()
		{
			_session.CheckLoggedIn();


			//Refreshes the underlying business object if in that mode.
			if (Binding == EnquiryBinding.BusinessMapping)
			{
				if (_obj != null)
					_obj.Refresh();
			}
			
			
			GetEnquiryData();
			
			if (Refreshed != null)
				Refreshed(this, EventArgs.Empty);
			
			// Added by Danny but may cause a problem with IsDirty
			GetDATATable().AcceptChanges();
			
		}
	
	
		/// <summary>
		/// Updates the database with the underlying DATA table contents. This also depends
		/// on the binding type.
		/// </summary>
		public void Update()
		{
			_session.CheckLoggedIn();

			DataTable data = GetDATATable();
			DataTable questions = GetQUESTIONSTable();
			
			DataView view = new DataView(questions);
			view.RowFilter = "qurequired = true";
			for (int ctr = 0; ctr < view.Count; ctr++)
			{
				DataRow row = view[ctr].Row;
				if (SQLRoutines.IsNullString(data.Rows[0][row["quname"].ToString()]))
				{
					ValidatedField field = new ValidatedField(row["quname"].ToString(), row["qudesc"].ToString(), Convert.ToString(row["qupage"]));
					throw new EnquiryValidationFieldException(HelpIndexes.EnquiryRequiredField, field);
				}
			}

			CancelEventArgs cancel = new CancelEventArgs(false);
			if (Updating!= null)
				Updating(this, cancel);

			if (cancel.Cancel)
			{
				throw new UpdateCancelledException();
			}

			//Branch to the relevant update method depending on the current binding context.
			try
			{
				switch(_binding)
				{
					case EnquiryBinding.Unbound:
						UpdateUnbound();
						break;
					case EnquiryBinding.Bound:
						UpdateBound();
						break;
					case EnquiryBinding.BusinessMapping:
						UpdateBusinessMapping();
						break;
				}

				//Accept the changes after the database has been successfully updated due to no
				//exceptions having been raised.  Do this just to make sure that all rows are flagged
				//as unchanged so that they are not added or edited again, and to make sure no
				//concurrency errors occur.
				_enquiry.AcceptChanges();

				//Likewise raise the updated event.
				if (Updated != null)
					Updated(this, EventArgs.Empty);


			}

			catch (System.Data.SqlClient.SqlException ex)
			{
				//Catch any duplicate keys by raising the custom exception below, looping through
				//all of the fields which match any conflicting constraints, but only if they are unique.


				if (data.Rows.Count > 0)
				{
					string err = ex.Message;
					ValidatedField 	field = null;

					foreach (DataRow row in questions.Rows)
					{

						if ((bool)row["quUnique"])
						{
							field = new ValidatedField(row["quname"].ToString(), row["qudesc"].ToString(), Convert.ToString(row["qupage"]));
						}
						
						if ((err.IndexOf(row["quConstraint"].ToString()) > -1 ) && (row["quConstraint"].ToString().Length > 0))
						{
							field = null;
							field = new ValidatedField(row["quname"].ToString(), row["qudesc"].ToString(), Convert.ToString(row["qupage"]));
							break;
						}

					}

					if (field != null) throw new EnquiryValidationFieldException(ex,HelpIndexes.EnquiryDuplicateKey, field);
				}

				throw ex;
			}

			catch (Exception ex)
			{
				//Make sure any other exception are passed through the stack.
				throw ex;
			}
		
			
		}

		/// <summary>
		/// Update routine for the unbound enquiry.  This should be logical as there is no
		/// data to update within the database.
		/// </summary>
		private void UpdateUnbound()
		{
			//Nothing needs to be done at the moment, but potentially any update logic can
			//go here.
			DataTable data = GetDATATable();
			AddError("BAL.Enquiry.UpdateUnbound()", "INFO", String.Format("Data Row Count: {0}", data.Rows.Count.ToString()));
			if (_offline)
				AddError("BAL.Enquiry.UpdateUnbound()", "INFO", "OFFLINE");
		}

		/// <summary>
		/// Update routine for the directly bound enquiries.  This could include multiple tables.
		/// </summary>
		private void UpdateBound()
		{
			DataTable data = GetDATATable();
			DataTable questions = GetQUESTIONSTable();

			AddError("BAL.Enquiry.UpdateBound()", "INFO", String.Format("Data Row Count: {0}", data.Rows.Count.ToString()));

			//Loop through each field and rename all of the data columns names back
			//to the original field name.  If there were any unboud fields then remove 
			//them from the update procedure.
			DataTable data2 = data.Copy();
			foreach (DataRow row in questions.Rows)
			{
				if (SQLRoutines.IsNullString(row["qufieldname"]))
					data2.Columns.Remove(row["quname"].ToString());
			}
			foreach (DataColumn col in data2.Columns)
			{
				col.ColumnName = col.Caption;
			}

			if (_offline == false)
			{
				try
				{
                    if (this.Mode == EnquiryMode.Add)
                    {
                        FWBS.OMS.Data.Connection cnn = (FWBS.OMS.Data.Connection)base.Connector;
                        string sql = "SELECT " + GetHeaderInfo("enqFields").ToString() + " FROM " + GetHeaderInfo("enqCall").ToString();
                        cnn.Update(data2, sql);
                    }

                    else
                    {
                        FWBS.OMS.Data.Connection cnn = (FWBS.OMS.Data.Connection)base.Connector;
                        cnn.Update(data2.Rows[0], GetHeaderInfo("enqCall").ToString());
                    }



                

				}
				catch (Exception e)
				{
					throw e;
				}
				finally
				{
					data2.Dispose();
					data2 = null;
				}
			}
			else
				AddError("BAL.Enquiry.UpdateBound()", "INFO", "OFFLINE");

		}

		
		/// <summary>
		/// Update routine for the Business logic mapped enquiries.  This will update the
		/// internal dataset of the mapped object.
		/// </summary>
		private void UpdateBusinessMapping()
		{

			//Get the updateable data set then run the update method of the business object.
			DataTable data = GetDATATable();
			DataTable questions = GetQUESTIONSTable();
            try
            {
                //Un-assign the property changed event.
                _obj.PropertyChanged -= new PropertyChangedEventHandler(this.PropertyChanged);

                //Only allow one add or edit at a time due to the direct mapping to the business object.
                if (data.Rows.Count < 1)
                    throw new EnquiryException(HelpIndexes.EnquiryNoUnderlyingData, _code);

                if (data.Rows.Count > 1)
                    AddError("BAL.Enquiry.UpdateBusinessMapping()", "WARNING", String.Format("There is more than one record within the unserlying DATA table.  These will be ignored as only one record applies to Business Mapping binding types."));

                AddError("BAL.Enquiry.UpdateBusinessMapping()", "INFO", String.Format("Data Row Count: {0}", data.Rows.Count.ToString()));


                //Get the first row, ignore any others.
                DataRow dat = data.Rows[0];


                if (_obj == null)
                    throw new EnquiryException(HelpIndexes.EnquiryNullBusinessObject, _code);

                //Get the type of the current referenced object.
                Type objType = _obj.GetType();

                if (objType == null)
                    throw new EnquiryException(HelpIndexes.EnquiryInvalidBusinessObjectType, GetHeaderInfo("enqSource").ToString());


                //Loop through each of the questions and invoke the object property or SetExtraInfo method depending on what it
                //can and cant do.
                foreach (DataRow qrow in questions.Rows)
                {

                    EnquiryProperty prop = null;

                    //If the value is IUpdateable then update the object.
                    object VALUE = dat[(string)qrow["quName"]];

                    //The following Equals method occured when using the Add Address wizard
                    //where a property of the addres object (AddressAsFormat) self refers 
                    //to the object being manipulated.
                    if (VALUE is IUpdateable && !VALUE.Equals(_obj))
                        ((IUpdateable)VALUE).Update();

                    //If the field name is not set then it is classed as unbound
                    //and does not need to be set.
                    if (SQLRoutines.IsNullString(qrow["qufieldname"]) == false || SQLRoutines.IsNullString(qrow["quproperty"]) == false)
                    {
                        //If the property sttribute is not set then set the data through
                        //the field name.
                        if (SQLRoutines.IsNullString(qrow["quProperty"]))
                        {
                            //If the extended data attribute is not set then use the SetExtraInfo
                            //method of the IEnquiryCompatible object.
                            //Otherwise, use the SetExtendedData method of the IExtendedDataCompatible
                            //object to set the data by field name.
                            if (SQLRoutines.IsNullString(qrow["quextendeddata"]))
                            {
                                _obj.SetExtraInfo((string)qrow["qufieldName"], VALUE);
                            }
                            else
                            {
                                if (_obj is IExtendedDataCompatible)
                                    ((IExtendedDataCompatible)_obj).ExtendedData[(string)qrow["quextendeddata"]].SetExtendedData((string)qrow["qufieldname"], VALUE);
                                else
                                    AddError("BAL.UpdateBusinessMapping()", "WARNING", String.Format("Extended data object '{0}' does not exist. The object is not IExtendedDataCompatible.", (string)qrow["quextendeddata"]));

                            }
                        }
                        else
                        {
                            //If a property name is specified then follow the following code.

                            ExtendedData ext = null;

                            //If the extended data attribute is set then get the property of
                            //the internal object within the extended data collection. Otherwise,
                            //use reflection to get the property of the IEnquiryCompatible object.
                            if (SQLRoutines.IsNullString(qrow["quextendeddata"]))
                            {
                                prop = _props[Convert.ToString(qrow["quProperty"])];
                            }
                            else
                            {
                                if (_obj is IExtendedDataCompatible)
                                {
                                    ext = ((IExtendedDataCompatible)_obj).ExtendedData[(string)qrow["quextendeddata"]];
                                    if (ext.Object != null)
                                        prop = new EnquiryPropertyCollection(ext.Object, true)[Convert.ToString(qrow["quproperty"])];
                                }
                                else
                                    AddError("BAL.UpdateBusinessMapping()", "WARNING", String.Format("Extended data object '{0}' does not exist. The object is not IExtendedDataCompatible.", (string)qrow["quextendeddata"]));

                            }

                            if (prop == null)
                            {
                                AddError("BAL.UpdateBusinessMapping()", "ERROR", String.Format("Property '{0}' does not exist on control '{1}'.", qrow["quProperty"].ToString(), qrow["quname"].ToString()));
                                continue;
                            }

                            //Set the property to the value within the data table, only if property has
                            //a set method.  This is captures on the read as well.  All properties with
                            // no set methods will be made read only to the renderer.
                            if (prop.CanWrite)
                            {
                                try
                                {
                                    //No property will be accepting a DBNull so ignore hte value.

                                    if (VALUE != DBNull.Value)
                                    {
                                        //DJRM - 09/09/04
                                        //I added to make sure that if the property type is a TriState, convert the number to the type.
                                        if (prop.PropertyType == typeof(FWBS.Common.TriState))
                                            VALUE = (TriState)Enum.ToObject(typeof(TriState), VALUE);

                                        prop.SetValue(VALUE);
                                    }
                                }

                                catch (Exception ex)
                                {
                                    if (ex.InnerException is OMSException)
                                        throw ex.InnerException;
                                    else
                                        throw new EnquiryException(ex, HelpIndexes.EnquiryPropertyInvokeError, prop.Name, GetHeaderInfo("enqSource").ToString(), _code);
                                }
                            }
                        }

                    }
                }


                //Run all the methods.
                //Make sure that the methods table exists within the data set.
                if (_enquiry.Tables.Contains(Table_Method))
                {
                    //Loop through each corresponding method for the enquiry form.
                    foreach (DataRow mrow in _enquiry.Tables[Table_Method].Rows)
                    {
                        SourceEngine.InstanceMethodSource method = new SourceEngine.InstanceMethodSource(_obj, Convert.ToString(mrow["enqmethod"]), Convert.ToString(mrow["enqparameters"]));
                        method.ParameterHandler = new SourceEngine.SetParameterHandler(this.MethodParameterHandler);

                        try
                        {
                            method.Run();
                        }
                        catch (SourceEngine.SourceException srcex)
                        {
                            throw new EnquiryException(srcex, HelpIndexes.EnquiryMethodInvokeError, Convert.ToString(mrow["enqmethod"]), GetHeaderInfo("enqSource").ToString(), _code);

                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException is OMSException)
                                throw ex.InnerException;
                            else
                                throw new EnquiryException(ex, HelpIndexes.EnquiryMethodInvokeError, Convert.ToString(mrow["enqmethod"]), GetHeaderInfo("enqSource").ToString(), _code);
                        }


                    }
                }

                //Update the object, again this method is expose through the IEnquiryCompatible interface.
                //Allow any underlying exceptions to 
                if (_offline == false)
                {
                    _obj.Update();

                    var file = _obj as OMSFile;
                    if (file != null)
                    {
                        foreach (Associate item in file.Associates)
                        {
                            var associate = item.Key.ToString();
                            ExtendedDataList extData;
                            if (file.AssociateExtendedData.TryGetValue(associate, out extData))
                            {
                                item.UpdateExtendedData(extData, item.ID);
                                file.AssociateExtendedData.Remove(associate);
                            }
                        }
                    }
                }
                else
                    AddError("BAL.Enquiry.UpdateBusinessMapping()", "INFO", "OFFLINE");
            }
            finally
            {
                //Assign the property changed event.
                _obj.PropertyChanged -= new PropertyChangedEventHandler(this.PropertyChanged);
                _obj.PropertyChanged += new PropertyChangedEventHandler(this.PropertyChanged);
            }	


			//}

			//return null;
			
		}
		
		/// <summary>
		/// A method which must be implemented so that each parameter in the paramater
		/// list has its value populated.
		/// </summary>
		/// <param name="name">The name of the parameter being set.</param>
		/// <param name="val">The value to be returned for the parameter listing.</param>
		private void MethodParameterHandler(string name, out object val)
		{
			val = GetDATATable().Rows[0][name];
		}

		#endregion

		#region Tracing

		public void AddError(string source, string level, string message)
		{
			if (_enquiry != null)
			{
				DataTable log = null;
				if (_enquiry.Tables.Contains("ERRORS") == false)
				{
					log = new DataTable("ERRORS");
					log.Columns.Add("Time");
					log.Columns.Add("source");
					log.Columns.Add("level");
					log.Columns.Add("message");
					_enquiry.Tables.Add(log);
				}
				else
					log = _enquiry.Tables["ERRORS"];

                //UTCFIX: DM - 30/11/06 - Used by local computer only.  Need it like this aswell.
				log.Rows.Add(new object[4]{System.DateTime.Now.ToLongTimeString(), source, level, message});
			}
			else
			{
				switch (level.ToUpper())
				{
					case "VERBOSE":
						Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, message, source);
						break;
					case "INFO":
						Trace.WriteLineIf(Global.LogSwitch.TraceInfo, message, source);
						break;
					case "ERROR":
						Trace.WriteLineIf(Global.LogSwitch.TraceError, message, source);
						break;
					case "WARNING":
						Trace.WriteLineIf(Global.LogSwitch.TraceWarning, message, source);
						break;
					default:
						goto case "VERBOSE";

				}
				
			}

		}

		#endregion

		#region Get Enquiry Information and Tables

		/// <summary>
		/// Returns a specified fields contents on the enquiry header information.
		/// </summary>
		/// <param name="fieldName">Specified field name.</param>
		/// <returns>Any object type.</returns>
		private object GetHeaderInfo(string fieldName)
		{
			if (_enquiry == null)
				throw new EnquiryException(HelpIndexes.EnquiryHeaderCorrupt, _code);
			if ((!_enquiry.Tables.Contains(Table_Header)) || (_enquiry.Tables[Table_Header].Rows.Count < 1))
				throw new EnquiryException(HelpIndexes.EnquiryHeaderCorrupt, _code);

			if (!_enquiry.Tables[Table_Header].Columns.Contains(fieldName))
				throw new EnquiryException(HelpIndexes.EnquiryInvalidField, fieldName, _code);
			
			return _enquiry.Tables[Table_Header].Rows[0][fieldName];
		}


		/// <summary>
		/// Returns a reference to the DATA table within the enquiry data set.
		/// </summary>
		/// <returns>A data table object.</returns>
		private DataTable GetDATATable()
		{
			if (_enquiry == null)
				throw new EnquiryException(HelpIndexes.EnquiryHeaderCorrupt, _code);
			if (!_enquiry.Tables.Contains(Table_Data))
				throw new EnquiryException(HelpIndexes.EnquiryNoDataSection, _code);
			
			return _enquiry.Tables[Table_Data];
		}

		/// <summary>
		/// Returns a reference to the QUESTION table within the enquiry data set.
		/// </summary>
		/// <returns>A data table object.</returns>
		private DataTable GetQUESTIONSTable()
		{
			if (_enquiry == null)
				throw new EnquiryException(HelpIndexes.EnquiryHeaderCorrupt, _code);

			if (InDesignMode)
			{
				if (!_enquiry.Tables.Contains(Table_Question))
					throw new EnquiryException(HelpIndexes.EnquiryNoQuestions, _code);
			}
			else
			{
				if ((!_enquiry.Tables.Contains(Table_Question))  || (_enquiry.Tables[Table_Question].Rows.Count < 1))
					throw new EnquiryException(HelpIndexes.EnquiryNoQuestions, _code);
			}
			return _enquiry.Tables[Table_Question];
		}



		#endregion

		#region Static Methods
		/// <summary>
		/// Gets the Version of the Enquiry Form
		/// </summary>
		/// <param name="EnquiryForm">The Enquiry Form Name</param>
		/// <returns>The Version</returns>
		public static long GetEnquiryFormVersion(string EnquiryForm)
		{
			try
			{
				Enquiry enq = Enquiry.GetEnquiry(EnquiryForm,Session.OMS,EnquiryEngine.EnquiryMode.Search,true,new KeyValueCollection());
				long v = enq.Version;
				enq.Dispose();
				return v;
			}
			catch
			{
				return -1;
			}

		}

		/// <summary>
		/// Gets a list of valid Static methods from
		/// a specified object type. 
		/// </summary>
		/// <param name="type">Fully qualified assembly type string.</param>
		/// <returns>MethodInfo array.</returns>
		public static ArrayList GetObjectStaticMethods(string type)
		{
			Type t;
            t = Session.CurrentSession.TypeManager.Load(type);
			return GetObjectStaticMethods(t);
		}

		/// <summary>
		/// Gets a list of Static methods from
		/// a specified object type.  
		/// </summary>
		/// <param name="type">Type object.</param>
		/// <returns>Method info object array.</returns>
		public static ArrayList GetObjectStaticMethods(Type type)
		{
			ArrayList ret = new ArrayList();
			MemberInfo [] members = type.FindMembers(MemberTypes.Method, BindingFlags.Static | BindingFlags.Public, null, null);
			
			foreach(MethodInfo mh in members)
			{
				if (mh.ReturnType == typeof(DataTable))
				{
					ParameterInfo[] parameter = mh.GetParameters();
					string _display = mh.Name + "(";
					foreach(ParameterInfo pm in parameter)
						_display = _display + pm.Name + ", ";
					if (_display.EndsWith(", ")) _display = _display.Substring(0,_display.Length-2);
					_display = _display + ")";
					ret.Add(new ReflectionMethods(mh.Name, _display, mh));
				}
			}
			return ret;
		}

		/// <summary>
		/// Gets a list of control names of an enquiry form.
		/// </summary>
		/// <param name="enquiryForm">Enquiry form code.</param>
		/// <param name="parameterNotation">Include the % parameter notation.</param>
		/// <returns>Lists the control names in a data table.</returns>
		public static DataTable GetControlNames(string enquiryForm, bool parameterNotation)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("CODE", SqlDbType.NVarChar, 15, enquiryForm);
			DataTable dt = null;
			if (parameterNotation) 
				dt = Session.CurrentSession.Connection.ExecuteSQLTable("select '%' + Q.quname + '%' from dbenquiryquestion Q inner join dbenquiry E on E.enqid = q.enqid where E.enqcode = @CODE", "CONTROLS", paramlist);
			else
				dt = Session.CurrentSession.Connection.ExecuteSQLTable("select Q.quname from dbenquiryquestion Q inner join dbenquiry E on E.enqid = q.enqid where E.enqcode = @CODE", "CONTROLS", paramlist);
			return dt;
		}

		
		/// <summary>
		/// Gets a list of Tables
		/// </summary>
		/// <returns>Lists the Table names in a data table.</returns>
		public static DataTable GetTableNames()
		{
			Session.CurrentSession.CheckLoggedIn();
			DataTable dt = null;
			IDataParameter [] paramlist = new IDataParameter[0];
			dt = Session.CurrentSession.Connection.ExecuteSQLTable("select [name] as tblname from sysobjects where type = 'U' order by name","TABLES",paramlist);
			return dt;
		}
		
		/// <summary>
		/// Returns a type array of all the exported types which could potentially be
		/// an IEnquiryCompatible object.
		/// </summary>
		/// <returns>Type array list.</returns>
		public static Type[] GetObjects()
		{
			return GetObjects(false);
		}
			
		/// <summary>
		/// Returns a type array of all the exported types which could potentially be
		/// an IEnquiryCompatible object.
		/// </summary>
		/// <param name="ShowAll">Return all Objects.</param>
		/// <returns>Type array list.</returns>
		public static Type[] GetObjects(bool ShowAll)
		{
			Type[] types = Assembly.GetExecutingAssembly().GetExportedTypes();
			Type[] ret;
			System.Collections.ArrayList list = new System.Collections.ArrayList(types.Length);

			foreach (Type t in types)
			{
				if (t.GetInterface(typeof(IEnquiryCompatible).Name, true) != null && t.IsClass || ShowAll)
				{
					list.Add(t);
				}
			}

			list.TrimToSize();
			ret = (Type[])list.ToArray(typeof(Type));
			return ret;
		}

		/// <summary>
		/// Gets a list of valid enquiry compatible constructors from
		/// a specified object type.  The specified type must be IEnquiryCompatible.
		/// </summary>
		/// <param name="type">Fully qualified assembly type string.</param>
		/// <returns>Constructor Info array.</returns>
		public static ArrayList GetObjectConstructors(string type)
		{
			Type t = Session.CurrentSession.TypeManager.Load(type);
			return GetObjectConstructors(t);
		}

		/// <summary>
		/// Gets a list of valid enquiry compatible constructors from
		/// a specified object type.  The specified type must be IEnquiryCompatible.
		/// </summary>
		/// <param name="type">Type object.</param>
		/// <returns>Constructor Info array.</returns>
		public static ArrayList GetObjectConstructors(Type type)
		{
			if (type.GetInterface(typeof(IEnquiryCompatible).Name, true) == null)
			{
				throw new EnquiryException(HelpIndexes.EnquiryInvalidBusinessObjectType, type.FullName);
			}
			ArrayList ret = new ArrayList();
			MemberInfo[] members = type.FindMembers(MemberTypes.Constructor, MemberBinding, new MemberFilter(MemberFilter), false);
			ConstructorInfo[] cons = new ConstructorInfo[members.Length]; 
			members.CopyTo(cons, 0);
			foreach(ConstructorInfo ci in cons)
			{
				ParameterInfo[] parameter = ci.GetParameters();
				string _display = type.Name + "(";
				foreach(ParameterInfo pm in parameter)
					_display = _display + pm.ParameterType.ToString() + " " + pm.Name + ", ";
				if (_display.EndsWith(", ")) _display = _display.Substring(0,_display.Length-2);
				_display = _display + ")";
				ret.Add(new ReflectionMethods(ci.Name, _display, ci));
			}


			return ret;
		}

		/// <summary>
		/// Gets a list of valid enquiry compatible properties from
		/// a specified object type.  The specified type must be IEnquiryCompatible.
		/// </summary>
		/// <param name="type">Fully qualified assembly type string.</param>
		/// <returns>Property Info array.</returns>
        public static EnquiryPropertyCollection GetObjectProperties(string type)
		{
            Type t = Session.CurrentSession.TypeManager.Load(type);
			return GetObjectProperties(t);
		}

		/// <summary>
		/// Gets a list of valid enquiry compatible properties from
		/// a specified object type.  The specified type must be IEnquiryCompatible.
		/// </summary>
		/// <param name="type">Type object.</param>
		/// <returns>Property Info array.</returns>
		public static EnquiryPropertyCollection GetObjectProperties(Type type)
		{
			if (type.GetInterface(typeof(IEnquiryCompatible).Name, true) == null)
			{
				throw new EnquiryException(HelpIndexes.EnquiryInvalidBusinessObjectType, type.FullName);
			}

            return new EnquiryPropertyCollection(type, false);

		}


		/// <summary>
		/// Gets a list of valid enquiry compatible methods from
		/// a specified object type.  The specified type must be IEnquiryCompatible.
		/// </summary>
		/// <param name="type">Fully qualified assembly type string.</param>
		/// <returns>MethodInfo array.</returns>
		public static ArrayList GetObjectMethods(string type)
		{
            Type t = Session.CurrentSession.TypeManager.Load(type);
			return GetObjectMethods(t);
		}

		/// <summary>
		/// Gets a list of valid enquiry compatible methods from
		/// a specified object type.  The specified type must be IEnquiryCompatible.
		/// </summary>
		/// <param name="type">Type object.</param>
		/// <returns>Method info object array.</returns>
		public static ArrayList GetObjectMethods(Type type)
		{
			if (type.GetInterface(typeof(IEnquiryCompatible).Name, true) == null)
			{
				throw new EnquiryException(HelpIndexes.EnquiryInvalidBusinessObjectType, type.FullName);
			}
			
			ArrayList ret = new ArrayList();

			MemberInfo [] members = type.FindMembers(MemberTypes.Method, MemberBinding, new MemberFilter(MemberFilter), false);
			MethodInfo [] methods = new MethodInfo[members.Length]; 
			members.CopyTo(methods, 0);

			foreach(MethodInfo mh in members)
			{
				if (mh.ReturnType == typeof(DataTable))
				{
					ParameterInfo[] parameter = mh.GetParameters();
					string _display = mh.Name + "(";
					foreach(ParameterInfo pm in parameter)
						_display = _display + pm.Name + ", ";
					if (_display.EndsWith(", ")) _display = _display.Substring(0,_display.Length-2);
					_display = _display + ")";
					ret.Add(new ReflectionMethods(mh.Name, _display, mh));
				}
			}
			

			return ret;
		}

		/// <summary>
		/// Gets the parameters associated to the method passed.
		/// </summary>
		/// <param name="method">Method object to get parameter signatures from.</param>
		/// <returns>Array of parameter information.</returns>
		public static ParameterInfo[] GetMethodParameters(MethodInfo method)
		{
			return method.GetParameters();
		}

        /// <summary>
        /// Gets the parameters associated to the constructor passed.
        /// </summary>
        /// <param name="constructor">Method object to get parameter signatures from.</param>
        /// <returns>Array of parameter information.</returns>
        public static ParameterInfo[] GetMethodParameters(ConstructorInfo constructor)
		{
			return constructor.GetParameters();
		}

		/// <summary>
		/// This method is called to evaluate each member passedso that only enquiry
		/// compatible members are exposed in the list.
		/// </summary>
		/// <param name="m">Member info object.</param>
		/// <param name="filterCriteria">Filter criteria.</param>
		/// <returns>Returns true if the filter succeeds (if enquiry compatible).</returns>
		internal static bool MemberFilter (MemberInfo m, object filterCriteria)
		{
			object[] objattrs =  m.GetCustomAttributes(false);
            Attribute[] attrs = Array.ConvertAll<object, Attribute>(objattrs, new Converter<object, Attribute>(delegate(object attr)
            {
                return (Attribute)attr;
            }));
            return HasEnquiryUsageAttribute(attrs) || (bool)filterCriteria;
		}

        internal static bool HasEnquiryUsageAttribute(Attribute[] attrs)
        {
            if (attrs == null)
                return false;

            foreach (Attribute attr in attrs)
            {
                EnquiryUsageAttribute attrenq = attr as EnquiryUsageAttribute;
                if (attrenq != null)
                    return attrenq.Usable;
            }

            return false;
        }

		/// <summary>
		/// Has the Enquiry Form Extended Data Questions
		/// </summary>
		/// <param name="Code">The Enquiry Code</param>
		/// <returns>Returns a Data Table of Extended Data Code</returns>
		public static DataTable HasExtendedData(string Code)
		{
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("@Code", System.Data.SqlDbType.NVarChar, 15, Code);
			return Session.CurrentSession.Connection.ExecuteSQLTable("SELECT dbEnquiryQuestion.quExtendedData FROM dbEnquiryQuestion INNER JOIN dbEnquiry ON dbEnquiryQuestion.enqID = dbEnquiry.enqID WHERE (dbEnquiry.enqCode = @Code) GROUP BY dbEnquiryQuestion.quExtendedData HAVING (NOT (dbEnquiryQuestion.quExtendedData IS NULL))","EXTENDED",false,paramlist);
		}

		public static bool Exists(string Code)
		{
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("@Code", System.Data.SqlDbType.NVarChar, 15, Code);
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbEnquiry WHERE (dbEnquiry.enqCode = @Code)","EXISTS",false,paramlist);
			return dt.Rows.Count > 0;
		}
		#endregion

		#region Designer Public

		public void Designer_RefreshQuestions()
		{
			try
			{
				CodeLookup cp = new CodeLookup("ENQQUESTION","");
				DataView _liverows = new DataView(_enquiry.Tables[Table_Question]);
				_liverows.RowStateFilter = DataViewRowState.CurrentRows;
				foreach(DataRowView dt in _liverows)
				{
					if (Convert.ToString(dt["quCode"]) != "")
					{
						try
						{
							cp.FindCodeLookup("ENQQUESTION",Convert.ToString(dt["quCode"]));
							dt["quDesc"] = Session.CurrentSession.Terminology.Parse(cp[System.Threading.Thread.CurrentThread.CurrentUICulture.Name].Caption,true);
							dt["quHelp"] = cp[System.Threading.Thread.CurrentThread.CurrentUICulture.Name].HelpText;
						}
						catch (Exception ex)
						{
							Trace.WriteLineIf(Global.LogSwitch.TraceWarning, ex.Message, "BAL.Designer_RefreshQuestions()");
						}
					}
				}
			}
			catch{}
		}

		#endregion
	}

	
	/// <summary>
	/// Storage of the Refection Method with Nice Display Name and MethodInfo Object
	/// </summary>
	public class ReflectionMethods
	{
		#region Fields
		private string _methodname = "";
		private string _methoddisplay = "";
		private MethodInfo _method;
		private ConstructorInfo _constuct;
		#endregion

		#region Constructor
		public ReflectionMethods(string MethodName, string MethodDisplay, MethodInfo Method)
		{
			_methodname = MethodName;
			_methoddisplay = MethodDisplay;
			_method = Method;
		}

		public ReflectionMethods(string MethodName, string MethodDisplay, ConstructorInfo Constructor)
		{
			_constuct = Constructor;
			_methoddisplay = MethodDisplay;
			_method = Method;
		}

		#endregion

		#region Properties
		public string MethodName
		{
			get
			{
				return _methodname;
			}
			set
			{
				_methodname = value;
			}
		}

		public string MethodDisplay
		{
			get
			{
				return _methoddisplay;
			}
			set
			{
				_methoddisplay = value;
			}
		}

		public MethodInfo Method
		{
			get
			{
				return _method;
			}
			set
			{
				_method = value;
			}
		}

		public ConstructorInfo Constuctor
		{
			get
			{
				return _constuct;
			}
			set
			{
				_constuct = value;
			}
		}
		#endregion
	}
	

	/// <summary>
	/// Business mapped property changed delegate.
	/// </summary>
	public delegate void PropertyChangedEventHandler (object sender, PropertyChangedEventArgs e);

	/// <summary>
	/// Property changed event arguments.
	/// </summary>
	public class PropertyChangedEventArgs : EventArgs
	{

		private readonly string _property = "";
        private readonly object _previousvalue = null;
		private readonly object _value = null;

		private PropertyChangedEventArgs(){}

        internal PropertyChangedEventArgs(string property, object value) : this(property, Type.Missing, value)
        {
        }

		public PropertyChangedEventArgs (string property, object previousValue, object value)
		{
			_property = property;
            _previousvalue = previousValue;
			_value = value;
		}

		public string Property
		{
			get
			{
				return _property;
			}
		}

		public object Value
		{
			get
			{
				return _value;
			}
		}

        public object PreviousValue
        {
            get
            {
                return _previousvalue;
            }
        }


	}



	public class EnquiryCommand
	{
		protected DataTable _enqcmd;
		protected string _source = "";
		protected string _call = "";
		protected string _parameters = "";

		public EnquiryCommand(string EnquiryCommandName)
		{
			try
			{
				IDataParameter[] paramlist = new IDataParameter[2];
				paramlist[0] = Session.CurrentSession.Connection.AddParameter("@Code", System.Data.SqlDbType.NVarChar, 15, EnquiryCommandName);
				_enqcmd = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbEnquiryCommand where cmdCode = @Code" , "EnqCmd",  paramlist);
				_source = Convert.ToString(_enqcmd.Rows[0]["cmdType"]);
				_call = Convert.ToString(_enqcmd.Rows[0]["cmdMethod"]);
				_parameters = Convert.ToString(_enqcmd.Rows[0]["cmdParameters"]);
			}
			catch (Exception ex)
			{
				throw new OMSException2("ERRECNOTFND","Screen command '%1%' cannot be found","",ex,true,EnquiryCommandName);
			}
		}

		public string Code
		{
			get
			{
				return Convert.ToString(_enqcmd.Rows[0]["cmdCode"]);
			}
		}
		
		public string Type
		{
			get
			{
				return _source;
			}
			set
			{
				_source=value;
				_enqcmd.Rows[0]["cmdType"] = value;
			}
		}

		public string Method
		{
			get
			{
				return _call;
			}
			set
			{
				_enqcmd.Rows[0]["cmdMethod"] = value;
				_call = value;
			}
		}

		public string Parameters
		{
			get
			{
				return _parameters;
			}
			set
			{
				_enqcmd.Rows[0]["cmdParameters"] = value;
				_parameters = value;
			}
		}


		public void Update()
		{
			Session.CurrentSession.Connection.Update(_enqcmd, "select * from dbEnquiryCommand");
		}
	}


	/// <summary>
	/// A DataList class that holds the configuration data.
	/// </summary>
	public class DataLists : SourceEngine.Source, IDisposable
	{
		#region Fields

        protected virtual bool InDesignMode { get { return false; } }
		protected DataTable _searchlisttb = null;
		protected DataTable _data = null;
		protected internal static string TableEdit = "dbEnquiryDataList";
		protected static string sql = "select *, dbo.GetCodeLookupDesc('ENQDATALIST', enqTable, @UI) as enqTableDesc from " + TableEdit;
		protected static string updateablesql = "select * from " + TableEdit;
		/// <summary>
		/// The Data Row that contains the Settings
		/// </summary>
		protected DataRow _dr;
		/// <summary>
		/// Data List Description
		/// </summary>
		protected string _description;
		/// <summary>
		/// The Main Document XML
		/// </summary>
		protected XmlDocument _xmlDParams;
		/// <summary>
		/// The Parameter XMLs
		/// </summary>
		protected XmlNode _xmlParameter;
		/// <summary>
		/// The Original Code to compare against for Renaming
		/// </summary>
		protected string _orgcode = "";
        /// <summary>
        /// The Data List Version number
        /// </summary>
        protected long _version;

		#endregion Fields

		#region Constructors
	
		/// <summary>
		/// Disable the use of the default constructor.  
		/// This object does not need to be created in this way.
		/// </summary>
		public DataLists() : this("")
		{}

		public DataLists(string code) : this (code,false)
		{
			DataListEditorInt();
		}
		
		public DataLists(string code, bool Clone)
		{
			IDataParameter[] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("@Code", System.Data.SqlDbType.NVarChar, 15, code);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 15, Thread.CurrentThread.CurrentCulture.Name);
			_searchlisttb = Session.CurrentSession.Connection.ExecuteSQLTable(sql + " where enqTable = @Code" , TableEdit,  paramlist);
			if (_searchlisttb.Rows.Count==0 && Clone == false)
			{
				_searchlisttb.Rows.Add(_searchlisttb.NewRow());
				_searchlisttb.Rows[0]["enqParameters"] = "<params></params>";
				_searchlisttb.Rows[0]["enqSourceType"] = "OMS";
			}
		}
		#endregion
	
		#region Public Methods

		public DataTable GetTable()
		{
			//A data object that could hold any list compatible data.
			object data = null;
			data = base.Run();
			if (data is DataTable)
			{
				DataTable dt = (DataTable)data;
				dt.TableName = "RESULTS";
				_data = (DataTable)data;
				return _data;
			}
			else
			{
				throw new FWBS.OMS.SearchEngine.SearchException(HelpIndexes.SearchNotCompatibleResultset);
			}
		}


		public virtual void Update()
		{
			if (Convert.ToString(_searchlisttb.Rows[0]["enqTable"]) == "")
				throw new EnquiryException(HelpIndexes.DataListNoCodeSet);

			Session.CurrentSession.Connection.Update(_searchlisttb, updateablesql);
		}


		public virtual void UpdateData()
		{
			if (_data != null && Call.ToUpper().StartsWith("SELECT"))
			{
				Session.CurrentSession.Connection.Update(_data, Call);
			}
		}


		public virtual void UpdateData(string Select)
		{
			if (_data != null && Select.StartsWith("SELECT"))
			{
				Session.CurrentSession.Connection.Update(_data, Select);
			}
		}
		
        #endregion Public Methods

		#region IDisposable Implementation

		/// <summary>
		/// Disposes the object immediately without waiting for the garbage collector.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		/// <summary>
		/// Disposes all internal objects used by this object.
		/// </summary>
		/// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
		void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				//Destroy the underlying data source.
				if (_searchlisttb != null)
				{
					_searchlisttb.Dispose();
					_searchlisttb = null;
				}
			}
		}

	
		#endregion

		#region Static Methods

		/// <summary>
		/// Delete a Search List
		/// </summary>
		/// <param name="Code">Search List Code</param>
		/// <returns>True if Succesful</returns>
		public static bool Delete(string Code)
		{
			Session.CurrentSession.Connection.ExecuteSQL("delete from DBCodeLookup where cdcode = @Code and cdtype = 'ENQDATALIST';delete from dbEnquiryDataList where enqTable = @Code", new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code)});
			return true;
		}

		/// <summary>
		/// Does the Code Exist
		/// </summary>
		/// <returns>Boolean.</returns>
		public static bool Exists(string Code)
		{
			Session.CurrentSession.CheckLoggedIn();
			object val = Session.CurrentSession.Connection.ExecuteSQLScalar("select enqTable from dbEnquiryDataList where enqTable = @Code", new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code)});
			if (val != null)
				return true;
			else
				return false;
		}


		public static DataTable GetDataLists()
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 15, Thread.CurrentThread.CurrentCulture.Name);
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(sql, "DATALISTS", paramlist);
			//Terminology parse each of the items.
			foreach (DataRow row in dt.Rows)
			{
				row["enqTableDesc"] = Session.CurrentSession.Terminology.Parse(row["enqTableDesc"].ToString(), true);
			}
			dt.AcceptChanges();
			
			return dt;
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Retreives the Attribure
		/// </summary>
		/// <param name="Node">The Node to Work On</param>
		/// <param name="Name">The Name of The Attribute</param>
		/// <param name="Value">The Value</param>
		protected void WriteAttribute(XmlNode Node, string Name, string Value)
		{
			try{Node.SelectSingleNode("@" + Name).Value = Value;}
			catch{Node.Attributes.Append(CreateAttribute(Node, Name,Value));}
		}

		/// <summary>
		/// Create the Attribute
		/// </summary>
		/// <param name="Node">The Node to Add the Attribute to</param>
		/// <param name="Name">The Name of the Attribute</param>
		/// <param name="Value">The Value </param>
		/// <returns>The Attribute Just Created</returns>
		protected System.Xml.XmlAttribute CreateAttribute(XmlNode Node, string Name, string Value)
		{
			System.Xml.XmlAttribute n = Node.OwnerDocument.CreateAttribute(Name);
			n.Value = Value;
			return n;
		}

		/// <summary>
		/// Returns the Attribute
		/// </summary>
		/// <param name="Node">The Node to Work On</param>
		/// <param name="Name">The Name of the Attribute</param>
		/// <param name="Value">The Value if it should Return a Error</param>
		/// <returns>The Contents of the Attribute</returns>
		protected string GetAttribute(XmlNode Node, string Name, string Value)
		{
			try
			{
				return Node.SelectSingleNode("@" + Name).Value;
			}
			catch
			{
				WriteAttribute(Node,Name,Value);
				return Value;
			}

		}

		/// <summary>
		/// A Protected Method to allow reuse of multi contructors
		/// </summary>
		protected internal virtual void DataListEditorInt()
		{
			_dr = _searchlisttb.Rows[0];
			_description = Convert.ToString(_dr["enqTableDesc"]);
			_orgcode = Code;
            _version = ConvertDef.ToInt32(_dr["enqDLVersion"],1);

			//
			// XML Parameters
			//
			_xmlDParams = new XmlDocument();
			_xmlDParams.PreserveWhitespace = false;
			_xmlDParams.LoadXml(Convert.ToString(_dr["enqParameters"]));
			_xmlParameter = _xmlDParams.DocumentElement.SelectSingleNode("/params");

			base.Parameters = _xmlDParams.InnerXml;
			base.Call =  Convert.ToString(_dr["enqCall"]);
			base.Src = Convert.ToString(_dr["enqSource"]);
			base.SourceType = (SourceType)Enum.Parse(typeof(SourceType),Convert.ToString(_dr["enqSourceType"]),true);
            if (!InDesignMode)
                base.ReBind();
            else
            {
                try
                {
                    base.ReBind();
                }
                catch
                {
                }
            }
		}

		#endregion Protected Methods

		#region Properties

		/// <summary>
		/// The Short name of the Data List
		/// </summary>
		[LocCategory("Data")]
		[Description("The Code that defines the individual Data List")]
		public string Code
		{
			get
			{
				return Convert.ToString(_dr["enqTable"]);
			}
			set
			{
				if (DataLists.Exists(value))
				{
					if (value != _orgcode)
						throw new ExtendedDataException(HelpIndexes.ExtendedDataCodeAlreadyExists,value);
				}
				else
				{
					if (_dr.RowState == DataRowState.Added)
					{
						_dr["enqTable"] = value;
						Description = _description;
					}
					else
						throw new OMSException2("34001","The Code cannot be changed when set");
				}
			}
		}


		/// <summary>
		/// The Localized Description of the Data List
		/// </summary>
		[LocCategory("Data")]
		[Description("The Description of the Data List")]
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
				if (Code != "")
					FWBS.OMS.CodeLookup.Create("ENQDATALIST",Code,value,"",CodeLookup.DefaultCulture,true,true,true);
			}
		}


        /// <summary>
        /// The Version of the Data List
        /// </summary>
        [LocCategory("Data")]
        [Description("The Version of the Data List")]
        [ReadOnly(true)]
        public long Version
        {
            get
            {
                return _version;
            }
            set
            {
                if (_dr.RowState == DataRowState.Added)
                {
                    _dr["enqDLVersion"] = 1;
                }
                else
                {
                    _version = value;
                }
            }
        }

		#endregion Properties
	}
}
