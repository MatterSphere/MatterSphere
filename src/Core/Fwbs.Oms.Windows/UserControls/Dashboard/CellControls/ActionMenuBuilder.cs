using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.ContextMenu;
using FWBS.OMS.UI.Windows;
using Infragistics.Win.Misc;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    public class ActionMenuBuilder
    {
        public delegate void ActionCall(string code, bool resetResult);
        public delegate void FilterCall(string code, string title);

        private UltraPeekPopup _menuPopup { get; set; }
        private ContextMenuItemBuilder _cmiBuilder;

        public ActionMenuBuilder(UltraPeekPopup menuPopup)
        {
            _cmiBuilder = new ContextMenuItemBuilder();
            _menuPopup = menuPopup;
            _menuPopup.Content.Controls.Clear();
        }

        public void AddButton(string title, bool isRoot, EventHandler clickHandler)
        {
            var button = isRoot
                ? _cmiBuilder.CreateIconButton(title, clickHandler)
                : _cmiBuilder.CreateTextButton(title, clickHandler);
            ((Panel)_menuPopup.Content).Controls.Add(button, true);
        }

        public void AddCloseButton(EventHandler clickHandler)
        {
            AddButton(
                CodeLookup.GetLookup("DASHBOARD", "DELETE", "Delete"),
                false,
                clickHandler);
        }

        public void AddPopOutButton(EventHandler clickHandler)
        {
            AddButton(
                CodeLookup.GetLookup("DASHBOARD", "POPOUT", "Pop Out"),
                false,
                clickHandler);
        }

        public void AddGridSettingsButton(EventHandler clickHandler)
        {
            AddButton(
                CodeLookup.GetLookup("DASHBOARD", "GRDSTTNGS", "Grid Settings"),
                true,
                clickHandler);
        }

        public void AddResizeButton(EventHandler clickHandler)
        {
            AddButton(
                CodeLookup.GetLookup("DASHBOARD", "RESIZE", "Resize"),
                true,
                clickHandler);
        }

        public virtual void AddActions(List<ActionItem> actionItems, ActionCall handler)
        {
            foreach (var actionItem in actionItems)
            {
                var button = new ContainerMenuButton
                {
                    Code = actionItem.Code,
                    Text = actionItem.Title
                };

                button.CodeItemClicked += handler;
                ((Panel)_menuPopup.Content).Controls.Add(button);
            }
        }

        public virtual void AddFilters(List<ActionItem> filterItems, FilterCall handler)
        {
            foreach (var filterItem in filterItems)
            {
                var button = new ContainerMenuButton
                {
                    Code = filterItem.Code,
                    Text = filterItem.Title,
                    IsFilter = true
                };

                button.FilterItemClicked += handler;
                ((Panel)_menuPopup.Content).Controls.Add(button);
            }
        }
    }
}
