using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pohgee.Framework.LightIoC.Abstractions;

namespace Pohgee.Framework.LightIoC.Concrete {
	public sealed class TypeFactory : ITypeFactory {
		private static volatile TypeFactory singleton;
		private static readonly object syncRoot = new object();

		/// <summary>
		/// Gets the container.
		/// </summary>
		/// <value>
		/// The container.
		/// </value>
		public ITypeContainer Container {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		/// <value>
		/// The logger.
		/// </value>
		private IGenericLogger Logger {
			get;
			set;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="TypeFactory"/> class.
		/// </summary>
		private TypeFactory(IGenericLogger logger) {
			Logger = logger;
		}


		/// <summary>
		/// Creates this instance.
		/// </summary>
		/// <returns></returns>
		public T Create<T>() {
			Container = TypeContainer.Current;
			return BuildObject<T>();

		}

		/// <summary>
		/// Builds up.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		private T BuildObject<T>() {
			object retval = null;
			ConstructorInfo[] ctors = null;

			if (typeof(T).IsInterface) {
				var found = TypeContainer.Current.Registry.FirstOrDefault(x => x.Key.Abstraction.Name.Equals(typeof(T).Name, StringComparison.OrdinalIgnoreCase));

				if (found.Key != null)
					ctors = found.Key.Concrete.GetConstructors(Consts.CtorBuildFlag);
				else
					throw new ArgumentException(Consts.MissingConcreteName);
			} else
				ctors = typeof(T).GetConstructors(Consts.CtorBuildFlag);

			var construct = new Dictionary<int, KeyValuePair<ConstructorInfo, TypeInformation>>();

			ctors.ToList().ForEach(v => TypeContainer.Current.Registry.Keys.ToList().ForEach(s => {
				if (v.GetParameters().TakeWhile(u => u.ParameterType.FullName == s.Abstraction.FullName).Any())
					construct.Add(construct.Count + 1, new KeyValuePair<ConstructorInfo, TypeInformation>(v, s));
			}));

			// Let's try to resolve constructor with dependencies
			if (construct.Count > 0) {
				var ctor = construct.FirstOrDefault().Value.Key;
				var param = ctor.GetParameters().ToList();
				var args = new object[param.Count];
				param.ForEach(x => GetConstructorExceptionCatcher(x, args));
				retval = ExecuteInSafeContext<object[], object>(ctor.SafelyInstantiateObject, args);
			} else {  // If we couldn't find any suitable constructor we use default one
				retval = typeof(T).GetConstructor(Type.EmptyTypes).SafelyInstantiateObject();
			}

			return ((T)retval);
		}

		/// <summary>
		/// Executes the in safe context.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TV">The type of the V.</typeparam>
		/// <param name="functor">The functor.</param>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		private TV ExecuteInSafeContext<T, TV>(Func<T, TV> functor, T t) {
			var retval = default(TV);

			try {
				retval = functor.Invoke(t);
			} catch (Exception ex) {
				Logger.Log(ex);
				throw;
			}

			return retval;
		}


		/// <summary>
		/// Gets the constructor exception catcher.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <param name="args">The args.</param>
		private void GetConstructorExceptionCatcher(ParameterInfo info, object[] args) {
			try {
				var instance = TypeContainer.Current.Registry.Keys.FirstOrDefault(z => z.Abstraction.FullName == info.ParameterType.FullName);
				if (instance != null)
					args[info.Position] = instance.Concrete.GetConstructor(Type.EmptyTypes).Invoke(null);
			} catch (ArgumentNullException ex) {
				Logger.Log(ex);
				throw;
			} catch (ArgumentException ex) {
				Logger.Log(ex);
				throw;
			} catch (NullReferenceException ex) {
				Logger.Log(ex);
				throw;
			}
		}

		/// <summary>
		/// Gets the current.
		/// </summary>
		/// <value>
		/// The current.
		/// </value>
		public static TypeFactory Current {
			get {
				if (singleton == null) {
					lock (syncRoot) {
						if (singleton == null)
							singleton = new TypeFactory(new IoCLogger());
					}
				}
				return singleton;
			}
		}
	}
}
