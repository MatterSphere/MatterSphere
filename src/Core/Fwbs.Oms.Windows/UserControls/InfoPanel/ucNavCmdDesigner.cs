using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for ucNavButDesigner.
    /// </summary>
    public class ucNavCmdDesigner : System.Windows.Forms.Design.ParentControlDesigner
	{
		private DesignerVerbCollection mVerbs;

		public override DesignerVerbCollection Verbs
		{
			get
			{
				return mVerbs;
			}
		}
		
		public ucNavCmdDesigner()
		{
			mVerbs = new DesignerVerbCollection();
			mVerbs.Add(new DesignerVerb("Add Command",new EventHandler(mnuAddCommand)));
		}

		public void mnuAddCommand(object sender, EventArgs e)
		{
			ucNavCommands a = (ucNavCommands)Control;
			ucNavCmdButtons b = new ucNavCmdButtons();
			b.Text = "Untitled";
			b.ImageList = a.ImageList;
			IContainer c = this.Component.Site.Container;
			c.Add(b);
			a.Controls.Add(b);
			if (this.Control.Parent !=null)
			{
				this.Control.Parent.Height += b.Height;
			}
		}

		public override bool CanParent(System.Windows.Forms.Control control)
		{
			if (control is ucNavPanel)
				return true;
			else
			{
				return false;
			}
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
	}
}
