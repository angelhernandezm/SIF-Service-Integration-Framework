using System;

namespace Pohgee.Framework.LightIoC.Abstractions {
	public interface IGenericLogger {
		/// <summary>
		/// Logs the specified details.
		/// </summary>
		/// <param name="details">The details.</param>
		void Log(string details);

		/// <summary>
		/// Logs the specified details.
		/// </summary>
		/// <param name="details">The details.</param>
		void Log(object details);

		/// <summary>
		/// Logs the specified error.
		/// </summary>
		/// <param name="error">The error.</param>
		void Log(Exception error);
	}
}
