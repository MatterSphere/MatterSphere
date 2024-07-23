#region References
using System;
using System.Activities;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

#if !ANOTHERDOMAIN
using FWBS.WF.Packaging;
using FWBS.OMS.WFRuntime.Context;
using Elite.Workflow.Framework.Client.ExecutionEngine;
using Elite.Workflow.Framework.Client.ExecutionEngine.WorkflowService;
using Elite.Workflow.Framework.Client.WorkflowToken;
#endif
#endregion

namespace FWBS.OMS.Workflow
{
    public class WFRuntime
	{
		#region Constants
		private const string SPECIFICDATA_WORKFLOWSERVER = "WORKFLOWSERVER";
		private const string SPECIFICDATA_WORKFLOWTOKEN = "WORKFLOWTOKEN";
		#endregion

		#region Constructor
		private WFRuntime() {}
		#endregion

#if !ANOTHERDOMAIN
		#region Execute workflow
		/// <summary>
		/// Executes a workflow synchronously.
		///		Any exceptions thrown during execution are not caught and it is up to the caller to handle them.
		///		This method should be used for executing client-side workflows.
		///		If the workflow is a server-side workflow, timeOut argument is ignored and workflow token is not returned
		/// </summary>
		/// <param name="workflowCode">
		/// The code of workflow definition to be executed as in the storage/database
		/// </param>
		/// <param name="timeOut">
		/// The time interval in which the workflow must complete before it is aborted and a TimeoutException is thrown
		/// </param>
		/// <param name="workflowParameters">
		/// The dictionary of input parameters to the workflow, keyed by argument name.
		/// </param>
		/// <param name="context">
		/// The FWBS context interface.
		/// </param>
		/// <returns>The dictionary of the root activity’s OutArgument and InOutArgument values keyed by argument name that represent the outputs of the workflow</returns>
		public static IDictionary<string, object> Execute(
			string workflowCode,
			TimeSpan timeOut,
			IDictionary<string, object> workflowInParameters,
			FWBS.OMS.IContext context
			)
		{
			FWBS.Logging.TraceLogger logger = FWBS.Logging.TraceSourceFactory.CreateTraceSource(FWBS.OMS.Workflow.Constants.TRACE_SOURCE_NAME, 1100);

			IDictionary<string, object> retValue = new Dictionary<string, object>();
			FWBS.WF.Packaging.WorkflowXaml currentWorkflow = null;
			StringReader strReader = null;

			//
			// Note: We do not catch the exceptions to allow the script writer decide what he/she wants to do with them...
			//
			try
			{
				logger.TraceVerbose("WFRuntime.Execute({0})", new object[] { workflowCode });

				// get the required assemblies
				currentWorkflow = new FWBS.WF.Packaging.WorkflowXaml();
				currentWorkflow.Fetch(workflowCode);
				// Check whether this can be run on the server
				if (currentWorkflow.IsServerWorkflow)
				{
					// Run workflow on the server
					WFRuntime.Execute(workflowCode, workflowInParameters, context);
				}
				else
				{
					currentWorkflow.ExtractDistribution();
					currentWorkflow.ExtractScripts();
					WFRuntime.LoadAssemblies(logger, currentWorkflow.GetDistributions());
					WFRuntime.LoadReferencedAssemblies(logger, currentWorkflow.GetReferences());
					WFRuntime.LoadScriptAssemblies(logger, currentWorkflow.GetScriptCodes());

					// create workflow
					strReader = new StringReader(currentWorkflow.Xaml);
					Activity activity = ActivityXamlServices.Load(strReader);
					// create wf4 runtime object
					WorkflowInvoker wfInvoker = new WorkflowInvoker(activity);
					// add tracking
					wfInvoker.Extensions.Add(new TrackingExtension());
					// add context
					if (context != null)
					{
						wfInvoker.Extensions.Add(context);
					}
					// execute workflow
					if (workflowInParameters != null)
					{
						retValue = wfInvoker.Invoke(workflowInParameters, timeOut);
					}
					else
					{
						retValue = wfInvoker.Invoke(timeOut);
					}
				}
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
				
				logger.TraceVerbose("WFRuntime.Execute() - End");
				logger.Close();
			}

			return retValue;
		}
		#endregion

		#region Execute workflow
		/// <summary>
		/// Executes a server workflow.
		///		Any exceptions thrown during execution are not caught and it is up to the caller to handle them.
		///		This method is used for executing server-side workflows
		/// </summary>
		/// <param name="workflowCode">
		/// The code of workflow definition to be executed as in the storage/database
		/// </param>
		/// <param name="workflowParameters">
		/// The dictionary of input parameters to the workflow, keyed by argument name.
		/// </param>
		/// <param name="context">
		/// The FWBS context interface.
		/// </param>
		/// <returns>The workflow token</returns>
		public static string Execute(
			string workflowCode,
			IDictionary<string, object> workflowInParameters,
			FWBS.OMS.IContext fwbsContext,
			Guid? mappingId = null,
			Guid? correlationId = null
			)
		{
			FWBS.Logging.TraceLogger logger = FWBS.Logging.TraceSourceFactory.CreateTraceSource(FWBS.OMS.Workflow.Constants.TRACE_SOURCE_NAME, 1100);

			//
			// Note: We do not catch the exceptions to allow the script writer decide what he/she wants to do with them...
			//
			try
			{
				// try to extract from current context
				if (workflowInParameters == null)
				{
					workflowInParameters = new Dictionary<string, object>();
				}
				WFRuntime.ExtractContext(workflowInParameters, fwbsContext);
				if (fwbsContext != null)
				{
					// try to extract from current context's parent context as well
					WFRuntime.ExtractContext(workflowInParameters, fwbsContext.Parent);
				}
				if (workflowInParameters.Count == 0)
				{
					workflowInParameters = null;
				}
				return WFRuntime.Execute(workflowCode, workflowInParameters, mappingId, correlationId);
			}
			finally
			{
				logger.TraceVerbose("WFRuntime.Execute() - End");
				logger.Close();
			}

			return string.Empty;
		}
        #endregion

        #region Execute workflow
        /// <summary>
        /// Executes a server workflow.
        ///		Any exceptions thrown during execution are not caught and it is up to the caller to handle them.
        ///		This method is used for executing server-side workflows
        /// </summary>
        /// <param name="workflowCode">
        /// The code of workflow definition to be executed as in the storage/database
        /// </param>
        /// <param name="workflowInParameters">
        /// The dictionary of input parameters to the workflow, keyed by argument name.
        /// </param>
        /// <param name="mappingId"></param>
        /// <param name="correlationId">The correlation identifier for server side logging</param>
        /// <returns>The workflow token</returns>
        public static string Execute(
            string workflowCode,
            IDictionary<string, object> workflowInParameters,
			Guid? mappingId = null,
			Guid? correlationId = null
            )
        {
            FWBS.Logging.TraceLogger logger = FWBS.Logging.TraceSourceFactory.CreateTraceSource(FWBS.OMS.Workflow.Constants.TRACE_SOURCE_NAME, 1100);
			string token = string.Empty;
			ExecutionEngine engine = null;
			Elite.Workflow.Framework.Client.WorkflowToken.TokenMetadata tokenMetadata = null;

            //
            // Note: We do not catch the exceptions to allow the script writer decide what he/she wants to do with them...
            //
			try
			{
				logger.TraceVerbose("WFRuntime.Execute({0})", new object[] { workflowCode });

				string url = (string)FWBS.OMS.Session.CurrentSession.GetSpecificData(SPECIFICDATA_WORKFLOWSERVER);
				string tokenUri = (string)FWBS.OMS.Session.CurrentSession.GetSpecificData(SPECIFICDATA_WORKFLOWTOKEN);
				if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(tokenUri))
				{
					engine = new ExecutionEngine(new Uri(url));
					tokenMetadata = new Elite.Workflow.Framework.Client.WorkflowToken.TokenMetadata(new Uri(tokenUri));

					// ignore version for the moment
					NewExecuteRequest request = new NewExecuteRequest()
					{
						WorkflowCode = workflowCode,
						WorkflowArguments = (Dictionary<string, object>)workflowInParameters,
						ResultFormat = ResultFormat.Binary,
						MappingID = mappingId == null ? Guid.NewGuid() : mappingId.Value,
						CorrelationID = correlationId == null ? Guid.NewGuid() : correlationId.Value,
						SynchronousExecution = false,
					};
					NewExecuteResponse response = engine.New(request);
					if (response.Error == ErrorCode.Success)
					{
						token = response.WorkflowToken;

						#region Save to token service
						Dictionary<string, string> tokenArgs = new Dictionary<string,string>();
						tokenArgs.Add(ArgumentNames.TokenInvokingUserId, FWBS.OMS.Session.CurrentSession.CurrentUser.ID.ToString());
						// go through the parameters supplied and save any matching key names with correct value type
						object value = null;
						if (workflowInParameters.TryGetValue(ArgumentNames.AssociateId, out value) &&
							value is long)
						{
							tokenArgs.Add(ArgumentNames.TokenAssociateId, value.ToString());
						}
						if (workflowInParameters.TryGetValue(ArgumentNames.ClientId, out value) &&
							value is long)
						{
							tokenArgs.Add(ArgumentNames.TokenClientId, value.ToString());
						}
						if (workflowInParameters.TryGetValue(ArgumentNames.ContactId, out value) &&
							value is long)
						{
							tokenArgs.Add(ArgumentNames.TokenContactId, value.ToString());
						}
						if (workflowInParameters.TryGetValue(ArgumentNames.DocumentId, out value) &&
							value is long)
						{
							tokenArgs.Add(ArgumentNames.TokenDocumentId, value.ToString());
						}
						if (workflowInParameters.TryGetValue(ArgumentNames.DocumentVersion, out value) &&
							value is long)
						{
							tokenArgs.Add(ArgumentNames.TokenDocumentVersion, value.ToString());
						}
						if (workflowInParameters.TryGetValue(ArgumentNames.FeeEarnerId, out value) &&
							value is long)
						{
							tokenArgs.Add(ArgumentNames.TokenFeeEarnerId, value.ToString());
						}
						if (workflowInParameters.TryGetValue(ArgumentNames.FileId, out value) &&
							value is long)
						{
							tokenArgs.Add(ArgumentNames.TokenFileId, value.ToString());						
						}
						if (workflowInParameters.TryGetValue(ArgumentNames.PrecedentId, out value) &&
							value is long)
						{
							tokenArgs.Add(ArgumentNames.TokenPrecedentId, value.ToString());
						}
						if (workflowInParameters.TryGetValue(ArgumentNames.UserId, out value) &&
							value is long)
						{
							tokenArgs.Add(ArgumentNames.TokenUserId, value.ToString());
						}
						if (workflowInParameters.TryGetValue(ArgumentNames.BranchId, out value) &&
							value is long)
						{
							tokenArgs.Add(ArgumentNames.TokenBranchId, value.ToString());
						}

						Elite.Workflow.Framework.Client.WorkflowToken.TokenMetadataService.NewWorkflowTokenMetadataRequest tokenRequest = new Elite.Workflow.Framework.Client.WorkflowToken.TokenMetadataService.NewWorkflowTokenMetadataRequest()
						{
							 WorkflowToken = response.WorkflowToken,
							 Metadata = tokenArgs,
							 DoFailIfUpdate = false,
						};

						Elite.Workflow.Framework.Client.WorkflowToken.TokenMetadataService.NewWorkflowTokenMetadataResponse tokenResponse = tokenMetadata.NewWorkflowTokenMetadata(tokenRequest);
						if (tokenResponse.Error != Elite.Workflow.Framework.Client.WorkflowToken.TokenMetadataService.ErrorCode.Success)
						{
							logger.TraceError("Token Service response error; " + response.Error.ToString());
							throw new InvalidOperationException("Error correlation id = " + response.CorrelationID.ToString("D"), response.Exception);
						}
						#endregion
					}
					else
					{
						// error
						logger.TraceError("Service response error; " + response.Error.ToString());
						throw new InvalidOperationException("Error correlation id = " + response.CorrelationID.ToString("D"), response.Exception);
					}
				}
				else
				{
					throw new ArgumentException("URI");
				}
			}
			finally
			{
				if (engine != null)
				{
					engine.Dispose();
					engine = null;
				}

				if (tokenMetadata != null)
				{
					tokenMetadata.Dispose();
					tokenMetadata = null;
				}

				logger.TraceVerbose("WFRuntime.Execute() - End");
				logger.Close();
			}

            return token;
        }
        #endregion

		#region Resume workflow
		/// <summary>
		/// Resumes a workflow intances that has been bookmarked
		///		Any exceptions thrown during execution are not caught and it is up to the caller to handle them.
		///		This method is used for executing server-side workflows
		/// </summary>
		/// <param name="workflowToken">The workflow instance token</param>
		/// <param name="bookmark">The bookmark name. If null then the engine will attempt to resume wherever the instance is</param>
		/// <param name="bookmarkParameter">Optional bookmark argument</param>
		/// <param name="correlationId">The correlation identifier for server side logging</param>
		/// <returns></returns>
		public static bool Resume(
			string workflowToken,
			string bookmark,
			object bookmarkParameter = null,
			Guid? correlationId = null
			)
		{
			bool retValue = false;
			FWBS.Logging.TraceLogger logger = FWBS.Logging.TraceSourceFactory.CreateTraceSource(FWBS.OMS.Workflow.Constants.TRACE_SOURCE_NAME, 1100);
			string token = string.Empty;
			ExecutionEngine engine = null;

			//
			// Note: We do not catch the exceptions to allow the script writer decide what he/she wants to do with them...
			//
			try
			{
				logger.TraceVerbose("WFRuntime.Resume({0},{1})", new object[] { workflowToken, bookmark ?? "null" });

				string url = (string)FWBS.OMS.Session.CurrentSession.GetSpecificData(SPECIFICDATA_WORKFLOWSERVER);
				if (!string.IsNullOrWhiteSpace(url))
				{
					engine = new ExecutionEngine(new Uri(url));

					// ignore version for the moment
					ResumeWorkflowInstanceRequest request = new ResumeWorkflowInstanceRequest()
					{
						WorkflowToken = workflowToken,
						Bookmark = bookmark,
						BookmarkArgument = bookmarkParameter,
						CorrelationID = correlationId == null ? Guid.NewGuid() : correlationId.Value,
					};
					ResumeWorkflowInstanceResponse response = engine.ResumeBookmark(request);
					if (response.Error == ErrorCode.Success)
					{
						retValue = true;
					}
					else
					{
						// error
						logger.TraceError("Service response error; " + response.Error.ToString());
						throw new InvalidOperationException("Error correlation id = " + response.CorrelationID.ToString("D"), response.Exception);
					}
				}
				else
				{
					throw new ArgumentException("URI");
				}
			}
			finally
			{
				if (engine != null)
				{
					engine.Dispose();
					engine = null;
				}

				logger.TraceVerbose("WFRuntime.Resume() - End");
				logger.Close();
			}

			return retValue;
		}
		#endregion

		#region Helpers
		private static void ExtractContext(
			IDictionary<string, object> arguments,
			FWBS.OMS.IContext fwbsContext)
		{
			if (fwbsContext != null)
			{
				if (!arguments.ContainsKey(ArgumentNames.AssociateId))
				{
					Associate assoc = fwbsContext.Get<object>(typeof(Associate)) as Associate;
					if (assoc != null)
					{
						arguments.Add(ArgumentNames.AssociateId, assoc.ID);
					}
				}
				if (!arguments.ContainsKey(ArgumentNames.ClientId))
				{
					Client client = fwbsContext.Get<object>(typeof(Client)) as Client;
					if (client != null)
					{
						arguments.Add(ArgumentNames.ClientId, client.ID);
					}
				}
				if (!arguments.ContainsKey(ArgumentNames.ContactId))
				{
					Contact contact = fwbsContext.Get<object>(typeof(Contact)) as Contact;
					if (contact != null)
					{
						arguments.Add(ArgumentNames.ContactId, contact.ID);
					}
				}
				if (!arguments.ContainsKey(ArgumentNames.DocumentId))
				{
					OMSDocument document = fwbsContext.Get<object>(typeof(OMSDocument)) as OMSDocument;
					if (document != null)
					{
						arguments.Add(ArgumentNames.DocumentId, document.ID);
					}
				}
				if (!arguments.ContainsKey(ArgumentNames.DocumentVersion))
				{
					FWBS.OMS.DocumentManagement.DocumentVersion documentVersion = fwbsContext.Get<object>(typeof(FWBS.OMS.DocumentManagement.DocumentVersion)) as FWBS.OMS.DocumentManagement.DocumentVersion;
					if (documentVersion != null)
					{
						arguments.Add(ArgumentNames.DocumentVersion, documentVersion.Version);
					}
				}
				if (!arguments.ContainsKey(ArgumentNames.FeeEarnerId))
				{
					FeeEarner feeEarner = fwbsContext.Get<object>(typeof(FeeEarner)) as FeeEarner;
					if (feeEarner != null)
					{
						arguments.Add(ArgumentNames.FeeEarnerId, feeEarner.ID);
					}
				}
				if (!arguments.ContainsKey(ArgumentNames.FileId))
				{
					OMSFile file = fwbsContext.Get<object>(typeof(OMSFile)) as OMSFile;
					if (file != null)
					{
						arguments.Add(ArgumentNames.FileId, file.ID);
					}
				}
				if (!arguments.ContainsKey(ArgumentNames.PrecedentId))
				{
					Precedent precedent = fwbsContext.Get<object>(typeof(Precedent)) as Precedent;
					if (precedent != null)
					{
						arguments.Add(ArgumentNames.PrecedentId, precedent.ID);
					}
				}
				if (!arguments.ContainsKey(ArgumentNames.UserId))
				{
					User user = fwbsContext.Get<object>(typeof(User)) as User;
					if (user != null)
					{
						arguments.Add(ArgumentNames.UserId, user.ID);
					}
				}
				if (!arguments.ContainsKey(ArgumentNames.BranchId))
				{
					if (FWBS.OMS.Session.CurrentSession.CurrentBranch != null)
					{
						arguments.Add(ArgumentNames.BranchId, FWBS.OMS.Session.CurrentSession.CurrentBranch.ID);
					}
				}
			}
		}
		#endregion
#else
		#region Execute workflow
		/// <summary>
		/// INTERNAL USE ONLY within the adminkit.
		/// Executes a workflow synchronously.
		///		Any exceptions thrown during execution are not caught and it is up to the caller to handle them.
		///		The routine is for internal use only for running workflow within the admin kit via another appdomain
		/// </summary>
		/// <param name="workflowCode">
		/// The code of workflow definition to be executed as in the storage/database - can be empty string
		/// </param>
		/// <param name="timeOut">
		/// The time interval in which the workflow must complete before it is aborted and a TimeoutException is thrown
		/// </param>
		/// <param name="workflowParameters">
		/// The dictionary of input parameters to the workflow, keyed by argument name.
		/// </param>
		/// <param name="workflowXaml">
		/// The XAML representation of the workflow.
		/// </param>
		/// <param name="workflowDistributions">
		/// The distributed assemblies the workflow is dependent on.
		/// </param>
		/// <param name="workflowScriptCodes">
		/// The workflow script codes that the workflow is dependent on.
		/// </param>
		/// <param name="context">
		/// The FWBS context interface.
		/// </param>
		/// <returns>The dictionary of the root activity’s OutArgument and InOutArgument values keyed by argument name that represent the outputs of the workflow</returns>
		internal static IDictionary<string, object> Execute(
			TimeSpan timeOut,
			IDictionary<string, object> workflowInParameters,
			string workflowXaml,
			HashSet<string> workflowDistributions,
			HashSet<string> workflowReferences,
			HashSet<string> workflowScriptCodes,
			FWBS.OMS.IContext context
			)
		{
			FWBS.Logging.TraceLogger logger = FWBS.Logging.TraceSourceFactory.CreateTraceSource(FWBS.OMS.Workflow.Constants.TRACE_SOURCE_NAME, 1100);

			IDictionary<string, object> retValue = new Dictionary<string, object>();
			StringReader strReader = null;

			//
			// Note: We do not catch the exceptions to allow the script writer decide what he/she wants to do with them...
			//	We do not get any distributed assemblies or script. If these are already on the workflow designer then the assemblies should exist!
			//
			try
			{
				logger.TraceVerbose("WFRuntime.Execute(XAML)");

				// load dependencies
				WFRuntime.LoadAssemblies(logger, workflowDistributions);
				WFRuntime.LoadReferencedAssemblies(logger, workflowReferences);
				WFRuntime.LoadScriptAssemblies(logger, workflowScriptCodes);

				// create workflow runtime and execute
				strReader = new StringReader(workflowXaml);
				Activity activity = ActivityXamlServices.Load(strReader);
				// Run workflow
				WorkflowInvoker wfInvoker = new WorkflowInvoker(activity);
				wfInvoker.Extensions.Add(new TrackingExtension());
				// add context
				if (context != null)
				{
					wfInvoker.Extensions.Add(context);
				}
				if (workflowInParameters != null)
				{
					retValue = wfInvoker.Invoke(workflowInParameters, timeOut);
				}
				else
				{
					retValue = wfInvoker.Invoke(timeOut);
				}
			}
			finally
			{
				if (strReader != null)
				{
					strReader.Close();
					strReader = null;
				}

				logger.TraceVerbose("WFRuntime.Execute(XAML) - End");
				logger.Close();
			}

			return retValue;
		}
		#endregion
#endif

		#region Load Distributed assemblies
		private static void LoadAssemblies(FWBS.Logging.TraceLogger logger, HashSet<string> distributions)
		{
			if (logger != null)
			{
				logger.TraceVerbose("WFRuntime.LoadAssemblies()");
			}

			foreach (string str in distributions)
			{
				if (logger != null)
				{
					logger.TraceVerbose("WFRuntime.LoadAssemblies - Processing {0}", new object[] { str });
				}

				// form the full filename we expect for a distributed assembly
				FileInfo fileInfo = new FileInfo(str);
				// NOTE: The path format depends on source file DistributedAssemblyManager.cs
				// It is used in WFRuntime.cs, LoadWorkflow.cs and WFControl.xaml.cs
				string destFileName = Path.Combine(FWBS.OMS.Session.CurrentSession.DistributedAssemblyManager.DistributedAssembliesDirectory.FullName, fileInfo.Name, fileInfo.Name);

				// load the assembly
				WFRuntime.LoadFromOrLoadAssembly(destFileName);
			}

			if (logger != null)
			{
				logger.TraceVerbose("WFRuntime.LoadAssemblies() - End");
			}
		}
		#endregion

		#region Load Script assemblies
		private static void LoadScriptAssemblies(FWBS.Logging.TraceLogger logger, HashSet<string> scriptCodes)
		{
			if (logger != null)
			{
				logger.TraceVerbose("WFRuntime.LoadScriptAssemblies()");
			}

			foreach (string str in scriptCodes)
			{
				if (logger != null)
				{
					logger.TraceVerbose("WFRuntime.LoadScriptAssemblies - Processing {0}", new object[] { str });
				}

				Script.ScriptGen sc = null;
				try
				{
					sc = new Script.ScriptGen(str);
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

			if (logger != null)
			{
				logger.TraceVerbose("WFRuntime.LoadScriptAssemblies() - End");
			}
		}
		#endregion

		#region Load Referenced assemblies
		private static void LoadReferencedAssemblies(FWBS.Logging.TraceLogger logger, HashSet<string> references)
		{
			if (logger != null)
			{
				logger.TraceVerbose("WFRuntime.LoadReferencedAssemblies()");
			}

			FileInfo fileInfo1 = new FileInfo(Assembly.GetExecutingAssembly().Location);
			foreach (string str in references)
			{
				if (logger != null)
				{
					logger.TraceVerbose("WFRuntime.LoadReferencedAssemblies - Processing {0}", new object[] { str });
				}

				// try the location of this assembly
				FileInfo fileInfo = new FileInfo(fileInfo1.DirectoryName + @"\" + str);
				if (FWBS.OMS.Session.CurrentSession.AssemblyManager.TryLoadFrom(fileInfo.FullName) == null)
				{
					// try default locations with just the file name
					WFRuntime.LoadFromOrLoadAssembly(str);
				}
			}

			if (logger != null)
			{
				logger.TraceVerbose("WFRuntime.LoadReferencedAssemblies() - End");
			}
		}
		#endregion

		#region LoadFromOrLoadAssembly
		// Try to load using LoadFrom() - if full path is given no problem, otherwise the current directory is searched for the file.
		//	If the file is not found then try to load using Load()//
		//	NOTE: If an assembly with the same identity is already loaded, LoadFrom returns the loaded assembly even if a different path is specified!
		internal static Assembly LoadFromOrLoadAssembly(string assemblyFileName)
		{
			Assembly ass = FWBS.OMS.Session.CurrentSession.AssemblyManager.TryLoadFrom(assemblyFileName);
			if (ass == null)
			{
				ass = FWBS.OMS.Session.CurrentSession.AssemblyManager.Load(assemblyFileName);
			}

			return ass;
		}
		#endregion
	}
}
