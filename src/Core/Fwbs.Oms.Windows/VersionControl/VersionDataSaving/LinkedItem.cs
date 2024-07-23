using System;
using System.Data;

namespace FWBS.OMS.UI.Windows
{
    public class LinkedItem
    {
        public string Code { get; set; }
        public int Version { get; set; }
        public DataSet VersionData { get; set; }
        public Guid VersionID { get; set; }
        public string Destination { get; set; }
        public LinkType Type { get; set; }
    }


    public enum LinkType
    { 
        EnquiryForm,
        SearchList,
        DataList,
        Precedent,
        Script,
        FileManagement
    }
}
