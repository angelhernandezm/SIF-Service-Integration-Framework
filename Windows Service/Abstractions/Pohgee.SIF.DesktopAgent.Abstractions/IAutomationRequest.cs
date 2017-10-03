using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	public interface IAutomationRequest {
		/// <summary>
		/// Gets or sets a value indicating whether [use unified shell].
		/// </summary>
		/// <value><c>true</c> if [use unified shell]; otherwise, <c>false</c>.</value>
		bool UseUnifiedShell { get; set; }
		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		string FileName { get; set; }

		/// <summary>
		/// Gets or sets the scripts.
		/// </summary>
		/// <value>The scripts.</value>
		Dictionary<string, IScriptable> Scripts { get; set; }


	}
}
