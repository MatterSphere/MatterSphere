using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.ContextMenu
{
    public class ToolBarActionsConverter
    {
        private eToolbars _toolbar;
        private ContextMenuItemBuilder _builder;
        private static readonly object EventClickKey;

        static ToolBarActionsConverter()
        {
            EventClickKey = typeof(Control).GetField("EventClick", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
        }

        private static EventHandler GetClickHandler(Control ctrl)
        {
            EventHandlerList eventHandlerList = typeof(Control).GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(ctrl) as EventHandlerList;
            return (eventHandlerList != null) ? eventHandlerList[EventClickKey] as EventHandler : null;
        }

        public ToolBarActionsConverter(eToolbars toolbar)
        {
            _toolbar = toolbar;
            _builder = new ContextMenuItemBuilder();
        }

        /// <summary>
        /// Converts actions from toolbar into ContextMenuButtons that can be used in action column
        /// </summary>
        /// <returns>Collection of buttons with encapsulated actions</returns>
        public List<ContextMenuButton> Convert()
        {
            var collection = _toolbar.NavCommandPanel?.Controls;
            var buttons = new List<ContextMenuButton>();
            if (collection != null)
            {
                foreach (var control in collection)
                {
                    var navButton = control as ucNavCmdButtons;
                    if (navButton != null)
                    {
                        var contextButton = _builder.CreateTextButton(
                            navButton.Text,
                            GetClickHandler(navButton),
                            navButton.Enabled);
                        navButton.AttachButton(contextButton);
                        buttons.Add(contextButton);
                    }
                }
            }
            return buttons;
        }
    }
}
