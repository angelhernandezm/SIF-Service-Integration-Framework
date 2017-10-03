using System;

namespace Pohgee.Framework.LightIoC.Concrete {
	public class TypeMismatchException : Exception {
		/// <summary>
		/// Gets the different types message.
		/// </summary>
		/// <value>
		/// The different types message.
		/// </value>
		protected string DifferentTypesMessage {
			get;
			private set;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="TypeMismatchException"/> class.
		/// </summary>
		public TypeMismatchException() {

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeMismatchException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public TypeMismatchException(string message)
			: this() {
			DifferentTypesMessage = message;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeMismatchException"/> class.
		/// </summary>
		/// <param name="expectedType">The expected type.</param>
		/// <param name="receivedType">Type of the received.</param>
		public TypeMismatchException(Type expectedType, Type receivedType)
			: this() {

			if (expectedType != null && receivedType != null) {
				DifferentTypesMessage = string.Format(Consts.DifferentTypeExpected, new object[] { expectedType.FullName, receivedType.FullName });
			} else
				throw new NullReferenceException(Consts.NullParametersException);

		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*" />
		///   </PermissionSet>
		public override string ToString() {
			return (!string.IsNullOrEmpty(DifferentTypesMessage) ? DifferentTypesMessage : base.ToString());
		}

	}
}
