using System;
using FWBS.OMS.UI.UserControls.ContextMenu;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    public class ContainerMenuButton : ContextMenuButton
    {
        public ActionMenuBuilder.ActionCall CodeItemClicked;
        public ActionMenuBuilder.FilterCall FilterItemClicked;
        public EventHandler ItemClicked;

        public ContainerMenuButton()
        {
            base.Click += (s, e) => OnItemClick();
        }

        public string Code { get; set; }
        public bool IsFilter { get; set; }

        private void OnItemClick()
        {
            if (string.IsNullOrWhiteSpace(Code))
            {
                ItemClicked?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (IsFilter)
            {
                FilterItemClicked?.Invoke(Code, Text);
            }
            else
            {
                CodeItemClicked?.Invoke(Code, true);
            }
        }
    }
}
