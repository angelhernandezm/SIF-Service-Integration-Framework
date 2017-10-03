using System.Configuration;
using Pohgee.Framework.LightIoC.Concrete;

namespace Pohgee.Framework.LightIoC.Configuration {
	public class RegistrationConfigElement : ConfigurationElement {

		#region "Ctors"

		public RegistrationConfigElement() {

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RegistrationConfigElement"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="assemblyFQN">The assembly FQN.</param>
		public RegistrationConfigElement(string name, string type = "", string assemblyFQN = "") {

		}

		#endregion

		#region "Properties"

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[ConfigurationProperty(Consts.NamePropName, IsRequired = true)]
		public string Name {
			get {
				return this[Consts.NamePropName].ToString();
			}
			set {
				this[Consts.NamePropName] = value;
			}
		}

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		[ConfigurationProperty(Consts.TypePropName, IsRequired = false)]
		public string Type {
			get {
				return this[Consts.TypePropName].ToString();
			}
			set {
				this[Consts.TypePropName] = value;
			}
		}

		/// <summary>
		/// Gets or sets the assembly FQN.
		/// </summary>
		/// <value>
		/// The assembly FQN.
		/// </value>
		[ConfigurationProperty(Consts.AssemblyFqnPropName, IsRequired = false)]
		public string AssemblyFQN {
			get {
				return this[Consts.AssemblyFqnPropName].ToString();
			}
			set {
				this[Consts.AssemblyFqnPropName] = value;
			}
		}

		#endregion


	}
}
