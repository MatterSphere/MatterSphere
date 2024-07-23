using System.Linq;

namespace Fwbs.Office
{
    using MSOffice = Microsoft.Office.Core;

    internal sealed partial class OfficeCommandBarPopup : 
        OfficeCommandBarControl,
        MSOffice.CommandBarPopup
    {
        private readonly MSOffice.CommandBarPopup popup;
        private readonly OfficeCommandBarControls children;

        internal OfficeCommandBarPopup(OfficeCommandBarControls parent, MSOffice.CommandBarPopup popup)
            :base(parent, popup)
        {
           
            this.popup = popup;
            this.children = new OfficeCommandBarControls(this, popup.Controls);

            Init(popup);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (children != null)
                    {
                        children.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #region CommandBarPopup Members


        public Microsoft.Office.Core.CommandBar CommandBar
        {
            get { return Parent; }
        }

        public override Microsoft.Office.Core.CommandBarControls Controls
        {
            get 
            { 
                return children;
            }
        }

        public Microsoft.Office.Core.MsoOLEMenuGroup OLEMenuGroup
        {
            get
            {
                return popup.OLEMenuGroup;
            }
            set
            {
                popup.OLEMenuGroup = value;
            }
        }


        public override void Delete(object Temporary)
        {
            
            if (children != null)
            {
                foreach (var child in children.Cast<MSOffice.CommandBarControl>().ToArray())
                    child.Delete(Temporary);
            }

            base.Delete(Temporary);
        }
        #endregion
    }
}
