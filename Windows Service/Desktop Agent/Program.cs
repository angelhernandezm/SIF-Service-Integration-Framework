using System.ServiceProcess;
using Pohgee.Framework.LightIoC.Concrete;
using Pohgee.SIF.DesktopAgent.Abstractions;
using Pohgee.SIF.DesktopAgent.Core.Concrete;

namespace Pohgee.SIF.DesktopAgent {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main() {
			GlobalService.Current.InitializeTypeContainer<SifAgentService>();
			GlobalService.Current.RegisterPerfCounters(TypeContainer.Current.Resolve<IPerfCounters>());
			ServiceBase.Run(new ServiceBase[] {TypeContainer.Current.Resolve<IBaseService<SifAgentService>>().ServiceInstance});
		}
	}
}