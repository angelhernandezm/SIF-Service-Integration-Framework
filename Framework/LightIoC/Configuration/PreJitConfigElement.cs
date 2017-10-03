using System.Configuration;
using Pohgee.Framework.LightIoC.Concrete;

namespace Pohgee.Framework.LightIoC.Configuration {
	public class PreJitConfigElement : ConfigurationElement {
		#region "Ctors"

		/// <summary>
		/// Initializes a new instance of the <see cref="PreJitConfigElement"/> class.
		/// </summary>
		public PreJitConfigElement() {

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PreJitConfigElement"/> class.
		/// </summary>
		/// <param name="isEnabled">The is enabled.</param>
		public PreJitConfigElement(string isEnabled) {
			Enabled = isEnabled;
		}

		#endregion


		#region "Properties"

		/// <summary>
		/// Gets or sets the enabled.
		/// </summary>
		/// <value>
		/// The enabled.
		/// </value>
		[ConfigurationProperty(Consts.EnabledPropName, DefaultValue = "true", IsRequired = false)]
		public string Enabled {
			get {
				return this[Consts.EnabledPropName].ToString();
			}
			set {
				this[Consts.EnabledPropName] = value;
			}
		}

		#endregion

	}
}
