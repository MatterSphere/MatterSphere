using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace MatterSphereBundlerWindowsService
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;

        public Installer()
        {
            InitializeComponent();

            EventLogInstaller eventLogInstaller = serviceInstaller1.Installers.OfType<EventLogInstaller>().FirstOrDefault();
            if (eventLogInstaller != null)
            {
                eventLogInstaller.Log = "PDFBundler";
                eventLogInstaller.Source = this.serviceInstaller1.DisplayName;
            }
        }

    }
}
