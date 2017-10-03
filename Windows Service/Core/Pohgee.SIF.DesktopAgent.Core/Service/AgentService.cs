using Pohgee.Framework.LightIoC.Concrete;
using Pohgee.SIF.DesktopAgent.Abstractions;
using Pohgee.SIF.DesktopAgent.Abstractions.DataContracts;
using Pohgee.SIF.DesktopAgent.Abstractions.Enums;
using Pohgee.SIF.DesktopAgent.Core.Concrete;

namespace Pohgee.SIF.DesktopAgent.Core.Service {
	public class AgentService : IAgentService {
		/// <summary>
		/// Gets the sif agent status.
		/// </summary>
		/// <returns></returns>
		public ServiceStatus GetSifAgentStatus() {
			IPerfCounters perfCounters;
			var retval = new ServiceStatus();

			lock ((perfCounters = GlobalService.Current.GetPerfCounters<IPerfCounters>())) {
				retval.ErrorCount = perfCounters[PerfCounter.ErrorCount].RawValue;
				retval.TotalFailedAutomations = perfCounters[PerfCounter.FailedAutomations].RawValue;
				retval.UnhandledExceptionCount = perfCounters[PerfCounter.UnhandledExceptions].RawValue;
				retval.TotalSucceededAutomations = perfCounters[PerfCounter.SucceededAutomations].RawValue;
				retval.TotalAutomationRequests = perfCounters[PerfCounter.TotalAutomationRequests].RawValue;
			}

			return retval;
		}

		/// <summary>
		/// Runs the command.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public AutomationResult RunCommand(CommandType command, AutomationRequest request) {
			var engine = TypeContainer.Current.Resolve<IAutomationEngine>();
			var retval = engine.ExecuteCommand(command, request) as AutomationResult;

			return retval;
		}
	}
}