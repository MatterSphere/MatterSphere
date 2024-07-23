namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookExplorer
    {

        #region _Explorer Members

        public MSOutlook.AccountSelector AccountSelector
        {
            get { return explorer.AccountSelector; }
        }

        public void AddToSelection(object Item)
        {
            var oi = Item as OutlookItem;
            if (oi != null)
                Item = oi.InternalObject;

            explorer.AddToSelection(Item);
        }

        public MSOutlook.AttachmentSelection AttachmentSelection
        {
            get { return explorer.AttachmentSelection; }
        }

        public void ClearSelection()
        {
            explorer.ClearSelection();
        }

        public bool IsItemSelectableInView(object Item)
        {
            return explorer.IsItemSelectableInView(Item);
        }

        public void RemoveFromSelection(object Item)
        {
            var oi = Item as OutlookItem;
            if (oi != null)
                Item = oi.InternalObject;

            throw new System.NotImplementedException();
        }

        public void SelectAllItems()
        {
            explorer.SelectAllItems();
        }

        #endregion

        #region ExplorerEvents_10_Event Members

        public event MSOutlook.ExplorerEvents_10_AttachmentSelectionChangeEventHandler AttachmentSelectionChange
        {
            add
            {
                explorer.AttachmentSelectionChange += value;
            }
            remove
            {
                explorer.AttachmentSelectionChange -= value;
            }
        }

        #endregion
    }
}
