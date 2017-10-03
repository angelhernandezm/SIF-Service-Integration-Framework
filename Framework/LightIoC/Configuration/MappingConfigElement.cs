using System.Configuration;
using Pohgee.Framework.LightIoC.Concrete;

namespace Pohgee.Framework.LightIoC.Configuration {
	public class MappingConfigElement : ConfigurationElement {
		#region "Ctors"

		/// <summary>
		/// Initializes a new instance of the <see cref="MappingConfigElement"/> class.
		/// </summary>
		public MappingConfigElement() {
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MappingConfigElement"/> class.
		/// </summary>
		/// <param name="abstraction">The abstraction.</param>
		/// <param name="to">To.</param>
		/// <param name="instanceRequired">if set to <c>true</c> [instance required].</param>
		public MappingConfigElement(string abstraction, string to, string instanceRequired = "true") {
			To = to;
			Abstraction = abstraction;
			InstanceRequired = instanceRequired;
		}

		#endregion


		#region "Properties"

		/// <summary>
		/// Gets or sets the abstraction.
		/// </summary>
		/// <value>
		/// The abstraction.
		/// </value>
		[ConfigurationProperty(Consts.AbstractionPropName, IsRequired = true)]
		public string Abstraction {
			get {
				return this[Consts.AbstractionPropName].ToString();
			}
			set {
				this[Consts.AbstractionPropName] = value;
			}
		}

		/// <summary>
		/// Gets or sets to.
		/// </summary>
		/// <value>
		/// To.
		/// </value>
		[ConfigurationProperty(Consts.ToPropName, IsRequired = true)]
		public string To {
			get {
				return this[Consts.ToPropName].ToString();
			}
			set {
				this[Consts.ToPropName] = value;
			}
		}

		/// <summary>
		/// Gets or sets the instance required.
		/// </summary>
		/// <value>
		/// The instance required.
		/// </value>
		[ConfigurationProperty(Consts.InstanceRequiredPropName, DefaultValue = "true", IsRequired = false)]
		public string InstanceRequired {
			get {
				return this[Consts.InstanceRequiredPropName].ToString();
			}
			set {
				this[Consts.InstanceRequiredPropName] = value;
			}
		}

		/// <summary>
		/// Gets or sets the life span.
		/// </summary>
		/// <value>
		/// The life span.
		/// </value>
		[ConfigurationProperty(Consts.LifeSpanPropName, DefaultValue = "0", IsRequired = false)]
		public string LifeSpan {
			get {
				return this[Consts.LifeSpanPropName].ToString();
			}
			set {
				this[Consts.LifeSpanPropName] = value;
			}
		}


		#endregion

	}
}
