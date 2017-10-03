using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	public interface IUnifiedShell {
		/// <summary>
		/// Starts the specified configuration.
		/// </summary>
		/// <param name="config">The configuration.</param>
		void Start(IShellConfig config);

		/// <summary>
		/// Gets the h WND desktop.
		/// </summary>
		/// <value>
		/// The h WND desktop.
		/// </value>
		IntPtr hWndDesktop { get; }
	}
}
