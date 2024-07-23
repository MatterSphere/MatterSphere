using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;

namespace IndexToolService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            var eventLogInstaller = serviceInstaller1.Installers.OfType<EventLogInstaller>().FirstOrDefault();
            if(eventLogInstaller != null)
            {
                eventLogInstaller.Log = "ESIndexToolService";
                eventLogInstaller.Source = serviceInstaller1.DisplayName;
            }
        }
    }
}
