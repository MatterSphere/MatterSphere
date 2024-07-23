#region References
using System;
using System.Activities;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using FWBS.Common;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    public enum SearchKind
    {
        SearchGroup,
        SearchList
    }

	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.ShowSearch.ShowSearchToolboxIcon.png")]
	[Description("Shows a Search")]
	[Designer(typeof(ShowSearchDesigner))]
	public sealed class ShowSearch : CodeActivity<KeyValueCollection>
	{       
		#region Arguments
		// Define an activity input argument of type string
		[RequiredArgument]
		public InArgument<string> Code { get; set; }

        public InArgument<object> Parent { get; set; }        
         
        public InArgument<Size?> Size { get; set; }

        public SearchKind SearchKind { get; set; }

		[Browsable(true)]
		[Editor(typeof(ParametersPropertyValueEditor), typeof(DialogPropertyValueEditor))]
		public ObservableCollection<KeyParameters> Parameters { get; set; }
		#endregion

		#region Constructor
		public ShowSearch()
		{
			this.DisplayName = "Show Search";

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

            argument = new RuntimeArgument("Size", typeof(Size?), ArgumentDirection.In);
            metadata.Bind(this.Size, argument);
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
		protected override KeyValueCollection Execute(CodeActivityContext context)
		{
			var type = Code.Get(context);
			var parent = Parent.Get(context);

            Size? size = Size.Get(context);

            if (!size.HasValue)
                size = new Size(-1, -1);

            bool isGroup = SearchKind == SearchKind.SearchGroup;

			ObservableCollection<KeyParameters> parameters = this.Parameters;

			if (type == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Type' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				KeyValueCollection kvc = new KeyValueCollection();
				foreach (var item in parameters)
				{
					object obj = context.GetValue(item.Value);
					kvc.Add(item.Key, obj);
				}

                return FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(null, type, isGroup, size.Value, parent, kvc);
			}
		}
		#endregion
	}
}
