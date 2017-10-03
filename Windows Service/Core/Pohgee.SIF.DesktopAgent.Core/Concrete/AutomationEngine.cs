using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pohgee.SIF.DesktopAgent.Abstractions;
using Pohgee.SIF.DesktopAgent.Abstractions.Enums;

namespace Pohgee.SIF.DesktopAgent.Core.Concrete {
	public class AutomationEngine : IAutomationEngine {
		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public IAutomationResult ExecuteCommand(CommandType command, IAutomationRequest request) {
			IAutomationResult retval = null;

			return retval;
		}

	}
}
