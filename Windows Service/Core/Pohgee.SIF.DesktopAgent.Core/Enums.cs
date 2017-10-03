using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pohgee.SIF.DesktopAgent.Core {
	/// <summary>
	/// 
	/// </summary>
	public enum FolderName {
		Not_Sent = 0,
		Processing,
		Sending,
		Sent
	}

	/// <summary>
	/// 
	/// </summary>
	public enum TreatDayAs {
		Month = 0,
		Day
	}

	/// <summary>
	/// 
	/// </summary>
	public enum FolderType {
		Source = 0,
		Target
	}

}
