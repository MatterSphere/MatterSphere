using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.ContextMenu
{
    public class ContextMenuItemBuilder
    {
        public ContextMenuButton CreateTextButton(string title, EventHandler clickHandler, bool enabled = true)
        {
            var button = new ContextMenuButton
            {
                Dock = DockStyle.Top,
                Cursor = Cursors.Hand,
                Text = title,
                Enabled = enabled
            };

            button.Click += clickHandler;

            return button;
        }

        public ContextMenuButton CreateIconButton(string title, EventHandler clickHandler, bool enabled = true)
        {
            var button = CreateTextButton(title, clickHandler, enabled);
            button.VisibleIcon = true;

            return button;
        }
    }
}
