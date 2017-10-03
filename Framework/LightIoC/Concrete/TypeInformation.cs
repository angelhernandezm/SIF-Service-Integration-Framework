using System;

namespace Pohgee.Framework.LightIoC.Concrete {
	public class TypeInformation {
		/// <summary>
		/// Gets or sets the abstraction.
		/// </summary>
		/// <value>
		/// The abstraction.
		/// </value>
		public Type Abstraction {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the concrete.
		/// </summary>
		/// <value>
		/// The concrete.
		/// </value>
		public Type Concrete {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the assembly FQN.
		/// </summary>
		/// <value>
		/// The assembly FQN.
		/// </value>
		public string AssemblyFqn {
			get;
			set;
		}

		/// <summary>
		/// Gets the created on.
		/// </summary>
		/// <value>
		/// The created on.
		/// </value>
		public DateTime CreatedOn {
			get;
			private set;
		}

		/// <summary>
		/// Gets the life span.
		/// </summary>
		/// <value>
		/// The life span.
		/// </value>
		public int LifeSpan {
			get;
			set;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="TypeInformation"/> class.
		/// </summary>
		public TypeInformation() {
			CreatedOn = DateTime.Now;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeInformation"/> class.
		/// </summary>
		/// <param name="lifeSpan">The life span.</param>
		public TypeInformation(int lifeSpan)
			: this() {
			LifeSpan = lifeSpan;
		}
	}
}
