namespace FWBS.OMS.FileManagement.Design
{
    internal class FileEventsNode : FMNode
    {

        public FileEventsNode(FMDesigner Designer)
            : base(Designer)
        {
            ImageKey = "event";
            SelectedImageKey = this.ImageKey;
            Text = Designer.FMEvents;

        }

        internal FileEventNode CreateFileEventNode(string eventName)
        {
            return CreateFileEventNode(this, eventName);
        }

        internal FileEventNode CreateFileEventNode(FMNode parent, string eventName)
        {
            FileEventNode node = new FileEventNode(Designer);

            node.Text = eventName;
            AddNode(parent, node);

            return node;
        }
    }
}
