using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace FWBS.OMS.OMSEXPORT
{
	/// <summary>
	/// Summary description for OMSExportInstaller.
	/// </summary>
	[RunInstaller(true)]
	public class OMSExportInstaller : System.Configuration.Install.Installer
	{
		private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
		private System.ServiceProcess.ServiceInstaller serviceInstaller1;

		public OMSExportInstaller()
		{
			// This call is required by the Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			EventLogInstaller eventLogInstaller = serviceInstaller1.Installers.OfType<EventLogInstaller>().FirstOrDefault();
			if (eventLogInstaller != null)
			{
				eventLogInstaller.Log = "OMSExport";
				eventLogInstaller.Source = "FWBS OMS Export Service";
			}
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
			}
			base.Dispose( disposing );
		}


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "Exports 3E MatterSphere data into external systems.";
            this.serviceInstaller1.DisplayName = "FWBS OMS Export Service";
            this.serviceInstaller1.ServiceName = "OMSExportService";
            this.serviceInstaller1.ServicesDependedOn = new string[] {
        "EventLog"};
            // 
            // OMSExportInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceInstaller1,
            this.serviceProcessInstaller1});

		}
		#endregion
	}
}
