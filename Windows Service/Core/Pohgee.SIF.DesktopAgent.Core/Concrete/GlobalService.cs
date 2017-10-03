using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pohgee.Framework.LightIoC.Concrete;
using Pohgee.SIF.DesktopAgent.Abstractions;

namespace Pohgee.SIF.DesktopAgent.Core.Concrete {
	public class GlobalService : IGlobalService {

		private static volatile IGlobalService singleton;
		private static readonly object syncRoot = new object();

		/// <summary>
		/// Gets or sets a value indicating whether [type container initialized].
		/// </summary>
		/// <value>
		/// <c>true</c> if [type container initialized]; otherwise, <c>false</c>.
		/// </value>
		protected bool TypeContainerInitialized {
			get;
			set;
		}


		/// <summary>
		/// Registers the perf counters.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance">The instance.</param>
		/// <param name="instanceName">Name of the instance.</param>
		public void RegisterPerfCounters<T>(T instance, string instanceName = "SifAgentService-DefaultPerfCounters") where T : IPerfCounters {
			if (AppDomain.CurrentDomain.GetData(instanceName) == null)
				AppDomain.CurrentDomain.SetData(instanceName, instance);

		}

		/// <summary>
		/// Gets the perf counters.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instanceName">Name of the instance.</param>
		/// <returns></returns>
		public T GetPerfCounters<T>(string instanceName = "SifAgentService-DefaultPerfCounters") where T : IPerfCounters {
			return ((T)AppDomain.CurrentDomain.GetData(instanceName));
		}


		/// <summary>
		/// Initializes the type container.
		/// </summary>
		public void InitializeTypeContainer<T>() where T: System.ComponentModel.Component {
			if (!TypeContainerInitialized) {
				TypeContainer.Current.RegisterType<IUnifiedShell>("UnifiedShell");
				TypeContainer.Current.RegisterType<IShellConfig>("ShellConfig");
				TypeContainer.Current.RegisterType<ILogger>("Logger", true);
				TypeContainer.Current.RegisterType<IAgentService>("AgentService");
				TypeContainer.Current.RegisterType<IAutomationResult>("AutomationResult");
				TypeContainer.Current.RegisterType<IAutomationRequest>("AutomationRequest");
				TypeContainer.Current.RegisterType<IPerfCounters>("PerfCounters", true);
				TypeContainer.Current.RegisterType<IServiceOperation>("ServiceOperation");
				TypeContainer.Current.RegisterType<IServiceInformation>("CustomServiceBase", true);
				TypeContainer.Current.RegisterType<IShellFactory>("ShellFactory");
				TypeContainer.Current.RegisterType<IBaseService<T>>(typeof(T).Name, true);
				TypeContainer.Current.RegisterType<IWtsOperator>("WtsOperator", true);
				TypeContainer.Current.RegisterType<IAutomationEngine>(TypeContainer.Current.Resolve<IBaseService<T>>());

				TypeContainerInitialized = true;
			}
		}


		/// <summary>
		/// Prevents a default instance of the <see cref="GlobalService"/> class from being created.
		/// </summary>
		private GlobalService() {

		}

		/// <summary>
		/// Gets the current.
		/// </summary>
		/// <value>
		/// The current.
		/// </value>
		public static IGlobalService Current {
			get {
				if (singleton == null) {
					lock (syncRoot) {
						if (singleton == null)
							singleton = new GlobalService();
					}
				}
				return singleton;
			}
		}
	}
}