using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Pohgee.SIF.DesktopAgent.Abstractions.DataContracts;
using Pohgee.SIF.DesktopAgent.Abstractions.Enums;
using Pohgee.SIF.DesktopAgent.Core.Concrete;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	[ServiceContract]
	public interface IAgentService {
		/// <summary>
		/// Gets the sif agent status.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		ServiceStatus GetSifAgentStatus();

		/// <summary>
		/// Runs the command.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		[OperationContract]
		AutomationResult RunCommand(CommandType command, AutomationRequest request);
	}
}
