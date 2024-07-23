using System.ComponentModel;
using System.ServiceProcess;

namespace MatterSphereEWSDelete
{
    [RunInstaller(true)]
    public partial class MSEWSDeleteInstaller : System.Configuration.Install.Installer
    {
        public MSEWSDeleteInstaller()
        {
            InitializeComponent();
            ServiceInstaller installer = new ServiceInstaller();
            ServiceProcessInstaller installer2 = new ServiceProcessInstaller();
            installer.ServiceName = "3EMatterSphereEWSDeleteService";
            installer.Description = "The 3E MatterSphere Exchange Sync Service deletes appointments in the 3E MatterSphere database, which were deleted from the fee earners' calendars.";
            installer.DisplayName = "3E MatterSphere EWS Delete Service";
            base.Installers.Add(installer);
            installer2.Account = ServiceAccount.LocalSystem;
            installer2.Password = null;
            installer2.Username = null;
            base.Installers.Add(installer2); 
        }
    }
}
