using System;

namespace FWBS.OMS.Workflow
{
	public class Constants
	{
		internal const string CACHE_DIRECTORY = "Workflow";		// sub-directory for workflow cached/copied assemblies, scripts etc

		// Logging source name
		internal const string TRACE_SOURCE_NAME = "FWBS_WFDesigner";
		internal const string TRACE_ACTIVITY_SOURCE_NAME = "FWBS_WFRuntime";

		//
		// AppDomain slot names for running workflow within admin kit
		//
		internal const string APPDOMAIN_WFCODE = "WorkflowCode";					// workflow code
		internal const string APPDOMAIN_WFXAML = "WorkflowXAML";					// workflow XAML
		internal const string APPDOMAIN_WFDISTRIBUTIONS = "WorkflowDistribution";	// workflow distribution list
		internal const string APPDOMAIN_WFSCRIPTCODES = "WorkflowScriptCodes";		// workflow script codes list
		internal const string APPDOMAIN_WFREFERENCES = "WorkflowReferences";		// workflow references

		internal const string SCRIPTTYPE = "WORKFLOW";						// workflow script type in dbScript

		internal const string CODELOOKUPTYPE = "WORKFLOW";
		internal const string CODELOOKUP_GROUPTYPE = "WFGROUP";
	}
}
