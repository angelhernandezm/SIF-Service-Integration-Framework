using System;

namespace Pohgee.SIF.DesktopAgent.Abstractions.DataContracts {
	public class ServiceStatus : IServiceStatus {
		/// <summary>
		/// Gets or sets the error count.
		/// </summary>
		/// <value>
		/// The error count.
		/// </value>
		public long ErrorCount {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the service image.
		/// </summary>
		/// <value>
		/// The service image.
		/// </value>
		public string ServiceImage {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the last execution.
		/// </summary>
		/// <value>
		/// The last execution.
		/// </value>
		public DateTime LastExecution {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the total files processed.
		/// </summary>
		/// <value>
		/// The total files processed.
		/// </value>
		public long TotalFilesProcessed {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the total automation requests.
		/// </summary>
		/// <value>
		/// The total automation requests.
		/// </value>
		public long TotalAutomationRequests {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the unhandled exception count.
		/// </summary>
		/// <value>
		/// The unhandled exception count.
		/// </value>
		public long UnhandledExceptionCount {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the total failed automations.
		/// </summary>
		/// <value>
		/// The total failed automations.
		/// </value>
		public long TotalFailedAutomations {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the total succeeded automations.
		/// </summary>
		/// <value>
		/// The total succeeded automations.
		/// </value>
		public long TotalSucceededAutomations {
			get;
			set;
		}
	}
}
