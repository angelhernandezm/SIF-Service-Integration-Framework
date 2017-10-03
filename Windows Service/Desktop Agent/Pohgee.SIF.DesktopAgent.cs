
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;
using Pohgee.Framework.LightIoC.Concrete;
using Pohgee.SIF.Desktop.DesktopAgent.Shell;
using Pohgee.SIF.DesktopAgent.Abstractions;
using Pohgee.SIF.DesktopAgent.Abstractions.Enums;
using Pohgee.SIF.DesktopAgent.Core.Concrete;

namespace Pohgee.SIF.DesktopAgent {
	public partial class SifAgentService : ServiceBase, ICustomServiceBase<IServiceInformation, IServiceOperation>, IBaseService<SifAgentService>, IAutomationEngine {
		/// <summary>
		/// The WCF service host
		/// </summary>
		private static ServiceHost WcfServiceHost;

		/// <summary>
		/// The unified shell
		/// </summary>
		private static IUnifiedShell unifiedShell;

		/// <summary>
		/// Gets the service information.
		/// </summary>
		/// <value>
		/// The service information.
		/// </value>
		public IServiceInformation ServiceInformation {
			get;
			private set;
		}

		/// <summary>
		/// Gets the WTS operator.
		/// </summary>
		/// <value>
		/// The WTS operator.
		/// </value>
		public IWtsOperator WtsOperator {
			get; private set;
		}

		/// <summary>
		/// Gets the service operation.
		/// </summary>
		/// <value>
		/// The service operation.
		/// </value>
		public IServiceOperation ServiceOperation {
			get;
			private set;
		}

		/// <summary>
		/// Gets the service instance.
		/// </summary>
		/// <value>
		/// The service instance.
		/// </value>
		public SifAgentService ServiceInstance {
			get {
				return this;
			}
		}


		/// <summary>
		/// Gets the unified shell.
		/// </summary>
		/// <value>The unified shell.</value>
		public IUnifiedShell UnifiedShell {
			get {
				return unifiedShell;
			}

			private set {
				unifiedShell = value;
			}
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="SifAgentService"/> class.
		/// </summary>
		public SifAgentService() {
			InitializeComponent();
			UnifiedShell = TypeContainer.Current.Resolve<IUnifiedShell>();
			ServiceInformation = TypeContainer.Current.Resolve<IServiceInformation>();
			ServiceOperation = TypeContainer.Current.Resolve<IServiceOperation>();
			WtsOperator = TypeContainer.Current.Resolve<IWtsOperator>();
		}

		/// <summary>
		/// Manages the service host.
		/// </summary>
		/// <param name="startListening">if set to <c>true</c> [start listening].</param>
		private void ManageServiceHost(bool startListening = false) {
			if (startListening) {
				if (WcfServiceHost != null) {
					WcfServiceHost.Close();
					WcfServiceHost = null;
				}

				WcfServiceHost = new ServiceHost(TypeContainer.Current.Resolve<IAgentService>().GetType());

				WcfServiceHost.Open();
			} else {
				if (WcfServiceHost != null) {
					WcfServiceHost.Close();
					WcfServiceHost = null;
				}
			}
		}

		/// <summary>
		/// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
		/// </summary>
		/// <param name="args">Data passed by the start command.</param>
		protected override void OnStart(string[] args) {
			ManageServiceHost(true);

			//System.Diagnostics.Debugger.Launch();

			UnifiedShell.Start(null);
		}

		/// <summary>
		/// When implemented in a derived class, <see cref="M:System.ServiceProcess.ServiceBase.OnContinue" /> runs when a Continue command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service resumes normal functioning after being paused.
		/// </summary>
		protected override void OnContinue() {
			ManageServiceHost(true);
		}


		/// <summary>
		/// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
		/// </summary>
		protected override void OnStop() {
			ManageServiceHost();
		}

		/// <summary>
		/// When implemented in a derived class, executes when a Pause command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service pauses.
		/// </summary>
		protected override void OnPause() {
			ManageServiceHost();
		}

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public IAutomationResult ExecuteCommand(CommandType command, IAutomationRequest request) {
			var retval = new AutomationResult();

			switch (command) {
				case CommandType.LaunchApplications:
					ApplicationLauncher.ProcessInformation procInfo;
					//var logonInfo = new ApplicationLauncher.LogOnDetails() {Domain = "Obi-wan", Password = "13670357", UserName = "Tester"};
					//ApplicationLauncher.Current.StartProcessInteractively(new ProcessStartInfo("calc.exe"), out procInfo /*, logonInfo */);
					ApplicationLauncher.Current.StartProcessInteractively(request, out procInfo /*, logonInfo */);

					break;

			}




			//////////////////////////////

			return retval;
		}
	}
}
