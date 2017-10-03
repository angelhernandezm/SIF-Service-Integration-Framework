using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Pohgee.SIF.DesktopAgent.Abstractions;

namespace Pohgee.SIF.Desktop.DesktopAgent.Shell {
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Pohgee.SIF.DesktopAgent.Abstractions.IWtsOperator" />
	public class WtsOperator : IWtsOperator {
		/// <summary>
		/// Initializes a new instance of the <see cref="WtsOperator"/> class.
		/// </summary>
		public WtsOperator() {
			uint pSessionId = 0;
			var channelName = "SIF";

			if (ApplicationLauncher.ProcessIdToSessionId((uint)Process.GetCurrentProcess().Id, ref pSessionId)) {
				// hVirtualChannel = ApplicationLauncher.WTSVirtualChannelOpen(IntPtr.Zero,   -1 /* (int)pSessionId */, channelName);
				hVirtualChannel = ApplicationLauncher.WTSVirtualChannelOpenEx(0, channelName, 1); // WTS_CHANNEL_OPTION_DYNAMIC
				var err = Marshal.GetLastWin32Error();

				System.Diagnostics.Debugger.Launch();

				
  
			}
		}

		/// <summary>
		/// Gets the h virtual channel.
		/// </summary>
		/// <value>
		/// The h virtual channel.
		/// </value>
		public IntPtr hVirtualChannel {
			get; private set;
		}
	}
}
