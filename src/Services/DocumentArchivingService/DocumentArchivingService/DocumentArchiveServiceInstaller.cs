using System.ComponentModel;

namespace DocumentArchivingService
{
    [RunInstaller(true)]
    public partial class DocumentArchiveServiceInstaller : System.Configuration.Install.Installer
    {
        public DocumentArchiveServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
