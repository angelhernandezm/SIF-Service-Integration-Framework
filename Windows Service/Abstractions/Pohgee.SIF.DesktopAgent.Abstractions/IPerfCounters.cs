using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	/// <summary>
	/// 
	/// </summary>
	public enum PerfCounter {
		ErrorCount = 0,
		TimeTaken,
		TotalAutomationRequests,
		UnhandledExceptions,
		FailedAutomations,
		SucceededAutomations
	}

	/// <summary>
	/// 
	/// </summary>
	public interface IPerfCounters {
		/// <summary>
		/// Gets the created configuration.
		/// </summary>
		/// <value>
		/// The created configuration.
		/// </value>
		DateTime CreatedOn {
			get;
		}

		/// <summary>
		/// Gets the instance unique identifier.
		/// </summary>
		/// <value>
		/// The instance unique identifier.
		/// </value>
		Guid InstanceId {
			get;
		}

		/// <summary>
		/// Gets the <see cref="PerformanceCounter"/> with the specified counter name.
		/// </summary>
		/// <value>
		/// The <see cref="PerformanceCounter"/>.
		/// </value>
		/// <param name="counterName">Name of the counter.</param>
		/// <returns></returns>
		PerformanceCounter this[PerfCounter counterName] {
			get;
		}
	}
}