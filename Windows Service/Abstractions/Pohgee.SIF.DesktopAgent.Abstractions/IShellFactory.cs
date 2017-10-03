using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	/// <summary>
	/// 
	/// </summary>
	public interface IShellFactory {
		/// <summary>
		/// Gets the CLR4 path.
		/// </summary>
		/// <value>
		/// The CLR4 path.
		/// </value>
		string Clr4Path { get; }

		/// <summary>
		/// Gets the shellh WND.
		/// </summary>
		/// <value>
		/// The shellh WND.
		/// </value>
		IntPtr ShellhWnd { get; }

		/// <summary>
		/// Builds the specified t.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		T Build<T>(T t) where T: class;
	}
}
