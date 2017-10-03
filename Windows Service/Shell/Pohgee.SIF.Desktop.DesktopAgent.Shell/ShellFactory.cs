using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Pohgee.SIF.DesktopAgent.Abstractions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Win32;
using Pohgee.SIF.DesktopAgent.Abstractions.DataContracts;

namespace Pohgee.SIF.Desktop.DesktopAgent.Shell {
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Pohgee.SIF.DesktopAgent.Abstractions.IShellFactory" />
	public class ShellFactory : IShellFactory {
		/// <summary>
		/// The color path value name
		/// </summary>
		private const string ClrPathValueName = "InstallPath";

		/// <summary>
		/// The CLR4 path inregistry
		/// </summary>
		private const string Clr4PathInregistry = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full";

		/// <summary>
		/// Gets the CLR4 path.
		/// </summary>
		/// <value>
		/// The CLR4 path.
		/// </value>
		public string Clr4Path => GetClr4Path();

		/// <summary>
		/// Gets the shellh WND.
		/// </summary>
		/// <value>
		/// The shellh WND.
		/// </value>
		public IntPtr ShellhWnd {
			get; private set;
		}

		/// <summary>
		/// Gets the CLR4 path.
		/// </summary>
		/// <returns></returns>
		private string GetClr4Path() {
			var retval = string.Empty;
			var value = Utils.GetRegistryKeyValues(Registry.LocalMachine, Clr4PathInregistry, new[] { ClrPathValueName });

			if (value.ContainsKey(ClrPathValueName))
				retval = value[ClrPathValueName];

			return retval;
		}

		/// <summary>
		/// Builds the specified t.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public T Build<T>(T t) where T : class {
			object reference = null;
			var retval = default(T);
			var shellName = typeof(T) == typeof(Form) ? "UnifiedShellF.exe" : "UnifiedShellW.exe";
			var workingFolder = $@"{AppDomain.CurrentDomain.BaseDirectory}Shell\";

			if (!Directory.Exists(workingFolder))
				Directory.CreateDirectory(workingFolder);

			var targetShell = $"{workingFolder}{shellName}";
			var tempCsFile = shellName.Replace(".exe", ".cs");

			var shellCode = GetBaseShellCode(t);

			//TODO: Let's delete existing shells (in real life) we should pass config and delete if specified
			Directory.GetFiles(workingFolder, "*.*").ToList().ForEach(File.Delete);

			File.AppendAllLines($"{workingFolder}{tempCsFile}", shellCode.Split('\n'));

			// Let's build shell
			var cmdLine = $@"{GetClr4Path()}csc.exe /target:winexe ""{workingFolder}{tempCsFile}""";
			var p = StartShell<T>($@"{GetClr4Path()}csc.exe", $"/target:winexe \"{workingFolder}{tempCsFile} \"", targetShell, workingFolder, tempCsFile, out reference);

			return retval;
		}


		/// <summary>
		/// Starts the shell.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="arguments">The arguments.</param>
		/// <param name="targetShell">The target shell.</param>
		/// <param name="workingFolder">The working folder.</param>
		/// <param name="tempCsFile">The temporary cs file.</param>
		/// <param name="reference">The reference.</param>
		/// <returns></returns>
		private T StartShell<T>(string fileName, string arguments, string targetShell, string workingFolder, string tempCsFile, out object reference) {
			reference = null;
			var retval = default(T);

			using (var shellProcess = new Process() {
				StartInfo = new ProcessStartInfo() {
					FileName = $@"{GetClr4Path()}csc.exe",
					Arguments = $"/target:winexe \"{workingFolder}{tempCsFile} \"",
					WindowStyle = ProcessWindowStyle.Hidden,
					WorkingDirectory = workingFolder,
					UseShellExecute = false,
					RedirectStandardOutput = true
				}
			}) {

				if (shellProcess.Start()) {
					// Let's start recently created shell
					ApplicationLauncher.ProcessInformation pi;
#if DEBUG
					var output = shellProcess.StandardOutput.ReadToEnd();
#endif
					if (ApplicationLauncher.Current.StartProcessInteractively(new AutomationRequest() { FileName = targetShell }, out pi)) {
						var winsta = new List<string>();
						ApplicationLauncher.EnumWindowStations((x, s) => {winsta.Add(x); return true;}, IntPtr.Zero);
						var sessions = ApplicationLauncher.Current.ListSessions(Environment.GetEnvironmentVariable("ComputerName"));

						if (!ApplicationLauncher.EnumWindows(EnumChildWindowProcedure, (int)pi.dwProcessId)) {
							int z = 0;

						}

						//var targetProcess = Process.GetProcesses().First(_ => _.Id == pi.dwProcessId);
						//var shell = Form.FromHandle(targetProcess.MainWindowHandle);

						//Thread.Sleep(1500);

						//var hwnd = ApplicationLauncher.FindWindow(null, "UnifiedShell"  /*new StringBuilder("UnifiedShell")*/);


					}
				}
			}




			return retval;

		}

		/// <summary>
		/// Enums the child window procedure.
		/// </summary>
		/// <param name="hWnd">The h WND.</param>
		/// <param name="lParam">The l parameter.</param>
		/// <returns></returns>
		private bool EnumChildWindowProcedure(IntPtr hWnd, int lParam) {
			var retval = true;
			long processId = 0;


			ApplicationLauncher.GetWindowThreadProcessId(hWnd, out processId);
			System.Diagnostics.Trace.WriteLine($"Selected hWnd:{hWnd.ToInt32()} - Target PID:{lParam} - Selected PID:{processId}");

			if (/* ApplicationLauncher.GetWindowThreadProcessId(hWnd, out processId) > 0 && */ processId == lParam) {
				ShellhWnd = hWnd;
				retval = false;
			}

			return retval;
		}


		/// <summary>
		/// Gets the base shell code.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		protected virtual string GetBaseShellCode<T>(T t) {
			//TODO: Here we should return shell definition code to compile at runtime, it should be a web service call
			//      but for the sake of simplicity and development purposes, we'll return the bare minimun code for a Window  

			var retval = string.Empty;

			if (typeof(Form) == (Type)((object)t)) {
				retval = @"
                           using System;
						   using System.Windows.Forms;

							namespace Pohgee.SIF.DesktopAgent.Shell {
							  public class UnifiedShellF {
								[STAThread]
								static void Main(string[] args) {
								   Application.EnableVisualStyles();
								   Application.SetCompatibleTextRenderingDefault(false);
								   Application.Run(new Form() {Text = ""UnifiedShell"" });
								}
							  }
							}
";
			} else if (typeof(Window) == (Type)((object)t)) {

			} else
				throw new ArgumentException("Specified type cannot be used to construct a unified shell object.");

			return retval;
		}
	}
}
