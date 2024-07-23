#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.RunMenuScript.RunMenuScriptToolboxIcon.png")]
	[Description("Run a Menu Script")]
	[Designer(typeof(RunMenuScriptDesigner))]
	public sealed class RunMenuScript : CodeActivity<bool>
	{
		#region Constructor
		public RunMenuScript()
		{
			this.DisplayName = "Run Menu Script";
		}
		#endregion

		#region Arguments
		// Define an activity input argument of type string
		[RequiredArgument]
		public InArgument<string> ScriptCommand { get; set; }
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("ScriptCommand", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.ScriptCommand, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override bool Execute(CodeActivityContext context)
		{
			string script = this.ScriptCommand.Get(context);

			if (script.Equals(string.Empty))
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.ScriptCommand' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				FWBS.OMS.Script.MenuScriptAggregator menuScriptAgg = new FWBS.OMS.Script.MenuScriptAggregator();

				if (menuScriptAgg != null)
				{
					return menuScriptAgg.Invoke(script);
				}
			}

			return false;
		}
		#endregion
	}
}
