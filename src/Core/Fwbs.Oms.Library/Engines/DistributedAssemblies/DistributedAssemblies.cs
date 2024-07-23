using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using FWBS.Common;
using FWBS.OMS.Data.Exceptions;

namespace FWBS.OMS
{
    public enum DistributedPackageType { Modules, Assembly, Package }
    public enum DistributedCheckResult { NotFound, OutOfDate, NotDownloaded, Ok, Error } 

    public class DistributedAssemblies : CommonObject
    {
        DistributedAssemblyManager _manager;

        #region CommonObject
        protected override string SelectStatement
        {
            get { return "select * from dbAssembly"; }
        }

        protected override string PrimaryTableName
        {
            get { return "dbAssembly"; }
        }

        protected override string DefaultForm
        {
            get { return ""; }
        }

        public override object Parent
        {
            get { return null; }
        }

        public override string FieldPrimaryKey
        {
            get { return "ID"; }
        }

        #endregion

        #region Static



        public static DateTime GetModifiedDateForCurrent(string assemblyfilenameandversion)
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("ID", assemblyfilenameandversion + Session.CurrentSession.AssemblyVersion.ToString());

            DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("select Modified from dbAssembly WHERE ID = @ID", "Modified", paramlist);
            if (dt.Rows.Count > 0)
                return ConvertDef.ToDateTime(dt.Rows[0]["Modified"], DateTime.MinValue);
            else
                throw new OMSException2("DANOTFOUND", "Distributed Assembly '%1%' cannot be found for OMS Assembly Version %2%", new Exception(), true, assemblyfilenameandversion, Session.CurrentSession.EngineVersion.ToString());
        }

        #endregion

        #region Constructors
        public DistributedAssemblies()
        {
            _manager = Session.CurrentSession.DistributedAssemblyManager;
        }
        #endregion

        #region Fields

        #endregion

        #region Public
        public void Fetch(string assemblyfilenameandversion)
        {
            base.Fetch(assemblyfilenameandversion);
        }

        public void FetchCurrent(string assemblyfilename)
        {
            try
            {
                base.Fetch(assemblyfilename + Session.CurrentSession.AssemblyVersion.ToString());
            }
            catch
            {
                base.Fetch(assemblyfilename);
            }
        }

        public override void Update()
        {
            if (xmlprops != null) xmlprops.Update(); 
            
            if (IsDirty && _data.Rows[0].RowState != System.Data.DataRowState.Deleted)
                StoreToDatabase(this.SourceFileName);
			try
			{
				base.Update();
			}
			catch (ConnectionException cex)
			{
				SqlException sqlex = cex.InnerException as SqlException;
				if (sqlex != null)
					throw new OMSException2("ERRDISTDUP", "The Assembly is already present in the Distributed Assemblies for this Version of OMS. Please find and Refresh instead");
				
				throw;
			}
            _manager.ClearCache();
        }
        #endregion

        #region Properties


        public string AssemblyName
        {
            get
            {
                return Convert.ToString(GetExtraInfo("Assembly"));
            }
        }


        public string SourceFileName
        {
            get
            {
                return Convert.ToString(GetExtraInfo("SourceFileName"));
            }
        }

        public void SetSourceFileName(string filename, string omsversion)
        {
            if (SourceFileName != filename)
            {
                System.IO.FileInfo file = new System.IO.FileInfo(filename);
                if (file.Exists)
                {
                    this.PackageType = OMS.DistributedPackageType.Package;
                    SetExtraInfo("SourceFileName", filename);
                    OMSVersion = omsversion;
                    SetExtraInfo("ID", file.Name + omsversion);
                }
                else
                    throw new OMSException2("ERRASSMISS", "Assembly file name '%1%' cannot be found", null, true, filename);
            }
        }

        public DateTime Modified
        {
            get
            {
                return Convert.ToDateTime(GetExtraInfo("Modified"));
            }
        }

        public string OMSVersion
        {
            get
            {
                return Convert.ToString(GetExtraInfo("OMSVersion"));
            }
            set
            {
                SetExtraInfo("OMSVersion", value);
                if (!string.IsNullOrEmpty(value))
                {
                    Version ver = new Version(value);
                    if (ver.Major < 5)
                        PackageType = DistributedPackageType.Assembly;
                }
            }
        }

        public DistributedPackageType PackageType
        {
            get
            {
                if (_data.Columns.Contains("PackageType"))
                {
                    var pt = Convert.ToString(GetExtraInfo("PackageType"));
                    var pte = (DistributedPackageType)ConvertDef.ToEnum(pt, DistributedPackageType.Assembly);
                    return pte;
                }
                else
                    return OMS.DistributedPackageType.Assembly;

            }
            set
            {
                if (_data.Columns.Contains("PackageType"))
                {
                    SetExtraInfo("PackageType", value);
                }
            }
        }
        #endregion

        #region XML Settings Methods

        /// <summary>
        /// XML type configuration settings.
        /// </summary>
        private XmlProperties xmlprops = null;

        private void BuildXML()
        {
            if (xmlprops == null)
                xmlprops = new XmlProperties(this, "Xml");
        }
        public object GetXmlProperty(string name, object defaultValue)
        {
            BuildXML();
            return xmlprops.GetProperty(name, defaultValue);
        }

        public void SetXmlProperty(string name, object val)
        {
            BuildXML();
            if (xmlprops.SetProperty(name, val))
                IsDirty = true;
        }
        #endregion

        #region Private

        public FileInfo ExtractFromDatabase()
        {
            try
            {
                FileInfo filename = null;
                switch (PackageType)
                {
                    case DistributedPackageType.Assembly:
                        filename = ExtractAssembly();
                        break;
                    case DistributedPackageType.Modules:
                    case DistributedPackageType.Package:
                        filename = ExtractPackage(PackageType);
                        break;
                    default:
                        break;
                }
                return filename;
            }
            catch (UnauthorizedAccessException ex)
            {
                _manager.ShowUpdateWarning(new string[]{ex.Message});
                throw ex;
            }
        }

        private const string DLL = ".dll";

        private void StoreToDatabase(string Filename)
        {
            if (_data.Rows[0].RowState != System.Data.DataRowState.Deleted)
            {
                switch (this.PackageType)
                {
                    case DistributedPackageType.Assembly:
                        CreateAssembly(Filename);
                        break;
                    case DistributedPackageType.Modules:
                    case DistributedPackageType.Package:
                        CreatePackage(Filename);
                        break;
                    default:
                        break;
                }
                System.IO.FileInfo file = new System.IO.FileInfo(Filename);
                SetExtraInfo("Modified", file.LastWriteTime);
                SetExtraInfo("Assembly", file.Name);
                if (file.Extension.Equals(DLL, StringComparison.OrdinalIgnoreCase))
                {
                    FileVersionInfo ver = FileVersionInfo.GetVersionInfo(Filename);
                    SetExtraInfo("Version", ver.FileVersion);
                }
            }
        }
        
        private FileInfo ExtractPackage(DistributedPackageType packageType)
        {
            FileInfo fileinfo = null;
            switch (packageType)
            {
                case DistributedPackageType.Modules:
                    fileinfo = DistributedAssemblyManager.GetExtractedModuleFileInfo(this.AssemblyName, _manager.DistributedAssembliesDirectory.FullName);
                    break;
                case DistributedPackageType.Package:
                    fileinfo = DistributedAssemblyManager.GetExtractedAssemblyFileInfo(this.AssemblyName, _manager.DistributedAssembliesDirectory.FullName);
                    break;
                default:
                    throw new NotSupportedException();
            }

            if (fileinfo == null)
                throw new NotSupportedException("package type of Assembly is not supported");

            fileinfo.Directory.Create();

            using (MemoryStream reader = new MemoryStream())
            {
                byte[] buffer = Convert.FromBase64String(Convert.ToString(GetExtraInfo("Attached")));
                reader.Write(buffer, 0, buffer.Length);
                reader.Position = 0;
                using (Package package = Package.Open(reader))
                {
                    // Add the Document part to the Package
                    Uri parturi = PackUriHelper.CreatePartUri(new Uri(fileinfo.Name, UriKind.Relative));
                    var part = package.GetPart(parturi);
                    if (fileinfo.Exists)
                        fileinfo.Delete();
                    using (FileStream fileStream = new FileStream(fileinfo.FullName, FileMode.Create))
                    {
                        CopyStream(part.GetStream(), fileStream);
                    }
                }

                if (Convert.ToString(GetExtraInfo("Modified")) != "")
                {
                    fileinfo.LastWriteTime = Convert.ToDateTime(Convert.ToString(GetExtraInfo("Modified")));
                }
            }

            return fileinfo;
        }

        private FileInfo ExtractAssembly()
        {
            var fileinfo = DistributedAssemblyManager.GetExtractedAssemblyFileInfo(this.AssemblyName, _manager.DistributedAssembliesDirectory.FullName);

            fileinfo.Directory.Create();

            using (System.IO.FileStream reader = System.IO.File.Create(fileinfo.FullName))
            {
                byte[] buffer = Convert.FromBase64String(Convert.ToString(GetExtraInfo("Attached")));
                reader.Write(buffer, 0, buffer.Length);
            }
            if (Convert.ToString(GetExtraInfo("Modified")) != "")
            {
                fileinfo.LastWriteTime = Convert.ToDateTime(Convert.ToString(GetExtraInfo("Modified")));
            }
            return fileinfo;
        }

        private void CreateAssembly(string Filename)
        {
            using (System.IO.FileStream reader = new System.IO.FileStream(Filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
            {
                byte[] buffer = new byte[reader.Length];
                reader.Read(buffer, 0, (int)reader.Length);
                SetExtraInfo("Attached", Convert.ToBase64String(buffer));
            }
        }

        private void CreatePackage(string Filename)
        {
            using (MemoryStream packagestream = new MemoryStream())
            {
                using (Package package = Package.Open(packagestream, FileMode.Create))
                {
                    FileInfo fileinfo = new FileInfo(Filename);
                    Uri parturi = PackUriHelper.CreatePartUri(new Uri(fileinfo.Name, UriKind.Relative));
                    PackagePart packpart = package.CreatePart(parturi, "application/octet-stream");
                    using (FileStream fileStream = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        CopyStream(fileStream, packpart.GetStream());
                    }
                }
                packagestream.Position = 0;
                byte[] buffer = new byte[packagestream.Length];
                packagestream.Read(buffer, 0, (int)packagestream.Length);
                SetExtraInfo("Attached", Convert.ToBase64String(buffer));
            }
        }

        private static void CopyStream(Stream source, Stream target)
        {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                target.Write(buf, 0, bytesRead);
        }
        #endregion


    }


}
