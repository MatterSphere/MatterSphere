namespace Horizon.Models.StartPage
{
    public class StartPageInfoItem
    {
        public StartPageInfoItem(StartPageInfoTypeEnum type, string title, string icon)
        {
            Type = type;
            Title = title;
            Icon = icon;
        }

        public StartPageInfoTypeEnum Type { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
    }
}
