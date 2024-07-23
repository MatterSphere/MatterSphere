using System;
using System.Data;
using System.Threading;

namespace FWBS.OMS
{
	/// <summary>
	/// Fetches localized resources and messages for the application. This uses the code lookup
	/// engine to find them out.
	/// </summary>
	public class Res
	{
		#region Fields

		/// <summary>
		/// Stores all of the resources into a table.
		/// </summary>
		private DataTable _resourceList;
	
		/// <summary>
		/// Stores all the message based resources.
		/// </summary>
		private DataTable _messageList;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Constructor that stores the resources into a collection..
		/// </summary>
		internal Res()
		{
            Fetch();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets a resource.
		/// </summary>
		/// <param name="resid">Resource identifier.</param>
        /// <param name="defaulttext"></param>
        /// <param name="defaulthelp"></param>
		/// <returns>Returns a resource item structure.</returns>
		public ResourceItem GetResource (string resid, string defaulttext, string defaulthelp)
		{
			return GetResource(resid,defaulttext,defaulthelp, true);
		}

		/// <summary>
		/// Get a message recource.
		/// </summary>
		/// <param name="resid">Resource identifier.</param>
        /// <param name="defaulttext"></param>
        /// <param name="defaulthelp"></param>
		/// <returns>Returns a resource item structure.</returns>
		public ResourceItem GetMessage (string resid, string defaulttext, string defaulthelp)
		{
			return GetMessage(resid,defaulttext,defaulthelp, true);
		}



		/// <summary>
		/// Gets the resource string specified.
		/// </summary>
		/// <param name="resid">Resource ID.</param>
        /// <param name="defaulttext"></param>
        /// <param name="defaulthelp"></param>
		/// <param name="useParser">Uses the terminology parser.</param>
		/// <param name="param">Parameter replacing arguments.</param>
		/// <returns>Need UI string value.</returns>
		public ResourceItem GetResource(string resid, string defaulttext, string defaulthelp, bool useParser, params string [] param)
		{
			return GetResource(_resourceList, resid, defaulttext, defaulthelp, useParser, param);
		}

		/// <summary>
		/// Gets the message string specified.
		/// </summary>
		/// <param name="resid">Resource ID.</param>
        /// <param name="defaulttext"></param>
        /// <param name="defaulthelp"></param>
		/// <param name="useParser">Uses the terminology parser.</param>
		/// <param name="param">Parameter replacing arguments.</param>
		/// <returns>Need UI string value.</returns>
		public ResourceItem GetMessage(string resid, string defaulttext, string defaulthelp, bool useParser, params string [] param)
		{
			return GetResource(_messageList, resid, defaulttext, defaulthelp, useParser, param);
		}

		/// <summary>
		/// Gets the message string specified.
		/// </summary>
		/// <param name="resid">Resource ID.</param>
        /// <param name="defaulttext"></param>
        /// <param name="defaulthelp"></param>
		/// <param name="param">Parameter replacing arguments.</param>
		/// <returns>Need UI string value.</returns>
		public ResourceItem GetMessage(string resid, string defaulttext, string defaulthelp, params string [] param)
		{
			return GetMessage(resid, defaulttext,defaulthelp, true, param);
		}

		/// <summary>
		/// Gets the resource string specified.
		/// </summary>
        /// <param name="dt"></param>
		/// <param name="resid">Resource ID.</param>
        /// <param name="defaulttext"></param>
        /// <param name="defaulthelp"></param>
		/// <param name="useParser">Uses the terminology parser.</param>
		/// <param name="param">Parameter replacing arguments.</param>
		/// <returns>Need UI string value.</returns>
		private ResourceItem GetResource(DataTable dt, string resid, string defaulttext, string defaulthelp, bool useParser, params string [] param)
		{
			if (param == null) param = new string[0];


			if (dt == null)
				return new ResourceItem("~" + resid + "~", String.Empty);
			else
			{
				DataView res = dt.DefaultView;
				res.RowFilter = "cdcode = '" + resid.Replace("'", "''") + "'";
				if (res.Count == 0)
				{
					if (defaulttext != "")
					{
						try
						{
							FWBS.OMS.CodeLookup.Create(dt.TableName,resid,defaulttext,defaulthelp,CodeLookup.DefaultCulture, true,false,true);
                            FWBS.OMS.Session.CurrentSession.Resources.Refresh();
                            return GetResourceItem(defaulttext, defaulthelp, useParser, param);
						}
						catch (ConstraintException)
                        {
                            Refresh();
                            switch (dt.TableName)
                            {
                                case "RESOURCE":
                                    res = _resourceList.DefaultView;
                                    break;
                                case "MESSAGE":
                                    res = _messageList.DefaultView;
                                    break;
                            }
                            res.RowFilter = "cdcode = '" + resid.Replace("'", "''") + "'";
                            if (res.Count == 0)
                                return new ResourceItem("~" + resid + "~", String.Empty);
                            else
                                return GetResourceItem(Convert.ToString(res[0]["cddesc"]), Convert.ToString(res[0]["cdhelp"]), useParser, param);
                        }
                        catch
						{
							return new ResourceItem("~" + resid + "~", String.Empty);
						}
					}
					else
						return new ResourceItem("~" + resid + "~", String.Empty);
				}
				else
				{
                    return GetResourceItem(Convert.ToString(res[0]["cddesc"]), Convert.ToString(res[0]["cdhelp"]), useParser, param);
                }
			}
		}


		/// <summary>
		/// Gets the resource string specified use the terminoly parser.
		/// </summary>
		/// <param name="resid">Resource ID.</param>
        /// <param name="defaulttext"></param>
        /// <param name="defaulthelp"></param>
		/// <param name="param">Parameter replacing arguments.</param>
		/// <returns>Need UI string value.</returns>
		public ResourceItem GetResource(string resid, string defaulttext, string defaulthelp, params string [] param)
		{
			return GetResource( resid,defaulttext,defaulthelp, true, param);
		}


        private void Fetch()
        {
            FWBS.OMS.Data.ConnectionExecuteParameters pars = new FWBS.OMS.Data.ConnectionExecuteParameters();
            pars.ForceRefresh = false;
            InternalRefresh(pars);
        }

        private void InternalRefresh(FWBS.OMS.Data.ConnectionExecuteParameters pars)
        {
            
            pars.Sql = "sprCodeLookupList";
            pars.CommandType = CommandType.StoredProcedure;
            pars.Parameters = new IDataParameter[2];

            pars.TableNames = new string[] { "RESOURCE" };
            pars.Parameters[0] = Session.CurrentSession.Connection.AddParameter("@Type", System.Data.SqlDbType.NVarChar, 15, "RESOURCE");
            pars.Parameters[1] = Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
            _resourceList = Session.CurrentSession.Connection.ExecuteSQLTable(pars);

            pars.TableNames = new string[] { "MESSAGE" };
            pars.Parameters[0] = Session.CurrentSession.Connection.AddParameter("@Type", System.Data.SqlDbType.NVarChar, 15, "MESSAGE");
            pars.Parameters[1] = Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
            _messageList = Session.CurrentSession.Connection.ExecuteSQLTable(pars);

        }

		public void Refresh()
		{
            FWBS.OMS.Data.ConnectionExecuteParameters pars = new FWBS.OMS.Data.ConnectionExecuteParameters();
            pars.ForceRefresh = true;
            InternalRefresh(pars);
		}

        private ResourceItem GetResourceItem(string text, string help, bool useParser, params string[] param)
        {
            for (int ctr = param.GetLowerBound(0); ctr <= param.GetUpperBound(0); ctr++)
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Append("%");
                str.Append((ctr + 1).ToString());
                str.Append("%");
                text = text.Replace(str.ToString(), Convert.ToString(param[ctr]));
                help = help.Replace(str.ToString(), Convert.ToString(param[ctr]));
            }
            if (Session.CurrentSession != null & useParser)
            {
                text = Session.CurrentSession.Terminology.Parse(text, true);
                help = Session.CurrentSession.Terminology.Parse(help ?? string.Empty, true);
            }
            return new ResourceItem(text, help);
        }

		#endregion

	}

	/// <summary>
	/// A resource item structure that displays a resource text and help text.
	/// </summary>
	public struct ResourceItem
	{
		private string _text;
		private string _help;


		public ResourceItem(string text, string help)
		{
			_text = text;
			_help = help;
		}

		public string Text
		{
			get
			{
				return _text;
			}
		}

		public string Help
		{
			get
			{
				return _help;
			}
		}
	}
}
