using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using FWBS.Common;


namespace FWBS.OMS
{

    /// <summary>
    /// Global information on a data layer.
    /// </summary>
    sealed public class Global
	{
		private Global(){}

		/// <summary>
		/// OMS cache file extension.
		/// </summary>
		public const string CacheExt = "xml";

		/// <summary>
		/// The Blank Template Code
		/// </summary>
		internal const string ReportTemplate = "RPTTEMPLATE";
		
		/// <summary>
		/// OMS cache file extension.
		/// </summary>
		internal const string ReportExt = "rpt";

		/// <summary>
		/// OMS cache file extension.
		/// </summary>
		internal const string ReportDataExt = "xml";

		/// <summary>
		/// OMS session identifier file extension.
		/// </summary>
		internal const string SessionExt = "oms";

		/// <summary>
		/// Application key for the registry setting, to be accessed statically.
		/// </summary>
		public const string ApplicationKey = "OMS";

		/// <summary>
		/// Application version key for the registry setting, to be accessed statically.
		/// </summary>
		public const string VersionKey = "2.0";

		/// <summary>
		/// Application data folder.
		/// </summary>
		private const string ApplicationData = @"FWBS\OMS";

		/// <summary>
		/// Application Name.
		/// </summary>
        private static string appname;
		public static string ApplicationName 
        {
            get
            {
                if (appname == null)
                    appname = Branding.GetApplicationName();
                return appname;
            }
        }

      
		
		/// <summary>
		/// Log switch for the configured trace.
		/// </summary>
		internal static TraceSwitch LogSwitch = new TraceSwitch("BAL", "OMS Business Application Layer");

	
		/// <summary>
		/// Help Registry key.
		/// </summary>
		public static ApplicationSetting Help = new ApplicationSetting(ApplicationKey, VersionKey, "", "Help", "http://www.fwbs.net/helpme.asp?");


		/// <summary>
		/// Caches any data set into the users application data directory.
		/// </summary>
		/// <param name="data">Dataset to cache.</param>
        /// <param name="fileName"></param>
		internal static void Cache(DataSet data, string fileName)
		{
			Cache(data, "", fileName);
		}

		internal static void Cache(DataSet data, string subDirectory, string fileName)
		{
			Common.Directory dir = GetCachePath() + @"\" + subDirectory;
			FilePath filename = dir.ToString() + @"\" + fileName;
			System.IO.Directory.CreateDirectory(dir);
			using (FileStream fs = File.Create(filename))
			{
				data.WriteXml(fs, XmlWriteMode.WriteSchema);
			}
		}

		internal static void Cache(DataTable data, string fileName)
		{
			Cache(data, "", fileName);
		}

		internal static void Cache(DataTable data, string subDirectory, string fileName)
		{
			DataSet ds = new DataSet();
			ds.Tables.Add(data);
			Cache(ds, subDirectory, fileName);
			ds.Tables.Remove(data);
		}

		internal static void Cache(byte [] bytes, string fileName)
		{
			Common.Directory dir = GetCachePath();
			FilePath filename = dir.ToString() + @"\" + fileName;
			using (FileStream fs = File.Create(filename))
			{
				fs.Write(bytes, 0, bytes.Length);
			}
		}

		/// <summary>
		/// Return a Dataview of all live Rows e.g Not Deleted
		/// </summary>
		/// <param name="Input"></param>
		/// <returns></returns>
		public static DataView GetLiveRows(DataTable Input)
		{
			DataView n = new DataView(Input);
			n.RowStateFilter = DataViewRowState.OriginalRows;
			n.RowStateFilter = DataViewRowState.CurrentRows;
			return n;
		}

		/// <summary>
		/// Returns the real number of Records excluding any Deleted
		/// </summary>
		/// <param name="Input"></param>
		/// <returns></returns>
		public static int GetRealCount(DataTable Input)
		{
			DataView n = new DataView(Input);
			n.RowStateFilter=DataViewRowState.Added | DataViewRowState.ModifiedCurrent | DataViewRowState.Unchanged;
			return n.Count;
		}

		/// <summary>
		/// Gets the application data path.
		/// </summary>
		/// <returns></returns>
		public static Common.Directory GetAppDataPath()
		{
			return Path.Combine(SpecialFolders.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Global.ApplicationData);
		}

        /// <summary>
        /// Gets the application data path.
        /// </summary>
        /// <returns></returns>
        public static System.IO.DirectoryInfo GetCommonAppDataPath()
        {
            string path = Path.Combine(SpecialFolders.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Global.ApplicationData);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
            if (dir.Exists == false)
            {
                dir.Create();
            }
            return dir;
        }




		/// <summary>
		/// Gets the temp directory data path.
		/// </summary>
		public static System.IO.DirectoryInfo GetTempPath()
		{
			string path = Path.Combine(Path.GetTempPath(), Global.ApplicationData);
			System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
			if (dir.Exists == false)
			{
				dir.Create();
			}
			return dir;
		}


		/// <summary>
		/// Gets a temporary file name from the temp directory.  Does not create the file though.
		/// </summary>
		public static System.IO.FileInfo GetTempFile()
		{
			return GetTempFile("tmp");
		}

		/// <summary>
		/// Gets a temporary file name from the temp directory.  Does not create the file though.
		/// </summary>
		public static System.IO.FileInfo GetTempFile(string extension)
		{
			return GetTempFile(Guid.NewGuid().ToString(), extension);
		}

		public static System.IO.FileInfo GetTempFile(string fileName, string extension)
		{
			System.IO.DirectoryInfo dir = GetTempPath();
			string file = FWBS.Common.FilePath.ExtractInvalidChars(fileName) + "." + FWBS.Common.FilePath.ExtractInvalidChars(extension).Replace(".", "");
			System.IO.FileInfo f = new System.IO.FileInfo(System.IO.Path.Combine(dir.FullName, file));
			return f;
		}



		/// <summary>
		/// Gets the application da6ta path of the current database instance.
		/// </summary>
		/// <returns></returns>
		public static Common.Directory GetDBAppDataPath()
		{ 
			System.Text.StringBuilder path = new System.Text.StringBuilder();
			path.Append(SpecialFolders.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			path.Append(Path.DirectorySeparatorChar);
			path.Append(Global.ApplicationData);
			if (Session.CurrentSession._multidb != null)
			{
				path.Append(Path.DirectorySeparatorChar);
				path.Append(Common.FilePath.ExtractInvalidChars(Session.CurrentSession._multidb.Number.ToString()));
				path.Append(Path.DirectorySeparatorChar);
				path.Append(Common.FilePath.ExtractInvalidChars(TrimAzureServerPath(Session.CurrentSession._multidb.Server)));
				path.Append(Path.DirectorySeparatorChar);
				path.Append(Common.FilePath.ExtractInvalidChars(Session.CurrentSession._multidb.DatabaseName));
			}
			return path.ToString();
		}

		/// <summary>
		/// Get the My Documents data path of the current database instanace
		/// </summary>
		/// <param name="ApplicationName">Application name with no File Invalid Charactors</param>
		/// <returns></returns>
		public static System.IO.DirectoryInfo GetDBPersonalDataPath(string ApplicationName)
		{
			DirectoryInfo path = new DirectoryInfo(GetDBSpecFolderDataPath(Environment.SpecialFolder.Personal, "My " + ApplicationName));
			path.Create();
			return path;
		}

		/// <summary>
		/// Get the Local Settings data path of the current database instanace
		/// </summary>
		/// <param name="ApplicationName">Application name with no File Invalid Charactors</param>
		/// <returns></returns>
		public static System.IO.DirectoryInfo GetDBLocalSettinngsDataPath(string ApplicationName)
		{
			DirectoryInfo path = new DirectoryInfo(GetDBSpecFolderDataPath(Environment.SpecialFolder.LocalApplicationData, "FWBS\\" + ApplicationName));
			path.Create();
			return path;
		}

        private static string TrimAzureServerPath(string server)
        {
            if (server.EndsWith(".database.windows.net", StringComparison.InvariantCultureIgnoreCase))
                server = server.Substring(0, server.Length - 21);

            return server;
        }

        private static string GetDBSpecFolderDataPath(Environment.SpecialFolder folder, string applicationName)
        {
            System.Text.StringBuilder path = new System.Text.StringBuilder();
            path.Append(SpecialFolders.GetFolderPath(folder));
            path.Append(Path.DirectorySeparatorChar);
            path.Append(applicationName);
            if (Session.CurrentSession._multidb != null)
            {
                path.Append(Path.DirectorySeparatorChar);
                path.Append(Common.FilePath.ExtractInvalidChars(TrimAzureServerPath(Session.CurrentSession._multidb.Server)));
                path.Append('.');
                path.Append(Common.FilePath.ExtractInvalidChars(Session.CurrentSession._multidb.DatabaseName));
                path.Append(Path.DirectorySeparatorChar);
                path.Append(Session.CurrentSession.CurrentUser.Initials);
            }
            return path.ToString();
        }

		/// <summary>
		/// Gets the directory path to the file cache location of OMS.
		/// </summary>
		/// <returns>A directory file path.</returns>
		public static Common.Directory GetCachePath()
		{
			return GetDBAppDataPath() + @"\Cache";
		}

        public static FileInfo GetCacheFile(string entityName, string fileName, bool cultureSpecific = false, bool editionSpecific = false)
        {
            System.Text.StringBuilder filePath = new System.Text.StringBuilder(GetCachePath());
            filePath.Append(Path.DirectorySeparatorChar).Append(entityName);
            if (cultureSpecific)
            {
                filePath.Append(Path.DirectorySeparatorChar).Append(System.Globalization.CultureInfo
                    .CreateSpecificCulture(Session.CurrentSession.DefaultCulture).Name);
            }
            if (editionSpecific)
            {
                filePath.Append(Path.DirectorySeparatorChar).Append(Session.CurrentSession.Edition);
            }
            filePath.Append(Path.DirectorySeparatorChar).Append(fileName).Append('.').Append(CacheExt);
            return new FileInfo(filePath.ToString());
        }

		/// <summary>
		/// Retrieves a cached item from the cache in a dataset form.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns>Returns a XML dataset.</returns>
		internal static DataSet GetCache(string fileName)
		{
			return GetCache("", fileName);
		}

		internal static DataSet GetCache(string subDirectory, string fileName)
		{
			DataSet ds = new DataSet();
			Common.Directory dir = GetCachePath() + subDirectory;
			FileInfo filename = new FileInfo(dir.ToString() + @"\" + fileName);
            if (filename.Exists)
            {
                ds.ReadXml(filename.FullName, XmlReadMode.ReadSchema);
                return ds;
            }
            return null;
		}

		/// <summary>
		/// Gets the resource string specified.
		/// </summary>
		/// <param name="resID">Resource ID.</param>
		/// <param name="useParser">Uses the terminology parser.</param>
		/// <param name="param">Parameter replacing arguments.</param>
		/// <returns>Need UI string value.</returns>
		public static string GetResString(string resID, bool useParser, params string [] param)
		{
			string text = GetResString( resID, useParser);
			if (param == null) return text;
			for (int ctr = param.GetLowerBound(0); ctr <= param.GetUpperBound(0); ctr++)
			{
				text = text.Replace($"%{ctr+1}%", param[ctr]);
			}
			return text;
		}

		public static string GetResString(string resID, params string [] param)
		{
			return GetResString( resID, true, param);
		}

		public static string GetResString(string resID,  bool useParser)
		{
			ResourceManager rm = new ResourceManager("FWBS.OMS.omsbl", Assembly.GetExecutingAssembly());
			string ret = rm.GetString(resID);
			if (ret == null) 
			{
				ret = "#" + resID + "# Not in Resource File";
				Trace.WriteLineIf(Global.LogSwitch.TraceWarning,ret,"RESOURCE");
			}
			if (Session.OMS != null && ret != null && useParser && Session.CurrentSession.IsLoggedIn)
			{
				ret = Session.CurrentSession.Terminology.Parse(ret,true);
			}
			return ret;
		}

		/// <summary>
		/// Creates a new record in the passed data table using its schema.  Allowing nulls will
		/// get round the issue of a column not allowing nulls.
		/// </summary>
		/// <param name="dt">Data table to add a new row to.</param>
		/// <param name="allowNulls">If set, allows all columns to allow nulls.</param>
		internal static DataRow CreateBlankRecord(ref DataTable dt, bool allowNulls)
		{
			if (allowNulls)
			{
				//Set up a new empty record for the enquiry engine to manipulate.
				foreach (DataColumn col in dt.Columns)
					if (!col.AllowDBNull) col.AllowDBNull = true;
			}
			DataRow r = dt.NewRow();
			dt.Rows.Add(r);
			return r;
		}


        internal static void Merge(DataRow current, DataRow changes)
        {
            if (current == null)
                throw new ArgumentNullException("current");

            if (changes == null)
                throw new ArgumentNullException("changes");

            Dictionary<string, object> changedvals = new Dictionary<string, object>();

            foreach (DataColumn col in changes.Table.Columns)
            {
                object cv = changes[col, DataRowVersion.Current];
                object ov = changes[col, DataRowVersion.Original];
                if (!object.Equals(cv, ov))
                {
                    changedvals.Add(col.ColumnName, cv);
                    continue;
                }

            }


            current.AcceptChanges();

            foreach (var key in changedvals)
            {
                if (current.Table.Columns.Contains(key.Key))
                    current[key.Key] = key.Value;
            }
        }

	}


}
