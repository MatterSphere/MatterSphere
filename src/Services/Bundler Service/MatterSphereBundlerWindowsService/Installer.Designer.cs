namespace MatterSphereBundlerWindowsService
{
    partial class Installer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            
            
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();

            // serviceProcessInstaller1 this should prompt the username dialog
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            //serviceInstaller1 this sets detils about the service.
            this.serviceInstaller1.Description = "The 3E MatterSphere PDF Bundler creates one PDF file made up of documents against a matter.";
            this.serviceInstaller1.DisplayName = "3E MatterSphere PDF Bundler Service";
            this.serviceInstaller1.ServiceName = "3EMatterSpherePDFBundlerService";
            this.serviceInstaller1.ServicesDependedOn = new string[] {
        "EventLog"};
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
																					  this.serviceInstaller1,
																					  this.serviceProcessInstaller1});

        }

        #endregion
    }
}