using System.Drawing;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for omsSplitter.
    /// </summary>
    public class omsSplitter : System.Windows.Forms.Splitter
	{
		public omsSplitter()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			// 
			// omsSplitter
			// 
			this.Size = new System.Drawing.Size(4, 5);
			this.SizeChanged += new System.EventHandler(this.omsSplitter_SizeChanged);
		}

		private void omsSplitter_SizeChanged(object sender, System.EventArgs e)
		{
			this.Invalidate();
		}
	}
}
