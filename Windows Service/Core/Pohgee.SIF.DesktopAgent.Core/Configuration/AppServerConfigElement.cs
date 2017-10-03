using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Pohgee.SIF.DesktopAgent.Abstractions;

namespace Pohgee.SIF.DesktopAgent.Core.Configuration {
	/// <summary>
	/// 
	/// </summary>
	public class AppServerConfigElement : ConfigurationElement {
		/// <summary>
		/// Initializes a new instance of the <see cref="AppServerConfigElement"/> class.
		/// </summary>
		public AppServerConfigElement() {

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AppServerConfigElement"/> class.
		/// </summary>
		/// <param name="host">The host.</param>
		public AppServerConfigElement(string host) {
			Host = host;
		}

		/// <summary>
		/// Gets or sets the host.
		/// </summary>
		/// <value>
		/// The host.
		/// </value>
		[ConfigurationProperty(Consts.HostPropName, IsRequired = true)]
		public string Host {
			get {
				return this[Consts.HostPropName].ToString();
			}
			set {
				this[Consts.HostPropName] = value;
			}
		}
	}
}