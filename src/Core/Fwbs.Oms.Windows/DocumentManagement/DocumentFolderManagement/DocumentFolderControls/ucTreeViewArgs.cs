using System;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement.DocumentFolderControls
{
    public class ucTreeViewArgs
    {
        public bool IncludeApplyToAllCheckBox { get; set; }

        public string ApplyToAllText { get; set; }

        public bool ApplyToAllValue { get; set; }

        public OMSFile OMSFile { get; set; }
        
        public Guid SelectedFolder { get; set; }

        public string SelectedFolderText { get; set; }
    }
}
