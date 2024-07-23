#region References
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Packaging;
using System.Text;
using System.Xml;
#endregion

namespace FWBS.WF.Packaging
{
    #region WorkflowScript class
    internal sealed class WorkflowScript : WorkflowCommonObject
	{ 
		#region Constants
		private const string SCRIPTTYPE = "WORKFLOW";						// workflow script type in dbScript
		private const string CODELOOKUP_SCRIPTTYPE = "WORKFLOW_SCRIPT";

		// SQL Related
		private const string DBS_TABLENAME = "dbScript";					// SQl table name for workflow scripts

		private const string DBS_SELECT_STMT = "select " + DBS_CODE_FIELD + "," + DBS_VERSION_FIELD + "," + DBS_CONFIG_FIELD + "," + DBS_TYPE_FIELD  + "," +
			DBS_CREATED_FIELD + "," + DBS_CREATEDBY_FIELD + "," + DBS_UPDATED_FIELD + "," + DBS_UPDATEDBY_FIELD +
			", dbo.GetUser(" + DBS_CREATEDBY_FIELD + ", 'USRFULLNAME') as " + DBS_UPDATEDBYUSER_FIELD +
			", dbo.GetUser(" + DBS_UPDATEDBY_FIELD + ", 'USRFULLNAME') as " + DBS_UPDATEDBYUSER_FIELD +
			" from " + DBS_TABLENAME;

		private const string DBS_SELECT_STMT2 = DBS_SELECT_STMT + " WHERE scrType = '" + SCRIPTTYPE + "'";	// select workflow types only from the SQL table
		private const string DBS_PRIMARYKEY = DBS_CODE_FIELD;				// Primary key of the table
		private const string DBS_CODE_FIELD = "scrCode";					// 'code' column
		private const string DBS_VERSION_FIELD = "scrVersion";				// 'version' column
		private const string DBS_CONFIG_FIELD = "scrText";					// 'config' of this script including any assembly references etc
		private const string DBS_TYPE_FIELD = "scrType";					// 'type' column - always 'WORKFLOW' for workflow scripts
		private const string DBS_CREATED_FIELD = "created";					// 'created' column
		private const string DBS_CREATEDBY_FIELD = "createdby";				// 'createdby' column
		private const string DBS_CREATEDBYUSER_FIELD = "createdByUser";		// 'createdByuser' column - full name of user that created it
		private const string DBS_UPDATED_FIELD = "updated";					// 'updated' column
		private const string DBS_UPDATEDBY_FIELD = "updatedby";				// 'updatedby' column
		private const string DBS_UPDATEDBYUSER_FIELD = "updatedByUser";		// 'updatedByuser' column - full name of user that last updated it

		// XML + XPATH related
		private const string XML_SCRIPTTEXT = "scriptText";
		private const string XPATH_SCRIPTTEXT = XPATH_WORKFLOW + "/" + XML_SCRIPTTEXT;	// "/config/script/scriptText";

		private const string VB_LANGUAGE = "VB";
		private const string CSHARP_LANGUAGE = "C";
		private const string XML_LANGUAGE = "ScriptLanguage";				// 'VB' or 'C' for C# - if missing then C#

		private const string XML_ISREADONLY = "IsReadOnly";					// Read only?

		// File extensions
		private const string FILEEXT_CSHARP = ".cs";
		private const string FILEEXT_VB = ".vb";

		// ZIP package
		private const string ZIP_URI_SCRIPT = "Script";							// URI within .zip file for the script
		private const string ZIP_MIMETYPE_SCRIPT = "application/octet-stream";	// script mime type, application/octet-stream means binary
		#endregion

		#region Fields
		// XML field containing all sorts of settings and data
		private Common.ConfigSetting settings = null;
		// Flag to hack the CommonObject SelectStatement property to return the correct data
		//	we could use FWBS.OMS.Script.ScriptGen.GetDetailedScripts() in GetList() ...
		//	If base classes call base GetList() it will get all scripts!
		private bool isGettingList = false;
		#endregion

		#region Constructor
		internal WorkflowScript()
			: base()
		{
			// base will create various things so that the below stmt won't bomb out
			this.ScriptType = SCRIPTTYPE;
		}
		#endregion

		#region Properties
		#region ScriptType
		private string ScriptType
		{
			set
			{
				this.SetExtraInfo(DBS_TYPE_FIELD, value);
			}
		}
		#endregion

		#region Code
		internal string Code
		{
			get
			{
				return Convert.ToString(this.GetExtraInfo(DBS_CODE_FIELD));
			}
			set
			{
				// only change if it is newly added
				if (this._data.Rows[0].RowState == DataRowState.Added)
				{
					this.SetExtraInfo(DBS_CODE_FIELD, value);
				}
			}
		}
		#endregion

		#region Description
		internal string Description
		{
			get
			{
				return FWBS.OMS.CodeLookup.GetLookup(CODELOOKUP_SCRIPTTYPE, this.Code);
			}
			set
			{
				FWBS.OMS.CodeLookup.Create(CODELOOKUP_SCRIPTTYPE, this.Code, value, string.Empty, FWBS.OMS.CodeLookup.DefaultCulture, true, true, true);
			}
		}
		#endregion

		#region Version
		internal long Version
		{
			get
			{
				return FWBS.Common.ConvertDef.ToInt64(this.GetExtraInfo(DBS_VERSION_FIELD), 0);
			}

			set
			{
				this.SetExtraInfo(DBS_VERSION_FIELD, value);
			}
		}
		#endregion

		#region Script
		internal string Script
		{
			get
			{
				string retValue = string.Empty;

				System.Xml.XmlNode nd = this.Settings.DocObject.SelectSingleNode(XPATH_SCRIPTTEXT);
				if (nd != null)
				{
					// convert from base64 first, then unzip ...
					byte[] buffer = Convert.FromBase64String(nd.InnerText);
					System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
					retValue = enc.GetString(this.ExtractPackage(buffer));
				}

				return retValue;
			}
			set
			{
				System.Xml.XmlNode nd = this.Settings.DocObject.SelectSingleNode(XPATH_SCRIPTTEXT);
				if (nd == null)
				{
					nd = this.Settings.DocObject.CreateElement(XML_SCRIPTTEXT);
					XmlNode nd2 = this.Settings.DocObject.SelectSingleNode(XPATH_WORKFLOW);
					if (nd2 == null)
					{
						nd2 = this.Settings.DocObject.CreateElement(XML_WORKFLOW);
						this.Settings.DocObject.SelectSingleNode(XPATH_CONFIG).AppendChild(nd2);
					}
					nd2.AppendChild(nd);
				}

				// zip first, then convert to base64 ...
				nd.InnerText= Convert.ToBase64String(this.CreatePackage(ASCIIEncoding.Default.GetBytes(value)));
			}
		}
		#endregion

		#region Settings
		protected override FWBS.Common.ConfigSetting Settings
		{
			get
			{
				if (this.settings == null)
				{
					this.settings = new Common.ConfigSetting(this._data.Rows[0], DBS_CONFIG_FIELD);
				}
				return this.settings;
			}
		}
		#endregion

		#region IsCSharp
		internal bool IsCSharp
		{
			get
			{
				bool retValue = true;

				System.Xml.XmlNode nd = this.Settings.DocObject.SelectSingleNode(XPATH_WORKFLOW);
				if (nd != null)
				{
					if (nd.Attributes[XML_LANGUAGE] != null)
					{
						if (nd.Attributes[XML_LANGUAGE].Value == VB_LANGUAGE)
						{
							retValue = false;
						}
					}
				}

				return retValue;
			}

			set
			{
				System.Xml.XmlNode nd = this.Settings.DocObject.SelectSingleNode(XPATH_WORKFLOW);
				if (nd == null)
				{
					nd = this.Settings.DocObject.CreateElement(XML_WORKFLOW);
					this.Settings.DocObject.SelectSingleNode(XPATH_CONFIG).AppendChild(nd);
				}

				XmlAttribute attrib = nd.Attributes[XML_LANGUAGE];
				if (attrib == null)
				{
					attrib = this.Settings.DocObject.CreateAttribute(XML_LANGUAGE);
					nd.Attributes.Append(attrib);
				}
				attrib.Value = value ? CSHARP_LANGUAGE : VB_LANGUAGE;
			}
		}
		#endregion

		#region IsReadOnly
		internal bool IsReadOnly
		{
			get
			{
				bool retValue = false;

				System.Xml.XmlNode nd = this.Settings.DocObject.SelectSingleNode(XPATH_WORKFLOW);
				if (nd != null)
				{
					if (nd.Attributes[XML_ISREADONLY] != null)
					{
						if (nd.Attributes[XML_ISREADONLY].Value == bool.TrueString)
						{
							retValue = true;
						}
					}
				}

				return retValue;
			}

			set
			{
				System.Xml.XmlNode nd = this.Settings.DocObject.SelectSingleNode(XPATH_WORKFLOW);
				if (nd == null)
				{
					nd = this.Settings.DocObject.CreateElement(XML_WORKFLOW);
					this.Settings.DocObject.SelectSingleNode(XPATH_CONFIG).AppendChild(nd);
				}

				XmlAttribute attrib = nd.Attributes[XML_ISREADONLY];
				if (attrib == null)
				{
					attrib = this.Settings.DocObject.CreateAttribute(XML_ISREADONLY);
					nd.Attributes.Append(attrib);
				}
				attrib.Value = value.ToString();
			}
		}
		#endregion

		#region FileName
		internal string FileName
		{
			get
			{
				return this.Code + (this.IsCSharp ? FILEEXT_CSHARP : FILEEXT_VB);
			}
		}
		#endregion

		#region UpdatedDate
		public DateTime UpdatedDate
		{
			get
			{
				return FWBS.Common.ConvertDef.ToDateTime(this.GetExtraInfo(DBS_UPDATED_FIELD), DateTime.MinValue);
			}
		}
		#endregion

		#region UpdatedBy
		public string UpdatedBy
		{
			get
			{
				object obj = this.GetExtraInfo(DBS_UPDATEDBYUSER_FIELD);
				if (obj is DBNull)
				{
					return string.Empty;
				}
				return Convert.ToString(obj);
			}
		}
		#endregion

		#region CreatedDate
		public DateTime CreatedDate
		{
			get
			{
				return FWBS.Common.ConvertDef.ToDateTime(this.GetExtraInfo(DBS_CREATED_FIELD), DateTime.MinValue);
			}
		}
		#endregion

		#region CreatedBy
		public string CreatedBy
		{
			get
			{
				object obj = this.GetExtraInfo(DBS_CREATEDBYUSER_FIELD);
				if (obj is DBNull)
				{
					return string.Empty;
				}
				return Convert.ToString(obj);
			}
		}
		#endregion
		#endregion

		#region Methods
		public override string ToString()
		{
			return this.Code;
		}
		#endregion

		#region CommonObject
		protected override string SelectStatement
		{
			get
			{
				return this.isGettingList ? DBS_SELECT_STMT2 : DBS_SELECT_STMT;
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return DBS_TABLENAME;
			}
		}

		protected override string DatabaseTableName
		{
			get
			{
				return DBS_TABLENAME;
			}
		}

		protected override string DefaultForm
		{
			get
			{
				return string.Empty;
			}
		}

		public override string FieldPrimaryKey
		{
			get
			{
				return DBS_PRIMARYKEY;
			}
		}

		public override object Parent
		{
			get
			{
				return null;
			}
		}

		public void Fetch(string code)
		{
			Dispose();
			base.Fetch(code);
		}

		public override void Create()
		{
			Dispose();

			base.Create();
		}

		public override void Update()
		{
			if (this.State != FWBS.OMS.ObjectState.Deleted)
			{
				// Check if there are any changes made before setting the updated and updated by properties then update. - what?
				this.SetExtraInfo(DBS_CONFIG_FIELD, this.Settings.DocObject.OuterXml);
			}

			if (this._data.GetChanges() != null)
			{
				if (this.State != FWBS.OMS.ObjectState.Deleted)
				{
					this.SetExtraInfo(DBS_VERSION_FIELD, this.Version + 1);
				}
				base.Update();
			}
		}

		public override void Cancel()
		{
			base.Cancel();

			Dispose();
		}

		new public DataTable GetList()
		{
			this.isGettingList = true;
			try
			{
				DataTable dt = base.GetList();
				return dt;
			}
			finally
			{
				this.isGettingList = false;
			}
		}
		#endregion

		#region ExtractScript
		// Returns the external references
		public string[] ExtractScript(DirectoryInfo dir)
		{
			List<string> externalReferences = new List<string>();

			try
			{
				// Get the managed assemblies
				this.ExtractDistribution();

				// create source file
				// IMPORTANT NOTE:
				//	If the file cannot be exclusively opened the it is deemed to be in use and
				//	we cannot write to it!
				FileInfo file = new FileInfo(dir.FullName + @"\" + this.FileName);
				if (!WorkflowScript.IsFileLocked(file))
				{
					StreamWriter sw = file.CreateText();
					sw.Write(this.Script);
					sw.Close();
				}

				#region Collect the external references for compilation
				// Add distribution references
				foreach (string str in this.GetDistributions())
				{
					string destFileName = null;
					FileInfo fileInfo = new FileInfo(str);
					// form the full filename we expect for a distributed assembly
					destFileName = string.Format(@"{0}\{1}", FWBS.OMS.Session.CurrentSession.DistributedAssemblyManager.DistributedAssembliesDirectory, fileInfo.Name);

					try
					{
						externalReferences.Add(destFileName);
					}
					catch (Exception ex)
					{
						if (this.DistributedErrorsHandler != null)
						{
							this.DistributedErrorsHandler(this, new DistributedEventArgs(ex));
						}
						else
						{
							throw;
						}
					}
				}

				// Add references
				foreach (string str in this.GetReferences())
				{
					FileInfo fileInfo = new FileInfo(str);
					try
					{
						externalReferences.Add(fileInfo.Name);
					}
					catch (Exception ex)
					{
						if (this.DistributedErrorsHandler != null)
						{
							this.DistributedErrorsHandler(this, new DistributedEventArgs(ex));
						}
						else
						{
							throw;
						}
					}
				}
				#endregion
			}
			catch (Exception ex)
			{
				if (this.DistributedErrorsHandler != null)
				{
					this.DistributedErrorsHandler(this, new DistributedEventArgs(ex));
				}
				else
				{
					throw;
				}
			}

			return externalReferences.ToArray();
		}
		#endregion

		#region CompileCode
		internal static System.CodeDom.Compiler.CompilerResults CompileCode(
			System.CodeDom.Compiler.CodeDomProvider codeCompiler,		// compiler (C# or VB)
			string sourceFile,											// file to compile
			string assemblyName,										// name of assembly dll
			string[] externalReferences,								// external references - actual .DLL files, NOT namespaces!
			ulong versionNo = 0)										// script version number
		{
			// Validate arguments
			if (string.IsNullOrEmpty(sourceFile) ||		// must give file name
				string.IsNullOrEmpty(assemblyName) ||	// must give assembly dll name
				!System.IO.File.Exists(sourceFile))		// file must exist
			{
				// argument error
				throw new ArgumentException();
			}

			// set file version bits
			uint filePrivatePart = (uint)(versionNo & 0x000000000000FFFF);
			uint fileBuildPart = (uint)((versionNo >> 16) & 0x000000000000FFFF);
			uint fileMinorPart = (uint)((versionNo >> 32) & 0x000000000000FFFF);
			uint fileMajorPart = (uint)((versionNo >> 48) & 0x000000000000FFFF);

			// Create temporary assembly attribute file - for security
			string tmpAssemblyAttributeFile = sourceFile + ".tmp";
			StreamWriter sw = File.CreateText(tmpAssemblyAttributeFile);
			if (codeCompiler is Microsoft.CSharp.CSharpCodeProvider)
			{
				// C#
				sw.WriteLine("[assembly: FWBS.OMS.Script.ScriptGenAssembly]");
				sw.WriteLine(string.Format("[assembly: System.Reflection.AssemblyFileVersion(\"{0}.{1}.{2}.{3}\")]", fileMajorPart, fileMinorPart, fileBuildPart, filePrivatePart));
			}
			else
			{
				// Assume VB.NET
				sw.WriteLine("<assembly: FWBS.OMS.Script.ScriptGenAssembly>");
				sw.WriteLine(string.Format("<assembly: System.Reflection.AssemblyFileVersion(\"{0}.{1}.{2}.{3}\")>", fileMajorPart, fileMinorPart, fileBuildPart, filePrivatePart));
			}
			sw.Close();

			// TODO: A way to clear the .TMP files
			// Check if the .dll is in use
			if (WorkflowScript.IsFileLocked(new FileInfo(assemblyName)))
			{
				// rename 'in use' file so that we can compile to the .dll
				File.Move(assemblyName, assemblyName + "." + DateTime.Now.Ticks.ToString() + ".TMP");
			}

			// Define parameters to invoke a compiler
			System.CodeDom.Compiler.CompilerParameters compilerParameters = new System.CodeDom.Compiler.CompilerParameters();
			// Set the assembly file name to generate.
			compilerParameters.OutputAssembly = assemblyName;
			// Generate an executable instead of a class library.
			compilerParameters.GenerateExecutable = false;
			compilerParameters.GenerateInMemory = false;

			// Add external references
			if (externalReferences != null)
			{
				foreach (string str in externalReferences)
				{
					compilerParameters.ReferencedAssemblies.Add(str);
				}
			}
			// Add OMS.Library - for security
			compilerParameters.ReferencedAssemblies.Add("OMS.Infrastructure.dll");

			// Generate debug information - creates a .pdb file
			compilerParameters.IncludeDebugInformation = false;
			// Set the level at which the compiler should start displaying warnings
			compilerParameters.WarningLevel = 3;
			// Set whether to treat all warnings as errors.
			compilerParameters.TreatWarningsAsErrors = false;
			// Set compiler argument to optimize output.
			compilerParameters.CompilerOptions = "/optimize";

			// Invoke and return the results of compilation
			System.CodeDom.Compiler.CompilerResults results = codeCompiler.CompileAssemblyFromFile(compilerParameters, new string [] { sourceFile, tmpAssemblyAttributeFile } );

			// Delete temporary file
			File.Delete(tmpAssemblyAttributeFile);

			return results;
		}
		#endregion

		#region IsFileLocked
		private static bool IsFileLocked(FileInfo file)
		{
			bool retValue = false;			// file is not locked or does not exist
			FileStream stream = null;

			try
			{
				if (file.Exists)
				{
					stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
				}
			}
			catch (IOException)
			{
				// the file is already open because it is:
				//		1) still being written to
				//	or	2) being processed by another thread
				//	or	3) it is in use
				retValue = true;
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}

			return retValue;
		}
		#endregion

		#region CreatePackage - zip
		private byte[] CreatePackage(byte[] script)
		{
			// create memory stream
			MemoryStream packagestream = new MemoryStream();
			// open package
			Package package = Package.Open(packagestream, FileMode.Create);
			// create package part uri i.e. location
			Uri partUri = PackUriHelper.CreatePartUri(new Uri(ZIP_URI_SCRIPT, UriKind.Relative));
			// create package part
			PackagePart packpart = package.CreatePart(partUri, ZIP_MIMETYPE_SCRIPT);
			// write the data to the package part
			packpart.GetStream().Write(script, 0, script.Length);
			// close package
			package.Close();
			// return the package
			return packagestream.ToArray();
		}
		#endregion

		#region ExtractPackage - zip
		private byte[] ExtractPackage(byte[] packageArray)
		{
			MemoryStream script = new MemoryStream();				// for converting to byte[]
			MemoryStream reader = new MemoryStream(packageArray);	// zip package - note memorystream uses the byte[] passed in and cannot increase its size 

			// open package
			Package package = Package.Open(reader, FileMode.Open);
			// create package part uri i.e. location
			Uri partUri = PackUriHelper.CreatePartUri(new Uri(ZIP_URI_SCRIPT, UriKind.Relative));
			// get package part
			PackagePart scriptPart = package.GetPart(partUri);
			// get the package part data
			scriptPart.GetStream().CopyTo(script);
			// return the script
			return script.ToArray();
		}
		#endregion
	}
	#endregion
}
