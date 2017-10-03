using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	/// <summary>
	/// Delegate StartProcessDelegate
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TV">The type of the tv.</typeparam>
	/// <param name="psInfo">The ps information.</param>
	/// <param name="t">The t.</param>
	/// <param name="tv">The tv.</param>
	/// <param name="runAsLocalSystem">if set to <c>true</c> [run as local system].</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	public delegate bool StartProcessDelegate<T, in TV>(ProcessStartInfo psInfo, out T t, TV tv, bool runAsLocalSystem = false);


	/// <summary>
	/// 
	/// </summary>
	public interface IApplicationManager {
		void RegisterManagedApplication(IProcessInformation processInformation);

	//	void RegisterManagedApplication(StartProcessDelegate )

		

		IProcessInformation this[Guid managedApplication] {
			get;
		}

		IProcessInformation this[int processId] {
			get;
		}

	}
}
