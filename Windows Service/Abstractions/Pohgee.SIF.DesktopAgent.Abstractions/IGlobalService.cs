using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	public interface IGlobalService {
		/// <summary>
		/// Initializes the type container.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		void InitializeTypeContainer<T>() where T : System.ComponentModel.Component;

		/// <summary>
		/// Registers the perf counters.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance">The instance.</param>
		/// <param name="instanceName">Name of the instance.</param>
		void RegisterPerfCounters<T>(T instance, string instanceName = "SifAgentService-DefaultPerfCounters") where T : IPerfCounters;

		/// <summary>
		/// Gets the perf counters.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instanceName">Name of the instance.</param>
		/// <returns></returns>
		T GetPerfCounters<T>(string instanceName = "SifAgentService-DefaultPerfCounters") where T : IPerfCounters;
	}
}
