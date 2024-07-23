#region References
using System;
using System.Collections.Generic;
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
				FWBS.OMS.DistributedAssemblies ds = null;
				try
				{
					ds = new FWBS.OMS.DistributedAssemblies();
					foreach (System.Xml.XmlNode x in nd.ChildNodes)
					{
						try
						{
							string fileName = x.Attributes[DA_FILENAME].Value;
							if (FWBS.OMS.Session.CurrentSession.DistributedAssemblyManager.CheckForUpdatedAssembly(fileName) != OMS.DistributedCheckResult.Ok)
							{
								// fetch current matching version, if not found fetch any version
								ds.FetchCurrent(fileName);
								// extract actual file from database to DA location
								ds.ExtractFromDatabase();
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
				finally
				{
					// get rid of object
					if (ds != null)
					{
						ds.Dispose();
						ds = null;
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
		public void ExtractScripts()
		{
			foreach (string scriptCode in this.GetScriptCodes())
			{
				try
				{
					// create script accessor
					FWBS.OMS.Script.ScriptGen sc = new OMS.Script.ScriptGen(scriptCode);
					sc.Compile();
					sc.Dispose();
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
		#endregion
		#endregion
	}
	#endregion
}
