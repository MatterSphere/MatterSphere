using System;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// Summary description for eTabControl.
    /// </summary>
    public class eTabControl : System.Windows.Forms.TabControl
	{
		public eTabControl() : base()
		{
		}

		protected override bool ProcessMnemonic(char charCode)
		{
			return base.ProcessMnemonic (charCode);
		}

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            if (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                this.RightToLeftLayout = true;
            else
                this.RightToLeftLayout = false;
        }
	}
}
