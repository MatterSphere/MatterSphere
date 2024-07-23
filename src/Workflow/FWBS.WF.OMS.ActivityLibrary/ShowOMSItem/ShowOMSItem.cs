#region References
using System;
using System.Activities;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.ShowOMSItem.ShowOMSItemToolboxIcon.png")]
	[Description("Shows an Item")]
	[Designer(typeof(ShowOMSItemDesigner))]
	public sealed class ShowOMSItem : CodeActivity<object>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<string> Code { get; set; }

		[RequiredArgument]
		public InArgument<object> Parent { get; set; }

		[Browsable(true)]
		public FWBS.OMS.EnquiryEngine.EnquiryMode Mode { get; set; }

		[Browsable(true)]
		[Editor(typeof(ParametersPropertyValueEditor), typeof(DialogPropertyValueEditor))]
		public ObservableCollection<KeyParameters> Parameters { get; set; }
		#endregion

		#region Constructor
		public ShowOMSItem()
		{
			this.DisplayName = "Show OMS Item";

			if (Parameters == null)
			{
				Parameters = new System.Collections.ObjectModel.ObservableCollection<KeyParameters>();
			}
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("Code", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.Code, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("Parent", typeof(object), ArgumentDirection.In);
			metadata.Bind(this.Parent, argument);
			metadata.AddArgument(argument);

			foreach (var item in this.Parameters)
			{
				RuntimeArgument runTimeArg = new RuntimeArgument(item.Key, item.Value.ArgumentType, item.Value.Direction, false);
				metadata.Bind(item.Value, runTimeArg);
				metadata.AddArgument(runTimeArg);
			}
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override object Execute(CodeActivityContext context)
		{
			var code = Code.Get(context);
			var parent = Parent.Get(context);
			ObservableCollection<KeyParameters> parameters = this.Parameters;

			if (code == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Code' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				FWBS.Common.KeyValueCollection replacementParameters = new Common.KeyValueCollection();

				if (parameters == null)
				{
					return FWBS.OMS.UI.Windows.Services.ShowOMSItem(code, parent, this.Mode, new FWBS.Common.KeyValueCollection());
				}
				else
				{
					foreach (var item in parameters)
					{
						object obj = context.GetValue(item.Value);
						replacementParameters.Add(item.Key, obj);
					}
					return FWBS.OMS.UI.Windows.Services.ShowOMSItem(code, parent, this.Mode, replacementParameters);
				}
			}
		}
		#endregion
	}
}
