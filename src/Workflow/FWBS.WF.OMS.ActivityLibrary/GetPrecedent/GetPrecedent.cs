#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.GetPrecedent.GetPrecedentToolboxIcon.png")]
	[Description("Get Precedent")]
	[Designer(typeof(GetPrecedentDesigner))]
	public sealed class GetPrecedent : CodeActivity<FWBS.OMS.Precedent>
	{
		#region Arguments
        public InArgument<string> Title { get; set; }
		public InArgument<string> Type { get; set; }
        public InArgument<long> ID { get; set; }
        public InArgument<FWBS.OMS.Associate> Associate { get; set; }
        public InArgument<string> Library { get; set; }
		public InArgument<string> Category { get; set; }
		public InArgument<string> Subcategory { get; set; }
		public InArgument<string> Language { get; set; }
		#endregion

		#region Constructor
		public GetPrecedent()
		{
			this.DisplayName = "Get Precedent";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
		protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("Title", typeof(string), ArgumentDirection.In);
			metadata.Bind(this.Title, argument);
			metadata.AddArgument(argument);

            argument = new RuntimeArgument("Type", typeof(string), ArgumentDirection.In);
            metadata.Bind(this.Type, argument);
            metadata.AddArgument(argument);

            argument = new RuntimeArgument("ID", typeof(long), ArgumentDirection.In);
            metadata.Bind(this.ID, argument);
            metadata.AddArgument(argument);

            argument = new RuntimeArgument("Associate", typeof(FWBS.OMS.Associate), ArgumentDirection.In);
            metadata.Bind(this.Associate, argument);
            metadata.AddArgument(argument);

			argument = new RuntimeArgument("Library", typeof(string), ArgumentDirection.In);
			metadata.Bind(this.Library, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("Category", typeof(string), ArgumentDirection.In);
			metadata.Bind(this.Category, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("Subcategory", typeof(string), ArgumentDirection.In);
			metadata.Bind(this.Subcategory, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("Language", typeof(string), ArgumentDirection.In);
			metadata.Bind(this.Language, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override FWBS.OMS.Precedent Execute(CodeActivityContext context)
		{
			string title = Title.Get(context);
			string type = Type.Get(context);
			string library = Library.Get(context);
			string category = Category.Get(context);
			string subcategory = Subcategory.Get(context);
			string language = Language.Get(context);
            long id = ID.Get(context);
            FWBS.OMS.Associate associate = Associate.Get(context);

            if (id > 0)
            {
                //  Use the ID only if supplied
                //  Please Note: if the database changes the ID my be different on the changed database and will not return the correct Precedent.
                return FWBS.OMS.Precedent.GetPrecedent(id);
            }
            else if (title != null && associate != null)
            {
                //  Use the Title and Associate combination
                return FWBS.OMS.Precedent.GetPrecedent(title, associate);
            }
            else if (title != null && type == null && category == null && subcategory == null)
            {
                //  Use the Title and a null Associate if Type, Category and Subcategory are all null
                return FWBS.OMS.Precedent.GetPrecedent(title, null);
            }
            else
            {
                //  Else use the full arguments
                return FWBS.OMS.Precedent.GetPrecedent(title, type, library, category, subcategory, language);
            }
		}
		#endregion
	}
}
