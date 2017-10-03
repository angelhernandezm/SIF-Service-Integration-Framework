using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	public interface IServiceManager {
		/// <summary>
		/// Initializes the channel.
		/// </summary>
		void InitializeChannel();

		/// <summary>
		/// Gets the service operation.
		/// </summary>
		/// <value>
		/// The service operation.
		/// </value>
		IServiceOperation ServiceOperation {
			get;
		}

		/// <summary>
		/// Gets the controller.
		/// </summary>
		/// <value>
		/// The controller.
		/// </value>
		System.ServiceProcess.ServiceController Controller {
			get;
		}
	}
}
