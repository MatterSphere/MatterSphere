namespace FWBS.OMS.UI.Windows
{
    public class ScriptVersionDataArchiver : BaseVersionDataArchiver
    {
        public ScriptVersionDataArchiver()
        {
            Destination = VersionTables.Script;
        }

        public override void ProcessLinkedItems()
        {
            var linkedItems = new LinkedItemVersionDataArchiver<ScriptVersionDataArchiver>(this);
            linkedItems.ProcessLinkedItems();
        }
    }


    public class EnquiryFormVersionDataArchiver : BaseVersionDataArchiver
    {
        public EnquiryFormVersionDataArchiver()
        {
            Destination = VersionTables.EnquiryForm;
        }

        public override void ProcessLinkedItems()
        {
            var linkedItems = new LinkedItemVersionDataArchiver<EnquiryFormVersionDataArchiver>(this);
            linkedItems.ProcessLinkedItems();
        }
    }


    public class SearchListVersionDataArchiver : BaseVersionDataArchiver
    {
        public SearchListVersionDataArchiver()
        {
            Destination = VersionTables.SearchList;
        }

        public override void ProcessLinkedItems()
        {
            var linkedItems = new LinkedItemVersionDataArchiver<SearchListVersionDataArchiver>(this);
            linkedItems.ProcessLinkedItems();
        }
    }


    public class DataListVersionDataArchiver : BaseVersionDataArchiver
    {
        public DataListVersionDataArchiver()
        {
            Destination = VersionTables.DataList;
        }

        public override void ProcessLinkedItems()
        {

        }
    }


    public class PrecedentVersionDataArchiver : BaseVersionDataArchiver
    {
        public PrecedentVersionDataArchiver()
        {
            Destination = VersionTables.Precedent;
        }

        public override void ProcessLinkedItems()
        {
            var linkedItems = new LinkedItemVersionDataArchiver<PrecedentVersionDataArchiver>(this);
            linkedItems.ProcessLinkedItems();
        }
    }


    public class FileManagementVersionDataArchiver : BaseVersionDataArchiver
    {
        public FileManagementVersionDataArchiver()
        {
            Destination = VersionTables.FileManagement;
        }

        public override void ProcessLinkedItems()
        {
            var linkedItems = new LinkedItemVersionDataArchiver<FileManagementVersionDataArchiver>(this);
            linkedItems.ProcessLinkedItems();
        }
    }
}
