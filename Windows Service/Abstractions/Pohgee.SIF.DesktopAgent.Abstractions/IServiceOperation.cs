using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	public interface IServiceOperation {
		/// <summary>
		/// Gets the instance id.
		/// </summary>
		/// <value>
		/// The instance id.
		/// </value>
		Guid InstanceId {
			get;
		}

		/// <summary>
		/// Gets the created on.
		/// </summary>
		/// <value>
		/// The created on.
		/// </value>
		DateTime CreatedOn {
			get;
		}


		/// <summary>
		/// Gets the log file location.
		/// </summary>
		/// <value>
		/// The log file location.
		/// </value>
		string LogFileLocation {
			get;
		}
	}
}
