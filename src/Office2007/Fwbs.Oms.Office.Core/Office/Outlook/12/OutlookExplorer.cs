namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;


    partial class OutlookExplorer 
    {

        public MSOutlook.NavigationPane NavigationPane
        {
            get { return explorer.NavigationPane; }
        }

        public void Search(string Query, MSOutlook.OlSearchScope SearchScope)
        {
            explorer.Search(Query, SearchScope);
        }

        public void ClearSearch()
        {
            explorer.ClearSearch();
        }

    }
}
