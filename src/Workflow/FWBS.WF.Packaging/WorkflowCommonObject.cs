#region References
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
#endregion

namespace FWBS.WF.Packaging
{
    #region WorkflowCommonObject abstract class
    public abstract class WorkflowCommonObject : FWBS.OMS.CommonObject
	{
		#region Constants
		// Assembly related
		private const string DA_FILENAME = "filename";
		private const string DA_MODIFIED = "modified";
		// XML + XPATH related
		protected const string XML_WORKFLOW = "script";						// renamed from 'workflow' to 'script' to make it compatible for scripting (confuses workflow though!)
		private const string XML_DISTRIBUTIONS = "distributions";
		private const string XML_DISTRIBUTION = "distribution";
		protected const string XPATH_CONFIG = "/config";
		private const string XML_REFERENCES = "references";
		private const string XML_REFERENCE = "reference";
		private const string XML_SCRIPTCODE = "scriptCode";
		private const string XML_ACTIVITY_SCRIPT_CODES = "activityScriptCodes";
		private const string XML_ACTIVITY_SCRIPT_CODE = "activityScriptCode";
		protected const string XPATH_WORKFLOW = XPATH_CONFIG + "/" + XML_WORKFLOW;				// "/config/script";
		private const string XPATH_DISTRIBUTIONS = XPATH_WORKFLOW + "/" + XML_DISTRIBUTIONS;	// "/config/script/distributions";
		private const string XPATH_REFERENCES = XPATH_WORKFLOW + "/" + XML_REFERENCES;			// "/config/script/references";
		private const string XPATH_WFSCRIPTS = XPATH_WORKFLOW + "/" + XML_ACTIVITY_SCRIPT_CODES;// "/config/script/activityScriptCodes";
		#endregion

		#region Fields
		// Events
		public event EventHandler<DistributedEventArgs> DistributedErrors;
		#endregion

		#region Constructors
		public WorkflowCommonObject()
		{
			base.Create();
		}
		#endregion

		#region Properties
		#region Settings
		// Derived classes MUST implement this property!
		abstract protected FWBS.Common.ConfigSetting Settings { get; }
		#endregion

		#region DistributedErrorsHandler
		protected EventHandler<DistributedEventArgs> DistributedErrorsHandler
		{
			get
			{
				return this.DistributedErrors;
			}
		}
		#endregion
		#endregion

		#region CommonObject
		protected override string SelectStatement
		{
			get
			{
				return string.Empty;
			}
		}

		protected override string PrimaryTableName
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
				return string.Empty;
			}
		}
		#endregion

		#region Distributions
		#region GetDistributions
		public virtual HashSet<string> GetDistributions()
		{
			HashSet<string> retValue = new HashSet<string>();

			System.Xml.XmlNode nd = this.Settings.DocObject.SelectSingleNode(XPATH_DISTRIBUTIONS);
			if (nd != null)
			{
				foreach (System.Xml.XmlNode x in nd.ChildNodes)
				{
					retValue.Add(x.Attributes[DA_FILENAME].Value);
				}
			}

			return retValue;
		}
		#endregion

		#region SetDistribution
		public virtual void SetDistribution(HashSet<string> distributions)
		{
			System.Xml.XmlNode nd = this.Settings.DocObject.SelectSingleNode(XPATH_DISTRIBUTIONS);
			if (nd == null)
			{
				nd = this.Settings.DocObject.CreateElement(XML_DISTRIBUTIONS);
				XmlNode nd2 = this.Settings.DocObject.SelectSingleNode(XPATH_WORKFLOW);
				if (nd2 == null)
				{
					nd2 = this.Settings.DocObject.CreateElement(XML_WORKFLOW);
					this.Settings.DocObject.SelectSingleNode(XPATH_CONFIG).AppendChild(nd2);
				}
				nd2.AppendChild(nd);
			}

			nd.RemoveAll();
			foreach (string s in distributions)
			{
				System.Xml.XmlElement nnd = nd.OwnerDocument.CreateElement(XML_DISTRIBUTION);
				System.IO.FileInfo file = new System.IO.FileInfo(s);

				System.Xml.XmlAttribute att = this.Settings.DocObject.CreateAttribute(DA_FILENAME);
				att.Value = file.Name;
				nnd.Attributes.Append(att);

				// distributed assembly
				FWBS.OMS.DistributedAssemblies ds = new FWBS.OMS.DistributedAssemblies();
				if (ds.Exists(file.Name + FWBS.OMS.Session.CurrentSession.AssemblyVersion.ToString()) == false)
				{
					// create
					ds.Create();
					ds.SetSourceFileName(file.FullName, FWBS.OMS.Session.CurrentSession.AssemblyVersion.ToString());
					// upload
					ds.Update();
				}
				nd.AppendChild(nnd);
			}
		}
		#endregion

		#region ExtractDistribution
		public virtual void ExtractDistribution()
		{
			System.Xml.XmlNode nd = this.Settings.DocObject.SelectSingleNode(XPATH_DISTRIBUTIONS);
			if (nd != null)
			{
				foreach (System.Xml.XmlNode x in nd.ChildNodes)
				{
					try
					{
						System.IO.FileInfo file = new System.IO.FileInfo(x.Attributes[DA_FILENAME].Value);
						file = new System.IO.FileInfo(string.Format(@"{0}\{1}", FWBS.OMS.Session.CurrentSession.DistributedAssemblyManager.DistributedAssembliesDirectory, file.Name));
						// if file does not exist then this will always be true as LastWriteTimeUtc will be 1601
						if (file.LastWriteTimeUtc != FWBS.OMS.DistributedAssemblies.GetModifiedDateForCurrent(file.Name).ToUniversalTime())
						{
							FWBS.OMS.DistributedAssemblies ds = new FWBS.OMS.DistributedAssemblies();
							// fetch current matching version, if not found fetch any version
							ds.FetchCurrent(file.Name);
							// extract actual file from database to DA location
							file = ds.ExtractFromDatabase();
							// set file time to db time
							file.LastWriteTime = ds.Modified;
						}
					}
					catch (Exception ex)
					{
						if (this.DistributedErrors != null)
						{
							this.DistributedErrors(this, new DistributedEventArgs(ex));
						}
						else
						{
							throw;
						}
					}
				}
			}
		}
		#endregion
		#endregion

		#region References
		#region GetReferences
		public virtual HashSet<string> GetReferences()
		{
			HashSet<string> retValue = new HashSet<string>();

			System.Xml.XmlNode nd = this.Settings.DocObject.SelectSingleNode(XPATH_REFERENCES);
			if (nd != null)
			{
				foreach (System.Xml.XmlNode x in nd.ChildNodes)
				{
					retValue.Add(x.InnerText);
				}
			}

			return retValue;
		}
		#endregion

		#region SetReferences
		public virtual void SetReferences(HashSet<string> references)
		{
			System.Xml.XmlNode nd = this.Settings.DocObject.SelectSingleNode(XPATH_REFERENCES);
			if (nd == null)
			{
				nd = this.Settings.DocObject.CreateElement(XML_REFERENCES);
				XmlNode nd2 = this.Settings.DocObject.SelectSingleNode(XPATH_WORKFLOW);
				if (nd2 == null)
				{
					nd2 = this.Settings.DocObject.CreateElement(XML_WORKFLOW);
					this.Settings.DocObject.SelectSingleNode(XPATH_CONFIG).AppendChild(nd2);
				}
				nd2.AppendChild(nd);
			}

			nd.RemoveAll();
			foreach (string s in references)
			{
				System.Xml.XmlElement nnd = nd.OwnerDocument.CreateElement(XML_REFERENCE);
				System.IO.FileInfo file = new System.IO.FileInfo(s);
				nnd.InnerText = file.Name;
				nd.AppendChild(nnd);
			}
		}
		#endregion
		#endregion

		#region Scripts
		#region GetScripts
		public HashSet<string> GetScriptCodes()
		{
			HashSet<string> scriptCodes = new HashSet<string>();

			System.Xml.XmlNode nd = this.Settings.DocObject.SelectSingleNode(XPATH_WFSCRIPTS);
			if (nd != null)
			{
				foreach (System.Xml.XmlNode x in nd.ChildNodes)
				{
					scriptCodes.Add(x.Attributes[XML_SCRIPTCODE].Value);
				}
			}

			return scriptCodes;
		}
		#endregion

		#region SetScripts
		public void SetScriptCodes(HashSet<string> scriptCodes)
		{
			System.Xml.XmlNode nd = this.Settings.DocObject.SelectSingleNode(XPATH_WFSCRIPTS);
			if (nd == null)
			{
				nd = this.Settings.DocObject.CreateElement(XML_ACTIVITY_SCRIPT_CODES);
				XmlNode nd2 = this.Settings.DocObject.SelectSingleNode(XPATH_WORKFLOW);
				if (nd2 == null)
				{
					nd2 = this.Settings.DocObject.CreateElement(XML_WORKFLOW);
					this.Settings.DocObject.SelectSingleNode(XPATH_CONFIG).AppendChild(nd2);
				}
				nd2.AppendChild(nd);
			}

			nd.RemoveAll();
			foreach (string code in scriptCodes)
			{
				System.Xml.XmlElement nnd = nd.OwnerDocument.CreateElement(XML_ACTIVITY_SCRIPT_CODE);
				System.Xml.XmlAttribute att = this.Settings.DocObject.CreateAttribute(XML_SCRIPTCODE);
				att.Value = code;
				nnd.Attributes.Append(att);

				nd.AppendChild(nnd);
			}
		}
		#endregion

		#region ExtractScripts
		public void ExtractScripts(string dirPath)
		{
			// validate argument and ensure there is a '\' at end of directory path
			if (string.IsNullOrEmpty(dirPath))
			{
				dirPath = Environment.CurrentDirectory + @"\";
			}
			else if (dirPath[dirPath.Length - 1] != '\\')
			{
				dirPath += @"\";
			}

			foreach (string scriptCode in this.GetScriptCodes())
			{
				try
				{
					// create script accessor
					WorkflowScript wfScript = new WorkflowScript();
					// fetch from database
					wfScript.Fetch(scriptCode);

					// create script directory in workflow cache
					DirectoryInfo dir = new DirectoryInfo(dirPath + scriptCode);
					if (!dir.Exists)
					{
						dir.Create();
					}

					// extract the script
					wfScript.DistributedErrors += new EventHandler<DistributedEventArgs>(wfScript_DistributedErrors);
					string[] externalReferences = wfScript.ExtractScript(dir);
					wfScript.DistributedErrors -= new EventHandler<DistributedEventArgs>(wfScript_DistributedErrors);

					#region Compile the script
					// create full file name
					FileInfo file = new FileInfo(dir.FullName + @"\" + wfScript.FileName);
					// create the assembly dll name
					string assemblyFileName = dir.FullName + @"\" + scriptCode + ".dll";
					ulong versionNo = (ulong)wfScript.Version;

					// Optimise i.e. only compile if script version is different
					bool compile = true;
					if (File.Exists(assemblyFileName))
					{
						FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assemblyFileName);
						if ((fvi.FilePrivatePart == (int)(versionNo & 0x000000000000FFFF)) &&
							(fvi.FileBuildPart == (int)((versionNo >> 16) & 0x000000000000FFFF)) &&
							(fvi.FileMinorPart == (int)((versionNo >> 32) & 0x000000000000FFFF)) &&
							(fvi.FileMajorPart == (int)((versionNo >> 48) & 0x000000000000FFFF)))
						{
							// no need to compile, file version is same as version no, just load assembly
							compile = false;
						}
					}

					if (compile)
					{
						// compile the code
						System.CodeDom.Compiler.CompilerResults compilerResults = WorkflowScript.CompileCode(
							wfScript.IsCSharp ? (System.CodeDom.Compiler.CodeDomProvider)new Microsoft.CSharp.CSharpCodeProvider() : (System.CodeDom.Compiler.CodeDomProvider)new Microsoft.VisualBasic.VBCodeProvider(),
							file.FullName, assemblyFileName, externalReferences, versionNo);
						if (compilerResults.Errors.HasErrors)
						{
							// we have errors
							throw new ApplicationException("Compilation error(s) in script " + scriptCode);
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
			}
		}

		void wfScript_DistributedErrors(object sender, DistributedEventArgs e)
		{
			if (this.DistributedErrorsHandler != null)
			{
				this.DistributedErrorsHandler(this, e);
			}
			else
			{
				throw e.Exception;
			}
		}
		#endregion
		#endregion
	}
	#endregion
}
