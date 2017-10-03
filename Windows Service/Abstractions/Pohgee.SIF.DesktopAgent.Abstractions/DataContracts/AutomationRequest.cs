using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pohgee.SIF.DesktopAgent.Abstractions;

namespace Pohgee.SIF.DesktopAgent.Abstractions.DataContracts {
	public class AutomationRequest : IAutomationRequest {
		/// <summary>
		/// Gets or sets a value indicating whether [use unified shell].
		/// </summary>
		/// <value><c>true</c> if [use unified shell]; otherwise, <c>false</c>.</value>
		public bool UseUnifiedShell { get; set; }

		public string FileName { get; set; }

		/// <summary>
		/// Gets or sets the scripts.
		/// </summary>
		/// <value>The scripts.</value>
		public Dictionary<string, IScriptable> Scripts { get; set; }

	}
}
