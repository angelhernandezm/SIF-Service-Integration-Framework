using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	public interface IWtsOperator {
		/// <summary>
		/// Gets the h virtual channel.
		/// </summary>
		/// <value>
		/// The h virtual channel.
		/// </value>
		IntPtr hVirtualChannel { get; }

	}
}
