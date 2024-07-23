using System.ComponentModel;
using System.ServiceProcess;

namespace MatterSphereEWSSync
{
    [RunInstaller(true)]
    public partial class MatterSphereEWSSyncInstaller : System.Configuration.Install.Installer
    {
        public MatterSphereEWSSyncInstaller()
        {
            InitializeComponent();
            ServiceInstaller installer = new ServiceInstaller();
            ServiceProcessInstaller installer2 = new ServiceProcessInstaller();
            installer.ServiceName = "3EMatterSphereEWSSyncService";
            installer.Description = "3E MatterSphere EWS Sync Service";
            installer.DisplayName = "The 3E MatterSphere Exchange Sync Service synchronizes appointments within the 3E MatterSphere database with users' calendars.";
            base.Installers.Add(installer);
            installer2.Account = ServiceAccount.LocalSystem;
            installer2.Password = null;
            installer2.Username = null;
            base.Installers.Add(installer2); 
        }
    }
}
