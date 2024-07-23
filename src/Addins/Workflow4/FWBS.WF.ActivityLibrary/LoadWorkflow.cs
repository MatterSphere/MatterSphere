#region References
using System.Activities;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
#endregion

namespace FWBS.WF.ActivityLibrary
{
    #region LoadWorkflow class
    public sealed class LoadWorkflow : CodeActivity<Activity>
	{
		#region Constants
		private const string DA_CACHE_DIRECTORY = "Distributed";// sub-directory for DA assemblies
		private const string CACHE_DIRECTORY = "Workflow";		// sub-directory for cached/copied assemblies, scripts etc
		#endregion

		#region Arguments
		[RequiredArgument]
		public InArgument<string> WorkflowCode { get; set; }
		#endregion

		#region Overrides
		// Override CacheMetadata() to stop the default reflection way of discovering the arguments
		protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			// manually add arguments - stops reflection being used for better performance
			RuntimeArgument argument = new RuntimeArgument("WorkflowCode", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.WorkflowCode, argument);
			metadata.AddArgument(argument);
		}

		protected override Activity Execute(CodeActivityContext context)
		{
			Activity retValue = null;
			string workflowName = this.WorkflowCode.Get(context);

			// get the required assemblies
			FWBS.WF.Packaging.WorkflowXaml currentWorkflow = null;
			StringReader strReader = null;
			try
			{
				currentWorkflow = new FWBS.WF.Packaging.WorkflowXaml();
				currentWorkflow.Fetch(workflowName);
				currentWorkflow.ExtractDistribution();
				currentWorkflow.ExtractScripts();
				LoadWorkflow.LoadDistributedAssemblies(currentWorkflow.GetDistributions());
				LoadWorkflow.LoadReferenceAssemblies(currentWorkflow.GetReferences());
				LoadWorkflow.LoadScriptAssemblies(currentWorkflow.GetScriptCodes());

				strReader = new StringReader(currentWorkflow.Xaml);
				retValue = ActivityXamlServices.Load(strReader);
			}
			finally
			{
				if (strReader != null)
				{
					strReader.Close();
					strReader = null;
				}

				if (currentWorkflow != null)
				{
					currentWorkflow.Dispose();
					currentWorkflow = null;
				}
			}

			// create workflow
			return retValue;
		}
		#endregion

		#region Helpers
		#region Load Distributed assemblies
		private static void LoadDistributedAssemblies(HashSet<string> distributions)
		{
			foreach (string str in distributions)
			{
				// form a FileInfo class to extract the filename
				FileInfo fileInfo = new FileInfo(str);
				// form the full filename we expect
				// NOTE: The path format depends on source file DistributedAssemblyManager.cs
				// It is used in WFRuntime.cs, LoadWorkflow.cs and WFControl.xaml.cs
				string destFileName = Path.Combine(FWBS.OMS.Session.CurrentSession.DistributedAssemblyManager.DistributedAssembliesDirectory.FullName, fileInfo.Name, fileInfo.Name);

				FWBS.OMS.Session.CurrentSession.AssemblyManager.LoadFrom(destFileName);
			}
		}
		#endregion

		#region Load Referenced assemblies
		private static void LoadReferenceAssemblies(HashSet<string> references)
		{
			FileInfo fileInfo1 = new FileInfo(Assembly.GetExecutingAssembly().Location);
			foreach (string str in references)
			{
				// try the location of this assembly
				FileInfo fileInfo = new FileInfo(fileInfo1.DirectoryName + @"\" + str);
				if (FWBS.OMS.Session.CurrentSession.AssemblyManager.TryLoadFrom(fileInfo.FullName) == null)
				{
					// try default locations with just the file name
					FWBS.OMS.Session.CurrentSession.AssemblyManager.Load(fileInfo.Name);
				}
			}
		}
		#endregion

		#region Load Script assemblies
		private static void LoadScriptAssemblies(HashSet<string> scriptCodes)
		{
			foreach (string str in scriptCodes)
			{
				FWBS.OMS.Script.ScriptGen sc = null;
				try
				{
					sc = new FWBS.OMS.Script.ScriptGen(str);
					sc.Load();
				}
				finally
				{
					if (sc != null)
					{
						sc.Dispose();
						sc = null;
					}
				}
			}
		}
		#endregion
		#endregion
	}
	#endregion
}