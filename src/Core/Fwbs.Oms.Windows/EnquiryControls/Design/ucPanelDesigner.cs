using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FWBS.OMS.UI.Windows.Design
{
    public class PanelDesigner : System.Windows.Forms.Design.ParentControlDesigner
	{
		private DesignerVerbCollection mVerbs;

		public PanelDesigner()
		{
			mVerbs = new DesignerVerbCollection();
			mVerbs.Add(new DesignerVerb("Add Panel",new EventHandler(mnuAddPanel)));
			mVerbs.Add(new DesignerVerb("Add RichText",new EventHandler(mnuAddRichText)));
			mVerbs.Add(new DesignerVerb("Add Commands",new EventHandler(mnuAddCommands)));
		}

		public void mnuAddPanel(object sender, EventArgs e)
		{
			ucPanelNav a = (ucPanelNav)Control;
			if (a.Controls.Count < 3)
			{
				a.pContainer = new ucNavPanel();
				a.pContainer.Location = new System.Drawing.Point(0,a.labHeader.Height);
				a.pContainer.Size = new System.Drawing.Size(a.Width,a.Height - a.labHeader.Height);
				a.pContainer.Dock = System.Windows.Forms.DockStyle.Fill;
				IContainer c = this.Component.Site.Container;
				c.Add(a.pContainer);
				a.Controls.Add(a.pContainer);
			}
		}

		public void mnuAddRichText(object sender, EventArgs e)
		{
			ucPanelNav a = (ucPanelNav)Control;
			if (a.Controls.Count < 3)
			{
				a.pContainer = new ucNavRichText();
				a.pContainer.Location = new System.Drawing.Point(0,a.labHeader.Height);
				a.pContainer.Size = new System.Drawing.Size(a.Width,a.Height - a.labHeader.Height);
				a.pContainer.Dock = System.Windows.Forms.DockStyle.Fill;
				IContainer c = this.Component.Site.Container;
				c.Add(a.pContainer);
				a.Controls.Add(a.pContainer);
			}
		}

		public void mnuAddCommands(object sender, EventArgs e)
		{
			ucPanelNav a = (ucPanelNav)Control;
			if (a.Controls.Count < 3)
			{
				a.pContainer = new ucNavCommands();
				a.pContainer.Location = new System.Drawing.Point(0,a.labHeader.Height);
				a.pContainer.Size = new System.Drawing.Size(a.Width,a.Height - a.labHeader.Height);
				a.pContainer.Dock = System.Windows.Forms.DockStyle.Fill;
				IContainer c = this.Component.Site.Container;
				c.Add(a.pContainer);
				a.Controls.Add(a.pContainer);
			}
		}

		public override DesignerVerbCollection Verbs
		{
			get
			{
				return mVerbs;
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

	public class ToolBarDesigner : System.Windows.Forms.Design.ControlDesigner
	{
		public ToolBarDesigner()
		{
		}

		public override SelectionRules SelectionRules 
		{
			get
			{
				return SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Moveable | SelectionRules.Visible;
			}
		}
	}

}
