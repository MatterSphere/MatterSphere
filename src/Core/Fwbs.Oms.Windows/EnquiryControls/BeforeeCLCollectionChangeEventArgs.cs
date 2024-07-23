using System;


namespace FWBS.OMS.UI.Windows
{
    public class BeforeeCLCollectionChangeEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public SelectionType SelectionType { get; set; }
        public Direction Direction { get; set; }
    }
}
