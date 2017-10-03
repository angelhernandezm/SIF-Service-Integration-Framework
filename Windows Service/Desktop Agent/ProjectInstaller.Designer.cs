namespace Pohgee.SIF.DesktopAgent {
	partial class ProjectInstaller {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.SifDesktopAgentProjectInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this.SifDesktopAgentServiceInstaller = new System.ServiceProcess.ServiceInstaller();
			// 
			// SifDesktopAgentProjectInstaller
			// 
			this.SifDesktopAgentProjectInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.SifDesktopAgentProjectInstaller.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.SifDesktopAgentServiceInstaller});
			this.SifDesktopAgentProjectInstaller.Password = null;
			this.SifDesktopAgentProjectInstaller.Username = null;
			// 
			// SifDesktopAgentServiceInstaller
			// 
			this.SifDesktopAgentServiceInstaller.ServiceName = "SifAgentService";
			this.SifDesktopAgentServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.SifDesktopAgentProjectInstaller});

		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller SifDesktopAgentProjectInstaller;
		private System.ServiceProcess.ServiceInstaller SifDesktopAgentServiceInstaller;
	}
}