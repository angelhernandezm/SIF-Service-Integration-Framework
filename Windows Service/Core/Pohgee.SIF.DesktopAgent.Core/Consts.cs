using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pohgee.SIF.DesktopAgent.Core {
	public class Consts {
		public const string HostPropName = "host";
		public const string DirPathFormat = @"{0}\{1}";
		public const string TargetPropertyName = "Target";
		public const string HostPortPropName = "hostPort";
		public const string UseHttpsPropName = "useHttps";
		public const string LogEntryFormat = "{0} - {1}\n";
		public const string RuntimePropertyName = "Runtime";
		public const string AppServerPropertyName = "Appserver";
		public const string ConfigSectionName = "SifAgentService";
		public const string LogFileLocationPropName = "logFileLocation";
		public const string LaunchAppsOnStartPropName = "launchAppsOnStart";
		public const string ServiceImageName = "Pohgee.SIF.DesktopAgent.exe";
		public const string SurfacePerfCountersPropName = "surfacePerfCounters";
		public const string UnableToFindLogFolder = "Unable to find log folder.";
		public const string AgentServiceUri = "{0}://localhost:{1}/SifAgentService))";
		public const string MissingArgumentException = "One or more arguments were expected.";
		public const string LogFileNameAndLocation = @"{0}\SifAgentService-{1}-Log-{2}.txt";

		public const string PerformanceCounterCategory = "SifAgentService";
		public const string TotalErrorCounter = "TotalErrors";
		public const string TimeTakenCounter = "TimeTaken";
		public const string TotalAutomationRequestCounter = "TotalAutomationRequests";
		public const string TotalUnhandledErrorCounter = "TotalUnhandledExceptions";
		public const string TotalFailedAutomationCounter = "FailedAutomations";
		public const string TotalSucceededAutomationCounter = "SucceededAutomations";

	}
}

