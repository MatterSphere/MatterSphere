using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    internal partial class DashboardContextMenuStrip : ContextMenuStrip
    {

        internal DashboardContextMenuStrip()
        {
            InitializeComponent();
            Renderer = new DashboardToolStripProfessionalRenderer();
        }

        internal DashboardContextMenuStrip(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

    }
}
