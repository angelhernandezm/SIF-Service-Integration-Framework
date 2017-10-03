using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Pohgee.SIF.DesktopAgent.Abstractions;

namespace Pohgee.SIF.DesktopAgent.Core.Configuration {
	public class ConfigReader : ConfigurationSection {
		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <returns></returns>
		public static ConfigReader GetConfiguration() {
			var retval = ConfigurationManager.GetSection(Consts.ConfigSectionName) as ConfigReader;

			return retval ?? new ConfigReader();
		}

		/// <summary>
		/// Gets or sets the runtime configuration.
		/// </summary>
		/// <value>
		/// The runtime configuration.
		/// </value>
		[ConfigurationProperty(Consts.RuntimePropertyName)]
		public RuntimeConfigElement RuntimeConfig {
			get {
				return (RuntimeConfigElement)this[Consts.RuntimePropertyName];
			}
			set {
				this[Consts.RuntimePropertyName] = value;
			}
		}

		/// <summary>
		/// Gets or sets the application server configuration.
		/// </summary>
		/// <value>
		/// The application server configuration.
		/// </value>
		[ConfigurationProperty(Consts.AppServerPropertyName)]
		public AppServerConfigElement AppServerConfig {
			get {
				return (AppServerConfigElement)this[Consts.AppServerPropertyName];
			}
			set {
				this[Consts.AppServerPropertyName] = value;
			}
		}
	}
}