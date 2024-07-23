namespace FWBS.OMS.FileManagement.Design
{
    internal class FileEventNode : FMNode
    {

        public FileEventNode(FMDesigner Designer)
            : base(Designer)
        {
            ImageKey = "event";
            SelectedImageKey = this.ImageKey;
            ContextMenuStrip = Designer.MenuStrip_FileEvents;

        }

    }
}
