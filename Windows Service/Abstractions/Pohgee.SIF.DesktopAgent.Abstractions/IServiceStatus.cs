using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	public interface IServiceStatus {
		/// <summary>
		/// Gets or sets the error count.
		/// </summary>
		/// <value>
		/// The error count.
		/// </value>
		long ErrorCount {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the service image.
		/// </summary>
		/// <value>
		/// The service image.
		/// </value>
		string ServiceImage {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the last execution.
		/// </summary>
		/// <value>
		/// The last execution.
		/// </value>
		DateTime LastExecution {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the total automation requests.
		/// </summary>
		/// <value>
		/// The total automation requests.
		/// </value>
		long TotalAutomationRequests {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the unhandled exception count.
		/// </summary>
		/// <value>
		/// The unhandled exception count.
		/// </value>
		long UnhandledExceptionCount {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the total failed automations.
		/// </summary>
		/// <value>
		/// The total failed automations.
		/// </value>
		long TotalFailedAutomations {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the total succeeded automations.
		/// </summary>
		/// <value>
		/// The total succeeded automations.
		/// </value>
		long TotalSucceededAutomations {
			get;
			set;
		}
	}
}
