using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	public interface ILogger {
		/// <summary>
		/// Writes the specified entry.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <param name="location">The location.</param>
		void Write(string entry, string location);
	}
}
