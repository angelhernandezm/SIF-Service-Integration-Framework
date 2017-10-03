using System;
using Pohgee.Framework.LightIoC.Abstractions;

namespace Pohgee.Framework.LightIoC.Concrete {
	public class IoCLogger: IGenericLogger {
		/// <summary>
		/// Logs the specified details.
		/// </summary>
		/// <param name="details">The details.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void Log(string details) {
			//throw new NotImplementedException();
		}

		/// <summary>
		/// Logs the specified details.
		/// </summary>
		/// <param name="details">The details.</param>
		public void Log(object details) {
			//throw new NotImplementedException();
		}

		/// <summary>
		/// Logs the specified error.
		/// </summary>
		/// <param name="error">The error.</param>
		public void Log(Exception error) {
			//throw new NotImplementedException();
		}
	}
}
