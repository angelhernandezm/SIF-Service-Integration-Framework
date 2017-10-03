using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	/// <summary>
	/// 
	/// </summary>
	public interface IProcessInformation {
		/// <summary>
		/// Gets the process unique identifier.
		/// </summary>
		/// <value>
		/// The process unique identifier.
		/// </value>
		int ProcessId { get; }

		/// <summary>
		/// Gets the managed application unique identifier.
		/// </summary>
		/// <value>
		/// The managed application unique identifier.
		/// </value>
		Guid ManagedApplicationId { get; }

		/// <summary>
		/// Gets the initial request.
		/// </summary>
		/// <value>
		/// The initial request.
		/// </value>
		IAutomationRequest InitialRequest { get;}
	}
}
