using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pohgee.Framework.LightIoC.Abstractions {
	public interface ITypeFactory {
		/// <summary>
		/// Creates this instance.
		/// </summary>
		/// <returns></returns>
		T Create<T>();

		/// <summary>
		/// Gets the container.
		/// </summary>
		/// <value>
		/// The container.
		/// </value>
		ITypeContainer Container {
			get;
		}


	}
}
