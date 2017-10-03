using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pohgee.SIF.DesktopAgent.Abstractions;

namespace Pohgee.SIF.DesktopAgent.Core.Service {
	public class CustomServiceBase : IServiceInformation {
		/// <summary>
		/// The SVC name
		/// </summary>
		private const string SvcName = "SifAgentService";

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomServiceBase"/> class.
		/// </summary>
		public CustomServiceBase() {
			Initialize();
		}

		/// <summary>
		/// Gets the name of the service.
		/// </summary>
		/// <value>
		/// The name of the service.
		/// </value>
		public string ServiceName {
			get {
				return SvcName;
			}
		}


		/// <summary>
		/// Initializes this instance.
		/// </summary>
		protected void Initialize() {

		}
	}
}
