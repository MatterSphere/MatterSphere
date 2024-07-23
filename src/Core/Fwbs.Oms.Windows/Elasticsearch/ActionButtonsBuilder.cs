using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.ContextMenu;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.Elasticsearch
{
    internal class ActionButtonsBuilder
    {
        private readonly int _objectTypeColumnIndex;
        private readonly int _fileIdColumnIndex;
        private readonly int _clientIdColumnIndex;
        private readonly int _contactIdColumnIndex;
        private readonly int _documentIdColumnIndex;
        private readonly int _associateIdColumnIndex;
        private readonly int _matterSphereIdColumnIndex;
        private readonly ActionProvider _actionProvider;

        public ActionButtonsBuilder(int objectTypeColumnIndex, int fileIdColumnIndex,
            int clientIdColumnIndex, int contactIdColumnIndex, int documentIdColumnIndex,
            int associateIdColumnIndex, int matterSphereIdColumnIndex)
        {
            _objectTypeColumnIndex = objectTypeColumnIndex;
            _fileIdColumnIndex = fileIdColumnIndex;
            _clientIdColumnIndex = clientIdColumnIndex;
            _contactIdColumnIndex = contactIdColumnIndex;
            _documentIdColumnIndex = documentIdColumnIndex;
            _associateIdColumnIndex = associateIdColumnIndex;
            _matterSphereIdColumnIndex = matterSphereIdColumnIndex;
            _actionProvider = new ActionProvider();
        }

        internal List<ContextMenuButton> BuildActionMenuButtons(DataGridViewRow row)
        {
            var buttons = new List<ContextMenuButton>();
            var actions = GetActions(row);
            foreach (var action in actions)
            {
                buttons.Add(CreateActionButton(action.Key, action.Value));
            }

            return buttons;
        }

        internal List<IconMenuItem> BuildContextMenuItems(DataGridViewRow row)
        {
            var menuItems = new List<IconMenuItem>();
            var actions = GetActions(row);
            foreach (var action in actions)
            {
                menuItems.Add(CreateMenuItem(action.Key, action.Value));
            }

            return menuItems;
        }

        private Dictionary<string, Action> GetActions(DataGridViewRow row)
        {
            var actions = new Dictionary<string, Action>();

            var fileId = GetId(row, _fileIdColumnIndex);
            if (fileId.HasValue)
            {
                actions.Add(Session.CurrentSession.Resources.GetResource("VIEWFILE", "View %FILE%", "").Text,
                    () => _actionProvider.GetAction("open_file").Invoke(fileId.Value));
            }

            var clientId = GetId(row, _clientIdColumnIndex);
            if (clientId.HasValue)
            {
                actions.Add(Session.CurrentSession.Resources.GetResource("VIEWCLIENT", "View %CLIENT%", "").Text,
                    () => _actionProvider.GetAction("open_client").Invoke(clientId.Value));
            }

            var contactId = GetId(row, _contactIdColumnIndex);
            if (contactId.HasValue)
            {
                actions.Add(Session.CurrentSession.Resources.GetResource("VIEWCONTACT", "View Contact", "").Text,
                    () => _actionProvider.GetAction("open_contact").Invoke(contactId.Value));
            }

            var documentId = GetId(row, _documentIdColumnIndex);
            if (documentId.HasValue)
            {
                actions.Add(Session.CurrentSession.Resources.GetResource("OPENDOC", "Open Document", "").Text,
                    () => _actionProvider.GetAction("open_document").Invoke(documentId.Value));

                actions.Add(Session.CurrentSession.Resources.GetResource("DOCPROP", "Document Properties", "").Text,
                    () => _actionProvider.GetAction("open_document_properties").Invoke(documentId.Value));
            }

            var objectType = GetValue(row, _objectTypeColumnIndex);
            if (objectType == "precedent")
            {
                var precedentId = GetId(row, _matterSphereIdColumnIndex);
                if (precedentId != null)
                {
                    actions.Add(Session.CurrentSession.Resources.GetResource("VIEWPRECBTN", "View %PRECEDENT%", "").Text,
                        () => _actionProvider.GetAction("open_precedent").Invoke(precedentId.Value));

                    actions.Add(Session.CurrentSession.Resources.GetResource("USEPREC", "Use %PRECEDENT%", "").Text,
                        () => _actionProvider.GetAction("use_precedent").Invoke(precedentId.Value));
                }
            }

            var associateId = GetId(row, _associateIdColumnIndex);
            if (associateId.HasValue)
            {
                actions.Add(Session.CurrentSession.Resources.GetResource("VIEWASSOCIATE", "View Associate", "").Text,
                    () => _actionProvider.GetAction("open_associate").Invoke(associateId.Value));
            }

            return actions;
        }

        private ContextMenuButton CreateActionButton(string title, Action action)
        {
            var button = new ContextMenuButton
            {
                Dock = DockStyle.Top,
                Cursor = Cursors.Hand,
                Text = title
            };

            button.Click += delegate (object sender, EventArgs e)
            {
                action.Invoke();
            };

            return button;
        }

        private IconMenuItem CreateMenuItem(string title, Action action)
        {
            var button = new IconMenuItem
            {
                Text = title
            };

            button.Click += delegate (object sender, EventArgs e)
            {
                action.Invoke();
            };

            return button;
        }

        private long? GetId(DataGridViewRow row, int index)
        {
            if (row.Cells[index].Value == null)
            {
                return null;
            }

            long id;
            return Int64.TryParse(row.Cells[index].Value.ToString(), out id)
                ? id
                : (long?)null;
        }

        private string GetValue(DataGridViewRow row, int index)
        {
            return row.Cells[index].Value.ToString();
        }
    }
}
