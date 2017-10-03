using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IBaseService<out T> where T : Component {
		/// <summary>
		/// Gets the service instance.
		/// </summary>
		/// <value>
		/// The service instance.
		/// </value>
		T ServiceInstance {
			get;
		}
	}
}
