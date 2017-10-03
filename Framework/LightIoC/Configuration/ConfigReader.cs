using System.Configuration;
using Pohgee.Framework.LightIoC.Concrete;

namespace Pohgee.Framework.LightIoC.Configuration {
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
		/// Gets or sets my child section.
		/// </summary>
		/// <value>
		/// My child section.
		/// </value>
		[ConfigurationProperty(Consts.PreJittingPropertyName)]
		public PreJitConfigElement PreJitting {
			get {
				return (PreJitConfigElement)this[Consts.PreJittingPropertyName];
			}
			set {
				this[Consts.PreJittingPropertyName] = value;
			}
		}

		/// <summary>
		/// Gets or sets the registrations.
		/// </summary>
		/// <value>
		/// The registrations.
		/// </value>
		[ConfigurationProperty(Consts.RegistrationsPropertyName, IsDefaultCollection = true)]
		[ConfigurationCollection(typeof(RegistrationConfigCollection), AddItemName = Consts.RegisterElementName)]
		public RegistrationConfigCollection Registrations {
			get {
				return (RegistrationConfigCollection)this[Consts.RegistrationsPropertyName];
			}
			set {
				this[Consts.RegistrationsPropertyName] = value;
			}
		}

		/// <summary>
		/// Gets or sets the mappings.
		/// </summary>
		/// <value>
		/// The mappings.
		/// </value>
		[ConfigurationProperty(Consts.MappingsPropertyName, IsDefaultCollection = true)]
		[ConfigurationCollection(typeof(MappingConfigCollection), AddItemName = Consts.MapElementName)]
		public MappingConfigCollection Mappings {
			get {
				return (MappingConfigCollection)this[Consts.MappingsPropertyName];
			}
			set {
				this[Consts.MappingsPropertyName] = value;
			}
		}
	}
}