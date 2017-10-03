using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Pohgee.SIF.DesktopAgent.Abstractions {
	public interface ICustomServiceBase<out T, out TV>
		where T : IServiceInformation
		where TV : IServiceOperation {
		/// <summary>
		/// Gets the service instance.
		/// </summary>
		/// <value>
		/// The service instance.
		/// </value>
		T ServiceInformation {
			get;
		}

		/// <summary>
		/// Gets the service operation.
		/// </summary>
		/// <value>
		/// The service operation.
		/// </value>
		TV ServiceOperation {
			get;
		}
	}
}
