using System.Configuration;
using Pohgee.Framework.LightIoC.Concrete;

namespace Pohgee.Framework.LightIoC.Configuration {
	public class RegistrationConfigSection : ConfigurationSection {
		/// <summary>
		/// Initializes a new instance of the <see cref="RegistrationConfigSection"/> class.
		/// </summary>
		public RegistrationConfigSection() {

		}

		/// <summary>
		/// Gets the registrations.
		/// </summary>
		/// <value>
		/// The registrations.
		/// </value>
		[ConfigurationProperty(Consts.RegistrationsPropertyName, IsDefaultCollection = true)]
		[ConfigurationCollection(typeof(RegistrationConfigCollection), AddItemName = Consts.RegisterElementName)]
		public RegistrationConfigSection Registrations {
			get {
				return this[Consts.RegistrationsPropertyName] as RegistrationConfigSection;
			}
		}
	}
}