using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pohgee.Framework.LightIoC.Abstractions {
	public interface ITypeContainer {
		/// <summary>
		/// Registers the type.
		/// </summary>
		/// <param name="useConfig">if set to <c>true</c> [use config].</param>
		void RegisterType(bool useConfig);

		/// <summary>
		/// Registers the type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="concreteName">Name of the concrete.</param>
		/// <param name="createInstance">if set to <c>true</c> [create instance].</param>
		/// <param name="ctorArgs">The ctor args.</param>
		/// <param name="isPrejittingEnabled">if set to <c>true</c> [is prejitting enabled].</param>
		void RegisterType<T>(string concreteName, bool createInstance = false, object[] ctorArgs = null, bool isPrejittingEnabled = true);

		/// <summary>
		/// Registers the type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="concreteObj">The concrete object.</param>
		void RegisterType<T>(object concreteObj);

		/// <summary>
		/// Registers the types.
		/// </summary>
		/// <param name="types">The types.</param>
		/// <param name="createInstances">if set to <c>true</c> [create instances].</param>
		/// <param name="isPrejittingEnabled">if set to <c>true</c> [is prejitting enabled].</param>
		void RegisterTypes(Dictionary<Type, Type> types, bool createInstances = false, bool isPrejittingEnabled = true);

		/// <summary>
		/// Registers the type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TV">The type of the V.</typeparam>
		/// <param name="abstraction">The abstraction.</param>
		/// <param name="concrete">The concrete.</param>
		/// <param name="createInstance">if set to <c>true</c> [create instance].</param>
		/// <param name="ctorArgs">The ctor args.</param>
		/// <param name="isPrejittingEnabled">if set to <c>true</c> [is prejitting enabled].</param>
		void RegisterType<T, TV>(T abstraction, TV concrete, bool createInstance = false, object[] ctorArgs = null, bool isPrejittingEnabled = true);

		/// <summary>
		/// Registers the types.
		/// </summary>
		/// <param name="types">The types.</param>
		/// <param name="createInstances">if set to <c>true</c> [create instances].</param>
		/// <param name="isPrejittingEnabled">if set to <c>true</c> [is prejitting enabled].</param>
		void RegisterTypes(Dictionary<Type, string> types, bool createInstances = false, bool isPrejittingEnabled = true);

		/// <summary>
		/// Resolves this instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="protectWithACriticalSection">if set to <c>true</c> [protect with A critical section].</param>
		/// <returns></returns>
		T Resolve<T>(bool protectWithACriticalSection = false);
	}
}
