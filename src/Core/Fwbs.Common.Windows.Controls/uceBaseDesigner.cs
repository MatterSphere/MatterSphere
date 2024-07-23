using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FWBS.Common.UI.Windows.Design
{
    /// <summary>
    /// Summary description for eBase Designer.
    /// </summary>
    public class eBaseDesigner : System.Windows.Forms.Design.ParentControlDesigner
	{
		public eBaseDesigner()
		{

		}

		public bool CanBeParentTo(IDesigner parentDesigner)
		{
			return false;
		}

		protected override void OnDragEnter(DragEventArgs de)
		{
			de.Effect = DragDropEffects.None;
		}

		protected override bool DrawGrid
		{
			get { return false; }
		}

		protected override bool EnableDragRect 
		{
			get{return false;}
		}

		public override SelectionRules SelectionRules 
		{
			get
			{
				IBasicEnquiryControl2 e = Control as IBasicEnquiryControl2;
				if (e != null && e.LockHeight)
					return SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Moveable | SelectionRules.Visible;
				else
					return SelectionRules.AllSizeable | SelectionRules.Moveable | SelectionRules.Visible;
			}
		}
	}
}
