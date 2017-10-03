using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Pohgee.SIF.DesktopAgent.Abstractions;

namespace Pohgee.SIF.DesktopAgent.Core.Configuration {
	public class RuntimeConfigElement : ConfigurationElement {
		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeConfigElement"/> class.
		/// </summary>
		public RuntimeConfigElement() {

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeConfigElement" /> class.
		/// </summary>
		/// <param name="launchAppsOnStart">The launch apps configuration start.</param>
		public RuntimeConfigElement(string launchAppsOnStart = "true") {
			LaunchAppsOnStart = launchAppsOnStart;
		}

		/// <summary>
		/// Gets or sets the launch apps configuration start.
		/// </summary>
		/// <value>
		/// The launch apps configuration start.
		/// </value>
		[ConfigurationProperty(Consts.LaunchAppsOnStartPropName)]
		public string LaunchAppsOnStart {
			get {
				return this[Consts.LaunchAppsOnStartPropName].ToString();
			}
			set {
				this[Consts.LaunchAppsOnStartPropName] = value;
			}
		}

		/// <summary>
		/// Gets or sets the host port.
		/// </summary>
		/// <value>
		/// The host port.
		/// </value>
		[ConfigurationProperty(Consts.HostPortPropName, IsRequired = true, DefaultValue = "8080")]
		public string HostPort {
			get {
				return this[Consts.HostPortPropName].ToString();
			}
			set {
				this[Consts.HostPortPropName] = value;
			}
		}

		/// <summary>
		/// Gets or sets the use HTTPS.
		/// </summary>
		/// <value>
		/// The use HTTPS.
		/// </value>
		[ConfigurationProperty(Consts.UseHttpsPropName, IsRequired = false, DefaultValue = "false")]
		public string UseHttps {
			get {
				return this[Consts.UseHttpsPropName].ToString();
			}
			set {
				this[Consts.UseHttpsPropName] = value;
			}
		}

		/// <summary>
		/// Gets or sets the surface performance counters.
		/// </summary>
		/// <value>
		/// The surface performance counters.
		/// </value>
		[ConfigurationProperty(Consts.SurfacePerfCountersPropName, IsRequired = false, DefaultValue = "false")]
		public string SurfacePerformanceCounters {
			get {
				return this[Consts.SurfacePerfCountersPropName].ToString();
			}
			set {
				this[Consts.SurfacePerfCountersPropName] = value;
			}
		}

		/// <summary>
		/// Gets or sets the log file location.
		/// </summary>
		/// <value>
		/// The log file location.
		/// </value>
		[ConfigurationProperty(Consts.LogFileLocationPropName, IsRequired = false)]
		public string LogFileLocation {
			get {
				return this[Consts.LogFileLocationPropName].ToString();
			}
			set {
				this[Consts.LogFileLocationPropName] = value;
			}
		}
	}
}